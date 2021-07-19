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

//   Modbus_ListIO_MultiServer : Modbus_ListIO : Modbus_IO : Modbus_FunctionData : Modbus_Frame : TCP_IP_Client_Thread

        public class ConcurrentList<T1> : ConcurrentDictionary<int, T1>
        {
            //ConcurrentDictionary<int, T1> list;
            int Key;

            public ConcurrentList() : base() { Key = 0; }

            int key(int index)
            {
                List<int> keys = this.Keys.ToList();
                    if (index < 0 || index >= keys.Count) throw new Exception("Index out of range!");
                return keys[index];
            }

            public bool Add(T1 value) {     return this.TryAdd(Key++, value) ;}

            public bool GetValue(int index, out T1 value)  
            {
                value = default(T1);
                try { return this.TryGetValue(key(index), out value); }
                catch (Exception) { return false; }
            }

            public bool GetValue(int index, int count, out ConcurrentList<T1> list)
            {
                list = new ConcurrentList<T1>();
                T1 value;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        if (this.TryGetValue(key(index + i), out value)) list.Add(value);
                        else return false;
                    }
                    catch (Exception) { return false; }
                }
                return true;
            }

            public bool Remove(int index, int count = 1)
            {
                T1 value;
                //     Не увеличиваем индекс на 1, т.к. при удалении элемента следующий 
                //  за ним сам смещается на место удаленного т.е. на тот же индекс.
                //for (int i = 0; i < count; i++) { if (!this.TryRemove(key(index + i), out value)) return false; }
                try
                {
                   for (int i = 0; i < count; i++) { if (!this.TryRemove(key(index), out value)) return false; }
                }
                catch (Exception) { return false; }

                return true; 
            }


            public bool Take(int index, out T1 value) { return this.TryRemove(key(index), out value); }

            public bool Take(int index, int count, out ConcurrentList<T1> list)
            {
                list = new ConcurrentList<T1>();
                T1 value;
                for (int i = 0; i < count; i++) 
                {
                    //     Не увеличиваем индекс на 1, т.к. при удалении элемента следующий 
                    //  за ним сам смещается на место удаленного т.е. на тот же индекс.
                    //if (this.TryRemove(key(index + i), out value)) list.Add(value);
                    try
                    {
                        if (this.TryRemove(key(index), out value)) list.Add(value);
                        else return false; 
                    }
                    catch (Exception) { return false; }

                }
                return true; 
            }

            public int IndexOf(T1[] str)
            {
                try
                {
                    int i, j=-1;
                    for (i = 0; i < this.Count - str.Length+1; )
                    {
                        for (j = 0; j < str.Length;)
                        {
                            if (!this[key(i++)].Equals(str[j++]))
                            {
                                break;
                            }
                        }
                        if (j == str.Length) break;
                    }

                    if (j == str.Length) return i - str.Length;
                    else return -1;
                }
                catch (Exception) { return -1; }
            }

        }



//**********************************************************************
//******
        //*********    MODBUS FRAME CLIENT      MODBUS FRAME CLIENT
