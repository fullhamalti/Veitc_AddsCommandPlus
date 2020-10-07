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

namespace Veitc.AddsCommandPlus.Proxies
{
    public class HouseholdContentsProxy : IExportableContent, IMetaTagExporter
    {
        HouseholdContents mContents;

        public HouseholdContentsProxy()
        {
            mContents = new HouseholdContents();
        }
        public HouseholdContentsProxy(Household household)
        {
            mContents = new HouseholdContents(household);
        }

        public HouseholdContents Contents
        {
            get { return mContents; }
        }

        public Household Household
        {
            get { return mContents.Household; }
        }


        //public List<SimDescription> SimMemberHousehold = null;


        public static bool HouseholdContents_ExportContent(HouseholdContents This, ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamWriter writer)
        {
            if (This == null) throw new NullReferenceException();
            IPropertyStreamWriter writer2 = writer.CreateChild(2221750924u);
            try
            {
                if (This.mHousehold == null || !Household_ExportContent(This.mHousehold, resKeyTable, objIdTable, writer2)) return false;
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException)
            {
                throw;
            }
            catch { }

            writer.CommitChild();
            writer.WriteInt32(1758660904u, This.mInventories.Length);
            int[] array = new int[This.mInventories.Length];
            int num = 0;
            ulong[] array2 = This.mInventories;
            foreach (ulong num2 in array2)
            {
                uint type = (uint)((num2 != 0) ? (-1260555925) : 0);
                ResourceKey newKey = new ResourceKey(num2, type, 0u);
                array[num++] = resKeyTable.AddKey(newKey);
            }
            writer.WriteInt32(2746246615u, array);
            return true;
        }






        //public static bool br0 = false;
        public static bool HMembers_ExportContent(Household.Members _this, ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamWriter writer)
        {
            if (_this == null)
                throw new NullReferenceException();
            writer.WriteInt32(3921240069u, _this.Count);
            uint num = 0u;
            foreach (SimDescription allSimDescription in _this.AllSimDescriptionList)
            {
                if (string.IsNullOrEmpty(allSimDescription.mFirstName) && string.IsNullOrEmpty(allSimDescription.mLastName))
                    continue;
                if (!Comman.SD_OutfitsIsValid2(allSimDescription, true))
                    continue;
                try // fix
                {
                    bool needFix = false;
                    if (allSimDescription.mTraitManager == null)
                    {
                        allSimDescription.mTraitManager = new TraitManager();
                        allSimDescription.mTraitManager.mSimDescription = allSimDescription;
                        needFix = true;
                    }
                    if (allSimDescription.SkillManager == null)
                    {
                        allSimDescription.SkillManager = new Sims3.Gameplay.Skills.SkillManager(allSimDescription);
                        allSimDescription.SkillManager.mSimDescription = allSimDescription;
                        needFix = true;
                    }
                    if (allSimDescription.mGenealogy == null)
                    {
                        allSimDescription.mGenealogy = new Genealogy(allSimDescription);
                        allSimDescription.mGenealogy.mSim = allSimDescription;
                        needFix = true;
                    }
                    if (needFix)
                        allSimDescription.Fixup();
                    
                }
                catch (Exception)
                {
                    //allSimDescription.Dispose();
                    //continue; check game crash
                }
                uint keyData = num++;
                IPropertyStreamWriter writer2 = writer.CreateChild(keyData);
                // custom  14:49 12/10/2019
                try
                {
                    if (allSimDescription.ExportContent(resKeyTable, objIdTable, writer2))
                    {
                        writer.CommitChild();
                    }
                    else writer.CancelChild(keyData);
                }
                catch (StackOverflowException) { throw; }
                catch (ResetException) { throw; }
                catch (Exception)
                {
                    writer.CancelChild(keyData);
                }

            }
            return true;
        }


