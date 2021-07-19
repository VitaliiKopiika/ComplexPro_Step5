// + Сделать вывод темплате или пустого проекта если произошла ошибка при загрузке.
// + Похоже путает координату "Y" с буквой "Y" в имени функции "SYS_TIME" - изменить имя координаты или сразу сделать поиск сочетания "Y=\""
// + сохранять Min Max Step K надо не как стринг а как double чтобы не терять точность из-за урезания дроби заданным форматом
// + сделать сохранение и загрузку номиналов.
// - При загрузке имен сделать проверку неповторяемости имен и адресов.

// + Перед загрузкой сделать контроль того что прежний проект сохранен.
// + добавить в конструктор список базовых и сохраняемых переменных
// + перенести в контсрукторы элементов поиск symbol по имени, привязку нетворков и т.д.
// + при добавлении пустого нетворка на поле и при сохранении сохраняется какой-то мусор.

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
using System.IO;

namespace ComplexPro_Step5
{
    public partial class Step5
    {


//*********************************************
static  partial class SaveProject
    {

//***************   NEW_PROJECT

static public void NEW_PROJECT(string mode)
{
        StreamReader file_stream = null;

        try
        {
            if (mode == "Load template" || mode == "Load empty")
            {
                //---  SAVE EXISTING PROJECT
                MessageBoxResult res = MessageBox.Show("Save changes?", PROJECT_NAME, MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

                if (res == MessageBoxResult.Cancel) return;
                //if (res == MessageBoxResult.Yes) SaveProject.SAVE_PROJECT("Save");
                if (res == MessageBoxResult.Yes) ApplicationCommands.Save.Execute("Save", null);

            }
            else if (mode == "Load template by default") {}
            else if (mode == "Load empty by default") {}
            

            //---  Создание нового автоматического имени проекта в корневом каталоге.
            Step5.GENERATE_STANDART_PROJECTS_FILE_NAME();
            


            //*************    OPEN NEW PROJECT TEMPLATE FILE

            if (mode != "Load empty" && mode != "Load empty by default")
            {
                //---  Загрузка шаблона нового проекта.
                string project_file_path = NEW_PROJECT_TEMPLATE_FILE_PATH;//APPLICATION_DIRECTORY_PATH + "\\" + "New_Project.ns5";

                Application.Current.MainWindow.Title = PROJECT_NAME;
                //---

                ERRORS.Clear();
                ERRORS.Message("\nCreating new project...");
                //-----------

                int tst = 0;

                if (File.Exists(project_file_path))
                {
                    ERRORS.Message("\n>Loading new project template file...");

                    file_stream = new StreamReader(project_file_path);

                    ERRORS.Message("\n>>Loading symbols...");
                    LOAD_SYMBOLS(file_stream);  //  должно идти перед SAVE DIAGRAMS для последующей загрузки,
                    //т.к. когда грузятся нетворки уже должны быть загружены переменные для их привязки.

                    if (ERRORS.Count == 0)
                    {
                        ERRORS.Message("\n>>Loading diagrams...");
                        //  открываем заново, чтобы считать строки от начала файла для номеров ошибок.
                        file_stream.Close();
                        file_stream = new StreamReader(project_file_path);

                        LOAD_DIAGRAMS(file_stream);
                    }

                    if (ERRORS.Count != 0)
                    {
                        ERRORS.Message("\nErrors: " + ERRORS.Count);
                        MessageBox.Show("New Project loading errors!");
                    }
                    else
                    {
                        ERRORS.Message("\nNo errors.");
                        tst = 1;
                    }
                }
                else
                {
                    ERRORS.Add("New project template file <" + project_file_path + "> doesn't exists.");
                    ERRORS.Message("\n>Creating new empty project.");
                }


                //  Если загрузить новый проект из шаблонного файла не удалось, то
                //  создаем типовый нулевой проект.
                if (tst == 0)
                {
                    SYMBOLS_LIST = new Step5.SYMBOLS();
                    DIAGRAMS_LIST = new List<Diagram_Of_Networks>() { new Step5.Diagram_Of_Networks() };
                }
            }
            else
            {
                SYMBOLS_LIST = new Step5.SYMBOLS();
                DIAGRAMS_LIST = new List<Diagram_Of_Networks>() { new Step5.Diagram_Of_Networks() };
            }

                SHOW_DIAGRAMS();

                //---  Сразу сохраняем созданный проект и в память и в RecentProjects            
                if (mode == "Load template" || mode == "Load empty") 
                                            ApplicationCommands.Save.Execute("Save as", null);//SaveProject.SAVE_PROJECT("Save as");
                else                        ApplicationCommands.Save.Execute("Save", null); //SaveProject.SAVE_PROJECT("Save");
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
        finally
        {
            if (file_stream != null) file_stream.Close();
        }
    }


//***************   SAVE_PROJECT

static public void com_SAVE_PROJECT_ExecutedHandler(object obj, ExecutedRoutedEventArgs e)
{
    _SAVE_PROJECT((string)e.Parameter);

    //object obj2 = e.Source

    e.Handled = true;
}


//*****

    static public void _SAVE_PROJECT(string mode)
    {

        StreamWriter file_stream = null;

        try
        {

            //*************    OPEN/CREATE FILE

            if (mode == "Save as" || PROJECT_DIRECTORY_PATH == null)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                //  Из полного пути с именем файла оставляем только путь, а имя удаляем.

                if (PROJECT_DIRECTORY_PATH != null) dlg.InitialDirectory = PROJECT_DIRECTORY_PATH;
                dlg.FileName = PROJECT_NAME; // Default file name
                dlg.DefaultExt = ".ps5"; // Default file extension
                dlg.Filter = "Project_S5 (.ps5)|*.ps5"; // Filter files by extension
                
                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    PROJECT_DIRECTORY_PATH = dlg.FileName.Remove(dlg.FileName.LastIndexOf('\\')+1); 
                    //---  вырезаем имя файла без расширения.
                    PROJECT_NAME = dlg.FileName.Split('\\').Last().Split('.')[0];
                    PROJECT_FILE_PATH = dlg.FileName;

                    Application.Current.MainWindow.Title = PROJECT_NAME;
                    //---
                    //file_stream = new StreamWriter(dlg.OpenFile());
                    //file_stream.AutoFlush = true;
                }
                else
                {
                    //MessageBox.Show("Can't save file!");
                    //return;
                }

                file_stream = new StreamWriter(PROJECT_DIRECTORY_PATH + PROJECT_NAME + ".ps5");
            }
            else if (mode == "Save") 
            {
                file_stream = new StreamWriter(PROJECT_DIRECTORY_PATH + PROJECT_NAME + ".ps5");
            }
            else if (mode == "Save as template") 
            {
                file_stream = new StreamWriter(NEW_PROJECT_TEMPLATE_FILE_PATH);
            }


            //file_stream = new StreamWriter(PROJECT_DIRECTORY_PATH + PROJECT_NAME + ".ps5");
            file_stream.AutoFlush = true;


            // Process save file dialog box results
                //*********  SAVE DIAGRAMS

                SAVE_SYMBOLS(file_stream);  //  должно идти перед SAVE DIAGRAMS для последующей загрузки,
                        //т.к. когда грузятся нетворки уже должны быть загружены переменные для их привязки.
                
                //*********  SAVE DIAGRAMS
                SAVE_DIAGRAMS(file_stream);

            //*********   Визуализация процесса записи.
            
            Cursor old_cursor = Application.Current.MainWindow.Cursor;
                Application.Current.MainWindow.Cursor = Cursors.Wait;
                int second = DateTime.Now.Second;
                while ((uint)(DateTime.Now.Second - second) < 1) { }
            Application.Current.MainWindow.Cursor = old_cursor;

            ADD_RECENT_PROJECT();
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
        finally
        {
            if (file_stream != null) file_stream.Close();
        }
    }


    //***************   SAVE SYMBOLS

    static public string SYMBOL_ToSaveString(SYMBOLS.Symbol_Data symbol)
    {
        try
        {
            //  Убираем из имени и комментариев все кавычки, иначе они собьют алгоритм считывания комментариев из файла.
            string name = symbol.Name.Replace('"', '\'');
            string comment = symbol.Comment.Replace('"', '\'');

// 18-06-18 После имени параметра должны без пробелов идти знак = и кавычка - это понадобилось когда 
//        имя параметра "Y" загрузчик начал путать с "Y" в имени функции SYS_TIME
//        поэтому для загрузчика сделали доиск по сочетанию имени параметра сразу со знаком = и кавычкой.

            string str =    " Name=\"" + name + "\"" +
                            " Tag_Name=\"" + symbol.Tag_Name + "\"" +
                            " Data_Type=\"" + symbol.Data_Type + "\"" +
                            " Memory_Type=\"" + symbol.Memory_Type + "\"" +
                            " Address=\"" + symbol.Address + "\"" +
                            " Bit_Base=\"" + symbol.Bit_Base + "\"" +
                            " Bit_Number=\"" + symbol.Bit_Number + "\"" +
                            " Initial_Value=\"" + symbol.Initial_Value + "\"" +
                            " str_Nom_Value=\"" + symbol.str_Nom_Value + "\"" +
                //  сохраняем double а не string значения, чтобы формат не съел точность
                            " K_Value=\"" + symbol.K_Value + "\"" +
                            " Min_Value=\"" + symbol.Min_Value + "\"" +
                            " Max_Value=\"" + symbol.Max_Value + "\"" +
                            " Step_Value=\"" + symbol.Step_Value + "\"" +
                            " Unit_Value=\"" + symbol.Unit_Value + "\"" +
                            " Format_Value=\"" + symbol.Format_Value + "\"" +
                            " Comment=\"" + comment + "\"";

            return str;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return excp.ToString();
        }
    }


    static public void SAVE_SYMBOLS(StreamWriter file_stream)
    {

        try
        {

            file_stream.WriteLine();
            file_stream.WriteLine("<Version" + " Compiler=\"" + Version.Compiler + "\"" +
                                               " FileFormat=\"" + Version.FileFormat + "\" />");
//**********    GLOBAL SYMBOLS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   GLOBAL SYMBOLS   *******************  --> ");

            file_stream.WriteLine();        
            file_stream.WriteLine("\n   <Symbols" + " Count=\"" + SYMBOLS.GLOBAL_SYMBOLS_LIST.Count + "\" >");
                                            
            foreach ( SYMBOLS.Symbol_Data symbol in SYMBOLS.GLOBAL_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine("      <Symbol" + SYMBOL_ToSaveString(symbol) + " />");
                }
                else  file_stream.WriteLine("      <Symbol Name=\"null\" />");
            }

            file_stream.WriteLine("   </Symbols>");

//**********    BASE SYMBOLS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   BASE SYMBOLS   *******************  --> ");

            file_stream.WriteLine();        
            file_stream.WriteLine("\n   <BaseSymbols" + " Count=\"" + SYMBOLS.BASE_SYMBOLS_LIST.Count + "\" >");

            foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.BASE_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine("      <BaseSymbol" + SYMBOL_ToSaveString(symbol) + " />");
                }
                else  file_stream.WriteLine("      <BaseSymbol Name=\"null\" />");
            }

