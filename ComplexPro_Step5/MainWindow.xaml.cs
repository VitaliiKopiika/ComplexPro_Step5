using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComplexPro_Step5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Step5.Diagram_Of_Networks  DIAGRAM ;

        public Step5 STEP5 ;

        public MainWindow()
        {
            InitializeComponent();

            STEP5 = new Step5(FIELD_PANEL, TAB_CONTROL_PANEL, MENU_PANEL);

        }

        
 
    }
}