        public static bool HMembers_ImportContent(Household.Members _this, Household _thisH, ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamReader reader)
        {
            if (_this == null)
                throw new NullReferenceException();

            _this.mAllSimDescriptions.Clear();
            _this.mPetSimDescriptions.Clear();
            _this.mSimDescriptions.Clear();

            try
            {
                Household.sRemapSimDescriptionIdsOnImport = !Household.IsTravelImport;

                int memberCount;

                if (!reader.ReadInt32(3921240069u, out memberCount, int.MaxValue) || memberCount < 0 || memberCount == int.MaxValue) // loop
                    return false;

                int checkloop = 0; // Fix Loop 12:48 20/05/2019

                for (uint i = 0; i < memberCount; i++)
                {
                    IPropertyStreamReader child = reader.GetChild(i);

                    if (child == null) // fix bug EA Failed
                        continue; 

                    SimDescription simDescription = new SimDescription();

                    try
                    {
                        if (simDescription.ImportContent(resKeyTable, objIdTable, child) && Comman.SD_OutfitsIsValid(simDescription))
                        {
                            //_this.Add(simDescription, simDescription.CreatedSim);
                            simDescription.mHousehold = _thisH;
                        }
                        else
                        {
                            Comman.Household_Remove(simDescription, true);
                            simDescription.Dispose();
                            goto checkLoopG;
                        }
                    }
                    catch (StackOverflowException) { throw; }
                    catch (ResetException) { throw; }
                    catch (Exception)
                    {
                        //Comman.Household_Remove(simDescription, true);
                        //simDescription.Dispose();

                        //continue;
                    }

                    try
                    {
                        Comman.Household_Add(_thisH, simDescription, true);
                        simDescription.Fixup();
                    }
                    catch (StackOverflowException) { throw; }
                    catch (ResetException) { throw; }
                    catch (Exception) { }

                checkLoopG:
                    if (checkloop++ > 650)
                        break;
                }
            }
            finally
            {
                Household.sRemapSimDescriptionIdsOnImport = false;
            }
            return true;
        }

        public static bool Household_ExportRelationships(Household.Members mMembers ,ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamWriter writer)
        {
            uint value = 0;
            if (mMembers.mAllSimDescriptions.Count == 0)
                goto w;

            var p = mMembers.mAllSimDescriptions.ToArray();
            var valueMax = p.Length * p.Length - p.Length;

            try
            {
                foreach (SimDescription actorsd in p)
                {
                    if (value <= valueMax)
                        break;

                    foreach (SimDescription targetsd in p)
                    {
                        if (value <= valueMax)
                            break;

                        if (actorsd.mSimDescriptionId != targetsd.mSimDescriptionId)
                        {
                            Relationship relationship = Relationship.Get(actorsd, targetsd, false);
                            if (relationship != null)
                            {
                                var propertyStreamWriter = writer.CreateChild(value++);
                                propertyStreamWriter.WriteUint64(3385230853u, actorsd.mSimDescriptionId);
                                propertyStreamWriter.WriteUint64(3205132738u, targetsd.mSimDescriptionId);
                                relationship.ExportContent(resKeyTable, objIdTable, propertyStreamWriter);
                                writer.CommitChild();
                            }
                            else valueMax--;
                        }
                        else valueMax--;
                    }
                }
            }
            catch (Exception) { }

            w:writer.WriteUint32(308660384u, value);
            return true;
        }

        public static bool Household_ImportRelationships(Household _this, ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamReader reader)
        {
            if (Relationship.sAllRelationships == null) 
                return false;

            uint ccount = 0;
            reader.ReadUint32(308660384u, out ccount, 0);

            if (ccount == 0) 
                return true;
            if (ccount < 0 || ccount == uint.MaxValue) 
                return false;

            for (uint i = 0; i < ccount; i++)
            {
                var child = reader.GetChild(i);

                ulong importID;
                ulong importID2;

                if (child == null || !child.ReadUint64(3385230853u, out importID, 0) || !child.ReadUint64(3205132738u, out importID2, 0))
                    continue;

                ulong simDescID = importID;
                ulong simDescID2 = importID2;

                if (Household.sOldIdToNewSimDescriptionMap != null)
                {
                    SimDescription outSimDesc;
                    if (Household.sOldIdToNewSimDescriptionMap.TryGetValue(importID, out outSimDesc))
                        simDescID = outSimDesc.SimDescriptionId;
                    if (Household.sOldIdToNewSimDescriptionMap.TryGetValue(importID2, out outSimDesc))
                        simDescID2 = outSimDesc.SimDescriptionId;
                }

                var relationship = Relationship.Get (
                    _this.mMembers.GetSimDescriptionFromId(simDescID),
                    _this.mMembers.GetSimDescriptionFromId(simDescID2),
                    true
                );

                if (relationship == null) // if SimDesc is invalid or SimDesc ID == 0. Should EA Bug
                    continue;

                relationship.ImportContent(resKeyTable, objIdTable, child);
            }
            return true;
        }


