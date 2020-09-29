// Copyright © 2020 Fullham Alfayet
// Licensed under terms of the GPL Version 3. See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;
using Sims3.SimIFace;
using Sims3.Gameplay.Core;
using Sims3.UI;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.CAS;
using Sims3.SimIFace.CustomContent;
using Sims3.Gameplay;
using Sims3.Gameplay.Utilities;
using Sims3.UI.GameEntry;
using System.Reflection;
using Sims3.Gameplay.Autonomy;
using Sims3.SimIFace.CAS;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Objects.Island;
using Sims3.Gameplay.Academics;
using Sims3.Gameplay.Scenarios;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.PetSystems;

namespace Veitc.AddsCommandPlus
{
    public static class VCommands
    {
    	public static void InitClass() { }
    	
        public static string _GetTextCommands()
        {
            return
                "[alllotclean|aasetp|exhousehold|savelot|excashousehold|forcesavegame|bulot|allpossimtocamera|dtargetobj|forcesetaa|forcesetaa2|ustsim" +
                "|testerrortrap|maxmood|fixsimbad|sdnoage|showalltask|fixahsims|settextpos|infoplib|importlot|importhouse|spt|forcelivemode|autop|targetct" +
                "|findhouse|moveinfromhouse|moveoutfromhouse|hideui|removeallfire|allbulot|forceallmovein|savenpchousehold|loadnpchousehold" +
                "|saveallsimdesc|copyallsimdesc|rallg|scpt|forcesetaa3|debuginfo|testbreak|sethdp|houseinvsim|fixallsimdesc/2|rnewpetpool|removepetpool" +
                "|savealllot|saveallhh|saveallhhandlot|newhhfromah|newhhfromts|savealllotnct|saveallhhnct|installdc|addsimtohousehold/2" +
                //"|" +
                //"|c" +
                //"|c" +
                //"|c" +
                //"|c" +
                "]";
        }

        public static int __ShowCommands()
        {
            //if (global:: Sims3.UI.NotificationManager.Instance == null) 
            //    return false;

            string text = "Usage: "  + "veitc" + " [text]";

            text += "\n" + "[text] = Other Mode";
            text += "\n" + "Try: " + "veitc" + " alllotclean";
            text += "\n" + "Helps";
            text += "\n" + _GetTextCommands();

            if (global:: Sims3.UI.NotificationManager.Instance == null)
            {
                SimpleMessageDialog.Show(_thisAssembly._name, text);
            }
            else Comman.PrintMessage(text, true, 10);

            return 0;
        }

        #region Fields
        public static bool debuginfo_data = false;
        public static ObjectGuid autop_objectid = default(ObjectGuid);
        private static CommodityKind[] Sim_MaxMood_sCommodities = new CommodityKind[14] {
		    CommodityKind.VampireThirst,
		    CommodityKind.Hygiene,
		    CommodityKind.Bladder,
		    CommodityKind.Energy,
		    CommodityKind.Fun,
		    CommodityKind.Hunger,
		    CommodityKind.Social,
		    CommodityKind.HorseThirst,
		    CommodityKind.HorseExercise,
		    CommodityKind.CatScratch,
		    CommodityKind.DogDestruction,
		    CommodityKind.AlienBrainPower,
		    CommodityKind.Maintenence,
		    CommodityKind.BatteryPower
	    };
        public static bool bHideGameUI__ = false;
        #endregion // Fields

        #region Metheds
        internal static void Test_Debugger_Break()
        {
//#if unusd
            if (Assembly.GetCallingAssembly() == Assembly.GetExecutingAssembly())
            {
                if (!Simulator.CheckYieldingContext(false))
                {
                    DEBUG_Utils.Print("Test_Debugger_Break()\nSimulator.CheckYieldingContext(false) failed");
                    return;
                }

                ////////////////////////
                var vEnableModalDialogs02 = false;
                var vEnableModalDialogs = ModalDialog.EnableModalDialogs;
                if (!vEnableModalDialogs)
                {
                    vEnableModalDialogs02 = true;
                    ModalDialog.EnableModalDialogs = true;
                }

                try
                {
                    if (!AcceptCancelDialog.Show("Are you sure?\nForce Game freeze\nYou need Debugger app\nAttach to TS3/w.exe!"))
                        return;
                }
                finally
                {
                    if (vEnableModalDialogs02)
                        ModalDialog.EnableModalDialogs = vEnableModalDialogs;
                }
                ////////////////////////

                Comman.Debugger_Break();

                //// TS3.exe/TS3W.exe no export func. except CAW App Sims3Comman.dll
                //
                //// World_*
                ////ScriptCore.World.World_IsEditInGameFromWBModeImpl(); // 00788640 World_IsEditInGameFromWBModeImpl x86 code: mov eax,[11EDBC4] 
                ////Comman.Debugger_Break();
                //
                ////ScriptCore.World.World_IsObjectOnScreenImpl(0xEAFCFAFC);
                ////Comman.Debugger_Break();
                //
                //ScriptCore.World.World_LoadTickImpl(); 
                //Comman.Debugger_Break();
                //
                //// Route_*
                //ScriptCore.Route.Route_Plan(0xFECFFFF1);
                //Comman.Debugger_Break();
                //
                //ScriptCore.Route.Route_Replan(0xFECFFFF2);
                //Comman.Debugger_Break();
                //
                //// LocalizedStringService_*
                //ScriptCore.LocalizedStringService.LocalizedStringService_GetLocalizedStringByString("ScriptCore::LocalizedStringService::LocalizedStringService_GetLocalizedStringByString");
                //Comman.Debugger_Break();
                //
                //ScriptCore.LocalizedStringService.LocalizedStringService_GetLocalizedStringByUInt64(0xFEEAFFC1);
                //Comman.Debugger_Break();
                //
                //// LoadSaveManager_*
                //ScriptCore.LoadSaveManager.LoadSaveManager_SaveGame_Impl(0xFBBBBBC1, true, true);
                //Comman.Debugger_Break();
                //
                //// DownloadContent_*
                ////ScriptCore.DownloadContent.DownloadContent_GenerateGUIDImpl();
                ////Comman.Debugger_Break();
                //
                //
                //// SACS_*
                //ScriptCore.SACS.SACS_RequestStateImpl(0xFACFFFF1, 0xFACFFFF2, 0xFACFFFF3, (Sims3.SimIFace.SACS.DriverRequestFlags)0xFACFFFF4, 0xFACFFFF5, 0xFACFFFF6, 0xFACFFFF7);
                //Comman.Debugger_Break();
                //
                ////ScriptCore.SACS.SACS_AcquireDriverNoYieldImpl(0, 245111456, 7452, 64212, 1);
                ////Comman.Debugger_Break();
                //
                ////ScriptCore.SACS.SACS_AcquireDriverImpl(0, 84265542, 2456, 421142, 1, null);
                ////Comman.Debugger_Break();
                //
                //// Random_*
                ////ScriptCore.Random.Random_NextDoubleImpl();
                //
                //// Objects_*
                ////ScriptCore.Objects.Objects_IsValid((ulong)ScriptCore.Random.Random_NextDoubleImpl());
                ////Comman.Debugger_Break();
                //
                //// Command_*
                ////ScriptCore.CommandSystem.Command_ExecuteCommandStringImpl("veitc");
                ////Comman.Debugger_Break();
                //
                //// ProfilerUtils_*
                ////try
                ////{
                ////    ScriptCore.ProfilerUtils.ProfilerUtils_BeginEventImpl("xt");
                ////} catch (EntryPointNotFoundException) { }
                ////Comman.Debugger_Break();
                ////
                ////try
                ////{
                ////    ScriptCore.ProfilerUtils.ProfilerUtils_BeginLoadEventImpl("xt");
                ////} catch (EntryPointNotFoundException) { }
                ////Comman.Debugger_Break();
                //
                //// Camera_*
                ////ScriptCore.CameraController.Camera_GetControllerType();
                ////Comman.Debugger_Break();
                //
                ////ScriptCore.CameraController.Camera_GetTarget();
                ////Comman.Debugger_Break();
                //
                ////ScriptCore.CameraController.Camera_GetPosition();
                ////Comman.Debugger_Break();
                //
                //// Simulator_*
                ////ScriptCore.Simulator.Simulator_DidSecuritySystemFailImpl();
                ////Comman.Debugger_Break();
                //
                ////ScriptCore.Simulator.Simulator_GetCurrentTaskImpl();
                ////Comman.Debugger_Break();
                //
                //// EATrace_*
                ////try
                ////{
                ////    ScriptCore.EATrace.EATrace_ShouldLogImpl(1, "test");
                ////    Comman.Debugger_Break();
                ////
                ////    ScriptCore.EATrace.EATrace_NotifyImpl(true, "test", null, -1, null);
                ////    Comman.Debugger_Break();
                ////
                ////    ScriptCore.EATrace.EATrace_AssertImpl(false, "test", null, 0, null);
                ////    Comman.Debugger_Break();
                ////}
                ////catch (EntryPointNotFoundException) { }
                //
                //
                //// EAText_*
                ////ScriptCore.EAText.EAText_GetMoneyString(100000);
                ////Comman.Debugger_Break();
                //
                //// UserToolUtils_*
                //ScriptCore.UserToolUtils.UserToolUtils_CreateNewCollectionImpl("TestName", 1, 0, 0, 0); // check anti-copy
                //Comman.Debugger_Break();
                //
                //// DeviceConfig_*
                //ScriptCore.DeviceConfig.DeviceConfig_AuthenticateDisc();
                //Comman.Debugger_Break();
                //
                //try
                //{
                //    ScriptCore.DeviceConfig.DeviceConfig_IsRegistryTelemetryEnabled();
                //    Comman.Debugger_Break();
                //}
                //catch (EntryPointNotFoundException) { }
                //
                //ScriptCore.DeviceConfig.DeviceConfig_IsDriverOld();
                //Comman.Debugger_Break();
                //
                //ScriptCore.DeviceConfig.DeviceConfig_GetProcessorLevel();
                //Comman.Debugger_Break();
                //
                //ScriptCore.DeviceConfig.DeviceConfig_GetVideoCardLevel();
                //Comman.Debugger_Break();
                //
                //// GameUtils_*
                //
                //ScriptCore.GameUtils.GameUtils_GetModFilesName(ScriptCore.GameUtils.GameUtils_GetModFilesCount());
                //Comman.Debugger_Break();
                //
                //ScriptCore.GameUtils.GameUtils_IsValidInstallation();
                //Comman.Debugger_Break();
                //
                //ScriptCore.GameUtils.GameUtils_StartLauncherImpl();
                //Comman.Debugger_Break();
                //
                //// UIManager_*
                //ScriptCore.UIManager.UIManager_SetResolutionImpl(1366, 768, 0, false);
                //Comman.Debugger_Break();
                //
                //
                //// Query_*
                //ScriptCore.Queries.Query_GetObjects(typeof(GameObject));
                //Comman.Debugger_Break();
                //
                //ScriptCore.Queries.Query_CountObjects(typeof(GameObject));
                //Comman.Debugger_Break();
                //
                //var v0 = default(Vector3);
                //var v1 = default(float);
                //ScriptCore.World.World_FindGoodLocation(0x0ABFFFF1, default(Vector3), 0x0ABFFFF2, 0x0ABFFFF3, 0x0ABFFFF4, 0x0ABFFFF5, 0x0ABFFFF6, 0x0ABFFFF7, null, null, ref v0, ref v1, true, 0x0ABFFFF8, 0x0ABFFFF9);

                //ScriptCore.GameUtils.GameUtils_GetWorldName();
                //Comman.Debugger_Break();
                //
                //Mono.Runtime.mono_runtime_install_handlers();
                //Comman.Debugger_Break();
                //
                //ScriptCore.VideoRecorder.VideoRecorder_TakeSnapshot();
                //Comman.Debugger_Break();
                //
                //ScriptCore.GameUtils.GameUtils_GetWorldType();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_DeleteSaveFileImpl("Veitc.dll");
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_IsCallstackProfilingEnabled();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_IsPausedImpl();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_TransitionToQuitImpl();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_IsSaveGameCorruptedByEP1("Veitc_C.dll");
                //Comman.Debugger_Break();
                //
                //
                //ScriptCore.Simulator.Simulator_DestroyObjectImpl(0);
                //Comman.Debugger_Break();
                ////ScriptCore.Simulator.Simulator_CreateObjectImpl(0xFFCADDD1, 0xFFCADDD2, 0xFFCADDD3, null);
                ////Comman.Debugger_Break();
                //ScriptCore.Simulator.Simulator_GetTickCountImpl();
                //Comman.Debugger_Break();
                //ScriptCore.Simulator.Simulator_AddObjectImpl(0xFFBDDDD1, 0xFFBDDDD2);
                //Comman.Debugger_Break();
                //ScriptCore.Simulator.Simulator_DisableScriptImpl(0xFFBDDDC1);
                //Comman.Debugger_Break();
                //ScriptCore.Simulator.Simulator_WakeImpl(0xFFBDDDB1, true);
                //Comman.Debugger_Break();
                //
                //var o = default(ResourceKey);
                //ScriptCore.DownloadContent.DownloadContent_SaveTravelSim(null, null, null, default(ResourceKey), ref o);


                //ScriptCore.GameUtils.GameUtils_SetGameTimeScaleImpl(100);
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_PauseImpl();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_SetGameTimeSpeedLevel(3);
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_UnpauseImpl();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_GetGameTimeSpeedLevel();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_GetGameTimeScaleImpl();
                //Comman.Debugger_Break();
                //ScriptCore.GameUtils.GameUtils_FreezeLotLODs(true);
                //Comman.Debugger_Break();
                ScriptCore.GameUtils.GameUtils_GetCurrentMemoryUsage();
                Comman.Debugger_Break();
                ScriptCore.GameUtils.GameUtils_GetFrameNumber();
                Comman.Debugger_Break();
                DateTime.GetNow();
                Comman.Debugger_Break();
                ScriptCore.DownloadContent.DownloadContent_InstallContentImpl("HelloWorld.Sims3Pack");

                throw new SacsErrorException("Test_Debugger_Break() Done! Create Mod The Sims 3 RTX and 64bit Like Minecraft RTX :)");
            }
            else throw new MethodAccessException("Assembly.GetCallingAssembly() == Assembly.GetExecutingAssembly() failed");
//#endif
            //throw new SacsErrorException("Test_Debugger_Break() Done! Create Mod The Sims 3 RTX and 64bit Like Minecraft RTX :)");
        }
        public static bool IsKeepGrave(Sims3.Gameplay.Objects.Urnstone grave)
        {
            if (grave == null || grave.HasBeenDestroyed)
                return false;

            SimDescription deadSimDesc = grave.DeadSimsDescription;
            if (deadSimDesc != null)
            {
                Sim createdSim = deadSimDesc.CreatedSim;
                if (createdSim != null)
                {
                    if (createdSim.SimDescription == deadSimDesc)
                    {
                        if (!deadSimDesc.IsGhost && !deadSimDesc.IsDead)
                            return false;
                    }
                }
            }

            Lot lotCurrent = grave.LotCurrent;
            if (lotCurrent != null)
            {
                if (lotCurrent.CommercialLotSubType == CommercialLotSubType.kGraveyard)
                    return true;
                if (deadSimDesc == null && Comman.SCO_GetObjectsOnLot<IMausoleum>(lotCurrent).Length > 0)
                    return true;
            }

            foreach (var mausoleum in Comman.SCO_GetObjects<IMausoleum>())
            {
                if (mausoleum == null)
                    continue;

                Inventory invetory = mausoleum.Inventory;
                if (invetory == null || invetory.mInventoryItems == null || invetory.mItems == null)
                    continue;

                if (invetory.Contains(grave))
                    return true;
            }

            return false;
        }
        public static List<SimDescription> CopyAllSimDesc()
        {
            Simulator.CheckYieldingContext(true);
            if (Bin.Singleton == null)
                return null;
            var si = Comman.GetAllSimDescription();
            if (si == null || si.Count == 0)
                return null;

            Household household = Household.Create();
            if (household == null)
            {
                // EA bug
                return null;
            }
           

            var hMembers = household.mMembers;
            if (hMembers == null)
            {
                // EA bug
                household.Destroy();
                return null;
            }

            household.SetName("CopyAllSimDesc World Name " + World.GetWorldFileName());
            household.mBioText = "Date " + Comman.GetNowTimeToStr();

            var simList = si.ToArray();
            bool shouldSleep = Simulator.CheckYieldingContext(false) && simList.Length > 190;
            int i = 0;

            foreach (var item in simList)
            {
                if (item == null)
                    continue;

                if (item.mHairColors == null || item.mSimDescriptionId == 0 || item.mOutfits == null)
                    continue;

                if (!Comman.SD_OutfitsIsValid(item))
                    continue;

                if (item.IsValidDescription)
                {
                    hMembers.mAllSimDescriptions.Add(item);

                    if (item.IsPet)
                        hMembers.mPetSimDescriptions.Add(item);
                    else
                        hMembers.mSimDescriptions.Add(item);
                }
                if (shouldSleep)
                {
                    i++;
                    if (i > 30)
                    {
                        i = 0;
                        Simulator.Sleep(0);
                    }
                }
            }
            if (hMembers.mAllSimDescriptions.Count == 0)
            {
                household.Destroy();
                return null;
            }
            household.mBioText += "\nCount " + hMembers.mAllSimDescriptions.Count;
            string packageFile = null;
            try
            {
                try
                {
                    packageFile = Proxies.HouseholdContentsProxy.VExportHousehold(Bin.Singleton, household, false, false, true, true);
                }
                finally
                {
                    hMembers.mAllSimDescriptions.Clear();

                    hMembers.mPetSimDescriptions.Clear();

                    hMembers.mSimDescriptions.Clear();

                    household.Destroy();
                }
            }
            // caused by call to Sleep(uint)
            catch (ExecutionEngineException e)
            { Comman.DPrintException(null, e); }

            // script error 
            if (packageFile == null)
                return null;

            Comman.PrintMessage("CopyAllSimDesc\n" +  packageFile, false, float.MaxValue);

            if (Simulator.CheckYieldingContext(false))
                Simulator.Sleep(0);

            var ts = Proxies.HouseholdContentsProxy.Import(packageFile);
            if (ts == null || ts.Household == null)
            {
                DEBUG_Utils.Print("CopyAllSimDesc\n" + "ImportHousehold(" + packageFile + ") failed!");
                return null;
            }
            LibraryUtls.AutoMoveInLotFromHousehold(ts.Household); // prevent auto destroy or kill homeless.

            if (ts.Household.AllSimDescriptions.Count == 0 || ts.Household.AllSimDescriptions.Count < 0) return null;
            i = 0;
            List<SimDescription> s = new List<SimDescription>(ts.Household.AllSimDescriptions.Count + 5);
            foreach (var item in ts.Household.AllSimDescriptions)
            {
                if (item == null)
                    continue;
                s.Add(item);
                Comman.SD_SetID(item, (ulong)RandomUtil.GetInt(int.MaxValue));
                if (shouldSleep)
                {
                    i++;
                    if (i > 30)
                    {
                        i = 0;
                        Simulator.Sleep(0);
                    }
                }
            }

            return s;
        }

