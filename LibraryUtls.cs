// Copyright © 2020 Fullham Alfayet
// Licensed under terms of the GPL Version 3. See LICENSE.txt

using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.CustomContent;
using Sims3.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Sims3.UI.GameEntry;
using Sims3.Gameplay.Objects.Island;
using Veitc.AddsCommandPlus.Proxies;

namespace Veitc.AddsCommandPlus
{
    public class LibraryUtls 
    {
        public enum ResultDEBUG : short
        {
            Success,
            Failed,
            ExportBinTypeIsHousehold,
            FailedImport,
            ContentFailure,
            LotTooLowSize,
            NoUIInfo,
            NoContentInfo,
            Error,
            CouldNotFind,
            NeedLoad,
            ArgCantNull
        }
        #region metheds
        public static bool AutoMoveInLotFromHousehold(Household household)
        {
            if (household == null || household.LotHome != null)
                return false;

            List<Lot> templotrandom = new List<Lot>();
            Lot Rlot = null;

            foreach (object obj in LotManager.AllLots)
            {
                Lot lot2 = (Lot)obj;
                if (!lot2.IsWorldLot && !lot2.IsCommunityLotOfType(CommercialLotSubType.kEP10_Diving) && !UnchartedIslandMarker.IsUnchartedIsland(lot2) && lot2.IsResidentialLot && lot2.Household == null && !World.LotIsEmpty(lot2.LotId) && !lot2.IsLotEmptyFromObjects())
                {
                    templotrandom.Add(lot2);
                }
                if (templotrandom.Count == 0)
                {
                    if (!lot2.IsWorldLot && !lot2.IsCommunityLotOfType(CommercialLotSubType.kEP10_Diving) && !UnchartedIslandMarker.IsUnchartedIsland(lot2) && lot2.IsResidentialLot && lot2.Household == null)
                    {
                        templotrandom.Add(lot2);
                    }
                }
            }

            if (templotrandom.Count > 0)
            {
                Rlot = RandomUtil.GetRandomObjectFromList<Lot>(templotrandom);

                try
                {
                    Rlot.MoveIn(household);
                }
                catch
                { }
            }
            else return false;

            return Rlot.Household != null;
        }
        public static List<ulong> CreateIndexMap_(Household house)
        {
            List<ulong> list = new List<ulong>();
            if (house != null)
            {
                foreach (SimDescription simDescription in house.AllSimDescriptions)
                {
                    list.Add(simDescription.SimDescriptionId);
                }
            }
            return list;
        }
        public static void SendHomeForSim(Sim item)
        {
            if (item == null)
                return;

            SimDescription simd = item.SimDescription;
            if (simd == null)
                return;
            if (item.HasBeenDestroyed)
                return;
            if (simd.LotHome == null)
                return;
            if (item.LotCurrent == simd.LotHome)
                return;

            Vector3 pos = Vector3.OutOfWorld;
            Vector3 fwd = Vector3.OutOfWorld;


            try
            {
                if (item.AttemptToFindSafeLocation(true, out pos, out fwd))
                {

                    item.SetPosition(pos);
                    item.SetForward(fwd);

                    if (item.SimRoutingComponent != null)
                        item.SimRoutingComponent.ForceUpdateDynamicFootprint();

                    Simulator.Sleep(0);
                    if (item.LotCurrent == item.LotHome)
                        return;
                }
            }
            catch (ResetException) { throw; }
            catch (Exception)
            {
                if (item.LotCurrent == item.LotHome)
                    return;
            }


            Mailbox mailbox = item.LotHome.FindMailbox();
            World.FindGoodLocationParams fglParams = new World.FindGoodLocationParams(mailbox != null ? mailbox.Position : item.LotHome.Position);
            fglParams.InitialSearchDirection = RandomUtil.GetInt(0x0, 0xF);
            fglParams.BooleanConstraints = FindGoodLocationBooleans.None;



            if (!GlobalFunctions.FindGoodLocation(item, fglParams, out pos, out fwd))
            {
                if (mailbox != null)
                {
                    pos = mailbox.Position;
                    fwd = mailbox.ForwardVector;
                }
                else
                {
                    pos = item.LotHome.Position;
                    fwd = item.LotHome.ForwardVector;
                }

            }

            item.SetPosition(pos);
            item.SetForward(fwd);

            if (item.SimRoutingComponent != null)
                item.SimRoutingComponent.ForceUpdateDynamicFootprint();
        }