            file_stream.WriteLine("   </BaseSymbols>");

//**********    BASE SYMBOLS TAGS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   BASE SYMBOLS TAGS  *******************  --> ");

            file_stream.WriteLine();        
            file_stream.WriteLine("\n   <BaseSymbolsTags" + " Count=\"" + SYMBOLS.BASE_TAGS_SYMBOLS_LIST.Count + "\" >");

            foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.BASE_TAGS_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine("      <BaseSymbolTag" + SYMBOL_ToSaveString(symbol) + " />");
                }
                else file_stream.WriteLine("      <BaseSymbolTag Name=\"null\" />");
            }

            file_stream.WriteLine("   </BaseSymbolsTags>");

//**********    STORABLE SYMBOLS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   STORABLE SYMBOLS   *******************  --> ");

            file_stream.WriteLine();
            file_stream.WriteLine("\n   <StorableSymbols" + " Count=\"" + SYMBOLS.STORABLE_SYMBOLS_LIST.Count + "\" >");

            foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.STORABLE_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine("      <StorableSymbol" + SYMBOL_ToSaveString(symbol) + " />");

                }
                else file_stream.WriteLine("      <StorableSymbol Name=\"null\" />");
            }

            file_stream.WriteLine("   </StorableSymbols>");

//**********    MSG_SYMBOLS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   MSG_SYMBOLS   *******************  --> ");

            file_stream.WriteLine();        
            file_stream.WriteLine("\n   <MsgSymbols" + " Count=\"" + SYMBOLS.MSG_SYMBOLS_LIST.Count + "\" >");

            foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.MSG_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine("      <MsgSymbol" + SYMBOL_ToSaveString(symbol) + " />");
                }
                else  file_stream.WriteLine("      <MsgSymbol Name=\"null\" />");
            }

            file_stream.WriteLine("   </MsgSymbols>");

//**********    SYMBOLS_NOMS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   SYMBOLS_NOMS   *******************  --> ");

            file_stream.WriteLine();        
            file_stream.WriteLine("\n   <NomSymbols" + " Count=\"" + SYMBOLS.NOMS_SYMBOLS_LIST.Count + "\" >");

            foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.NOMS_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine("      <NomSymbol" + SYMBOL_ToSaveString(symbol) + " />");
                }
                else  file_stream.WriteLine("      <NomSymbol Name=\"null\" />");
            }

            file_stream.WriteLine("   </NomSymbols>");

