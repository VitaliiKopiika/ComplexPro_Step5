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

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.IO;

namespace ComplexPro_Step5
{
    public partial class Step5
    {

        static public class Version
        {
            public static string Compiler   = "1.0";
            public static string FileFormat = "1.0";
        }


        //***************  PROJECT DIRECTORIES

        public class RECENT_PROJECT_Data
        {
            public string Name; 
            public string Directory_Path;
            public string SaveDate;
            public string File_Path
            {
                get
                {
                    return Directory_Path + Name + ".ps5";
                }
            }

            public RECENT_PROJECT_Data(string name, string directory, string save_date)
            {
                Name = name; 
                Directory_Path = directory;
                SaveDate = save_date;
            }

            //---  сделал стринг чтобы делать список в меню, т.к. в байндинге меню 
            //--- почемуто не работал конвертер
            public override string ToString()
            {
                return Name + "  [" + SaveDate + ", " + Directory_Path + "]";
            }
        }

        //---   Сделал коллекцию внутри класса, чтобы в байндинге в меню было что задать в Source и в Path.
        public class Recent_Projects
        {
            public ObservableCollection<RECENT_PROJECT_Data> List { get; set; }

            public Recent_Projects()
            {
                List = new ObservableCollection<RECENT_PROJECT_Data>();
            }
        }

        static public Recent_Projects RECENT_PROJECTS ; 

//--------------- DIRECTORIES

        static public string APPLICATION_DIRECTORY_PATH = null; // чтобы вызвать диалоговое окно выбора пути.

        static public string PCC_INI_FILE_NAME = null;
        static public string PCC_INI_FILE_PATH = null;

        static public string CPU_LIB_C_FILE_NAME = null;
        static public string CPU_LIB_H_FILE_NAME = null;

        static public string APPLICATION_INI_FILE_PATH = null;
        static public string APPLICATION_LOG_FILE_PATH = null;

        static public string NEW_PROJECT_TEMPLATE_FILE_PATH = null;

        static public string PROJECT_NAME { get; set; }
        static public string PROJECT_DIRECTORY_PATH = null; // чтобы вызвать диалоговое окно выбора пути.
        static public string PROJECT_FILE_PATH = null;

        static public string PROJECT_MAP_DIRECTORY_PATH { get { return PROJECT_DIRECTORY_PATH + "Map\\"; } }
        static public string PROJECT_MAP_FILE_PATH { get { return PROJECT_DIRECTORY_PATH + "Map\\" + PROJECT_NAME + "_prj.map"; } }
        static public string PROJECT_MAP_FILE_EXT = ".map";
        
        static public string STANDART_PROJECTS_DIRECTORY_PATH = null;
        
        static public string INTERPRETER_DIRECTORY_PATH = null;
        static public string INTERPRETER_EXE_FILE_NAME = "Pcc.exe";
        static public string INTERPRETER_INI_FILE_NAME = "Pcc.ini";

        static public string PROGRAMMER_DIRECTORY_PATH = null;
        static public string PROGRAMMER_EXE_FILE_NAME = "PCompProg.exe";

//---------------

        static public SYMBOLS SYMBOLS_LIST;

        static double xFontSize = 12;

        static int    xSetPoint_TagName_PrintSize = 12;

        static bool ShowNetworkGridLines = true;
        static public Visibility NetworkCellsVisibility {get; set;} 

