//  доделать для остальных команд
//  убрать в diagramfiel show gridlines.
//  добавить в ShowNetwork show gridlines.
//  добавить в файл данных View поле для show gridlines.


// + найти куда привязать события по F7 и др. чтобы срабатывали из всех точек.

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


        //*********************************************
class MainMenu
{
    public MainMenu()
    {
                //Application.Current.MainWindow.Resources. 
        try
        {     
       
//************   COMMAND BINDINGS & KEY_GESTURES

                CommandBindings_KeyGestures();

//************   KEYBOARD HANDLER

                Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;

//************   MAIN MENU

                    MenuItem menu_item = new MenuItem();
                    menu_item.Header = "File    ";
                    menu_item.MinWidth = 100;

                        MenuItem menu2_item = new MenuItem();
                        menu2_item.Header = "Recent";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += RecentProject_item_Click;
                        menu2_item.Tag = RECENT_PROJECTS.List;
                            Binding recent_files_binding = new Binding();
                            recent_files_binding.Source = Step5.RECENT_PROJECTS;
                            recent_files_binding.Path = new PropertyPath("List");
                            // почему то не вызывается конвертер при изменении Step5.RECENT_PROJECTS.List,
                            //  пришлось убрать конвертер и ввести ToString() в элементы массива.
                            //recent_files_binding.Converter = new Items_source_Converter();
                            BindingOperations.SetBinding(menu2_item, MenuItem.ItemsSourceProperty, recent_files_binding);
                    menu_item.Items.Add(menu2_item);
                
                        menu2_item = new MenuItem();
                        menu2_item.Header = "New template";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += NewProject_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "New empty";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += NewEmptyProject_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Open";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += OpenProject_item_Click;
                    menu_item.Items.Add(menu2_item);

//*******   SAVE
                        menu2_item = new MenuItem();
                        menu2_item.MinWidth = 100;
                            menu2_item.Command = ApplicationCommands.Save;
                            menu2_item.CommandParameter = "Save";
                    menu_item.Items.Add(menu2_item);

//*******   SAVE AS
                        menu2_item = new MenuItem();
                        menu2_item.MinWidth = 100;
                            menu2_item.Command = ApplicationCommands.SaveAs;
                            menu2_item.CommandParameter = "Save as";
                    menu_item.Items.Add(menu2_item);

//*******   SAVE AS TEMPLATE
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Save as template";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += SaveAsTemplate_item_Click;
                    menu_item.Items.Add(menu2_item);

//*******   SAVE AS XPS
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Save  As  Xps";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += SaveProjectAsXPS_item_Click;
                    menu_item.Items.Add(menu2_item);

                MENU_PANEL.Items.Add(menu_item);

                    menu_item = new MenuItem();
                    menu_item.Header = "View   ";
                    menu_item.MinWidth = 100;
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Gridlines  Ctl+G";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += GridLines_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Cellnumbers  Ctl+N";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += CellNumbers_item_Click;
                    menu_item.Items.Add(menu2_item);

                MENU_PANEL.Items.Add(menu_item);

                    menu_item = new MenuItem();
                    menu_item.Header = "Symbols";
                    menu_item.MinWidth = 100;
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Global symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Global_symbols_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Base symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Base_symbols_item_Click;
                    menu_item.Items.Add(menu2_item);
                    //    menu2_item = new MenuItem();
                    //    menu2_item.Header = "Base symbols tags";
                    //    menu2_item.MinWidth = 100;
                    //    menu2_item.Click += Base_symbols_tags_item_Click;
                    //menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "SetPoint symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Storable_symbols_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Msg symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += MSG_symbols_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Local symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Local_symbols_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Noms symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Symbols_noms_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Indication symbols";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Indication_symbols_item_Click;
                    menu_item.Items.Add(menu2_item);

                MENU_PANEL.Items.Add(menu_item);


                    menu_item = new MenuItem();
                    menu_item.Header = "Build   ";
                    menu_item.MinWidth = 100;
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Compile    F7";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Build_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Output List";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += ShowCodeList_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Download CPU   F8";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Programmer_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Debug            ";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Debug_item_Click;
                    menu_item.Items.Add(menu2_item);

                MENU_PANEL.Items.Add(menu_item);

                    menu_item = new MenuItem();
                    menu_item.Header = "Pcc.ini";
                    menu_item.MinWidth = 100;
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Load Base Symbols Tags";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += Base_symbols_tags_item_Click;
                    menu_item.Items.Add(menu2_item);
                        menu2_item = new MenuItem();
                        menu2_item.Header = "Upgrade Base Functions";
                        menu2_item.MinWidth = 100;
                        menu2_item.Click += UpgradePccIniBaseFunctions_item_Click;
                    menu_item.Items.Add(menu2_item);


                MENU_PANEL.Items.Add(menu_item);


        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
      
    }



    void CommandBindings_KeyGestures()
    {
        try
        {
                KeyGesture key_gesture ;
                KeyBinding key_binding;

//***********   SAVE_AS COMMAND

                key_gesture = new KeyGesture(Key.D, ModifierKeys.Control, "Ctrl+D");
                //--- Чтобы с KeyGesture передавался параметр в команду, иначе передается нуль.
                key_binding = new KeyBinding(ApplicationCommands.SaveAs, key_gesture) { CommandParameter = "Save as" };
            Application.Current.MainWindow.InputBindings.Add(key_binding);
                //---  Чтобы в Меню рядом с названием команды рисовалась надпись "Ctrl+D", иначе не рисуется если у команды нет стандартного KeyGesture
            ApplicationCommands.SaveAs.InputGestures.Add(key_gesture);
                //---   Изменение стандартного текста с русского на английский.
            ApplicationCommands.SaveAs.Text = "Save As...";
                //---
                CommandBinding command_binding = new CommandBinding(ApplicationCommands.SaveAs,
                                                 new ExecutedRoutedEventHandler(SaveProject.com_SAVE_PROJECT_ExecutedHandler),
                                                 new CanExecuteRoutedEventHandler(com_AlwaysCanExecuteHandler));
            Application.Current.MainWindow.CommandBindings.Add(command_binding);

//***********   SAVE COMMAND

                key_gesture = new KeyGesture(Key.S, ModifierKeys.Control);//, "Ctrl+S");
                //--- Чтобы с KeyGesture передавался параметр в команду, иначе передается нуль.
                key_binding = new KeyBinding(ApplicationCommands.Save, key_gesture) { CommandParameter = "Save" };
            Application.Current.MainWindow.InputBindings.Add(key_binding);
                //---   Изменение стандартного текста с русского на английский.
            ApplicationCommands.Save.Text = "Save";
                //---
                command_binding = new CommandBinding(ApplicationCommands.Save,
                                  new ExecutedRoutedEventHandler(SaveProject.com_SAVE_PROJECT_ExecutedHandler),
                                  new CanExecuteRoutedEventHandler(com_AlwaysCanExecuteHandler));
            Application.Current.MainWindow.CommandBindings.Add(command_binding);

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }

    }

    static public void com_AlwaysCanExecuteHandler(object obj, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;

        e.Handled = true;
    }

//*************  HANDLERS

//*************  RECENT_PROJECTS

            void RecentProject_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    //  Поиск нажатого item.

                    MenuItem menuitem = (MenuItem)sender;

                    string selected_item_string = ((MenuItem)e.OriginalSource).Header.ToString();

                    if( Step5.RECENT_PROJECTS.List.Any(value=>(value.ToString() == selected_item_string)))
                    {
                        RECENT_PROJECT_Data recent_data = Step5.RECENT_PROJECTS.List.First(value=>(value.ToString() == selected_item_string));
                        SaveProject.OPEN_PROJECT("Recent", recent_data);
                    }

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

            }


