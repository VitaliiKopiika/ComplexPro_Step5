//   ошибки при загрузке проекта из файла с "_func0_"  не связано
// + поиск в с-коде всех базовых функций и вставка их списка в include.h [BaseFunctions].
// + cделать вывод маски бита вывод 16-ричке 
// + проверить элементы как размножается 0х8000 для типа-word 
// + сделать проверку чтобы нельзя было поставить на поле в одной диаграмме две метки с одним именем
// + дорисовать значки JUMPs

// + убрать из компиляции нетворка старый вариант LABEL
// + сделать при компиляции распечатку базовых переменных в h.файл
// + сделать проверку в самом начале компиляции всех списков переменных на ошибки
// + доделать унификацию обработчиков кнопок в окнах переменных.

// + сделать конвертер для базовых тегов для отбора списка по типу данных
// + сделать для BOOL-типов токие же колонки комбобокс как для Тегов.
// + сделать конвертер обновления BitBaseList

// + сделать загрузку тегов из Колиного файла
// + сделать при компиляции замену имени MSG-переменной на номер сообщения и добавлять комментарий
// + сделать валидейшен на подпрограммах как BIT_Validation(...)
// + сделать валидейшен чтобы в локал-переменных типа ин и оут не было типа BOOL
// + сделать validation при не указанном bit_base_column
// + сделать validation что при смене bit_base_column с большего на меньшее разрядность не остается номер бита с большей разрядностью
// + сделать новую форму для базовых переменных
// + доделать во всех остальных формах для ComboBox номера бита подбор для байта, слова двойного слова
// + заменить во всех элементах библиотеки tmp на MAIN_IN_tmp и т.п.
// + собрать все имена коннектор-переменных и объявить их в начале диаграм.
// + вынести распечатку объявлений tmp переменных из заголовка нетворка в h.файл

// + сделать замену BIT_MASK на новые имена из BIT_BASE 
// + сделать чтобы в MAIN функции можно было добавлять локальные переменные только TEMP типа
// + сделать стартовый блок для обнуления всех коннект-переменных и задания начальных условий.
// + запись в файл С-кода
// + запись в файл схемы
// + загрузка схемы из файла
// + сделать ContextMenu для удаления элемента с networka

// + додобавить привязку остальных переменных элементов.
// + имя переменной заменять не на номер ячейки а на номер цепи: Имя цепи с номером нетворка.
// + перед каждым нетворком обнулять все его коннект-переменные для реализации нулей в них
//    когда разрывающих элементы на их входах разомкнуты
// + Добавить к именам номера нетворков чтобы имена не перекрывались.

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
using System.Diagnostics;


namespace ComplexPro_Step5
{
    public partial class Step5
    {


        //*********************************************
public class DiagramCompiler
        {

            public static Window CODE_WINDOW = null;

            public static string C_CODE_STR = null, H_CODE_STR = null;
    
            public const  string VAR_TERMINATORS = " ,.:;=+-*/^&|%~()[]";

            public static string COMPILING_DATE = null;

            //***********   CONSTRUCTOR #1    