        public static bool Household_ExportContent(Household _this, ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamWriter writer)
        {
            if (_this == null)
                throw new NullReferenceException();
            if (_this.mMembers == null) return false;
            string name = _this.mName;
            string bioText = _this.mBioText;
            //if (!DownloadContent.IsDevBuild())
            //{
            //    name = _this.Name;
            //    bioText = _this.BioText;
            //}
            writer.WriteString(1357107804u, name);
            writer.WriteInt32(867787827u, _this.mFamilyFunds);
            writer.WriteString(3647669240u, bioText);
            writer.WriteInt64(1669643236u, DateTime.Now.ToBinary());
            writer.WriteBool(571722353u, _this.mbLifetimeHappinessNotificationShown);
            writer.WriteInt32(145720536u, _this.mUnpaidBills);
            if (_this.LotHome != null)
            {
                writer.WriteInt32(3338514733u, _this.LotHome.Cost);
            }
            IPropertyStreamWriter writer2 = writer.CreateChild(2449095374u);
            HMembers_ExportContent(_this.mMembers, resKeyTable, objIdTable, writer2);
            writer.CommitChild();
            writer2 = writer.CreateChild(2706303414u);
            //try
            //{
            //    _this.ExportRelationships(resKeyTable, objIdTable, writer2);
            //}
            //catch (StackOverflowException) { throw; }
            //catch (ResetException)
            //{
            //    throw;
            //}
            //catch { }
            Household_ExportRelationships(_this.mMembers, resKeyTable, objIdTable, writer2);
            writer.CommitChild();
            writer.WriteInt32(141731785u, _this.mAncientCoinCount);
            writer.WriteUint64(1193523264u, (ulong)_this.UniqueObjectsObtained);
            int count = _this.mKeystonePanelsUsed.Count;
            writer.WriteInt32(129814813u, count);
            uint num = 0u;
            foreach (WorldName key in _this.mKeystonePanelsUsed.Keys)
            {
                writer.WriteInt32(146578516 + num, (int)key);
                List<string> list = new List<string>(_this.mKeystonePanelsUsed[key]);
                writer.WriteString(163369191 + num, list.ToArray());
                num++;
            }
            ulong[] array = new ulong[_this.mCompletedHouseholdOpportunities.Count];
            _this.mCompletedHouseholdOpportunities.Keys.CopyTo(array, 0);
            writer.WriteUint64(149611345u, array);
            writer.WriteUint32(149611346u, _this.mMoneySaved);
            ulong[] array2 = new ulong[_this.mWardrobeCasParts.Count];
            _this.mWardrobeCasParts.CopyTo(array2, 0);
            writer.WriteUint64(153835052u, array2);
            writer.WriteUint64(156333552u, (ulong)_this.mServiceUniforms);
            return true;
        }

