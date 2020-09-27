// Copyright © 2020 Fullham Alfayet
// Licensed under terms of the GPL Version 3. See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;

using Sims3.SimIFace;

using Sims3.Gameplay.Utilities;

namespace Veitc.AddsCommandPlus
{
    #region class:LogUtils
    internal class LogUtils
    {
        // Fields
        public static readonly string NewLine = System.Environment.NewLine;
        public static int sLogEnumerator = 0;
        public static List<string> sPendingNotifications = new List<string>(100);

        // Metheds
        public static void TriggerPendingNotifications()
        {
            if (sPendingNotifications.Count != 0)
            {
                foreach (string i in sPendingNotifications)
                {
                    Comman.PrintMessage(i, false, 100);
                }
                sPendingNotifications.Clear();
            }
        }
        public static bool WriteLog(string text, bool scripterror, bool printmessage)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return false;

                uint fileHandle = 0x0;
                string str = Simulator.CreateScriptErrorFile(ref fileHandle);
                if (fileHandle == 0)
                {
                    return false;
                }
                //if (addHeader)
                {
                    sLogEnumerator++;

                    try
                    {
                        if (printmessage)
                        {
                            Comman.PrintMessage("Write Log is Created", false, 100);
                        }
                        else if (scripterror)
                        {
                            Comman.PrintMessage(_thisAssembly._name + NewLine + "Script Error is Found No: " + sLogEnumerator, true, 100);
                        }
                    }
                    catch (StackOverflowException) { throw; }
                    catch (Exception)
                    { }

                    string[] labels = GameUtils.GetGenericString(GenericStringID.VersionLabels).Split(new char[] { '\n' });
                    string[] data = GameUtils.GetGenericString(GenericStringID.VersionData).Split(new char[] { '\n' });

                    string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + NewLine;
                    header += "<" + _thisAssembly._name + ">" + NewLine;
                    header += "<ModVersion value=\"" + _thisAssembly._version + "\"/>" + NewLine;


                    int le = (labels.Length > data.Length) ? data.Length : labels.Length;
                    for (int j = 0x0; j < le; j++)
                    {
                        string label = labels[j].Replace(":", "").Replace(" ", "");

                        switch (label)
                        {
                            //case "GameVersion":
                            case "BuildVersion":
                                header += "<" + label + " value=\"" + data[j] + "\"/>" + NewLine;
                                break;
                        }
                    }

                    IGameUtils utils = AppDomain.CurrentDomain.GetData("GameUtils") as IGameUtils;
                    if (utils != null)
                    {
                        ProductVersion version = (ProductVersion)utils.GetProductFlags();

                        header += "<Installed value=\"" + version + "\"/>" + NewLine;


                    }

                    header += "<Enumerator value=\"" + sLogEnumerator + "\"/>" + NewLine;

                    header += "<FileName value=\"" + str + "\"/>" + NewLine;
                    header += "<Content>" + NewLine;
                    if (scripterror)
                    {
                        header += "Script Error:" + NewLine + NewLine;
                    }
                    text = header + text.Replace("&", "&amp;");//.Replace(NewLine, "  <br />" + NewLine);

                    text += NewLine + "</Content>";
                    text += NewLine + "</" + _thisAssembly._name + ">";
                }



                if (fileHandle != 0x0)
                {
                    CustomXmlWriter xmlWriter = new CustomXmlWriter(fileHandle);

                    xmlWriter.WriteToBuffer(text);

                    xmlWriter.WriteEndDocument();
                }
                return true;
            }
            catch (StackOverflowException) { throw; }
            catch
            {
                return false;
            }
        }
        public static bool WriteLogWithPrefix(string text, string prefix, bool scripterror, bool printmessage)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return false;

                uint fileHandle = 0x0;
                string str = Simulator.CreateExportFile(ref fileHandle, prefix ?? _thisAssembly._name);//Simulator.CreateScriptErrorFile(ref fileHandle);

                if (fileHandle == 0)
                {
                    return false;
                }

                text += "\n\nDate: " + DateTime.Now.ToString();
                //if (addHeader)
                {
                    sLogEnumerator++;

                    try
                    {
                        if (printmessage)
                        {
                            Comman.PrintMessage("Write Log is Created", true, 100);
                        }
                        else if (scripterror)
                        {
                            Comman.PrintMessage(_thisAssembly._name + NewLine + "Script Error is Found No: " + sLogEnumerator, true, 100);
                        }
                    }
                    catch (StackOverflowException) { throw; }
                    catch (Exception)
                    { }

                    string[] labels = GameUtils.GetGenericString(GenericStringID.VersionLabels).Split(new char[] { '\n' });
                    string[] data = GameUtils.GetGenericString(GenericStringID.VersionData).Split(new char[] { '\n' });

                    string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + NewLine;

                    header += "<" + _thisAssembly._name + ">" + NewLine;
                    header += "<ModVersion value=\"" + _thisAssembly._version + "\"/>" + NewLine;

                    int le = (labels.Length > data.Length) ? data.Length : labels.Length;
                    for (int j = 0x0; j < le; j++)
                    {
                        string label = labels[j].Replace(":", "").Replace(" ", "");

                        switch (label)
                        {
                            //case "GameVersion":
                            case "BuildVersion":
                                header += "<" + label + " value=\"" + data[j] + "\"/>" + NewLine;
                                break;
                        }
                    }

                    IGameUtils utils = AppDomain.CurrentDomain.GetData("GameUtils") as IGameUtils;
                    if (utils != null)
                    {
                        ProductVersion version = (ProductVersion)utils.GetProductFlags();

                        header += "<Installed value=\"" + version + "\"/>" + NewLine;
                    }

                    header += "<Enumerator value=\"" + sLogEnumerator + "\"/>" + NewLine;

                    header += "<FileName value=\"" + str + "\"/>" + NewLine;
                    header += "<Content>" + NewLine;

                    if (scripterror)
                    {
                        header += "Script Error:" + NewLine + NewLine;
                    }
                    text = header + text.Replace("&", "&amp;");//.Replace(NewLine, "  <br />" + NewLine);

                    text += NewLine + "</Content>";
                    text += NewLine + "</" + _thisAssembly._name + ">";
                    text += "\0";
                }

                if (fileHandle != 0x0)
                {
                    Simulator.AppendToScriptErrorFile(fileHandle, text.ToCharArray());
                    Simulator.CloseScriptErrorFile(fileHandle);
                }

                return true;
            }
            catch (StackOverflowException) { throw; }
            catch
            {
                return false;
            }
        }
    }
    #endregion // class:LogUtils
}
