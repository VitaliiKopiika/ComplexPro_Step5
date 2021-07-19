//  сделать закрытие всех окон приложения при выходе из него - для окон debug_window.Visibility = Visibility.Hidden;
//  + также не затирается в принимающем окне последний символ
//  проверить получение большего числа чем размер массива      

//сделать закрытие MODBUS_Thread при закрытии окна Debug
//сделать окна переменных для отображения значений переменных
//сделать автосчитывание данных по списку.

//2019-06  закончил на упрощении кодировки
//начать делать протокол считывания уставок чер управляющее слово
//начать делать считывание уставок по частям через маленький буфер


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

using System.Net; // For Dns, IPHostEntry, IPAddress
using System.Net.Sockets; // For SocketException
using System.Collections.Concurrent;
using System.Runtime.InteropServices; // For [DllImport("***.dll")]
using System.IO;
using System.Xml;

namespace ComplexPro_Step5
{

    public partial class Step5
    {
//            [DllImport("d:\\Visual_Studio_2012\\Projects\\ComplexPro_Step5\\ComplexPro_Step5\\DLL\\MemoryDLL.dll")]
//            public static extern void SendCtrlCode(long Code);


        public static Debug DEBUG;

        
        public class Debug : DependencyObject
        {
            Task ReadTimer_Thread_ ;

            //// Echo Server.
            //TCP_IP_Server_Thread MODBUS_SERVER ;

            // Modbus Client
            ModbusTcpIp_Client MODBUS_DATA_FRAME_CLIENT ;
            ModbusMaster MODBUS_THREAD;

            //GetControllerDataThread MODBUS_THREAD;

            public static Window debug_window = null;

            List<UIElement> Bindings_UIElement_list;

            public TextBox ReceivingTextBox;

            public bool IsEnable
            {
                get { return (bool)GetValue(IsEnableProperty); }
                set { SetValue(IsEnableProperty, value); }
            }

            public static readonly DependencyProperty IsEnableProperty =
                                     DependencyProperty.Register("IsEnable", typeof(bool), typeof(Debug));

            public string Status_Text
            {
                get { return (string)GetValue(Status_TextProperty); }
                set { SetValue(Status_TextProperty, value); }
            }

            public static readonly DependencyProperty Status_TextProperty =
                                     DependencyProperty.Register("Status_Text", typeof(string), typeof(Debug));


            public string time_str
            {
                get { return (string)GetValue(time_strProperty); }
                set { SetValue(time_strProperty, value); }
            }
            public static readonly DependencyProperty time_strProperty =
                                     DependencyProperty.Register("time_str", typeof(string), typeof(Debug));


            public Dictionary<string, string> Ports_list ;

            public IP_Address_Box IPAddressBox;
            public ComboBox IPPortBox;
            public ModbusTCPIP_ADU Send_Modbus_ADU;
            public ModbusTCPIP_ADU Receive_Modbus_ADU;

            public string GetServerAddress() {return IPAddressBox.IP_Address;}
            public string GetServerPort() { return Ports_list.Values.ToArray()[IPPortBox.SelectedIndex]; }

            StackPanel servicesPanel;
            //------------

            public Debug()
            {

                IsEnable = false;
                Status_Text = "No errors";

                //time_str = DateTime.Now.ToString();
                ReadTimer_Thread_ = new Task(Run_ReadTimer_Thread);
                ReadTimer_Thread_.Start();

                ReceivingTextBox = null;

                //TCP_IP_CONNECTION = null;// new TCP_IP_Connection();

                Bindings_UIElement_list = new List<UIElement>();

                Ports_list = new Dictionary<string, string>() {   {"502 - Modbus", "502" },
                                                                  {"007 - Echo  ",   "7" }
                                                              };

                MODBUS_DATA_FRAME_CLIENT = null;
                MODBUS_THREAD = null;
            }


            void Run_ReadTimer_Thread()
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate()
                    {
                        time_str = DateTime.Now.Second.ToString();

                        //foreach (SYMBOLS.Symbol_Data symbol in SYMBOLS.GLOBAL_SYMBOLS_LIST)
                        //{
                        //    if (symbol.Data_Type != "BOOL") symbol.fstr_Value = time_str;
                        //    else
                        //    {
                        //        double dax;
                        //        string str = "null";
                        //        if (time_str == null || time_str == "") str = "null";
                        //        // "NumberStyles.Any" - чтобы использовать десятичную  точку и десятичную запятую.
                        //        else if (double.TryParse((string)time_str, NumberStyles.Any, null, out dax))
                        //        {
                        //            if (dax == 0) str = "0";
                        //            else str = "1";
                        //        }
                        //        symbol.fstr_Value = str;
                        //    }
                        //}

                        Step5.Debug.Fill_ENI_ENO_Connectors();

                    });
                }
            }