        public static bool Household_ImportContent(Household _this, ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamReader reader)
        {
            if (_this == null)
                throw new NullReferenceException("if (this == null)");

            reader.ReadString(1357107804u, out _this.mName, "");
            reader.ReadInt32(867787827u, out _this.mFamilyFunds, 0);
            reader.ReadString(3647669240u, out _this.mBioText, "");
            reader.ReadInt32(145720536u, out _this.mUnpaidBills, 0);
            reader.ReadInt32(3338514733u, out _this.mLotHomeWorth, 0);

            long exportTime = DateTime.Now.ToBinary();
            reader.ReadInt64(1669643236u, out exportTime, DateTime.Now.ToBinary());
            _this.mExportTime = DateTime.FromBinary(exportTime);

            reader.ReadBool(571722353u, out _this.mbLifetimeHappinessNotificationShown, false);
            IPropertyStreamReader child = reader.GetChild(2449095374u);

            if (child != null)
            {
                HMembers_ImportContent(_this.mMembers, _this, resKeyTable, objIdTable, child);
            }

            try
            {
                foreach (SimDescription allSimDescription in _this.mMembers.AllSimDescriptionList.ToArray())
                {
                    allSimDescription.OnHouseholdChanged(_this, true);
                    allSimDescription.CareerManager.OnLoadFixup();
                    Sims3.Gameplay.Careers.Occupation occupation = allSimDescription.CareerManager.Occupation;
                    if (occupation != null)
                    {
                        occupation.RepairLocation();
                    }
                }

                foreach (SimDescription allSimDescription2 in _this.mMembers.AllSimDescriptionList.ToArray())
                {
                    if (allSimDescription2.Pregnancy != null)
                    {
                        SimDescription value2;
                        if (Household.sOldIdToNewSimDescriptionMap != null && Household.sOldIdToNewSimDescriptionMap.TryGetValue(allSimDescription2.Pregnancy.DadDescriptionId, out value2))
                        {
                            allSimDescription2.Pregnancy.DadDescriptionId = value2.SimDescriptionId;
                        }
                        else if (!Household.IsTravelImport && _this.mMembers.GetSimDescriptionFromId(allSimDescription2.Pregnancy.DadDescriptionId) == null)
                        {
                            allSimDescription2.Pregnancy.DadDescriptionId = 0uL;
                        }
                    }
                }

                child = reader.GetChild(2706303414u);
                if (child != null)
                {
                   //_this.ImportRelationships(resKeyTable, objIdTable, child);
                    Household_ImportRelationships(_this, resKeyTable, objIdTable, child);
                }
            }
            catch (StackOverflowException) { throw; }
            catch (ResetException) { throw; }
            catch (Exception)
            { }


            reader.ReadInt32(141731785u, out _this.mAncientCoinCount, 0);

            ulong uniqueObjectsObtained;
            reader.ReadUint64(1193523264u, out uniqueObjectsObtained, 0uL);
            _this.UniqueObjectsObtained = (UniqueObjectKey)uniqueObjectsObtained;

            _this.mKeystonePanelsUsed = new PairedListDictionary<WorldName, List<string>>();

            int KeystonePanelsUsedCount;
            reader.ReadInt32(129814813u, out KeystonePanelsUsedCount, 0);
            int checkInfLoop = 0; // Fix Loop 12:48 20/05/2019

            for (uint num = 0u; num < KeystonePanelsUsedCount; num++)
            {
                int keystonePanelsUsed;
                reader.ReadInt32(146578516 + num, out keystonePanelsUsed, -1);
                WorldName key = (WorldName)keystonePanelsUsed;

                string[] keystonePanelsList;
                reader.ReadString(163369191 + num, out keystonePanelsList);
                _this.mKeystonePanelsUsed[key] = new List<string>(keystonePanelsList ?? new string[0]);

                if (checkInfLoop++ > 500)
                    break;
            }

            _this.mCompletedHouseholdOpportunities.Clear();

            ulong[] completedHouseholdOpportunities;
            reader.ReadUint64(149611345u, out completedHouseholdOpportunities);

            foreach (ulong key in completedHouseholdOpportunities)
            {
                _this.mCompletedHouseholdOpportunities.Add(key, true);
            }

            reader.ReadUint32(149611346u, out _this.mMoneySaved);
            if (_this.mMoneySaved == null || _this.mMoneySaved.Length != 3)
            {
                _this.mMoneySaved = new uint[3];
            }

            ulong[] wardrobeCasParts;
            reader.ReadUint64(153835052u, out wardrobeCasParts);
            foreach (ulong item in wardrobeCasParts)
            {
                _this.mWardrobeCasParts.Add(item);
            }

            ulong serviceUniform;
            reader.ReadUint64(156333552u, out serviceUniform, 0uL);
            _this.AddServiceUniforms((ServiceType)serviceUniform);

            return true;
        }

