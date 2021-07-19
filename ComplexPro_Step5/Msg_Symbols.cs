// + сделать остальные элементы для установки сообщений
// + сделать формирование MSG имен в h-файле
// + cделать при компиляции h-файла подмену текстов на номера сообщений

//  + сделать элемент для установки сообщений
//  + сделать привязку переменных к элементу
//  + доделать сохранение MSG-символов в файле 
//  + загрузку
//  + XPS



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

        //********    MSG_SYMBOLS

        public partial class SYMBOLS
        {
            //************    PANELs

            static public Window MSG_SYMBOLS_LIST_window;

            //static public DataGrid MSG_SYMBOLS_LIST_PANEL { get; set; }

            static public ObservableCollection<Symbol_Data> MSG_SYMBOLS_LIST { get; set; }

            //static public bool MSG_Validation_Ok = false;
            static public string MSG_Validation_Error_Line = null;


//****************************************************************

//************   FOR XPS PRINTING

public static List<List<string>> MSG_SYMBOLS_LIST_ToXpsList(ObservableCollection<Symbol_Data> symbols_list,
               out List<List<string>> headers, out List<string> alignment, out List<double> collumn_widths)
{
        headers = new List<List<string>>() 
                        { 
                            new List<string> () {"#", "Symbol", "Data type", "Initial value", "Comment" }
                        };
        alignment = new List<string>() 
                        {                             
                            "Center", "Left", "Center", "Center", "Left" 
                        };
        collumn_widths = new List<double>() { 30, 100, 70, 250, 350 };
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


//***************   SHOW_MSG_SYMBOLS_LIST

public Border SHOW_MSG_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {
        DataGrid datagrid = symbols_list_window.Tag as DataGrid;

        Grid grid = datagrid.Parent as Grid;
       

//****************   COLUMNS

// 123 InputBitPanel	M     100.3	BOOL	Comment

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        DataGridComboBoxColumn data_type_column = new DataGridComboBoxColumn();
                        DataGridTextColumn initial_value_column = new DataGridTextColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        data_type_column.Header     = "Data type";
                        initial_value_column.Header = "Initial value";
                        comment_column.Header       = "Comment";
        
                        item_number_column.Width    =  50;
                        name_column.Width           = 100;
                        data_type_column.Width      = 100;
                        initial_value_column.Width  = 100;
                        comment_column.Width        = 300;

                        item_number_column.IsReadOnly = true;

        
//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new MSG_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = MSG_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                            name_column.Binding = name_column_binding;


                            data_type_column.ItemsSource = MSG_DATA_TYPES;
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


//****************   MULTIBINDING 

                            //MultiBinding multi_binding = new MultiBinding();
                            //multi_binding.Bindings.Add(data_type_column_binding);
                            //multi_binding.Bindings.Add(address_column_binding);
                            //multi_binding.Bindings.Add(initial_value_column_binding);
                            //multi_binding.Converter = new MultiBinding_Converter();
                            //initial_value_column.Binding = multi_binding;

//****************   BINDING GROUP  for  VALIDATION

                        /*    BindingGroup bindinggroup = new BindingGroup();
                            bindinggroup.NotifyOnValidationError = true;
                            bindinggroup.ValidationRules.Add( new MSG_Group_ValidationRule());
                            bindinggroup.ValidationRules.Add(new ExceptionValidationRule());
                        //  привязываем Binding не ко всей datagrid, а только к row-строке.
                        datagrid.ItemBindingGroup = bindinggroup;
                        //datagrid.BindingGroup = bindinggroup;

                        //    Обработчик вывода сообщений для всех Validation общий.
                        // можно и так и так Validation.AddErrorHandler(datagrid, new EventHandler<ValidationErrorEventArgs>(Address_ValidationErrorEvent));
                        datagrid.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(MSG_ValidationErrorEvent));
                        */

//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
                        datagrid.Columns.Add(data_type_column);
                        datagrid.Columns.Add(initial_value_column);
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


public class MSG_Item_number_column_Converter : IValueConverter
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

public class MSG_Group_ValidationRule : ValidationRule
{

    // Ensure that an item over $100 is available for at least 7 days.
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    
        try
        {

            // Сделано и здесь и по прерыванию Validation.ErrorRemoved т.к. 
            // Он не заходит в Validation если ошибка исправлена по Esc.
            //MSG_Validation_Ok = false;

            ValidationResult valid_res;

            BindingGroup bg = value as BindingGroup;

            // Get the source object.
            Symbol_Data item = bg.Items[0] as Symbol_Data;

            //  Определение номера проверяемой строки.
            //  Изнутри этого класса нет доступа к коллекции SYMBOLS_LIST.
            DataGridRow datagridrow = (DataGridRow)bg.Owner;
            int item_index = datagridrow.GetIndex() + 1;
            MSG_Validation_Error_Line = item_index.ToString();

            object o_name = null;
            object o_data_type = null;
            object o_initial_value = null;
            object o_comment = null;

            // Get the proposed values for Price and OfferExpires.

            if (!bg.TryGetValue(item, "Name", out o_name))                      { return new ValidationResult(false, "Property 'Symbol.Name' not found!");  }
            if (!bg.TryGetValue(item, "Data_Type", out o_data_type))            { return new ValidationResult(false, "Property 'Symbol.Data_Type' not found!"); }
            if (!bg.TryGetValue(item, "Initial_Value", out o_initial_value))    { return new ValidationResult(false, "Property 'Symbol.Initial_Value' not found!"); }
            if (!bg.TryGetValue(item, "Comment", out o_comment))                { return new ValidationResult(false, "Property 'Symbol.Comment' not found!"); }

            string name = (string)o_name;
            string data_type = (string)o_data_type;
            string initial_value = (string)o_initial_value;
            string comment = (string)o_comment;

//********   NAME VALIDATION
//********   COMMENT  VALIDATION
//********   NAME DUPLICATING VALIDATION

            if ((valid_res = NAME_COMMENT_Validation(item_index, item, name, comment)) != null) return valid_res;


//********   INITIAL VALUE VALIDATION

            if ((valid_res = INITIAL_VALUE_Validation(item_index, item)) != null) return valid_res;

//********  END

            // и здесь и в прерывании по Validation 
            //MSG_Validation_Ok = true;
            //MSG_Validation_Error_Line = null;

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

    private void MSG_ValidationErrorEvent(object sender, ValidationErrorEventArgs e)
    {
        if (e.Action == ValidationErrorEventAction.Added)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());

            //---
            // Сделано по прерыванию Validation.ErrorRemoved т.к. 
            // он не заходит в Validation если ошибка исправлена по Esc, а сюда заходит.
            //MSG_Validation_Ok = false;
        }
        else if (e.Action == ValidationErrorEventAction.Removed)
        {
            //MessageBox.Show("Error removed!");
            //MSG_Validation_Ok = true;
        }


        e.Handled = true;
    }




//**********************************************************

//***************   HANDLERS  ******************************

/*void MSG_button_AddRow_Click(object sender, RoutedEventArgs e)
{
    try
    {   //  Вставка строки в конце списка.
        MSG_SYMBOLS_LIST.Add(new Symbol_Data(MSG_SYMBOLS_LIST, null));
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

void MSG_button_InsertRow_Click(object sender, RoutedEventArgs e)
{
    try
    {

        //  Вставка строки в список.
        Symbol_Data selecteditem = (Symbol_Data)MSG_SYMBOLS_LIST_PANEL.SelectedItem;

        if (selecteditem != null)
        {
            int index = MSG_SYMBOLS_LIST.IndexOf(selecteditem);
            if (index > -1)
            {
                //  Вставляем пустые строки или скопированные.
                if (SYMBOLS_LIST_BUFFER == null)
                {
                    MSG_SYMBOLS_LIST.Insert(index, new Symbol_Data(MSG_SYMBOLS_LIST, null));
                }
                else
                {
                    foreach (Symbol_Data item in SYMBOLS_LIST_BUFFER)
                    {
                        MSG_SYMBOLS_LIST.Insert(index++,
                        new Symbol_Data(MSG_SYMBOLS_LIST, item.Name, item.Tag_Name, item.Data_Type, item.Memory_Type, item.Address, item.Bit_Base, item.Bit_Number, item.Initial_Value, item.Comment));
                    }

                    SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
                }

                //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
                RESHOW_MSG_SYMBOLS_LIST_window();
            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void MSG_button_CopyRow_Click(object sender, RoutedEventArgs e)
{
    try
    {       
        SYMBOLS_LIST_BUFFER = new List<Symbol_Data>(); 

        foreach (SYMBOLS.Symbol_Data item in MSG_SYMBOLS_LIST_PANEL.SelectedItems)
        {
            SYMBOLS_LIST_BUFFER.Add(item);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


void MSG_button_DeleteRow_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //  Удаление выбранных строк списка.
        
            for (int tst = 0; tst == 0; )
            {   //   перед удаление строки удаляем все элементы из грид привязанные к ней,
                // т.к. иначе грид запихнет их на ставшую последней строку.
                tst = 1;
                foreach (SYMBOLS.Symbol_Data item in MSG_SYMBOLS_LIST_PANEL.SelectedItems)
                {
                    MSG_SYMBOLS_LIST.Remove(item);
                        //  приходится прерывать поиск и начинать его сначала, т.к. 
                        //  коллекция после удаления изменилась и foreach не может 
                        //  продолжать поиск далее.
                    tst = 0;
                    break;
                }
            }

        //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
        RESHOW_MSG_SYMBOLS_LIST_window();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void MSG_button_BindToElement_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (MSG_Validation_Ok == true)
        {
            SYMBOLS_LIST_BUFFER = new List<Symbol_Data>();
            SYMBOLS_LIST_BUFFER.Add((Symbol_Data)MSG_SYMBOLS_LIST_PANEL.SelectedItem);
            //---
            MSG_SYMBOLS_LIST_window.Close();
        }
        else
        {
            MessageBox.Show("Can't bind. Error exists. Line: " + MSG_Validation_Error_Line);
        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
*/

//void MSG_button_Cancel_Click(object sender, RoutedEventArgs e)
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

        //if (MSG_Validation_Ok == true)
        //{
//            MSG_SYMBOLS_LIST_window.Close();
        //}
        //else
        //{
            //MessageBox.Show("Can't exit. Error exists. Line: " + MSG_Validation_Error_Line);
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
