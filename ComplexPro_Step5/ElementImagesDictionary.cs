//  Проверить почему на ноутбуке ТурСА не открывалось окно OpenProject
//  Проверить почему 27-й проект сохранялся в 26-м каталоге
//  Сделать контроль при загрузке проекта если какая-то функция не найдена в Dictionary
//  Сделать контроль при загрузке проекта если какая-то переменная не найдена в SymbolsList
// сделать функции для COUNTERs

// + Продолжить переделывать функции под tmp1,...
// + Заменить везде (slw)&... на (slw*)&...

// + сделать контроль переполнения для конвенртеров ROUND, FLOOR, TRUNC, CEIL и др..
// разобраться почему когда создаеш вторую диаграмму то в ее параметрах пишется мусор если не привязан ее выход к переменной
// + нужно ли RD, RW для локальных переменных?
// + добавить в базовые переменные типы RD, RW
// + сделать &param в перевалочные функции для таймеров и везде где вызов функции из функции, через передачу параметрра на верх через 
// + при передаче в функции битовых переменных использовать только lw_tmp, а не по типу базовой перменной для BOOL-типа
// + заменить области памяти на Read Write.
// + сделать в таблицах символов переменные TIME и TIMER
// + сделать валидашейн "TIMER" 
// + сделать валидашейн "TIME"
// + ввести переменную  SYS_TIME - чтобы иметь одинаковое текущее время для одного цикла.
// + сделать при загрузке инициализац. вызов _STEP5_TIMER1 (0, SYS_TIME)
// + сделать циклический вызов _STEP5_TIMER1 (1, SYS_TIME) чтобы не было переполнения timer1 если долго ее не вызывали

// + Сделать обнуление по старту всех таймерных переменных.
// + ввести в GlobalVars и SetPoint Переменные типа TIME
// + ввести в GlobalVars переменную SYSTEM_TIMER
// + Позаменял CD и где надо и где не надо. Придумать интеллект в замену имен переменных - ввести разделители: пробел, запятая, скобка, точка с запятой.
// + добавить контроль входов элемента на допустимые для него типы данных.
// + добавить к списку входов элемента список допустимых для него типов данных.
// + в компиляторе если имена верхней пары входов заданы или не заданы, то подменять ее ENI, ENO.



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

namespace ComplexPro_Step5
{
    public partial class Step5
    {

//            public ElementImage(string category, string name, int type, int type_int, string sign_name, bool border, 
//                                     List<List<string>> io_list, List<List<double>> points_list,
//                                     string sign_text, List<double> sign_text_position,
//                                     List<string> directions, List<string> code)            


  static  public  class ELEMENTS_DICTIONARY 
        {

