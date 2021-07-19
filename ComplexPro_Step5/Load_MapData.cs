
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


        static partial class SaveProject
        {

            //*****************  Load_PCC_DATA_Window

            static public void Load_MAP_DATA_Window()
            {

                StreamReader file_stream = null;

                try
                {
                    //*************    OPEN FILE

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    //  Из полного пути с именем файла оставляем только путь, а имя удаляем.
                    dlg.InitialDirectory = PROJECT_MAP_DIRECTORY_PATH; // PCC_INI_FILE_PATH;
                    dlg.FileName = PROJECT_MAP_FILE_PATH; // Default file name
                    dlg.DefaultExt = PROJECT_MAP_FILE_EXT; // Default file extension
                    dlg.Filter = "Map ("+PROJECT_MAP_FILE_EXT+")|*"+PROJECT_MAP_FILE_EXT; // Filter files by extension

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        ERRORS.Clear();
                        ERRORS.Message("\nLoading '" + PROJECT_MAP_FILE_PATH + "'...");
                        //-----------
                        //  для чтения Dos-кодировки русского текста.
                        file_stream = new StreamReader(dlg.OpenFile(), Encoding.Default);

                        string error = LOAD_MAP_DATA(file_stream);
                        if (error != null) ERRORS.Add(error);
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


            //***************  LOAD MAP CONTENT

//  Загрузка адресов переменных из MAP-файла Оноприенко в свойства <Address> глобальных символов,
//      коннект переменных, и локальных переменных функций.
            static string LOAD_MAP_DATA(StreamReader file_stream)
            {
                string error = null;
                try
                {
                    //  загрузка всего файла в массив.
                    List<string> fileContent = new List<string>();
                    while (!file_stream.EndOfStream) fileContent.Add(file_stream.ReadLine());
                    //  загрузка секции в массив.
                    List<string> sectionContent;
                    if( (error=getMapSectionContent(fileContent, "[Global]", "[GlobalEnd]", out sectionContent)) == null)
                    {
                        // загрузка адресов переменных глобальных символов
                        getMapAdresses(sectionContent, SYMBOLS.GLOBAL_SYMBOLS_LIST);
                        return null;
                    }
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString()); error = "Exception";
                }
                finally {}
                return error;
            }

//  Загрузка заданной секции в массив

            static string getMapSectionContent(List<string> sectionIn, string startTag, string endTag, out List<string> list)
            {
                list = null;
                string error = null;
                try
                {
                    List<string> content = new List<string>();

                    for (int i = 0 ; i < sectionIn.Count ; i++)
                    {
                        if (sectionIn[i].Contains(startTag))
                        {
                            for (; i < sectionIn.Count; i++)
                            {
                                content.Add(sectionIn[i]);
                                if (sectionIn[i].Contains(endTag)) { list = content; return null; }
                            }
                            error = "Section tag not found: <" + endTag + ">";
                        }
                    }
                    error = "Section tag not found: <" + startTag + ">";
                }
                catch (Exception excp)  {  MessageBox.Show(excp.ToString());  error = "Exception";  }
                finally { }
                return error;
            }


            
//******  Перебор всех глобальных символов и поиск их адресов в считанном фрагменте Global-секции Map-файла

            static string getMapAdresses(List<string> sectionContent, ObservableCollection<SYMBOLS.Symbol_Data> symbolsList)
            {
                try
                {
                    string error = null;
                    string address;

                    //  перебор всех символов в заданном списке символов
                    foreach( SYMBOLS.Symbol_Data symbol in symbolsList)
                    {
                        if( symbol.Data_Type != "BOOL")
                        {
                            //  перебор всех строк секции в поиске искомого имени символа
                            for (int i = 0; i < sectionContent.Count; i++)
                            {
                                string str = sectionContent[i];
                                //  проверка строки в поиске искомого имени символа
                                if ((error = getAddressByName(str, symbol.Name, out address)) == null)
                                {
                                    int ax;
                                    //  TryParse(..., NumberStyles.HexNumber...) не работает с 0x...
                                    address = address.Replace('x', '0'); address = address.Replace('X', '0');
                                    if (int.TryParse(address, NumberStyles.HexNumber, null ,out ax)) symbol.Address = ax;
                                }
                            }
                        }
                    }
                    return null;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return "Exception"; }
                finally { }

            }

//******  Считывание из строки адреса по заданному имени

            static public string getAddressByName(string str, string name, out string address)
            {
                address = null;

                try
                {
                    //*** Поиск имени переменной.
                    //********  Поиск первой и второй кавычек
                    if (str.IndexOf("Name") >= 0)
                    {   //  разбиваем строку н асписок лексем разделенных пробелом
                        List<string> list = str.Split(new char[]{' '}).ToList();
                        //  выбрасываем из списка строки с повторяющимися пробелами
                        list.RemoveAll((value) => value == "" );

                        int i = 0;
                        if (list[i++] == "Name" && list[i++] == "=" && list[i++] == name)
                        {
                            if (list[i++] == "Addr" && list[i++] == "=")
                            {
                                address = list[i];
                                return null;
                            }
                        }
                    }
                    return "Name or address not found: <" + name + ">";
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return "Exception"; }
                finally { }
            }


            ////***************  GET_SYMBOLS_NOMS


            //static public ObservableCollection<SYMBOLS.Symbol_Data> GET_SYMBOLS_NOMS(string section_content)
            //{

            //    try
            //    {
            //        string error = null;
            //        ObservableCollection<SYMBOLS.Symbol_Data> symbols_list = null;
            //        string name, value, comment;

            //        string[] pcc_symbols_list;

            //        //--- разбиваем одну длинную строку на список строк.
            //        pcc_symbols_list = section_content.Split(new char[] { '\n' });

            //        if (pcc_symbols_list == null || pcc_symbols_list.Length == 0)
            //        {
            //            ERRORS.Add("Empty section: [Nominal].");

            //            return null;
            //        }
            //        //---

            //        symbols_list = new ObservableCollection<SYMBOLS.Symbol_Data>();

            //        for (int i = 0; i < pcc_symbols_list.Length; i++)
            //        {
            //            string str = pcc_symbols_list[i];

            //            error = null; //  при ошибке загрузку не прерываем и на каждом проходе выводим свою ошибку.

            //            if (str.Contains("\""))
            //            {
            //                error = GetNomValues(str, out name, out value, out comment);
            //                //*******  ADD_SYMBOL
            //                int ax;
            //                if (value == null) ax = 1;
            //                else if (value == "") ax = 1;
            //                else if (int.TryParse((string)value, out ax)) { }
            //                else ax = 1;

            //                symbols_list.Add(new SYMBOLS.Symbol_Data(symbols_list, name, null, "INT", null, null, null, null, ax.ToString(), comment));
            //            }

            //            if (error != null) ERRORS.Add(error + "  Section: [Nominal]. Line: " + (i + 1));
            //        }

            //        //---  Конец списка. 

            //        if (symbols_list == null || symbols_list.Count == 0) ERRORS.Add("Empty section: [Nominal].");

            //        return symbols_list;

            //    }
            //    catch (Exception excp)
            //    {
            //        MessageBox.Show(excp.ToString());
            //        return null;
            //    }

            //}

            ////**************   GET NOM from string

            //static public string GetNomValues(string str, out string name, out string value, out string comment)
            //{
            //    name = null;
            //    value = null;
            //    comment = null;

            //    try
            //    {

            //        int start_index = 0, end_index = 0;

            //        //---

            //        //********  Поиск первой и второй кавычек
            //        start_index = str.IndexOf("\"");
            //        end_index = str.IndexOf("\"", start_index + 1);

            //        //********  Есть ли вторая кавычка и зазор между кавычками чтобы влезла хотя бы одна буква
            //        if (end_index <= start_index + 1) return "No second '\"'-symbol.";
            //        name = str.Substring(start_index + 1, end_index - start_index - 1);

            //        //********  Поиск '=' после второй кавычки
            //        start_index = end_index;
            //        end_index = str.IndexOf("=", start_index + 1);
            //        //---  есть ли вторая кавычка - зазор не нужен
            //        if (end_index < start_index + 1) return "No '='-symbol.";

            //        //********  Есть ли запятая и зазор между нею и предыдущим '=' чтобы влезла хотя бы одна буква
            //        start_index = end_index;
            //        end_index = str.IndexOf(",", start_index + 1);
            //        if (end_index <= start_index + 1) return "No ','-symbol.";

            //        value = str.Substring(start_index + 1, end_index - (start_index + 1));

            //        //********  Поиск комментария
            //        start_index = end_index;
            //        end_index = str.IndexOf("//", start_index + 1);
            //        if (end_index <= start_index + 1) comment = "";//return "No Nom comment";
            //        else comment = str.Substring(end_index + 2);
            //        //---

            //        return null;
            //    }
            //    catch (Exception excp)
            //    {
            //        MessageBox.Show(excp.ToString());
            //        return "GetNomValues exception.";
            //    }
            //}

            //**************


        }   //*************    END of Class

    }  //*************    END of Class <Step5>
}

