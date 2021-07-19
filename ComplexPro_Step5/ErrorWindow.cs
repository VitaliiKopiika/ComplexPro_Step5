
//Найти как активировать на передний план вкладку Error
//Работа с файлами.

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

        static public TabItem ERRORS_TAB_ITEM = null;

public class ERRORS
{

    static public RichTextBox ERROR_TEXT_BOX;

    static public List<string> NETWORKS_ERROR_LIST = new List<string>();

    public static int Count {get { return NETWORKS_ERROR_LIST.Count ;}}        

static public void Clear()  
{
    try
    {
        NETWORKS_ERROR_LIST.Clear();
// Теперь это окно - это Лог-файл а не окно только компилятора
//  и очищать его не надо   Clear_Window();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());

    }
}

new static public string ToString()
{
    try
    {
        StringBuilder sstr = new StringBuilder();
        foreach (string str in NETWORKS_ERROR_LIST) sstr.Append(str + "\n");

        return sstr.ToString();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}


static public void Show_Window(TabControl error_tab_control_panel)
{
    try
    {
        TabItem tabitem = new TabItem();
            ERRORS_TAB_ITEM = tabitem;
        tabitem.Header = "Log";
        
            ScrollViewer scroll = new ScrollViewer();
            scroll.HorizontalAlignment = HorizontalAlignment.Stretch;
            scroll.VerticalAlignment   = VerticalAlignment.Stretch;

                StackPanel errors_stack_panel = new StackPanel();
                errors_stack_panel.MinWidth = 200 ;
                errors_stack_panel.MinHeight= 350 ;
        

            RichTextBox error_box =  new RichTextBox();
            ERROR_TEXT_BOX = error_box ;

                error_box.MinWidth = 200;
                error_box.MinHeight = 700;
                error_box.MaxHeight = 700;
                error_box.HorizontalAlignment = HorizontalAlignment.Stretch;
                error_box.VerticalAlignment = VerticalAlignment.Stretch;
                //error_box.AppendText("ERROR_TEXT_BOX");
                error_box.IsReadOnly = true;
            
            errors_stack_panel.Children.Add(error_box);

            scroll.Content = errors_stack_panel;

            tabitem.Content = scroll;

        error_tab_control_panel.Items.Add(tabitem);

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        //            return null;
    }

}

//***************

static public void _Clear_Window()
{
    try
    {
        //--- очищаем окно.
        ERROR_TEXT_BOX.SelectAll();
        ERRORS.ERROR_TEXT_BOX.Cut();

        //--- выводим TabItem на передний план.
        TOOLS_TAB_CONTROL_PANEL.SelectedItem = ERRORS_TAB_ITEM;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        //            return null;
    }
}

//***************

static public void Message(string msg)
{
    try
    {
        ERROR_TEXT_BOX.AppendText(msg);

        ERROR_TEXT_BOX.ScrollToEnd();

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        //            return null;
    }

}

//***************

static public void Add(string error, Network_Of_Elements network, int[] xy)
{
    try
    {
        StringBuilder str = new StringBuilder("\nError: ");
        
            str.Append(error);

            if (network != null) str.Append("   Network: " + network.NUM);

            if ( xy != null )  str.Append(",   Cell: " + xy[0] + xy[1]);
    
            NETWORKS_ERROR_LIST.Add(str.ToString());

        ERROR_TEXT_BOX.AppendText(str.ToString());

        ERROR_TEXT_BOX.ScrollToEnd();

        //--- выводим TabItem на передний план.
        TOOLS_TAB_CONTROL_PANEL.SelectedItem = ERRORS_TAB_ITEM;

    }
    catch (Exception excp)
    {
            MessageBox.Show(excp.ToString());
    }

}

static public void Add(string error)
{
    try
    {
        StringBuilder str = new StringBuilder("\nError: ");
        
            str.Append(error);

            NETWORKS_ERROR_LIST.Add(str.ToString());

        ERROR_TEXT_BOX.AppendText(str.ToString());

        ERROR_TEXT_BOX.ScrollToEnd();

        //--- выводим TabItem на передний план.
        TOOLS_TAB_CONTROL_PANEL.SelectedItem = ERRORS_TAB_ITEM;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}


        }  //*************    END of Class <ERRORS>

    }  //*************    END of Class <Step5>
}