        public static void ForceSendHomeAllActors(Household hh)
        {
            if (hh.LotHome == null)
                return;
            foreach (var item in hh.AllActors)
            {
                if (item == null)
                    continue;
                SimDescription simd = item.SimDescription;
                if (simd == null)
                    continue;

                if (simd.LotHome == null)
                    continue;
                if (item.LotCurrent == simd.LotHome)
                    continue;

                Vector3 pos = Vector3.OutOfWorld;
                Vector3 fwd = Vector3.OutOfWorld;


                try
                {
                    if (item.AttemptToFindSafeLocation(true, out pos, out fwd))
                    {

                        item.SetPosition(pos);
                        item.SetForward(fwd);

                        if (item.SimRoutingComponent != null)
                            item.SimRoutingComponent.ForceUpdateDynamicFootprint();

                        Simulator.Sleep(0);
                        if (item.LotCurrent == item.LotHome)
                            continue;
                    }
                }
                catch (ResetException) { throw; }
                catch (Exception)
                {
                    if (item.LotCurrent == item.LotHome)
                        continue;
                }


                Mailbox mailbox = item.LotHome.FindMailbox();
                World.FindGoodLocationParams fglParams = new World.FindGoodLocationParams(mailbox != null ? mailbox.Position : item.LotHome.Position);
                fglParams.InitialSearchDirection = RandomUtil.GetInt(0x0, 0xF);
                fglParams.BooleanConstraints = FindGoodLocationBooleans.None;



                if (!GlobalFunctions.FindGoodLocation(item, fglParams, out pos, out fwd))
                {
                    if (mailbox != null)
                    {
                        pos = mailbox.Position;
                        fwd = mailbox.ForwardVector;
                    }
                    else
                    {
                        pos = item.LotHome.Position;
                        fwd = item.LotHome.ForwardVector;
                    }

                }

                item.SetPosition(pos);
                item.SetForward(fwd);

                if (item.SimRoutingComponent != null)
                    item.SimRoutingComponent.ForceUpdateDynamicFootprint();
            }
        }
        #endregion //metheds

        public static
            void AsyncSaveLot(Lot lot)
        {
            Comman.FuncTask.CreateTask(delegate
            {
                SaveLot(lot);
            });
        }

        public static
           void AsyncSaveLotNoCreateThum(Lot lot)
        {
            Comman.FuncTask.CreateTask(delegate
            {
                SaveLotNoCreateThum(lot);
            });
        }

        public static
            void AsyncSaveHousehold(Household household)
        {
            Comman.FuncTask.CreateTask(delegate
            {
                SaveHousehold(household);
            });
        }

        public static
           void AsyncSaveHouseholdNoCreateThum(Household household)
        {
            Comman.FuncTask.CreateTask(delegate
            {
                SaveHouseholdNoCreateThum(household);
            });
        }

        public static
           void AsyncSaveHouseholdAndLot(Household household)
        {
            if (household.LotHome != null)
            Comman.FuncTask.CreateTask(delegate
            {
                SaveHouseholdAndLot(household);
            });
        }

        public static
            string SaveHousehold(Household household)
        {
            string packageFile = null;
            packageFile = Proxies.HouseholdContentsProxy.VExportHousehold
                (Bin.Singleton, household, false, false, false, false);

            Comman.PrintMessage(packageFile, false, 2000);
            if (Sims3.Gameplay.BinModel.Singleton != null && !string.IsNullOrEmpty(packageFile))
            {
                Sims3.Gameplay.BinModel.Singleton.AddToExportBin(packageFile);
            }
            return packageFile;
        }