//******
//**********************************************************************

        public class Modbus_DataFrame
        {
                public int Transaction_ID { get; set; } // <any code>
                public int Protocol_ID { get; set; } // <0>
                public int Data_Length { get; set; } // длина посылки начиная с Unit_ID.
                public byte Unit_ID { get; set; }      // <0>  
                public byte Function_Code { get; set; }
                public byte[] Data_Array;

                public Modbus_DataFrame() 
                {
                    Transaction_ID = 0; Protocol_ID = 0; Data_Length = 0;
                    Unit_ID = 0; Function_Code = 0; Data_Array = null;
                }

                public Modbus_DataFrame( int transaction_id, int protocol_id, int data_length,
                                         byte unit_id, byte function_code, byte[] data_array)
                {
                        Transaction_ID = transaction_id; Protocol_ID = protocol_id; Data_Length = data_length;
                        Unit_ID = unit_id; Function_Code = function_code; Data_Array = data_array;
                }

                public string ToString(string divider1 = "\n", string divider2 = ", ")
                {
                    StringBuilder data_str = new StringBuilder();
                    int i;
                    for ( i = 0 ; i < Data_Array.Length-1; i++) data_str.Append(Data_Array[i] + divider2);
                    data_str.Append(Data_Array[i]);

                    return "Transaction_ID = " + Transaction_ID + divider1 +
                            "Protocol_ID = " + Protocol_ID + divider1 +
                            "Data_Length = " + Data_Length + divider1 +
                            "Unit_ID = " + Unit_ID + divider1 +
                            "Function_Code = " + Function_Code + divider1 +
                            "Data[" + Data_Array.Length + "] = { " + data_str.ToString() + " }";
                }
                
        }



        public class Modbus_Frame_Client : TCP_IP_Client_Thread
        {
            public ConcurrentList<byte> ReceivedData_buff;
         

//***********    CONSTRUCTOR

                public Modbus_Frame_Client( string serv_address, int serv_port ) : base ( serv_address, serv_port)
                {
                    ReceivedData_buff = new ConcurrentList<byte>();
                    //Receiving_stream.Data_Changed.AddedPropertyChanged_Event += ReceivedData_Event;                                        
                }


                new public bool Close(int timeout = -1)  
                {
                    bool closed = false;
                    try
                    {
                        closed = base.Close(timeout);
                        ReceivedData_buff.Clear();
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }

                    return closed;
                }


            public bool TrySend(Modbus_DataFrame frame)
            {
                return Send(frame, -1);
            }


            //  timeout = 0 - ждать до бесконечности 
            //          = time - ждать time-милисекунд     
            //          = -1 - проверить есть-нет и выходить не ождая
            public bool Send(Modbus_DataFrame frame, int timeout = 0)
            {
                try
                {
                    //  ожидание данных
                    //int time = DateTime.Now.Millisecond;
                    DateTime time = DateTime.Now;
                    do
                    {
                        if (!IsConnected) continue ;

                        List<byte> byte_list = new List<byte>();

                        //  Записываем двухбайтные данные старшим байтом вперед.
                        int ax = frame.Transaction_ID; byte_list.Add((byte)(ax >> 8)); byte_list.Add((byte)ax);
                        ax = frame.Protocol_ID; byte_list.Add((byte)(ax >> 8)); byte_list.Add((byte)ax);
                        ax = frame.Data_Length; byte_list.Add((byte)(ax >> 8)); byte_list.Add((byte)ax);
                        ax = frame.Unit_ID; byte_list.Add((byte)ax);
                        ax = frame.Function_Code; byte_list.Add((byte)ax);

                        for (int i = 0; frame.Data_Array != null && i < frame.Data_Array.Length; i++) byte_list.Add(frame.Data_Array[i]);

                        Sending_stream.Write(byte_list.ToArray());

                        return true;

                    } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);
                    //} while (timeout == 0 || (time2=(TimeSpan)(DateTime.  Now.Millisecond - time)) <= (uint)timeout);

                }
                catch (Exception excp) { MessageBox.Show( excp.ToString()); return false; }
                finally { }

                return false;
            }


            public bool TryReceive(Modbus_DataFrame sent_frame, out Modbus_DataFrame received_frame)
            {
                return Receive( sent_frame, out received_frame, -1);
            }


            //  timeout = 0 - ждать до бесконечности 
            //          = time - ждать time-милисекунд     
            //          = -1 - проверить есть-нет и выходить не ождая
            public bool Receive(Modbus_DataFrame sent_frame, out Modbus_DataFrame received_frame, int timeout = 0)
            {
                received_frame = null;

                try
                {
                    //  По отосланной посылке определяем искомый ответный Header.
                    List<byte> header_list = new List<byte>();
                    //  Записываем двухбайтные данные старшим байтом вперед.
                    int ax = sent_frame.Transaction_ID; header_list.Add((byte)(ax >> 8)); header_list.Add((byte)ax);
                    ax = sent_frame.Protocol_ID; header_list.Add((byte)(ax >> 8)); header_list.Add((byte)ax);

                    //  ожидание данных
                    DateTime time = DateTime.Now;
                    do
                    {
                        if (!IsConnected) continue;


                            //****************     Переносим все полученное из FIFO-буфера TCP_IP в  буфер c произвольным доступом MODBUS

                            byte[] buff = new byte[1];
                            while (this.Receiving_stream.TryRead(buff, 0, buff.Length) > 0) ReceivedData_buff.Add(buff[0]);

                            //****************     Выискиваем требуемую посылку в любом месте буфера MODBUS и считываем ее с удалением - буфер смыкается.

                            //  Проверяем есть ли Header требуемого сообщения в принятых данных.
                            int index = ReceivedData_buff.IndexOf(header_list.ToArray());

                            if (index < 0) continue;

                            ConcurrentList<byte> temp;

                            if (!ReceivedData_buff.GetValue(index, 2 + 2 + 2 + 1 + 1, out temp)) continue;
                            byte[] array = temp.Values.ToArray();

                            Modbus_DataFrame frame = new Modbus_DataFrame();

                            //  Считываем двухбайтные данные старшим байтом вперед.
                            int i = 0;
                            frame.Transaction_ID = ((int)array[i++] << 8) + (int)array[i++];
                            frame.Protocol_ID = ((int)array[i++] << 8) + (int)array[i++];
                            frame.Data_Length = ((int)array[i++] << 8) + (int)array[i++];
                            frame.Unit_ID = array[i++];
                            frame.Function_Code = array[i++];

                            if (!ReceivedData_buff.GetValue(index + 8, frame.Data_Length - 2, out temp)) continue;

                            frame.Data_Array = temp.Values.ToArray();

                            //  Посылка считана - удаляем ее из буфера.
                            ReceivedData_buff.Remove(index, 8 + frame.Data_Length - 2);

                            received_frame = frame;

                            return true;

                        } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);
                        //} while (timeout == 0 || (time2=(TimeSpan)(DateTime.Now.Millisecond - time)) <= (uint)timeout);


                        return false;

                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }

                return false;
            }


            //********   HANDLER  RECEIVED DATA Changed

            //void ReceivedData_Event(object sender, DependencyPropertyChangedEventArgs e)
            //{
            //    try
            //    {
            //        ProducerConsumer_QueueStream stream = ((ProducerConsumer_QueueStream.DataChanged)sender).Parent_stream;

            //        byte[] buff = new byte[1];
            //        while (stream.TryRead(buff, 0, buff.Length) > 0) ReceivedData_buff.Add(buff[0]);
            //    }
            //    catch (Exception excp) { MessageBox.Show(excp.ToString()); }

            //}


         

        }  //  Modbus_Frame



