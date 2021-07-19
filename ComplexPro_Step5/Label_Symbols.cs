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

        //********    LABEL_SYMBOLS

        public partial class SYMBOLS
        {
            //************    PANELs

            static public Window LABEL_SYMBOLS_LIST_window;

            //static public DataGrid LABEL_SYMBOLS_LIST_PANEL { get; set; }

            static public bool Lab_Validation_Ok = false;
            static public string Lab_Validation_Error_Line = null;


//****************************************************************



//***************   SHOW_LABEL_SYMBOLS_LIST

public Border SHOW_LABEL_SYMBOLS_LIST(Window symbols_list_window)
{

    try
    {
        Diagram_Of_Networks diagram = ACTIV_DIAGRAM;
        //---

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
                        //LABEL_SYMBOLS_LIST_PANEL = datagrid;
                        symbols_list_window.Tag = datagrid; //  для обработчика Validation при открытии окна
                        datagrid.SetValue(Grid.ColumnProperty, 0);
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
                        datagrid.Tag = diagram.LABEL_SYMBOLS_LIST; //  для обработчика добавления строк.

                        //  чтобы автоматически не добавлялась пустая строка снизу "PlaceHolder", которая еще мешает и Validation.
                        datagrid.CanUserAddRows = false;  

                            Binding items_binding = new Binding();
                            items_binding.Source = diagram;
                            items_binding.Path = new PropertyPath("LABEL_SYMBOLS_LIST");
                            items_binding.Mode = BindingMode.TwoWay;
                        datagrid.SetBinding(DataGrid.ItemsSourceProperty, items_binding);

                        //  обработчик для вызова ручного Validation_By_Event 
                        datagrid.CurrentCellChanged += DataGrid_CurrentCellChanged;

//****************   COLUMNS

                        DataGridTextColumn item_number_column = new DataGridTextColumn();
                        DataGridTextColumn name_column = new DataGridTextColumn();
                        DataGridTextColumn comment_column = new DataGridTextColumn();

//****************   HEADERS

                        item_number_column.Header   = "#";
                        name_column.Header          = "Symbol";
                        comment_column.Header       = "Comment";
        
                        item_number_column.Width    =  50;
                        name_column.Width           = 100;
                        comment_column.Width        = 300;

                        item_number_column.IsReadOnly = true;
        
//****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new Lab_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = diagram.LABEL_SYMBOLS_LIST;
                            item_number_column.Binding = item_number_column_binding;


                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column_binding.Mode = BindingMode.TwoWay;
                            name_column.Binding = name_column_binding;



                            Binding comment_column_binding = new Binding();
                            comment_column_binding.Path = new PropertyPath("Comment");
                            comment_column_binding.Mode = BindingMode.TwoWay;
                            comment_column.Binding = comment_column_binding;



//****************   COLUMNS ORDER
                        
                        datagrid.Columns.Add(item_number_column);
                        datagrid.Columns.Add(name_column);
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


//***************   CONVERTERS

//**************    MULTI BINDING CONVERTER 


public class Lab_Item_number_column_Converter : IValueConverter
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





        }  // ******  END of Class Mymory_Symbols

    }  // ******  END of Class Step5

}