//***********   SHOW_DEBUG_window

            public void SHOW_DEBUG_window(string code)
            {
                try
                {
                    IsEnable = true ;

//                  if ( debug_window == null )
                  {

                    //**************   CREATE WINDOW

                    debug_window = new Window();

                    debug_window.Title = "Debug";
                    debug_window.SizeToContent = SizeToContent.WidthAndHeight;
                    debug_window.Width = 500;
                    debug_window.Height = 300;
                    debug_window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    //symbols_list_window.ContentRendered += SYMBOLS_LIST_window_ContentRendered;
                    debug_window.Closing += DEBUG_window_Closing;

                    //**********  CREATE WINDOW CONTENT

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

                            RowDefinition row0 = new RowDefinition();
                            RowDefinition row1 = new RowDefinition();
                            row0.Height = GridLength.Auto;
                            row1.Height = GridLength.Auto;
                            grid.RowDefinitions.Add(row0);
                            grid.RowDefinitions.Add(row1);

                                //   Окно вывода сообщений об ошибках в таблице символов.
                                GroupBox err_groupbox = new GroupBox();
                                err_groupbox.Header = "Send";
                                err_groupbox.SetValue(Grid.ColumnProperty, 0);
                                err_groupbox.SetValue(Grid.RowProperty, 0);

                                    ScrollViewer err_scrollviewer = new ScrollViewer();
                                    err_scrollviewer.MaxWidth = 700;
                                    err_scrollviewer.MaxHeight = 500;
                                    err_scrollviewer.Margin = new Thickness(5,5,0,5);
                                    err_scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
 
                                    //    TextBox err_textblock = new TextBox();
                                    //    err_textblock.TextWrapping = TextWrapping.Wrap;
                                    //    err_textblock.Width = 700; 
                                    //    err_textblock.MinHeight = 200;
                                    //    err_textblock.TextChanged += err_textblock_TextChanged;

                                    //err_scrollviewer.Content = err_textblock;

                                    StackPanel modbus_stackpanel = new StackPanel();
                                    modbus_stackpanel.Orientation = Orientation.Vertical;

                                        Send_Modbus_ADU = new ModbusTCPIP_ADU(ModbusTCPIP_ADU.ItemType.Byte, 7);
                                        Send_Modbus_ADU.SetValue( new Modbus_DataFrame(1,0,6,0,3, new byte[]{0,0,0,1}));    
                                        Receive_Modbus_ADU = new ModbusTCPIP_ADU(ModbusTCPIP_ADU.ItemType.Byte, 7);

                                    modbus_stackpanel.Children.Add(Send_Modbus_ADU.Get_UIElement());
                                    modbus_stackpanel.Children.Add(Receive_Modbus_ADU.Get_UIElement());
                                    
                                        servicesPanel = new StackPanel();
                                        servicesPanel.HorizontalAlignment = HorizontalAlignment.Left;

                                    modbus_stackpanel.Children.Add(servicesPanel);

                                    err_scrollviewer.Content = modbus_stackpanel;

                                err_groupbox.Content = err_scrollviewer;

                            grid.Children.Add(err_groupbox);
                            //----------------

                                  //   Окно вывода сообщений об ошибках в таблице символов.
                                GroupBox err_groupbox2 = new GroupBox();
                                err_groupbox2.Header = "Receive";
                                err_groupbox2.SetValue(Grid.ColumnProperty, 0);
                                err_groupbox2.SetValue(Grid.RowProperty, 1);

                                    ScrollViewer err_scrollviewer2 = new ScrollViewer();
                                    err_scrollviewer2.MaxWidth = 700;
                                    err_scrollviewer2.MaxHeight = 500;
                                    err_scrollviewer2.Margin = new Thickness(5,5,0,5);
                                    err_scrollviewer2.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
 
                                        ReceivingTextBox = new TextBox();
                                        ReceivingTextBox.TextWrapping = TextWrapping.Wrap;
                                        ReceivingTextBox.Width = 700;
                                        ReceivingTextBox.MinHeight = 200;

                                        ////  при открывании окна i-тый раз заходит сюда по i-раза на одну 
                                        ////  принятую посылку - 2-й раз вхолостую - возвращает нуль - затирает текстбокс
                                        ////  ГДЕ-ТО накапливается коллекция старых байндингов
                                        ////  очистить старые байдинги перед созданием нового не помогает
                                        ////BindingOperations.ClearAllBindings(DEBUG.TCP_IP_CONNECTION.Client_receivingStream.Data_Changed);

                                        ////ReceivingTextBox.SetBinding(TextBox.TextProperty,
                                        //  //              DEBUG.TCP_IP_CONNECTION.Client.Receiving_stream.Data_Changed.Get_AddedProperty_Binding());
                                        ////Bindings_UIElement_list.Add(ReceivingTextBox);

                                        //DEBUG.TCP_IP_CONNECTION.Client.Receiving_stream.Data_Changed.Tag = ReceivingTextBox;
                                        //DEBUG.TCP_IP_CONNECTION.Client.Receiving_stream.Data_Changed.AddedPropertyChanged_Event += DataChanged_AddedPropertyChangedEvent;                                        

                                    err_scrollviewer2.Content = ReceivingTextBox;

                                err_groupbox2.Content = err_scrollviewer2;

                            grid.Children.Add(err_groupbox2);

//****************  ADD BUTTONS

                                StackPanel stackpanel = new StackPanel();
                                stackpanel.Orientation = Orientation.Vertical;
                                stackpanel.VerticalAlignment = VerticalAlignment.Top;
                                stackpanel.SetValue(Grid.ColumnProperty, 1);
                                stackpanel.SetValue(Grid.RowProperty, 0);
                                stackpanel.MinWidth = 50;

                                    TextBox textbox = new TextBox();
                                    textbox.MinHeight = 20;
                                    textbox.Background = new SolidColorBrush(Colors.Transparent);
                                    textbox.HorizontalContentAlignment = HorizontalAlignment.Center;
                                    textbox.VerticalContentAlignment = VerticalAlignment.Center;
                                       Binding text_binding = new Binding();
                                        text_binding.Source = Step5.DEBUG;
                                        text_binding.Path = new PropertyPath("time_str");
                                    textbox.SetBinding(TextBox.TextProperty, text_binding);
                                    Bindings_UIElement_list.Add(textbox);

                                    //IPAddressBox = new IP_Address_Box(new IPAddress(new byte[] { 192, 168, 1, 38 }));
                                    ////IP_Address_Box ip_box = new IP_Address_Box(IPAddress.Loopback);
                                    //IPAddressBox.Header = "Server IPAdress";
                                    //    //Binding ip_box_binding = new Binding();
                                    //    //ip_box_binding.Source = Step5.DEBUG.TCP_IP_CONNECTION;
                                    //    //ip_box_binding.Path = new PropertyPath("Server_Name");
                                    //    //ip_box_binding.Mode = BindingMode.TwoWay;
                                    ////ip_box.SetBinding(IP_Address_Box.IP_AdrressProperty, ip_box_binding);
                                    ////Bindings_UIElement_list.Add(ip_box);


                                    GroupBox port_box = new GroupBox();
                                    port_box.Header = "Server Port";
                                        IPPortBox = new ComboBox();
                                        IPPortBox.ItemsSource = Ports_list.Keys;
                                        IPPortBox.SelectedIndex = 0;
                                    port_box.Content = IPPortBox;

                                    IPAddressBox = new IP_Address_Box("192.168.1.202"); //IPAddress.Loopback);
                                    //IPAddressBox = new IP_Address_Box(IPAddress.Loopback);
                                    IPAddressBox.Header = "Server IPAdress";

                                    //Button button_MyHostEntry = SYMBOLS.Get_Button("MyHostEntry", button_MyHostEntry_Click, debug_window);

                                    Button button_OpenConnection = SYMBOLS.Get_Button("Open Connection", button_OpenConnection_Click, debug_window);
                                    //Button button_StartEchoServer = SYMBOLS.Get_Button("Start echo server", button_StartEchoServer_Click, debug_window);
                                    Button button_CloseConnection = SYMBOLS.Get_Button("Close Connection", button_CloseConnection_Click, debug_window);
                                    Button button_SendFrame = SYMBOLS.Get_Button("Send Frame", button_Send_Click, debug_window);
                                    Button button_ReceiveFrame = SYMBOLS.Get_Button("Receive Frame", button_Receive_Click, debug_window);
                                    Button button_ReadVar = SYMBOLS.Get_Button("ReadVar", button_ReadVar_Click, debug_window);
                                    Button button_LoadMapData = SYMBOLS.Get_Button("Load map-data", button_LoadMapData_Click, debug_window);
                                    Button button_ReadXmlData = SYMBOLS.Get_Button("Read Xml-data", button_GetXml_Click, debug_window);
                                    //TextBox textbox_XmlCommand = new TextBox() { Text = "<Command Name=\"Read\" Path=\"\"/>" };
                                    TextBox textbox_XmlCommand = new TextBox() { Text = "<Command Id=\"123\" Name=\"Write\" Path=\"0.0.0\" AttributeName=\"Value\" AttributeValue=\"1000\" />" };
                                    Button button_WriteXmlCommand = SYMBOLS.Get_Button("Write Xml-command", button_WriteVar_Click, debug_window);
                                    button_WriteXmlCommand.Tag = textbox_XmlCommand;
                                    Button button_AutoReadXmlData = SYMBOLS.Get_Button("AutoReadXmlData", button_AutoReadXmlData_ClickAsync, debug_window);
                                    Button button_Cancel = SYMBOLS.Get_Button("Ok/Cancel", button_Cancel_Click, debug_window);

                                // иначе по cancel окно закрывается безусловно  button_Cancel.IsCancel = true;
                                FocusManager.SetFocusedElement(debug_window, button_Cancel);


                                stackpanel.Children.Add(IPAddressBox);
                                stackpanel.Children.Add(port_box);
                                stackpanel.Children.Add(textbox);
                               // stackpanel.Children.Add(button_StartEchoServer);
                                stackpanel.Children.Add(button_OpenConnection);
                                stackpanel.Children.Add(button_SendFrame);
                                stackpanel.Children.Add(button_ReceiveFrame);
                                stackpanel.Children.Add(button_ReadVar);
                                stackpanel.Children.Add(button_LoadMapData);
                                stackpanel.Children.Add(button_CloseConnection);
                                stackpanel.Children.Add(button_ReadXmlData);
                                stackpanel.Children.Add(textbox_XmlCommand); 
                                stackpanel.Children.Add(button_WriteXmlCommand);
                                stackpanel.Children.Add(button_AutoReadXmlData);
                                stackpanel.Children.Add(button_Cancel);

                            grid.Children.Add(stackpanel);


                        xborder.Child = grid;

                    debug_window.Content = xborder;

                    //**********  SHOW WINDOW

                    if (code == "Show") debug_window.Show();
                    else if (code == "ShowDialog") debug_window.ShowDialog();
                    else throw new Exception("<SHOW_SYMBOLS_LIST_window()>: Unknown code <Refresh>");

                }
  //              else debug_window.Visibility = Visibility.Visible;

                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                }

                return;

            }