        static public ElementImage GetElementByName(string category_name, string element_name)
        {
            try
            {
                foreach ( Category category in ELEMENTS_DICTIONARY.CATEGORYs)
                {
                    if( category.NAME == category_name)
                    {
                        foreach(ElementImage element_image in category.ELEMENTs)
                        {
                            if( element_image.Name == element_name) return element_image;
                        }
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


        static public bool AddElement(string category_name, ElementImage element)
        {
            try
            {
                foreach (Category category in ELEMENTS_DICTIONARY.CATEGORYs)
                {
                    if (category.NAME == category_name)
                    {        
                        category.ELEMENTs.Add(element);
                        return true;
                    }
                }

                return false;

            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                return false;
            }
        }



        static public bool RemoveElementByName(string category_name, string element_name)
        {
            try
            {

                ElementImage element = null;
                Category categor = null;

                foreach (Category category in ELEMENTS_DICTIONARY.CATEGORYs)
                {
                    if (category.NAME == category_name)
                    {
                        foreach (ElementImage element_image in category.ELEMENTs)
                        {
                            if (element_image.Name == element_name) 
                            {
                                element = element_image;
                                categor = category;
                            }
                        }
                    }
                }

                if ( categor != null ) 
                {
                    categor.ELEMENTs.Remove(element);
                    return true;
                }
                else return false ;

            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                return false;
            }
        }


        static public bool RemoveElementByImage(string category_name, ElementImage element)
        {
            try
            {
                Category categor = null;

                foreach (Category category in ELEMENTS_DICTIONARY.CATEGORYs)
                {
                    if (category.NAME == category_name)
                    {
                        foreach (ElementImage element_image in category.ELEMENTs)
                        {
                            if (element_image == element)
                            {
                                categor = category;
                            }
                        }
                    }
                }

                if (categor != null)
                {
                    categor.ELEMENTs.Remove(element);
                    return true;
                }
                else return false;

            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                return false;
            }
        }

        static public bool RemoveUserFunctions(string category_name = "USER FUNCTIONS")
        {
            try
            {
        
                foreach (Category category in ELEMENTS_DICTIONARY.CATEGORYs)
                {
                    if (category.NAME == category_name)
                    {
                        category.ELEMENTs.Clear();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                return false;
            }
        }


        static public bool ContainsElementByName(string category_name, string element_name)
        {
            if (GetElementByName(category_name, element_name) != null) return true;
            else return false;
        }


        static public bool ContainsElementByImage(string category_name, ElementImage element)
        {
            try
            {
                foreach (Category category in ELEMENTS_DICTIONARY.CATEGORYs)
                {
                    if (category.NAME == category_name)
                    {
                        foreach (ElementImage element_image in category.ELEMENTs)
                        {
                            if (element_image == element) return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
                return false;
            }
        }



//***************
//***************  CATEGORY

            public class Category
            {
                public string NAME {get;set;}

                public ObservableCollection<ElementImage> ELEMENTs { get; set; }

                public Category ( string name, ObservableCollection<ElementImage> elements)
                {
                    NAME    = name;
                    ELEMENTs = elements;
                }
                
            }


//************    СОСТАВЛЕНИЕ РИСУНКА ЗНАЧКА ФУНКЦИИ ПО ЕГО КОЛИЧЕСТВУ ВХОДОВ ВЫХОДОВ НАЗНАЧЕННЫХ ПОЛЬЗОВАТЕЛЕМ.

    // При создании значка когда уже существует диаграмма передаем 1-й параметр.
    //  При загрузке из файла когда еще существует диаграмма передаем 2-й и 3-й параметры.

public static ElementImage AddReplace_DiagramElementImage ( Diagram_Of_Networks diagram, 
                                                    string element_name = null,
                                ObservableCollection<SYMBOLS.Symbol_Data> local_symbols_list = null)
{
    try
    {
        if ( (diagram != null && diagram.NAME == "MAIN") || element_name == "MAIN") return null;

        string category_name = "USER FUNCTIONS";

        if (diagram != null)
        {
            element_name = diagram.NAME;
            local_symbols_list = diagram.LOCAL_SYMBOLS_LIST;
        }
        
        //   Отсортировываем входные переменные от выходных.

        List<SYMBOLS.Symbol_Data> in_symbols = new List<SYMBOLS.Symbol_Data>();
        List<SYMBOLS.Symbol_Data> out_symbols = new List<SYMBOLS.Symbol_Data>();

        StringBuilder parameters = new StringBuilder();

        int begin = 1;
        foreach (SYMBOLS.Symbol_Data symbol in local_symbols_list)
        {
            if ( symbol.Memory_Type == "IN" ) in_symbols.Add(symbol);
            else if ( symbol.Memory_Type == "OUT" || symbol.Memory_Type == "IN_OUT") out_symbols.Add(symbol);
            //---

            if ( symbol.Memory_Type == "IN" || symbol.Memory_Type == "OUT" || symbol.Memory_Type == "IN_OUT")
            {
                if( begin != 1 ) parameters.Append(", ");
                begin = 0;
                //---
                parameters.Append(symbol.Name);
            }
        }
        

        //    Составляем массив входов для элемента.

        List<List<ElementImage.IO_Data>> io_list =  new List<List<ElementImage.IO_Data>>() ;

        //  1я пара входов - типовая, неиспользуемая - можно заменить нулями.
        //io_list.Add( new List<ElementImage.IO_Data>() { new ElementImage.IO_Data("ENI",  "IN", null, null),
          //                                              new ElementImage.IO_Data("ENO", "OUT", null, null)
            //                                          });
        io_list.Add( new List<ElementImage.IO_Data>() { null, null });

        int count = in_symbols.Count;
        if ( count < out_symbols.Count) count = out_symbols.Count;

        for ( int i = 0 ; i < count; i++)
        {
            ElementImage.IO_Data in_io_data, out_io_data ;

            if ( i < in_symbols.Count) 
            {
                in_io_data = new ElementImage.IO_Data(in_symbols[i].Name, "IN", new List<string>() {in_symbols[i].Data_Type}, null );
            }
            else in_io_data = null;

            if ( i < out_symbols.Count) 
            {
                out_io_data = new ElementImage.IO_Data(out_symbols[i].Name, "OUT", new List<string>() {out_symbols[i].Data_Type}, null );
            }
            else out_io_data = null;

            //--------------

            List<ElementImage.IO_Data> io_list2 = new List<ElementImage.IO_Data>()
                                                  {
                                                      in_io_data,
                                                      out_io_data 
                                                  };
            io_list.Add(io_list2);
        }

        //  последняя пара входов MAIN_IN - типовая, неиспользуемая.
        io_list.Add(new List<ElementImage.IO_Data>() { null, null });

        //-----------

        ElementImage element_image = new ElementImage(category_name, element_name, 2, 3, element_name, true, 
                                        io_list,
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;", 
                                            "       if ( ENI == 1 ) goto x1_LOOP;",  // выделение отрицательного перепада сигнала ENI.
                                            "       ENO = 1 ;", 
                                            "       " + element_name + "( " + parameters.ToString() + " );",
                                            "x1_LOOP:"
                                        });

        ElementImage old_element_image = null;

        // При загрузке не из файла и из файла.
        if (diagram != null)
        {
            old_element_image = diagram.ELEMENT_IMAGE;
            element_image.Diagram = diagram;  //   взаимные ссылки для быстрого поиска соответвующей диаграммы и наоборот.
            diagram.ELEMENT_IMAGE = element_image;

            //----------
            //   Заменяем не по имени а по имиджу т.к. имя может измениться.

            if (ELEMENTS_DICTIONARY.ContainsElementByImage(category_name, old_element_image))
            {
                ELEMENTS_DICTIONARY.RemoveElementByImage(category_name, old_element_image);
            }
        }
        else  // При загрузке из файла.
        {
            element_image.Diagram = null;  //   взаимные ссылки для быстрого поиска соответвующей диаграммы и наоборот.
            //----------
            //   Заменяем по имени.

            if (ELEMENTS_DICTIONARY.ContainsElementByName(category_name, element_name))
            {
                ELEMENTS_DICTIONARY.RemoveElementByName(category_name, element_name);
            }
        }

        ELEMENTS_DICTIONARY.AddElement(category_name, element_image);

        if ( old_element_image != null )   UPDATE_USER_FUNCTION_IMAGES(old_element_image, element_image);

        return element_image;

        //TOOLS_TREE_VIEW.UpdateLayout();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}

public static void Remove_DiagramElementImage(Diagram_Of_Networks diagram)
{
    try
    {
        string category_name = "USER FUNCTIONS";
             

        if (ELEMENTS_DICTIONARY.ContainsElementByImage(category_name, diagram.ELEMENT_IMAGE))
        {
            ELEMENTS_DICTIONARY.RemoveElementByImage(category_name, diagram.ELEMENT_IMAGE);
        }
        
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return;
    }

}

            static public ObservableCollection<Category> CATEGORYs { get { return Categorys; } }

            //Не получится хранить в библиотеке готовые CANVAS - т.к. у них на всех один Grid.Row
            //В библиотеке надо хранить описательные данные на элемент а формтировать его непосредственно при Drop.!!

            static public ObservableCollection<Category> Categorys = 
                      new ObservableCollection<Category>() 
        {
            {new Category( "CONNECTORS", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("CONNECTORS", "NEW_LINE", 0, 0, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 0, 0 }, new List<double>() {  1000,0 }
                                        }, null, null, 
                                        new List<string>() { "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 1;", 
                                        })
                     }, 

                    {new ElementImage("CONNECTORS", "END_OF_LINE", 0, 4, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 0, 0 }, new List<double>() {  50,  0 }, null,
                                            new List<double>() {50,20 }, new List<double>() {  50,-20 }
                                        }, null, null, 
                                        new List<string>() { "Left" }, null)
                     }, 

                    {new ElementImage("CONNECTORS", "HORIZONTAL", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 1000,  0 } 
                                        }, null, null, 
                                        new List<string>() { "Left", "Right" }, null)
                     }, 

                    {new ElementImage("CONNECTORS", "VERTICAL", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 50,  1000 }, new List<double>() { 50, -1000 } 
                                        }, null, null, 
                                        new List<string>() { "Up", "Down" }, null)
                     }, 

                     {new ElementImage("CONNECTORS", "T_UP", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 1000,  0 }, null,
                                            new List<double>() { 50,  0 }, new List<double>() {   50, 50 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Right", "Up" }, null)
                     }, 

                    {new ElementImage("CONNECTORS", "T_DOWN", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 1000,  0 }, null,
                                            new List<double>() { 50,  0 }, new List<double>() {  50,-1000 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Right", "Down" }, null)
                     }, 
                    {new ElementImage("CONNECTORS", "T_RIGHT", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 50, 1000 }, new List<double>() {  50,-1000 }, null,
                                            new List<double>() { 50,    0 }, new List<double>() { 1000,  0 }
                                        }, null, null, 
                                        new List<string>() { "Up", "Right", "Down" }, null)
                     }, 
                    {new ElementImage("CONNECTORS", "T_LEFT", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 50, 1000 }, new List<double>() {  50,-1000 }, null,
                                            new List<double>() { 50,  0 }, new List<double>() {   0,  0 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Up", "Down" }, null)
                     },
                    {new ElementImage("CONNECTORS", "L_LEFT_UP", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() {  50,    0 }, null,
                                            new List<double>() { 50,  0 }, new List<double>() {  50, 1000 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Up" }, null)
                     },
                    {new ElementImage("CONNECTORS", "L_LEFT_DOWN", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() {  50,    0 }, null,
                                            new List<double>() { 50,  0 }, new List<double>() {  50,-1000 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Down" }, null)
                     },
                    {new ElementImage("CONNECTORS", "L_RIGHT_UP", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 1000,  0 }, new List<double>() {  50,    0 }, null,
                                            new List<double>() {   50,  0 }, new List<double>() {  50, 1000 }
                                        }, null, null, 
                                        new List<string>() { "Right", "Up" }, null)
                     },
                    {new ElementImage("CONNECTORS", "L_RIGHT_DOWN", 0, 1, null, false, null, 
                                        new List<List<double>>() 
                                        {
                                            new List<double>() { 1000,  0 }, new List<double>() {  50,    0 }, null,
                                            new List<double>() {   50,  0 }, new List<double>() {  50,-1000 }
                                        }, null, null, 
                                        new List<string>() { "Right", "Down" }, null)
                     }


                })
            },
//**************************************

            {new Category( "BIT_LOGIC", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("BIT_LOGIC", "OPEN_CONT", 1, 3, null, false, 
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {   0,   0 }, new List<double>() {  40,   0 }, null,
                                            new List<double>() {  40,  20 }, new List<double>() {  40, -20 }, null,
                                            new List<double>() {1000,   0 }, new List<double>() {  60,   0 }, null,
                                            new List<double>() {  60,  20 }, new List<double>() {  60, -20 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       MAIN_IN_tmp0 = MAIN_IN;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 == 0 ) goto x1_LOOP;", 
                                            "       ENO = ENI;", 
                                            "x1_LOOP:" 
                                        })
                    }, 

                    {new ElementImage("BIT_LOGIC", "CLOSED_CONT", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {   0,   0 }, new List<double>() {  40,   0 }, null,
                                            new List<double>() {  40,  20 }, new List<double>() {  40, -20 }, null,
                                            new List<double>() {1000,   0 }, new List<double>() {  60,   0 }, null,
                                            new List<double>() {  60,  20 }, new List<double>() {  60, -20 }, null,
                                            new List<double>() {  42, -19 }, new List<double>() {  59,  19 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       MAIN_IN_tmp0 = MAIN_IN;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 != 0 ) goto x1_LOOP;", 
                                            "       ENO = ENI;", 
                                            "x1_LOOP:" 
                                        })
                    }, 

                    {new ElementImage("BIT_LOGIC", "NOT_CONT", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {   0,   0 }, new List<double>() {  25,   0 }, null,
                                            new List<double>() {  25,  20 }, new List<double>() {  25, -20 }, null,
                                            new List<double>() {1000,   0 }, new List<double>() {  75,   0 }, null,
                                            new List<double>() {  75,  20 }, new List<double>() {  75, -20 }
                                        }, "NOT", new List<double>() {  27,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 1 ;", 
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       ENO = 0 ;", 
                                            "x1_LOOP:" 
                                        })
                    }, 

                    {new ElementImage("BIT_LOGIC", "COIL", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 35,  0 }, null,
                                            new List<double>() { 40, 17 }, new List<double>() { 35,  0 },
                                                                           new List<double>() { 40,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 65,  0 }, null,
                                            new List<double>() { 60, 17 }, new List<double>() { 65,  0 },
                                                                           new List<double>() { 60,-17 }
                                        }, null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   // MAIN_IN = 1;
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", 
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",
                                            "       goto x2_LOOP;", 
                                            "x1_LOOP:", 
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   // MAIN_IN = 0;
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", 
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "MIDLINE_COIL", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 35,  0 }, null,
                                            new List<double>() { 40, 17 }, new List<double>() { 35,  0 },
                                                                           new List<double>() { 40,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 65,  0 }, null,
                                            new List<double>() { 60, 17 }, new List<double>() { 65,  0 },
                                                                           new List<double>() { 60,-17 }
                                        }, "#", new List<double>() {  39,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   // MAIN_IN = 1;
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", 
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",
                                            "       ENO = 1 ;", 
                                            "       goto x2_LOOP;", 
                                            "x1_LOOP:", 
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   // MAIN_IN = 0;
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", 
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "       ENO = 0 ;", 
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "RESET_COIL", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 35,  0 }, null,
                                            new List<double>() { 40, 17 }, new List<double>() { 35,  0 },
                                                                           new List<double>() { 40,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 65,  0 }, null,
                                            new List<double>() { 60, 17 }, new List<double>() { 65,  0 },
                                                                           new List<double>() { 60,-17 }
                                        }, "R", new List<double>() {  39,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   // MAIN_IN = 0;
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", 
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "SET_COIL", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 35,  0 }, null,
                                            new List<double>() { 40, 17 }, new List<double>() { 35,  0 },
                                                                           new List<double>() { 40,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 65,  0 }, null,
                                            new List<double>() { 60, 17 }, new List<double>() { 65,  0 },
                                                                           new List<double>() { 60,-17 }
                                        }, "S", new List<double>() {  39,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   // MAIN_IN = 1;
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", 
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },


                    {new ElementImage("BIT_LOGIC", "NEG_RLO_EDGE", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 35,  0 }, null,
                                            new List<double>() { 40, 17 }, new List<double>() { 35,  0 },
                                                                           new List<double>() { 40,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 65,  0 }, null,
                                            new List<double>() { 60, 17 }, new List<double>() { 65,  0 },
                                                                           new List<double>() { 60,-17 }
                                        }, "N", new List<double>() {  39,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;", 
                                            "       if ( ENI == 1 ) goto x1_LOOP;",  // выделение отрицательного перепада сигнала ENI.
                                            "       MAIN_IN_tmp0 = MAIN_IN;",  //  в MAIN_IN хранится прошлое значение сигнала.
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 == 0 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:",              // В MAIN_IN повторяем ENI
                                            "       MAIN_IN_tmp0 = MAIN_IN;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", // MAIN_IN = 0;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "       if ( ENI == 0 ) goto x2_LOOP;",     
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", // MAIN_IN = 1;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "POS_RLO_EDGE", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 35,  0 }, null,
                                            new List<double>() { 40, 17 }, new List<double>() { 35,  0 },
                                                                           new List<double>() { 40,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 65,  0 }, null,
                                            new List<double>() { 60, 17 }, new List<double>() { 65,  0 },
                                                                           new List<double>() { 60,-17 }
                                        }, "P", new List<double>() {  39,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;", 
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  // выделение положительного перепада сигнала ENI.
                                            "       MAIN_IN_tmp0 = MAIN_IN;",  //  в MAIN_IN хранится прошлое значение сигнала.
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 != 0 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:",              // В MAIN_IN повторяем ENI
                                            "       MAIN_IN_tmp0 = MAIN_IN;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", // MAIN_IN = 0;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "       if ( ENI == 0 ) goto x2_LOOP;",     
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", // MAIN_IN = 1;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x2_LOOP:" 
                                        })
                    },


                    {new ElementImage("BIT_LOGIC", "RS", 2, 3, "RS", true, 
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("_R", "IN", null, null), 
                                                new ElementImage.IO_Data("_Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("_S", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {           // Вместо R и Q прописываем: R = ENI, Q = ENO.
                                            // MAIN_IN инверсен выходу и повторяет верхний левый входной сигнал
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       ENO = 0;",          //    За исключением ситуации S=1 & R=1
                                                                        // MAIN_IN повторяет верхний левый входной сигнал
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", // MAIN_IN = 1;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x1_LOOP:", 
                                            "       _S_tmp1 = _S;", 
                                            "       _S_tmp1 = _S_tmp1 & _S_BIT_MASK;", 
                                            "       if ( _S_tmp1 == 0 ) goto x2_LOOP;", 
                                            "       ENO = 1;",      // MAIN_IN повторяет верхний левый  входной сигнал
                                            "       MAIN_IN_tmp0 = MAIN_IN;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", // MAIN_IN = 0;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "       goto  x3_LOOP;", 
                                            "x2_LOOP:",    //  Когда R=0 & S=0 повторяем состояние ENO равным прошлому проходу:
                                            "       ENO = 0 ; ",   //  т.е. инверсное MAIN_IN.
                                            "       MAIN_IN_tmp0 = MAIN_IN;", 
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 != 0 ) goto x3_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x3_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "SR", 2, 3, "SR", true, 
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("_S", "IN", null, null), 
                                                new ElementImage.IO_Data("_Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("_R", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {           // Вместо R и Q прописываем: S = ENI, Q = ENO.
                                            // MAIN_IN равен выходу и повторяет верхний левый входной сигнал
                                            "       if ( ENI == 0 ) goto x1_LOOP;", 
                                            "       ENO = 1;",         //    За исключением ситуации S=1 & R=1
                                                                        // MAIN_IN повторяет верхний левый входной сигнал
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 | MAIN_IN_BIT_MASK;", // MAIN_IN = 1;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "x1_LOOP:", 
                                            "       _R_tmp1 = _R;", 
                                            "       _R_tmp1 = _R_tmp1 & _R_BIT_MASK;", 
                                            "       if ( _R_tmp1 == 0 ) goto x2_LOOP;", 
                                            "       ENO = 0;",      // MAIN_IN повторяет верхний левый  входной сигнал
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & ~MAIN_IN_BIT_MASK;", // MAIN_IN = 0;
                                            "       MAIN_IN = MAIN_IN_tmp0 ;",  
                                            "       goto  x3_LOOP;", 
                                            "x2_LOOP:",    //  Когда R=0 & S=0 повторяем состояние ENO равным прошлому проходу:
                                            "       ENO = 0 ; ",   //  т.е. MAIN_IN.
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 == 0 ) goto x3_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x3_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "NEG_ADDR_EDGE", 2, 3, "NEG", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null, 
                                                new ElementImage.IO_Data("_Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("M_BIT", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 != 0 ) goto x1_LOOP;", 
                                            "       M_BIT_tmp1 = M_BIT;", 
                                            "       M_BIT_tmp1 = M_BIT_tmp1 & M_BIT_BIT_MASK;", 
                                            "       if ( M_BIT_tmp1 == 0 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:",    //  в M_BIT хранится прошлое значение сигнала MAIN_IN.
                                            "       M_BIT_tmp1 = M_BIT;", 
                                            "       M_BIT_tmp1 = M_BIT_tmp1 & ~M_BIT_BIT_MASK;", // M_BIT = 0;
                                            "       M_BIT = M_BIT_tmp1;",  
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 == 0 ) goto x2_LOOP;",  
                                            "       M_BIT_tmp1 = M_BIT;", 
                                            "       M_BIT_tmp1 = M_BIT_tmp1 | M_BIT_BIT_MASK;", // M_BIT = 1;
                                            "       M_BIT = M_BIT_tmp1;",  
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("BIT_LOGIC", "POS_ADDR_EDGE", 2, 3, "POS", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null, 
                                                new ElementImage.IO_Data("_Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("M_BIT", "OUT", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 == 0 ) goto x1_LOOP;", 
                                            "       M_BIT_tmp1 = M_BIT;", 
                                            "       M_BIT_tmp1 = M_BIT_tmp1 & M_BIT_BIT_MASK;", 
                                            "       if ( M_BIT_tmp1 != 0 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:",    //  в M_BIT хранится прошлое значение сигнала MAIN_IN.
                                            "       M_BIT_tmp1 = M_BIT;", 
                                            "       M_BIT_tmp1 = M_BIT_tmp1 & ~M_BIT_BIT_MASK;", // M_BIT = 0;
                                            "       M_BIT = M_BIT_tmp1;",  
                                            "       MAIN_IN_tmp0 = MAIN_IN;",   
                                            "       MAIN_IN_tmp0 = MAIN_IN_tmp0 & MAIN_IN_BIT_MASK;", 
                                            "       if ( MAIN_IN_tmp0 == 0 ) goto x2_LOOP;",  
                                            "       M_BIT_tmp1 = M_BIT;", 
                                            "       M_BIT_tmp1 = M_BIT_tmp1 | M_BIT_BIT_MASK;", // M_BIT = 1;
                                            "       M_BIT = M_BIT_tmp1;",  
                                            "x2_LOOP:"
                                        })
                    }

                })
            },

