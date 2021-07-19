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
using System.Globalization;

namespace ComplexPro_Step5
{
    public partial class Step5
    {
 
        // Class incapsulates all neccessary to work with A7-Settings of MCU
        //  -   load from MCU
        //  -   create neccessary DataGrids for setting groups 
        //  -   autosaving of every changed setting in DataGrids into MCU
        //  -   create TabControls with DataGrids

        public class Service_A7
        {
            //static bool SourceUpdateEnabled = false;

            public Service_A7(XmlCommander comm, string path, System.Windows.Threading.Dispatcher dispatcher=null)
            {
                dataSource = new DataSource(comm, path, dispatcher);
            }

            public DataSource dataSource;

 
            //*********    A7-DataSource    *********

            public class DataSource
            {
                public XmlCommander xmlCommander;
                public string netPath { get; set; }   // путь по которому Item была извлечена из контроллера, для обратной записи значения при редактировании
                public System.Windows.Threading.Dispatcher dispatcher;
                //---

                public DataSource(XmlCommander comm = null, string path = null, System.Windows.Threading.Dispatcher dispatcher=null) 
                {
                    xmlCommander = comm;
                    netPath = path;
                    this.dispatcher = dispatcher;
                    //---
                    Items = new ObservableCollection<DataGroup>();
                }

                
                public string Name { get; set; }   // имя
                public ObservableCollection<DataGroup> Items { get; set; }
                private string netId = "A7";