//********


//********   HANDLER  TextChanged

            //  При каждом изменении текста отправляем весь текст заново.
            //void err_textblock_TextChanged(object sender, TextChangedEventArgs e)
            //{
            //    try
            //    {
            //        TextBox textbox = (TextBox)sender;

            //        string str = null;

            //        if (String.IsNullOrEmpty(textbox.Text)) str = " ";
            //        else str = textbox.Text;
                   
            //        //DEBUG.TCP_IP_CONNECTION.Client.Sending_stream.Write(str);
            //        DEBUG.TCP_IP_CONNECTION.Client.Sending_stream.WriteFrame(str);

            //    }
            //    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
            //}

//********   HANDLER  RECEIVED DATA Changed

//            void DataChanged_AddedPropertyChangedEvent(object sender, DependencyPropertyChangedEventArgs e)
//            {
//                try
//                {
//                    ProducerConsumer_QueueStream.DataChanged datachanged = (ProducerConsumer_QueueStream.DataChanged)sender;

//                    //--------------
//                    //datachanged.Parent_stream.ReadFrame();
////--------
//                    TextBox textbox = (TextBox)datachanged.Tag;

//                    //List<string> str_list;
//                    //datachanged.Parent_stream.TryReadFrame(out str_list);
//                    //textbox.Text = str_list.Last();

