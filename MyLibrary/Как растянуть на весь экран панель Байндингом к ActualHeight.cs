// + заменить popUp на ContextMenu

namespace ComplexPro_Step5
{
    public partial class Step5
    {

        //************   SHOW_DIAGRAMS()

public void SHOW_DIAGRAMS()
    {
        try
        {

            DIAGRAMS_TAB_CONTROL = new TabControl();
                Binding binding = new Binding();
                binding.Source = DIAGRAMS_PANEL;
                binding.Path =  new PropertyPath("ActualHeight");
            DIAGRAMS_TAB_CONTROL.SetBinding(TabControl.HeightProperty, binding);
                binding = new Binding();
                binding.Source = DIAGRAMS_PANEL;
                binding.Path = new PropertyPath("ActualWidth");
            DIAGRAMS_TAB_CONTROL.SetBinding(TabControl.WidthProperty, binding);
               

            DIAGRAMS_PANEL.Children.Add(DIAGRAMS_TAB_CONTROL);

            //DIAGRAMS_PANEL.Width = DIAGRAMS_PANEL.ExtentWidth;
            //DIAGRAMS_PANEL.Height = DIAGRAMS_PANEL.ExtentHeight;

            foreach( Diagram_Of_Networks diagram in DIAGRAMS_LIST )
            {
                TabItem tab_item = new TabItem();
                tab_item.Header = diagram.NAME;

                    ScrollViewer scroll_viewer = new ScrollViewer();
                    scroll_viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

                        StackPanel stack_panel = new StackPanel();
                        stack_panel.Orientation = Orientation.Vertical;

                    scroll_viewer.Content = stack_panel;

                tab_item.Content = scroll_viewer;

                DIAGRAMS_TAB_CONTROL.Items.Add(tab_item);

                diagram.SHOW_DIAGRAM(stack_panel);
            }

        }
        catch (Exception excp)
        {
            MessageBox.Show(excp.ToString());
        }
    }


//****************************************************
