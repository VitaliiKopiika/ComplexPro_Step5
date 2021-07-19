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

namespace Wpf_Binding
{
    public partial class Graph
    {

public ObservableCollection<ObservableCollection_Item>  ListList_Rotate90_to_ObservableObservable( List<List<double>> in_list)
{
    try
    {
        ObservableCollection<ObservableCollection_Item> out_list = new ObservableCollection<ObservableCollection_Item>() ;

                for ( int i=0 ;  ; i++ )
                {
                    ObservableCollection_Item out_list_items = new ObservableCollection_Item();

                    out_list_items.items.Add( (double)i ); 
                    
                    int end = 1;
                    for (int j = 0; j < in_list.Count; j++)
                    {
                        if (i < in_list[j].Count) { out_list_items.items.Add(in_list[j][i]); end = 0; }
                        else                        out_list_items.items.Add(double.NaN);   
                    }
                    if (end == 0) out_list.Add(out_list_items);
                    else break;
                }

            return out_list;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}

public List<List<double>> ObservableObservable_Back_to_ListList ( ObservableCollection<ObservableCollection_Item> in_list)
{
    try
    {
        List<List<double>> out_list = new List<List<double>>();

        for (int j = 1; j < in_list[0].items.Count; j++) out_list.Add(new List<double>());

        for (int i = 0; i < in_list.Count; i++)
        {
            ObservableCollection_Item out_list_items = new ObservableCollection_Item();

            for (int j = 1; j < in_list[i].items.Count; j++)
            {
                if (in_list[i].items[j] != double.NaN) { out_list[j - 1].Add( in_list[i].items[j]); }
            }
        }

        return out_list;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }

}


public class Button_Enable_Converter : IValueConverter
{
    public object Convert(object value, Type targetType,
              object parametr, System.Globalization.CultureInfo culture)
    {
        try
        {
            if (((string)value).Length == 0) return false;
            else                             return true;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null ;
        }
                
    }

    public object ConvertBack(object value, Type targetType,
        object parametr, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }

}

/*
public class MultiRadiobutton_Converter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try
        {
            int result = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if ((bool)values[i] == true)
                {
                    result = i;
                    break;
                }
            }

            return result;

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null ;
        }
                
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        MessageBox.Show("INPUT_DATA_RadioButton_Multibinding_ConvertBack_Exception");
        throw new NotSupportedException();
    }
}
*/

public class Radiobutton_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        //MessageBox.Show("ConvertForward");
        try
        {
            if ((int)value == (int)parameter) return true;
            else                              return false;
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
            if ((bool)value == true) return parameter;
            else
            {
                MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                return 0 ;
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
public class List_converter : IValueConverter
{
    public ObservableCollection<VIEW_data> VIEW_LIST { get; set; } 

    public object Convert(object value, Type targetType,
              object parametr, System.Globalization.CultureInfo culture)
    {
        try
        {
            List<List<double>> data = new List<List<double>>() ;    //(List<List<double>>) value ;
            data.Add(new List<double>() {1,2,3}) ;
            data.Add(new List<double>() { 1, 2, 3 });
            data.Add(new List<double>() { 1, 2, 3 });

            VIEW_LIST = new ObservableCollection<VIEW_data>();

            if (data != null)
            {
                for (int i = 0 ; ; i++)
                {
                    VIEW_data list1 = new VIEW_data();

                    list1.view_data.Add((double)i);
                    //VIEW_X_Numerator.Add( k ); 
                    //k++;

                    int end = 1;
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (i < data[j].Count) { list1.view_data.Add(data[j][i]); end = 0; }
                        else list1.view_data.Add(0);
                    }
                    if (end == 0) VIEW_LIST.Add(list1);
                    else break;
//добавляется откудато лишняя строка 
                }

                return VIEW_LIST;
            }

            MessageBox.Show("Нулевой VIEW_LIST");
            return null ;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null ;
        }
                
    }

    public object ConvertBack(object value, Type targetType,
        object parametr, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }

}


        
public class INPUT_DATA_cell_converter : IValueConverter
{

    public object Convert(object value, Type targetType,
                      object parametr, System.Globalization.CultureInfo culture)
    {
        try{

            VIEW_data item = value as VIEW_data;

            if (item != null)
            {
                //VIEW_data item = (VIEW_data)value ;
                MessageBox.Show(value.ToString() + "___" + "___" + targetType.ToString());
                return item.view_data[(int)parametr];
            }
            else return null ;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }

                public object ConvertBack(object value, Type targetType,
                    object parametr, System.Globalization.CultureInfo culture)
                {
                    throw new NotSupportedException();
                }

            }

*/

        //---------------  SHOW_INPUT_DATA_window


public void SHOW_INPUT_DATA_window()
        {  

            INPUT_DATA_window = new Window();
            INPUT_DATA_window.Title = "Input data";
            //INPUT_DATA_window.SizeToContent = SizeToContent.WidthAndHeight;
            INPUT_DATA_window.Width = 500;
            INPUT_DATA_window.Height = 300;
            INPUT_DATA_window.Closing += INPUT_DATA_window_Closing;
            INPUT_DATA_window.ShowInTaskbar = false;
            INPUT_DATA_window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            

            Viewbox viewbox = new Viewbox();
            viewbox.HorizontalAlignment = HorizontalAlignment.Center;
            viewbox.VerticalAlignment = VerticalAlignment.Center;

                Button xborder = new Button();
                xborder.Background = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderThickness = new Thickness(0.5);
                xborder.HorizontalAlignment = HorizontalAlignment.Center;
                xborder.VerticalAlignment = VerticalAlignment.Center;
                xborder.Margin = new Thickness(10);

                    Grid grid = new Grid() ;
                    //grid.Margin = new Thickness(10);
                    grid.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.VerticalAlignment = VerticalAlignment.Center;

                    ColumnDefinition column0 = new ColumnDefinition();
                    ColumnDefinition column1 = new ColumnDefinition(); 
                        column0.Width = GridLength.Auto;
                        column1.Width = GridLength.Auto;
                    grid.ColumnDefinitions.Add(column0);
                    grid.ColumnDefinitions.Add(column1);

                        ScrollViewer scroll = new ScrollViewer() ;
                        scroll.SetValue(Grid.ColumnProperty, 0);
                        scroll.MaxWidth  = 500 ;
                        scroll.MaxHeight = 300;
                        scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                        scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                            INPUT_DATA_textbox = new TextBox();
                            INPUT_DATA_textbox.AcceptsReturn = true;
                            INPUT_DATA_textbox.FontSize = MARK_font_size;
                            //textbox.Text="Вставьте текст!" ;
                            INPUT_DATA_textbox.MinWidth = 300;
                            INPUT_DATA_textbox.MinHeight = 200;
                            //DIALOG_InputData_textbox.Margin = new Thickness(3);
                            INPUT_DATA_textbox.TextWrapping = TextWrapping.Wrap;
                            //INPUT_DATA_textbox.Name = "INPUT_DATA_textbox";
                            //INPUT_DATA_textbox.Name = "INPUT_DATA_textbox";

                        FocusManager.SetFocusedElement(INPUT_DATA_window, INPUT_DATA_textbox);    
                            
      //                      INPUT_DATA_textbox.TextChanged += INPUT_DATA_textbox_TextChanged;

/*                                EventTrigger trigger_text_changed = new EventTrigger();
                                //trigger_text_changed.Actions.Add
                                    //SetValue(Button.IsEnabledProperty, true);
                                trigger_text_changed.    
                                trigger_text_changed.RoutedEvent = TextBox.TextChangedEvent ;
                            INPUT_DATA_textbox.Triggers.Add(trigger_text_changed);
    */
                         
                        scroll.Content = INPUT_DATA_textbox;

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Bottom;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                            Button button2 = new Button();
                            button2.Content = "Cancel";
                            button2.IsCancel = true;
                            button2.FontSize = MARK_font_size;
                            button2.Margin = new Thickness(5);
                            button2.Click += INPUT_DATA_Cancel_Click;
                            button2.MinWidth = 75;
                            button2.VerticalAlignment = VerticalAlignment.Bottom;
                            button2.HorizontalAlignment = HorizontalAlignment.Stretch;

                        //FocusManager.SetFocusedElement(INPUT_DATA_window, button2);    

                            Button button3 = new Button();
                            button3.Content = "Ok";
                            button3.IsDefault = true;
                            button3.IsEnabled = false;
                            button3.FontSize = MARK_font_size;
                            button3.Margin = new Thickness(5);
                            button3.Click += INPUT_DATA_View_Click;
                            button3.MinWidth = 75;
                            button3.VerticalAlignment = VerticalAlignment.Bottom;
                            button3.HorizontalAlignment = HorizontalAlignment.Stretch;

                                Binding button_binding = new Binding();
                                button_binding.Source = INPUT_DATA_textbox ;
                                button_binding.Path = new PropertyPath("Text");
                                button_binding.Converter = new Button_Enable_Converter();

                            button3.SetBinding(Button.IsEnabledProperty, button_binding);
                                
                            //    EventTrigger trigger_text_changed = new EventTrigger();
                            //    trigger_text_changed.SetValue(Button.IsEnabledProperty, true);
                            //        //RoutedEvent tst =  new RoutedEvent();
                            //        //tst.  "INPUT_DATA_textbox.TextChanged") ;
                            //    trigger_text_changed.RoutedEvent = TextBox.TextChangedEvent ;
                            //    //trigger_text_changed.SourceName = "INPUT_DATA_textbox";
                            //button3.Triggers.Add(trigger_text_changed);

                            GroupBox groupbox = new GroupBox();
                            groupbox.Header =  "Col.splitter:";
                            groupbox.Margin = new Thickness(5);
                            groupbox.FontSize = MARK_font_size;
                            groupbox.MinWidth = 75;
                            groupbox.VerticalAlignment = VerticalAlignment.Bottom;
                            groupbox.HorizontalAlignment = HorizontalAlignment.Stretch;

                                ComboBox combobox = new ComboBox();
                                //combobox.IsEditable = true;
                                combobox.IsTextSearchEnabled = true;
                                //combobox.Padding = new Thickness(0);
                                combobox.ItemsSource = INPUT_DATA_splitters ;
                                //combobox.BorderBrush = new SolidColorBrush(Colors.Blue);
                                combobox.Resources[SystemColors.HighlightBrushKey] = new SolidColorBrush(Colors.LightSkyBlue);
                                combobox.Resources[SystemColors.HighlightTextBrushKey] = new SolidColorBrush(Colors.White);

//<SolidColorBrush x:Key="{x:Static SystemColors.HighlightBrushKey}" Color="Orange" />
//<SolidColorBrush x:Key="{x:Static SystemColors.ControlBrushKey}" Color="Orange" />
//<SolidColorBrush x:Key="{x:Static SystemColors.HighlightBrushKey}" Color="#00ABDF" />
//<SolidColorBrush x:Key="{x:Static SystemColors.HighlightTextBrushKey}" Color="#FFFFFFFF" />                                                                

        //    Для Стилей и Bindinga нет свойства к которому можно былобы привязаться для изменения фона посветки выбираемого Itema.
        //  Единственный способ изменить системную коллекцию цветов ComboBox если знаеш конечно ключ нужного цвета.
        //  для просмотра всех ключей можно в Dictionary распечатать коллекцию всех ключей и коллекцию всех цветов
                                //  и потом угадывать или искать в инете. Еще проще по F12 SystemColors.
       // см. http://geekswithblogs.net/kobush/archive/2007/03/25/109753.aspx

       // СДЕЛАЕМ СОЧЕИАНИЕ БЕЛОГО HIGHLIGHT И BUTTON КОНТЕЙНЕРОВ.

                                    /*Style style = new Style();
                                    style.Setters.Add(new Setter(ComboBox.BackgroundProperty, new SolidColorBrush(Colors.White)));
        
                                        Trigger trigg_selected_item = new Trigger();
                                        trigg_selected_item.Property = ComboBox.IsMouseOverProperty;
                                        trigg_selected_item.Value = true;
                                        trigg_selected_item.Setters.Add(new Setter(ComboBox.BackgroundProperty, new SolidColorBrush(Colors.Red)));

                                        Trigger trigg_selected_item2 = new Trigger();
                                        trigg_selected_item2.Property = ComboBox.Item;
                                        trigg_selected_item2.Value = true;
                                        trigg_selected_item2.Setters.Add(new Setter(ComboBox.BackgroundProperty, new SolidColorBrush(Colors.Red)));

                                    style.Triggers.Add(trigg_selected_item);
                                    style.Triggers.Add(trigg_selected_item2);

                                combobox.ItemContainerStyle = style;
    */
                                

                               /*     List<Button> combobox_items = new List<Button>();
                                    for (int i = 0; i < INPUT_DATA_splitters.Count; i++)
                                    {
                                        Button border = new Button();
                                        border.MinWidth = 75;
                                        border.Background = new SolidColorBrush(Colors.White);
                                        border.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                                        border.BorderThickness = new Thickness(0.5);
                                        border.HorizontalAlignment = HorizontalAlignment.Stretch;
                                        border.Content = INPUT_DATA_splitters[i];
                                    
                                        combobox_items.Add(border);
                                    }
                                combobox.ItemsSource = combobox_items;
                              */
                                // combobox.SelectedIndex = 0; устанавливается автом. в привязке 
                                //combobox.Margin = new Thickness(5);
                                combobox.FontSize = MARK_font_size;
                                combobox.MinWidth = 75;
                                combobox.VerticalAlignment = VerticalAlignment.Bottom;
                                combobox.HorizontalAlignment = HorizontalAlignment.Stretch;
                                combobox.HorizontalContentAlignment = HorizontalAlignment.Center;
                                //combobox.
                                    Binding combobox_binding = new Binding();
                                    combobox_binding.Source = this ;
                                    combobox_binding.Path = new PropertyPath("INPUT_DATA_splitter");
                                    combobox_binding.Mode = BindingMode.TwoWay;
                                combobox.SetBinding(ComboBox.SelectedIndexProperty, combobox_binding);
                            groupbox.Content = combobox;

                            GroupBox groupbox2 = new GroupBox();
                            groupbox2.Header = "Decimal point:";
                            groupbox2.Margin = new Thickness(5);
                            groupbox2.FontSize = MARK_font_size;
                            groupbox2.MinWidth = 75;
                            groupbox2.VerticalAlignment = VerticalAlignment.Bottom;
                            groupbox2.HorizontalAlignment = HorizontalAlignment.Stretch;
                            
                            
                                StackPanel radiopanel = new StackPanel();
                                radiopanel.Orientation = Orientation.Vertical;
                                radiopanel.VerticalAlignment = VerticalAlignment.Bottom;
        

                                //MultiBinding multibinding = new MultiBinding();
                                //multibinding.Converter = new MultiRadiobutton_Converter();
                                //multibinding.BindingGroupName = "Group1";
                                //multibinding.Mode = BindingMode.TwoWay;

                                                                
                                for (int i = 0; i < INPUT_DATA_decimal_points.Count; i++)
                                {
                                    Button radiobutton_border = new Button();
                                    radiobutton_border.Background = new SolidColorBrush(Colors.White);
                                    radiobutton_border.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                                    radiobutton_border.BorderThickness = new Thickness(0.5);
                                    radiobutton_border.HorizontalContentAlignment = HorizontalAlignment.Left;

                                        RadioButton radiobutton = new RadioButton();
                                        radiobutton.Content = INPUT_DATA_decimal_points[i];
                                        radiobutton.FontSize = MARK_font_size;
                                        //radiobutton.FontWeight = 
                                        radiobutton.MinWidth = 75;
                                        //radiobutton.VerticalAlignment = VerticalAlignment.Bottom;
                                        radiobutton.HorizontalAlignment = HorizontalAlignment.Stretch;
                                        radiobutton.HorizontalContentAlignment = HorizontalAlignment.Center;
                                    
                                            Binding radiobutton_binding = new Binding();
                                            //radiobutton_binding.BindingGroupName = "Group1";
                                            radiobutton_binding.Source = this;
                                            radiobutton_binding.Path = new PropertyPath("INPUT_DATA_decimal_point");
                                            radiobutton_binding.Mode = BindingMode.TwoWay;
                                            radiobutton_binding.Converter = new Radiobutton_Converter();
                                            radiobutton_binding.ConverterParameter = i;
                                            radiobutton.SetBinding(RadioButton.IsCheckedProperty, radiobutton_binding);

                                        //multibinding.Bindings.Add(radiobutton_binding);

                                    radiobutton_border.Content = radiobutton;

                                    radiopanel.Children.Add(radiobutton_border);
                                }
    
                            groupbox2.Content = radiopanel;

                        stackpanel.Children.Add(groupbox2);
                        stackpanel.Children.Add(groupbox);
                        stackpanel.Children.Add(button2);
                        stackpanel.Children.Add(button3);
                        
                    grid.Children.Add(scroll);
                    grid.Children.Add(stackpanel);

                xborder.Content = grid;

              viewbox.Child = xborder;

            INPUT_DATA_window.Content = viewbox;
            //INPUT_DATA_window.CaptureMouse();
            INPUT_DATA_window.ShowDialog();
                    
        return ;

        }

//void INPUT_DATA_textbox_TextChanged(object sender, TextChangedEventArgs e)
//{
  //   e.Handled = true;
//}


public void SHOW_VIEW_INPUT_DATA_window()
{

    try
    {

        //INPUT_DATA_window = new Window();
        //INPUT_DATA_window.Title = "Input data";
        //INPUT_DATA_window.SizeToContent = SizeToContent.WidthAndHeight;
        //INPUT_DATA_window.Width = 500;
        //INPUT_DATA_window.Height = 300;
        //INPUT_DATA_window.Closing += INPUT_DATA_window_Closing;

            Viewbox viewbox = new Viewbox();
            viewbox.HorizontalAlignment = HorizontalAlignment.Center;
            viewbox.VerticalAlignment = VerticalAlignment.Center;

                Button xborder = new Button();
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
                        datagrid.SetValue(Grid.ColumnProperty, 0);
                        datagrid.AutoGenerateColumns = false ;
                        datagrid.CanUserAddRows = false;
                        datagrid.MaxHeight = 300;
                        datagrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                        datagrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
                        datagrid.AlternatingRowBackground = new SolidColorBrush(Colors.AliceBlue);
                        datagrid.AlternationCount = 2;                        //datagrid.Background = new SolidColorBrush(Colors.AliceBlue);
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

                            VIEW_list = ListList_Rotate90_to_ObservableObservable(INPUT_list);
                            Binding items_binding = new Binding();
                            items_binding.Source = this;
                            items_binding.Path = new PropertyPath("VIEW_list");
                            //items_binding.Converter = new List_converter();
                            items_binding.Mode = BindingMode.TwoWay;

                        datagrid.SetBinding(DataGrid.ItemsSourceProperty, items_binding);

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

        //-----------------
                
                        for (int i = 0; i < VIEW_list[0].items.Count; i++)
                        {
                            DataGridTextColumn column = new DataGridTextColumn();

                    //------------          HEADER

                            if (i == 0)
                            {
                                column.Header = "№";
                                column.IsReadOnly = true;
                            }
                            else
                            {
                                column.Header = "Fig." + i;
                                column.IsReadOnly = false;
                            }
                            
                            column.MinWidth = 50;

                                     //       Binding header_binding = new Binding();
                                     //       //header_binding.Source = VIEW_list[i-1];
                                     //       //header_binding.Path = new PropertyPath("Name");
                                     //       //header_binding.Mode = BindingMode.TwoWay;
                                     //       header_binding.Converter = new INPUT_DATA_header_converter();
                                     //       header_binding.ConverterParameter = i;
                                     //       BindingOperations.SetBinding(column, DataGridTextColumn.HeaderProperty, header_binding);

                    //------------          LIST
                    // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                            Binding cell_binding = new Binding();
                            //cell_binding.Source = VIEW_list ;
                            cell_binding.Path = new PropertyPath("items[" + i + "]");
                            cell_binding.Mode = BindingMode.TwoWay;
                            if (i == 0) cell_binding.StringFormat = "00";
                            else cell_binding.StringFormat = d_CalcStringformat(INPUT_list[i-1]);

                            //cell_binding.Converter = new INPUT_DATA_cell_converter();
                            //cell_binding.ConverterParameter = i;

                            column.Binding = cell_binding;

                            datagrid.Columns.Add(column);
                     
                        }


                //---------------------

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.VerticalAlignment = VerticalAlignment.Bottom;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                            Button button1 = new Button();
                            button1.Content = "Show";
                            button1.FontSize = MARK_font_size;
                            button1.Margin = new Thickness(5);
                            button1.Click += INPUT_DATA_Show_Click;
                            button1.MinWidth = 75;
                            button1.VerticalAlignment = VerticalAlignment.Bottom;
                            button1.HorizontalAlignment = HorizontalAlignment.Stretch;

                        FocusManager.SetFocusedElement(INPUT_DATA_window, button1);    

                            Button button2 = new Button();
                            button2.Content = "Calc&Show";
                            button2.FontSize = MARK_font_size;
                            button2.Margin = new Thickness(5);
                            button2.Click += INPUT_DATA_CalcShow_Click;
                            button2.MinWidth = 75;
                            button2.VerticalAlignment = VerticalAlignment.Bottom;
                            button2.HorizontalAlignment = HorizontalAlignment.Stretch;
                            if (USER_CALCULATION == null) button2.IsEnabled = false;

                            Button button3 = new Button();
                            button3.Content = "Cancel";
                            button3.IsCancel = true;
                            button3.FontSize = MARK_font_size;
                            button3.Margin = new Thickness(5);
                            button3.Click += INPUT_DATA_Cancel_Click;
                            button3.MinWidth = 75;
                            button3.VerticalAlignment = VerticalAlignment.Bottom;
                            button3.HorizontalAlignment = HorizontalAlignment.Stretch;

                        stackpanel.Children.Add(button1);
                        stackpanel.Children.Add(button2);
                        stackpanel.Children.Add(button3);

                    grid.Children.Add(datagrid);
                    grid.Children.Add(stackpanel);

                xborder.Content = grid;

            viewbox.Child = xborder;

        INPUT_DATA_window.Content = viewbox;
        //INPUT_DATA_window.CaptureMouse();
        //INPUT_DATA_window.ShowDialog();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return ;

}

// -----------------   END of CLASS
    }
}

/*
 * 
 *         public void SHOW_VIEW_INPUT_DATA_window()
        {  
            INPUT_DATA_window = new Window();
            INPUT_DATA_window.Title = "Input data";
            //INPUT_DATA_window.SizeToContent = SizeToContent.WidthAndHeight;
            INPUT_DATA_window.Width = 500;
            INPUT_DATA_window.Height = 300;
            INPUT_DATA_window.Closing += INPUT_DATA_window_Closing;

            Viewbox viewbox = new Viewbox();
            viewbox.HorizontalAlignment = HorizontalAlignment.Center;
            viewbox.VerticalAlignment = VerticalAlignment.Center;

                Button xborder = new Button();
                xborder.Background = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderThickness = new Thickness(0.5);
                xborder.HorizontalAlignment = HorizontalAlignment.Center;
                xborder.VerticalAlignment = VerticalAlignment.Center;
                xborder.Margin = new Thickness(10);

                    Grid grid = new Grid() ;
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
                        datagrid.SetValue(Grid.ColumnProperty, 0);
                            Binding  items_binding = new Binding();
                            items_binding.Source = this ;
                            items_binding.Path = new PropertyPath("VIEW_X_Numerator") ;
                            items_binding.Mode = BindingMode.TwoWay;
                        datagrid.SetBinding(DataGrid.ItemsSourceProperty, items_binding);

                                                   //  +1 - для колонки с номерами строк.
                    for ( int i = 0 ; i < VIEW_list.Count+1 ; i++ )
                    {
                        DataGridTextColumn column = new DataGridTextColumn();

//------------          HEADER

                        if (i == 0) column.Header = "№";
                        else
                        {
                            Binding header_binding = new Binding();
                            header_binding.Source = VIEW_list[i-1];
                            header_binding.Path = new PropertyPath("Name");
                            header_binding.Mode = BindingMode.TwoWay;
                            BindingOperations.SetBinding(column, DataGridTextColumn.HeaderProperty, header_binding);
                        }

//                        column.IsReadOnly = false;

//------------          LIST
                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      

                        Binding cell_binding = new Binding();
                        //cell_binding.Mode = BindingMode.TwoWay;
                       
                        if (i == 0)
                        {
                            cell_binding.StringFormat = i_CalcStringformat(X_Numerator);
                        }
                        else
                        {
                            cell_binding.Converter = new INPUT_DATA_cell_converter();
                            cell_binding.StringFormat = d_CalcStringformat(VIEW_list[i-1].DataList);
                            cell_binding.ConverterParameter = VIEW_list[i-1].DataList;
                        }
                        
                        column.Binding = cell_binding;

                        datagrid.Columns.Add(column);
                    }

            
                //---------------------

                        StackPanel stackpanel = new StackPanel();
                        stackpanel.Orientation = Orientation.Vertical;
                        stackpanel.SetValue(Grid.ColumnProperty, 1);

                            Button button1 = new Button();
                            button1.Content="Show" ;
                            button1.FontSize = MARK_font_size;
                            button1.Margin = new Thickness(5);
                            button1.Click += INPUT_DATA_Show_Click;
                           // button1.MaxWidth = 75;
                            button1.VerticalAlignment = VerticalAlignment.Stretch;
                            button1.HorizontalAlignment= HorizontalAlignment.Stretch;

                            Button button2 = new Button();
                            button2.Content = "Calc&Show";
                            button2.FontSize = MARK_font_size;
                            button2.Margin = new Thickness(5);
                            button2.Click += INPUT_DATA_CalcShow_Click;
                           // button2.MaxWidth = 75;
                            button2.VerticalAlignment = VerticalAlignment.Stretch;
                            button2.HorizontalAlignment = HorizontalAlignment.Stretch;

                            Button button3 = new Button();
                            button3.Content = "View";
                            button3.FontSize = MARK_font_size;
                            button3.Margin = new Thickness(5);
                            button3.Click += INPUT_DATA_View_Click;
                            // button2.MaxWidth = 75;
                            button3.VerticalAlignment = VerticalAlignment.Stretch;
                            button3.HorizontalAlignment = HorizontalAlignment.Stretch;
    

                        stackpanel.Children.Add(button1);
                        stackpanel.Children.Add(button2);
                        stackpanel.Children.Add(button3);
                        
                    grid.Children.Add(datagrid);
                    grid.Children.Add(stackpanel);

                xborder.Content = grid;

              viewbox.Child = xborder;

            INPUT_DATA_window.Content = viewbox;
            //INPUT_DATA_window.CaptureMouse();
            INPUT_DATA_window.ShowDialog();
                    
        return ;

        }

*/