        public static
           string SaveHouseholdNoCreateThum(Household household)
        {
            string packageFile = null;
            packageFile = Proxies.HouseholdContentsProxy.VExportHousehold
                (Bin.Singleton, household, false, false, false, true);

            Comman.PrintMessage(packageFile, false, 2000);
            if (Sims3.Gameplay.BinModel.Singleton != null && !string.IsNullOrEmpty(packageFile))
            {
                Sims3.Gameplay.BinModel.Singleton.AddToExportBin(packageFile);
            }
            return packageFile;
        }


        public static
            string SaveHouseholdAndLot(Household household)
        {
            string packageFile = null;
            packageFile = Proxies.HouseholdContentsProxy.VExportHousehold
                (Bin.Singleton, household, household.LotHome != null, false, false, false);

            Comman.PrintMessage(packageFile, false, 2000);
            if (Sims3.Gameplay.BinModel.Singleton != null && !string.IsNullOrEmpty(packageFile))
            {
                Sims3.Gameplay.BinModel.Singleton.AddToExportBin(packageFile);
            }
            return packageFile;
        }

        public static
            string SaveLot(Lot lot)
        {
            if (lot != null && !lot.IsWorldLot)
            {
                string packageFile = null;
                ulong lotContentsID = Sims3.SimIFace.CustomContent.DownloadContent.StoreLotContents(lot, lot.LotId);
                if (lotContentsID != 0)
                {
                    ThumbnailHelper.GenerateLotThumbnailSet(lot.LotId, lotContentsID, ThumbnailSizeMask.ExtraLarge);
                    ThumbnailHelper.GenerateLotThumbnail(lot.LotId, lotContentsID, 0x0, ThumbnailSizeMask.Medium | ThumbnailSizeMask.Large);
                    packageFile = Sims3.SimIFace.CustomContent.DownloadContent.ExportLotContentsToExportBin(lotContentsID);
                    ThumbnailManager.InvalidateLotThumbnails(lot.LotId, lotContentsID, ThumbnailSizeMask.ExtraLarge);
                    ThumbnailManager.InvalidateLotThumbnailsForGroup(lot.LotId, lotContentsID, ThumbnailSizeMask.Medium, 0x0);
                }
                Comman.PrintMessage(packageFile, true, 1000);
                if (Sims3.Gameplay.BinModel.Singleton != null && !string.IsNullOrEmpty(packageFile))
                {
                    Sims3.Gameplay.BinModel.Singleton.AddToExportBin(packageFile);
                }
                return packageFile;
            }
            return null;
        }

        public static
           string SaveLotNoCreateThum(Lot lot)
        {
            if (lot != null && !lot.IsWorldLot)
            {
                string packageFile = null;
                ulong lotContentsID = Sims3.SimIFace.CustomContent.DownloadContent.StoreLotContents(lot, lot.LotId);
                if (lotContentsID != 0)
                {
                    //ThumbnailHelper.GenerateLotThumbnailSet(lot.LotId, lotContentsID, ThumbnailSizeMask.ExtraLarge);
                    //ThumbnailHelper.GenerateLotThumbnail(lot.LotId, lotContentsID, 0x0, ThumbnailSizeMask.Medium | ThumbnailSizeMask.Large);
                    packageFile = Sims3.SimIFace.CustomContent.DownloadContent.ExportLotContentsToExportBin(lotContentsID);
                    try
                    {
                        ThumbnailManager.InvalidateLotThumbnails(lot.LotId, lotContentsID, ThumbnailSizeMask.ExtraLarge);
                        ThumbnailManager.InvalidateLotThumbnailsForGroup(lot.LotId, lotContentsID, ThumbnailSizeMask.Medium, 0x0);
                    }
                    catch (Exception)
                    {}
                   
                }
                Comman.PrintMessage(packageFile, true, 1000);
                if (Sims3.Gameplay.BinModel.Singleton != null && !string.IsNullOrEmpty(packageFile))
                {
                    Sims3.Gameplay.BinModel.Singleton.AddToExportBin(packageFile);
                }
                return packageFile;
            }
            return null;
        }


