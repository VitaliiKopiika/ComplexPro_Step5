//  Придумать начальные значения для переменных и для кнопки "ADD New"
// сделать удаления добавления вставки по кнопкам клавиатуры delete insert 
// сделать  для ячеек автоматический ReadOnly

//  доразобраться и сделать универсальными public void RESHOW_GLOBAL_SYMBOLS_LIST_window()

// + сделать распечатку табл. локальных символов в XPS
// + сделать блокировку коррекции в gridtab параметров базовых переменных.
// + сделать контроль чтения записи базовых переменных
// + сделать Validation для новых типов в сочетании с типами памяти
// + переделать сохранение загрузку переменных под новые поля DATA_TYPE MEMORY_TYPE
// + сделать проверку типов при выборе типа из списка символов

// + список переменных для Коли: 
// +                             - переменные-коннекторы
// +                             - временные переменные w_ax, r_ax

// + сделать выбор номера бита через выпадающий список ComboBox
// + доделать convertback для initial value чтобы задавать null
// + сделать конвертер для bit_number чтобы задавать его null
// + сделать validation для initial value максимальной величины в зависимости от типа

//+ убрать выскакивание дважды сообщения об ошибке ввода.
//+ сделать ограничения через Validation
//+ дочитать про Validation
//+ сделать ограничения для остальных через конвертер
//+ сделать кнопки удаления добавления вставки 
//+ сделать обновление лкна после вставки удаления
//+ добавление строки 
//+ вставка строки
//+ колонка с номерами строк

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

        //                Иерархия данных.
        //
        //Массив Главный [ Networks ]
        //				Массив Network [ Lines ]
        //								Массив Line [ Elements ]
        //											Массив Element_Info [ Name, Code, ...]
        //                                                                Массив Names[ BlocksCollection ]
        //

        //********    GLOBAL SYMBOLS

        public partial class SYMBOLS : DependencyObject
        {

            //---  Для <DependencyObject> свойства нужно делать <DependencyProperty>
            //---  иначе не работает автообновление текста на экране привязанных свойст.
            public string SYMBOLS_VALIDATION_ERRORS
            {
                get { return (string)GetValue(SYMBOLS_VALIDATION_ERRORSProperty); }
                set { SetValue(SYMBOLS_VALIDATION_ERRORSProperty, value); }
            }
            public static readonly DependencyProperty SYMBOLS_VALIDATION_ERRORSProperty =
                                     DependencyProperty.Register("SYMBOLS_VALIDATION_ERRORS", typeof(string), typeof(SYMBOLS));

//****** Data Types
//  1 Bit 	 1 Byte     1 Word      2 Words     
//          (8 Bits)	(2 Bytes)	(4 Bytes)
//                      (16 Bits)   (32 Bits)
//  BOOL	 BYTE	    WORD	    DWORD
//           CHAR 		
//                      INT	        DINT
//                      DATE	
//                      S5TIME	
//                                  REAL
//                                  TIME
//                                  TOD

            static public List<string> GLOBAL_DATA_TYPES = new List<string>() {    "BOOL",
                                                                            "BYTE", "CHAR", 
                                                                            "WORD", "INT", "DATE", "S5TIME",
                                                                            "DWORD","DINT",
                                                                            "REAL", "DREAL",
                                                                            "TIME", "TOD",
                                                                            "TIMER", "COUNTER"
                                                                       };

            static public List<string> BASE_DATA_TYPES = new List<string>() 
                                                                       {    "BOOL",
                                                                            "BYTE", "CHAR", 
                                                                            "WORD", "INT",
                                                                            "DWORD","DINT",
                                                                            "TIME",
                                                                            "REAL", "DREAL", ""
                                                                       };

            static public List<string> STORABLE_DATA_TYPES = new List<string>() 
                                                                       {    "BOOL",
                                                                            "BYTE", //"CHAR", 
                                                                            "WORD", "INT",
                                                                            "DWORD","DINT",
                                                                            "TIME",
                                                                            "REAL", "DREAL"
                                                                       };

            static public List<string> MSG_DATA_TYPES = new List<string>() 
                                                                       {    "FMSG",
                                                                            "WMSG",
                                                                            "SMSG"
                                                                       };

            static public List<string> LOCAL_DATA_TYPES = new List<string>() 
                                                                       {    "BOOL",
                                                                            "BYTE", "CHAR", 
                                                                            "WORD", "INT",
                                                                            "DWORD","DINT",
                                                                            "TIMER","TIME",
                                                                            "REAL", "DREAL",
                                                                            "LABEL"
                                                                       };

            static public List<string> PROGRAMM_DATA_TYPES = new List<string>() 
                                                                       {    "byte", "sbyte", 
                                                                            "word", "sword",
                                                                            "lword","slword",
                                                                            "float","double"
                                                                       };

            static public Dictionary<string, string>  EQUAL_DATA_TYPES = new Dictionary<string,string> 
                                                                       {    
                                                                            {"CHAR", "byte"},
                                                                            {"BYTE", "byte"},
                                                                            {"WORD", "word"},
                                                                            {"INT",  "sword"},
                                                                            {"DWORD","lword"},
                                                                            {"TIMER","lword"},
                                                                            {"TIME", "lword"},
                                                                            {"DINT", "slword"},
                                                                            {"REAL", "double"},
                                                                            {"DREAL","double"}
                                                                       };

            static public Dictionary<string, string> EQUAL_DATA_TYPES_REVERSED = new Dictionary<string, string> 
                                                                       {    
                                                                            {"byte",  "BYTE"},
                                                                            {"sbyte", "BYTE"},
                                                                            {"word",  "WORD"},
                                                                            {"sword", "INT" },
                                                                            {"lword", "DWORD"},
                                                                            {"slword","DINT"},
                                                                            {"double","REAL"}
                                                                          //  {"double","DREAL"}
                                                                       };

            static public Dictionary<string, string> EQUAL_MEMORY_TYPES_REVERSED = new Dictionary<string, string> 
                                                                       {    
                                                                            {"rd", "RD"},
                                                                            {"rw", "RW"}
                                                                       };

            static public Dictionary<string, string> EQUAL_SHORT_DATA_TYPES = new Dictionary<string, string> 
                                                                       {    
                                                                            //{"BOOL", "word"},
                                                                            {"CHAR", "b"},
                                                                            {"BYTE", "b"},
                                                                            {"WORD", "w"},
                                                                            {"INT",  "sw"},
                                                                            {"DWORD","lw"},
                                                                            {"TIMER","lw"},
                                                                            {"TIME", "lw"},
                                                                            {"DINT", "slw"},
                                                                            {"REAL", "f"},
                                                                            {"DREAL", "d"}
                                                                       };


//******  Memory Areas
//Process image addresses (I, IB, IW, ID, Q, QB, QW, QD)
//	Bit memory addresses (M, MB, MW, MD)
//	Timer / counter addresses (T / C)
//	Logic blocks (FB, FC, SFB, SFC)
//	Data blocks (DB)
//
// Базовые переменные КТЭ:
//  "BI", "BIB", "BIW", "BID"
//  "BQ", "BQB", "BQW", "BQD"
            
/*            static public List<string> MEMORY_TYPES = new List<string>() {  "I", "IB", "IW", "ID",
                                                                            "Q", "QB", "QW", "QD",
                                                                            "M", "MB", "MW", "MD",
                                                                            "T", "C"
                                                                         };

            static public List<string> BASE_MEMORY_TYPES = new List<string>() 
                                                                         {  
                                                                            "BI", "BIB", "BIW", "BID",
                                                                            "BQ", "BQB", "BQW", "BQD"
                                                                         };

            static public List<string> STORABLE_MEMORY_TYPES = new List<string>() 
                                                                         {  
                                                                            "SI", "SIB", "SIW", "SID",
                                                                            "SQ", "SQB", "SQW", "SQD"
                                                                         };

            static public List<string> LOCAL_MEMORY_TYPES = new List<string>() 
                                                                         {  "", "IN", "OUT", //"IN_OUT", 
                                                                            "TEMP"//, "RETURN"
                                                                         };
*/


                        static public List<string> GLOBAL_MEMORY_TYPES = new List<string>() {  "RD", "RW" };

                        static public List<string> BASE_MEMORY_TYPES = GLOBAL_MEMORY_TYPES;//new List<string>() { "RD", "RW" };

                        static public List<string> STORABLE_MEMORY_TYPES = GLOBAL_MEMORY_TYPES;//new List<string>() { "RD", "RW" };

                        static public List<string> LOCAL_MEMORY_TYPES = new List<string>() // "" - пустое место для Label-типа.
                                                                                     {  "", "IN", "OUT", //"IN_OUT", 
                                                                                        "TEMP"//, "RETURN"
                                                                                     };
            



//******  Bit Numbers   Первые два символа в номере бита должны быть обязательно его номер, а потом уже можно дописывать имя.
                    
            
            static public List<string> BIT8_NUMBERS  = new List<string>() { "", " 0", " 1", " 2", " 3", " 4", " 5", " 6", " 7" };
            static public List<string> BIT16_NUMBERS = new List<string>() { "", " 0", " 1", " 2", " 3", " 4", " 5", " 6", " 7",
                                                                                " 8", " 9", "10", "11", "12", "13", "14", "15"};
            static public List<string> BIT32_NUMBERS = new List<string>() { "", " 0", " 1", " 2", " 3", " 4", " 5", " 6", " 7",
                                                                                " 8", " 9", "10", "11", "12", "13", "14", "15",
                                                                                "16", "17", "18", "19", "20", "21", "22", "23",
                                                                                "24", "25", "26", "27", "28", "29", "30", "31"};
            static public List<string> TIMER_BIT_NUMBERS = new List<string>() { "",    " 0-runnig", 
                                                                                       " 1-elapsed",
                                                                                       " 2-eni",
                                                                                       " 3-eni_pe",
                                                                                       " 4-eni_ne",
		                                                                               " 5-eno",    
                                                                                       " 6-reset",  
                                                                                       " 7-reset_pe",
                                                                                       " 8-reset_ne",
		                                                                               " 9-ENI_init",
                                                                                       "10-RES_init",
                                                                                       "11-BI_init",
                                                                               "12", "13", "14", "15"};//,
                                                                               //"16","17", "18", "19", "20", "21", "22", "23",
                                                                               //"24","25", "26", "27", "28", "29", "30", "31"};
            static public List<string> BIT_NUMBERS = BIT8_NUMBERS;



            static public Dictionary<string, List<string>> FORMAT_TYPES = new Dictionary<string, List<string>> 
                                                                       {    
                                                                            { "",            new List<string> {"_form(0, 3, 2, type)", "##0.##" } },
                                                                            { "+    x.xxx",  new List<string> {"_form(0, 1, 3, type)", "0.###"  } },
                                                                            { "+    x.xx ",  new List<string> {"_form(0, 1, 2, type)", "0.##"   } },
                                                                            { "+    x.x  ",  new List<string> {"_form(0, 1, 1, type)", "0.#"    } },
                                                                            { "+    x.   ",  new List<string> {"_form(0, 1, 0, type)", "0"      } },                               
                                                                            { "+   xx.xxx",  new List<string> {"_form(0, 2, 3, type)", "#0.###" } },
                                                                            { "+   xx.xx ",  new List<string> {"_form(0, 2, 2, type)", "#0.##"  } },
                                                                            { "+   xx.x  ",  new List<string> {"_form(0, 2, 1, type)", "#0.#"   } },
                                                                            { "+   xx.   ",  new List<string> {"_form(0, 2, 0, type)", "#0"     } },                               
                                                                            { "+  xxx.xxx",  new List<string> {"_form(0, 3, 3, type)", "##0.###"} },
                                                                            { "+  xxx.xx ",  new List<string> {"_form(0, 3, 2, type)", "##0.##" } },
                                                                            { "+  xxx.x  ",  new List<string> {"_form(0, 3, 1, type)", "##0.#"  } },
                                                                            { "+  xxx.   ",  new List<string> {"_form(0, 3, 0, type)", "##0"    } },                               
                                                                            { "+ xxxx.xxx",  new List<string> {"_form(0, 4, 3, type)", "###0.###"} },
                                                                            { "+ xxxx.xx ",  new List<string> {"_form(0, 4, 2, type)", "###0.##"} },
                                                                            { "+ xxxx.x  ",  new List<string> {"_form(0, 4, 1, type)", "###0.#" } },
                                                                            { "+ xxxx.   ",  new List<string> {"_form(0, 4, 0, type)", "###0"   } },                               
                                                                            { "+xxxxx.xxx",  new List<string> {"_form(0, 5, 3, type)", "####0.###"} },
                                                                            { "+xxxxx.xx ",  new List<string> {"_form(0, 5, 2, type)", "####0.##"} },
                                                                            { "+xxxxx.x  ",  new List<string> {"_form(0, 5, 1, type)", "####0.#"} },
                                                                            { "+xxxxx.   ",  new List<string> {"_form(0, 5, 0, type)", "####0"  } },                               

                                                                            { "     x.xxx",  new List<string> {"_form(1, 1, 3, type)", "0.###"  } },
                                                                            { "     x.xx ",  new List<string> {"_form(1, 1, 2, type)", "0.##"   } },
                                                                            { "     x.x  ",  new List<string> {"_form(1, 1, 1, type)", "0.#"    } },
                                                                            { "     x.   ",  new List<string> {"_form(1, 1, 0, type)", "0"      } },                               
                                                                            { "    xx.xxx",  new List<string> {"_form(1, 2, 3, type)", "#0.###" } },
                                                                            { "    xx.xx ",  new List<string> {"_form(1, 2, 2, type)", "#0.##"  } },
                                                                            { "    xx.x  ",  new List<string> {"_form(1, 2, 1, type)", "#0.#"   } },
                                                                            { "    xx.   ",  new List<string> {"_form(1, 2, 0, type)", "#0"     } },                               
                                                                            { "   xxx.xxx",  new List<string> {"_form(1, 3, 3, type)", "##0.###"} },
                                                                            { "   xxx.xx ",  new List<string> {"_form(1, 3, 2, type)", "##0.##" } },
                                                                            { "   xxx.x  ",  new List<string> {"_form(1, 3, 1, type)", "##0.#"  } },
                                                                            { "   xxx.   ",  new List<string> {"_form(1, 3, 0, type)", "##0"    } },                               
                                                                            { "  xxxx.xxx",  new List<string> {"_form(1, 4, 3, type)", "###0.###"} },
                                                                            { "  xxxx.xx ",  new List<string> {"_form(1, 4, 2, type)", "###0.##"} },
                                                                            { "  xxxx.x  ",  new List<string> {"_form(1, 4, 1, type)", "###0.#" } },
                                                                            { "  xxxx.   ",  new List<string> {"_form(1, 4, 0, type)", "###0"   } },                               
                                                                            { " xxxxx.xxx",  new List<string> {"_form(1, 5, 3, type)", "####0.###"} },
                                                                            { " xxxxx.xx ",  new List<string> {"_form(1, 5, 2, type)", "####0.##"} },
                                                                            { " xxxxx.x  ",  new List<string> {"_form(1, 5, 1, type)", "####0.#"} },
                                                                            { " xxxxx.   ",  new List<string> {"_form(1, 5, 0, type)", "####0"  } }
                                                                       };

static public string GetKolinFormat( Symbol_Data symbol, string format = null, string data_type = null )
{
    try
    {
        if (symbol != null) 
        { 
            if ( format == null ) format = symbol.Format_Value;
            if (data_type == null) data_type = symbol.Data_Type;
        }

                string format_type = null;
                if (data_type == "BYTE") format_type = "1";
                else if (data_type == "WORD" || data_type == "INT") format_type = "2";
                else if (data_type == "DWORD" || data_type == "DINT") format_type = "4";
                else if (data_type == "REAL") format_type = "0";
        //---

        return SYMBOLS.FORMAT_TYPES[format][0].Replace("type", format_type);

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}

//********  SYMBOL_DATA
                            
            public class Symbol_Data : DependencyObject
            {
                static int Count=0;
                //---

                public ObservableCollection<Symbol_Data> Owner { get; set;}

                //---  Для <DependencyObject> свойства нужно делать <DependencyProperty>
                //---  иначе не работает автообновление текста на экране привязанных свойст.
                public string Name
                {
                    get { return (string)GetValue(NameProperty); }
                    set { SetValue(NameProperty, value); }
                }
                public static readonly DependencyProperty NameProperty =
                                         DependencyProperty.Register("Name", typeof(string), typeof(Symbol_Data));

//**********
                public string Tag_Name
                {
                    get { return (string)GetValue(Tag_NameProperty); }
                    set { SetValue(Tag_NameProperty, value); }
                }
                public static readonly DependencyProperty Tag_NameProperty =
                                         DependencyProperty.Register("Tag_Name", typeof(string), typeof(Symbol_Data));

//**********
                public string Data_Type
                {
                    get { return (string)GetValue(Data_TypeProperty); }
                    set { SetValue(Data_TypeProperty, value); }
                }
                public static readonly DependencyProperty Data_TypeProperty =
                                         DependencyProperty.Register("Data_Type", typeof(string), typeof(Symbol_Data));

//**********
                public string Memory_Type
                {
                    get { return (string)GetValue(Memory_TypeProperty); }
                    set { SetValue(Memory_TypeProperty, value); }
                }
                public static readonly DependencyProperty Memory_TypeProperty =
                                         DependencyProperty.Register("Memory_Type", typeof(string), typeof(Symbol_Data));

//**********                
                public double? Address
                {   //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                    get { return (double?)(Int64?)(double?)GetValue(AddressProperty); }
                    set { SetValue(AddressProperty, value); }
                }
                public static readonly DependencyProperty AddressProperty =
                                         DependencyProperty.Register("Address", typeof(double?), typeof(Symbol_Data));

//****
                public string str_Address
                {
                    get { return Address.ToString(); }
                    set
                    {
                        double dax;
                        if (value == null) Address = null;
                        else if (value == "") Address = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Address = dax;
                        //else Address = null; оставляем прежнее значениеа не обнуляем
                    }
                }
//**********
                public string Bit_Base
                {
                    get { return (string)GetValue(Bit_BaseProperty); }
                    set { SetValue(Bit_BaseProperty, value); }
                }
                public static readonly DependencyProperty Bit_BaseProperty =
                                         DependencyProperty.Register("Bit_Base", typeof(string), typeof(Symbol_Data));


                public string str_Bit_Base_Type
                {
                    get
                    {
                        //  ищем блок данных переменной BIT_BASE по ее имени.
                        foreach (SYMBOLS.Symbol_Data symbol in Owner)
                        {
                            if (symbol.Name == Bit_Base)  return symbol.Data_Type;
                        }

                        return null;
                    }
                }


                public Int32? getBit_Base_Value
                {
                    get
                    {
                        if ( Owner != null)
                        {
                            foreach (Symbol_Data symbol in Owner)
                            {
                                if (symbol.Name == Bit_Base)
                                {
                                    Int32 lax;
                                    if (Int32.TryParse(symbol.Real_Value, NumberStyles.Any, null, out lax)) return lax;
                                    else break;
                                }
                            }
                        }
                        return null; 
                    }
                }

//**********
                public double? Bit_Number
                {
                    get { return (double?)GetValue(Bit_NumberProperty); }
                    set { SetValue(Bit_NumberProperty, value); }
                }
                public static readonly DependencyProperty Bit_NumberProperty =
                                        DependencyProperty.Register("Bit_Number", typeof(double?), typeof(Symbol_Data),
                                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
                                        new PropertyChangedCallback(Onstr_Nom_ValueChanged)));

//****

                public string str_Bit_Number 
                {
                    get 
                    {
                        if( Bit_Number == null ) return ""; 
                        else if (str_Bit_Base_Type == "TIMER") return TIMER_BIT_NUMBERS[(int)Bit_Number+1];
                        else return BIT32_NUMBERS[(int)Bit_Number+1]; 
                    }
                    set
                    {
                        double dax;
                        if (value == null) Bit_Number = null;
                        else if (value == "") Bit_Number = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        //     Substring(0, 2) -  первые два символа в имени бита должны быть его номер цифровой номер!
                        else if (double.TryParse((string)value.Substring(0, 2).Trim(), NumberStyles.Any, null, out dax)) Bit_Number = dax;
                        //else Bit_Number = null;  оставляем прежнее значениеа не обнуляем
                    }
                }
//**********
//**********

                public string Initial_Value
                {
                    //get { return (string)GetValue(Initial_ValueProperty); }
                    get
                    {
                        double dax;
                        //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        if (Data_Type == "REAL" || Data_Type == "DREAL")
                        {
                            // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                            if (double.TryParse((string)GetValue(Initial_ValueProperty), NumberStyles.Any, null, out dax)) return dax.ToString();
                            else return (string)GetValue(Initial_ValueProperty); 
                        }
                        else if (Data_Type == "BYTE" || Data_Type == "WORD" || Data_Type == "DWORD" || Data_Type == "INT" || Data_Type == "DINT")
                        {
                            if (double.TryParse((string)GetValue(Initial_ValueProperty), NumberStyles.Any, null, out dax)) return ((Int64)dax).ToString();
                            else return (string)GetValue(Initial_ValueProperty); 
                        }
                        else return (string)GetValue(Initial_ValueProperty); 
                    }

                    //set { SetValue(Initial_ValueProperty, value); }
                    set
                    {
                        if (Data_Type == "REAL" || Data_Type == "DREAL" || Data_Type == "BYTE" || Data_Type == "WORD" ||
                            Data_Type == "DWORD" || Data_Type == "INT" || Data_Type == "DINT")
                        {
                            double dax;
                            // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                            if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) SetValue(Initial_ValueProperty, dax.ToString());
                            //else   оставляем прежнее значениеа не обнуляем
                        }
                        else SetValue(Initial_ValueProperty, value); 
                    }


                }
                public static readonly DependencyProperty Initial_ValueProperty =
                                         DependencyProperty.Register("Initial_Value", typeof(string), typeof(Symbol_Data));


                public string fstr_Initial_Value
                {
                    get 
                    {
                        double dax;
                        if (Initial_Value == null) return null;
                        else if (Initial_Value == "") return null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse(Initial_Value, NumberStyles.Any, null, out dax))
                        {
                            dax = dax / (double)Nom_Value * (double)K_Value;
                            //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                            //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                            //10-07-2018 if (Data_Type == "REAL" || Data_Type == "DREAL") return dax.ToString(FORMAT_TYPES[Format_Value][1]); 
                            //else 
                            //{
                              //  return ((Int64)dax).ToString(FORMAT_TYPES[Format_Value][1]); 
                            //}
                            return dax.ToString(FORMAT_TYPES[Format_Value][1]);
                        }
                        else return null;
                    }
                    set
                    {
                        double dax;
                        if (value == null) Initial_Value = null;
                        else if (value == "") Initial_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Initial_Value = (dax * (double)Nom_Value / (double)K_Value).ToString();
                        //else Initial_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }

                //public int? Initial_Value { get { return initial_value; }
                  //                          set { initial_value = value; }}

                //public string str_Initial_Value
                //{
                    //get { return initial_value.ToString(); }
                    //set
                    //{
                        //int ax;
                        //if (value == null) initial_value = null;
                        //else if (value == "") initial_value = null;
                        //else if (int.TryParse((string)value, out ax)) initial_value = ax;
                    //}
                //}

//**********  
                public string Comment
                {
                    get { return (string)GetValue(CommentProperty); }
                    set { SetValue(CommentProperty, value); }
                }
                public static readonly DependencyProperty CommentProperty =
                                         DependencyProperty.Register("Comment", typeof(string), typeof(Symbol_Data));

//**********   FOR  DEBUG  SERVICE
                //  Специфическое свойство.
                //  Свойство отличается от остальных тем что в него пишутся данные из внешнего потока
                // и заблокирована запись с экрана форм, т.к. не смог справиться с побочным двойным масштабированием
                public string Real_Value
                {
                    get
                    {
                        double dax;
                        //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        if (Data_Type == "BOOL")
                        {
                            Int32? lax = getBit_Base_Value;
                            if( lax != null)
                            {
                                if ((lax.Value & (1<<(int)(Bit_Number.Value))) != 0) return "1";
                                return "0";
                            }
                            return null;
                        }
                        else if (Data_Type == "REAL" || Data_Type == "DREAL")
                        {
                            // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                            if (double.TryParse((string)GetValue(Real_ValueProperty), NumberStyles.Any, null, out dax)) return dax.ToString();
                            else return (string)GetValue(Real_ValueProperty);
                        }
                        else if (Data_Type == "BYTE" || Data_Type == "WORD" || Data_Type == "DWORD" || Data_Type == "INT" || Data_Type == "DINT")
                        {
                            if (double.TryParse((string)GetValue(Real_ValueProperty), NumberStyles.Any, null, out dax)) return ((Int64)dax).ToString();
                            else return (string)GetValue(Real_ValueProperty);
                        }
                        else return (string)GetValue(Real_ValueProperty);
                    }

                    set
                    {
                        if (Data_Type == "BOOL") 
                        {
                            //  данные он возьмет из базовой переменной
                            //!!! добавили запись, чтобы активировать обновление этой переменной на экране 
                            // когда во внешнем потоке пишется в Real_Value.
                            SetValue(fstr_Real_ValueProperty, fstr_Real_Value);
                        }
                        else if (Data_Type == "REAL" || Data_Type == "DREAL" || Data_Type == "BYTE" || Data_Type == "WORD" ||
                            Data_Type == "DWORD" || Data_Type == "INT" || Data_Type == "DINT")
                        {
                            double dax;
                            // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                            if (double.TryParse((string)value, NumberStyles.Any, null, out dax))
                            {
                                SetValue(Real_ValueProperty, dax.ToString());
                                //!!! добавили запись, чтобы активировать обновление этой переменной на экране 
                                // когда во внешнем потоке пишется в Real_Value.
                                SetValue(fstr_Real_ValueProperty, fstr_Real_Value); // такой обман чтобы не изменять значение
                            }
                            //else   оставляем прежнее значениеа не обнуляем
                        }
                        else
                        {
                            SetValue(Real_ValueProperty, value);
                            //!!! добавили запись, чтобы активировать обновление этой переменной на экране 
                            // когда во внешнем потоке пишется в Real_Value.
                            SetValue(fstr_Real_ValueProperty, fstr_Real_Value);
                        }
                    }
                }
                public static readonly DependencyProperty Real_ValueProperty =
                                         DependencyProperty.Register("Real_Value", typeof(string), typeof(Symbol_Data));



                public string fstr_Real_Value
                {
                    get
                    {
                        double dax;
                        if (Data_Type == "BOOL") return Real_Value;
                        else
                        {
                            if (Real_Value == null) return null;
                            else if (Real_Value == "") return null;
                            // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                            else if (double.TryParse(Real_Value, NumberStyles.Any, null, out dax))
                            {
                                dax = dax / (double)Nom_Value * (double)K_Value;
                                return dax.ToString(FORMAT_TYPES[Format_Value][1]);
                            }
                        }
                        return null;
                    }
                    //set  //    Используется при записи сюда значений вручную с экрана 
                    //{    //  Для записи значений из внешнего потока должен использоваться не масштабируемый Real_Value.
                    //    double dax;            //  Пишем в два источника, чтобы для fstr_Value срабатывало оповещение при обновлении.
                    //    if (value == null) { Real_Value = null; }//SetValue(fstr_Real_ValueProperty, Real_Value); }
                    //    else if (value == "") { Real_Value = null;}// SetValue(fstr_Real_ValueProperty, Real_Value); }
                    //    // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                    //    else if (double.TryParse((string)value, NumberStyles.Any, null, out dax))
                    //    {
                    //        Real_Value = (dax * (double)Nom_Value / (double)K_Value).ToString();
                    //        //SetValue(fstr_Real_ValueProperty, Real_Value);
                    //    }
                    //    //else Value = null;  оставляем прежнее значениеа не обнуляем
                    //}
                }

                public static readonly DependencyProperty fstr_Real_ValueProperty =
                                        DependencyProperty.Register("fstr_Real_Value", typeof(string), typeof(Symbol_Data));

//**********


//**********  FOR  STORABLE  SYMBOLS

                public double? Min_Value
                {
                    get
                    {   //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        if (Data_Type == "REAL" || Data_Type == "DREAL") return (double?)GetValue(Min_ValueProperty);
                        else
                        {
                            return (double?)(Int64?)(double?)GetValue(Min_ValueProperty);
                        }
                    }

                    set { SetValue(Min_ValueProperty, value); }
                }
                public static readonly DependencyProperty Min_ValueProperty =
                                         DependencyProperty.Register("Min_Value", typeof(double?), typeof(Symbol_Data));

//****

                public string str_Min_Value
                {
                    get { return Min_Value.ToString(); }
                    set
                    {
                        double dax;
                        if (value == null) Min_Value = null;
                        else if (value == "") Min_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Min_Value = dax;
                        //else Min_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }

                public string fstr_Min_Value
                {
                    get { return ((double)Min_Value / (double)Nom_Value * (double)K_Value).ToString(FORMAT_TYPES[Format_Value][1]); }
                    set
                    {
                        double dax;
                        if (value == null) Min_Value = null;
                        else if (value == "") Min_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Min_Value = dax * (double)Nom_Value / (double)K_Value;
                        //else Min_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }
//*********

                public double? Max_Value
                {
                    get
                    {   //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        if (Data_Type == "REAL" || Data_Type == "DREAL") return (double?)GetValue(Max_ValueProperty); 
                        else 
                        {
                            return (double?)(Int64?)(double?)GetValue(Max_ValueProperty); 
                        }
                    }
                    set { SetValue(Max_ValueProperty, value); }
                }
                public static readonly DependencyProperty Max_ValueProperty =
                                         DependencyProperty.Register("Max_Value", typeof(double?), typeof(Symbol_Data));

//****

                public string str_Max_Value
                {
                    get { return Max_Value.ToString(); }
                    set
                    {
                        double dax;
                        if (value == null) Max_Value = null;
                        else if (value == "") Max_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Max_Value = dax;
                        //else Max_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }

                public string fstr_Max_Value
                {
                    get { return ((double)Max_Value / (double)Nom_Value * (double)K_Value).ToString(FORMAT_TYPES[Format_Value][1]); }
                    set
                    {
                        double dax;
                        if (value == null) Max_Value = null;
                        else if (value == "") Max_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Max_Value = dax * (double)Nom_Value / (double)K_Value;
                        //else Max_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }

//********    От Nom_Value и K_Value зависят значения Initial_Value, Min_Value, Max_Value и т.д.
//          поэтому их делаем dependancy чтобы они оповещали о своем изменении и мы обновляли DataGrid.

                public double Nom_Value
                {
                    get 
                    {
                        string str = null;
                        if (NOMS_SYMBOLS_LIST.Any(value => value.Name == str_Nom_Value)) str = NOMS_SYMBOLS_LIST.First(value => value.Name == str_Nom_Value).Initial_Value;
                        double dax;
                        if (str == null) dax = 1;
                        else if (str == "") dax = 1;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse(str, NumberStyles.Any, null, out dax)) { }
                        else dax = 1;
                        //---  Отсекаем дробь в возвращаемой переменной если кто-то ввел с дробью.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        return (double)(Int64)dax ; 
                    }
                    //set { nom_value = value; }
                }


                public static readonly  DependencyProperty str_Nom_ValueProperty =
                                        DependencyProperty.Register("str_Nom_Value", typeof(string), typeof(Symbol_Data), 
                                        new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsRender,
                                        new PropertyChangedCallback(Onstr_Nom_ValueChanged)));

                public string str_Nom_Value
                {
                    get { return (string)GetValue(str_Nom_ValueProperty); }
                    set { SetValue(str_Nom_ValueProperty, value); }
                }

                private static void Onstr_Nom_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
                {
                    //---   блокируем срабатывание события при создании символа из конструктора
                    if (((Symbol_Data)d).ConstructorInProgress != true)  Refresh_DataGridItemsSource((Symbol_Data)d);
                }

            //---


                public static readonly DependencyProperty K_ValueProperty =
                                        DependencyProperty.Register("K_Value", typeof(double?), typeof(Symbol_Data),
                                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
                                        new PropertyChangedCallback(OnK_ValueChanged)));

                public double? K_Value
                {
                    get
                    {   //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        if (Data_Type == "REAL" || Data_Type == "DREAL") return (double?)GetValue(K_ValueProperty);
                        else
                        {
                            return (double?)(Int64?)(double?)GetValue(K_ValueProperty);
                        }
                    }
                    set { SetValue(K_ValueProperty, value); }
                }

                private static void OnK_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
                {
                    if (((Symbol_Data)d).ConstructorInProgress != true) Refresh_DataGridItemsSource((Symbol_Data)d);
                }


                public string str_K_Value
                {
                    get { return K_Value.ToString(); }
                    set
                    {
                        double dax;
                        if (value == null) K_Value = null;
                        else if (value == "") K_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) K_Value = dax;
                        //else K_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }
//**********

                public double? Step_Value
                {
                    get
                    {   //---  Если тип переменной целочисленный - то отсекаем дробь в возвращаемой переменной.
                        //---  иначе компилятор проставляет в скомпилированном тексте цифры с запятыми и дробью.
                        if (Data_Type == "REAL" || Data_Type == "DREAL") return (double?)GetValue(Step_ValueProperty);
                        else
                        {
                            return (double?)(Int64?)(double?)GetValue(Step_ValueProperty);
                        }
                    }
                    set { SetValue(Step_ValueProperty, value); }
                }
                public static readonly DependencyProperty Step_ValueProperty =
                                         DependencyProperty.Register("Step_Value", typeof(double?), typeof(Symbol_Data));

//****

                public string str_Step_Value
                {
                    get { return Step_Value.ToString(); }
                    set
                    {
                        double dax;
                        if (value == null) Step_Value = null;
                        else if (value == "") Step_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Step_Value = dax;
                        //else Step_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }

                public string fstr_Step_Value
                {
                    get { return ((double)Step_Value / (double)Nom_Value * (double)K_Value).ToString(FORMAT_TYPES[Format_Value][1]); }
                    set
                    {
                        double dax;
                        if (value == null) Step_Value = null;
                        else if (value == "") Step_Value = null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse((string)value, NumberStyles.Any, null, out dax)) Step_Value = dax * (double)Nom_Value / (double)K_Value;
                        //else Step_Value = null;  оставляем прежнее значениеа не обнуляем
                    }
                }
//***********

                public string Unit_Value
                {
                    get { return (string)GetValue(Unit_ValueProperty); }
                    set { SetValue(Unit_ValueProperty, value); }
                }
                public static readonly DependencyProperty Unit_ValueProperty =
                                         DependencyProperty.Register("Unit_Value", typeof(string), typeof(Symbol_Data));
//**********

                public string Format_Value
                {
                    get { return (string)GetValue(Format_ValueProperty); }
                    set { SetValue(Format_ValueProperty, value); }
                }
                public static readonly DependencyProperty Format_ValueProperty =
                                         DependencyProperty.Register("Format_Value", typeof(string), typeof(Symbol_Data));

//***************************

                //  ввел промежуточные переменные, чтобы они сообщали суммарному имени об их изменении.
                //public string binding_name;

                public bool ConstructorInProgress; //  для блокировки Nom_ValueChanged

                //public string name;
                //public string tag_name;
                //public string memory_type;
                //public string data_type;
                //public int?   address;
                //public string bit_base;
                //public Symbol_Data bit_base;
                //public int?   bit_number;
//                public string initial_value;
                //public int?   initial_value;
                //public string comment;

                //public double?   min_value, max_value/*, k_value*/, step_value;
                //public string /*str_nom_value,*/ unit_value, format_value;

//***********   CONSTRUCTOR #1      Применяется при установке элемента на поле при загрузке из файла.                
//   конструктор сам заполняет себя по имени переменной поиском своих данных в списке переменных.

                public Symbol_Data(ObservableCollection<Symbol_Data> owner, string name) //  конструктор без параметров не пропускает конвертер привязки.
                {
                    ConstructorInProgress = true;
                    //---
                    Count++;
                    Owner = owner;
                    //---  Новый пустой символ: применяется при Add_Row в таблице символов.
                    if (name == null || owner == null)
                    {
                        Name = "NoName";
                        Tag_Name = null;
                        Data_Type = "";// "INT";// "WORD";
                        Memory_Type = "";// "MW";
                        Address = 0;
                        Bit_Base = null;
                        Bit_Number = null;
                        Real_Value = null;
                        Initial_Value = null;
                        Comment = "";// "Comment:...";

                        //---  Storable Symbols
                        str_Nom_Value = "";
                        K_Value = 1.0;
                        Step_Value = 0.0;
                        Min_Value = 0.0;
                        Max_Value = 0.0;
                        Unit_Value = "";
                        Format_Value = "";

                    }
                    else  //  поиск своих данных по своему имени переменой.
                    {
                        foreach (Symbol_Data symbol in owner)//GLOBAL_SYMBOLS_LIST)
                        {
                            if (symbol.Name == name)
                            {
                                if (owner == null) Owner = symbol.Owner;
                                //---
                                Name = symbol.Name;
                                Tag_Name = symbol.Tag_Name;
                                Data_Type = symbol.Data_Type;
                                Memory_Type = symbol.Memory_Type;
                                Address = symbol.Address;
                                Bit_Base = symbol.Bit_Base;
                                Bit_Number = symbol.Bit_Number;
                                Real_Value = null;
                                Initial_Value = symbol.Initial_Value;
                                Comment = symbol.Comment;

                                //---  Storable Symbols
                                Min_Value = symbol.Min_Value; 
                                Max_Value = symbol.Max_Value; 
                                str_Nom_Value = symbol.str_Nom_Value; 
                                K_Value = symbol.K_Value; 
                                Step_Value = symbol.Step_Value;
                                Unit_Value = symbol.Unit_Value; 
                                Format_Value = symbol.Format_Value;

                                return;
                            }
                        }
                        //  имя не найдено.
                                                
                            Name = "???_" + name;
                            Data_Type = "";
                            Memory_Type = "";
                            Address = null;
                            Bit_Base = null;
                            Bit_Number = null;
                            Real_Value = null;
                            Initial_Value = "";
                            Comment = "Name not found!";

                            //---  Storable Symbols
                            str_Nom_Value = "";
                            K_Value = 1.0;
                            Step_Value = 0.0;
                            Min_Value = 0.0;
                            Max_Value = 0.0;
                            Unit_Value = "";
                            Format_Value = "";
                    }

                    ConstructorInProgress = false;
                }

//***********   CONSTRUCTOR #2     Применяется при составлении списка переменных в программе или при загрузке списка из файла.
                public Symbol_Data( ObservableCollection<Symbol_Data> owner, 
                                    string name, string tag_name, string data_type, string memory_type, double? address, 
                                    string bit_base, double? bit_number, string initial_value, string comment,
                                        //---  Storable Symbols
                                    double? min_value = 0, double? max_value = 0, string nom_value = "", double? k_value = 1, 
                                    double? step_value = 0, string unit_value = "", string format_value = ""
                                    )
                {
                    ConstructorInProgress = true;
                    //---
                    Count++;
                    Owner = owner;
                    //---
                    Name = name;
                    Tag_Name = tag_name;
                    Data_Type = data_type;
                    Memory_Type = memory_type;
                    Address = address;
                    Bit_Base = bit_base;
                    Bit_Number = bit_number;
                    Real_Value = null;
                    Initial_Value = initial_value;
                    Comment = comment;

                    //---  Storable Symbols
                    Min_Value = min_value;
                    Max_Value = max_value;
                    str_Nom_Value = nom_value;
                    K_Value = k_value;
                    Step_Value = step_value;
                    Unit_Value = unit_value;
                    Format_Value = format_value;

                    ConstructorInProgress = false;
                }


                //public void Set_Binding_Name()
                //{
                  //  string str = Memory_Type + Address;     
                    //if ( Bit_Number != null ) str = str + "." + Bit_Number;
                    //str = str + "\n" + Comment + "\n\"" + Name + "\"";
                    //Binding_Name = str;
                //}

            }

//**************    Refresh_DataGridItemsSource

            //---      Обновление содержимого окна DataGrid если изменили свойство от которго зависят показания соседних свойств.
            //---  Обновление окна делается через обновление DataGrid.ItemsSource.

            static void Refresh_DataGridItemsSource(Symbol_Data symbol)
            {
                try
                {
                    Window window = null;

                    if (symbol != null && symbol.Owner != null)
                    {
                        if (symbol.Owner == GLOBAL_SYMBOLS_LIST) window = GLOBAL_SYMBOLS_LIST_window;
                        if (symbol.Owner == BASE_SYMBOLS_LIST) window = BASE_SYMBOLS_LIST_window;
                        if (symbol.Owner == BASE_TAGS_SYMBOLS_LIST) window = BASE_TAGS_SYMBOLS_LIST_window;
                        if (symbol.Owner == STORABLE_SYMBOLS_LIST) window = STORABLE_SYMBOLS_LIST_window;
                        if (symbol.Owner == MSG_SYMBOLS_LIST) window = MSG_SYMBOLS_LIST_window;
                        if (symbol.Owner == ACTIV_DIAGRAM.LOCAL_SYMBOLS_LIST) window = LOCAL_SYMBOLS_LIST_window;
                        if (symbol.Owner == NOMS_SYMBOLS_LIST) window = NOMS_SYMBOLS_LIST_window;
                        //if (symbol.Owner == INDICATION_SYMBOLS_LIST) window = NOMS_SYMBOLS_LIST_window;
                        //---

                        SYMBOLS_LIST.REFRESH_SYMBOLS_LIST_window(window);
                    }
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }


            //************    PANELs

            static public Window GLOBAL_SYMBOLS_LIST_window ;

            //static public DataGrid GLOBAL_SYMBOLS_LIST_PANEL { get; set; }

            static public ObservableCollection<Symbol_Data> GLOBAL_SYMBOLS_LIST { get; set; }

            static public List<Symbol_Data> SYMBOLS_LIST_BUFFER { get; set; }

            static public bool Validation_Ok = false;
            static public string Validation_Error_Line = null;

            //    CONSTRUCTOR #1

            //  Создание пустой SYMBOLS_LIST с одной NEW_LINE.

public SYMBOLS()//StackPanel symbols_list_panel)
{
    try
    {
        SYMBOLS_VALIDATION_ERRORS = "No errors";

//**************    GLOBAL_SYMBOLS_LIST

                GLOBAL_SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data(null, "g_Test1", null, "INT", "RW", null, null, null, null, "Comments1............."),
                    new Symbol_Data(null, "g_Test2", null, "WORD", "RW", null, null, null, null, "Comments2............."),
                    new Symbol_Data(null, "g_Test3", null, "BOOL", "RW", null, "g_Test2",    2, null, "Comments3.............")
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in GLOBAL_SYMBOLS_LIST) symbol.Owner = GLOBAL_SYMBOLS_LIST;


//**************    BASE_SYMBOLS_LIST

                BASE_SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data(null, "Pi0", "Pi0", "BYTE", "RW", null, null, null, null, "входной дискретный порт Pi0, чтение")
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in BASE_SYMBOLS_LIST) symbol.Owner = BASE_SYMBOLS_LIST;

                BASE_TAGS_SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data(null, "Pi0", null, "BYTE", "RW", null, null, null, null, "входной дискретный порт Pi0, чтение")
                };

//**************    STORABLE_SYMBOLS_LIST

                STORABLE_SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data(null, "s_Test1", "s_Test1", "INT", "RD", null, null, null, "0", "Comments1.............")//, 0,100,"",1,1,"oe","+  xxx.xx ")
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in STORABLE_SYMBOLS_LIST) symbol.Owner = STORABLE_SYMBOLS_LIST;

//**************    MSG_SYMBOLS_LIST

                MSG_SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data(null, "Fail1", null, "FMSG", null, null, null, null, "Test_failure1", "Comments1............."),
                    new Symbol_Data(null, "Warn1", null, "WMSG", null, null, null, null, "Test_warning1", "Comments2............."),
                    new Symbol_Data(null, "Serv1", null, "SMSG", null, null, null, null, "Test_service1", "Comments3.............")
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in MSG_SYMBOLS_LIST) symbol.Owner = MSG_SYMBOLS_LIST;

//**************    SYMBOLS_NOMS_LIST

                NOMS_SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data(null, "", null, "INT", "RD", null, null, null, "1", ""),
                    new Symbol_Data(null, "_1", null, "INT", "RD", null, null, null, "1", ""),
                    new Symbol_Data(null, "_100", null, "INT", "RD", null, null, null, "100", ""),
                    new Symbol_Data(null, "_1000", null, "INT", "RD", null, null, null, "1000", ""),
                    new Symbol_Data(null, "_256", null, "INT", "RD", null, null, null, "256", "")
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in NOMS_SYMBOLS_LIST) symbol.Owner = NOMS_SYMBOLS_LIST;
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS_NOMS_DEFAULT_LIST) symbol.Owner = SYMBOLS_NOMS_DEFAULT_LIST;


//**************    INDICATION_SYMBOLS_LIST

                INDICATION_SYMBOLS_LIST = new ObservableCollection<Indication_Symbol_Data>()
                { 
                    new Indication_Symbol_Data()
                };
                //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                foreach (SYMBOLS.Indication_Symbol_Data symbol in INDICATION_SYMBOLS_LIST) symbol.Owner = INDICATION_SYMBOLS_LIST;

//**************

                SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
//**************

            //    CONSTRUCTOR #2

            //  Создание SYMBOLS_LIST с загруженной из файла List<Network_Of_Elements> symbols.

            public SYMBOLS( ObservableCollection<Symbol_Data> global_symbols,
                            ObservableCollection<Symbol_Data> base_symbols,
                            ObservableCollection<Symbol_Data> base_symbols_tags,
                            ObservableCollection<Symbol_Data> storable_symbols,
                            ObservableCollection<Symbol_Data> msg_symbols,
                            ObservableCollection<Symbol_Data> noms_symbols,
                            ObservableCollection<Indication_Symbol_Data> indication_symbols )
                
            {
                //SYMBOLS_LIST_PANEL = symbols_list_panel;
                GLOBAL_SYMBOLS_LIST = global_symbols;
                BASE_SYMBOLS_LIST = base_symbols;
                BASE_TAGS_SYMBOLS_LIST = base_symbols_tags;
                STORABLE_SYMBOLS_LIST = storable_symbols;
                MSG_SYMBOLS_LIST = msg_symbols;

                NOMS_SYMBOLS_LIST = noms_symbols;
                    //  в догонку заполняем Owner, т.к. на стадии создания массива GLOBAL_SYMBOLS_LIST еще равно нулю.
                    foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS_NOMS_DEFAULT_LIST) symbol.Owner = SYMBOLS_NOMS_DEFAULT_LIST;

                INDICATION_SYMBOLS_LIST = indication_symbols;
                SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
            }

//************   FOR XPS PRINTING

public static List<List<string>> SYMBOLS_LIST_ToXpsList(ObservableCollection<Symbol_Data> symbols_list,
               out List<List<string>> headers, out List<string> alignment, out List<double> collumn_widths)
{
        headers = new List<List<string>>() 
                        { 
                            new List<string> () {"#", "Symbol", "Memory type", "Address", "Bit base", "Bit number", "Data type", "Initial value", "Comment" }
                        };
        alignment = new List<string>() 
                        {                             
                            "Center", "Left", "Center", "Center", "Center", "Center", "Center", "Center", "Left" 
                        };
        collumn_widths = new List<double>() { 30, 100, 70, 70, 70, 70, 70, 70, 350 };
        //---

    try
    {
        List<List<string>> list0 = new List<List<string>>();

        int item_number = 1;
        foreach (Symbol_Data symbol in symbols_list)
        {
            List<string> list1 = new List<string>();

                list1.Add(item_number.ToString());
                list1.Add(symbol.Name);
                list1.Add(symbol.Memory_Type);
                list1.Add(symbol.str_Address);
                list1.Add(symbol.Bit_Base);
                list1.Add(symbol.str_Bit_Number);
                list1.Add(symbol.Data_Type);
                list1.Add(symbol.Initial_Value);
                list1.Add(symbol.Comment);

                item_number++;
                //---

            list0.Add(list1);            
        }

        return list0;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}


//***************   For ComboBox for Bit_Base

public static List<string> Get_BitBaseSymbols_List( ObservableCollection<Symbol_Data> in_symbols_list )
{
    try
    {
        List<string> bit_base_list = new List<string>();

        bit_base_list.Add(""); //null); //  1-й элемент Null, чтобы можно было установить нулевой выбор.

        foreach (Symbol_Data symbol in in_symbols_list)
        {
            if (symbol.Data_Type == "BYTE" || 
                symbol.Data_Type == "WORD" || 
                symbol.Data_Type == "DWORD"||
                symbol.Data_Type == "TIMER" )  bit_base_list.Add(symbol.Name);
        }

        return bit_base_list;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}


//***************  UNIVERSAL  SHOW_SYMBOLS_LIST_window


public delegate Border SHOW_SYMBOLS_LIST(Window list_window);

public void SHOW_SYMBOLS_LIST_window(string code, object x_symbols_list)
{

    try
    {
        //**************   CREATE WINDOW
        Window symbols_list_window = new Window();
        //---
        string name = null;
        SHOW_SYMBOLS_LIST show_symbols_list = null;
        
        //---

        if (x_symbols_list is ObservableCollection<Symbol_Data>)
        {
            ObservableCollection<Symbol_Data> symbols_list = x_symbols_list as ObservableCollection<Symbol_Data>;

            if (symbols_list == GLOBAL_SYMBOLS_LIST)
            {
                name = "Global symbols"; show_symbols_list = SHOW_GLOBAL_SYMBOLS_LIST; GLOBAL_SYMBOLS_LIST_window = symbols_list_window;
            }
            else if (symbols_list == BASE_SYMBOLS_LIST)
            {
                name = "Base symbols"; show_symbols_list = SHOW_BASE_SYMBOLS_LIST; BASE_SYMBOLS_LIST_window = symbols_list_window;
            }
            else if (symbols_list == BASE_TAGS_SYMBOLS_LIST)
            {
                name = "Base Tags-symbols"; show_symbols_list = SHOW_BASE_TAGS_SYMBOLS_LIST; BASE_TAGS_SYMBOLS_LIST_window = symbols_list_window;
            }
            else if (symbols_list == STORABLE_SYMBOLS_LIST)
            {
                name = "SetPoint symbols"; show_symbols_list = SHOW_STORABLE_SYMBOLS_LIST; STORABLE_SYMBOLS_LIST_window = symbols_list_window;
            }
            else if (symbols_list == MSG_SYMBOLS_LIST)
            {
                name = "Msg symbols"; show_symbols_list = SHOW_MSG_SYMBOLS_LIST; MSG_SYMBOLS_LIST_window = symbols_list_window;
            }
            else if (symbols_list == ACTIV_DIAGRAM.LOCAL_SYMBOLS_LIST)
            {
                name = "Diagram: " + ACTIV_DIAGRAM.NAME + ". Local symbols."; show_symbols_list = SHOW_LOCAL_SYMBOLS_LIST; LOCAL_SYMBOLS_LIST_window = symbols_list_window;
            }
            //else if (symbols_list == ACTIV_DIAGRAM.LABEL_SYMBOLS_LIST)
            //{
                //name = "Diagram: " + ACTIV_DIAGRAM.NAME + ". Label symbols."; show_symbols_list = SHOW_LABEL_SYMBOLS_LIST; LABEL_SYMBOLS_LIST_window = symbols_list_window;
            //}
            else if (symbols_list == NOMS_SYMBOLS_LIST)
            {
                name = "Nominals symbols"; show_symbols_list = SHOW_NOMS_SYMBOLS_LIST; NOMS_SYMBOLS_LIST_window = symbols_list_window;
            }
            else throw new Exception("SHOW_SYMBOLS_LIST_window: Unknown Symbols_List");
        }
        else if (x_symbols_list is ObservableCollection<Indication_Symbol_Data>)
        {
            name = "Indication symbols"; show_symbols_list = SHOW_INDICATION_SYMBOLS_LIST; INDICATION_SYMBOLS_LIST_window = symbols_list_window;
        }
        else throw new Exception("<SHOW_SYMBOLS_LIST_window()>: Unknown <Symbols_List>");

//**************   CREATE WINDOW

            //symbols_list_window = new Window();
            symbols_list_window.Title = name;
            symbols_list_window.SizeToContent = SizeToContent.WidthAndHeight;
            symbols_list_window.Width = 500;
            symbols_list_window.Height = 300;
            symbols_list_window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            symbols_list_window.ContentRendered += SYMBOLS_LIST_window_ContentRendered;
            symbols_list_window.Closing += SYMBOLS_LIST_window_Closing;

//**********  CREATE DATA_GRID

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

                    RowDefinition row0 = new RowDefinition();
                    RowDefinition row1 = new RowDefinition();
                    row0.Height = GridLength.Auto;
                    row1.Height = GridLength.Auto;
                    grid.RowDefinitions.Add(row0);
                    grid.RowDefinitions.Add(row1);

                    //   Окно вывода сообщений об ошибках в таблице символов.
                    GroupBox err_groupbox = new GroupBox();
                    err_groupbox.Header = "Status";
                    err_groupbox.SetValue(Grid.ColumnProperty, 0);
                    err_groupbox.SetValue(Grid.RowProperty, 1);

                        ScrollViewer err_scrollviewer = new ScrollViewer();
                        err_scrollviewer.MaxHeight = 50;
                        err_scrollviewer.Margin = new Thickness(5,5,0,5);    
                            TextBlock err_text_block = new TextBlock();
                            //err_text_block.IsReadOnly = true;
                            //err_text_block.Background = new SolidColorBrush(Colors.White); 
                                Binding err_binding = new Binding();
                                    err_binding.Source = SYMBOLS_LIST;
                                    err_binding.Path = new PropertyPath("SYMBOLS_VALIDATION_ERRORS");
                                    err_text_block.SetBinding(TextBlock.TextProperty, err_binding);

                        err_scrollviewer.Content = err_text_block;

                    err_groupbox.Content = err_scrollviewer;
                    //----------------


                    DataGrid datagrid = new DataGrid();
       
                        //datagrid.ToolTip
                        //Binding tooltip_binding = new Binding();
                        //tooltip_binding.Source = SYMBOLS_LIST;
                        //tooltip_binding.Path = new PropertyPath("SYMBOLS_VALIDATION_ERRORS");
                        //BindingOperations.SetBinding(datagrid, DataGrid.ToolTipProperty, tooltip_binding);
                        ////datagrid.ToolTipOpening += datagrid_ToolTipOpening;

                    //GLOBAL_SYMBOLS_LIST_PANEL = datagrid;
                    symbols_list_window.Tag = datagrid;  //  для обработчика Validation при открытии окна

                        datagrid.SetValue(Grid.ColumnProperty, 0);
                        //datagrid.SetValue(Grid.RowProperty, 1);
                        datagrid.AutoGenerateColumns = false ;
                        //datagrid.CanUserAddRows = true;
                        //datagrid.CanUserDeleteRows = false;
                        datagrid.MaxHeight = 300;
                        datagrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                        datagrid.GridLinesVisibility = DataGridGridLinesVisibility.All;
                        //datagrid.AlternatingRowBackground = new SolidColorBrush(Colors.AliceBlue);
                        //datagrid.AlternationCount = 2;                        //datagrid.Background = new SolidColorBrush(Colors.AliceBlue);
                        datagrid.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                        datagrid.BorderThickness = new Thickness(0.5);
                        datagrid.VerticalAlignment = VerticalAlignment.Top;
                            Style style = new Style() ;
                            //style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.AliceBlue)));
                            style.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.AliceBlue )));
                            style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness( 0.5 )));
                     //   datagrid.ColumnHeaderStyle = style;
                        //datagrid.Style = style;
                        datagrid.CellStyle = style;
                        datagrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                        datagrid.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                        //datagrid.HorizontalContentAlignment = HorizontalAlignment.Center;
                        datagrid.RowHeight = 30;
                        //datagrid.MouseRightButtonDown += datagrid_MouseRightButtonDown;
                        datagrid.Tag = x_symbols_list; //  для обработчика добавления строк.
                        
                        //  чтобы автоматически не добавлялась пустая строка снизу "PlaceHolder", которая еще мешает и Validation.
                        datagrid.CanUserAddRows = false;

                        datagrid.IsReadOnly = DEBUG.IsEnable;  

                            //Binding items_binding = new Binding();
                            //items_binding.Source = this;
                            //items_binding.Path = new PropertyPath("GLOBAL_SYMBOLS_LIST");
                            ////items_binding.Converter = new List_converter();
                            //items_binding.Mode = BindingMode.TwoWay;
                        //datagrid.SetBinding(DataGrid.ItemsSourceProperty, items_binding);

                        if (x_symbols_list is ObservableCollection<Symbol_Data>)
                        {
                            datagrid.ItemsSource = x_symbols_list as ObservableCollection<Symbol_Data>;
                        }
                        else if (x_symbols_list is ObservableCollection<Indication_Symbol_Data>)
                        {
                            datagrid.ItemsSource = x_symbols_list as ObservableCollection<Indication_Symbol_Data>;
                        }
        
                        //  обработчик для установки IsReadOnly.
                        //datagrid.PreviewMouseLeftButtonDown += datagrid_MouseLeftButtonDown;                            

                        //  обработчик для вызова ручного Validation_By_Event 
                        datagrid.CurrentCellChanged += DataGrid_CurrentCellChanged;

                    grid.Children.Add(datagrid);
                    grid.Children.Add(err_groupbox);

                xborder.Child = grid;

//**********  ADD COLUMNS & BUTTONS

                show_symbols_list(symbols_list_window);

//**********  SHOW WINDOW

            symbols_list_window.Content = xborder; 

            if (code == "Show") symbols_list_window.Show();
            else if (code == "ShowDialog") symbols_list_window.ShowDialog();
            else throw new Exception("<SHOW_SYMBOLS_LIST_window()>: Unknown code <Refresh>");
        
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return ;

}

//**************    Всплывающий Вывод ошибок валидации.
//void datagrid_ToolTipOpening(object sender, ToolTipEventArgs e)
//{

//    if (String.IsNullOrWhiteSpace(SYMBOLS_VALIDATION_ERRORS)) ((DataGrid)sender).ToolTip = null;//e.Handled = true;
//    else ((DataGrid)sender).ToolTip = SYMBOLS_VALIDATION_ERRORS;
//}

//**************   REFRESH WINDOW

public void REFRESH_SYMBOLS_LIST_window(Window symbols_list_window)
{
    try
    {
            if (symbols_list_window != null && symbols_list_window.Tag != null)
            {
                DataGrid datagrid = (DataGrid)symbols_list_window.Tag;

                System.Collections.IEnumerable itemssource = datagrid.ItemsSource;
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = itemssource;
            }
        
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}



//***************   SHOW_GLOBAL_SYMBOLS_LIST

public Border SHOW_GLOBAL_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {

        DataGrid datagrid = symbols_list_window.Tag as DataGrid;

        Grid grid = datagrid.Parent as Grid;

//****************   COLUMNS

// 123 InputBitPanel	M     100.3	BOOL	Comment

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        DataGridComboBoxColumn memory_type_column = new DataGridComboBoxColumn();
                        DataGridComboBoxColumn bit_base_column = new DataGridComboBoxColumn();
                        DataGridTemplateColumn bit_number_column = DataGridUserComboBoxColumn("str_Bit_Number", new Bit_number_column_Converter()); 
                        //DataGridComboBoxColumn bit_number_column = new DataGridComboBoxColumn();
                        DataGridComboBoxColumn data_type_column = new DataGridComboBoxColumn();
                        DataGridTextColumn initial_value_column = new DataGridTextColumn();
                        DataGridTextColumn real_value_column = new DataGridTextColumn();
                        DataGridTextColumn address_column = new DataGridTextColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

                        DataGridComboBoxColumn nom_column = new DataGridComboBoxColumn();
                        DataGridTextColumn k_column = new DataGridTextColumn(); 
                        DataGridTextColumn unit_column = new DataGridTextColumn();
                        DataGridComboBoxColumn format_column = new DataGridComboBoxColumn();
                        
//****************   COLUMNS  READ ONLY

                        item_number_column.IsReadOnly = true;
                        real_value_column.IsReadOnly = true;
                        address_column.IsReadOnly = true;

                        //Binding  read_only_binding = new Binding();
                        ////read_only_binding.Source = ...
                        //read_only_binding.Path = new PropertyPath("Memory_Type");
                        //read_only_binding.Converter = new Item_Comment_read_only_Converter();
                        //read_only_binding.ConverterParameter = comment_column;
                        //comment_column.Binding = read_only_binding;
                        
                        ////comment_column.IsReadOnly = true;

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        memory_type_column.Header   = "Memory";
                        bit_base_column.Header      = "Bit base";
                        bit_number_column.Header    = "Bit num";
                        data_type_column.Header     = "Data type";
                        initial_value_column.Header = "Initial value";
                        address_column.Header       = "Address";
                        real_value_column.Header    = "Value";

                        comment_column.Header       = "Comment";

                        nom_column.Header           = "Nom";
                        k_column.Header             = "K";
                        unit_column.Header          = "Units";
                        format_column.Header        = "Format";

//**************

                        item_number_column.Width    =  30;
                        name_column.Width           = 100;
                        memory_type_column.Width    =  70;
                        bit_base_column.Width       = 100;
                        bit_number_column.Width     =  70;
                        data_type_column.Width      =  70;
                        initial_value_column.Width  =  70;
                        real_value_column.Width     = 70 ;
                        address_column.Width        = 70;
                        comment_column.Width        = 200;

                        nom_column.Width = 70;
                        k_column.Width = 50;
                        unit_column.Width = 50;
                        format_column.Width = 70;

//DataGrid.IsReadOnlyProperty
 //   address_column.C
//    datagrid.Row
/*  ??? DataGridCell.IsReadOnly 
                
        Binding cell_binding = new Binding();
        cell_binding.Source = datagrid;
        cell_binding.Converter = new CellEdit_Converter();
        cell_binding.Path = new PropertyPath("CurrentItem");

        datagrid.SetBinding(DataGrid.CurrentCellProperty, cell_binding);
*/
        
//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            //cell_binding.Source = ...
                            //item_number_column_binding.Path = ... //new PropertyPath("Name");
                            //item_number_column_binding.Mode = BindingMode.TwoWay;
                            //if (i == 0) cell_binding.StringFormat = "00";
                            //else cell_binding.StringFormat = d_CalcStringformat(INPUT_list[i-1]);
                            item_number_column_binding.Converter = new Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = datagrid.Tag;//GLOBAL_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                                //name_column_binding.ValidationRules.Add(new Name_ValidationRule(16)); 
                                //// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                ////name_column_binding.NotifyOnValidationError = true;
                            name_column.Binding = name_column_binding;
        

                            memory_type_column.ItemsSource = GLOBAL_MEMORY_TYPES;
                            Binding memory_type_column_binding = new Binding();
                            memory_type_column_binding.Path = new PropertyPath("Memory_Type");
                            memory_type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            memory_type_column.SelectedItemBinding = memory_type_column_binding;


                            Binding address_column_binding = new Binding();
                            address_column_binding.Path = new PropertyPath("str_Address");
                            address_column_binding.Mode = BindingMode.TwoWay;
                            //// его использовали вместо Validation address_column_binding.Converter = new Address_column_Converter();
                            ////address_column_binding.ValidationRules.Add(new Adress_ValidationRule(0, 65534));
                            //// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                            ////address_column_binding.NotifyOnValidationError = true;
                            //  конвертер, чтобы приравнивать initial_value к address.
                            //address_column_binding.Converter = new Initial_value_column_Converter();
                            //address_column_binding.ConverterParameter = datagrid;
                            address_column.Binding = address_column_binding;


                            //bit_base_column.ItemsSource = Get_BitBaseSymbols_List(GLOBAL_SYMBOLS_LIST);
                            Binding bit_base_items_source_binding = new Binding();
                                bit_base_items_source_binding.Source = datagrid;
                                bit_base_items_source_binding.Path = new PropertyPath("SelectedItem");
                                bit_base_items_source_binding.ConverterParameter = datagrid.Tag;
                                bit_base_items_source_binding.Converter = new Bit_base_items_source_Converter();
                            BindingOperations.SetBinding(bit_base_column, DataGridComboBoxColumn.ItemsSourceProperty, bit_base_items_source_binding);   
                            Binding bit_base_column_binding = new Binding();
                                bit_base_column_binding.Path = new PropertyPath("Bit_Base");
                                bit_base_column_binding.Mode = BindingMode.TwoWay;
                                //bit_number_column_binding.ValidationRules.Add(new BitNumber_ValidationRule());
                                //// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                ////bit_number_column_binding.NotifyOnValidationError = true;
                            bit_base_column.SelectedItemBinding = bit_base_column_binding;


                            //bit_number_column.ItemsSource = BIT_NUMBERS;                               
                            /*Binding bit2_number_column_binding = new Binding();
                                bit2_number_column_binding.Source = datagrid;
                                bit2_number_column_binding.Path = new PropertyPath("SelectedItem");
                                bit2_number_column_binding.Converter = new Bit_number_column_Converter();
                                //bit2_number_column_binding.ConverterParameter = ;
                            BindingOperations.SetBinding(bit_number_column, DataGridComboBoxColumn.ItemsSourceProperty, bit2_number_column_binding);   
                            Binding bit_number_column_binding = new Binding();
                                bit_number_column_binding.Path = new PropertyPath("str_Bit_Number");
                                bit_number_column_binding.Mode = BindingMode.TwoWay;
                                //bit_number_column_binding.ValidationRules.Add(new BitNumber_ValidationRule());
                                //// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                ////bit_number_column_binding.NotifyOnValidationError = true;
                            bit_number_column.SelectedItemBinding = bit_number_column_binding;
                            */

                            data_type_column.ItemsSource = GLOBAL_DATA_TYPES;
                            Binding data_type_column_binding = new Binding();
                            data_type_column_binding.Path = new PropertyPath("Data_Type");
                            data_type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            data_type_column.SelectedItemBinding = data_type_column_binding;


                            Binding initial_value_column_binding = new Binding();
                            initial_value_column_binding.Path = new PropertyPath("fstr_Initial_Value");
                            //initial_value_column_binding.Path = new PropertyPath("Initial_Value");
                            initial_value_column_binding.Mode = BindingMode.TwoWay;
                            ////// его использовали вместо Validation address_column_binding.Converter = new Address_column_Converter();
                            ////initial_value_column_binding.ValidationRules.Add(new InitialValue_ValidationRule(-65534, 65535));
                            ////// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                            //////address_column_binding.NotifyOnValidationError = true;
                            ////initial_value_column_binding.Converter = new Initial_value_column_Converter();
                            ////initial_value_column_binding.ConverterParameter = datagrid;
                            initial_value_column.Binding = initial_value_column_binding;


                            Binding real_value_column_binding = new Binding();
                            real_value_column_binding.Path = new PropertyPath("fstr_Real_Value");
                            real_value_column_binding.Mode = BindingMode.TwoWay;
                            real_value_column.Binding = real_value_column_binding;


                            Binding comment_column_binding = new Binding();
                            comment_column_binding.Path = new PropertyPath("Comment");
                            comment_column_binding.Mode = BindingMode.TwoWay;
                            comment_column.Binding = comment_column_binding;

                //*******   Additional data

                            List<string> items_source = new List<string>();
                                foreach (Symbol_Data symbol in NOMS_SYMBOLS_LIST) items_source.Add(symbol.Name);
                            nom_column.ItemsSource = items_source;
                            Binding nom_column_binding = new Binding();
                            nom_column_binding.Path = new PropertyPath("str_Nom_Value");
                            nom_column_binding.Mode = BindingMode.TwoWay;
                            nom_column.SelectedItemBinding = nom_column_binding;

                            Binding k_column_binding = new Binding();
                            k_column_binding.Path = new PropertyPath("str_K_Value");
                            k_column_binding.Mode = BindingMode.TwoWay;
                            k_column.Binding = k_column_binding;

                            Binding unit_column_binding = new Binding();
                            unit_column_binding.Path = new PropertyPath("Unit_Value");
                            unit_column_binding.Mode = BindingMode.TwoWay;
                            unit_column.Binding = unit_column_binding;

                            format_column.ItemsSource = FORMAT_TYPES.Keys;
                            Binding format_column_binding = new Binding();
                            format_column_binding.Path = new PropertyPath("Format_Value");
                            format_column_binding.Mode = BindingMode.TwoWay;
                            format_column.SelectedItemBinding = format_column_binding;


//****************   MULTIBINDING 

                          /* Замучался: вызывается по пять раз на один клик для одной ячейки, передает значение UnsetValue
                           * нельзя сделать в обратном конвертере масштабирование возвращаемого значения под номиналы, т.к. нет 
                           * доступа к  привязанным свойствам. Решил создать свое DependancyProperty.
                           * MultiBinding multi_binding = new MultiBinding();
                            multi_binding.Bindings.Add(data_type_column_binding);
                            multi_binding.Bindings.Add(address_column_binding);
                            multi_binding.Bindings.Add(nom_column_binding);
                            multi_binding.Bindings.Add(initial_value_column_binding);
                            multi_binding.Bindings.Add(format_column_binding); 
                            multi_binding.Converter = new MultiBinding_Converter();
                            initial_value_column.Binding = multi_binding;
        */
//****************   BINDING GROUP  for  VALIDATION
        //            перешел от Group_Validation встроенного на вручную по событиям
        //            после того как добавил TemplateColumn для TagName, которая не имеет Binding и 
        //            соотв. GroupValidation не может проверить TagNames.

                        /*
                            BindingGroup bindinggroup = new BindingGroup();
                            bindinggroup.NotifyOnValidationError = true;
                            bindinggroup.ValidationRules.Add( new Group_ValidationRule());
                            bindinggroup.ValidationRules.Add(new ExceptionValidationRule());
                        //  привязываем Binding не ко всей datagrid, а только к row-строке.
                        datagrid.ItemBindingGroup = bindinggroup;
                        //datagrid.BindingGroup = bindinggroup;

                        //    Обработчик вывода сообщений для всех Validation общий.
                        // можно и так и так Validation.AddErrorHandler(datagrid, new EventHandler<ValidationErrorEventArgs>(Address_ValidationErrorEvent));
                        datagrid.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(ValidationErrorEvent));
                        */

//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
                        datagrid.Columns.Add(data_type_column);
                        datagrid.Columns.Add(memory_type_column);
                        datagrid.Columns.Add(bit_base_column);
                        datagrid.Columns.Add(bit_number_column);
                        datagrid.Columns.Add(initial_value_column);
                        datagrid.Columns.Add(real_value_column);
                        datagrid.Columns.Add(address_column);

                        datagrid.Columns.Add(nom_column);
                        datagrid.Columns.Add(k_column);
                        datagrid.Columns.Add(unit_column);
                        datagrid.Columns.Add(format_column);

                        datagrid.Columns.Add(comment_column);


//****************   BUTTONS  **************************

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Top;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);
                        //stackpanel.SetValue(Grid.RowProperty, 1);

                        Button button_AddRow = Get_Button("Add Row", button_AddRow_Click, symbols_list_window);
                        Button button_InsertRow = Get_Button("Insert Row", button_InsertRow_Click, symbols_list_window);
                        Button button_DeleteRow = Get_Button("Delete Row", button_DeleteRow_Click, symbols_list_window);
                        Button button_CopyRow = Get_Button("Copy Row", button_CopyRow_Click, symbols_list_window);
                        Button button_BindToElement = Get_Button("Bind to Element", button_BindToElement_Click, symbols_list_window);
                        Button button_Cancel = Get_Button("Ok/Cancel", button_Cancel_Click, symbols_list_window);

                             
                        // иначе по cancel окно закрывается безусловно  button_Cancel.IsCancel = true;
                        FocusManager.SetFocusedElement(symbols_list_window, button_Cancel);    
        
                        stackpanel.Children.Add(button_AddRow);
                        stackpanel.Children.Add(button_CopyRow);
                        stackpanel.Children.Add(button_InsertRow);
                        stackpanel.Children.Add(button_DeleteRow);
                        stackpanel.Children.Add(button_BindToElement);
                        stackpanel.Children.Add(button_Cancel);

                    //grid.Children.Add(datagrid);
                    grid.Children.Add(stackpanel);

                //xborder.Child = grid;

        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return null;

}

/*
*                            //datagrid.CellEditEnding += datagrid_CellEditEnding;
                            //datagrid.SelectedCellsChanged += datagrid_SelectedCellsChanged;
                            //datagrid.SelectionChanged += datagrid_SelectionChanged;
                            //datagrid.TargetUpdated += datagrid_TargetUpdated;
                        datagrid.CurrentCellChanged += datagrid_CurrentCellChanged;


static int xxxcode = 1;

void datagrid_CurrentCellChanged(object sender, EventArgs e)
{

    if (xxxcode++ < 3) return;
    xxxcode = 0;


    System.Collections.IEnumerable itemssource = ((DataGrid)sender).ItemsSource;
    ((DataGrid)sender).ItemsSource = null;
    ((DataGrid)sender).ItemsSource = itemssource;
    //MessageBox.Show("CurrentCellChanged");
    
}

void datagrid_TargetUpdated(object sender, DataTransferEventArgs e)
{
    System.Collections.IEnumerable itemssource = ((DataGrid)sender).ItemsSource;
    ((DataGrid)sender).ItemsSource = null;
    ((DataGrid)sender).ItemsSource = itemssource;
    MessageBox.Show("TargetUpdated");

}

void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    System.Collections.IEnumerable itemssource = ((DataGrid)sender).ItemsSource;
    ((DataGrid)sender).ItemsSource = null;
    ((DataGrid)sender).ItemsSource = itemssource;
    //MessageBox.Show("SelectionChanged");

}

void datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
{
    System.Collections.IEnumerable itemssource = ((DataGrid)sender).ItemsSource;
    ((DataGrid)sender).ItemsSource = null;
    ((DataGrid)sender).ItemsSource = itemssource;
    //MessageBox.Show("111");
}

void datagrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
{
    //((DataGrid)sender).Visibility = Visibility.Visible;
}

void datagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
{
    
    System.Collections.IEnumerable itemssource =  ((DataGrid)sender).ItemsSource ;
    ((DataGrid)sender).ItemsSource = null;
    ((DataGrid)sender).ItemsSource = itemssource;

    //MessageBox.Show("111");
}
*/

//****************

public static Button Get_Button(string Name,  RoutedEventHandler handler, Window window)
{
    try
    {
        Button button = new Button();
        button.Content = Name;
        //button3.FontSize = MARK_font_size;
        button.Margin = new Thickness(5);
        button.Click += handler;
        button.MinWidth = 75;
        button.VerticalAlignment = VerticalAlignment.Bottom;
        button.HorizontalAlignment = HorizontalAlignment.Stretch;
        button.Tag = window;

        return button;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}

//****************   DataGrid Udser ComboBoxColumn

                        /*<DataGridTemplateColumn Header="Машины">
                            <DataGridTemplateColumn.CellTemplate>
                                <DataTemplate>
                                    <ComboBox ItemsSource="{Binding Cars}">
                                        <ComboBox.ItemTemplate>
                                            <DataTemplate>
                                                <TextBlock Text="{Binding Name}"/>
                                            </DataTemplate>
                                        </ComboBox.ItemTemplate>
                                    </ComboBox>
                                </DataTemplate>
                            </DataGridTemplateColumn.CellTemplate>
                        </DataGridTemplateColumn>
                        */

//    Перешел от DataGridComboBoxColumn к DataGridTemplateColumn, которая позволяет назначать каждой строке в колонке 
//  свой список ItemsSource в отличие от DataGridComboBoxColumn где назначается один ItemsSource для всех строк колонки 
//  и переназначение его невозможно.

public static DataGridTemplateColumn DataGridUserComboBoxColumn(string property_path, IValueConverter items_source_converter,
                                                                string header = "NoName", double width = 100.0)
{
    try  
    {

            DataGridTemplateColumn column_template = new DataGridTemplateColumn();
            column_template.Header = header;
            column_template.Width = width;
      

//************   CELL TEMPLATE: TEXTBLOCK 

                    DataTemplate textblock_template = new DataTemplate();

                        FrameworkElementFactory textblock = new FrameworkElementFactory(typeof(TextBlock));
                        //textblock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                        //textblock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                        //textblock.SetValue(TextBlock.MinWidthProperty, 50.0);

                            Binding textblock_binding = new Binding();
                            textblock_binding.Path = new PropertyPath(property_path);

                        textblock.SetBinding(TextBlock.TextProperty, textblock_binding);

                    textblock_template.VisualTree = textblock;
        
                column_template.CellTemplate = textblock_template;

//************   CELL EDIT TEMPLATE:  COMBOBOX 

                    DataTemplate combobox_template = new DataTemplate();

                        FrameworkElementFactory combobox = new FrameworkElementFactory(typeof(ComboBox));

                            Binding combobox_binding = new Binding();
                            combobox_binding.Path = new PropertyPath(property_path);
                            combobox_binding.Mode = BindingMode.TwoWay;

                        combobox.SetBinding(ComboBox.TextProperty, combobox_binding);

                            Binding combobox_items_binding = new Binding();
                            //combobox_items_binding.Source = datagrid;
                            //combobox_items_binding.Path = new PropertyPath("SelectedItem");
                            combobox_items_binding.Converter = items_source_converter;
                            //bit2_number_column_binding.ConverterParameter = ;

                        combobox.SetBinding(ComboBox.ItemsSourceProperty, combobox_items_binding);
        
                    combobox_template.VisualTree = combobox;
//**************

                column_template.CellEditingTemplate = combobox_template;

            return column_template;

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }
        


//***************   CONVERTERS

//**************    MULTI BINDING CONVERTER 

public class MultiBinding_Converter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        try
        {

            // Вынимаем в том порядке в котором ложили в multi_binding.Bindings.Add(...).

            string data_type = (values[0]).ToString();

            string address = (values[1]).ToString();

            string nom_value = (values[2]).ToString();

            string initial_value;

                if (values[3]!=null)  initial_value = (values[3]).ToString();
                else                  initial_value = "";
                
            string format_value = (values[4]).ToString();

            //  Для типа COUNTER  Initial_Value = Address.

            if (data_type == "COUNTER") // Data_Type
            {
                return address;
            }
            else
            {
                        string str = NOMS_SYMBOLS_LIST.First( value => value.Name == nom_value).Initial_Value;
                        int ax;
                        if (str == null) ax = 1;
                        else if (str == "") ax = 1;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (int.TryParse(str, NumberStyles.Any, null, out ax)) { }
                        else ax = 1;
                        
                        double dax;
                        if (initial_value == null) return null;
                        else if (initial_value == "") return null;
                        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        else if (double.TryParse(initial_value, NumberStyles.Any, null, out dax)) return (dax / (double)ax /* * (double)K_Value*/).ToString(FORMAT_TYPES[format_value][1]); 
                        else return null;
            }
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



public class Item_number_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            return ((ObservableCollection<Symbol_Data>)parameter).IndexOf((Symbol_Data)value)+1;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
            //if ((bool)value == true) return parameter;
            //else
            {
                MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                return 0;
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}
//***********

public class Bit_base_items_source_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {

            return Get_BitBaseSymbols_List((ObservableCollection<Symbol_Data>)parameter);

            //return ((ObservableCollection<Symbol_Data>)parameter).IndexOf((Symbol_Data)value) + 1;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return BIT32_NUMBERS;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
            //if ((bool)value == true) return parameter;
            //else
            {
                MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                return 0;
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}

//***********
public class Bit_number_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            //  пока ничего не выбрано берем максимальный список чтобы всем угодить.
            if (value == null) return BIT32_NUMBERS;

            string base_bit_type = ((Symbol_Data)value).str_Bit_Base_Type;//null;

/*            //  ищем блок данных переменной BIT_BASE по ее имени.
            foreach (SYMBOLS.Symbol_Data symbol in ((Symbol_Data)value).Owner)
            {
                if (symbol.Name == ((Symbol_Data)value).Bit_Base)
                {
                    base_bit_type = symbol.Data_Type;
                    break;
                }
            }
 */ 
            //  переменная не найдена.
            if (base_bit_type == null) return BIT32_NUMBERS; // иначе вся колонка перестает показывать номера битов и где надо и где не надо

            if (base_bit_type == "BYTE") return BIT8_NUMBERS;
            if (base_bit_type == "WORD") return BIT16_NUMBERS;
            if (base_bit_type =="DWORD") return BIT32_NUMBERS;
            if (base_bit_type == "TIMER") return TIMER_BIT_NUMBERS;
            
            return BIT32_NUMBERS;
            
            //return ((ObservableCollection<Symbol_Data>)parameter).IndexOf((Symbol_Data)value) + 1;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return BIT32_NUMBERS;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
            //if ((bool)value == true) return parameter;
            //else
            {
                MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                return 0;
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}

/*public class Initial_value_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  
        {
            //  Делаем Для типа COUNTER  Initial_Value = Address.
 
            if (parameter != null && (Symbol_Data)((DataGrid)parameter).SelectedItem != null)
            {
                if (((Symbol_Data)((DataGrid)parameter).SelectedItem).Data_Type == "COUNTER") // Data_Type
                {
                    ((Symbol_Data)((DataGrid)parameter).SelectedItem).str_Initial_Value = (string)value;
                }
            }
            
            //  значение адреса пропускаем сквозняком.
            //  работаем только с начальным значением.
            return value; 

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
            return value;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}
 */ 
//***********



/*public class Comment_column_read_only_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            Symbol_Data symbol = (Symbol_Data)((DataGrid)parameter).CurrentItem;

            if (symbol != null)
            {
                if (symbol.Memory_Type == "BI" || symbol.Memory_Type == "BQ" || symbol.Memory_Type == "BIB" ||
                     symbol.Memory_Type == "BQB" || symbol.Memory_Type == "BIW" || symbol.Memory_Type == "BQB")
                {
                    ((DataGrid)parameter).IsReadOnly = true;
                }
                else ((DataGrid)parameter).IsReadOnly = false;
            }
            //if ((string)value == "111") ((DataGrid)parameter).CurrentItem.  IsReadOnly = true;

            return value;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
                return value;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}
*/



/*
public class Initial_value_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            if ((int?)value != null) return value;
            else return null; // "---";
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
            return value;
            //if ((bool)value == true) return parameter;
            //else
            //{
             //   MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
               // return 0;
            //}
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}
*/
/*
public class CellEdit_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            Symbols_data item = (Symbols_data)value;
            if (item != null)
            {

                if (item.Type == "CONST") return true;
                else return false;
            }
            return true;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {
            //if ((bool)value == true) return parameter;
            //else
            {
                MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                return 0;
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}
*/

/*  Использовал для контроля правильности ввода до того как применил Validation.
public class Address_column_Converter : IValueConverter
{
    int old_value ;

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            old_value = (int)value;

            return value;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        try
        {  //  Проверка правильности ввода работает и так, но по теории ее надо делать через Validation.
           // Кроме того Validation еще может подсвечивать окно с ошибкой/
/*            int ax;

            if ( Int32.TryParse((string)value, out ax) ) return ax;
            else
            {
                //MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                return old_value ;
            }
 * /
            return value;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

}
*/

//***************   VALIDATION BY EVENT

    //            перешел от Group_Validation встроенного на вручную по событиям
    //            после того как добавил TemplateColumn для TagName, которая не имеет Binding и 
    //            соотв. GroupValidation не может проверить TagNames.

static void SYMBOLS_LIST_window_ContentRendered(object sender, EventArgs e)
{
    try
    {
        
        if (((DataGrid)((Window)sender).Tag).Tag is ObservableCollection<Symbol_Data>)
        {
            Validation_By_Event((ObservableCollection<Symbol_Data>)((DataGrid)((Window)sender).Tag).Tag, (DataGrid)((Window)sender).Tag);    
        }
        else if (((DataGrid)((Window)sender).Tag).Tag is ObservableCollection<Indication_Symbol_Data>)
        {
            Indication_Validation_By_Event((ObservableCollection<Indication_Symbol_Data>)((DataGrid)((Window)sender).Tag).Tag, (DataGrid)((Window)sender).Tag);    
        }
        else
        {
            throw new Exception("Error in <SYMBOLS_LIST_window_ContentRendered(...)>: Unknown <Symbols list>");
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}



//static int takt = 1; // что бы не при каждом щелчке мышью выскакивало сообщение, а то не возможно работать.
static void DataGrid_CurrentCellChanged(object sender, EventArgs e)
{
    try
    {
  //      takt *= -1;
    //    if (takt == 1)
        {

            DataGrid data_grid = (DataGrid)sender;

            //---      Чтобы при изменении ячейки Nom сразу обновлялись связанные с нею Min_Value и т.д.:
            //---  Завершаем редактирование всего ряда по завершению редактирования одной ячейки, 
            //---  чтобы ее данные срабатывали и обновлялись связанные с нею ячейки.
            data_grid.CommitEdit(DataGridEditingUnit.Row, true);
            //---


            if ( data_grid.Tag is ObservableCollection<Symbol_Data>)
            {
                Validation_By_Event((ObservableCollection<Symbol_Data>)data_grid.Tag, data_grid);
            }
            else if (data_grid.Tag is ObservableCollection<Indication_Symbol_Data>)
            {
                Indication_Validation_By_Event((ObservableCollection<Indication_Symbol_Data>)data_grid.Tag, data_grid);
            }
            else
            {
                throw new Exception("Error in <DataGrid_CurrentCellChanged(...)>: Unknown <Symbols list>");
            }
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


static public string Validation_By_Event (ObservableCollection<Symbol_Data> symbols_list, DataGrid datagrid_panel, bool message = true)
{

    //Base_Validation_Ok = false;

    ValidationResult valid_res;

    StringBuilder error_str = new StringBuilder();

    int symbol_index = 0;

    SYMBOLS.Symbol_Data item_first_error = null;

    if (symbols_list == BASE_TAGS_SYMBOLS_LIST) return null;

    try
    {

        for (int i = 0; i < symbols_list.Count ; i++) 
        {
            symbol_index++;

            SYMBOLS.Symbol_Data symbol = symbols_list[i];


            //   Общее для всех Validation
            if (symbol.Data_Type == "BOOL" || symbol.Data_Type == "TIMER")
            {
                symbol.str_Nom_Value = "";
                symbol.K_Value = 1.0;
                symbol.Step_Value = 0.0;
                symbol.Min_Value = 0.0;
                symbol.Max_Value = 0.0;
                symbol.Unit_Value = "";
                symbol.Format_Value = "";
            }

            if (symbol.Data_Type != "BOOL")
            {
                symbol.Bit_Base = "";
                symbol.str_Bit_Number = "";
            }

//********   NAME VALIDATION
//********   COMMENT  VALIDATION
//********   NAME DUPLICATING VALIDATION

            if ((valid_res = NAME_COMMENT_Validation(symbol_index, symbol, symbol.Name, symbol.Comment)) != null)
            {
                error_str.Append("\n" + valid_res.ErrorContent.ToString());
                if (item_first_error == null) item_first_error = symbol;
                //break;
            }

//********   TAG VALIDATION

            if (symbols_list == BASE_SYMBOLS_LIST)
            {
                if ((valid_res = TAG_Validation(symbol_index, symbol)) != null)//.Tag_Name, symbol.Data_Type, symbol.Memory_Type)) != null)
                {
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }
            }

//********   DATA TYPES VALIDATION

            if (symbols_list != NOMS_SYMBOLS_LIST)
            {
                if ((valid_res = DATA_TYPE_Validation(symbol_index, symbol.Data_Type)) != null)
                {
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }
            }

//********   MEMORY-DATA TYPES VALIDATION

            if (symbols_list == GLOBAL_SYMBOLS_LIST || 
                symbols_list == BASE_SYMBOLS_LIST  ||
                symbols_list == STORABLE_SYMBOLS_LIST )
            {
                if ((valid_res = MEMORY_TYPE_Validation(symbol_index, symbol.Memory_Type, symbol.Data_Type)) != null)
                {
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }
            }


            foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
            {
                if (symbols_list == diagram.LOCAL_SYMBOLS_LIST)
                {
                    if ((valid_res = LOCAL_MEMORY_TYPE_Validation(symbol_index, symbol.Memory_Type, symbol.Data_Type)) != null)
                    {
                        error_str.Append("\n" + valid_res.ErrorContent.ToString());
                        if (item_first_error == null) item_first_error = symbol;
                        //break;
                    }
                    break;
                }
            }
          

//********   ADDRESS VALIDATION

         //КВВ 29-05-18
         //   if (symbols_list == GLOBAL_SYMBOLS_LIST)
         //   {
         //       if ((valid_res = ADDRESS_Validation(symbol_index, symbol.str_Address, symbol.Data_Type)) != null)
         //       {
         //           error_str.Append("\n" + valid_res.ErrorContent.ToString());
         //           if (item_first_error == null) item_first_error = symbol;
         //           //break;
         //       }
         //   }

//********   BIT BASE & NUMBER VALIDATION

            if ((valid_res = BIT_Validation(symbol_index, symbol.Bit_Base, symbol.str_Bit_Number, symbol.Data_Type, symbols_list)) != null)
            {
                error_str.Append("\n" + valid_res.ErrorContent.ToString());
                if (item_first_error == null) item_first_error = symbol;
                //break;
            }


//********   INITIAL VALUE VALIDATION

            if ((valid_res = INITIAL_VALUE_Validation(symbol_index, symbol)) != null)
            {
                error_str.Append("\n" + valid_res.ErrorContent.ToString());
                if (item_first_error == null) item_first_error = symbol;
                //break;
            }

//********   NOM K UNIT FORMAT VALUE VALIDATION

            if (symbols_list == GLOBAL_SYMBOLS_LIST || symbols_list == STORABLE_SYMBOLS_LIST)
            {
                if ((valid_res = NOM_K_UNIT_FORMAT_Validation(symbol_index, symbol)) != null)
                {
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }
            }

//********   STORABLE DATA VALIDATION  -   должно идти после проверки InitialValue.

            if (symbols_list == STORABLE_SYMBOLS_LIST)
            {
                if ((valid_res = STORABLE_DATA_Validation(symbol_index, symbol)) != null)
                {
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }
            }


//********  END

        }

        //---  Не создано ли на поле несколько меток привязанных к одному и тому же имени.

        foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        {
            if (symbols_list == diagram.LOCAL_SYMBOLS_LIST)
            {
                string str;
                if ((str = Check_DiagramLabelDuplication(diagram)) != null)
                {
                    error_str.Append("\n" + str);
                }
            }
        }

        if (error_str.Length == 0)
        {
            //Base_Validation_Ok = true;
            SYMBOLS_LIST.SYMBOLS_VALIDATION_ERRORS = "No errors";
            return null;
        }
        else
        {
            if (datagrid_panel != null)
            {
                datagrid_panel.SelectedItem = item_first_error;
                datagrid_panel.ScrollIntoView(item_first_error);
            }
            //---

            SYMBOLS_LIST.SYMBOLS_VALIDATION_ERRORS = "Errors found:" + error_str.ToString();

            //if (message) MessageBox.Show(SYMBOLS_LIST.SYMBOLS_VALIDATION_ERRORS, "Validation", MessageBoxButton.OK, MessageBoxImage.Error);

            return error_str.ToString();
        }

        
    }
    catch (Exception excp)
    {
        string msg = excp.ToString() + " Line: " + symbol_index + ".";
        MessageBox.Show(msg);
        return msg;
    }
}




//***************  VALIDATION_RULES  ***********************

//***************  GROUP VALIDATION
//            перешел от Group_Validation встроенного на вручную по событиям
//            после того как добавил TemplateColumn для TagName, которая не имеет Binding и 
//            соотв. GroupValidation не может проверить TagNames.

public class Group_ValidationRule : ValidationRule
{

    // Ensure that an item over $100 is available for at least 7 days.
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    
        try
        {

            // Сделано и здесь и по прерыванию Validation.ErrorRemoved т.к. 
            // Он не заходит в Validation если ошибка исправлена по Esc.
             Validation_Ok = false;

             ValidationResult valid_res;

            BindingGroup bg = value as BindingGroup;

            // Get the source object.
            Symbol_Data item = bg.Items[0] as Symbol_Data;

            //  Определение номера проверяемой строки.
            //  Изнутри этого класса нет доступа к коллекции SYMBOLS_LIST.
            DataGridRow datagridrow = (DataGridRow)bg.Owner;
            int item_index = datagridrow.GetIndex() + 1;
            Validation_Error_Line = item_index.ToString();

            object o_name = null;
            object o_memory_type = null;
            object o_data_type = null;
            object o_address = null;
            object o_bit_base = null;
            object o_bit_number = null;
            object o_initial_value = null;
            object o_comment = null;

            // Get the proposed values for Price and OfferExpires.

            if (!bg.TryGetValue(item, "Name", out o_name))                      { return new ValidationResult(false, "Property 'Symbol.Name' not found!");  }
            if (!bg.TryGetValue(item, "Memory_Type", out o_memory_type))        { return new ValidationResult(false, "Property 'Symbol.Memory_Type' not found!"); }
            if (!bg.TryGetValue(item, "Data_Type", out o_data_type))            { return new ValidationResult(false, "Property 'Symbol.Data_Type' not found!"); }
            if (!bg.TryGetValue(item, "str_Address", out o_address))            { return new ValidationResult(false, "Property 'Symbol.Address' not found!"); }
            if (!bg.TryGetValue(item, "Bit_Base", out o_bit_base))          { return new ValidationResult(false, "Property 'Symbol.Bit_Base' not found!"); }
            if (!bg.TryGetValue(item, "str_Bit_Number", out o_bit_number))      { return new ValidationResult(false, "Property 'Symbol.Bit_Number' not found!"); }
            if (!bg.TryGetValue(item, "Initial_Value", out o_initial_value))    { return new ValidationResult(false, "Property 'Symbol.Initial_Value' not found!"); }
            if (!bg.TryGetValue(item, "Comment", out o_comment))                { return new ValidationResult(false, "Property 'Symbol.Comment' not found!"); }

            string name = (string)o_name;
            string memory_type = (string)o_memory_type;
            string data_type = (string)o_data_type;
            string address = (string)o_address;
            string bit_base = (string)o_bit_base;
            string bit_number = (string)o_bit_number;
            string initial_value = (string)o_initial_value;
            string comment = (string)o_comment;

//********   NAME VALIDATION
//********   COMMENT  VALIDATION
//********   NAME DUPLICATING VALIDATION

            if ((valid_res = NAME_COMMENT_Validation(item_index, item, name, comment)) != null) return valid_res;


//********   MEMORY-DATA TYPES VALIDATION

            if ((valid_res = MEMORY_TYPE_Validation(item_index, memory_type, data_type)) != null) return valid_res;

//********   ADDRESS VALIDATION

            //if ((valid_res = ADDRESS_Validation(item_index, address, data_type)) != null) return valid_res;

//********   BIT BASE & NUMBER VALIDATION

            if ((valid_res = BIT_Validation(item_index, bit_base, bit_number, data_type, GLOBAL_SYMBOLS_LIST)) != null) return valid_res;

//********   INITIAL VALUE VALIDATION

            if ((valid_res = INITIAL_VALUE_Validation(item_index, item)) != null) return valid_res;


//********  END

            // и здесь и в прерывании по Validation 
            Validation_Ok = true;
            Validation_Error_Line = null;

            return ValidationResult.ValidResult;

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg);
        }
 
    }
}

//********   NAME & COMMENT VALIDATION

static public ValidationResult NAME_COMMENT_Validation(int item_index, Symbol_Data item, string name, string comment)
                                         //ObservableCollection<SYMBOLS.Symbol_Data> symbols_list)
{
    try
    {
//********   NAME VALIDATION

            // Is in range?      //   чтобы для номиналов можно было задавать пустое имя
            if (name == null || /*name.Length < 1 ||*/ name.Length > 16)
            {
                return new ValidationResult(false, "Name length must be between 1 and 16. Line: " + item_index + ".");
            }
            else if ( name.Length > 0 && Char.IsDigit(name[0]))
            {
                return new ValidationResult(false, "Name can't begin from digit. Line: " + item_index + ".");
            }
            else if (((string)name).Any(val => {   if ( (val >= 'A' && val <= 'Z') || (val >= 'a' && val <= 'z') ||
                                                        (val >= '0' && val <= '9') || (val == '#' || val <= '_') ) return false;
                                                   else return true; }))
            {
                string msg = string.Format("Name can contain only following symbols:a-z, A-Z, 0-9, '#', '_'.  Line: " + item_index + ".");
                return new ValidationResult(false, msg);
            }

//*******    ПРОВЕРКА ПОВТОРЕНИЯ ИМЕН.

            foreach (Symbol_Data symbol in GLOBAL_SYMBOLS_LIST)
            {
                if (symbol.Name == name && symbol != item )
                {
                    return new ValidationResult(false, "Name <" + name + "> already exists in Global-symbols. Line: " + (GLOBAL_SYMBOLS_LIST.IndexOf(symbol) + 1) + ".");
                }
            }

            foreach (Symbol_Data symbol in BASE_SYMBOLS_LIST)
            {
                if (symbol.Name == name && symbol != item)
                {
                    return new ValidationResult(false, "Name <" + name + "> already exists in Base-symbols. Line: " + (BASE_SYMBOLS_LIST.IndexOf(symbol) + 1) + ".");
                }
            }

            foreach (Symbol_Data symbol in MSG_SYMBOLS_LIST)
            {
                if (symbol.Name == name && symbol != item)
                {
                    return new ValidationResult(false, "Name <" + name + "> already exists in Msg-symbols. Line: " + (MSG_SYMBOLS_LIST.IndexOf(symbol) + 1) + ".");
                }
            }

            foreach (Symbol_Data symbol in STORABLE_SYMBOLS_LIST)
            {
                if (symbol.Name == name && symbol != item)
                {
                    return new ValidationResult(false, "Name <" + name + "> already exists in Storable-symbols. Line: " + (STORABLE_SYMBOLS_LIST.IndexOf(symbol) + 1) + ".");
                }
            }


            foreach( Diagram_Of_Networks diagram in DIAGRAMS_LIST)
            {
                foreach (Symbol_Data symbol in diagram.LOCAL_SYMBOLS_LIST)
                {
                    if (symbol.Name == name && symbol != item)
                    {
                        return new ValidationResult(false, "Name <" + name + "> already exists in Local-symbols of Diagram: " + diagram.NAME + ". Line: " + (diagram.LOCAL_SYMBOLS_LIST.IndexOf(symbol) + 1) + ".");
                    }
                }
            }


//********   COMMENT VALIDATION

            // Is in range?
            if ((comment.Length > 96))
            {
                return new ValidationResult(false, "Comment length must be less then 96 symbols. Line: " + item_index + ".");
            }

            return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + item_index + ".");
    }
}

//********   LABEL DUPLICATION VALIDATION

static public string Check_DiagramLabelDuplication (Diagram_Of_Networks diagram, string label_name = null)
{
    try
    {
//---  Не создано ли на поле несколько меток привязанных к одному и тому же имени:
// - проверка на повторение с заданным именем.
// - проверка на повторение уже имеющихся имен.

        Dictionary<string, string> label_names_dict = new Dictionary<string,string>();

        //---  Составляем список всех Label в диаграмме.
        foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
        {
            foreach (List<Element_Data> list in network.Network_panel_copy)
            {
                foreach (Element_Data element in list)
                {
                    if (element != null && element.Name != null && element.Name == "LABEL")
                    {
                        foreach (SYMBOLS.Symbol_Data symbol in element.IO_VARs_list)
                        {
                            if (symbol != null && symbol.Name != null)
                            {
                                // проверка на повторение с заданным именем.
                                if ( label_name != null )
                                {
                                    if ( label_name == symbol.Name) 
                                        return  "Label <" + symbol.Name + "> already exists in Network <" + 
                                                            network.NAME + ">. Diagram: <" + diagram.NAME + ">.";

                                }
                                // проверка на повторение уже имеющихся имен.
                                else
                                {
                                    if ( !label_names_dict.ContainsKey(symbol.Name)) label_names_dict.Add(symbol.Name, network.NAME);
                                    else return "Labels duplication. Label <" + symbol.Name + "> exists in Network <" + 
                                                            network.NAME + "> and Network <" + label_names_dict[symbol.Name] + 
                                                            ">. Diagram: <" + diagram.NAME + ">.";

                                }
                            }
                        }
                    }
                }
            }
        }

        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return excp.ToString();
    }

}
//********   ADDRESS  VALIDATION
/*
static public ValidationResult ADDRESS_Validation(int item_index, string address, string data_type)
{
    try
    {
        int ax;

        // Is a number?
        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
        if (!int.TryParse(address, NumberStyles.Any, null, out ax))
        {
            return new ValidationResult(false, "Address isn't a number. Line: " + item_index + ".");
        }

        if (data_type == "TIMER")
        {
            if ((ax < 0) || (ax > 7)) return new ValidationResult(false, "Address for TIMER-type must be between '0' and '7'. Line: " + item_index + ".");
        }

        if (data_type == "COUNTER")
        {
            if ((ax < 0) || (ax > 7)) return new ValidationResult(false, "Address for COUNTER-type must be between '0' and '7'. Line: " + item_index + ".");
        }

        // Is in range?
        if ((ax < 0) || (ax > 65535))
        {
            return new ValidationResult(false, "Address must be between '0' and '65535'. Line: " + item_index + ".");
        }
        // Does address align with 2 
        if ((data_type == "WORD" || data_type == "INT" || data_type == "DATE" || data_type == "S5TIME") && (ax % 2 != 0))
        {
            return new ValidationResult(false, "For WORD, INT, S5TIME, COUNTER, TIMER and DATE types address must be aligned with 2. Line: " + item_index + ".");
        }
        // Does address align with 4 
        if ((data_type == "DWORD" || data_type == "REAL" || data_type == "REAL" || data_type == "TIME" || data_type == "TOD") && (ax % 4 != 0))
        {
            return new ValidationResult(false, "For DWORD, REAL, TIME and TOD types address must be aligned with 4. Line: " + item_index + ".");
        }

        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + item_index + ".");
    }
}
*/
//********   DATA TYPE VALIDATION

static public ValidationResult DATA_TYPE_Validation(int item_index, string data_type)
{
    try
    {
        if ( string.IsNullOrWhiteSpace(data_type))
        {
            return new ValidationResult(false, "Data type isn't set. Line: " + item_index + ".");
        }
        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + item_index + ".");
    }
}

//********   MEMORY TYPE VALIDATION

static public ValidationResult MEMORY_TYPE_Validation(int item_index, string memory_type, string data_type)
{
    try
    {
        if ( string.IsNullOrWhiteSpace(memory_type))
        {
            return new ValidationResult(false, "Memory access type isn't set. Line: " + item_index + ".");
        }

        int i = 0;
        for(i = 0 ; i < GLOBAL_MEMORY_TYPES.Count ; i++)
        {
            if(memory_type.Contains(GLOBAL_MEMORY_TYPES[i])) break;
        }

            if (i == GLOBAL_MEMORY_TYPES.Count)
            {
                return new ValidationResult(false, "Unknown memory access type. Line: " + item_index + ".");
            }

        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + item_index + ".");
    }
}

/* 31-05-18 Ввели RD, RW.
 * 
static public ValidationResult MEMORY_TYPE_Validation(int item_index, string memory_type, string data_type)
{
    try
    {

        //****** Data Types
        //  1 Bit 	 1 Byte     1 Word      2 Words     
        //          (8 Bits)	(2 Bytes)	(4 Bytes)
        //                      (16 Bits)   (32 Bits)
        //  BOOL	 BYTE	    WORD	    DWORD
        //           CHAR 		
        //                      INT	        DINT
        //                      DATE	
        //                      S5TIME	
        //                                  REAL
        //                                  TIME
        //                                  TOD

        //Process image addresses (I, IB, IW, ID, Q, QB, QW, QD)
        //	Bit memory addresses (M, MB, MW, MD)
        //	Timer / counter addresses (T / C)
        //
        // Базовые переменные КТЭ:
        //  "BI", "BIB", "BIW", "BID"
        //  "BQ", "BQB", "BQW", "BQD"


        //if ( data_type == "BOOL" &&
        //(memory_type != "M" && memory_type != "I" && memory_type != "Q"))
        //{
        //return new ValidationResult(false, "For BOOL-type memory-type mast be one of: M, I, Q. Line: " + item_index + ".");
        //}


        if ((data_type == "CHAR" || data_type == "BYTE") &&
            (memory_type != "MB" && memory_type != "IB" && memory_type != "QB"))
        {
            return new ValidationResult(false, "For CHAR and BYTE-types memory-type mast be one of: MB, IB, QB. Line: " + item_index + ".");
        }
        if ((data_type == "WORD" || data_type == "INT" || data_type == "DATE" || data_type == "S5TIME") &&
            (memory_type != "MW" && memory_type != "IW" && memory_type != "QW"))
        {
            return new ValidationResult(false, "For INT, WORD, DATE and S5TIME-types memory-type mast be one of: MW, IW, QW. Line: " + item_index + ".");
        }
        if ((data_type == "DWORD" || data_type == "DINT" || data_type == "REAL" || data_type == "TIME" || data_type == "TOD") &&
            (memory_type != "MD" && memory_type != "ID" && memory_type != "QD"))
        {
            return new ValidationResult(false, "For DINT, DWORD, REAL, TIME and TOD-types memory-type mast be one of: MD, ID, QD. Line: " + item_index + ".");
        }
        if ((data_type == "COUNTER") && (memory_type != "C"))
        {
            return new ValidationResult(false, "For COUNTER-type memory-type mast be: C. Line: " + item_index + ".");
        }
        if ((data_type == "TIMER") && (memory_type != "T"))
        {
            return new ValidationResult(false, "For TIMER-type memory-type mast be: T. Line: " + item_index + ".");
        }

        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + item_index + ".");
    }
}
*/
//********   LOCAL MEMORY TYPE VALIDATION

static public ValidationResult LOCAL_MEMORY_TYPE_Validation(int item_index, string memory_type, string data_type)
{
    try
    {
        if (data_type == "LABEL")
        {
            if (memory_type != null && memory_type != "") return new ValidationResult(false, "For LABEL-type memory-type mast be empty. Line: " + item_index + ".");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(memory_type))
            {
                return new ValidationResult(false, "Memory access type isn't set. Line: " + item_index + ".");
            }
            //---

            int i = 0;
            for (i = 0; i < LOCAL_MEMORY_TYPES.Count; i++)
            {
                if (memory_type.Contains(LOCAL_MEMORY_TYPES[i])) break;
            }

            if (i == LOCAL_MEMORY_TYPES.Count)
            {
                return new ValidationResult(false, "Unknown memory access type. Line: " + item_index + ".");
            }
            //---
            
            if (memory_type != "TEMP" && ACTIV_DIAGRAM.NAME == "MAIN")
            {
                return new ValidationResult(false, "Memory type for MAIN-diagram can be 'TEMP' only. Line: " + item_index + ".");
            }

            if (memory_type != "TEMP" && data_type == "BOOL")
            {
                return new ValidationResult(false, "Data type 'BOOL' can be set for memory type 'TEMP' only. Line: " + item_index + ".");
            }
        }
        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + item_index + ".");
    }
}


//********   BIT VALIDATION

static public ValidationResult BIT_Validation ( int item_index, string bit_base, string bit_number, string data_type, 
                                         ObservableCollection<SYMBOLS.Symbol_Data> symbols_list)
{
    try
    {
//********   BIT BASE  VALIDATION

            if (data_type == "BOOL")
            {
                if (bit_base == null || bit_base.Length == 0)
                {
                    return new ValidationResult(false, "Bit base symbol isn't set for BOOL-type. Line: " + item_index + ".");
                }

                SYMBOLS.Symbol_Data base_symbol = null;
                foreach (SYMBOLS.Symbol_Data symbol in symbols_list)
                {
                    if (bit_base == symbol.Name)
                    {
                        if (symbol.Data_Type != "BYTE" && symbol.Data_Type != "WORD" &&
                            symbol.Data_Type != "DWORD" && symbol.Data_Type != "TIMER")
                        {
                            return new ValidationResult(false, "Bit base symbol data type mast be one of: BYTE, WORD, DWORD, TIMER. Line: " + item_index + ".");
                        }
                        base_symbol = symbol;
                        break;
                    }
                }

                if (base_symbol == null) return new ValidationResult(false, "Bit base symbol not found. Line: " + item_index + ".");

//********   BIT NUMBER  VALIDATION

                if (bit_number == null || bit_number.Length == 0)
                {
                    return new ValidationResult(false, "Bit number isn't set for BOOL-type. Line: " + item_index + ".");
                }

                int bit_ind ;
                          //  у TIMER-переменной биты кроме номера имеют еще имена, поэтому их проверяют отдельно.
                if (base_symbol.Data_Type == "TIMER") bit_ind = TIMER_BIT_NUMBERS.IndexOf(bit_number);
                else                                  bit_ind = BIT32_NUMBERS.IndexOf(bit_number);

                if (base_symbol.Data_Type == "BYTE" && (bit_ind < 1 || bit_ind > 8))
                {
                    return new ValidationResult(false, "Bit number for base symbol BYTE-type must be in range 0...7. Line: " + item_index + ".");
                }
                if (base_symbol.Data_Type == "WORD" && (bit_ind < 1 || bit_ind > 16))
                {
                    return new ValidationResult(false, "Bit number for base symbol WORD-type must be in range 0...15. Line: " + item_index + ".");
                }
                if (base_symbol.Data_Type == "DWORD" && (bit_ind < 1 || bit_ind > 32))
                {
                    return new ValidationResult(false, "Bit number for base symbol DWORD-type must be in range 0...31. Line: " + item_index + ".");
                }
                if (base_symbol.Data_Type == "TIMER" && (bit_ind < 1 || bit_ind > 16))
                {
                    return new ValidationResult(false, "Bit number for base symbol TIMER-type must be in range 0...15. Line: " + item_index + ".");
                }

            }
            else if ( data_type != "BOOL")
            {
                if (bit_base != null && bit_base.Length != 0) 
                {
                    return new ValidationResult(false, "Bit base symbol is valid only for BOOL-type. Line: " + item_index + ".");
                }

                if (bit_number != null && bit_number.Length != 0)
                {
                    return new ValidationResult(false, "Bit number is valid only for BOOL-type. Line: " + item_index + ".");
                }
            }

            return null;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg + " Line: " + item_index + ".");
        }
}


//********   INITIAL VALUE VALIDATION               
                                                                           
static public ValidationResult INITIAL_VALUE_Validation ( int item_index, Symbol_Data symbol)
{
    try
    {
        string initial_value = symbol.Initial_Value;
        string data_type = symbol.Data_Type;
        string address = symbol.str_Address;

        // Is a number?
        if (initial_value != null && initial_value.Length != 0)
        {
            //if (data_type != "CHAR")
            //{
            //if (!int.TryParse(initial_value, out ax))
            //{
            //return new ValidationResult(false, "Initial value isn't a number. Line: " + item_index + ".");
            //}
            //}

            //****** Data Types
            //  1 Bit 	 1 Byte     1 Word      2 Words     
            //          (8 Bits)	(2 Bytes)	(4 Bytes)
            //                      (16 Bits)   (32 Bits)
            //  BOOL	 BYTE	    WORD	    DWORD
            //           CHAR 		
            //                      INT	        DINT
            //                      DATE	
            //                      S5TIME	
            //                                  REAL
            //                                  TIME
            //                                  TOD

            //Int64 ax;

            if (data_type == "BOOL")
            {
                if (initial_value != "0" && initial_value != "1")
                {
                    return new ValidationResult(false, "Initial value for BOOL-type must be '0' or '1'. Line: " + item_index + ".");
                }
            }
            else if (data_type == "CHAR" && initial_value.Length > 1)
            {
                return new ValidationResult(false, "Initial value for CHAR-type must be 1-char long. Line: " + item_index + ".");
            }
            else if (data_type == "TIMER" && initial_value.Length >= 1) 
            {
                return new ValidationResult(false, "Initial value for TIMER-type can't be set. Line: " + item_index + ".");
            }
            else if (data_type == "FMSG" || data_type == "WMSG" || data_type == "SMSG")
            {
                    // Is in range?
                if (initial_value.Length < 1 || initial_value.Length > 14)
                {
                    return new ValidationResult(false, "Initial value length must be between 1 and 14. Line: " + item_index + ".");
                }
                else if (((string)initial_value).Any(val =>
                {
                    if ((val >= 'A' && val <= 'Z') || (val >= 'a' && val <= 'z') ||
                            (val >= '0' && val <= '9') || (val == '#' || val <= '_')) return false;
                    else return true;
                }))
                {
                    string msg = string.Format("Initial value can contain only following symbols:a-z, A-Z, 0-9, '#', '_'.  Line: " + item_index + ".");
                    return new ValidationResult(false, msg);
                }
            }
            else //  Остальные числовые типы.
            {
                string err = RANGE_Validation( symbol, initial_value);
                if ( err != null ) return new ValidationResult(false, "Initial " + err + ". Line: " + item_index + ".");
            }
        }
        else
        {
            if (data_type == "FMSG" || data_type == "WMSG" || data_type == "SMSG")
            {
                return new ValidationResult(false, "Initial value must be set for MSG-type. Line: " + item_index + ".");
            }
        }

        if (data_type == "COUNTER" && initial_value != address)
        {
            return new ValidationResult(false, "Initial value for COUNTER-type must be equal to address. Line: " + item_index + ".");
        }
        //if (data_type == "TIMER" && (ax < 0 || ax > 7))
        //{
        //return new ValidationResult(false, "Initial value for TIMER-type must be between '0' and '7'. Line: " + item_index + ".");
        //}

            return null;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg + " Line: " + item_index + ".");
        }
}


static public string RANGE_Validation ( Symbol_Data symbol, string initial_value)
{
    try
    {
        string data_type = symbol.Data_Type;

        double Kx = (double)symbol.Nom_Value / symbol.K_Value.Value;

            //if (data_type == "REAL")
            //{
                //double dax;
                //if (!double.TryParse(initial_value, out dax)) return "value isn't a number";
            //}
            //else //  Остальные типы числовые.
            //{
                double dax;

                // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                if (!Double.TryParse(initial_value, NumberStyles.Any, null, out dax)) return "value isn't a number";

                if (data_type == "BYTE") 
                {
                    if (dax < 0 || dax > +255) return "value for BYTE-type must be between '" +
                                                              (  0.0 / Kx).ToString("0") + "' and '" +
                                                              (255.0 / Kx).ToString("0") + "'";
                }
                else if (data_type == "WORD")
                {
                    if (dax < 0 || dax > UInt16.MaxValue) return "value for WORD-type must be between '" +
                                                              (    0.0 / Kx).ToString("0") + "' and '" +
                                                              (UInt16.MaxValue / Kx).ToString("0") + "'";
                }
                else if (data_type == "DWORD" || data_type == "TIME")
                {
                    if (dax < 0 || dax > UInt32.MaxValue) return "value for DWORD-type must be between '" +
                                                              (    0.0 / Kx).ToString("0") + "' and '" +
                                                        (UInt32.MaxValue / Kx).ToString("0") + "'";
                }
                else if (data_type == "INT")
                {
                    if (dax < Int16.MinValue || dax > Int16.MaxValue) return "value for INT-type must be between '" +
                                                              (Int16.MinValue / Kx).ToString("0") + "' and '" +
                                                              (Int16.MaxValue / Kx).ToString("0") + "'";
                }
                else if (data_type == "DINT")
                {
                    if (dax < Int32.MinValue || dax > Int32.MaxValue) return "value for DINT-type must be between '" +
                                                              (Int32.MinValue / Kx).ToString("0") + "' and '" +
                                                              (Int32.MaxValue / Kx).ToString("0") + "'";
                }
                else if (data_type == "REAL")
                {
                    if (dax < float.MinValue || dax > float.MaxValue ) return "value for DINT-type must be between '" +
                                                              (float.MinValue / Kx).ToString("0") + "' and '" +
                                                              (float.MaxValue / Kx).ToString("0") + "'";
                }
                else return "value unknown type";
            //}

            return null;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return excp.ToString();
        }
}

static public ValidationResult NOM_K_UNIT_FORMAT_Validation(int symbol_index, Symbol_Data symbol)
{
    try
    {
        if (symbol.Data_Type == null) return null;  //  не установленный тип проверится в другом месте

        if (symbol.Data_Type == "BOOL" || symbol.Data_Type == "TIMER")
        {
            // 20-06-18 symbol.str_Nom_Value = "" ;
            //symbol.str_K_Value = null ;
            //symbol.Unit_Value = null ;
            //symbol.Format_Value = null;

            //if (symbol.str_Nom_Value != null && symbol.str_Nom_Value != "") return new ValidationResult(false, "Nom value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.str_K_Value != null && symbol.str_K_Value != "") return new ValidationResult(false, "K value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.Unit_Value != null && symbol.Unit_Value != "") return new ValidationResult(false, "Unit value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.Format_Value != null && symbol.Format_Value != "") return new ValidationResult(false, "Format value for BOOL-type must be empty. Line: " + symbol_index + ".");
        }
        else
        {
            if (symbol.str_Nom_Value == null ) return new ValidationResult(false, "Nom value isn't set. Line: " + symbol_index + ".");
            if (symbol.str_K_Value == null ) return new ValidationResult(false, "K value isn't set. Line: " + symbol_index + ".");
            if (symbol.Format_Value == null ) return new ValidationResult(false, "Format value isn't set. Line: " + symbol_index + ".");

            //---
            if (symbol.Unit_Value.Length > 3) return new ValidationResult(false, "Unit value must be (0...3)-char long. Line: " + symbol_index + ".");
            //---
            if (symbol.Nom_Value <= 0) return new ValidationResult(false, "Nom value must be non-zero positive. Line: " + symbol_index + ".");
            //---
            if (symbol.K_Value <= 0) return new ValidationResult(false, "K-value must be non-zero positive. Line: " + symbol_index + ".");
            //---


            string err = RANGE_Validation(symbol, symbol.Nom_Value.ToString());
            if (err != null) return new ValidationResult(false, "Nom " + err + ". Line: " + symbol_index + ".");

            err = RANGE_Validation(symbol, symbol.K_Value.ToString());
            if (err != null) return new ValidationResult(false, "K " + err + ". Line: " + symbol_index + ".");

            //---

        }


        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + symbol_index + ".");
    }
}


//********   STORABLE DATA VALIDATION

static public ValidationResult STORABLE_DATA_Validation(int symbol_index, Symbol_Data symbol)
{
    try
    {
        if (symbol.Data_Type == null) return null;  //  не установленный тип проверится в другом месте

        if (symbol.Data_Type == "BOOL")
        {
            //20-06-18 symbol.Tag_Name = null;
            //symbol.Initial_Value = null;
            //symbol.Min_Value = null;
            //symbol.Max_Value = null;
            //symbol.Step_Value = null;

            //if (symbol.Tag_Name != null) return new ValidationResult(false, "Tag value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.Initial_Value != null) return new ValidationResult(false, "Initial value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.Min_Value != null) return new ValidationResult(false, "Min value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.Max_Value != null) return new ValidationResult(false, "Max value for BOOL-type must be empty. Line: " + symbol_index + ".");
            //if (symbol.Step_Value != null) return new ValidationResult(false, "Step value for BOOL-type must be empty. Line: " + symbol_index + ".");
        }
        else
        {
            if (symbol.Tag_Name == null) return new ValidationResult(false, "Tag value isn't set. Line: " + symbol_index + ".");
            if (symbol.Initial_Value == null) return new ValidationResult(false, "Initial value isn't set. Line: " + symbol_index + ".");
            if (symbol.Min_Value == null) return new ValidationResult(false, "Min value isn't set. Line: " + symbol_index + ".");
            if (symbol.Max_Value == null) return new ValidationResult(false, "Max value isn't set. Line: " + symbol_index + ".");
            if (symbol.Step_Value == null) return new ValidationResult(false, "Step value isn't set. Line: " + symbol_index + ".");

            //---
            if (symbol.Tag_Name.Length < 1 || symbol.Tag_Name.Length > xSetPoint_TagName_PrintSize) return new ValidationResult(false, 
                    "Tag name must be (1..." + xSetPoint_TagName_PrintSize + ")-char long. Line: " + symbol_index + ".");
            //---

            string err = RANGE_Validation(symbol, symbol.str_Min_Value);
            if (err != null) return new ValidationResult(false, "Min " + err + ". Line: " + symbol_index + ".");

            err = RANGE_Validation(symbol, symbol.str_Max_Value);
            if (err != null) return new ValidationResult(false, "Max " + err + ". Line: " + symbol_index + ".");

            err = RANGE_Validation(symbol, symbol.str_Step_Value);
            if (err != null) return new ValidationResult(false, "Step " + err + ". Line: " + symbol_index + ".");
            if (symbol.Step_Value < 0) return new ValidationResult(false, "Step value must be positive. Line: " + symbol_index + ".");

            //---  MIN ... MAX

            double dax;
            // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
            if (!double.TryParse(symbol.Initial_Value, NumberStyles.Any, null, out dax))
            {
                if (dax < symbol.Min_Value || dax > symbol.Max_Value) return new ValidationResult(false, "Initial value must be between Min-value and Max-value. Line: " + symbol_index + ".");
            }

            //---

        }


        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        string msg = excp.ToString();
        return new ValidationResult(false, msg + " Line: " + symbol_index + ".");
    }
}

//***************  NAME VALIDATION
/*  Перенес все в ValidationGroup - Так удобнее.
 * 
public class Name_ValidationRule : ValidationRule
{
    int min, max;

    public Name_ValidationRule(int maxx) : base()
    {
        min = 1;  max = maxx;
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            int ax = ((string)value).Length;

            // Is in range?
            if ((ax < this.min) || (ax > this.max))
            {
                string msg = string.Format("Name length must be between {0} and {1}.", this.min, this.max);
                return new ValidationResult(false, msg);
            }
            else if (Char.IsDigit(((string)value)[0]))
            {
                string msg = string.Format("Name can't begin from digit.");
                return new ValidationResult(false, msg);
            }
            // Number is valid
            return new ValidationResult(true, null);
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg);
        }
    }
}


//***************  ADDRESS VALIDATION


    public class Adress_ValidationRule : ValidationRule
    {
        int min, max;

        public Adress_ValidationRule(int minn, int maxx) : base()
        {
            min = minn; max = maxx;
        }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {   
        try
        {
            int ax;

            // Is a number?
            if (!int.TryParse((string)value, out ax))
            {
                return new ValidationResult(false, "Not a number.");
            }
            // Is in range?
            if ((ax < this.min) || (ax > this.max))
            {
                string msg = string.Format("Address must be between {0} and {1}.", this.min, this.max);
                return new ValidationResult(false, msg);
            }
            // Number is valid
            return new ValidationResult(true, null);

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg);
        }
    }
}


//***************  BIT_NUMBER VALIDATION

public class BitNumber_ValidationRule : ValidationRule
{
        int min;//, max;
        
        public BitNumber_ValidationRule()
            : base()
        {
            min = 0; // max = 32;
        }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            int ax;
            // Is a number?
            if (!int.TryParse((string)value, out ax))
            {
                return new ValidationResult(false, "Not a number.");
            }
            // Is in range?
            if ((ax < this.min))// || (ax > this.max))
            {
                //string msg = string.Format("Bit number must be between {0} and {1}.", this.min, this.max);
                string msg = string.Format("Bit number can't be less then {0}.", this.min);
                return new ValidationResult(false, msg);
            }
            // Number is valid
            return new ValidationResult(true, null);

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg);

        }
    }
}

//***************  INITIAL VALUE VALIDATION

public class InitialValue_ValidationRule : ValidationRule
{
    int min, max;

    public InitialValue_ValidationRule(int minn, int maxx)
        : base()
    {
        min = minn; max = maxx;
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            int ax;

            // Is a number?
            if (!int.TryParse((string)value, out ax))
            {
                return new ValidationResult(false, "Not a number.");
            }
            // заменили на GroupValidation Is in range?
            //if ((ax < this.min) || (ax > this.max))
            //{
              //  string msg = string.Format("Initial value must be between {0} and {1}.", this.min, this.max);
                //return new ValidationResult(false, msg);
            //}
            // Number is valid
            return new ValidationResult(true, null);

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            string msg = excp.ToString();
            return new ValidationResult(false, msg);
        }
    }
}



//***************  COMMENT VALIDATION

    public class Comment_ValidationRule : ValidationRule
    {
        int min, max;

        public Comment_ValidationRule(int maxx)
            : base()
        {
            min = 0; max = maxx;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int ax = ((string)value).Length;

                // Is in range?
                if ((ax < this.min) || (ax > this.max))
                {
                    string msg = string.Format("Comment length must be less then {0}.", this.max);
                    return new ValidationResult(false, msg);
                }
                // Number is valid
                return new ValidationResult(true, null);
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                string msg = excp.ToString();
                return new ValidationResult(false, msg);
            }
        }
    }
*/
//***************  VALIDATION  HANDLER

    private void ValidationErrorEvent(object sender, ValidationErrorEventArgs e)
    {
        if (e.Action == ValidationErrorEventAction.Added)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());

            //---
            // Сделано по прерыванию Validation.ErrorRemoved т.к. 
            // он не заходит в Validation если ошибка исправлена по Esc, а сюда заходит.
            Validation_Ok = false;
        }
        else if (e.Action == ValidationErrorEventAction.Removed)
        {
            //MessageBox.Show("Error removed!");
            Validation_Ok = true;
        }


        e.Handled = true;
    }