//**********    INDICATION SYMBOLS

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   INDICATION SYMBOLS   *******************  --> ");

            file_stream.WriteLine();        
            file_stream.WriteLine("\n   <IndicationSymbols" + " Count=\"" + SYMBOLS.INDICATION_SYMBOLS_LIST.Count + "\" >");

            foreach (SYMBOLS.Indication_Symbol_Data symbol in SYMBOLS.INDICATION_SYMBOLS_LIST)
            {
                if (symbol != null)
                {
                    file_stream.WriteLine(  "      <IndicationSymbol" +
                                            " Text=\"" + symbol.Text + "\"" +
                                            " Name1=\"" + symbol.Name1 + "\"" +
                                            " Name2=\"" + symbol.Name2 + "\"" +
                                            " Format1=\"" + symbol.Format1 + "\"" +
                                            " Format2=\"" + symbol.Format2 + "\"" +
                                            " />");
                }
                else  file_stream.WriteLine("      <IndicationSymbol Name=\"null\" />");
            }

            file_stream.WriteLine("   </IndicationSymbols>");


//**********    LOCAL SYMBOLS  -  вынесено из диаграмм сюда вверх, чтобы его загрузить раньше диаграмм 
//                                                                        для формирования значков диаграмм.

            file_stream.WriteLine();
            file_stream.WriteLine("<!--  ******************   LOCAL SYMBOLS   *******************  --> ");
            file_stream.WriteLine();

            file_stream.WriteLine("   <LocalSymbols" + " Count=\"" + DIAGRAMS_LIST.Count + "\" >");

            foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
            {

                file_stream.WriteLine();
                file_stream.WriteLine("\n   <Diagram_LocalSymbols" + " DiagramName=\"" + diagram.NAME + "\"" + 
                                                        " Count=\"" + diagram.LOCAL_SYMBOLS_LIST.Count + "\" >");

                foreach (SYMBOLS.Symbol_Data symbol in diagram.LOCAL_SYMBOLS_LIST)
                {
                    if (symbol != null)
                    {
                        file_stream.WriteLine("      <LocalSymbol" + SYMBOL_ToSaveString(symbol) + " />");
                    }
                    else file_stream.WriteLine("      <LocalSymbol Name=\"null\" />");
                }

                file_stream.WriteLine("   </Diagram_LocalSymbols>");
                file_stream.WriteLine();
            }

            file_stream.WriteLine("   </LocalSymbols>");
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }

    }



    //***************   SAVE DIAGRAMS

static public void SAVE_DIAGRAMS(StreamWriter file_stream)
    {

        try
        {
                file_stream.WriteLine();
                file_stream.WriteLine();
                file_stream.WriteLine("<!--  ******************  DIAGRAMS  *******************  --> ");
                file_stream.WriteLine();

                file_stream.WriteLine("   <Diagrams" + " Count=\"" + DIAGRAMS_LIST.Count + "\" >");

                foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
                {

//**********  DIAGRAM

                    file_stream.WriteLine();
                    file_stream.WriteLine("<!--  ***********  DIAGRAM: " + diagram.NAME + "***********  --> ");
                    file_stream.WriteLine();

                    file_stream.WriteLine("      <Diagram" + " Name=\"" + diagram.NAME + "\"" +
                                                             " Comment=\"" + diagram.COMMENT + "\"" +
                                                             " >");
                    file_stream.WriteLine();

//***********  NETWORKS

                    file_stream.WriteLine("         <Networks" + " Count=\"" + diagram.NETWORKS_LIST.Count + "\" >");

                    foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
                    {
                        file_stream.WriteLine("            <Network" + " Name=\"" + network.NAME + "\"" +
                                                                       " Comment=\"" + network.COMMENT + "\"" +
                                                                       //" Label=\"" + network.LABEL + "\"" +
                                                                       " Width=\"" + network.WIDTH + "\"" +
                                                                       " Height=\"" + network.HEIGHT + "\"" +
                                                                       " >");
                        //---   Подсчет в двухмерной копии нетворка ненулевых элементов.
                        int count = 0;
                        foreach (List<Element_Data> list in network.Network_panel_copy)
                        {
                            foreach (Element_Data element in list) { if (element != null) count++; }
                        }
                        file_stream.WriteLine("               <Elements" + " Count=\"" + count + "\" >");

                        //---   Распечатка элементов.
                        foreach (List<Element_Data> list in network.Network_panel_copy)
                        {
                            foreach (Element_Data element in list)
                            {
                                if (element != null)
                                {
                                    file_stream.WriteLine("                  <Element" +
                                                                        " Name=\"" + element.Name + "\"" +
                                                                        " Category=\"" + element.Category + "\"" +
                                                                        " X=\"" + element.Coordinats.X + "\"" +
                                                                        " Y=\"" + element.Coordinats.Y + "\"" +
                                                                        " >");

                                    file_stream.WriteLine("                     <Vars" + " Count=\"" + element.IO_VARs_list.Count + "\" >");

                                    foreach (SYMBOLS.Symbol_Data symbol in element.IO_VARs_list)
                                    {
                                        if (symbol != null)
                                        {
                                            string owner;
                                            if ( symbol.Owner == SYMBOLS.GLOBAL_SYMBOLS_LIST ) owner = "Global" ;
                                            else if (symbol.Owner == SYMBOLS.BASE_SYMBOLS_LIST) owner = "Base";
                                            else if (symbol.Owner == SYMBOLS.STORABLE_SYMBOLS_LIST) owner = "Storable";
                                            else if (symbol.Owner == SYMBOLS.MSG_SYMBOLS_LIST) owner = "Msg";
                                            else  owner = "Local";

                                            file_stream.WriteLine("                        <Var" +
                                                                            " Name=\"" + symbol.Name + "\"" +
                                                                            " Owner=\"" + owner + "\"" +
                                                                            " />");
                                        }
                                        else file_stream.WriteLine("                        <Var Name=\"null\" Owner=\"Global\" />");
                                    }
                                    file_stream.WriteLine("                     </Vars>");
                                    file_stream.WriteLine("                  </Element>");
                                }
                            }
                        }

                        file_stream.WriteLine("               </Elements>");
                        file_stream.WriteLine("            </Network>");
                    }
                    file_stream.WriteLine("         </Networks>");
                    file_stream.WriteLine("      </Diagram>");
                }

                file_stream.WriteLine("   </Diagrams>");

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }

    }

//*****************

