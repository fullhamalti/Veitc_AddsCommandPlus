// Copyright © 2020 Fullham Alfayet
// Licensed under terms of the GPL Version 3. See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Sims3.SimIFace;
using Sims3.Gameplay;
using Sims3.SimIFace.CAS;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;

namespace Veitc.AddsCommandPlus
{
    #region class:DEBUG_Utils
    internal static class DEBUG_Utils
    {

        // Fields
        public static readonly string NewLine = System.Environment.NewLine;
        public static bool IsDEBUG =
#if DEBUG
            true;
#else
 false;
#endif

        public static
            bool InfoPackagesLib_HasPets(List<Sims3.UI.GameEntry.UISimInfo> householdMembers)
        {
            foreach (var householdSim in householdMembers.ToArray())
            {
                if (householdSim.Species != 0 && householdSim.Species != CASAgeGenderFlags.Human)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetHouseholdInfo(Household household, bool notile, string msg)
        {
            string logText = null;
            if (household != null)
            {
                if (msg == null) msg = "";
                logText = "";
                if (!notile)
                    logText += "Household Name: " + household.Name + NewLine;
                //else 
                //    logText += msg + "Household: " + household.Name + NewLine;
                logText += msg + " Id: " + household.HouseholdId + NewLine;
                try
                {
                    logText += msg + " Has Been Destroyed: " + household.HasBeenDestroyed + NewLine;

                    if (!string.IsNullOrEmpty(household.BioText))
                        logText += msg + " Bio: " + household.BioText + NewLine;

                    logText += msg + " Family Funds: " + EAText.GetMoneyString(household.mFamilyFunds) + NewLine;
                    logText += msg + " Delinquent Funds: " + EAText.GetMoneyString(household.mDelinquentFunds) + NewLine;
                    logText += msg + " Inited: " + household.mbInited + NewLine;
                    logText += msg + " Ancient Coin Count: " + EAText.GetMoneyString(household.mAncientCoinCount) + NewLine;
                    logText += msg + " UnPaid Bills: " + EAText.GetMoneyString(household.mUnpaidBills) + NewLine;
                    try
                    {
                        if (household.mMoneySaved != null)
                        {
                            long it = 0;
                            foreach (var item in household.mMoneySaved)
                                it += item;
                            logText += msg + " Money Saved: " + EAText.GetMoneyString(it) + ", Length: " + household.mMoneySaved.Length + NewLine;
                        }
                    }
                    catch (ResetException)
                    { throw; }
                    catch
                    { }
                }
                catch (ResetException)
                {
                    throw;
                }
                catch
                { }

                logText += msg + " -----------------------------------------------" + NewLine;
                try
                {
                    logText += msg + "  Active Household: " + (household == Household.ActiveHousehold || (PlumbBob.sSingleton != null && PlumbBob.sSingleton.mSelectedActor != null && household == PlumbBob.sSingleton.mSelectedActor.Household)) + NewLine;
                }
                catch (ResetException)
                {

                    throw;
                }
                catch
                {
                    logText += msg + "  Active Household: False" + NewLine;
                }

                logText += msg + "  Servobot Household: " + (household == Household.sServobotHousehold) + NewLine;
                logText += msg + "  Alien Household: " + (household == Household.sAlienHousehold) + NewLine;
                logText += msg + "  Mermaid Household: " + (household == Household.sMermaidHousehold) + NewLine;
                logText += msg + "  Previous Traveler Household: " + (household == Household.sPreviousTravelerHousehold) + NewLine;
                logText += msg + "  Service NPC Household: " + (household == Household.sNpcHousehold) + NewLine;
                logText += msg + "  Pet Household: " + (household == Household.sPetHousehold) + NewLine;
                logText += msg + "  Tourist Household: " + (household == Household.sTouristHousehold) + NewLine;
                logText += msg + "  Travel Household: " + (household == Comman.TravelHousehold) + NewLine;
                logText += msg + " -----------------------------------------------" + NewLine;
                Household.Members me = household.mMembers;
                if (me != null)
                {
                    try
                    {
                        logText += msg + " Members: " + household.NumMembers + NewLine;
                    }
                    catch (ResetException)
                    {
                        throw;
                    }
                    catch
                    { }
                }
                if (me != null && me.mAllSimDescriptions != null)
                {
                    try
                    {
                        foreach (SimDescription sim2 in me.mAllSimDescriptions)
                        {
                            if (sim2 != null)
                            {
                                logText += msg + "  Member: (" + sim2.FullName + ", Id: " + sim2.mSimDescriptionId + ")" + NewLine;
                            }
                            else logText += msg + "  Member: (NULL)" + NewLine;
                        }
                    }
                    catch (ResetException)
                    {
                        throw;
                    }
                    catch
                    { }
                }
                try
                {
                    if (household.VirtualLotHome != null)
                    {
                        // if (!string.IsNullOrEmpty(household.VirtualLotHome.Name))
                        logText += msg + " Virtual Home Lot: " + (string.IsNullOrEmpty(household.VirtualLotHome.Name) ? "No Name" : household.VirtualLotHome.Name) + NewLine;
                        logText += msg + " Virtual Home Lot Id: " + household.mVirtualLotId + NewLine;
                        logText += msg + " Virtual Home Address: " + (string.IsNullOrEmpty(household.VirtualLotHome.Address) ? "No Address" : household.VirtualLotHome.Address) + NewLine;
                    }
                    else
                    {
                        logText += msg + " No Virtual Lot Home" + NewLine;
                    }
                }
                catch (ResetException)
                {
                    throw;
                }
                catch
                { }
                try
                {
                    if (household.LotHome != null)
                    {
                        //if (!string.IsNullOrEmpty(household.LotHome.Name))
                        logText += msg + " Home Lot: " + (string.IsNullOrEmpty(household.LotHome.Name) ? "No Name" : household.LotHome.Name) + NewLine;
                        logText += msg + " Home Lot Id: " + household.mLotId + NewLine;
                        //if (!string.IsNullOrEmpty(household.LotHome.Address))
                        logText += msg + " Home Address: " + (string.IsNullOrEmpty(household.LotHome.Address) ? "No Address" : household.LotHome.Address);//+ NewLine;
                    }
                    else
                    {
                        logText += msg + " No Lot Home";// + NewLine;
                    }
                }
                catch (ResetException)
                {
                    throw;
                }
                catch
                { }

            }
            return logText;
        }

        public static
           int InfoPackagesLib_PetsCount(List<Sims3.UI.GameEntry.UISimInfo> householdMembers)
        {
            int i = 0;
            foreach (var householdSim in householdMembers.ToArray())
            {
                if (householdSim.Species != 0 && householdSim.Species != CASAgeGenderFlags.Human)
                {
                    i++;
                }
            }
            return i;
        }

        public static
            string InfoPackagesLib()
        {
            if (BinModel.sBinModel == null || BinModel.sBinModel.mExportBin == null)
                return null;

            if (BinModel.sBinModel.mExportBin._items == null)
                BinModel.sBinModel.mExportBin = new List<ExportBinContents>();

            if (BinModel.sBinModel.mExportBin.Count == 0)
                return null;

            //string[] packageNames = ExportBin.GetPackageNames();
            //if (packageNames == null || packageNames.Length == 0)
            //{
            //    return null;
            //}

            var exportBinlist = new List<ExportBinContents>(BinModel.sBinModel.mExportBin ?? new List<ExportBinContents>());
            Comparison<ExportBinContents> sortTime = delegate(ExportBinContents a, ExportBinContents b)
            {
                if (a == b)
                {
                    return 0;
                }
                if (a == null)
                {
                    return -1;
                }
                if (b == null)
                {
                    return 1;
                }
                return b.ExportDateTime.CompareTo(a.ExportDateTime);
            };

            try
            {
                if (sortTime != null)
                    exportBinlist.Sort(sortTime);
            }
            catch (Exception)
            { }

            StringBuilder sb = new StringBuilder();

            sb.Append("InfoPackagesLib() Count: " + BinModel.sBinModel.mExportBin.Count);
            sb.AppendLine();

            sb.Append("--------------------- Start ---------------------");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();

            sb.Append("-------------------------------------------------------");
            sb.AppendLine();

            int no = 0;
            foreach (var item in exportBinlist.ToArray())
            {
                if (item == null)
                    continue;
                no++;

                sb.Append("No: " + no);
                sb.AppendLine();
                // -----------------

                sb.Append("Package Name: " + (item.PackageName ?? "NULL"));
                sb.AppendLine();
                // -----------------

                sb.Append("Export Date: " + item.ExportDateTime.ToString());
                sb.AppendLine();
                // -----------------

                sb.Append("Modified Date: " + item.ModifiedDateTime.ToString());
                sb.AppendLine();

                sb.Append("-------------------------------------------------------");
                sb.AppendLine();
                // -----------------

                sb.AppendLine();

                try
                {
                    string t;
                    switch (item.mExportBinType)
                    {
                        case Sims3.UI.GameEntry.ExportBinType.Household:
                            sb.Append("-Household-");
                            sb.AppendLine();

                            sb.Append("  Name: " + item.HouseholdName + "\n");
                            sb.Append("  ID: " + item.HouseholdId + "\n");
                            t = string.IsNullOrEmpty(item.HouseholdBio) ? "[None]" : item.HouseholdBio;
                            sb.Append("  Bio: " + t + "\n");
                            sb.Append("  Funds: " + item.HouseholdFunds + "\n");
                            sb.Append("  Difficulty: " + item.HouseholdDifficulty + "\n");
                            sb.Append("  Sims Count: " + item.HouseholdSims.Count + "\n");
                            sb.Append("  Pets Count: " + InfoPackagesLib_PetsCount(item.HouseholdSims) + "\n");
                            sb.Append("  Has Pets: " + InfoPackagesLib_HasPets(item.HouseholdSims) + "\n");

                            break;
                        case Sims3.UI.GameEntry.ExportBinType.HouseholdLot:
                            sb.Append("-Household And Lot-");
                            sb.AppendLine();

                            sb.Append("  Lot Name: " + item.LotName + "\n");
                            sb.Append("  Lot ID: " + item.LotId + "\n");
                            sb.Append("  Lot Worth: " + item.LotWorth + "\n");
                            sb.Append("  Lot Size_X: " + item.LotContentsSizeX + "\n");
                            sb.Append("  Lot Size_Y: " + item.LotContentsSizeY + "\n");
                            t = string.IsNullOrEmpty(item.LotDescription) ? "[None]" : item.LotDescription;
                            sb.Append("  Lot Description: " + t + "\n");

                            sb.Append("  ---------------------------------------------------\n");

                            sb.Append("  Household Name: " + item.HouseholdName + "\n");
                            sb.Append("  Household ID: " + item.HouseholdId + "\n");
                            t = string.IsNullOrEmpty(item.HouseholdBio) ? "[None]" : item.HouseholdBio;
                            sb.Append("  Household Bio: " + t + "\n");
                            sb.Append("  Household Funds: " + item.HouseholdFunds + "\n");
                            sb.Append("  Household Difficulty: " + item.HouseholdDifficulty + "\n");
                            sb.Append("  Household Sims Count: " + item.HouseholdSims.Count + "\n");
                            sb.Append("  Household Pets Count: " + InfoPackagesLib_PetsCount(item.HouseholdSims) + "\n");
                            sb.Append("  Household Has Pets: " + InfoPackagesLib_HasPets(item.HouseholdSims) + "\n");

                            break;
                        case Sims3.UI.GameEntry.ExportBinType.Lot:
                            sb.Append("-Lot-");
                            sb.AppendLine();

                            sb.Append("  Name: " + item.LotName + "\n");
                            sb.Append("  ID: " + item.LotId + "\n");
                            sb.Append("  Lot Worth: " + item.LotWorth + "\n");
                            sb.Append("  Size_X: " + item.LotContentsSizeX + "\n");
                            sb.Append("  Size_Y: " + item.LotContentsSizeY + "\n");
                            t = string.IsNullOrEmpty(item.LotDescription) ? "[None]" : item.LotDescription;
                            sb.Append("  Description: " + t + "\n");

                            break;
                        default:
                            sb.Append("Unkrown? Type: " + item.mExportBinType);
                            continue;
                    }
                }
                // see MsCorlibModifed
                catch (ExecutionEngineException)
                {
                    sb.Append("-- Canceled --");
                    break;
                }
                catch (StackOverflowException)
                {
                    break;
                }
                catch (Exception ex)
                { sb.Append("-- Bad Package ExceptionMessage: " + ex.Message + " --"); }

                sb.AppendLine();
                sb.Append("-------------------------------------------------------");
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("--------------------- End ---------------------");
            exportBinlist.Clear();

            //uint key = 0;
            //string createFile = Simulator.CreateExportFile(ref key, "InfoPackageLib");
            //
            //if (key == 0 || createFile == null)
            //    return sb.ToString();
            //
            //sb.Append("\nFile Name: " + createFile + "\nDate: " + DateTime.Now.ToString());
            //sb.AppendLine();
            //sb.AppendLine();
            //for (int i = 0; i < 15; i++)
            //{
            //    sb.Append('\0');
            //}
            //
            //var result = sb.ToString();
            ////var v = result.ToCharArray();
            //
            //if (!Simulator.AppendToScriptErrorFile(key, result.ToCharArray()))
            //    return result;
            //
            //Simulator.CloseScriptErrorFile(key);

            var result = sb.ToString();
            LogUtils.WriteLogWithPrefix(result, "InfoPackagesLib", false, false);

            return result;
        }
        public static
           string _GetHouseholdLiteInfo(Household hold)
        {
            if (hold == null)
                return "Info: NULL";
            return "Info: " + hold.Name + ", Id: " + hold.mHouseholdId;
        }
        public static string GetObjectStackTrace(ulong ObjectHandle)
        {
            ScriptCore.TaskContext context;
            if (!ScriptCore.TaskControl.GetTaskContext(ObjectHandle, true, out context) || context.mFrames == null)
            {
                return "<no call stack>";
            }
            StringBuilder stringBuilder = new StringBuilder();
            ScriptCore.TaskFieldReference fieldRef = default(ScriptCore.TaskFieldReference);
            for (int num = context.mFrames.Length - 1; num >= 0; num--)
            {
                ScriptCore.TaskFrame taskFrame = context.mFrames[num];
                fieldRef.mFrameIndex = num;
                if (taskFrame.mMethodHandle.Value != IntPtr.Zero)
                {
                    MethodBase methodInfo = MethodBase.GetMethodFromHandle(taskFrame.mMethodHandle);
                    stringBuilder.Append("    at ");

                    var methedinfoC = methodInfo as MethodInfo;
                    if (methedinfoC != null)
                    {
                        stringBuilder.Append(methedinfoC.ReturnType.Name);
                    }
                    else
                        stringBuilder.Append("Void");
                    //
                    stringBuilder.Append(' ');


                    Type declaringType = methodInfo.DeclaringType;
                    stringBuilder.Append(declaringType.FullName);
                    stringBuilder.Append('.');
                    //stringBuilder.Append(declaringType.Name);
                    //stringBuilder.Append('.');
                    stringBuilder.Append(methodInfo.Name);
                    stringBuilder.Append('(');
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (i != 0)
                        {
                            stringBuilder.Append(", ");
                        }
                        ParameterInfo parameterInfo = parameters[i];
                        Type parameterType = parameterInfo.ParameterType;
                        stringBuilder.Append(parameterType.Name);
                        stringBuilder.Append(' ');
                        stringBuilder.Append(parameterInfo.Name);
                        fieldRef.mFieldIndex = i;
                        switch (Type.GetTypeCode(parameterType))
                        {
                            case TypeCode.Boolean:
                                {
                                    object fieldValue = ScriptCore.TaskControl.GetFieldValue(ObjectHandle, fieldRef);
                                    if (fieldValue != null)
                                    {
                                        stringBuilder.Append(((bool)fieldValue) ? " = true" : " = false");
                                    }
                                    break;
                                }
                            case TypeCode.Char:
                                {
                                    object fieldValue = ScriptCore.TaskControl.GetFieldValue(ObjectHandle, fieldRef);
                                    if (fieldValue != null)
                                    {
                                        stringBuilder.AppendFormat(" = {0}", (int)(char)fieldValue);
                                    }
                                    break;
                                }
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                            case TypeCode.DateTime:
                                {
                                    object fieldValue = ScriptCore.TaskControl.GetFieldValue(ObjectHandle, fieldRef);
                                    if (fieldValue != null)
                                    {
                                        stringBuilder.AppendFormat(" = {0}", fieldValue);
                                    }
                                    break;
                                }
                            case TypeCode.String:
                                {
                                    object fieldValue = ScriptCore.TaskControl.GetFieldValue(ObjectHandle, fieldRef);
                                    if (fieldValue != null)
                                    {
                                        stringBuilder.AppendFormat(" = \"{0}\"", fieldValue);
                                    }
                                    else
                                    {
                                        stringBuilder.Append(" = null");
                                    }
                                    break;
                                }
                        }
                    }
                    stringBuilder.Append(')');
                    stringBuilder.Append('\n');
                }
                else
                {
                    stringBuilder.AppendLine("<Invalid method>");
                }
            }
            if (context.mNativeYieldCall != 0)
            {
                ScriptCore.TaskControl.TaskControl_ReleaseYieldingCall(context.mNativeYieldCall);
            }
            stringBuilder.Append("\nSleep: " + context.mSleepTicks);
            return stringBuilder.ToString();
        }
        public static void Print(string Message)
        {
            if (!IsDEBUG)
                return;
            Comman.PrintMessage(Message, false, 100);
        }
        public static void ShowAllTask(bool TrimFast)
        {
            if (Simulator.CheckYieldingContext(false))
            {
                for (int sleep = 0; sleep < 100; sleep++)
                {
                    Simulator.Sleep(0);
                }
            }

            StringBuilder tar = new StringBuilder();
            tar.Append("\nTasks\n");
            int c = 0;
            Dictionary<ulong, ITask> dictionary = new Dictionary<ulong, ITask>(ScriptCore.Simulator.mObjHash);
            if (TrimFast)
            {
                foreach (var item in dictionary)
                {
                    if (item.Value == null)
                        continue;

                    ScriptCore.TaskContext context;
                    if (!ScriptCore.TaskControl.GetTaskContext(item.Key, true, out context) || context.mFrames == null)
                    {
                        continue;
                    }
                    c++;
                    tar.Append("\nNo: " + c);
                    try
                    {
                        tar.Append("\nName Task: " + item.Value.GetType().AssemblyQualifiedName);
                        tar.Append("\nObject Id: " + item.Key);
                        tar.Append("\nObject Id Task: " + item.Value.ObjectId);

                    }
                    catch
                    { }
                    try
                    {
                        tar.Append("\nClass Name: " + item.Value.ClassName);
                    }
                    catch
                    { }

                    try
                    {
                        tar.Append("\nExecute Type: " + item.Value.ExecuteType);
                    }
                    catch
                    { }


                    try
                    {
                        tar.Append("\nTask.ToString() {\n" + item.Value.ToString() + "\n}");
                    }
                    catch
                    { }
                    try
                    {
                        tar.Append("\nHash Code: " + item.Value.GetHashCode());
                    }
                    catch
                    { }

                    try
                    {
                        string st = GetObjectStackTrace(item.Key);
                        if (st == "<no call stack>")
                            st = GetObjectStackTrace(item.Value.ObjectId.mValue);
                        tar.Append("\nStack Trace:\n" + st + "\n");

                    }
                    catch
                    { }
                }
            }
            else // TrimFast
            {
                foreach (var item in dictionary)
                {

                    if (item.Value == null)
                        continue;
                    c++;
                    //tar.Append("\n");

                    tar.Append("\nNo: " + c);
                    try
                    {
                        tar.Append("\nName Task: " + item.Value.GetType().AssemblyQualifiedName);
                        tar.Append("\nObject Id: " + item.Key);
                        tar.Append("\nObject Id Task: " + item.Value.ObjectId);

                    }
                    catch
                    { }
                    try
                    {
                        tar.Append("\nClass Name: " + item.Value.ClassName);
                    }
                    catch
                    { }

                    try
                    {
                        tar.Append("\nExecute Type: " + item.Value.ExecuteType);
                    }
                    catch
                    { }


                    try
                    {
                        tar.Append("\nTask.ToString() {\n" + item.Value.ToString() + "\n}");
                    }
                    catch
                    { }
                    try
                    {
                        tar.Append("\nHash Code: " + item.Value.GetHashCode());
                    }
                    catch
                    { }

                    try
                    {
                        string st = GetObjectStackTrace(item.Key);
                        if (st == "<no call stack>")
                            st = GetObjectStackTrace(item.Value.ObjectId.mValue);
                        tar.Append("\nStack Trace:\n" + st + "\n");

                    }
                    catch
                    { }
                }
            }


            tar.Append("\nEnd Tasks\n");

            LogUtils.WriteLogWithPrefix(tar.ToString(), _thisAssembly._name + "_ShowAllTask", false, false);
        }
    }
    #endregion // class:DEBUG_Utils
}