            public class Items_source_Converter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                {
                    try  //  находим индекс текущего item в коллекции.
                    {
                        ObservableCollection<string> list = new ObservableCollection<string>();

                        foreach(Step5.RECENT_PROJECT_Data project in (ObservableCollection<RECENT_PROJECT_Data>)value)//Step5.RECENT_PROJECTS.List)
                        {
                            list.Add(project.Name + "  [" + project.SaveDate + ", " + project.Directory_Path + "]");
                        }

                        return list;
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


//*************  NEW_TEMPLATE_PROJECT

            void NewProject_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject.NEW_PROJECT("Load template");

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  NEW_EMPTY_PROJECT
    
            void NewEmptyProject_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject.NEW_PROJECT("Load empty");

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }


//*************  OPEN_PROJECT

            void OpenProject_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject.OPEN_PROJECT("Open");

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }


//*************  SAVE_PROJECT

/*            void SaveProject_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject.SAVE_PROJECT("Save");

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  SAVE_PROJECT AS

            void SaveProjectAs_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject.SAVE_PROJECT("Save as");

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }
*/

//*************  SAVE_PROJECT_To_XPS

            void  SaveAsTemplate_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject._SAVE_PROJECT("Save as template");

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  SAVE_PROJECT_To_XPS

            void SaveProjectAsXPS_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    Save_XPS.SAVE_XPS_PROJECT();

                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  BUILD

