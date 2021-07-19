//  доделать сохранение значков-диаграмм                            
//  доделать загрузку значков-диаграмм                            
//  удаление значка из списка при удалении диаграммы

//  разобраться и может привести к одному типу  с бордером и без и убрать признак border.


// + разобраться с нестыковкой вертикального бордера у 3-х этажного элемента
// + сделать RemoveFromField( List<Object_Data> )
// + сделать коннекторы все растяжимыми за счет бесконечной дляны или высоты с ClipToBounds в канвасах
// + сделать угловые коннекторы
// + сделать Comment элемента под элементом в виде чистого TextBlock а не в составе канваса.
// + чтобы легче было раздвигать сетку Grid весь элемент помещать в одну растяжимую ячейку network , 
// + а чтобы сходились по уровню линии входов для односложных элементов надпись делать не над элементом 
// + а под элементом.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace ComplexPro_Step5
{
    public partial class Step5
    {

//*********************************************


        public class ElementImage
        {
//*********   INITIAL_DATA

            public string Category { get; set; }//  Имя категории элемента для возможности его поиска в базе.

            public string Name{get;set;}        //  Имя элемента для отображения в дереве меню.
                                                // sign_name не подходит потому что может отсутсвовать у элемента.
            public string Sign_name{get;set;}   //  Имя отображаемое в шапке значка элемента.
            
            //public TextBlock Sign_name_TextBlock { get; set; }   //  Для Binding USER_FUNCTION элементов.

            public int    Type{get;set;}        //    тип элемента: 
            //          0 - соединители (один Canvas без текстовых полей)
            //          1 - односложные без удлиняющих коннекторов с полем комментариев.
            //          2 - многосложные со всеми полями.

            public int Type_int { get; set; }   //    тип элемента: 
            //          0 - начальный элемент - источник сигнала "1" - NEW_LINE
            //          1 - коннектор, пропускающий сигнал во всех направлениях безусловно
            //          2 - пропускающий сигнал только слева направо безусловно
            //          3 - прерывающий, пропускающий сигнал сквозь себя слева направо по условию 
            //          4 - конечный элемент принимающий сигнал слева.

            public string Sign_text { get; set; } //  Текст (одна две буквы) отображаемый внутри графического рисунка элемента.

            public List<double> Sign_text_position { get; set; }  //  Положение текста - одна пара [X,Y] координат

            public bool Sign_border { get; set; }      //  border:  0 - без border, 1 - не замкнутый снизу border, 2- полный border.

            public List<List<IO_Data>> IO_list { get; set; }      //    io_list - имена входов выходов для каждой части-этажа Image.

            public List<List<double>> Points_list { get; set; }  // коллекция [X,Y] координат точек кривой рисунка элемента в %. 
                                                                 //    Для разрывов в линиях рисунка список координат разрывать значением "null".
            public List<string> IO_Directions_list { get; set; }     

            public List<string> C_Code_list { get; set; } //  Си-код соответвующий элементу.

            public Diagram_Of_Networks Diagram { get; set; } //  Для USER_FUNCTION диаграмма - соответвующая элементу.


//*********   ADDITIONAL_DATA

            public class IO_Data
            {
                public string Name;             // имя входа-выхода
                public string IO_Type;          // тип входа: IN, OUT
                public List<string> Types;      // допустимые типы данных
                public List<string> IO_Match;   // имена входов-выходов у которых между собою должно быть равенство типов

                public IO_Data(string name, string io_type, List<string> types, List<string> io_match)
                {
                    Name = name;
                    IO_Type = io_type;
                    Types = types;
                    IO_Match = io_match;
                }
            }

//*********   VISUAL_DATA

    //public  static Visibility NetworkCellNumbersVisibility {get; set;} 
    public  static double xCanvasWidth = 100;
    public  static double xCanvasNewLineWidth = 30;
            public static double xCanvasHeight = 60;
    public  static SolidColorBrush xCanvasBackground = new SolidColorBrush(Colors.White);

            static double xConnectorWidth = 5;
            static double xBorderWidth = xCanvasWidth - xConnectorWidth * 2;
            static double xBorderHeight = xCanvasHeight;
            static double xY0 = xBorderHeight / 2;
            static double xX0 = xConnectorWidth;

            static SolidColorBrush xStroke = new SolidColorBrush(Colors.Black);
            static double xStrokeThickness = 1;

            static SolidColorBrush xBorderStroke = new SolidColorBrush(Colors.Black);
            static double xBorderThickness = 1.0;

            static double xFontSize = 12;
            static double xTextPadding = 10;
            static double xCellNumberTextWidth = 21;
            static double xCellNumberTextPadding = 25;
            static double xImageTextFontSize = 18;

//*********   OPERATIVE_DATA
                                   //  Количество составляющих частей элемента равно: 
                                   //                 Width*Height.
            public int Width;      //  Количество составляющих частей элемента по ширине.
            public int Height;     //  Количество составляющих частей элемента по высоте.
                                   //   Height = HeightUp + 1 + HeightDown
            //public int HeightUp;   //  Количество составляющих частей элемента по высоте над центральным элементом.
            //public int HeightDown; //  Количество составляющих частей элемента по высоте под центральным элементом.
         

            public class Object_Data 
            { 
                public object obj ;
                public Type obj_type;
                public string param;

                public Object_Data ( object _obj, Type _obj_type )
                {
                    obj      = _obj ;
                    obj_type = _obj_type;
                    param = null;
                }

                public Object_Data(object _obj, Type _obj_type, string _param)
                {
                    obj = _obj;
                    obj_type = _obj_type;
                    param = _param;
                }

             } ;


            public Viewbox Icon { get; set; }

            
            //    type - тип элемента: 
            //          0 - соединители (один Canvas без текстовых полей)
            //          1 - односложные без удлиняющих коннекторов с полем комментариев.
            //          2 - многосложные со всеми полями.
            //    type_int - тип элемента: 
            //          0 - начальный элемент - источник сигнала "1" - NEW_LINE
            //          1 - коннектор, пропускающий сигнал во всех направлениях безусловно
            //          2 - пропускающий сигнал только слева направо безусловно
            //          3 - прерывающий, пропускающий сигнал сквозь себя слева направо по условию 
            //          4 - конечный элемент принимающий сигнал слева.
            //    name - имя элемента.
            //    points - набор [X,Y] координат точек кривой рисунка элемента в %. 
            //    Для разрывов в линиях рисунка список координат разрывать значением "null".
            //    border:  0 - без border, 1 - не замкнутый снизу border, 2- полный border.
            //    io_list - имена входов выходов для каждой части Image.

            public ElementImage(string category, string name, int type, int type_int, string sign_name, bool border,
                                     List<List<IO_Data>> io_list, List<List<double>> points_list,
                                     string sign_text, List<double> sign_text_position,
                                     List<string> directions, List<string> code)            
            {
                Category    = category; 
                Name        = name;
                Type        = type ;
                Type_int    = type_int;
                Sign_name   = sign_name;
                Sign_border = border ;
                IO_list     = io_list ;
                Points_list = points_list;
                Sign_text   = sign_text;
                Sign_text_position = sign_text_position;
                IO_Directions_list = directions;
                C_Code_list = code;
                Diagram = null;
                //Sign_name_TextBlock = null; //  Для Binding USER_FUNCTION элементов.

                //**************************
                //---
                //NetworkCellNumbersVisibility = Visibility.Hidden; 
                //---

                Icon = GetIconImage();

                //**************************

                if (Type == 0)   // коннекторы
                {
                    Height = 1; //HeightUp = 0; HeightDown = 0;
                    Width = 1; //3
                    if (border == true) Height++;// низ border теперь задается отдельным этажем для возможности растягивания элемента.
                }
                else if (Type == 1)  //  односложные c удлиняющими коннекторами с полем комментариев.
                {
                    Height = 2; //HeightUp = 0; HeightDown = 0;
                    Width = 3;
                    if (border == true) Height++;// низ border теперь задается отдельным этажем для возможности растягивания элемента.
                }
                else
                {
                    //    1-й Этаж.
                    Height = 2; //HeightUp = 1; HeightDown = 0;
                    Width = 3;
                    //    i-е Этажы.
                    if (IO_list != null && IO_list.Count > 1)
                    {
                        Height = IO_list.Count + 1; //HeightUp = 1; HeightDown = Height - 1 - HeightUp;
                        Width = 3;
                    }
                    if (border == true) Height++;// низ border теперь задается отдельным этажем для возможности растягивания элемента.
                }

            }  //************  END Of CONSTRUCTOR

//***************************************


            //    Коллекция IO_TextBlocks_list полностью синхронна коллекции IO_list + 1 TextBlock для Comment под элементом. 
            //    Содержит ссылки на TextBlocki необходимые в на верхнем уровне для Binding для отображения 
            //    привязанных переменных к входам выходов для каждой части-этажа Image.
            //  Для нулевого Top-этажа она содержит нулевые ссылки, т.к. у него нет внешних переменных привязки.

        //public Grid GetImage(out List<TextBlock> IO_TextBlocks_list)
        public Border GetImage(out List<TextBlock> IO_TextBlocks_list, out TextBlock ENI_textblock, out TextBlock ENO_textblock)
        {

            try
            {
                Border xborder = new Border();
                xborder.HorizontalAlignment = HorizontalAlignment.Stretch;
                xborder.VerticalAlignment = VerticalAlignment.Stretch;
                //xborder.Margin = new Thickness(0.0);  //  небольшой отступ чтобы было место для Border.
                //xborder.Padding= new Thickness(0.0);  //  небольшой отступ чтобы было место для Border.
                    Style style = new Style();
                    style.Setters.Add(new Setter(Border.BorderBrushProperty, new SolidColorBrush( Colors.Transparent)));
                    style.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(0.5)));
                        Trigger trigg = new Trigger();
                        trigg.Property = Border.IsMouseOverProperty ;
                        trigg.Value    = true;
                        trigg.Setters.Add(new Setter(Border.BorderBrushProperty, new SolidColorBrush(Colors.Black)));
                        trigg.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(0.5)));
                    style.Triggers.Add(trigg);
                xborder.Style = style;

                List<Object_Data> list_of_objects = GetFullImage(out IO_TextBlocks_list, out ENI_textblock, out ENO_textblock);

                    Grid grid_panel = new Grid();
                    //grid_panel.ShowGridLines = true;
                    
                    //  -   для коннекторов делаем вертикальный "Stretch" - чтобы их вертикальные линии 
                    //      растягивались вслед за растяжением клетки - вертикальные линии есть только у коннекторов.
                    //  -   для остальных делаем вертикальный "Top" - чтобы комментарий был всегда поджат 
                    //      вверх под символ.
                    if (Type == 0) //***** CONNECTORS
                    {
                        grid_panel.VerticalAlignment = VerticalAlignment.Stretch;
                    }
                    else grid_panel.VerticalAlignment = VerticalAlignment.Top;

                    // формируем личное поле для всех частей элемента.
                    for (int i = 0; i < Height; i++)
                    {
                        RowDefinition row = new RowDefinition();
                        grid_panel.RowDefinitions.Add(row);
                    }
                    for (int i = 0; i < Width; i++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        grid_panel.ColumnDefinitions.Add(column);
                    }

                    // заполняем личное поле  элемента.

                    int X = 0, Y = 0;

                    for (int i = 0; i < list_of_objects.Count; i++)
                    {

                            ElementImage.Object_Data obj_data = list_of_objects[i];

                            if (obj_data != null)
                            {
                                switch (obj_data.obj_type.Name)
                                {
                                    case "Canvas": 
                                        ((Canvas)obj_data.obj).SetValue(Grid.ColumnProperty, X);
                                        ((Canvas)obj_data.obj).SetValue(Grid.RowProperty, Y);
                                        grid_panel.Children.Add((Canvas)obj_data.obj);
                                        break;

                                    case "TextBlock": 
                                        ((TextBlock)obj_data.obj).SetValue(Grid.ColumnProperty, X);
                                        ((TextBlock)obj_data.obj).SetValue(Grid.RowProperty, Y);
                                        grid_panel.Children.Add((TextBlock)obj_data.obj);
                                        break;

                                    case "TextBox": 
                                        ((TextBox)obj_data.obj).SetValue(Grid.ColumnProperty, X);
                                        ((TextBox)obj_data.obj).SetValue(Grid.RowProperty, Y);
                                        grid_panel.Children.Add((TextBox)obj_data.obj);
                                        break;

                                    default: throw new Exception();
                                    //break;
                                }
                            }
                               //  размещаем элементы слева направо сверху вниз.
                            X++;
                            if (X >= Width)
                            {
                                X = 0;
                                Y++;
                            }
                        }

                    xborder.Child = grid_panel;

                return xborder;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                IO_TextBlocks_list = null;
                ENI_textblock = null;  ENO_textblock = null;
                return null;
            }

        }
       