                //    Loading empty groups in page "A7" 
                // to have ability create datagrids with no data
                // and load data later
                public void LoadEmptyGroups()
                {
                    try
                    {
                        if (xmlCommander != null)
                        {
                            // Block auto update in DataItems of their source 
                            // because is loading them from the same source
                            //SourceUpdateEnabled = false;

                            //   To increase speed - reading Element into array and then 
                            // taking Attributes from array
                            byte[] buff = xmlCommander.readElement(netPath);
                            // check path: is this <Service_A7> path
                            if (xmlCommander.readStringAttribute(null, "Id", buff) == netId)
                            {
                                Name = xmlCommander.readStringAttribute(null, "Name", buff);
                                // reading number of groups in page "A7"
                                int count = xmlCommander.readIntAttribute(null, "Count", buff) ?? 0;
                                // reading groups in page "A7"
                                Items.Clear();
                                for (int i = 0; i < count; i++)
                                {
                                    DataGroup dg = new DataGroup(xmlCommander, netPath + "." + i.ToString("0"), dispatcher);
                                    dg.LoadName();
                                    Items.Add(dg);
                                }
                                //---
                                //SourceUpdateEnabled = true;
                            }
                        }
                        else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                }

                // Loading content of empty groups loaded before
                public void LoadGroupsContent(int? index=null)
                {
                    try
                    {
                        if (xmlCommander != null)
                        {
                            if (index == null) for (int i = 0; i < Items.Count; i++) Items[i].Load();
                            else if (index >= 0 && index < Items.Count) Items[index.Value].Load();
                            else throw (new ArgumentOutOfRangeException());
                        }
                        else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                }

                // async Loading content of empty groups loaded before
                public async Task LoadGroupsContentAsync()
                {
                    try
                    {
                        await Task.Run(() => LoadGroupsContent());
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                }

                // the same as LoadGroupsContentAsync() for using with BuildEmptyTabControl()
                public async Task LoadTabsContentAsync()
                {
                    try
                    {
                        await Task.Run(() => LoadGroupsContent());
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                }

                //  Loading groups with content 
                public void Load()
                {
                    if (xmlCommander != null)
                    {
                        LoadEmptyGroups();
                        LoadGroupsContent();
                    }
                    else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                }

                //  async Loading groups with content 
                public async Task LoadAsync()
                {
                    await Task.Run(()=>Load());
                }


                // to get Datagrids is neccessary to have list of groups
                // therefore have to be executed LoadEmptyGroups() or Load() first
                public ObservableCollection<DataGrid> BuildDataGrids(int? index = null)
                {
                    if (Items.Count == 0) LoadEmptyGroups();
                    //---

                    ObservableCollection<DataGrid> datagrids = new ObservableCollection<DataGrid>();

                    if (index == null) foreach (DataGroup dg in Items) datagrids.Add(dg.BuildDataGrid());
                    else if (index >= 0 && index < Items.Count) datagrids.Add(Items[index.Value].BuildDataGrid());
                    else throw (new ArgumentOutOfRangeException());
                    
                    return datagrids;
                }

                // async Loading given TabControl with TabItems containing empty DataGrids
                // Example of using:
                //      TabControl tbc = new TabControl();
                //      servicesPanel.Children.Clear();
                //      servicesPanel.Children.Add(tbc);
                //      serviceA7.dataSource.BuildEmptyTabControl(tbc);
                //      await serviceA7.dataSource.LoadTabsContentAsync();
 
                public void BuildEmptyTabControl(TabControl tab)
                {
                    try
                    {
                        if (Items.Count == 0) LoadEmptyGroups();
                        //---
                        tab.Items.Clear();
                        foreach (DataGroup dg in Items)
                        {
                            TabItem ti = new TabItem();
                            ti.Header = dg.Name;
                            ti.Content = dg.BuildDataGrid();
                            tab.Items.Add(ti);
                        }
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                }

                // async Loading given TabControl with TabItems containing DataGrids
                // Example of using:
                //      TabControl tbc = new TabControl();
                //      servicesPanel.Children.Clear();
                //      servicesPanel.Children.Add(tbc);
                //      await serviceA7.dataSource.BuildTabControlAsync(tbc);

                async public Task BuildTabControlAsync(TabControl tab)
                {
                    try
                    {
                        if (dispatcher != null) await dispatcher.Invoke(async () =>
                        {
                            if (Items.Count == 0) LoadEmptyGroups();
                            //---
                            tab.Items.Clear();
                            foreach (DataGroup dg in Items)
                            {
                                TabItem ti = new TabItem();
                                ti.Header = dg.Name;
                                ti.Content = dg.BuildDataGrid();
                                tab.Items.Add(ti);
                                await dg.LoadAsync();
                            }
                        });
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                }

                //*********   DataGroup   **********

                public class DataGroup
                {
                    XmlCommander xmlCommander;
                    public string netPath { get; set; }   // путь по которому Item была извлечена из контроллера, для обратной записи значения при редактировании
                    public System.Windows.Threading.Dispatcher dispatcher;
                    //---

                    public DataGroup(XmlCommander comm = null, string path = null, System.Windows.Threading.Dispatcher dispatcher=null) 
                    {
                        xmlCommander = comm;
                        netPath = path;
                        this.dispatcher = dispatcher;
                        //---
                        Items = new ObservableCollection<DataItem>();
                    }

                    //---
                    public string Name { get; set; }   // имя группы
                    public ObservableCollection<DataItem> Items { get; set; }

                    public void LoadName()
                    {
                        try
                        {
                            if (xmlCommander != null)
                            {
                                byte[] buff = xmlCommander.readElement(netPath);
                                Name = xmlCommander.readStringAttribute(null, "Name", buff);
                            }
                            else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                    }


                    public void Load()
                    {
                        try
                        {
                            if (xmlCommander != null)
                            {
                                byte[] buff = xmlCommander.readElement(netPath);
                                Name = xmlCommander.readStringAttribute(null, "Name", buff);
                                //---
                                int count = xmlCommander.readIntAttribute(null, "Count", buff) ?? 0;

                                if (dispatcher != null) dispatcher.Invoke(() => Items.Clear());
                                else Items.Clear();

                                for (int i = 0; i < count; i++)
                                {
                                    DataItem di = new DataItem(xmlCommander, netPath + "." + i.ToString("0"), dispatcher);
                                    di.Load();
                                    //---
                                    if(dispatcher!= null) dispatcher.Invoke(()=>Items.Add(di));
                                    else Items.Add(di);
                                }
                            }
                            else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                    }


                    async public Task LoadAsync()
                    {
                        try
                        {
                            await Task.Run(() => Load());
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return; }
                    }



                    //--------  getDataGrid()   ---------

                    public DataGrid BuildDataGrid()
                    {
                        try
                        {
                            DataGrid datagrid = new DataGrid();

                            datagrid.AutoGenerateColumns = false ;
                            datagrid.CanUserAddRows = false; //  чтобы автоматически не добавлялась пустая строка снизу "PlaceHolder", которая еще мешает и Validation.
                            datagrid.CanUserDeleteRows = false;
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
                            //datagrid.ColumnHeaderStyle = style;
                            //datagrid.Style = style;
                            datagrid.CellStyle = style;
                            datagrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                            datagrid.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                            //datagrid.HorizontalContentAlignment = HorizontalAlignment.Center;
                            datagrid.RowHeight = 30;
                            datagrid.Tag = this; //  для обработчиков
                            datagrid.ItemsSource = Items;

                            //****************   HANDLER
                            //datagrid.CellEditEnding += datagrid_CellEditEnding;
                            datagrid.PreviewTextInput += datagrid_TextInput;

                            //****************   COLUMNS
                                                        
                            DataGridTextColumn item_number_column = new DataGridTextColumn();
                            DataGridTextColumn name_column = new DataGridTextColumn();
                            DataGridTextColumn value_column = new DataGridTextColumn();
                            //DataGridTemplateColumn value_column = DataGridUserScrollerColumn("viewValue", "viewMin", "viewMax", "viewDX", 100);
                            DataGridTextColumn unit_column = new DataGridTextColumn();
                            DataGridTextColumn comment_column = new DataGridTextColumn();
                            

                            //****************   HEADERS

                            item_number_column.Header = "#";
                            name_column.Header = "Name";
                            value_column.Header = "Value";
                            unit_column.Header = "Unit";
                            comment_column.Header = "Comment";

                            item_number_column.MinWidth = 50;
                            name_column.MinWidth = 100;
                            value_column.MinWidth = 100;
                            unit_column.MinWidth = 100;
                            comment_column.MinWidth = 300;

                            item_number_column.IsReadOnly = true;
                            name_column.IsReadOnly = true;
                            unit_column.IsReadOnly = true;
                            comment_column.IsReadOnly = true;


                            //****************   COLUMNS BINDING

                            // чтобы брать по умолчанию текущее содержимое Item нельзя задавать Source и Path.      
                            Binding item_number_column_binding = new Binding();
                            item_number_column_binding.Converter = new _Noms_Item_number_column_Converter();
                            item_number_column_binding.ConverterParameter = Items;
                            item_number_column.Binding = item_number_column_binding;

                            Binding name_column_binding = new Binding();
                            name_column_binding.Path = new PropertyPath("Name");
                            name_column.Binding = name_column_binding;

                            Binding value_column_binding = new Binding();
                            value_column_binding.Path = new PropertyPath("viewValue");
                            // value_column_binding.Converter = new Value_column_Converter();
                            // value_column_binding.ConverterParameter = datagrid;
                            value_column_binding.Mode = BindingMode.TwoWay;
                            value_column_binding.ValidationRules.Add(new ValueValidationRule(datagrid));
                            value_column.Binding = value_column_binding;

                            Binding unit_column_binding = new Binding();
                            unit_column_binding.Path = new PropertyPath("Unit");
                            unit_column.Binding = unit_column_binding;

                            Binding comment_column_binding = new Binding();
                            comment_column_binding.Path = new PropertyPath("Comment");
                            comment_column.Binding = comment_column_binding;


                            //****************   COLUMNS ORDER

                            datagrid.Columns.Add(item_number_column);
                            datagrid.Columns.Add(name_column);
                            datagrid.Columns.Add(value_column);
                            datagrid.Columns.Add(unit_column);
                            datagrid.Columns.Add(comment_column);


                            return datagrid;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    }



                    class ValueValidationRule : ValidationRule
                    {
                        DataGrid sender;
                        public ValueValidationRule(DataGrid sender)
                        {
                            this.sender = sender;
                        }

                        override public ValidationResult Validate(object value, CultureInfo cultureInfo)
                        {
                            double ax = 0;

                            // check text for wrong symbols
                            if (double.TryParse(value as string, out ax))
                            {
                                //  check Min-Max
                                DataGrid dg = sender as DataGrid;
                                //DataGroup datagroup = datagrid.Tag as DataGroup;
                                DataItem di = (dg.CurrentItem as DataItem);
                                if( di != null && ax >= di.viewMin && ax <= di.viewMax) return new ValidationResult(true, null);
                                MessageBox.Show("Acceptable range: <" + di.viewMin.ToString("0.0##") +
                                                " ... " + di.viewMax.ToString("0.0##") + ">", "Wrong value range", MessageBoxButton.OK,MessageBoxImage.Warning);
                            }
                            else MessageBox.Show("Can't parse value as number", "Wrong value expression", MessageBoxButton.OK, MessageBoxImage.Warning);

                            return new ValidationResult(false, null);
                        }
                    }


                    //  this event shows not entire text of cell but only 
                    //  currently inputed text char by char 
                    //  so this method not resolve all problems
                    void datagrid_TextInput(object sender, TextCompositionEventArgs e)
                    {
                        if (e.Text.Length == 0) return;
                        if (e.Text.Length == 1 && (e.Text[0] == '-' || e.Text[0] == '+' 
                            || e.Text[0] == '.')) return;

                        double ax = 0;
                        if (double.TryParse(e.Text, out ax)) return;
                        //  Cancel Input
                        e.Handled = true;
                    }



                    //void datagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
                    //{
                    //    try  //  находим индекс текущего item в коллекции.
                    //    {
                    //        DataGrid datagrid = sender as DataGrid;
                    //        DataGroup datagroup = datagrid.Tag as DataGroup;
                    //        int index = e.Row.GetIndex();
                    //        double ax = 0;
                    //        if (Double.TryParse((e.EditingElement as TextBox).Text, out ax)) 
                    //        {
                    //            xmlCommander.writeStringAttribute(datagroup.Items[index].netPath, "Value", ax.ToString());
                    //        }
                    //        else e.Cancel = true;
                    //    }
                    //    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    //}

                    //***************   CONVERTERS

                    public class _Noms_Item_number_column_Converter : IValueConverter
                    {
                        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                        {
                            try  //  находим индекс текущего item в коллекции.
                            {
                                return ((ObservableCollection<DataItem>)parameter).IndexOf((DataItem)value) + 1;
                            }
                            catch (Exception excp) { MessageBox.Show(excp.ToString()); return null;}

                        }

                        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                        {

                            try
                            {
                               MessageBox.Show("Programm: RadioButton_ConvertBack_Warning");
                               return 0;
                            }
                            catch (Exception excp) { MessageBox.Show(excp.ToString()); return null;}
                        }

                    }
                    //***********

                    //public class Value_column_Converter : IValueConverter
                    //{
                    //    static int count = 0;
                    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    //    {
                    //        try  //  находим индекс текущего item в коллекции.
                    //        {
                    //            count += 10;
                    //            return value;

                    //            //return ((DataItem)value).Value.ToString();
                    //        }
                    //        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    //    }

                    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    //    {

                    //        try
                    //        {
                    //            DataGrid datagrid = parameter as DataGrid;
                    //            string path = (datagrid.Items.CurrentItem as DataItem).netPath;
                    //            //MessageBox.Show("path=" + path + "; value = " + value);
                    //            count++;
                    //            return value;//.ToString();// +count.ToString();
                    //        }
                    //        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    //    }

                    //}
                    //***********

                    //***********   DataItem   ***********

                    // шаблон блока данных закрепляемого за каждой уставкой.
                    public class DataItem
                    {
                        XmlCommander xmlCommander;
                        public string netPath { get; set; }   // путь по которому Item была извлечена из контроллера, для обратной записи значения при редактировании
                        public System.Windows.Threading.Dispatcher dispatcher;
                        //---

                        //public DataItem() { }

                        public DataItem(XmlCommander comm=null, string path=null, System.Windows.Threading.Dispatcher dispatcher=null) 
                        { 
                            xmlCommander = comm;
                            netPath = path;
                            this.dispatcher = dispatcher;
                        }
                        //---

                        // Block auto-update of source of DataItem  
                        // because is loading it from the same source
                        bool SourceUpdateEnabled = false;

                        public string Name { get; set; }   // имя уставки
                        public double Min { get; set; }   // минимальное значение вличины в программных дискретах
                        public double Max { get; set; }   // максимальное значение величины в программных дискретах
                        public double Nom { get; set; }   // номинальное значение данной уставки в программных дискретах
                        public double AbsNom { get; set; }   // номинальное значение данной уставки в физических единицах
                        public double dX { get; set; }   // дискретность инкрементации уставки
                        public string Unit { get; set; }   // текст единиц измерения
                        public string Format { get; set; }   // printf-like format
                        public string Comment { get; set; }   // комментарий
                        //---
                        double _value;
                        public double Value  // значение уставки в программных дискретах
                        {
                            get { return _value; }
                            set
                            {
                                _value = value;
                                UpdateSource();
                            }
                        }   

                        //  для отображения
                        public double viewValue { 
                            get {   return Value / Nom * AbsNom; }
                            set { 
                                    double ax = value * Nom / AbsNom;
                                    ax = ax > Max ? Max : ax;
                                    ax = ax < Min ? Min : ax;
                                    this.Value = ax;}
                                }
                        public double viewDX
                        {
                            get { return dX / Nom * AbsNom; }
                        }
                        public double viewMin
                        {
                            get { return Min / Nom * AbsNom; }
                        }
                        public double viewMax
                        {
                            get { return Max / Nom * AbsNom; }
                        }


                        public void Load()
                        {
                            if (xmlCommander != null)
                            {
                                SourceUpdateEnabled = false;
                                //---
                                // read into buff all Attributes from CPU  
                                byte[] buff = xmlCommander.readElement(netPath);
                                // read from buff Attributes one by one
                                Name = xmlCommander.readStringAttribute(null, "Name", buff);
                                Value = xmlCommander.readDoubleAttribute(null, "Value", buff) ?? 0;
                                Min = xmlCommander.readDoubleAttribute(null, "Min", buff) ?? 0;
                                Max = xmlCommander.readDoubleAttribute(null, "Max", buff) ?? 0;
                                Nom = xmlCommander.readDoubleAttribute(null, "Nom", buff) ?? 0;
                                AbsNom = xmlCommander.readDoubleAttribute(null, "AbsNom", buff) ?? 0;
                                dX = xmlCommander.readDoubleAttribute(null, "dX", buff) ?? 0;
                                Unit = xmlCommander.readStringAttribute(null, "Unit", buff);
                                Format = xmlCommander.readStringAttribute(null, "Format", buff);
                                //---
                                SourceUpdateEnabled = true;
                            }
                            else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                        }

                        public bool UpdateSource()
                        {
                            if (xmlCommander != null)
                            {
                                if (SourceUpdateEnabled)
                                {
                                    xmlCommander.writeStringAttribute(netPath, "Value", _value.ToString());
                                    // check - if MCU accepted value
                                    _value = xmlCommander.readDoubleAttribute(netPath, "Value") ?? 0;
                                    return true;
                                }
                            }
                            else throw (new Exception("Reference is Null: " + this.ToString() + " : " + " Load() : " + xmlCommander.ToString()));
                            return false;
                        }

                    }
                    //----
                    //  не работает привязка SmallChange
                    //
                    //public static DataGridTemplateColumn DataGridUserScrollerColumn(
                    //    string valueProperty_path,
                    //    string minProperty_path, string maxProperty_path, string dxProperty_path, double minWidth,
                    //    IValueConverter items_source_converter=null)
                    //{
                    //    try
                    //    {

                    //        DataGridTemplateColumn column_template = new DataGridTemplateColumn();
                    //        //column_template.Header = header;
                    //        //column_template.Width = width;

                    //        //************   CELL TEMPLATE: TEXTBLOCK 

                    //        column_template.CellTemplate = new DataTemplate();
                    //            FrameworkElementFactory stackpanel = new FrameworkElementFactory(typeof(StackPanel));
                    //            stackpanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                    //                FrameworkElementFactory textblock = new FrameworkElementFactory(typeof(TextBlock));
                    //                //textblock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    //                //textblock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                    //                //textblock.SetValue(TextBlock.MinWidthProperty, width);
                    //                Binding textblock_binding = new Binding();
                    //                textblock_binding.Path = new PropertyPath(valueProperty_path);
                    //                textblock.SetBinding(TextBlock.TextProperty, textblock_binding);
                    //                //  он сйечас не нужен, но оставил его т.к. он определяет высоту ячейки
                    //                //FrameworkElementFactory scrollbar = new FrameworkElementFactory(typeof(ScrollBar));
                    //                //scrollbar.SetValue(ScrollBar.VisibilityProperty, Visibility.Hidden);

                    //            stackpanel.AppendChild(textblock);
                    //        column_template.CellTemplate.VisualTree = stackpanel;

                    //        //stackpanel.AppendChild(scrollbar);

                    //        ////************   CELL EDIT TEMPLATE:  COMBOBOX 

                    //        column_template.CellEditingTemplate = new DataTemplate();

                    //            stackpanel = new FrameworkElementFactory(typeof(StackPanel));
                    //            stackpanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                    //                FrameworkElementFactory textbox = new FrameworkElementFactory(typeof(TextBox));
                    //                //textblock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    //                //textblock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                    //                textbox.SetValue(TextBlock.MinWidthProperty, minWidth*0.9);
                    //                Binding textbox_binding = new Binding();
                    //                textbox_binding.Path = new PropertyPath(valueProperty_path);
                    //                textbox_binding.Mode = BindingMode.TwoWay;
                    //                textbox.SetBinding(TextBox.TextProperty, textbox_binding);

                    //                FrameworkElementFactory scrollbar = new FrameworkElementFactory(typeof(ScrollBar));
                    //                scrollbar.SetValue(ScrollBar.WidthProperty, minWidth * 0.1);
                    //                scrollbar.AddHandler(ScrollBar.ValueChangedEvent, new RoutedPropertyChangedEventHandler<double>(scrollbar_ValueChanged));

                    //                Binding scrollbar_binding = new Binding();
                    //                scrollbar_binding.Path = new PropertyPath(valueProperty_path);
                    //                scrollbar_binding.Mode = BindingMode.TwoWay;
                    //                scrollbar_binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    //                scrollbar.SetBinding(ScrollBar.ValueProperty, scrollbar_binding);

                    //                scrollbar_binding = new Binding();
                    //                scrollbar_binding.Path = new PropertyPath(minProperty_path);
                    //                scrollbar.SetBinding(ScrollBar.MinimumProperty, scrollbar_binding);

                    //                scrollbar_binding = new Binding();
                    //                scrollbar_binding.Path = new PropertyPath(maxProperty_path);
                    //                scrollbar.SetBinding(ScrollBar.MaximumProperty, scrollbar_binding);

                    //                scrollbar_binding = new Binding();
                    //                scrollbar_binding.Path = new PropertyPath(dxProperty_path);
                    //                scrollbar_binding.Mode = BindingMode.OneWay;
                    //                scrollbar.SetBinding(ScrollBar.SmallChangeProperty, scrollbar_binding);
                    //                //scrollbar.SetBinding(ScrollBar.LargeChangeProperty, scrollbar_binding);

                    //            stackpanel.AppendChild(textbox);
                    //            stackpanel.AppendChild(scrollbar);

                    //        column_template.CellEditingTemplate.VisualTree = stackpanel;


                    //        return column_template;

                    //    }
                    //    catch (Exception excp)
                    //    {
                    //        MessageBox.Show(excp.ToString());
                    //        return null;
                    //    }

                    //}

                    //static void scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
                    //{
                    //    ScrollBar s = sender as ScrollBar;
                    //    double value = s.Value;
                    //    double min = s.Minimum;
                    //    double max = s.Maximum;
                    //    double dx = s.SmallChange;
                    //    double sx = s.LargeChange;
                    //    //MessageBox.Show(" " +

                    //    //при щелкании наряде строк датагрид прчему-то возникает два захода сюда, при
                    //    //    этом первый заход строк неправильной привязкой данных value=2.0 min=2.0 max=2.0

                    //    //не работает привязка для SmallChange
                    //    //если убрать привязку SmallChange то работает по другому

                    //    //если вместо dX подставить другое поле то работает SmallChange
                    //    //переименовал dX на SmallChange - не помогло
                    //}


                }
                //---------
            }
            //----------
        }
    }
}