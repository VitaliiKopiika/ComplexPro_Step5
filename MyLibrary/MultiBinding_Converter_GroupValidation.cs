
// сделать удаления добавления вставки по кнопкам клавиатуры delete insert 
// сделать  для ячеек автоматический ReadOnly

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

        //********    MEMORY_SYMBOLS

        public class Symbols_List
        {
            static public List<string> Memory_Types = new List<string>() { "MW", "MB", "MD", "M", "CONST" };
            //static public List<string> Memory_Types = new List<string>() { "M", "MB", "MW", "MD" };
            
            public class Symbol_Data
            {
                static int Count=0;
                //---
                
                public string Name { get{ return name;} 
                                     set{ name = value ;
                                          Set_Binding_Name(); // = comment + "\n" + name + "\n" + address;
                                   }    }

                public string Type { get; set; }
                
                public int Address { get{ return address;} 
                                     set{ address = value ;
                                          Set_Binding_Name(); // = comment + "\n" + name + "\n" + address;
                                   }    }

                public int Bit_Number { get; set; }

                public string Comment { get{ return comment;} 
                                        set{ comment = value ;
                                             Set_Binding_Name(); // = comment + "\n" + name + "\n" + address;
                                      }    }

                //  ввел промежуточные переменные, чтобы они сообщали суммарному имени об их изменении.
                public string binding_name;
                public string name;
                public int    address;
                public string comment;
                public string Binding_Name { get { return binding_name; }
                                             set { binding_name = value;}
                                           }

                public Symbol_Data( object zaglushka) //  конструктор без параметров не пропускает конвертер привязки.
                {
                    Count++;
                    //---
                    Name = Memory_Types[0].ToString();// +Count.ToString();
                    Type = Memory_Types[0];
                    Address = 0;
                    Bit_Number = 0;
                    Comment = "Comment:...";
                }
                
                public Symbol_Data(string name, string type, int address, int bit_number, string comment)
                {
                    Count++;
                    //---
                    Name = name;
                    Type = type;
                    Address = address;
                    Bit_Number = bit_number;
                    Comment = comment;
                }

                public void Set_Binding_Name()
                {
                    Binding_Name = Comment + "\n" + Name + "\n" + Address;
                }

            }




            //************    PANELs

            Window INPUT_SYMBOLS_LIST_window ;

            DataGrid SYMBOLS_LIST_PANEL { get; set; }

            public ObservableCollection<Symbol_Data> SYMBOLS_LIST { get; set; }

            static public List<Symbol_Data> SYMBOLS_LIST_BUFFER { get; set; }

            //    CONSTRUCTOR #1

            //  Создание пустой SYMBOLS_LIST с одной NEW_LINE.

            public Symbols_List()//StackPanel symbols_list_panel)
            {
                //SYMBOLS_LIST_PANEL = symbols_list_panel;
                //SYMBOLS_LIST = new List<Symbols_data>();// { new Network_Of_Elements() };
                SYMBOLS_LIST = new ObservableCollection<Symbol_Data>()
                { 
                    new Symbol_Data("Id0", "MW", 200, 0, "Comments1............."),
                    new Symbol_Data("Id1", "MB", 202, 0, "Comments2............."),
                    new Symbol_Data("Id2", "MD", 204, 0, "Comments3............."),
                    new Symbol_Data("Id3", "M" , 206, 0, "Comments4.............")
                };

                SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
            }

            //    CONSTRUCTOR #2

            //  Создание SYMBOLS_LIST с загруженной из файла List<Network_Of_Elements> symbols.

            public Symbols_List(/*StackPanel symbols_list_panel,*/ ObservableCollection<Symbol_Data> symbols)
            {
                //SYMBOLS_LIST_PANEL = symbols_list_panel;
                SYMBOLS_LIST = symbols;
                SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
            }


//***************     

public void SHOW_SYMBOLS_LIST_window(string code)
{

    try
    {

        INPUT_SYMBOLS_LIST_window = new Window();
        INPUT_SYMBOLS_LIST_window.Title = "Input data";
        INPUT_SYMBOLS_LIST_window.SizeToContent = SizeToContent.WidthAndHeight;
        INPUT_SYMBOLS_LIST_window.Width = 500;
        INPUT_SYMBOLS_LIST_window.Height = 300;
        INPUT_SYMBOLS_LIST_window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

//INPUT_SYMBOLS_LIST_window.Closing += INPUT_DATA_window_Closing;

//            Viewbox viewbox = new Viewbox();
//            viewbox.HorizontalAlignment = HorizontalAlignment.Center;
//            viewbox.VerticalAlignment = VerticalAlignment.Center;
            //viewbox.Child = xborder;

        INPUT_SYMBOLS_LIST_window.Content = SHOW_SYMBOLS_LIST();
        //INPUT_SYMBOLS_LIST_window.CaptureMouse();

        if      (code == "Show" )      INPUT_SYMBOLS_LIST_window.Show();
        else if (code == "ShowDialog") INPUT_SYMBOLS_LIST_window.ShowDialog();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return ;

}

//***************     

public void RESHOW_SYMBOLS_LIST_window()
{

    try
    {
        INPUT_SYMBOLS_LIST_window.Content = SHOW_SYMBOLS_LIST();
        //INPUT_SYMBOLS_LIST_window.Show();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return;

}

//***************     

public Border SHOW_SYMBOLS_LIST()
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
                    

                        DataGrid datagrid = new DataGrid();
                        SYMBOLS_LIST_PANEL = datagrid;
                        datagrid.SetValue(Grid.ColumnProperty, 0);
                        datagrid.AutoGenerateColumns = false ;
                        datagrid.CanUserAddRows = true;
                        datagrid.CanUserDeleteRows = false;
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
                        datagrid.Tag = SYMBOLS_LIST; //  для обработчика добавления строк.

                            Binding items_binding = new Binding();
                            items_binding.Source = this;
                            items_binding.Path = new PropertyPath("SYMBOLS_LIST");
                            //items_binding.Converter = new List_converter();
                            items_binding.Mode = BindingMode.TwoWay;
                        datagrid.SetBinding(DataGrid.ItemsSourceProperty, items_binding);


//****************   COLUMNS

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        DataGridTextColumn address_column = new DataGridTextColumn();
                        DataGridTextColumn bit_number_column = new DataGridTextColumn();
                        DataGridComboBoxColumn type_column = new DataGridComboBoxColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

                        //name_column.IsReadOnly = false;

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        address_column.Header       = "Address";
                        bit_number_column.Header    = "Bit";
                        type_column.Header          = "Data type";
                        comment_column.Header       = "Comment";
        
                        item_number_column.Width    =  50;
                        name_column.Width           = 200;
                        address_column.Width        = 100;
                        bit_number_column.Width     =  50;
                        type_column.Width           = 100;
                        comment_column.Width        = 500;

                        item_number_column.IsReadOnly = true;
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
                            item_number_column_binding.ConverterParameter = SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                                name_column_binding.ValidationRules.Add(new Name_ValidationRule(16)); 
                                // при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                //name_column_binding.NotifyOnValidationError = true;
                            name_column.Binding = name_column_binding;
        

                            Binding address_column_binding = new Binding();
                            address_column_binding.Path = new PropertyPath("Address");
                            address_column_binding.Mode = BindingMode.TwoWay;
                                // его использовали вместо Validation address_column_binding.Converter = new Address_column_Converter();
                                address_column_binding.ValidationRules.Add(new Adress_ValidationRule(0, 65534));
                                // при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                //address_column_binding.NotifyOnValidationError = true;
                            address_column.Binding = address_column_binding;

                               
                            Binding bit_number_column_binding = new Binding();
                            //bit_number_column_binding.Source = ;
                            bit_number_column_binding.Path = new PropertyPath("Bit_Number");
                            bit_number_column_binding.Mode = BindingMode.TwoWay;
                                bit_number_column_binding.ValidationRules.Add(new BitNumber_ValidationRule());
                                // при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                //bit_number_column_binding.NotifyOnValidationError = true;
                            bit_number_column.Binding = bit_number_column_binding;


                            type_column.ItemsSource = Memory_Types;
                            Binding type_column_binding = new Binding();
                            type_column_binding.Path = new PropertyPath("Type");
                            type_column_binding.Mode = BindingMode.TwoWay;
                                //  Validation не требуется - это ComboBox.
                            type_column.SelectedItemBinding = type_column_binding;


                            Binding comment_column_binding = new Binding();
                            comment_column_binding.Path = new PropertyPath("Comment");
                            comment_column_binding.Mode = BindingMode.TwoWay;
                                comment_column_binding.ValidationRules.Add(new Comment_ValidationRule(96));
                                // при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                                //comment_column_binding.NotifyOnValidationError = true;
                            comment_column.Binding = comment_column_binding;
        
                        //    Обработчик вывода сообщений для всех Validation общий.
                        // можно и так и так Validation.AddErrorHandler(datagrid, new EventHandler<ValidationErrorEventArgs>(Address_ValidationErrorEvent));
                        datagrid.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(ValidationErrorEvent));


//****************   BINDING GROUP

                            BindingGroup bindinggroup = new BindingGroup();
                            bindinggroup.NotifyOnValidationError = true;
                            bindinggroup.ValidationRules.Add( new Group_ValidationRule());
                        //  привязываем Binding не ко всей datagrid, а только к row-строке.
                        datagrid.ItemBindingGroup = bindinggroup;
        

//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
                        datagrid.Columns.Add(type_column);
                        datagrid.Columns.Add(address_column);
                        datagrid.Columns.Add(bit_number_column);
                        datagrid.Columns.Add(comment_column);


//****************   BUTTONS  **************************

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Top;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                            Button button_AddRow = Get_Button("Add Row", button_AddRow_Click);
                            Button button_InsertRow = Get_Button("Insert Row", button_InsertRow_Click);
                            Button button_DeleteRow = Get_Button("Delete Row", button_DeleteRow_Click);
                            //if (USER_CALCULATION == null) button2.IsEnabled = false;
                            Button button_CopyRow = Get_Button("Copy Row", button_CopyRow_Click);
                            Button button_BindToElement = Get_Button("Bind to Element", button_BindToElement_Click);
                            Button button_Cancel = Get_Button("Cancel", button_Cancel_Click);
                             
                        button_Cancel.IsCancel = true;
                        FocusManager.SetFocusedElement(INPUT_SYMBOLS_LIST_window, button_Cancel);    
        
                        stackpanel.Children.Add(button_AddRow);
                        stackpanel.Children.Add(button_CopyRow);
                        stackpanel.Children.Add(button_InsertRow);
                        stackpanel.Children.Add(button_DeleteRow);
                        stackpanel.Children.Add(button_BindToElement);
                        stackpanel.Children.Add(button_Cancel);

                    grid.Children.Add(datagrid);
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

//****************

private Button Get_Button(string Name,  RoutedEventHandler handler)
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

        return button;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}

//***************   CONVERTERS

public class Item_number_column_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            return ((ObservableCollection<Symbol_Data>)parameter).IndexOf((Symbol_Data)value);
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
//***************  VALIDATION_RULES  ***********************

//***************  GROUP VALIDATION

public class Group_ValidationRule : ValidationRule
{

    // Ensure that an item over $100 is available for at least 7 days.
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    
        try
        {  
 
            BindingGroup bg = value as BindingGroup;

            // Get the source object.
            Symbol_Data item = bg.Items[0] as Symbol_Data;

            //  Определение номера проверяемой строки.
            //  Изнутри этого класса нет доступа к коллекции SYMBOLS_LIST.
            DataGridRow datagridrow = (DataGridRow)bg.Owner;
            int item_index = datagridrow.GetIndex();

            object o_type;
            object o_bit_number;

            // Get the proposed values for Price and OfferExpires.
            bool Result1 = bg.TryGetValue(item, "Type", out o_type);
            bool Result2 = bg.TryGetValue(item, "Bit_Number", out o_bit_number);

            if (!Result1 || !Result2)
            {
                return new ValidationResult(false, "Properties not found");
            }

            string type = (string)o_type;
            int bit_number = int.Parse((string)o_bit_number);

            // Check that an item over $100 is available for at least 7 days.
            if (type == "MB" && bit_number >= 8)
            {
                return new ValidationResult(false, "Bit_Number can't be more then 7 for 'MB'-type. Line: " + item_index + ".");
            }
            if (type == "MW" && bit_number >= 16)
            {
                return new ValidationResult(false, "Bit_Number can't be more then 15 for 'MW'-type Line: " + item_index + ".");
            }
            if (type == "MLW" && bit_number >= 32)
            {
                return new ValidationResult(false, "Bit_Number can't be more then 31 for 'MLW'-type Line: " + item_index + ".");
            }

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


//***************  NAME VALIDATION

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

//***************  VALIDATION  HANDLER

    private void ValidationErrorEvent(object sender, ValidationErrorEventArgs e)
    {
        if (e.Action == ValidationErrorEventAction.Added)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());
        }
        e.Handled = true;
    }




//**********************************************************

//***************   HANDLERS  ******************************


void button_AddRow_Click(object sender, RoutedEventArgs e)
{
    try
    {   //  Вставка строки в конце списка.
        SYMBOLS_LIST.Add(new Symbol_Data(null));
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
        //  Вставка строки в список.
        Symbol_Data selecteditem = (Symbol_Data)SYMBOLS_LIST_PANEL.SelectedItem;

        if (selecteditem != null)
        {
            int index = SYMBOLS_LIST.IndexOf(selecteditem);
            if (index > -1)
            {
                //  Вставляем пустые строки или скопированные.
                if (SYMBOLS_LIST_BUFFER == null)
                {
                    SYMBOLS_LIST.Insert(index, new Symbol_Data(null));
                }
                else
                {
                    foreach (Symbol_Data item in SYMBOLS_LIST_BUFFER)
                    {
                        SYMBOLS_LIST.Insert(index++,
                       new Symbol_Data(item.Name, item.Type, item.Address, item.Bit_Number, item.Comment));
                    }

                    SYMBOLS_LIST_BUFFER = null; // null-нужен для кнопки вставка, чтобы знать вставлять пустые строки или скопированные.
                }

                //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
                RESHOW_SYMBOLS_LIST_window();
            }
        }

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
        SYMBOLS_LIST_BUFFER = new List<Symbol_Data>(); 

        foreach (Symbols_List.Symbol_Data item in SYMBOLS_LIST_PANEL.SelectedItems)
        {
            SYMBOLS_LIST_BUFFER.Add(item);
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
        
        //  Удаление выбранных строк списка.
        
            for (int tst = 0; tst == 0; )
            {   //   перед удаление строки удаляем все элементы из грид привязанные к ней,
                // т.к. иначе грид запихнет их на ставшую последней строку.
                tst = 1;
                foreach (Symbols_List.Symbol_Data item in SYMBOLS_LIST_PANEL.SelectedItems)
                {
                    SYMBOLS_LIST.Remove(item);
                        //  приходится прерывать поиск и начинать его сначала, т.к. 
                        //  коллекция после удаления изменилась и foreach не может 
                        //  продолжать поиск далее.
                    tst = 0;
                    break;
                }
            }

        //  обновление окна т.к. после вставки строк не обновляются автоматически их номера.
        RESHOW_SYMBOLS_LIST_window();
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
        SYMBOLS_LIST_BUFFER = new List<Symbol_Data>();
        SYMBOLS_LIST_BUFFER.Add( (Symbol_Data)SYMBOLS_LIST_PANEL.SelectedItem);
        //---
        INPUT_SYMBOLS_LIST_window.Close();
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
        INPUT_SYMBOLS_LIST_window.Close();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

//***************    MultiBinding for VARs


static public void SetVarBinding( TextBlock sender, Symbols_List.Symbol_Data symbol_data )
{
    try
    {

                //  В Tage каждого TextBlock при его размежении на поле будет положена ссылка на его владельца Element.
                //  Ищем на котором из TextBlockov кликнули.
                Element_Data element = (Element_Data)((TextBlock)sender).Tag;

                Binding name_binding = new Binding(); 
                name_binding.Source = symbol_data;
                name_binding.Path = new PropertyPath("Name");

                Binding type_binding = new Binding();
                type_binding.Source = symbol_data;
                type_binding.Path = new PropertyPath("Type");

                Binding address_binding = new Binding();
                address_binding.Source = symbol_data;
                address_binding.Path = new PropertyPath("Address");
                Binding comment_binding = new Binding();

                comment_binding.Source = symbol_data;
                comment_binding.Path = new PropertyPath("Comment");

                MultiBinding multi_binding = new MultiBinding();
                    multi_binding.Bindings.Add(comment_binding);
                    multi_binding.Bindings.Add(name_binding);
                    multi_binding.Bindings.Add(type_binding);
                    multi_binding.Bindings.Add(address_binding);
                multi_binding.Converter = new MultiBinding_Converter();
                ((TextBlock)sender).SetBinding(TextBlock.TextProperty, multi_binding);

                int index = element.IO_TextBlocks_list.IndexOf((TextBlock)sender);
                element.IO_VARs_list[index]    = symbol_data;
                element.IO_Binding_list[index] = multi_binding;

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
}

//**************    MULTI BINDING CONVERTER 

public class MultiBinding_Converter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        try
        {

            StringBuilder str = new StringBuilder();
            // Вынимаем в том порядке в котором ложили в multi_binding.Bindings.Add(...).
            str.Append((string)values[0]); // Comment
            str.Append("\n");

            str.Append((string)values[1]); // Name
            str.Append("\n");

            str.Append((string)values[2]);    // Type
            str.Append(": ");

            str.Append((int)values[3]);    // Address
            //str.Append("");


            return str.ToString();
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
