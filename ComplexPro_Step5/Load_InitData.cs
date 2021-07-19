


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

        //static Window PCC_INI_BaseSymbols_Window ;

        static string PCC_INI_Section_Content;

//*****************  ANSI TO OEM  &&  OEM TO ANSI

        static char ANSI_to_OEM(char ch)
        {
            if (ch >= (char)128 && ch <= (char)175) ch += (char)64;
            else if (ch >= (char)224 && ch <= (char)239) ch += (char)16;
            else if (ch == (char)252) ch = (char)185;

            return ch;
        }


        static char OEM_to_ANSI(char ch)
        {
            if (ch >= (char)240) ch -= (char)16;
            else if (ch >= (char)192) ch -= (char)64;
            else if (ch == (char)185) ch = (char)252;

            return ch;
        }


//*****************  Load_PCC_DATA_Window

static public void Load_PCC_DATA_Window(string loading_section)
    {

        StreamReader file_stream = null;

        try
        {

            //*************    OPEN FILE

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
            dlg.InitialDirectory = APPLICATION_DIRECTORY_PATH; // PCC_INI_FILE_PATH;
            dlg.FileName = PCC_INI_FILE_NAME; // Default file name
            dlg.DefaultExt = ".ini"; // Default file extension
            dlg.Filter = "Pcc (.ini)|*.ini"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
         

            // Process save file dialog box results
            if (result == true)
            {

                ERRORS.Clear();
                ERRORS.Message("\nLoading '" + PCC_INI_FILE_NAME + "'...");
                //-----------
                                                                //  для чтения Dos-кодировки русского текста.
                file_stream = new StreamReader(dlg.OpenFile(), Encoding.Default);

                
                PCC_INI_Section_Content = LOAD_PCC_Section(file_stream, loading_section);

                //SHOW_PCC_INI_window("ShowDialog");

                ERRORS.Message("\nLoading symbols...");

                switch (loading_section)
                {
                    case "[BaseVariable]":

                        //--- сохраняем прежний адрес Collection, т.к. он привязан к ItemsSource через которую мы делаем RefreshWindow.
                        SYMBOLS.BASE_TAGS_SYMBOLS_LIST.Clear();
                            
                        foreach( SYMBOLS.Symbol_Data symbol in GET_BASE_TAGS_SYMBOLS(PCC_INI_Section_Content))
                        {
                            symbol.Owner = SYMBOLS.BASE_TAGS_SYMBOLS_LIST;
                            SYMBOLS.BASE_TAGS_SYMBOLS_LIST.Add(symbol);
                        }

                        break;

                    case "[Nominal]":    //---  добавляем к списку по умолчанию загруженный список.

                        //--- сохраняем прежний адрес Collection, т.к. он привязан к ItemsSource через которую мы делаем RefreshWindow.
                        SYMBOLS.NOMS_SYMBOLS_LIST.Clear();

                        foreach (SYMBOLS.Symbol_Data symbol in new ObservableCollection<SYMBOLS.Symbol_Data>(
                                 SYMBOLS.SYMBOLS_NOMS_DEFAULT_LIST.Concat(GET_SYMBOLS_NOMS(PCC_INI_Section_Content))))
                        {
                            symbol.Owner = SYMBOLS.NOMS_SYMBOLS_LIST;
                            SYMBOLS.NOMS_SYMBOLS_LIST.Add(symbol);
                        }
                        break;
                }

             //----------

                if (ERRORS.Count != 0)
                {
                    ERRORS.Message("\nErrors: " + ERRORS.Count);
                    MessageBox.Show("Loading errors!");
                }
                else ERRORS.Message("\nNo errors.");

            }
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


//***************  LOAD PCC CONTENT


static public string LOAD_PCC_Section(StreamReader file_stream, string loading_section)
{

    try
    {
        StringBuilder pcc_content = null;
        StringBuilder pcc_content2 = null;

            string label;
            int first_pass = 1;

            for (label = "start"; !file_stream.EndOfStream; )
            {
                string str = file_stream.ReadLine();

                switch (label)
                {
                    case "start":

                        if (str.Contains(loading_section))
                        {
                            label = "Load";
                            pcc_content = new StringBuilder();
                        }
                        break;

                    case "Load":

                        //--- Началась следующая секция?
                        if (str.Contains("[")) label = "end";
                        else
                        {
                            if (first_pass == 0) pcc_content.Append("\n");

                            pcc_content.Append(str);

                            first_pass = 0;
                        }

                        break;

                        //**************  end of switch
                }

                if (label == "end") break;
            }

            //--- Заменяем Колины кавычки на стандартные.
            if (pcc_content != null)
            {
                pcc_content2 = new StringBuilder();

                foreach(char ch in pcc_content.ToString())
                {
                    if( ch == '“' || ch == '”') pcc_content2.Append( '\"' );
                    else pcc_content2.Append( ch );
                }

                return pcc_content2.ToString();
            }
            else
            {
                ERRORS.Add(loading_section + "-section not found.");

                return loading_section + "-section not found.";
            }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
    finally
    {
        if (file_stream != null) file_stream.Close();
    }

}



//***************  GET_BASE_SYMBOLS_TAGS


static public ObservableCollection<SYMBOLS.Symbol_Data> GET_BASE_TAGS_SYMBOLS(string section_content)
{

    try
    {
            string error = null;
            ObservableCollection<SYMBOLS.Symbol_Data> symbols_list = null;
            string name, data_type, memory_type, comment;

            string[] pcc_base_symbols_list;

            //--- разбиваем одну длинную строку на список строк.
            pcc_base_symbols_list = section_content.Split(new char[] { '\n' });

            if (pcc_base_symbols_list == null || pcc_base_symbols_list.Length == 0)
            {
                ERRORS.Add("Empty section: [BaseVariables].");

                return null;
            }
            //---

            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();

            for (int i = 0; i < pcc_base_symbols_list.Length; i++)
            {
                string str = pcc_base_symbols_list[i];
                
                error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

                        if (str.Contains("\""))
                        {
                            error = GetTagValues(str, out name, out data_type, out memory_type, out comment);
//*******  ADD_SYMBOL
                            symbols_list.Add(new SYMBOLS.Symbol_Data(symbols_list, name, null, data_type, memory_type, null, null, null, null, comment));
                        }

                if (error != null) ERRORS.Add(error + "  Section: [BaseVariables]. Line: " + (i+1));
            }

            //---  Конец списка. 

            if (symbols_list == null || symbols_list.Count == 0)  ERRORS.Add("Empty section: [BaseVariables].");

            return symbols_list;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}
    
//**************   GET TAG from string

static public string GetTagValues ( string str, out string name, out string data_type, out string memory_type, out string comment)
    {
        name = null;
        data_type = null;
        memory_type = null;
        comment = null;

        try
        {

            int start_index=0, end_index=0 ;

            //---

//*** Поиск имени переменной в кавычках.
//********  Поиск первой и второй кавычек
            start_index = str.IndexOf("\"");
            end_index = str.IndexOf("\"", start_index + 1);

//********  Есть ли вторая кавычка и зазор между кавычками чтобы влезла хотя бы одна буква
            if (end_index <= start_index + 1) return "No second '\"'-symbol.";
            name = str.Substring(start_index+1, end_index - start_index - 1);

//*** Поиск типа данных переменной после имени между двумя запятыми.
//********  Поиск запятой после второй кавычки
            start_index = end_index;
            end_index = str.IndexOf(",", start_index+1);
            //---  есть ли вторая кавычка - зазор не нужен
            if (end_index < start_index + 1) return "No ','-symbol after second '\"'-symbol.";

//********  Есть ли запятая и зазор между нею и предыдущей запятой чтобы влезла хотя бы одна буква
            start_index = end_index;
            end_index = str.IndexOf(",", start_index + 1);
            if (end_index <= start_index + 1) return "No second ','-symbol for data-type reading.";

//********  Есть ли какой-либо из известных типов данных в строке между запятой и точкой с запятой.
            foreach (string data_str in SYMBOLS.EQUAL_DATA_TYPES_REVERSED.Keys)
            {   //  так не подходит, т.к. оно находит byte вместо sbyte, word вместо sword и т.д.
                //int index = str.IndexOf(data_str, start_index + 1);
                int index = DiagramCompiler.Index_Of(str, data_str, start_index + 1);
                if ( index > start_index && index < end_index)
                {
                    data_type = SYMBOLS.EQUAL_DATA_TYPES_REVERSED[data_str];
                    break;
                }
            }

            if (data_type == null)
            {
                return "No Tag data type.";
            }

//*** Поиск типа доступа переменной после типа переменной между запятой и заключающей точкой-запятой.
//********  Есть ли точка с запятой и зазор между нею и предыдущей запятой чтобы влезла хотя бы одна буква
            start_index = end_index;
            end_index = str.IndexOf(";", start_index + 1);
            if (end_index <= start_index + 1) return "No ';'-symbol after ','-symbol for memory-type reading.";

            //********  Есть ли какой-либо из известных типов памяти в строке между запятой и точкой с запятой.
            foreach (string data_str in SYMBOLS.EQUAL_MEMORY_TYPES_REVERSED.Keys)
            {   //  так не подходит, т.к. оно находит byte вместо sbyte, word вместо sword и т.д.
                //int index = str.IndexOf(data_str, start_index + 1);
                int index = DiagramCompiler.Index_Of(str, data_str, start_index + 1);
                if (index > start_index && index < end_index)
                {
                    memory_type = SYMBOLS.EQUAL_MEMORY_TYPES_REVERSED[data_str];
                    break;
                }
            }

            if (data_type == null)
            {
                return "No Tag memory type.";
            }



//*** Поиск комментария
            start_index = end_index;
            end_index = str.IndexOf("//", start_index + 1);
            if (end_index <= start_index + 1) comment = "";//return "No Tag comment";
            else comment = str.Substring(end_index + 2);
            //---


            return null;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return "GetTagValues exception.";
        }
    }


//***************  GET_SYMBOLS_NOMS


static public ObservableCollection<SYMBOLS.Symbol_Data> GET_SYMBOLS_NOMS(string section_content)
{

    try
    {
            string error = null;
            ObservableCollection<SYMBOLS.Symbol_Data> symbols_list = null;
            string name, value, comment;

            string[] pcc_symbols_list;

            //--- разбиваем одну длинную строку на список строк.
            pcc_symbols_list = section_content.Split(new char[] { '\n' });

            if (pcc_symbols_list == null || pcc_symbols_list.Length == 0)
            {
                ERRORS.Add("Empty section: [Nominal].");

                return null;
            }
            //---

            symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();

            for (int i = 0; i < pcc_symbols_list.Length; i++)
            {
                string str = pcc_symbols_list[i];
                
                error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

                        if (str.Contains("\""))
                        {
                            error = GetNomValues ( str, out name, out value, out comment);
//*******  ADD_SYMBOL
                            int ax;
                            if (value == null) ax = 1;
                            else if (value == "") ax = 1;
                            else if (int.TryParse((string)value, out ax)) {}
                            else ax = 1;

                            symbols_list.Add(new SYMBOLS.Symbol_Data(symbols_list, name, null, "INT", null, null, null, null, ax.ToString(), comment));
                        }

                if (error != null) ERRORS.Add(error + "  Section: [Nominal]. Line: " + (i+1));
            }

            //---  Конец списка. 

            if (symbols_list == null || symbols_list.Count == 0)  ERRORS.Add("Empty section: [Nominal].");

            return symbols_list;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}
    
//**************   GET NOM from string

static public string GetNomValues ( string str, out string name, out string value, out string comment)
    {
        name = null;
        value = null;
        comment = null;

        try
        {

            int start_index=0, end_index=0 ;

            //---

//********  Поиск первой и второй кавычек
            start_index = str.IndexOf("\"");
            end_index = str.IndexOf("\"", start_index + 1);

//********  Есть ли вторая кавычка и зазор между кавычками чтобы влезла хотя бы одна буква
            if (end_index <= start_index + 1) return "No second '\"'-symbol.";
            name = str.Substring(start_index+1, end_index - start_index - 1);

//********  Поиск '=' после второй кавычки
            start_index = end_index;
            end_index = str.IndexOf("=", start_index+1);
            //---  есть ли вторая кавычка - зазор не нужен
            if (end_index < start_index + 1) return "No '='-symbol.";

//********  Есть ли запятая и зазор между нею и предыдущим '=' чтобы влезла хотя бы одна буква
            start_index = end_index;
            end_index = str.IndexOf(",", start_index + 1);
            if (end_index <= start_index + 1) return "No ','-symbol.";

            value = str.Substring(start_index + 1, end_index - (start_index + 1));

//********  Поиск комментария
            start_index = end_index;
            end_index = str.IndexOf("//", start_index + 1);
            if (end_index <= start_index + 1) comment = "";//return "No Nom comment";
            else comment = str.Substring(end_index + 2);
            //---

            return null;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return "GetNomValues exception.";
        }
    }

//**************

//      Обновление списка базовых функций:
//  -   выборка прототипов функций из файла связки базовых функций между ПО КВВ-ОНА-ТСА <Step5_CPU_lib.c>
//  -   замена списка базовых функций в <Pcc.ini>
//  -   замена списка базовых функций в <Step5_CPU_lib.h>

//*****************  UpgradePccIniBaseFunctions_PCC_DATA

static public string UpgradePccIniBaseFunctions_Window()
{

    StreamReader file_stream = null;

    try
    {

        List<string> base_functions_raw_list = null;

//*********  OPEN CPU BASE FUNCTIONS FILE

        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
        dlg.InitialDirectory = APPLICATION_DIRECTORY_PATH;
        dlg.FileName = CPU_LIB_C_FILE_NAME; // Default file name
        dlg.DefaultExt = ".c"; // Default file extension
        dlg.Filter = "*(.c)|*.c"; // Filter files by extension

        // Show save file dialog box
        Nullable<bool> result = dlg.ShowDialog();

        // Process save file dialog box results
        if (result == true)
        {

            ERRORS.Clear();
            ERRORS.Message("\nLoading '" + dlg.FileName + "'...");

            //-----------
            //  для чтения Dos-кодировки русского текста.
            file_stream = new StreamReader(dlg.OpenFile(), Encoding.Default);

//*********  LOADING FUNCTIONS LIST

            ERRORS.Message("\nLoading functions...");

            LOAD_FUNCTION_PROTOTYPES( file_stream, ref base_functions_raw_list );

            //----------
            if (ERRORS.Count != 0)
            {
                ERRORS.Message("\nErrors: " + ERRORS.Count);
                MessageBox.Show(ERRORS.ToString(), "Errors!");
                return ERRORS.ToString();
            }
            else ERRORS.Message("\nNo errors.");

            StringBuilder sstr = new StringBuilder();
            foreach (string str in base_functions_raw_list) sstr.Append(str + "\n");

//*********  SHOW FOUND FUNCTIONS
            MessageBox.Show(sstr.ToString(), "Found Base Functions");

//*********  REPLACE FUNCTIONS IN "PCC.INI"

            if (MessageBox.Show("Replace <BaseFunction>-section in \"Pcc.ini\"?", "Replacing \"Pcc.ini\"", 
                  MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No) == MessageBoxResult.Yes)
            {

                //*********  OPEN "PCC.INI" FILE

                dlg = new Microsoft.Win32.OpenFileDialog();
                //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
                dlg.InitialDirectory = APPLICATION_DIRECTORY_PATH; // PCC_INI_FILE_PATH;
                dlg.FileName = PCC_INI_FILE_NAME; // Default file name
                dlg.DefaultExt = ".ini"; // Default file extension
                dlg.Filter = "Pcc (.ini)|*.ini"; // Filter files by extension

                // Show save file dialog box
                result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {

                    ERRORS.Message("\nUpgrading '" + dlg.FileName + "'...");

                    REPLACE_BASE_FUNCTIONS_IN_PCC_INI_FILE(dlg, base_functions_raw_list);

                    //----------
                    if (ERRORS.Count != 0)
                    {
                        ERRORS.Message("\nErrors: " + ERRORS.Count);
                        MessageBox.Show(ERRORS.ToString(), "Errors!");
                        //return ERRORS.ToString();
                    }
                    else ERRORS.Message("\nNo errors.");

                }

            }

//*********  REPLACE FUNCTIONS IN "CPU_LIB_H_FILE_NAME"
            
            if (MessageBox.Show("Replace <BaseFunction>-section in \""+ CPU_LIB_H_FILE_NAME + "\"?", "Replacing \"Pcc.ini\"", 
                  MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                ERRORS.Clear();

                //*********  OPEN "PCC.INI" FILE

                dlg = new Microsoft.Win32.OpenFileDialog();
                //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
                dlg.InitialDirectory = APPLICATION_DIRECTORY_PATH;
                dlg.FileName = CPU_LIB_H_FILE_NAME; // Default file name
                dlg.DefaultExt = ".h"; // Default file extension
                dlg.Filter = "*(.h)|*.h"; // Filter files by extension

                // Show save file dialog box
                result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {

                    ERRORS.Message("\nUpgrading '" + dlg.FileName + "'...");

                    REPLACE_BASE_FUNCTIONS_IN_CPU_LIB_H_FILE(dlg, base_functions_raw_list);

                    //----------
                    if (ERRORS.Count != 0)
                    {
                        ERRORS.Message("\nErrors: " + ERRORS.Count);
                        MessageBox.Show(ERRORS.ToString(), "Errors!");
                        //return ERRORS.ToString();
                    }
                    else ERRORS.Message("\nNo errors.");

                }

            }

        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    finally
    {
        if (file_stream != null) file_stream.Close();
    }

    return null;
}


//**************   LOAD_FUNCTION_PROTOTYPES


static public void LOAD_FUNCTION_PROTOTYPES( StreamReader file_stream, ref List<string> symbols_list)
{

    try
    {
        string error = null;
        string text;
        symbols_list = new List<string>();
        
        int line_count = 1; // для отображения при ошибке.
        string label;
        for (label = "<BaseFunctionPrototype"; !file_stream.EndOfStream; line_count++)
        {
            string str = file_stream.ReadLine();
            string value;
            error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

            switch (label)
            {

                //***************  LOAD GLOBAL SYMBOLS

                case "<BaseFunctionPrototype":

                    if (str.Contains(label))
                    {
                        value = _GetValue(label, " Text=\"", str);
                        if (value != null) { text = value; }
                        else { error = label + " Text" + ": No Value."; break; }
                        symbols_list.Add(value);
                    }

                    break;

                //**************  end of switch
            }

            if (error != null) ERRORS.Add(error + "   Line: " + line_count);
        }

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


//**************   REPLACE_BASE_FUNCTIONS_IN_PCC_INI_FILE
//   Образец того как должны быть оформлены функции для интерпретатора Коли в PCC_INI_FILE:

//[BaseFunction]
//  BCD_DI( 0, lword IN1, slword OUT, word ENO );
//  BCD_I( 0, word IN1, sword OUT, word ENO );
//  DI_BCD( 0, slword IN1, lword OUT, word ENO );


static public void  REPLACE_BASE_FUNCTIONS_IN_PCC_INI_FILE( Microsoft.Win32.OpenFileDialog dlg, List<string> functions_list)
{
    StreamReader file_streamreader = null;
    StreamWriter file_streamwriter = null;

    try
    {
        //-----------
        //  сохраняем копию файла.
        File.Copy(dlg.FileName, dlg.FileName+"_bak", true);

        //  И в читаемом файле и в записываемом надо делать одинаковую кодировку иначе в записываемый идет мусор после второго круга записи
        file_streamreader = new StreamReader(dlg.OpenFile(), Encoding.Default);  //  для чтения Dos-кодировки русского текста.
        
        // переписываем старый файл в новый за исключением секции "[BaseFunctions]" - ее заменяем на новый список функций.
        file_streamwriter = new StreamWriter("Temp.txt", false, Encoding.Default);

        int line_count = 1; // для отображения при ошибке.

        int tst = 0;
        for (; !file_streamreader.EndOfStream; line_count++)
        {
            string str = file_streamreader.ReadLine();
            file_streamwriter.WriteLine(str);
 
            if (str.Contains("[BaseFunction]"))
            {
                tst = 1;
                foreach(string fstr in functions_list)
                {
                    if (fstr.Contains("("))
                    {
                        var index1 = fstr.IndexOf(" ");
                        var index2 = fstr.IndexOf("(");

                        //  Вырезаем в прототипе функции все идущее спереди перед именем функции (void, int ...)
                        if (index2 > index1) file_streamwriter.WriteLine(fstr.Remove(0, index1).TrimStart());
                        else file_streamwriter.WriteLine(fstr);
                    }
                }

                break;
            }
        }

        if ( tst == 1 )
        {
            //  Ищем в старом файле начало следующей секции после "[BaseFunctions]"
            for (; !file_streamreader.EndOfStream; line_count++)
            {
                string str = file_streamreader.ReadLine();
                if (str.Contains("["))
                {
                    file_streamwriter.WriteLine(str);
                    break;
                }
            }

            //  Дописываем в новый файл все из старого после секции "[BaseFunctions]"
            for (; !file_streamreader.EndOfStream; ) file_streamwriter.WriteLine(file_streamreader.ReadLine());

            //  Закрываем файлы иначе ОС не дает их удалить
            if (file_streamreader != null) file_streamreader.Close();
            if (file_streamwriter != null) file_streamwriter.Close();

            //  удаляем старый Pcc.ini
            File.Delete(dlg.FileName);

            File.Copy("Temp.txt", dlg.FileName);

            File.Delete("Temp.txt");

        }
        else ERRORS.Add("Can't find [BaseFunction]-section");
        //------

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    finally
    {
        if (file_streamreader != null) file_streamreader.Close();
        if (file_streamwriter != null) file_streamwriter.Close();
    }

}


//**************   REPLACE_BASE_FUNCTIONS_IN_CPU_LIB_H_FILE
//   Образец того как должны быть оформлены функции для контроллера в CPU_LIB_H_FILE:

////<BaseFunctions>
// word iSYS_TIME( union Operand *Operands);
// word SYS_TIME( union Operand *Operands );
// word S_PULSE( union Operand *Operands );
// word SP( union Operand *Operands );
// word RES_T( union Operand *Operands );
// word ADD_I( union Operand *Operands );

//#define ExtFunctions   iSYS_TIME,\
//                       SYS_TIME,\
//                       S_PULSE,\
//                       SP,\
//                       RES_T,\
//                       ADD_I

//#define ExtFunctNames  {"iSYS_TIME", 0xffff},\
//                       {"SYS_TIME" , 0xffff},\
//                       {"S_PULSE"  , 0xffff},\
//                       {"SP"       , 0xffff},\
//                       {"RES_T"    , 0xffff},\
//                       {"ADD_I"    , 0xffff}
////</BaseFunctions>



static public void REPLACE_BASE_FUNCTIONS_IN_CPU_LIB_H_FILE(Microsoft.Win32.OpenFileDialog dlg, List<string> functions_list)
{
    StreamReader file_streamreader = null;
    StreamWriter file_streamwriter = null;

    try
    {
        //-----------
        //  сохраняем копию файла.
        File.Copy(dlg.FileName, dlg.FileName + "_bak", true);

        //  И в читаемом файле и в записываемом надо делать одинаковую кодировку иначе в записываемый идет мусор после второго круга записи
        file_streamreader = new StreamReader(dlg.OpenFile(), Encoding.Default);  //  для чтения Dos-кодировки русского текста.

        // переписываем старый файл в новый за исключением секции "[BaseFunctions]" - ее заменяем на новый список функций.
        file_streamwriter = new StreamWriter("Temp.txt", false, Encoding.Default);

        int line_count = 1; // для отображения при ошибке.

        int tst = 0;
        for (; !file_streamreader.EndOfStream; line_count++)
        {
            string str = file_streamreader.ReadLine();
            file_streamwriter.WriteLine(str);

            if (str.Contains("<BaseFunctions>"))
            {
                tst = 1;
//----  Делаем список чистых имен функций.

                List<string> f_list = new List<string>();

                for (int i = 0; i < functions_list.Count; i++)
                {
                    var fstr = functions_list[i];
                    if (fstr.Contains("("))
                    {
                        var index1 = fstr.IndexOf(" ");
                        var index2 = fstr.IndexOf("(");

                        //  Вырезаем в прототипе функции все идущее перед и после имени функции 
                        fstr = fstr.Remove(index2);
                        if (index2 > index1) fstr = fstr.Remove(0, index1);
                        fstr = fstr.Trim();

                        f_list.Add(fstr);
                    }
                }


//----   Первая вставка
// word iSYS_TIME( union Operand *Operands);
// word SYS_TIME( union Operand *Operands );

                file_streamwriter.WriteLine();

                for (int i = 0; i < f_list.Count; i++ )
                {
                    var fstr = f_list[i];
                        fstr = fstr.Insert(0, "word  ");
                        fstr = fstr.Insert(fstr.Length, "( union Operand *Operands );");
                        file_streamwriter.WriteLine(fstr);
                }


//----   Вторая вставка
//#define ExtFunctions   \
//                       iSYS_TIME,\

                file_streamwriter.WriteLine("\n\n#define ExtFunctions   \\");

                for (int i = 0; i < f_list.Count; i++)
                {
                    var fstr = f_list[i];
                        fstr = fstr.Insert(0, "\t");
                        //  добавляем в конец оформление - кроме последней строки
                        if (i != f_list.Count - 1) fstr = fstr.Insert(fstr.Length, ",\t\\");
                        file_streamwriter.WriteLine(fstr);
                }


//----   Третья вставка
//#define ExtFunctNames  \
//                       {"iSYS_TIME", 0xffff},\

                file_streamwriter.WriteLine("\n");
                file_streamwriter.WriteLine("#define ExtFunctNames  \\");

                for (int i = 0; i < f_list.Count; i++)
                {
                    var fstr = f_list[i];
                        fstr = fstr.Insert(0, "\t{\"");
                        fstr = fstr.Insert(fstr.Length, "\", 0xffff}");
                        //  добавляем в конец оформление - кроме последней строки
                        if (i != f_list.Count - 1) fstr = fstr.Insert(fstr.Length, ",\t\\");
                        file_streamwriter.WriteLine(fstr);
                }


                break;
            }
        }

//-----  Дозапись остального файла
        
        if ( tst == 1 )
        {
            //  Ищем в старом файле начало следующей секции после "[BaseFunctions]"
            tst = 0;
            for (; !file_streamreader.EndOfStream; line_count++)
            {
                string str = file_streamreader.ReadLine();
                if (str.Contains("</BaseFunctions>"))
                {
                    tst = 1;
                    file_streamwriter.WriteLine("\n" + str);
                    break;
                }
            }

            if ( tst == 1 )
            {
                //  Дописываем в новый файл все из старого после секции "[BaseFunctions]"
                for (; !file_streamreader.EndOfStream; ) file_streamwriter.WriteLine(file_streamreader.ReadLine());

                //  Закрываем файлы иначе ОС не дает их удалить
                if (file_streamreader != null) file_streamreader.Close();
                if (file_streamwriter != null) file_streamwriter.Close();

                //  удаляем старый Pcc.ini
                File.Delete(dlg.FileName);

                File.Copy("Temp.txt", dlg.FileName);

                File.Delete("Temp.txt");
            }
            else ERRORS.Add("Can't find end of </BaseFunctions>-section");
        }
        else ERRORS.Add("Can't find <BaseFunctions>-section");
        //------

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    finally
    {
        if (file_streamreader != null) file_streamreader.Close();
        if (file_streamwriter != null) file_streamwriter.Close();
    }

}

/*
public static void SHOW_PCC_INI_window(string code) //, Diagram_Of_Networks)
{

    try
    {

        PCC_INI_BaseSymbols_Window = new Window();
        PCC_INI_BaseSymbols_Window.Title = "Pcc.ini";
        PCC_INI_BaseSymbols_Window.SizeToContent = SizeToContent.WidthAndHeight;
        PCC_INI_BaseSymbols_Window.Width = 500;
        PCC_INI_BaseSymbols_Window.Height = 300;
        PCC_INI_BaseSymbols_Window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //PCC_INI_window.Closing += STORABLE_SYMBOLS_LIST_window_Closing;

        PCC_INI_BaseSymbols_Window.Content = SHOW_PCC_INI();

        if (code == "Show") PCC_INI_BaseSymbols_Window.Show();
        else if (code == "ShowDialog") PCC_INI_BaseSymbols_Window.ShowDialog();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}

//***************     

static public void RESHOW_PCC_INI_window()
{

    try
    {
        PCC_INI_BaseSymbols_Window.Content = SHOW_PCC_INI();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}


//***************   SHOW_PCC_INI

public static Border SHOW_PCC_INI()
{

    try
    {
        
                Border xborder = new Border();
                xborder.Background = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderThickness = new Thickness(0.5);
                xborder.HorizontalAlignment = HorizontalAlignment.Center;
                xborder.VerticalAlignment = VerticalAlignment.Center;
                xborder.Margin = new Thickness(10);

                    Grid grid = new Grid();
                    //grid.Margin = new Thickness(10);
                    grid.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.VerticalAlignment = VerticalAlignment.Center;

                    ColumnDefinition column0 = new ColumnDefinition();
                    ColumnDefinition column1 = new ColumnDefinition();
                    column0.Width = GridLength.Auto;
                    column1.Width = GridLength.Auto;
                    grid.ColumnDefinitions.Add(column0);
                    grid.ColumnDefinitions.Add(column1);
                    

//****************   TEXTBOX

                        TextBox text_box = new TextBox();
                        text_box.Text = PCC_INI_BaseSymbols_Content;
                        text_box.SetValue(Grid.ColumnProperty, 0);

//****************   BUTTONS  **************************

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Top;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                            Button button_Ok = SYMBOLS.Get_Button("Ok", Ok_button_Click);
                             
                        // иначе по cancel окно закрывается безусловно  button_Cancel.IsCancel = true;
                        FocusManager.SetFocusedElement(PCC_INI_BaseSymbols_Window, button_Ok);    
                                
                        stackpanel.Children.Add(button_Ok);
                        
                    grid.Children.Add(text_box);
                    grid.Children.Add(stackpanel);

                xborder.Child = grid;

        return xborder;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return null;

}


static void Ok_button_Click(object sender, RoutedEventArgs e)
{
    try
    {
        PCC_INI_BaseSymbols_Window.Close();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

*/
    }   //*************    END of Class

    }  //*************    END of Class <Step5>
}