//************************
            //    type - тип элемента: 1 - обычный, 0 - соединители (один Canvas без текстовых полей)
            //    name - имя элемента.
            //    points - набор [X,Y] координат точек кривой рисунка элемента в %. 
            //    Для разрывов в линиях рисунка список координат разрывать значением "null".
            //    border:  0 - без border, 1 - не замкнутый снизу border, 2- полный border.
            //    io_list - имена входов выходов для каждой части Image.

            //    Коллекция IO_TextBlocks_list полностью синхронна коллекции IO_list + 1 TextBlock для Comment под элементом. 
            //    Содержит ссылки на TextBlocki необходимые в на верхнем уровне для Binding для отображения 
            //    привязанных переменных к входам выходов для каждой части-этажа Image.
            //  Для нулевого Top-этажа она содержит нулевые ссылки, т.к. у него нет внешних переменных привязки.

        public List<Object_Data> GetFullImage(out List<TextBlock> IO_TextBlocks_list, out TextBlock ENI_textblock, out TextBlock ENO_textblock)
            {
                //   В примитивной комплектации элемент имеет:
                // - Canvas    - графический значок элемента;
                // это элементы-соединители: горизонтальный, вертикальный, Т-образный.

                //   В минимальной комплектации элемент имеет:
                // - Canvas    - графический значок элемента;
                // - TextBlock - надпись над ним;
                // - null*2    - пустые места слева и справа от надписи над ним.
                // - Canvas*2  - боковые коннекторы элемента.
                // - null*2    -  пустые места слева и справа от надписи над ним.
                // При этом в параметрах: io_list = null.

                //   В максимальной комплектации элемент имеет:
                // - Canvas*Nэтажей - графический значок элемента в Nэтажей = io_list.Count;
                // - TextBlock - надпись над ним;
                // - null*2    - пустые места слева и справа от надписи над ним.
                // - Canvas*2  - боковые коннекторы элемента.
                // - TextBlock*2*(Nэтажей-1) - на каждом этаже слева и справа от графического 
                //                             значка этажа наименование его правого и левого вх./вых.
                //               На одном этаже может быть только один вход слева и один выход справа.
                // - под каждым из TextBlock*2*(Nэтажей-1) - резервируется TextBlock для индикации 
                //                                           параметра соотв. вх./вых.


                IO_TextBlocks_list = new List<TextBlock>();

                TextBlock Status_Textblock;

                ENI_textblock = null; ENO_textblock=null;

                List<Canvas> list_of_graphimage = GetGraphImage(out Status_Textblock);

                //Icon = new ElementGraphImage(type, sign_name, border, io_list,
                  //                                         points_list, sign_text, text_position).GetIconImage();

                List<Object_Data>  list_of_objects = new List<Object_Data>();
                

                if (Type == 0) //***** CONNECTORS, NO BORDERs
                {
                        //list_of_objects.Add(null);
                        list_of_objects.Add(new Object_Data(list_of_graphimage[0], typeof(Canvas)));
                        //list_of_objects.Add(null);
                }
                else if (Type == 1) //***** ONE-STAGE SIMBOLS, NO BORDERs, ONE MAIN_TEXT_BLOCK
                {
                    TextBlock tin;
                    Object_Data tin_obj;

                    //  На каждый этаж положено по два Внешних TextBlock: левый и правый (вход и выход).
                    //  Если входа или выхода нет записываем null.
                    //  У первого этажа нет TextBlock. 
                    IO_TextBlocks_list.Add(null);
                    IO_TextBlocks_list.Add(null);

                    //--- MAIN_TEXT_BLOCK под элементом.

                    if (IO_list != null && IO_list.Last() != null && IO_list.Last()[0] != null)
                    {   //  для удобства вместо многоточия подписываем вход его типом данных, Он потом затрется при привязке переменной.
                        tin = GetTextBlock(Element_Data.GetVarTypesString(IO_list.Last()[0].Types));
                        //tin = GetTextBlock(" ??? ... ");
                        tin.VerticalAlignment = VerticalAlignment.Top;
                        //---
                        IO_TextBlocks_list.Add(tin);
                        IO_TextBlocks_list.Add(null);
                        tin_obj = new Object_Data(tin, typeof(TextBlock));
                    }
                    else
                    {
                        IO_TextBlocks_list.Add(null);
                        IO_TextBlocks_list.Add(null);
                        tin = null;
                        tin_obj = null;
                    }


                    if (Sign_border == false) // низ border теперь задается отдельным этажем
                    {
                        //list_of_objects.Add(new Object_Data(list_of_graphimage[0], typeof(Canvas)));
                        //list_of_objects.Add(tin_obj);

                        list_of_objects.Add(new Object_Data(HorizontalConnector("Left", out ENI_textblock), typeof(Canvas)));
                        list_of_objects.Add(new Object_Data(list_of_graphimage[0], typeof(Canvas)));
                        list_of_objects.Add(new Object_Data(HorizontalConnector("Right", out ENO_textblock), typeof(Canvas)));
                        //---
                        list_of_objects.Add(null);//  COMMENT under SIGN
                        list_of_objects.Add(tin_obj);
                        list_of_objects.Add(null);

                    }
                    else  //  добавляем к образу односложного элемента плечики из null.
                    {
                        list_of_objects.Add(new Object_Data(HorizontalConnector("Left", out ENI_textblock), typeof(Canvas)));
                        list_of_objects.Add(new Object_Data(list_of_graphimage[0], typeof(Canvas)));
                        list_of_objects.Add(new Object_Data(HorizontalConnector("Right", out ENO_textblock), typeof(Canvas)));
                        //---
                        list_of_objects.Add(null);//  BOTTOM of BORDER
                        list_of_objects.Add(new Object_Data(list_of_graphimage[1], typeof(Canvas)));
                        list_of_objects.Add(null);
                        //---
                        list_of_objects.Add(null);//  COMMENT under SIGN
                        list_of_objects.Add(tin_obj);
                        list_of_objects.Add(null);
                    }

                }
                else  //***** MULTI-STAGE SIMBOLS
                {
                    //    1-й Этаж.
                    //---  значок элемента
                    list_of_objects.Add(new Object_Data(HorizontalConnector("Left", out ENI_textblock), typeof(Canvas)));
                    list_of_objects.Add(new Object_Data(list_of_graphimage[0], typeof(Canvas)));
                    list_of_objects.Add(new Object_Data(HorizontalConnector("Right", out ENO_textblock), typeof(Canvas)));

                    //  На каждый этаж положено по два Внешних TextBlock: левый и правый (вход и выход).
                    //  Если входа или выхода нет записываем null.
                    //  У первого этажа нет TextBlock. 
                    IO_TextBlocks_list.Add(null);
                    IO_TextBlocks_list.Add(null);

                    //    i-е Этажы.
                    int i = 1;
                    TextBlock tin, tout;

                    if (IO_list != null && IO_list.Count > 2) // >1)
                    {

                        for (; i < IO_list.Count - 1; i++)
                        {

                            if ((IO_list[i])[0] != null)
                            {   //  для удобства вместо многоточия подписываем вход его типом данных, Он потом затрется при привязке переменной.
                                tin = GetTextBlock(Element_Data.GetVarTypesString(IO_list[i][0].Types));
                                //tin = GetTextBlock(" ??? ... ");
                                list_of_objects.Add(new Object_Data(tin, typeof(TextBlock)));
                            }
                            else
                            {
                                tin = null;
                                list_of_objects.Add(null); //new Object_Data(GetCanvas(), typeof(Canvas)));
                            }

                            list_of_objects.Add(new Object_Data(list_of_graphimage[i], typeof(Canvas)));

                            if ((IO_list[i])[1] != null)
                            {   //  для удобства вместо многоточия подписываем вход его типом данных, Он потом затрется при привязке переменной.
                                tout = GetTextBlock(Element_Data.GetVarTypesString(IO_list[i][1].Types));
                                //tout = GetTextBlock(" ??? ... ");
                                list_of_objects.Add(new Object_Data(tout, typeof(TextBlock)));
                            }
                            else
                            {
                                tout = null;
                                list_of_objects.Add(null); //new Object_Data(GetCanvas(), typeof(Canvas)));
                            }

                            IO_TextBlocks_list.Add(tin);
                            IO_TextBlocks_list.Add(tout);

                        }//  end of for
                    }

                    //--- низ bordera.
                    if (Sign_border == true) // низ border теперь задается отдельным этажем
                    {
                        list_of_objects.Add(null);
                        list_of_objects.Add(new Object_Data(list_of_graphimage[i], typeof(Canvas)));
                        list_of_objects.Add(null);
                    }


                    //--- комментарий под элементом.
                    list_of_objects.Add(null); //new Object_Data(GetCanvas(), typeof(Canvas)));

                    if (IO_list != null && IO_list.Last() != null && IO_list.Last()[0] != null)
                    {   //  для удобства вместо многоточия подписываем вход его типом данных, Он потом затрется при привязке переменной.
                        tin = GetTextBlock(Element_Data.GetVarTypesString(IO_list.Last()[0].Types));
                        //tin = GetTextBlock(" ??? ... ");
                        tin.VerticalAlignment = VerticalAlignment.Top;

                        list_of_objects.Add(new Object_Data(tin, typeof(TextBlock)));
                        IO_TextBlocks_list.Add(tin);
                    }
                    else
                    {
                        list_of_objects.Add(null);
                        IO_TextBlocks_list.Add(null);
                    }

                    list_of_objects.Add(null); //new Object_Data(GetCanvas(), typeof(Canvas)));
                }

                //  текстовое поле в правом верхнем углу любого элемента отображающее "0"/"1" для его клетки.
                //  используется для отладки.
                //  это поле есть у всех элементов и является последним в списке IO_TextBlocks_list.
                IO_TextBlocks_list.Add(Status_Textblock); 

                return list_of_objects;
            }