static public void OPEN_PROJECT(string mode, RECENT_PROJECT_Data project_data = null)
{
        StreamReader file_stream = null;

        try
        {
            //ERRORS.Clear();

            if (mode == "Start")
            {
                ERRORS.Message("\nOpen last recent project...");
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("Save changes?", PROJECT_NAME, MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

                if (res == MessageBoxResult.Cancel) return;
                if (res == MessageBoxResult.Yes) ApplicationCommands.Save.Execute("Save", null); //SaveProject.SAVE_PROJECT("Save");


                //*************    OPEN FILE

                if (mode == "Open")
                {
                    ERRORS.Message("\nOpen from disk project...");

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
                    dlg.InitialDirectory = PROJECT_DIRECTORY_PATH;//.Remove(PROJECT_PATH.LastIndexOf('\\'));
                    //dlg.FileName = "Project_S5"; // Default file name
                    dlg.DefaultExt = ".ps5"; // Default file extension
                    dlg.Filter = "Project_S5 (.ps5)|*.ps5"; // Filter files by extension

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        PROJECT_DIRECTORY_PATH = dlg.FileName.Remove(dlg.FileName.LastIndexOf('\\') + 1);
                        //---  вырезаем имя файла без расширения.
                        PROJECT_NAME = dlg.FileName.Split('\\').Last().Split('.')[0];

                        PROJECT_FILE_PATH = dlg.FileName;
                    }
                }
                else if (mode == "Recent")
                {
                    ERRORS.Message("\nOpen recent project...");

                    if (!File.Exists(project_data.File_Path))
                    {
                        ERRORS.Add("Project file <" + project_data.File_Path + "> doesn't exist");
                        return;
                    }

                        PROJECT_DIRECTORY_PATH = project_data.Directory_Path;

                        PROJECT_NAME = project_data.Name;

                        PROJECT_FILE_PATH = project_data.File_Path;
                    
                }
            }

                Application.Current.MainWindow.Title = PROJECT_NAME;
                //---

                //ERRORS.Message("\n>Loading project...");
                ERRORS.Message("\n>Loading symbols...");
                //-----------

                file_stream = new StreamReader(PROJECT_FILE_PATH);//dlg.OpenFile());                                

                LOAD_SYMBOLS(file_stream);  //  должно идти перед SAVE DIAGRAMS для последующей загрузки,
                        //т.к. когда грузятся нетворки уже должны быть загружены переменные для их привязки.

                if (ERRORS.Count == 0)
                {
                    ERRORS.Message("\n>Loading diagrams...");
                    //  открываем заново, чтобы считать строки от начала файла для номеров ошибок.
                    file_stream.Close();
                    file_stream = new StreamReader(PROJECT_FILE_PATH);

                    LOAD_DIAGRAMS(file_stream);
                }
                if (ERRORS.Count != 0)
                {
                    ERRORS.Message("\nLoading errors: " + ERRORS.Count);
                    MessageBox.Show("Loading errors!");

                    SaveProject.NEW_PROJECT("Load template by default");
                }
                else
                {
                    ERRORS.Message("\nNo loading errors.");

                    SHOW_DIAGRAMS();
                }

                ADD_RECENT_PROJECT();
                            
            //----------

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
        finally
        {
            if (file_stream != null) file_stream.Close();
        }
    }





//***************  LOAD_SYMBOLS

static List<ObservableCollection<SYMBOLS.Symbol_Data>> LOCAL_SYMBOLS_LIST;
static List<ElementImage> ELEMENT_IMAGE_LIST;


static public string LOAD_ADD_SYMBOL(string str, string label, ObservableCollection<SYMBOLS.Symbol_Data> symbols_list)
{
    
    try
    {
        string error = null;

        string value, name, tag_name, data_type, memory_type, bit_base, comment, initial_value;
        int? address = null, bit_number = null;
        double? min_value = null, max_value = null, k_value = null, step_value = null;
        string nom_value = null, unit_value = null, format_value = null;

        int ax;
        double dax;
        
         /*  string str = " Name=\"" + name + "\"" +
                        " Tag_Name=\"" + symbol.Tag_Name + "\"" +
                        " Data_Type=\"" + symbol.Data_Type + "\"" +
                        " Memory_Type=\"" + symbol.Memory_Type + "\"" +
                        " Address=\"" + symbol.Address + "\"" +
                        " Bit_Base=\"" + symbol.Bit_Base + "\"" +
                        " Bit_Number=\"" + symbol.Bit_Number + "\"" +
                        " Initial_Value=\"" + symbol.Initial_Value + "\"" +
                        " str_Nom_Value=\"" + symbol.str_Nom_Value + "\"" +
                        " K_Value=\"" + symbol.K_Value + "\"" +
                        " Min_Value=\"" + symbol.Min_Value + "\"" +
                        " Max_Value=\"" + symbol.Max_Value + "\"" +
                        " Step_Value=\"" + symbol.Step_Value + "\"" +
                        " Unit_Value=\"" + symbol.Unit_Value + "\"" +
                        " Format_Value=\"" + symbol.Format_Value + "\"" +
                        " Comment=\"" + comment + "\"";
        */

                                value = _GetValue(label, " Name=\"", str);
                                if (value != null) { name = value; }
                                else { error = label + " Name" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Tag_Name=\"", str);
                                if (value != null) { tag_name = value; }
                                else { error = label + " Tag_Name" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Data_Type=\"", str);
                                if (value != null) { data_type = value; }
                                else { error = label + " Data_Type" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Memory_Type=\"", str);
                                if (value != null) { memory_type = value; }
                                else { error = label + " Memory_Type" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Address=\"", str);
                                if (value != null)
                                {
                                    if (value.Length == 0) address = null;
                                    else
                                    {
                                        if (int.TryParse(value, out ax)) { address = ax; }
                                        else { error = label + " Address" + ": No Value."; return error; }
                                    }
                                }
                                else { error = label + " Address" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Bit_Base=\"", str);
                                if (value != null) { bit_base = value; }
                                else { error = label + " Bit_Base" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Bit_Number=\"", str);
                                if (value != null)
                                {
                                    if (value.Length == 0) bit_number = null;
                                    else
                                    {
                                        if (int.TryParse(value, out ax)) { bit_number = ax; }
                                        else { error = label + " Bit_Number" + ": No Value."; return error; }
                                    }
                                }
                                else { error = label + " Bit_Number" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Initial_Value=\"", str);
                                if (value != null)  initial_value = value;
                                else { error = label + " Initial_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " str_Nom_Value=\"", str);
                                if (value != null) nom_value = value;
                                else { error = label + " Nom_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " K_Value=\"", str);
                                if (value != null)
                                {
                                    if (value.Length == 0) k_value = null;
                                    else
                                    {
                                        if (double.TryParse(value, out dax)) { k_value = dax; }
                                        else { error = label + " K_Value" + ": No Value."; return error; }
                                    }
                                }
                                else { error = label + " K_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Min_Value=\"", str);
                                if (value != null)
                                {
                                    if (value.Length == 0) min_value = null;
                                    else
                                    {
                                        if (double.TryParse(value, out dax)) { min_value = dax; }
                                        else { error = label + " Min_Value" + ": No Value."; return error; }
                                    }
                                }
                                else { error = label + " Min_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Max_Value=\"", str);
                                if (value != null)
                                {
                                    if (value.Length == 0) max_value = null;
                                    else
                                    {
                                        if (double.TryParse(value, out dax)) { max_value = dax; }
                                        else { error = label + " Max_Value" + ": No Value."; return error; }
                                    }
                                }
                                else { error = label + " Max_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Step_Value=\"", str);
                                if (value != null)
                                {
                                    if (value.Length == 0) step_value = null;
                                    else
                                    {
                                        if (double.TryParse(value, out dax)) { step_value = dax; }
                                        else { error = label + " Step_Value" + ": No Value."; return error; }
                                    }
                                }
                                else { error = label + " Step_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Unit_Value=\"", str);
                                if (value != null) unit_value = value;
                                else { error = label + " Unit_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Format_Value=\"", str);
                                if (value != null) format_value = value;
                                else { error = label + " Format_Value" + ": No Value."; return error; }
                                //---
                                value = _GetValue(label, " Comment=\"", str);
                                if (value != null) { comment = value; }
                                else { error = label + " Comment" + ": No Value."; return error; }


        symbols_list.Add( new SYMBOLS.Symbol_Data(symbols_list, name, tag_name, data_type, memory_type, address, bit_base, bit_number,
                                                  initial_value, comment, min_value, max_value, nom_value, k_value, step_value, 
                                                  unit_value, format_value));
        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return excp.ToString();
    }
}


static public void LOAD_SYMBOLS( StreamReader file_stream )
{

    try
    {
            string error = null;
            ObservableCollection<SYMBOLS.Symbol_Data> symbols_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> global_symbols_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> base_symbols_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> base_symbols_tags_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> storable_symbols_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> msg_symbols_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> nom_symbols_list = null;
            ObservableCollection<SYMBOLS.Symbol_Data> local_symbols_list = null;
            ObservableCollection<SYMBOLS.Indication_Symbol_Data> indication_symbols_list = new ObservableCollection<SYMBOLS.Indication_Symbol_Data>();

            LOCAL_SYMBOLS_LIST = new List<ObservableCollection<SYMBOLS.Symbol_Data>>();
            ELEMENT_IMAGE_LIST = new List<ElementImage>();

            int ax, symbols_count = 0;
            int diagrams_count = 0;
            string diagram_name = null;
        
        
            int line_count = 1; // для отображения при ошибке.
            string label;
            for (label = "<Version"; !file_stream.EndOfStream; line_count++)
            {
                string str = file_stream.ReadLine();
                string value;
                error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

                switch (label)
                {
                    case "<Version":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Compiler=\"", str);
                            if (value != null) { Version.Compiler = value; }
                            else { error = label + " Compiler" + ": No Value."; break; }

                            value = _GetValue(label, " FileFormat=\"", str);
                            if (value != null) { Version.FileFormat = value; }
                            else { error = label + " FileFormat" + ": No Value."; break; }
                            //---
                            label = "<Symbols";
                        }

                        break;

//***************  LOAD GLOBAL SYMBOLS

                    case "<Symbols":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }
                            //---

                            if (symbols_count != 0) label = "<Symbol";
                            else label = "</Symbols>";

                            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                        }

                        break;

                    case "<Symbol":

                        if (str.Contains(label))
                        {
                            if (symbols_list.Count < symbols_count)
                            {
                                //*******  LOAD & ADD SYMBOL if no errors
                                if ( (error = LOAD_ADD_SYMBOL( str, label, symbols_list)) != null ) break;

                                //---  прверяем после добавления не закончился ли список.
                                if (symbols_list.Count == symbols_count) label = "</Symbols>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<Symbol";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</Symbols>")) { error = "</Symbols>" + ": Too few symbols."; break; }

                        break;

                    case "</Symbols>":

                        if (str.Contains(label))
                        {
                            ////SYMBOLS.SYMBOLS_LIST = symbols_list;
                            //SYMBOLS_LIST = new SYMBOLS (symbols_list);
                            //  сохраняем временно список пока не загрузим остальные списки и затем сформируем уже весь класс.
                            global_symbols_list = symbols_list;
                            //---
                            label = "<BaseSymbols";
                        }

                        break;

//***********   BASE SYMBOLS

                    case "<BaseSymbols":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }
                            //---

                            if (symbols_count != 0) label = "<BaseSymbol";
                            else label = "</BaseSymbols>";

                            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                        }

                        break;

                    case "<BaseSymbol":

                        if (str.Contains(label))
                        {
                            if (symbols_list.Count < symbols_count)
                            {
                                //*******  LOAD & ADD SYMBOL if no errors
                                if ((error = LOAD_ADD_SYMBOL(str, label, symbols_list)) != null) break;

                                //---  проверяем после добавления не закончился ли список.
                                if (symbols_list.Count == symbols_count) label = "</BaseSymbols>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<BaseSymbol";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</BaseSymbols>")) { error = "</BaseSymbols>" + ": Too few symbols."; break; }

                        break;

                    case "</BaseSymbols>":

                        if (str.Contains(label))
                        {
                            base_symbols_list = symbols_list;
                            //---
                            label = "<BaseSymbolsTags";
                        }

                        break;

//***********   BASE SYMBOLS TAGS

                    case "<BaseSymbolsTags":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }

                            //---

                            if (symbols_count != 0) label = "<BaseSymbolTag";
                            else label = "</BaseSymbolsTags>";

                            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                        }

                        break;

                    case "<BaseSymbolTag":

                        if (str.Contains(label))
                        {
                            if (symbols_list.Count < symbols_count)
                            {
                                //*******  LOAD & ADD SYMBOL if no errors
                                if ((error = LOAD_ADD_SYMBOL(str, label, symbols_list)) != null) break;

                                //---  проверяем после добавления не закончился ли список.
                                if (symbols_list.Count == symbols_count) label = "</BaseSymbolsTags>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<BaseSymbolTag";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</BaseSymbolsTags>")) { error = "</BaseSymbolsTags>" + ": Too few symbols."; break; }

                        break;

                    case "</BaseSymbolsTags>":

                        if (str.Contains(label))
                        {
                            base_symbols_tags_list = symbols_list;
                            //---
                            label = "<StorableSymbols";
                        }

                        break;


//***********   STORABLE SYMBOLS

                    case "<StorableSymbols":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }

                            //---

                            if (symbols_count != 0) label = "<StorableSymbol";
                            else label = "</StorableSymbols>";

                            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                        }

                        break;

                    case "<StorableSymbol":

                        if (str.Contains(label))
                        {
                            if (symbols_list.Count < symbols_count)
                            {
                                //*******  LOAD & ADD SYMBOL if no errors
                                if ((error = LOAD_ADD_SYMBOL(str, label, symbols_list)) != null) break;

                                //---  проверяем после добавления не закончился ли список.
                                if (symbols_list.Count == symbols_count) label = "</StorableSymbols>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<StorableSymbol";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</StorableSymbols>")) { error = "</StorableSymbols>" + ": Too few symbols."; break; }

                        break;

                    case "</StorableSymbols>":

                        if (str.Contains(label))
                        {
                            storable_symbols_list = symbols_list;
                            //---
                            label = "<MsgSymbols";
                        }

                        break;

//***********   MSG_SYMBOLS

                    case "<MsgSymbols":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }

                            //---

                            if (symbols_count != 0) label = "<MsgSymbol";
                            else label = "</MsgSymbols>";

                            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                        }

                        break;

                    case "<MsgSymbol":

                        if (str.Contains(label))
                        {
                            if (symbols_list.Count < symbols_count)
                            {
                                //*******  LOAD & ADD SYMBOL if no errors
                                if ((error = LOAD_ADD_SYMBOL(str, label, symbols_list)) != null) break;

                                //---  проверяем после добавления не закончился ли список.
                                if (symbols_list.Count == symbols_count) label = "</MsgSymbols>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<MsgSymbol";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</MsgSymbols>")) { error = "</MsgSymbols>" + ": Too few symbols."; break; }

                        break;

                    case "</MsgSymbols>":

                        if (str.Contains(label))
                        {
                            msg_symbols_list = symbols_list;
                            //---
                            label = "<NomSymbols";
                        }

                        break;

//***********   NOM_SYMBOLS

                    case "<NomSymbols":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }

                            //---

                            if (symbols_count != 0) label = "<NomSymbol";
                            else label = "</NomSymbols>";

                            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                        }

                        break;

                    case "<NomSymbol":

                        if (str.Contains(label))
                        {
                            if (symbols_list.Count < symbols_count)
                            {
                                //*******  LOAD & ADD SYMBOL if no errors
                                if ((error = LOAD_ADD_SYMBOL(str, label, symbols_list)) != null) break;

                                //---  проверяем после добавления не закончился ли список.
                                if (symbols_list.Count == symbols_count) label = "</NomSymbols>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<NomSymbol";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</NomSymbols>")) { error = "</NomSymbols>" + ": Too few symbols."; break; }

                        break;

                    case "</NomSymbols>":

                        if (str.Contains(label))
                        {
                            nom_symbols_list = symbols_list;
                            //---
                            label = "<IndicationSymbols";
                        }

                        break;

//***********   INDICATION_SYMBOLS

                    case "<IndicationSymbols":

                        if (str.Contains(label))
                        {
                            value = _GetValue(label, " Count=\"", str);
                            if (value != null)
                            {
                                if (int.TryParse(value, out symbols_count)) {}
                                else { error = label + " Count" + ": No Value."; break; }
                            }
                            else { error = label + " Count" + ": No Value."; break; }

                            //---

                            if (symbols_count != 0) label = "<IndicationSymbol";
                            else label = "</IndicationSymbols>";

                            indication_symbols_list = new ObservableCollection<SYMBOLS.Indication_Symbol_Data>();
                        }

                        break;

                    case "<IndicationSymbol":

                        if (str.Contains(label))
                        {
                            if (indication_symbols_list.Count < symbols_count)
                            {
                                string text, name1, name2, format1, format2;

                                value = _GetValue(label, " Text=\"", str);
                                if (value != null) text = value;
                                else { error = label + " Text" + ": No Value."; break; }
                                //---
                                value = _GetValue(label, " Name1=\"", str);
                                if (value != null) name1 = value;
                                else { error = label + " Name1" + ": No Value."; break; }
                                //---
                                value = _GetValue(label, " Name2=\"", str);
                                if (value != null) name2 = value;
                                else { error = label + " Name2" + ": No Value."; break; }
                                //---
                                value = _GetValue(label, " Format1=\"", str);
                                if (value != null) format1 = value;
                                else { error = label + " Format1" + ": No Value."; break; }
                                //---
                                value = _GetValue(label, " Format2=\"", str);
                                if (value != null) format2 = value;
                                else { error = label + " Format2" + ": No Value."; break; }
                                //---

                                indication_symbols_list.Add(new SYMBOLS.Indication_Symbol_Data(indication_symbols_list, text, 
                                                                    name1, name2, format1, format2));

                                //---  проверяем после добавления не закончился ли список.
                                if (indication_symbols_list.Count == symbols_count) label = "</IndicationSymbols>";
                                // возвращаемся к заполнению следующего элемента.
                                else label = "<IndicationSymbol";  

                            }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                            //else { error = label + ": Too many symbols."; break; }
                        }
                        else    //  раньше времени встретили конец списка. 
                            if (str.Contains("</IndicationSymbols>")) { error = "</IndicationSymbols>" + ": Too few symbols."; break; }

                        break;

                    case "</IndicationSymbols>":

                        if (str.Contains(label))
                        {
                            SYMBOLS_LIST = new SYMBOLS(global_symbols_list, base_symbols_list, base_symbols_tags_list, storable_symbols_list, 
                                                        msg_symbols_list, nom_symbols_list, indication_symbols_list);
                            //---
                            label = "<LocalSymbols";
                        }

                        break;


//***********   LOCAL SYMBOLS

                        case "<LocalSymbols":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Count=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) diagrams_count = ax;
                                    else { error = label + " Count" + ": No Value."; break; }
                                }
                                else { error = label + " Count" + ": No Value."; break; }

                                ELEMENTS_DICTIONARY.RemoveUserFunctions();

                                //---
                                if (diagrams_count != 0) label = "<Diagram_LocalSymbols";
                                else label = "</LocalSymbols>";
                            }
                            break;
                        
                        case "<Diagram_LocalSymbols": 

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " DiagramName=\"", str);
                                if (value != null) { diagram_name = value; }
                                else { error = label + " Name" + ": No Value."; break; }

                                value = _GetValue(label, " Count=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out symbols_count)) {}
                                    else { error = label + " Count" + ": No Value."; break; }
                                }
                                else { error = label + " Count" + ": No Value."; break; }

                                //---

                                if (symbols_count != 0) label = "<LocalSymbol";
                                else label = "</Diagram_LocalSymbols>";

                                local_symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();
                            }

                            break;

                        case "<LocalSymbol":

                            if (str.Contains(label))
                            {
                                if (local_symbols_list.Count < symbols_count)
                                {
                                    //*******  LOAD & ADD SYMBOL if no errors
                                    if ((error = LOAD_ADD_SYMBOL(str, label, local_symbols_list)) != null) break;

                                    //---  проверяем после добавления не закончился ли список.
                                    if (local_symbols_list.Count == symbols_count) label = "</Diagram_LocalSymbols>";
                                    // возвращаемся к заполнению следующего элемента.
                                    else label = "<LocalSymbol";  

                                }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                                //else { error = label + ": Too many symbols."; break; }
                            }
                            else    //  раньше времени встретили конец списка. 
                                if (str.Contains("</Diagram_LocalSymbols>")) { error = "</Diagram_LocalSymbols>" + ": Too few local symbols."; break; }

                            break;

                        case "</Diagram_LocalSymbols>":

                            if (str.Contains(label))
                            {
                                if (LOCAL_SYMBOLS_LIST.Count < diagrams_count)
                                {
                                    ELEMENT_IMAGE_LIST.Add( ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage(null, diagram_name, local_symbols_list) );
                                    //---
                                    LOCAL_SYMBOLS_LIST.Add(local_symbols_list);
                                    //---  прверяем после добавления не закончился ли список.
                                    if (LOCAL_SYMBOLS_LIST.Count == diagrams_count) label = "</LocalSymbols>";
                                    // возвращаемся к заполнению следующего элемента.
                                    else label = "<Diagram_LocalSymbols";
                                }
                            }
                            else    //  раньше времени встретили конец списка. 
                                if (str.Contains("</Local_Symbols>")) { error = "</LocalSymbols>" + ": Too few diagram local symbols."; break; }

                            break;

                        case "</LocalSymbols>":

                            if (str.Contains(label))
                            {
                                return;
                            }

                            break;

                        //**************  end of switch
                }

                if (error != null) ERRORS.Add(error + "   Line: " + line_count);
            }

        // До конца файла дойти не должны.
        // если цикл закончился - значит дошли до конца файла - значит чегото не нашли.
        ERRORS.Add("End of file. \"" + label + "\"  not found.");

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    finally
    {
        if (file_stream != null) file_stream.Close();
    }

}