        static public Menu MENU_PANEL;
        static public StackPanel DIAGRAMS_PANEL;//, TREE_PANEL, ERRORS_PANEL;
        static public TabControl TOOLS_TAB_CONTROL_PANEL;

 
public Step5( StackPanel Diagrams_Panel, TabControl TabControl_Panel, Menu menu)
{
    try
    {
        //**********
        DIAGRAMS_PANEL = Diagrams_Panel;

        TOOLS_TAB_CONTROL_PANEL = TabControl_Panel;

        MENU_PANEL = menu;

        //---  надо ставить раньше MainMenu
        RECENT_PROJECTS = new Recent_Projects();

        //---
        NetworkCellsVisibility = Visibility.Hidden;
        //---

        MainMenu step5_menu = new MainMenu();

        Step5.DictionaryTree.SHOW_TREE(TOOLS_TAB_CONTROL_PANEL);

        //***************   DEBUG

        DEBUG = new Debug();

//*************

        Step5.ERRORS.Show_Window(TOOLS_TAB_CONTROL_PANEL);
        
        ERRORS.Message("\nCOMPLEX_PRO_STEP5");

        ERRORS.Message("\nStart application");
        ERRORS.Message("  [ " + DiagramCompiler.GetDateString() + " ]");

//*************     DIRECTORIES

        //---  APPLICATION

        APPLICATION_DIRECTORY_PATH = AppDomain.CurrentDomain.BaseDirectory; //Application.Current.Properties[Application. 

        PCC_INI_FILE_NAME = "Pcc.ini";  PCC_INI_FILE_PATH = APPLICATION_DIRECTORY_PATH + PCC_INI_FILE_NAME;

        CPU_LIB_C_FILE_NAME = "Step5_CPU_lib.c"; CPU_LIB_H_FILE_NAME = "Step5_CPU_lib.h"; 
        
        APPLICATION_INI_FILE_PATH = APPLICATION_DIRECTORY_PATH + "PComplex_Step5.ini";

        APPLICATION_LOG_FILE_PATH = APPLICATION_DIRECTORY_PATH + "PComplex_Step5.log";

        INTERPRETER_DIRECTORY_PATH = APPLICATION_DIRECTORY_PATH;// +"\\" + "Bin"; ;

        PROGRAMMER_DIRECTORY_PATH = APPLICATION_DIRECTORY_PATH;// +"\\" + "Bin"; ;

        STANDART_PROJECTS_DIRECTORY_PATH = APPLICATION_DIRECTORY_PATH + "Projects\\";

        NEW_PROJECT_TEMPLATE_FILE_PATH = APPLICATION_DIRECTORY_PATH + "New_Project.ns5";

        //---   LOAD LAST RECENT PROJECT PATH

        if (LOAD_APPLICATION_INI_FILE(APPLICATION_INI_FILE_PATH) == null)
        {
            PROJECT_DIRECTORY_PATH = RECENT_PROJECTS.List[0].Directory_Path;
            PROJECT_NAME = RECENT_PROJECTS.List[0].Name;
            PROJECT_FILE_PATH = PROJECT_DIRECTORY_PATH + PROJECT_NAME + ".ps5"; 
        }
        else
        {
            PROJECT_NAME = null;
            PROJECT_DIRECTORY_PATH = null;
            PROJECT_FILE_PATH = null;
        }

//---  CHECK EXISTING LAST RECENT PROJECT

        if (PROJECT_DIRECTORY_PATH == null || !Directory.Exists(PROJECT_DIRECTORY_PATH) ||
                                              !File.Exists(PROJECT_FILE_PATH))
        {
            //  заход по ветке несуществующего ini-файла?
            if (PROJECT_FILE_PATH != null)
            {
                if (!Directory.Exists(PROJECT_DIRECTORY_PATH)) ERRORS.Message("\nLast recent project directory <" + PROJECT_DIRECTORY_PATH + "> doesn't exists");
                else if (!File.Exists(PROJECT_FILE_PATH)) ERRORS.Message("\nLast recent project file <" + PROJECT_FILE_PATH + "> doesn't exists");
            }
            //---  Create STANDART_PROJECTS_DIRECTORY

            SaveProject.NEW_PROJECT("Load template by default");
        }
        else
        {
            SaveProject.OPEN_PROJECT("Start");
        }

        //***************   WINDOWs

        //   автоматически закрывает все открытые окна приложения.
        //Application.Current.ShutdownMode =ShutdownMode.OnMainWindowClose ;

        //  закрываю вручную окна приложения и сохраняю данные.                
        Application.Current.MainWindow.Closing += MainWindow_Closing;
        Application.Current.MainWindow.Closed += MainWindow_Closed;
        Application.Current.Exit += Application_Exit;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    finally
    {
    }

}

//***************   Create STANDART_PROJECTS_DIRECTORY

static public void GENERATE_STANDART_PROJECTS_FILE_NAME()
{

    try
    {

            if (!Directory.Exists(STANDART_PROJECTS_DIRECTORY_PATH)) Directory.CreateDirectory(STANDART_PROJECTS_DIRECTORY_PATH);


            //---  Подборка имени нового каталого проекта.
            for (int i = 1; i < 100; i++)
            {
                PROJECT_NAME = "New_Project" + i.ToString();

                PROJECT_DIRECTORY_PATH = STANDART_PROJECTS_DIRECTORY_PATH + PROJECT_NAME + "\\";

                if (!Directory.Exists(PROJECT_DIRECTORY_PATH)) break;
            }

            Directory.CreateDirectory(PROJECT_DIRECTORY_PATH);

            PROJECT_FILE_PATH = PROJECT_DIRECTORY_PATH + PROJECT_NAME + ".ps5";            

        //**********
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    finally
    {
    }

}

//***************   SAVE_APPLICATION

static public void SAVE_APPLICATION_INI_FILE()
{

    StreamWriter file_stream = null;

    try
    {

        //*************    CREATE FILE

        if (APPLICATION_INI_FILE_PATH == null) return;

        file_stream = new StreamWriter(APPLICATION_INI_FILE_PATH);
        file_stream.AutoFlush = true;

        //********* 

        file_stream.WriteLine();
        file_stream.WriteLine("<Version" + " Compiler=\"" + Version.Compiler + "\"" +
                                           " FileFormat=\"" + Version.FileFormat + "\" />");

        //**********    RECENT PROJECTS

        ADD_RECENT_PROJECT();
             

        file_stream.WriteLine();
        file_stream.WriteLine("<!--  ******************   RECENT PROJECTS   *******************  --> ");

        file_stream.WriteLine();
        file_stream.WriteLine("\n   <RecentProjects" + " Count=\"" + RECENT_PROJECTS.List.Count + "\" >");

        foreach (RECENT_PROJECT_Data project in RECENT_PROJECTS.List)
        {
            if (project != null)
            {
                file_stream.WriteLine("      <Project" +    " Name=\"" + project.Name + "\"" +
                                                            " Directory=\"" + project.Directory_Path + "\"" +
                                                            " SaveDate=\"" + project.SaveDate + "\"" +
                                      " />");
            }
            else file_stream.WriteLine("      <Project Name=\"null\" />");
        }

        file_stream.WriteLine("   </RecentProjects>");


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

//**********    RECENT PROJECTS

static public void ADD_RECENT_PROJECT()
{

    try
    {
        //if (RECENT_PROJECTS_List == null) RECENT_PROJECTS_List = new ObservableCollection<RECENT_PROJECT_Data>();

        //---  сохранеие текущего проекта: или добавление если его не было или перенос в начало списка.

        if (RECENT_PROJECTS.List.Any(value => value.Name == PROJECT_NAME))
        {
            RECENT_PROJECT_Data recent_project = RECENT_PROJECTS.List.First(value => value.Name == PROJECT_NAME);
            //--- удаление прежней записи если она есть
            if (recent_project != null) RECENT_PROJECTS.List.Remove(recent_project);
        }

        //--- удаление последней записи если размер более 5
        if (RECENT_PROJECTS.List.Count >= 5) RECENT_PROJECTS.List.Remove(RECENT_PROJECTS.List.Last());

            //--- занесение в начало новой записи
        RECENT_PROJECTS.List.Insert(0, new RECENT_PROJECT_Data( PROJECT_NAME, 
                                                                PROJECT_DIRECTORY_PATH, 
                                                                DiagramCompiler.GetDateString()));
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

//***************   LOAD_APPLICATION

static public string LOAD_APPLICATION_INI_FILE(string ini_file_path)
{
    string error = null;
    StreamReader file_stream = null;

    try
    {
        ERRORS.Message("\nLoad application ini-file");
        
        //*************    OPEN FILE

        if (ini_file_path == null || !File.Exists(ini_file_path))
        {
            error = "Application ini-file <" + ini_file_path + "> doesn't exists";

            ERRORS.Add(error);

            return error;
        }

        file_stream = new StreamReader(ini_file_path);                                

        //********* 

        ObservableCollection<RECENT_PROJECT_Data> recent_projects_list = null;

        int line_count = 1; // для отображения при ошибке.
        string label;
        int recent_projects_count = 0;
        string directory = null, name = null, save_date = null;

        for (label = "<Version"; !file_stream.EndOfStream; line_count++)
        {
            string str = file_stream.ReadLine();
            string value;
            error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

            switch (label)
            {
                case "<Version":

                    //if (str.Contains(label))
                    //{
                        //value = SaveProject.GetValue(label, "Compiler", str);
                        //if (value != null) { Version.Compiler = value; }
                        //else { error = label + " Compiler" + ": No Value."; break; }

                        //value = SaveProject.GetValue(label, "FileFormat", str);
                        //if (value != null) { Version.FileFormat = value; }
                        //else { error = label + " FileFormat" + ": No Value."; break; }
                        //---
                        label = "<RecentProjects";
                    //}

                    break;

                //***************  LOAD GLOBAL SYMBOLS

                case "<RecentProjects":

                    if (str.Contains(label))
                    {
                        value = SaveProject._GetValue(label, " Count=\"", str);
                        if (value != null)
                        {
                            if (int.TryParse(value, out recent_projects_count)) { }
                            else { error = label + " Count" + ": No Value."; break; }
                        }
                        else { error = label + " Count" + ": No Value."; break; }

                        //---
                        label = "<Project";

                        recent_projects_list = new ObservableCollection<RECENT_PROJECT_Data>();
                    }

                    break;

                case "<Project":

                    if (str.Contains(label))
                    {
                        if (recent_projects_list.Count < recent_projects_count)
                        {
                            value = SaveProject._GetValue(label, " Name=\"", str);
                            if (value != null) { name = value; }
                            else { error = label + " Name" + ": No Value."; return error; }
                            //---
                            value = SaveProject._GetValue(label, " Directory=\"", str);
                            if (value != null) { directory = value; }
                            else { error = label + " Directory" + ": No Value."; return error; }
                            //---
                            value = SaveProject._GetValue(label, " SaveDate=\"", str);
                            if (value != null) { save_date = value; }
                            else { error = label + " SaveDate" + ": No Value."; return error; }
                            //---

                            recent_projects_list.Add( new RECENT_PROJECT_Data(name, directory, save_date));

                            //---  проверяем после добавления не закончился ли список.
                            if (recent_projects_list.Count == recent_projects_count) label = "</RecentProjects>";
                            // возвращаемся к заполнению следующего элемента.
                            else label = "<Project";

                        }     //  сюда мы наверное никогда не зайдем слишком много переменных. 
                        //else { error = label + ": Too many symbols."; break; }
                    }
                    else    //  раньше времени встретили конец списка. 
                        if (str.Contains("</RecentProjects>")) { error = "</RecentProjects>" + ": Too few projects."; break; }

                    break;

                case "</RecentProjects>":

                    if (str.Contains(label))
                    {
                        RECENT_PROJECTS.List.Clear();
                        foreach (RECENT_PROJECT_Data prj in recent_projects_list) RECENT_PROJECTS.List.Add(prj);
                        //RECENT_PROJECTS_List = recent_projects_list;
                        //---
                        //label = "<BaseSymbols";
                        return null;
                    }

                    break;

                //**************  end of switch
            }

            if (error != null) ERRORS.Add(error + "   Line: " + line_count);
        }

        // До конца файла дойти не должны.
        // если цикл закончился - значит дошли до конца файла - значит чегото не нашли.
        ERRORS.Add("End of ini-file. \"" + label + "\"  not found.");

    }
    catch (Exception excp)
    {
        error = excp.ToString();
        MessageBox.Show(error);
    }
    finally
    {
        if (file_stream != null) file_stream.Close();
    }

    return error;
}

//***************   SAVE_LOG_FILE

static public void SAVE_APPLICATION_LOG_FILE()
{

    StreamWriter file_stream = null;

    try
    {
        //*************    CREATE FILE

        if (APPLICATION_LOG_FILE_PATH == null) return;

        file_stream = new StreamWriter(APPLICATION_LOG_FILE_PATH);
        file_stream.AutoFlush = true;

        ERRORS.ERROR_TEXT_BOX.SelectAll();
        ERRORS.ERROR_TEXT_BOX.Copy();
        file_stream.WriteLine(System.Windows.Clipboard.GetText());

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



//***********  Handlers         

        //-----------    Application Closing

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", PROJECT_NAME, MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

            if ( result == MessageBoxResult.Cancel ) e.Cancel = true ;
            if (result == MessageBoxResult.Yes) ApplicationCommands.Save.Execute("Save", null); //SaveProject.SAVE_PROJECT("Save");

            Step5.SAVE_APPLICATION_INI_FILE();

            ERRORS.Message("\nClose application");
            ERRORS.Message("  [ " + DiagramCompiler.GetDateString() + " ]");

            Step5.SAVE_APPLICATION_LOG_FILE();
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            foreach (Window win in Application.Current.Windows) win.Close();

        }

        void Application_Exit(object sender, ExitEventArgs e)
        {

        }



    }  // ******  END of Class Step5
}
