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
        static public  TreeView TOOLS_TREE_VIEW = null;
        static public  TabItem  TOOLS_TAB_ITEM = null;
        //static ElementImage DRAG_DROP_BUFF;

        static  ElementImage.Object_Data  _DRAG_DROP_BUFF = new ElementImage.Object_Data(null, null);


static public class DictionaryTree
        {

            //static double xFontSize = 12;

            //delegate void Dlg_MouseDown(object sender, MouseButtonEventArgs e);

static public void SHOW_TREE(TabControl tree_tab_control_panel)
{
    try
    {
        TabItem tabitem = new TabItem();
            TOOLS_TAB_ITEM = tabitem;
        tabitem.Header = "Tools";
        
            ScrollViewer scroll = new ScrollViewer();
            scroll.HorizontalAlignment = HorizontalAlignment.Stretch;
            scroll.VerticalAlignment   = VerticalAlignment.Stretch;

                StackPanel tree_stack_panel = new StackPanel();
                tree_stack_panel.MinWidth = 200 ;
                tree_stack_panel.MinHeight= 350 ;
                //tree_stack_panel.Background = new SolidColorBrush(Colors.White);

//после каждого элемента добавлять одну пустую клетку с коннектором или без чтобы было куда вставить следующий элемент

                    TOOLS_TREE_VIEW = new TreeView();
                    TOOLS_TREE_VIEW.HorizontalAlignment = HorizontalAlignment.Left;
                    TOOLS_TREE_VIEW.HorizontalContentAlignment = HorizontalAlignment.Left;
                    TOOLS_TREE_VIEW.VerticalAlignment = VerticalAlignment.Top;
                    TOOLS_TREE_VIEW.VerticalContentAlignment = VerticalAlignment.Center;
                    TOOLS_TREE_VIEW.Background = new SolidColorBrush(Colors.Transparent);
                    TOOLS_TREE_VIEW.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    TOOLS_TREE_VIEW.Margin = new Thickness(0, 5, 10, 5);

                    TOOLS_TREE_VIEW.Resources[SystemColors.HighlightBrushKey] = new SolidColorBrush(Colors.AliceBlue);
                    TOOLS_TREE_VIEW.Resources[SystemColors.HighlightTextBrushKey] = new SolidColorBrush(Colors.Black);

                    //tree_view.MouseDown += tree_view_MouseDown;

                    TOOLS_TREE_VIEW.ItemsSource = ELEMENTS_DICTIONARY.CATEGORYs;//.Values;

                        HierarchicalDataTemplate level1_template = new HierarchicalDataTemplate();

                        //FrameworkElementFactory border_lev1 = new FrameworkElementFactory(typeof(Button));
                        //border_lev1.SetValue(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
                        //border_lev1.SetValue(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent));
                        //border_lev1.SetValue(Button.MarginProperty, new Thickness(1));
                        ////border_lev1.SetValue(Button.IsEnabledProperty, false);
                        ///border_lev1.AddHandler( Button.ClickEvent, butt_Click);
                    
                            FrameworkElementFactory textblock = new FrameworkElementFactory(typeof(TextBlock));
                            textblock.SetValue(TextBlock.MinWidthProperty, 100.0);
                            textblock.SetValue(TextBlock.MarginProperty, new Thickness(0, 3, 0, 3));
                                Binding textblock_binding = new Binding();
                                textblock_binding.Path = new PropertyPath("NAME");
                            textblock.SetBinding(TextBlock.TextProperty, textblock_binding);


                            Binding items_lev1_binding = new Binding();
                            items_lev1_binding.Path = new PropertyPath("ELEMENTs");
                            //  В конвертере отбрасываем от Dictionary ключи и получаем Values-чистый List елементов.
                            //items_lev1_binding.Converter = new Items_lev1_binding_Converter();
                        level1_template.ItemsSource = items_lev1_binding;

                        //********************

                            HierarchicalDataTemplate level2_template = new HierarchicalDataTemplate();

                            //FrameworkElementFactory border_lev2 = new FrameworkElementFactory(typeof(Button));
                            //border_lev2.SetValue(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
                            //border_lev2.SetValue(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent));
                            //border_lev2.SetValue(Button.MarginProperty, new Thickness(1));

                                FrameworkElementFactory stack_panel_lev2 = new FrameworkElementFactory(typeof(StackPanel));                    
                                stack_panel_lev2.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                                stack_panel_lev2.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                                stack_panel_lev2.SetValue(StackPanel.VerticalAlignmentProperty, VerticalAlignment.Stretch);

                                    FrameworkElementFactory textblock_lev2 = new FrameworkElementFactory(typeof(TextBlock));
                                    textblock_lev2.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    textblock_lev2.SetValue(TextBlock.MinWidthProperty, 100.0);
                                        Binding textblock_lev2_binding = new Binding();
                                        textblock_lev2_binding.Path = new PropertyPath("Name");
                                    textblock_lev2.SetBinding(TextBlock.TextProperty, textblock_lev2_binding);
                                    //  К Tag привязываем весь ElementImage.
                                        Binding textblock_tag_lev2_binding = new Binding();
                                    textblock_lev2.SetBinding(TextBlock.TagProperty, textblock_tag_lev2_binding);
                                    textblock_lev2.AddHandler(TextBlock.MouseDownEvent, new MouseButtonEventHandler(tree_view_MouseDown1));

                                    FrameworkElementFactory icon_lev2 = new FrameworkElementFactory(typeof(Label));
                                    //icon_lev2.SetValue(Label.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
                                    //icon_lev2.SetValue(Label.BorderBrushProperty, new SolidColorBrush(Colors.Black));
                                    //icon_lev2.SetValue(Label.BorderThicknessProperty, new Thickness(0.5));
                                    icon_lev2.SetValue(Label.PaddingProperty, new Thickness(1));
                                    icon_lev2.SetValue(Label.VerticalContentAlignmentProperty, VerticalAlignment.Center);
                                    //icon_lev2.SetValue(Label.HorizontalContentAlignmentProperty, HorizontalAlignment.Right);
                                    icon_lev2.SetValue(Label.MaxWidthProperty, 40.0);
                                    icon_lev2.SetValue(Label.MaxHeightProperty, 40.0);
                                    Binding icon_lev2_binding = new Binding();
                                        icon_lev2_binding.Path = new PropertyPath("Icon");
                                    icon_lev2.SetBinding(Label.ContentProperty, icon_lev2_binding);
                                    //  К Tag привязываем весь ElementImage.
                                    Binding icon_tag_lev2_binding = new Binding();
                                    icon_lev2.SetBinding(Label.TagProperty, icon_tag_lev2_binding);
                                    icon_lev2.AddHandler(Label.MouseDownEvent, new MouseButtonEventHandler(tree_view_MouseDown2));
  //Dlg_MouseDown dlg = tree_view_MouseDown;
  // Button suka перехватывает левую кнопку мыши button_lev2.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(tree_view_MouseDown));
//Может надо привязывать событие не к баттон а тому что на него выведено Canvas?
                                stack_panel_lev2.AppendChild(textblock_lev2);
                                stack_panel_lev2.AppendChild(icon_lev2);

                            //border_lev2.AppendChild(stack_panel_lev2);

                            level2_template.VisualTree = stack_panel_lev2;

                        //********************

                        level1_template.ItemTemplate = level2_template;

                        //border_lev1.AppendChild(textblock);

                        level1_template.VisualTree = textblock;

                    TOOLS_TREE_VIEW.ItemTemplate = level1_template;

                tree_stack_panel.Children.Add(TOOLS_TREE_VIEW);

            scroll.Content = tree_stack_panel;

            tabitem.Content = scroll;

        tree_tab_control_panel.Items.Add(tabitem);
            
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        //return null;
    }

}
    //**************************************

static void tree_view_MouseDown1(object sender, MouseButtonEventArgs e)
            {
                ElementImage element = (ElementImage)(((TextBlock)sender).Tag);
              //  MessageBox.Show( element.Name);
                _DRAG_DROP_BUFF.obj = element;
                _DRAG_DROP_BUFF.obj_type = typeof(ElementImage);
                _DRAG_DROP_BUFF.param = "New";
                DragDrop.DoDragDrop((TextBlock)sender, "", DragDropEffects.Copy);

                // не дает TreeView выделять мышкой выбранные items. e.Handled = true;
            }

static void tree_view_MouseDown2(object sender, MouseButtonEventArgs e)
            {
                ElementImage element = (ElementImage)(((Label)sender).Tag);
              //  MessageBox.Show( element.Name);
                _DRAG_DROP_BUFF.obj = element;
                _DRAG_DROP_BUFF.obj_type = typeof(ElementImage);
                _DRAG_DROP_BUFF.param = "New";
                DragDrop.DoDragDrop((Label)sender, "", DragDropEffects.Copy);

                // не дает TreeView выделять мышкой выбранные items. e.Handled = true;
            }

    //*************************

public class Items_lev1_binding_Converter : IValueConverter
{
    public object Convert(object value, Type targetType,
              object parametr, System.Globalization.CultureInfo culture)
    {
        try  //  В конвертере отбрасываем от Dictionary ключи и получаем чистый List елементов.
        {
            return ((Dictionary<string, ElementImage>)value).Values;
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


        }  //*************    END of Class <DictionaryTree>

    }  //*************    END of Class <Step5>
}


/*
 *                  TreeView tree_view = new TreeView() ;
                    tree_view.HorizontalAlignment = HorizontalAlignment.Left;
                    tree_view.HorizontalContentAlignment = HorizontalAlignment.Left;
                    tree_view.VerticalAlignment = VerticalAlignment.Top;
                    tree_view.VerticalContentAlignment = VerticalAlignment.Center;
                    tree_view.Background = new SolidColorBrush(Colors.Transparent);
                    tree_view.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    //tree_view.ItemsSource
                    //tree_view.ItemTemplate
                    //tree_view.SetBinding
                    //tree_view.VerticalAlignment
                    //tree_view.VerticalContentAlignment

                    foreach(ELEMENTS_DICTIONARY.Category category in ELEMENTS_DICTIONARY.CATEGORY.Values)
                    {
                        TreeViewItem item1 = new TreeViewItem();
                        item1.Header = category.Name;

                            foreach (ElementFullImage element in category.Element.Values)
                            {
                                TreeViewItem item2 = new TreeViewItem();

                                Button xborder = new Button();
                                xborder.Background = new SolidColorBrush(Colors.Transparent);
                                xborder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
                                xborder.BorderThickness = new Thickness(0.5);
                                xborder.HorizontalAlignment = HorizontalAlignment.Left;
                                xborder.VerticalAlignment = VerticalAlignment.Stretch;
                                xborder.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                                xborder.VerticalContentAlignment = VerticalAlignment.Stretch;
                                //xborder.Margin = new Thickness(5);

                                    StackPanel stack_panel = new StackPanel();
                                    stack_panel.Orientation = Orientation.Horizontal;
                                    stack_panel.HorizontalAlignment = HorizontalAlignment.Left;
                                    stack_panel.VerticalAlignment = VerticalAlignment.Center;

                                        TextBlock textblock = new TextBlock();
                                        textblock.Text = element.Name;
                                        textblock.VerticalAlignment = VerticalAlignment.Center;
                                        textblock.Margin = new Thickness(0,0,5,0);

                                    stack_panel.Children.Add(textblock);
                                    stack_panel.Children.Add(element.Icon);

                                xborder.Content = stack_panel;

                                item2.Header = xborder;
                                item2.ToolTip = "Здесь будет подсказка";
                                item1.Items.Add(item2);
                            }

                        tree_view.Items.Add(item1);
                    }
                     
                    //tree_view.ItemsSource = ELEMENTS_DICTIONARY.CATEGORY ;
                      //  Binding item_binding = new Binding();
                        //item_binding.Path    = new PropertyPath( "CategoryName" );


                        //tree_view.SetBinding(TreeView.TreeView ,item_binding);

                    tree_stack_panel.Children.Add(tree_view);
                                        

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    //            return null;
                }

*/