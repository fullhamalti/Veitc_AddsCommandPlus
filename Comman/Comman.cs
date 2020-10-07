// Copyright © 2020 Fullham Alfayet
// Licensed under terms of the GPL Version 3. See LICENSE.txt

#region Using Directives

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

using Sims3.Gameplay;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.UI;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.RealEstate;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.Objects.Island;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.ObjectComponents;

using Sims3.SimIFace;
using Sims3.SimIFace.CAS;

using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.GameEntry;

#endregion

namespace Veitc.AddsCommandPlus
{
    public class Comman
    {

        // Class

        #region class:FuncTask
        public delegate void FuncTask_Function();
        public class FuncTask : Task
        {
        	public static void InitClass() { }
        	
            private ScriptExecuteType _ScriptExecuteType;

            public FuncTask_Function _FuncTask_Function;

            public override ScriptExecuteType ExecuteType
            {
                get
                {
                    return _ScriptExecuteType;
                }
            }

            public FuncTask()
            {
                _FuncTask_Function = OnTick;
                _ScriptExecuteType = ScriptExecuteType.Task;
            }

            public FuncTask(FuncTask_Function func)
            {
                _ScriptExecuteType = ScriptExecuteType.Task;
                _FuncTask_Function = func;
            }

            public FuncTask(ScriptExecuteType executeType, FuncTask_Function func)
            {
                if (executeType == ScriptExecuteType.None || executeType == ScriptExecuteType.InitFailed)
                {
                    _ScriptExecuteType = ScriptExecuteType.Task;
                }
                else
                {
                    _ScriptExecuteType = executeType;
                }
                if (func == null)
                {
                    DEBUG_Utils.Print("CreateTask(): func == null");
                }
                _FuncTask_Function = func;
            }

            public static ObjectGuid CreateTask(FuncTask_Function func)
            {
                if (func == null)
                {
                    DEBUG_Utils.Print("CreateTask(): func == null");
                    return ObjectGuid.InvalidObjectGuid;
                }
                return new FuncTask(func).AddToSimulator();
            }

            public static ObjectGuid CreateTask(ScriptExecuteType executeType, FuncTask_Function func)
            {
                if (executeType == ScriptExecuteType.None || executeType == ScriptExecuteType.InitFailed)
                {
                    executeType = ScriptExecuteType.Task;
                }
                if (func == null)
                {
                    DEBUG_Utils.Print("CreateTask(): func == null");
                    return ObjectGuid.InvalidObjectGuid;
                }
                return new FuncTask(executeType, func).AddToSimulator();
            }

            public static FuncTask GetCurrentTask()
            {
                return ScriptCore.Simulator.Simulator_GetTaskImpl(ScriptCore.Simulator.Simulator_GetCurrentTaskImpl(), false) as FuncTask;
            }

            public ObjectGuid AddToSimulator()
            {
                return Sims3.SimIFace.Simulator.AddObject(this);
            }

            protected virtual void OnTick() { }

            public override void Simulate()
            {
                try
                {
                    //if (_FuncTask_Function != null)
                    //{
                    //    _FuncTask_Function();
                    //}
                    //else
                    // mono fail GC bug 
                    if (_FuncTask_Function == null || _FuncTask_Function.method_info == null || (!_FuncTask_Function.Method.IsStatic && _FuncTask_Function.Target == null))
                    {
                        DEBUG_Utils.Print("_FuncTask_Function == null");
                    }
                    else _FuncTask_Function();
                }
                catch (ResetException ex)
                {
                    if (ex.StackTrace != null)
                    {
                        PrintException(ToString(), ex);
                    }
                }
                catch (Exception exception)
                {
                    PrintException(ToString(), exception);
                }
#pragma warning disable 1058
                catch
                {
                    PrintMessage("Failed to Mono Runtime!\n" + ToString(), false, 100);
                }
#pragma warning restore 1058
                finally
                {
#if TEST_FASTCALL_NATIVE
                    ScriptCore.Simulator.Simulator_DestroyObjectImpl(base.ObjectId.Value); 
#else
                    Simulator.DestroyObject(base.ObjectId);
#endif
                }
            }

            public override void Dispose()
            {
                try
                {
                    base.Dispose();
                }
                catch (Exception ex)
                {
                    PrintException(ToString(), ex);
                }
            }

            public override string ToString()
            {
                if (_FuncTask_Function == null)
                {
                    return "(FuncTask_Function) NULL function";
                }
                return "(FuncTask_Function) Function method: " + _FuncTask_Function.Method.ToString() + ", Declaring Type: " + _FuncTask_Function.Method.DeclaringType.ToString();
            }
        }
        #endregion // class:FuncTask

        // Metheds

