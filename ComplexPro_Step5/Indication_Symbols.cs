//  сделать сохранение и загрузку из файла
//  сделать nom, format, units для GlobalSymbols
//  разделить Validation для nom, format, units для GlobalSymbols
//  сделать Validation для индикации


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

        //********    INDICATION_SYMBOLS

        public partial class SYMBOLS
        {
            //************    PANELs

            static public Window INDICATION_SYMBOLS_LIST_window;
            
            //static public DataGrid INDICATION_SYMBOLS_LIST_PANEL { get; set; }

            static public ObservableCollection<Indication_Symbol_Data> INDICATION_SYMBOLS_LIST { get; set; }

            static List<Indication_Symbol_Data> INDICATION_SYMBOLS_LIST_BUFFER = new List<Indication_Symbol_Data>();

            public class Indication_Symbol_Data
            {
                //---

                public ObservableCollection<Indication_Symbol_Data> Owner { get; set; }

                string text, format1, format2;
                string name1, name2;
                

                public string Text
                {
                    get { return text; }
                    set { text = value; }
                }

                public string Name1
                {
                    get { return name1; }
                    set { name1 = value; }
                }

                public string Name2
                {
                    get { return name2; }
                    set { name2 = value; }
                }

                public string Format1
                {
                    get { return format1; }
                    set { format1 = value; }
                }

                public string Format2
                {
                    get { return format2; }
                    set { format2 = value; }
                }


                //***********   CONSTRUCTOR #1     Применяется при составлении списка в программе.
                public Indication_Symbol_Data()
                {
                    Owner = INDICATION_SYMBOLS_LIST;
                    //---
                    Name1 = Get_IndicationSymbolsTags_List()[0];  //не ставить 1-1 элемент т.к. GloblList может быть пустым  1]; // [0] - пустой
                    Name2 = null;

                    Text = "  " + Name1 + "  ";

                    Format1 = "+  xxx.xx ";
                    Format2 = null;
                  
                }

                //***********   CONSTRUCTOR #2     Применяется при загрузке списка из файла.
                public Indication_Symbol_Data( ObservableCollection<Indication_Symbol_Data> owner, 
                                    string text, string name_1, string name_2, string format_1, string format_2)
                {
                    Owner = owner;
                    //---
                    Text = text;

                    Name1 = name_1;
                    Name2 = name_2;
                    //if ( name_2 != null ) Name2 = GLOBAL_SYMBOLS_LIST.First(value => value.Name == name_2); 

                    Format1 = format_1;
                    Format2 = format_2;

                }

            }

//**********


//****************************************************************

//************   FOR XPS PRINTING

public static List<List<string>> INDICATION_SYMBOLS_LIST_ToXpsList(ObservableCollection<Indication_Symbol_Data> symbols_list,
               out List<List<string>> headers, out List<string> alignment, out List<double> collumn_widths)
{
        headers = new List<List<string>>() 
                        { 
                            new List<string> () {"#", "Text", "Symbol-1", "Symbol-2", "Format-1", "Format-2" }
                        };
        alignment = new List<string>() 
                        {                             
                            "Center", "Left", "Left", "Left", "Left", "Left"
                        };
        collumn_widths = new List<double>() { 30, 200, 150, 150, 150, 150 };
        //---

    try
    {
        List<List<string>> list0 = new List<List<string>>();

        int item_number = 1;
        foreach (Indication_Symbol_Data symbol in symbols_list)
        {
            List<string> list1 = new List<string>();

                list1.Add(item_number.ToString());
                list1.Add(symbol.Text);
                list1.Add(symbol.Name1);
                list1.Add(symbol.Name2);
                list1.Add(symbol.Format1);
                list1.Add(symbol.Format2);

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

//***************   For ComboBox for Tags Names

public static List<string> Get_IndicationSymbolsTags_List()//ObservableCollection<Symbol_Data> in_symbols_list, string type = null)
{
    try
    {
        ObservableCollection<Symbol_Data> in_symbols_list = GLOBAL_SYMBOLS_LIST;

        List<string> bit_base_list = new List<string>();

        bit_base_list.Add(""); //null); //  1-й элемент Null, чтобы можно было установить нулевой выбор.

        foreach (Symbol_Data symbol in in_symbols_list)
        {
            if (symbol.Data_Type == "BYTE" || symbol.Data_Type == "REAL" || 
                symbol.Data_Type == "WORD" || symbol.Data_Type == "INT" || 
                symbol.Data_Type == "DWORD" || symbol.Data_Type == "DINT" ) bit_base_list.Add(symbol.Name);
        }

        return bit_base_list;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}


//***************   SHOW_INDICATION_SYMBOLS_LIST

public Border SHOW_INDICATION_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {
        DataGrid datagrid = symbols_list_window.Tag as DataGrid;

        Grid grid = datagrid.Parent as Grid;

//****************   COLUMNS

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn text_column = new DataGridTextColumn();
                        DataGridComboBoxColumn symbol1_column = new DataGridComboBoxColumn();
                        DataGridComboBoxColumn symbol2_column = new DataGridComboBoxColumn();
                        //DataGridTextColumn nom1_column = new DataGridTextColumn();
                        //DataGridTextColumn nom2_column = new DataGridTextColumn();
                        //DataGridTextColumn unit1_column = new DataGridTextColumn();
                        //DataGridTextColumn unit2_column = new DataGridTextColumn();
                        //DataGridTextColumn k1_column = new DataGridTextColumn();
                        //DataGridTextColumn k2_column = new DataGridTextColumn();
                        DataGridComboBoxColumn format1_column = new DataGridComboBoxColumn();
                        DataGridComboBoxColumn format2_column = new DataGridComboBoxColumn();

//****************   HEADERS

                        item_number_column.Header   = "#";
                        text_column.Header          = "Text";
                        symbol1_column.Header       = "Symbol-1";
                        symbol2_column.Header       = "Symbol-2";
                        //nom1_column.Header          = "Nom-1";
                        //nom2_column.Header          = "Nom-2";
                        //unit1_column.Header         = "Unit-1";
                        //unit2_column.Header         = "Unit-2";
                        //k1_column.Header            = "K-1";
                        //k2_column.Header            = "K-2";
                        format1_column.Header       = "Format-1";
                        format2_column.Header       = "Format-2";
        
                        item_number_column.Width    =  50;
                        text_column.Width           = 150;
                        symbol1_column.Width        = 100;
                        symbol2_column.Width        = 100;
                        //nom1_column.Width = 100;
                        //nom2_column.Width           = 100;
                        //unit1_column.Width          = 100;
                        //unit2_column.Width          = 100;
                        //k1_column.Width             = 100;
                        //k2_column.Width             = 100;
                        format1_column.Width        = 100;
                        format2_column.Width        = 100;

                        item_number_column.IsReadOnly = true;

        
//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new Indication_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = INDICATION_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding text_column_binding = new Binding();
                            text_column_binding.Path = new PropertyPath("Text");
                            text_column_binding.Mode = BindingMode.TwoWay;
                            text_column.Binding = text_column_binding;


                            symbol1_column.ItemsSource = Get_IndicationSymbolsTags_List(); 
                            Binding symbol1_column_binding = new Binding();
                            symbol1_column_binding.Path = new PropertyPath("Name1");
                            symbol1_column_binding.Mode = BindingMode.TwoWay;
                            symbol1_column.SelectedItemBinding = symbol1_column_binding;

                            symbol2_column.ItemsSource = Get_IndicationSymbolsTags_List();
                            Binding symbol2_column_binding = new Binding();
                            symbol2_column_binding.Path = new PropertyPath("Name2");
                            symbol2_column_binding.Mode = BindingMode.TwoWay;
                            symbol2_column.SelectedItemBinding = symbol2_column_binding;
                  

                            format1_column.ItemsSource = FORMAT_TYPES.Keys;
                            Binding format1_column_binding = new Binding();
                            format1_column_binding.Path = new PropertyPath("Format1");
                            format1_column_binding.Mode = BindingMode.TwoWay;
                            format1_column.SelectedItemBinding = format1_column_binding;

                            format2_column.ItemsSource = FORMAT_TYPES.Keys;
                            Binding format2_column_binding = new Binding();
                            format2_column_binding.Path = new PropertyPath("Format2");
                            format2_column_binding.Mode = BindingMode.TwoWay;
                            format2_column.SelectedItemBinding = format2_column_binding;

//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(text_column);
                        datagrid.Columns.Add(symbol1_column);
                        datagrid.Columns.Add(symbol2_column);
                        datagrid.Columns.Add(format1_column);
                        datagrid.Columns.Add(format2_column);
        

//****************   BUTTONS  **************************

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Top;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                        Button button_AddRow = Get_Button("Add Row", button_Indication_AddRow_Click, symbols_list_window);
                        Button button_InsertRow = Get_Button("Insert Row", button_Indication_InsertRow_Click, symbols_list_window);
                        Button button_DeleteRow = Get_Button("Delete Row", button_Indication_DeleteRow_Click, symbols_list_window);
                        Button button_CopyRow = Get_Button("Copy Row", button_Indication_CopyRow_Click, symbols_list_window);
                        Button button_Cancel = Get_Button("Ok/Cancel", button_Cancel_Click, symbols_list_window);
     
                        // иначе по cancel окно закрывается безусловно  button_Cancel.IsCancel = true;
                        FocusManager.SetFocusedElement(symbols_list_window, button_Cancel);


                        stackpanel.Children.Add(button_AddRow);
                        stackpanel.Children.Add(button_CopyRow);
                        stackpanel.Children.Add(button_InsertRow);
                        stackpanel.Children.Add(button_DeleteRow);
                        stackpanel.Children.Add(button_Cancel);


                    grid.Children.Add(stackpanel);


        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return null;

}

//************   INDICATION VALIDATION 

static public string Indication_Validation_By_Event(ObservableCollection<Indication_Symbol_Data> symbols_list, DataGrid datagrid_panel, bool message = true)
{

    //Base_Validation_Ok = false;

    ValidationResult valid_res;

    StringBuilder error_str = new StringBuilder();

    int symbol_index = 0;

    SYMBOLS.Indication_Symbol_Data item_first_error = null;

    try
    {

        for (int i = 0; i < symbols_list.Count; i++)
        {
            symbol_index++;

            SYMBOLS.Indication_Symbol_Data symbol = symbols_list[i];

//********   TEXT VALIDATION

                // Is in range?    
                if (symbol.Text == null || symbol.Text.Length < 1 || symbol.Text.Length > 14)
                {
                    valid_res = new ValidationResult(false, "Text length must be between 1 and 14. Line: " + symbol_index + ".");
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }

//********   NAME1 VALIDATION

                if (symbol.Name1 == null || symbol.Name1 == "")
                {
                    valid_res = new ValidationResult(false, "Symbol-1 name isn't set. Line: " + symbol_index + ".");
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }
                else
                {
                    if (!SYMBOLS.GLOBAL_SYMBOLS_LIST.Any(value => (value.Name == symbol.Name1)))
                    {
                        valid_res = new ValidationResult(false, "Symbol-1 name \"" +  symbol.Name1 + "\" doesn't exist in Global symbols list. Line: " + symbol_index + ".");
                        error_str.Append("\n" + valid_res.ErrorContent.ToString());
                        if (item_first_error == null) item_first_error = symbol;
                        //break;
                    }
                }

//********   NAME2 VALIDATION

                if (symbol.Name2 == null || symbol.Name2 == "")
                {
                }
                else
                {
                    if (!SYMBOLS.GLOBAL_SYMBOLS_LIST.Any(value => (value.Name == symbol.Name2)))
                    {
                        valid_res = new ValidationResult(false, "Symbol-2 name \"" + symbol.Name2 + "\" doesn't exist in Global symbols list. Line: " + symbol_index + ".");
                        error_str.Append("\n" + valid_res.ErrorContent.ToString());
                        if (item_first_error == null) item_first_error = symbol;
                        //break;
                    }
                }
            
//********   FORMAT VALIDATION

                if (symbol.Format1 == null ) 
                {
                    valid_res = new ValidationResult(false, "Format-1 isn't set. Line: " + symbol_index + ".");
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }

                if (symbol.Format2 == null && (symbol.Name2 != null && symbol.Name2 != ""))
                {
                    valid_res = new ValidationResult(false, "Format-2 isn't set. Line: " + symbol_index + ".");
                    error_str.Append("\n" + valid_res.ErrorContent.ToString());
                    if (item_first_error == null) item_first_error = symbol;
                    //break;
                }

//********  END

        }

        if (error_str.Length == 0)
        {
            //Base_Validation_Ok = true;
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

            //if (message) MessageBox.Show("Errors found:" + error_str.ToString(), "Validation", MessageBoxButton.OK, MessageBoxImage.Error);

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



//***************   CONVERTERS

//**************    MULTI BINDING CONVERTER 


public class Indication_Item_number_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            return ((ObservableCollection<Indication_Symbol_Data>)parameter).IndexOf((Indication_Symbol_Data)value)+1;
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

//**********************************************************

//***************   HANDLERS  ******************************
            

void button_Indication_AddRow_Click(object sender, RoutedEventArgs e)
{
    try
    {   //  Вставка строки в конце списка.
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Indication_Symbol_Data> symbols_list = (ObservableCollection<Indication_Symbol_Data>)datagrid.Tag;

        symbols_list.Add(new Indication_Symbol_Data());
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


void button_Indication_CopyRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        INDICATION_SYMBOLS_LIST_BUFFER = new List<Indication_Symbol_Data>();

        foreach (SYMBOLS.Indication_Symbol_Data item in datagrid.SelectedItems)
        {
            INDICATION_SYMBOLS_LIST_BUFFER.Add(item);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void button_Indication_InsertRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Indication_Symbol_Data> symbols_list = (ObservableCollection<Indication_Symbol_Data>)datagrid.Tag;

        //  Вставка строки в список.
        Indication_Symbol_Data selecteditem = (Indication_Symbol_Data)datagrid.SelectedItem;

        if (selecteditem != null)
        {
            int index = symbols_list.IndexOf(selecteditem);
            if (index > -1)
            {
                //  Вставляем пустые строки или скопированные.
                if (INDICATION_SYMBOLS_LIST_BUFFER == null)
                {
                    symbols_list.Insert(index, new Indication_Symbol_Data());
                }
                else
                {
                    foreach (Indication_Symbol_Data item in INDICATION_SYMBOLS_LIST_BUFFER)
                    {
                        symbols_list.Insert(index++,
                       new Indication_Symbol_Data(symbols_list, item.Text, item.Name1, item.Name2, item.Format1, item.Format2));
                    }

                    INDICATION_SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
                }

                //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
                REFRESH_SYMBOLS_LIST_window((Window)((Button)sender).Tag); // SYMBOLS.INDICATION_SYMBOLS_LIST);

            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void button_Indication_DeleteRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DataGrid datagrid = (DataGrid)((Window)((Button)sender).Tag).Tag;
        ObservableCollection<Indication_Symbol_Data> symbols_list = (ObservableCollection<Indication_Symbol_Data>)datagrid.Tag;
        
        //  Удаление выбранных строк списка.
        
            for (int tst = 0; tst == 0; )
            {   //   перед удаление строки удаляем все элементы из грид привязанные к ней,
                // т.к. иначе грид запихнет их на ставшую последней строку.
                tst = 1;
                foreach (SYMBOLS.Indication_Symbol_Data item in datagrid.SelectedItems)
                {
                    symbols_list.Remove(item);
                        //  приходится прерывать поиск и начинать его сначала, т.к. 
                        //  коллекция после удаления изменилась и foreach не может 
                        //  продолжать поиск далее.
                    tst = 0;
                    break;
                }
            }

        //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
        REFRESH_SYMBOLS_LIST_window((Window)((Button)sender).Tag); // SYMBOLS.INDICATION_SYMBOLS_LIST);

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}



        }  // ******  END of Class Mymory_Symbols

    }  // ******  END of Class Step5

}