//**********************************************************

//***************   HANDLERS  ******************************




void button_AddRow_Click(object sender, RoutedEventArgs e)
{
    try
    {   //  Вставка строки в конце списка.
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Symbol_Data> symbols_list = (ObservableCollection<Symbol_Data>)datagrid.Tag;

        symbols_list.Add(new Symbol_Data(symbols_list, null));
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


void button_CopyRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        SYMBOLS_LIST_BUFFER = new List<Symbol_Data>();

        foreach (SYMBOLS.Symbol_Data item in datagrid.SelectedItems)
        {
            SYMBOLS_LIST_BUFFER.Add(item);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void button_InsertRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Symbol_Data> symbols_list = (ObservableCollection<Symbol_Data>)datagrid.Tag;

        //  Вставка строки в список.
        Symbol_Data selecteditem = (Symbol_Data)datagrid.SelectedItem;

        if (selecteditem != null)
        {
            int index = symbols_list.IndexOf(selecteditem);
            if (index > -1)
            {
                //  Вставляем пустые строки или скопированные.
                if (SYMBOLS_LIST_BUFFER == null)
                {
                    symbols_list.Insert(index, new Symbol_Data(symbols_list, null));
                }
                else
                {
                    foreach (Symbol_Data item in SYMBOLS_LIST_BUFFER)
                    {
                        symbols_list.Insert(index++,
                       new Symbol_Data(symbols_list, item.Name, item.Tag_Name, item.Data_Type, item.Memory_Type, item.Address, item.Bit_Base, item.Bit_Number, item.Initial_Value, item.Comment));
                    }

                    SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
                }

                //---  Обновление DataGrid т.к. после вставки строк не обновляются автоматически их номера.
                System.Collections.IEnumerable itemssource = datagrid.ItemsSource;
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = itemssource;
            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void button_DeleteRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Symbol_Data> symbols_list = (ObservableCollection<Symbol_Data>)datagrid.Tag;
        
        //  Удаление выбранных строк списка.
        
            for (int tst = 0; tst == 0; )
            {   //   перед удаление строки удаляем все элементы из грид привязанные к ней,
                // т.к. иначе грид запихнет их на ставшую последней строку.
                tst = 1;
                foreach (SYMBOLS.Symbol_Data item in datagrid.SelectedItems)
                {
                    symbols_list.Remove(item);
                        //  приходится прерывать поиск и начинать его сначала, т.к. 
                        //  коллекция после удаления изменилась и foreach не может 
                        //  продолжать поиск далее.
                    tst = 0;
                    break;
                }
            }

        //---  Обновление DataGrid т.к. после вставки строк не обновляются автоматически их номера.
        System.Collections.IEnumerable itemssource = datagrid.ItemsSource;
        datagrid.ItemsSource = null;
        datagrid.ItemsSource = itemssource;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void button_BindToElement_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Symbol_Data> symbols_list = (ObservableCollection<Symbol_Data>)datagrid.Tag;

        if (datagrid.SelectedItem == null) return;

        Symbol_Data symbol = (Symbol_Data)datagrid.SelectedItem;

        //if (Validation_Ok == true)
        string str;
        if ((str=Check_DiagramLabelDuplication(ACTIV_DIAGRAM, symbol.Name)) != null)
        {
            MessageBox.Show("Can't bind. Error exists: " + str);
        }
        else if (Validation_By_Event(symbols_list, datagrid, false) == null)
        {
            SYMBOLS_LIST_BUFFER = new List<Symbol_Data>();
            SYMBOLS_LIST_BUFFER.Add(symbol);
            //---
            ((Window)((Button)sender).Tag).Close();
        }
        else
        {
            MessageBox.Show("Can't bind. Error exists. Line: " + Validation_Error_Line);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void button_Cancel_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //if (SYMBOLS_LIST_PANEL.ItemBindingGroup.CommitEdit())
        //SYMBOLS_LIST_PANEL.CommitEdit();
        //SYMBOLS_LIST_PANEL.CancelEdit();
        //SYMBOLS_LIST_PANEL.Edit

        //  Вставили отмену как при закрытии окна, но там это нужно, а здесь для красоты.
        // ... потеря фокуса и Validation срабатывает раньше и отмена уже не работает 
        // SYMBOLS_LIST_PANEL.CancelEdit();

        //if (Validation_Ok == true)
        //{
            //GLOBAL_SYMBOLS_LIST_window.Close();
               
            ((Window)((Button)sender).Tag).Close();
        //}
        //else
        //{
            //MessageBox.Show("Can't exit. Error exists. Line: " + Validation_Error_Line);
        //}
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void SYMBOLS_LIST_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{

    //      Вставляем отмену редактирования на случай:
    // если вошли в окно, сделали корректировку с ошибкой, и без ввода или другой потери фокуса
    // чтобы включилась Validation нажали крестик закрытия окна то оно закрывается без Validation
    // и потом уже невозможно его открыть заново.
    ////SYMBOLS_LIST_PANEL.CommitEdit();
    //BASE_SYMBOLS_LIST_PANEL.CancelEdit();

    if (((DataGrid)((Window)sender).Tag).Tag is ObservableCollection<Symbol_Data>)
    {
        if (Validation_By_Event((ObservableCollection<Symbol_Data>)((DataGrid)((Window)sender).Tag).Tag, (DataGrid)((Window)sender).Tag, false) != null)
        {
            //MessageBox.Show("Can't exit. Error exists."); // Line: " + Base_Validation_Error_Line);
            if (MessageBox.Show("Errors exist. Exit?", "Validation", MessageBoxButton.OKCancel, MessageBoxImage.Error)
                  != MessageBoxResult.OK) e.Cancel = true;
        }
        else
        {
            //-----   Перерисовываем элемент - Изменяли-не изменяли набор символов, разбираться нет смысла.
            ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage(ACTIV_DIAGRAM);
        }
    }
    else if (((DataGrid)((Window)sender).Tag).Tag is ObservableCollection<Indication_Symbol_Data>)
    {
        if (Indication_Validation_By_Event((ObservableCollection<Indication_Symbol_Data>)((DataGrid)((Window)sender).Tag).Tag, (DataGrid)((Window)sender).Tag, false) != null)
        {
            if (MessageBox.Show("Errors exist. Exit?", "Validation", MessageBoxButton.OKCancel, MessageBoxImage.Error)
                  != MessageBoxResult.OK) e.Cancel = true;
        }
    }
    else
    {
        throw new Exception("Error <SYMBOLS_LIST_window_Closing(...)>: Unknown <Symbols list>");
    }
    //  sender = Window:  обнуляем признак открытости окна для для ПО перерисовки его datagrid при изменении свойства Nom_Value
    sender = null;

}



        }  // ******  END of Class Mymory_Symbols

    }  // ******  END of Class Step5

}


        //-----------------
   /*     
                                    DataTemplate row_header_template = new DataTemplate();
                            FrameworkElementFactory row_header_text = new FrameworkElementFactory(typeof(TextBlock));
                                    Binding row_header_binding = new Binding() ;
                                    row_header_binding.Source = datagrid.CurrentItem;
                                //    row_header_binding.Path =  new PropertyPath("items[0]");
                                    row_header_binding.StringFormat = "00.0";// d_CalcStringformat(VIEW_list[i - 1].DataList);
                                    
                            row_header_text.SetBinding(TextBlock.TextProperty, row_header_binding);                     
                            //row_header_text.SetValue(TextBlock.TextProperty, "1111");                     
                            row_header_template.VisualTree =  row_header_text ;
                            datagrid.RowHeaderTemplate = row_header_template;
     */               

                                     //       Binding header_binding = new Binding();
                                     //       //header_binding.Source = VIEW_list[i-1];
                                     //       //header_binding.Path = new PropertyPath("Name");
                                     //       //header_binding.Mode = BindingMode.TwoWay;
                                     //       header_binding.Converter = new INPUT_DATA_header_converter();
                                     //       header_binding.ConverterParameter = i;
                                     //       BindingOperations.SetBinding(column, DataGridTextColumn.HeaderProperty, header_binding);
