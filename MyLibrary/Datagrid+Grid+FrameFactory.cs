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
        class DiagramsField
        {

//************    PANELs

            StackPanel DIAGRAM_FIELD_PANEL { get; set; }

//************    DATA

            public ObservableCollection<double> NetworkCollection { get; set; }

//************    XXXXX


//************    CONSTRUCTOR

            public DiagramsField(StackPanel panel)
            {
                DIAGRAM_FIELD_PANEL = panel;

                NetworkCollection = new ObservableCollection<double>() { 1,2,3 };

                SHOW_DIAGRAM_FIELD() ;
            }



        public void SHOW_DIAGRAM_FIELD()
        {
            try
            {
                Button xborder = new Button();
                xborder.Background = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                xborder.BorderThickness = new Thickness(0.5);
                xborder.HorizontalAlignment = HorizontalAlignment.Stretch;
                xborder.VerticalAlignment   = VerticalAlignment.Stretch;
                xborder.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                xborder.VerticalContentAlignment = VerticalAlignment.Stretch;
                xborder.Margin = new Thickness(10);

                    DataGrid datagrid = new DataGrid();
                    datagrid.AutoGenerateColumns = false;
                    datagrid.CanUserAddRows = false;
                    //datagrid.MinHeight = 300;
                    //datagrid.MinWidth = 500;
                    datagrid.HeadersVisibility = DataGridHeadersVisibility.None;
                    datagrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
                    datagrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    datagrid.VerticalAlignment = VerticalAlignment.Stretch;
                    //datagrid.AlternatingRowBackground = new SolidColorBrush(Colors.AliceBlue);
                    //datagrid.AlternationCount = 2;                        //datagrid.Background = new SolidColorBrush(Colors.AliceBlue);
                    datagrid.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                    datagrid.BorderThickness = new Thickness(0.5);
                        Style style = new Style();
                        //style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.AliceBlue)));
                        style.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.AliceBlue)));
                        style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0.5)));
                    datagrid.CellStyle = style;

                        Binding items_binding = new Binding();
                        items_binding.Source = this;
                        items_binding.Path = new PropertyPath("NetworkCollection");
                        //items_binding.Converter = new List_converter();
                        items_binding.Mode = BindingMode.TwoWay;
                    datagrid.SetBinding(DataGrid.ItemsSourceProperty, items_binding);

                    //-----------------

                    DataGridTemplateColumn datagrid_column = new DataGridTemplateColumn();

                    DataTemplate cell_template = new DataTemplate();

                    FrameworkElementFactory item_panel = new FrameworkElementFactory(typeof(Grid));

                    item_panel.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                    item_panel.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Stretch);

                        //Binding binding1 = new Binding();
                        //binding1.Converter = new Items_background_converter();
                        //row_panel.SetBinding(ListBox.BackgroundProperty, binding1);

                            FrameworkElementFactory column = new FrameworkElementFactory(typeof(ColumnDefinition));
                            column.SetValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                        item_panel.AppendChild(column);
                            column = new FrameworkElementFactory(typeof(ColumnDefinition));
                            column.SetValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                        item_panel.AppendChild(column);

                            FrameworkElementFactory row = new FrameworkElementFactory(typeof(RowDefinition));
                            row.SetValue(RowDefinition.HeightProperty, GridLength.Auto);
                        item_panel.AppendChild(row);
                            row = new FrameworkElementFactory(typeof(RowDefinition));
                            row.SetValue(RowDefinition.HeightProperty, GridLength.Auto);
                        item_panel.AppendChild(row);

                        FrameworkElementFactory textblock = new FrameworkElementFactory(typeof(TextBlock));
                        textblock.SetValue(Grid.RowProperty, 0); 
                        textblock.SetValue(Grid.ColumnProperty, 0);
                        textblock.SetValue(Grid.MinWidthProperty, 100.0);
                        textblock.SetValue(TextBlock.TextProperty, "Network 1");
                        textblock.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Left);
                        //textblock.SetValue(TextBlock.FontSizeProperty, MARK_font_size);
                        item_panel.AppendChild(textblock);

                        FrameworkElementFactory textbox = new FrameworkElementFactory(typeof(TextBox)); 
                        textbox.SetValue(Grid.RowProperty, 0);
                        textbox.SetValue(Grid.ColumnProperty, 1);
                        textbox.SetValue(Grid.MinWidthProperty, 300.0);
                        textbox.SetValue(TextBox.TextProperty, "Comments:");
                        textbox.SetValue(TextBox.TextAlignmentProperty, TextAlignment.Left);
                        //textbox.SetValue(TextBlock.FontSizeProperty, MARK_font_size);
                        item_panel.AppendChild(textbox);


                    cell_template.VisualTree = item_panel;
                    
                    datagrid_column.CellTemplate = cell_template;
                    datagrid.Columns.Add(datagrid_column);

                    //FocusManager.SetFocusedElement(INPUT_DATA_window, button1);    

                    xborder.Content = datagrid;

                    DIAGRAM_FIELD_PANEL.Children.Add(xborder);

                }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

    return ;

}






        }  // ******  END of Class DiagramsField
        
    }  // ******  END of Class Step5
}

/*


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

*/