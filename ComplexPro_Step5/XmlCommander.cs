using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml;
using System.Threading;

namespace ComplexPro_Step5
{
        //********   XmlCommander   ******************************

        //  Reading Attribute of Element selected by referenced Path
    public partial class Step5
    {
        public class XmlCommander
        {
            Modbus_VarData varCommand;
            Modbus_VarData varStatus;
            Modbus_VarData varResponse;
            Encoding encoding;
            int commandId;
            XmlTextReader reader;
            byte[] byteArray;

            public XmlCommander(ModbusTcpIp_Client modbusClient, Encoding encoding)
            {
                varCommand = new Modbus_VarData("", 155,
                    new Modbus_VarData("", 154, VarType.Word, modbusClient), modbusClient);
                varStatus = new Modbus_VarData("", 157,
                    new Modbus_VarData("", 156, VarType.Word, modbusClient), modbusClient);
                varResponse = new Modbus_VarData("", 159,
                    new Modbus_VarData("", 158, VarType.Word, modbusClient), modbusClient);
                //---
                this.encoding = encoding;
                commandId = 0;
            }

            // read Element header as byte[] array
            public byte[] readElement(string path)
            {
                try
                {
                    commandId++;
                    //  Send Command 
                    if (!varCommand.setValue("<Command Id=\"" + commandId + "\" Name=\"Read\" Path=\"" + path + "\"/>", encoding)) { MessageBox.Show("Data isn't sent!"); return null; }
                    //  Wait Response
                    if (!waitStatusReady()) { MessageBox.Show("Command isn't responsed!"); return null; }
                    //---
                    if (!varResponse.getValue(out byteArray)) { MessageBox.Show("Data isn't received!"); return null; }

                    return byteArray;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
            }

            // read Element header as String with given encoding from MCU or from given byte[] array
            public string readStringElement(string path, byte[] source = null)
            {
                try
                {
                    if (source == null) byteArray = readElement(path);
                    else byteArray = source;

                    return byteArray == null ? null : encoding.GetString(byteArray);
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
            }

            // read Element attribut as String with given encoding from MCU or from given byte[] array
            public string readStringAttribute(string path, string attribut, byte[] source = null)
            {
                try
                {
                    if (source == null) byteArray = readElement(path);
                    else byteArray = source;

                    if (byteArray != null)
                    {
                        //MessageBox.Show(Encoding.GetEncoding(1251).GetString(byteArray));
                        reader = new XmlTextReader(new StreamReader(new MemoryStream(byteArray), encoding));
                        // иначе при чтении аттрибутов воспринимает пробелы внутри кавычек как новые узлы и 
                        reader.WhitespaceHandling = WhitespaceHandling.None; // выдает null вместо знаечения

                        while (reader.Read())
                        {
                            string str = reader.GetAttribute(attribut);
                            if (str != null) return str;
                        }
                    }
                    return null;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return null; }
            }

            // read Element attribute as Int from MCU or from given byte[] array
            public int? readIntAttribute(string path, string attribut, byte[] source = null)
            {
                double ax;
                if (double.TryParse(readStringAttribute(path, attribut, source), out ax)) return (int)ax;
                return null;
            }

            // read Element attribute as Double from MCU or from given byte[] array
            public double? readDoubleAttribute(string path, string attribut, byte[] source = null)
            {
                double ax;
                if (double.TryParse(readStringAttribute(path, attribut, source), out ax)) return ax;
                return null;
            }

            //---------

            // write Element attribut as String with given encoding from MCU or from given byte[] array
            public bool writeStringAttribute(string path, string attributeName, string attributeValue)
            {
                try
                {
                    commandId++;
                    //  Send Command 
                    if (!varCommand.setValue("<Command Id=\"" + commandId + "\" Name=\"Write\" Path=\"" + path + "\"" +
                        " AttributeName=\"" + attributeName + "\" AttributeValue=\"" + attributeValue + "\"/>", encoding)) { MessageBox.Show("Data isn't sent!"); return false; }
                    //  Wait Response
                    if (!waitStatusReady()) { MessageBox.Show("Command isn't responsed!"); return false; }

                    return true;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
            }

            //---------
            // check ready-state of output data in MCU for last command
            bool? isResponseReady()
            {
                try
                {
                    if (!varStatus.getValue(out byteArray)) { MessageBox.Show("Data isn't received!"); return false; }

                    //MessageBox.Show(Encoding.GetEncoding(1251).GetString(byteArray));

                    reader = new XmlTextReader(new StreamReader(new MemoryStream(byteArray), encoding));
                    // иначе при чтении аттрибутов воспринимает пробелы внутри кавычек как новые узлы и 
                    reader.WhitespaceHandling = WhitespaceHandling.None; // выдает null вместо знаечения

                    while (reader.Read())
                    {
                        if (reader.Name == "CommandStatus")
                        {
                            string str = reader.GetAttribute("Id");
                            if (str == null) return false;
                            if (str != commandId.ToString()) return null;

                            str = reader.GetAttribute("Status");
                            if (str == null) return false;
                            if (str == "Ready") return true;
                            if (str == "NotReady") return null;
                            if (str == "Error")
                            {
                                MessageBox.Show("Error: <" + reader.GetAttribute("Info") + ">", "CommandStatus");
                                return false;
                            }
                        }
                    }
                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
            }

            // check ready-state of output data in MCU for last command
            bool waitStatusReady(int timeout = 1000)
            {
                try
                {
                    //  ожидание данных
                    DateTime time = DateTime.Now;
                    do
                    {
                        Thread.Sleep(2); // to give time for VCU prepare response
                        //---
                        bool? resp = isResponseReady();
                        if (resp == null) continue;
                        if (resp.Value) return true;
                        return false;
                    } while ((DateTime.Now - time).TotalMilliseconds <= timeout);
                    return false;
                }
                catch (Exception excp) { MessageBox.Show(excp.ToString()); return false; }
            }


        }
        //********  End of  XmlCommander   *******************
    }
    //  Step5
}
