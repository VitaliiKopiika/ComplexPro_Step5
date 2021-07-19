
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

        //**********   ProducerConsumer Stream      ProducerConsumer Stream     : Parallel ThreadSafe Queue Collection

        //**********************************************************************




        public class ProducerConsumer_QueueStream : ConcurrentQueue<byte>  //, DependencyObject
        {
            public ProducerConsumer_QueueStream()
                : base()
            {
                Data_Changed = new DataChanged(this);
            }

            public ProducerConsumer_QueueStream(IEnumerable<byte> collection)
                : base(collection)
            {
                Data_Changed = new DataChanged(this);
            }

            public bool Closed { get; set; }

            public object Tag { get; set; }

            //*****    DATA_CHANGED   CLASS

            public class DataChanged : DependencyObject
            {
                public ProducerConsumer_QueueStream Parent_stream { get; set; }
                public object Tag { get; set; }

                public DataChanged(ProducerConsumer_QueueStream parent_stream)
                    : base()
                {
                    Added = 0;
                    Removed = 0;
                    Parent_stream = parent_stream;
                }

                //*************  PROPERTYs

                public int Added
                {
                    get { return (int)GetValue(AddedProperty); }
                    set { SetValue(AddedProperty, value); }
                }
                public static readonly DependencyProperty AddedProperty =
                                         DependencyProperty.Register("Added", typeof(int), typeof(DataChanged), new PropertyMetadata(new PropertyChangedCallback(AddedPropertyChanged_Callback)));  //Added_PropertyChangedCallback));

                public int Removed
                {
                    get { return (int)GetValue(RemovedProperty); }
                    set { SetValue(RemovedProperty, value); }
                }
                public static readonly DependencyProperty RemovedProperty =
                                         DependencyProperty.Register("Removed", typeof(int), typeof(DataChanged), new PropertyMetadata(new PropertyChangedCallback(RemovedPropertyChanged_Callback)));

                //**************     BINDINGs

                public Binding Get_AddedProperty_Binding()
                {
                    try
                    {
                        Binding binding = new Binding();
                        binding.Source = this;
                        binding.Path = new PropertyPath("Added");
                        binding.ConverterParameter = Parent_stream;
                        binding.Converter = new DataChanged_AddedConverter();
                        binding.Mode = BindingMode.OneWay;

                        return binding;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                }

                public Binding Get_RemovedProperty_Binding()
                {
                    try
                    {
                        Binding binding = new Binding();
                        binding = new Binding();
                        binding.Source = this;
                        binding.Path = new PropertyPath("Removed");
                        binding.ConverterParameter = Parent_stream;
                        binding.Converter = new DataChanged_RemovedConverter();
                        binding.Mode = BindingMode.OneWay;

                        return binding;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                }

                public class DataChanged_AddedConverter : IValueConverter
                {
                    // static int count = 0; int local_count;
                    // public Data_Changed_Added_Converter() : base() { count++; local_count = count; }

                    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    {
                        try
                        {   //  при открывании окна i-тый раз заходит сюда по i-раза на одну 
                            //  принятую посылку - 2-й раз вхолостую - возвращает нуль - затирает текстбокс
                            //  ГДЕ-ТО накапливается коллекция старых байндингов
                            //  очистить старые байдинги перед созданием нового не помогает

                            //   if (local_count != count) return Binding.DoNothing;
                            //---
                            string str = null;
                            ((ProducerConsumer_QueueStream)parameter).TryRead(out str);
                            return str;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    }

                    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    {
                        try
                        {
                            MessageBox.Show("Not handled ConvertBack: DataChanged.Removed");
                            return Binding.DoNothing;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    }
                }

                public class DataChanged_RemovedConverter : IValueConverter
                {
                    //    static int count = 0; int local_count;
                    //   public Data_Changed_Removed_Converter() : base() { count++; local_count = count; }

                    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    {
                        try
                        {
                            //   if (local_count != count) return Binding.DoNothing;
                            return null;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    }

                    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                    {
                        try
                        {
                            MessageBox.Show("Not handled ConvertBack: DataChanged.Removed");
                            return Binding.DoNothing;
                        }
                        catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
                    }
                }

                //***************    EVENTs

                public event EventHandler<DependencyPropertyChangedEventArgs> AddedPropertyChanged_Event;
                public event EventHandler<DependencyPropertyChangedEventArgs> RemovedPropertyChanged_Event;

                //***************    CALLBACKs

                static void AddedPropertyChanged_Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
                {
                    try
                    {
                        DataChanged sender = (DataChanged)d;
                        if (sender.AddedPropertyChanged_Event != null)
                            sender.AddedPropertyChanged_Event(d, e);
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return;
                }

                static void RemovedPropertyChanged_Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
                {
                    try
                    {
                        DataChanged sender = (DataChanged)d;
                        if (sender.RemovedPropertyChanged_Event != null)
                            sender.RemovedPropertyChanged_Event(d, e);
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return;
                }

                //----------
            }
            //*****  END OF DATA_CHANGED


            public DataChanged Data_Changed;

            //****

            public int Write(byte[] buffer, int offset = -1, int count = -1, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {
                    if (count == -1) { offset = 0; count = buffer.Length; }

                    if (this.Closed) return -1;

                    for (int i = offset; i < offset + count; i++) this.Enqueue(buffer[i]);

                    if (dispatcher != null) dispatcher.BeginInvoke((ThreadStart)delegate()
                    {
                        Data_Changed.Added++;
                    });
                    else Data_Changed.Added++;


                    return count;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            public int Write(string str, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {
                    byte[] byteBuffer = Encoding.ASCII.GetBytes(str);

                    return Write(byteBuffer, 0, byteBuffer.Length, dispatcher);
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            //****

            public int WriteFrame(byte[] buffer, int offset = -1, int count = -1, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {
                    if (count == -1) { offset = 0; count = buffer.Length; }

                    //byte [] data_size = new byte[sizeof(Int32)];
                    //for (int i = 0, k = 0; i < data_size.Length ; i++, k+=8) data_size[i] = (byte)(count >> k );

                    byte[] buff2 = new byte[count];
                    for (int i = 0; i < count; i++) buff2[i] = buffer[offset + i];

                    List<byte> list = new List<byte>(Encoding.ASCII.GetBytes("<Frame {"));

                    list = new List<byte>(list.Concat(new List<byte>(buff2)));

                    list = new List<byte>(list.Concat(new List<byte>(Encoding.ASCII.GetBytes("} />"))));

                    return Write(list.ToArray(), 0, list.Count, dispatcher);
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            public int WriteFrame(string str, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {

                    byte[] byteBuffer = Encoding.ASCII.GetBytes(str);

                    return WriteFrame(byteBuffer, 0, byteBuffer.Length, dispatcher);

                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            //*******   READ 

            //   Awaiting Read  -  Ожидание ввода хотя бы одного символа.
            //  timeout = 0 - ждать до бесконечности 
            //          = time - ждать time-милисекунд     
            //          = -1 - проверить есть-нет и выходить не ождая
            public int Read(byte[] buffer, int offset, int count, int timeout = 0, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {
                    for (int i = offset; i < offset + count; i++)
                    {
                        if (this.Closed) return -1;

                        if (!this.TryDequeue(out buffer[i]))
                        {                       //  выход без ожидания данных
                            if (i != offset || timeout == -1) return i - offset;

                            //  ожидание ввода данных
                            DateTime time = DateTime.Now;
                            while (!this.TryDequeue(out buffer[i]))
                            {
                                if (this.Closed) return -1;

                                if (timeout != 0 && (DateTime.Now - time).TotalMilliseconds > timeout) return 0;
                            }
                        }
                    }

                    if (dispatcher != null) dispatcher.BeginInvoke((ThreadStart)delegate()
                    {
                        Data_Changed.Removed++;
                    });
                    else Data_Changed.Removed++;

                    return count;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            public int Read(out string str, int timeout = 0, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                str = null;

                try
                {
                    byte[] byteBuffer = new byte[this.Count];

                    int input_size = Read(byteBuffer, 0, byteBuffer.Length, timeout, dispatcher);

                    if (input_size != 0) str = Encoding.ASCII.GetString(byteBuffer, 0, input_size);

                    return input_size;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            //****

            //   Not Awaiting Read - выход если нет данных для выхода
            public int TryRead(byte[] buffer, int offset, int count, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                return Read(buffer, offset, count, -1, dispatcher);
            }

            public int TryRead(out string str, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                return Read(out str, -1, dispatcher);
            }

            //***

            public int ReadFrame(out byte[] buffer, int timeout = 0, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                buffer = null;

                try
                {

                    //***************   CHECK FRAME

                    byte[] queue_copy = new List<byte>(this).ToArray();

                    byte[] begin_code = Encoding.ASCII.GetBytes("<Frame {");
                    byte[] end_code = Encoding.ASCII.GetBytes("} />");

                    int begin_index = IndexOf(queue_copy, begin_code);
                    int end_index = IndexOf(queue_copy, end_code);

                    if (begin_index != -1 && end_index != -1)
                    {
                        if (end_index > begin_index + begin_code.Length)
                        {

                            //****************  READ FRAME DATA
                            int size = begin_index + begin_code.Length;
                            byte[] buff_tmp = new byte[size];
                            if (Read(buff_tmp, 0, size, timeout, dispatcher) <= 0) return -1;

                            size = end_index - (begin_index + begin_code.Length);
                            buff_tmp = new byte[size];
                            if (Read(buff_tmp, 0, size, timeout, dispatcher) <= 0) return -1;
                            buffer = buff_tmp;

                            size = end_code.Length;
                            buff_tmp = new byte[size];
                            if (Read(buff_tmp, 0, size, timeout, dispatcher) <= 0) return -1;

                            return end_index - (begin_index + begin_code.Length);
                            //****************
                        }
                    }

                    return -1;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            public int ReadFrame(out List<byte[]> buff_list, int timeout = 0, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                buff_list = null;

                try
                {
                    byte[] byteBuffer;
                    int input_size;

                    while ((input_size = ReadFrame(out byteBuffer, timeout, dispatcher)) > 0)
                    {
                        if (buff_list == null) buff_list = new List<byte[]>();

                        buff_list.Add(byteBuffer);
                    }

                    if (buff_list == null) return -1;

                    return buff_list.Count;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            public int ReadFrame(out string str, int timeout = 0, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                str = null;

                try
                {
                    byte[] byteBuffer;

                    int input_size = ReadFrame(out byteBuffer, timeout, dispatcher);

                    if (input_size > 0) str = Encoding.ASCII.GetString(byteBuffer, 0, input_size);

                    return input_size;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }


            public int ReadFrame(out List<string> str_list, int timeout = 0, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                str_list = null;

                try
                {
                    string str;
                    int input_size;

                    while ((input_size = ReadFrame(out str, timeout, dispatcher)) > 0)
                    {
                        if (str_list == null) str_list = new List<string>();

                        str_list.Add(str);
                    }

                    if (str_list == null) return -1;

                    return str_list.Count;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }


            //***

            public int IndexOf(byte[] source, byte[] example)
            {
                try
                {
                    for (int i = 0; i < source.Length; i++)
                    {
                        if (source[i] == example[0])
                        {
                            for (int j = 1; j < example.Length && i + j < source.Length; j++)
                            {
                                if (source[i + j] == example[j] && j == example.Length - 1)
                                {
                                    return i;
                                }
                            }
                        }
                    }
                    return -1;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return -1; }
            }

            //****

            //   Not Awaiting Read - выход если нет данных для выхода
            public int TryReadFrame(out byte[] buffer, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                return ReadFrame(out buffer, -1, dispatcher);
            }

            public int TryReadFrame(out string str, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                return ReadFrame(out str, -1, dispatcher);
            }

            public int TryReadFrame(out List<byte[]> buff_list, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                return ReadFrame(out buff_list, -1, dispatcher);
            }

            public int TryReadFrame(out List<string> str_list, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                return ReadFrame(out str_list, -1, dispatcher);
            }


            //***

            public bool Exists(string str)
            {
                try
                {
                    byte[] byte_str = Encoding.ASCII.GetBytes(str);
                    byte[] byte_queue = new List<byte>(this).ToArray();

                    if (IndexOf(byte_queue, byte_str) != -1) return true;

                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
            }



            //****

            public void Close() { Closed = true; }
            public void Close(int timeout) { Closed = true; }
            //****


        }  // end of class: ProducerConsumerStream
    }
}
