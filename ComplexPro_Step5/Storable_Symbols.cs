//  сделать Validation для новых колонок min, max, ...
// + cделать сохранение/загрузку новых колок в файл
// + СДЕЛАТЬ распечатку новых колонок Коле в h.файл
// + сделать сохранение в файл и в XPS. 
// + Вставить в нужных местaх/месте занесение в базу элементов значка USER_FUNCTION

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

        //********    STORABLE_SYMBOLS

        public partial class SYMBOLS
        {
            //************    PANELs

            static public Window STORABLE_SYMBOLS_LIST_window;

            //static public DataGrid STORABLE_SYMBOLS_LIST_PANEL { get; set; }

            static public ObservableCollection<Symbol_Data> STORABLE_SYMBOLS_LIST { get; set; }

            static public bool Storable_Validation_Ok = false;
            static public string Storable_Validation_Error_Line = null;


//****************************************************************

//************   FOR XPS PRINTING

public static List<List<string>> STORABLE_SYMBOLS_LIST_ToXpsList(ObservableCollection<Symbol_Data> symbols_list,
               out List<List<string>> headers, out List<string> alignment, out List<double> collumn_widths)
{
        headers = new List<List<string>>() 
                        { 
                            new List<string> () {   "#", "Symbol", "Bit base", "Bit number", "Data type", "Initial value",
                                                     "Tag","Min", "Max", "Nom", "K", "Step", "Units", "Format", "Comment" }
                        };
        alignment = new List<string>() 
                        {                             
                            "Center", "Left", "Center", "Center", "Center", "Center", "Center", "Center", "Center", "Center", 
                            "Center","Center", "Center", "Center", "Left" 
                        };
        collumn_widths = new List<double>() { 30, 100, 70, 70, 70, 50, 
                                              70, 50, 50, 50, 50, 50, 50, 70, 200 };
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
                //list1.Add(symbol.Memory_Type);
                //list1.Add(symbol.str_Address);
                list1.Add(symbol.Bit_Base);
                list1.Add(symbol.str_Bit_Number);
                list1.Add(symbol.Data_Type);
                list1.Add(symbol.Initial_Value);

                list1.Add(symbol.Tag_Name);
                list1.Add(symbol.str_Min_Value);
                list1.Add(symbol.str_Max_Value);
                list1.Add(symbol.str_Nom_Value);
                list1.Add(symbol.str_K_Value);
                list1.Add(symbol.str_Step_Value);
                list1.Add(symbol.Unit_Value);
                list1.Add(symbol.Format_Value);

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



//***************   SHOW_STORABLE_SYMBOLS_LIST

public Border SHOW_STORABLE_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {
        DataGrid datagrid = symbols_list_window.Tag as DataGrid;

        Grid grid = datagrid.Parent as Grid;

//****************   COLUMNS

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        DataGridComboBoxColumn memory_type_column = new DataGridComboBoxColumn();
                        //DataGridTextColumn address_column = new DataGridTextColumn();
                        DataGridComboBoxColumn bit_base_column = new DataGridComboBoxColumn();
                        DataGridTemplateColumn bit_number_column = DataGridUserComboBoxColumn("str_Bit_Number", new Bit_number_column_Converter()); 
                        //DataGridComboBoxColumn bit_number_column = new DataGridComboBoxColumn();
                        DataGridComboBoxColumn data_type_column = new DataGridComboBoxColumn();
                        DataGridTextColumn initial_value_column = new DataGridTextColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

                        DataGridTextColumn tag_column = new DataGridTextColumn();
                        DataGridTextColumn min_column = new DataGridTextColumn();
                        DataGridTextColumn max_column = new DataGridTextColumn();
                        DataGridComboBoxColumn nom_column = new DataGridComboBoxColumn();
                        DataGridTextColumn k_column = new DataGridTextColumn(); 
                        DataGridTextColumn step_column = new DataGridTextColumn();
                        DataGridTextColumn unit_column = new DataGridTextColumn();
                        DataGridComboBoxColumn format_column = new DataGridComboBoxColumn();

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        memory_type_column.Header   = "Memory";
                        //address_column.Header       = "Address";
                        bit_base_column.Header = "Bit base";
                        bit_number_column.Header    = "Bit number";
                        data_type_column.Header     = "Data type";
                        initial_value_column.Header = "Initial value";
                        comment_column.Header       = "Comment";

                        tag_column.Header = "Tag";
                        min_column.Header = "Min";
                        max_column.Header = "Max";
                        nom_column.Header = "Nom";
                        k_column.Header = "K";
                        step_column.Header = "Step";
                        unit_column.Header = "Units";
                        format_column.Header = "Format";

//**************
        
                        item_number_column.Width    =  50;
                        name_column.Width           =  70;
                        memory_type_column.Width    =  70;
                        //address_column.Width        = 100;
                        bit_base_column.Width       =  70;
                        bit_number_column.Width     =  80;
                        data_type_column.Width      =  70;
                        initial_value_column.Width  =  80;
                        comment_column.Width        = 250;

                        item_number_column.IsReadOnly = true;

                        tag_column.Width = 70;
                        min_column.Width = 50;
                        max_column.Width = 50;
                        nom_column.Width = 70;
                        k_column.Width = 50;
                        step_column.Width = 50;
                        unit_column.Width = 50;
                        format_column.Width = 70;

//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new Storable_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = STORABLE_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                            name_column.Binding = name_column_binding;


                            memory_type_column.ItemsSource = STORABLE_MEMORY_TYPES;
                            Binding memory_type_column_binding = new Binding();
                            memory_type_column_binding.Path = new PropertyPath("Memory_Type");
                            memory_type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            memory_type_column.SelectedItemBinding = memory_type_column_binding;


                            //bit_base_column.ItemsSource = Get_BitBaseSymbols_List(STORABLE_SYMBOLS_LIST);
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
                           /* Binding bit2_number_column_binding = new Binding();
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

                            data_type_column.ItemsSource = STORABLE_DATA_TYPES;
                            Binding data_type_column_binding = new Binding();
                            data_type_column_binding.Path = new PropertyPath("Data_Type");
                            data_type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            data_type_column.SelectedItemBinding = data_type_column_binding;
        

                            Binding initial_value_column_binding = new Binding();
                            initial_value_column_binding.Path = new PropertyPath("fstr_Initial_Value");
                            initial_value_column_binding.Mode = BindingMode.TwoWay;
                            initial_value_column.Binding = initial_value_column_binding;


                            Binding comment_column_binding = new Binding();
                            comment_column_binding.Path = new PropertyPath("Comment");
                            comment_column_binding.Mode = BindingMode.TwoWay;
                            //comment_column_binding.Converter = new Comment_column_read_only_Converter();
                            //comment_column_binding.ConverterParameter = datagrid;

                            //comment_column_binding.ValidationRules.Add(new Comment_ValidationRule(96));
                                //// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                ////comment_column_binding.NotifyOnValidationError = true;
                            comment_column.Binding = comment_column_binding;

                //*******   Additional data

                            Binding tag_column_binding = new Binding();
                            tag_column_binding.Path = new PropertyPath("Tag_Name");
                            tag_column_binding.Mode = BindingMode.TwoWay;
                            tag_column.Binding = tag_column_binding;

                            Binding min_column_binding = new Binding();
                            min_column_binding.Path = new PropertyPath("fstr_Min_Value");
                            min_column_binding.Mode = BindingMode.TwoWay;
                            min_column.Binding = min_column_binding;

                            Binding max_column_binding = new Binding();
                            max_column_binding.Path = new PropertyPath("fstr_Max_Value");
                            max_column_binding.Mode = BindingMode.TwoWay;
                            max_column.Binding = max_column_binding;

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

                            Binding step_column_binding = new Binding();
                            step_column_binding.Path = new PropertyPath("fstr_Step_Value");
                            step_column_binding.Mode = BindingMode.TwoWay;
                            step_column.Binding = step_column_binding;

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

                            //MultiBinding multi_binding = new MultiBinding();
                            //multi_binding.Bindings.Add(data_type_column_binding);
                            //multi_binding.Bindings.Add(address_column_binding);
                            //multi_binding.Bindings.Add(initial_value_column_binding);
                            //multi_binding.Converter = new MultiBinding_Converter();
                            //initial_value_column.Binding = multi_binding;

//****************   BINDING GROUP  for  VALIDATION

                           /* BindingGroup bindinggroup = new BindingGroup();
                            bindinggroup.NotifyOnValidationError = true;
                            bindinggroup.ValidationRules.Add( new Storable_Group_ValidationRule());
                            bindinggroup.ValidationRules.Add(new ExceptionValidationRule());
                        //  привязываем Binding не ко всей datagrid, а только к row-строке.
                        datagrid.ItemBindingGroup = bindinggroup;
                        //datagrid.BindingGroup = bindinggroup;

                        //    Обработчик вывода сообщений для всех Validation общий.
                        // можно и так и так Validation.AddErrorHandler(datagrid, new EventHandler<ValidationErrorEventArgs>(Address_ValidationErrorEvent));
                        datagrid.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(Storable_ValidationErrorEvent));
                        */

//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
                        datagrid.Columns.Add(data_type_column);
                        datagrid.Columns.Add(memory_type_column);
                        //datagrid.Columns.Add(address_column);
                        datagrid.Columns.Add(bit_base_column);
                        datagrid.Columns.Add(bit_number_column);
                        datagrid.Columns.Add(initial_value_column);

                        datagrid.Columns.Add(tag_column);
                        datagrid.Columns.Add(min_column);
                        datagrid.Columns.Add(max_column);
                        datagrid.Columns.Add(nom_column);
                        datagrid.Columns.Add(k_column);
                        datagrid.Columns.Add(step_column);
                        datagrid.Columns.Add(unit_column);
                        datagrid.Columns.Add(format_column);

                        datagrid.Columns.Add(comment_column);


//****************   BUTTONS  **************************

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Top;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

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

                    grid.Children.Add(stackpanel);

        return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return null;

}


//****************


//***************   CONVERTERS

//**************    MULTI BINDING CONVERTER 


public class Storable_Item_number_column_Converter : IValueConverter
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



//***************  VALIDATION_RULES  ***********************

//***************  GROUP VALIDATION

public class Storable_Group_ValidationRule : ValidationRule
{

    // Ensure that an item over $100 is available for at least 7 days.
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    
        try
        {

            // Сделано и здесь и по прерыванию Validation.ErrorRemoved т.к. 
            // Он не заходит в Validation если ошибка исправлена по Esc.
            Storable_Validation_Ok = false;

            ValidationResult valid_res;

            BindingGroup bg = value as BindingGroup;

            // Get the source object.
            Symbol_Data item = bg.Items[0] as Symbol_Data;

            //  Определение номера проверяемой строки.
            //  Изнутри этого класса нет доступа к коллекции SYMBOLS_LIST.
            DataGridRow datagridrow = (DataGridRow)bg.Owner;
            int item_index = datagridrow.GetIndex() + 1;
            Storable_Validation_Error_Line = item_index.ToString();

            object o_name = null;
            object o_memory_type = null;
            object o_data_type = null;
            //object o_address = null;
            object o_bit_base = null;
            object o_bit_number = null;
            object o_initial_value = null;
            object o_comment = null;

            // Get the proposed values for Price and OfferExpires.

            if (!bg.TryGetValue(item, "Name", out o_name))                      { return new ValidationResult(false, "Property 'Symbol.Name' not found!");  }
            if (!bg.TryGetValue(item, "Memory_Type", out o_memory_type))        { return new ValidationResult(false, "Property 'Symbol.Memory_Type' not found!"); }
            if (!bg.TryGetValue(item, "Data_Type", out o_data_type))            { return new ValidationResult(false, "Property 'Symbol.Data_Type' not found!"); }
            //if (!bg.TryGetValue(item, "str_Address", out o_address))            { return new ValidationResult(false, "Property 'Symbol.Address' not found!"); }
            if (!bg.TryGetValue(item, "Bit_Base", out o_bit_base))          { return new ValidationResult(false, "Property 'Symbol.Bit_Base' not found!"); }            
            if (!bg.TryGetValue(item, "str_Bit_Number", out o_bit_number))      { return new ValidationResult(false, "Property 'Symbol.Bit_Number' not found!"); }
            if (!bg.TryGetValue(item, "Initial_Value", out o_initial_value))    { return new ValidationResult(false, "Property 'Symbol.Initial_Value' not found!"); }
            if (!bg.TryGetValue(item, "Comment", out o_comment))                { return new ValidationResult(false, "Property 'Symbol.Comment' not found!"); }

            string name = (string)o_name;
            string memory_type = (string)o_memory_type;
            string data_type = (string)o_data_type;
            //string address = (string)o_address;
            string bit_base = (string)o_bit_base;
            string bit_number = (string)o_bit_number;
            string initial_value = (string)o_initial_value;
            string comment = (string)o_comment;

//********   NAME VALIDATION
//********   COMMENT  VALIDATION
//********   NAME DUPLICATING VALIDATION

            if ((valid_res = NAME_COMMENT_Validation(item_index, item, name, comment)) != null) return valid_res;


//********   MEMORY-DATA TYPES VALIDATION
            
//********   ADDRESS VALIDATION

//********   BIT BASE & NUMBER VALIDATION

            if ((valid_res = BIT_Validation(item_index, bit_base, bit_number, data_type, STORABLE_SYMBOLS_LIST)) != null) return valid_res;

//********   INITIAL VALUE VALIDATION

            if ((valid_res = INITIAL_VALUE_Validation(item_index, item)) != null) return valid_res;

//********  END

            // и здесь и в прерывании по Validation 
            Storable_Validation_Ok = true;
            Storable_Validation_Error_Line = null;

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

//***************  VALIDATION  HANDLER

    private void Storable_ValidationErrorEvent(object sender, ValidationErrorEventArgs e)
    {
        if (e.Action == ValidationErrorEventAction.Added)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());

            //---
            // Сделано по прерыванию Validation.ErrorRemoved т.к. 
            // он не заходит в Validation если ошибка исправлена по Esc, а сюда заходит.
            Storable_Validation_Ok = false;
        }
        else if (e.Action == ValidationErrorEventAction.Removed)
        {
            //MessageBox.Show("Error removed!");
            Storable_Validation_Ok = true;
        }


        e.Handled = true;
    }




//**********************************************************

//***************   HANDLERS  ******************************


/*void Storable_button_AddRow_Click(object sender, RoutedEventArgs e)
{
    try
    {   //  Вставка строки в конце списка.
        STORABLE_SYMBOLS_LIST.Add(new Symbol_Data(STORABLE_SYMBOLS_LIST, null));
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

void Storable_button_InsertRow_Click(object sender, RoutedEventArgs e)
{
    try
    {

        //  Вставка строки в список.
        Symbol_Data selecteditem = (Symbol_Data)STORABLE_SYMBOLS_LIST_PANEL.SelectedItem;

        if (selecteditem != null)
        {
            int index = STORABLE_SYMBOLS_LIST.IndexOf(selecteditem);
            if (index > -1)
            {
                //  Вставляем пустые строки или скопированные.
                if (SYMBOLS_LIST_BUFFER == null)
                {
                    STORABLE_SYMBOLS_LIST.Insert(index, new Symbol_Data(STORABLE_SYMBOLS_LIST, null));
                }
                else
                {
                    foreach (Symbol_Data item in SYMBOLS_LIST_BUFFER)
                    {
                        STORABLE_SYMBOLS_LIST.Insert(index++,
                        new Symbol_Data(STORABLE_SYMBOLS_LIST, item.Name, null, item.Data_Type, item.Memory_Type, item.Address, item.Bit_Base, item.Bit_Number, item.Initial_Value, item.Comment));
                    }

                    SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
                }

                //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
                RESHOW_STORABLE_SYMBOLS_LIST_window();
            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void Storable_button_CopyRow_Click(object sender, RoutedEventArgs e)
{
    try
    {       
        SYMBOLS_LIST_BUFFER = new List<Symbol_Data>(); 

        foreach (SYMBOLS.Symbol_Data item in STORABLE_SYMBOLS_LIST_PANEL.SelectedItems)
        {
            SYMBOLS_LIST_BUFFER.Add(item);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void Storable_button_DeleteRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //  Удаление выбранных строк списка.
        
            for (int tst = 0; tst == 0; )
            {   //   перед удаление строки удаляем все элементы из грид привязанные к ней,
                // т.к. иначе грид запихнет их на ставшую последней строку.
                tst = 1;
                foreach (SYMBOLS.Symbol_Data item in STORABLE_SYMBOLS_LIST_PANEL.SelectedItems)
                {
                    STORABLE_SYMBOLS_LIST.Remove(item);
                        //  приходится прерывать поиск и начинать его сначала, т.к. 
                        //  коллекция после удаления изменилась и foreach не может 
                        //  продолжать поиск далее.
                    tst = 0;
                    break;
                }
            }

        //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
        RESHOW_STORABLE_SYMBOLS_LIST_window();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void Storable_button_BindToElement_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (Storable_Validation_Ok == true)
        {
            SYMBOLS_LIST_BUFFER = new List<Symbol_Data>();
            SYMBOLS_LIST_BUFFER.Add((Symbol_Data)STORABLE_SYMBOLS_LIST_PANEL.SelectedItem);
            //---
            STORABLE_SYMBOLS_LIST_window.Close();
        }
        else
        {
            MessageBox.Show("Can't bind. Error exists. Line: " + Storable_Validation_Error_Line);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
*/

//void Storable_button_Cancel_Click(object sender, RoutedEventArgs e)
//{
//    try
//    {
        //if (SYMBOLS_LIST_PANEL.ItemBindingGroup.CommitEdit())
        //SYMBOLS_LIST_PANEL.CommitEdit();
        //SYMBOLS_LIST_PANEL.CancelEdit();
        //SYMBOLS_LIST_PANEL.Edit

        //  Вставили отмену как при закрытии окна, но там это нужно, а здесь для красоты.
        // ... потеря фокуса и Validation срабатывает раньше и отмена уже не работает 
        // SYMBOLS_LIST_PANEL.CancelEdit();

        //if (Storable_Validation_Ok == true)
        //{
//            STORABLE_SYMBOLS_LIST_window.Close();
        //}
        //else
        //{
            //MessageBox.Show("Can't exit. Error exists. Line: " + Storable_Validation_Error_Line);
        //}
//    }
//    catch (Exception excp)
//    {
//        MessageBox.Show(excp.ToString());
//    }

//}



        }  // ******  END of Class Mymory_Symbols

    }  // ******  END of Class Step5

}