        public static bool IsLotEmptyFromObjects(Lot lot)
        {
            foreach (IGameObject gameObject in Comman.SCO_GetObjectsOnLot<IGameObject>(lot))
            {
                try
                {
                    var trashcan = gameObject as Sims3.Gameplay.Objects.Miscellaneous.Trashcan;
                    if (trashcan != null)
                    {
                        continue;
                    }
                    Mailbox mailbox = gameObject as Mailbox;
                    if (mailbox != null)
                    {
                        continue;
                    }
                    var fishingSpot = gameObject as Sims3.Gameplay.Objects.Fishing.FishingSpot;
                    if (fishingSpot != null)
                    {
                        continue;
                    }
                    ObjectSpawner objectSpawner = gameObject as ObjectSpawner;
                    if (objectSpawner != null && !objectSpawner.CountsAsLotObject)
                    {
                        continue;
                    }
                    PlumbBob plumbBob = gameObject as PlumbBob;
                    if (plumbBob != null)
                    {
                        continue;
                    }
                    Sim sim = gameObject as Sim;
                    if (sim != null)
                    {
                        continue;
                    }
                    SpawnerBase spawnerBase = gameObject as SpawnerBase;
                    if (spawnerBase == null || !spawnerBase.StaysAfterBulldoze)
                    {
                        SeasonalLotMarker seasonalLotMarker = gameObject as SeasonalLotMarker;
                        if (seasonalLotMarker == null && !(gameObject is IResortFrontDesk) && !(gameObject is Helm) && !(gameObject is HouseboatJig))
                        {
                            return false;
                        }
                    }
                }
                catch (ResetException)
                {
                    throw;
                }
                catch { }
                
            }
            return true;
        }

        public static
            bool AsyncSaveAllLot()
        {
            if (!Simulator.CheckYieldingContext(false))
                return false;
            var lots = LotManager.sLots;
            if (lots == null || lots.Count == 0)
                return false;

            bool done = false;
            foreach (var item in lots)
            {
                var itemLot = item.Value;
                if (itemLot == null || itemLot.IsWorldLot) 
                    continue;
                if (ScriptCore.World.World_LotIsEmptyImpl(itemLot.mLotId) && IsLotEmptyFromObjects(itemLot))
                    continue;

                done = true;
                AsyncSaveLot(itemLot);
            }

            return done;
        }

        public static
           bool AsyncSaveAllLotNoCreateThum()
        {
            if (!Simulator.CheckYieldingContext(false))
                return false;
            var lots = LotManager.sLots;
            if (lots == null || lots.Count == 0)
                return false;

            bool done = false;
            foreach (var item in lots)
            {
                var itemLot = item.Value;
                if (itemLot == null || itemLot.IsWorldLot)
                    continue;
                if (ScriptCore.World.World_LotIsEmptyImpl(itemLot.mLotId) && IsLotEmptyFromObjects(itemLot))
                    continue;

                done = true;
                AsyncSaveLotNoCreateThum(itemLot);
            }

            return done;
        }



        public static
            bool AsyncSaveAllHousehold()
        {
            if (!Simulator.CheckYieldingContext(false))
                return false;

            bool done = false;
            foreach (var itemHousehold in Comman.SCO_GetObjects<Household>())
            {
                if (itemHousehold == null)
                    continue;

                var members = itemHousehold.mMembers;
                if (members == null || members.mAllSimDescriptions == null || members.mPetSimDescriptions == null || members.mSimDescriptions == null) 
                    continue;

                if (members.Count == 0)
                    continue;

                while(members.mAllSimDescriptions.Remove(null));
                while (members.mPetSimDescriptions.Remove(null));
                while (members.mSimDescriptions.Remove(null));

                if (members.Count == 0)
                    continue;

                done = true;
                AsyncSaveHousehold(itemHousehold);
            }

            return done;
        }