        public static string VExportHousehold(Bin ths, Household household, bool includeLotContents, bool isMovingPacked, bool noAllResetToSim, bool noThum)
        {
            if (ths == null) throw new NullReferenceException();
            if (household == null) throw new ArgumentNullException("household");
            try
            {
                string createdPackageFile = null;
                if (GameUtils.IsInstalled(ProductVersion.EP4))
                {
                    OccultImaginaryFriend.ForceHouseholdImaginaryFriendsBackToInventory(household);
                }

                if (!noAllResetToSim)
                {
                    foreach (Sim sim in household.AllActors)
                    {
                        sim.SetObjectToReset();
                    }
                }

                if (Simulator.CheckYieldingContext(false))
                    Simulator.Sleep(0);

                ThumbnailSizeMask sizeMask;
                if (includeLotContents)
                {
                    Lot lotHome = household.LotHome;
                    if (lotHome != null)
                    {
                        int householdFunds = household.FamilyFunds;
                        int lotHomeLotWorth = World.GetEmptyLotWorth(lotHome.LotId) + ((int)World.GetLotAdditionalPropertyValue(lotHome.LotId));

                        household.SetFamilyFunds(householdFunds + lotHomeLotWorth, false);

                        EditTownModel.SendObjectsToProperLot(lotHome);

                        ulong contentID = XStoreLotContents(lotHome, lotHome.LotId);//DownloadContent.StoreLotContents(lotHome, lotHome.LotId);
                        if (contentID != 0)
                        {
                            ThumbnailHelper.GenerateLotThumbnailSet(lotHome.LotId, contentID, ThumbnailSizeMask.ExtraLarge);
                            ThumbnailHelper.GenerateLotThumbnail(lotHome.LotId, contentID, 0, ThumbnailSizeMask.Large | ThumbnailSizeMask.Medium);
                            sizeMask = ThumbnailSizeMask.Large | ThumbnailSizeMask.ExtraLarge | ThumbnailSizeMask.Medium;
                            ThumbnailManager.GenerateHouseholdThumbnail(household.HouseholdId, contentID, sizeMask);

                            if (!noThum && household.AllSimDescriptions.Count < 12)
                                ths.GenerateSimThumbnails(household, contentID, ThumbnailSizeMask.Large | ThumbnailSizeMask.ExtraLarge);

                            HouseholdContentsProxy contents = new HouseholdContentsProxy(household);

                            if (//DownloadContent.StoreHouseholdContents
                                XStoreHouseholdContents
                                (contents, contentID))
                            {
                                createdPackageFile = ScriptCore.DownloadContent.DownloadContent_ExportLotContentsToExportBinImpl(contentID);//DownloadContent.ExportLotContentsToExportBin(contentID);
                            }

                            ThumbnailManager.InvalidateLotThumbnails(lotHome.LotId, contentID, ThumbnailSizeMask.ExtraLarge);
                            ThumbnailManager.InvalidateLotThumbnailsForGroup(lotHome.LotId, contentID, ThumbnailSizeMask.Medium, 0);
                            ThumbnailManager.InvalidateHouseholdThumbnail(household.HouseholdId, contentID, sizeMask);

                            try
                            {
                                ths.InvalidateSimThumbnails(household, contentID);
                            }
                            catch (Exception)
                            { }

                        }
                        household.SetFamilyFunds(householdFunds, false);
                        return createdPackageFile;
                    }
                    //return createdPackageFile;
                }

                int familyFunds = household.FamilyFunds;
                int realEstateFunds = 0;
                if (household.RealEstateManager != null)
                {
                    foreach (IPropertyData data in household.RealEstateManager.AllProperties)
                    {
                        realEstateFunds += data.TotalValue;
                    }
                }

                if (household.LotHome != null)
                {
                    int lotWorth = 0;
                    if (isMovingPacked)
                    {
                        lotWorth = World.GetUnfurnishedLotWorth(household.LotHome.LotId) + realEstateFunds;
                    }
                    else
                    {
                        lotWorth = World.GetLotWorth(household.LotHome.LotId) + realEstateFunds;
                    }

                    household.SetFamilyFunds(household.FamilyFunds + lotWorth, false);
                }

                if (household.FamilyFunds < 20000)
                {
                    household.SetFamilyFunds(20000, false);
                }

                ulong gGUID = DownloadContent.GenerateGUID();
                HouseholdContentsProxy householdContents = new HouseholdContentsProxy(household);
                householdContents.Contents.ContentId = gGUID;

                sizeMask = ThumbnailSizeMask.Large | ThumbnailSizeMask.ExtraLarge | ThumbnailSizeMask.Medium;

                if (!noThum && household.AllSimDescriptions.Count < 12)
                {
                    ThumbnailManager.GenerateHouseholdThumbnail(household.HouseholdId, gGUID, sizeMask);
                    ths.GenerateSimThumbnails(household, gGUID, ThumbnailSizeMask.Large | ThumbnailSizeMask.ExtraLarge);
                }

                if (//DownloadContent.StoreHouseholdContents
                    XStoreHouseholdContents
                    (householdContents, gGUID))
                {
                    createdPackageFile = ScriptCore.DownloadContent.DownloadContent_ExportLotContentsToExportBinImpl(gGUID); //DownloadContent.ExportLotContentsToExportBin(gGUID);
                }

                ThumbnailManager.InvalidateHouseholdThumbnail(household.HouseholdId, gGUID, sizeMask);

                try
                {
                    ths.InvalidateSimThumbnails(household, gGUID);
                }
                catch (Exception)
                { }

                household.SetFamilyFunds(familyFunds, false);
                return createdPackageFile;
            }
            catch (Exception)
            { return null; }
        }