//***************  LOAD_DIAGRAMS

static public void LOAD_DIAGRAMS(StreamReader file_stream)
{
        string error = null;
       
        try
        {
                //--- Diagrams
                List<Diagram_Of_Networks> diagrams_list = null;
                Diagram_Of_Networks diagram = null;
                string diagram_name = null;
                string diagram_comment = null;

                //--- Local Symbols
                //ObservableCollection<SYMBOLS.Symbol_Data> local_symbols_list = null;

                //--- Network
                ObservableCollection<Network_Of_Elements> networks_list = null;
                Network_Of_Elements network = null;
                string network_name = null, network_comment = null;//, network_label = null;
                int ax = 0, network_width = 0, network_height = 0;

                //---  Element
                List<Element_Data> elements_list = null;
                Element_Data element = null;
                ElementCoordinats element_coordinats = null;
                string element_name = null, element_category = null;
                int element_x=0, element_y=0;

                //--- Vars
                List<List<string>> vars_list = null;
                //List<string> local_vars_list = null;

                //---  Counts
                int diagrams_count = 0, networks_count = 0, elements_count = 0, vars_count = 0;


                int line_count = 1; // для отображения при ошибке.
                string label;
                for (label = "<Diagrams"; !file_stream.EndOfStream; line_count++)
                {
                    string str = file_stream.ReadLine();
                    string value;
                    error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

                    switch( label )
                    {

                        case "<Diagrams":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Count=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) diagrams_count = ax;
                                    else { error = label + " Count" + ": No Value."; break; }
                                }
                                else { error = label + " Count" + ": No Value."; break; }

                                //---
                                if (diagrams_count != 0) label = "<Diagram";
                                else label = "</Diagrams>";

                                diagrams_list = new List<Diagram_Of_Networks>();
                            }
                            break;
                        
                        case "<Diagram": 

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Name=\"", str);
                                if (value != null) { diagram_name = value; }
                                else { error = label + " Name" + ": No Value."; break; }

                                value = _GetValue(label, " Comment=\"", str);
                                if (value != null) { diagram_comment = value; }
                                else { error = label + " Comment" + ": No Value."; break; }

                                label = "<Networks";
                            }

                            break;