//                    byte[] buff = new byte[200];
//                    int count = datachanged.Parent_stream.TryRead(buff, 0, 200);

//                    Receive_Modbus_ADU.SetValue(buff, count);

//                    StringBuilder str = new StringBuilder();
//                    for (int i = 0; i < count; i++) str.Append(buff[i].ToString() + " ");

//                    textbox.Text = str.ToString();

//                }
//                catch (Exception excp) { MessageBox.Show(excp.ToString()); }

//            }


//***********  CONNECT VARs 

    static public string Fill_ENI_ENO_Connectors()
    {
        try
        {

                    //**************  Поиск всех исполнительных элементов.

                    //  Столбец за столбцом сверху вниз сканируем ячейки на предмет 
                    // исполнительных элементов - находя элемент заносим в его 
                    // ENI-ENO текстблоки значения коннект-переменных

            if (DIAGRAMS_LIST != null && DEBUG != null )
            {
                foreach (Diagram_Of_Networks diagram in DIAGRAMS_LIST)
                {
                    if (diagram.NETWORKS_LIST != null)
                    {
                        foreach (Network_Of_Elements network in diagram.NETWORKS_LIST)
                        {
                            for (int x = 0; x < network.Network_panel_copy[0].Count; x++)
                            {
                                for (int y = 0; y < network.Network_panel_copy.Count; y++)
                                {
                                    //********************

                                    // Поиск очередного элемента не коннектора.
                                    y = DiagramCompiler.Find_NextNonConnector(network, x, y);
                                    // В столбце нет ни одного.
                                    if (y == -1) break;

                                    string str = DEBUG.IsEnable == true ? DEBUG.time_str : "";
                                    TextBlock ptr = network.Network_panel_copy[y][x].ENI_textblock;
                                    if ( ptr != null ) ptr.Text = str;
                                    ptr = network.Network_panel_copy[y][x].ENO_textblock;
                                    if (ptr != null) ptr.Text = str;

                                }
                            }
                        }
                    }
                }
            }

            return null;

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return null;
    }
}

//*************  HANDLERS

void button_OpenConnection_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (MODBUS_DATA_FRAME_CLIENT == null)
        {
            // Modbus Client
            MODBUS_DATA_FRAME_CLIENT = new ModbusTcpIp_Client(this.GetServerAddress(), int.Parse(this.GetServerPort()), 2000, 2);

            if (!MODBUS_DATA_FRAME_CLIENT.Connect())
            {
                ReceivingTextBox.Text = "Can't open client connection!";
                MODBUS_DATA_FRAME_CLIENT.Close(); MODBUS_DATA_FRAME_CLIENT = null;
            }
            else    ReceivingTextBox.Text = "Connection is open!";
        }
        else    ReceivingTextBox.Text = "Connection is currently open!";
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

//void button_StartEchoServer_Click(object sender, RoutedEventArgs e)
//{
//    try
//    {
//        if (MODBUS_SERVER == null)
//        {
//            // Echo Server.
//            MODBUS_SERVER = new TCP_IP_Server_Thread(7, 2000);

//            if (!MODBUS_SERVER.IsConnected)
//            {
//                MessageBox.Show("Can't open server connection!");
//                MODBUS_SERVER.Close(2000); MODBUS_SERVER = null; 
//                return;
//            }
//        }
//        else
//        {
//            MessageBox.Show("Connection is currently open!");
//        }

//    }
//    catch (Exception excp)
//    {
//        MessageBox.Show(excp.ToString());
//    }

//}