        #region metheds:Lot_*
        private static bool Lot_Bulldoze_pshouldKeepSpawnerAfterBulldoze(SpawnerBase spawner)
        {
            if (spawner.StaysAfterBulldoze)
            {
                return spawner.Level >= 0;
            }
            return false;
        }
        public static void  Lot_Bulldoze(Lot _This, bool bAllowDestruction, bool bRestartLotRenderer, bool bForceIncludeSpawners, bool bAllowEnsureObjects)
        {
            if (_This == null)
                throw new NullReferenceException();
            try
            {
                foreach (var seasonalLotMarker in SCO_GetObjectsOnLot<SeasonalLotMarker>(_This))
                {
                    GO_ForceDestroy(seasonalLotMarker);
                }
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }


            List<SpawnerBase> listSpawnerBase = null;
            Vector3[] verSpawnerList = null;

            try
            {
                if (!bForceIncludeSpawners)
                {
                    listSpawnerBase = new List<SpawnerBase>();
                    var listTemp = SCO_GetObjectsOnLot<SpawnerBase>(_This);
                    foreach (var item in listTemp)
                    {
                        if (item != null && Lot_Bulldoze_pshouldKeepSpawnerAfterBulldoze(item))
                        {
                            listSpawnerBase.Add(item);
                        }
                    }

                    verSpawnerList = new Vector3[listSpawnerBase.Count];
                    for (int j = 0; j < listSpawnerBase.Count; j++)
                    {
                        verSpawnerList[j] = listSpawnerBase._items[j].Position;
                        listSpawnerBase._items[j].SetPosition(Vector3.OutOfWorld);
                    }
                }
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }


            try
            {
                RealEstateManager.OnLotBulldozed(_This);
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }


            bool isDeleteLot = bAllowDestruction && World.LotIsEmpty(_This.LotId);

            ScriptCore.World.World_BulldozeLotImpl(_This.LotId, bAllowDestruction, bRestartLotRenderer);

            try
            {
                if (listSpawnerBase != null)
                {
                    for (int k = 0; k < listSpawnerBase.Count; k++)
                    {
                        listSpawnerBase._items[k].SetPosition(verSpawnerList[k]);
                    }
                }


                if (_This.mSavedData.mOpportunityHelper != null)
                {
                    _This.mSavedData.mOpportunityHelper.CancelOpportunity();
                }

                if (bAllowDestruction)
                {
                    if (_This.mVirtualSlotHouseholds != null && _This.mVirtualSlotHouseholds.Count > 0)
                    {
                        _This.VirtualMoveOut(_This.mVirtualSlotHouseholds);
                    }
                    if (_This.mVirtualSlotSims != null && _This.mVirtualSlotSims.Count > 0)
                    {
                        _This.VirtualMoveOut(_This.mVirtualSlotSims);
                    }
                }
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }



            _This.mImportedDoorList = null;
            _This.mImportedResidentList = null;
            _This.mImportedDoorIsHouseholdList = null;

            if (_This.IsCommunityLot)
            {
                _This.CommercialLotSubType = CommercialLotSubType.kMisc_NoVisitors;
            }

            try
            {
                if (isDeleteLot)
                {
                    if (_This.IsHouseboatLot())
                    {
                        Houseboat houseboat = _This.GetHouseboat();
                        if (houseboat != null)
                        {
                            houseboat.Destroy();
                        }
                    }
                    MetaAutonomyManager.RemoveHotSpotsOrDeadZoneIfNecessary(_This);
                }
                else
                {
                    Occupation.OnBulldozeLot(_This);
                    if (bAllowEnsureObjects)
                    {
                        _This.EnsureLotObjects();
                    }

                    _This.Name = "";
                    _This.Description = "";
                }
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }
        }
        public static void  Lot_RepairAllForLot(Lot lot)
        {
            if (lot == null)
            {
                throw new NullReferenceException();
            }

            Sim sim_ = PlumbBob.SelectedActor ?? GetRandomSim(null);

            foreach (GameObject gameObject in SCO_GetObjectsOnLot<GameObject>(lot))
            {
                if (gameObject == null || gameObject.InUse || !gameObject.InWorld)
                {
                    continue;
                }

                if (gameObject.Charred)
                {
                    gameObject.Charred = false;
                    if (gameObject is Windows)
                    {
                        RepairableComponent.CreateReplaceObject(gameObject);
                    }
                }

                RepairableComponent repairable = gameObject.Repairable;
                if (repairable != null && repairable.Broken)
                {
                    repairable.ForceRepaired(sim_);
                }
            }

            TombRoomManager tombm = lot.TombRoomManager;

            LotLocation[] burntTiles = World.GetBurntTiles(lot.LotId, LotLocation.Invalid);
            if (burntTiles != null && burntTiles.Length > 0)
            {
                foreach (LotLocation lotLocation in burntTiles)
                {
                    if (lot.LotLocationIsPublicResidential(lotLocation) && (tombm == null || !tombm.IsObjectInATombRoom(lotLocation)))
                    {
                        World.SetBurnt(lot.LotId, lotLocation, burnt: false);
                    }
                }
            }
        }
        public static void  Lot_CleanUpAllForLot(Lot lot, bool bNeedCleanBill)
        {
            if (lot == null)
            {
                throw new NullReferenceException();
            }

            World.DecayLeaves(lot.LotId, 1f);

            TombRoomManager tombm = lot.TombRoomManager;

            foreach (GameObject gameObject in SCO_GetObjectsOnLot<GameObject>(lot))
            {
                if (gameObject == null || gameObject.InUse || gameObject.InInventory)
                {
                    continue;
                }

                var cleanable = gameObject.Cleanable;

                if (cleanable != null)
                {
                    cleanable.ForceClean();
                }

                Hamper hamper = gameObject as Hamper;
                if (hamper != null)
                {
                    hamper.RemoveClothingPiles();
                }

                if (bNeedCleanBill)
                {
                    Bill bill = gameObject as Bill;
                    if (bill != null)
                    {
                        bill.Amount = 0;
                        bill.OriginatingHousehold = null;
                        bill.mBillAgeInDays = 0;
                        bill.DestroyBill(false);
                    }
                }

                IThrowAwayable throwAwayable = gameObject as IThrowAwayable;
                if (throwAwayable != null &&
                    !throwAwayable.InUse &&
                    throwAwayable.HandToolAllowUserPickupBase() &&
                    throwAwayable.ShouldBeThrownAway() &&
                    (throwAwayable.Parent == null || !throwAwayable.Parent.InUse) &&
                    !(throwAwayable is Sims3.Gameplay.Objects.Counters.Bar.Glass) &&
                    !(throwAwayable is Bill))
                {
                    bool isBarGlass = false;
                    if (throwAwayable is Sims3.Gameplay.Objects.CookingObjects.BarTray)
                    {
                        foreach (Slot slotName in throwAwayable.GetContainmentSlots())
                        {
                            if (throwAwayable.GetContainedObject(slotName) is Sims3.Gameplay.Objects.Counters.Bar.Glass)
                            {
                                isBarGlass = true;
                                break;
                            }
                        }
                    }

                    if (!isBarGlass)
                    {
                        throwAwayable.ThrowAwayImmediately();
                    }
                }

                if (gameObject is IDestroyOnMagicalCleanup && !(gameObject is Sim) && !(gameObject is PlumbBob))
                {
                    gameObject.FadeOut(false, true);
                }

            }

            var firem = lot.FireManager;

            if (firem != null)
                firem.RestoreObjects();

            LotLocation[] puddles = World.GetPuddles(lot.LotId, LotLocation.Invalid);

            if (puddles != null && puddles.Length > 0)
            {
                foreach (LotLocation lotl in puddles)
                {
                    if (tombm == null || !tombm.IsObjectInATombRoom(lotl))
                    {
                        PuddleManager.RemovePuddle(lot.LotId, lotl);
                    }
                }
            }

            LotLocation[] burntTiles = World.GetBurntTiles(lot.LotId, LotLocation.Invalid);
            if (burntTiles != null && burntTiles.Length > 0)
            {
                foreach (LotLocation lotl in burntTiles)
                {
                    if (lot.LotLocationIsPublicResidential(lotl) && (tombm == null || !tombm.IsObjectInATombRoom(lotl)))
                    {
                        World.SetBurnt(lot.LotId, lotl, false);
                    }
                }
            }

            var householdLot = lot.Household;
            if (householdLot != null)
            {
                var listFridge = SCO_GetObjectsOnLot<Sims3.Gameplay.Objects.Appliances.Fridge>(lot);
                if (listFridge.Length > 0)
                {
                    Sims3.Gameplay.Objects.Appliances.Fridge fridge = listFridge[0];
                    Inventory hSharedFridgeInventory = householdLot.SharedFridgeInventory == null ? null : householdLot.SharedFamilyInventory.Inventory;

                    if (fridge != null && hSharedFridgeInventory != null)
                    {
                        var servingConList = SCO_GetObjectsOnLot
                            <Sims3.Gameplay.Objects.CookingObjects.ServingContainer>(lot);

                        foreach (var servingContainer in servingConList)
                        {
                            if (!servingContainer.InUse &&
                                fridge.HandToolAllowDragDrop(servingContainer) &&
                                servingContainer.HasFood &&
                                servingContainer.HasFoodLeft() &&
                                !servingContainer.IsSpoiled &&
                                servingContainer.GetQuality() >= Sims3.Gameplay.Objects.Quality.Neutral)
                            {
                                hSharedFridgeInventory.TryToAdd((IGameObject)servingContainer, false);
                            }
                        }
                    }
                }
            }

            foreach (Sim allActor in SCO_GetObjectsOnLot<Sim>(lot))
            {
                var sInventory = allActor.Inventory;
                if (sInventory != null && sInventory.mItems != null && sInventory.mInventoryItems != null)
                {
                    foreach (IThrowAwayable item in sInventory.FindAll<IThrowAwayable>(true))
                    {
                        if (item != null &&
                            item.HandToolAllowUserPickupBase() &&
                            item.ShouldBeThrownAway() &&
                            !item.InUse &&
                            (!(item is Newspaper) || (item as Newspaper).IsOld) &&
                            !(item is TrashPileOpportunity) &&
                            (!(item is Sims3.Gameplay.Objects.CookingObjects.PreparedFood) || (item as Sims3.Gameplay.Objects.CookingObjects.PreparedFood).IsSpoiled))
                        {
                            item.ThrowAwayImmediately();
                        }
                    }
                }
            }
        }
        public static
            Vector3 Lot_SafeGetPositionInRandomLot(Lot lot)
        {
            Vector3 vector = Vector3.Origin;
            Vector3[] posResults;
            Quaternion[] orientResults;

            if (lot == null && (LotManager.sLots == null || LotManager.sLots.Count == 0))
            {
                if (World.FindPlaceOnRoadOffScreen(null, vector, FindPlaceOnRoadOption.FootpathOrSidewalk, 30f, out posResults, out orientResults) && posResults != null && posResults.Length > 0)
                {
                    return posResults[0];
                }
                if (World.FindPlaceOnRoadOffScreen(null, vector, FindPlaceOnRoadOption.FootpathOrSidewalk, 200f, out posResults, out orientResults) && posResults != null && posResults.Length > 0)
                {
                    return posResults[0];
                }
                Vector3 posResult = Vector3.Invalid;
                Quaternion orientResult = Quaternion.Identity;
                if (World.FindPlaceOnRoad(null, vector, 4u, ref posResult, ref orientResult))
                {
                    return posResult;
                }
                return vector;
            }

            if (lot != null)
            {
                vector = lot.EntryPoint();
            }
            else if (LotManager.AllLots.Count > 0)
            {
                Lot[] array = new Lot[LotManager.AllLots.Count];
                LotManager.AllLots.CopyTo(array, 0);
                lot = array[RandomUtil.GetInt(array.Length - 1)];
            }

            if (World.FindPlaceOnRoadOffScreen(null, vector, FindPlaceOnRoadOption.FootpathOrSidewalk, 30f, out posResults, out orientResults) && posResults != null && posResults.Length > 0)
            {
                return posResults[0];
            }
            if (World.FindPlaceOnRoadOffScreen(null, vector, FindPlaceOnRoadOption.FootpathOrSidewalk, 200f, out posResults, out orientResults) && posResults != null && posResults.Length > 0)
            {
                return posResults[0];
            }
            if (lot != null)
            {
                uint offsetHint = 0u;
                Vector3 outPos = Vector3.Invalid;
                if (LotManager.FindPlaceOutsideLot(lot, ref offsetHint, ref outPos))
                {
                    return outPos;
                }
            }
            return vector;
        }
        public static void Lot_AllDestroyObjects(Lot targetLot)
        {
            var worldpos = Sims3.Gameplay.Services.Service.GetPositionInRandomLot(LotManager.GetWorldLot());
            if (!Vector3_IsUnSafe(worldpos))
            {
                try
                {
                    foreach (var iteam in SCO_GetObjectsOnLot<Sim>(targetLot))
                    {
                        iteam.SetObjectToReset();
                    }
                }
                catch (ResetException)
                {
                    throw;
                }
                catch { }
                foreach (var iteam in SCO_GetObjectsOnLot<Sim>(targetLot))
                {
                    iteam.SetPosition(worldpos);
                }
            }
            foreach (var iteam in SCO_GetObjectsOnLot<Sims3.Gameplay.Objects.Urnstone>(targetLot))
            {
                var simd = iteam.DeadSimsDescription;
                iteam.DeadSimsDescription = null;
                SD_ResurrectSim(simd, Household_FindIDSC(iteam.mOriginalHouseholdId), true, false);
                GO_ForceDestroy(iteam);
            }
            foreach (var iteam in SCO_GetObjectsOnLot<GameObject>(targetLot))
            {
                GO_ForceDestroy(iteam);
            }
        }
        #endregion // metheds:Lot_*