        public static
           Household sTravelHousehold
        {
            get
            {
                if (GameStates.sTravelData != null)
                {
                    return GameStates.sTravelData.mTravelHouse;
                }
                return null;
            }
            set
            {
                if (GameStates.sTravelData != null)
                {
                    GameStates.sTravelData.mTravelHouse = value;
                }
            }
        }

        public static
            Household SetHouseholdFieldStaticSpecial(out int result)
        {
            result = 0;
            if (!Simulator.CheckYieldingContext(false))
                return null;

            int r = 0;
            Household hold = Comman.FindHouse(false);

            //if (hold != null && (//hold == Household.ActiveHousehold || 
            //    (PlumbBob.sSingleton != null && PlumbBob.sSingleton.mSelectedActor != null && hold == PlumbBob.sSingleton.mSelectedActor.Household && PlumbBob.sCurrentNonNullHousehold != null)))
            //{
            //    Comman.PrintMessage("SetHouseholdFieldStaticSpecial()\nCannot be Active Household.", false, 100);
            //    return;
            //}

            Comman.PrintMessage(
                "ActiveHousehold:\n" +              DEBUG_Utils._GetHouseholdLiteInfo(PlumbBob.sCurrentNonNullHousehold) +
                "\nNpcHousehold:\n" +               DEBUG_Utils._GetHouseholdLiteInfo(Household.sNpcHousehold) +
                "\nPetHousehold:\n" +               DEBUG_Utils._GetHouseholdLiteInfo(Household.sPetHousehold) +
                "\nAlienHousehold:\n" +             DEBUG_Utils._GetHouseholdLiteInfo(Household.sAlienHousehold) +
                "\nMermaidHousehold:\n" +           DEBUG_Utils._GetHouseholdLiteInfo(Household.sMermaidHousehold) +
                "\nPreviousTravelerHousehold:\n" +  DEBUG_Utils._GetHouseholdLiteInfo(Household.sPreviousTravelerHousehold) +
                "\nServobotHousehold:\n" +          DEBUG_Utils._GetHouseholdLiteInfo(Household.sServobotHousehold) +
                "\nTouristHousehold:\n" +           DEBUG_Utils._GetHouseholdLiteInfo(Household.sTouristHousehold) +
                "\nTravelHousehold:\n" +            DEBUG_Utils._GetHouseholdLiteInfo(sTravelHousehold)
                // (GameStates.sTravelData != null ?   "\nTravelHousehold:\n" +            DEBUG_Utils._GetHouseholdLiteInfo(sTravelHousehold) : "")
            ,false, 100);


            const string textC 
            = 
                "0: Cancel\n" +
                "1: NPCHousehold\n" +
                "2: PetHousehold\n" +
                "3: AlienHousehold\n" +
                "4: MermaidHousehold\n" +
                "5: PreviousTravelerHousehold\n" +
                "6: ServobotHousehold\n" +
                "7: TouristHousehold\n" +
                "8: PlumbBobCurrentHousehold\n" +
                "9: TravelHousehold"
            ;

            int.TryParse(StringInputDialog.Show(
                "SetHouseholdSpecial\nWhat you want?", 
                "TravelData is null: " + (GameStates.sTravelData == null).ToString() + "\nHousehold " + (hold != null
                // && !string.IsNullOrEmpty(hold.Name) 
                ? "Name: " + hold.Name + "\n" : "NULL" + "\n") + textC, "0",
                2, StringInputDialog.Validation.Number), out r);

            result = r;
            switch (r)
            {
            case 0:
                return null;
            case 1:
                Household.sNpcHousehold = hold;
                return hold;
            case 2:
                Household.sPetHousehold = hold;
                return hold;
            case 3:
                Household.sAlienHousehold = hold;
                return hold;
            case 4:
                Household.sMermaidHousehold = hold;
                return hold;
            case 5:
                Household.sPreviousTravelerHousehold = hold;
                return hold;
            case 6:
                Household.sServobotHousehold = hold;
                return hold;
            case 7:
                Household.sTouristHousehold = hold;
                return hold;
            case 8:
                PlumbBob.sCurrentNonNullHousehold = hold;
                return hold;
            case 9:
                sTravelHousehold = hold;
                return hold;
            default:
                if (DEBUG_Utils.IsDEBUG)
                    throw new NotSupportedException("r: " + r.ToString());
                else {
                    result = 0;
                    goto case 0;
            }   }
        }