//****************************************************************


public List<Canvas> GetGraphImage(out TextBlock status_textblock)
            {
                try
                {
                    List<Canvas> list_canvas = new List<Canvas>();

                    //--- TopPart
                    int bord = 0;
                    if (Sign_border == true)
                    {
                        if (IO_list == null) bord = 2;  // замкнутый border
                        else if (IO_list.Count < 2) bord = 2;  // замкнутый border
                        else bord = 1;  // разомкнутый border 
                    }


                    list_canvas.Add(TopPart(bord, out status_textblock));
                                        
                    if (Type == 2)
                    {
                        //--- Middle & End Parts

                        if (IO_list != null && IO_list.Count > 2)
                        {
                            for (int i = 1; i < IO_list.Count-1; i++)
                            {
                                bord = 0;
                                if (Sign_border == true)
                                {          // последняя часть элемента - замкнутый border.
                                    if (i == IO_list.Count - 1) bord = 2;  // замкнутый border
                                    else bord = 1;  // разомкнутый border 
                                }

                                list_canvas.Add(MiddlePart(bord, IO_list[i]));
                            }
                        }
                    }

                    if (Sign_border == true) list_canvas.Add(BottomPart());

                    return list_canvas ;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    status_textblock = null;
                    return null;
                }
            }




            //    name - имя элемента.
            //    points - набор [X,Y] координат точек кривой рисунка элемента в %. 
            //    Для разрывов в линиях рисунка список координат разрывать значением "null".
            //    border:  0 - без border, 1 - не замкнутый снизу border, 2- полный border.
            //    EN - имена входов выходов EN/ENO.

    public Canvas TopPart(int border, out TextBlock status_textblock)
            {
                try
                {
                    Canvas canvas = GetCanvas();

                    //  не получилось - разсходятся коннекторы canvas.MaxHeight = canvas.MinHeight; // чтобы у маленьких элементов Comment не оттягивался 
                                               // далего вниз при растягивании их ячеек соседними элементами.

                    // колонку для NEW_LINE делаем узкой, чтобы съекономить место.
                    if (Name == "NEW_LINE") canvas.MinWidth = xCanvasNewLineWidth;
                
                    //  Имя элемента с координатами привязки к верхнему левому углу канвас.
                    //canvas.Background = new SolidColorBrush(Colors.Blue);

                    //******************  NAME

                    if (Sign_name != null)
                    {
                        TextBlock txt = new TextBlock();

                        if (Diagram == null) txt.Text = Sign_name;
                        else
                        {
                            Binding binding = new Binding();
                            binding.Source = Diagram;
                            binding.Path = new PropertyPath("NAME");
                            txt.SetBinding(TextBlock.TextProperty, binding);

                            //---  Обратная привязка, чтобы Sign_name отслеживал на всякий случай изменения NAME.
                            //Binding binding_one_way = new Binding();
                            //binding_one_way.Mode = BindingMode.OneWayToSource;
                            //binding_one_way.Source = this;
                            //binding_one_way.Path = new PropertyPath("Name");
                            //txt.SetBinding(TextBlock.TextProperty, binding);
                        }

                        //txt.Text = Sign_name;
                        txt.FontSize = xFontSize;
                        txt.Width = xBorderWidth;
                        txt.SetValue(Canvas.LeftProperty, xX0);
                        txt.SetValue(Canvas.TopProperty, 2.0);
                        txt.TextAlignment = TextAlignment.Center;
                        canvas.Children.Add(txt);
                    }

                    //******************  BORDER

                    if (border != 0)
                    {
                        PointCollection points = new PointCollection();
                        Polyline polyline = GetPolyline(xBorderStroke, xBorderThickness);

                        //---   Разделил линию на две части, чтобы не двоилась, левая вертикальная линия.

                        points.Add(new Point(xX0, xBorderHeight));
                        points.Add(new Point(xX0, 0 + xBorderThickness/2));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                        //---
                        points = new PointCollection();
                        polyline = GetPolyline(xBorderStroke, xBorderThickness);
                        points.Add(new Point(xX0, 0 + xBorderThickness / 2));
                        points.Add(new Point(xX0 + xBorderWidth, 0 + xBorderThickness / 2));
                        points.Add(new Point(xX0 + xBorderWidth, xBorderHeight));
                        
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                    }

                    //******************  CONNECTORS
                    //        чтобы в NEW_LINE коннекторе не рисовался левый усик:
                    //        для элеементов типа коннекторы не рисовать боковые усики, а рисовать элемент во всю длину.

                    if (Type != 0)
                    {
                        // не помогло if (Type == 1) canvas.MinHeight = xY0; // чтобы у маленьких элементов Comment не оттягивался далеко вниз

                        PointCollection points = new PointCollection();
                        Polyline polyline = GetPolyline(xStroke, xStrokeThickness);
                        //---
                        points.Add(new Point(0, xY0));
                        points.Add(new Point(xConnectorWidth, xY0));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                        //---
                        points = new PointCollection();
                        polyline = GetPolyline(xStroke, xStrokeThickness);
                        //---
                        points.Add(new Point(xCanvasWidth, xY0));
                        points.Add(new Point(xCanvasWidth - xConnectorWidth, xY0));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                    }


                    //******************  EN-ENO

                    if (IO_list != null)
                    {
                        if (IO_list.Count != 0 && (IO_list[0])[0] != null)
                        {
                            TextBlock txt = new TextBlock();
                            txt.Text = (IO_list[0])[0].Name;
                            txt.FontSize = xFontSize;
                            txt.Width = xBorderWidth - xTextPadding * 2; // отступ по 3 ед.
                            txt.SetValue(Canvas.LeftProperty, xX0 + xTextPadding);
                            txt.SetValue(Canvas.TopProperty, xY0 - xFontSize);
                            txt.TextAlignment = TextAlignment.Left;
                            canvas.Children.Add(txt);
                        }
                        //---
                        if (IO_list.Count != 0 && (IO_list[0])[1] != null)
                        {
                            TextBlock txt = new TextBlock();
                            txt.Text = (IO_list[0])[1].Name;
                            txt.FontSize = xFontSize;
                            txt.Width = xBorderWidth - xTextPadding * 2; // отступ по 3 ед.
                            txt.SetValue(Canvas.LeftProperty, xX0 + xTextPadding);
                            txt.SetValue(Canvas.TopProperty, xY0 - xFontSize);
                            txt.TextAlignment = TextAlignment.Right;
                            canvas.Children.Add(txt);
                        }
                    }

                    //******************  CELL NUMBER

                    status_textblock = (TextBlock)canvas.Children[0];
                    
                    //******************  IMAGE

                    if (Sign_text != null && Sign_text_position != null && Sign_text_position.Count == 2)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = Sign_text;
                        txt.FontSize = xImageTextFontSize;
                        txt.FontWeight = FontWeights.Thin;
                        txt.SetValue(Canvas.LeftProperty, xX0 + Sign_text_position[0]);
                        txt.SetValue(Canvas.TopProperty, xY0 - Sign_text_position[1]);
                        txt.TextAlignment = TextAlignment.Center;
                        canvas.Children.Add(txt);
                    }


                    if (Points_list != null)
                    {
                        PointCollection points = new PointCollection();
                        Polyline polyline = GetPolyline(xStroke, xStrokeThickness); 

                        foreach (List<double> point in Points_list)
                        {
                            if (point == null)  //Разрыв в линии - начало новой линии.
                            {
                                polyline.Points = points;
                                canvas.Children.Add(polyline);
                                //---
                                points = new PointCollection();
                                polyline = GetPolyline(xStroke, xStrokeThickness);
                            }
                            else if (point.Count == 2)
                            {
                                //        чтобы в NEW_LINE коннекторе не рисовался левый усик:
                                //        для элеементов типа коннекторы не рисовать боковые усики, а рисовать элемент во всю длину.
                                if (Type == 0) points.Add(new Point(0   + xCanvasWidth / 100 * point[0], xY0 - xBorderHeight / 100 * point[1]));
                                else           points.Add(new Point(xX0 + xBorderWidth / 100 * point[0], xY0 - xBorderHeight / 100 * point[1]));
                            }
                        }
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                    }

                    return canvas;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    status_textblock = null;
                    return null;
                }
            }


            //    border:  0 - без border, 1 - не замкнутый снизу border, 2- полный border.
            //    EN - имена входов выходов EN/ENO.

            public Canvas MiddlePart(int border, List<IO_Data> io_list)
            {
                Canvas canvas = GetCanvas();

                try
                {
                    //********  BORDER

                    if (border != 0)
                    {
                        PointCollection points = new PointCollection();
                        Polyline polyline = GetPolyline(xBorderStroke, xBorderThickness);
                        //---
                        points.Add(new Point(xX0, xBorderHeight*3));
                        points.Add(new Point(xX0, 0));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                        //---
                        points = new PointCollection();
                        polyline = GetPolyline(xBorderStroke, xBorderThickness);                        //---
                        points.Add(new Point(xX0 + xBorderWidth, 0));
                        points.Add(new Point(xX0 + xBorderWidth, xBorderHeight*3));
                        //if (border == 2) points.Add(new Point(xX0, xBorderHeight));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                    }

                    //******************  CONNECTORS

                    {
                        PointCollection points = new PointCollection();
                        Polyline polyline = GetPolyline(xStroke, xStrokeThickness);                  
                        points.Add(new Point(0, xY0));
                        points.Add(new Point(xConnectorWidth, xY0));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                        //---
                        points = new PointCollection();
                        polyline = GetPolyline(xStroke, xStrokeThickness);                         
                        points.Add(new Point(xCanvasWidth, xY0));
                        points.Add(new Point(xCanvasWidth - xConnectorWidth, xY0));
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                    }

                    //******************  EN-ENO

                    if (io_list != null)
                    {
                        if (io_list.Count != 0 && io_list[0] != null)
                        {
                            TextBlock txt = new TextBlock();
                            txt.Text = io_list[0].Name;
                            txt.FontSize = xFontSize;
                            txt.Width = xBorderWidth - xTextPadding * 2; // отступ по 3 ед.
                            txt.SetValue(Canvas.LeftProperty, xX0 + xTextPadding);
                            txt.SetValue(Canvas.TopProperty, xY0 - xFontSize);
                            txt.TextAlignment = TextAlignment.Left;
                            canvas.Children.Add(txt);
                        }
                        //---
                        if (io_list.Count > 1 && io_list[1] != null)
                        {
                            TextBlock txt = new TextBlock();
                            txt.Text = io_list[1].Name;
                            txt.FontSize = xFontSize;
                            txt.Width = xBorderWidth - xTextPadding * 2; // отступ по 3 ед.
                            txt.SetValue(Canvas.LeftProperty, xX0 + xTextPadding);
                            txt.SetValue(Canvas.TopProperty, xY0 - xFontSize);
                            txt.TextAlignment = TextAlignment.Right;
                            canvas.Children.Add(txt);
                        }
                    }

                    return canvas;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


            //рисуем одну горизонтальную линию - низ bordera
            public Canvas BottomPart()
            {
                try
                {
                    //******************  BORDER

                    // замкнутый снизу border
                    if (Sign_border == true)
                    {
                        Canvas canvas = GetCanvas();
                        canvas.MinHeight = xBorderThickness+2;
                        
                        PointCollection points = new PointCollection();
                        Polyline polyline = GetPolyline(xBorderStroke, xBorderThickness);
                        //---
                        points.Add(new Point(xX0 - xBorderThickness/2 , 0 + xBorderThickness / 2));
                        points.Add(new Point(xX0 + xBorderThickness/2 + xBorderWidth, 0 + xBorderThickness / 2));
                        
                        polyline.Points = points;
                        canvas.Children.Add(polyline);
                        return canvas;
                    }

                    return null;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


//*****************  ICON


            public Viewbox GetIconImage()
            {
                try
                {
                    TextBlock plug;//  заглушка.

                        xStrokeThickness *= 2;  xBorderThickness *= 2;

                    List<Canvas> image_list = GetGraphImage(out plug);

                        xStrokeThickness /= 2; xBorderThickness /= 2;

                    Viewbox viewbox = new Viewbox();
                    viewbox.Margin = new Thickness(0);
                    

                    Grid grid = new Grid();
                    grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    grid.VerticalAlignment = VerticalAlignment.Stretch;

                    //******************************************

                    //   Создаем поле.

                    for (int i = 0; i < image_list.Count; i++)
                    {
                        RowDefinition row = new RowDefinition();
                        row.Height = GridLength.Auto;
                        grid.RowDefinitions.Add(row);
                    }
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = GridLength.Auto;
                    grid.ColumnDefinitions.Add(column);

                    //   Размещаем на поле строку за строкой.

                    for (int i = 0; i < image_list.Count; i++)
                    {
                        Canvas canvas = image_list[i];
                        canvas.Background = new SolidColorBrush( Colors.Transparent);
                        canvas.SetValue(Grid.RowProperty, i);
                        canvas.SetValue(Grid.ColumnProperty, 0);
                        grid.Children.Add(canvas);
                    }

                    viewbox.Child = grid;

                    return viewbox;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


//*********************   GRAPH_PRIMITIVES

    static public Canvas HorizontalConnector(string type, out TextBlock textblock)
            {
                //Canvas canvas = GetCanvas();

                try
                {

                    Canvas canvas = GetCanvas();
                    canvas.MinWidth = 0;

                        Polyline polyline = GetPolyline(xStroke, xStrokeThickness); 
                        PointCollection points = new PointCollection();
                        points.Add(new Point(0, xY0 ));
                        points.Add(new Point(xCanvasWidth+1000, xY0 ));

                        polyline.Points = points;

                    canvas.Children.Add(polyline);

                        //  Поле для подписи состояния ENi/ENO для отладчика
                        textblock = new TextBlock();
                        //textblock.Text = "1";
                        textblock.SetValue(Canvas.TopProperty, 5.0);

                        if( type == "Left" )    textblock.SetValue(Canvas.RightProperty, 0.0);
                        else                    textblock.SetValue(Canvas.LeftProperty, 0.0);

                    canvas.Children.Add(textblock);

                        //Ellipse ellipse = new Ellipse();
                        //ellipse.Width = 4; ellipse.Height = 4;
                        //ellipse.Fill = xStroke;
                        //ellipse.SetValue(Canvas.LeftProperty, 0.0 - ellipse.Width / 2);
                        //ellipse.SetValue(Canvas.TopProperty, xY0-ellipse.Height/2-xStrokeThickness/2);

                    //canvas.Children.Add(ellipse);

/*                        ellipse = new Ellipse();
                        ellipse.Width = 4; ellipse.Height = 4;
                        ellipse.Fill = xStroke;
                        ellipse.SetValue(Canvas.LeftProperty, xCanvasWidth - ellipse.Width / 2);
                        ellipse.SetValue(Canvas.TopProperty, xY0 - ellipse.Height / 2 - xStrokeThickness / 2);

                    canvas.Children.Add(ellipse);
*/
                    return canvas;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    textblock = null;
                    return null;
                }
            }


            // Коротка, но растяживая в вертикаль линия.
    static public Canvas LeftVerticalMainLine()
            {

                try
                {
                    //Viewbox viewbox = new Viewbox();
                    //viewbox.VerticalAlignment = VerticalAlignment.Stretch;
                    //viewbox.HorizontalAlignment = HorizontalAlignment.Center;
                    
                        Canvas canvas = new Canvas();
                        canvas.Background = xCanvasBackground;
                        canvas.VerticalAlignment = VerticalAlignment.Stretch;
                        canvas.Width = 25;
                        canvas.ClipToBounds = true ;
                        

                            Polyline polyline = GetPolyline(xStroke, xStrokeThickness); 
                            PointCollection points = new PointCollection();
                            points.Add(new Point(25-xStrokeThickness/2, 0));
                            points.Add(new Point(25-xStrokeThickness/2, 1000));
                            polyline.Points = points;
                            //polyline.VerticalAlignment = VerticalAlignment.Stretch;
                            
                        canvas.Children.Add(polyline);

                        //viewbox.Child = canvas;

                    return canvas;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


            
            //-------   Универсальная Polyline

    static public Polyline GetPolyline(SolidColorBrush color, double thickness)
            {
                Polyline polyline = new Polyline();
                polyline.Stroke = color;
                polyline.StrokeThickness = thickness;
                polyline.SnapsToDevicePixels = true;
                return polyline;
            }

    static public Canvas GetCanvas()
            {
                Canvas canvas = new Canvas();
                canvas.MinWidth  = xCanvasWidth;
                canvas.MinHeight = xCanvasHeight;
                canvas.Background = xCanvasBackground;
                canvas.VerticalAlignment= VerticalAlignment.Stretch;
                canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                canvas.ClipToBounds = true;
                //  Добавляем елементом с нулевым индексом текстблок для номера ячейки в нетворке.
                canvas.Children.Add(GetCellNumberTextBlock()); 

                return canvas;
            }

    static public TextBlock GetTextBlock( string name )
            {
                TextBlock text_block = new TextBlock();
                text_block.Text = name;
                text_block.FontSize = xFontSize;
                text_block.MinWidth = 0;// xCanvasWidth;
                text_block.MaxWidth = xCanvasWidth;
                text_block.HorizontalAlignment = HorizontalAlignment.Stretch;
                text_block.VerticalAlignment = VerticalAlignment.Center;// Top
                text_block.TextAlignment = TextAlignment.Center;
                text_block.TextWrapping = TextWrapping.Wrap;

                //text_block.Tag заполняется при установке элемента на поле в конструкторе Element_Data(ElementImage txt_image, ElementCoordinats coordinats)

                //  К каждому TextBlock прикремпяем один на все TextBlock и на все виды элементов Handler.
                //text_block.MouseRightButtonUp += text_block_MouseRightButtonUp;
                
//***************  ELEMENT CONTEXT MENU
                    
                    ContextMenu menu = new ContextMenu();
                        MenuItem menu_item = new MenuItem();
                        menu_item.Header = "Global symbols";
                        menu_item.Tag = text_block;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += GlobalSymbols_context_menu_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Base symbols";
                        menu_item.Tag = text_block;  // прикрепляем к графическому образу информацию о элементе с адресом diagram.
                        menu_item.Click += BaseSymbols_context_menu_Click; ;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Setpoint symbols";
                        menu_item.Tag = text_block;  // прикрепляем к графическому образу информацию о элементе с адресом diagram.
                        menu_item.Click += StorableSymbols_context_menu_Click; ;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Msg symbols";
                        menu_item.Tag = text_block;  // прикрепляем к графическому образу информацию о элементе с адресом diagram.
                        menu_item.Click += MsgSymbols_context_menu_Click; ;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Local/Label symbols";
                        menu_item.Tag = text_block;  // прикрепляем к графическому образу информацию о элементе с адресом diagram.
                        menu_item.Click += LocalSymbols_context_menu_Click; 
                    menu.Items.Add(menu_item);

            text_block.ContextMenu = menu;


                return text_block;
            }


//******************  CELL NUMBER

static public TextBlock GetCellNumberTextBlock()
{
        try
        {
                TextBlock textblock = new TextBlock();
                textblock.FontSize = xFontSize*0.7;
                textblock.Width = xCellNumberTextPadding; // отступ по 3 ед.
                textblock.SetValue(Canvas.LeftProperty, xBorderWidth - xCellNumberTextWidth);
                textblock.SetValue(Canvas.TopProperty, 3.0);
                textblock.TextAlignment = TextAlignment.Center;
                // здесь нельзя, т.к. это запрещает писать номера и на канвасах внутри грид элеиментов.
                // textblock.Visibility = Step5.NetworkCellsVisibility ;
                    //Binding binding = new Binding();
                    //binding.Source = this;
                    //binding.Path = new PropertyPath("NetworkCellNumbersVisibility");
                    //textblock.SetBinding(TextBlock.VisibilityProperty, binding);
                            
                return textblock;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }


        }  //*************    END of Class <ElementImage>


//****************   HANDLERS of TEXTBLOCK CONTEXT MENU 

//static void text_block_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
static void GlobalSymbols_context_menu_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            //   Окно возвращает выбранный символ Symbol_Data.
            SYMBOLS.SYMBOLS_LIST_BUFFER = null;
            SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.GLOBAL_SYMBOLS_LIST);

            //  Для апередачи элемента из окна используем только нулевой элемент массива.
            if (SYMBOLS.SYMBOLS_LIST_BUFFER != null &&
                SYMBOLS.SYMBOLS_LIST_BUFFER[0] != null)
            {

                Element_Data.SetVarBinding((TextBlock)((MenuItem)sender).Tag, SYMBOLS.SYMBOLS_LIST_BUFFER[0]);
            }

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }

        e.Handled = true;
    }

static void BaseSymbols_context_menu_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //   Окно возвращает выбранный символ Symbol_Data.
        SYMBOLS.SYMBOLS_LIST_BUFFER = null;
        SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.BASE_SYMBOLS_LIST);

        //  Для апередачи элемента из окна используем только нулевой элемент массива.
        if (SYMBOLS.SYMBOLS_LIST_BUFFER != null &&
            SYMBOLS.SYMBOLS_LIST_BUFFER[0] != null)
        {

            Element_Data.SetVarBinding((TextBlock)((MenuItem)sender).Tag, SYMBOLS.SYMBOLS_LIST_BUFFER[0]);
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    e.Handled = true;
}

static void StorableSymbols_context_menu_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //   Окно возвращает выбранный символ Symbol_Data.
        SYMBOLS.SYMBOLS_LIST_BUFFER = null;
        SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.STORABLE_SYMBOLS_LIST);

        //  Для апередачи элемента из окна используем только нулевой элемент массива.
        if (SYMBOLS.SYMBOLS_LIST_BUFFER != null &&
            SYMBOLS.SYMBOLS_LIST_BUFFER[0] != null)
        {

            Element_Data.SetVarBinding((TextBlock)((MenuItem)sender).Tag, SYMBOLS.SYMBOLS_LIST_BUFFER[0]);
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    e.Handled = true;
}

static void MsgSymbols_context_menu_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //   Окно возвращает выбранный символ Symbol_Data.
        SYMBOLS.SYMBOLS_LIST_BUFFER = null;
        SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.MSG_SYMBOLS_LIST);

        //  Для апередачи элемента из окна используем только нулевой элемент массива.
        if (SYMBOLS.SYMBOLS_LIST_BUFFER != null &&
            SYMBOLS.SYMBOLS_LIST_BUFFER[0] != null)
        {

            Element_Data.SetVarBinding((TextBlock)((MenuItem)sender).Tag, SYMBOLS.SYMBOLS_LIST_BUFFER[0]);
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    e.Handled = true;
}

static void LocalSymbols_context_menu_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //  Добираемся до диаграммы с ее локальными переменными.
        Diagram_Of_Networks diagram = ((Element_Data)((TextBlock)((MenuItem)sender).Tag).Tag).Coordinats.NETWORK.DIAGRAM;

        //   Окно возвращает выбранный символ Symbol_Data.
        SYMBOLS.SYMBOLS_LIST_BUFFER = null;
        SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", diagram.LOCAL_SYMBOLS_LIST);

        //  Для апередачи элемента из окна используем только нулевой элемент массива.
        if (SYMBOLS.SYMBOLS_LIST_BUFFER != null &&
            SYMBOLS.SYMBOLS_LIST_BUFFER[0] != null)
        {

            Element_Data.SetVarBinding((TextBlock)((MenuItem)sender).Tag, SYMBOLS.SYMBOLS_LIST_BUFFER[0]);
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    e.Handled = true;
}




    }  //*************    END of Class <Step5>
}
