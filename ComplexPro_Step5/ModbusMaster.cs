//2019-03-19
//Сделано все: проверил связь с контроллером и отображение переменных на мнемосхеме
//нужно добавить н амнемосхему отображение базовых переменных
//добавить отображение коннект переменных

//    доделать интерфейс: 
//+   отображение BOOL-переменных
//-   автозагрузку map-файла и ручную загрузку при его отсутствии
//-   поле задания номера переменных соотв. массивам
//-   считывание из ini-файла номера переменных соотв. массивам
//-   вывод переменных на коннекторы
//-   облагородить исключения при обрывах связи
//-   показать индикатор наличия связи

//Закончил здесь 07-02-2019 поиском как сделать -   вывод переменных на коннекторы


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

//********     class ModbusVarList       ****************

        class ModbusMaster : DependencyObject
        {

            System.Windows.Threading.Dispatcher dispatcher;
            //---
            ModbusTcpIp_Client modbusClient;
            Task mainThread;
            Dictionary<string, Modbus_VarData> varDictionaryList;
            public bool IsClosed { get; set; }
            int scanPeriod;
            public int CikleCount;
            int timeOut;
            bool closeThread;
            //--------

            //****************

            public ModbusMaster()
            {
                IsClosed = false;
                closeThread = false;
                modbusClient = null;
                mainThread = null;
                timeOut = 2000;
                //dispatcher = null;
                scanPeriod = 200; //msec
                CikleCount = 0;
                //dispatcher = this.Dispatcher;
                varDictionaryList = new Dictionary<string, Modbus_VarData>() 
                { {"Id", new Modbus_VarData("", 12, VarType.Word, null)},
                  {"ByteArray", new Modbus_VarData("", 129,
                       new Modbus_VarData("", 128, VarType.Word, null), null)},
                  {"WordArray", new Modbus_VarData("", 131, 
                       new Modbus_VarData("", 130, VarType.Word, null), null)},
                  {"LwordArray", new Modbus_VarData("", 133, 
                      new Modbus_VarData("", 132, VarType.Word, null), null)}
                };
                
            }

            public bool Close()
            {
                bool closed_tst = true;
                try
                {
                    if (IsClosed) return IsClosed; // уже было один раз закрыто
                    //---
                    closeThread = true;

                    if (mainThread != null)
                    {   //  ожидание 
                        if (timeOut == -1) mainThread.Wait();
                        else closed_tst &= mainThread.Wait(timeOut);
                        mainThread.Dispose(); mainThread = null;
                    }
                    modbusClient.Close(); modbusClient = null;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.ToString());
                    closed_tst = false;
                    return IsClosed = closed_tst;
                    //Environment.Exit(excp.ErrorCode);
                }
                finally{}
                return IsClosed = closed_tst;
            }


            public bool Start(string serv_address, int serv_port, int timeout)
            {
                try
                {
                    if ( modbusClient != null )  return false;

                    modbusClient = new ModbusTcpIp_Client(serv_address, serv_port, 2000, 2);
                    modbusClient.Connect();
                    if (!modbusClient.IsConnected)
                    {
                        MessageBox.Show("Can't open connection");
                        modbusClient.Close(); modbusClient = null;
                        return false;
                    }
                    else
                    {
                        foreach (Modbus_VarData varitem in varDictionaryList.Values) varitem.modbusClient=modbusClient;

                        dispatcher = this.Dispatcher;
                        closeThread = false;
                        mainThread = new Task(Run);
                        mainThread.Start();
                        return true;
                    }
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { };
                return false;
            }


            //  Циклический опрос массивов пока не прервет главная программа
            void Run()
            {
                try
                {
                    byte[] byteArray = null;
                    byte[] wordArray = null;
                    byte[] lwordArray = null;
                    ushort? Id=null;
                    StringBuilder str = new StringBuilder();

                    //  Циклический опрос массивов пока не прервет главная программа
                    while (!closeThread && modbusClient.IsConnected)
                    {
                        //this.Dispatcher.BeginInvoke((ThreadStart)delegate()
                            //foreach (Modbus_VarData varitem in varList)
                            {
                              //  varitem.getValue(ref size);
                              //  varitem.getValue(out arrr, 200);
                              varDictionaryList["Id"].getValue(ref Id);
                              varDictionaryList["ByteArray"].getValue(out byteArray, null, 200);
                              varDictionaryList["WordArray"].getValue(out wordArray, null, 200);
                              varDictionaryList["LwordArray"].getValue(out lwordArray, null, 200);
                            }

                            str.Clear();
                            str.Append("Id: <" + Id + ">\n");
                            str.Append("Array size: <" + wordArray.Length + ">\n");
                            for (int i = 0; i < wordArray.Length; i++) str.Append(wordArray[i].ToString() + " ");

                        dispatcher.BeginInvoke((ThreadStart)delegate()
                        {
                            DEBUG.ReceivingTextBox.Text = str.ToString();
                            //---

                            setSymbolsDataFromCPUArrays(SYMBOLS.GLOBAL_SYMBOLS_LIST, byteArray, wordArray, lwordArray);
                            //SYMBOLS_LIST.REFRESH_SYMBOLS_LIST_window(SYMBOLS.GLOBAL_SYMBOLS_LIST_window);

                        });
                        Thread.Sleep(scanPeriod);
                    }
                    //**************
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); }
                finally { };

                return;
            }