        public static string SaveAllSimDesc()
        {
            Simulator.CheckYieldingContext(true);
            if (Bin.Singleton == null) 
                return null;
            var si = Comman.GetAllSimDescription();
            if (si == null || si.Count == 0)
                return null;

            Household household = Household.Create();
            if (household == null)
            {
                // EA bug
                return null;
            }


            var hMembers = household.mMembers;
            if (hMembers == null)
            {
                // EA bug
                household.Destroy();
                return null;
            }

            household.SetName("SaveAllSimDesc World Name " + World.GetWorldFileName());
            household.mBioText = "Date " + Comman.GetNowTimeToStr();

            var simList = si.ToArray();

            bool shouldSleep = Simulator.CheckYieldingContext(false) && simList.Length > 190;

            int i = 0;

            foreach (var item in simList)
            {
                if (item == null)
                    continue;

                if (item.mHairColors == null || item.mSimDescriptionId == 0 || item.mOutfits == null)
                    continue;

                if (!Comman.SD_OutfitsIsValid(item))
                    continue;

                if (item.IsValidDescription)
                {
                    hMembers.mAllSimDescriptions.Add(item);

                    if (item.IsPet)
                        hMembers.mPetSimDescriptions.Add(item);
                    else
                        hMembers.mSimDescriptions.Add(item);
                }
                if (shouldSleep)
                {
                    i++;
                    if (i > 30)
                    {
                        i = 0;
                        Simulator.Sleep(0);
                    }
                }
            }

            if (hMembers.mAllSimDescriptions.Count == 0)
            {
                household.Destroy();
                return null;
            }

            household.mBioText += "\nCount " + hMembers.mAllSimDescriptions.Count;

            string packageFile = Proxies.HouseholdContentsProxy.VExportHousehold(Bin.Singleton, household, false, false, true, true);

            hMembers.mAllSimDescriptions.Clear();
            hMembers.mPetSimDescriptions.Clear();
            hMembers.mSimDescriptions.Clear();
            
            household.Destroy();

            Comman.PrintMessage("SaveAllSimDesc\n" + packageFile, false, float.MaxValue);

            return packageFile;
        }
        public static void HideUI(bool hide)
        {
            WindowBase window = UIManager.GetUITopWindow();
            if (window != null)
            {
                WindowBase[] windows = new WindowBase[] {
					window.GetChildByID(57857283u, true),
					window.GetChildByID(57857293u, true),
					window.GetChildByID(57857282u, true),
					window.GetChildByID(57857293u, true),
					window.GetChildByID(57857292u, true),
					window.GetChildByID(57857296u, true),
					window.GetChildByID(57857291u, true),
					window.GetChildByID(57857295u, true)
				};

                foreach (WindowBase windowBase in windows)
                {
                    if (windowBase == null)
                        continue;

                    windowBase.Visible = !hide;
                }

                if (hide)
                {
                    if (Sims3.UI.Hud.BorderTreatmentsController.sInstance != null)
                    {
                        Sims3.UI.Hud.BorderTreatmentsController.sInstance.Visible = false;
                    }
                }
                else
                {
                    if (Sims3.UI.Hud.BorderTreatmentsController.sInstance != null)
                    {
                        Sims3.UI.Hud.BorderTreatmentsController.sInstance.Visible = true;
                    }
                }
            }
        }
        public static void FixService()
        {
            if (!GameStates.IsInWorld()) 
                return;

            if (Services.sAllServices == null)
                Services.sAllServices = new List<Service>();

            int found = 0;
            try
            {
                if (Services.sInstance == null)
                {
                    Services.sInstance = new Services();
                }

                if (ResortWorkerBar.Instance == null)
                {
                    found++;
                    new ResortWorkerBar();
                }
                else if (!Services.sAllServices.Contains(ResortWorkerBar.Instance))
                    Services.Add(ResortWorkerBar.Instance);

                if (SocialWorkerPetAdoption.Instance == null)
                {
                    found++;
                    new SocialWorkerPetAdoption();
                }
                else if (!Services.sAllServices.Contains(SocialWorkerPetAdoption.Instance))
                    Services.Add(SocialWorkerPetAdoption.Instance);

                if (DJ.Instance == null)
                {
                    found++;
                    new DJ();
                }
                else if (!Services.sAllServices.Contains(DJ.Instance))
                    Services.Add(DJ.Instance);

                if (Dancer.Instance == null)
                {
                    found++;
                    new Dancer();
                }
                else if (!Services.sAllServices.Contains(Dancer.Instance))
                    Services.Add(Dancer.Instance);

                if (ScienceGeek.Instance == null)
                {
                    found++;
                    new ScienceGeek();
                }
                else if (!Services.sAllServices.Contains(ScienceGeek.Instance))
                    Services.Add(ScienceGeek.Instance);

                if (PerformanceArtist.Instance == null)
                {
                    found++;
                    new PerformanceArtist();
                }
                else if (!Services.sAllServices.Contains(PerformanceArtist.Instance))
                    Services.Add(PerformanceArtist.Instance);

                if (SocialWorkerPetPutUp.Instance == null)
                {
                    found++;
                    new SocialWorkerPetPutUp();
                }
                else if (!Services.sAllServices.Contains(SocialWorkerPetPutUp.Instance))
                    Services.Add(SocialWorkerPetPutUp.Instance);

                if (GrimReaper.Instance == null)
                {
                    found++;
                    new GrimReaper();
                }
                else if (!Services.sAllServices.Contains(GrimReaper.Instance))
                    Services.Add(GrimReaper.Instance);

                if (TimeTraveler.Instance == null)
                {
                    found++;
                    new TimeTraveler();
                }
                else if (!Services.sAllServices.Contains(TimeTraveler.Instance))
                    Services.Add(TimeTraveler.Instance);

                if (Repoman.Instance == null)
                {
                    found++;
                    new Repoman();
                }
                else if (!Services.sAllServices.Contains(Repoman.Instance))
                    Services.Add(Repoman.Instance);

                if (Repairman.Instance == null)
                {
                    found++;
                    new Repairman();
                }
                else if (!Services.sAllServices.Contains(Repairman.Instance))
                    Services.Add(Repairman.Instance);

                if (Police.Instance == null)
                {
                    found++;
                    new Police();
                }
                else if (!Services.sAllServices.Contains(Police.Instance))
                    Services.Add(Police.Instance);

                if (Maid.Instance == null)
                {
                    found++;
                    new Maid();
                }
                else if (!Services.sAllServices.Contains(Maid.Instance))
                    Services.Add(Maid.Instance);

                if (Magician.Instance == null)
                {
                    found++;
                    new Magician();
                }
                else if (!Services.sAllServices.Contains(Magician.Instance))
                    Services.Add(Magician.Instance);

                if (Firefighter.Instance == null)
                {
                    found++;
                    new Firefighter();
                }
                else if (!Services.sAllServices.Contains(Firefighter.Instance))
                    Services.Add(Firefighter.Instance);

                if (FakeMetaAutonomy.Instance == null)
                {
                    found++;
                    new FakeMetaAutonomy();
                }
                else if (!Services.sAllServices.Contains(FakeMetaAutonomy.Instance))
                    Services.Add(FakeMetaAutonomy.Instance);

                if (Butler.Instance == null)
                {
                    found++;
                    new Butler();
                }
                else if (!Services.sAllServices.Contains(Butler.Instance))
                    Services.Add(Butler.Instance);

                if (Burglar.Instance == null)
                {
                    found++;
                    new Burglar();
                }
                else if (!Services.sAllServices.Contains(Burglar.Instance))
                    Services.Add(Burglar.Instance);

                if (Babysitter.Instance == null)
                {
                    found++;
                    new Babysitter();
                }
                else if (!Services.sAllServices.Contains(Babysitter.Instance))
                    Services.Add(Babysitter.Instance);

                if (BartenderService.Instance == null)
                {
                    found++;
                    new BartenderService();
                }
                else if (!Services.sAllServices.Contains(BartenderService.Instance))
                    Services.Add(BartenderService.Instance);

                if (SocialWorkerAdoption.Instance == null)
                {
                    found++;
                    new SocialWorkerAdoption();
                }
                else if (!Services.sAllServices.Contains(SocialWorkerAdoption.Instance))
                    Services.Add(SocialWorkerAdoption.Instance);

                if (SocialWorkerChildAbuse.Instance == null)
                {
                    found++;
                    new SocialWorkerChildAbuse();
                }
                else if (!Services.sAllServices.Contains(SocialWorkerChildAbuse.Instance))
                    Services.Add(SocialWorkerChildAbuse.Instance);

                if (MailCarrier.Instance == null)
                {
                    found++;
                    new MailCarrier();
                }
                else if (!Services.sAllServices.Contains(MailCarrier.Instance))
                    Services.Add(MailCarrier.Instance);

                if (NewspaperDelivery.Instance == null)
                {
                    found++;
                    new NewspaperDelivery();
                }
                else if (!Services.sAllServices.Contains(NewspaperDelivery.Instance))
                    Services.Add(NewspaperDelivery.Instance);

                if (ResortWorker.Instance == null)
                {
                    found++;
                    new ResortWorker();
                }
                else if (!Services.sAllServices.Contains(ResortWorker.Instance))
                    Services.Add(ResortWorker.Instance);

                if (ResortMaintenanceLow.Instance == null)
                {
                    found++;
                    new ResortMaintenanceLow();
                }
                else if (!Services.sAllServices.Contains(ResortMaintenanceLow.Instance))
                    Services.Add(ResortMaintenanceLow.Instance);

                if (ResortMaintenanceMedium.Instance == null)
                {
                    found++;
                    new ResortMaintenanceMedium();
                }
                else if (!Services.sAllServices.Contains(ResortMaintenanceMedium.Instance))
                    Services.Add(ResortMaintenanceMedium.Instance);

                if (ResortMaintenanceHigh.Instance == null)
                {
                    found++;
                    new ResortMaintenanceHigh();
                }
                else if (!Services.sAllServices.Contains(ResortMaintenanceHigh.Instance))
                    Services.Add(ResortMaintenanceHigh.Instance);

                if (UniversityMascot.Instance == null)
                {
                    found++;
                    new UniversityMascot();
                }
                else if (!Services.sAllServices.Contains(UniversityMascot.Instance))
                    Services.Add(UniversityMascot.Instance);

                if (PizzaDelivery.Instance == null)
                {
                    found++;
                    new PizzaDelivery();
                }
                else if (!Services.sAllServices.Contains(PizzaDelivery.Instance))
                    Services.Add(PizzaDelivery.Instance);

                if (Singer.Instance == null)
                {
                    found++;
                    new Singer();
                }
                else if (!Services.sAllServices.Contains(Singer.Instance))
                    Services.Add(Singer.Instance);

                foreach (var item in Services.sAllServices.ToArray())
                {
                    if (item == null)
                    {
                        Services.sAllServices.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            { Comman.PrintException("FixService()", ex); }
            finally
            {
                if (found != 0)
                {
                    Comman.PrintMessage("Instance == null is Found Fix: " + found, false, 100);
                }
            }
        }
        public static void ForceAllMoveIn (params Household[] BlockHousehold)
        {
            List<Lot> lieastERS = new List<Lot>();
            Lot lotERS = null;
            List<Household> newlistERS = new List<Household>();
            try
            {
                foreach (var item in Comman.SCO_GetObjects<Household>())
                {
                    if (item == null) continue;
                    if (!newlistERS.Contains(item))
                    {
                        newlistERS.Add(item);
                    }
                }
            }
            catch (Exception)
            { }

            try
            {
                foreach (var item in Household.sHouseholdList.ToArray())
                {
                    if (item == null) continue;
                    if (!newlistERS.Contains(item))
                    {
                        newlistERS.Add(item);
                    }
                }
            }
            catch (Exception)
            { }
            foreach (Household mhouselist in newlistERS)
            {
                if (mhouselist == null) continue;

                bool found = false;
                if (BlockHousehold != null)
                {
                    foreach (var item in BlockHousehold)
                    {
                        if (item == mhouselist)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        continue;
                }
                if (mhouselist.mMembers == null || mhouselist.AllSimDescriptions == null || mhouselist.AllSimDescriptions.Count == 0) continue;
                try
                {
                    if (mhouselist == Household.sNpcHousehold || mhouselist == Household.sPetHousehold || mhouselist == Household.sMermaidHousehold || mhouselist == Household.sTouristHousehold || mhouselist == Household.sPreviousTravelerHousehold || mhouselist == Household.sAlienHousehold || mhouselist == Household.sServobotHousehold) continue;
                }
                catch
                { }







                try
                {
                    if (mhouselist.LotHome != null) continue;
                }
                catch
                { }


                lieastERS.Clear();

                foreach (object obj in LotManager.AllLots)
                {
                    Lot lot2 = (Lot)obj;
                    if (!lot2.IsWorldLot && !lot2.IsCommunityLotOfType(CommercialLotSubType.kEP10_Diving) && !UnchartedIslandMarker.IsUnchartedIsland(lot2) && lot2.IsResidentialLot && lot2.Household == null && !World.LotIsEmpty(lot2.LotId) && !lot2.IsLotEmptyFromObjects())
                    {
                        lieastERS.Add(lot2);
                    }
                    if (lieastERS.Count == 0)
                    {
                        if (!lot2.IsWorldLot && !lot2.IsCommunityLotOfType(CommercialLotSubType.kEP10_Diving) && !UnchartedIslandMarker.IsUnchartedIsland(lot2) && lot2.IsResidentialLot && lot2.Household == null)
                        {
                            lieastERS.Add(lot2);
                        }
                    }
                }

                if (lieastERS.Count > 0)
                {
                    lotERS = RandomUtil.GetRandomObjectFromList<Lot>(lieastERS);
                    try
                    {
                        lotERS.MoveIn(mhouselist);
                    }
                    catch (Exception)
                    {
                        mhouselist.mLotHome = lotERS;
                        mhouselist.mLotId = lotERS.mLotId;
                        lotERS.mHousehold = mhouselist;
                    }
                }

                else return;


            }
        }
        public static void RemoveAllFireFromLot()
        {
            var lotm = LotManager.sLots;
            if (lotm == null) return;
            if (Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("Camera Target Lot"))
            {
               // LotLocation LotLocation = default(LotLocation);
               // ulong Location = World.GetLotLocation(CameraController.GetTarget(), ref LotLocation);
               // Lot lot = LotManager.GetLot(Location);
                var lot = Comman.GetCameraTargetLotOrTargetLot();
                if (lot != null)
                {
                    lot.RemoveAllFires();
                }
            }
            else
            {
                foreach (Lot item in lotm.Values)
                {
                    if (item == null)
                        continue;
                    try
                    {
                        item.RemoveAllFires();
                    }
                    catch
                    { }

                }
            }
        }
        public static void Motives_ForceSetMax(Motives ths, CommodityKind commodity)
        {
            Motive motive = ths.GetMotive(commodity);
            if (motive != null && motive.Value != motive.Tuning.Max)
            {
                motive.UpdateMotiveBuffs(ths.mSim, commodity, (int)motive.Tuning.Max);
                motive.mValue = motive.Tuning.Max;
            }
        }
        public static void Sim_MaxMood(Sim sim)
        {
            if (sim != null && ScriptCore.Objects.Objects_IsValid(sim.ObjectId.mValue))
            {
                var autonomy = sim.Autonomy;// ?? (sim.mAutonomy = new Autonomy(sim);
                if (autonomy == null)
                    return;

                bool fixUp = false;
                var vMotives = autonomy.Motives;

                if (vMotives == null)
                {
                    fixUp = true;
                }
                else
                {
                    foreach (KeyValuePair<int, Motive> mMotive in vMotives.mMotives)
                    {
                        if (mMotive.Value == null || mMotive.Value.Tuning == null)
                        {
                            fixUp = true;
                            break;
                        }
                    }
                }

                if (fixUp)
                {
                    autonomy.RecreateAllMotives();
                }

                vMotives = autonomy.Motives;

                if (vMotives == null)
                    return;

                foreach (CommodityKind commodity in Sim_MaxMood_sCommodities)
                {
                    Motives_ForceSetMax(vMotives, commodity);
                }
            }
        }
        public static void TestSetActiveActor(Sim Target)
        {
            if (!GameStates.IsInWorld())
                return;

            if (PlumbBob.sSingleton == null)
            {
                DEBUG_Utils.Print("TestSetActiveActor(): PlumbBob.sSingleton == null");
                return;
            }

            PlumbBob.sCurrentNonNullHousehold = null;
            PlumbBob.sSingleton.mSelectedActor = null;

            for (int i = 0; i < 5; i++)
            {
                PlumbBob.ForceSelectActor(null);
            }

            PlumbBob.sCurrentNonNullHousehold = null;
            PlumbBob.sSingleton.mSelectedActor = null;

            PlumbBob.ForceSelectActor(Target);

            if (Target != null)
            {
                if (Target.Household == null)
                {
                    DEBUG_Utils.Print
                       ("TestSetActiveActor(): PlumbBob.sCurrentNonNullHousehold == null");
                }
                if (PlumbBob.sSingleton.mSelectedActor == null)
                    DEBUG_Utils.Print("TestSetActiveActor(): PlumbBob.sSingleton.mSelectedActor == null");
            }
            else
            {
                if (PlumbBob.sCurrentNonNullHousehold == null)
                    DEBUG_Utils.Print("TestSetActiveActor(): PlumbBob.sCurrentNonNullHousehold == null");
                if (PlumbBob.sSingleton.mSelectedActor == null)
                    DEBUG_Utils.Print("TestSetActiveActor(): PlumbBob.sSingleton.mSelectedActor == null");
            }
        }
        public static void UnSafeForceErrorTargetSim(Sim obj_Sim)
        {
            Autonomy autonmy = obj_Sim.Autonomy;
            if (autonmy != null)
            {
                var aSituationComponent = autonmy.SituationComponent;

                if (aSituationComponent != null)
                    aSituationComponent.mSituations = new List<Situation>();

                autonmy.mSituationComponent = null;
                autonmy.mMotives = null;
            }

            obj_Sim.mBuffManager = null;
            obj_Sim.mMoodManager = null;
            obj_Sim.mInteractionQueue = null;
            obj_Sim.mLookAtManager = null;
            obj_Sim.mIdleManager = null;
            obj_Sim.mThoughtBalloonManager = null;
            obj_Sim.mSocialComponent = null;
            obj_Sim.mSnubManager = null;
            obj_Sim.mMapTagManager = null;
            obj_Sim.mDreamsAndPromisesManager = null;
            obj_Sim.mDreamsAndPromisesManager = null;
            obj_Sim.mDeepSnowEffectManager = null;
            obj_Sim.mFlags = 0;
            obj_Sim.SleepDreamManager = null;

            ScriptCore.Simulator.Simulator_DestroyObjectImpl(obj_Sim.mSimUpdateId.mValue);
        }
        public static string ForceSaveGame_SaveLocalizeString(string name, params object[] parameters)
        {
            return Localization.LocalizeString("Ui/Caption/Options:" + name, parameters);
        }
        public static bool ForceSaveGame_SaveFileNameExists(string fileName)
        {
            foreach (string item in new WorldFileSearch(1u))
            {
                string text2 = item;
                if (text2.ToLower().EndsWith(".sims3"))
                {
                    text2 = text2.Substring(0, text2.Length - 6);
                }
                if (text2.ToLower() == fileName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        public static bool ForceSaveGame(string name, bool forceSaveAs, bool ovDeleteSave)
        {

            if (!Simulator.CheckYieldingContext(false) || GameStates.IsInMainMenuState)
                return false;

            if (ScriptCore.World.World_IsEditInGameFromWBModeImpl())
            {
                UIManager.DarkenBackground(true);
                bool a = LoadSaveManager.SaveWorld(); // Custom
                UIManager.DarkenBackground(false);
                return a;
            }

            string worldFileName = World.GetWorldFileName(); // custom
            if (worldFileName != null)
            {
                worldFileName = worldFileName.Replace(".world", ".sims3");
                if (!GameUtils.IsValidFilename(worldFileName ?? ""))
                {
                    Comman.PrintMessage("Current World Name is invalid", false,100);
                    return false;
                }
            }
            else
            {
                Comman.PrintMessage("Current World Name is invalid", false, 100);
                return false;
            }

            if (string.IsNullOrEmpty(name))
                return false;

            string titleText = Localization.LocalizeString("Ui/Caption/GameEntry/MainMenu:NewTownTitle");
            string promptText = Localization.LocalizeString("Ui/Caption/GameEntry/MainMenu:NewTownPrompt");


            string text = name;

            string temptext = name;

            if (!temptext.EndsWith(".sims3"))
            {
                temptext += ".sims3";
            }

            if (ForceSaveGame_SaveFileNameExists(temptext) && ovDeleteSave)
            {
                GameUtils.DeleteSaveFile(temptext);
            }
            if (!ovDeleteSave)
            {
                if (text == null || text == "" || forceSaveAs)
                {
                    if (!forceSaveAs)
                    {
                        if (GameStates.WorldFileMetadata != null)
                        {
                            text = GameStates.WorldFileMetadata.mCaption;
                        }
                        else if (GameStates.SaveGameMetadata != null)
                        {
                            WorldFileMetadata info = new WorldFileMetadata();
                            info.mWorldFile = GameStates.SaveGameMetadata.mWorldFile;
                            if (Responder.Instance.MainMenuModel.GetWorldFileDetails(ref info))
                            {
                                text = info.mCaption;
                            }
                        }
                        else
                        {
                            text = "";
                        }
                        if (text.ToLower().EndsWith(".world"))
                        {
                            text = text.Substring(0, text.Length - 6);
                        }
                    }
                    else if (text.ToLower().EndsWith(".sims3"))
                    {
                        text = text.Substring(0, text.Length - 6);
                    }

                    bool forceShowDialog = true;
                    //if (CommandLine.FindSwitch("ForceShowSaveDialog") != null)
                    //{
                    //    forceShowDialog = true;
                    //}
                    while (Simulator.CheckYieldingContext(false))
                    {
                        text = StringInputDialog.Show(titleText, promptText, text, -1, ThumbnailKey.kInvalidThumbnailKey, new Vector2(-1f, -1f), StringInputDialog.Validation.SaveGameName, false, ModalDialog.PauseMode.PauseSimulator, forceShowDialog, false);
                        if (text.ToLower().EndsWith(".sims3"))
                        {
                            text = text.Substring(0, text.Length - 6);
                        }
                        if (!ForceSaveGame_SaveFileNameExists(text))
                        {
                            break;
                        }
                        string promptText2 = Localization.LocalizeString("Ui/Caption/Options:SaveConfirm");
                        if (AcceptCancelDialog.Show(promptText2, forceShowDialog))
                        {
                            GameUtils.DeleteSaveFile(text);
                            break;
                        }
                        Simulator.Sleep(0);
                    }
                }
            }
            if (text == null || text.Length <= 0)
            {
                return false;
            }
            if (!text.EndsWith(".sims3"))
            {
                text += ".sims3";
            }
            else if (!text.EndsWith(".sims3", StringComparison.OrdinalIgnoreCase))
            {
                text += ".sims3";
            }





            string householdName = null;
            string householdBio = null;
            string homeworldName = null;
            ulong householdId = 0;
            ulong lotId = 0;
            Simulator.Sleep(0);


            Sims3.SimIFace.Gameflow.GameSpeed currentGameSpeed = Responder.Instance.ClockSpeedModel.CurrentGameSpeed;
            bool gameSpeedLocked = Responder.Instance.ClockSpeedModel.GameSpeedLocked;

            try
            {
                if (!gameSpeedLocked)
                {
                    Responder.Instance.ClockSpeedModel.LockGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Pause);
                }
            }
            catch (Exception ex)
            {
                Comman.PrintException(null, ex);
            }







            try
            {
                try
                {
                    ScreenCaptureOverlay.Display(true, Simulator.CheckYieldingContext(false));
                }
                catch (Exception ex)
                {
                    Comman.PrintException(null, ex);
                }

                ProgressDialog.Show(ForceSaveGame_SaveLocalizeString("Saving"), false);

                try
                {
                    if (PlumbBob.SelectedActor != null)
                    {
                        Household household = PlumbBob.SelectedActor.Household;
                        if (household != null)
                        {
                            householdName = household.Name;
                            householdBio = household.BioText;
                            householdId = household.HouseholdId;
                            homeworldName = GameStates.HomeworldMetadataName;
                            lotId = household.LotId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Comman.PrintException(null, ex);
                }
                Simulator.Sleep(0);
                using (LoadSaveFileInfo loadSaveFileInfo = LoadSaveManager.GetCurrentSaveGameInfo())
                {
                    string text2 = text;
                    if (text2 != null && text2.Length > 0)
                    {
                        LoadSaveManager.eLoadSaveErrorCode eLoadSaveErrorCode = LoadSaveManager.eLoadSaveErrorCode.kNoError;
                        string text3 = null;
                        using (LoadSaveFileInfo loadSaveFileInfo2 = LoadSaveManager.CreateLoadSaveFileInfo(text2))
                        {
                            if (loadSaveFileInfo2 == null)
                            {
                                return false;
                            }
                            Sims3.Gameplay.UI.OptionsModel tOptionsModel = null;
                            var tInstanceResponder = Sims3.Gameplay.UI.Responder.Instance;
                            if (tInstanceResponder != null)
                            {
                                tOptionsModel = tInstanceResponder.OptionsModel as Sims3.Gameplay.UI.OptionsModel;
                                if (tOptionsModel != null)
                                    tOptionsModel.mSaveInProgress = true;
                            }

                            eLoadSaveErrorCode = (LoadSaveManager.eLoadSaveErrorCode)LoadSaveManager.SaveGame(loadSaveFileInfo2, (GameStates.HasTravelData || GameStates.MovingWorldName != null) ? true : false);

                            if (tOptionsModel != null)
                                tOptionsModel.mSaveInProgress = false;
                            switch (eLoadSaveErrorCode)
                            {
                                case LoadSaveManager.eLoadSaveErrorCode.kNoError:
                                    break;
                                case LoadSaveManager.eLoadSaveErrorCode.kInvalidSaveFileLength:
                                case LoadSaveManager.eLoadSaveErrorCode.kFilePathLengthTooLarge:
                                    SimpleMessageDialog.Show(ForceSaveGame_SaveLocalizeString("SaveGameDialog"), ForceSaveGame_SaveLocalizeString("SaveGameFailTooLong"), ModalDialog.PauseMode.PauseSimulator);
                                    return false;
                                case LoadSaveManager.eLoadSaveErrorCode.kFilenameIsInvalid:
                                    SimpleMessageDialog.Show(ForceSaveGame_SaveLocalizeString("SaveGameDialog"), ForceSaveGame_SaveLocalizeString("SaveGameFailInvalid"), ModalDialog.PauseMode.PauseSimulator);
                                    return false;
                                default:
                                    SimpleMessageDialog.Show(ForceSaveGame_SaveLocalizeString("SaveGameDialog"), ForceSaveGame_SaveLocalizeString("SaveGameFailErrorCode", (int)eLoadSaveErrorCode), ModalDialog.PauseMode.PauseSimulator);
                                    return false;
                            }
                            text3 = loadSaveFileInfo2.ID;
                            //text3.ToLower();

                            if (!text3.EndsWith(".sims3"))
                            {
                                text3 += ".sims3";
                            }

                            GameStates.LoadFileName = text3;
                        }
                        if (eLoadSaveErrorCode == LoadSaveManager.eLoadSaveErrorCode.kNoError)
                        {
                            UIManager.SetSaveGameMetadata(text3, householdName, householdBio, homeworldName, householdId, lotId, false);
                        }
                    }
                }
                return true;
            }
            finally
            {
                try
                {
                    if (!gameSpeedLocked)
                    {
                        Responder.Instance.ClockSpeedModel.UnlockGameSpeed();
                    }
                    if (currentGameSpeed != 0)
                    {
                        Responder.Instance.ClockSpeedModel.CurrentGameSpeed = currentGameSpeed;
                    }
                }
                catch (Exception ex)
                {
                    Comman.PrintException(null, ex);
                }


                try
                {
                    ProgressDialog.Close();
                }
                catch
                { }


                try
                {
                    ScreenCaptureOverlay.Display(false, Simulator.CheckYieldingContext(false));
                }
                catch (Exception ex)
                {
                    Comman.PrintException(null, ex);
                    Comman.CreateThread(ScriptExecuteType.Task,delegate
                    {
                        ScreenCaptureOverlay.Display(false, Simulator.CheckYieldingContext(false));
                    });

                }
            }
        }
        #endregion //Metheds

        #region Commands

        public static
            bool newhouseholdfromah_command()
        {
            var ah = Household.ActiveHousehold;
            if (ah == null) 
                return false;

            var aa = PlumbBob.SelectedActor;
            if (aa == null)
                return false;

            if (aa != PlumbBob.sSingleton.mSelectedActor) // Player Two by Core Mod
                return false;

            var aasimd = aa.SimDescription;
            if (aasimd == null)
                return false;

            if (aasimd.Household != null) // Player Two by Core Mod
            {
                var aaa = PlumbBob.sSingleton.mSelectedActor;
                if (aaa != null)
                {
                    var aaasimd = aaa.SimDescription;
                    if (aaasimd != null && aaasimd.Household != aasimd.Household)
                        return false;
                }
            }

            var lot = Comman.GetCameraTargetLotOrTargetLot();
            if (lot == null || lot.Household != null || lot.IsWorldLot)
                return false;

            if (!forcesetaa3_command(null, true))
                return false;

            Comman.Household_Remove(aasimd, false);

            var newhh = Household.Create(true);
            if (newhh == null) // EA Bug or Mono Bug
            {
                Comman.Household_Add(ah, aasimd, false);
                forcesetaa3_command(aa, true);
                return false;
            }

            Comman.Household_Add(newhh, aasimd, false);

            newhh.mName = aasimd.mLastName;
            newhh.mFamilyFunds = 20000;

            try
            {
                lot.MoveIn(newhh);
            }
            catch (Exception)
            {
                newhh.mLotHome = lot;
                newhh.mLotId = lot.mLotId;
                lot.mHousehold = newhh;
            }

            TestSetActiveActor(aa);
            return true;
        }


        public static
            bool newhouseholdfromts_command()
        {
            var sim = Comman.GetTargetGameObject() as Sim;
            if (sim == null)
                return false;

            var simsimd = sim.SimDescription;
            if (simsimd == null)
                return false;

            if (sim == PlumbBob.SelectedActor)
            {
                return newhouseholdfromah_command();
            }
            else
            {
                var plu = PlumbBob.sSingleton;
                if (plu != null && plu.mSelectedActor == sim)
                    return newhouseholdfromah_command();
            }

  
            var lot = Comman.GetCameraTargetLotOrTargetLot();
            if (lot == null || lot.Household != null || lot.IsWorldLot)
                return false;

            var simhh = simsimd.Household;
            Comman.Household_Remove(simsimd, false);

            var newhh = Household.Create(true);
            if (newhh == null) // EA Bug or Mono Bug
            {
                Comman.Household_Add(simhh, simsimd, false);
                return false;
            }

            Comman.Household_Add(newhh, simsimd, false);

            newhh.mName = simsimd.mLastName;
            newhh.mFamilyFunds = 20000;

            try
            {
                lot.MoveIn(newhh);
            }
            catch (Exception)
            {
                newhh.mLotHome = lot;
                newhh.mLotId = lot.mLotId;
                lot.mHousehold = newhh;
            }

            if (simhh == Household.ActiveHousehold)
                Comman.Household_RefrashAllActors(Household.ActiveHousehold);

            return true;
        }



        public static
            void addsimtohousehold_command()
        {

            var addsimtohousehold_data = Household.ActiveHousehold;
            if (addsimtohousehold_data == null)
                return;
            if (!Simulator.CheckYieldingContext(false))
                return;

            Comman.Household_RemoveNullForHouseholdMembers(addsimtohousehold_data);

            var sim = Comman.GetTargetGameObject() as Sim;
            if (sim == null)
                return;

            var simd = sim.mSimDescription;
            if (simd == null)
                return;

            if (simd.Household == addsimtohousehold_data)
                return;
           
            Comman.Household_Add(addsimtohousehold_data, simd, false);

            simd.IsNeverSelectable = false;

            try
            {
                if (simd.Household == Household.ActiveHousehold && simd.CreatedSim.mDreamsAndPromisesManager == null)
                    simd.CreatedSim.OnBecameSelectable();
            }
            catch (Exception)
            { }

            Comman.Household_RefrashAllActors(addsimtohousehold_data);
        }
        public static
            void addsimtohousehold2_command()
        {
            if (!Simulator.CheckYieldingContext(false))
                return;
            if (!GameStates.IsInWorld())
                return;

            var addsimtohousehold_data = Comman.GetTargetSimHousehold();
            if (addsimtohousehold_data == null)
            {
                Comman.PrintMessage("Please Target Household", false, 5);
                while (true)
                {
                    addsimtohousehold_data = Comman.GetTargetSimHousehold();
                    if (addsimtohousehold_data != null)
                        break;
                    Simulator.Sleep(0);
                }
            }

            var simHousehold = Comman.GetTargetGameObject() as Sim;
            Comman.Household_RemoveNullForHouseholdMembers(addsimtohousehold_data);

            Sim sim = null;
            SimDescription simd = null;

            while (sim == null || sim == simHousehold)
            {
                simd = null;
                sim = Comman.GetTargetGameObject() as Sim;
                if (sim != null)
                {
                    simd = sim.mSimDescription;
                    if (simd == null || simd.mHousehold == addsimtohousehold_data)
                    {
                        sim = null;
                    }
                }
                Simulator.Sleep(0);
            }

            if (simd == null)
            {
                return;
            }
            if (simd.Household == addsimtohousehold_data)
            {
                return;
            }

           
            Comman.Household_Add(addsimtohousehold_data, simd, false);

            simd.IsNeverSelectable = false;

            try
            {
                if (addsimtohousehold_data == Household.ActiveHousehold && simd.CreatedSim.mDreamsAndPromisesManager == null)
                    simd.CreatedSim.OnBecameSelectable();
            }
            catch (Exception)
            { }

            if (addsimtohousehold_data == Household.ActiveHousehold)
                Comman.Household_RefrashAllActors(addsimtohousehold_data);
        }

        public static
            void alllotclean_command()
        {
            var Lots = LotManager.sLots;
            if (Lots == null)
            {
                return;
            }

            Simulator.CheckYieldingContext(true);
            bool ForceCleanBill = AcceptCancelDialog.Show("Force Clean Bill?");

            var ActiveLotHome = ForceCleanBill ? null : Sims3.Gameplay.CAS.Household.ActiveHouseholdLot;
            var CopyLots = new Dictionary<ulong, Lot>(Lots);

            try
            {
                foreach (var itemLot in CopyLots.Values)
                {
                    if (itemLot == null || itemLot.LotId == 0 || itemLot.LotId == ulong.MaxValue)
                        continue;

                    try
                    {
                        Comman.Lot_RepairAllForLot(itemLot);
                        Comman.Lot_CleanUpAllForLot(itemLot, ForceCleanBill || ActiveLotHome != itemLot);
                    }
                    catch (StackOverflowException)
                    {
                        throw;
                    }
                    catch (ResetException)
                    {
                        throw;
                    }
                    catch (Exception e) { Comman.PrintException("alllotclean_command(): LotID: " + itemLot.LotId, e); }
                }
            }
            finally
            {
                CopyLots.Clear();
            }
        }

        public static
            void aasetp_command()
        {
            if (Sim.ActiveActor != null && Sim.ActiveActor.SimDescription != null && !Sim.ActiveActor.HasBeenDestroyed)
            {
                //var rt = World.SnapToFloor(CameraController.GetTarget());
                Vector3 campos;
                var lot = Comman.GetCameraTargetLotOrTargetLot();
                if (lot != null && !lot.IsWorldLot && lot.IsHouseboatLot())
                    campos = CameraController.GetLODInterestPosition();
                else
                    campos = World.SnapToFloor(CameraController.GetTarget());

                if (!Comman.Vector3_IsUnSafe(campos))
                {
                    Sim.ActiveActor.SetPosition(campos);
                    Sim.ActiveActor.FadeIn();
                }
                else DEBUG_Utils.Print("Camera Position Is UnSafe Found");
            }
        }

        public static
            void savealllot_command() { LibraryUtls.AsyncSaveAllLot(); }
        public static
           void saveallhhandlot_command() { LibraryUtls.AsyncSaveAllHouseholdAndLot(); }
        public static
           void saveallhh_command() { LibraryUtls.AsyncSaveAllHousehold(); }

        public static
            void exhousehold_command()
        {
            Simulator.CheckYieldingContext(true);
            var household = Comman.FindHouse(true); //Sims3.Gameplay.CAS.Household.ActiveHousehold;
            if (household != null && household.CurrentMembers != null && Bin.Singleton != null)
            {
                if ((string.IsNullOrEmpty(household.mBioText) 
                    || ScriptCore.LocalizedStringService.LocalizedStringService_GetLocalizedStringByString(household.mBioText ?? "") != "") 

                    && household.mMembers.AllSimDescriptionList.Count == 0)
                {
                    Comman.PrintMessage("The household members is empty.", false, 400);
                    if (household.IsActive)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            PlumbBob.ForceSelectActor(null);
                        }
                        if (GameStates.IsLiveState)
                            GameStates.TransitionToPlayFlow();
                    }
                    if (Comman.Household_IsSpecialHousehold(household))
                    {
                        var lot = household.mLotHome;
                        if (lot != null)
                        {
                            lot.mHousehold = null;
                        }
                        household.mLotId = 0;
                        household.mLotHome = null;
                    }
                    else
                    {
                        household.Destroy();
                    }
                    return;
                }
                string packageFile = null;
                packageFile = Proxies.HouseholdContentsProxy.VExportHousehold
                    (Bin.Singleton, household, household.LotHome != null && AcceptCancelDialog.Show("Save Lot?"), false, false, false); 

                Comman.PrintMessage(packageFile, false, 2000);
                if (!string.IsNullOrEmpty(packageFile))
                {
                    Sims3.Gameplay.BinModel.Singleton.AddToExportBin(packageFile);
                }
            }
        }

        public static
            void savelot_command()
        {
            if (Sims3.Gameplay.GameStates.IsInMainMenuState) 
                return;
            Simulator.CheckYieldingContext(true);
            Lot TargetLot = Comman.GetCameraTargetLotOrTargetLot();
            if (TargetLot != null && !TargetLot.IsWorldLot)
            {
                string result = null;
                ulong lotContentsID = Sims3.SimIFace.CustomContent.DownloadContent.StoreLotContents(TargetLot, TargetLot.LotId);
                if (lotContentsID != 0)
                {
                    ThumbnailHelper.GenerateLotThumbnailSet(TargetLot.LotId, lotContentsID, ThumbnailSizeMask.ExtraLarge);
                    ThumbnailHelper.GenerateLotThumbnail(TargetLot.LotId, lotContentsID, 0x0, ThumbnailSizeMask.Medium | ThumbnailSizeMask.Large);
                    result = Sims3.SimIFace.CustomContent.DownloadContent.ExportLotContentsToExportBin(lotContentsID);
                    ThumbnailManager.InvalidateLotThumbnails(TargetLot.LotId, lotContentsID, ThumbnailSizeMask.ExtraLarge);
                    ThumbnailManager.InvalidateLotThumbnailsForGroup(TargetLot.LotId, lotContentsID, ThumbnailSizeMask.Medium, 0x0);
                }
                Comman.PrintMessage(result, true, 1000);
                if (!string.IsNullOrEmpty(result))
                {
                    Sims3.Gameplay.BinModel.Singleton.AddToExportBin(result);
                }
            }
        }

        public static
            void excashousehold_command()
        {
            if (Bin.sSingleton == null)
                return;
            var casCore = CASLogic.sSingleton;
            if (casCore == null)
                return;
            if (casCore.mHouseholdScriptHandle.mValue != 0)
            {
                var household = Simulator.GetProxy(casCore.mHouseholdScriptHandle) as Household;
                if (household != null)
                {
                    if (household.mMembers != null && household.mMembers.mAllSimDescriptions != null && household.mMembers.mAllSimDescriptions.Count != 0)
                    {
                        Simulator.CheckYieldingContext(true);
                        if (household.mName == null || household.mName == "")
                        {
                            household.SetName("CASHousehold " + Comman.GetNowTimeToStr());
                        }
                        if (household.mBioText == null || household.mBioText == "")
                        {
                            household.mBioText = "";
                        }
                        try
                        {
                            casCore.SetHouseholdStartingFunds();
                        }
                        catch (Exception)
                        { }
                        if (household.mFamilyFunds == 0)
                        {
                            household.mFamilyFunds = 50000;
                        }
                        var ty = household.mMembers.mAllSimDescriptions;
                        foreach (var item in ty.ToArray())
                        {
                            if (item == null)
                            {
                                ty.Remove(null);
                                continue;
                            }
                            if (item.mFirstName == null || item.mFirstName == "")
                            {
                                item.mFirstName = "No name";
                            }
                            if (item.mLastName == null || item.mLastName == "")
                            {
                                item.mLastName = "No name";
                            }
                            if (item.mBio == null)
                            {
                                item.mBio = "";
                            }
                        }

                        string packageFile = null;
                        //packageFile = Bin.Singleton.StoreCASHouseholdToExportBin (household);
                        packageFile = Proxies.HouseholdContentsProxy.VExportHousehold
                            (Bin.Singleton, household, false, false, true, false);
                        Comman.PrintMessage (packageFile, false, 4000);

                    }
                }
            }
        }

        public static
            void forcesavegame_command()
        {
            Simulator.CheckYieldingContext(true);
            bool isInMainMenuState = false;
            try
            {
                if (GameStates.IsInMainMenuState)
                {
                    isInMainMenuState = true;
                }
            }
            catch (Exception ex)
            {
                Comman.PrintException(null, ex);
                if (!Simulator.CheckYieldingContext(false) || AcceptCancelDialog.Show(_thisAssembly._name + "\nFound Error Safe?", true))
                {
                    return;
                }
            }

            if (!isInMainMenuState)
            {

                if (ScriptCore.World.World_IsEditInGameFromWBModeImpl())
                    ForceSaveGame(null, true, true);
                else
                    ForceSaveGame("Save ID " + new Random().Next(5, 10000000), true, true);

            }
        }

        public static
            void debuginfo_command(bool call2)
        {
            debuginfo_data = !debuginfo_data;
            if (debuginfo_data)
            {
                ScriptCore.GameUtils.GameUtils_ToggleDebugInfo(false);
                ScriptCore.GameUtils.GameUtils_ToggleDebugInfo(true);
                if (call2)
                    ScriptCore.GameUtils.GameUtils_ToggleDebugInfo(true);
            }
            else ScriptCore.GameUtils.GameUtils_ToggleDebugInfo(false);
        }

        public static
            void bulot_command()
        {
            Simulator.CheckYieldingContext(true);
            var TargetLot = Comman.GetCameraTargetLotOrTargetLot();
            if (TargetLot != null && !TargetLot.IsWorldLot)
            {
                Sim ActiveActor = PlumbBob.SelectedActor;
                if (ActiveActor != null && ActiveActor.LotCurrent == TargetLot)
                {
                    ActiveActor.SetPosition(Sims3.Gameplay.Services.Service.GetPositionInRandomLot(LotManager.GetWorldLot()));
                    Simulator.Sleep(0);
                }
                foreach (var itemC in Comman.SCO_GetObjectsOnLot<GameObject>(TargetLot)) //TargetLot.GetObjects<Sims3.Gameplay.Abstracts.GameObject>())
                {
                    if (itemC == null || itemC == ActiveActor)
                        continue;
                    try
                    {
                        // itemC.Dispose(); show error:
                        // System.NullReferenceException
                        // #0: 0x00007 callvirt   in Sims3.Gameplay.Controllers.Sims3.Gameplay.Controllers.FireManager:UnregisterFire (Sims3.Gameplay.Objects.Fire) (4BB3D080 [49FBA620] )
                        // #1: 0x00010 callvirt   in Sims3.Gameplay.Objects.Sims3.Gameplay.Objects.Fire:Dispose () ()
                        // #2: 0x00008 callvirt   in ScriptCore.ScriptCore.ScriptProxy:Dispose () ()
                        itemC.Destroy();
                        // auto Dispose() Calling ScriptProxy :)
                    }
                    catch (Exception)
                    { Simulator.DestroyObject(itemC.ObjectId); }
                }
                
                Comman.Lot_Bulldoze(TargetLot, false, true, true, false);
            }
        }

        public static
            void allpossimtocamera_command()
        {
            Simulator.CheckYieldingContext(true);
            Sim[] sim = Comman.SCO_GetObjects<Sim>();
            foreach (var item in sim)
            {
                if (item == null) continue;
                item.mPosture = item.Standing;
                try
                {
                    item.SetObjectToReset();
                    Simulator.Sleep(0);
                }
                catch (StackOverflowException) { throw; }
                catch (ResetException) { throw; }
                catch (Exception e) { Comman.DPrintException(null, e); }

                if (item.SimDescription != null && !item.HasBeenDestroyed)
                {
                    Vector3 campos;
                    var lot = Comman.GetCameraTargetLotOrTargetLot();
                    if (lot != null && !lot.IsWorldLot && lot.IsHouseboatLot())
                        campos = CameraController.GetTarget();
                    else
                        campos = World.SnapToFloor(CameraController.GetTarget());

                    if (!Comman.Vector3_IsUnSafe(campos))
                        item.SetPosition(campos);
                }
            }
        }

        public static
            void dtargetobj_command()
        {
            Simulator.CheckYieldingContext(true);
            var obj = Comman.GetTargetGameObject();
            if (obj != null)
            {
                if (obj == PlumbBob.SelectedActor || obj == PlumbBob.sSingleton)
                    SimpleMessageDialog.Show(_thisAssembly._name, "Destroy Active Actor is not allowed.");
                else
                {
                    Comman.GO_ForceDeAttachAndDestroyAllSlots(obj, true);
                    Comman.GO_ForceDestroy(obj);
                }
            }
            else
            {
                var objectID = Comman.GetTargetObjectDontHaveScript();
                if (objectID != 0)
                {
                    var gameObjectID = Simulator.GetProxy(new ObjectGuid(objectID)) as GameObject;
                    if (gameObjectID != null && (gameObjectID == PlumbBob.SelectedActor || gameObjectID == PlumbBob.sSingleton))
                    {
                        SimpleMessageDialog.Show(_thisAssembly._name, "Destroy Active Actor is not allowed.");
                    }
                    else if (AcceptCancelDialog.Show("Destroy Object?\n(ObjectID: " + objectID + ")"))
                    {
                        Comman.GO_ForceDeAttachAndDestroyAllSlots(gameObjectID, false);
                        Comman.GO_ForceDeAttachAndDestroyAllSlots(gameObjectID, true);
                        
                        Simulator.DestroyObject(new ObjectGuid(objectID));
                    }
                }
            }
        }

        public static
            void forcesetaa_command()
        {
            Sim targetSim = Comman.GetTargetGameObject() as Sim;
            if (targetSim != null)
            {
                PlumbBob.ForceSelectActor(targetSim);
            }
            else if (Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("Are you sure?\nPlumbBob.ForceSelectActor(null)"))
            {
                PlumbBob.ForceSelectActor(null);
            }
        }

        public static
            void forcesetaa2_command(bool noDialog)
        {
            Sim targetSim = Comman.GetTargetGameObject() as Sim;
            if (

                noDialog || targetSim != null
                            ||
                PlumbBob.sSingleton == null ||
                PlumbBob.sSingleton.mSelectedActor == null ||
                (Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("Are you sure?\nTestSetActiveActor(null);"))

                )

            { TestSetActiveActor(targetSim); }
            else
            { }
        }

        public static
            bool forcesetaa3_command(Sim sim, bool noDialog)
        {
            if (PlumbBob.sSingleton == null)
                return false;
            if (sim == null && (!noDialog && PlumbBob.sSingleton.mSelectedActor != null &&
                Simulator.CheckYieldingContext(false) &&
                !AcceptCancelDialog.Show("Are you sure?\nPlumbBob.sSingleton.mSelectedActor = null;")))
            {
                return false;
            }
            PlumbBob.sSingleton.mSelectedActor = sim;
            PlumbBob.sCurrentNonNullHousehold = sim != null ? sim.Household : null;
            return true;
        }

        public static
            void testerrortrap_command()
        {
            var obj = Comman.GetTargetGameObject() ?? Comman.GetTargetLot();
            if (obj != null)
            {
                ScriptCore.ScriptProxy sci = obj.Proxy as ScriptCore.ScriptProxy;
                if (sci == null)
                {
                    Comman.PrintMessage("simObj.Proxy as ScriptCore.ScriptProxy failed!", false, 100);
                    return;
                }

                MethodInfo mebinfo = typeof(ScriptCore.ScriptProxy).GetMethod("OnScriptError");
                try
                {
                    if (!(bool)mebinfo.Invoke(sci, new object[] { new ResetException() }))
                        SimpleMessageDialog.Show(_thisAssembly._name, "Failed to OnScriptError!");
                }
                catch (StackOverflowException) { throw; }
                catch (ResetException) { throw; }
                catch (Exception ex)
                {
                    Comman.DPrintException("testerrortrap_command()", ex);
                    SimpleMessageDialog.Show(_thisAssembly._name, "Failed to OnScriptError!\nMessage: " + ex.Message);
                }
            }
        }

        public static
            void loadnpchousehold_command()
        {
            if (!Simulator.CheckYieldingContext(false))
                return;
            if (World.IsEditInGameFromWBMode() || Sims3.SimIFace.Environment.HasEditInGameModeSwitch)
                FixService();
           
            

            var hoc = Proxies.HouseholdContentsProxy.Import(StringInputDialog.Show(_thisAssembly._name, "Package File Name", "NPCHousehold.package", 256, StringInputDialog.Validation.None) ?? "None.package");
            if (hoc != null)
            {
                Household mhouse = hoc.Household;
                if (mhouse != null)
                {

                    string namehoc = mhouse.mName;
                    try
                    {
                        foreach (SimDescription siteem in mhouse.AllSimDescriptions)
                        {
                            if (siteem == null) continue;
                            try
                            {
                                if (siteem.Service != null)
                                    siteem.Service.EndService(siteem);
                                if (siteem.CreatedByService != null)
                                    siteem.CreatedByService.EndService(siteem);
                            }
                            catch (Exception ex)
                            { Comman.PrintException(null, ex); }

                            siteem.Service = null;
                            siteem.CreatedByService = null;

                            try
                            {
                                siteem.RemoveOutfits(Sims3.SimIFace.CAS.OutfitCategories.Career, true);
                            }
                            catch (Exception ex)
                            { Comman.PrintException(null, ex); }

                        }
                        mhouse.mName = "NpcHousehold";
                    }
                    catch (Exception ex)
                    { Comman.PrintException(null, ex); }
                    try
                    {
                        if (Household.sNpcHousehold != null && Household.sNpcHousehold.AllSimDescriptions.Count == 0)
                        {
                            Household.sNpcHousehold.Destroy();
                            Household.sNpcHousehold.Dispose();
                            Household.sNpcHousehold = null;
                        }
                    }
                    catch (Exception ex)
                    { Comman.PrintException(null, ex); }


                    if (Household.sNpcHousehold != null && Household.sNpcHousehold.LotHome == null)
                    {

                        try
                        {
                            //foreach (long objt in LotManager.AllLots) { }//objt.CheckKillSimNotUrnstonePro(); }
                            List<Lot> lieast = new List<Lot>();
                            Lot lot = null;
                            foreach (var obj in LotManager.AllLots)
                            {
                                Lot lot2 = (Lot)obj;
                                if (!lot2.IsWorldLot && !lot2.IsCommunityLotOfType(CommercialLotSubType.kEP10_Diving) && !UnchartedIslandMarker.IsUnchartedIsland(lot2) && lot2.IsResidentialLot && lot2.Household == null)
                                {
                                    lieast.Add(lot2);
                                }
                            }
                            try
                            {
                                Lot virtualLotHome = Household.sNpcHousehold.VirtualLotHome;
                                if (virtualLotHome != null)
                                {
                                    virtualLotHome.VirtualMoveOut(Household.sNpcHousehold);
                                }
                            }
                            catch (Exception ex)
                            { Comman.PrintException(null, ex); }

                            lot = RandomUtil.GetRandomObjectFromList<Lot>(lieast);
                            lot.MoveIn(Household.sNpcHousehold);

                        }
                        catch (Exception ex)
                        { Comman.PrintException(null, ex); }
                    }

                    Household.sNpcHousehold = mhouse;
                    List<SimDescription> nofoundList = null;
                    Household nofoundList_householdNew = null;
                    foreach (var item in Household.sNpcHousehold.AllSimDescriptions.ToArray())
                    {
                        if (item == null) continue;
                        if (item.mBio == null) continue;

                        string bio = item.mBio;
                        string tempa = "";

                        bool found = false;
                        //bool simdispose = false;

                        if (bio.Contains("SServiceType:"))
                        {
                            found = true;
                            tempa = bio.Replace("SServiceType:", "");
                        }
                        else if (bio.Contains("OServiceType:"))
                        {
                            found = true;
                            tempa = bio.Replace("OServiceType:", "");
                        }
                        // else if TODO: your mod custom save npc household :)

                        if (found)
                        {
                            //bool sdone = false;
                            foreach (var serv in Services.sAllServices.ToArray())
                            {
                                if (serv == null) continue;
                                if (serv.GetType() != null && serv.GetType().ToString() == tempa || serv.ServiceType.ToString() == tempa)
                                {
                                    item.Service = serv;
                                    item.CreatedByService = serv;
                                    if (!serv.mPool.Contains(item))
                                        serv.AddSimToPool(item);
                                    //sdone = true;
                                    break;
                                }

                            }
                        }
                        else { 
                            Comman.PrintMessage("Name: " + item.mFirstName + "\nServiceType Bio: " + item.mBio, false, 100);
                            if (nofoundList_householdNew == null)
                                nofoundList_householdNew = Household.Create();
                            if (nofoundList == null)
                            {
                                nofoundList = new List<SimDescription>(Household.sNpcHousehold.AllSimDescriptions.Count + 1);
                            }
                            nofoundList.Add(item);
                        }
                    }
                    if (nofoundList_householdNew != null)
                    {
                        nofoundList_householdNew.SetName("No Bio " + namehoc);
                        foreach (var item in nofoundList.ToArray())
                        {
                            try
                            {
                                nofoundList_householdNew.AddTemporary(item);
                            }
                            catch (Exception)
                            {}
                        }
                        LibraryUtls.AutoMoveInLotFromHousehold(nofoundList_householdNew);
                    }
                    Comman.PrintMessage("Done Household ID: " + mhouse.HouseholdId, false, 100);
                }
                else
                {
                    Comman.PrintMessage("HouseholdContentsProxy.Household is NULL!", false, 100);
                    return;
                }
            }
            else
            {
                Comman.PrintMessage("Could not find Package File.", false, 100);
                return;
            }
        }

        public static
            void savenpchousehold_command()
        {
            Household npc = Household.sNpcHousehold;
            if ( npc == null || npc.mMembers == null || npc.AllSimDescriptions == null || npc.AllSimDescriptions.Count == 0 )//|| (npc.AllSimDescriptions.Count == 1 && (npc.AllSimDescriptions[0].CreatedByService is GrimReaper || npc.AllSimDescriptions[0].Service is GrimReaper)))
                Comman.PrintMessage("Can't Save NpcHousehold is Invalid.", false, 100);
            npc.Name = "NPCHousehold";
            try
            {
                npc.Name += " " + Comman.GetNowTimeToStr();
            }
            catch (Exception ex)
            { Comman.DPrintException("\nsavenpchousehold\n", ex); }

            try
            {
                foreach (var item in npc.AllSimDescriptions.ToArray()) //new List<SimDescription>( Household.sNpcHousehold.AllSimDescriptions))
                {
                    try
                    {
                        // EA fail Crash Game!  Saving NPC Household. Sim Name is null
                        if (item.mBio == null)
                            item.mBio = "";

                        if (item.mFirstName == null)
                            item.mFirstName = "";

                        if (item.mLastName == null)
                            item.mLastName = "";

                        if (item.Service != null && item.Service.GetType() != null && (item.Service.IsSimDescriptionInPool(item)))
                            item.mBio = "SServiceType:" + item.Service.GetType().ToString();
                        else if (item.CreatedByService != null && item.CreatedByService.GetType() != null && (item.CreatedByService.IsSimDescriptionInPool(item)))
                            item.mBio = "SServiceType:" + item.CreatedByService.GetType().ToString();
                    }
                    catch (Exception ex)
                    { Comman.DPrintException("\nsavenpchousehold\nName: " + item.mFirstName, ex); }

                }
            }
            catch (Exception ex)
            { Comman.DPrintException("\nsavenpchousehold\n", ex); }


            string createdPackageFile = Proxies.HouseholdContentsProxy.VExportHousehold(Bin.Singleton, npc, false, false, true, true);

            Comman.PrintMessage(createdPackageFile, false, 100);
            LogUtils.WriteLog(createdPackageFile, false, false);

            if (createdPackageFile != null)
                BinModel.Singleton.AddToExportBin(createdPackageFile);

        }

        public static
            void installdc_command()
        {
            Simulator.CheckYieldingContext(true);
            string sims3PackFile = StringInputDialog.Show(
                  "Sims3Pack File",
                  "",
                  "",
                  256,
                  StringInputDialog.Validation.None
            );

            if (sims3PackFile == null || sims3PackFile.Length == 0) 
                return;

            uint errorCode;

            try 
            {
                bool isInited;

                var sciprtCoreCC = ContentManager.sContentManager as ScriptCore.DownloadContent;
                if (sciprtCoreCC != null)
                {
                    if (sciprtCoreCC.mImportsFolder == null)
                    {
                        isInited = true;
                        ScriptCore.DownloadContent.DownloadContent_InitImpl();
                        sciprtCoreCC.mImportsFolder = ScriptCore.DownloadContent.DownloadContent_GetImportsFolderImpl();
                    }
                    else 
                        isInited = false;
                }
                else 
                    isInited = false;

                errorCode = ScriptCore.DownloadContent.DownloadContent_InstallContentImpl(sims3PackFile);
                if (isInited)
                {
                    try
                    {
                        if (sciprtCoreCC != null)
                            sciprtCoreCC.mImportsFolder = null;
                        ScriptCore.DownloadContent.DownloadContent_ShutdownImpl();
                    }
                    catch // Need Call Mono.Runtime.mono_runtime_install_handlers();
                    {}
                }
            }
            catch // Need Call Mono.Runtime.mono_runtime_install_handlers();
            { errorCode = 20; }
            

            Comman.PrintMessage("installdc ErrorCode: " + errorCode, false, float.MaxValue);
        }

        public static
            void allbulot_command()
        {
            Simulator.CheckYieldingContext(true);
            var lotM = LotManager.sLots;
            if (lotM == null) return;
            var activeHousehold = Household.ActiveHousehold;
            if (activeHousehold != null && activeHousehold.mMembers != null && activeHousehold.mMembers.mAllSimDescriptions != null)
            {
                var pos = World.SnapToFloor(CameraController.GetTarget());
                if (Comman.GetCameraTargetLot() != LotManager.GetWorldLot())
                {
                    pos = Service.GetPositionInRandomLot(LotManager.GetWorldLot());
                }
                if (!Comman.Vector3_IsUnSafe(pos))
                {
                    foreach (var item in activeHousehold.AllActors)
                    {
                        item.SetPosition(pos);
                        item.FadeIn();
                    }
                } 
                else return;
                Simulator.Sleep(0);
            }
            foreach (Lot item in lotM.Values)
            {
                if (item == null)
                    continue;

                Comman.Lot_Bulldoze(item, false, true, true, false);
            }
        }

        public static
            void hideui_command()
        {
            if (!GameStates.IsLiveState)
                return;
            bHideGameUI__ = !bHideGameUI__;
            HideUI(bHideGameUI__);
        }

        public static
            void ustsim_command()
        {
            var objSim = Comman.GetTargetGameObject() as Sim;
            if (objSim != null)
            {
                UnSafeForceErrorTargetSim(objSim);
            }
        }

        public static
            void maxmood_command()
        {
            foreach (var item in Comman.SCO_GetObjects<Sim>())
            {
                if (item == null)
                    continue;

                var sd = item.SimDescription;
                if (sd == null || sd.Household == null)
                    continue;

                if (sd.IsValidDescription && sd.IsValid)
                    Sim_MaxMood(item);
            }
        }

        public static
            void sdnoage_command()
        {
            int myAge = -264;
            foreach (var itemH in Comman.SCO_GetObjects<Household>()) //Household.sHouseholdList.ToArray())
            {
                if (itemH == null || itemH.mMembers == null || itemH.CurrentMembers.mAllSimDescriptions == null) continue;
                foreach (var item in itemH.AllSimDescriptions.ToArray())
                {
                    if (item == null)
                        continue;

                    if (!item.IsValidDescription) 
                        continue;

                    if (string.IsNullOrEmpty(item.mFirstName) && string.IsNullOrEmpty(item.mLastName))
                        continue;

                    if (item.AgingState != null)
                    {
                        int simDesc_Age;
                        int simDesc_maxAge;

                        Comman.SD_GetAging(item, out simDesc_Age, out simDesc_maxAge);

                        simDesc_Age = myAge;
                        if (item.Age != CASAgeGenderFlags.Elder && myAge > simDesc_maxAge)
                        {
                            simDesc_Age = simDesc_maxAge;
                        }

                        Comman.SD_SetAging(item, simDesc_Age);
                    }
                }
            }
        }

        public static
            void settextpos_command()
        {
            if (!Simulator.CheckYieldingContext(false)) throw new NotSupportedException("No Yield!!");

            //Sim sim = Comman.GetTargetSim() ?? PlumbBob.SelectedActor;
            //if (sim != null)
            {

                string textpos = StringInputDialog.Show("veitc" + " Text Pos", "X: Y: Z:", CameraController.GetTarget().ToString(), 256, StringInputDialog.Validation.None);
                if (string.IsNullOrEmpty(textpos))
                    return;

                textpos = textpos.Trim();

                textpos = textpos.Replace("(", "").Replace(")", "");

                if (string.IsNullOrEmpty(textpos))
                    return;

                float x, y, z;

                string[] pos = textpos.Split(new string[] { ", ", "," }, StringSplitOptions.None);

                if (pos.Length == 3)
                {

                    float.TryParse(pos[0], out x);
                    float.TryParse(pos[1], out y);
                    float.TryParse(pos[2], out z);




                    Sim sim = null;

                    sim = PlumbBob.SelectedActor;

                    if (sim != null && AcceptCancelDialog.Show("Active Actor"))
                    {
                        sim.SetPosition(new Vector3(x, y, z));

                        if (sim.SimRoutingComponent != null)
                            sim.SimRoutingComponent.ForceUpdateDynamicFootprint();

                        return;
                    }

                    sim = null;

                    SimpleMessageDialog.Show(_thisAssembly._name, "Please Hit Target Sim :)");


                    for (int i = 0; i < 550; i++)
                    {
                        Simulator.Sleep(0);



                        if (sim != null)
                        {
                            sim.SetPosition(new Vector3(x, y, z));

                            if (sim.SimRoutingComponent != null)
                                sim.SimRoutingComponent.ForceUpdateDynamicFootprint();

                            return;
                        }
                        sim = Comman.GetTargetGameObject() as Sim;
                    }

                    if (sim == null)
                        sim = PlumbBob.SelectedActor;

                    if (sim != null)
                    {
                        sim.SetPosition(new Vector3(x, y, z));

                        if (sim.SimRoutingComponent != null)
                            sim.SimRoutingComponent.ForceUpdateDynamicFootprint();
                    }
                }
                else Comman.PrintMessage("Text Invalid", false, 100);
            }
        }

        public static
            void fixsimbad_command(Sim[] simList)
        {
            var simAC = PlumbBob.SelectedActor;
            SimDescription simd = null;
            foreach (var item in simList ?? Comman.SCO_GetObjects<Sim>())
            {

                if (item == null)
                    continue;
                if (item.SimDescription == null || item.Autonomy == null || item.HasBeenDestroyed)
                {
                    if (item.mSimDescription == null)
                    {
                        if (simd == null)
                            simd = new SimDescription();
                        item.mSimDescription = simd;
                    } 
                    if (simAC == item)
                    {
                        TestSetActiveActor(null);
                    }
                    Comman.GO_ForceDestroy(item);
                    
                }
            }
        }

        public static
            void fixahsims_command()
        {
            Household ActiveHousehold = Household.ActiveHousehold;
            if (ActiveHousehold != null)
            {
                foreach (var item in ActiveHousehold.AllActors)
                {
                    if (item == null) continue;
                    if (item.IsSelectable)
                    {
                        item.OnBecameSelectable();
                    }
                }
            }
        }

        public static
            void importlot_command()
        {
            if (!Simulator.CheckYieldingContext(false))
                throw new NotSupportedException("No Yield!!");
            Lot targetLot = Comman.GetCameraTargetLotOrTargetLot();
            if (targetLot != null && !targetLot.IsWorldLot && (World.LotIsEmpty(targetLot.LotId) && targetLot.IsLotEmptyFromObjects() || AcceptCancelDialog.Show("UnSafe")))
            {
                LotRotationAngle lotAngle = LotRotationAngle.kLotRotateAuto;
                if (typeof(LotRotationAngle) != null)
                {
                    LotRotationAngle result = (LotRotationAngle)Comman.GetIntDialog
                       ("LotRotate Code\nkLotRotateNone = 0\nkLotRotate90 = 1\nkLotRotate180 = 2\nkLotRotate270 = 3\nkLotRotateAuto = 4");
                    if (!Enum.IsDefined(typeof(LotRotationAngle), result)) { }
                    else
                    {
                        lotAngle = result;
                    }
                }

                string packageFile = StringInputDialog.Show (
                    "Package File",
                    "",
                    Comman.GetLastPackageName(true) ?? "",
                    256,
                    StringInputDialog.Validation.None
                );

                if (packageFile == null || packageFile == "") // cancel
                    return; 

                LotPosition lotPosition = LotPosition.kLotPositionCenter;
                LibraryUtls.ResultDEBUG re = LibraryUtls.ImportLot (
                    targetLot,
                    packageFile,
                    ref lotAngle,
                    ref lotPosition
                );

                if (re != LibraryUtls.ResultDEBUG.Success)
                {
                    SimpleMessageDialog.Show(_thisAssembly._name, "Failed to Import Lot!\nResult: " + re);
                }
            }
        }

        public static
            void spt_command(object[] parameters)
        {
            int speed = (int)parameters[1];
            if (speed < 0 || speed > 4) return;
            if (speed != 0 && ScriptCore.GameUtils.GameUtils_IsPausedImpl() && Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("UnPaused off?"))
            {
                ScriptCore.GameUtils.GameUtils_UnpauseImpl();
                Responder.Instance.ClockSpeedModel.UnlockGameSpeed();
            }
            Sims3.Gameplay.Gameflow.SetGameSpeed((Sims3.SimIFace.Gameflow.GameSpeed)speed, false);
        }

        public static
            void scpt_command()
        {
            if (!Simulator.CheckYieldingContext(false)) throw new NotSupportedException("No Yield!!");
            // _thisAssembly._name + ":
            string textpos = StringInputDialog.Show("Text Vector3", "P(X: Y: Z:)//T(X: Y: Z:)", ScriptCore.CameraController.Camera_GetPosition().ToString() + "//" + ScriptCore.CameraController.Camera_GetTarget().ToString(), 256, StringInputDialog.Validation.None);
            if (string.IsNullOrEmpty(textpos))
                return;
            string[] pt = textpos.Split(new string[] { "//" }, StringSplitOptions.None);
            if (pt.Length != 2) return;
            Vector3[] art = new Vector3[2];
            for (int i = 0; i < pt.Length; i++)
            {
                string bpos = pt[i].Trim();

                bpos = bpos.Replace("(", "").Replace(")", "");

                if (string.IsNullOrEmpty(bpos))
                    return;

                float x, y, z;

                string[] pos = bpos.Split(new string[] { ", ", "," }, StringSplitOptions.None);

                if (pos.Length == 3)
                {
                    float.TryParse(pos[0], out x);
                    float.TryParse(pos[1], out y);
                    float.TryParse(pos[2], out z);
                    if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z))
                    {
                        SimpleMessageDialog.Show(_thisAssembly._name, "Text NAN Is not allowed!");
                        return;
                    }
                    art[i] = new Vector3(x, y, z);
                }
                else
                {
                    SimpleMessageDialog.Show(_thisAssembly._name, "Text Invalid");
                    return;
                }
            }
            Vector3 p1 = art[0];
            Vector3 p2 = art[1];
            if (!(Comman.Vector3_Is_NAN_Or_Zero(p1) || Comman.Vector3_Is_NAN_Or_Zero(p2) || Comman.Vector3_IsUnSafe(p1) || Comman.Vector3_IsUnSafe(p2)))
                ScriptCore.CameraController.Camera_SetPositionAndTarget(p1, p2);
            else
            {
                SimpleMessageDialog.Show(_thisAssembly._name, "Text is Illegal");
                return;
            }
        }

        public static
            void targetct_command()
        {
            GameObject obj = Comman.GetTargetGameObject();
            if (obj != null) 
            {
                string st = DEBUG_Utils.GetObjectStackTrace(obj.ObjectId.mValue);
                if (st != "<no call stack>")
                {
                    ScriptCore.TaskContext context;
                    
                    if (!ScriptCore.TaskControl.GetTaskContext(obj.ObjectId.mValue, true, out context) || context.mFrames == null)
                        return;

                    StringBuilder stringBuilder = new StringBuilder();

                    for (int stack = context.mFrames.Length - 1; stack >= 0; stack--)
                    {
                        ScriptCore.TaskFrame taskFrame = context.mFrames[stack];


                        if (taskFrame.mMethodHandle.Value == IntPtr.Zero)
                            continue;

                        MethodBase methodInfo = MethodBase.GetMethodFromHandle(taskFrame.mMethodHandle);
                        Type declaringType = methodInfo.DeclaringType;
                        stringBuilder.Append(declaringType.Name);
                        stringBuilder.Append("::");
                        stringBuilder.Append(methodInfo.Name);
                        stringBuilder.Append('\n');
                    }

                    Comman.PrintMessage("Call Stack\nSleep: " + context.mSleepTicks + "\n" + stringBuilder.ToString(), false, 20);
                    LogUtils.WriteLog(st, false, false);
                }
            }
        }

        public static
            void autop_command()
        {
            if (autop_objectid == ObjectGuid.InvalidObjectGuid)
            {
                if (GameStates.IsInMainMenuState || !GameStates.IsLiveState) 
                    return;

                autop_objectid = Comman.CreateThread(ScriptExecuteType.Task, delegate
                {
                    while (true)
                    {
                        Simulator.Sleep(0);
                        if (GameStates.IsInMainMenuState) { return; }
                        Responder.Instance.ClockSpeedModel.UnlockGameSpeed();
                        if (ScriptCore.World.World_IsEditInGameFromWBModeImpl())
                        {
                            ProgressDialog.Close();
                            Sims3.UI.GameEntry.ScreenCaptureOverlay.Display(false, Simulator.CheckYieldingContext(false));
                        }
                        try
                        {
                            //ModalDialog.EnableModalDialogs = false;
                            InWorldSubState inworld = GameStates.GetInWorldSubState();
                            if (inworld is BuildModeState || inworld is BuyModeState || inworld is BlueprintModeState)
                            {
                                Responder.Instance.ClockSpeedModel.UnlockGameSpeed();
                                GameUtils.Unpause();
                            }
                        }
                        catch (ResetException) { throw; }
                        catch (Exception e)
                        {
                            Comman.DPrintException(null, e);
                            for (int isleep = 0; isleep < 150; isleep++)
                            {
                                Simulator.Sleep(0);
                            }
                        }
                    }
                });
            }
            else
            {
                //ModalDialog.EnableModalDialogs = true;
                Comman.RemoveTaskFromSimulator(ref autop_objectid);
            }
        }

        public static
            void rnewpetpool_command()
        {
            PetPoolManager.sPetPoolManager = null;
            PetPoolManager.sPetConfigManager = null;

            PetAdoption.sNeighborAdoption = null;

            if (GameUtils.IsInstalled(ProductVersion.EP5))
            {
                Lazy.Allocate(ref PetPoolManager.sPetPoolManager);
                Lazy.Allocate(ref PetPoolManager.sPetConfigManager);

                try
                {
                    PetAdoption.CreatePetInNeighbor();
                }
                catch (Exception)
                {}
            }

        }
        public static
           void removepetpool_command()
        {
            PetPoolManager.sPetPoolManager = null;
            PetPoolManager.sPetConfigManager = null;

            PetAdoption.sNeighborAdoption = null;

        }
        public static
            void fixallsimdesc2_command()
        {
            var slist = Comman.GetAllSimDescription();
            if (slist == null || slist.Count == 0) 
                return;
            foreach (var item in slist)
            {
                if (item == null || item.mHousehold == null)
                    continue;
                try
                {
                    item.mIsValidDescription = false;

                    if (item.SkillManager == null)
                        item.SkillManager = new Sims3.Gameplay.Skills.SkillManager(item);

                    item.SkillManager.mSimDescription = item;

                    if (item.TraitManager == null)
                        item.TraitManager = new Sims3.Gameplay.ActorSystems.TraitManager();

                    item.TraitManager.mSimDescription = item;

                    if (item.mGenealogy == null)
                        item.mGenealogy = new Sims3.Gameplay.Socializing.Genealogy(item);

                    if (item.mGenealogy.mSim != item)
                    {
                        item.mGenealogy = new Sims3.Gameplay.Socializing.Genealogy(item);
                    }

                    if (item.CareerManager == null)
                        item.CareerManager = new Sims3.Gameplay.Careers.CareerManager(item);

                    item.CareerManager.mSimDescription = item;

                    if (item.VisaManager == null)
                        item.VisaManager = new Sims3.Gameplay.Visa.VisaManager(item);

                    item.VisaManager.mSimDescription = item;

                    if (item.CelebrityManager == null)
                        item.CelebrityManager = new Sims3.Gameplay.CelebritySystem.CelebrityManager(item.mSimDescriptionId);

                    item.CelebrityManager.mSimDescriptionId = item.mSimDescriptionId;

                    if (item.LifeEventManager == null)
                        item.LifeEventManager = new Sims3.Gameplay.ActorSystems.LifeEventManager(item);

                    item.LifeEventManager.mOwnerDescription = item;

                    if (item.ReadBookDataList == null)
                        item.ReadBookDataList = new Dictionary<string, Sims3.Gameplay.Objects.ReadBookData>();

                    item.Fixup();
                }
                catch (Exception)
                { }
            }
        }

        public static
           void fixallsimdesc_command()
        {
            var slist = Comman.GetAllSimDescription();
            if (slist == null || slist.Count == 0)
                return;
            foreach (var item in slist)
            {
                if (item == null || item.mHousehold == null)
                    continue;
                try
                {
                    item.mIsValidDescription = false;

                    if (item.SkillManager == null)
                        item.SkillManager = new Sims3.Gameplay.Skills.SkillManager(item);

                    if (item.TraitManager == null)
                        item.TraitManager = new Sims3.Gameplay.ActorSystems.TraitManager();

                    if (item.mGenealogy == null)
                        item.mGenealogy = new Sims3.Gameplay.Socializing.Genealogy(item);

                    if (item.mGenealogy.mSim != item)
                    {
                        item.mGenealogy = new Sims3.Gameplay.Socializing.Genealogy(item);
                    }

                    if (item.CareerManager == null)
                        item.CareerManager = new Sims3.Gameplay.Careers.CareerManager(item);

                    if (item.VisaManager == null)
                        item.VisaManager = new Sims3.Gameplay.Visa.VisaManager(item);

                    if (item.CelebrityManager == null)
                        item.CelebrityManager = new Sims3.Gameplay.CelebritySystem.CelebrityManager(item.mSimDescriptionId);

                    if (item.LifeEventManager == null)
                        item.LifeEventManager = new Sims3.Gameplay.ActorSystems.LifeEventManager(item);

                    if (item.ReadBookDataList == null)
                        item.ReadBookDataList = new Dictionary<string, Sims3.Gameplay.Objects.ReadBookData>();

                    item.Fixup();
                }
                catch (Exception)
                { }
            }
        }

        public static
            void forcelivemode_command()
        {
            if (!GameStates.IsInWorld()) 
                return;

            if (GameStates.sSingleton != null && GameStates.sSingleton.mInWorldState != null)
                GameStates.sSingleton.mInWorldState.GotoReturnState();
            if (EditTownPuck.sInstance != null && EditTownPuck.sInstance.mChangeSelectedHouseholdButton != null)
                EditTownPuck.sInstance.mChangeSelectedHouseholdButton.Enabled = true;

            // Fix Bug
            if (Household.sNpcHousehold == null)
            {
                Household.sNpcHousehold = Household.Create();
                Household.sNpcHousehold.mName = "NPCHousehold";
            }

            var ied = EditTownPuck.sInstance;
            if (ied != null && !GameStates.IsGameShuttingDown && !GameStates.IsCurrentlySwitchingSubStates && !GameStates.mQuitting)
            {
                ied.HidePanels();
                ied.UpdateBackButton(true);
                GameStates.TransitionToReturnState();
                if (GameStates.NextInWorldStateId != GameStates.ReturnState)
                {
                    ied.UpdateBackButton(false);
                }
            }
        }

        public static
            void houseinvsim_command()
        {
            var lot = Comman.GetCameraTargetLotOrTargetLot();
            if (lot != null && lot.mHousehold != null && !(lot is WorldLot))
            {
                var lotHousehold = lot.mHousehold;
                if (lotHousehold != null)
                {
                    Comman.Household_RemoveNullForHouseholdMembers(lotHousehold);
                    if (lotHousehold.mMembers.mAllSimDescriptions.Count == 0)
                    {
                        Comman.PrintMessage(lotHousehold.Name + " is empty members" , false, float.MaxValue);
                        return;
                    }

                    bool done = false;
                    foreach (var item in lotHousehold.mMembers.mAllSimDescriptions.ToArray())
                    {
                        if (item == null) {
                            continue;
                        }
                        var sim = item.mSim;
                        if (sim != null) {
                            if (!ScriptCore.Objects.Objects_IsValid(sim.ObjectId.mValue) || sim.mSimDescription != item)
                            {
                                item.mSim = null;
                                Comman.GO_ForceDestroy(sim);
                                sim = null;
                            }
                            else continue;
                        }

                        if (item.mSimDescriptionId == 0) {
                            Comman.PrintMessage(item.mFirstName + " is invalid", false, float.MaxValue);
                            continue;
                        }
                        
                        if (item.mHousehold == null)
                        {
                            while (lotHousehold.mMembers.mAllSimDescriptions.Remove(item));
                            while (lotHousehold.mMembers.mPetSimDescriptions.Remove(item));
                            while (lotHousehold.mMembers.mSimDescriptions.Remove(item));

                            item.mHousehold = null;
                            Comman.Household_Remove(item, true);

                            if (!item.IsValidDescription)
                            {
                                try
                                {
                                    item.Fixup();
                                }
                                catch (Exception ex)
                                {
                                    Comman.PrintException("Cannot Fix Sim Desc\nName: " + item.FirstName, ex);
                                    continue;
                                }
                            }
                            if (!item.IsValidDescription)
                            {
                                Comman.PrintMessage("Cannot Fix Sim Desc\nName: " + item.FirstName, false, float.MaxValue);
                                continue;
                            }

                            if (!Comman.Household_Add(lotHousehold, item, true) || item.mHousehold == null)
                            {
                                Comman.PrintMessage("Could not add Sim to Household\nName: " + item.FirstName, false, float.MaxValue);
                                continue;
                            }
                        }

                        if (!item.IsValidDescription)
                        {
                            try
                            {
                                item.Fixup();
                            }
                            catch (Exception ex)
                            {
                                Comman.PrintException("Cannot Fix Sim Desc\nName: " + item.FirstName, ex);
                                continue;
                            }
                        }
                        if (!item.IsValidDescription)
                        {
                            Comman.PrintMessage("Cannot Fix Sim Desc\nName: " + item.FirstName, false, float.MaxValue);
                            continue;
                        }

                        sim = Comman.SD_SafeInstantiate(item, Vector3.OutOfWorld);
                        if (sim == null)
                        {
                            Comman.PrintMessage("Could not create Sim\nName: " + item.FirstName, false, float.MaxValue);
                            continue;
                        }

                        LibraryUtls.SendHomeForSim(sim);
                        done = true;
                    }
                    if (done)
                    {
                        if (GameStates.IsLiveState && PlumbBob.SelectedActor == null && lotHousehold.mMembers.mAllSimDescriptions.Count > 0 && lotHousehold.mMembers.ActorList.Count > 0 && !ScriptCore.World.World_IsEditInGameFromWBModeImpl() && Simulator.CheckYieldingContext(false))
                        {
                            var sim = Comman.HouseholdMembersToSim(lotHousehold, false, false);
                            if (AcceptCancelDialog.Show("Do you set Active Sim?" + (sim == null ? "\nCould not find Household Members!" : "\nName: " + sim.Name)))
                                PlumbBob.ForceSelectActor(sim);
                        }
                        Comman.PrintMessage("houseinvsim_command: Done", false, 50);
                    }
                    else
                    {
                        Comman.PrintMessage("Already have Sim in household members", false, 50);
                    }
                }
            }
        }

        public static
            void moveinfromhouse_command()
        {
            var lot = Comman.GetCameraTargetLotOrTargetLot();
            if (lot != null && !(lot is WorldLot) && lot.Household == null)
            {
                Household household = Comman.FindHouse(false);
                if (household != null)
                {
                    try
                    {
                        //lot.MoveOut(household);
                        Lot lothousehold = household.LotHome;
                        try
                        {

                            if (lothousehold != null)
                                lothousehold.MoveOut(household);
                        }
                        catch
                        {
                            household.mLotHome = null;
                            if (lothousehold != null)
                                lothousehold.mHousehold = null;
                        }



                        lot.MoveIn(household);
                    }
                    catch (Exception ex)
                    {
                        lot.mHousehold = household;
                        household.mLotHome = lot;
                        household.mLotId = lot.LotId;
                        Comman.PrintException(DEBUG_Utils.GetHouseholdInfo(household, false, ""), ex);
                    }
                }
            }
            
        }

        public static
            void moveoutfromhouse_command()
        {
            var lotHome = Comman.GetCameraTargetLotOrTargetLot();
            if (lotHome != null)// && !(lotHome is WorldLot))
            {
                Household household = lotHome.mHousehold;
                if (household != null)
                {
                    if (household != null && (household == Household.ActiveHousehold || (PlumbBob.sSingleton != null && PlumbBob.sSingleton.mSelectedActor != null && household == PlumbBob.sSingleton.mSelectedActor.Household)))
                    {
                        Comman.PrintMessage("MoveOut:\nActive Household is not allowed.", true, 100);
                        return;
                    }
                    Comman.PrintMessage("MoveOut: Done\nHousehold ID: " + household.HouseholdId, true, 100);
                    try
                    {
                        lotHome.MoveOut(household);
                    }
                    catch (Exception ex)
                    {
                        lotHome.mHousehold = null;
                        household.mLotHome = null;
                        household.mLotId = 0;
                        Comman.PrintException(null, ex);
                    }
                }
            }
        }

        public static
            void rallg_command()
        {
            Vector3 vOutOfWorld = Vector3.OutOfWorld;
            int sieeo = 0;
            //bool checkDispose = AcceptCancelDialog.Show("Dispose DeadSimsDescription?");

            foreach (var item in Comman.SCO_GetObjects<Sims3.Gameplay.Objects.Urnstone>())
            {
                sieeo++;
                if (sieeo > 150) { sieeo = 0; Simulator.Sleep(0); }
                if (IsKeepGrave(item))
                    continue;
                var pos = item.Position;
                item.SetPosition(vOutOfWorld);
                var t = item.DeadSimsDescription;

                if (t == null || Comman.SD_ResurrectSim(t, Comman.Household_FindIDSC(item.mOriginalHouseholdId), true, false))
                {
                    item.DeadSimsDescription = null;
                    Comman.GO_ForceDestroy(item);
                }
                else
                {
                    item.SetPosition(pos);
                    item.FadeIn();
                }
            }
        }

        public static
            void sethdp_command()
        {
            int i;
            SetHouseholdFieldStaticSpecial(out i);
        }

        public static
            void importhouse_command()
        {
            if (!Simulator.CheckYieldingContext(false))
                return;

            var lot = Comman.GetCameraTargetLotOrTargetLot();
            if (lot != null && !(lot is WorldLot))
            {
                if (lot.Household == null)
                {
                    HouseholdContents aa;
                    string packageFileName = Comman.GetLastPackageName(false);
                    Household hoc = LibraryUtls._ImportHousehold
                        (Simulator.CheckYieldingContext(false) ? StringInputDialog.Show("Package File", "", packageFileName ?? "Funke.package", 256, StringInputDialog.Validation.None) : "", lot, false, true, out aa);
                    if (hoc != null)
                    {
                        Comman.PrintMessage("Imported Household ID: " + hoc.mHouseholdId, false, float.MaxValue);
                        LibraryUtls.ForceSendHomeAllActors(hoc);
                        var hocCount = hoc.AllSimDescriptions.Count;
                        if (hocCount > 0 && hoc.mMembers.ActorList.Count > 0 && !ScriptCore.World.World_IsEditInGameFromWBModeImpl() && GameStates.IsLiveState && PlumbBob.SelectedActor == null && Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("Force Set Active Actor?"))
                        {
                            PlumbBob.ForceSelectActor(Comman.HouseholdMembersToSim(hoc, false, false));
                        }
                    }

                }
                else
                {
                    if (AcceptCancelDialog.Show("Merge Household?"))
                    {
                        Comman.Household_RemoveNullForHouseholdMembers(lot.Household);
                        string packageFileName = Comman.GetLastPackageName(false);
                        HouseholdContents contents;

                        Household householdImported = LibraryUtls._ImportHousehold(
                            Simulator.CheckYieldingContext(false) ?
                                StringInputDialog.Show("Package File", "", packageFileName ?? "Funke.package", 256, StringInputDialog.Validation.None) 
                                :
                                ""
                            ,
                            (lot.Household == null) ? lot : null,
                            false,
                            false,
                            out contents
                        );

                        if (householdImported != null)
                        {
                            var lotHousehold = lot.Household;
                            if (lotHousehold == null)
                            {
                                lot.MoveIn(householdImported);
                                Comman.PrintMessage("lotHousehold == null", false, 500f);
                                return;
                            }

                            var memberHoc = householdImported.mMembers;
                            if (memberHoc == null)
                            {
                                Comman.PrintMessage("Error: memberHoc == null", false, 500f);
                                return;
                            }

                            if (memberHoc.mAllSimDescriptions == null)
                            {
                                Comman.PrintMessage("Error: memberHoc.mAllSimDescriptions == null", false, 500f);
                                return;
                            }

                            var householdSimMembers = memberHoc.mAllSimDescriptions.ToArray();
                            foreach (SimDescription simd in householdSimMembers)
                            {
                                if (simd != null)
                                {
                                    Comman.Household_Remove(simd, true);
                                    simd.mHousehold = null;
                                }
                            }

                            memberHoc.mAllSimDescriptions.Clear();
                            memberHoc.mPetSimDescriptions.Clear();
                            memberHoc.mSimDescriptions.Clear();

                            lotHousehold.mFamilyFunds += householdImported.mFamilyFunds;

                            householdImported.Destroy();
                            //householdImported.Dispose(); auto dispose :)
                            householdImported = null;

                            foreach (SimDescription simDesc in householdSimMembers)
                            {
                                if (Comman.Household_Add(lotHousehold, simDesc, false))
                                {
                                    LibraryUtls.SendHomeForSim(Comman.SD_SafeInstantiate(simDesc, Vector3.OutOfWorld));
                                }
                            }

                            for (int i = 0; i < 15; i++)
                            {
                                Simulator.Sleep(0);
                            }

                            foreach (SimDescription simDesc in householdSimMembers)
                            {
                                if (simDesc.mSim != null)
                                {
                                    try
                                    {
                                        simDesc.Fixup();
                                    }
                                    catch (ResetException)
                                    { throw; }
                                    catch { }
                                    simDesc.SendSimHome();
                                }
                            }

                            if (GameStates.IsLiveState && PlumbBob.SelectedActor == null && lotHousehold.mMembers.mAllSimDescriptions.Count > 0 && lotHousehold.mMembers.ActorList.Count > 0 && !ScriptCore.World.World_IsEditInGameFromWBModeImpl() && Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("Force Set Active Actor?"))
                            {
                                PlumbBob.ForceSelectActor(Comman.HouseholdMembersToSim(lotHousehold, false, false));
                            }
                        }
                    }
                    else Comman.PrintMessage("Lot Household Name: " + lot.Household.Name + "\nPlease use move out command:\n'veitc moveoutfromhouse'", false, 100);
                }
            }
        }

        #endregion //Commands

        public static void _RunCommands(object[] parameters)
        {
            if (parameters != null && parameters.Length > 0 && (parameters[0] is string))
            {
                string temp = (parameters[0] as string).ToLower();
                if (temp == "allpossimtocamera") // Test :)
                {
                    allpossimtocamera_command();
                }
                else if (temp == "savealllotnct")
                {
                    LibraryUtls.AsyncSaveAllLotNoCreateThum();
                }
                else if (temp == "saveallhhnct")
                {
                    LibraryUtls.AsyncSaveAllHouseholdNoCreateThum();
                }
                else if (temp == "scpt")
                {
                    scpt_command();
                }
                else if (temp == "installdc")
                {
                    installdc_command();
                }
                else if (temp == "hideui")
                {
                    hideui_command();
                }
                else if (temp == "forcesetaa3")
                {
                    forcesetaa3_command(Comman.GetTargetGameObject() as Sim, false);
                }
                else if (temp == "allbulot")
                {
                    allbulot_command();
                }
                else if (temp == "rallg")
                {
                    rallg_command();
                }
                else if (temp == "forceallmovein")
                {
                    ForceAllMoveIn();
                }
                else if (temp == "removeallfire")
                {
                    RemoveAllFireFromLot();
                }
                else if (temp == "findhouse")
                {
                    Comman.FindHouse(true);
                }
                else if (temp == "savealllot")
                {
                    savealllot_command();
                }
                else if (temp == "saveallhh")
                {
                    saveallhh_command();
                }
                else if (temp == "saveallhhandlot")
                {
                    saveallhhandlot_command();
                }
                else if (temp == "savenpchousehold")
                {
                    savenpchousehold_command();
                }
                else if (temp == "copyallsimdesc")
                {
                    CopyAllSimDesc();
                }
                else if (temp == "saveallsimdesc")
                {
                    SaveAllSimDesc();
                }
                else if (temp == "loadnpchousehold")
                {
                    loadnpchousehold_command();
                }
                else if (temp == "moveinfromhouse")
                {
                    moveinfromhouse_command();
                }
                else if (temp == "moveoutfromhouse")
                {
                    moveoutfromhouse_command();
                }
                else if (temp == "targetct")
                {
                    targetct_command();
                }
                else if (temp == "autop")
                {
                    autop_command();
                }
                else if (temp == "forcelivemode")
                {
                    forcelivemode_command();
                }
                else if (temp == "importlot")
                {
                    importlot_command();
                }
                else if (temp == "importhouse")
                {
                    importhouse_command();
                }
                else if (temp == "spt")
                {
                    spt_command(parameters);
                }
                else if (temp == "settextpos")
                {
                    settextpos_command();
                }
                else if (temp == "debuginfo")
                {
                    debuginfo_command(true);
                }
                else if (temp == "fixahsims")
                {
                    fixahsims_command();
                }
                else if (temp == "infoplib")
                {
                    DEBUG_Utils.InfoPackagesLib();
                }
                else if (temp == "newhhfromah")
                {
                    newhouseholdfromah_command();
                }
                else if (temp == "newhhfromts")
                {
                    newhouseholdfromts_command();
                }
                else if (temp == "showalltask")
                {
                    DEBUG_Utils.ShowAllTask(true);
                }
                else if (temp == "fixsimbad")
                {
                    fixsimbad_command(null);
                }
                else if (temp == "sdnoage")
                {
                    sdnoage_command();
                }
                else if (temp == "maxmood")
                {
                    maxmood_command();
                }
                else if (temp == "ustsim")
                {
                    ustsim_command();
                }
                else if (temp == "testbreak")
                {
                    Test_Debugger_Break();
                }
                else if (temp == "removepetpool")
                {
                    removepetpool_command();
                }
                else if (temp == "fixallsimdesc")
                {
                    fixallsimdesc_command();
                }
                else if (temp == "fixallsimdesc2")
                {
                    fixallsimdesc2_command();
                }
                else if (temp == "rnewpetpool")
                {
                    rnewpetpool_command();
                }
                else if (temp == "forcesetaa")
                {
                    forcesetaa_command();
                }
                else if (temp == "forcesetaa2")
                {
                    forcesetaa2_command(false);
                }
                else if (temp == "testerrortrap")
                {
                    testerrortrap_command();
                }
                else if (temp == "dtargetobj")
                {
                    dtargetobj_command();
                }
                else if (temp == "aasetp")
                {
                    aasetp_command();
                }
                else if (temp == "sethdp")
                {
                    sethdp_command();
                }
                else if (temp == "houseinvsim")
                {
                    houseinvsim_command();
                }
                else if (temp == "alllotclean")
                {
                    alllotclean_command();
                }
                else if (temp == "addsimtohousehold")
                {
                    addsimtohousehold_command();
                }
                else if (temp == "addsimtohousehold2")
                {
                    addsimtohousehold2_command();
                }
                else if (temp == "forcesavegame")
                {
                    forcesavegame_command();
                }
                else if (temp == "savelot")
                {
                    savelot_command();
                }
                else if (temp == "exhousehold")
                {
                    exhousehold_command();
                }
                else if (temp == "excashousehold")
                {
                    excashousehold_command();
                }
                else if (temp == "bulot")
                {
                    bulot_command();
                }
                else
                    __ShowCommands();
            }
            else __ShowCommands();
        }
    }
}
