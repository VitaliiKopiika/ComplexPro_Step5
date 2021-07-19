// + доделать привязку глобальных и локальных переменных
// + выгрузка диаграммы вфайл и загрузка из файла

// + сделать Binding  Vars к TextBlockam при "загрузке network из файла"

// + добавить к элементам С-текст 
// + привязать к С-тексту VARs

// + добавить handlers к TextBlockam
// + сделать в handlersah меню выбора переменных и их запись в массив переменных Vars
// + сделать Binding  Vars к TextBlockam

// + переделать коллекции VARs и TextBlocks на одноуровневые.
// + создал и заполнил массив TextBlockov
// + создал нулевой массив переменных Vars синхронный с масивом TextBlockov
// + по упорядочиванию структуры данных закончено.
// + теперь упорядочить вызовы функций из-за изменившся параметров и структур

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

        static public List<Diagram_Of_Networks> DIAGRAMS_LIST { get; set; }

        //************    PANELs

        static public TabControl DIAGRAMS_TAB_PANEL { get; set; }

        static Diagram_Of_Networks ACTIV_DIAGRAM
        {
            get 
            {
                return (Diagram_Of_Networks)(((TabItem)DIAGRAMS_TAB_PANEL.SelectedItem).Tag);
            }
        }
        


//                Иерархия данных.
//
//Массив Главный [ Networks ]
//				Массив Network [ Lines ]
//								Массив Line [ Elements ]
//											Массив Element_Info [ Name, Code, ...]
//                                                                Массив Names[ BlocksCollection ]
//

        //********    DIAGRAM_OF_NETWORKS

        public partial class Diagram_Of_Networks
        {
            //static int Count = 0 ;

            public string NAME { get; set; }

            public string COMMENT { get; set; }

            //public int NUM { get; set; }

            public ObservableCollection<SYMBOLS.Symbol_Data> LOCAL_SYMBOLS_LIST { get; set; }

            //public ObservableCollection<SYMBOLS.Symbol_Data> LABEL_SYMBOLS_LIST { get; set; }
                       
//************    PANELs

            public Step5 PROJECT;

            // запоминаем свой TabItem для возможности его удаления из TabСontrol
            public TabItem TAB_ITEM { get; set; }  

            public StackPanel MAIN_PANEL { get; set; }

            public ObservableCollection<Network_Of_Elements> NETWORKS_LIST { get; set; }

            public ElementImage ELEMENT_IMAGE;

            public double diagram_scrollbar_value;

            //    CONSTRUCTOR #1

            //  Создание пустой DIAGRAM с одной NEW_LINE.

        public Diagram_Of_Networks()
        {
            try
            {   
                //Count++;
                //NUM = Count;

                if (Step5.DIAGRAMS_LIST == null || Step5.DIAGRAMS_LIST.Count == 0) NAME = "MAIN"; 
                else NAME = "Diagram" + Step5.DIAGRAMS_LIST.Count;

                LOCAL_SYMBOLS_LIST = new ObservableCollection<SYMBOLS.Symbol_Data>()
                { 
                    new SYMBOLS.Symbol_Data(null, "Local1", null, "WORD", "TEMP", null, null, null, null, "Comments1............."),
                    new SYMBOLS.Symbol_Data(null, "Local2", null, "BOOL", "TEMP", null, "Local1",  2, null, "Comments4............."),
                    new SYMBOLS.Symbol_Data(null, "Label1", null, "LABEL", "", null, null,  null, null, "Comments4.............")
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива LOCAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in LOCAL_SYMBOLS_LIST) symbol.Owner = LOCAL_SYMBOLS_LIST;

                //LABEL_SYMBOLS_LIST = new ObservableCollection<SYMBOLS.Symbol_Data>()
                //{ 
                    //new SYMBOLS.Symbol_Data(null, "Label1", null, "LABEL", "TEMP", null, null, null, null, "Comments1.............")
                //};
                //  в догонку заполняем Owner, т.к. на стадии создания массива LOCAL_SYMBOLS_LIST еще равно нулю.
                //foreach (SYMBOLS.Symbol_Data symbol in LABEL_SYMBOLS_LIST) symbol.Owner = LABEL_SYMBOLS_LIST;


                COMMENT = "Comment:";

                //TAB_ITEM = null;
                TAB_ITEM = new TabItem(); //  запоминаем для возможности его удаления.
                TAB_ITEM.Tag = this;

                //  инициализируем для создания новой диаграммы на поле.
                //  при загрузке из файла это не  надо.
                NETWORKS_LIST = new ObservableCollection<Network_Of_Elements>() { new Network_Of_Elements(this, "Network: ") };

                //  У MAIN нет значка - ее нет смысла откудато вызывать.
                if( this.NAME != "MAIN" ) ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage(this);
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
            }
        }

            //    CONSTRUCTOR #2

            //  Создание DIAGRAM с загруженной из файла List<Network_Of_Elements> networks.

        public Diagram_Of_Networks(string Name, string Comment, ObservableCollection<Network_Of_Elements> networks,
                                    ObservableCollection<SYMBOLS.Symbol_Data> local_symbols, 
                                    ElementImage element_image)
        {
            try
            {   

                //Count++;
                //NUM = Count;
                NAME = Name;
                COMMENT = Comment;
                LOCAL_SYMBOLS_LIST = local_symbols;

                //TAB_ITEM = null;
                TAB_ITEM = new TabItem(); //  запоминаем для возможности его удаления.
                TAB_ITEM.Tag = this;

                //MAIN_PANEL = diagram_panel;
                NETWORKS_LIST = networks;

                foreach (Network_Of_Elements netw in NETWORKS_LIST)
                {
                    netw.DIAGRAM = this;
                }

                //  У MAIN нет значка - ее нет смысла откудато вызывать.
                //if (this.NAME != "MAIN") ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage(this);
                //   привязываем взаимные сссылки, которые нельзя было привязать при загрузке из файла.
                if (element_image != null && this.NAME != "MAIN")
                {
                    element_image.Diagram = this;
                    ELEMENT_IMAGE = element_image;
                }    
                

                
                //---
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
            }
        }


//********    GET DIAGRAM DEEP COPY


    public Diagram_Of_Networks GetDiagramCopy()
    {
        try
        {

            ObservableCollection<Network_Of_Elements> networks_list = new ObservableCollection<Network_Of_Elements>();

            foreach (Network_Of_Elements network in NETWORKS_LIST)
            {
                //Network_Of_Elements xnetwork = network.GetNetworkCopy();
                //xnetwork.Get_NetworkImage(); //прорисовываем вхолостую чтобы заполнился MAIN_BORDER_PANEL;
                //networks_list.Add(xnetwork);
                networks_list.Add(network.GetNetworkCopy());
            }

            ObservableCollection<SYMBOLS.Symbol_Data> new_symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
            foreach (SYMBOLS.Symbol_Data symbol in LOCAL_SYMBOLS_LIST)
            {
                new_symbols_list.Add(new SYMBOLS.Symbol_Data(LOCAL_SYMBOLS_LIST, symbol.Name));
                new_symbols_list.Last().Owner = new_symbols_list;
            }

                                   // добавляем к имени диаграммы подчеркивание что бы не двоились имена.
            return new Diagram_Of_Networks("_"+NAME, COMMENT, networks_list, new_symbols_list,
                                            ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage(this));
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}


//********    NETWORK_OF_ELEMENTS


        public partial class Network_Of_Elements
        {
            //************    STATIC_DATA

            static int xMinWidth  = 9;
            static int xMinHeight = 2;

            static int xMaxWidth  = 11;
            static int xMaxHeight =  9;

            //************    MAIN_DATA

            public Diagram_Of_Networks DIAGRAM;
            public int NUM { get; set; }
            public string NAME { get; set; }
            public string COMMENT { get; set; }
            //public string LABEL { get; set; }
            public int HEIGHT;
            public int WIDTH;
                    
                    //  заполняется при загрузке из файла.
            public List<Element_Data> ELEMENTS;  // список имен элементов с их данными.

            //************    PANELs

            //  нельзя так инициализировать, т.к. это окно потом теряется, 
            //     остается открытым и не дает закрыть приложение.
            //static public Window COPY_DELETE_window = null;// = new Window();   
            static public Popup COPY_DELETE_popup = null;// = new Window();   

            public Border MAIN_BORDER_PANEL;
            public Grid Grid_panel;
            public Grid Main_grid0;
            
            //   Копия визуального поля для последующего его декомпиляции в С.текст
            public List<List<Element_Data>> Network_panel_copy;  //  поле для морского боя - отмечать клетки занятые элементами.

            //  Список цепей.
            public List<List<int[]>> Chains_list ;

            //  Копия поля Network: полные имена для всех ячеек с элементами: 
            //                                          Буква+номер нетворка+координаты ячейки.
            public List<List<string>> Chains_FullVars_panel_copy;

            //  Копия поля Network: имена цепей для всех ячеек с элементами: Буква+номер нетворка.
            public List<List<string>> Chains_Vars_panel_copy;

            //  Простой список имен цепей: имена цепей: Буква+номер нетворка.
            public List<string> Chains_Vars_list;

            public double network_scrollbar_value, scrollbar_value, scrollbar_h_value;

            //    CONSTRUCTOR #1
            //  используется при создании пустого поля из программы.
    public Network_Of_Elements(Diagram_Of_Networks diagram, string name)
    {
            try
            {
                DIAGRAM = diagram;

                NAME = name;

                COMMENT = "Comment:" ;

                //LABEL = null;

                HEIGHT   = xMinHeight + 1 ; // место только для горизонтальной линии питания и 
                                        // для красоты по одной пустой строке сверху и снизу.
                WIDTH    = xMinWidth  ;

                Network_panel_copy = new List<List<Element_Data>>();

                Chains_list = new List<List<int[]>>();

                Chains_FullVars_panel_copy = new List<List<string>>();

                Chains_Vars_panel_copy = new List<List<string>>();

                Chains_Vars_list = new List<string>();

                ELEMENTS = new List<Element_Data>()
                { 
                    //new Element_Data("CONNECTORS", "VERTICAL", 0, 0),
                    new Element_Data("CONNECTORS", "NEW_LINE", new ElementCoordinats(this, 0, 1), null, null),
                    new Element_Data("CONNECTORS", "HORIZONTAL", new ElementCoordinats(this, 1, 1), null, null),
                    new Element_Data("CONNECTORS", "END_OF_LINE", new ElementCoordinats(this, 2, 1), null, null)
                };

                scrollbar_value = 0; // делаем запас, т.к. scrollbar работает в минус только до нуля

            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
            }

    }

            //    CONSTRUCTOR #2
            //  используется при создании полей при загрузке данных диаграмы из файла.
    public Network_Of_Elements(Diagram_Of_Networks diagram, string name, string comment,/*string label,*/ int height, int width, List<Element_Data> elements )
    {
            try
            {   

                DIAGRAM = diagram;
                NAME = name;
                COMMENT = comment ;
                //LABEL = label;
                HEIGHT   = height < xMinHeight ? xMinHeight : height;
                WIDTH    = width  < xMinWidth ? xMinWidth : width;
                ELEMENTS = elements;

                foreach (Element_Data elem in ELEMENTS)
                {
                    elem.Coordinats.NETWORK = this;
                }

                Network_panel_copy = new List<List<Element_Data>>();
                
                Chains_list = new List<List<int[]>>();

                Chains_FullVars_panel_copy = new List<List<string>>();

                Chains_Vars_panel_copy = new List<List<string>>();

                Chains_Vars_list = new List<string>();

                //---

                scrollbar_value = 0;

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

            }



//********    GET NETWORK DEEP COPY


    public Network_Of_Elements GetNetworkCopy()
    {
        try
        {

            //---   Распечатка элементов.
            List<Element_Data> elements = new List<Element_Data>();

            foreach (List<Element_Data> list in Network_panel_copy)
            {
                foreach (Element_Data element in list)
                {
                    if (element != null)
                    {
                        List<List<string>> vars_list = new List<List<string>>();
                        
                        foreach (SYMBOLS.Symbol_Data symbol in element.IO_VARs_list)
                        {
                            List<string> name_buff = new List<string>() { null, null };
                            if (symbol != null)
                            {
                                name_buff[0] = symbol.Name;
                                //---
                                if (symbol.Owner == SYMBOLS.GLOBAL_SYMBOLS_LIST) name_buff[1] = "Global";
                                else if (symbol.Owner == SYMBOLS.BASE_SYMBOLS_LIST) name_buff[1] = "Base";
                                else if (symbol.Owner == SYMBOLS.STORABLE_SYMBOLS_LIST) name_buff[1] = "Storable";
                                else if (symbol.Owner == SYMBOLS.MSG_SYMBOLS_LIST) name_buff[1] = "Msg";
                                else if (symbol.Owner == this.DIAGRAM.LOCAL_SYMBOLS_LIST) name_buff[1] = "Local";
                                //else if (symbol.Owner == this.DIAGRAM.LABEL_SYMBOLS_LIST) name_buff[1] = "Label";
                                else throw (new Exception("Unknown symbols list!")) ;
                            }
                            else name_buff[0] ="null";

                            vars_list.Add(name_buff);
                        }

                        elements.Add( new Element_Data(element.Category, element.Name, 
                                      new ElementCoordinats(null, element.Coordinats.X, element.Coordinats.Y),
                                      vars_list, DIAGRAM.LOCAL_SYMBOLS_LIST));
                    }
                }
            }

            return new Network_Of_Elements(DIAGRAM, NAME, COMMENT,/*LABEL,*/ HEIGHT, WIDTH, elements);

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

} //***** End of Network_Of_Elements


//********    ELEMENT_DATA


            public class ElementCoordinats
            {
                public Network_Of_Elements NETWORK;
                public int X, Y;
                //public string[,] NETWORK_PANEL_copy;

                public ElementCoordinats(Network_Of_Elements network, int x, int y)//, string[,] network_panel_copy)
                {
                    NETWORK = network;
                    X = x;
                    Y = y;
                    //NETWORK_PANEL_copy = network_panel_copy;
                }
            }
        


        public class Element_Data
        {
            public string Category { get; set; }

            public string Name   { get; set; }

            public ElementImage Txt_Image { get; set; }

            public Border Border_Image { get; set; }

            public ElementCoordinats Coordinats ;


            //  Коллекция переменных одноуровневая но синхронная с 
            // List<List<TextBlock>> IO_TextBlocks_list - на каждый 
            // этаж по два элемента плюс один           элемент на Comment блока.
            //                и еще плюс один последний элемент на Bool_State блока.

            public List<TextBlock> IO_TextBlocks_list; //{ get; set; }      

            public List<SYMBOLS.Symbol_Data> IO_VARs_list { get; set; }      

            public List<MultiBinding> IO_Binding_list { get; set; }

            public TextBlock ENI_textblock, ENO_textblock;
            public Binding  ENI_Binding { get; set; }
            public Binding  ENO_Binding { get; set; }

//            public List<string> ProgCode   { get; set; }


//***********   CONSTRUCTOR #1     Применяется при установке элемента на поле из ToolBar.

            public Element_Data(ElementImage txt_image, ElementCoordinats coordinats)
            {
                try
                {
                    Category = txt_image.Category;
                    Name = txt_image.Name;
                    Txt_Image = txt_image;
                    Border_Image = Txt_Image.GetImage(out IO_TextBlocks_list, out ENI_textblock, out ENO_textblock);
                    Border_Image.Tag = this;  // прикрепляем к графическому образу информацию о элементе c координатами, чтобы его можно было перетаскивать.
                    Border_Image.MouseLeftButtonDown += Network_Of_Elements.ElementMove_MouseLeftButtonDown;
                    Border_Image.AllowDrop = true;
                    Border_Image.Drop += Network_Of_Elements.canvas_Drop;
                    //Border_Image.DragEnter += canvas_DragEnter;  в нем не работает изменение Drag.Copy на Drag.None
                    Border_Image.DragOver += Network_Of_Elements.canvas_DragOver;

                    Coordinats = coordinats;

                    ElementContextMenu();

                    ENI_Binding = null;
                    ENO_Binding = null;

                    //  Создаем пустую коллекцию синхронную с List<List<TextBlock>> IO_TextBlocks_list
                    IO_VARs_list = new List<SYMBOLS.Symbol_Data>();
                    IO_Binding_list = new List<MultiBinding>();
                    for (int i = 0; i < IO_TextBlocks_list.Count; i++)
                    {
                        IO_VARs_list.Add(null);//new List<Symbols_List.Symbols_data>() { null, null });
                        //---
                        IO_Binding_list.Add(null);
                        //---
                        if (IO_TextBlocks_list[i] != null) IO_TextBlocks_list[i].Tag = this;
                    }
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//***********   CONSTRUCTOR #2      Применяется при установке элемента на поле при загрузке из файла,
//                                  а также при создании пустого поля Network - 
//  при этом передается vars_list = null из конструктора  Network_Of_Elements(diagram, name)
public Element_Data(string category, string name, ElementCoordinats coordinats, List<List<string>> vars_list, 
                        ObservableCollection<SYMBOLS.Symbol_Data> local_symbols_list)
    {
        try
        {
            Category = category;
            Name = name;
            Coordinats = coordinats;
            Txt_Image = ELEMENTS_DICTIONARY.GetElementByName(Category,Name);
            Border_Image = Txt_Image.GetImage(out IO_TextBlocks_list, out ENI_textblock, out ENO_textblock);
            Border_Image.Tag = this;  // прикрепляем к графическому образу информацию о элементе c координатами, чтобы его можно было перетаскивать.
            Border_Image.MouseLeftButtonDown += Network_Of_Elements.ElementMove_MouseLeftButtonDown;
            Border_Image.AllowDrop = true;
            Border_Image.Drop += Network_Of_Elements.canvas_Drop;
            //Border_Image.DragEnter += canvas_DragEnter;  в нем не работает изменение Drag.Copy на Drag.None
            Border_Image.DragOver += Network_Of_Elements.canvas_DragOver;

            ElementContextMenu();

            ENI_Binding = null;
            ENO_Binding = null;

            //  Создаем пустую коллекцию равную и синхронную с List<TextBlock> IO_TextBlocks_list
            //  + за каждым TextBlock закрепляем ссылку на его владельца Element для его MouseHendlera.

            IO_VARs_list = new List<SYMBOLS.Symbol_Data>();
            IO_Binding_list = new List<MultiBinding>();

            for( int i=0; i < IO_TextBlocks_list.Count; i++) 
            {
                if (IO_TextBlocks_list[i] != null)  IO_TextBlocks_list[i].Tag = this;
                //---
                IO_VARs_list.Add(null);   // заполняется в SetVarBinding
                IO_Binding_list.Add(null); // заполняется в SetVarBinding
            }

            //---    ПРИВЯЗКА ПЕРЕМЕННЫХ.

            if (vars_list != null) //  vars_list = null передается из конструктора  Network_Of_Elements() при создании пустого поля.
            {
                //---  SetVarBinding должен идти после IO_TextBlocks_list[i].Tag = this;
                for (int i = 0; i < IO_TextBlocks_list.Count; i++)
                {
                    if (IO_TextBlocks_list[i] != null && vars_list[i][0] != "null")
                    {
                        if (vars_list[i][1] == "Global")
                        {
                            SetVarBinding(IO_TextBlocks_list[i], SYMBOLS.GLOBAL_SYMBOLS_LIST.First(value => value.Name == vars_list[i][0]));
                        }
                        else if (vars_list[i][1] == "Base")
                        {
                            SetVarBinding(IO_TextBlocks_list[i], SYMBOLS.BASE_SYMBOLS_LIST.First(value => value.Name == vars_list[i][0]));
                        }
                        else if (vars_list[i][1] == "Storable")
                        {
                            SetVarBinding(IO_TextBlocks_list[i], SYMBOLS.STORABLE_SYMBOLS_LIST.First(value => value.Name == vars_list[i][0]));
                        }
                        else if (vars_list[i][1] == "Msg")
                        {
                            SetVarBinding(IO_TextBlocks_list[i], SYMBOLS.MSG_SYMBOLS_LIST.First(value => value.Name == vars_list[i][0]));
                        }
                        else if (vars_list[i][1] == "Local" && local_symbols_list != null)
                        {
                            SetVarBinding(IO_TextBlocks_list[i], local_symbols_list.First(value => value.Name == vars_list[i][0]));
                        }
                        else throw (new Exception("Unknown symbols list!"));
                    }
                }
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
    
    }


//***********   CONSTRUCTOR #3      Применяется при копировании уже установленного элемента на поле.

public Element_Data(ElementImage txt_image, ElementCoordinats coordinats, List<SYMBOLS.Symbol_Data> io_vars_list )
{
    try
    {
        Category = txt_image.Category;
        Name = txt_image.Name;
        Coordinats = coordinats;
        Txt_Image = txt_image;
        Border_Image = Txt_Image.GetImage(out IO_TextBlocks_list, out ENI_textblock, out ENO_textblock);
        Border_Image.Tag = this;  // прикрепляем к графическому образу информацию о элементе c координатами, чтобы его можно было перетаскивать.
        Border_Image.MouseLeftButtonDown += Network_Of_Elements.ElementMove_MouseLeftButtonDown;
        Border_Image.AllowDrop = true;
        //Border_Image.Drop += Border_Image_Drop;
        //Border_Image.DragEnter += canvas_DragEnter;  в нем не работает изменение Drag.Copy на Drag.None
        Border_Image.DragOver += Network_Of_Elements.canvas_DragOver;

        ElementContextMenu();

        ENI_Binding = null;
        ENO_Binding = null;

        //  Создаем пустую коллекцию равную и синхронную с List<TextBlock> IO_TextBlocks_list
        //  + за каждым TextBlock закрепляем ссылку на его владельца Element для его MouseHendlera.

        IO_VARs_list = new List<SYMBOLS.Symbol_Data>(io_vars_list);
        IO_Binding_list = new List<MultiBinding>();

        for (int i = 0; i < IO_TextBlocks_list.Count; i++)
        {
            if (IO_TextBlocks_list[i] != null) IO_TextBlocks_list[i].Tag = this;
            //---
            //IO_VARs_list.Add(null);  уже заполнено
            IO_Binding_list.Add(null); // заполняется в SetVarBinding
        }

        //---    ПРИВЯЗКА ПЕРЕМЕННЫХ.
        //---  SetVarBinding должен идти после IO_TextBlocks_list[i].Tag = this;
        for (int i = 0; i < IO_TextBlocks_list.Count; i++)
        {
            if (IO_TextBlocks_list[i] != null && ( i < IO_VARs_list.Count && IO_VARs_list[i] != null))
            {
                SetVarBinding(IO_TextBlocks_list[i], IO_VARs_list[i]);
            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


//***************  ELEMENT CONTEXT MENU

public void ElementContextMenu()
{
    try
    {
            Border xborder = this.Border_Image;

                //xborder.MouseRightButtonUp  += ElementMenu_MouseRightButtonUp;

                    ContextMenu menu = new ContextMenu();
                    menu.MinWidth = 75;

                        MenuItem menu_item = new MenuItem();
                        menu_item.Header = "Copy";
                        menu_item.Tag = xborder;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Menu_Copy_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Delete";
                        menu_item.Tag = xborder;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Menu_Delete_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Insert collumn";
                        menu_item.Tag = xborder;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Menu_Insert_collumn_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Insert row";
                        menu_item.Tag = xborder;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Menu_Insert_row_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Cancel";
                        menu_item.Tag = xborder;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Menu_Cancel_Click;
                    menu.Items.Add(menu_item);
                xborder.ContextMenu = menu;
                
            xborder.VerticalAlignment = VerticalAlignment.Stretch;
            xborder.SetValue(Grid.RowProperty, this.Coordinats.Y);
            xborder.SetValue(Grid.ColumnProperty, this.Coordinats.X);

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
 }

//******************   HANDLERS


static void Menu_Copy_Click(object sender, RoutedEventArgs e)
{
    try
    {
        _DRAG_DROP_BUFF.obj = ((MenuItem)sender).Tag;
        _DRAG_DROP_BUFF.obj_type = typeof(Border);
        _DRAG_DROP_BUFF.param = "Copy";

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}



static void Menu_Delete_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Border xborder = (Border)((MenuItem)sender).Tag;
        ElementCoordinats coordinats = ((Element_Data)xborder.Tag).Coordinats;

        coordinats.NETWORK.Grid_panel.Children.Remove(xborder);

        // Сразу заполняем копию поля - удаляем элемент с прежними координатами.
        coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X] = null;

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


static void Menu_Insert_collumn_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Border xborder = (Border)((MenuItem)sender).Tag;
        ElementCoordinats coordinats = ((Element_Data)xborder.Tag).Coordinats;

        Network_Of_Elements.Insert_network_collumn(coordinats.NETWORK, coordinats.X);

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

static void Menu_Insert_row_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Border xborder = (Border)((MenuItem)sender).Tag;
        ElementCoordinats coordinats = ((Element_Data)xborder.Tag).Coordinats;

        Network_Of_Elements.Insert_network_row(coordinats.NETWORK, coordinats.Y);

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

static void Menu_Cancel_Click(object sender, RoutedEventArgs e)
{
    try
    {
        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}



//***************    ПОДПИСЬ ТИПОВ ДАННЫХ ДЛЯ ТЕКСТ-БЛОКОВ


static public string GetVarTypesString(List<string> data_types_list)
{
    try
    {
        if ( data_types_list == null ) return null;

        //---  для удобства вместо многоточия подписываем вход его типом данных, Он потом затрется при привязке переменной.
        StringBuilder var_type = new StringBuilder();
        foreach (string str in data_types_list) var_type.AppendLine(" " + str + " ");
        return var_type.ToString();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}


//***************    MultiBinding for VARs


static public void SetVarBinding(TextBlock sender, SYMBOLS.Symbol_Data symbol_data)
{
    try
    {

        //  В Tage каждого TextBlock при его размежении на поле будет положена ссылка на его владельца Element.
        //  Ищем на котором из TextBlockov кликнули.
        Element_Data element = (Element_Data)((TextBlock)sender).Tag;

        int index = element.IO_TextBlocks_list.IndexOf((TextBlock)sender);
        
//*********   TYPES COMPATABILITY CHECKING
                                                      // выбираем пару io_list //  выбираем 0-й или 1-й элемент в паре.
        ElementImage.IO_Data io_data = (element.Txt_Image.IO_list[index/2])[index%2] ;

        //  поиск - есть ли в списке допустимых для элемента типов тип выбранного из таблицы символа.
        int ok = 0;
        StringBuilder str_types = new StringBuilder();
        foreach(string type in io_data.Types )
        {
            if (symbol_data.Data_Type == type)
            {
                ok = 1;
                break;
            }
            str_types.Append(type + " ");
        }

        if (ok == 0)
        {
            MessageBox.Show("Incompatible types. Must be one of: " + str_types.ToString());
            return;
        }

//*********   In-OUT inputs and READ-ONLY Types COMPATABILITY CHECKING        

        //  Не подсоединят ли переменную типа Read_only на выход элемента.
        //  Read_only определяется по типу памяти с буквой 'I'.
        if (io_data.IO_Type == "OUT")
        {
            if (symbol_data.Memory_Type.Contains("RD"))
            {
                MessageBox.Show("Can't place Read-only variable on elements output.");
                return;
            }

            //  For Local symbols
            if (symbol_data.Memory_Type.Contains("IN"))
            {
                MessageBox.Show("Can't place Input-only variable on elements output.");
                return;
            }

        }

       

//*********   BINDING

        Binding name_binding = new Binding();
        name_binding.Source = symbol_data;
        name_binding.Path = new PropertyPath("Name");

        Binding owner_binding = new Binding();
        owner_binding.Source = symbol_data;
        owner_binding.Path = new PropertyPath("Owner");

        Binding type_binding = new Binding();
        type_binding.Source = symbol_data;
        type_binding.Path = new PropertyPath("Data_Type");

        Binding comment_binding = new Binding();
        comment_binding.Source = symbol_data;
        comment_binding.Path = new PropertyPath("Comment");

        Binding real_value_binding = new Binding();
        real_value_binding.Source = symbol_data;
        real_value_binding.Path = new PropertyPath("fstr_Real_Value");

        Binding is_enable_binding = new Binding();   //  Для обновления надписей при включении/выключении отладчика
        is_enable_binding.Source = Step5.DEBUG;
        is_enable_binding.Path = new PropertyPath("IsEnable");

        MultiBinding multi_binding = new MultiBinding();

        multi_binding.Bindings.Add(name_binding);
        multi_binding.Bindings.Add(type_binding); 
        multi_binding.Bindings.Add(owner_binding);
        multi_binding.Bindings.Add(comment_binding);

        multi_binding.Bindings.Add(real_value_binding);
        multi_binding.Bindings.Add(is_enable_binding);


        multi_binding.Converter = new MultiBinding_Converter();
        ((TextBlock)sender).SetBinding(TextBlock.TextProperty, multi_binding);

        //int index = element.IO_TextBlocks_list.IndexOf((TextBlock)sender);
        element.IO_VARs_list[index] = symbol_data.Owner[symbol_data.Owner.IndexOf(symbol_data)];
        element.IO_Binding_list[index] = multi_binding;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

//**************    MULTI BINDING CONVERTER 

public class MultiBinding_Converter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        try
        {
            //  string str = Memory_Type + Address;     
            //if ( Bit_Number != null ) str = str + "." + Bit_Number;
            //str = str + "\n" + Comment + "\n\"" + Name + "\"";
            //Binding_Name = str;

            /*            StringBuilder str = new StringBuilder();
                        // Вынимаем в том порядке в котором ложили в multi_binding.Bindings.Add(...).
                        str.Append((string)values[0]); // Memory_Type
                        //str.Append("\n");

                        str.Append((int?)values[1]); // Address
            
                        if ((int?)values[2] != null)   // Bit_Number
                        {
                            str.Append(".");
                            str.Append((int?)values[2]);
                        }
                        str.Append("\n");

                        str.Append((string)values[3]); // Comment
                        str.Append("\n");

                        str.Append("\"");
                        str.Append((string)values[4]);    // Name
                        str.Append("\"");
                        str.Append("\n");
            */

            StringBuilder str = new StringBuilder();

            // Вынимаем в том порядке в котором ложили в multi_binding.Bindings.Add(...).
            if (Step5.DEBUG != null && values[4] != null) 
            {
                if (Step5.DEBUG.IsEnable == true)
                {
                    str.Append("[ " + (string)values[4] + " ]"); // Value
                    str.Append("\n");
                }
            }

            str.Append("\"" + (string)values[0] + "\""); // Name
            str.Append("\n");

            str.Append((string)values[1]); // Data_Type
            //str.Append("\n");

            string list = "LOCAL";
            if ((ObservableCollection<SYMBOLS.Symbol_Data>)values[2] == SYMBOLS.GLOBAL_SYMBOLS_LIST) list = "GLOBAL";
            if ((ObservableCollection<SYMBOLS.Symbol_Data>)values[2] == SYMBOLS.BASE_SYMBOLS_LIST) list = "BASE";
            if ((ObservableCollection<SYMBOLS.Symbol_Data>)values[2] == SYMBOLS.STORABLE_SYMBOLS_LIST) list = "SETPOINT";
            if ((ObservableCollection<SYMBOLS.Symbol_Data>)values[2] == SYMBOLS.MSG_SYMBOLS_LIST) list = "MSG";

            str.Append(", " + list);
            str.Append("\n");

            str.Append((string)values[3]);    // Comment

            return str.ToString();
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo cultureInfo)
    {
        return new object[0];
    }

}  

        }

    }  //---------  END of Class Step5
}  