//********   Заполнение GLOBAL_LIST и др. значениями из считанного из контроллера массива
//  
//  В массиве памяти контроллера переменные лежат в порядке от младшего байта к старшему
//
            string setSymbolsDataFromCPUArrays(ObservableCollection<SYMBOLS.Symbol_Data> symbolsList, 
                byte[] byteArray, byte[] wordArray, byte[] lwordArray)
            {
                try
                {
                    //  перебор всех символов в заданном списке символов
                    foreach (SYMBOLS.Symbol_Data symbol in symbolsList)
                    {
                        //  определение размености уставки для поиска ее в соответсвующем 
                        //  блоке считанном с контроллера
                        if( symbol.Data_Type != "BOOL") //  BOOL-будет выведено после считывания всех переменных
                        {
                            string data_type = SYMBOLS.EQUAL_DATA_TYPES[symbol.Data_Type];

                            if (data_type == "byte")
                            {
                                byte value = 0;
                                if ( myBitConverter(byteArray, (int)symbol.Address.Value*sizeof(byte), ref value)) symbol.Real_Value = value.ToString();
                            }
                            else if (data_type == "word" || data_type == "sword")
                            {
                                Int16 value = 0;
                                if (myBitConverter(wordArray, (int)symbol.Address.Value * sizeof(Int16), ref value)) symbol.Real_Value = value.ToString();
                            }
                            else if (data_type == "lword" || data_type == "slword")
                            {
                                Int32 value = 0;
                                if (myBitConverter(lwordArray, (int)symbol.Address.Value * sizeof(Int32), ref value)) symbol.Real_Value = value.ToString();
                            }
                            //else if (data_type == "float"){}
                            //else if (data_type == "double"){}
                        }
                    }
                    //  Вывод BOOL-значений только после получения данных для их базовых символов
                    foreach (SYMBOLS.Symbol_Data symbol in symbolsList)
                    {
                        // запись только чтобы активировать вывод символа на экран
                        if (symbol.Data_Type == "BOOL") symbol.Real_Value = null; //  а данные он возьмет из базовой переменной
                    }

                    return null;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return "Exception"; }
                finally { }

            }

            //*******   array -> byte
            bool myBitConverter(byte[] array, int index, ref byte value )
            {
                if (index+sizeof(byte)-1 >= array.Length) return false;
                value = array[index];
                return true;
            }

//  В массиве памяти контроллера переменные лежат в порядке от младшего байта к старшему

            //*******   array -> short
            bool myBitConverter(byte[] array, int index, ref Int16 value)
            {
                if (index + sizeof(Int16) - 1 >= array.Length) return false;
                value = BitConverter.ToInt16(array, index);
                return true;
            }
            //*******   array -> int
            bool myBitConverter(byte[] array, int index, ref Int32 value)
            {
                if (index + sizeof(Int32) - 1 >= array.Length) return false;
                value = BitConverter.ToInt32(array, index);
                return true;
            }


        }// ModbusMaster