//**************  NETWORKS

                        case "<Networks":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Count=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) networks_count = ax;
                                    else { error = label + " Count" + ": No Value."; break; }
                                }
                                else { error = label + " Count" + ": No Value."; break; }

                                //---
                                if (networks_count != 0) label = "<Network";
                                else label = "</Networks>";
                                
                                networks_list = new ObservableCollection<Network_Of_Elements>();
                            }
                            break;

                        case "<Network":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Name=\"", str);
                                if (value != null) { network_name = value; }
                                else { error = label + " Name" + ": No Value."; break; }

                                value = _GetValue(label, " Comment=\"", str);
                                if (value != null) { network_comment = value; }
                                else { error = label + " Comment" + ": No Value."; break; }

                                //value = GetValue(label, "Label", str);
                                //if (value != null) { network_label = value; }
                                //else { error = label + " Label" + ": No Value."; break; }

                                value = _GetValue(label, " Width=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) network_width = ax;
                                    else { error = label + " Width" + ": No Value."; break; }
                                }
                                else { error = label + " Width" + ": No Value."; break; }

                                value = _GetValue(label, " Height=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) network_height = ax;
                                    else { error = label + " Height" + ": No Value."; break; }
                                }
                                else { error = label + " Height" + ": No Value."; break; }

                                //---
                                label = "<Elements";
                            }
                            break;

                        case "<Elements":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Count=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) elements_count = ax;
                                    else { error = label + " Count" + ": No Value."; break; }
                                }
                                else { error = label + " Count" + ": No Value."; break; }

                                //---
                                if ( elements_count != 0 ) label = "<Element";
                                else label = "</Elements>";
                            
                                elements_list = new List<Element_Data>();
                            }

                            break;

                        case "<Element":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Name=\"", str);
                                if (value != null) { element_name = value; }
                                else { error = label + " Name" + ": No Value."; break; }

                                value = _GetValue(label, " Category=\"", str);
                                if (value != null) { element_category = value; }
                                else { error = label + " Category" + ": No Value."; break; }

                                value = _GetValue(label, " X=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) element_x = ax;
                                    else { error = label + " X" + ": No Value."; break; }
                                }
                                else { error = label + " X" + ": No Value."; break; }

                                value = _GetValue(label, " Y=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) element_y = ax;
                                    else { error = label + " Y" + ": No Value."; break; }
                                }
                                else { error = label + " Y" + ": No Value."; break; }

                                //---
                                label = "<Vars";
                            }
                            break;

                        case "<Vars":

                            if (str.Contains(label))
                            {
                                value = _GetValue(label, " Count=\"", str);
                                if (value != null)
                                {
                                    if (int.TryParse(value, out ax)) vars_count = ax;
                                    else { error = label + " Count" + ": No Value."; break; }
                                }
                                else { error = label + " Count" + ": No Value."; break; }

                                //---
                                if (vars_count != 0) label = "<Var";
                                else label = "</Vars>";
                                                            
                                vars_list = new List<List<string>>();
                            }

                            break;

                        case "<Var":

                            if (str.Contains(label))
                            {
                                if (vars_list.Count < vars_count)
                                {
                                    List<string> name_buff = new List<string>() { null, null };

                                    value = _GetValue(label, " Name=\"", str);
                                    //if (value != null) { vars_list.Add(value); } 
                                    if (value != null) { name_buff[0] = value; }
                                    else { error = label + " Name" + ": No Value."; break; }

                                    value = _GetValue(label, " Owner=\"", str);
                                    if (value != null) 
                                    {
                                        if (value == "Global" || value == "Base" ||
                                            value == "Storable" || value == "Msg" || value == "Local") { name_buff[1] = value; }
                                        else { error = label + " Unknown Owner."; break; }
                                    }
                                    else { error = label + " Owner" + ": No Value."; break; }

                                    vars_list.Add(name_buff);

                                    //---  проверяем после добавления не закончился ли список.
                                    if (vars_list.Count == vars_count) label = "</Vars>";
                                    // возвращаемся к заполнению следующего элемента.
                                    else label = "<Var";  
                                }
                            }    
                            else    //  раньше времени встретили конец списка. 
                                if (str.Contains("</Vars>")) { error = "</Vars>" + ": Too few vars."; break; }

                            break;

                        case "</Vars>":

                            if (str.Contains(label))
                            {
                                label = "</Element>";
                            }

                            break;

                        case "</Element>":

                            if (str.Contains(label))
                            {
                                if (elements_list.Count < elements_count)
                                {
                                    element_coordinats = new ElementCoordinats(null, element_x, element_y);

                                    element = new Element_Data( element_category, element_name, 
                                                                element_coordinats, vars_list, 
                                                                LOCAL_SYMBOLS_LIST[diagrams_list.Count]);
                                    elements_list.Add(element);
                                    //---  прверяем после добавления не закончился ли список.
                                    if (elements_list.Count == elements_count) label = "</Elements>";
                                    // возвращаемся к заполнению следующего элемента.
                                    else label = "<Element";  
                                }
                            }
                            else    //  раньше времени встретили конец списка. 
                                if (str.Contains("</Elements>")) { error = "</Elements>" + ": Too few elements."; break; }

                            break;

                        case "</Elements>":

                            if (str.Contains(label))
                            {
                                label = "</Network>";
                            }

                            break;

                        case "</Network>":

                            if (str.Contains(label))
                            {
                                if (networks_list.Count < networks_count)
                                {
                                    network = new Network_Of_Elements(null, network_name, network_comment,/* network_label,*/ network_height, network_width, elements_list );
                                    //---
                                    networks_list.Add(network);
                                    //---  прверяем после добавления не закончился ли список.
                                    if (networks_list.Count == networks_count) label = "</Networks>";
                                    // возвращаемся к заполнению следующего элемента.
                                    else label = "<Network";  
                                }
                            }
                            else    //  раньше времени встретили конец списка. 
                                if (str.Contains("</Networks>")) { error = "</Networks>" + ": Too few networks."; break; }

                            break;

                        case "</Networks>":

                            if (str.Contains(label))
                            {
                                label = "</Diagram>";
                            }

                            break;

                        case "</Diagram>":

                            if (str.Contains(label))
                            {
                                if (diagrams_list.Count < diagrams_count)
                                {
//в конструкторе диаграммы убрать повторное формирование значка при загрузке из файла.
                                    diagram = new Diagram_Of_Networks(diagram_name, diagram_comment, networks_list, 
                                                                        LOCAL_SYMBOLS_LIST[diagrams_list.Count], 
                                                                        ELEMENT_IMAGE_LIST[diagrams_list.Count]);
                                    diagrams_list.Add(diagram);

                                    //---  прверяем после добавления не закончился ли список.
                                    if (diagrams_list.Count == diagrams_count) label = "</Diagrams>";
                                    // возвращаемся к заполнению следующего элемента.
                                    else label = "<Diagram";  
                                }
                            }
                            else    //  раньше времени встретили конец списка. 
                                if (str.Contains("</Diagrams>")) { error = "</Diagrams>" + ": Too few diagrams."; break; }

                            break;

                        case "</Diagrams>":

                            if (str.Contains(label))
                            {
                                DIAGRAMS_LIST = diagrams_list;
                                return;
                            }

                            break;
                    }

                    if ( error != null) ERRORS.Add("Error: " + error + "   Line: " + line_count) ;
                }

                // До конца файла дойти не должны.
                // если цикл закончился - значит дошли до конца файла - значит чегото не нашли.
                ERRORS.Add("Error: End of file. \"" + label + "\"  not found.");
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
        finally
        {
            if (file_stream != null) file_stream.Close();
        }
 
    }

