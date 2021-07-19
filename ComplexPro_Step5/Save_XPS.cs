// + Save_XPS создает новую диаграмму а не сохраняет существующую с именем комментарием и т.д.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;


namespace ComplexPro_Step5
{
public partial class Step5
{

static public class Save_XPS
    {

        static public double xPage_Height = 297 / 25.4 * 96;
        static public double xPage_Width = 210 / 25.4 * 96;
        static public double xPageMargin_Left = 20 / 25.4 * 96;
        static public double xPageMargin_Right = 10 / 25.4 * 96;
        static public double xPageMargin_Top = 15 / 25.4 * 96;
        static public double xPageMargin_Bottom = 15 / 25.4 * 96;
     
            
//***************

        static public double xStroke_Thickness = 1.0;
        static public double xHeader_RowHeight = 30.0;
        static public double xHeader_Height = 0;
        static public double xRowHeight = 20.0;
            
            //---

        static public List<List<string>> HEADERS_LIST;
        static public List<double> COLLUMNS_WIDTH_LIST;
        static public List<List<string>> DATA_LIST ;
            

//***********    SAVE XPS PROJECT

static public void SAVE_XPS_PROJECT()   
{

    try
    {

        Window XPS_Viewer_window = new Window();

            DocumentViewer DocViewer = new DocumentViewer();

                FixedDocument doc = new FixedDocument();

//*************  PAGE 1

                    PageContent current_page_content = new PageContent();
                            
                        FixedPage current_fixed_page = new FixedPage();

                        //  страница в книжном формате.
                        GetNextPage(doc, ref current_page_content, ref current_fixed_page, xPage_Width, xPage_Height);
                double current_x = xPage_Width*0.2;
                double current_y = xPage_Height*0.3;

                current_fixed_page.Children.Add(Get_TextBlock("Project:  " + PROJECT_NAME, 36, current_x, current_y));


//**************    GLOBAL SYMBOLS TABLE

                //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                GetNextPage(doc, ref current_page_content, ref current_fixed_page, xPage_Height, xPage_Width);
                current_x = xPageMargin_Left;
                current_y = xPageMargin_Top;
                //---

                current_fixed_page.Children.Add(Get_TextBlock("Global symbols:", 16, current_x, current_y));
                current_y += 40;

                    List<List<string>> headers, data_collumns ;
                    List<string> alignment ;
                    List<double> collumn_widths ;

                    data_collumns = SYMBOLS.SYMBOLS_LIST_ToXpsList(SYMBOLS.GLOBAL_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;


//**************    BASE SYMBOLS TABLE


                    current_fixed_page.Children.Add(Get_TextBlock("Base symbols:", 16, current_x, current_y));
                    current_y += 40;

                    data_collumns = SYMBOLS.BASE_SYMBOLS_LIST_ToXpsList(SYMBOLS.BASE_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;


//**************    BASE SYMBOLS TABLE


                    current_fixed_page.Children.Add(Get_TextBlock("Base symbols tags:", 16, current_x, current_y));
                    current_y += 40;

                    data_collumns = SYMBOLS.BASE_SYMBOLS_TAGS_LIST_ToXpsList(SYMBOLS.BASE_TAGS_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;


//**************    STORABLE SYMBOLS TABLE


                    current_fixed_page.Children.Add(Get_TextBlock("SetPoint symbols:", 16, current_x, current_y));
                    current_y += 40;

                    data_collumns = SYMBOLS.STORABLE_SYMBOLS_LIST_ToXpsList(SYMBOLS.STORABLE_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;


//**************    MSG SYMBOLS TABLE


                    current_fixed_page.Children.Add(Get_TextBlock("Msg symbols:", 16, current_x, current_y));
                    current_y += 40;

                    data_collumns = SYMBOLS.MSG_SYMBOLS_LIST_ToXpsList(SYMBOLS.MSG_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;


//**************    INDICATION SYMBOLS TABLE


                    current_fixed_page.Children.Add(Get_TextBlock("Indicatiuon symbols:", 16, current_x, current_y));
                    current_y += 40;

                    data_collumns = SYMBOLS.INDICATION_SYMBOLS_LIST_ToXpsList(SYMBOLS.INDICATION_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;


//**************    LOCAL SYMBOLS TABLES

                foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
                {
                    current_fixed_page.Children.Add(Get_TextBlock("Local symbols. Diagram: " + diagram.NAME, 16, current_x, current_y));
                    current_y += 40;

                    data_collumns = SYMBOLS.LOCAL_SYMBOLS_LIST_ToXpsList(diagram.LOCAL_SYMBOLS_LIST, out headers, out alignment, out collumn_widths);

                    //  выводим в альбомном формате: меняем местами  xPage_Height и xPage_Width.
                    Table_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Height, xPage_Width,
                            headers, collumn_widths, alignment, data_collumns, null);

                    current_y += 40;
                }



//**************    NETWORKS

                    //  выводим в книжном формате.
                    Networks_to_FixedDoc(doc, ref current_page_content, ref current_fixed_page, current_x, ref current_y, xPage_Width, xPage_Height);

//**************    PAGE NUMBERS

                    Page_Numbers(doc, "Project: " + PROJECT_NAME +
                                      "   Date: " + DiagramCompiler.GetDateString());

//***************  PRINT INTO WINDOW

            DocViewer.Document = doc;

        XPS_Viewer_window.Content = DocViewer;

        XPS_Viewer_window.Show();

//***************  PRINT INTO FILE

        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
        //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
        dlg.InitialDirectory = PROJECT_DIRECTORY_PATH;//.Remove(PROJECT_PATH.LastIndexOf('\\')); 
        dlg.FileName = PROJECT_NAME; // Default file name
        dlg.DefaultExt = ".xps"; // Default file extension
        dlg.Filter = "XPS (.xps)|*.xps"; // Filter files by extension

        Nullable<bool> result = dlg.ShowDialog();
        if (result == true)
        {
            XpsDocument xps_document = new XpsDocument(dlg.FileName, System.IO.FileAccess.Write);

            XpsDocumentWriter xps_writer = XpsDocument.CreateXpsDocumentWriter(xps_document);

            xps_writer.Write(doc);

            xps_document.Close();
        }

        return;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}

//*****************   

static public TextBlock Get_TextBlock(string text, double fontsize, double x, double y)
{
    try
    {
            TextBlock text_block = new TextBlock();
            text_block.Text = text;
            text_block.FontSize = fontsize;
            text_block.FontWeight = FontWeights.Bold;
            text_block.SetValue(FixedPage.TopProperty, y);
            text_block.SetValue(FixedPage.LeftProperty, x);

            return text_block;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}



static public void Page_Numbers(FixedDocument doc, string page_title)
{
    try
    {
        for (int i = 0; i < doc.Pages.Count; i++)
        {
            PageContent page_content = doc.Pages[i];
            FixedPage fixed_page = page_content.Child;

            double height = fixed_page.Height - xPageMargin_Bottom * 0.7;
            double width = fixed_page.Width;
            //---

            TextBlock text_block = new TextBlock();
            text_block.Text = page_title;
            text_block.FontSize = 10;
            text_block.SetValue(FixedPage.TopProperty, height);
            text_block.SetValue(FixedPage.LeftProperty, xPageMargin_Left);

            fixed_page.Children.Add(text_block);
            //---

            text_block = new TextBlock();
            text_block.Text = (i+1).ToString();
            text_block.FontSize = 12;
            text_block.SetValue(FixedPage.TopProperty, height);
            text_block.SetValue(FixedPage.LeftProperty, width - xPageMargin_Right - 10);

            fixed_page.Children.Add(text_block);
            //---

        }

        return;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}


//*****************   NETWORKS TO FIXED DOC


static public void Networks_to_FixedDoc( FixedDocument doc, 
                          ref PageContent current_page_content, 
                          ref FixedPage current_fixed_page,
                          double current_x, ref double current_y,
                          double page_width, double page_height)
{
    try
    {
        //***************    ФОРМИРУЕМ КОПИИ НЕТВОРКОВ И ДИАГРАММ

        List<Diagram_Of_Networks> diagrams_list = new List<Diagram_Of_Networks>();

        //foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        //{
          //  Diagram_Of_Networks xdiagram = new Diagram_Of_Networks();
            //xdiagram.NETWORKS_LIST.Clear();//  убираем нетворк который сформировался автоматически конструктором.

            //foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
            //{
                //Network_Of_Elements xnetwork = network.GetNetworkCopy();
                //xnetwork.DIAGRAM = xdiagram;
                //xnetwork.Get_NetworkImage(); //прорисовываем вхолостую чтобы заполнился MAIN_BORDER_PANEL;
                //xdiagram.NETWORKS_LIST.Add(xnetwork);
            //}
            //diagrams_list.Add(xdiagram);
        //}

        foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        {
            Diagram_Of_Networks diagram_copy = diagram.GetDiagramCopy();
            //  возвращаем первоначальное имя диаграмме, т.к. при создании копии к ее имени 
            // добавляется символ для исключения дублирования имен.
            diagram_copy.NAME = diagram.NAME;  
            diagrams_list.Add(diagram_copy);
        }

        //****************    ВЫВОДИМ КОПИИ НЕТВОРКОВ


        foreach (Diagram_Of_Networks diagram in diagrams_list)
        {

//---  NEW DIAGRAM   -   с новой страницы

            GetNextPage(doc, ref current_page_content, ref current_fixed_page, page_width, page_height);
            current_x = xPageMargin_Left;
            current_y = xPageMargin_Top;

//---  DIAGRAM NAME

            TextBlock text_block = new TextBlock();
                text_block.Text = "Diagram:  " + diagram.NAME;
                text_block.FontSize = 16;
                text_block.FontWeight = FontWeights.Bold;
                text_block.SetValue(FixedPage.TopProperty, current_y);
                text_block.SetValue(FixedPage.LeftProperty, current_x);
            current_fixed_page.Children.Add(text_block);

            current_y += 30;

//---  DIAGRAM COMMENT

                TextBox text_box = new TextBox();
                text_box.Text = "Comment:  " + diagram.COMMENT;
                text_box.MinWidth = 300;
                text_box.MaxWidth = 700;
                text_box.TextWrapping = TextWrapping.Wrap;
                text_box.SetValue(FixedPage.TopProperty, current_y);
                text_box.SetValue(FixedPage.LeftProperty, current_x);
                current_fixed_page.Children.Add(text_box);

            current_y += 50;
            //---


//---  NETWORKS

            // прорисовываем вхолостую Get_NetworkImage() чтобы заполнился MAIN_BORDER_PANEL;
            //  т.к. далее при напихивании нетворков на страницу: если очередной нетворк после примерки
            //   не влазит, на следующей странице повторно берется его имидж - а сделать это дважды вызвав
            //   GetImage() нельзя, т.к. он одноразовый.
            for (int i = 0; i < diagram.NETWORKS_LIST.Count; i++)  diagram.NETWORKS_LIST[i].Get_NetworkImage(); 

            for (int i = 0; i < diagram.NETWORKS_LIST.Count; )
            {
                StackPanel stack_panel = new StackPanel();
                stack_panel.Orientation = Orientation.Vertical;
                //stack_panel.Background = new SolidColorBrush(Colors.AliceBlue);
                    ScaleTransform scale_transform = new ScaleTransform(1.0, 1.0);
                stack_panel.LayoutTransform = scale_transform;
                stack_panel.SetValue(FixedPage.TopProperty, current_y);
                stack_panel.SetValue(FixedPage.LeftProperty, current_x);
                //---                

                double xpage_width = page_width - xPageMargin_Right - current_x;
                double xpage_height = page_height - xPageMargin_Bottom - current_y;
                //---


//---   Напихиваем нетворки в стекпанель пока их не станет на 10% больше допустммой высоты стекпанели.

                for (int j = 0; i < diagram.NETWORKS_LIST.Count; i++, j++)
                {
                    Network_Of_Elements network = diagram.NETWORKS_LIST[i];
                    Border border = network.MAIN_BORDER_PANEL;
                    //---
                    stack_panel.Children.Add(border);

                    // делаем перерасчет рамеров
                    stack_panel.Measure(new Size(xpage_width*10, xpage_height*10));
                    stack_panel.Arrange(new Rect());

                    //   если вылезли за 1.1 то последний элемент выбрасываем если он не единственный.
                    //   после этого ужимаем панель масштабом и оформляем страницу.

                    double kyy = 1.1;
                    //  если коэффициент ужатия по оси Х будет выше чем 1.1 то принимаем его и по оси Y
                    //  иначе в конце страницы после ужатия по Х и Y получаются пустые места куда мог бы 
                    //  влезть еще один нетворк.
                    double kxx = stack_panel.ActualWidth / xpage_width;
                    if (kxx > kyy) kyy = kxx;

                    if (stack_panel.ActualHeight > xpage_height * kyy)
                    {
                        if (j != 0) stack_panel.Children.Remove(border);
                        else i++;
                        //---                          
                        break;
                    }
                }

//---  ОФОРМЛЯЕМ  current_fixed_page

                // делаем итоговый перерасчет рамеров
                //???? если задать ширину требуемую фактическую "page_width", то почему-то
                // при превышении стекпанелью этой ширины ActualWidth возвращает величину "page_width"
                // а рисует стекпанель на экране выходящую за пределы "page_width"
                stack_panel.Measure(new Size(xpage_width*10, xpage_height*10));
                stack_panel.Arrange(new Rect());

                double kx = 1.0, ky = 1.0;
                
                    if (stack_panel.ActualHeight > xpage_height)
                    {
                        ky = xpage_height / stack_panel.ActualHeight;
                        //---
                        current_y += xpage_height;
                    }
                    else current_y += stack_panel.ActualHeight;
                    
                    if (stack_panel.ActualWidth > xpage_width) kx = xpage_width / stack_panel.ActualWidth;
                
                    //  масштабируем в двух измерениях по наибольшему ужатию.
                    if (kx > ky) kx = ky;

                scale_transform.ScaleX = kx;
                scale_transform.ScaleY = kx;
                //---                       
 
                current_fixed_page.Children.Add(stack_panel);
                //---

//---    Если это был не последний элемент, то продолжаем
                if (i < diagram.NETWORKS_LIST.Count)
                {
                    GetNextPage(doc, ref current_page_content, ref current_fixed_page, page_width, page_height);
                    current_x = xPageMargin_Left;
                    current_y = xPageMargin_Top;
                }
            }

        }

        return;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}



//*****************   TABLE TO FIXED DOC


static public void Table_to_FixedDoc( FixedDocument doc, 
                          ref PageContent current_page_content, 
                          ref FixedPage current_fixed_page,
                          double current_x, ref double current_y,
                          double page_width, double page_height,
                          List<List<string>> headers, List<double> collumns_width, List<string> alignment,
                          List<List<string>> rows_data, List<List<string>> collumns_data)
{
    try
    {

                HEADERS_LIST = headers;
                COLLUMNS_WIDTH_LIST = collumns_width;

                xHeader_Height = xHeader_RowHeight * HEADERS_LIST.Count;

                if ( rows_data != null ) DATA_LIST = rows_data;
                else if (collumns_data != null)
                {
                    //  Переворачиваем массив колонок в массив строк.
                    
                    DATA_LIST = new List<List<string>>();
                                        
                    int max = 0;
                    //  ищем самый длинный массив.
                    foreach (List<string> list in collumns_data) { if (list.Count > max) max = list.Count; }

                    for (int i = 0; i < max; i++)
                    {
                        List<string> new_list = new List<string>();
                        //---
                        for (int j = 0; j < collumns_data.Count; j++)
                        {
                            if (i < collumns_data[j].Count) new_list.Add(collumns_data[j][i]);
                            else new_list.Add("---");
                        }
                        //---
                        DATA_LIST.Add(new_list);
                    }
                }

//****************   ПЕЧАТАЕМ DOC

                    int start_index = 0;
                    int end_index = DATA_LIST.Count - 1;
                    //---
                    Canvas canvas = GetCanvas(0, 0);
                    current_fixed_page.Children.Add(canvas);
                    

                        //      Выводим заголовок если он влазит на страницу, 
                        //   иначе выводим его со следующей страницы.
                    if (GetHeader(canvas, current_x, ref current_y, page_height-xPageMargin_Bottom) != null)
                        {
                            GetNextPage(doc, ref current_page_content, ref current_fixed_page, page_width, page_height);
                            //---
                            current_y = xPageMargin_Top;
                            //---
                            canvas = GetCanvas(0, 0); 
                            current_fixed_page.Children.Add(canvas);
                            //---
                            GetHeader(canvas, current_x, ref current_y, page_height-xPageMargin_Bottom);
                        }


                        //      Выводим строки таблицы. Если они влазят на страницу, 
                        //   то продолжаем выводить их со следующей страницы.
                        while (GetRows(canvas, current_x, ref current_y, page_height - xPageMargin_Bottom, ref start_index, end_index, alignment) != null)
                        {
                            GetNextPage(doc, ref current_page_content, ref current_fixed_page, page_width, page_height);
                            //---
                            current_y = xPageMargin_Top;
                            //---
                            canvas = GetCanvas(0, 0);
                            current_fixed_page.Children.Add(canvas);
                        }

                    return ;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

                return;

            }



//*************   NEW PAGE

static public string GetNextPage(FixedDocument doc,
                                 ref PageContent current_page_content, 
                                 ref FixedPage current_fixed_page,
                                 double page_width, double page_height)
            {
                try
                {
                    //if (current_page_content.Child == null)  current_page_content.Child = current_fixed_page;

                    //if (current_page_content.Child.Children.Contains(current_fixed_page)) { }
                    //else current_page_content.Child = current_fixed_page ;

                    //if ( !doc.Pages.Contains(current_page_content)) doc.Pages.Add(current_page_content);

                    //---
                    current_page_content = new PageContent();
                    current_fixed_page = new FixedPage();
                    current_fixed_page.Width = page_width;
                    current_fixed_page.Height = page_height;
                    //---

                    current_page_content.Child = current_fixed_page;
                    doc.Pages.Add(current_page_content);

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

                return null;
            }



//*************   HEADER 

static public string GetHeader(Canvas canvas, double x, ref double y, double y_max)
            {
                try
                {
                    //  Если заголовок и +1 строка помещаются на страницу то продолжаем, иначе с новой старницы.
                    if (y + xHeader_RowHeight*HEADERS_LIST.Count+xRowHeight > y_max)   return "Continue"; 

                    foreach (List<string> list in HEADERS_LIST)
                    {
                        GetHeaderRow(canvas, x, ref y, list);
                    }
                    //---
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

                return null;
            }



//*************   HEADER ROW


static public void GetHeaderRow(Canvas canvas, double x, ref double y, List<string> collumns_names)
            {
                try
                {
                    
                    //*********  общая ширина.
                    double width = 0;
                    foreach (double wid in COLLUMNS_WIDTH_LIST) width += wid;
                    
                    canvas.Children.Add(GetPolyline(x - xStroke_Thickness / 2, y, x + width + xStroke_Thickness / 2, y));

                    //canvas.Children.Add(GetPolyline(x, y, x, y + xHeader_RowHeight));
                    //canvas.Children.Add(GetPolyline(x + width, y - xStroke_Thickness / 2, x + width, y + xHeader_RowHeight));

                    //*************

                    //  Объединяем колонки для которых не заданы имена.

                    //  коллекции для последующего выравнивая высоты строки по border с максимальной высотой.
                    List<Border> borders_list = new List<Border>();
                    List<double> xx_list = new List<double>();
                    
                    double ax = xStroke_Thickness / 2;                        
                    double xx = x;
                    double collumn_width = 0;
                    int ii = -1;
                    for (int i = 0; i < COLLUMNS_WIDTH_LIST.Count; i++ )
                    {
                        collumn_width += COLLUMNS_WIDTH_LIST[i];
                        if (ii == -1) ii = i;
                        //---
                        if (i < COLLUMNS_WIDTH_LIST.Count - 1 && collumns_names[i + 1] == null) continue;
                        //---
                        Border border = GetTextBlock(collumns_names[ii], xx+ax, y+ax, collumn_width-2*ax, xHeader_RowHeight, "Center");
                        canvas.Children.Add(border);
                        //---
                        xx += collumn_width; collumn_width = 0; ii = -1;
                        //---
                        borders_list.Add(border);  //  накапливаем borders и xx
                        xx_list.Add(xx);
                    }

//***********   Выравнивание строки по высоте максимально высокого TextBlock.

                    //---  Поиск в построеной строке TextBlock с максимальной высотой.
                    double max = 0;
                    foreach (Border border in borders_list)
                    {
                        TextBlock textblock = (TextBlock)(border.Child);
                        // делаем перерасчет рамеров
                        textblock.Measure(new Size(border.Width, border.Height));
                        textblock.Arrange(new Rect());
                        
                        if (max < textblock.ActualHeight) max = textblock.ActualHeight;
                    }

                    //   выравниваем высоту всех Border равной масимальной.
                    for (int i = 0; i < borders_list.Count ; i++)
                    {
                        borders_list[i].Height = max;
                        canvas.Children.Add(GetPolyline(xx_list[i], y, xx_list[i], y + max+2*ax));
                    }

                    canvas.Children.Add(GetPolyline(x, y, x, y + max + 2 * ax));
//**********
                    //---  Для внешних программ - на каком Y закончили.
                    y += max+2*ax;// xHeader_RowHeight;

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

                return;
            }


//******************    GET  ROWS

static public string  GetRows(Canvas canvas, double x, ref double y, double y_max, ref int start_index, int end_index, List<string> alignment)
            {
                string ret_code = null;

                try
                {
                                        
                    //*********  общая ширина.
                    double width = 0;
                    foreach (double wid in COLLUMNS_WIDTH_LIST) width += wid;

                    canvas.Children.Add(GetPolyline(x - xStroke_Thickness / 2, y, x + width + xStroke_Thickness / 2, y));


                    //*************

                    int xend_index = end_index;
                    if (xend_index > DATA_LIST.Count-1) xend_index = DATA_LIST.Count-1;

                    
                    for (int i = start_index; i <= xend_index; i++)
                    {

                        //  коллекции для последующего выравнивая высоты строки по border с максимальной высотой.
                        List<Border> borders_list = new List<Border>();
                        List<double> xx_list = new List<double>();

                        double ax = xStroke_Thickness / 2;
                        double xx = x;

//***  сначала нахождение максимальной высоты, затем проверку влезет ли стока а затем ее размещение

                        for (int j = 0; j < DATA_LIST[i].Count; j++)
                        {
                            //---
                            Border border = GetTextBlock(DATA_LIST[i][j], xx + ax, y + ax, COLLUMNS_WIDTH_LIST[j] - 2 * ax, xRowHeight, alignment[j]);
                            //canvas.Children.Add(border);
                            //---
                            xx += COLLUMNS_WIDTH_LIST[j]; 
                            //---
                            borders_list.Add(border); //  накапливаем borders и xx
                            xx_list.Add(xx);
                        }

//***********   Выравнивание строки по высоте максимально высокого TextBlock.

                        //---  Поиск в построеной строке TextBlock с максимальной высотой.
                        double max = 0;
                        foreach (Border border in borders_list)
                        {
                            TextBlock textblock = (TextBlock)(border.Child);
                            // делаем перерасчет рамеров
                            textblock.Measure(new Size(border.Width, border.Height));
                            textblock.Arrange(new Rect());
                        
                            if (max < textblock.ActualHeight) max = textblock.ActualHeight;
                        }
                       //---

                    if (y + max > y_max)
                    {
                        start_index = i;
                        ret_code = "Continue";
                        break;
                    }
                        //---

                        //   выравниваем высоту всех Border равной масимальной.
                        for (int ii = 0; ii < borders_list.Count ; ii++)
                        {
                            borders_list[ii].Height = max;
                            canvas.Children.Add(borders_list[ii]);
                            canvas.Children.Add(GetPolyline(xx_list[ii], y, xx_list[ii], y + max+3*ax));
                        }

                        canvas.Children.Add(GetPolyline(x, y, x, y + max + 3 * ax));
//**********
                        //---  Для внешних программ - на каком Y закончили.
                        y += max+3*ax;// xHeader_RowHeight;

                    }

                    canvas.Children.Add(GetPolyline(x - xStroke_Thickness / 2, y, x + width + xStroke_Thickness / 2, y));

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

                return ret_code;
            }



//**************    Универсальная Polyline

static public Polyline GetPolyline(double x1, double y1, double x2, double y2)//SolidColorBrush color, double thickness)
            {
                Polyline polyline = new Polyline();
                polyline.Stroke = new SolidColorBrush(Colors.Black);
                polyline.StrokeThickness = xStroke_Thickness;
                polyline.SnapsToDevicePixels = true;

                Point point1 = new Point(x1, y1);
                Point point2 = new Point(x2, y2);

                polyline.Points.Add(point1);
                polyline.Points.Add(point2);

                return polyline;
            }

//**************

static public Canvas GetCanvas(double x, double y)
            {
                Canvas canvas = new Canvas();
                //canvas.MinWidth  = xCanvasWidth;
                //canvas.MinHeight = xCanvasHeight;
                //canvas.Background = xCanvasBackground;
                //canvas.VerticalAlignment = VerticalAlignment.Stretch;
                //canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                //canvas.ClipToBounds = true;
                canvas.SetValue(FixedPage.LeftProperty, x);
                canvas.SetValue(FixedPage.TopProperty, y);

                return canvas;
            }

//**************

static public Border GetTextBlock(string text, double x, double y, double width, double height, string alignment)
            {
                Border border = new Border();
                border.Width = width;
                border.Height = height;
                //border.Background = new SolidColorBrush(Colors.AliceBlue);

                TextBlock text_block = new TextBlock();
                text_block.Text = text;
                text_block.FontFamily = new FontFamily("Italic");
                text_block.FontSize = 14;
                text_block.Padding = new Thickness(5, 2, 5, 2);
                text_block.VerticalAlignment = VerticalAlignment.Center;
                text_block.TextWrapping = TextWrapping.Wrap;

                if (alignment == "Center")
                {
                    text_block.HorizontalAlignment = HorizontalAlignment.Center; 
                    text_block.TextAlignment = TextAlignment.Center;
                }
                else if (alignment == "Left")
                {
                    text_block.HorizontalAlignment = HorizontalAlignment.Left;
                    text_block.TextAlignment = TextAlignment.Left;
                }
                else if (alignment == "Right")
                {
                    text_block.HorizontalAlignment = HorizontalAlignment.Right;
                    text_block.TextAlignment = TextAlignment.Right;
                }

                border.Child = text_block;
                border.SetValue(Canvas.LeftProperty, x);
                border.SetValue(Canvas.TopProperty, y);

                return border;
            }


        }


//**********  END of MainWindow
    }
}