//********************************
//  #2
        //class ModbusVarListMaster : DependencyObject
        //{

        //    //  Список переменных для автоматического их опроса Мастером.
        //    public List<ModbusVarListItem> varList;

        //    Modbus_IO_Client modbusClient;
        //    int timeOut;
        //    System.Windows.Threading.Dispatcher dispatcher;
        //    //---
        //    Task threadMain;
        //    bool closeClient { get; set; }
        //    public bool isClosed { get; set; }
        //    int scanPeriod;
        //    public  int CikleCount;
        //    //--------

        //    //****************

        //    public ModbusVarListMaster()
        //    {
        //        Init();
        //    }

        //    public ModbusVarListMaster( List<ModbusVarListItem> varList )
        //    {
        //        Init();
        //        this.varList = varList;
        //    }

        //    void Init()
        //    {
        //        varList = new List<ModbusVarListItem>();
        //        closeClient = false; isClosed = false;
        //        modbusClient = null;
        //        threadMain = null;
        //        timeOut = 5000;
        //        dispatcher = null;
        //        scanPeriod = 100; //msec
        //        CikleCount = 0;
        //        //dispatcher = this.Dispatcher;
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


        //    public string Start(string serv_address, int serv_port, int timeout, System.Windows.Threading.Dispatcher dispatcher)
        //    {
        //        try
        //        {
        //            if (modbusClient == null)
        //            {
        //                // Modbus Client
        //                Modbus_IO_Client client = new Modbus_IO_Client(serv_address, serv_port, timeout);
        //                Start(client, timeout, dispatcher);
        //            }
        //            else return "Connection is currently open!";
        //            //**************
        //        }
        //        catch (Exception excp)
        //        {
        //            MessageBox.Show(excp.ToString());
        //        }

        //        return null;
        //    }


        //    public string Start(Modbus_IO_Client client, int timeout, System.Windows.Threading.Dispatcher dispatcher)
        //    {
        //        try
        //        {
        //            if (modbusClient == null)
        //            {
        //                // Modbus Client
        //                modbusClient = client;
        //                timeOut = timeout;
        //                this.dispatcher = dispatcher;
        //                if (!modbusClient.IsConnected)
        //                {
        //                    modbusClient.Close(timeOut); modbusClient = null;
        //                    return "Can't open Modbus connection!";
        //                }
        //            }
        //            else return "Connection is currently open!";
        //            //**************

        //            //  запускаем задачу фонового опроса переменных
        //            threadMain = new Task(Thread_Modbus);
        //            threadMain.Start();

        //        }
        //        catch (Exception excp)
        //        {
        //            MessageBox.Show(excp.ToString());
        //        }

        //        return null;
        //    }


        //    //  Циклический опрос массивов пока не прервет главная программа
        //    void Thread_Modbus()
        //    {
        //        try
        //        {
        //            //  Циклический опрос массивов пока не прервет главная программа
        //            while (!closeClient && modbusClient.IsConnected)
        //            {
        //                string str;
        //                foreach (ModbusVarListItem varitem in varList)
        //                {
        //                    //this.Dispatcher.BeginInvoke((ThreadStart)delegate()
        //                 //   dispatcher.BeginInvoke((ThreadStart)delegate()
        //                   // {
        //                       str = varitem.ReadVar(modbusClient, timeOut, dispatcher);
        //                   // });
        //                }

        //                    //this.Dispatcher.BeginInvoke((ThreadStart)delegate()
        //                    //dispatcher.BeginInvoke((ThreadStart)delegate()
        //                    //{
        //                        CikleCount++;
        //                    //});
        //                //----
        //                //Thread.Sleep(scanPeriod);
        //            }
        //            //**************
        //        }
        //        catch (Exception excp)
        //        {
        //            MessageBox.Show(excp.ToString());
        //        }


        //        return;
        //    }



        //}// ModbusMaster





    }
}