        public static
           bool AsyncSaveAllHouseholdNoCreateThum()
        {
            if (!Simulator.CheckYieldingContext(false))
                return false;

            bool done = false;
            foreach (var itemHousehold in Comman.SCO_GetObjects<Household>())
            {
                if (itemHousehold == null)
                    continue;

                var members = itemHousehold.mMembers;
                if (members == null || members.mAllSimDescriptions == null || members.mPetSimDescriptions == null || members.mSimDescriptions == null)
                    continue;

                if (members.Count == 0)
                    continue;

                while (members.mAllSimDescriptions.Remove(null)) ;
                while (members.mPetSimDescriptions.Remove(null)) ;
                while (members.mSimDescriptions.Remove(null)) ;

                if (members.Count == 0)
                    continue;

                done = true;
                AsyncSaveHouseholdNoCreateThum(itemHousehold);
            }

            return done;
        }

        public static
            bool AsyncSaveAllHouseholdAndLot()
        {
            if (!Simulator.CheckYieldingContext(false))
                return false;

            bool done = false;
            foreach (var itemHousehold in Comman.SCO_GetObjects<Household>())
            {
                if (itemHousehold == null || itemHousehold.mLotHome == null)
                    continue;

                var members = itemHousehold.mMembers;
                if (members == null || members.mAllSimDescriptions == null || members.mPetSimDescriptions == null || members.mSimDescriptions == null)
                    continue;

                if (members.Count == 0)
                    continue;

                while (members.mAllSimDescriptions.Remove(null));
                while (members.mPetSimDescriptions.Remove(null));
                while (members.mSimDescriptions.Remove(null));

                if (members.Count == 0)
                    continue;

                done = true;
                AsyncSaveHouseholdAndLot(itemHousehold);
            }

            return done;
        }