//******************   COMPARATORS

//******************  COMPARATORS <INT>

            {new Category( "COMPARATOR", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("COMPARATOR", "EQ_I", 2, 3, "CMP ==I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 != IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "NE_I", 2, 3, "CMP <>I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 == IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "GT_I", 2, 3, "CMP >I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 <= IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "LT_I", 2, 3, "CMP <I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 >= IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "GE_I", 2, 3, "CMP >=I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 < IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "LE_I", 2, 3, "CMP <=I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 > IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

//*************   COMPARATORS <DINT>

                    {new ElementImage("COMPARATOR", "EQ_D", 2, 3, "CMP ==D", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 != IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "NE_D", 2, 3, "CMP <>D", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 == IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "GT_D", 2, 3, "CMP >D", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 <= IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "LT_D", 2, 3, "CMP <D", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 >= IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "GE_D", 2, 3, "CMP >=D", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 < IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "LE_D", 2, 3, "CMP <=D", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 > IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

//*************   COMPARATORS <REAL>

                    {new ElementImage("COMPARATOR", "EQ_R", 2, 3, "CMP ==R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 != IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "NE_R", 2, 3, "CMP <>R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 == IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "GT_R", 2, 3, "CMP >R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 <= IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "LT_R", 2, 3, "CMP <R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 >= IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "GE_R", 2, 3, "CMP >=R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 < IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("COMPARATOR", "LE_R", 2, 3, "CMP <=R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   // выделение отрицательного перепада сигнала MAIN_IN.
                                            "       ENO = 0 ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       if ( IN1 > IN2 ) goto x1_LOOP;", 
                                            "       ENO = 1 ;", 
                                            "x1_LOOP:"
                                        })
                    }

                })
            },

//******************   CONVERTERs

            {new Category( "CONVERTER", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("CONVERTER", "BCD_I", 2, 3, "BCD_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"WORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       w_tmp1 = IN1;",  
                                            "       _func0_BCD_I( w_tmp1, sw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
//продолжить набивать функции
                    {new ElementImage("CONVERTER", "I_BCD", 2, 3, "I_BCD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       _func0_I_BCD( sw_tmp1, w_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = w_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "BCD_DI", 2, 3, "BCD_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"DWORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       lw_tmp1 = IN1;",  
                                            "       _func0_BCD_DI( lw_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "DI_BCD", 2, 3, "DI_BCD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       _func0_DI_BCD( slw_tmp1, lw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = lw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "B_I", 2, 3, "B_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"BYTE"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sb_tmp0 = IN ;", 
                                            "       OUT = sb_tmp0 ;", //  Интерпретатор должен сам растянуть знак.
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "I_DI", 2, 3, "I_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       OUT = IN ;", //  Интерпретатор должен сам растянуть знак.
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "DI_R", 2, 3, "DI_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       OUT = IN ;", //  Интерпретатор должен сам растянуть знак.
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "R_DR", 2, 3, "R_DR", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DREAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       OUT = IN ;", //  Интерпретатор должен сам растянуть знак.
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "I_B", 2, 3, "I_B", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"BYTE"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",
                                            "       _func0_I_B( sw_tmp1, sb_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sb_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "DI_I", 2, 3, "DI_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       _func0_DI_I( slw_tmp1, sw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "R_DI", 2, 3, "R_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_R_DI( f_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "DR_R", 2, 3, "DR_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"DREAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       d_tmp1 = IN1;",  
                                            "       _func0_DR_R( d_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "INV_I", 2, 3, "INV_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       IN_tmp0 = IN;",
                                            "       IN_tmp0 = IN_tmp0 ^ 0xFFFF;", 
                                            "       OUT = IN_tmp0;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "INV_DI", 2, 3, "INV_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       IN_tmp0 = IN;",
                                            "       IN_tmp0 = IN_tmp0 ^ 0xFFFFFFFF;", 
                                            "       OUT = IN_tmp0;", 
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "NEG_I", 2, 3, "NEG_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       _func0_NEG_I( sw_tmp1, sw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "NEG_DI", 2, 3, "NEG_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       _func0_NEG_DI( slw_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("CONVERTER", "NEG_R", 2, 3, "NEG_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       IN_tmp0 = IN ;",
                                            "       IN_tmp0 = IN_tmp0 * -1 ;",
                                            "       OUT = IN_tmp0 ;", 
                                            "x1_LOOP:"
                                        })
                    },

                     //"Round-toward-nearest"   округление до ближайшего числа.
                    {new ElementImage("CONVERTER", "ROUND", 2, 3, "ROUND", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_ROUND( f_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                     //"Round-toward-zero" -  округление отбрасыванием дробной части.
                    {new ElementImage("CONVERTER", "TRUNC", 2, 3, "TRUNC", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_TRUNC( f_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                     // "Round-towards-positive-infinity"
                    {new ElementImage("CONVERTER", "CEIL", 2, 3, "CEIL", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_CEIL( f_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                     // "Round-towards-negative-infinity" 
                    {new ElementImage("CONVERTER", "FLOOR", 2, 3, "FLOOR", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",   "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_FLOOR( f_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    }

                })
            },


//******************   COUNTERs

            {new Category( "COUNTER", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("COUNTER", "S_CUD", 2, 3, "S_CUD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("CU",  "IN", null, null),
                                                new ElementImage.IO_Data("Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("CD",  "IN", new List<string>() {"BOOL"}, null),
                                                new ElementImage.IO_Data("CV", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("S",  "IN", new List<string>() {"BOOL"}, null),
                                                new ElementImage.IO_Data("CV_BCD", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("PV",  "IN", new List<string>() {"WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R",  "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"COUNTER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       CD_tmp0 = CD;", 
                                            "       CD_tmp0 = CD_tmp0 & CD_BIT_MASK;", 
                                            "       S_tmp1 = S;", 
                                            "       S_tmp1 = S_tmp1 & S_BIT_MASK;", 
                                            "       R_tmp2 = R;", 
                                            "       R_tmp2 = R_tmp2 & R_BIT_MASK;", 
                                            "       _func0_COUNTER_S_CUD( MAIN_IN, ENI, CD_tmp0, S_tmp1, R_tmp2, PV, CV, CV_BCD );", 
                                            "x1_LOOP:",
                                            "       ENO = 0;",   
                                            "       if ( CV == 0 ) goto x2_LOOP;",   
                                            "       ENO = 1;", 
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("COUNTER", "S_CU", 2, 3, "S_CU", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("CU",  "IN", null, null),
                                                new ElementImage.IO_Data("Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("S",  "IN", new List<string>() {"BOOL"}, null),
                                                new ElementImage.IO_Data("CV", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("PV",  "IN", new List<string>() {"WORD"}, null),
                                                new ElementImage.IO_Data("CV_BCD", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R",  "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"COUNTER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       S_tmp1 = S;", 
                                            "       S_tmp1 = S_tmp1 & S_BIT_MASK;", 
                                            "       R_tmp2 = R;", 
                                            "       R_tmp2 = R_tmp2 & R_BIT_MASK;", 
                                            "       _func0_COUNTER_S_CU( MAIN_IN, ENI, S_tmp1, R_tmp2, PV, CV, CV_BCD );", 
                                            "x1_LOOP:",
                                            "       ENO = 0;",   
                                            "       if ( CV == 0 ) goto x2_LOOP;",   
                                            "       ENO = 1;", 
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("COUNTER", "S_CD", 2, 3, "S_CD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("CD",  "IN", null, null),
                                                new ElementImage.IO_Data("Q", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("S",  "IN", new List<string>() {"BOOL"}, null),
                                                new ElementImage.IO_Data("CV", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("PV",  "IN", new List<string>() {"WORD"}, null),
                                                new ElementImage.IO_Data("CV_BCD", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R",  "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"COUNTER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       S_tmp1 = S;", 
                                            "       S_tmp1 = S_tmp1 & S_BIT_MASK;", 
                                            "       R_tmp2 = R;", 
                                            "       R_tmp2 = R_tmp2 & R_BIT_MASK;", 
                                            "       _func0_COUNTER_S_CD( MAIN_IN, ENI, S_tmp1, R_tmp2, PV, CV, CV_BCD );", 
                                            "x1_LOOP:",
                                            "       ENO = 0;",   
                                            "       if ( CV == 0 ) goto x2_LOOP;",   
                                            "       ENO = 1;", 
                                            "x2_LOOP:"
                                        })
                    },

                    {new ElementImage("COUNTER", "SC", 2, 3, null, true, 
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null, 
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("PV", "IN", new List<string>() {"WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"COUNTER"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 30,  0 }, null,
                                            new List<double>() { 35, 17 }, new List<double>() { 30,  0 },
                                                                           new List<double>() { 35,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 70,  0 }, null,
                                            new List<double>() { 65, 17 }, new List<double>() { 70,  0 },
                                                                           new List<double>() { 65,-17 }
                                        }, "SC", new List<double>() {  34,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {           // Вместо R и Q прописываем: R = ENI, Q = ENO.
                                            "       _func0_COUNTER_SC( MAIN_IN, ENI, PV);"
                                        })
                    },

                    {new ElementImage("COUNTER", "CU", 2, 3, null, true, 
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null, 
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"COUNTER"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 30,  0 }, null,
                                            new List<double>() { 35, 17 }, new List<double>() { 30,  0 },
                                                                           new List<double>() { 35,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 70,  0 }, null,
                                            new List<double>() { 65, 17 }, new List<double>() { 70,  0 },
                                                                           new List<double>() { 65,-17 }
                                        }, "CU", new List<double>() {  34,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {           // Вместо R и Q прописываем: R = ENI, Q = ENO.
                                            "       _func0_COUNTER_CU( MAIN_IN, ENI);"
                                        })
                    },

                    {new ElementImage("COUNTER", "CD", 2, 3, null, true, 
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null, 
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"COUNTER"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 30,  0 }, null,
                                            new List<double>() { 35, 17 }, new List<double>() { 30,  0 },
                                                                           new List<double>() { 35,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 70,  0 }, null,
                                            new List<double>() { 65, 17 }, new List<double>() { 70,  0 },
                                                                           new List<double>() { 65,-17 }
                                        }, "CD", new List<double>() {  34,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {           // Вместо R и Q прописываем: R = ENI, Q = ENO.
                                            "       _func0_COUNTER_CD( MAIN_IN, ENI);"
                                        })
                    }

                })
             },

//******************   JMPs

            {new Category( "JUMPS", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("JUMPS", "JUMP", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"LABEL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 20,  0 }, null,
                                            new List<double>() { 25, 17 }, new List<double>() { 20,  0 },
                                                                           new List<double>() { 25,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 80,  0 }, null,
                                            new List<double>() { 75, 17 }, new List<double>() { 80,  0 },
                                                                           new List<double>() { 75,-17 }
                                        }, "Jmp", new List<double>() {  30,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       if ( ENI == 1 ) goto MAIN_IN;" 
                                        })
                    },
                    {new ElementImage("JUMPS", "JUMPN", 1, 3, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"LABEL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 20,  0 }, null,
                                            new List<double>() { 25, 17 }, new List<double>() { 20,  0 },
                                                                           new List<double>() { 25,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 80,  0 }, null,
                                            new List<double>() { 75, 17 }, new List<double>() { 80,  0 },
                                                                           new List<double>() { 75,-17 }
                                        }, "Jmpn", new List<double>() {  25,  13 }, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       if ( ENI == 0 ) goto MAIN_IN;" 
                                        })
                    },
                    {new ElementImage("JUMPS", "LABEL", 1, 4, null, false,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() {      null,   null  },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "", new List<string>() {"LABEL"}, null),
                                                null
                                            }
                                        },
                                        new List<List<double>>() 
                                        {
                                            new List<double>() {  0,  0 }, new List<double>() { 20,  0 }, null,
                                            new List<double>() { 20, 17 }, new List<double>() { 20,-17 }, null,
                                            new List<double>() {1000, 0 }, new List<double>() { 80,  0 }, null,
                                            new List<double>() { 80, 17 }, new List<double>() { 80,-17 }
                                        }, "Label", new List<double>() {  23,  13 }, 
                                        new List<string>() { "Left" },
                                        new List<string>() 
                                        {   
                                            "MAIN_IN:" 
                                        })
                    }

                })
             },

//******************  WORD LOGIC

            {new Category( "WORD LOGIC", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("WORD LOGIC", "WAND_W", 2, 3, "WAND_W", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"WORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       w_tmp0 = IN1;", 
                                            "       w_tmp0 = w_tmp0 & IN2;", 
                                            "       OUT = w_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("WORD LOGIC", "WOR_W", 2, 3, "WOR_W", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"WORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       w_tmp0 = IN1;", 
                                            "       w_tmp0 = w_tmp0 | IN2;", 
                                            "       OUT = w_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("WORD LOGIC", "WXOR_W", 2, 3, "WXOR_W", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"WORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"WORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       w_tmp0 = IN1;", 
                                            "       w_tmp0 = w_tmp0 ^ IN2;", 
                                            "       OUT = w_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("WORD LOGIC", "WAND_DW", 2, 3, "WAND_DW", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DWORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DWORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       lw_tmp0 = IN1;", 
                                            "       lw_tmp0 = lw_tmp0 & IN2;", 
                                            "       OUT = lw_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("WORD LOGIC", "WOR_DW", 2, 3, "WOR_DW", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DWORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DWORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       lw_tmp0 = IN1;", 
                                            "       lw_tmp0 = lw_tmp0 & IN2;", 
                                            "       OUT = lw_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("WORD LOGIC", "WXOR_DW", 2, 3, "WXOR_DW", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DWORD"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DWORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       lw_tmp0 = IN1;", 
                                            "       lw_tmp0 = lw_tmp0 ^ IN2;", 
                                            "       OUT = lw_tmp0 ;",  
                                            "x1_LOOP:"
                                        })
                    }

                })
             },

//******************  INTEGER FCT.

//******************  INTEGER FCT. - INT-TYPE

            {new Category( "INTEGER FUNCTIONS", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("INTEGER FUNCTIONS", "ADD_I", 2, 3, "ADD_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       _func0_ADD_I( sw_tmp1, sw_tmp2, sw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "SUB_I", 2, 3, "SUB_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       _func0_SUB_I( sw_tmp1, sw_tmp2, sw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MUL_I", 2, 3, "MUL_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       _func0_MUL_I( sw_tmp1, sw_tmp2, sw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MUL_IDI", 2, 3, "MUL_IDI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       _func0_MUL_IDI( sw_tmp1, sw_tmp2, slw_tmp3 );",
                                            "       OUT = slw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "DIV_I", 2, 3, "DIV_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       _func0_DIV_I( sw_tmp1, sw_tmp2, sw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MULDIV_I", 2, 3, "MULDIV_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN3", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       sw_tmp3 = IN3;",  
                                            "       _func0_MULDIV_I( sw_tmp1, sw_tmp2, sw_tmp3, sw_tmp4, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp4;",  
                                            "x1_LOOP:"
                                        })
                    },

//******************  INTEGER FCT. - DINT-TYPE

                    {new ElementImage("INTEGER FUNCTIONS", "ADD_DI", 2, 3, "ADD_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       slw_tmp2 = IN2;",  
                                            "       _func0_ADD_DI( slw_tmp1, slw_tmp2, slw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "SUB_DI", 2, 3, "SUB_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       slw_tmp2 = IN2;",  
                                            "       _func0_SUB_DI( slw_tmp1, slw_tmp2, slw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MUL_DI", 2, 3, "MUL_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       slw_tmp2 = IN2;",  
                                            "       _func0_MUL_DI( slw_tmp1, slw_tmp2, slw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MUL_DIR", 2, 3, "MUL_DIR", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       slw_tmp2 = IN2;",  
                                            "       _func0_MUL_DIR( slw_tmp1, slw_tmp2, f_tmp3 );",
                                            "       OUT = f_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "DIV_DI", 2, 3, "DIV_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       slw_tmp2 = IN2;",  
                                            "       _func0_DIV_DI( slw_tmp1, slw_tmp2, slw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MOD_I", 2, 3, "MOD_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"INT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       sw_tmp2 = IN2;",  
                                            "       _func0_MOD_I( sw_tmp1, sw_tmp2, sw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "MOD_DI", 2, 3, "MOD_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"DINT"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       slw_tmp2 = IN2;",  
                                            "       _func0_MOD_DI( slw_tmp1, slw_tmp2, slw_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "ABS_I", 2, 3, "ABS_I", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"INT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       sw_tmp1 = IN1;",  
                                            "       _func0_ABS_I( sw_tmp1, sw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = sw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("INTEGER FUNCTIONS", "ABS_DI", 2, 3, "ABS_DI", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       slw_tmp1 = IN1;",  
                                            "       _func0_ABS_DI( slw_tmp1, slw_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = slw_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    }

                })
             },


//******************  FLOAT.POINT FUNCTIONS


            {new Category( "FLOAT.POINT FUNCTIONS", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "ADD_R", 2, 3, "ADD_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       f_tmp2 = IN2;",  
                                            "       _func0_ADD_R( f_tmp1, f_tmp2, f_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "SUB_R", 2, 3, "SUB_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       f_tmp2 = IN2;",  
                                            "       _func0_SUB_R( f_tmp1, f_tmp2, f_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "MUL_R", 2, 3, "MUL_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       f_tmp2 = IN2;",  
                                            "       _func0_MUL_R( f_tmp1, f_tmp2, f_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "DIV_R", 2, 3, "DIV_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN1", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN2", "IN", new List<string>() {"REAL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       f_tmp2 = IN2;",  
                                            "       _func0_DIV_R( f_tmp1, f_tmp2, f_tmp3, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp3;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "ABS_R", 2, 3, "ABS_R", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_ABS_R( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },

                    {new ElementImage("FLOAT.POINT FUNCTIONS", "SQRT", 2, 3, "SQRT", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_SQRT( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "SQR", 2, 3, "SQR", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_SQR( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "LN", 2, 3, "LN", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_LN( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "EXP", 2, 3, "EXP", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_EXP( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "SIN", 2, 3, "SIN", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_SIN( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "COS", 2, 3, "COS", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_COS( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "TAN", 2, 3, "TAN", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_TAN( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "ASIN", 2, 3, "ASIN", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_ASIN( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "ACOS", 2, 3, "ACOS", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_ACOS( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "ATAN", 2, 3, "ATAN", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_ATAN( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "GRD_RAD", 2, 3, "GRD_RAD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_GRD_RAD( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("FLOAT.POINT FUNCTIONS", "RAD_GRD", 2, 3, "RAD_GRD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"REAL"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"REAL"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",                                               
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       f_tmp1 = IN1;",  
                                            "       _func0_RAD_GRD( f_tmp1, f_tmp2, ENO ); //  ENO = 0 - Overflow Status",
                                            "       OUT = f_tmp2;",  
                                            "x1_LOOP:"
                                        })
                    }

                })
             },


//******************  MOVE

            {new Category( "MOVE FUNCTIONS", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("MOVE FUNCTIONS", "MOVE_32", 2, 3, "MOVE_32", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"DWORD","DINT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"BYTE","WORD","DWORD","INT","DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       lw_tmp0 = IN;", 
                                            "       OUT = lw_tmp0;", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MOVE FUNCTIONS", "MOVE_16", 2, 3, "MOVE_16", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"WORD","INT"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"BYTE","WORD","DWORD","INT","DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       w_tmp0 = IN;", 
                                            "       OUT = w_tmp0;", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MOVE FUNCTIONS", "MOVE_8", 2, 3, "MOVE_8", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN", "IN", new List<string>() {"BYTE"}, null),
                                                new ElementImage.IO_Data("OUT", "OUT", new List<string>() {"BYTE","WORD","DWORD","INT","DINT"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       b_tmp0 = IN;", 
                                            "       OUT = b_tmp0;", 
                                            "x1_LOOP:"
                                        })
                    }

                })
             },

//******************  TIMERS
/*http://control.com/thread/1026175631#1026175631
 * By Ken Muir on 23 June, 2003 - 6:47 pm
Martin,
There are several different ways of handling this.
Firstly, the S5Timer format you refer to is really a hangover in S7 from S5 PLCs. It was deliberately included in order to provide 
 * compatibility and familiarity for existing S5 users when S7 was launched. Since the basic concept of the S5Timer goes back 20 
 * years or more, it is an entirely proprietary format which uses the top nibble of the word to identify one of 3 time-bases, and 
 * the remaining 3 nibbles to identify the magnitude of the timer in BCD format. The overall duration is then the product of both 
 * the time-base and the magnitude.
The S7 system was designed to comply with the then emerging IEC61131-3 programming standard from the outset. This meant that a new 
 * data type named TIME was included, plus 3 standardised Function Blocks, all in accordance with IEC1131. You'll find them in the 
 * STEP7 Standard Library \ System Function Blocks. They are SFBs 3, 4 and 5.
The IEC TIME data type is actually a 32-bit variable, which effectively holds a time value as a number of milliseconds. 
 * Double Integer arithmetic can be used to perform manipulations of a TIME variable under certain conditions if your require.
You will also find a library folder named IEC Function Blocks. In here are two FCs, 33 and 40, which are used to convert between 
 * S5TIME and IEC TIME data types.
If your plan is to enter a time (nb: not TIME) value via your HMI, then you can use different approaches according to the range 
 * and resolution you require. If you define an HMI tag as a TIME data type it goes straight to the PLC as a TIME variable with 
 * anything from 1mS up to 24 days+ as the duration. Alternatively, an INTEGER HMI tag holding a number of seconds can be multipled 
 * in the PLC by 1000 to give mS, and then used as a 32-bit TIME variable etc.
It's not so much "can I do it?", more a case of "which of these 6 ways is best suited to me?"
Good luck
Regards
Ken Muir
 */


            {new Category( "TIMERS", new ObservableCollection<ElementImage>()
                { 

                    {new ElementImage("TIMERS", "SYS_TIME", 2, 3, "SYS_TIME", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null,
                                                new ElementImage.IO_Data("ST", "OUT", new List<string>() {"TIME", "DWORD"}, null),
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                null,
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI;",
                                            "       _func0_SYS_TIME( lw_tmp1 ); ",
                                            "       ST = lw_tmp1;"
                                         })
                    },


                    {new ElementImage("TIMERS", "S_PULSE", 2, 3, "S_PULSE", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                new ElementImage.IO_Data("BI", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   //  перешли от R_tmp0 к lw_tmp0 Из-за тоо что в функцию надо передавать 
                                            // какой-то один тип - выбрали lword в который помещаются все остальные 
                                            // типы.
                                            //"       R_tmp0 = R;", 
                                            //"       R_tmp0 = R_tmp0 & R_BIT_MASK;", 
                                            "       lw_tmp0 = R;", 
                                            "       lw_tmp0 = lw_tmp0 & R_BIT_MASK;", 
                                            //"       _func0_S_PULSE( MAIN_IN, ENI, TV, lw_tmp0, BI, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       lw_tmp3 = BI;",
                                            "       _func0_S_PULSE( lw_tmp1, ENI, lw_tmp2, lw_tmp0, lw_tmp3, ENO );",
                                            "       MAIN_IN = lw_tmp1;",
                                            "       BI = lw_tmp3;"

                                       })
                    },



                    {new ElementImage("TIMERS", "S_PEXT", 2, 3, "S_PEXT", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                new ElementImage.IO_Data("BI", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       lw_tmp0 = R;", 
                                            "       lw_tmp0 = lw_tmp0 & R_BIT_MASK;", 
                                            //"       _func0_S_PEXT( MAIN_IN, ENI, TV, lw_tmp0, BI, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       lw_tmp3 = BI;",
                                            "       _func0_S_PEXT( lw_tmp1, ENI, lw_tmp2, lw_tmp0, lw_tmp3, ENO );",
                                            "       MAIN_IN = lw_tmp1;",
                                            "       BI = lw_tmp3;"
                                        })
                    },



                    {new ElementImage("TIMERS", "S_ODT", 2, 3, "S_ODT", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                new ElementImage.IO_Data("BI", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       lw_tmp0 = R;", 
                                            "       lw_tmp0 = lw_tmp0 & R_BIT_MASK;", 
                                            //"       _func0_S_ODT( MAIN_IN, ENI, TV, lw_tmp0, BI, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       lw_tmp3 = BI;",
                                            "       _func0_S_ODT( lw_tmp1, ENI, lw_tmp2, lw_tmp0, lw_tmp3, ENO );",
                                            "       MAIN_IN = lw_tmp1;",
                                            "       BI = lw_tmp3;"
                                        })
                    },



                    {new ElementImage("TIMERS", "S_ODTS", 2, 3, "S_ODTS", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                new ElementImage.IO_Data("BI", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       lw_tmp0 = R;", 
                                            "       lw_tmp0 = lw_tmp0 & R_BIT_MASK;", 
                                            //"       _func0_S_ODTS( MAIN_IN, ENI, TV, lw_tmp0, BI, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       lw_tmp3 = BI;",
                                            "       _func0_S_ODTS( lw_tmp1, ENI, lw_tmp2, lw_tmp0, lw_tmp3, ENO );",
                                            "       MAIN_IN = lw_tmp1;",
                                            "       BI = lw_tmp3;"
                                        })
                    },




                    {new ElementImage("TIMERS", "S_OFFDT", 2, 3, "S_OFFDT", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                new ElementImage.IO_Data("BI", "OUT", new List<string>() {"DWORD"}, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("R", "IN", new List<string>() {"BOOL"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       lw_tmp0 = R;", 
                                            "       lw_tmp0 = lw_tmp0 & R_BIT_MASK;", 
                                            //"       _func0_S_OFFDT( MAIN_IN, ENI, TV, lw_tmp0, BI, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       lw_tmp3 = BI;",
                                            "       _func0_S_OFFDT( lw_tmp1, ENI, lw_tmp2, lw_tmp0, lw_tmp3, ENO );",
                                            "       MAIN_IN = lw_tmp1;",
                                            "       BI = lw_tmp3;"
                                        })
                    },



//**************    Таймеры с раздельными Start и Reset.

                    {new ElementImage("TIMERS", "RES_T", 2, 3, "RES_T", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI;",
                                            //"       _func0_RES_T( MAIN_IN, ENI, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       _func0_S_ODT( lw_tmp1, ENI, ENO );",
                                            "       MAIN_IN = lw_tmp1;"
                                        })
                    },



                    {new ElementImage("TIMERS", "SP", 2, 3, "SP", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            //"       _func0_SP( MAIN_IN, ENI, TV, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       _func0_SP( lw_tmp1, ENI, lw_tmp2, ENO );",
                                            "       MAIN_IN = lw_tmp1;"
                                      })
                    },


                    {new ElementImage("TIMERS", "SE", 2, 3, "SE", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            //"       _func0_SP( MAIN_IN, ENI, TV, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       _func0_SE( lw_tmp1, ENI, lw_tmp2, ENO );",
                                            "       MAIN_IN = lw_tmp1;"
                                      })
                    },


                    {new ElementImage("TIMERS", "SS", 2, 3, "SS", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            //"       _func0_SS( MAIN_IN, ENI, TV, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       _func0_SS( lw_tmp1, ENI, lw_tmp2, ENO );",
                                            "       MAIN_IN = lw_tmp1;"
                                        })
                    },

    

                    {new ElementImage("TIMERS", "SD", 2, 3, "SD", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            //"       _func0_SD( MAIN_IN, ENI, TV, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       _func0_SD( lw_tmp1, ENI, lw_tmp2, ENO );",
                                            "       MAIN_IN = lw_tmp1;"
                                        })
                    },


                    {new ElementImage("TIMERS", "SF", 2, 3, "SF", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            {     
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("TV", "IN", new List<string>() {"TIME", "DWORD", "WORD"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("MAIN_IN", "IN", new List<string>() {"TIMER"}, null),
                                                null
                                            }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            //"       _func0_SF( MAIN_IN, ENI, TV, ENO );"
                                            "       lw_tmp1 = MAIN_IN;",
                                            "       lw_tmp2 = TV;",
                                            "       _func0_SF( lw_tmp1, ENI, lw_tmp2, ENO );",
                                            "       MAIN_IN = lw_tmp1;"
                                        })
                    }


                })
             },

//******************   MESSAGEs

            {new Category( "MESSAGE", new ObservableCollection<ElementImage>()
                { 
                    {new ElementImage("MESSAGE", "FMSG_S", 2, 3, "FMSG_S", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"FMSG"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_SetAvMsg( IN );", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "WMSG_S", 2, 3, "WMSG_S", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"WMSG"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_SetPrMsg( IN );", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "SMSG_S", 2, 3, "SMSG_S", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"SMSG"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_SetSrvMsg( IN );", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "FMSG_C", 2, 3, "FMSG_C", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"FMSG"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_ClrAvMsg( IN );", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "WMSG_C", 2, 3, "WMSG_C", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"WMSG"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_ClrPrMsg( IN );", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "SMSG_C", 2, 3, "SMSG_C", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("IN",  "IN", new List<string>() {"SMSG"}, null),
                                                null
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_ClrSrvMsg( IN );", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "FMSG_C_ALL", 2, 3, "FMSG_C_ALL", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_ClrAllAvMsg();", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "WMSG_C_ALL", 2, 3, "WMSG_C_ALL", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_ClrAllPrMsg();", 
                                            "x1_LOOP:"
                                        })
                    },
                    {new ElementImage("MESSAGE", "SMSG_C_ALL", 2, 3, "SMSG_C_ALL", true,
                                        new List<List<ElementImage.IO_Data>>() 
                                        {
                                            new List<ElementImage.IO_Data>() 
                                            { 
                                                new ElementImage.IO_Data("ENI",  "IN", null, null),
                                                new ElementImage.IO_Data("ENO", "OUT", null, null)
                                            },
                                            new List<ElementImage.IO_Data>() {      null,   null  }
                                        },
                                        null,
                                        null, null, 
                                        new List<string>() { "Left", "Right" },
                                        new List<string>() 
                                        {   
                                            "       ENO = ENI ;",   
                                            "       if ( ENI == 0 ) goto x1_LOOP;",  
                                            "       _func0_ClrAllSrvMsg();", 
                                            "x1_LOOP:"
                                        })
                    }
                })
             },


//******************   USER FUNCTIONS

            {new Category ( "USER FUNCTIONS", new ObservableCollection<ElementImage>())}


         };
            
        } //-------------  END of Class ElementImagesDictionary

    } //-------------  END of Class Step5
}