//**********************************************************************

//*********       MODBUS TCPIP       UIElement         MODBUS TCPIP   

//**********************************************************************


public class ModbusTCPIP_ADU
{
    public enum ItemType {Byte, Word};

    //****************  ADU_Data

    public class ADU_Data : DependencyObject
    {
        private ItemType type;
        //private string svalue;

        public string Name;

        public ADU_Data() { Name = null; type = ItemType.Word; sValue = null; }
        public ADU_Data(string name, ItemType xtype) { Name = name; type = xtype; sValue = null; }
        public ADU_Data(string name, ItemType xtype, string data) { Name = name; type = xtype; sValue = null; sValue = data; }
        public ADU_Data(string name, ItemType xtype, UInt16 data) { Name = name; type = xtype; sValue = null; iValue = data; }


        public ItemType Type { get { return type; } }

        public string sValue
        {
            get { return (string)GetValue(svalueProperty); }
            set { SetValue(svalueProperty, value); }
        }
        public static readonly DependencyProperty svalueProperty =
                                 DependencyProperty.Register("svalue", typeof(string), typeof(ADU_Data), new PropertyMetadata(new PropertyChangedCallback(svaluePropertyChanged_Callback)));
        
        public UInt16? iValue
        {
            get
            {
                if (String.IsNullOrWhiteSpace(sValue)) return null;
                if (Type == ItemType.Byte) { Byte ax; if (Byte.TryParse(sValue, out ax)) return ax; }
                else { UInt16 ax; if (UInt16.TryParse(sValue, out ax)) return ax; }
                return null;
            }
            set { sValue = value.ToString(); }
        }