            public DiagramCompiler()
            {
                try
                {
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

            }


//***************  Запрос времени для XPS И для даты компиляции.

public static string GetDateString()
{
    try
    {
        return  DateTime.Now.Day.ToString("00") + "." +
                DateTime.Now.Month.ToString("00") + "." +
                DateTime.Now.Year.ToString("0000") + "  " +
                DateTime.Now.Hour.ToString("00") + "." +
                DateTime.Now.Minute.ToString("00") + "." +
                DateTime.Now.Second.ToString("00");
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}


//***************

static public void Compiling()
{
    StreamWriter file_stream = null;

    try
    {
        COMPILING_DATE = DiagramCompiler.GetDateString();
        
        ERRORS.Clear();
        ERRORS.Message("\r\nCompiling... [ " + COMPILING_DATE + " ]");


//************   VALIDATION SYMBOLS LISTs

        ERRORS.Message("\r\n>Compiling symbols lists...");

            string sym_err;

            sym_err = SYMBOLS.Validation_By_Event(SYMBOLS.GLOBAL_SYMBOLS_LIST, null, false);
                if (sym_err != null) ERRORS.Add("Global symbols: " + sym_err);
            sym_err = SYMBOLS.Validation_By_Event(SYMBOLS.BASE_SYMBOLS_LIST, null, false);
                if (sym_err != null) ERRORS.Add("Base symbols: " + sym_err);
            sym_err = SYMBOLS.Validation_By_Event(SYMBOLS.STORABLE_SYMBOLS_LIST, null, false);
                if (sym_err != null) ERRORS.Add("SetPoint symbols: " + sym_err);
            sym_err = SYMBOLS.Validation_By_Event(SYMBOLS.MSG_SYMBOLS_LIST, null, false);
                if (sym_err != null) ERRORS.Add("Msg symbols. " + sym_err);
            foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
            {
                sym_err = SYMBOLS.Validation_By_Event(diagram.LOCAL_SYMBOLS_LIST, null, false);
                if (sym_err != null) ERRORS.Add("Diagram <" + diagram.NAME + ">. Local symbols: " + sym_err);
            }

            sym_err = SYMBOLS.Indication_Validation_By_Event(SYMBOLS.INDICATION_SYMBOLS_LIST, null, false);
            if (sym_err != null) ERRORS.Add("Indication symbols. " + sym_err);

//************   COMPILING H-FILE

        StringBuilder h_code_string = new StringBuilder(); 
       

//*********  [Variable]

        //h_code_string.Append("\r\n\r\n// Global symbols:");

        h_code_string.Append("\r\n//*****    Compiling Date:  " + COMPILING_DATE);

        h_code_string.Append("\r\n\r\n[Variable]");

        h_code_string.Append("\r\n    lword  Sys_Time;   //  System_Timer per 1msec");
        h_code_string.Append("\r\n     word  FirstCall;  //  Переменная для однократной стартовой инициализации");

        foreach( SYMBOLS.Symbol_Data symbol in SYMBOLS.GLOBAL_SYMBOLS_LIST)
        {
            string data_type;
            if (symbol.Data_Type != "BOOL")
            {
                if (SYMBOLS.EQUAL_DATA_TYPES.ContainsKey(symbol.Data_Type))
                {
                    data_type = SYMBOLS.EQUAL_DATA_TYPES[symbol.Data_Type];
                }
                else
                {
                    data_type = " ??? " + symbol.Data_Type;
                    ERRORS.Add("Unknown global symbol type: Symbol=\"" + symbol.Name + "\", Type=\"" + symbol.Data_Type + "\"", null, null);
                    // сообщение в С-текст.
                }

                h_code_string.Append("\r\n    " + data_type + "  " + symbol.Name + ";  //  " + symbol.Comment);
            }
        }


//*********  Temporary vars for [AvMessage], [PrMessage], [SrvMessage]
        //---  Создание промежуточных перемнных для номеров сообщений

        //h_code_string.Append("\r\n\r\n//*********  Temporary vars for [AvMessage], [PrMessage], [SrvMessage]");
        //foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.MSG_SYMBOLS_LIST)
        //{
            //h_code_string.Append("\r\n    " + "word" + "  " + symbol.Name + ";  //  " + symbol.Comment);
        //}


//*********  [SetPoint]
        
        h_code_string.Append("\r\n\r\n[SetPoint]");

        foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.STORABLE_SYMBOLS_LIST)
        {
            string data_type;
            if (symbol.Data_Type != "BOOL")
            {
                if (SYMBOLS.EQUAL_DATA_TYPES.ContainsKey(symbol.Data_Type))
                {
                    data_type = SYMBOLS.EQUAL_DATA_TYPES[symbol.Data_Type];
                }
                else
                {   // сообщение в С-текст.
                    data_type = " ??? " + symbol.Data_Type;
                    ERRORS.Add("Unknown base symbol type: Symbol=\"" + symbol.Name + "\", Type=\"" + symbol.Data_Type + "\"", null, null);
                }

                string format_type = SYMBOLS.GetKolinFormat( symbol );
                //---

                //  Колин компилятор не любит пустой строки в единицах измерений.
                string unit_value = symbol.Unit_Value;
                if (unit_value == null || unit_value == "") unit_value = " ";
                //---

                h_code_string.Append("\r\n    { \"" + symbol.Tag_Name.PadRight(xSetPoint_TagName_PrintSize, ' ') + 
                                   "\", " + data_type + "  " + symbol.Name + 
                                     ", " + symbol.Initial_Value + ", " + symbol.Min_Value + ", " + symbol.Max_Value + 
                                     ", " + symbol.Nom_Value + ", " + symbol.K_Value + ", " + symbol.Step_Value +
                                     ", \"" + unit_value + "\", " + format_type + 
                                     " }; // " + symbol.Comment );
            }
        }


//*********  [BaseVariable]

        h_code_string.Append("\r\n\r\n[BaseVariable]");

        foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.BASE_SYMBOLS_LIST)
        {
            string data_type;
            if (symbol.Data_Type != "BOOL")
            {
                if (SYMBOLS.EQUAL_DATA_TYPES.ContainsKey(symbol.Data_Type))
                {
                    data_type = SYMBOLS.EQUAL_DATA_TYPES[symbol.Data_Type];
                }
                else
                {   // сообщение в С-текст.
                    data_type = " ??? " + symbol.Data_Type;
                    ERRORS.Add("Unknown base symbol type: Symbol=\"" + symbol.Name + "\", Type=\"" + symbol.Data_Type + "\"", null, null);
                }

                h_code_string.Append("\r\n    \"" + symbol.Tag_Name + "\", " + data_type + "  " + symbol.Name + ";  //  " + symbol.Comment);
            }
        }

//*********  [BaseFunction]

        h_code_string.Append("\r\n\r\n[BaseFunction]");

        string h_code_BaseFunction_InsertPosition = "h_code_BaseFunction_InsertPosition";
        h_code_string.Append(h_code_BaseFunction_InsertPosition);
        
//*********  [Indication]

        h_code_string.Append("\r\n\r\n[Indication]");

        foreach( SYMBOLS.Indication_Symbol_Data ind_symbol in SYMBOLS.INDICATION_SYMBOLS_LIST)
        {
          //“txt”, {name1, name2}, {nom1, nom2}, {“txt1”, “txt2”}, {k1, k2}, {format1, format2} }; 

            SYMBOLS.Symbol_Data symbol1 = null, symbol2 = null;
            string name1 = "???", nom1 = "???", unit1 = "???", k1 = "???", format1 = "???";
            string name2 = "", nom2 = "", unit2 = "", k2 = "", format2 = "";

            if (ind_symbol.Name1 != null && ind_symbol.Name1 != "")
            {
                if ( SYMBOLS.GLOBAL_SYMBOLS_LIST.Any(value => (value.Name == ind_symbol.Name1)))
                {
                    symbol1 = SYMBOLS.GLOBAL_SYMBOLS_LIST.First(value => (value.Name == ind_symbol.Name1));
                }
            }


            if (ind_symbol.Name2 != null && ind_symbol.Name2 != "")
            {
                if (SYMBOLS.GLOBAL_SYMBOLS_LIST.Any(value => (value.Name == ind_symbol.Name2)))
                {
                    symbol2 = SYMBOLS.GLOBAL_SYMBOLS_LIST.First(value => (value.Name == ind_symbol.Name2));
                }
                else { name2 = "???"; nom2 = "???"; unit2 = "???"; k2 = "???"; format2 = "???"; }
            }
            //-------

            if (symbol1 != null) 
            { 
                name1 = symbol1.Name ;  nom1 = symbol1.Nom_Value.ToString("0");
                //  Колин компилятор не любит пустой строки в единицах измерений.
                unit1 = symbol1.Unit_Value; if (unit1 == null || unit1 == "") unit1 = " ";
                k1 = symbol1.K_Value.Value.ToString("0");   format1 = SYMBOLS.GetKolinFormat( symbol1, ind_symbol.Format1 );
            }
           

            if (symbol2 != null)
            {
                name2 = symbol2.Name;   nom2 = symbol2.Nom_Value.ToString("0");
                unit2 = symbol2.Unit_Value; if (unit2 == null || unit2 == "") unit2 = " ";
                k2 = symbol2.K_Value.Value.ToString("0"); format2 = SYMBOLS.GetKolinFormat(symbol2, ind_symbol.Format2);
            }
            //-------

            if (symbol2 != null)
            {
                h_code_string.Append("\r\n    { \"" + ind_symbol.Text + "\" , " +
                                                "{ " + name1 + " ,  " + name2 + " } , " +
                                                "{ " + nom1 + " ,  " + nom2 + " } , " +
                                                "{ \"" + unit1 + "\" , \"" + unit2 + "\" }, " +
                                                "{ " + k1 + " ,  " + k2 + " } , " +
                                                "{ " + format1 + " ,  " + format2 + " }" + " } ;");
            }
            else
            {
                h_code_string.Append("\r\n    { \"" + ind_symbol.Text + "\" , " +
                                                "{ " + name1 + "         } , " +
                                                "{ " + nom1  + "         } , " +
                                                "{ \"" + unit1 + "\"     } , " +
                                                "{ " + k1 + "        } , " +
                                                "{ " + format1 + "       }" + " } ;");
            }

        }

//*********  [AvMessage]

        //   Прописывание в адреса MSG-переменных порядковых номеров сообщений. 
        DiagramCompiler.MsgSymbols_InitialValues(SYMBOLS.MSG_SYMBOLS_LIST);
        //---

        h_code_string.Append("\r\n\r\n[AvMessage]");
        string msg_str;
        foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.MSG_SYMBOLS_LIST)
        {
            //---  Дополняем тексты сообщений пробелами до длины в 14 символов.
            msg_str = symbol.Initial_Value;
            if (msg_str.Length < 14) msg_str = msg_str.PadRight(14);

            if (symbol.Data_Type == "FMSG") h_code_string.Append("\r\n    \"" + msg_str + "\";");
        }

//*********  [PrMessage]

        h_code_string.Append("\r\n\r\n[PrMessage]");

        foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.MSG_SYMBOLS_LIST)
        {
            msg_str = symbol.Initial_Value;
            if (msg_str.Length < 14) msg_str = msg_str.PadRight(14);

            if (symbol.Data_Type == "WMSG") h_code_string.Append("\r\n    \"" + msg_str + "\";");
        }

//*********  [SrvMessage]

        h_code_string.Append("\r\n\r\n[SrvMessage]");

        foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.MSG_SYMBOLS_LIST)
        {
            msg_str = symbol.Initial_Value;
            if (msg_str.Length < 14) msg_str = msg_str.PadRight(14);

            if (symbol.Data_Type == "SMSG") h_code_string.Append("\r\n    \"" + msg_str + "\";");
        }

        
        // перенесено вниз после подсчета и вставки списка BaseFunctions
        //H_CODE_STR = h_code_string.ToString();

//************   COMPILING C-FILE

        StringBuilder code_string = new StringBuilder();

        code_string.Append("\r\n//*****    Compiling Date:  " + COMPILING_DATE);

        string c_code_TMPs_InsertPosition = "c_code_TMPs_InsertPosition";

        foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        {
            ERRORS.Message("\r\n>Compiling: " + diagram.NAME);

            code_string.Append("\r\n\r\n//***  DIAGRAM: " + diagram.NAME);
            code_string.Append("\r\n//***  COMMENT: " + diagram.COMMENT);
            code_string.Append("\r\n");

            //---  запоминаем чтобы потом в это место вставить объявления TMP-переменных.

            if (diagram.NAME == "MAIN")
            {
                code_string.Append("\r\n" + "void  main( void )");
                code_string.Append("\r\n{");
                //---
                code_string.Append(Print_Diagram_Local_Vars(diagram));
                //---  запоминаем чтобы потом в это место вставить объявления TMP-переменных.
                //code_string_TMPs_position = code_string.Length; 
                code_string.Append(c_code_TMPs_InsertPosition);
                //---

                //code_string.Append("\r\n\r\n  lword Sys_Time;  //  System_Timer per 1msec");

                //  Проверка для инициализации пемеменных при первом вызове main() после запуска контроллера.
                code_string.Append("\r\n\r\n//***  Проверка для инициализации переменных при первом вызове main() после запуска контроллера.");
                code_string.Append("\r\n  _func0_FIRST_CALL( FirstCall );");
                code_string.Append("\r\n\r\n  if ( FirstCall == 0 ) goto Start;");
                //---
                //---  Инициализация системного таймера.
                code_string.Append("\r\n\r\n//***  System timer initialization");
                code_string.Append("\r\n  _func0_iSYS_TIME(Sys_Time);");
                //---
                code_string.Append("\r\n\r\n//***  Global Symbols Initial Values");
                code_string.Append(DiagramCompiler.Symbols_InitialValues(SYMBOLS.GLOBAL_SYMBOLS_LIST));
                //---
                code_string.Append("\r\n\r\n//***  Base Symbols Initial Values");
                code_string.Append(DiagramCompiler.Symbols_InitialValues(SYMBOLS.BASE_SYMBOLS_LIST));
                //---
                //  Обнуление локальных статических и таймерных переменных - если они когда-нибудь будут.
                code_string.Append("\r\n\r\n//***  Local Symbols Initial Values");
                foreach (Diagram_Of_Networks local_diagram in DIAGRAMS_LIST)
                {
                    if (local_diagram.NAME != "MAIN")
                    {
                        code_string.Append("\r\n\r\n//***  Diagram: " + local_diagram.NAME);
                        code_string.Append(DiagramCompiler.Symbols_InitialValues(local_diagram.LOCAL_SYMBOLS_LIST));
                    }
                }

                //---
                code_string.Append("\r\n\r\n//***  End of Symbols Initial Values");

                //---  Начало программного цикла
                code_string.Append("\r\n\r\n//***  Start of Programm Cikl");
                code_string.Append("\r\n  Start:");

                //---  Циклический вызов системного таймера для актуализации его внутренних переменных и переменной Sys_Time.
                code_string.Append("\r\n\r\n//***  System timer");
                code_string.Append("\r\n  _func0_SYS_TIME(Sys_Time);");
            }
            else
            {
                code_string.Append(DiagramCompiler.Diagram_Name_and_Parameters(diagram));

                //---  запоминаем чтобы потом в это место вставить объявления TMP-переменных.
                //code_string_TMPs_position = code_string.Length; 
                code_string.Append(c_code_TMPs_InsertPosition);
            }


            List<string> connect_vars_list = new List<string>();
            List<string>  out_tmp_vars_list = new List<string>();


            foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
            {
                ERRORS.Message("\r\n>>Compiling: " + network.NAME);

                DiagramAnalysator.Network_Analysator(network);

                code_string.Append(DiagramCompiler.Network_Compiler(network, out_tmp_vars_list));

                connect_vars_list.AddRange(network.Chains_Vars_list);
            }
            
            //---   Распечатка Connect-vars и TMP-vars в начале diagram.

            code_string.Replace(c_code_TMPs_InsertPosition, "\r\n//***  Connect symbols" +  
                                                            Print_Connect_vars(connect_vars_list) +
                                                            "\r\n//***  Temporary symbols" +  
                                                            Print_TMP_vars(out_tmp_vars_list));

            //---
            if (diagram.NAME == "MAIN")
            {
                //---  Конец программного цикла
                code_string.Append("\r\n\r\n//***  End of Programm Cikle");
                code_string.Append("\r\n  exit;");
            }
            else
            {
                code_string.Append("\r\n  return;");
            }

            code_string.Append("\r\n}");
            //---

            //if (ERRORS.Count == 0) ERRORS.Message("\r\nNo errors.");
            //else ERRORS.Message("\r\nErrors: " + ERRORS.Count);
            //---
        }

//*************  INSERT BASE FUNCTIONS LIST INTO INCLUDE.H

         //---   Поиск в с-коде всех базовых функций и вставка их списка в include.h [BaseFunctions].
        string func0_prefix = "_func0_";
        h_code_string.Replace(h_code_BaseFunction_InsertPosition, Print_BaseFunctions( code_string.ToString(), func0_prefix));

            //  очищаем из текста все "_func0_"
        code_string.Replace(func0_prefix, "");

//*************  NO ERRORS

        H_CODE_STR = h_code_string.ToString();

        C_CODE_STR = code_string.ToString();

//*************  NO ERRORS

        if (ERRORS.Count == 0) ERRORS.Message("\r\nNo errors.");
        else ERRORS.Message("\r\nErrors: " + ERRORS.Count);

        //ERRORS.Message("\r\n------------------------------");
        

//*************    CREATING FILE NAMES

        ERRORS.Message("\r\nSave output files...");

        string prj_file_path, c_file_path, h_file_path;