        #region metheds:SimDescription_*
        public static bool SD_ResurrectSim(SimDescription deadSimDesc, Household originalHousehold, bool createHousehold, bool toSpecialHousehold)
        {
            if (deadSimDesc == null  || !SD_OutfitsIsValid(deadSimDesc) || deadSimDesc.mHairColors == null)
                return false;

            if (string.IsNullOrEmpty(deadSimDesc.mFirstName) && string.IsNullOrEmpty(deadSimDesc.mLastName))
                return false;

            if (!deadSimDesc.IsValidDescription)
            {
                try
                {
                    deadSimDesc.Fixup();
                }
                catch (StackOverflowException) { throw; }
                catch (ResetException)
                { throw; }
                catch
                { deadSimDesc.mIsValidDescription = false; return false; }
            }

            if (!deadSimDesc.IsValidDescription)
                return false;

            if (deadSimDesc.Household == null)
            {
                if (originalHousehold != null)
                {
                    Household_Add(originalHousehold, deadSimDesc, false);
                }
                if (deadSimDesc.Household == null)
                {
                    Household_Add(GetRandomGameObject<Household>(delegate(Household item)
                    {
                        if (item == null)
                            return false;

                        if (Household_IsSpecialHousehold(item))
                            return false;

                        return item.LotHome != null;
                    }), deadSimDesc, false);
                }
                if (createHousehold && deadSimDesc.Household == null)
                {
                    Household va = Household.Create();
                    LibraryUtls.AutoMoveInLotFromHousehold(va);
                    Household_Add(va, deadSimDesc, false);
                }
                if (toSpecialHousehold && deadSimDesc.Household == null)
                {
                    Household_Add(Household.sServobotHousehold ?? Household.sPetHousehold ?? Household.ActiveHousehold, deadSimDesc, false);
                }
                if (deadSimDesc.Household == null)
                {
                    DEBUG_Utils.Print("SD_ResurrectSim() Could not find household!");
                    return false;
                }
            }

            deadSimDesc.IsGhost = false;
            deadSimDesc.mDeathStyle = SimDescription.DeathType.None;
            deadSimDesc.IsNeverSelectable = false;

            foreach (var itemGrave in SCO_GetObjects<Urnstone>())
            {
                if (itemGrave != null && itemGrave.DeadSimsDescription == deadSimDesc)
                    itemGrave.GhostTurnDeathEffectOff(VisualEffect.TransitionType.SoftTransition);
            }

            Sim createdSim = deadSimDesc.CreatedSim;
            if (createdSim != null && createdSim.SimDescription == deadSimDesc)
            {
                World.ObjectSetGhostState(createdSim.ObjectId, 0, (uint)deadSimDesc.AgeGenderSpecies);
                Autonomy auon = createdSim.Autonomy;
                if (auon != null)
                    auon.AllowedToRunMetaAutonomy = true;
                if (createdSim.DeathReactionBroadcast != null)
                {
                    createdSim.DeathReactionBroadcast.Dispose();
                    createdSim.DeathReactionBroadcast = null;
                }
                if (createdSim.GhostReactionBroadcast != null)
                {
                    createdSim.GhostReactionBroadcast.Dispose();
                    createdSim.GhostReactionBroadcast = null;
                }
            }
            return true;
        }
        public static bool SD_OutfitsIsValid2(SimDescription _This, bool pa)
        {
            try
            {
                var p = _This.mOutfits;
                if (p != null && p.Count > 0)
                {
                    foreach (OutfitCategories key in p.Keys)
                    {
                        var arrayList = p[key] as System.Collections.ArrayList;
                        if (arrayList != null && arrayList.Count > 0)
                        {
                            foreach (object item in arrayList)
                            {
                                if (!(item is SimOutfit)) 
                                    return false;
                            }
                        }
                    }
                    return true;
                }
                return p != null && pa;
            }
            catch (StackOverflowException)
            { throw; }
            catch (ResetException)
            { throw; }
            catch { }
           
            return false;
        }
        public static bool SD_OutfitsIsValid(SimDescription _This)
        {
            if (_This == null)
                throw new NullReferenceException();
            if (_This.mOutfits == null)
                return false;
            if (_This.Pregnancy != null && _This.mMaternityOutfits == null)
                return false;
            if (_This.GetCurrentOutfits() == null)
                return false;

            try
            {
                SimOutfit o = _This.GetOutfit(OutfitCategories.Everyday, 0);
                return o != null && o.IsValid;
            }
            catch (StackOverflowException)
            { throw; }
            catch (ResetException)
            { throw; }
            catch { return false; }

        }
        public static Sim SD_SafeInstantiate(SimDescription simDesc, Vector3 pos)
        {
            if (simDesc == null) {
                throw new NullReferenceException();
            }

            if (simDesc.CreatedSim != null) {
                return simDesc.CreatedSim;
            }

            if (simDesc.Household == null) {
                throw new ArgumentException("if (simDesc.Household == null)");
            }

            if (SD_OutfitsIsValid(simDesc) && simDesc.IsValidDescription && simDesc.IsValid)
            {
                if (pos == Vector3.OutOfWorld)
                {
                    pos = Lot_SafeGetPositionInRandomLot((simDesc.LotHome ?? LotManager.GetWorldLot()));
                    if (pos == Vector3.Origin || pos == Vector3.OutOfWorld || pos == Vector3.Empty)
                    {
                        pos = World.SnapToFloor(ScriptCore.CameraController.Camera_GetTarget());
                    }
                }
                try { return simDesc.Instantiate(pos, false); }
                catch (StackOverflowException) { throw; }
                catch (ResetException) { throw; }
                catch (Exception)
                {
                    var sim = simDesc.CreatedSim;
                    if (sim != null && ScriptCore.Objects.Objects_IsValid(sim.ObjectId.mValue))
                    {
                        sim.mSimDescription = simDesc;
                        return sim;
                    }
                    else
                    {
                        simDesc.mSim = null;
                        GO_ForceDestroy(sim);
                        return null;
                    }
                }
            }
            return simDesc.CreatedSim;
        }
        public static void SD_GetAging(IMiniSimDescription me, out int age, out int maxAge)
        {
            var amst = AgingManager.Singleton;
            if (amst == null) { age = 0; maxAge = 0; return; }

            SimDescription simDescription = me as SimDescription;
            MiniSimDescription miniSimDescription = me as MiniSimDescription;
            float agingYears = 0f;

            if (simDescription != null)
            {
                agingYears = simDescription.AgingYearsSinceLastAgeTransition;
            }
            else if (miniSimDescription != null)
            {
                agingYears = miniSimDescription.AgingYearsSinceLastAgeTransition;
            }

            maxAge = (int)amst.AgingYearsToSimDays(AgingManager.GetMaximumAgingStageLength(me));
            age = (int)amst.AgingYearsToSimDays(agingYears);
        }
        public static void SD_SetAging(IMiniSimDescription me, int newAge)
        {
            SimDescription simDescription = me as SimDescription;
            MiniSimDescription miniSimDescription = me as MiniSimDescription;

            if (simDescription != null && simDescription.AgingState == null)
            {
                simDescription.AgingState = new AgingState(simDescription);
            }

            float agingYears = 0f;

            if (simDescription != null)
            {
                agingYears = simDescription.AgingYearsSinceLastAgeTransition;
            }
            else if (miniSimDescription != null)
            {
                agingYears = miniSimDescription.AgingYearsSinceLastAgeTransition;
            }

            var amst = AgingManager.Singleton;

            

            var rederInst = Sims3.UI.Responder.Instance;

            if (rederInst == null)
            {
                return;
            }

            var hudModel = (rederInst.HudModel as HudModel);

            if (amst == null || hudModel == null)
            {
                return;
            }

            int currentAge = (int)amst.AgingYearsToSimDays(agingYears);
            if (newAge == currentAge)
            {
                return;
            }

            if (simDescription != null)
            {
                if (amst != null)
                {
                    amst.CancelAgingAlarmsForSim(simDescription.AgingState);
                    simDescription.AgingYearsSinceLastAgeTransition = amst.SimDaysToAgingYears(newAge);
                }

                if (rederInst != null && simDescription.Household == Household.ActiveHousehold && simDescription.CreatedSim != null)
                {
                    if (hudModel != null)
                    {
                        hudModel.OnSimAgeChanged(simDescription.CreatedSim.ObjectId);
                    }
                }
            }
            else if (miniSimDescription != null && amst != null)
            {
                miniSimDescription.AgingYearsSinceLastAgeTransition = amst.SimDaysToAgingYears(newAge);
            }
        }
        public static void SD_SetID(SimDescription This, ulong ValueID)
        {

            if (This == null)
                throw new NullReferenceException();

            ulong tempidx = This.SimDescriptionId;
            This.mSimDescriptionId = ValueID;
            This.mOldSimDescriptionId = tempidx;


            try
            {
                var FindMiniX = MiniSimDescription.Find(tempidx);

                if (FindMiniX != null)
                    FindMiniX.mSimDescriptionId = ValueID;

                if (This.CelebrityManager != null)
                    This.CelebrityManager.ResetOwnerSimDescription(This.mSimDescriptionId);
                if (This.PetManager != null)
                    This.PetManager.ResetOwnerSimDescription(This.mSimDescriptionId);
                if (This.TraitChipManager != null)
                    This.TraitChipManager.ResetOwnerSimDescription(This.mSimDescriptionId);
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }

        }
        #endregion // metheds:SimDescription_*

