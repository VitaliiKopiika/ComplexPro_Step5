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


//**********************************************************************
//******
        //*********    MODBUS IO CLIENT      MODBUS FRAME CLIENT
//******
//**********************************************************************


        public enum ModbusCommand { Null = 0,   ReadHoldingRegisters = 0x03, 
                                                PresetMultipleRegisters = 16, 
                                                ReadByteArray = 0x47, 
                                                PresetByteArray = 0x48 };
        public enum VarType { Null = 0, Word = 2, sWord = 3, ByteArray = 7 };

        public class Modbus_VarData
        {
            
//*******   CONSTRUCTORS  ********

            private void initVarData(string name, short var_num, VarType var_type, ushort? array_size, Modbus_VarData array_size_var, ModbusTcpIp_Client client)
            {
                try
                {
                    Name = name;
                    modbusClient = client;
                    varNum = var_num;
                    varType = var_type;
                    arraySize = array_size; 
                    arraySizeOnServerSideVar = array_size_var;
                    //frame_rec = new Modbus_DataFrame();
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }
            }


            //      конструктор для простой переменной
            //   Чтение Word.
            public Modbus_VarData(string name, short var_num, VarType var_type, ModbusTcpIp_Client client)
            {
                initVarData(name, var_num, var_type, null, null, client);
            }

            //   Чтение  ByteArray.
            //  Считывание массива происходит в два этапа:
            //  -  если размер не задан в конструкторе, то однократное считывание из контроллера переменной указывающей размер массива
            //  -  если размер задан в конструкторе, то считывания размера из контроллера не производится.
            //  -  считывание массива

            //  1. конструктор для массива с заранее известным размером
            public Modbus_VarData(string name, short var_num, ushort? array_size, ModbusTcpIp_Client client)
            {
                initVarData(name, var_num, VarType.ByteArray, array_size, null, client);
            }
            //  2. конструктор для массива с предварительным запросом из контролера его размера
            public Modbus_VarData(string name, short var_num, Modbus_VarData array_size_var, ModbusTcpIp_Client client)
            {
                initVarData(name, var_num, VarType.ByteArray, null, array_size_var, client);
            }


//*******   PROPERTIES  ********

            public ModbusTcpIp_Client modbusClient { get; set;}

            static int maxNameLength = 6;
            string name;
            public string Name
            {
                get { return name; }
                set
                {
                    if (value.Length > maxNameLength) name = value.Remove(maxNameLength);
                    else name = value;
                }
            }

            public short varNum;
            public VarType varType;
            //public ModbusCommand Command;
            //public DateTime Time { get; set; }

            //  Дополнительные данные для массивов.
            public ushort? arraySize; // размер массива
            public ushort? arrayFixedSizeOnServerSide; // размер массива
            public Modbus_VarData arraySizeOnServerSideVar; //  переменная в контроллере содержащая размер массива
            //public List<byte> list;

            //public void setClient(Modbus_Client client)
            //{
            //    connectionClient = client;
            //}


//*******   IO Methods   ********
            
                //  timeout = 0 - ждать до бесконечности 
                //          = time - ждать time-милисекунд     
                //          = -1 - проверить есть-нет и выходить не ождая

                //   Short  Reading
                public bool getValue(ref short? value)
                {
                    try
                    {
                        return modbusClient.Read(varNum, ref value);
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                    finally { }
                }

                //   Ushort  Reading
                public bool getValue(ref ushort? value)
                {
                    try
                    {
                        return modbusClient.Read(varNum, ref value);
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                    finally { }
                }


                //   Array Reading

                //   if <array_size> == null - use max size of array - <arraySizeVarOnServerSide>
                //   if <array_size> < <arraySizeVarOnServerSide> - use min size of array - <array_size>
                public bool getValue( out byte[] value, ushort? array_size=null, ushort? portion_size = null)
                {
                    value = null;
                    try
                    {
                        //  Если размер массива не задан фиксированно, то значит он может изменяться
                        // поэтому запрашиваем его у абонента на каждом обороте
                        if (arrayFixedSizeOnServerSide == null)
                        {
                            //  если не задан ни размер ни переменная по которой его запросить то выходим.
                            if (arraySizeOnServerSideVar == null) return false; //"Array size not set";
                            arraySizeOnServerSideVar.modbusClient = modbusClient;
                            if (!arraySizeOnServerSideVar.getValue(ref arraySize)) return false;
                            if (arraySize==null) return false; 
                        }
                        else arraySize = arrayFixedSizeOnServerSide;

                        //  проверка на ограничение - не запросилили только часть массива
                        if (array_size != null && array_size < arraySize) arraySize = array_size;

                        //***  Array Read Portion By Portion
                        return modbusClient.Read(varNum, out value, arraySize.Value, portion_size); 
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                    finally { }
                }


                //   Short/Ushort Writing
                public bool setValue(short value)
                {
                    try
                    {
                        return modbusClient.Write(varNum, value);
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                    finally { }
                }


                //   Array Writing

                //   if <array_size> == null - use max size of array - <arraySizeVarOnServerSide>
                //   if <array_size> < <arraySizeVarOnServerSide> - use min size of array - <array_size>
                public bool setValue(string value, Encoding encoding, ushort? portion_size = null)
                {
                    byte[] byteArray = encoding.GetBytes(value);
                    //---
                    return setValue(byteArray, (ushort)byteArray.Length, portion_size);
                }

                public bool setValue(byte[] value, ushort? array_size = null, ushort? portion_size = null)
                {
                    try
                    {
                        //  Если размер массива не задан фиксированно, то значит он может изменяться
                        // поэтому запрашиваем его у абонента на каждом обороте
                        if (arrayFixedSizeOnServerSide == null)
                        {
                            //  если не задан ни размер ни переменная по которой его запросить то выходим.
                            if (arraySizeOnServerSideVar == null) return false; //"Array size not set";
                            arraySizeOnServerSideVar.modbusClient = modbusClient;
                            if (!arraySizeOnServerSideVar.getValue(ref arraySize)) return false;
                            if (arraySize == null) return false;
                        }
                        else arraySize = arrayFixedSizeOnServerSide;

                        //  проверка на ограничение - не запросилили только часть массива
                        if (array_size != null && array_size < arraySize) arraySize = array_size;

                        //***
                        if (value.Length < arraySize) arraySize = (ushort)value.Length;

                        //***  Array Write Portion By Portion
                        return modbusClient.Write(varNum, value, arraySize, portion_size);

                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                    finally { }
                }



    }



//**********************************************************************
//******
        //*********    MODBUS FRAME CLIENT      MODBUS FRAME CLIENT
//******
//**********************************************************************

        static public byte[] Swap(byte[] arr, bool RetNewArray = false)
        {
            try
            {
                if (RetNewArray)
                {
                    byte[] res;
                    res = new byte[arr.Length];
                    for (int i = 0; i < arr.Length; i++) res[arr.Length - 1 - i] = arr[i];
                    return res;
                }
                else
                {
                    byte tmp;
                    for (int i = 0; i < arr.Length / 2; i++)
                    { tmp = arr[i]; arr[i] = arr[arr.Length - 1 - i]; arr[arr.Length - 1 - i] = tmp; }
                    return arr;
                }
            }
            catch (Exception excp) { 
                MessageBox.Show(excp.ToString()); 
            }
            finally { }
            return null;
        }


        public class ConcurrentList<T1>
        {
            List<T1> list;

            public ConcurrentList() 
            {
                list = new List<T1>();
            }

            public bool TryAdd(T1 value, int timeout = -1) 
            {
                try { list.Add(value); return true; }
                catch (Exception) { return false; }
            }

            public bool TryGetValue(int index, out T1 value, int timeout = -1)  
            {
                value = default(T1);
                try { value = list[index]; return true; }
                catch (Exception) { return false; }
            }

            public bool TryGetValue(int index, int count, out List<T1> out_list, int timeout = -1)
            {
                out_list = new List<T1>();
                try 
                {
                    for (int i = 0; i < count; i++)  out_list.Add(list[index + i]);
                    return true;
                }
                catch (Exception) { return false; }
            }

            public bool TryRemove(int index, int count = 1, int timeout = -1)
            {
                //     Не увеличиваем индекс на 1, т.к. при удалении элемента следующий 
                //  за ним сам смещается на место удаленного т.е. на тот же индекс.
                try
                {
                    list.RemoveRange(index, count); 
                    return true; 
                }
                catch (Exception) { return false; }
            }

            public bool TryClear(int timeout = -1)
            {
                try
                {
                    list.Clear();
                    return true;
                }
                catch (Exception) { return false; }
            }

            public bool TryTake(int index, out T1 value, int timeout = -1) 
            {
                value = default(T1);
                try
                {
                    value = list[index]; list.RemoveAt(index); 
                    return true;
                }
                catch (Exception) { return false; }
            }

            public bool TryTake(int index, int count, out List<T1> out_list, int timeout = -1)
            {
                out_list = new List<T1>();
                try
                {
                    for (int i = 0; i < count; i++)  out_list.Add(list[index + i]);
                    list.RemoveRange(index, count); 
                    return true;
                }
                catch (Exception) { return false; }
            }

            public int IndexOf(T1[] str, int timeout = -1)
            {
                try
                {
                    int i, j;
                    for (i = 0; i < list.Count - str.Length+1; )
                    {
                        bool ok = false;
                        for (j = 0; j < str.Length;)
                        {
                            if (!list[i++].Equals(str[j++])) { ok = false; break; }
                            else ok = true;
                        }
                        if (ok) return i - str.Length; 
                    }
                    return -1;
                }
                catch (Exception) { return -1; }
            }

        }



        public class Modbus_DataFrame
        {
                static int Transaction_ID_Counter = 0;
                public int Transaction_ID { get; set; } // <any code>
                public int Protocol_ID { get; set; } // <0>
                public int Data_Length { get; set; } // длина посылки начиная с Unit_ID.
                public byte Unit_ID { get; set; }      // <0>  
                public byte Function_Code { get; set; }
                public byte[] Data_Array;

//*******    CONSTRUCTORS    ********

                public Modbus_DataFrame() 
                {
                    Transaction_ID = 0; Protocol_ID = 0; Data_Length = 0;
                    Unit_ID = 0; Function_Code = 0; Data_Array = new byte[0]; //null;
                }

                public Modbus_DataFrame( int transaction_id, int protocol_id, int data_length,
                                         byte unit_id, byte function_code, byte[] data_array)
                {
                        Transaction_ID = transaction_id; Protocol_ID = protocol_id; Data_Length = data_length;
                        Unit_ID = unit_id; Function_Code = function_code; Data_Array = data_array;
                        //if (data_array.Length > Data_Array_MaxSize) throw new Exception("Data size exceeds maximum size, bytes: " + Data_Array_MaxSize);
                }

//*******    METHODS    ********

                public List<byte> frameToByteList()
                {
                    try{    
                            List<byte> byte_list = new List<byte>();

                            //  Записываем двухбайтные данные старшим байтом вперед.
                            int ax = Transaction_ID; byte_list.Add((byte)(ax >> 8)); byte_list.Add((byte)ax);
                            ax = Protocol_ID; byte_list.Add((byte)(ax >> 8)); byte_list.Add((byte)ax);
                            ax = Data_Length; byte_list.Add((byte)(ax >> 8)); byte_list.Add((byte)ax);
                            ax = Unit_ID; byte_list.Add((byte)ax);
                            ax = Function_Code; byte_list.Add((byte)ax);

                            for (int i = 0; Data_Array != null && i < Data_Array.Length; i++) byte_list.Add(Data_Array[i]);

                            return byte_list;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return null;
                }

                public int byteListToFrame(ConcurrentList<byte> ReceivedData_buff, int index)
                {
                    try
                    {
                        List<byte> temp;
                        //  Получены ли все данные заголовка фрейма?
                        if (!ReceivedData_buff.TryGetValue(index, 2 + 2 + 2 + 1 + 1, out temp)) return -1;
                        byte[] array = temp.ToArray();

                        Modbus_DataFrame frame = new Modbus_DataFrame();

                            //  Считываем двухбайтные данные старшим байтом вперед.
                            int i = 0;
                            frame.Transaction_ID = ((int)array[i++] << 8) + (int)array[i++];
                            frame.Protocol_ID = ((int)array[i++] << 8) + (int)array[i++];
                            frame.Data_Length = ((int)array[i++] << 8) + (int)array[i++];
                            frame.Unit_ID = array[i++];
                            frame.Function_Code = array[i++];
                            //  Получены ли все данные ?
                            if (!ReceivedData_buff.TryGetValue(index + 8, frame.Data_Length - 2, out temp)) return -1;

                        //  Копируем к себе только если получены все данные фрейма.
                        Transaction_ID = frame.Transaction_ID;
                        Protocol_ID = frame.Protocol_ID;
                        Data_Length = frame.Data_Length;
                        Unit_ID = frame.Unit_ID;
                        Function_Code = frame.Function_Code;
                        Data_Array = temp.ToArray();
                        //  возвращаем длину считанных данных чтобы мастер мог удалить из буфера
                        return 8 + Data_Length - 2;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return -1;
                }

                public List<byte>  getHeader()
                {
                    try
                    {   //  По отосланной посылке определяем искомый ответный Header.
                        List<byte> header_list = new List<byte>();
                        //  Записываем двухбайтные данные старшим байтом вперед.
                            int ax = Transaction_ID; header_list.Add((byte)(ax >> 8)); header_list.Add((byte)ax);
                            ax = Protocol_ID; header_list.Add((byte)(ax >> 8)); header_list.Add((byte)ax);
                        return header_list;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return null;
                }

                public string frameToString(string divider1 = "\n", string divider2 = ", ")
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

//  Перенесено из верхнего уровня

                //********  FRAME  CONVERSION   METHODS  **********

                //  Reading Int16 
                public bool getFrameValue( ref short? value)
                {
                    try
                    {
                        value = null;

                        if (Function_Code >= 0x80) return false;

                        //if (varType != ModbusVarType.sWord) return false;

                        int size = (byte)Data_Array[0];
                        if (size != Data_Array.Length - 1) return false;
                        int index = 1;

                        if (BitConverter.IsLittleEndian)
                        {
                            index = Data_Array.Length - index - sizeof(Int16);
                            if (index < 0) return false;
                            value = BitConverter.ToInt16(Step5.Swap(Data_Array, true), index);
                        }
                        else value = BitConverter.ToInt16(Data_Array, index);

                        return true;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return false;
                }

                //  Reading UInt16
                public bool getFrameValue( ref ushort? value)
                {
                    try
                    {
                        if (Function_Code >= 0x80) return false;

                        //if (varType != ModbusVarType.Word) return false;

                        int size = (byte)Data_Array[0];
                        if (size != Data_Array.Length - 1) return false;
                        int index = 1;

                        if (BitConverter.IsLittleEndian)
                        {
                            index = Data_Array.Length - index - sizeof(UInt16);
                            if (index < 0) return false;
                            value = BitConverter.ToUInt16(Step5.Swap(Data_Array, true), index);
                        }
                        else value = BitConverter.ToUInt16(Data_Array, index);

                        return true;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return false;
                }


                // Reading  byte[] array
                public bool getFrameValue(ref byte[] value)
                {
                    try
                    {

                        if (Function_Code >= 0x80) return false;

                        //if (varType != ModbusVarType.ByteArray) return false;

                        int size, index;

                        index = 0;
                        if (BitConverter.IsLittleEndian)
                        {
                            index = Data_Array.Length - index - sizeof(UInt16);
                            if (index < 0) return false;
                            size = BitConverter.ToUInt16(Step5.Swap(Data_Array, true), index);
                        }
                        else size = BitConverter.ToUInt16(Data_Array, index);

                        if (size != Data_Array.Length - 2) return false;
                        List<byte> list = Data_Array.ToList();
                        list.RemoveRange(0, 2);
                        list.CopyTo(value);

                        return true;
                    }
                    catch (Exception excp)
                    {
                        MessageBox.Show(excp.ToString());
                    }
                    finally { }
                    return false;
                }

                //  Writing Int16
                public bool getFrameStatus()
                {
                    try
                    {
                        Modbus_DataFrame frame_rec = this;

                        if (frame_rec == null) return false;
                        if (frame_rec.Function_Code >= 0x80) return false;
                        return true;
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return false;
                }


                //*********     Methods    ***********

                //  Read HoldingRegisters
                public static Modbus_DataFrame getSendFrame(short varNum)
                {
                    ModbusCommand command = ModbusCommand.ReadHoldingRegisters;

                    List<byte> data_list = new List<byte>();
                    if (BitConverter.IsLittleEndian)
                    {
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)varNum)));
                        //  number of vars = 1
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)0x0001)));
                    }
                    else
                    {
                        data_list.AddRange(BitConverter.GetBytes((Int16)varNum));
                        data_list.AddRange(BitConverter.GetBytes((Int16)0x0001));
                    }

                    int data_length = 2 + data_list.Count;

                    return new Modbus_DataFrame(Transaction_ID_Counter++, 0, data_length, 0, (byte)command, data_list.ToArray());

                }

                //---  Read ByteArray
                public static Modbus_DataFrame getSendFrame(short varNum, ushort start_index, ushort count)
                {
                    ModbusCommand command = ModbusCommand.ReadByteArray;

                    List<byte> data_list = new List<byte>();
                    if (BitConverter.IsLittleEndian)
                    {
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)varNum)));
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)start_index)));
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)count)));
                    }
                    else
                    {
                        data_list.AddRange(BitConverter.GetBytes((Int16)varNum));
                        data_list.AddRange(BitConverter.GetBytes((Int16)start_index));
                        data_list.AddRange(BitConverter.GetBytes((Int16)count));
                    }

                    int data_length = 2 + data_list.Count;

                    return new Modbus_DataFrame(Transaction_ID_Counter++, 0, data_length, 0, (byte)command, data_list.ToArray());
                }


                // Preset MultipleRegisters
                public static Modbus_DataFrame getSendFrame(short varNum, short value)
                {
                    ModbusCommand command = ModbusCommand.PresetMultipleRegisters;

                    List<byte> data_list = new List<byte>();
                    if (BitConverter.IsLittleEndian)
                    {   //  vartab index of Register
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)varNum)));
                        //  number of Registers = 1
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)0x0001)));
                        //!!! Size of Registers type - it not mentioned in Manual
                        data_list.Add((Byte)sizeof(Int16));
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)value)));
                    }
                    else
                    {
                        data_list.AddRange(BitConverter.GetBytes((Int16)varNum));
                        data_list.AddRange(BitConverter.GetBytes((Int16)0x0001));
                        //!!! Size of Registers type - it not mentioned in Manual
                        data_list.Add((Byte)sizeof(Int16));
                        data_list.AddRange(BitConverter.GetBytes((Int16)value));
                    }

                    int data_length = 2 + data_list.Count;

                    return new Modbus_DataFrame(Transaction_ID_Counter++, 0, data_length, 0, (byte)command, data_list.ToArray());

                }


                //---  Preset ByteArray
                public static Modbus_DataFrame getSendFrame(short varNum, ushort start_index, ushort count, byte[] data)
                {
                    ModbusCommand command = ModbusCommand.PresetByteArray;

                    List<byte> data_list = new List<byte>();
                    if (BitConverter.IsLittleEndian)
                    {
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)varNum)));
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)start_index)));
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)count)));
                        data_list.AddRange(Step5.Swap(BitConverter.GetBytes((Int16)count)));
                    }
                    else
                    {
                        data_list.AddRange(BitConverter.GetBytes((Int16)varNum));
                        data_list.AddRange(BitConverter.GetBytes((Int16)start_index));
                        data_list.AddRange(BitConverter.GetBytes((Int16)count));
                        data_list.AddRange(BitConverter.GetBytes((Int16)count));
                    }

                    for (int i = 0; i < count; i++) data_list.Add(data[i]);

                    int data_length = 2 + data_list.Count;

                    return new Modbus_DataFrame(Transaction_ID_Counter++, 0, data_length, 0, (byte)command, data_list.ToArray());
                }

                
        }