//**************   GET VALUE of parameter from string

static public string _GetValue ( string key, string name, string str )
    {
        try
        {
            int start_index = str.IndexOf(key) + key.Length;

            if (str.Contains(name))
            {
                start_index = str.IndexOf(name, start_index) + name.Length;

                for (int i = start_index; i < str.Length; i++)
                {
                    // поиск замыкающей кавычки.
                    if (str[i] == '"') return str.Substring(start_index, i - start_index);
                }
            }
            return null;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

//static public string GetValue(string key, string name, string str)
//{
//    try
//    {
//        int start_index = str.IndexOf(key) + key.Length;

//        if (str.Contains(name))
//        {
//            start_index = str.IndexOf(name, start_index) + name.Length;

//            for (int i = start_index, tst = 0; i < str.Length; i++)
//            {
//                if (tst == 0)
//                {
//                    switch (str[i])
//                    {
//                        case '"': start_index = i + 1; tst++; // поиск первой кавычки.
//                            break;
//                        case ' ': break;
//                        case '=': break;

//                        default: return null; // ошибочный символ.
//                    }
//                }
//                else// поиск замыкающей кавычки.
//                {
//                    if (str[i] == '"') return str.Substring(start_index, i - start_index);
//                }
//            }
//        }
//        return null;
//    }
//    catch (Exception excp)
//    {
//        MessageBox.Show(excp.ToString());
//        return null;
//    }
//}

    }   //*************    END of Class <SaveProject>


//*********************************************

//***********************    HANDLERS   **************************************
                    

    }  //*************    END of Class <Step5>
}