            void Build_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    Step5.DiagramCompiler.Compiling();
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  SHOW CODE LIST

            void ShowCodeList_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    DiagramCompiler.SHOW_CODE_WINDOW();
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  CPU_PROGRAMMER

            void Programmer_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    Step5.DiagramCompiler.RUN_CPU_PROGRAMMER();
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  DEBUG

            void Debug_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    Step5.DEBUG.SHOW_DEBUG_window("Show");
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }


//*************  Upgrade Pcc.ini Base Functions

            void UpgradePccIniBaseFunctions_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveProject.UpgradePccIniBaseFunctions_Window();
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  GLOBAL SYMBOLS

            void Global_symbols_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.GLOBAL_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  BASE SYMBOLS

            void Base_symbols_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.BASE_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  BASE SYMBOLS TAGS

            void Base_symbols_tags_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.BASE_TAGS_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  STORABLE SYMBOLS

            void Storable_symbols_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.STORABLE_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  MSG SYMBOLS

            void MSG_symbols_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.MSG_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  LOCAL SYMBOLS

            void Local_symbols_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", ACTIV_DIAGRAM.LOCAL_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }


//*************  SYMBOLS NOMS

            void Symbols_noms_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.NOMS_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  INDICATION SYMBOLS

            void Indication_symbols_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SYMBOLS_LIST.SHOW_SYMBOLS_LIST_window("ShowDialog", SYMBOLS.INDICATION_SYMBOLS_LIST);
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  GRID LINES

            void GridLines_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    Toggle_NetworkGridLines();
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//*************  GRID LINES

            void CellNumbers_item_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    Toggle_NetworkCellsVisibility();
                    //---
                    e.Handled = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }

//************  KEYBOARD

            void MainWindow_KeyUp(object sender, KeyEventArgs e)
            {
                try  
                {
                    if (e.Key == Key.F7) Step5.DiagramCompiler.Compiling();

                    //  !!! - это событие на отпускание клавиши!.
                    //     т.е. для Ctrl+S возникнет когда отпустят клавишу 'S'.

                    //if (e.Key == Key.S && ( Keyboard.IsKeyDown(Key.LeftCtrl) ||
                      //                      Keyboard.IsKeyDown(Key.RightCtrl))) SaveProject.SAVE_PROJECT("Save");
                    //---

                    if (e.Key == Key.O && ( Keyboard.IsKeyDown(Key.LeftCtrl) ||
                                            Keyboard.IsKeyDown(Key.RightCtrl))) SaveProject.OPEN_PROJECT("Open");
                    //---

                    if (e.Key == Key.G && (Keyboard.IsKeyDown(Key.LeftCtrl) ||
                                            Keyboard.IsKeyDown(Key.RightCtrl))) Toggle_NetworkGridLines();
                    e.Handled = true;
                    //---

                    if (e.Key == Key.N && (Keyboard.IsKeyDown(Key.LeftCtrl) ||
                                            Keyboard.IsKeyDown(Key.RightCtrl))) Toggle_NetworkCellsVisibility();

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }
            }


//**************   Toggle_NetworkGridLines

static bool Toggle_NetworkGridLines()
{
    try
    {

        if (Step5.ShowNetworkGridLines == false) Step5.ShowNetworkGridLines = true;
        else Step5.ShowNetworkGridLines = false;

        foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        {
            foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
            {
                network.Grid_panel.ShowGridLines = Step5.ShowNetworkGridLines;
            }
        }
        return Step5.ShowNetworkGridLines ;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return false;
    }
    
}

//**************   Toggle_NetworkCellsVisibility

static bool Toggle_NetworkCellsVisibility()
{
    try
    {

        if (Step5.NetworkCellsVisibility == Visibility.Visible) Step5.NetworkCellsVisibility = Visibility.Hidden;
        else Step5.NetworkCellsVisibility = Visibility.Visible;

        //if (ElementImage.NetworkCellNumbersVisibility == Visibility.Visible) ElementImage.NetworkCellNumbersVisibility = Visibility.Hidden;
        //else ElementImage.NetworkCellNumbersVisibility = Visibility.Visible;

        //  Для всех Canvas во всех диагрем и всех нетворках изменяем Visibility.
        foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
        {
            foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
            {
                foreach (object obj in network.Grid_panel.Children)
                {
                    Canvas canvas = obj as Canvas;
                    if ( canvas != null )((TextBlock)canvas.Children[0]).Visibility = Step5.NetworkCellsVisibility;
                }
            }
        }
        return Step5.ShowNetworkGridLines;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return false;
    }

}

//*************
        }
    }
}