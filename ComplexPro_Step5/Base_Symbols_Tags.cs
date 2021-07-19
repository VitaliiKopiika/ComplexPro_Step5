

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

        //********    BASE_SYMBOLS_TAGS

        public partial class SYMBOLS
        {
            //************    PANELs

            static public Window BASE_TAGS_SYMBOLS_LIST_window;
            
            //DataGrid BASE_SYMBOLS_TAGS_LIST_PANEL { get; set; }

            static public ObservableCollection<Symbol_Data> BASE_TAGS_SYMBOLS_LIST { get; set; }

//****************************************************************

//************   FOR XPS PRINTING

public static List<List<string>> BASE_SYMBOLS_TAGS_LIST_ToXpsList(ObservableCollection<Symbol_Data> symbols_list,
               out List<List<string>> headers, out List<string> alignment, out List<double> collumn_widths)
{
        headers = new List<List<string>>() 
                        { 
                            new List<string> () {"#", "Symbol", "Data type", "Comment" }
                        };
        alignment = new List<string>() 
                        {                             
                            "Center", "Left", "Center", "Left" 
                        };
        collumn_widths = new List<double>() { 30, 100, 70, 350 };
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


//***************   SHOW_BASE_SYMBOLS_TAGS_LIST

public Border SHOW_BASE_TAGS_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {
        DataGrid datagrid = symbols_list_window.Tag as DataGrid;

        Grid grid = datagrid.Parent as Grid;

                        //  IsReadOnly.
                        datagrid.IsReadOnly = true; 

//****************   COLUMNS

// 123 InputBitPanel	M     100.3	BOOL	Comment

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        DataGridTextColumn data_type_column = new DataGridTextColumn();
                        DataGridTextColumn memory_type_column = new DataGridTextColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        data_type_column.Header     = "Data type";
                        memory_type_column.Header   = "Memory";
                        comment_column.Header       = "Comment";
        
                        item_number_column.Width    =  50;
                        name_column.Width           = 100;
                        data_type_column.Width      = 100;
                        memory_type_column.Width    = 100;
                        comment_column.Width        = 300;

                        item_number_column.IsReadOnly = true;

        
//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new Base_Tags_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = BASE_TAGS_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                            name_column.Binding = name_column_binding;


                            Binding data_type_column_binding = new Binding();
                            data_type_column_binding.Path = new PropertyPath("Data_Type");
                            data_type_column_binding.Mode = BindingMode.TwoWay;
                            data_type_column.Binding = data_type_column_binding;


                            Binding memory_type_column_binding = new Binding();
                            memory_type_column_binding.Path = new PropertyPath("Memory_Type");
                            memory_type_column_binding.Mode = BindingMode.TwoWay;
                            memory_type_column.Binding = memory_type_column_binding;

                            Binding comment_column_binding = new Binding();
                            comment_column_binding.Path = new PropertyPath("Comment");
                            comment_column_binding.Mode = BindingMode.TwoWay;
                            comment_column.Binding = comment_column_binding;


//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
                        datagrid.Columns.Add(data_type_column);
                        datagrid.Columns.Add(memory_type_column);
                        datagrid.Columns.Add(comment_column);


//****************   BUTTONS  **************************

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Top;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                        Button button_LoadTags = Get_Button("Load New Tags", BaseTags_button_LoadTags_Click, symbols_list_window);
                        Button button_Cancel = Get_Button("Ok", button_Cancel_Click, symbols_list_window);
                             
                        // иначе по cancel окно закрывается безусловно  button_Cancel.IsCancel = true;
                        FocusManager.SetFocusedElement(symbols_list_window, button_Cancel);

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


//****************


//***************   CONVERTERS

//**************    MULTI BINDING CONVERTER 


public class Base_Tags_Item_number_column_Converter : IValueConverter
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




//**********************************************************

//***************   HANDLERS  ******************************


void BaseTags_button_LoadTags_Click(object sender, RoutedEventArgs e)
{
    try
    {
        SaveProject.Load_PCC_DATA_Window("[BaseVariable]");

        SYMBOLS_LIST.REFRESH_SYMBOLS_LIST_window((Window)((Button)sender).Tag);
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}



        }  // ******  END of Class Mymory_Symbols

    }  // ******  END of Class Step5

}