        public Binding Get_Binding()
        {
               try
                {
                    Binding binding = new Binding();
                    binding.Source = this;
                    binding.Path = new PropertyPath("svalue");
                    binding.Mode = BindingMode.TwoWay;
                    return binding;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }
                return null;
        }

        static void svaluePropertyChanged_Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ADU_Data adu = (ADU_Data)d;
            string str = (string)e.NewValue;

            if (String.IsNullOrWhiteSpace(str))  return ;
            //  если значение недопустимое то оставляем без изменений прежнее.
            else if (adu.Type == ItemType.Byte) { Byte ax; if (Byte.TryParse(str, out ax)) return; }
            else { UInt16 ax; if (UInt16.TryParse(str, out ax)) return; }

            adu.SetValue(svalueProperty, (string)e.OldValue);
          
        }

    }

    //**************

    public ItemType Data_Type;

    public ADU_Data Transaction_ID {get; set;}
    public ADU_Data Protocol_ID { get; set; }
    public ADU_Data Data_Length { get; set; }
    public ADU_Data Unit_ID { get; set; }
    public ADU_Data Function_Code { get; set; }
    public ADU_Data[] Data_Array;
        

    public ModbusTCPIP_ADU(ItemType data_type, int data_count )
    {

        Transaction_ID  = new ADU_Data("Transaction ID", ItemType.Word);
        Protocol_ID = new ADU_Data("Protocol ID", ItemType.Word);
        Data_Length = new ADU_Data("Length", ItemType.Word);
        Unit_ID = new ADU_Data("Unit ID", ItemType.Byte);
        Function_Code = new ADU_Data("Function Code", ItemType.Byte);
        Data_Array = new ADU_Data[data_count];

        Data_Type = data_type;
        if (data_type == ItemType.Byte)    for (int i = 0; i < data_count; i++) Data_Array[i] = new ADU_Data("Byte"+i.ToString(), ItemType.Byte);
        else                                for (int i = 0; i < data_count; i++) Data_Array[i] = new ADU_Data("Word"+i.ToString(), ItemType.Word);

    }
    
            public StackPanel Get_UIElement()
            {
                try
                {
                    StackPanel stackpanel = new StackPanel();
                    stackpanel.Orientation = Orientation.Horizontal;
                    stackpanel.VerticalAlignment = VerticalAlignment.Top;

                    stackpanel.Children.Add(GetGroupBox(Transaction_ID));
                    stackpanel.Children.Add(GetGroupBox(Protocol_ID));
                    stackpanel.Children.Add(GetGroupBox(Data_Length));
                    stackpanel.Children.Add(GetGroupBox(Unit_ID));
                    stackpanel.Children.Add(GetGroupBox(Function_Code));

                    for (int i = 0; i < Data_Array.Length; i++) stackpanel.Children.Add(GetGroupBox(Data_Array[i]));

                    return stackpanel;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }
                return null;
            }

            private GroupBox GetGroupBox(ADU_Data source)
            {
                try
                {
                    GroupBox groupbox = new GroupBox();
                    groupbox.Header = source.Name;
                        TextBox textbox = new TextBox();
                        textbox.TextAlignment = TextAlignment.Center;
                            //Binding binding = new Binding();
                            //binding.Source = source;// this;
                            //binding.Path = new PropertyPath("sValue");
                            //binding.Mode = BindingMode.TwoWay;
                        textbox.SetBinding(TextBox.TextProperty, source.Get_Binding());
                        textbox.KeyUp += textbox_KeyUp;
                    groupbox.Content = textbox;

                    return groupbox;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }

                return null;

            }

            void textbox_KeyUp(object sender, KeyEventArgs e)
            {
                try
                {
                    if (e.Key == Key.Enter) ((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }
                
            }

            public string GetValue( out byte[] byte_array)
            {
                byte_array = null;
                List<byte>  data_list = new List<byte>();

                try
                {
                    Convert_ADU_data(Transaction_ID, data_list);
                    Convert_ADU_data(Protocol_ID, data_list);
                    Convert_ADU_data(Data_Length, data_list);
                    Convert_ADU_data(Unit_ID, data_list);
                    Convert_ADU_data(Function_Code, data_list);

                    for (int i = 0; i < Data_Array.Length && 
                                        Data_Array[i].iValue != null; i++) Convert_ADU_data(Data_Array[i], data_list);

                    byte_array = data_list.ToArray();
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return excp.ToString(); }
                finally { }

                return null;
            }

            public string GetValue(out Modbus_DataFrame frame)
            {
                frame = new Modbus_DataFrame();
                List<byte> data_list = new List<byte>();

                try
                {
                    frame.Transaction_ID = (int)Transaction_ID.iValue;
                    frame.Protocol_ID = (int)Protocol_ID.iValue;
                    frame.Data_Length = (int)Data_Length.iValue;
                    frame.Unit_ID = (byte)Unit_ID.iValue;
                    frame.Function_Code = (byte)Function_Code.iValue;

                    for (int i = 0; i < Data_Array.Length &&
                                        Data_Array[i].iValue != null; i++) Convert_ADU_data(Data_Array[i], data_list);

                    frame.Data_Array = data_list.ToArray();
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return excp.ToString(); }
                finally { }

                return null;
            }


            private string Convert_ADU_data(ADU_Data adu_data,  List<byte> byte_list)
            {
                try
                {
                        UInt16? ax = adu_data.iValue;

                        if (ax == null)  ax = 0;
                        //  Записываем старшим байтом вперед.
                        if (adu_data.Type == ItemType.Word) byte_list.Add((byte)(ax >> 8));
                        byte_list.Add((byte)ax);
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return excp.ToString(); }
                finally { }

                return null;
            }


            public string SetValue(byte[] byte_array, int count = -1)
            {
                try
                {
                    if (count > byte_array.Length) count = byte_array.Length;

                    if (count == -1) count = byte_array.Length;

                    int i = 0;
                    if (count - i >= 2) Transaction_ID.iValue = (ushort)((ushort)byte_array[i++] * 256u + (ushort)byte_array[i++]);
                    if (count - i >= 2) Protocol_ID.iValue = (ushort)((ushort)byte_array[i++] * 256u + (ushort)byte_array[i++]);
                    if (count - i >= 2) Data_Length.iValue = (ushort)((ushort)byte_array[i++] * 256u + (ushort)byte_array[i++]);
                    if (count - i >= 1) Unit_ID.iValue = byte_array[i++];
                    if (count - i >= 1) Function_Code.iValue = byte_array[i++];

                    for (int j = 0; j < Data_Array.Length; j++) 
                    {
                        if (Data_Array[j].Type == ItemType.Word)
                        {
                            if (count - i >= 2) Data_Array[j].iValue = (ushort)((ushort)byte_array[i++] * 256u + (ushort)byte_array[i++]);
                            else break;
                        }
                        else
                        {
                            if (count - i >= 1) Data_Array[j].iValue = byte_array[i++];
                            else break;
                        }
                    }
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return excp.ToString(); }
                finally { }

                return null;
            }

            public string SetValue(Modbus_DataFrame frame)
            {
                try
                {
                    Transaction_ID.iValue = (ushort)frame.Transaction_ID;
                    Protocol_ID.iValue = (ushort)frame.Protocol_ID;
                    Data_Length.iValue = (ushort)frame.Data_Length;
                    Unit_ID.iValue = (ushort)frame.Unit_ID;
                    Function_Code.iValue = (ushort)frame.Function_Code;

                    int count = frame.Data_Array.Length;
                    int i = 0;
                    for (int j = 0; j < Data_Array.Length; j++)
                    {
                        if (Data_Array[j].Type == ItemType.Word)
                        {
                            if (count - i >= 2) Data_Array[j].iValue = (ushort)((ushort)frame.Data_Array[i++] * 256u + (ushort)frame.Data_Array[i++]);
                            else break;
                        }
                        else
                        {
                            if (count - i >= 1) Data_Array[j].iValue = frame.Data_Array[i++];
                            else break;
                        }
                    }
                 
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return excp.ToString(); }
                finally { }

                return null;
            }

//******** Modbus
        }
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