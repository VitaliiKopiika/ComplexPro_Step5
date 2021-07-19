// + убрать из нетворка старый вариант LABEL
// + сделать для компилятора общую для всех диаграмм Validation: проверка повторяемости меток, имен переменных, диаграмм и т.д.
//  сделать для Label проверку достоверности ввода.

// + добавить в компилятор распечатку метки нетворка в С-текст.
// + в XPS выводится толька одна локальная переменная.
// + сделать Upgrade всех значков Диаграм в нетворках при изменении значка
// + сделать загрузку из файла диаграмм со значками.
// + cделать копирование диаграмм
// + сделать копирование вставку network
// + доделать вставку элемента- в ставляется почемуто не перед элементом а после него
// - не надо - сделать чтобы при перетаскивании элемиента на его прежнем месте было не пустое место а ... неизвестно что...
// + заменить popUp на ContextMenu
// + заменить Window на popUp
// + делать добавление убавление Networkov v 
// + вставить удаление строки в Field_copy после удаления строки из нетворка

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

        //************   SHOW_DIAGRAMS()

static public void SHOW_DIAGRAMS()
    {
        try
        {

            DIAGRAMS_TAB_PANEL = new TabControl();
                Binding binding = new Binding();
                binding.Source = DIAGRAMS_PANEL;
                binding.Path =  new PropertyPath("ActualHeight");
            DIAGRAMS_TAB_PANEL.SetBinding(TabControl.HeightProperty, binding);
                binding = new Binding();
                binding.Source = DIAGRAMS_PANEL;
                binding.Path = new PropertyPath("ActualWidth");
            DIAGRAMS_TAB_PANEL.SetBinding(TabControl.WidthProperty, binding);

            DIAGRAMS_PANEL.Children.Clear();
            DIAGRAMS_PANEL.Children.Add(DIAGRAMS_TAB_PANEL);

            //DIAGRAMS_PANEL.Width = DIAGRAMS_PANEL.ExtentWidth;
            //DIAGRAMS_PANEL.Height = DIAGRAMS_PANEL.ExtentHeight;

            foreach( Diagram_Of_Networks diagram in DIAGRAMS_LIST )
            {
                TabItem tab_item = diagram.Get_DiagramImage();
                
                DIAGRAMS_TAB_PANEL.Items.Add(tab_item);
            }

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
    }



//************   UPDATE  USER_FUNCTION ELEMENT IMAGES

static public void UPDATE_USER_FUNCTION_IMAGES(ElementImage old_element_image, ElementImage new_element_image)
{
    try
    {

        foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        {
            foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
            {
                for (int row = 0 ; row < network.Network_panel_copy.Count ; row++)
                {
                    for (int col = 0; col < network.Network_panel_copy[row].Count; col++)
                    {
                        Element_Data element = network.Network_panel_copy[row][col];

                        if (element != null)
                        {
                            if (element.Txt_Image.Name == old_element_image.Name)
                            {
                                //---   COPY OLD ELEMENT

                                _DRAG_DROP_BUFF.obj = element.Border_Image;
                                _DRAG_DROP_BUFF.obj_type = typeof(Border);
                                _DRAG_DROP_BUFF.param = "UpDate";

                                //---   UPDATE ELEMENT
                                Network_Of_Elements.CopyDrop_Element(element.Coordinats, new_element_image);

                            }
                        }
                    }
                }
            }


        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


//****************************************************

//****************************************************
//****************************************************

//************   SHOW ONE DIAGRAM

public partial class Diagram_Of_Networks
{
//************   SHOW_DIAGRAM()

    public TabItem Get_DiagramImage()
    {
        try
        {
            //TabItem tab_item = new TabItem();
            
            //TAB_ITEM = tab_item; //  запоминаем для возможности его удаления.

            //TAB_ITEM.Tag = this;

            //TAB_ITEM.HorizontalAlignment = HorizontalAlignment.Stretch;
            //TAB_ITEM.VerticalAlignment = VerticalAlignment.Stretch;


        //***************  INSERT ELEMENT CONTEXT MENU
                    
            ContextMenu menu = new ContextMenu();
                        MenuItem menu_item = new MenuItem();
                        menu_item.Header = "Copy diagram";
                        menu_item.Tag = this;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Copy_Diagram_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Insert diagram";
                        menu_item.Tag = this;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Insert_Diagram_Click;
                    menu.Items.Add(menu_item);

            TAB_ITEM.ContextMenu = menu;


                TextBlock textblock0 = new TextBlock();
                textblock0.MaxWidth = 100;
                    Binding name_binding = new Binding();
                    name_binding.Source = this;
                    name_binding.Path = new PropertyPath("NAME");
                    name_binding.NotifyOnTargetUpdated = true;
                textblock0.SetBinding(TextBlock.TextProperty, name_binding);

                textblock0.TargetUpdated += DiagramName_textblock0_TargetUpdated;
                textblock0.Tag = this; //  для события.

            TAB_ITEM.Header = textblock0;

                //ScrollViewer scroll_viewer = new ScrollViewer();
                //scroll_viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            //TAB_ITEM.Content = scroll_viewer;

//*************  DIAGRAM COMMENT & BUTTONS

            Grid grid = new Grid();
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                grid.RowDefinitions.Add(row);
                row = new RowDefinition();
                grid.RowDefinitions.Add(row);
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.VerticalAlignment = VerticalAlignment.Stretch;


            //scroll_viewer.Content = grid; 
            TAB_ITEM.Content = grid;

                StackPanel stack_panel = new StackPanel();
                stack_panel.Orientation = Orientation.Horizontal;
                stack_panel.Margin = new Thickness(0,0,0,5);
                stack_panel.SetValue(Grid.RowProperty, 0);
            
                    //   DELETE DIAGRAM
                    Button delete_diagram_button = new Button();
                    delete_diagram_button.Content = "×";
                    delete_diagram_button.FontSize = 14.0;
                    delete_diagram_button.Foreground = new SolidColorBrush(Colors.Black);
                    delete_diagram_button.VerticalAlignment = VerticalAlignment.Top;
                    delete_diagram_button.VerticalContentAlignment = VerticalAlignment.Top;
                    delete_diagram_button.HorizontalContentAlignment = HorizontalAlignment.Right;
                    delete_diagram_button.Margin = new Thickness(1,0,1,0);
                    delete_diagram_button.Padding = new Thickness(0,0,0.5,1.5);
                    delete_diagram_button.Tag = this;
                    delete_diagram_button.Click += Delete_Diagram_button_Click;
            
                    //   ADD DIAGRAM
                    ScrollBar scrollbar_h = new ScrollBar();
                    scrollbar_h.Visibility = Visibility.Visible;
                    scrollbar_h.Orientation = Orientation.Horizontal;
                    scrollbar_h.Background = new SolidColorBrush(Colors.Transparent);
                    scrollbar_h.Margin = new Thickness(0, 0, 0, 1);
                    scrollbar_h.Height = 10;
                    scrollbar_h.Width = 25;
                    scrollbar_h.Value = 0;
                    scrollbar_h.Minimum = -100;
                    scrollbar_h.Maximum = +100;
                    scrollbar_h.ValueChanged += Add_Diagram_scrollbar_ValueChanged;
                    scrollbar_h.Tag = this;

                    TextBox textbox0 = new TextBox();
                    textbox0.MinWidth = 100;
                    textbox0.MaxWidth = 150;
                    textbox0.Margin = new Thickness(5, 0, 5, 0);
                    textbox0.SetBinding(TextBox.TextProperty, name_binding);

                    TextBox textbox = new TextBox();
                    textbox.MinWidth = 300.0;
                    textbox.MaxWidth = 500.0;
                    textbox.TextWrapping = TextWrapping.Wrap;
                        Binding comment_binding = new Binding();
                        comment_binding.Source = this;
                        comment_binding.Path = new PropertyPath("COMMENT");
                        comment_binding.Mode = BindingMode.TwoWay;
                    textbox.SetBinding(TextBox.TextProperty, comment_binding);
                    textbox.TextAlignment = TextAlignment.Left;
                    textbox.FontSize = xFontSize;

                    Slider scale_slider = new Slider();
                    scale_slider.Width = 70;
                    scale_slider.VerticalAlignment = VerticalAlignment.Stretch;
                    scale_slider.Orientation = Orientation.Horizontal;
                    scale_slider.Minimum = 0.3;
                    scale_slider.Maximum = 1.5;
                    scale_slider.Value = 1.0;

                    TextBlock textbox_scale = new TextBlock();
                    textbox_scale.FontSize = xFontSize;
                    textbox_scale.MinWidth = 30.0;
                    textbox_scale.TextAlignment = TextAlignment.Right;
                    Binding textbox_scale_binding = new Binding();
                        textbox_scale_binding.Source = scale_slider;
                        textbox_scale_binding.Path = new PropertyPath("Value");
                        textbox_scale_binding.Converter = new DiagramScale_binding_Converter();
                    textbox_scale.SetBinding(TextBlock.TextProperty, textbox_scale_binding);
                    
                    
                stack_panel.Children.Add(delete_diagram_button);
                //stack_panel.Children.Add(scrollbar_h);
                stack_panel.Children.Add(textbox0);
                stack_panel.Children.Add(textbox);
                stack_panel.Children.Add(scale_slider);
                stack_panel.Children.Add(textbox_scale);

            grid.Children.Add(stack_panel);

//*************  ADD ALL NETWORKS

            StackPanel diagram_stack_panel = new StackPanel();
            diagram_stack_panel.Orientation = Orientation.Vertical;
            diagram_stack_panel.HorizontalAlignment = HorizontalAlignment.Stretch;
            diagram_stack_panel.VerticalAlignment = VerticalAlignment.Stretch;
            MAIN_PANEL = diagram_stack_panel;
            diagram_stack_panel.SetValue(Grid.RowProperty, 1);
            
            ScaleTransform scale = new ScaleTransform();
            //diagram_stack_panel.RenderTransform = scale; вылазит за рамки ScrollViewer и он этого не проматывает
            diagram_stack_panel.LayoutTransform = scale;
           
                Binding scale_binding = new Binding();
                        scale_binding.Source = scale_slider;
                        scale_binding.Path = new PropertyPath("Value");
                BindingOperations.SetBinding(scale, ScaleTransform.ScaleXProperty, scale_binding);
                BindingOperations.SetBinding(scale, ScaleTransform.ScaleYProperty, scale_binding);


            foreach( Network_Of_Elements network in NETWORKS_LIST )
            {
                diagram_stack_panel.Children.Add(network.Get_NetworkImage());
            }


            //grid.Children.Add(diagram_stack_panel);

            ScrollViewer scroll_viewer = new ScrollViewer();
                scroll_viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scroll_viewer.SetValue(Grid.RowProperty, 1);
                //scroll_viewer.HorizontalAlignment = HorizontalAlignment.Stretch;
                //scroll_viewer.VerticalAlignment = VerticalAlignment.Stretch;
            
            scroll_viewer.Content = diagram_stack_panel;

            grid.Children.Add(scroll_viewer);


//*************
            return TAB_ITEM;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }
    }


    //*******************   CONVERTERS  *******************************

    public class DiagramScale_binding_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try  //  находим индекс текущего item в коллекции.
            {
                int ax = (int)((double)value * 100);
                return ax.ToString()+"%";
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
                    MessageBox.Show("Programm: No ConvertBack available.");
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

//***********************    HANDLERS   **************************************


//*************  COPY-INSERT DIAGRAM

static void Copy_Diagram_Click(object sender, RoutedEventArgs e)
{
    try
    {
        _DRAG_DROP_BUFF.obj = ((MenuItem)sender).Tag;
        _DRAG_DROP_BUFF.obj_type = typeof(Diagram_Of_Networks);
        _DRAG_DROP_BUFF.param = "Copy Diagram";

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

static void Insert_Diagram_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (_DRAG_DROP_BUFF.param == "Copy Diagram")
        {
            //  Diagram перед которым будет вставка.
            Diagram_Of_Networks diagram = (Diagram_Of_Networks)((MenuItem)sender).Tag;
            int index = DIAGRAMS_LIST.IndexOf(diagram);

            //  Network который будет вставляться.
            Diagram_Of_Networks new_diagram = ((Diagram_Of_Networks)_DRAG_DROP_BUFF.obj).GetDiagramCopy();
            new_diagram.NAME = "No Name";
            new_diagram.PROJECT = diagram.PROJECT;
            TabItem tabitem = new_diagram.Get_DiagramImage();

            //  не разрешаем вставлять перед Main-diagram.
            if (index == 0) index = 1;

            //  на случай если копируем Main-diagram, а на поле всего она одна - чтобы индекс не был за пределами массива.
            if (DIAGRAMS_LIST.Count > 1)
            {
                DIAGRAMS_LIST.Insert(index, new_diagram);
                DIAGRAMS_TAB_PANEL.Items.Insert(index, tabitem);
            }
            else
            {
                DIAGRAMS_LIST.Add(new_diagram);
                DIAGRAMS_TAB_PANEL.Items.Add(tabitem);
            }
            
                //--- выводим TabItem на передний план.
            DIAGRAMS_TAB_PANEL.SelectedItem = tabitem;

        }

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}




//***************  DELETE DIAGRAM

static void Delete_Diagram_button_Click(object sender, RoutedEventArgs e)
{
        try
        {
            Diagram_Of_Networks diagram = (Diagram_Of_Networks)((Button)sender).Tag;
            int index = Step5.DIAGRAMS_LIST.IndexOf(diagram);

            if ( index != 0 ) //  удалять нулевой-main Diagram не разрешается.
            {
                MessageBoxResult res = MessageBox.Show("Delete?\nYou can't restore it after! ", PROJECT_NAME, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (res == MessageBoxResult.No) return;
                if (res == MessageBoxResult.Yes)
                {
                    ELEMENTS_DICTIONARY.Remove_DiagramElementImage(diagram);
                    Step5.DIAGRAMS_LIST.Remove(diagram);
                    Step5.DIAGRAMS_TAB_PANEL.Items.Remove(diagram.TAB_ITEM);
                }
            }
            
            //---

            e.Handled = true;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
    }



//***************  ADD DIAGRAM

    static void Add_Diagram_scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        try
        {

            Diagram_Of_Networks diagram = (Diagram_Of_Networks)((ScrollBar)sender).Tag;
            int count = Step5.DIAGRAMS_LIST.Count;
            int index = Step5.DIAGRAMS_LIST.IndexOf(diagram);

            Diagram_Of_Networks new_diagram = new Diagram_Of_Networks();

            TabItem tabitem = new_diagram.Get_DiagramImage();

            //    Добавление/вставка после Diagram.        
            if (((ScrollBar)sender).Value > ((Diagram_Of_Networks)((ScrollBar)sender).Tag).diagram_scrollbar_value)
            {

                //  если элемент является последним в списке то - добавление, иначе вставка  после него.
                if (index == count - 1)    //  добавление в конец списка.
                {
                    Step5.DIAGRAMS_LIST.Add(new_diagram);
                    Step5.DIAGRAMS_TAB_PANEL.Items.Add(tabitem);
                }
                else  //  вставка после текущего элемента.
                {
                    //  определение индекса текущего network для добавления после него.
                    //индекс должен совпадать с уже вычисленным индексом.
                    // index.index = network.DIAGRAM.MAIN_PANEL.Children.IndexOf(network.MAIN_BORDER_PANEL);

                    Step5.DIAGRAMS_LIST.Insert(index + 1, new_diagram);
                    Step5.DIAGRAMS_TAB_PANEL.Items.Insert(index + 1, tabitem);
                }
            }
            //    Вставка перед текущим NDiagram.  
            else
            {
                if (index != 0)    //  добавлять перед нулевым-main Diagram не разрешается.
                {
                    Step5.DIAGRAMS_LIST.Insert(index, new_diagram);
                    Step5.DIAGRAMS_TAB_PANEL.Items.Insert(index, tabitem);
                }
            }

            //--- выводим TabItem на передний план.
            DIAGRAMS_TAB_PANEL.SelectedItem = tabitem;

            //---

            ((Diagram_Of_Networks)((ScrollBar)sender).Tag).diagram_scrollbar_value = ((ScrollBar)sender).Value;

            e.Handled = true;
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
    }

}

//************   DIAGRAM NAME CHANGED - FOR UPDATE DIAGRAM SIGN

static void DiagramName_textblock0_TargetUpdated(object sender, DataTransferEventArgs e)
{
    try
    {
        //  Чтобы не срабатывало пока идет загрузка из файла.
        //if (ACTIV_DIAGRAM != null)
        //{
            //ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage(ACTIV_DIAGRAM);
        //}

        ELEMENTS_DICTIONARY.AddReplace_DiagramElementImage((Diagram_Of_Networks)((TextBlock)sender).Tag);
        
        //---
        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return;
    }

}


public partial class Network_Of_Elements
{

//************   SHOW_NETWORK()

    public Border Get_NetworkImage () // Network_Of_Elements network)
    {
        try
        {
            Network_Of_Elements network = this;

            Border xborder = new Border();
        MAIN_BORDER_PANEL = xborder;
            xborder.Background = new SolidColorBrush(Colors.White);
            xborder.HorizontalAlignment = HorizontalAlignment.Left;
            xborder.VerticalAlignment = VerticalAlignment.Stretch;
            
                //Binding binding = new Binding();
                //binding.Source = DIAGRAM.MAIN_PANEL;
                //binding.Path = new PropertyPath("ActualWidth");
                //xborder.SetBinding(Border.WidthProperty, binding);

            xborder.Padding = new Thickness(3);
            xborder.Margin  = new Thickness(5);
            Style style = new Style();
                style.Setters.Add(new Setter(Border.BorderBrushProperty, new SolidColorBrush(Colors.Gray)));
                style.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(1.0)));
                    Trigger trigg = new Trigger();
                    trigg.Property = Border.IsMouseOverProperty;
                    trigg.Value = true;
                    trigg.Setters.Add(new Setter(Border.BorderBrushProperty, new SolidColorBrush(Colors.Black)));
                    trigg.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(1.0)));
                style.Triggers.Add(trigg);
            xborder.Style = style;

            StackPanel network_stack_panel = new StackPanel();
            network_stack_panel.Orientation = Orientation.Vertical;
            network_stack_panel.HorizontalAlignment = HorizontalAlignment.Stretch;
            network_stack_panel.VerticalAlignment = VerticalAlignment.Stretch;

//**************    OUTER CONTROLs

                    StackPanel stack_panel = new StackPanel();
                    stack_panel.Orientation = Orientation.Horizontal;
                    //stack_panel.HorizontalAlignment = HorizontalAlignment.Left;
                    //stack_panel.VerticalAlignment = VerticalAlignment.Center;

                        //   DELETE NETWORK
                        Button delete_network_button = new Button();
                        //delete_network_button.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        //delete_network_button.Background = new SolidColorBrush(Colors.Transparent);
                        delete_network_button.Content = "×";
                        delete_network_button.FontSize = 14.0;
                        delete_network_button.Foreground = new SolidColorBrush(Colors.Black);
                        //delete_network_button.Width  = 15.0;
                        //delete_network_button.Height = 15.0; 
                        //delete_network_button.HorizontalAlignment = HorizontalAlignment.Right;
                        delete_network_button.VerticalAlignment = VerticalAlignment.Top;
                        delete_network_button.VerticalContentAlignment = VerticalAlignment.Top;
                        delete_network_button.HorizontalContentAlignment = HorizontalAlignment.Right;
                        delete_network_button.Margin = new Thickness(1,0,1,0);
                        delete_network_button.Padding = new Thickness(0,0,0.5,1.5);
                        delete_network_button.Tag = network;
                        delete_network_button.Click += Delete_Network_button_Click;
            
                        //   ADD NETWORK
                        ScrollBar scrollbar = new ScrollBar();
                        scrollbar.Visibility = Visibility.Visible;
                        scrollbar.Orientation = Orientation.Vertical;
                        //scrollbar.VerticalAlignment = VerticalAlignment.Top;
                        scrollbar.Background = new SolidColorBrush(Colors.Transparent);
                        scrollbar.Margin = new Thickness(1,0,5,0);
                        scrollbar.Height = 30;
                        scrollbar.Width = 10;
                        scrollbar.Value = 0;
                        scrollbar.Minimum = -100;
                        scrollbar.Maximum = +100;
                        scrollbar.ValueChanged += Add_Network_scrollbar_ValueChanged;
                        scrollbar.Tag = network;

                        TextBlock textblock = new TextBlock();
                        textblock.MinWidth = 100.0;
                            Binding name_binding = new Binding();
                            name_binding.Source = network.DIAGRAM.NETWORKS_LIST;
                            name_binding.Path = new PropertyPath("Count");
                            name_binding.Converter = new NetworkName_binding_Converter();
                            name_binding.ConverterParameter = network;
                        textblock.SetBinding(TextBlock.TextProperty, name_binding);
                        textblock.TextAlignment = TextAlignment.Left;
                        textblock.FontSize = xFontSize;

                        TextBox textbox = new TextBox();
                        textbox.MinWidth = 300.0;
                        textbox.MaxWidth = 500.0;
                        textbox.TextWrapping = TextWrapping.Wrap;
                            Binding comment_binding = new Binding();
                            comment_binding.Source = network;
                            comment_binding.Path = new PropertyPath("COMMENT");
                            comment_binding.Mode = BindingMode.TwoWay;
                        textbox.SetBinding(TextBox.TextProperty, comment_binding);
                        textbox.TextAlignment = TextAlignment.Left;
                        textbox.FontSize = xFontSize;

                    stack_panel.Children.Add(delete_network_button);
                    stack_panel.Children.Add(scrollbar);
                    stack_panel.Children.Add(textblock);
                    stack_panel.Children.Add(textbox);
                    
            //---   добавляем строку для LABEL нетворка.

                    /*StackPanel stack_panel2 = new StackPanel();
                    stack_panel2.Orientation = Orientation.Horizontal;

                        TextBlock textblock2 = new TextBlock();
                        textblock2.Text = "Label:  ";
                        textblock2.TextAlignment = TextAlignment.Left;
                        textblock2.FontSize = xFontSize;
                        textblock2.FontWeight = FontWeights.DemiBold;
                        textblock2.Margin = new Thickness(0, 3, 0, 0); //ровняем по уровню с textbox2.

                        TextBox textbox2 = new TextBox();
                        textbox2.Width = 100.0;
                        Binding label_binding = new Binding();
                            label_binding.Source = network;
                            label_binding.Path = new PropertyPath("LABEL");
                            label_binding.Mode = BindingMode.TwoWay;
                            label_binding.ValidationRules.Add(new Label_ValidationRule()); 
                            //// при использовании BindingGroup Notify делается в группе, а иначе сообщение выскакивает по 2 раза.
                            label_binding.NotifyOnValidationError = true;
                        //    Обработчик вывода сообщений для всех Validation общий.
                        // можно и так и так Validation.AddErrorHandler(datagrid, new EventHandler<ValidationErrorEventArgs>(Address_ValidationErrorEvent));
                        textbox2.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(LabelValidationErrorEvent));


                        textbox2.SetBinding(TextBox.TextProperty, label_binding);
                        textbox2.TextAlignment = TextAlignment.Left;
                        textbox2.FontSize = xFontSize;
                        
                    stack_panel2.Children.Add(textblock2);
                    stack_panel2.Children.Add(textbox2);
            */

//**************    MAIN GRID

            //   Создаем grid из двух колонок - левая для вертикальной линии,
            //   правая для network.

            Main_grid0 = new Grid();
            Main_grid0.HorizontalAlignment = HorizontalAlignment.Stretch;
            Main_grid0.VerticalAlignment = VerticalAlignment.Stretch;
            Main_grid0.Margin = new Thickness(0, 5, 0, 0);

                ColumnDefinition column = new ColumnDefinition();
                column.Width = GridLength.Auto;
            Main_grid0.ColumnDefinitions.Add(column);
                column = new ColumnDefinition();
                column.Width = GridLength.Auto;
            Main_grid0.ColumnDefinitions.Add(column);

            //---

            Canvas main_line = ElementImage.LeftVerticalMainLine();
                main_line.SetValue(Grid.ColumnProperty, 0);
                main_line.SetValue(Grid.RowProperty, 1);
            Main_grid0.Children.Add(main_line);

            //   Добавляем кнопку для увеличения количества строк.
            Add_Buttons(network);

                    //******************************************

                    //   Создаем grid-поле заданных размеров.

                    network.Grid_panel = new Grid();
                    network.Grid_panel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    network.Grid_panel.VerticalAlignment = VerticalAlignment.Stretch;
                    network.Grid_panel.SetValue(Grid.ColumnProperty, 1);
                    network.Grid_panel.SetValue(Grid.RowProperty, 0);
                    network.Grid_panel.ShowGridLines = Step5.ShowNetworkGridLines;

                    for ( int i = 0; i < network.HEIGHT; i++)
                    {
                        RowDefinition rows = new RowDefinition();
                        rows.Height = GridLength.Auto;
                        network.Grid_panel.RowDefinitions.Add(rows);
                    }
                    for (int i = 0; i < network.WIDTH; i++)
                    {
                        ColumnDefinition columns = new ColumnDefinition();
                        columns.Width = GridLength.Auto;
                        network.Grid_panel.ColumnDefinitions.Add(columns);
                    }


                    //   Заполняем поле строками пустых canvas способными принимать Элементы.

                    for (int i = 0; i < network.HEIGHT; i++)
                    {
                        Fill_network_line(network, i);
                    }

                    //******************************************

                    //   Размещаем на поле загруженные из файла Элементы строку за строкой.

                    foreach( Element_Data element in network.ELEMENTS )
                    {   
                        // //Добавляем информацию о созданном Networke, т.к. в файле ее не может быть.
                        // все-таки загружаем при загрузке из файла 
                        //element.Coordinats.NETWORK = network;
                        //---
                        Border eborder = element.Border_Image ;
                        eborder.VerticalAlignment = VerticalAlignment.Stretch;
                            eborder.SetValue(Grid.RowProperty   , element.Coordinats.Y);
                            eborder.SetValue(Grid.ColumnProperty, element.Coordinats.X);
                            network.Grid_panel.Children.Add(eborder);
                        //---

                        // Сразу заполняем копию поля.
                        network.Network_panel_copy[element.Coordinats.Y][element.Coordinats.X] = element ;
                    }


                    Main_grid0.Children.Add(network.Grid_panel);

                network_stack_panel.Children.Add(stack_panel);
                //network_stack_panel.Children.Add(stack_panel2);
                network_stack_panel.Children.Add(Main_grid0) ;

            xborder.Child = network_stack_panel;

            return xborder;

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
            return null;
        }

    }

//***************  NAME VALIDATION
  
public class Label_ValidationRule : ValidationRule
{
    int min, max;

    public Label_ValidationRule() : base()
    {
        min = 0;  max = 8;
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            int ax = ((string)value).Length;

            // Is in range?
            if ((ax < this.min) || (ax > this.max))
            {
                string msg = string.Format("Label length must be between {0} and {1}.", this.min, this.max);
                return new ValidationResult(false, msg);
            }
            else if (ax > 0 && Char.IsDigit(((string)value)[0]))
            {
                string msg = string.Format("Label can't begin from digit.");
                return new ValidationResult(false, msg);
            }
            else if (((string)value).Any(val => {   if ( (val >= 'A' && val <= 'Z') || (val >= 'a' && val <= 'z') ||
                                                         (val >= '0' && val <= '9') || (val <= '_') ) return false;
                                                    else return true; }))
            {
                string msg = string.Format("Label can contain only following symbols:a-z, A-Z, 0-9, '_'.");
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

private void LabelValidationErrorEvent(object sender, ValidationErrorEventArgs e)
{
    if (e.Action == ValidationErrorEventAction.Added)
    {
        MessageBox.Show(e.Error.ErrorContent.ToString());
        //---
        // Сделано по прерыванию Validation.ErrorRemoved т.к. 
        // он не заходит в Validation если ошибка исправлена по Esc, а сюда заходит.
    }
    else if (e.Action == ValidationErrorEventAction.Removed)
    {
        //MessageBox.Show("Error removed!");
    }


    e.Handled = true;
}


//***************  FILL LINE

static public void Fill_network_line(Network_Of_Elements network, int line)
{
    try{
            // Сразу формируем и заполняем нулями копию поля.
            network.Network_panel_copy.Add(new List<Element_Data>());

            for (int j = 0; j < network.Grid_panel.ColumnDefinitions.Count; j++)
            {
                network.Grid_panel.Children.Add(GetFieldCanvas(network, j, line));
                //---
                // Сразу формируем и заполняем нулями копию поля/
                //   растягиваем List чтобы к нему можно было обращаться по индексам.
                network.Network_panel_copy[network.Network_panel_copy.Count-1].Add(null);

            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
        
}


//***************  FILL NEW END EMPTY COLLUMN

static public void Fill_network_collumn(Network_Of_Elements network, int column)
{
    try
    {
        //   Заполняем колонку поля чистыми Canvas с координатами.
        for (int j = 0; j < network.Grid_panel.RowDefinitions.Count; j++)
        {
            network.Grid_panel.Children.Add(GetFieldCanvas(network, column, j));
            //---
            // Сразу формируем и заполняем нулями колонку копии поля.
            //   растягиваем List чтобы к нему можно было обращаться по индексам.
            network.Network_panel_copy[j].Add(null);

        }
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


//***************  INSERT COLUMN

static public string Insert_network_collumn(Network_Of_Elements network, int new_collumn)
{
    try
    {
        int count = network.Grid_panel.ColumnDefinitions.Count;
        
        //  Добавление колонки.        
            if (new_collumn == 0)
            {
                MessageBox.Show("Can't insert collumn in front of 0-collumn!");
                return "Error";
            }

            if (count >= Network_Of_Elements.xMaxWidth)
            {
                MessageBox.Show("Maximum network width!");
                return "Error";
            }
            
            //  добавляем новую колонку в конец network.
            ColumnDefinition collumn = new ColumnDefinition();
            collumn.Width = GridLength.Auto;
            network.Grid_panel.ColumnDefinitions.Add(collumn);

            // Сразу вставляем колонку в копии поля.
            foreach(List<Element_Data> list in network.Network_panel_copy)   list.Insert(new_collumn, null);


            network.WIDTH++;

            //  перемещаем все элементы: изменяем Х-координаты на "+1" всех элементов правее new_collumn.

            List<UIElement> buff_new_uielements = new List<UIElement>();

            foreach (UIElement ui_element in network.Grid_panel.Children)
            {
                if (ui_element != null)
                {
                    int x = (int)ui_element.GetValue(Grid.ColumnProperty);
                    int y = (int)ui_element.GetValue(Grid.RowProperty);

//*************   Переносим элементы на поле.

                    if (x >= new_collumn)
                    {
                        ui_element.SetValue(Grid.ColumnProperty, x + 1);

                        // исправляем координаты элемента хранимые в Tag.
                        if (ui_element.GetType() == typeof(Canvas))
                        {
                            Canvas canvas = (Canvas)ui_element;
                            ((ElementCoordinats)canvas.Tag).X = x + 1;

                            //   изменяем координаты ячейки в уже имеющеемся поле индекса в canvas.
                            ((TextBlock)canvas.Children[0]).Text = (x + 1).ToString() + y.ToString();
                            ((TextBlock)canvas.Children[0]).Visibility = Step5.NetworkCellsVisibility;
                        }
                        else if (ui_element.GetType() == typeof(Border))
                        {
                            Border border = (Border)ui_element;
                            ((Element_Data)border.Tag).Coordinats.X = x + 1;
                        }
                        else MessageBox.Show("QQQ: Insert_network_collumn types error.");
                    }

//*************   Заполняем образровавшуюся пустую колонку.

                    if ( x == new_collumn)
                    {
// на месте канваса ставим снова канвас.
                        if (ui_element.GetType() == typeof(Canvas))
                        {
                            //   нельзя на ходу вставлять элементы в грид.т.к. перестает работать foreach
                            //  накапливаем и вставим потом.
                            //network.Grid_panel.Children.Add(GetFieldCanvas(network, x, y));
                            buff_new_uielements.Add(GetFieldCanvas(network, x, y));
                            //---
                            // Сразу формируем и заполняем нулями колонку копии поля.
                            //   растягиваем List чтобы к нему можно было обращаться по индексам.
                            network.Network_panel_copy[y][x] = null;
                        }
//  на месте другого элемента с левым входом ставим ставим горизонтальный коннектор  
                        // иначе ставим канвас.
                        else if (ui_element.GetType() == typeof(Border))
                        {
                            //   вынимаем текстовую информ. о элементе
                            Border border = (Border)ui_element;

                            //  Есть ли у элемента вход слева?
                            if (((Element_Data)border.Tag).Txt_Image.IO_Directions_list.Exists(value => value == "Left"))
                            {
                                //   создаем коннектор.
                                ElementImage txt_image = ELEMENTS_DICTIONARY.GetElementByName("CONNECTORS", "HORIZONTAL");

                                //  создаем элемент с координатами.
                                ElementCoordinats coordinats = new ElementCoordinats(network, x, y);
                                Element_Data element = new Element_Data(txt_image, coordinats);

                                //   нельзя на ходу вставлять элементы в грид.т.к. перестает работать foreach
                                //  накапливаем и вставим потом.
                                //network.Grid_panel.Children.Add(element.Border_Image);
                                buff_new_uielements.Add(element.Border_Image);
                                //---
                                // Сразу заполняем копию поля.
                                network.Network_panel_copy[y][x] = element;
                            }
                            else
                            {
                                buff_new_uielements.Add(GetFieldCanvas(network, x, y));
                                network.Network_panel_copy[y][x] = null;
                            }
                        }
                    }
                }
            }
        
            //  вставляем накопленные элементы.
            foreach (UIElement element in buff_new_uielements)
            {
               network.Grid_panel.Children.Add(element);
            }

            return null;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return "Error";
    }

}


//***************  INSERT ROW

static public void Insert_network_row(Network_Of_Elements network, int new_row)
{
    try
    {
        int count = network.Grid_panel.RowDefinitions.Count;

        //  Добавление колонки.        

        if (count >= Network_Of_Elements.xMaxHeight)
        {
            MessageBox.Show("Maximum network height!");
            return;
        }

        //  добавляем новую колонку в конец network.
        RowDefinition row = new RowDefinition();
        row.Height = GridLength.Auto;
        network.Grid_panel.RowDefinitions.Add(row);

        // Сразу вставляем и растягиваем ряд копии поля.
        network.Network_panel_copy.Insert(new_row, new List<Element_Data>());

        for (int i = 0; i < network.Network_panel_copy[new_row+1].Count; i++)
        {
            network.Network_panel_copy[new_row].Add(null);
        }

        network.HEIGHT++;

        //  перемещаем все элементы: изменяем Y-координаты на "+1" всех элементов ниже new_row.

        List<UIElement> buff_new_uielements = new List<UIElement>();

        foreach (UIElement ui_element in network.Grid_panel.Children)
        {
            if (ui_element != null)
            {
                int x = (int)ui_element.GetValue(Grid.ColumnProperty);
                int y = (int)ui_element.GetValue(Grid.RowProperty);

                //*************   Переносим элементы на поле.

                if (y >= new_row)
                {
                    ui_element.SetValue(Grid.RowProperty, y + 1);

                    // исправляем координаты элемента хранимые в Tag.
                    if (ui_element.GetType() == typeof(Canvas))
                    {
                        Canvas canvas = (Canvas)ui_element;
                        ((ElementCoordinats)canvas.Tag).Y = y + 1;

                        //   изменяем координаты ячейки в уже имеющеемся поле индекса в canvas.
                        ((TextBlock)canvas.Children[0]).Text = (x).ToString() + (y+1).ToString();
                        ((TextBlock)canvas.Children[0]).Visibility = Step5.NetworkCellsVisibility;
                    }
                    else if (ui_element.GetType() == typeof(Border))
                    {
                        Border border = (Border)ui_element;
                        ((Element_Data)border.Tag).Coordinats.Y = y + 1;
                    }
                    else MessageBox.Show("QQQ: Insert_network_row types error.");
                }

                //*************   Заполняем образровавшуюся пустую колонку.

                if (y == new_row)
                {
                    // на месте канваса ставим снова канвас.
                    if (ui_element.GetType() == typeof(Canvas))
                    {
                        //   нельзя на ходу вставлять элементы в грид.т.к. перестает работать foreach
                        //  накапливаем и вставим потом.
                        //network.Grid_panel.Children.Add(GetFieldCanvas(network, x, y));
                        buff_new_uielements.Add(GetFieldCanvas(network, x, y));
                        //---
                        // Сразу формируем и заполняем нулями колонку копии поля.
                        //   растягиваем List чтобы к нему можно было обращаться по индексам.
                        //  как с колонками здесь не получится: вставлено выше 
                        network.Network_panel_copy[y][x] = null;
                    }
                    //  на месте другого элемента с верхним входом ставим ставим вертикальный коннектор  
                    // иначе ставим канвас.
                    else if (ui_element.GetType() == typeof(Border))
                    {
                        //   вынимаем текстовую информ. о элементе
                        Border border = (Border)ui_element;

                        //  Есть ли у элемента вход сверху?
                        if (((Element_Data)border.Tag).Txt_Image.IO_Directions_list.Exists(value => value == "Up"))
                        {
                            //   создаем коннектор.
                            ElementImage txt_image = ELEMENTS_DICTIONARY.GetElementByName("CONNECTORS", "VERTICAL");

                            //  создаем элемент с координатами.
                            ElementCoordinats coordinats = new ElementCoordinats(network, x, y);
                            Element_Data element = new Element_Data(txt_image, coordinats);

                            //   нельзя на ходу вставлять элементы в грид.т.к. перестает работать foreach
                            //  накапливаем и вставим потом.
                            //network.Grid_panel.Children.Add(element.Border_Image);
                            buff_new_uielements.Add(element.Border_Image);
                            //---
                            // Сразу заполняем копию поля.
                            network.Network_panel_copy[y][x] = element;
                        }
                        else
                        {
                            buff_new_uielements.Add(GetFieldCanvas(network, x, y));
                            network.Network_panel_copy[y][x] = null;
                        }
                    }
                }
            }
        }

        //  вставляем накопленные элементы.
        foreach (UIElement element in buff_new_uielements)
        {
            network.Grid_panel.Children.Add(element);
        }


    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

//***************  GET_FIELD_CANVAS

//  Добавляем к базовому ElementImage.Canvas обработичики номер ячейки и привязываем к Grid

static public Canvas GetFieldCanvas(Network_Of_Elements network, int x, int y)
{
    try
    {
            Canvas canvas = ElementImage.GetCanvas();
        
            // колонку для NEW_LINE делаем узкой, чтобы съекономить место.
            if (x == 0) canvas.MinWidth = ElementImage.xCanvasNewLineWidth;  

            canvas.AllowDrop = true;
            canvas.Drop += canvas_Drop;
            //canvas.DragEnter += canvas_DragEnter;  в нем не работает изменение Drag.Copy на Drag.None
            canvas.DragOver += canvas_DragOver;
            canvas.Tag = new ElementCoordinats(network, x, y);
            canvas.SetValue(Grid.RowProperty, y);
            canvas.SetValue(Grid.ColumnProperty, x);
            //   выводим координаты ячейки в уже имеющееся поле индекса в canvas.
            ((TextBlock)canvas.Children[0]).Text = x.ToString() + y.ToString();
            ((TextBlock)canvas.Children[0]).Visibility = Step5.NetworkCellsVisibility;

        //***************  INSERT ELEMENT CONTEXT MENU
                    
            ContextMenu menu = new ContextMenu();
                        MenuItem menu_item = new MenuItem();
                        menu_item.Header = "Insert element";
                        menu_item.Tag = canvas.Tag;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Menu_Insert_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Copy network";
                        menu_item.Tag = canvas.Tag;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Copy_Network_Click;
                    menu.Items.Add(menu_item);
                        menu_item = new MenuItem();
                        menu_item.Header = "Insert network";
                        menu_item.Tag = canvas.Tag;  // прикрепляем к графическому образу информацию о элементе c координатам.
                        menu_item.Click += Insert_Network_Click;
                    menu.Items.Add(menu_item);

            canvas.ContextMenu = menu;
               
            canvas.VerticalAlignment = VerticalAlignment.Stretch;

            return canvas;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}


//*************  COPY-INSERT ELEMENT  with  PERMISSION CHECK

static void Menu_Insert_Click(object sender, RoutedEventArgs e)
{
    try
    {
        // Копирование по полю уже установленный ранее элемент.
        if (_DRAG_DROP_BUFF.param == "Copy" ) //obj_type == typeof(Border))
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            Border xborder = (Border)(_DRAG_DROP_BUFF.obj);

            ElementImage txt_image = ((Element_Data)xborder.Tag).Txt_Image; 

            //  считываем из Canvas лежащего на этом месте координаты места.
            ElementCoordinats coordinats = (ElementCoordinats)(((MenuItem)sender).Tag);

            if (Check_DropPermission(txt_image, coordinats, null) == true)
            {
                //  считываем из Canvas лежащего на этом месте координаты места.
                CopyDrop_Element((ElementCoordinats)(((MenuItem)sender).Tag));
            }
        }

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

//*************  COPY-INSERT NETWORK

static void Copy_Network_Click(object sender, RoutedEventArgs e)
{
    try
    {
        _DRAG_DROP_BUFF.obj = ((ElementCoordinats)(((MenuItem)sender).Tag)).NETWORK;
        _DRAG_DROP_BUFF.obj_type = typeof(Network_Of_Elements);
        _DRAG_DROP_BUFF.param = "Copy Network";

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}

static void Insert_Network_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (_DRAG_DROP_BUFF.param == "Copy Network")
        {
            //  Network перед которым будет вставка.
            Network_Of_Elements network = ((ElementCoordinats)(((MenuItem)sender).Tag)).NETWORK;
            int index = network.DIAGRAM.NETWORKS_LIST.IndexOf(network);

            //  Network который будет вставляться.
            Network_Of_Elements new_network = ((Network_Of_Elements)_DRAG_DROP_BUFF.obj).GetNetworkCopy();
            new_network.DIAGRAM = network.DIAGRAM;

            network.DIAGRAM.NETWORKS_LIST.Insert(index, new_network);
            network.DIAGRAM.MAIN_PANEL.Children.Insert(index, new_network.Get_NetworkImage());

        }

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


//******************** ADD BUTTONS
   

static public void Add_Buttons( Network_Of_Elements network )
{
    try
    {
        StackPanel stackpanel = new StackPanel();
        stackpanel.Orientation = Orientation.Vertical;
        stackpanel.SetValue(Grid.RowProperty, 0);
        stackpanel.SetValue(Grid.ColumnProperty, 0);

            //   ADD COLUMNs
            ScrollBar scrollbar_h = new ScrollBar();
            scrollbar_h.Visibility = Visibility.Visible;
            scrollbar_h.Orientation = Orientation.Horizontal;
            //scrollbar_h.VerticalAlignment = VerticalAlignment.Top;
            scrollbar_h.Background = new SolidColorBrush(Colors.White);
            scrollbar_h.Margin = new Thickness(0, 0, 0, 1);
            scrollbar_h.Height = 10;
            scrollbar_h.Width = 25;
            scrollbar_h.Value = 0;
            scrollbar_h.Minimum = -100;
            scrollbar_h.Maximum = +100;
            scrollbar_h.ValueChanged += Add_Column_scrollbar_ValueChanged;
            scrollbar_h.Tag = network;

            //   ADD ROWs
            ScrollBar scrollbar = new ScrollBar();
            scrollbar.Visibility = Visibility.Visible;
            scrollbar.Orientation = Orientation.Vertical;
            //scrollbar.VerticalAlignment = VerticalAlignment.Top;
            scrollbar.Background = new SolidColorBrush(Colors.White);
            scrollbar.Margin = new Thickness(0, 1, 0, 1);
            scrollbar.Height = 30;
            scrollbar.Width = 10;
            scrollbar.Value = 0 ;
            scrollbar.Minimum = -100;
            scrollbar.Maximum = +100;
            scrollbar.ValueChanged += Add_Row_scrollbar_ValueChanged;
            scrollbar.Tag = network;

            //   ANALYSE CONNECTIONs
            Button analyse_button = new Button();
            analyse_button.HorizontalAlignment = HorizontalAlignment.Center;
            analyse_button.Content = "A";
            analyse_button.BorderBrush = new SolidColorBrush(Colors.Transparent);
            analyse_button.Background = new SolidColorBrush(Colors.Transparent);
            analyse_button.Tag = network;
            analyse_button.Click += Analyse_button_Click;

            //   COMPILE CONNECTIONs
            //Button compile_button = new Button();
            //compile_button.HorizontalAlignment = HorizontalAlignment.Center;
            //compile_button.Content = "C";
            //compile_button.BorderBrush = new SolidColorBrush(Colors.Transparent);
            //compile_button.Background = new SolidColorBrush(Colors.Transparent);
            //compile_button.Tag = network;
            //compile_button.Click += Compile_button_Click;
        

            //   SHOW GRID LINEs
            Button gridlines_button = new Button();
            gridlines_button.HorizontalAlignment = HorizontalAlignment.Center;
            gridlines_button.Content = "G";
            gridlines_button.BorderBrush = new SolidColorBrush(Colors.Transparent);
            gridlines_button.Background = new SolidColorBrush(Colors.Transparent);
            gridlines_button.Tag = network;
            gridlines_button.Click += ShowGridLine_button_Click;

        stackpanel.Children.Add(scrollbar_h);
        stackpanel.Children.Add(scrollbar);
        stackpanel.Children.Add(analyse_button);
        //stackpanel.Children.Add(compile_button);
        stackpanel.Children.Add(gridlines_button);            

        network.Main_grid0.Children.Add(stackpanel);
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

 }


//******************** CHECK DROP PERMISSION


static public bool Check_DropPermission(ElementImage txt_image, ElementCoordinats coordinats, 
                                                                ElementCoordinats old_coordinats)
{
        int X, Y;
        int X_old, Y_old;

    try
    {

        X = coordinats.X;   
        Y = coordinats.Y;
        
        if ( old_coordinats != null ) 
        { 
            X_old = old_coordinats.X; 
            Y_old = old_coordinats.Y;
        }
        else { X_old = -1; Y_old = -1;}

        List<List<Element_Data>> panel_copy =  coordinats.NETWORK.Network_panel_copy;

//**************  X==0

        //  В нулевом столбце можно ставить только NEW_LINE.
        if (X == 0)
        {
            if (txt_image.Name != "NEW_LINE") return false;
        }

//**************  NEW LINE 

        //  NEW_LINE можно ставить только в нулевом столбце .
        if (txt_image.Name == "NEW_LINE")
        {
            if (X != 0) return false;

            //  Есть ли элемент справа?
            if ( panel_copy[Y][1] == null ) return true;

            //  Есть ли у элемента справа левый вход?
            if (panel_copy[Y][1].Txt_Image.IO_Directions_list.Exists(value => value == "Left")) return true;
            else return false;
        }

//**************  LABEL

        //  LABEL можно ставить только в первом столбце .
        if (txt_image.Name == "LABEL")
        {
            if (X != 1) return false;

            //  Есть ли элемент справа?
            if (panel_copy[Y][2] != null) return false;
            else return true;
        }

//**************  OTHERS && X >= 1

//****  Есть ли у элемента вход слева, справа, сверху, снизу?

        //  Есть ли у элемента вход слева?
        if ( txt_image.IO_Directions_list.Exists( value => value == "Left") )
        {
            //  Есть ли элемент слева?
                                                // Чтобы элемент не мешал сам себе
            if (Y == Y_old && X-1 == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
            else if (panel_copy[Y][X-1] == null) { }
            //  Есть ли у элемента слева правый вход?
            else if (panel_copy[Y][X-1].Txt_Image.IO_Directions_list.Exists(value => value == "Right")) { }
            else return false;
        }
        else  //  Не стоит ли слева элемент с правым выходом?
        {
            //  Есть ли элемент слева?
            if (Y == Y_old && X-1 == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
            else if (panel_copy[Y][X-1] == null) { }
            //  Есть ли у элемента слева правый вход?
            else if (panel_copy[Y][X-1].Txt_Image.IO_Directions_list.Exists(value => value == "Right")) return false;
            else { }
        }

//****  Есть ли у элемента вход справа?

        if (txt_image.IO_Directions_list.Exists(value => value == "Right"))
        {   
            //  Не в конце ли строки находится элемент?
            if (X == panel_copy[Y].Count - 1) return false;

            //  Есть ли элемент справа?
            if (Y == Y_old && X+1 == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
            else if (panel_copy[Y][X+1] == null) { }
            //  Есть ли у элемента справа правый вход?
            else if (panel_copy[Y][X+1].Txt_Image.IO_Directions_list.Exists(value => value == "Left")) {}
            else return false;
        }
        else  //  Не стоит ли справа элемент с левым выходом?
        {
            //  Не в конце ли строки находится элемент?
            if (X != panel_copy[Y].Count - 1)
            {
                //  Есть ли элемент справа?
                if (Y == Y_old && X+1 == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
                else if (panel_copy[Y][X+1] == null) { }
                //  Есть ли у элемента справа левый выход?
                else if (panel_copy[Y][X+1].Txt_Image.IO_Directions_list.Exists(value => value == "Left")) return false;
                else { }
            }
        }


//****  Есть ли у элемента вход сверху?

        if (txt_image.IO_Directions_list.Exists(value => value == "Up"))
        {
            //  Не в самой ли верхней строке находится элемент?
            if (Y == 0) return false;

            //  Есть ли элемент сверху?
            if (Y-1 == Y_old && X == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
            else if (panel_copy[Y-1][X] == null) { }
            //  Есть ли у элемента сверху нижний вход?
            else if (panel_copy[Y-1][X].Txt_Image.IO_Directions_list.Exists(value => value == "Down")) { }
            else return false;
        }
        else  //  Не стоит ли сверху элемент с нижним выходом?
        {
            //  Не в самой ли верхней строке находится элемент?
            if (Y != 0)
            {
                //  Есть ли элемент сверху?
                if (Y-1 == Y_old && X == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
                else if (panel_copy[Y-1][X] == null) { }
                //  Есть ли у элемента справа левый выход?
                else if (panel_copy[Y-1][X].Txt_Image.IO_Directions_list.Exists(value => value == "Down")) return false;
                else { }
            }
        }


//****  Есть ли у элемента вход снизу?

        if (txt_image.IO_Directions_list.Exists(value => value == "Down"))
        {
            //  Не в самой ли нижней строке находится элемент?
            if (Y == panel_copy.Count - 1) return false;

            //  Есть ли элемент снизу?
            if (Y+1 == Y_old && X == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
            else if (panel_copy[Y+1][X] == null) { }
            //  Есть ли у элемента снизу верхний вход?
            else if (panel_copy[Y+1][X].Txt_Image.IO_Directions_list.Exists(value => value == "Up")) { }
            else return false;
        }
        else  //  Не стоит ли снизу элемент с верхним выходом?
        {
            //  Не в самой ли нижней строке находится элемент?
            if (Y != panel_copy.Count - 1)
            {
                //  Есть ли элемент снизу?
                if (Y+1 == Y_old && X == X_old) { } // == null, т.к. после перетаскивания тут будет пусто.
                else if (panel_copy[Y+1][X] == null) { }
                //  Есть ли у элемента снизу верхний вход?
                else if (panel_copy[Y+1][X].Txt_Image.IO_Directions_list.Exists(value => value == "Up")) return false;
                else { }
            }
        }

        //  Ограничений на установку элемента не обнаружено.
        
        return true;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return false;
    }

 }


//******************** CHECK DROP PERMISSION


static public bool Check_InsertDropPermission(ElementImage new_txt_image, ElementImage old_txt_image )
{

    try
    {
        //**************  NEW LINE 

        //  NEW_LINE можно ставить только в нулевом столбце .
        if (old_txt_image.Name == "NEW_LINE")
        {
            return false;
        }

        //   Вставляем элемент только если у вставляемого элемента есть вход и слева и справа 
        // и  у элемента на место которого вставляем есть вход и слева и справа.

        //****  Есть ли у элемента вход слева? //и справа?

        if (    old_txt_image.IO_Directions_list.Exists(value => value == "Left") && 
                old_txt_image.IO_Directions_list.Exists(value => value == "Right") &&
                new_txt_image.IO_Directions_list.Exists(value => value == "Left") //&& 
                // чтобы можно было вставлять элементы перед END_OF_LINE
                //new_txt_image.IO_Directions_list.Exists(value => value == "Right")
            )  return true;

        else return false;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return false;
    }

}


//*******************   CONVERTERS  *******************************


public class NetworkName_binding_Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        try  //  находим индекс текущего item в коллекции.
        {
            Network_Of_Elements network = (Network_Of_Elements)parameter;
            int index = network.DIAGRAM.NETWORKS_LIST.IndexOf(network);
            // на этой стадии в StakPanel еще нет этого элемента int index = network.DIAGRAM.MAIN_PANEL.Children.IndexOf(network.MAIN_BORDER_PANEL);

            network.NAME = "Network: " + index.ToString();
            network.NUM = index ;

            return network.NAME;
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
                MessageBox.Show("Programm: No ConvertBack available.");
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


//***********************    HANDLERS   **************************************


//***************  DELETE NETWORK

static void Delete_Network_button_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Network_Of_Elements network = (Network_Of_Elements)((Button)sender).Tag;
        int count = network.DIAGRAM.NETWORKS_LIST.Count;

        //  Не даем удалить последний оставшийся нетворк - оставляем его на размножение.
        if (count > 1 )
        {
            MessageBoxResult res = MessageBox.Show("Delete?\nYou can't restore it after! ", PROJECT_NAME, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (res == MessageBoxResult.No) return;
            if (res == MessageBoxResult.Yes)
            {
                network.DIAGRAM.NETWORKS_LIST.Remove(network);
                network.DIAGRAM.MAIN_PANEL.Children.Remove(network.MAIN_BORDER_PANEL);
            }
        }

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

//***************  ADD NETWORK

static void Add_Network_scrollbar_ValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e)
{
    try
    {
        Network_Of_Elements network = (Network_Of_Elements)((ScrollBar)sender).Tag;
        int count = network.DIAGRAM.NETWORKS_LIST.Count;
        int index = network.DIAGRAM.NETWORKS_LIST.IndexOf(network);

        if (count >= 2)
        {
            MessageBox.Show("");
            //Network_Of_Elements new_network = new Network_Of_Elements(network.DIAGRAM, "");
            //network.DIAGRAM.MAIN_PANEL.Children.Insert(index + 1, new_network.Get_NetworkImage());
            e.Handled = true;
            return;
        }

        //    Добавление/вставка после Networka.        
        if ( ((ScrollBar)sender).Value > ((Network_Of_Elements)((ScrollBar)sender).Tag).network_scrollbar_value )
        {
            Network_Of_Elements new_network = new Network_Of_Elements(network.DIAGRAM, "Network: ");

            //  если элемент является последним в списке то - добавление, иначе вставка  после него.
            if (index == count - 1)    //  добавление в конец списка.
            {
                network.DIAGRAM.NETWORKS_LIST.Add(new_network);
                network.DIAGRAM.MAIN_PANEL.Children.Add(new_network.Get_NetworkImage());
            }
            else  //  вставка после текущего элемента.
            {
                //  определение индекса текущего network для добавления после него.
                //индекс должен совпадать с уже ычисленным индексом.
                // index.index = network.DIAGRAM.MAIN_PANEL.Children.IndexOf(network.MAIN_BORDER_PANEL);

                network.DIAGRAM.NETWORKS_LIST.Insert(index+1, new_network);
                network.DIAGRAM.MAIN_PANEL.Children.Insert(index + 1, new_network.Get_NetworkImage());
            }
        }
        //    Вставка перед текущим Networkom.  
        else
        {
            Network_Of_Elements new_network = new Network_Of_Elements(network.DIAGRAM, "Network: ");

            network.DIAGRAM.NETWORKS_LIST.Insert(index, new_network);
            network.DIAGRAM.MAIN_PANEL.Children.Insert(index, new_network.Get_NetworkImage());
        }
        //---

        ((Network_Of_Elements)((ScrollBar)sender).Tag).network_scrollbar_value = ((ScrollBar)sender).Value;

        e.Handled = true;
    }
    catch (Exception excp)
    {   
        MessageBox.Show(excp.ToString());
    }
}




//***************  ADD COLUMNs

static void Add_Column_scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
{
    try
    {
        Network_Of_Elements network = (Network_Of_Elements)((ScrollBar)sender).Tag;
        int count = network.Grid_panel.ColumnDefinitions.Count;

        //  Добавление колонки.        
        if (((ScrollBar)sender).Value > ((Network_Of_Elements)((ScrollBar)sender).Tag).scrollbar_h_value)
        {
            //Insert_network_collumn(network, count);  эта функция не может вставить колонку на пустое место.
            if (count < Network_Of_Elements.xMaxWidth)
            {
                //  добавляем новую колонку
                ColumnDefinition column = new ColumnDefinition();
                column.Width = GridLength.Auto;
                network.Grid_panel.ColumnDefinitions.Add(column);

                Fill_network_collumn(network, count);
                network.WIDTH++;
            }
        }
        //  Удаление колонки.
        else
        {
            if (count > Network_Of_Elements.xMinWidth)
            {
                for (int tst = 0; tst == 0; )
                {   //   перед удалением колонки удаляем все элементы из грид привязанные к ней,
                    // т.к. иначе грид запихнет их на ставшую последней колонку.
                    foreach (object obj in network.Grid_panel.Children)
                    {
                        tst = 1;

                        if ((int)((UIElement)obj).GetValue(Grid.ColumnProperty) >= (count - 1))
                        {
                            network.Grid_panel.Children.Remove((UIElement)obj);
                            //  приходится прерывать поиск и начинать его сначала, т.к. 
                            //  коллекция после удаления изменилась и foreach не может 
                            //  продолжать поиск далее.
                            tst = 0;
                            break;
                        }
                    }
                }
                //  удаляем последнюю колонку.
                network.Grid_panel.ColumnDefinitions.RemoveAt(count - 1);
                network.WIDTH--;
                // Сразу удаляем колонку из копии поля.
                foreach( List<Element_Data> elements_list in network.Network_panel_copy)
                {
                    elements_list.RemoveAt(count - 1);
                }
            }
        }
        //---

        ((Network_Of_Elements)((ScrollBar)sender).Tag).scrollbar_h_value = ((ScrollBar)sender).Value;

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


//***************  ADD ROWs

static void Add_Row_scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
{
    try
    {
        Network_Of_Elements network = (Network_Of_Elements)((ScrollBar)sender).Tag;
        int count = network.Grid_panel.RowDefinitions.Count;

        //  Добавление строки.        
        if ( ((ScrollBar)sender).Value > ((Network_Of_Elements)((ScrollBar)sender).Tag).scrollbar_value )
        {
            //  добавляем новый ряд
            if (count < Network_Of_Elements.xMaxHeight)
            {
                RowDefinition rows = new RowDefinition();
                rows.Height = GridLength.Auto;
                network.Grid_panel.RowDefinitions.Add(rows);

                Fill_network_line(network, count);
                network.HEIGHT++;
            }
        }
        //  Удаление строки.
        else
        {
            if (count > Network_Of_Elements.xMinHeight)
            {
                for (int tst = 0; tst == 0; )
                {   //   перед удалением строки удаляем все элементы из грид привязанные к ней,
                    // т.к. иначе грид запихнет их на ставшую последней строку.
                    foreach (object obj in network.Grid_panel.Children)
                    {
                        tst = 1;

                        if ((int)((UIElement)obj).GetValue(Grid.RowProperty) >= (count - 1))
                        {
                            network.Grid_panel.Children.Remove((UIElement)obj);
                            //  приходится прерывать поиск и начинать его сначала, т.к. 
                            //  коллекция после удаления изменилась и foreach не может 
                            //  продолжать поиск далее.
                            tst = 0;
                            break;
                        }
                    }
                }
                //  удаляем последнюю строку.
                network.Grid_panel.RowDefinitions.RemoveAt(count - 1);
                network.HEIGHT--;
                // Сразу удаляем строку из копии поля.
                network.Network_panel_copy.RemoveAt(count - 1);
            }

        }
        //---

        ((Network_Of_Elements)((ScrollBar)sender).Tag).scrollbar_value = ((ScrollBar)sender).Value;

        e.Handled = true;
    }
    catch (Exception excp)
    {        MessageBox.Show(excp.ToString());
    }
}


//***************  SHOW GRID LINEs

static void ShowGridLine_button_Click (object sender, RoutedEventArgs e)
{
    try
    {
        Network_Of_Elements network = (Network_Of_Elements)((Button)sender).Tag;

        //if ( network.Grid_panel.ShowGridLines == false ) network.Grid_panel.ShowGridLines = true ;
        //else network.Grid_panel.ShowGridLines = false;

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}


//*******************  CHECK DROP PERMISSION

static public void canvas_DragOver(object sender, DragEventArgs e)
{
    try
    {            //  В DRAG_DROP_BUFF  мы тащим текстовое описание элемента из библиотеки, а не графический образ. 

        // Перетаскиваем на поле из ToolTree - элемент впервые появляется на поле- оформляем его обработчики.
        if (_DRAG_DROP_BUFF.param == "New" )//typeof(ElementImage))
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            ElementImage txt_image = (ElementImage)(_DRAG_DROP_BUFF.obj);

            //   DragOver над пустым местом или над уже уставновенным элементом?
            if (sender.GetType() == typeof(Canvas))
            {
                //  считываем из Canvas лежащего на этом месте координаты места.
                ElementCoordinats coordinats = (ElementCoordinats)(((Canvas)sender).Tag);
                //---

                if (Check_DropPermission(txt_image, coordinats, null) == true) e.Effects = DragDropEffects.Copy;
                else e.Effects = DragDropEffects.None;
            }
            else if (sender.GetType() == typeof(Border))
            {
                if (Check_InsertDropPermission( ((Element_Data)((Border)sender).Tag).Txt_Image , txt_image) == true) e.Effects = DragDropEffects.Copy;
                else e.Effects = DragDropEffects.None;
            }
            else throw (new Exception());

        }
        // Перетаскиваем по полю уже установленный ранее элемент.
        else if (_DRAG_DROP_BUFF.param == "Move")//typeof(Border))
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            Border xborder = (Border)(_DRAG_DROP_BUFF.obj);

            ElementImage txt_image = ((Element_Data)xborder.Tag).Txt_Image; 

            //   DragOver над пустым местом или над уже уставновенным элементом?
            if (sender.GetType() == typeof(Canvas))
            {
                //  считываем из Canvas лежащего на этом месте координаты места.
                ElementCoordinats coordinats = (ElementCoordinats)(((Canvas)sender).Tag);
                ElementCoordinats old_coordinats = ((Element_Data)xborder.Tag).Coordinats;

                if (Check_DropPermission(txt_image, coordinats, old_coordinats) == true) e.Effects = DragDropEffects.Copy;
                else  e.Effects = DragDropEffects.None; 
            }
            else if (sender.GetType() == typeof(Border))
            {
                if (Check_InsertDropPermission( ((Element_Data)((Border)sender).Tag).Txt_Image , txt_image) == true) e.Effects = DragDropEffects.Copy;
                else e.Effects = DragDropEffects.None;
            }
            else throw (new Exception());

        }
        else throw (new Exception());

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}



static public void canvas_Drop(object sender, DragEventArgs e)
{
    try
    {            
        //  В DRAG_DROP_BUFF  мы тащим текстовое описание элемента из библиотеки, а не графический образ. 


//***********   ЗАЩИТА ОТ ЩЕЛЧКА БЕЗ ПЕРЕТАСКИВАНИЯ 
//       проверка что координаты не изменились - т.е. щелчок на месте.

        if (_DRAG_DROP_BUFF.param == "Move")
        {
            ElementCoordinats new_coord, old_coord ;

            //---  считываем из Canvas лежащего на этом месте координаты места.
            if (sender.GetType() == typeof(Canvas)) new_coord = (ElementCoordinats)(((Canvas)sender).Tag);
            //--- Перетаскиваем по полю уже установленный ранее элемент.
            else if (sender.GetType() == typeof(Border)) new_coord = ((Element_Data)((Border)sender).Tag).Coordinats;
            else new_coord = null;

            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            Border xborder = (Border)(_DRAG_DROP_BUFF.obj);
            old_coord = ((Element_Data)xborder.Tag).Coordinats;

            if (new_coord.NETWORK == old_coord.NETWORK &&
                new_coord.X == old_coord.X &&
                new_coord.Y == old_coord.Y)
            {
                _DRAG_DROP_BUFF.param = null;
                return;
            }
        }

//************


        //--- Перетаскиваем на поле из ToolTree - элемент впервые появляется на поле- оформляем его обработчики.
        if (sender.GetType() == typeof(Canvas))
        {
            //  считываем из Canvas лежащего на этом месте координаты места.
            CopyDrop_Element((ElementCoordinats)(((Canvas)sender).Tag));
        }
        //--- Перетаскиваем по полю уже установленный ранее элемент.
        else if (sender.GetType() == typeof(Border))
        {
            ElementCoordinats coord = ((Element_Data)((Border)sender).Tag).Coordinats;

            //  Создаем "полу-глубокую" копию координат (без копии network) т.к. после вставки колонки 
            //  переменная "Coordinats" изменяется, т.к. это ссылка привязанная к элементу, а элемент сместится.
            ElementCoordinats coordinats = new ElementCoordinats(coord.NETWORK, coord.X, coord.Y);
            
            //  вставляем колонку.
            if (Network_Of_Elements.Insert_network_collumn(coordinats.NETWORK, coordinats.X) == null)
            {
                //   Теперь после вставки колонки на требуемом месте лежит горизонтальный коннектор.

                Element_Data element = coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X];

                // Сразу заполняем копию поля - удаляем коннектор.
                coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X] = null;
                //  Удалаем коннектор элемент с поля.
                coordinats.NETWORK.Grid_panel.Children.Remove(element.Border_Image);

                //  роняем на освободившийся Canvas перетаскиваемый элемент.
                CopyDrop_Element(coordinats);
            }

        }
        else throw (new Exception());


        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    
}


static public void  CopyDrop_Element(ElementCoordinats coordinats, ElementImage new_element_image = null)
{
    try
    {            //  В DRAG_DROP_BUFF  мы тащим текстовое описание элемента из библиотеки, а не графический образ. 

        // Перетаскиваем на поле из ToolTree - элемент впервые появляется на поле- оформляем его обработчики.
        if (_DRAG_DROP_BUFF.param == "New") //typeof(ElementImage))
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            ElementImage txt_image = (ElementImage)(_DRAG_DROP_BUFF.obj);
            
                //  считываем из Canvas лежащего на этом месте координаты места.
                //ElementCoordinats coordinats = (ElementCoordinats)(((Canvas)sender).Tag);

                //  создаем элемент с координатами.
                Element_Data element = new Element_Data(txt_image, coordinats);

            coordinats.NETWORK.Grid_panel.Children.Add(element.Border_Image);
            //---

            // Сразу заполняем копию поля.
            coordinats.NETWORK.Network_panel_copy[element.Coordinats.Y][element.Coordinats.X] = element ;

        }
        // Перетаскиваем по полю уже установленный ранее элемент.
        else if (_DRAG_DROP_BUFF.param == "Move") //typeof(Border))
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            Border xborder = (Border)(_DRAG_DROP_BUFF.obj);

                // Сразу заполняем копию поля - удаляем элемент с прежними координатами c ПРЕЖНЕГО НЕТВОРКА!!!
                // не подходит если перетаскивать элемент между нетворками 
                //coordinats.NETWORK.Network_panel_copy[((Element_Data)xborder.Tag).Coordinats.Y][((Element_Data)xborder.Tag).Coordinats.X] = null;
                ((Element_Data)xborder.Tag).Coordinats.NETWORK.Network_panel_copy[((Element_Data)xborder.Tag).Coordinats.Y][((Element_Data)xborder.Tag).Coordinats.X] = null;
                //  удаляем элемент с поля его прежнего нетворка.
                ((Element_Data)xborder.Tag).Coordinats.NETWORK.Grid_panel.Children.Remove(xborder);

                //---- перетаскивание уже сформированного элемента - обработчик ему не нужен.
                //    изменяем только координаты и удаляем с прежнего места.
               ((Element_Data)xborder.Tag).Coordinats = coordinats; 

                xborder.SetValue(Grid.RowProperty, coordinats.Y);
                xborder.SetValue(Grid.ColumnProperty, coordinats.X);
            
            coordinats.NETWORK.Grid_panel.Children.Add(xborder);
            //---
            // Сразу заполняем копию поля - добавляем элемент с новыми координатами.
            coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X] = (Element_Data)xborder.Tag ;

        }
        // Копирование по полю уже установленный ранее элемент.
        else if (_DRAG_DROP_BUFF.param == "Copy") //typeof(Border))
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            Border xborder = (Border)(_DRAG_DROP_BUFF.obj);
                        
                //((Element_Data)xborder.Tag).Coordinats = coordinats;

                ElementImage txt_image = ((Element_Data)xborder.Tag).Txt_Image;

                //  создаем элемент с координатами.
                Element_Data element = new Element_Data(txt_image, coordinats, ((Element_Data)xborder.Tag).IO_VARs_list);

            coordinats.NETWORK.Grid_panel.Children.Add(element.Border_Image);
            //---

            // Сразу заполняем копию поля.
            coordinats.NETWORK.Network_panel_copy[element.Coordinats.Y][element.Coordinats.X] = element;

        }
        // Замена/обновление на поле старого значка USER_FUNCTION на новый с сохранением привязанных переменных.
        else if (_DRAG_DROP_BUFF.param == "UpDate") 
        {
            //   вынимаем из DRAG_DROP_BUFF текстовую информ. о элементе
            Border xborder = (Border)(_DRAG_DROP_BUFF.obj);

            //---   DELETE

            // Сразу заполняем копию поля - удаляем элемент с прежними координатами
            coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X] = null;
            //  удаляем элемент с поля его прежнего нетворка.
            coordinats.NETWORK.Grid_panel.Children.Remove(xborder);

            //  NEW ELEMENT_DATA с координатами.
            Element_Data element = new Element_Data(new_element_image, coordinats, ((Element_Data)xborder.Tag).IO_VARs_list);
            coordinats.NETWORK.Grid_panel.Children.Add(element.Border_Image);
            //---

            // Сразу заполняем копию поля.
            coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X] = element;

        }
        else throw (new Exception());
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
    
}




//**************   Захват и Перетаскивание уже установленного элемента.

public static void ElementMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
{
    _DRAG_DROP_BUFF.obj = sender;
    _DRAG_DROP_BUFF.obj_type = typeof(Border);
    _DRAG_DROP_BUFF.param = "Move";
    DragDrop.DoDragDrop((Border)sender, "", DragDropEffects.Copy);
    
    e.Handled = true;
}




//*************     NETWORK ANALYSATOR BUTTON

static public void Analyse_button_Click(object sender, RoutedEventArgs e)
{
    try
    {
        ERRORS.Clear();
        ERRORS.Message("\nAnalysing...");

            DiagramAnalysator.Network_Analysator((Network_Of_Elements)((Button)sender).Tag);

        if (ERRORS.Count == 0) ERRORS.Message("\nNo errors.");
        //---
        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

//*************     NETWORK COMPILER BUTTON
/*
static public void Compile_button_Click(object sender, RoutedEventArgs e)
{
    try
    {
        DiagramCompiler.Compiling();

        //---
        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
*/


}  // ******  END of Class Network_Of_Elements

        
    }  // ******  END of Class Step5
}

/*
 
*/
//**********************************
//   PopUp Меню удаления/копирования элемента уже установленного элемента.

/*static void PopUp ElementMenu_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
{
    try
    {
            //Window window = new Window();
            ////window.Title = "Input data";
            //window.Background = new SolidColorBrush( Colors.LightGray);
            //window.SizeToContent = SizeToContent.WidthAndHeight;
            ////COPY_DELETE_window.MinWidth = 500;
            ////COPY_DELETE_window.MinHeight = 100;
            //window.Padding = new Thickness(5);
            ////window.Closing += INPUT_DATA_window_Closing;
            //window.ShowInTaskbar = false;
            //COPY_DELETE_window.WindowStartupLocation = WindowStartupLocation.CenterScreen;//Manual;//CenterOwner;
            ////COPY_DELETE_window.Left = Mouse.GetPosition((Border)sender).X;
            ////COPY_DELETE_window.Top = Mouse.GetPosition((Border)sender).Y;
            //window.ResizeMode = ResizeMode.NoResize;
            //window.WindowStyle = WindowStyle.None;// ToolWindow;

        COPY_DELETE_popup = new Popup();
        COPY_DELETE_popup.Placement = PlacementMode.MousePoint;
        COPY_DELETE_popup.StaysOpen = false;
        //см. хендлер COPY_DELETE_popup.Opened += COPY_DELETE_popup_Opened;
        //   так и не смог добиться, чтобы кнопки срабатывали по нажатию Enter или Esc.
        //COPY_DELETE_popup.Focusable = true;
        //COPY_DELETE_popup.Focus();
        ////FocusManager.SetFocusedElement(COPY_DELETE_popup, button2);
        //COPY_DELETE_popup.PopupAnimation = PopupAnimation.Fade;
        //COPY_DELETE_popup.AllowsTransparency = true;
        //COPY_DELETE_popup.IsEnabled = true;

            Button button = new Button();
        
                StackPanel stackpanel = new StackPanel();
                stackpanel.Orientation = Orientation.Vertical ;//Horizontal;
                stackpanel.Background = new SolidColorBrush( Colors.LightGray); //HorizontalAlignment.Center;
            
                    Button button1 = new Button();
                    button1.Content = "Copy";
                    //button1.IsCancel = true;
                    button1.IsEnabled = false;
                    //button1.Background = new SolidColorBrush(Colors.White);
                    button1.Margin = new Thickness(5);
                    button1.Click +=Copy_Click;  
                    button1.MinWidth = 75;
    
                    Button button2 = new Button();
                    button2.Name = "Delete";
                    button2.Content = "Delete";
                    button2.IsDefault = true;
                    //button2.IsCancel = true;
                    //button2.Background = new SolidColorBrush(Colors.White);
                    button2.Margin = new Thickness(5);
                    button2.Click += Delete_Click;
                    button2.MinWidth = 75;
                    button2.Tag = (Border)sender;
                    //button2.VerticalAlignment = VerticalAlignment.Bottom;
                    //button2.HorizontalAlignment = HorizontalAlignment.Stretch;

                    //FocusManager.SetFocusedElement(COPY_DELETE_popup, button2);
                    //FocusManager.SetFocusedElement(INPUT_DATA_window, button2);    

                    Button button3 = new Button();
                    button3.Content = "Cancel";
                    //button3.IsDefault = true;
                    button3.IsCancel = true;
                    //button3.IsEnabled = false;
                    //button3.FontSize = MARK_font_size;
                    //button3.Background = new SolidColorBrush(Colors.White);
                    button3.Margin = new Thickness(5);
                    button3.Click += Cancel_Click;
                    button3.MinWidth = 75;
                    //button3.VerticalAlignment = VerticalAlignment.Bottom;
                    //button3.HorizontalAlignment = HorizontalAlignment.Stretch;

                stackpanel.Children.Add(button1);
                stackpanel.Children.Add(button2);
                stackpanel.Children.Add(button3);

            //window.Content = stackpanel;
            ////window.CaptureMouse();
            //window.ShowDialog();

            button.Content = stackpanel;

        COPY_DELETE_popup.Child = button;// stackpanel;
        COPY_DELETE_popup.IsOpen = true; 


        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

/*static void COPY_DELETE_popup_Opened(object sender, EventArgs e)
{
    try
    {
        Popup p = (Popup)sender;
   Не находит "Delete"     Button button = (Button)((StackPanel)((Button)p.Child).Content).FindName("Delete");
        
        button.Focus();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
* /
            // Copy element
static void Copy_Click(object sender, RoutedEventArgs e)
{
    e.Handled = true;
}
 
             // Delete element
static void Delete_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Border xborder = (Border)((Button)sender).Tag;
        ElementCoordinats coordinats = ((Element_Data)xborder.Tag).Coordinats;

        coordinats.NETWORK.Grid_panel.Children.Remove(xborder);

        // Сразу заполняем копию поля - удаляем элемент с прежними координатами.
        coordinats.NETWORK.Network_panel_copy[coordinats.Y][coordinats.X] = null;

        //((Window)COPY_DELETE_popup.Child).Close();
        COPY_DELETE_popup.IsOpen = false;

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}
            // Cancel
static void Cancel_Click(object sender, RoutedEventArgs e)
{
    try
    {
        //((Window)COPY_DELETE_popup.Child).Close();
        COPY_DELETE_popup.IsOpen = false;

        e.Handled = true;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}
 
 
 */
