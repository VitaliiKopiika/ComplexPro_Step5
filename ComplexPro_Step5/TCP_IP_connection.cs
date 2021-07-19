
// добиться управляемого выхода при отсутствии коннекта

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
using System.IO; // For Dns, IPHostEntry, IPAddress
using System.Collections.Concurrent;

namespace ComplexPro_Step5
{
    public partial class Step5
    {


//**********************************************************************
//******
//*********    TCPIP  CLIENT  
//******
//**********************************************************************

//18-01-2019
//далее перепривязать к новому классу         
//    структуру ModbusTCPIP
//ввести в Read буфер куда будет все считываться для свободного просмотра

//сделать   опрос считывания внутри каждой записи и наоборот чтобы не забивался буфер

                public class TCP_IP_Client : DependencyObject
                {
                    string Server_address { get; set; }
                    int Server_port { get; set; }

                    const int _Socket_BuffSize = 1500;

                    public bool IsConnected { get { return (TCP_Socket != null && TCP_Socket.Connected) ; }  }
                    public bool IsClosed { get { return TCP_Socket == null; } }

                    TcpClient TCP_Socket { get; set; }
                    NetworkStream NetStream { get; set; }
                    public int Received_bytes { get; set; }
                    public int Sent_bytes { get; set; }
                    
                    //***********    CONSTRUCTOR

                    public TCP_IP_Client(string serv_address, int serv_port)//, int buff_size = _Socket_BuffSize)
                    {
                        try
                        {
                            Server_address = serv_address;
                            Server_port = serv_port;
                            //---
                            TCP_Socket = null;   NetStream = null;
                            //---
                            Received_bytes = 0; Sent_bytes = 0;
                        }
                        catch (Exception excp)
                        {
                            MessageBox.Show(excp.ToString());
                        }

                        return;
                    }

                    ~TCP_IP_Client()
                    { 
                        Close(); 
                    }
 
                    //*******

                    public bool Connect(int timeout = -1)
                    {
                        try
                        {
                                if (IsConnected) return true;

                                // Create socket that is connected to server on specified port
                                TCP_Socket = new TcpClient();
                                //  Используем Async-чтобы можно было прервать Connection по истечении таймаут.
                                TCP_Socket.ConnectAsync(Server_address, Server_port);

                                //  ожидание IsConnected
                                DateTime time = DateTime.Now;
                                do{
                                    if (IsConnected)
                                    {
                                        if ((NetStream = TCP_Socket.GetStream()) != null) return true;
                                    }
                                } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);
                                //  Connection failed
                                Close(timeout);
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                        finally {}

                        return false;
                    }
                    //---


