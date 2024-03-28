namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    internal class CompareProgramsData
    {
        private ProgramCr programOne;
        private ProgramCr programTwo;

        public CompareProgramsData()
        {
            GrandTotal = new RecordRealtyObject();
            CountRows = 0;
        }

        public IWindsorContainer Container { get; set; }

        public RecordRealtyObject GrandTotal { get; set; }

        public int CountRows { get; set; }

        public List<RecordForGroup> GetData(IWindsorContainer container, ProgramCr progrOne, ProgramCr progTwo, long[] municipalities, long[] sources)
        {
            Container = container;
            GrandTotal = new RecordRealtyObject();

            this.programOne = progrOne;
            this.programTwo = progTwo;

            var servContragentContact = Container.Resolve<IDomainService<ContragentContact>>();
           
            var ojectCrProgramOneList = this.GetRecordsForObjectCr(municipalities, programOne.Id);
            var ojectCrProgramTwoList = this.GetRecordsForObjectCr(municipalities, programTwo.Id);

            var leadersDict = servContragentContact.GetAll()
             .Where(x => x.Position.Code == "1")
             .Select(x => new { x.Contragent.Id, x.Surname })
             .AsEnumerable()
             .GroupBy(x => x.Id)
             .ToDictionary(x => x.Key, x => x.First())
             .ToDictionary(x => x.Key, x => x.Value.Surname);

            this.FillInfoForManOrg(ojectCrProgramOneList, programOne, leadersDict);
            this.FillInfoForManOrg(ojectCrProgramTwoList, programTwo, leadersDict);

            var result = new List<RecordForProgram>();
            result.AddRange(ojectCrProgramOneList);
            result.AddRange(ojectCrProgramTwoList);
            var ids = result.Select(x => x.ObjectCrId).Distinct().ToArray();

            var financeSourceAndWorksDict = GetFinanceSourceAndWorks(ids, sources);

            // Группировка по районy
            var groupByMunicipality = result.Where(x => financeSourceAndWorksDict.ContainsKey(x.RealityObjectId))
                               .OrderBy(x => x.Municipality)
                               .GroupBy(x => x.MunicipalityId)
                               .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Address).ToList());
            
            var recordsForGroup = new List<RecordForGroup>();
            foreach (var rec in groupByMunicipality)
            {
                var group = rec.Value[0].GroupMunicipality;
                var municipality = rec.Value[0].Municipality;

                var recordForGroup = group != string.Empty ? recordsForGroup.Find(x => x.Group == group) : null;
                if (recordForGroup == null)
                {
                    recordForGroup = new RecordForGroup(group);
                    recordsForGroup.Add(recordForGroup);
                }

                var recordMunicipality = new RecordMunicipality(municipality);
                recordForGroup.Municipalities.Add(municipality, recordMunicipality);

                var groupByRealtyObject = rec.Value.GroupBy(x => x.RealityObjectId).ToDictionary(x => x.Key, x => x.ToList());

                foreach (var recordsRealtyObj in groupByRealtyObject)
                {
                    var recRealtyObject = new RecordRealtyObject
                    {
                        RecordOne = recordsRealtyObj.Value.FirstOrDefault(x => x.ProgramCrId == this.programOne.Id),
                        RecordTwo = recordsRealtyObj.Value.FirstOrDefault(x => x.ProgramCrId == this.programTwo.Id)
                    };

                    recordMunicipality.RealtyObjects.Add(recordsRealtyObj.Key, recRealtyObject);

                    var financeSourceList = financeSourceAndWorksDict.ContainsKey(recordsRealtyObj.Key)
                                                ? financeSourceAndWorksDict[recordsRealtyObj.Key]
                                                : new List<TypeWorkCrAndFinanceSourceProxy>();

                    var groupByTypeFinance = financeSourceList.GroupBy(x => x.TypeFinanceGroup);

                    foreach (var recFinance in groupByTypeFinance)
                    {
                        var recordGroupFinance = new RecordGroupFinance(recFinance.Key.GetEnumMeta().Display);
                        recRealtyObject.GroupFinances.Add(recordGroupFinance.GroupSource, recordGroupFinance);

                        AddSource(recordGroupFinance, recFinance.ToList());
                    }

                    recordMunicipality.Total.Add(recRealtyObject);
                    recordForGroup.Grand.Add(recRealtyObject);
                    GrandTotal.Add(recRealtyObject);
                }
            }

            return recordsForGroup;
        }

        private Dictionary<int, List<FinanceSourceResourceProxy>> GetFinanceSource(long[] ids)
        {
             var start = 1000;
             var tmpIds = ids.Take(1000).ToArray();

            var servFinanceSourceResource = Container.Resolve<IDomainService<FinanceSourceResource>>();
             var resultsOfRecourses = servFinanceSourceResource.GetAll()
                 .Where(x => tmpIds.Contains(x.ObjectCr.Id))
                 .Select(
                     x =>
                     new FinanceSourceResourceProxy
                     {
                         ObjectCrId = x.ObjectCr.Id,
                         TypeFinanceGroup = x.FinanceSource.TypeFinanceGroup,
                         FinanceSourceId = x.FinanceSource.Id,
                         FinanceSource = x.FinanceSource.Name,
                         ProgramCrId = x.ObjectCr.ProgramCr.Id,
                         FundResource = x.FundResource.HasValue ? x.FundResource.Value : 0M,
                         BudgetSubject = x.BudgetSubject.HasValue ? x.BudgetSubject.Value : 0M,
                         BudgetMu = x.BudgetMu.HasValue ? x.BudgetMu.Value : 0M,
                         OwnerResource = x.OwnerResource.HasValue ? x.OwnerResource.Value : 0M,
                     })
                 .AsEnumerable()
                 .Distinct()
                 .ToList();

             while (start < ids.Length)
             {
                 var tmpObjectsCrIds = ids.Skip(start).Take(1000).ToArray();

                 resultsOfRecourses.AddRange(servFinanceSourceResource
                     .GetAll()
                     .Where(x => tmpObjectsCrIds.Contains(x.ObjectCr.Id))
                     .Select(
                     x =>
                     new FinanceSourceResourceProxy
                     {
                         ObjectCrId = x.ObjectCr.Id,
                         RealtyObjectId = x.ObjectCr.RealityObject.Id,
                         FinanceSourceId = x.FinanceSource.Id,
                         FinanceSource = x.FinanceSource.Name,
                         ProgramCrId = x.ObjectCr.ProgramCr.Id,
                         FundResource = x.FundResource.HasValue ? x.FundResource.Value : 0M,
                         BudgetSubject = x.BudgetSubject.HasValue ? x.BudgetSubject.Value : 0M,
                         BudgetMu = x.BudgetMu.HasValue ? x.BudgetMu.Value : 0M,
                         OwnerResource = x.OwnerResource.HasValue ? x.OwnerResource.Value : 0M,
                     })
                 .AsEnumerable()
                 .Distinct()
                 .ToList());

                 start += 999;
             }

             return resultsOfRecourses.GroupBy(x => string.Format("{0}_{1}", x.ObjectCrId, x.FinanceSourceId).GetHashCode()).ToDictionary(x => x.Key, x => x.ToList());
        }

        private Dictionary<long, List<TypeWorkCrAndFinanceSourceProxy>> GetFinanceSourceAndWorks(long[] ids, long[] sources)
        {
            var servTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();

            var worksAndFinanceSourceList = new List<TypeWorkCrAndFinanceSourceProxy>();

            for (var i = 0; i < ids.Length; i += 1000)
            {
                var takeCount = ids.Length - i < 1000 ? ids.Length - i : 1000;
                var tempList = ids.Skip(i).Take(takeCount).ToArray();

                worksAndFinanceSourceList.AddRange(servTypeWorkCr.GetAll()
                             .Where(x => tempList.Contains(x.ObjectCr.Id))
                             .WhereIf(sources.Length > 0, x => sources.Contains(x.FinanceSource.Id))
                             .Select(x => new TypeWorkCrAndFinanceSourceProxy
                             {
                                 ObjectCrId = x.ObjectCr.Id,
                                 RealtyObjectId = x.ObjectCr.RealityObject.Id,
                                 ProgramCrId = x.ObjectCr.ProgramCr.Id,
                                 FinanceSourceId = x.FinanceSource != null ? x.FinanceSource.Id.ToStr() : string.Empty,
                                 FinanceSource = x.FinanceSource != null ? x.FinanceSource.Name : string.Empty,
                                 Volume = x.Volume,
                                 Sum = x.Sum,
                                 Code = x.Work != null ? x.Work.Code : string.Empty,
                                 TypeFinanceGroup = x.FinanceSource.TypeFinanceGroup
                             })
                             .ToList());
            }

            var financeSourceDict = GetFinanceSource(ids);

            foreach (var rec in worksAndFinanceSourceList)
            {
                var key = string.Format("{0}_{1}", rec.ObjectCrId, rec.FinanceSourceId).GetHashCode();
                var financeSource = financeSourceDict.ContainsKey(key) ? financeSourceDict[key] : null;
                if (financeSource == null)
                {
                    continue;
                }

                rec.FundResource = financeSource.Sum(x => x.FundResource);
                rec.BudgetSubject = financeSource.Sum(x => x.BudgetSubject);
                rec.BudgetMu = financeSource.Sum(x => x.BudgetMu);
                rec.OwnerResource = financeSource.Sum(x => x.OwnerResource);
            }

            return worksAndFinanceSourceList.GroupBy(x => x.RealtyObjectId).ToDictionary(x => x.Key, x => x.OrderBy(y => y.FinanceSource).ToList());
        }

        private void FillInfoForManOrg(List<RecordForProgram> records, ProgramCr program, Dictionary<long, string> leadersContactDict)
        {
            var servManOrgContractRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var ids = records.Select(x => x.RealityObjectId).ToArray();
            var start = 1000;
            var tmpIds = ids.Take(1000).ToArray();

             var recordsManOrg = servManOrgContractRealityObject.GetAll()
                 .Where(x => tmpIds.Contains(x.RealityObject.Id) && x.ManOrgContract.ManagingOrganization != null)
                 .Select(x =>
                            new
                            {
                                x.ManOrgContract.StartDate,
                                x.ManOrgContract.EndDate,
                                RealityObjectId = x.RealityObject.Id,
                                x.ManOrgContract.ManagingOrganization.TypeManagement,
                                ContragentId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                                ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                                x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                                x.ManOrgContract.ManagingOrganization.Contragent.Kpp,
                                x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                                x.ManOrgContract.ManagingOrganization.Contragent.Email,
                                x.ManOrgContract.ManagingOrganization.Contragent.FiasJuridicalAddress
                            })
                 .AsEnumerable()
                .Where(x =>
                ((x.StartDate.HasValue && x.StartDate.Value >= program.Period.DateStart)
                 && ((program.Period.DateEnd.HasValue && program.Period.DateEnd.Value >= x.StartDate.Value) || !program.Period.DateEnd.HasValue))
                ||
                (((x.StartDate.HasValue && program.Period.DateStart >= x.StartDate.Value) || !x.StartDate.HasValue)
                    &&
               ((x.StartDate.HasValue && program.Period.DateEnd.HasValue && x.EndDate.HasValue && x.EndDate.Value >= program.Period.DateStart) || !x.EndDate.HasValue)))
                        .Select(x =>
                            new
                            {
                                x.RealityObjectId,
                                x.TypeManagement,
                                x.ContragentId,
                                x.ContragentName,
                                x.Inn,
                                x.Kpp,
                                x.Phone,
                                x.Email,
                                PlaceName = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.PlaceName : string.Empty,
                                PostCode = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.PostCode : string.Empty,
                                StreetName = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.StreetName : string.Empty,
                                House = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.House : string.Empty,
                                Flat = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.Flat : string.Empty
                            })
                            .ToList();

            while (start < ids.Length)
            {
                tmpIds = ids.Skip(start).Take(1000).ToArray();
                recordsManOrg.AddRange(servManOrgContractRealityObject.GetAll()
                 .Where(x => tmpIds.Contains(x.RealityObject.Id) && x.ManOrgContract.ManagingOrganization != null)
                 .Select(x =>
                            new
                            {
                                x.ManOrgContract.StartDate,
                                x.ManOrgContract.EndDate,
                                RealityObjectId = x.RealityObject.Id,
                                x.ManOrgContract.ManagingOrganization.TypeManagement,
                                ContragentId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                                ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                                x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                                x.ManOrgContract.ManagingOrganization.Contragent.Kpp,
                                x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                                x.ManOrgContract.ManagingOrganization.Contragent.Email,
                                x.ManOrgContract.ManagingOrganization.Contragent.FiasJuridicalAddress
                            })
                 .AsEnumerable()
                .Where(x =>
                ((x.StartDate.HasValue && x.StartDate.Value >= program.Period.DateStart)
                 && ((program.Period.DateEnd.HasValue && program.Period.DateEnd.Value >= x.StartDate.Value) || !program.Period.DateEnd.HasValue))
                ||
                (((x.StartDate.HasValue && program.Period.DateStart >= x.StartDate.Value) || !x.StartDate.HasValue)
                    &&
               ((x.StartDate.HasValue && program.Period.DateEnd.HasValue && x.EndDate.HasValue && x.EndDate.Value >= program.Period.DateStart) || !x.EndDate.HasValue)))
                        .Select(x =>
                            new
                            {
                                x.RealityObjectId,
                                x.TypeManagement,
                                x.ContragentId,
                                x.ContragentName,
                                x.Inn,
                                x.Kpp,
                                x.Phone,
                                x.Email,
                                PlaceName = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.PlaceName : string.Empty,
                                PostCode = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.PostCode : string.Empty,
                                StreetName = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.StreetName : string.Empty,
                                House = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.House : string.Empty,
                                Flat = x.FiasJuridicalAddress != null ? x.FiasJuridicalAddress.Flat : string.Empty
                            })
                            .ToList());
                start += 1000;
            }

            var manOrgDict = recordsManOrg.GroupBy(x => x.RealityObjectId).ToDictionary(x => x.Key, x => x.First());

            foreach (var rec in records)
            {
                var realityObjectId = rec.RealityObjectId;
                if (!manOrgDict.ContainsKey(realityObjectId)) continue;

                var infoManOrg = manOrgDict[realityObjectId];
                rec.TypeManagement = infoManOrg.TypeManagement;
                rec.ManagementOrganization = infoManOrg.ContragentName;
                rec.Inn = infoManOrg.Inn;
                rec.Kpp = infoManOrg.Kpp;
                rec.Phone = infoManOrg.Phone;
                rec.Email = infoManOrg.Email;
                rec.PlaceAddressManOrg = infoManOrg.PlaceName;
                rec.PostCode = infoManOrg.PostCode;
                rec.Street = infoManOrg.StreetName;
                rec.House = infoManOrg.House;
                rec.Flat = infoManOrg.Flat;
                rec.Leader = leadersContactDict.ContainsKey(infoManOrg.ContragentId)
                                 ? leadersContactDict[infoManOrg.ContragentId]
                                 : string.Empty;
            }
        }

        private List<RecordForProgram> GetRecordsForObjectCr(long[] municipalities, long programId)
        {
            var servObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            return servObjectCr.GetAll()
                               //.Where(x => x.RealityObject.Id == 9166)
                               .WhereIf(municipalities.Length > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                               .Where(x => x.ProgramCr.Id == programId)
                               .Select(x => new RecordForProgram
                                                {
                                                    ObjectCrId = x.Id,
                                                    GroupMunicipality = x.RealityObject.Municipality.Group,
                                                    MunicipalityId = x.RealityObject.Municipality.Id,
                                                    Municipality = x.RealityObject.Municipality.Name,
                                                    ProgramCrId = x.ProgramCr.Id,
                                                    RealityObjectId = x.RealityObject.Id,
                                                    Address = x.RealityObject.Address,
                                                    MaximumFloors = x.RealityObject.MaximumFloors.HasValue ? x.RealityObject.MaximumFloors.Value.ToStr() : string.Empty,
                                                    SeriesHome = x.RealityObject.SeriesHome,
                                                    CapitalGroup = x.RealityObject.CapitalGroup.Name,
                                                    AreaMkd = x.RealityObject.AreaMkd.HasValue ? x.RealityObject.AreaMkd.Value : 0M,
                                                    AreaLiving = x.RealityObject.AreaLiving.HasValue ? x.RealityObject.AreaLiving.Value : 0M,
                                                    AreaLivingNotLivingMkd = x.RealityObject.AreaLivingNotLivingMkd.HasValue ? x.RealityObject.AreaLivingNotLivingMkd.Value : 0M,
                                                    AreaLivingOwned = x.RealityObject.AreaLivingOwned.HasValue ? x.RealityObject.AreaLivingOwned.Value : 0M,
                                                    NumberApartments = x.RealityObject.NumberApartments.HasValue ? x.RealityObject.NumberApartments.Value : 0,
                                                    NumberLiving = x.RealityObject.NumberLiving.HasValue ? x.RealityObject.NumberLiving.Value : 0,
                                                    WallMaterial = x.RealityObject.WallMaterial.Name,
                                                    RoofingMaterial = x.RealityObject.RoofingMaterial.Name,
                                                    YearCommissioning = x.RealityObject.DateCommissioning.HasValue ? x.RealityObject.DateCommissioning.Value.Year.ToStr() : string.Empty,
                                                    PhysicalWear = x.RealityObject.PhysicalWear.HasValue ? x.RealityObject.PhysicalWear.Value : 0M,
                                                    YearLastOverhaul = x.RealityObject.DateLastOverhaul.HasValue ? x.RealityObject.DateLastOverhaul.Value.Year.ToStr() : string.Empty,
                                                    SumDevolopmentPsd = x.SumDevolopmentPsd.HasValue ? x.SumDevolopmentPsd.Value : 0M,
                                                    SumTehInspection = x.SumTehInspection.HasValue ? x.SumTehInspection.Value : 0M,
                                                    SumSmrApproved = x.SumSmrApproved.HasValue ? x.SumSmrApproved.Value : 0M,
                                                })
                                           .ToList();
        }

        private void AddSource(RecordGroupFinance recordGroupFinance, IEnumerable<TypeWorkCrAndFinanceSourceProxy> record)
        {
            foreach (var recSource in record.GroupBy(x => x.FinanceSource))
            {
                var recordForSource = new RecordForSource(recSource.Key);

                foreach (var rec in recSource)
                {
                    if (rec.ProgramCrId == programOne.Id)
                    {
                        recordForSource.AddWorksOne(rec.Code.ToInt(), new RecordWorks(rec.Volume, rec.Sum));
                        recordForSource.BudgetOne = new Budget(rec.FundResource, rec.BudgetSubject, rec.BudgetMu, rec.OwnerResource);
                    }
                    else if (rec.ProgramCrId == programTwo.Id)
                    {
                        recordForSource.AddWorksTwo(rec.Code.ToInt(), new RecordWorks(rec.Volume, rec.Sum));
                        recordForSource.BudgetTwo = new Budget(rec.FundResource, rec.BudgetSubject, rec.BudgetMu, rec.OwnerResource);
                    }
                }

                recordGroupFinance.Sources.Add(recSource.Key.ToStr(), recordForSource);
            }
        }
    }
}