        #region metheds:GameObject_*
        public static void GO_ForceDeAttachAndDestroyAllSlots(GameObject obj, bool needHaveScript)
        {
            ObjectGuid guid = obj.ObjectId;
            Slot[] slots = Slots.GetContainmentSlotNames(guid);
            if (slots == null || slots.Length == 0) { return; }
            foreach (var itemSlot in slots)
            {
                ObjectGuid[] itemChilderns = Slots.GetChildren(guid, itemSlot);
                if (itemChilderns == null || itemChilderns.Length == 0)
                { continue; }

                foreach (var itemChild in itemChilderns)
                {
                    if (ScriptCore.Objects.Objects_IsValid(itemChild.Value))
                    {
                        IScriptProxy proxy = Simulator.GetProxy(itemChild);
                        if (proxy != null)
                        {
                            var gameObj = proxy.Target as GameObject;
                            if (gameObj == null)
                            {
                                if (needHaveScript)
                                {
                                    continue;
                                }
                                Simulator.DestroyObject(itemChild);
                            }
                            else
                            {
                                if (gameObj == obj || gameObj is PlumbBob || gameObj is Lot)
                                {
                                    continue;
                                }
                                var simChild = gameObj as Sim;
                                if (simChild != null)
                                {
                                    simChild.mPosture = simChild.Standing;
                                    Slots.DetachFromSlot(simChild.ObjectId);
                                    continue;
                                }
                                GO_ForceDestroy(gameObj);
                            }
                        }
                        else if (!needHaveScript)
                        {
                            Simulator.DestroyObject(itemChild);
                        }
                    }
                    else if (!needHaveScript)
                    {
                        if (itemChild != ObjectGuid.InvalidObjectGuid)
                        {
                            var task = Simulator.GetTask(itemChild);
                            if (task != null)
                            {
                                if (task == obj || task is PlumbBob || task is Lot)
                                {
                                    continue;
                                }

                                var simChild = task as Sim;
                                if (simChild != null)
                                {
                                    simChild.mPosture = simChild.Standing;
                                    Slots.DetachFromSlot(simChild.ObjectId);
                                    continue;
                                }

                                IDisposable sdispose = task as IDisposable;
                                if (sdispose != null)
                                {
                                    sdispose.Dispose();
                                }
                                Simulator.DestroyObject(itemChild);
                            }
                            else { Slots.DetachFromSlot(itemChild); }
                        }
                        else { Slots.DetachFromSlot(itemChild); }
                    }
                }
            }
        }
        public static Lot GO_GetLot(ulong objID)
        {
            if (objID == 0) return null;
            var proxy = ScriptCore.Simulator.Simulator_GetTaskImpl(ScriptCore.Objects.Objects_GetLotId(objID), false) as IScriptProxy;
            if (proxy != null)
            {
                var targetLot = proxy.Target;
                if (targetLot != null && targetLot is Lot)
                    return targetLot as Lot;
            }
            return null;
        }
        public static Lot GO_GetLot(GameObject obj)
        {
            if (obj == null) return null;
            var proxy = ScriptCore.Simulator.Simulator_GetTaskImpl(ScriptCore.Objects.Objects_GetLotId(obj.ObjectId.mValue), false) as IScriptProxy;
            if (proxy != null)
            {
                var targetLot = proxy.Target;
                if (targetLot != null && targetLot is Lot)
                    return targetLot as Lot;
            }
            return null;
        }
        public static bool GO_AddInventoryInteraction(GameObject gameObject, InteractionDefinition singleton)
        {
            if (gameObject == null)
            {
                return false;
            }

            if (singleton == null)
            {
                DEBUG_Utils.Print("GO_AddInteraction(): singleton == null");
                return false;
            }

            var type = singleton.GetType();
            var itemComp = gameObject.ItemComp;
            if (((itemComp != null) ? itemComp.InteractionsInventory : null) != null)
            {
                foreach (InteractionObjectPair item in gameObject.ItemComp.InteractionsInventory)
                {
                    if (item.InteractionDefinition.GetType() == type)
                    {
                        return false;
                    }
                }
                gameObject.AddInventoryInteraction(singleton);
            }
            return true;
        }
        public static bool GO_AddInteraction(GameObject gameObject, InteractionDefinition singleton)
        {
            if (gameObject == null)
            {
                return false;
            }
            if (singleton == null)
            {
                DEBUG_Utils.Print("GO_AddInteraction(): singleton == null");
                return false;
            }
            //if (gameObject.mInteractions == null)
            //{
            //    return false;
            //}

            var type = singleton.GetType();
            foreach (InteractionObjectPair interaction in gameObject.Interactions)
            {
                if (interaction.InteractionDefinition.GetType() == type)
                {
                    return false;
                }
            }

            gameObject.AddInteraction(singleton, false);
            return true;
        }
        public static bool GO_ForceDestroy(GameObject gameObject)
        {
            if (gameObject == null)
                return false;

            ObjectGuid ObjectID = gameObject.ObjectId;
            try
            {
                gameObject.Destroy();
                if (!gameObject.HasBeenDestroyed)
                { try { Simulator.DestroyObject(ObjectID); } catch { } }
            }
            catch (ResetException) { throw; }
            catch { try { Simulator.DestroyObject(ObjectID); } catch { } }
            return true;
        }
        public static bool GO_ForceDestroy02(IGameObject iGameObject)
        {
            if (iGameObject == null)
                return false;

            ObjectGuid ObjectID = iGameObject.ObjectId;
            try
            {
                iGameObject.Destroy();
                if (!iGameObject.HasBeenDestroyed)
                { try { Simulator.DestroyObject(ObjectID); } catch { } }
            }
            catch (ResetException) { throw; }
            catch { try { Simulator.DestroyObject(ObjectID); } catch { } }
            return true;
        }
        #endregion // metheds:GameObject_*

        #region metheds:Message
        // Bypass if (Responder.Instance.TutorialModel.IsTutorialRunning())
        public static void PrintMessage(string Message, bool NoMyModText, float TimeOut)
        {

            if (NotificationManager.sNotificationManager == null)
            {
                //string m = null;
                //if (NoMyModText)
                //{
                //    m = (Message == "" ? "No Message" : Message ?? "No Message");
                //}
                //else
                //{
                //    m = (_thisAssembly._name + "\n" + (Message == "" ? "No Message" : Message ?? "No Message"));
                //}

                LogUtils.sPendingNotifications.Add(string.IsNullOrEmpty(Message) ? "No Message" : Message);
                return;
            }
            StyledNotification.Format format =
                (!NoMyModText) ? new StyledNotification.Format
                //(_thisAssembly._name + "\n" + (Message == "" ? "No Message" : Message ?? "No Message"),
                    (_thisAssembly._name + "\n" + (string.IsNullOrEmpty(Message) ? "No Message" : Message),
                    StyledNotification.NotificationStyle.kGameMessagePositive)
                : new StyledNotification.Format
                //((Message == "" ? "No Message" : Message ?? "No Message"),
                    ((string.IsNullOrEmpty(Message) ? "No Message" : Message),
                    StyledNotification.NotificationStyle.kGameMessagePositive);

            format.mConnectionType =
                StyledNotification.ConnectionType.kSpeech;
            format.mTNSCategory =
                    NotificationManager.TNSCategory.Information;


            StyledNotification styledNotification = new StyledNotification
                (format, TimeOut, null, null, ProductVersion.BaseGame, ProductVersion.BaseGame);

            NManager_Add
                (NotificationManager.sNotificationManager, styledNotification, format.mTNSCategory);
        }
        public static void NManager_Add(NotificationManager ths, Notification notification, NotificationManager.TNSCategory category)
        {
            if (ths == null)
            {
                notification.Dispose();
                return;
            }
            WindowBase win = ths.mTabGlows[category];
            ths.SetGlow(win, true);
            ths.mNotifications[category].Add(notification);
            ths.mTabs[category].Enabled = true;
            if (ths.mNotifications[ths.mCurrentCategory].Count == 0)
            {
                ths.mCurrentCategory = category;
            }
            if (category == ths.mCurrentCategory)
            {
                if (!ths.mOpen)
                {
                    ths.AddShowDelayTask();
                    return;
                }
                if (ths.mShowDelayTask == null)
                {
                    ths.SetGlow(win, false);
                }
                ths.SetGlow(ths.mMaxTextGlow, true);
                ths.UpdatePageInfo();
            }
            else if (!ths.mOpen)
            {
                ths.AddShowDelayTask();
            }
        }
        #endregion // metheds:Message