        public static
            Household _ImportHousehold(string packageFile, Lot moveinLot, bool full, bool askToCreateSim, out HouseholdContents contents)
        {
            contents = null;
            if (packageFile == null || packageFile.Length == 0)
                return null;
            try
            {

                HouseholdContentsProxy hoc = HouseholdContentsProxy.Import(packageFile);
                if (hoc != null)
                {
                    Household mhouse = hoc.Household;
                    if (mhouse != null && mhouse.mMembers != null && mhouse.AllSimDescriptions != null && mhouse.AllSimDescriptions.Count != 0)
                    {
                        foreach (SimDescription siteem in mhouse.AllSimDescriptions.ToArray())
                        {
                            //Comman.Household_Remove(siteem, true);
                            //Comman.Household_Add(mhouse,siteem, true);
                            siteem.mHousehold = mhouse;
                        }


                        foreach (SimDescription siteem in mhouse.AllSimDescriptions.ToArray())
                        {
                            try
                            {
                                siteem.Fixup();
                            }
                            catch (Exception ex)
                            {
                                Comman.PrintException(siteem.mFirstName + "\nFailed Fixup()", ex);
                            }
                        }
                        
                       

                        bool donelotofmovein = false;
                        if (moveinLot != null && moveinLot.mHousehold == null)
                        {
                            donelotofmovein = true;
                            try
                            {
                                moveinLot.MoveIn(mhouse);

                            }
                            catch (Exception ex)
                            {
                                moveinLot.mHousehold = mhouse;
                                mhouse.mLotId = moveinLot.mLotId;
                                mhouse.mLotHome = moveinLot;
                                Comman.PrintException(null, ex);
                            }
                        }
                        if (!full)
                            full = askToCreateSim && Simulator.CheckYieldingContext(false) && AcceptCancelDialog.Show("Create Sim?");
                        if (full)
                        {
                            //Sim sim = null;
                            if (donelotofmovein)
                            {
                                try
                                {
                                    foreach (SimDescription siteem in mhouse.AllSimDescriptions.ToArray())
                                    {
                                        try
                                        {
                                            if (Comman.SD_OutfitsIsValid(siteem))
                                                siteem.Instantiate(Service.GetPositionInRandomLot(moveinLot), false);
                                            else
                                            {
                                                //siteem.Dispose();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Comman.PrintException(siteem.mFirstName + "\nFailed Instantiate()", ex); 
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Comman.PrintException(null, ex);
                                }
                            }
                            try
                            {
                                BinCommon.CreateInventories(mhouse, hoc.Contents, CreateIndexMap_(mhouse));
                            }
                            catch (Exception ex)
                            {
                                Comman.PrintException(null, ex);
                            }
                            try
                            {
                                mhouse.FixupGenealogy();
                            }
                            catch (Exception ex)
                            {
                                Comman.PrintException(null, ex);
                            }
                        }
                        contents = hoc.Contents;
                        return mhouse;
                    }
                    else
                    {
                        if (mhouse != null)
                            mhouse.Destroy();
                        Comman.PrintMessage("Check mhouse is invalid.\nSorry :(", false, 100);
                    }
                }
                else
                {
                    Comman.PrintMessage("Could not find Package File" + "\n" + packageFile, false, 1000); //"Please Install " + packageFile + " :)", false, 100);
                }
            }
            catch (Exception ex)
            {
                Comman.PrintException(null, ex);
            }
            return null;
        }

        #region metheds:ImportLot*
        public static bool ImportLot_CanBePlacedOnLot(UIBinInfo lotToPlace, UIBinInfo targetLot)
        {
            if (lotToPlace == null || targetLot == null)
            {
                return false;
            }
            float lotx_loty = Math.Max(lotToPlace.LotSizeX, lotToPlace.LotSizeY);
            float lotx_loty2 = Math.Min(lotToPlace.LotSizeX, lotToPlace.LotSizeY);
            float lotx_loty3 = Math.Max(targetLot.LotSizeX, targetLot.LotSizeY);
            float lotx_loty4 = Math.Min(targetLot.LotSizeX, targetLot.LotSizeY);
            if (Math.Round(lotx_loty) > Math.Round(lotx_loty3))
            {
                return false;
            }
            if (Math.Round(lotx_loty2) > Math.Round(lotx_loty4))
            {
                return false;
            }
            return true;
        }
        public static bool ImportLot_PlaceLotContents (Lot targetLot, ulong contentId, ref LotRotationAngle angle, ref LotPosition position, bool bShowProgressDialog, bool forceBulldozeSpawners)
        {
            bool result;

            try
            {
                Sim ActiveActor = PlumbBob.SelectedActor;
                if (ActiveActor != null && ActiveActor.LotCurrent == targetLot)
                {
                    ActiveActor.SetPosition(Service.GetPositionInRandomLot(LotManager.GetWorldLot()));
                }
            }
            catch (Exception)
            { return false; }
            try
            {
                if (!bShowProgressDialog)
                    Comman.Lot_AllDestroyObjects(targetLot);
                targetLot.Bulldoze(false, false, forceBulldozeSpawners, false);
            }
            catch
            { }
            result = DownloadContent.ImportLotContents(contentId, targetLot, targetLot.LotId, ref angle, ref position);
            if (result)
            {
                ThumbnailManager.UpdateDirtyLotThumbnails();
                SeasonalLotMarker[] objects = Comman.SCO_GetObjectsOnLot<SeasonalLotMarker>(targetLot);
                if (objects.Length > 0)
                {
                    objects[0].InitialUpdate();
                }
                targetLot.SetDisplayLevel(targetLot.RoofLevel + 1);
            }
            return result;
        }
        public static ExportBinContents ImportLot_InternalExportBinContents(BinModel bin, string packageFile)
        {
            if (packageFile == null) 
                return null;

            //ExportBin.RefreshExportBin();
            ScriptCore.DownloadContent.DownloadContent_RefreshExportBin();

            ExportBinContents package = BinCommon.GetPackage(ExportBin.GetPackageInfo(packageFile) ?? "");
            if (package != null)
            {
                if (bin != null && bin.mExportBin != null && !bin.mExportBin.Contains(package))
                    bin.mExportBin.Add(package);
                return package;
            }
            return null;
        }
        public static ExportBinContents ImportLot_ExportBinContents(BinModel bin, string packageFile, bool strContains)
        {
            if (bin == null || bin.mExportBin == null) 
                return null;
            if (string.IsNullOrEmpty(packageFile)) 
                return null;

            foreach (var item in bin.mExportBin)
            {
                if (item == null)
                    continue;

                if (strContains)
                {
                    string pN = item.PackageName;
                    if (pN == null) continue;
                    if (pN.Contains(packageFile))
                    {
                        return item;
                    }
                }
                else
                {
                    if (item.PackageName == packageFile)
                    {
                        return item;
                    }
                }
            }
            return ImportLot_InternalExportBinContents(bin, packageFile);
        }
        public static ResultDEBUG ImportLot
            (Lot targetLot,
             string packageFile,
             ref LotRotationAngle lotAngle,
             ref LotPosition lotPosition)
        {
            Simulator.CheckYieldingContext(true);
            if (targetLot == null || string.IsNullOrEmpty(packageFile)) return ResultDEBUG.ArgCantNull;

            IEditTownModel editTownModel = Sims3.UI.Responder.Instance.EditTownModel;
            if (editTownModel == null)
                return ResultDEBUG.NeedLoad;

            ExportBinContents exBinLotC = ImportLot_ExportBinContents(BinModel.Singleton, packageFile, false);
            if (exBinLotC == null)
                return ResultDEBUG.CouldNotFind;

            ulong lotId = targetLot.LotId;

            UIBinInfo targetLotInfo = editTownModel.FindLotBinInfo(lotId) ??
                editTownModel.FindCommunityLotBinInfo(lotId) ??
                editTownModel.FindHouseholdBinInfo(lotId);

            if (targetLotInfo == null)
                return ResultDEBUG.NoUIInfo;

            UIBinInfo importLotInfo = exBinLotC.BinInfo;
            if (importLotInfo == null) return ResultDEBUG.NoContentInfo;

            if (exBinLotC.mExportBinType == ExportBinType.Household)
                return ResultDEBUG.ExportBinTypeIsHousehold;

            if (!ImportLot_CanBePlacedOnLot(importLotInfo, targetLotInfo))
                return ResultDEBUG.LotTooLowSize;

            if (!exBinLotC.IsLoaded())
                exBinLotC.Import(true);


            if (exBinLotC.LotContents == null) return ResultDEBUG.ContentFailure;


            ulong cId = exBinLotC.LotContents.ContentId;

            if (cId == 0 || cId == ulong.MaxValue)
                return ResultDEBUG.ContentFailure;

            Household oldHousehold = targetLot.Household;

            Sim ActiveActor = PlumbBob.SelectedActor;
            if (ActiveActor != null && ActiveActor.LotCurrent == targetLot)
            {
                ActiveActor.SetPosition(Service.GetPositionInRandomLot(LotManager.GetWorldLot()));
            }

            try
            {
                if (!ImportLot_PlaceLotContents(targetLot, cId, ref lotAngle, ref lotPosition, false, true))
                {
                    Simulator.Sleep(0);
                    Comman.Lot_Bulldoze(targetLot,false, true, false, false);
                    Simulator.Sleep(0);

                    targetLot.CommercialLotSubType = CommercialLotSubType.kMisc_NoVisitors;
                    targetLot.ResidentialLotSubType = ResidentialLotSubType.kResidentialUndefined;
                    targetLot.LotType = LotType.Residential;

                    return ResultDEBUG.FailedImport;
                }

                try
                {
                    SeasonalLotMarker[] objects = Comman.SCO_GetObjectsOnLot<SeasonalLotMarker>(targetLot);
                    if (objects.Length > 0)
                    {
                        objects[0].InitialUpdate();
                    }
                }
                catch (StackOverflowException) { throw; }
                catch (ResetException) { throw; }
                catch { }

                targetLot.LotType = importLotInfo.LotType;
                targetLot.CommercialLotSubType = importLotInfo.CommercialLotSubType;
                targetLot.ResidentialLotSubType = importLotInfo.ResidentialLotSubType;
                targetLot.Name = importLotInfo.LotName;
                targetLot.Description = importLotInfo.LotDescription;

                targetLot.UpdateCachedValues();

                var householdContents = exBinLotC.HouseholdContents;
                if (householdContents != null)
                {
                    var household = householdContents.Household;

                    if (household != null)
                    {
                        try
                        {
                            if (oldHousehold != null)
                            {
                                oldHousehold.MoveOut();
                            }
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch { }

                        try
                        {
                            foreach (SimDescription siteem in household.AllSimDescriptions)
                                siteem.Fixup();
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch { }

                        if (targetLot != null && targetLot.Household == null)
                        {
                            //lotID = targetLot.LotId;
                            try
                            {
                                targetLot.MoveIn(household);
                            }
                            catch
                            {
                                targetLot.mHousehold = household;
                                household.mLotId = targetLot.mLotId;
                                household.mLotHome = targetLot;
                            }
                        }

                        try
                        {
                            foreach (SimDescription siteem in household.AllSimDescriptions)
                            {
                                try
                                {
                                    siteem.mHousehold = household;

                                    Sim simCreated;
                                    simCreated = siteem.Instantiate(Vector3.OutOfWorld);
                                    if (simCreated != null)
                                        simCreated.SetPosition(Vector3.OutOfWorld);
                                    siteem.GetMiniSimForProtection().AddProtection(MiniSimDescription.ProtectionFlag.PartialFromPlayer);
                                }
                                catch (StackOverflowException) { throw; }
                                catch (ResetException) { throw; }
                                catch
                                { }
                            }
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch
                        { }

                        try
                        {
                            BinCommon.CreateInventories(household, householdContents, CreateIndexMap_(household));
                            BinCommon.CreateFamilyInventories(household, householdContents);
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch
                        { }

                        try
                        {
                            ForceSendHomeAllActors(household);
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch
                        { }

                        try
                        {
                            household.PostImport();
                            if (!BinCommon.PayForLot(household, targetLot, true))
                            {
                                household.SetFamilyFunds(0, true);
                            }
                            Comman.CreateThread(ScriptExecuteType.Task, delegate { ThumbnailManager.GenerateHouseholdThumbnail(household.HouseholdId, household.HouseholdId, ThumbnailSizeMask.Medium | ThumbnailSizeMask.Large); });
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch
                        { }

                        try
                        {
                            if (oldHousehold != null)
                                AutoMoveInLotFromHousehold(oldHousehold);
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch { }

                        try
                        {
                            if (!ScriptCore.World.World_IsEditInGameFromWBModeImpl() && Simulator.CheckYieldingContext(false) && ((PlumbBob.SelectedActor == null && GameStates.IsLiveState) || (!Comman.IsOnVacation(true) && !GameStates.IsPlayFlowState && !GameStates.IsEditTownState && AcceptCancelDialog.Show("Move In Activate"))))
                            {
                                Household currentHousehold = PlumbBob.sCurrentNonNullHousehold;
                                Sim active = PlumbBob.SelectedActor;
                                PlumbBob.sCurrentNonNullHousehold = null;
                                PlumbBob.ForceSelectActor(null);

                                if (Comman.HouseholdMembersToSim(household, false, true) == null)
                                    PlumbBob.ForceSelectActor(active);
                                else
                                {
                                    PlumbBob.sCurrentNonNullHousehold = currentHousehold;
                                    PlumbBob.CheckForChangeInActiveHousehold(Household.ActiveHousehold, true);
                                }
                            }
                        }
                        catch (StackOverflowException) { throw; }
                        catch (ResetException) { throw; }
                        catch { }
                    }
                }
                return ResultDEBUG.Success;
            }
            catch (Exception)
            { return ResultDEBUG.Error; }
            finally 
            {
                try
                {
                    string name = targetLot.Name;
                    if (name != null && !Lot.IsValidName(name))
                    {
                        targetLot.Name = Lot.GetDefaultLotName(targetLotInfo.LotType);
                        targetLot.Description = "";
                    }
                    if (!Simulator.CheckYieldingContext(false))
                        Comman.CreateThread(ScriptExecuteType.Task,delegate { World.LotClusterTrees(targetLot.LotId, false); ThumbnailManager.UpdateDirtyLotThumbnails(); });
                    else ThumbnailManager.UpdateDirtyLotThumbnails();

                }
                catch (Exception)
                { }
                exBinLotC.Flush();
            }
        }
        #endregion //metheds:ImportLot*
    }
}