        if (PROJECT_DIRECTORY_PATH != null &&
               Directory.Exists(PROJECT_DIRECTORY_PATH))
        {
            prj_file_path = PROJECT_DIRECTORY_PATH + PROJECT_NAME + "_prj.c";
            c_file_path = PROJECT_DIRECTORY_PATH + PROJECT_NAME + ".c";
            h_file_path = PROJECT_DIRECTORY_PATH + "include.h";

            ERRORS.Message("\r\n>Save <" + PROJECT_NAME + "_prj.c" + ">.");
            ERRORS.Message("\r\n>Save <" + "include.h" + ">.");
            ERRORS.Message("\r\n>Save <" + PROJECT_NAME + ".c" + ">.");

//*************  SAVE PRG-C-FILE 

            file_stream = new StreamWriter(prj_file_path);
            file_stream.AutoFlush = true;

            file_stream.WriteLine("\r\n#include \"include.h\"");
            file_stream.WriteLine("\r\n#include \"" + PROJECT_NAME + ".c\"");

            if (file_stream != null) file_stream.Close();

//*************  SAVE C-FILE 

            file_stream = new StreamWriter(c_file_path);
            file_stream.AutoFlush = true;

            file_stream.WriteLine(C_CODE_STR);

            if (file_stream != null) file_stream.Close();

//*************  SAVE H-FILE 

            file_stream = new StreamWriter(h_file_path);
            file_stream.AutoFlush = true;

            file_stream.WriteLine(H_CODE_STR);

            if (file_stream != null) file_stream.Close();


            //*********   Имитация "задержки" процесса записи.

            Cursor old_cursor = Application.Current.MainWindow.Cursor;
            Application.Current.MainWindow.Cursor = Cursors.Wait;
            int second = DateTime.Now.Second;
            while ((uint)(DateTime.Now.Second - second) < 2) { }
            Application.Current.MainWindow.Cursor = old_cursor;


//*************  SHOW CODE 

            if (ERRORS.Count != 0) SHOW_CODE_WINDOW();

            //*************  START INTERPRETER

            if (ERRORS.Count == 0)
            {
                string output = RUN_INTERPRETER(prj_file_path);

                //*************  SHOW INTERPRETER ERRORS

                ERRORS.Message(output);

                SHOW_CODE_WINDOW();

            }
        }
        else
        {
            ERRORS.Add("\r\nProject directory <" + PROJECT_DIRECTORY_PATH + "> doesn't exists");

            ERRORS.Message("\r\nCompilation terminated");
        }
//**********

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


//*************  DIAGRAM NAME & PARAMETERS

static public string Diagram_Name_and_Parameters(Diagram_Of_Networks diagram)
{
    try
    {
        StringBuilder code_string = new StringBuilder();

        code_string.Append("\r\n" + "void  " + diagram.NAME + " (");

        int i = 0;

        foreach (SYMBOLS.Symbol_Data symbol in diagram.LOCAL_SYMBOLS_LIST)
        {
            if (symbol.Memory_Type == "IN" || symbol.Memory_Type == "OUT")
            {
                if (i != 0) code_string.Append(",");
                //---
                string data_type;

                    if (SYMBOLS.EQUAL_DATA_TYPES.ContainsKey(symbol.Data_Type))
                    {
                        data_type = SYMBOLS.EQUAL_DATA_TYPES[symbol.Data_Type];
                    }
                    else
                    {
                        data_type = " ??? " + symbol.Data_Type;
                        ERRORS.Add("Unknown local IN/OUT-symbol type: Symbol=\"" + symbol.Name + "\", Type=\"" + symbol.Data_Type + "\"", null, null);
                    }

                    code_string.Append(" " + data_type + "  " + symbol.Name); //+ ";  //  " + symbol.Comment);

                i++;               
            }
        }

        code_string.Append(" )");
        //---

        code_string.Append("\r\n{");

        code_string.Append(Print_Diagram_Local_Vars(diagram));

        return code_string.ToString();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//*************  PRINT DIAGRAM LOCAL VARS

static public string Print_Diagram_Local_Vars(Diagram_Of_Networks diagram)
{
    try
    {
        StringBuilder code_string = new StringBuilder();

        code_string.Append("\r\n//---  Local symbols");

        foreach (SYMBOLS.Symbol_Data symbol in diagram.LOCAL_SYMBOLS_LIST)
        {
            if (symbol.Memory_Type == "TEMP" && symbol.Data_Type != "BOOL")
            {
                code_string.Append("\r\n    ");
                //---
                string data_type;

                if (SYMBOLS.EQUAL_DATA_TYPES.ContainsKey(symbol.Data_Type))
                {
                    data_type = SYMBOLS.EQUAL_DATA_TYPES[symbol.Data_Type];
                }
                else
                {
                    data_type = " ??? " + symbol.Data_Type;
                    ERRORS.Add("Unknown local TEMP-symbol type: Symbol=\"" + symbol.Name + "\", Type=\"" + symbol.Data_Type + "\"", null, null);
                }

                code_string.Append(data_type + "  " + symbol.Name + ";  //  " + symbol.Comment);
            }
        }

        return code_string.ToString();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//*************  SYMBOLS INITIAL VALUES 

static public string Symbols_InitialValues(ObservableCollection<SYMBOLS.Symbol_Data> symbols_list)
{
    try
    {
        StringBuilder code_string = new StringBuilder();


        foreach (SYMBOLS.Symbol_Data symbol in symbols_list)
        {
            if (symbol.Initial_Value != null && symbol.Initial_Value.Length != 0)
            {
                if (symbol.Data_Type == "BOOL")
                {
                    int ax = 1 << (int)symbol.Bit_Number.Value;

                    if (symbol.Initial_Value == "1")
                    {
                        code_string.Append("\r\n        " + symbol.Name + " = " + symbol.Name + " | " + ax + " ;");
                    }
                    else
                    {
                        code_string.Append("\r\n        " + symbol.Name + " = " + symbol.Name + " & " + ~ax + " ;");
                    }
                }
                else code_string.Append("\r\n        " + symbol.Name + " = " + symbol.Initial_Value + " ;");
            }

            //---  Обнуление переменных типа TIMER

            if (symbol.Data_Type == "TIMER") code_string.Append("\r\n        " + symbol.Name + " = 0 ;");

        }

        //code_string.Append("\r\n\r\n//***  End of Symbols Initial Values");

        return code_string.ToString();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//*************  MSG SYMBOLS INITIAL VALUES 

static public string MsgSymbols_InitialValues(ObservableCollection<SYMBOLS.Symbol_Data> symbols_list)
{
    try   
    {     

        StringBuilder code_string = new StringBuilder();
        int fmsg_count = 0, wmsg_count = 0, smsg_count = 0;

        //---  Подстановка вместо текстовых значений порядковых номеров сообщений.
        //---  Сами тексты прописываются в формируемом h-файле.  
        foreach (SYMBOLS.Symbol_Data symbol in symbols_list)
        {
            if (symbol.Data_Type == "FMSG")
            {
                fmsg_count++;
                symbol.Address = fmsg_count;
                if ( fmsg_count > 32 )  ERRORS.Add("Number of FMSG-messages exceeds 32.", null, null);
                //code_string.Append("\r\n        " + symbol.Name + " = " + fmsg_count + " ;");
            }
            else if (symbol.Data_Type == "WMSG")
            {
                wmsg_count++;
                symbol.Address = wmsg_count;
                if (wmsg_count > 32) ERRORS.Add("Number of WMSG-messages exceeds 32.", null, null);
                //code_string.Append("\r\n        " + symbol.Name + " = " + wmsg_count + " ;");
            }
            else if (symbol.Data_Type == "SMSG")
            {
                smsg_count++;
                symbol.Address = smsg_count;
                if (smsg_count > 32) ERRORS.Add("Number of SMSG-messages exceeds 32.", null, null);
                //code_string.Append("\r\n        " + symbol.Name + " = " + smsg_count + " ;");
            }
            else ERRORS.Add("Unknown MSG-symbol type: Symbol=\"" + symbol.Name + "\", Type=\"" + symbol.Data_Type + "\"", null, null);
            //---

        }

        //code_string.Append("\r\n\r\n//***  End of Symbols Initial Values");

        return code_string.ToString();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//***************
                                                                    // TMPs_List - накопитель для всех используемых в элементах tmp.
            static public string Network_Compiler(Network_Of_Elements network, List<string> out_TMPs_List)
            {
                try
                {

//**************  Поиск всех исполнительных элементов.

                    //  Столбец за столбцом сверху вниз сканируем ячейки на предмет 
                    // исполнительных элементов - находя элемент распечатываем его С-код.

                    StringBuilder code_string = new StringBuilder();

                    code_string.Append("\r\n\r\n//***  " + network.NAME);
                    code_string.Append("\r\n//***  Comment: " + network.COMMENT);
                    code_string.Append("\r\n");

                    //if ( network.LABEL != null && network.LABEL != "")  code_string.Append("\r\n" + network.LABEL + ":\r\n");

                    //  Обнуление всех коннект-переменных.
    //??? это нарушает работу триггеров.
                    foreach (string str in network.Chains_Vars_list)
                    {
                        code_string.Append("\r\n        " + str + " = 0 ;");
                    }

                    //  накопитель для всех используемых в элементах tmp.
                    //List<string> TMPs_List = new List<string>();

                    for (int x = 0; x < network.Network_panel_copy[0].Count; x++)
                    {
                        for (int y = 0; y < network.Network_panel_copy.Count; y++)
                        {
                            //********************
                            
                            // Поиск очередного элемента не коннектора.
                            y = Find_NextNonConnector(network, x, y);
                            // В столбце нет ни одного.
                            if (y == -1) break;

                            code_string.Append("\r\n\r\n//***  Block " + network.Chains_FullVars_panel_copy[y][x] );
                            code_string.Append(" :  " + network.Network_panel_copy[y][x].Name);
                            code_string.Append(",  " + network.Network_panel_copy[y][x].Category);
                            code_string.Append("\r\n");
                            code_string.Append( Compiling_ReplaceVarsInElementCcode(network, x, y, out_TMPs_List));
                        }
                    }

                    return code_string.ToString();

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


//***************   Распечатка в include.h всех найденных в выходном с-тексте имен базовых переменных.

            static public string Print_BaseFunctions(string code_string, string func0_prefix)
            {
                try
                {
                    // Подсчет и вставка списка BaseFunctions

                    List<string> funcs_list = new List<string>();

                    string code_str = code_string.ToString();

                    //******

                    int func_ind = 0, func_ind2 = 0, func_ind3 = 0;

                    // составляем список всех встретившихся в тексте вызовов функций.
                    while ((func_ind = code_str.IndexOf(func0_prefix, func_ind3)) >= 0)
                    {
                        func_ind2 = func_ind + func0_prefix.Length;
                        func_ind3 = code_str.IndexOf("(", func_ind2);
                        if (func_ind3 - func_ind2 < 2 || func_ind3 - func_ind2 > 10)
                        {
                            int err_line = 1;
                            for (int i = 0; i < func_ind2; i++) if (code_str[i] == '\n') err_line++;

                            ERRORS.Add("Base function name is out of range 2...10 symbols. Line " + err_line);
                            break;
                        }
                        //  считываем имя функции находящееся между "_func0_" и "(".
                        funcs_list.Add(code_str.Substring(func_ind2, func_ind3 - func_ind2));
                    }

                    //  очистка дублирующихся имен.
                    if (funcs_list.Count >= 2)
                    {
                        for (int i = 0; i < funcs_list.Count; i++)
                        {
                            string str = funcs_list[i]; // временно сохраняем значение
                            //  удаляем все копии
                            funcs_list.RemoveAll(value => (value == str));
                            //  восстанавливаем элемент на прежнем индексе
                            funcs_list.Insert(i, str);
                        }
                    }

                    StringBuilder insert_code = new StringBuilder();
                    for (int i = 0; i < funcs_list.Count; i++) insert_code.Append("\r\n    \"" + funcs_list[i] + "\" ;");

                    return insert_code.ToString();
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


//***************   Распечатка всех Connect-vars Tmp-vars в начале листинга.

            static public string Print_Connect_vars(List<string> Connect_List)
            {
                try
                {

                    //---   Отбираем символы без повторов.
                    List<string> temp_list = new List<string>();

                    foreach (string str in Connect_List)
                    {
                        if (!temp_list.Contains(str)) temp_list.Add(str);
                    }


                    //---  Распечатка.

                    StringBuilder code_string = new StringBuilder();

                    foreach (string str in temp_list) code_string.Append("\r\n    word    " + str + " ;");

                    return code_string.ToString();
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }

//**********

            static public string Print_TMP_vars(List<string> TMPs_List)
            {
                try
                {

                    //---   Отбираем символы без повторов.
                    List<string> temp_list = new List<string>();

                    foreach (string str in TMPs_List)
                    {
                        if (!temp_list.Contains(str)) temp_list.Add(str);
                    }


                    //---  Распечатка.

                    StringBuilder code_string = new StringBuilder();

                    //code_string.Append("\r\n//***  Temporary symbols");

                    foreach (string str in  temp_list)
                    {
                        string h_code;

                        if (str[0] == 'l' && str[1] == 'w')         h_code = "\r\n    lword   ";
                        else if (str[0] == 's' && str[1] == 'l' &&  str[2] == 'w') 
                                                                    h_code = "\r\n    slword  ";
                        else if (str[0] == 'w')                     h_code = "\r\n    word    ";
                        else if (str[0] == 's' && str[1] == 'w')    h_code = "\r\n    sword   ";
                        else if (str[0] == 'b')                     h_code = "\r\n    byte    ";
                        else if (str[0] == 's' && str[1] == 'b')    h_code = "\r\n    sbyte   ";
                        //else if (str[0] == 'i')                     h_code = "\r\n    sword   ";
                        else if (str[0] == 'f')                     h_code = "\r\n    float   ";
                        else if (str[0] == 'd')                     h_code = "\r\n    double  ";
                        else
                        {
                            h_code = "\r\ntmp_???   ";
                            ERRORS.Add("Wrong data type of tmp-symbol. ", null, null);//new int[] { x, y });
                        }

                        code_string.Append(h_code + str + " ;");
                    }

                    return code_string.ToString();
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }

//***************   Поиск в заданном столбце элемента не коннектора.

            static public int Find_NextNonConnector(Network_Of_Elements network, int start_x, int start_y)
            {
                try
                {
                    for (int y = start_y; y < network.Network_panel_copy.Count; y++)
                    {
                        List<Element_Data> elements_list = network.Network_panel_copy[y];
                        Element_Data element;

                        //  Найден элемент. Проверяем нет ли его уже в списке. Если есть то ищем следующий.

                        element = elements_list[start_x];

                        if (element != null)
                        {
                            //  тип элемента: коннекторы, NEW_LINE, END_OF_LINE( для красотыы отображения в С-листинге)
                            //  элементы пропускающие сигналы только слева направо сюда нельзя.
                            if (element.Txt_Image.Type_int != 1 )
                            {
                                return y;
                            }
                        }

                    }

                    return -1;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return -1;
                }
            }

//***************   Привязка коннект-переменных ко входам EN/ENO элемента.

            static public string Compiling_ReplaceVarsInElementCcode(Network_Of_Elements network, int x, int y, List<string> tmp_list)
            {
                try
                {
                    Element_Data element = network.Network_panel_copy[y][x];

//****************   ERRORs

                    if (element.Txt_Image.Name != "END_OF_LINE" &&
                                                        element.Txt_Image.C_Code_list == null)
                    {
                        // ERROR
                        //ERRORS.Add("No code exists.", network, new int[] { x , y });
                        ERRORS.Add("System: No code exists. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });

                        //MessageBox.Show("No code exists: " + network.Chains_FullVars_panel_copy[y][x]);
                        return "Error code!"; // сообщение в С-текст.
                    }


                    if (element.Txt_Image.Name != "NEW_LINE")
                    {
                        if (network.Chains_Vars_panel_copy[y][x - 1] == null)
                        {
                            // ERROR
                            ERRORS.Add("Cell empty.", network, new int[] { x - 1, y });
                            //MessageBox.Show("Left cell Empty: " + network.Chains_FullVars_panel_copy[y][x]);
                            return "Error code!";  // сообщение в С-текст.
                        }
                    }


                    if (element.Txt_Image.Name != "END_OF_LINE" && element.Txt_Image.Name != "LABEL")
                    {
                        if (network.Chains_Vars_panel_copy[y][x + 1] == null)
                        {
                            // ERROR
                            ERRORS.Add("Cell empty.", network, new int[] { x + 1, y });
                            //MessageBox.Show("Right cell Empty: " + network.Chains_FullVars_panel_copy[y][x]);
                            return "Error code!";
                        }
                    }

                    if (element.Txt_Image.Name == "LABEL")
                    {
                        if (x != 1)
                        {
                            // ERROR
                            ERRORS.Add("Element <LABEL> can be placed at column #1 only.", network, new int[] { x, y });
                            //MessageBox.Show("Right cell Empty: " + network.Chains_FullVars_panel_copy[y][x]);
                            return "Error code!";
                        }
                    }

//**************  COMPILATION

                    if (element.Txt_Image.Name == "END_OF_LINE") return "";

                    if (element.Txt_Image.Name == "NEW_LINE")
                    {
                        string new_str = element.Txt_Image.C_Code_list[0] ;

                        //  замена выхода элемента ENO на имя своей ячейки.
                        //  нельзя заменять на имя примыкающей справа ячейки, т.к. там может сразу 
                        //  стоять прерывающий элемент и мы запишем единицу на его выход т.е. в следущую цепь.
                        new_str = Replace(new_str, "ENO", network.Chains_Vars_panel_copy[y][x]);

                        return new_str;
                    }
                    

                    StringBuilder code_string = new StringBuilder();

                    //  для контроля что все входы/выходы привязаны.
                    //  составляем коллекцию имен именованых входов у элемента.
                    //  и далее будем вычеркивать из нее найденные имена.
                    List<string> var_list = new List<string>();  
                    if (element.Txt_Image.IO_list != null)
                    {
                        foreach ( List<ElementImage.IO_Data> list in element.Txt_Image.IO_list)
                        {
                            if ( list != null)
                            {
                                foreach (ElementImage.IO_Data io_data in list)
                                {
                                    if (io_data != null && io_data.Name != null) var_list.Add(io_data.Name);
                                }
                            }
                        }  
                    }

//***********  C-CODE

                    //   Перебираем все по порядку строки С-кода элемента и заменяем в нем
                    //  шаблонные имена пеменных на привязанные Binding-имена.

                    // синхронные списки старых и новых имен tmp.
                    //List<string> tmp_list = new List<string>();
                    //List<string> TMP_List = new List<string>();

                    foreach (string str in element.Txt_Image.C_Code_list )
                    {
                        string new_str;

//***********  ENI
                        //  замена входа элемента EN на имя переменной примыкающей слева ячейки.
                        new_str = Replace(str, "ENI", network.Chains_Vars_panel_copy[y][x - 1]);
//***********  ENO
                        //  замена выхода элемента ENO на имя переменной своей ячейки.
                        //  нельзя заменять на имя примыкающей справа ячейки, т.к. там может сразу 
                        //  стоять прерывающий элемент и мы запишем единицу на его выход т.е. в следущую цепь.
                        new_str = Replace(new_str, "ENO", network.Chains_Vars_panel_copy[y][x]);
//***********  LOOP
                        //  замена имени метки loop на имя переменной своей ячейки+коорд.своей ячейки.
                        new_str = Replace(new_str, "LOOP", network.Chains_FullVars_panel_copy[y][x], VAR_TERMINATORS + "_");

//***********  IO_list - имена входов выходов для каждой части-этажа Image.

                        if (element.Txt_Image.IO_list != null)
                        {
                            for( int i = 0 ; i <  element.Txt_Image.IO_list.Count; i++)
                            {
                                if ( element.Txt_Image.IO_list[i] != null )
                                {
                                    //  пропускаем нулевую пару имен - пару ENI/ENO.
                                    //  даже если им заданы имена в С-тексте их обозначаем ENI/ENO.
                                    if (i == 0)
                                    {   //  просто вычеркиваем переменные из списка чтобы работала защита.
                                        for (int j = 0; j < element.Txt_Image.IO_list[i].Count; j++)
                                        {
                                            ElementImage.IO_Data io_data = element.Txt_Image.IO_list[i][j];
                                            if (io_data != null && io_data.Name != null) var_list.Remove(io_data.Name);
                                        }
                                    }
                                    else 
                                    {
                                        for (int j = 0; j < element.Txt_Image.IO_list[i].Count; j++)
                                        {
                                            string image_name = null;

                                            ElementImage.IO_Data io_data = element.Txt_Image.IO_list[i][j];
                                            if (io_data != null && io_data.Name != null) image_name = io_data.Name;
                                           

                                            //  ПРОВЕРЯЕМ НЕТ ЛИ В ЭТОЙ СТРОКЕ  С-ТЕКСТА  ДАННОЙ ПЕРЕМЕННОЙ И
                                            //  ПРОВЕРЯЕМ НЕТ ЛИ В ЭТОЙ СТРОКЕ  С-ТЕКСТА  БИТОВОЙ-МАСКИ ДЛЯ ДАННОЙ ПЕРЕМЕННОЙ
                                            if (image_name != null)
                                            {
//***********  VAR_BIT_MASK
                                                //  Сначала заменяем битовую маску, а потом саму переменную, 
                                                //  т.к. битовая маска содержит в себе имя переменной и если первой 
                                                //  заменять имя переменной, то олно будет заменено и в имени битовой-маски.
                                                while (Index_Of(new_str, image_name + "_BIT_MASK", 0) >= 0)
                                                {
                                                    if (element.IO_VARs_list[i * 2 + j] != null)
                                                    {
                                                        // НА БУДУЩЕЕ: для универсальности чтобы в С-тексте везде ставить "_BIT_MASK"
                                                        //  а если переменная не битоая то для нее задавать маску "null"
                                                        //  и в С-тексте заменять "_BIT_MASK" на распущенную маску.
                                                        //Int32 mask = -1; // <= 0xFFFFFFFF;
                                                        UInt32 mask = UInt32.MaxValue; // <= 0xFFFFFFFF;
                                                        if (element.IO_VARs_list[i * 2 + j].Bit_Number != null)
                                                        {
                                                            //   вычисляем маску по номеру бита.
                                                            int bit_num = (int)(element.IO_VARs_list[i * 2 + j].Bit_Number.Value);
                                                            //mask = 1 << bit_num;
                                                            mask = (UInt32)1 << bit_num;
                                                        }
                                                            //    сначала заменяем инверсное имя, а затем неинверсное, иначе прямое имя заменится и там и там.
                                                            if (Index_Of(new_str, "~" + image_name + "_BIT_MASK", 0) >= 0)
                                                            {
                                                                new_str = Replace(new_str, "~" + image_name + "_BIT_MASK", "0x" + (~mask).ToString("X8") + "u");
                                                            }
                                                            //---
                                                            if (Index_Of(new_str, image_name + "_BIT_MASK", 0) >= 0)
                                                            {
                                                                new_str = Replace(new_str, image_name + "_BIT_MASK", "0x" + (mask).ToString("X8") + "u");
                                                            }
                                                        
                                                    }
                                                    else    // ERROR
                                                    {       //  Заносим строку все равно, чтобы посмотреть ошибку.
                                                        new_str = Replace(new_str, image_name + "_BIT_MASK", "Name_Bit_Mask???");
                                                        // добавляется внизу в общей ветви code_string.Append(new_str);
                                                        //---
                                                        //ERRORS.Add("No Bit-Name.", network, new int[] { x, y });
                                                        ERRORS.Add("No i/o name. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                        break;
                                                    }
                                                }
//***********  VAR_TMP
                                                //  Сначала заменяем TMP-переменные, а потом саму переменную, 
                                                //  т.к. TMP-переменные содержит в себе имя переменной и если первой 
                                                //  заменять имя переменной, то олно будет заменено и в имени битовой-маски.
                                                while (Index_Of(new_str, image_name + "_tmp", 0, VAR_TERMINATORS + "0123456789") >= 0)
                                                {
                                                    if (element.IO_VARs_list[i * 2 + j] != null)
                                                    {
                                                        //---   Для обычной переменной тип (lw,w,b)_tmp определяется типом самой переменной
                                                        //--- а для битовой переменной (lw,w,b)_tmp определяется типом базовой переменной.
                                                        if (element.IO_VARs_list[i * 2 + j].Data_Type != "BOOL")
                                                        {
                                                            string data_type = element.IO_VARs_list[i * 2 + j].Data_Type;

                                                            if (SYMBOLS.EQUAL_SHORT_DATA_TYPES.ContainsKey(data_type))
                                                            {
                                                                data_type = SYMBOLS.EQUAL_SHORT_DATA_TYPES[data_type];
                                                            }
                                                            else
                                                            {
                                                                new_str = Replace(new_str, image_name + "_tmp", image_name + "_tmp???");
                                                                ERRORS.Add("Unknown data-type for tmp symbol. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                                // сообщение в С-текст.
                                                                break;
                                                            }

                                                            new_str = Replace(new_str, image_name + "_tmp", data_type + "_tmp", VAR_TERMINATORS + "0123456789");
                                                        }
                                                        else 
                                                        {
                                                            new_str = Replace(new_str, image_name + "_tmp", "lw_tmp", VAR_TERMINATORS + "0123456789");
/* 11-06-18
 * проставляем тип tmp равным lw_tmp независимо от типа базы
 * иначе нестыкухи при BOOL для передачи в функцию или BOOL не для предачи.
 *                                                          // Поиск типа данных базы битовой переменной для подмены префикса в tmp.
                                                            if (element.IO_VARs_list[i * 2 + j].Bit_Base != null)
                                                            {
                                                                string base_bit_type=null;

                                                                //  ищем блок данных переменной BIT_BASE по ее имени.
                                                                foreach (SYMBOLS.Symbol_Data symbol in element.IO_VARs_list[i * 2 + j].Owner)
                                                                {
                                                                    if (symbol.Name == element.IO_VARs_list[i * 2 + j].Bit_Base)
                                                                    {
                                                                        base_bit_type = symbol.Data_Type;
                                                                        break;
                                                                    }
                                                                }
                                                                //  переменная не найдена.
                                                                if (base_bit_type == null)
                                                                {
                                                                    new_str = Replace(new_str, image_name + "_tmp", image_name + "_tmp???");
                                                                    ERRORS.Add("Unknown Base_Bit symbol for BOOL-tmp symbol. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                                    break;
                                                                }

                                                                if (SYMBOLS.EQUAL_SHORT_DATA_TYPES.ContainsKey(base_bit_type))
                                                                {
                                                                    base_bit_type = SYMBOLS.EQUAL_SHORT_DATA_TYPES[base_bit_type];
                                                                }
                                                                else
                                                                {
                                                                    new_str = Replace(new_str, image_name + "_tmp", image_name + "_tmp???");
                                                                    ERRORS.Add("Unknown BOOL-type for BOOL-tmp symbol. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                                    // сообщение в С-текст.
                                                                    break;
                                                                }
//  При вызове в С-коде элемента п/п Сереги для передачи в нее BOOL в всегда проставляем lw_tmp0 в С-коде самого элемента
//  При использовании BOOL в самом С-коде элемента заменяем тип tmp на тип базы битовой переменной.
                                                                new_str = Replace(new_str, image_name + "_tmp", base_bit_type + "_tmp", VAR_TERMINATORS + "0123456789");
                                                                
                                                                //new_str = Replace(new_str, image_name + "_tmp", "lw_tmp", VAR_TERMINATORS + "0123456789");
                                                            }
                                                            else    // ERROR
                                                            {       //  Заносим строку все равно, чтобы посмотреть ошибку.
                                                                new_str = Replace(new_str, image_name, "Name_Bit_Base???");
                                                                // добавляется внизу в общей ветви code_string.Append(new_str);
                                                                //---
                                                                //ERRORS.Add("No Bit-Name.", network, new int[] { x, y });
                                                                ERRORS.Add("No i/o Bit_Base-name. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                                break;
                                                            }
*/                                                        }
                                                    }
                                                    else    // ERROR
                                                    {       //  Заносим строку все равно, чтобы посмотреть ошибку.
                                                        new_str = Replace(new_str, image_name + "_BIT_MASK", "Name_Bit_Mask???");
                                                        // добавляется внизу в общей ветви code_string.Append(new_str);
                                                        //---
                                                        //ERRORS.Add("No Bit-Name.", network, new int[] { x, y });
                                                        ERRORS.Add("No i/o name. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                        break;
                                                    }
                                                }

//***********  VAR_NAME
                                                //**************  ЗАМЕНА ИМЕНИ ПЕРЕМЕННОЙ.

                                                while (Index_Of(new_str, image_name, 0) >= 0)
                                                {
                                                    if (element.IO_VARs_list[i * 2 + j] != null)
                                                    {
                                                        //  заменяем переменную 
                                                        if (element.IO_VARs_list[i * 2 + j].Data_Type != "BOOL")
                                                        {
                                                            // для MSG-переменных в текст вмнсто имени подставляем порядковый номер сообщения хранящийся в Address.
                                                            // в Address порядковый номер сообщения записывается в п.п. MsgSymbols_InitialValues(...)
                                                            if (element.IO_VARs_list[i * 2 + j].Data_Type == "FMSG" ||
                                                                element.IO_VARs_list[i * 2 + j].Data_Type == "WMSG" ||
                                                                element.IO_VARs_list[i * 2 + j].Data_Type == "SMSG")
                                                            {
                                                                new_str = Replace(new_str, image_name, element.IO_VARs_list[i * 2 + j].str_Address);
                                                            }
                                                            else //  БАЗОВАЯ ВЕТВЬ.
                                                            {
                                                                new_str = Replace(new_str, image_name, element.IO_VARs_list[i * 2 + j].Name);
                                                            }
                                                        }
                                                        else 
                                                        {
                                                            if (element.IO_VARs_list[i * 2 + j].Bit_Base != null)
                                                            {
                                                                new_str = Replace(new_str, image_name, element.IO_VARs_list[i * 2 + j].Bit_Base);
                                                            }
                                                            else    // ERROR
                                                            {       //  Заносим строку все равно, чтобы посмотреть ошибку.
                                                                new_str = Replace(new_str, image_name, "Name_Bit_Base???");
                                                                // добавляется внизу в общей ветви code_string.Append(new_str);
                                                                //---
                                                                //ERRORS.Add("No Bit-Name.", network, new int[] { x, y });
                                                                ERRORS.Add("No i/o Bit_Base-name. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                                break;
                                                            }
                                                        }

                                                        //  вычеркиваем переменную из списка.
                                                        var_list.Remove(image_name);
                                                    }
                                                    else    // ERROR
                                                    {       //  Заносим строку все равно, чтобы посмотреть ошибку.
                                                        new_str = Replace(new_str, image_name, "Name???");
                                                        // добавляется внизу в общей ветви code_string.Append(new_str);
                                                        //---
                                                        //ERRORS.Add("No Var-Name.", network, new int[] { x, y });
                                                        ERRORS.Add("No i/o name. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                                                        break;
                                                    }

                                                    //  Когда все заменено - ДОБАВЛЯЕМ КОММЕНТАРИЙ ПЕРЕМЕННОЙ.
                                                    new_str = new_str + "  //  " + element.IO_VARs_list[i * 2 + j].Comment;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //***********  TMP  Поиск и составление коллекции всех tmp.

                            Extract(new_str, "tmp", tmp_list);

                        }

//***********  Output to Code


                        code_string.Append(new_str);
                        code_string.Append("\r\n");

                    }

//*********** ERROR - не все переменные элемента нашли использование в С-коде.

                    if ( var_list.Count > 0 )   
                    {      
                        //ERRORS.Add("Not all element inputs/outputs implemented in C-code.", network, new int[] { x, y });
                        ERRORS.Add("System: Not all element i/o implemented in C-code. Block " + network.Chains_FullVars_panel_copy[y][x], network, null);//new int[] { x, y });
                    }

                    return code_string.ToString();
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    return null;
                }
            }


//***********  SEEK INDEX OF SUBSTRING, return -1 if isn't found

static public int Index_Of(string in_str, string sub_str, int start_index, string terminators = VAR_TERMINATORS)
{
    //string terminators = " ,.;=+-*/^&|%~()[]";

    try
    {
        int end_index;

        for (; start_index < in_str.Length; )
        {
            start_index=in_str.IndexOf(sub_str,start_index);
            if (start_index < 0) break;  // дошли до конца строки.

            end_index = start_index + sub_str.Length;
            //---

            // Проверка: не является ли искомая подстрока частью слова.
            // Искомая подстрока должна быть ограничена каким-либо из символов terminators или началом/концом строки.
            int start_ok = 0;
            if (start_index == 0 ) start_ok = 1;
            else
            {
              foreach(char symbol in terminators)
              {
                  if (in_str[start_index - 1] == symbol) { start_ok = 1; break; }
              }
            }

            int end_ok = 0;                    //  для tmp0: не проверяем завершающий символ т.к. цифра мешает проверке.
            if (end_index == in_str.Length) end_ok = 1;
            else
            {
                foreach (char symbol in terminators)
                {
                    if (in_str[end_index] == symbol) { end_ok = 1; break; }
                }
            }

            if (start_ok == 1 && end_ok == 1) break;

            //--- возврат к продолжению поиска.
            start_index = end_index;
        }

        if (start_index >= in_str.Length) start_index = -1; // ничего не найдено.

        return start_index;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return -1;
    }
}

//**************  REPLACE


static public string Replace(string in_str, string old_value, string new_value, string terminators = VAR_TERMINATORS)
{
    try
    {
        string out_str;
        int start_index=0;

        out_str = in_str;

        for (; start_index < out_str.Length; )
        {
            start_index = Index_Of(out_str, old_value, start_index, terminators ); //, terminators);
            if (start_index < 0) break;  // дошли до конца строки.

            //  Заменяем найденную подстроку.

            out_str = out_str.Remove(start_index, old_value.Length);
            out_str = out_str.Insert(start_index, new_value);
            
            //--- возврат к продолжению поиска.
            start_index += old_value.Length; 
        }

        return out_str;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//***********  EXTRACT  TMP-SUBSTRING, return -1 if isn't found

static public int Extract(string in_str, string sub_str, List<string> list, string terminators = VAR_TERMINATORS)
{

    try
    {
        //string terminators = " ,.;=+-*/^&|%~()[]";

        int end_index;
        
        for (int start_index = 0 ; start_index < in_str.Length; )
        {
            start_index = in_str.IndexOf(sub_str, start_index);
            if (start_index < 0) break;  // дошли до конца строки.

            end_index = start_index + sub_str.Length;
            //---

            // Проверка: не является ли искомая подстрока частью слова.
            // Искомая подстрока должна быть ограничена каким-либо из символов terminators или началом/концом строки.

            //  идем влево пока не появится разделитель или начало строки.
            for (; start_index >= 0; start_index--)
            {
                int ok = 0;
                foreach (char symbol in terminators)
                {
                    if (in_str[start_index] == symbol) { ok = 1; break; }
                }
                if (ok == 1) break;
            }
            start_index++;

            //  идем вправо пока не появится разделитель или начало строки.

            for (; end_index < in_str.Length; end_index++)
            {
                int ok = 0;
                foreach (char symbol in terminators)
                {
                    if (in_str[end_index] == symbol) { ok = 1; break; }
                }
                if (ok == 1) break;
            }
            //end_index--;


            //   извлекаем полное имя tmp.
            string str = in_str.Substring(start_index, end_index - start_index);

            //  если такого имени еще нет в коллекции, то добавляем его.
            if (list.IndexOf(str) == -1) list.Add(str);

            //--- возврат к продолжению поиска.
            start_index = end_index;
        }


        return 0;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return -1;
    }
}


//*************  SHOW CODE WINDOW


static public void  SHOW_CODE_WINDOW ()
{
    try
    {

        if (H_CODE_STR != null && C_CODE_STR != null)
        {
            //--- Закрываем предыдущее окно чтобы не запутаться в них при просмотре: какое старое какое новое.
            if (CODE_WINDOW != null) CODE_WINDOW.Close();

            //Window CODE_WINDOW;
            CODE_WINDOW = new Window();
            CODE_WINDOW.Title = "Output List. " + COMPILING_DATE;
            //CODE_WINDOW.SizeToContent = SizeToContent.WidthAndHeight;
            CODE_WINDOW.Height = 500;
            CODE_WINDOW.Width = 700;

                ScrollViewer scrollviewer = new ScrollViewer();
                scrollviewer.VerticalAlignment = VerticalAlignment.Stretch;
                scrollviewer.HorizontalAlignment = HorizontalAlignment.Stretch;
                scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                    TextBox text_box = new TextBox();
                    text_box.IsReadOnly = true;
                    //text_box.MinWidth = 700;
                    //text_box.MinHeight = 500;

                scrollviewer.Content = text_box;

            CODE_WINDOW.Content = scrollviewer;


            string c_file_path = PROJECT_DIRECTORY_PATH + "\\" + PROJECT_NAME + ".c";
            string h_file_path = PROJECT_DIRECTORY_PATH + "\\" + "include.h";

            //********  нумерация строк для дальнейшего поиска ошибок.

            int i = 1;
            string h_buff_str = H_CODE_STR;

            h_buff_str = h_buff_str.Insert(0, (i++).ToString("000") + "  ");
            for (int j = 0; (j = h_buff_str.IndexOf("\r\n", j + 2)) > 0; ) h_buff_str = h_buff_str.Insert(j + 2, (i++).ToString("000") + "  ");

            i = 1;
            string buff_str = C_CODE_STR;

            buff_str = buff_str.Insert(0, (i++).ToString("000") + "  ");
            for (int j = 0; (j = buff_str.IndexOf("\r\n", j + 2)) > 0; ) buff_str = buff_str.Insert(j + 2, (i++).ToString("000") + "  ");

            text_box.Text = "\r\n//***** " + h_file_path +
                            "\r\n" + h_buff_str +
                            "\r\n\r\n//***** " + c_file_path +
                            "\r\n" + buff_str;

            CODE_WINDOW.Show();
        }
        else MessageBox.Show( "No compiled list.", "Compiler", MessageBoxButton.OK, MessageBoxImage.Warning);
        
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return ;
    }
}


//*************  START INTERPRETER

static public string RUN_INTERPRETER( string prj_file_path)
{
    try
    {
        //string exe_file_path = PROJECT_DIRECTORY_PATH + "\\" + "Bin\\Pcc.exe";
        string exe_file_path = INTERPRETER_DIRECTORY_PATH + INTERPRETER_EXE_FILE_NAME;

                        if (!File.Exists(exe_file_path))
                        {
                            //MessageBox.Show("Can't find interpreter output file: " + exe_file_path, "Compiler", MessageBoxButton.OK, MessageBoxImage.Error);
                            ERRORS.Add("Can't find interpreter exe-file: " + exe_file_path);
                            return null;
                        }
                        else if (!File.Exists(prj_file_path))
                        {
                            //MessageBox.Show("\r\nCan't find interpreter output file: " + prj_file_path, "Compiler", MessageBoxButton.OK, MessageBoxImage.Error);
                            ERRORS.Add("Can't find interpreter prj-file: " + prj_file_path);
                            return null;
                        }
                        else
                        {

                            ERRORS.Message("\r\nRun interpreter.");//: <" + exe_file_path + " " + prj_file_path + ">");
                            ERRORS.Message("\r\n------------------------------");
                            //---

                            ProcessStartInfo process_info = new ProcessStartInfo();
                            process_info.FileName = exe_file_path;
                            process_info.Arguments = prj_file_path;
                            process_info.WorkingDirectory = INTERPRETER_DIRECTORY_PATH;  //PROJECT_DIRECTORY_PATH;
                            process_info.UseShellExecute = false;
                            process_info.CreateNoWindow = true;
                            process_info.ErrorDialog = true;
                            process_info.RedirectStandardOutput = true;
                            process_info.RedirectStandardError = true;

                            string process_out = null, process_err = null;

                            using (Process proc = new Process())
                            {
                                proc.StartInfo = process_info;

                                // Start the process.
                                proc.Start();

                                // Attach to stdout and stderr.
                                using (StreamReader std_out = proc.StandardOutput)
                                {
                                    using (StreamReader std_err = proc.StandardError)
                                    {
                                        // Display the results.
                                        process_out = std_out.ReadToEnd();
                                        process_err = std_err.ReadToEnd();

                                        // Clean up.
                                        std_err.Close();
                                        std_out.Close();
                                        proc.Close();
                                    }
                                }
                            }

                            return process_out +"\r\n\r\n" + process_err;
                        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//*************  RUN_CPU_PROGRAMMER

static public string RUN_CPU_PROGRAMMER()
{
    try
    {
        string exe_file_path = PROGRAMMER_DIRECTORY_PATH + PROGRAMMER_EXE_FILE_NAME;
        
        if (!File.Exists(exe_file_path))
        {
            //MessageBox.Show("Can't find interpreter output file: " + exe_file_path, "Compiler", MessageBoxButton.OK, MessageBoxImage.Error);
            ERRORS.Message("Can't find file: " + exe_file_path);
            return null;
        }
        //else if (!File.Exists(prj_file_path))
        //{
            //MessageBox.Show("\r\nCan't find interpreter output file: " + prj_file_path, "Compiler", MessageBoxButton.OK, MessageBoxImage.Error);
            //ERRORS.Message("\r\nCan't find loading file: " + prj_file_path);
            //return null;
        //}
        else
        {

            ERRORS.Message("\r\nRun downloader.");//: <" + exe_file_path + ">");
            ERRORS.Message("\r\n------------------------------");
            //---

            ProcessStartInfo process_info = new ProcessStartInfo();
            process_info.FileName = exe_file_path;
            process_info.Arguments = null;
            process_info.WorkingDirectory = PROGRAMMER_DIRECTORY_PATH;
            process_info.UseShellExecute = false;
            process_info.CreateNoWindow = true;
            process_info.ErrorDialog = true;
            process_info.RedirectStandardOutput = true;
            process_info.RedirectStandardError = true;

            string process_out = null, process_err = null;

            using (Process proc = new Process())
            {
                proc.StartInfo = process_info;

                // Start the process.
                proc.Start();

                // Attach to stdout and stderr.
                using (StreamReader std_out = proc.StandardOutput)
                {
                    using (StreamReader std_err = proc.StandardError)
                    {
                        // Display the results.
                        process_out = std_out.ReadToEnd();
                        process_err = std_err.ReadToEnd();

                        // Clean up.
                        std_err.Close();
                        std_out.Close();
                        proc.Close();
                    }
                }
            }

            return process_out + "\r\n\r\n" + process_err;
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//***********************    HANDLERS   **************************************


        }


    }  //*************    END of Class <Step5>
}