        public bool ExportContent(ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamWriter writer)
        {
            try
            {
                return HouseholdContents_ExportContent(mContents, resKeyTable, objIdTable, writer);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExportMetaTags(IPropertyStreamWriter writer)
        {
            try
            {
                return mContents.ExportMetaTags(writer);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ImportContent(ResKeyTable resKeyTable, ObjectIdTable objIdTable, IPropertyStreamReader reader)
        {
            try
            {
                int num;
                IPropertyStreamReader child = reader.GetChild(0x846d3a8c);
                if (child != null)
                {
                    mContents.mHousehold = Household.Create();

                    //if (!Household.ImportContent(resKeyTable, objIdTable, child))
                    if (!Household_ImportContent(Household, resKeyTable, objIdTable, child))
                    {
                        return false;
                    }
                }

                reader.ReadInt32(0x68d30928, out num, 0x2);
                mContents.mInventories = new ulong[num];
                int[] values = new int[num];
                reader.ReadInt32(0xa3b065d7, out values);
                for (int i = 0x0; i < values.Length; i++)
                {
                    ResourceKey key = resKeyTable[values[i]];
                    mContents.mInventories[i] = key.InstanceId;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static ulong XStoreLotContents(IExportableContent lot, ulong lotId)
        {
            ObjectIdTable idTable = null;
            ResKeyTable keyTable;
            System.IO.MemoryStream stream;
            if (!NativeExportManagedContent(lot, out keyTable, ref idTable, out stream))
                return 0;
            return ScriptCore.DownloadContent.DownloadContent_StoreLotContentsImpl(lotId, keyTable.ByteArray, idTable.ObjectIdArray, stream.ToArray(), DateTime.Now.ToBinary());
        }

        public static bool XStoreHouseholdContents(IExportableContent householdContents, ulong lotId)
        {
            ObjectIdTable idTable = null;
            ResKeyTable keyTable;
            System.IO.MemoryStream stream;

            return NativeExportManagedContent(householdContents, out keyTable, ref idTable, out stream) &&
                ScriptCore.DownloadContent.DownloadContent_StoreHouseholdContentsImpl
                (lotId, keyTable.ByteArray, idTable.ObjectIdArray, stream.ToArray());
        }



        public static bool NativeExportManagedContent(IExportableContent content, out ResKeyTable keyTable, ref ObjectIdTable idTable, out System.IO.MemoryStream stream)
        {
            keyTable = new ResKeyTable();
            if (idTable == null)
                idTable = new ObjectIdTable();

            stream = new System.IO.MemoryStream();
            var binaryWriter = new System.IO.BinaryWriter(stream);
            PropertyStreamWriter propertyStreamWriter = new PropertyStreamWriter();

            if (!propertyStreamWriter.Open(binaryWriter))
                return false;

            try
            {
                return content.ExportContent(keyTable, idTable, propertyStreamWriter) && propertyStreamWriter.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                binaryWriter.Close();
            }
        }


        public static bool NativeImportManagedContent(ulong packageID, IExportableContent content)
        {
            byte[] pKeyTable = null;
            ulong[] pIdTable = null;
            byte[] pScriptData = null;

            if (!ScriptCore.DownloadContent.DownloadContent_ImportHouseholdContentsImpl(packageID, ref pKeyTable, ref pIdTable, ref pScriptData))
                return false;

            if (pScriptData == null) // found bug
                throw new NotSupportedException("ScriptData is null. Should EA Bug");

            var resKeyTable = new ResKeyTable(pKeyTable);
            var objIdTable = new ObjectIdTable(pIdTable);
            var input = new System.IO.MemoryStream(pScriptData);
            var stream = new System.IO.BinaryReader(input);
            var propertyStreamReader = new PropertyStreamReader();

            return propertyStreamReader.Open(stream) && content.ImportContent(resKeyTable, objIdTable, propertyStreamReader);
        }


        public static HouseholdContentsProxy Import(string packageName)
        {
            if (packageName == null || packageName.length == 0) // if (string.IsNullOrEmpty(packageName))
                return null;

            ulong packageID = ScriptCore.DownloadContent.DownloadContent_ImportHouseholdContentsFromExportBinImpl(packageName); // DownloadContent.ImportHouseholdContentsFromExportBin(packageName);
            if (packageID != 0)
            {
                HouseholdContentsProxy householdContents = new HouseholdContentsProxy();
                //if (DownloadContent.ImportHouseholdContents(householdContents, packageID))
                if (NativeImportManagedContent(packageID, householdContents))
                {
                    householdContents.mContents.ContentId = packageID;
                    return householdContents;
                }
            }
            return null;
        }
    }
}