        #region metheds:SCO_*
        // fast code
        public static T[] SCO_GetObjects<T>() where T : class
        {
            return (ScriptCore.Queries.Query_GetObjects(typeof(T)) as T[] ?? new T[0]);
        }
        public static T[] SCO_GetObjectsRadius<T>(Vector3 center, float radius) where T : class
        {
            return (ScriptCore.Queries.Query_GetObjectsInRange(typeof(T), center, radius) as T[] ?? new T[0]);
        }
        public static T[] SCO_GetObjectsOnLot<T>(Lot targetLot) where T : class
        {
            if (targetLot == null)
                return new T[0];

            return (ScriptCore.Queries.Query_GetObjectsOnLot(typeof(T), targetLot.LotId, -1) as T[] ?? new T[0]);
        }
        public static T[] SCO_GetObjectsOnLotID<T>(ulong lotID, int roomID) where T : class
        {
            return (ScriptCore.Queries.Query_GetObjectsOnLot(typeof(T), lotID, roomID) as T[] ?? new T[0]);
        }
        #endregion // metheds:SCO_*

        #region metheds:Vector3_*
        public static
            bool Vector3_Is_NAN_Or_Zero(Vector3 pos)
        {
            float x, y, z;
            x = pos.x;
            y = pos.y;
            z = pos.z;
            return float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z) || (x == 0 && y == 0 && z == 0);
        }
        public static
            bool Vector3_IsUnSafe(Vector3 pos)
        {
            return Vector3_Is_NAN_Or_Zero(pos) || pos == Vector3.OutOfWorld || pos == Vector3.Invalid;
        }
        public static
            float Verctor3_GetDistance(Vector3 pos1, Vector3 pos2)
        {
            return (pos1 - pos2).Length();
        }
        #endregion // metheds:Vector3_*