void button_CloseConnection_Click(object sender, RoutedEventArgs e)
{
    try
    {
            // Modbus Client
        if (MODBUS_DATA_FRAME_CLIENT != null)  
        { 
            
            if (!MODBUS_DATA_FRAME_CLIENT.Close()) ReceivingTextBox.Text = "Can't close client connection!";
            else               ReceivingTextBox.Text = "Connection is closed!";
            MODBUS_DATA_FRAME_CLIENT = null;
        }
        else
        {
            ReceivingTextBox.Text = "Connection is currently closed!";
        }

        if (MODBUS_THREAD != null) MODBUS_THREAD.Close();
        
            // Echo Server.
        //if (MODBUS_SERVER != null) { MODBUS_SERVER.Close(2000); MODBUS_SERVER = null; }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void button_Send_Click(object sender, RoutedEventArgs e)
{
    try
    {
        // Frames
        Modbus_DataFrame frame_send;//= new Modbus_DataFrame( 1, 2, 5, 6, 7, new byte[]{11,12,13});
        Send_Modbus_ADU.Transaction_ID.iValue++; 
        Send_Modbus_ADU.GetValue(out frame_send);
        //frame_send.Data_Array[0]++;

        //  Sending frame
        if (MODBUS_DATA_FRAME_CLIENT != null && MODBUS_DATA_FRAME_CLIENT.IsConnected)
        {
            if (MODBUS_DATA_FRAME_CLIENT.Send(frame_send)) ReceivingTextBox.Text = "Frame is sent";
            else ReceivingTextBox.Text = "Can't send frame";
        }
        else ReceivingTextBox.Text = "No connection";

    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}

void button_Receive_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Modbus_DataFrame frame_rec = new Modbus_DataFrame();
        Modbus_DataFrame frame_send;

        Send_Modbus_ADU.GetValue(out frame_send);

        if (MODBUS_DATA_FRAME_CLIENT != null && MODBUS_DATA_FRAME_CLIENT.IsConnected)
        {
            if (MODBUS_DATA_FRAME_CLIENT.Receive(frame_send, ref frame_rec))
            {
                ReceivingTextBox.Text = "Frame is received: \n" + frame_rec.frameToString();
                Receive_Modbus_ADU.SetValue(frame_rec);
            }
            else ReceivingTextBox.Text = "No received frame";
        }
        else ReceivingTextBox.Text = "No connection";
    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}


void button_ReadVar_Click(object sender, RoutedEventArgs e)
{
    try
    {
        MODBUS_THREAD = new ModbusMaster();

        MODBUS_THREAD.Start(this.GetServerAddress(), int.Parse(this.GetServerPort()), 2000);

//#1 
        //Modbus_DataFrame_Client client = null;

        //client = new Modbus_DataFrame_Client(this.GetServerAddress(), int.Parse(this.GetServerPort()));
        //client.Connect(2000);
        //if (client.IsConnected)
        //{
        //    //ushort? ax = 0;

        //    Modbus_VarData arrsize = new Modbus_VarData(ModbusCommand.ReadHoldingRegisters, "", 21, ModbusVarType.Word, client, 2000);

        //    //if (arrsize.getValue(ref ax)) 
        //    {
        //        Modbus_VarData arr = new Modbus_VarData(ModbusCommand.ReadByteArray, "", 22, arrsize, client, 2000);
        //        byte[] arrr ;
        //        if (arr.getValue(out arrr, 200))
        //        {
        //            StringBuilder str = new StringBuilder("Array size: <" + arrr.Length + ">\n");
        //            for (int i = 0; i < arrr.Length; i++) str.Append(arrr[i].ToString() + " ");
        //            ReceivingTextBox.Text = str.ToString();
        //        }
        //        else ReceivingTextBox.Text = "Can't read array";
        //    }
        //    //else ReceivingTextBox.Text = "Can't read array size";
        //}
        //else ReceivingTextBox.Text = "Can't open client connection";


        //client.Close(2000); client = null;

        return;
 
    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}






void button_LoadMapData_Click(object sender, RoutedEventArgs e)
{
    try
    {
        Step5.SaveProject.Load_MAP_DATA_Window();

        return;

    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}


void button_GetXml_Click(object sender, RoutedEventArgs e)
{
    ModbusTcpIp_Client modbusClient = null;
    
    try
    {
// ***********  Установление соединения и получение ссылки на клиента    ************

            modbusClient = new ModbusTcpIp_Client(this.GetServerAddress(), int.Parse(this.GetServerPort()), 2000, 2);
            modbusClient.Connect();
            if (!modbusClient.IsConnected)
            {
                MessageBox.Show("Can't open connection");
                modbusClient.Close(); modbusClient = null;
                return;
            }
        
// ************************************************************************************

        Modbus_VarData varXml = new Modbus_VarData("", 159,
              new Modbus_VarData("", 158, VarType.Word, modbusClient), modbusClient);
        
        byte[] byteArray = null;
        StringBuilder str = new StringBuilder();

        if (varXml.getValue(out byteArray, null, 100)) MessageBox.Show("Data is received!");
        else { MessageBox.Show("Data isn't received!"); return; }

//читаются нули XmlDataProvider-массива
//размер 20000 читается правильно

            str.Clear();
            str.Append("XML:\n");
            str.Append("Array size: <" + byteArray.Length + ">\n");

//  коды страниц кодировок для Encoding.GetEncoding(...)
//https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=netframework-4.7.2            

            // Преобразвоание строки полученной из контроллера в Dos-кодировке
            // в строку в Windows-кодировке:
            // экспериментально определено что контроллер передает в кодировке pc866 (русский вариант ASCII)
            // а компьютер выводит в кодировке Unicode(UTF16-litleendian)
            // Сложный вариант: (может пригодиься для С++)
            //byte[] buff;
            //for (int i = 0; i < byteArray.Length; i += 1) 
            //{
            ////  вынимаем по одному дайту из входного массива и преобразовываем в 
            ////  двухбайтные символы Unicode(UTF16-litleendian)
            //    buff = Encoding.Convert(Encoding.GetEncoding(866), Encoding.Unicode, byteArray,i,1);
            //    if (buff.Length == 2) str.Append((char)((UInt32)buff[0] + ((UInt32)buff[1] << 8))) ;
            //    else   { throw new Exception("Encoding error!"); }
            //}
            // Простой вариант:
            int sizex;
            sizex = (sizex = byteArray.ToList().IndexOf(0)) == -1 ? byteArray.Length : sizex;
            str.Append(Encoding.GetEncoding(1251).GetString(byteArray, 0, sizex));

            DEBUG.ReceivingTextBox.Text = str.ToString();

        return;

    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
        if (modbusClient != null) { modbusClient.Close(); modbusClient = null;}
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}

void button_WriteVar_Click(object sender, RoutedEventArgs e)
{
    ModbusTcpIp_Client modbusClient = null;
 
    try
    {
        // ***********  Установление соединения и получение ссылки на клиента    ************

        modbusClient = new ModbusTcpIp_Client(this.GetServerAddress(), int.Parse(this.GetServerPort()), 2000, 2);
        modbusClient.Connect();
        if (!modbusClient.IsConnected)
        {
            MessageBox.Show("Can't open connection");
            modbusClient.Close(); modbusClient = null;
            return;
        }

        XmlCommander comm = new XmlCommander(modbusClient, Encoding.GetEncoding(1251));

        comm.writeStringAttribute("0.0.0", "Value", ((sender as Button).Tag as TextBox).Text);
        //Modbus_VarData varXml= new Modbus_VarData("", 155,
        //    new Modbus_VarData("", 154, VarType.Word, modbusClient), modbusClient);

        //if (varXml.setValue(((sender as Button).Tag as TextBox).Text + "\0", Encoding.GetEncoding(1251))) MessageBox.Show("Data is sent!");
        //else { MessageBox.Show("Data isn't sent!"); return; }


    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
        if (modbusClient != null) { modbusClient.Close(); modbusClient = null; }
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}


async void button_AutoReadXmlData_ClickAsync(object sender, RoutedEventArgs e)
{
    ModbusTcpIp_Client modbusClient = null;

    try
    {
        // ***********  Установление соединения и получение ссылки на клиента    ************

        modbusClient = new ModbusTcpIp_Client(this.GetServerAddress(), int.Parse(this.GetServerPort()), 2000, 2);
        modbusClient.Connect();
        if (!modbusClient.IsConnected)
        {
            MessageBox.Show("Can't open connection");
            modbusClient.Close(); modbusClient = null;
            return;
        }
        //******

        XmlCommander comm = new XmlCommander(modbusClient, Encoding.GetEncoding(1251));
        
        string path = "";

        //*************

        Service_A7 serviceA7; 
        ////  reading number of root-pages (A7, A8, ...)
        //int rootCount = comm.readIntAttribute(path, "Count").Value;
        ////for (int i = 0; i < rootCount; i++)
        //{
        //    path = "0";// i.ToString("0");
        //    serviceA7 = new Service_A7(comm, path, this.Dispatcher);
        //    serviceA7.dataSource.LoadEmptyGroups();
        //    servicesPanel.Children.Add(serviceA7.dataSource.BuildDataGrids()[0]);
        //    await serviceA7.dataSource.LoadGroupsContentAsync();
        //}

        int rootCount = comm.readIntAttribute(path, "Count").Value;
        //for (int i = 0; i < rootCount; i++)
        {
            path = "0";// i.ToString("0");
            serviceA7 = new Service_A7(comm, path, this.Dispatcher);
           // serviceA7.dataSource.LoadEmptyGroups();
            TabControl tbc = new TabControl();
            servicesPanel.Children.Clear();
            servicesPanel.Children.Add(tbc);
            await serviceA7.dataSource.BuildTabControlAsync(tbc);
            //serviceA7.dataSource.BuildEmptyTabControl(tbc);
            //await serviceA7.dataSource.LoadTabsContentAsync();
        }


    }
    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
    finally
    {
// связь должна остаться и после загрузки уставок для их корректировки из datagrid
//  для их записи в контроллер при их корректировке 
//        if (modbusClient != null) { modbusClient.Close(); modbusClient = null; }
        e.Handled = true;
        ((Button)sender).IsEnabled = true;
    }

}

//void scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
//{
//    ScrollBar s = sender as ScrollBar;
//}

//********   ServicesPanel   ***********************

//interface IServicesPanel
//{
//    string getId();
//    string createPanel();
//    string showPanel();
//}


//class ServicesPanel_Main : IServicesPanel
//{
//    Panel mainPanel ;
//    XmlCommander commander;
    
//    public ServicesPanel_Main(Panel panel, XmlCommander commander)
//    {
//        this.mainPanel = panel;
//        this.commander = commander;
//        //---
//        servicePanelsList = new List<IServicesPanel>();
//    }
//    //---

//    List<IServicesPanel> servicePanelsList;

//    //---
    
//    override public string getId() { return "Root"; }

//    override public void createPanel()
//    {
//        try
//        {
//            string path = "";

//            GroupBox gb = new GroupBox();
//                gb.Header = "Services";
//            mainPanel.Children.Clear();
//            mainPanel.Children.Add(gb);

//            StackPanel sp = new StackPanel();
//            sp.Orientation = Orientation.Vertical;
//            gb.Content = sp;

//            //  reading number of pages in the root
//            int rootCount = commander.readIntAttribute(path, "Count").Value;

//            for (int i = 0; i < rootCount; i++)
//            {
//                // scanning all pages "A7", "A8" ...
//                path = i.ToString("0");
//                //   To increase speed - reading Element into array and then 
//                // taking Attributes from array
//                byte[] bytes = commander.readElement(path);
//                //---
//                Button bt = new Button();
//                bt.Width = 200; //bt.Height = 30;
//                bt.HorizontalContentAlignment = HorizontalAlignment.Left;
//                bt.Padding = new Thickness(15,5,5,5); 
//                bt.Margin = new Thickness(5);
//                //bt.Background = new SolidColorBrush(Colors.Transparent);
//                bt.Content = commander.readStringAttribute(null, "Name", bytes);
//                bt.Tag = path.Clone();
//                sp.Children.Add(bt);
//            }
//        }
//        catch (Exception excp) { MessageBox.Show(excp.ToString()); }
//    }
//}

////продолжить создание панели

////********   A7_ServicePanel   ***********************




//class ServicesPanel_A7 :  IServicesPanel
//{
//    Panel mainPanel;
//    XmlCommander commander;

//    public ServicesPanel_A7(Panel panel, XmlCommander commander)
//    {
//        this.mainPanel = panel;
//        this.commander = commander;
//    }
//    //---

//    override public string getId() { return "A7"; }

//    override public void createPanel()
//    {
//        try
//        {
//            string path = "";

//            GroupBox gb = new GroupBox();
//            gb.Header = "Services";
//            mainPanel.Children.Clear();
//            mainPanel.Children.Add(gb);

//            StackPanel sp = new StackPanel();
//            sp.Orientation = Orientation.Vertical;
//            gb.Content = sp;

//            //  reading number of pages in the root
//            int rootCount = commander.readIntAttribute(path, "Count").Value;

//            for (int i = 0; i < rootCount; i++)
//            {
//                // scanning all pages "A7", "A8" ...
//                path = i.ToString("0");
//                //   To increase speed - reading Element into array and then 
//                // taking Attributes from array
//                byte[] bytes = commander.readElement(path);
//                //---
//                Button bt = new Button();
//                bt.Width = 200; //bt.Height = 30;
//                bt.HorizontalContentAlignment = HorizontalAlignment.Left;
//                bt.Padding = new Thickness(15, 5, 5, 5);
//                bt.Margin = new Thickness(5);
//                //bt.Background = new SolidColorBrush(Colors.Transparent);
//                bt.Content = commander.readStringAttribute(null, "Name", bytes);
//                bt.Tag = path.Clone();
//                sp.Children.Add(bt);
//            }
//        }
//        catch (Exception excp) { MessageBox.Show(excp.ToString()); }
//    }
//}






byte decode_ansi_to_oem(byte ch)
{
    if (ch >= (byte)128 && ch <= (byte)175) ch += (byte)64;
    else if (ch >= (byte)224 && ch <= (byte)239) ch += (byte)16;
    else if (ch == (byte)252) ch = (byte)185;

    return ch;
}

void button_Cancel_Click(object sender, RoutedEventArgs e)
{
    try
    {
        button_CloseConnection_Click(null, null);
        ((Window)((Button)sender).Tag).Close();
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }

}

void DEBUG_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
    //      Вставляем отмену редактирования на случай, если вошли в окно ...

    //if ( ReadTimer_Thread_ != null ) ReadTimer_Thread_.Dispose(); 

    //if (TCP_IP_CONNECTION != null) { TCP_IP_CONNECTION.Close(3000); TCP_IP_CONNECTION = null; }

    //GC.Collect(0, GCCollectionMode.Forced);

  //  debug_window.Visibility = Visibility.Hidden;

    //BindingOperations.ClearAllBindings((Window)sender);

    //((Window)sender).InputBindings.Clear();

    //DEBUG.ReceivingTextBox.InputBindings.Clear();

    //ReceivingTextBox = null;

    //debug_window.InputBindings.Clear();

    button_CloseConnection_Click(null, null);

    foreach(UIElement uielement in DEBUG.Bindings_UIElement_list)  
                                        BindingOperations.ClearAllBindings(uielement) ;// ReceivingTextBox);
    DEBUG.Bindings_UIElement_list.Clear();

    IsEnable = false ;
//    e.Cancel = true;
    }
}
//******** Debug


//**********************************
//**********************************

//public class GetControllerDataThread: DependencyObject
//{
    
//    Modbus_IO_Client modbusIOClient;
//    Task threadMain;
//    string servAddress;   int servPort;   int timeOut;
//    bool closeClient { get; set; }
//    public bool isClosed { get; set; }
//    int scanPeriod;
//    //--------

//    public class MapByteArray
//    {
//        public byte[]   array;
//        public short    size; 
//        public int      arrayVarnum;  // Номер переменной в vartab
//        public int      sizeVarnum;

//        public MapByteArray(int arrayVar , int sizeVar) 
//        {  
//            array = null;   arrayVarnum = arrayVar;     size = -1;    sizeVarnum = sizeVar; 
//        }
//    }

//    //  Список требуемых опроса массивов
//    public List<MapByteArray> mapByteArraysList;

////****************

//    public GetControllerDataThread(string serv_address, int serv_port, int timeout = 5000)
//    {
//        servAddress = serv_address; servPort = serv_port;  timeOut = timeout;
//        closeClient = false; isClosed = false;

//        mapByteArraysList = new List<MapByteArray>();
//        mapByteArraysList.Add(new MapByteArray( 22, 21 )); //ByteArraySize_varnum = 21; ByteArray_varnum = 22;

//        modbusIOClient = null;
//        threadMain = null;
//        scanPeriod = 500; //msec
//    }

//    public bool Close(int timeout = -1)
//    {
//        bool closed_tst = true;

//        try
//        {
//            if (isClosed) return isClosed; // уже было один раз закрыто
//            //---

//            closeClient = true;
//            if (threadMain != null)
//            {   //  ожидание 
//                if (timeout == -1) threadMain.Wait();
//                else closed_tst &= threadMain.Wait(timeout);
//            }

//            if (threadMain != null) { threadMain.Dispose(); threadMain = null; }

//        }
//        catch (Exception excp)
//        {
//            MessageBox.Show(excp.ToString());
//            closed_tst = false;
//            return isClosed = closed_tst;
//            //Environment.Exit(excp.ErrorCode);
//        }
//        finally
//        {
//        }

//        return isClosed = closed_tst; 

//    }



//    public string Start(string serv_address, int serv_port, int timeout = 5000)
//        {
//            try
//            {
//                if (modbusIOClient == null)
//                {
//                    // Modbus Client
//                    modbusIOClient = new Modbus_IO_Client(serv_address, serv_port, timeout);

//                    if (!modbusIOClient.IsConnected)
//                    {
//                        modbusIOClient.Close(timeOut); modbusIOClient = null;
//                        return "Can't open Modbus connection!";
//                    }
//                }
//                else  return "Connection is currently open!";
//                //**************

//                //  Считываем размеры всех массивов
//                foreach (MapByteArray arr in mapByteArraysList)
//                {
//                    if (modbusIOClient.Read(arr.sizeVarnum, out arr.size, timeOut, this.Dispatcher))
//                    {
//                        arr.array = new byte[arr.size];
//                    }
//                    else
//                    {
//                        modbusIOClient.Close();
//                        return "Can't read array size";
//                    }
//                }
////сделал чтобы Read для массивов не возвращала out на новый массив а принимала ссылку на готовый массив
////    дальше сделать запуск опроса по кнопке 

//                //  если размеры считаны то запускаем задачу фонового опороса этих массивов
//                threadMain = new Task(Thread_Modbus);
//                threadMain.Start();

//            }
//            catch (Exception excp)
//            {
//                MessageBox.Show(excp.ToString());
//            }

//            return null;
//        }


//    //  Циклический опрос массивов пока не прервет главная программа
//    public void Thread_Modbus()
//    {
//        try
//        {
//            //  Циклический опрос массивов пока не прервет главная программа
//            while (!closeClient && modbusIOClient.IsConnected)
//            {
//                foreach (MapByteArray arr in mapByteArraysList)
//                {
//                  //this.Dispatcher.BeginInvoke((ThreadStart)delegate()
//                  //{
//                    modbusIOClient.Read(arr.arrayVarnum, ref arr.array, arr.size, timeOut, this.Dispatcher);
//                  //});
//                }
//                //----
//                Thread.Sleep(scanPeriod);
//            }
//            //**************
//        }
//        catch (Exception excp)
//        {
//            MessageBox.Show(excp.ToString());
//        }


//        return;
//    }
//}

//***********  DebugData  ***************

//public class DebugData : DependencyObject
//{
//    public TCP_IP_Connection TCP_IP_CONNECTION;
//    Task ReadTimer_Thread_;

//    // Echo Server.
    //TCP_IP_Server_Thread MODBUS_SERVER;

//    // Modbus Client
//    Modbus_DataFrame_Client MODBUS_DATA_FRAME_CLIENT;

//    typeModbusThread MODBUS_THREAD;

//    public static Window debug_window = null;

//    List<UIElement> Bindings_UIElement_list;

//    public TextBox ReceivingTextBox;

//    public bool IsEnable
//    {
//        get { return (bool)GetValue(IsEnableProperty); }
//        set { SetValue(IsEnableProperty, value); }
//    }

//    public static readonly DependencyProperty IsEnableProperty =
//                             DependencyProperty.Register("IsEnable", typeof(bool), typeof(Debug));

//    public string Status_Text
//    {
//        get { return (string)GetValue(Status_TextProperty); }
//        set { SetValue(Status_TextProperty, value); }
//    }

//    public static readonly DependencyProperty Status_TextProperty =
//                             DependencyProperty.Register("Status_Text", typeof(string), typeof(Debug));
//}




//******** Step5
    }
}
