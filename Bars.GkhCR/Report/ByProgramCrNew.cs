// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByProgramCrNew.cs" company="BarsGroup">
//   BarsGroup
// </copyright>
// <summary>
//   Отчет По программам КР (новый)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет По программам КР (новый)
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ByProgramCrNew : BasePrintForm
    {
        #region параметры

        /// <summary>
        /// The program id.
        /// </summary>
        private List<long> programCrIds = new List<long>();

        /// <summary>
        /// The report date.
        /// </summary>
        private DateTime reportDate = DateTime.MinValue;

        /// <summary>
        /// The municipality ids.
        /// </summary>
        private List<long> municipalityIds = new List<long>();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ByProgramCrNew"/> class.
        /// </summary>
        public ByProgramCrNew() : base(new ReportTemplateBinary(Properties.Resources.ByProgramCrNew))
        {
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "По программам КР (новый)";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "По программам КР (новый)";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        /// <summary>
        /// Gets the parameters controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ByProgramCrNew";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.ByProgramCrNew";
            }
        }

        /// <summary>
        /// The set user params.
        /// </summary>
        /// <param name="baseParams">
        /// The base params.
        /// </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.ParseIds(this.programCrIds, baseParams.Params["programCrIds"].ToString());
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.ParseIds(this.municipalityIds, baseParams.Params["municipalityIds"].ToString());
        }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var repManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                                        .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= this.reportDate)
                                        .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= this.reportDate);

            if (this.programCrIds.Count == 0)
            {
                throw new Exception("Не указан параметр \"Программы кап.ремонта\"");
            }

            if (this.reportDate == DateTime.MinValue || this.reportDate == null)
            {
                throw new Exception("Не указан параметр \"Дата отчета\"");
            }

            var serviceProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>().GetAll()
                .Where(x => this.programCrIds.Contains(x.Id));

            var programIds = serviceProgramCr.Select(x => x.Id);

            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                       .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                       .Select(x => new { id = x.Id, name = x.Name })
                       .ToDictionary(x => x.id, x => x.name);

            var objectCrQuery = this.Container.Resolve<IDomainService<ObjectCr>>()
                                         .GetAll()
                                         .Where(x => programIds.Contains(x.ProgramCr.Id))
                                         .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var objectCrIds = objectCrQuery.Select(x => x.Id);
            var realObjIds = objectCrQuery.Select(x => x.RealityObject.Id);

            var financeSourceResource = this.Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll()
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new
                    {
                        objectCrId = x.ObjectCr.Id,
                        municipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                        finSourceName = x.FinanceSource.Name,
                        budgetMu = x.BudgetMu,
                        budgetSubject = x.BudgetSubject,
                        ownerResource = x.OwnerResource,
                        fundResource = x.FundResource
                    })
                .AsEnumerable()
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.objectCrId).ToDictionary(y => y.Key, y => y.ToList()));
            
            var objectCrByMunisipalityDict = objectCrQuery.Select(x => new
                {
                    muId = x.RealityObject.Municipality.Id,
                    muName = x.RealityObject.Municipality.Name ?? string.Empty,
                    realObjId = x.RealityObject.Id,
                    objectCrId = x.Id,
                    programId = x.ProgramCr.Id,
                    programName = x.ProgramCr.Name,
                    adress = x.RealityObject.Address,
                    floors = x.RealityObject.MaximumFloors,
                    series = x.RealityObject.SeriesHome,
                    capitalGroup = x.RealityObject.CapitalGroup.Name,
                    areaMkd = x.RealityObject.AreaMkd,
                    areaLivingNotLivingMkd = x.RealityObject.AreaLivingNotLivingMkd,
                    areaLiving = x.RealityObject.AreaLiving,
                    areaLivingOwned = x.RealityObject.AreaLivingOwned,
                    numberApartments = x.RealityObject.NumberApartments,
                    numberLiving = x.RealityObject.NumberLiving,
                    wallMaterial = x.RealityObject.WallMaterial.Name,
                    roofingMaterial = x.RealityObject.RoofingMaterial.Name,
                    dateCommissioning = x.RealityObject.DateCommissioning,
                    physicalWear = x.RealityObject.PhysicalWear,
                    dateLastOverhaul = x.RealityObject.DateLastOverhaul,
					typeProject = x.RealityObject.TypeProject
                })
                .AsEnumerable()
                .OrderBy(x => x.muName)
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.adress).ToList());

            var typeWorks = this.Container.Resolve<IDomainService<TypeWorkCr>>()
                .GetAll()
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Where(x => programIds.Contains(x.ObjectCr.ProgramCr.Id))

                // .Where(x => x.FinanceSource.TypeFinanceGroup == TypeFinanceGroup.ProgramCr) // TODO уточнить
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    realObjId = x.ObjectCr.RealityObject.Id,

                    typeWorkId = x.Id,
                    workCode = x.Work.Code,
                    consistent185Fz = x.Work.Consistent185Fz,
                                
                    volume = x.Volume,
                    sum = x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key, 
                    x => x.GroupBy(y => y.realObjId)
                            .ToDictionary(
                                y => y.Key, 
                                y => y.GroupBy(z => z.workCode)
                                    .ToDictionary(
                                        z => z.Key, 
                                        z => new
                                                    {
                                                        sum = z.Sum(p => p.sum), 
                                                        volume = z.Sum(p => p.volume),
                                                        Consistent185Fz = z.All(p => p.consistent185Fz)
                                                    })));

            var manOrgContractIdsQuery = repManOrgContractRealityObject
                    .Where(x => realObjIds.Contains(x.RealityObject.Municipality.Id))
                    .Select(x => x.Id);

            var contractRelationDict = this.Container.Resolve<IDomainService<ManOrgContractRelation>>().GetAll()
                .Where(x => manOrgContractIdsQuery.Contains(x.Parent.Id))
                .Where(x => x.Children.StartDate <= this.reportDate)
                .Where(x => x.Children.EndDate == null || x.Children.EndDate >= this.reportDate)
                .Select(x => new
                {
                    x.Parent.Id,
                    ManagingOrganizationId = x.Children.ManagingOrganization.Id,
                    ManagingOrganizationName = x.Children.ManagingOrganization.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                        x => x.Key, 
                        x => new ManOrgContractProxy
                                {
                                    ManageOrgId = x.First().ManagingOrganizationId,
                                    Name = x.First().ManagingOrganizationName
                                });

             // УО дома
            var realityObjectManOrgDict = repManOrgContractRealityObject
                    .Where(x => realObjIds.Contains(x.RealityObject.Id))
                    .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                            manOrgContractId = x.Id,
                            x.ManOrgContract.TypeContractManOrgRealObj, 
                            moId = x.ManOrgContract.ManagingOrganization != null ? x.ManOrgContract.ManagingOrganization.Id : -1
                        })
                    .AsEnumerable().GroupBy(x => x.roId).ToDictionary(
                        x => x.Key,
                        y =>
                            {
                               var moContractList = y.Select(x => new ManOrgContractProxy
                                    {
                                        ManageOrgId = x.moId, 
                                        TypeContractManOrgRealObj = x.TypeContractManOrgRealObj
                                    })
                                 .ToList();

                               return this.GetManOrg(moContractList, contractRelationDict);
                            });

            var managingOrganization = this.Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                .Where(x => x.Contragent != null)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id));

            var managementOrganizationDict = managingOrganization
                .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                {
                    manageOrgId = x.Id,
                    contragentId = x.Contragent.Id,
                    uoName = x.Contragent.Name,
                    typeManagement = x.TypeManagement.GetEnumMeta().Display,

                    jurFiasPostCode = x.Contragent.FiasJuridicalAddress != null ? x.Contragent.FiasJuridicalAddress.PostCode : string.Empty,
                    jurStreet = x.Contragent.FiasJuridicalAddress != null ? x.Contragent.FiasJuridicalAddress.StreetName : string.Empty,
                    jurHouse = x.Contragent.FiasJuridicalAddress != null ? x.Contragent.FiasJuridicalAddress.House : string.Empty,

                    inn = x.Contragent.Inn,
                    kpp = x.Contragent.Kpp,

                    shortName = x.Contragent.ShortName,
                    factAddress = x.Contragent.FactAddress,
                    jurAddress = x.Contragent.JuridicalAddress,
                    organizationForm = x.Contragent.OrganizationForm,
                    ogrn = x.Contragent.Ogrn,
                    phone = x.Contragent.Phone,
                    email = x.Contragent.Email
                })
                .ToDictionary(x => x.manageOrgId);

            var contragentId = managingOrganization.Select(x => x.Contragent.Id).Distinct();
            var contragentContact = this.Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                .Where(x => contragentId.Contains(x.Contragent.Id))
                .Where(x => x.Position.Code == "1" || x.Position.Code == "4")
                .Select(x => new
                        {
                            contagentId = x.Contragent.Id,
                            fio = string.Format("{0} {1} {2}", x.Surname, x.Name, x.Patronymic)
                        })
                .AsEnumerable()
                .GroupBy(x => x.contagentId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.fio).FirstOrDefault());
            
            var counter = 1;
            string empty = string.Empty;
            const string Format = "0.00";
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            var sectionMo = section.ДобавитьСекцию("СекцияМо");

            foreach (var objectCrGroup in objectCrByMunisipalityDict)
            {
                section.ДобавитьСтроку();
                section["район"] = municipalityDict[objectCrGroup.Key];

                decimal? sumMo36 = 0, sumMo37 = 0, sumMo80 = 0;

                foreach (var objectCr in objectCrGroup.Value.OrderBy(x => x.adress))
                {
                    sectionMo.ДобавитьСтроку();

                    sectionMo["col1"] = counter;
                    sectionMo["col2"] = string.Empty;
                    sectionMo["col3"] = objectCr.adress;

                    if (realityObjectManOrgDict.ContainsKey(objectCr.realObjId))
                    {
                        var manageOrgId = realityObjectManOrgDict[objectCr.realObjId].ManageOrgId;

                        var manageOrg = managementOrganizationDict.ContainsKey(manageOrgId) ? managementOrganizationDict[manageOrgId] : null;

                        if (manageOrg != null)
                        {
                            sectionMo["col4"] = manageOrg.typeManagement;
                            sectionMo["col5"] = manageOrg.uoName;
                            sectionMo["col6"] = manageOrg.jurAddress;
                            sectionMo["col7"] = manageOrg.jurFiasPostCode;
                            sectionMo["col8"] = manageOrg.jurStreet;
                            sectionMo["col9"] = manageOrg.jurHouse;
                            sectionMo["col10"] = string.Empty;
                            sectionMo["col11"] = manageOrg.inn;
                            sectionMo["col12"] = manageOrg.kpp;
                            if (contragentContact.ContainsKey(manageOrg.contragentId))
                            {
                                sectionMo["col13"] = contragentContact[manageOrg.contragentId];
                            }

                            sectionMo["col14"] = manageOrg.phone;
                            sectionMo["col15"] = manageOrg.email;
                        }
                    }

                    sectionMo["col16"] = objectCr.floors;
                    sectionMo["col17"] = objectCr.typeProject != null ? objectCr.typeProject.Name : "";
                    sectionMo["col18"] = objectCr.capitalGroup;
                    sectionMo["col19"] = objectCr.areaMkd;
                    sectionMo["col20"] = objectCr.areaLivingNotLivingMkd;
                    sectionMo["col21"] = objectCr.areaLiving;
                    sectionMo["col22"] = objectCr.areaLivingOwned;
                    sectionMo["col23"] = objectCr.numberApartments;
                    sectionMo["col24"] = objectCr.numberLiving;
                    sectionMo["col25"] = objectCr.wallMaterial;
                    sectionMo["col26"] = objectCr.roofingMaterial;
                    sectionMo["col27"] = objectCr.dateCommissioning.HasValue ? objectCr.dateCommissioning.Value.Year.ToStr() : "-";
                    sectionMo["col28"] = objectCr.physicalWear;
                    sectionMo["col29"] = objectCr.dateLastOverhaul.HasValue ? objectCr.dateLastOverhaul.Value.Year.ToStr() : "-";
                    sectionMo["col299"] = objectCr.programName;
                    
                    if (financeSourceResource.ContainsKey(objectCr.muId))
                    {
                        if (financeSourceResource[objectCr.muId].ContainsKey(objectCr.objectCrId))
                        {
                            var finSources = financeSourceResource[objectCr.muId][objectCr.objectCrId];
                            sectionMo["col30"] = finSources.Select(x => x.finSourceName).Aggregate(string.Empty, (current, s) => string.IsNullOrEmpty(current) ? s : current + ", " + s);

                            var col31 = finSources.Sum(x => x.fundResource + x.budgetSubject + x.budgetMu + x.ownerResource);
                            var col32 = finSources.Sum(x => x.fundResource);
                            var col33 = finSources.Sum(x => x.budgetSubject);
                            var col34 = finSources.Sum(x => x.budgetMu);
                            var col35 = finSources.Sum(x => x.ownerResource);

                            sectionMo["col31"] = col31.HasValue && col31.Value != 0 ? col31.Value.ToString(Format) : empty;
                            sectionMo["col32"] = col32.HasValue && col32.Value != 0 ? col32.Value.ToString(Format) : empty;
                            sectionMo["col33"] = col33.HasValue && col33.Value != 0 ? col33.Value.ToString(Format) : empty;
                            sectionMo["col34"] = col34.HasValue && col34.Value != 0 ? col34.Value.ToString(Format) : empty;
                            sectionMo["col35"] = col35.HasValue && col35.Value != 0 ? col35.Value.ToString(Format) : empty;
                        }
                    }
                    
                    if (typeWorks.ContainsKey(objectCrGroup.Key))
                    {
                        var typeWork = typeWorks[objectCrGroup.Key].FirstOrDefault(x => x.Key == objectCr.realObjId).Value;
                        if (typeWork != null)
                        {
                            foreach (var work in typeWork)
                            {
                                sectionMo["volume" + work.Key] = work.Value.volume.HasValue && work.Value.volume != 0 ? work.Value.sum.Value.ToString(Format) : empty;
                                sectionMo["summ" + work.Key] = work.Value.sum.HasValue && work.Value.sum != 0 ? work.Value.sum.Value.ToString(Format) : empty;
                            }

                            var s36 = (typeWork.ContainsKey("1018") ? typeWork["1018"].sum : 0) + (typeWork.ContainsKey("1019") ? typeWork["1019"].sum : 0);
                            var s37 = typeWork.ContainsKey("1020") ? typeWork["1020"].sum : 0;
                            var s80 = typeWork.Values.Where(x => x.Consistent185Fz).Sum(x => x.sum);

                            sectionMo["col36"] = s36.HasValue ? s36.Value.ToString(Format) : empty;
                            sectionMo["col37"] = s37.HasValue ? s37.Value.ToString(Format) : empty;
                            sectionMo["col80"] = s80.HasValue ? s80.Value.ToString(Format) : empty;
                            
                            sumMo36 += s36;
                            sumMo37 += s37;
                            sumMo80 += s80;
                        }
                    }

                    ++counter;
                }

                section["sum19"] = objectCrGroup.Value.Sum(x => x.areaMkd);
                section["sum20"] = objectCrGroup.Value.Sum(x => x.areaLivingNotLivingMkd);
                section["sum21"] = objectCrGroup.Value.Sum(x => x.areaLiving);
                section["sum22"] = objectCrGroup.Value.Sum(x => x.areaLivingOwned);
                section["sum23"] = objectCrGroup.Value.Sum(x => x.numberApartments).ToInt();
                section["sum24"] = objectCrGroup.Value.Sum(x => x.numberLiving).ToInt();
                section["sum28"] = objectCrGroup.Value.Average(x => x.physicalWear);
                
                // Инсточники финансирования
                var financeSourceResourceData = financeSourceResource.ContainsKey(objectCrGroup.Key)
                                                    ? financeSourceResource[objectCrGroup.Key]
                                                    : null;

                if (financeSourceResourceData != null)
                {
                    var sum31 = financeSourceResourceData
                                .Values.Sum(x => x.Sum(y => y.fundResource + y.budgetSubject + y.budgetMu + y.ownerResource));

                    section["sum31"] = sum31.HasValue ? sum31.Value.ToString("#.##") : empty;

                    var sum32 = financeSourceResourceData.Values.Sum(x => x.Sum(y => y.fundResource));
                    var sum33 = financeSourceResourceData.Values.Sum(x => x.Sum(y => y.budgetSubject));
                    var sum34 = financeSourceResourceData.Values.Sum(x => x.Sum(y => y.budgetMu));
                    var sum35 = financeSourceResourceData.Values.Sum(x => x.Sum(y => y.ownerResource));

                    section["sum32"] = sum32.HasValue ? sum32.Value.ToString(Format) : empty;
                    section["sum33"] = sum32.HasValue ? sum33.Value.ToString(Format) : empty;
                    section["sum34"] = sum32.HasValue ? sum34.Value.ToString(Format) : empty;
                    section["sum35"] = sum32.HasValue ? sum35.Value.ToString(Format) : empty;
                }
                else
                {
                    section["sum31"] = empty;
                    section["sum32"] = empty;
                    section["sum33"] = empty;
                    section["sum34"] = empty;
                    section["sum35"] = empty;
                }

                section["sum36"] = sumMo36.HasValue && sumMo36.Value != 0 ? sumMo36.Value.ToString(Format) : empty;
                section["sum37"] = sumMo37.HasValue && sumMo37.Value != 0 ? sumMo37.Value.ToString(Format) : empty;
                section["sum80"] = sumMo80.HasValue && sumMo80.Value != 0 ? sumMo80.Value.ToString(Format) : empty;

                var typeWorkData = typeWorks.ContainsKey(objectCrGroup.Key) ? typeWorks[objectCrGroup.Key] : null;

                if (typeWorkData != null)
                {
                    Enumerable.Range(1, 23).Select(x => x.ToStr()).ForEach(x =>
                        {
                            var sum = typeWorkData.Values.Sum(y => y.ContainsKey(x) ? y[x].sum : 0);
                            var volume = typeWorkData.Values.Sum(y => y.ContainsKey(x) ? y[x].volume : 0);

                            section["totalsum" + x] = sum.Value != 0 ? sum.Value.ToString(Format) : empty;
                            section["totalvolume" + x] = volume.Value != 0 ? volume.Value.ToString() : empty;
                        });
                }
            }
        }

        /// <summary>
        /// The get man org.
        /// </summary>
        /// <param name="manOrgContractList">
        /// The man org contract list.
        /// </param>
        /// <param name="contractRelationDict">
        /// The contract Relation Dict.
        /// </param>
        /// <returns>
        /// The <see cref="ManOrgContractProxy"/>.
        /// </returns>
        private ManOrgContractProxy GetManOrg(List<ManOrgContractProxy> manOrgContractList, Dictionary<long, ManOrgContractProxy> contractRelationDict)
        {
            if (manOrgContractList.Count == 1)
            {
                return manOrgContractList.First();
            }

            var managingOrgOwners = manOrgContractList.FirstOrDefault(x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners);

            if (managingOrgOwners != null)
            {
                return managingOrgOwners;
            }

            var jskTsj = manOrgContractList.FirstOrDefault(x => x.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj);

            if (jskTsj != null)
            {
                var contractRelationList =
                    contractRelationDict.Keys.Intersect(manOrgContractList.Select(x => x.ManageOrgId)).ToList();

                var res = jskTsj;

                if (contractRelationList.Any())
                {
                    res.Name = string.Format(
                        "{0} ({1})", res.Name, contractRelationDict[contractRelationList.First()].Name);
                }

                return res;
            }

            // От безысходности
            return manOrgContractList.First();
        }

        /// <summary>
        /// The parse ids.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="strId">
        /// The ids.
        /// </param>
        private void ParseIds(List<long> list, string strId)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(strId))
            {
                var ids = strId.Split(',');
                foreach (var id in ids)
                {
                    int result;
                    if (int.TryParse(id, out result))
                    {
                        if (!list.Contains(result))
                        {
                            list.Add(result);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The man org contract proxy.
        /// </summary>
        private sealed class ManOrgContractProxy
        {
            /// <summary>
            /// Gets or sets the mo id.
            /// </summary>
            public long ManageOrgId { get; set; }

            /// <summary>
            /// Gets or sets the type contract man org real obj.
            /// </summary>
            public TypeContractManOrg TypeContractManOrgRealObj { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