                    public bool Close(int timeout = -1)
                    {
                        try
                        {
                            if ( TCP_Socket == null ) return true; // уже было один раз закрыто
                            //---

                            if (NetStream != null) 
                            {
                                if (timeout == -1)  NetStream.Close();
                                else                NetStream.Close(timeout); 
                                NetStream.Dispose();  NetStream = null; 
                            }
                            if (TCP_Socket != null) {  TCP_Socket.Close();   TCP_Socket = null; }

                            return true;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                        finally { }

                        return false;
                    }


                    //*******
                    public int Write(byte[] byteBuffer, int startIndex, int size, int timeout = -1)
                    {
                        try
                        {
                            //***********   Sending
                            if (IsConnected)
                            {
                                // Send the encoded string to the server
                                NetStream.Write(byteBuffer, startIndex, size);
                                Sent_bytes += size;
                                return size;
                            }
                            return -1;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                        finally {}

                        return -1;
                    }


                    public int Read(byte[] byteBuffer, int startIndex, int buffSize, int timeout = -1)
                    {
                        try
                        {
                            if (!IsConnected)  return -1;
                            //---
                            DateTime time = DateTime.Now;  //  Waiting Data 
                            do
                            {   if (NetStream.DataAvailable)
                                {
                                    int bytesRcvd = NetStream.Read(byteBuffer, startIndex, buffSize);
                                    if (bytesRcvd == 0) return -1; 
                                    return Received_bytes += bytesRcvd;
                                }
                            } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);
                            return 0;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                        finally {}
                        return -1;
                    }  

                }  //  class  Client

//***************************************************


//**********************************************************************
//******
//*********    TCPIP  SERVER  THREAD    ECHO    TCPIP  SERVER  THREAD
//******
//**********************************************************************

/* 18-01-2019 Переделать как Client по олнопоточное исполнение
 * 
                public class TCP_IP_Server_Thread : DependencyObject
                {
                    public int Current_buff_size { get; set; } // Size of receive buffer
                    public int Current_port { get; set; } // Size of receive buffer
                    public Task Thread_main { get; set; }
                    public List<TcpClient> Client_list;

                    public int Received_bytes { get; set; }
                    public int Sent_bytes { get; set; }

                    public bool IsConnected { get ; set; }
                    public bool IsActiveConnection { get { return Client_list.Count != 0 ? true : false; } }
                    public bool Closed_thread_main { get; set; }

                    public bool Close_server { get; set; }
                    public bool Closed { get; set; }

                    const int _Socket_BuffSize = 1500;

                    public int userTimeOut { get ; set; }

//***********    CONSTRUCTOR

                    public TCP_IP_Server_Thread(int port, int timeout=-1 , int buff_size = _Socket_BuffSize)
                    {
                        try
                        {
                            Current_port = port;
                            Current_buff_size = buff_size;
                            Client_list  = new List<TcpClient>();

                            Close_server = false;
                            Closed = false;

                            Received_bytes = 0 ;
                            Sent_bytes = 0 ;

                            IsConnected = false;
                            Thread_main = new Task(Run_Thread);
                            Thread_main.Start();
                            Closed_thread_main = false;

                            userTimeOut = timeout;

                            //  ожидание IsConnected
                            DateTime time = DateTime.Now;
                            do
                            {
                                if (IsConnected || Closed_thread_main) break;
                            } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);
                        }
                        catch (Exception excp)
                        {
                            MessageBox.Show(excp.ToString());
                        }
                    }


                    public bool Close(int timeout = -1)
                    {
                        bool closed_tst = true;
                        try
                        {
                            if (Thread_main != null)
                            {
                                Close_server = true;
                                //  ожидание 
                                if (timeout == -1)  Thread_main.Wait();
                                else closed_tst &= Thread_main.Wait(timeout);
                            }
                            //finally
                            {
                                if (Thread_main != null) { Thread_main.Dispose(); Thread_main = null; }
                                IsConnected = false;
                            }
                        }
                        catch (Exception excp)
                        {
                            MessageBox.Show(excp.ToString());
                            closed_tst = false;
                            //Environment.Exit(excp.ErrorCode);
                        }
                        finally
                        {
                        //    if (Current_thread != null) { Current_thread.Dispose(); Current_thread = null; }
                        //    IsConnected = false;
                        }

                        return Closed = closed_tst;

                    }

             //*******
                    public void Run_Thread()
                    {
                        TcpListener listener = null;

                        try
                        {
                                // Create a TCPListener to accept client connections
                            listener = new TcpListener(IPAddress.Any, Current_port);
                             
                            if (listener == null)  throw new Exception(this.ToString() + "\nError: <new TcpListener(IPAddress.Any, Current_port)>");  
                            IsConnected = true;

                            listener.Start();

                            byte[] rcvBuffer = new byte[Current_buff_size]; // Receive buffer

                            for (; !Close_server ;)
                            { // Run forever, accepting and servicing connections
                                TcpClient client = null;
                                NetworkStream netStream = null;

                                try
                                {
                                    if (!listener.Pending()) continue;

                                    client = listener.AcceptTcpClient(); // Get client connection
                                    Client_list.Add(client);
                                    netStream = client.GetStream();
                                    if (netStream != null)
                                    {
                                        // Receive until client closes connection, indicated by 0 return value
                                        while (!Close_server && client.Connected)
                                        {
                                            if (netStream.DataAvailable)
                                            {
                                                int bytesRcvd = 0;

                                                if ((bytesRcvd = netStream.Read(rcvBuffer, 0, rcvBuffer.Length)) == 0) break;

                                                Received_bytes += bytesRcvd;

                                                netStream.Write(rcvBuffer, 0, bytesRcvd);
                                                Sent_bytes += bytesRcvd;
                                            }
                                        }
                                    }
                                }
                                catch (Exception excp)  { MessageBox.Show(excp.ToString()); }
                                finally
                                {
                                    if (netStream != null) { netStream.Close(userTimeOut); netStream = null; }
                                    if (client != null) { client.Close(); Client_list.Remove(client); client = null; }
                                }
                            }
                        }
                        catch (Exception excp)
                        {
                            MessageBox.Show(excp.ToString()); 
                            //Environment.Exit(excp.ErrorCode);
                        }
                        finally
                        {
                            if (listener != null) { listener.Stop(); listener = null; }
                            Closed_thread_main = true;
                        }
                    }

                }  //  class  Server
*/
}  // class TCP_IP_



//**********************************************************************

//*********       IP ADRESS       UIElement         IP ADRESS   

//**********************************************************************

