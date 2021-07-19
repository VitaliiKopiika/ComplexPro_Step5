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

        //********    BASE_SYMBOLS

        public partial class SYMBOLS
        {
            //************    PANELs

            static public Window BASE_SYMBOLS_LIST_window;
            
            //static public DataGrid BASE_SYMBOLS_LIST_PANEL { get; set; }

            static public ObservableCollection<Symbol_Data> BASE_SYMBOLS_LIST { get; set; }

            static public bool Base_Validation_Ok = false;
            static public string Base_Validation_Error_Line = null;


//****************************************************************

//************   FOR XPS PRINTING

public static List<List<string>> BASE_SYMBOLS_LIST_ToXpsList(ObservableCollection<Symbol_Data> symbols_list,
               out List<List<string>> headers, out List<string> alignment, out List<double> collumn_widths)
{
        headers = new List<List<string>>() 
                        { 
                            new List<string> () {"#", "Symbol", "Symbol tag", "Bit base", "Bit number", "Data type", "Initial value", "Comment" }
                        };
        alignment = new List<string>() 
                        {                             
                            "Center", "Left", "Left", "Center", "Center", "Center", "Center", "Left" 
                        };
        collumn_widths = new List<double>() { 30, 100, 100, 70, 70, 70, 70, 350 };
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
                list1.Add(symbol.Tag_Name);
                //list1.Add(symbol.Memory_Type);
                //list1.Add(symbol.str_Address);
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

//***************   For ComboBox for Tags Names

public static List<string> Get_BaseSymbolsTags_List(ObservableCollection<Symbol_Data> in_symbols_list, string type)
{
    try
    {
        List<string> bit_base_list = new List<string>();

        bit_base_list.Add(""); //null); //  1-й элемент Null, чтобы можно было установить нулевой выбор.

        foreach (Symbol_Data symbol in in_symbols_list)
        {
            if (symbol.Data_Type == type || type == null) bit_base_list.Add(symbol.Name);
        }

        return bit_base_list;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}



//***************   SHOW_BASE_SYMBOLS_LIST

public Border SHOW_BASE_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {
        DataGrid datagrid = symbols_list_window.Tag as DataGrid;

        Grid grid = datagrid.Parent as Grid;
        

//****************   COLUMNS

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        //DataGridComboBoxColumn tag_column = new DataGridComboBoxColumn();
                        DataGridTemplateColumn tag_column = DataGridUserComboBoxColumn("Tag_Name", new Tags_source_column_Converter());
                        DataGridComboBoxColumn memory_type_column = new DataGridComboBoxColumn();
                        //DataGridTextColumn address_column = new DataGridTextColumn();
                        DataGridComboBoxColumn bit_base_column = new DataGridComboBoxColumn();
                        DataGridTemplateColumn bit_number_column = DataGridUserComboBoxColumn("str_Bit_Number", new Bit_number_column_Converter()); 
                        //DataGridComboBoxColumn bit_number_column = new DataGridComboBoxColumn();
                        DataGridComboBoxColumn data_type_column = new DataGridComboBoxColumn();
                        DataGridTextColumn initial_value_column = new DataGridTextColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

                        DataGridComboBoxColumn nom_column = new DataGridComboBoxColumn();
                        DataGridTextColumn k_column = new DataGridTextColumn();
                        DataGridTextColumn unit_column = new DataGridTextColumn();
                        DataGridComboBoxColumn format_column = new DataGridComboBoxColumn();

//****************   COLUMNS  READ ONLY

                        item_number_column.IsReadOnly = true;

                        //nom_column.IsReadOnly = true;

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        tag_column.Header           = "Tag name";
                        memory_type_column.Header   = "Memory";
                        //address_column.Header       = "Address";
                        bit_base_column.Header      = "Bit base";
                        bit_number_column.Header    = "Bit num";
                        data_type_column.Header     = "Data type";
                        initial_value_column.Header = "Initial value";
                        comment_column.Header       = "Comment";

                        nom_column.Header           = "Nom";
                        k_column.Header             = "K";
                        unit_column.Header          = "Units";
                        format_column.Header        = "Format";

//**************

                        item_number_column.Width    =  30;
                        name_column.Width           = 100;
                        tag_column.Width            = 100;
                        memory_type_column.Width    = 70;
                        //address_column.Width        = 70;
                        bit_base_column.Width       = 100;
                        bit_number_column.Width     =  70;
                        data_type_column.Width      =  70;
                        initial_value_column.Width  =  70;
                        comment_column.Width        = 200;
        
                        nom_column.Width = 70;
                        k_column.Width = 50;
                        unit_column.Width = 50;
                        format_column.Width = 70;

        
//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new Base_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = BASE_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                            name_column.Binding = name_column_binding;


                            /*  
                            //    Перешел от DataGridComboBoxColumn к DataGridTemplateColumn, которая позволяет назначать каждой строке в колонке 
                            //  свой список ItemsSource в отличие от DataGridComboBoxColumn где назначается один ItemsSource для всех строк колонки 
                            //  и переназначение его невозможно.
                             * 
                             * tag_column.ItemsSource = Get_BaseSymbolsTags_List(BASE_SYMBOLS_TAGS_LIST, null);
                            Binding tag2_column_binding = new Binding();
                                tag2_column_binding.Source = datagrid;
                                tag2_column_binding.Path = new PropertyPath("SelectedItem");
                                tag2_column_binding.Converter = new Tags_source_column_Converter();
                                //bit2_number_column_binding.ConverterParameter = ;
                            BindingOperations.SetBinding(tag_column, DataGridComboBoxColumn.ItemsSourceProperty, tag2_column_binding);   

                            Binding tag_column_binding = new Binding();
                                tag_column_binding.Path = new PropertyPath("Tag_Name");
                                tag_column_binding.Mode = BindingMode.TwoWay;
                            tag_column.SelectedItemBinding = tag_column_binding;
                                */

                            Binding tag_column_binding = new Binding();
                            tag_column_binding.Path = new PropertyPath("Tag_Name");
                            tag_column_binding.Mode = BindingMode.TwoWay;
                            tag_column.ClipboardContentBinding = tag_column_binding;

                            memory_type_column.ItemsSource = BASE_MEMORY_TYPES;
                            Binding memory_type_column_binding = new Binding();
                            memory_type_column_binding.Path = new PropertyPath("Memory_Type");
                            memory_type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            memory_type_column.SelectedItemBinding = memory_type_column_binding;


                            //bit_base_column.ItemsSource = Get_BitBaseSymbols_List(BASE_SYMBOLS_LIST);
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
                          /*  Binding bit2_number_column_binding = new Binding();
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

                            data_type_column.ItemsSource = BASE_DATA_TYPES;
                            Binding data_type_column_binding = new Binding();
                            data_type_column_binding.Path = new PropertyPath("Data_Type");
                            data_type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            data_type_column.SelectedItemBinding = data_type_column_binding;
        

                            Binding initial_value_column_binding = new Binding();
                            initial_value_column_binding.Path = new PropertyPath("Initial_Value");
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

                            //MultiBinding multi_binding = new MultiBinding();
                            //multi_binding.Bindings.Add(data_type_column_binding);
                            //multi_binding.Bindings.Add(address_column_binding);
                            //multi_binding.Bindings.Add(initial_value_column_binding);
                            //multi_binding.Converter = new MultiBinding_Converter();
                            //initial_value_column.Binding = multi_binding;

//****************   BINDING GROUP  for  VALIDATION
                            //            перешел от Group_Validation встроенного на вручную по событиям
                            //            после того как добавил TemplateColumn для TagName, которая не имеет Binding и 
                            //            соотв. GroupValidation не может проверить TagNames.

                            //BindingGroup bindinggroup = new BindingGroup();
                            //bindinggroup.NotifyOnValidationError = true;
                            //bindinggroup.ValidationRules.Add( new Base_Group_ValidationRule());
                            //bindinggroup.ValidationRules.Add(new ExceptionValidationRule());
                        ////  привязываем Binding не ко всей datagrid, а только к row-строке.
                            //datagrid.ItemBindingGroup = bindinggroup;
                        ////datagrid.BindingGroup = bindinggroup;

                        ////    Обработчик вывода сообщений для всех Validation общий.
                        //// можно и так и так Validation.AddErrorHandler(datagrid, new EventHandler<ValidationErrorEventArgs>(Address_ValidationErrorEvent));
                            //datagrid.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(Base_ValidationErrorEvent));
        

//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
                        datagrid.Columns.Add(data_type_column);
                        datagrid.Columns.Add(tag_column);
                        datagrid.Columns.Add(memory_type_column);
                        //datagrid.Columns.Add(address_column);
                        datagrid.Columns.Add(bit_base_column);
                        datagrid.Columns.Add(bit_number_column);
                        datagrid.Columns.Add(initial_value_column);
                        //  НАДО СНАЧАЛА ДОБАВИТЬ ПОЛЯ ИНФОРМАЦИИ В pCC.INI 
                        //datagrid.Columns.Add(nom_column);
                        //datagrid.Columns.Add(k_column);
                        //datagrid.Columns.Add(unit_column);
                        //datagrid.Columns.Add(format_column);

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
                        Button button_LoadTags = Get_Button("Symbols Tags", Base_button_SymbolsTags_Click, symbols_list_window);
     
                        // иначе по cancel окно закрывается безусловно  button_Cancel.IsCancel = true;
                        FocusManager.SetFocusedElement(symbols_list_window, button_Cancel);


                        stackpanel.Children.Add(button_AddRow);
                        stackpanel.Children.Add(button_CopyRow);
                        stackpanel.Children.Add(button_InsertRow);
                        stackpanel.Children.Add(button_DeleteRow);
                        stackpanel.Children.Add(button_BindToElement);
                        stackpanel.Children.Add(button_LoadTags); 
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




//***************   CONVERTERS

//**************    MULTI BINDING CONVERTER 


public class Base_Item_number_column_Converter : IValueConverter
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

//***********  TAGS SOURCE CONVERTER

public class Tags_source_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            //  пока ничего не выбрано берем максимальный список чтобы всем угодить.
            if (value == null || String.IsNullOrWhiteSpace(((Symbol_Data)value).Data_Type))
            {
                return Get_BaseSymbolsTags_List(BASE_TAGS_SYMBOLS_LIST, null);
            }

            string selected_item_type = null;

            //  ищем блок данных переменной selected_item_type по ее имени.
            foreach (SYMBOLS.Symbol_Data symbol in ((Symbol_Data)value).Owner)
            {
                if (symbol.Name == ((Symbol_Data)value).Name)
                {
                    selected_item_type = symbol.Data_Type;
                    break;
                }
            }
            //  переменная не найдена.
            if (selected_item_type == null)
            {
                return Get_BaseSymbolsTags_List(BASE_TAGS_SYMBOLS_LIST, null); // иначе вся колонка перестает показывать номера битов и где надо и где не надо
            }

            return Get_BaseSymbolsTags_List(BASE_TAGS_SYMBOLS_LIST, selected_item_type);

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


//***************  VALIDATION_RULES  ***********************

//***************  TAG VALIDATION

/*public class Tag_ValidationRule : ValidationRule
{
    //int min, max;

    public Tag_ValidationRule(int maxx) : base()
    {
      //  min = 1;  max = maxx;
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            int ax = ((string)value).Length;

            return new ValidationResult(false, msg);

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

//***************  GROUP VALIDATION
//            перешел от Group_Validation встроенного на вручную по событиям
//            после того как добавил TemplateColumn для TagName, которая не имеет Binding и 
//            соотв. GroupValidation не может проверить TagNames.

public class Base_Group_ValidationRule : ValidationRule
{

    // Ensure that an item over $100 is available for at least 7 days.
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    
        try
        {

            // Сделано и здесь и по прерыванию Validation.ErrorRemoved т.к. 
            // Он не заходит в Validation если ошибка исправлена по Esc.
            Base_Validation_Ok = false;

            ValidationResult valid_res;

            BindingGroup bg = value as BindingGroup;
                        
            // Get the source object.
            Symbol_Data item = bg.Items[0] as Symbol_Data;

            //  Определение номера проверяемой строки.
            //  Изнутри этого класса нет доступа к коллекции SYMBOLS_LIST.
            DataGridRow datagridrow = (DataGridRow)bg.Owner;
            int item_index = datagridrow.GetIndex() + 1;
            Base_Validation_Error_Line = item_index.ToString();

            object o_name = null;
            object o_tag_name = null;
            object o_data_type = null;
            object o_memory_type = null;
            //object o_address = null;
            object o_bit_base = null;
            object o_bit_number = null;
            object o_initial_value = null;
            object o_comment = null;

            // Get the proposed values for Price and OfferExpires.

            if (!bg.TryGetValue(item, "Name", out o_name))                      { return new ValidationResult(false, "Property 'Symbol.Name' not found!");  }
            if (!bg.TryGetValue(item, "Tag_Name", out o_tag_name))              { return new ValidationResult(false, "Property 'Symbol.Tag_Name' not found!"); }
            if (!bg.TryGetValue(item, "Data_Type", out o_data_type))            { return new ValidationResult(false, "Property 'Symbol.Data_Type' not found!"); }
            if (!bg.TryGetValue(item, "Memory_Type", out o_memory_type))        { return new ValidationResult(false, "Property 'Symbol.Memory_Type' not found!"); }
            //if (!bg.TryGetValue(item, "str_Address", out o_address))            { return new ValidationResult(false, "Property 'Symbol.Address' not found!"); }
            if (!bg.TryGetValue(item, "Bit_Base", out o_bit_base))          { return new ValidationResult(false, "Property 'Symbol.Bit_Base' not found!"); }            
            if (!bg.TryGetValue(item, "str_Bit_Number", out o_bit_number))      { return new ValidationResult(false, "Property 'Symbol.Bit_Number' not found!"); }
            if (!bg.TryGetValue(item, "Initial_Value", out o_initial_value))    { return new ValidationResult(false, "Property 'Symbol.Initial_Value' not found!"); }
            if (!bg.TryGetValue(item, "Comment", out o_comment))                { return new ValidationResult(false, "Property 'Symbol.Comment' not found!"); }

            string name = (string)o_name;
            //string tag_name = (string)o_tag_name; //так сделать для TemplateColumn не получается т.к. пишет "Property 'Symbol.Tag_Name' not found!"
            string tag_name = item.Tag_Name;
            string data_type = (string)o_data_type;
            string memory_type = (string)o_memory_type;
            //string address = (string)o_address;
            string bit_base = (string)o_bit_base;
            string bit_number = (string)o_bit_number;
            string initial_value = (string)o_initial_value;
            string comment = (string)o_comment;

//********   NAME VALIDATION
//********   COMMENT  VALIDATION
//********   NAME DUPLICATING VALIDATION

            if ((valid_res = NAME_COMMENT_Validation(item_index, item, name, comment)) != null) return valid_res;

//********   TAG VALIDATION

            //if ((valid_res = TAG_Validation(item_index, tag_name, data_type, memory_type)) != null) return valid_res;
            if ((valid_res = TAG_Validation(item_index, item)) != null) return valid_res;
            

//********   MEMORY-DATA TYPES VALIDATION
            
//********   ADDRESS VALIDATION

//********   BIT BASE & NUMBER VALIDATION

            if ((valid_res = BIT_Validation(item_index, bit_base, bit_number, data_type, BASE_SYMBOLS_LIST)) != null) return valid_res;


//********   INITIAL VALUE VALIDATION

            if ((valid_res = INITIAL_VALUE_Validation(item_index, item)) != null) return valid_res;

//********  END

            // и здесь и в прерывании по Validation 
            Base_Validation_Ok = true;
            Base_Validation_Error_Line = null;

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

//********   TAG VALIDATION

static public ValidationResult TAG_Validation ( int item_index, Symbol_Data symbol )//string tag_name, string data_type, string memory_type)
{
    try
    {
            if (symbol.Data_Type == "BOOL")
            {
                if (symbol.Tag_Name != null && symbol.Tag_Name.Length != 0)
                {
                    symbol.Tag_Name = null;
                    return new ValidationResult(false, "Tag name must be empty for BOOL-type. Line: " + item_index + ".");
                }
            }
            else
            {
                if (symbol.Tag_Name == null || symbol.Tag_Name.Length == 0)
                {
                    return new ValidationResult(false, "Tag name isn't set. Line: " + item_index + ".");
                }
                    

                SYMBOLS.Symbol_Data base_symbol = null;
                foreach (SYMBOLS.Symbol_Data tag in SYMBOLS.BASE_TAGS_SYMBOLS_LIST)
                {
                    if (symbol.Tag_Name == tag.Name)
                    {
                        symbol.Data_Type = tag.Data_Type;
                        symbol.Memory_Type = tag.Memory_Type;
                        if (String.IsNullOrWhiteSpace(symbol.Comment)) symbol.Comment = tag.Comment;

                        //if (tag.Data_Type != symbol.Data_Type)
                        //{
                          //  return new ValidationResult(false, "Corrected: Symbol data type didn't match tag data type. Line: " + item_index + ".");
                        //}

                        //if (tag.Memory_Type != symbol.Memory_Type)
                        //{
                          //  return new ValidationResult(false, "Corrected: Symbol memory type didn't match tag memory type. Line: " + item_index + ".");
                        //}

                        base_symbol = tag;
                        break;
                    }
                }

                if (base_symbol == null) return new ValidationResult(false, "Tag name not found. Line: " + item_index + ".");
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

//***************  VALIDATION  HANDLER      

    private void Base_ValidationErrorEvent(object sender, ValidationErrorEventArgs e)
    {
        if (e.Action == ValidationErrorEventAction.Added)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());

            //---
            // Сделано по прерыванию Validation.ErrorRemoved т.к. 
            // он не заходит в Validation если ошибка исправлена по Esc, а сюда заходит.
            Base_Validation_Ok = false;
        }
        else if (e.Action == ValidationErrorEventAction.Removed)
        {
            //MessageBox.Show("Error removed!");
            Base_Validation_Ok = true;
        }


        e.Handled = true;
    }


//**********************************************************

//***************   HANDLERS  ******************************

//**********   IS READ ONLY для базовых переменных.

void datagrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
{
    try
    {   //  После  того как изменили с Оноприенко стратегию базовых пееремнных на введение Tag-ов эта блокировка не нужна.
        //Symbol_Data symbol = (Symbol_Data)((DataGrid)sender).CurrentItem;

        //if (symbol != null)
        //{
            //if (symbol.Memory_Type == "BI" || symbol.Memory_Type == "BQ" || symbol.Memory_Type == "BIB" ||
                 //symbol.Memory_Type == "BQB" || symbol.Memory_Type == "BIW" || symbol.Memory_Type == "BQB")
            //{
              //((DataGrid)sender).IsReadOnly = true;
            //}
            //else ((DataGrid)sender).IsReadOnly = false;

            ////   через Binding это сделать не получилось.
            ////if (symbol.Data_Type == "COUNTER")
            ////{
                ////symbol.str_Initial_Value = symbol.str_Address;
            ////}

        //}

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    
}



//****************
void Base_button_SymbolsTags_Click(object sender, RoutedEventArgs e)
{
    try
    {
        SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.BASE_TAGS_SYMBOLS_LIST);

        REFRESH_SYMBOLS_LIST_window((Window)((Button)sender).Tag);
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


/*void Base_button_AddRow_Click(object sender, RoutedEventArgs e)
{
    try
    {   //  Вставка строки в конце списка.
        BASE_SYMBOLS_LIST.Add(new Symbol_Data(BASE_SYMBOLS_LIST, null));
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

void Base_button_InsertRow_Click(object sender, RoutedEventArgs e)
{
    try
    {

        //  Вставка строки в список.
        Symbol_Data selecteditem = (Symbol_Data)BASE_SYMBOLS_LIST_PANEL.SelectedItem;

        if (selecteditem != null)
        {
            int index = BASE_SYMBOLS_LIST.IndexOf(selecteditem);
            if (index > -1)
            {
                //  Вставляем пустые строки или скопированные.
                if (SYMBOLS_LIST_BUFFER == null)
                {
                    BASE_SYMBOLS_LIST.Insert(index, new Symbol_Data(BASE_SYMBOLS_LIST, null));
                }
                else
                {
                    foreach (Symbol_Data item in SYMBOLS_LIST_BUFFER)
                    {
                        BASE_SYMBOLS_LIST.Insert(index++,
                        new Symbol_Data(BASE_SYMBOLS_LIST, item.Name, item.Tag_Name, item.Data_Type, item.Memory_Type, item.Address, item.Bit_Base, item.Bit_Number, item.Initial_Value, item.Comment));
                    }

                    SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
                }

                //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
                RESHOW_BASE_SYMBOLS_LIST_window();
            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void Base_button_CopyRow_Click(object sender, RoutedEventArgs e)
{
    try
    {       
        SYMBOLS_LIST_BUFFER = new List<Symbol_Data>(); 

        foreach (SYMBOLS.Symbol_Data item in BASE_SYMBOLS_LIST_PANEL.SelectedItems)
        {
            SYMBOLS_LIST_BUFFER.Add(item);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void Base_button_DeleteRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //  Удаление выбранных строк списка.
        
            for (int tst = 0; tst == 0; )
            {   //   перед удаление строки удаляем все элементы из грид привязанные к ней,
                // т.к. иначе грид запихнет их на ставшую последней строку.
                tst = 1;
                foreach (SYMBOLS.Symbol_Data item in BASE_SYMBOLS_LIST_PANEL.SelectedItems)
                {
                    BASE_SYMBOLS_LIST.Remove(item);
                        //  приходится прерывать поиск и начинать его сначала, т.к. 
                        //  коллекция после удаления изменилась и foreach не может 
                        //  продолжать поиск далее.
                    tst = 0;
                    break;
                }
            }

        //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
        RESHOW_BASE_SYMBOLS_LIST_window();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void Base_button_BindToElement_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (Base_Validation_Ok == true)
        {
            SYMBOLS_LIST_BUFFER = new List<Symbol_Data>();
            SYMBOLS_LIST_BUFFER.Add((Symbol_Data)BASE_SYMBOLS_LIST_PANEL.SelectedItem);
            //---
            BASE_SYMBOLS_LIST_window.Close();
        }
        else
        {
            MessageBox.Show("Can't bind. Error exists. Line: " + Base_Validation_Error_Line);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
*/

//void Base_button_Cancel_Click(object sender, RoutedEventArgs e)
//{
  //  try
//    {
        //if (SYMBOLS_LIST_PANEL.ItemBindingGroup.CommitEdit())
        //SYMBOLS_LIST_PANEL.CommitEdit();
        //SYMBOLS_LIST_PANEL.CancelEdit();
        //SYMBOLS_LIST_PANEL.Edit

        //  Вставили отмену как при закрытии окна, но там это нужно, а здесь для красоты.
        // ... потеря фокуса и Validation срабатывает раньше и отмена уже не работает 
        // SYMBOLS_LIST_PANEL.CancelEdit();

        //if (Base_Validation_Ok == true)
        //{
//            BASE_SYMBOLS_LIST_window.Close();
        //}
        //else
        //{
          //  MessageBox.Show("Can't exit. Error exists."); // + Base_Validation_Error_Line);
        //}
//    }
//    catch (Exception excp)
//    {
//        MessageBox.Show(excp.ToString());
//    }

//}

//***********************



        }  // ******  END of Class Mymory_Symbols

    }  // ******  END of Class Step5

}