//*******    CLASS:  Modbus_DataFrame_Client    ********

        public class ModbusTcpIp_Client 
        {

//*********    PROPERTIES    ***********

            public static ushort Data_Array_MaxSize = 1450;// еще запасик на тело фрейма 1460;

            ConcurrentList<byte> ReceivedData_buff;
            TCP_IP_Client tcpIpClient ;
            public bool IsConnected { get { return (tcpIpClient != null && tcpIpClient.IsConnected); } }
            public int timeout;
            public int connectionAttempts;

//*********    CONSTRUCTORS    ***********

            public ModbusTcpIp_Client(string serv_address, int serv_port, int timeout, int connectionAttempts )//= -1)
                {
                    ReceivedData_buff = new ConcurrentList<byte>();
                    tcpIpClient = new TCP_IP_Client(serv_address, serv_port);
                    this.timeout = timeout;
                    this.connectionAttempts = connectionAttempts;
                }

            public ModbusTcpIp_Client(TCP_IP_Client tcpIpClient, int timeout, int connectionAttempts)//= -1)
            {
                ReceivedData_buff = new ConcurrentList<byte>();
                this.tcpIpClient = tcpIpClient;
                this.timeout = timeout;
                this.connectionAttempts = connectionAttempts;
            }


                ~ModbusTcpIp_Client()
                { 
                    Close(); 
                }

//*********    METHODS    ***********

            public bool Connect()
            {
                try
                {
                    for (int i = 0; i < connectionAttempts; i++)
                    {
                        if (tcpIpClient.Connect(timeout)) return true;
                        MessageBox.Show("Connection failed. New attempt");
                    }
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }
                return false;
            }

            public bool Close()  
                {
                    try
                    {
                        ReceivedData_buff.TryClear(timeout);
                        return tcpIpClient.Close(timeout);
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                    finally { }
                    return false;
                }


            //  timeout = 0 - ждать до бесконечности 
            //          = time - ждать time-милисекунд     
            //          = -1 - проверить есть-нет и выходить не ождая
            public bool Send(Modbus_DataFrame frame, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {
                    //  если связи нет то она уже и не появится
                    if (!tcpIpClient.IsConnected)
                    {
                        MessageBox.Show("Connection lost. New attempt");
                        if (!Connect()) return false; // attempt of reconnecting
                    }

                    //  ожидание данных
                    List<byte> byte_list = frame.frameToByteList();
                    DateTime time = DateTime.Now;
                    do
                    { 
                        if (tcpIpClient.Write(byte_list.ToArray(), 0, byte_list.Count) != -1) return true;
                    } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);
                    return false;
                }
                catch (Exception excp) { MessageBox.Show( excp.ToString()); return false; }
                finally { }
            }


            //  timeout = 0 - ждать до бесконечности 
            //          = time - ждать time-милисекунд     
            //          = -1 - проверить есть-нет и выходить не ождая
            public bool Receive(Modbus_DataFrame sent_frame, ref Modbus_DataFrame received_frame, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                try
                {
                    //  если связи нет то она уже и не появится
                    if (!tcpIpClient.IsConnected)
                    {
                        MessageBox.Show("Connection lost. New attempt");
                        if (!Connect()) return false; // attempt of reconnecting
                    }

                    //  По отосланной посылке определяем искомый ответный Header.
                    List<byte> header_list = sent_frame.getHeader();

                        //  ожидание данных
                        DateTime time = DateTime.Now;
                        do
                        {       //****************     Переносим все полученное из FIFO-буфера TCP_IP в  буфер c произвольным доступом MODBUS

                            byte[] buff = new byte[1];
                            while (tcpIpClient.Read(buff, 0, buff.Length, -1) > 0) ReceivedData_buff.TryAdd(buff[0], timeout);

                            //****  Выискиваем требуемую посылку в любом месте буфера MODBUS и считываем ее с удалением - буфер смыкается.

                            //  Проверяем есть ли Header требуемого сообщения в принятых данных.
                            int index = ReceivedData_buff.IndexOf(header_list.ToArray(), timeout);
                            if (index < 0) continue;
                            //добавить проверку длины ответа
                            int bx = received_frame.byteListToFrame(ReceivedData_buff, index);
                            //  посылка пришла не в полном объеме - ожидаем остаток
                            if (bx < 0) continue;

                            //  Посылка считана - удаляем ее из буфера.
                            ReceivedData_buff.TryRemove(index, bx, timeout);
                            return true;

                        } while (timeout == 0 || (DateTime.Now - time).TotalMilliseconds <= timeout);

                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { }

                return false;
            }

            //  timeout = 0 - ждать до бесконечности 
            //          = time - ждать time-милисекунд     
            //          = -1 - проверить есть-нет и выходить не ождая
            public bool SendReceive(Modbus_DataFrame send_frame, ref Modbus_DataFrame received_frame, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                //  Send and Receive have their own Timeouts without attempts
                //  This method has additionaly attempts of full cikle of Send-Receive
                try
                {
                    for (int i = 0; i < connectionAttempts; i++)
                    {
                        //  Sending frame with internal timeout- запрос
                        if (!Send(send_frame, dispatcher)) break;

                        //  Receiving frame with internal timeout -  ответ
                        if (Receive(send_frame, ref received_frame, dispatcher)) return true;
                        MessageBox.Show("Transaction failed. New attempt");
                    }
                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                finally { }
            }

             //   Short  Reading
            public bool Read(short varNum, ref short? value)
            {
                try
                {
                    Modbus_DataFrame frame_rec = new Modbus_DataFrame();
                    ModbusTcpIp_Client modbusClient = this;

                    if (modbusClient.SendReceive(Modbus_DataFrame.getSendFrame(varNum), ref frame_rec))
                        return frame_rec.getFrameValue(ref value);
                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                finally { }
            }

            //   Ushort  Reading
            public bool Read(short varNum, ref ushort? value)
            {
                try
                {
                    Modbus_DataFrame frame_rec = new Modbus_DataFrame();
                    ModbusTcpIp_Client modbusClient = this;

                    if (modbusClient.SendReceive(Modbus_DataFrame.getSendFrame(varNum), ref frame_rec))
                        return frame_rec.getFrameValue(ref value);
                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                finally { }
            }


            //   Array Reading

            //   if <array_size> == null - use max size of array - <arraySizeVarOnServerSide>
            //   if <array_size> < <arraySizeVarOnServerSide> - use min size of array - <array_size>
            public bool Read(short varNum, out byte[] value, ushort array_size, ushort? portion_size = null)
            {
                    value = null;
                    try
                    {
                        Modbus_DataFrame frame_rec = new Modbus_DataFrame();
                        ModbusTcpIp_Client modbusClient = this;

                            if (array_size < 0 ) array_size = 0;

                            //***  Array Read Portion By Portion

                            value = new byte[array_size];
                            ushort count = 0;
                            //  порционное считывание
                            do
                            {
                                ushort size = (ushort)(array_size - count);

                                //  размер массива помещается в одну порцию или в несколько?
                                if (portion_size == null) portion_size = Data_Array_MaxSize;
                                size = size > portion_size.Value ? portion_size.Value : size;

                                //  считывание очередной порции
                                if (!modbusClient.SendReceive(Modbus_DataFrame.getSendFrame(varNum, count, size), ref frame_rec)) return false;

                                byte[] data = new byte[size];
                                if (!frame_rec.getFrameValue(ref data)) return false;

                                //  добавление порции в выходной массив
                                for ( int i = 0 ; i < size ; i++) value[count++] = data[i]; 
                                if (count >= array_size) return true;

                            } while (true);
                        
                    }
                    catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                    finally { }
            }


            //   Short/Ushort Writing
            public bool Write(short varNum, short value)
            {
                try
                {
                    Modbus_DataFrame frame_rec = new Modbus_DataFrame();
                    ModbusTcpIp_Client modbusClient = this;

                    if (modbusClient.SendReceive(Modbus_DataFrame.getSendFrame(varNum, value), ref frame_rec))
                        return frame_rec.getFrameStatus();
                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                finally { }
            }


            //   Array Writing

            //   if <array_size> == null - use max size of array - <arraySizeVarOnServerSide>
            //   if <array_size> < <arraySizeVarOnServerSide> - use min size of array - <array_size>
            public bool Write(short varNum, string value, int encoding = 1251, ushort? portion_size = null)
            {
                byte[] byteArray = Encoding.GetEncoding(encoding).GetBytes(value);
                //---
                return Write(varNum, byteArray, (ushort)byteArray.Length, portion_size);
            }

            public bool Write(short varNum, byte[] value, ushort? array_size = null, ushort? portion_size = null)
            {
                try
                {
                    Modbus_DataFrame frame_rec = new Modbus_DataFrame();
                    ModbusTcpIp_Client modbusClient = this;

                    if (array_size < 0) array_size = 0;

                    //***  Array Write Portion By Portion

                    ushort count = 0;
                    do
                    {
                        ushort size = (ushort)(array_size - count);

                        //  размер массива помещается в одну порцию или в несколько?
                        if (portion_size == null) portion_size = Data_Array_MaxSize;
                        size = size > portion_size.Value ? portion_size.Value : size;

                        byte[] data = new byte[size];
                        //  добавление порции в посылаемый выходной массив
                        for (int i = 0; i < size; i++) data[i] = value[count + i];

                        //  запись очередной порции
                        if (!modbusClient.SendReceive(Modbus_DataFrame.getSendFrame(varNum, count, size, data), ref frame_rec)) return false;

                        if (!frame_rec.getFrameStatus()) return false;

                        count += size;
                        if (count >= array_size) return true;

                    } while (true);
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
                finally { }
            }

         

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