            //****    EXAMPLE:
            //
            //    //IP_Address_Box ip_box = new IP_Address_Box("192.168.1.100");
            //    IP_Address_Box ip_box = new IP_Address_Box(new IPAddress(new byte[] { 192, 168, 1, 100 }));
            //    ip_box.Header = "IP_Adress";


            public class IP_Address_Box : Viewbox //GroupBox  //Viewbox 
            {
                GroupBox Groupbox { get; set; }

                public string Header { get { return (string)Groupbox.Header; } set { Groupbox.Header = value; } }
                public double FontSize { get { return (double)Groupbox.FontSize; } set { Groupbox.FontSize = value; } }

                //---

                public string IP_Address
                {
                    get { return (string)GetValue(IP_AdrressProperty); }
                    set { SetValue(IP_AdrressProperty, value); }
                }
                public static readonly DependencyProperty IP_AdrressProperty =
                                       DependencyProperty.Register("IP_Adrress", typeof(string), typeof(IP_Address_Box));
                //---


                List<TextBox> Textbox_list;
                string old_value_buff; // запоминание предыдущего значения поля при нажатии клавиши для его востановления при отпускании клавиши.


                //*********  Constructors

                public IP_Address_Box(IPAddress ip_address) : this(ip_address.ToString()) { }

                public IP_Address_Box(string ip_address) : base()
                {

                    try
                    {
                        IPAddress ip_address2;

                        if (IPAddress.TryParse(ip_address, out ip_address2)) IP_Address = ip_address;
                        else IP_Address = IPAddress.Loopback.ToString();

                        string[] list = IP_Address.Split(new char[] { '.' });

                        Textbox_list = new List<TextBox>();
                        old_value_buff = null;

                        Groupbox = new GroupBox();

                            StackPanel stackpanel = new StackPanel();
                            stackpanel.Orientation = Orientation.Horizontal;

                                for (int i = 0; i < 7; i++)
                                {
                                    if (i % 2 == 0)
                                    {
                                        TextBox textbox = Get_TextBox(i/2);
                                        Textbox_list.Add(textbox);
                                        stackpanel.Children.Add(textbox);
                                    }
                                    else stackpanel.Children.Add(Get_TextBlock());
                                }

                        Groupbox.Content = stackpanel;

                        this.Child = Groupbox;

                //**********    Копирование вставка содержимого IP_Box
                        this.ContextMenu = new ContextMenu();
                        List<MenuItem> items_list = new List<MenuItem>();
                        this.ContextMenu.ItemsSource = items_list;
                                MenuItem item = new MenuItem();
                                item.Header = "Copy";  item.Tag = this;
                                item.Click += item_Copy_Click;
                            items_list.Add(item);
                                item = new MenuItem();
                                item.Header = "Insert"; item.Tag = this;
                                item.Click += item_Insert_Click;
                            items_list.Add(item);



                        ////stackpanel.Measure(new Size(500, 500));
                        //stackpanel.Arrange(new Rect());
                        //object obj = stackpanel.Width;
                        //obj = stackpanel.ActualWidth;
                        //obj = stackpanel.RenderSize.Width;

                        //this.Measure(new Size(500, 500));
                        //this.Arrange(new Rect());
                        //obj = this.Width;
                        //obj = this.ActualWidth;
                        //obj = this.RenderSize.Width;


                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }
                }


//*******   COPY INSERT
                
                void item_Copy_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        Clipboard.SetText( ((IP_Address_Box)((MenuItem)sender).Tag).IP_Address);
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }
                }

                void item_Insert_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        string str = Clipboard.GetText();
                        IPAddress addr;
                        if (IPAddress.TryParse(str, out addr)) 
                            ((IP_Address_Box)((MenuItem)sender).Tag).IP_Address = str;
                        else System.Media.SystemSounds.Beep.Play();
                        
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }
                }