        #region metheds:Household_*
        public static Household Household_FindID(ulong household_ID)
        {
            var hlist = Household.sHouseholdList;
            if (hlist != null)
            {
                foreach (Household household in hlist)
                {
                    if (household == null) continue;
                    if (household.mHouseholdId == household_ID)
                    {
                        return household;
                    }
                }
            }
            return null;
        }
        public static void Household_RemoveNullForHouseholdMembers(Household item)
        {
            if (item == null)
                throw new NullReferenceException();
            Sims3.Gameplay.CAS.Household.Members mem = item.mMembers;

            int checkloop;// = 10000;

            if (mem == null)
                item.mMembers = new Household.Members();

            else if (mem != null)
            {
                checkloop = 10000;
                if (mem.mAllSimDescriptions == null)
                    mem.mAllSimDescriptions = new List<SimDescription>();
                else
                {
                    while (mem.mAllSimDescriptions.Remove(null))
                    {
                        checkloop--;
                        if (checkloop < 0)
                        {
                            break;
                        }
                    }
                }

                checkloop = 10000;

                if (mem.mPetSimDescriptions == null)
                    mem.mPetSimDescriptions = new List<SimDescription>();
                else
                {
                    while (mem.mPetSimDescriptions.Remove(null))
                    {
                        checkloop--;
                        if (checkloop < 0)
                        {
                            break;
                        }
                    }
                }

                checkloop = 10000;

                if (mem.mSimDescriptions == null)
                    mem.mSimDescriptions = new List<SimDescription>();
                else
                {
                    while (mem.mSimDescriptions.Remove(null))
                    {
                        checkloop--;
                        if (checkloop < 0)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public static Household Household_FindTelemetryID(ulong household_TelemetryID)
        {
            var hlist = Household.sHouseholdList;
            if (hlist != null)
            {
                foreach (Household household in hlist)
                {
                    if (household == null) continue;
                    if (household.mHouseholdTelemetryId == household_TelemetryID)
                    {
                        return household;
                    }
                }
            }
            return null;
        }
        public static Household Household_FindIDSC(ulong household_ID)
        {
            foreach (Household household in SCO_GetObjects<Household>())
            {
                if (household == null) continue;
                if (household.mHouseholdId == household_ID)
                {
                    return household;
                }
            }
            return null;
        }
        public static Household Household_FindTelemetryIDSC(ulong household_TelemetryID)
        {
            foreach (Household household in SCO_GetObjects<Household>())
            {
                if (household == null) continue;
                if (household.mHouseholdTelemetryId == household_TelemetryID)
                {
                    return household;
                }
            }
            return null;
        }
        public static List<Sim> Household_GetAllActors(Household household)
        {
            if (household != null)
            {
                Household_RemoveNullForHouseholdMembers(household);
                var hm = household.mMembers;
                if (hm != null)
                {
                    List<Sim> vlist = new List<Sim>();
                    foreach (var item in hm.mAllSimDescriptions)
                    {
                        if (item == null) continue;
                        var simCreated = item.CreatedSim;
                        if (simCreated != null && simCreated.mSimDescription == item)
                        {
                            vlist.Add(simCreated);
                        }
                    }
                    return vlist;
                }
            }
            return null;
        }
        public static void Household_RefrashAllActors(Household _This)
        {
            if (_This == null) return;
            try
            {
                Household_RemoveNullForHouseholdMembers(_This);
                foreach (var item in Household_GetAllActors(_This))
                {
                    _This.OnMemberChanged(item.SimDescription, item);
                }
            }
            catch (ResetException)
            {
                throw;
            }
            catch { }
        }
        public static bool Household_IsSpecialHousehold(Household household) // fast code
        {
            if (household == null)
                return false;

            return household == Household.sNpcHousehold ||
                   household == Household.sPreviousTravelerHousehold ||
                   household == Household.sTouristHousehold ||
                   household == Household.sPetHousehold ||
                   household == Household.sAlienHousehold ||
                   household == Household.sMermaidHousehold;
        }
        public static bool Household_IsRole(Household home)
        {
            Household.Members mem = home.mMembers; // custom
            if (mem != null && mem.mAllSimDescriptions != null && mem.mAllSimDescriptions._items != null)
            {
                //foreach (SimDescription item in mem.mAllSimDescriptions._items)
                var a = mem.mAllSimDescriptions.Count;
                var t = mem.mAllSimDescriptions._items;
                for (int i = 0; i < t.Length; i++)
                {
                    if (i >= a)
                        break;

                    var item = t[i];
                    if (item != null && item.AssignedRole != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool Household_Add(Household targetHousehold, SimDescription simDesc, bool bKeepHousehold)
        {

            if (simDesc == null || targetHousehold == null || targetHousehold.mMembers == null)
                return false;

            Household simDescHousehold = simDesc.Household;
            if (simDescHousehold != null)
            {
                Household.Members mem = simDescHousehold.mMembers;
                if (mem != null)
                {
                    simDesc.mHousehold = null;
                    for (int i = 0; i < 10; i++)
                    {
                        if (mem.mAllSimDescriptions != null)
                            mem.mAllSimDescriptions.Remove(simDesc);
                        if (mem.mPetSimDescriptions != null)
                            mem.mPetSimDescriptions.Remove(simDesc);
                        if (mem.mSimDescriptions != null)
                            mem.mSimDescriptions.Remove(simDesc);
                    }
                    if (!bKeepHousehold && ((mem.mAllSimDescriptions == null || mem.mAllSimDescriptions.Count == 0) && !Household_IsSpecialHousehold(simDescHousehold)))
                        simDescHousehold.Destroy();
                }
            }

            Household_Remove(simDesc, bKeepHousehold);

            try
            {
                targetHousehold.Add(simDesc);
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch
            {
                Household.Members targetMembers = targetHousehold.mMembers;
                if (targetMembers != null)
                {
                    Household_Remove(simDesc, false);

                    if (!targetMembers.mAllSimDescriptions.Contains(simDesc))
                    {
                        targetMembers.mAllSimDescriptions.Add(simDesc);

                        if (simDesc.IsPet)
                            targetMembers.mPetSimDescriptions.Add(simDesc);
                        else
                            targetMembers.mSimDescriptions.Add(simDesc);
                    }
                }
                else return simDesc.Household == targetHousehold;

                simDesc.mHousehold = targetHousehold;
                return true;
            }
            return simDesc.Household == targetHousehold;
        }
        public static void Household_Remove(SimDescription simd, bool bKeepHousehold)
        {
            if (simd == null) return;
            try
            {
                List<Household> outHouseholdlist;
                if (FindAllHousehold(simd, true, out outHouseholdlist))
                {
                    foreach (var itemHousehold in outHouseholdlist.ToArray())
                    {
                        if (itemHousehold == null) continue;
                        Household.Members mem = itemHousehold.mMembers; 
                        if (mem == null)
                            continue;
                        for (int i = 0; i < 100; i++)
                        {
                            if (mem.mAllSimDescriptions != null)
                                mem.mAllSimDescriptions.Remove(simd);
                            if (mem.mPetSimDescriptions != null)
                                mem.mPetSimDescriptions.Remove(simd);
                            if (mem.mSimDescriptions != null)
                                mem.mSimDescriptions.Remove(simd);
                        }
                        simd.mHousehold = null;
                        if (!bKeepHousehold && ((mem.mAllSimDescriptions == null || mem.mAllSimDescriptions.Count == 0) && !Household_IsSpecialHousehold(itemHousehold)))
                        {
                            itemHousehold.Destroy();
                        }
                    }
                    simd.mHousehold = null;
                    outHouseholdlist.Clear();
                }
                else simd.mHousehold = null;
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch { }

        }
        #endregion // metheds:Household_*

        /// <summary>
        /// See mscorlib doc
        /// Debugger.Break()
        /// </summary>
        /// <code>
        /// static void Test_Debugger_Break() {
        ///     Comman.Debugger_Break();
        ///     // TS3.exe no symbol
        ///     ScriptCore.World.World_IsEditInGameFromWBModeImpl();
        ///     return;
        /// }
        /// </code>
       
        internal static void Debugger_Break() // Debugger App Only
        {
            if (Assembly.GetCallingAssembly() != Assembly.GetExecutingAssembly())
                throw new MethodAccessException();
            if (!DEBUG_Utils.IsDEBUG)
                return;
            
            // IL code
            // IL_0000: break
            // IL_0001: call bool ScriptCore.World::World_IsEditInGameFromWBModeImpl()
            // IL_0005: ret

            // x86 code
            // int3
            // call <ts3.World_IsEditInGameFromWBModeImpl>
            // ret

            // test mono mscorlib mod
            //System.Diagnostics.Debugger.Break();

            // mono interpreter
            try
            {
                while (true) {
                    // no code
                }
            }
            catch (Exception) // x32dbg/x64dbg throw Exception
            {
                // Debugger found
            }
        }

        public static bool FindAllHousehold(SimDescription simDescDontHaveHousehold, bool force, out List<Household> householdFound)
        {
            householdFound = null;
            if (simDescDontHaveHousehold != null && (force || simDescDontHaveHousehold.mHousehold == null))
            {
                foreach (var item in SCO_GetObjects<Household>())
                {
                    if (item == null)
                        continue;
                    Household.Members mem = item.mMembers;
                    if (mem == null)
                        continue;
                    List<SimDescription> householdMembers = mem.mAllSimDescriptions;
                    if (householdMembers == null) continue;


                    if (householdMembers.Contains(simDescDontHaveHousehold))
                        Lazy.Add<List<Household>, Household>(ref householdFound, item);

                }
                return householdFound != null;
            }
            return false;
        }

        // Funcs List Statc Fiaed
        public static bool IsNotPortInHouseBoat(Houseboat houseboat)
        {
            var lot = GetLot(houseboat.mHouseboatLotId);
            if (lot != null && !(lot is WorldLot) && !lot.IsPortLot())
            {
                return true;
            }
            return false;
        }

        public static bool FixUpPlumbBobSingletonNull()
        {
            if (PlumbBob.sSingleton == null)
            {
                try
                {
                    GlobalFunctions.CreateObjectOutOfWorld("PlumbBob");
                }
                catch (ResetException)
                {
                    throw;
                }
                catch { }

                if (Simulator.CheckYieldingContext(false))
                    Simulator.Sleep(0);
            }

            if (PlumbBob.sSingleton == null)
            {
                Comman.PrintMessage("Could not create PlumbBob!", false, 50);
                return false;
            }

            return true;
        }

        public static Household FindHouse(bool showno)
        {
            if (!Simulator.CheckYieldingContext(false)) 
                return null;
            string text;

            Sim hitsim = GetTargetGameObject() as Sim;
            if (hitsim != null && hitsim.Household != null)
            {
                text = StringInputDialog.Show("Find Household", "Household's Id", hitsim.Household.mHouseholdId.ToString(), 256, StringInputDialog.Validation.None);
            }
            else if (Household.ActiveHousehold != null)
            {
                text = StringInputDialog.Show("Find Household", "Household's Id", Household.ActiveHousehold.mHouseholdId.ToString(), 256, StringInputDialog.Validation.None);
            }
            else
            {
                text = StringInputDialog.Show("Find Household", "Household's Id", "1", 256, StringInputDialog.Validation.None);
            }
            if (string.IsNullOrEmpty(text)) return null;

            ulong newValue = 0;
            if (!ulong.TryParse(text, out newValue))
            {
                if (showno)
                    SimpleMessageDialog.Show("Sorry Failed.", "Invalid Text");

                return null;
            }
            else
            {
                foreach (Household item in SCO_GetObjects<Household>())
                {
                    if (item.mHouseholdId == newValue)
                    {
                        if (showno)
                        {
                            string asod = DEBUG_Utils.GetHouseholdInfo(item, false, null);
                            SimpleMessageDialog.Show("Find Household", asod);
                            LogUtils.WriteLog(asod, false, false);
                        }
                        return item; 
                    }
                }

            }
            try
            {
                //Household simat = Household.Find(newValue);

                foreach (Household item in Household.sHouseholdList)
                {

                    if (item != null && item.mHouseholdId == newValue)
                    {
                        if (showno)
                        {
                            string asod = DEBUG_Utils.GetHouseholdInfo(item, false, null);
                            SimpleMessageDialog.Show("Find Household\nFound Household.sHouseholdList", asod);
                            LogUtils.WriteLog(asod, false, false);
                        }
                        return item;
                    }
                }
            }
            catch (ResetException)
            { throw; }
            catch (Exception)
            { }


            try
            {
                if (PlumbBob.sCurrentNonNullHousehold != null && newValue == PlumbBob.sCurrentNonNullHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(PlumbBob.sCurrentNonNullHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == PlumbBob.sCurrentNonNullHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return PlumbBob.sCurrentNonNullHousehold;
                }

                if (Household.sNpcHousehold != null && newValue == Household.sNpcHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sNpcHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sNpcHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sNpcHousehold;
                }

                if (Household.sPetHousehold != null && newValue == Household.sPetHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sPetHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sPetHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sPetHousehold;
                }

                if (Household.sAlienHousehold != null && newValue == Household.sAlienHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sAlienHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sAlienHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sAlienHousehold;
                }

                if (Household.sMermaidHousehold != null && newValue == Household.sMermaidHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sMermaidHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sMermaidHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sMermaidHousehold;
                }


                if (Household.sPreviousTravelerHousehold != null && newValue == Household.sPreviousTravelerHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sPreviousTravelerHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sPreviousTravelerHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sPreviousTravelerHousehold;
                }

                if (Household.sServobotHousehold != null && newValue == Household.sServobotHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sServobotHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sServobotHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sServobotHousehold;
                }

                if (Household.sTouristHousehold != null && newValue == Household.sTouristHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(Household.sTouristHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == Household.sTouristHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return Household.sTouristHousehold;
                }
                if (TravelHousehold != null && newValue == TravelHousehold.mHouseholdId)
                {
                    if (showno)
                    {
                        string asod = DEBUG_Utils.GetHouseholdInfo(TravelHousehold, false, null);
                        SimpleMessageDialog.Show("Find Household\nFound Value == sTravelHousehold", asod);
                        LogUtils.WriteLog(asod, false, false);
                    }
                    return TravelHousehold;
                }
            }
            catch (ResetException)
            { throw; }
            catch (Exception)
            { }

            if (showno)
                PrintMessage("Could not find\nID: " + newValue, true, 100);

            return null;
        }

        public static Sim HouseholdMembersToSim(Household household, bool needTeenOrAbove, bool needForceSelectActor)
        {
            if (household == null)
                return null;

            Household.Members currentMembers = household.mMembers;
            if (currentMembers == null)
                return null;

            Sim newSim = null;

            foreach (SimDescription simDesc in currentMembers.mAllSimDescriptions)
            {
                var createdSim = simDesc.CreatedSim;
                if (createdSim != null && !simDesc.IsNeverSelectable)
                {
                    if (needTeenOrAbove && (simDesc.IsPet || !simDesc.TeenOrAbove))
                        continue;
                    newSim = createdSim;
                    break;
                }
            }

            if (newSim != null && needForceSelectActor)
                PlumbBob.ForceSelectActor(newSim);

            return newSim;
        }

        public static ObjectGuid CreateThread(ScriptExecuteType executeType, FuncTask_Function func)
        {
            return FuncTask.CreateTask(executeType, func);
        }

        public static
            bool IsSimDescAndCreateSimValid(SimDescription[] sim_list, Sim created_sim, out SimDescription out_sim_desc)
        {
            out_sim_desc = null;
            if (created_sim == null || created_sim.Proxy == null)
                return false;
            foreach (var item in sim_list)
            {
                if (item == null)
                    continue;
                if (item.mSim == created_sim)
                {
                    out_sim_desc = item;
                    return true;
                }
            }
            return false;
        }

        public static bool IsOnVacation()
        {
            return IsOnVacation(true) || IsOnVacation(false);
        }

        public static bool IsOnVacation(bool EP1_Worlds)
        {
            if (EP1_Worlds)
            {
                var currentWorld = ScriptCore.GameUtils.GameUtils_GetWorldName();
                if (currentWorld >= WorldName.Egypt)
                {
                    return currentWorld <= WorldName.France;
                }
                return false;
            }
            return global::Sims3.Gameplay.GameStates.IsOnVacation; // GameUtils.IsOnVacation();
        }

        public static Lot GetPostionTargetLot(Vector3 pos)
        {
            LotLocation LotLocation = default(LotLocation);
            ulong Location = World.GetLotLocation(pos, ref LotLocation);
            Lot TargetLot = GetLot(Location);
            if (TargetLot == null)
                return LotManager.GetWorldLot();
            return TargetLot;
        }

        public static Lot GetLot(ulong lotID)
        {
            if (lotID == 0)
                return null;
            if (lotID == ulong.MaxValue)
                return LotManager.sWorldLot;
            var proxy = ScriptCore.Simulator.Simulator_GetTaskImpl(lotID, false) as IScriptProxy;
            if (proxy != null)
            {
                var targetLot = proxy.Target;
                if (targetLot != null && targetLot is Lot)
                    return targetLot as Lot;
            }
            return LotManager.sLots != null ? LotManager.GetLot(lotID) : null;
        }

        public static T FindObjectDistance<T>(Vector3 actorPos, float maxDistance) where T : GameObject
        {
            T gameobjectResult = default(T);
            float f1 = float.MaxValue;
            foreach (T gameobj in SCO_GetObjects<T>())
            {
                float distance = (actorPos - ScriptCore.Objects.Objects_GetPosition(gameobj.ObjectId.mValue)).Length();
                if (distance < maxDistance && distance < f1)
                {
                    f1 = distance;
                    gameobjectResult = gameobj;
                }
            }
            return gameobjectResult;
        }
        public static T FindObjectDistanceIsBool<T>(Predicate<T> func, Vector3 actorPos, float maxDistance) where T : GameObject
        {
            if (func == null)
                throw new ArgumentNullException();
            T gameobjectResult = default(T);
            float f1 = float.MaxValue;
            foreach (T gameobj in SCO_GetObjects<T>())
            {
                float distance = (actorPos - ScriptCore.Objects.Objects_GetPosition(gameobj.ObjectId.mValue)).Length();
                if (distance < maxDistance && distance < f1)
                {
                    f1 = distance;
                    if (!func(gameobj))
                        continue;
                    gameobjectResult = gameobj;
                }
            }
            return gameobjectResult;
        }

        public static int GetIntDialog(string promptText)
        {
            Simulator.CheckYieldingContext(true);
            string str = StringInputDialog.Show(
                _thisAssembly._name,
                promptText ?? "Number Dialog",
                "0",
                StringInputDialog.Validation.None
            );

            if (string.IsNullOrEmpty(str))
                return -101;

            int ix; bool nofoundError = int.TryParse(
                str,
                out ix
            );

            if (!nofoundError)
                return -102;

            return ix;
        }

        public static string GetLastPackageName(bool needLotPackageFile)
        {
            if (BinModel.sBinModel == null || BinModel.sBinModel.mExportBin == null)
                return null;

            if (BinModel.sBinModel.mExportBin._items == null)
                BinModel.sBinModel.mExportBin = new List<ExportBinContents>();

            if (BinModel.sBinModel.mExportBin.Count == 0)
                return null;

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
            catch (Exception e)
            { DPrintException(null, e); }

            if (needLotPackageFile)
            {
                foreach (var item in exportBinlist._items)
                {
                    if (item == null)
                        continue;

                    if (item.Type == ExportBinType.Lot)
                    {
                        return item.mPackageName;
                    }
                }
            }
            else
            {
                foreach (var item in exportBinlist._items)
                {
                    if (item == null) 
                        continue;

                    if (item.Type == ExportBinType.HouseholdLot || item.Type == ExportBinType.Household)
                    {
                        return item.mPackageName;
                    }
                }
            }
            DEBUG_Utils.Print("GetLastPackageName: Could not find Package!");
            return null;
        }

        public static GameObject GetTargetGameObject()
        {
            if ((GameStates.IsInMainMenuState || GameStates.IsEditTownState || GameStates.IsCasState))
                return null;
            SceneMgrWindow t = UIManager.GetSceneWindow();
            if (t != null)
            {
                ScenePickArgs p = t.GetPickArgs();
                if (p.mObjectId != 0)
                {
                    IScriptProxy proxy = Simulator.GetProxy(new ObjectGuid(p.mObjectId));
                    if (proxy != null)
                    {
                        return proxy.Target as GameObject;
                    }
                }
            }
            return null;
        }

        public static ulong GetTargetObjectDontHaveScript()
        {
            if ((GameStates.IsInMainMenuState || GameStates.IsEditTownState || GameStates.IsCasState))
                return 0;

            SceneMgrWindow t = UIManager.GetSceneWindow();
            if (t != null)
            {
                return t.GetPickArgs().mObjectId;
            }
            return 0;
        }

        public static Lot GetTargetLot()
        {
            if ((GameStates.IsInMainMenuState || GameStates.IsEditTownState || GameStates.IsCasState))
                return null;
            SceneMgrWindow t = UIManager.GetSceneWindow();
            if (t != null)
            {
                ScenePickArgs p = t.GetPickArgs();
                if (p.mObjectId != 0)
                {
                    IScriptProxy proxy = Simulator.GetProxy(new ObjectGuid(p.mObjectId));
                    if (proxy != null && proxy.Target is Lot)
                    {
                        return proxy.Target as Lot;
                    }
                    else if (p.mObjectId != 0 && LotManager.sLots != null)
                    {
                        Lot lot;
                        LotManager.sLots.TryGetValue(p.mObjectId, out lot);
                        if (lot != null && lot.Proxy != null)
                        {
                            return lot;
                        }
                        else
                        {
                            ObjectGuid lotObjectGuid = LotManager.GetLotObjectGuid(p.mObjectId);
                            if (lotObjectGuid != ObjectGuid.InvalidObjectGuid)
                            {
                                return GameObject.GetObject<Lot>(lotObjectGuid);
                            }
                            return LotManager.GetLot(p.mObjectId);
                        }
                    }
                }
            }
            return null;
        }

        public static Lot GetPickMouseGameObjectLot()
        {
            var proxy = ScriptCore.Simulator.Simulator_GetTaskImpl(ScriptCore.Objects.Objects_GetLotId(GetTargetObjectDontHaveScript()), false) as IScriptProxy;
            if (proxy != null)
            {
                var targetLot = proxy.Target;
                if (targetLot != null && targetLot is Lot)
                    return targetLot as Lot;
            }
            return null;
        }
        public static Lot GetCameraTargetLot()
        {
            Vector3 campos = ScriptCore.CameraController.Camera_GetTarget();
            if (campos == Vector3.Empty)
                return null;

            LotLocation LotLocation = default(LotLocation);
            ulong Location = World.GetLotLocation(campos, ref LotLocation);
            if (Location == 0)
            {
                Location = World.GetLotLocation(World.SnapToFloor(campos), ref LotLocation);
            }
            Lot TargetLot = GetLot(Location);
            if (TargetLot == null)
                return LotManager.GetWorldLot();
            return TargetLot;
        }
        public static Household GetTargetSimHousehold()
        {
            var sim = GetTargetGameObject() as Sim;
            if (sim != null && sim.mSimDescription != null)
            {
                return sim.Household;
            }
            return null;
        }
        public static Lot GetCameraTargetLotOrTargetLot()
        {
            Vector3 campos = ScriptCore.CameraController.Camera_GetTarget();
            if (campos == Vector3.Empty) 
                return null;

            LotLocation LotLocation = default(LotLocation);
            ulong Location = World.GetLotLocation(campos, ref LotLocation);
            if (Location == 0)
            {
                Location = World.GetLotLocation(World.SnapToFloor(campos), ref LotLocation);
                if (Location == 0)
                Location = World.GetLotLocation(ScriptCore.CameraController.Camera_GetLODInterestPosition(), ref LotLocation);
            }
            Lot TargetLot = GetLot(Location);
            if (TargetLot == null || TargetLot is WorldLot)
            {
                //foreach (var item in SCO_GetObjectsRadius<GameObject>(campos, 2f))
                //{
                //    if (item == null) continue;
                //    Lot lot = null;
                //    var houseboat = item as Houseboat;
                //    if (houseboat != null)
                //    {
                //        lot = GetLot(houseboat.mHouseboatLotId);
                //        if (lot != null && !(lot is WorldLot))
                //            break;
                //    }
                //    lot = GO_GetLot(item);
                //    if (lot == null  || lot.IsPortLot() || lot is WorldLot)
                //        continue;
                //    TargetLot = lot;
                //    break;
                //}

                //foreach (var houseboat in SCO_GetObjectsRadius<Houseboat>(campos, 3f))
                //{
                //    if (houseboat != null)
                //    {
                //        var lot = GetLot(houseboat.mHouseboatLotId);
                //        if (lot != null && !(lot is WorldLot) && !lot.IsPortLot())
                //        {
                //            TargetLot = lot;
                //            break;
                //        }
                //    }
                //}

                //var gameObj = GetTargetGameObject();
                //if (gameObj != null && !(gameObj is Lot))
                //{
                //    TargetLot = gameObj.mLotCurrent;
                //}

                Houseboat houseboat = FindObjectDistanceIsBool<Houseboat>(IsNotPortInHouseBoat, campos, 5f);
                if (houseboat != null)
                {
                    TargetLot = GetLot(houseboat.mHouseboatLotId);
                }

                if (TargetLot == null || TargetLot is WorldLot)
                {
                    TargetLot = GetPickMouseGameObjectLot();
                }
                if (TargetLot == null || TargetLot is WorldLot)
                {
                    TargetLot = GetTargetLot();
                }
                //if (TargetLot == null || TargetLot is WorldLot)
                //{
                //    TargetLot = LotManager.sActiveLot;
                //}
                if (TargetLot == null)
                {
                    TargetLot = LotManager.sWorldLot;
                }
            }
            //    return GetTargetLot() ?? LotManager.ActiveLot ?? LotManager.GetWorldLot();
            return TargetLot;
        }

        public static List<SimDescription> GetAllSimDescription()
        {
            List<SimDescription> list = new List<SimDescription>(1000);
            foreach (Household item in SCO_GetObjects<Household>())
            {
                if (item == null || item.mMembers == null || item.mMembers.mAllSimDescriptions == null) 
                    continue;
                foreach (var member in item.mMembers.mAllSimDescriptions._items)
                {
                    if (member == null || list.Contains(member)) 
                        continue;
                    list.Add(member);
                }
            } 
            foreach (Urnstone item in SCO_GetObjects<Urnstone>())
            {
                if (item == null)
                    continue;
                var simd = item.DeadSimsDescription;
                if (simd == null || list.Contains(simd)) 
                    continue;
                list.Add(simd);
            }
            foreach (Sim item in SCO_GetObjects<Sim>())
            {
                if (item == null)
                    continue;
                var simd = item.mSimDescription;
                if (simd == null || list.Contains(simd)) 
                    continue;
                list.Add(simd);
            }
            return list;
        }

        public static ScriptExecuteType GetCurrentExecuteType()
        {
            var iTask = ScriptCore.Simulator.Simulator_GetTaskImpl(ScriptCore.Simulator.Simulator_GetCurrentTaskImpl(), false); // Simulator.GetTask(Simulator.CurrentTask);
            if (iTask == null)
            {
                return ScriptExecuteType.None;
            }

            var proxy = iTask as ScriptCore.ScriptProxy;
            if (proxy != null)
            {
                return proxy.mExecuteType;
            }

            var task = iTask as Task;
            if (task != null)
            {
                return task.ExecuteType;
            }

            return iTask.ExecuteType;
        }

        public static string GetNowTimeToStr()
        {
            return DateTime.Now.ToString().Replace('/', '-').Replace(':', '_');
        }

        public static Household TravelHousehold
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

        public static void SleepTask(uint tickCount)
        {
            Simulator.CheckYieldingContext(true); // Simulator.CheckYieldingContext();
            if (tickCount == 0)
            {
                Simulator.Sleep(0);
            }
            else
            {
                uint t = tickCount == 1 ? 1 : tickCount - 1;
                for (uint i = 0; i < t; i++)
                {
                    Simulator.Sleep(0);
                }
                Simulator.Sleep(0);
            }
        }

        public static bool PrintException(string Msg, Exception ex)
        {
            bool ExceptionIsNotNull = ex != null;
            if (!ExceptionIsNotNull && string.IsNullOrEmpty(Msg))
                return false;

            if (ExceptionIsNotNull && ex.StackTrace == null)
            {
                try
                {
                    throw ex;
                }
                catch (Exception tex)
                {
                    ex = tex;
                }
            }

            bool Done = false;
            bool MessageIsNullOrEmpty = string.IsNullOrEmpty(Msg);

            if (!ExceptionIsNotNull)
                Done = LogUtils.WriteLog(Msg, false, false);
            else
                Done = LogUtils.WriteLog(Msg + "\n" + "Script Error:" + LogUtils.NewLine + "\n" + ex.ToString(), false, false);

            if (NotificationManager.Instance != null)
            {
                if (MessageIsNullOrEmpty)
                    PrintMessage(_thisAssembly._name + LogUtils.NewLine + "Script Error is Found No: " + LogUtils.sLogEnumerator, true, 250);
                else if (ExceptionIsNotNull)
                    PrintMessage(_thisAssembly._name + LogUtils.NewLine + "Script Error\n" + "No:" + LogUtils.sLogEnumerator + "\n" + Msg, true, 250);
            }
            else
            {
                if (MessageIsNullOrEmpty)
                    LogUtils.sPendingNotifications.Add("Script Error is Found No: " + LogUtils.sLogEnumerator);
                else if (ExceptionIsNotNull)
                    LogUtils.sPendingNotifications.Add("Script Error\n" + "No:" + LogUtils.sLogEnumerator + "\n" + Msg);
            }
            return Done;
        }

        public static bool DPrintException(string Msg, Exception ex)
        {
            if (DEBUG_Utils.IsDEBUG)
            {
                return PrintException(Msg, ex);
            }
            else
            {
                // TODO: add your code
                return false;
            }
        }

        public static void OnWorldLoadFinished()
        {
            FuncTask.CreateTask(delegate
            {
                while (NotificationManager.sNotificationManager == null)
                {
                    Simulator.Sleep(0);
                }
                LogUtils.TriggerPendingNotifications();
            });
        }

        public static Sim GetRandomSim(Predicate<Sim> customTest)
        {
            var simList = SCO_GetObjects<Sim>();
            var count = simList.Length;
            var simListTemp = count > 0 ? new List<Sim>(count) : new List<Sim>();

            foreach (var item in simList)
            {
                if (item == null) continue;
                if (customTest == null || customTest(item))
                    simListTemp.Add(item);
            }

            if (simListTemp.Count > 0)
            {
                try
                {
                    return RandomUtil.GetRandomObjectFromList<Sim>(simListTemp);
                }
                finally
                {
                    simListTemp.Clear();
                }
            }

            return null;
        }

        public static GObject GetRandomGameObject<GObject>(Predicate<GObject> customTest) where GObject : class
        {
            List<GObject> listTemp = new List<GObject>();
            bool cacheISNull = customTest == null;
            foreach (var item in SCO_GetObjects<GObject>())
            {
                if (item == null) continue;
                if (cacheISNull || customTest(item))
                    listTemp.Add(item);
            }
            if (listTemp.Count > 0)
            {
                try
                {
                    return RandomUtil.GetRandomObjectFromList<GObject>(listTemp, new Random());
                }
                finally
                {
                    listTemp.Clear();
                }
            }
            return null;
        }

        public static ObjectGuid FindTaskPro(string FindCallStack, string FindClassName, out ITask OutTask)
        {
            OutTask = null;
            if (ScriptCore.Simulator.mObjHash == null)
                return ObjectGuid.InvalidObjectGuid;

            bool haveFindClassName = FindClassName != null && FindClassName != "";
            bool haveFindCallStack = FindCallStack != null && FindCallStack != "";

            if (!haveFindClassName && !haveFindCallStack)
                return ObjectGuid.InvalidObjectGuid;

            //Dictionary<ulong, ITask> dictionary = new Dictionary<ulong, ITask>(ScriptCore.Simulator.mObjHash);
            foreach (var item in ScriptCore.Simulator.mObjHash)
            {
                if (item.Value == null)
                    continue;
                if (haveFindClassName && item.Value.ClassName == FindClassName)
                {
                    OutTask = item.Value;
                    return item.Value.ObjectId;
                }
                if (haveFindCallStack)
                {
                    string st = DEBUG_Utils.GetObjectStackTrace(item.Key);

                    if (st == null || st == "<no call stack>")
                        continue;
                    if (st.Contains(FindCallStack))
                    {
                        OutTask = item.Value;
                        return new ObjectGuid(item.Key);
                    }
                }
            }
            return ObjectGuid.InvalidObjectGuid;
        }

        public static void RemoveHandleFromAlarmGlobal(ref AlarmHandle handle)
        {
            var am = AlarmManager.Global;
            if (am != null)
                am.RemoveAlarm(handle);
            handle = new AlarmHandle();
        }

        public static void RemoveTaskFromSimulator(ref ObjectGuid taskHandle)
        {
#if TEST_FASTCALL_NATIVE
            ScriptCore.Simulator.Simulator_DestroyObjectImpl(taskHandle.Value);
#else
            Simulator.DestroyObject(taskHandle);
#endif
            taskHandle = new ObjectGuid(0);
        }

    }
}