//***********   MOVE FOCUS

                void textbox_LostFocus(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        TextBox textbox = (TextBox)sender;

                        int res=0;
                        Int32.TryParse(textbox.Text, out res);
                        if (res > 255)
                        {
                            textbox.Text = "255";// old_value_buff;   // запоминание предыдущего значения поля при нажатии клавиши для ешго востановления при отпускании клавиши.
                            //Thread.Sleep(200);
                            System.Media.SystemSounds.Beep.Play(); //.Asterisk.Play();.Exclamation.Play();.Hand.Play();.Question.Play();
                            //---
                        }

                            StringBuilder str = new StringBuilder();

                            //  Считываем все 4-поля.
                            for (int i = 0; i < 4; i++)
                            {
                                if (Textbox_list[i].Text == "") str.Append("0");
                                else str.Append(Textbox_list[i].Text);

                                if (i != 3) str.Append(".");
                            }
                            //  Если адрес изменился то обновляем свойство класса.
                            //  Если адрес изменился то обновляем свойство класса:               
                            //  при обновлении сработает сразу Binding и выведет строку в поле.
                            IP_Address = str.ToString();
                            //---

                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }

                }


                void textbox_KeyUpMoveFocus(object sender, KeyEventArgs e)
                {
                    try
                    {
                        TextBox textbox = (TextBox)sender;
                            //  Перемещаем курсор в следующее поле если ввели 3 цифры.
                        if (e.Key == Key.Enter || 
                                (textbox.Text.Length >= 3 && e.Key >= Key.D0 && e.Key <= Key.D9)) textbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }
                }

                //*******   KEYs FILTER

                void textbox_PreviewKeyDownFiltr(object sender, KeyEventArgs e)
                {
                    try
                    {
                        if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Enter
                                                                && e.Key != Key.Tab && e.Key != Key.LeftShift
                                                                && e.Key != Key.Left && e.Key != Key.Right
                                                                                                            ) e.Handled = true;
                        else if (((TextBox)sender).Text.Length >= 3 && (e.Key >= Key.D0 && e.Key <= Key.D9)) e.Handled = true;
                        else    old_value_buff = ((TextBox)sender).Text; // запоминание предыдущего значения поля при нажатии клавиши для ешго востановления при отпускании клавиши.
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }
                }

                //*******   TEXTBOXs

                TextBox Get_TextBox(int num)
                {
                    TextBox textbox = new TextBox();
                    textbox.MinWidth = 30;
                    textbox.Padding = new Thickness(2, 0, 2, 0);
                    textbox.BorderThickness = new Thickness(0);
                    textbox.Background = new SolidColorBrush(Colors.Transparent);
                    textbox.VerticalAlignment = VerticalAlignment.Center;
                    textbox.HorizontalContentAlignment = HorizontalAlignment.Center;

                    textbox.PreviewKeyDown += textbox_PreviewKeyDownFiltr;
                    textbox.KeyUp += textbox_KeyUpMoveFocus;
                    textbox.LostFocus += textbox_LostFocus;

                    textbox.Tag = num;
                    Binding binding = new Binding();
                    binding.Source = this;
                    binding.Path = new PropertyPath(IP_Address_Box.IP_AdrressProperty);
                    binding.Converter = new TextBox_Converter();
                    binding.ConverterParameter = textbox;
                    textbox.SetBinding(TextBox.TextProperty, binding);

                    return textbox;
                }


                TextBlock Get_TextBlock()
                {
                    TextBlock textbox = new TextBlock();
                    textbox.Text = ".";
                    textbox.VerticalAlignment = VerticalAlignment.Center;
                    textbox.Background = new SolidColorBrush(Colors.Transparent);
                    return textbox;
                }



                public class TextBox_Converter : IValueConverter
                {
                    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    {
                        try  //  находим индекс текущего item в коллекции.
                        {
                            if (value != null)
                            {
                                string[] list = ((string)value).Split(new char[] { '.' });

                                if (((TextBox)parameter).Tag != null) return list[(int)((TextBox)parameter).Tag];
                                else return "";
                            }
                            return Binding.DoNothing;
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
                            return Binding.DoNothing;
                        }
                        catch (Exception excp)
                        {
                            MessageBox.Show(excp.ToString());
                            return null;
                        }
                    }

                }
                //***********

                // Сократил уровни вложенности  18-01-2019 
                ////**********************************************************************

                ////*********       TCPIP CONNECTION       TCPIP CONNECTION       TCPIP CONNECTION

                ////**********************************************************************

                //            public class TCP_IP_Connection : DependencyObject
                //            {
                //                public string Server_Name
                //                {
                //                    get { return (string)GetValue(Server_NameProperty); }
                //                    set { SetValue(Server_NameProperty, value); }
                //                }
                //                public static readonly DependencyProperty Server_NameProperty =
                //                                         DependencyProperty.Register("Server_Name", typeof(string), typeof(TCP_IP_Connection));

                //                public string sServer_Port { get; set ; }
                //                public int iServer_Port
                //                {
                //                    get { short ax = 0; Int16.TryParse(sServer_Port, out  ax); return ax; }
                //                    set { sServer_Port = value.ToString(); }
                //                }


                //                public TCP_IP_Connection(string serv_address, string serv_port, int timeout = -1, int buff_size = _Socket_BuffSize)
                //                {
                //                    Server_Name = serv_address;// IPAddress.Loopback.ToString();
                //                    sServer_Port = serv_port;// = 7
                //                }


                //                //************  GET HOST INFO

                //                public static string GetHostInfo(String host)
                //                {
                //                    try
                //                    {
                //                        StringBuilder str = new StringBuilder();
                //                        IPHostEntry hostInfo;

                //                        // Attempt to resolve DNS for given host or address
                //                        hostInfo = Dns.GetHostEntry(host); //Dns.Resolve(host);
                //                        // Display the primary host name

                //                        str.AppendLine("\tCanonical Name: " + hostInfo.HostName);

                //                        // Display list of IP addresses for this host
                //                        str.Append("\tIP Addresses: ");
                //                        foreach (IPAddress ipaddr in hostInfo.AddressList) { str.Append("\n\t\t" + ipaddr.ToString()); }

                //                        str.AppendLine();

                //                        // Display list of alias names for this host
                //                        str.Append("\tAliases: ");
                //                        foreach (String alias in hostInfo.Aliases) { str.Append("\n\t\t" + alias); }

                //                        str.AppendLine();

                //                        return str.ToString();
                //                    }
                //                    catch (Exception excp)
                //                    {
                //                        MessageBox.Show(excp.ToString());
                //                        return null;
                //                    }
                //                }

                //                //************  TCPIP ECHO CLIENT

                //                public string EchoTest(string addr, int port, string echo_string)
                //                {

                //                    StringBuilder str = new StringBuilder();

                //                    try
                //                    {
                //                        StringBuilder strb = new StringBuilder();

                //                        // Convert input String to bytes
                //                        byte[] byteBuffer = Encoding.ASCII.GetBytes(echo_string);

                //                        //***********  RUN THREADS

                //                        // Send the encoded string to the server
                //                        Client.Sending_stream.Write(byteBuffer, 0, byteBuffer.Length);
                //                        str.AppendLine("Sent <" + byteBuffer.Length + "> bytes to server...");

                //                        int totalBytesRcvd = 0; // Total bytes received so far
                //                        int bytesRcvd = 0; // Bytes received in last read

                //                        // Receive the same string back from the server
                //                        while (totalBytesRcvd < byteBuffer.Length)
                //                        {
                //                            if ((bytesRcvd = Client.Receiving_stream.TryRead(byteBuffer, totalBytesRcvd,
                //                                                            byteBuffer.Length - totalBytesRcvd)) > 0)
                //                            {
                //                                str.AppendLine("Received <" + bytesRcvd + "> bytes");
                //                                totalBytesRcvd += bytesRcvd;
                //                            }
                //                        }

                //                        str.AppendLine("Received total <" + totalBytesRcvd + "> bytes from server: <" +
                //                                            Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd) + ">");

                //                    }
                //                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                //                    finally { }

                //                    return str.ToString();
                //                }                



                //}  //******** TCP_IP_CONNECTION


//******** TCP_IP
//        }

//******** Step5
    }
}

//****************      ZIP 

//public void UploadFile(string filename) 
//    { 
//     using (var temporaryFileStream = File.Open("tempfile.tmp", FileMode.CreateNew, FileAccess.ReadWrite)) 
//     { 
//      using (var fileStream = File.OpenRead(filename)) 
//      using (var compressedStream = new GZipStream(temporaryFileStream, CompressionMode.Compress, true)) 
//      { 
//       fileStream.CopyTo(compressedStream); 
//      } 

//      temporaryFileStream.Position = 0; 

//      Uploader.Upload(temporaryFileStream); 
//     } 
//    } 