// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetTwoOutlineBoiler.cs" company="BarsGroup">
//   ©BarsGroup
// </copyright>
// <summary>
//   Defines the SetTwoOutlineBoiler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhCrTp.Report
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
    using Bars.Gkh.PassportProvider;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;
    using Bars.GkhCr.Localizers;

    /// <summary>
    /// Установка 2-х контур.котла
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class SetTwoOutlineBoiler : BasePrintForm
    {
        #region параметры отчета

        /// <summary>
        /// The program cr id.
        /// </summary>
        private long programCrId;

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
        /// Initializes a new instance of the <see cref="SetTwoOutlineBoiler"/> class.
        /// </summary>
        public SetTwoOutlineBoiler() : base(new ReportTemplateBinary(Properties.Resource.SetTwoOutlineBoiler))
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
                return "Установка 2-х контур.котла";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Установка 2-х контур.котла";
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
        /// Gets the params controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.SetTwoOutlineBoiler";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.SetTwoOutlineBoiler";
            }
        }

        /// <summary>
        /// The set user parameters.
        /// </summary>
        /// <param name="baseParams">
        /// The base parameters.
        /// </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToLong();

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            this.municipalityIds.Clear();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (string.IsNullOrEmpty(municipalityStr))
            {
                return;
            }

            var mcp = municipalityStr.Split(',');
            foreach (var id in mcp)
            {
                long mcpId;
                if (!long.TryParse(id, out mcpId))
                {
                    continue;
                }

                if (!this.municipalityIds.Contains(mcpId))
                {
                    this.municipalityIds.Add(mcpId);
                }
            }
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report params.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);

            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                    .Select(x => new { id = x.Id, name = x.Name })
                    .AsEnumerable()
                    .OrderBy(x => x.name)
                    .GroupBy(x => x.id)
                    .ToDictionary(x => x.Key, x => x.First().name);

            var objectCrQuery = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                    .Where(x => x.ProgramCr.Id == this.programCrId)
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
                                finSourceCode = x.FinanceSource.Code
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.municipalityId)
                    .ToDictionary(x => x.Key, x => x.GroupBy(y => y.objectCrId).ToDictionary(y => y.Key, y => y.ToList()));

            var objectCrByMunisipalityDict = objectCrQuery.Select(
                    x => new
                        {
                            muId = x.RealityObject.Municipality.Id,
                            muName = x.RealityObject.Municipality.Name,
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
                            dateLastOverhaul = x.RealityObject.DateLastOverhaul
                        })
                             .AsEnumerable()
                             .OrderBy(x => x.muName)
                             .GroupBy(x => x.muId)
                             .ToDictionary(x => x.Key, x => x.OrderBy(y => y.adress).ToList());

            var typeWorks = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                    .Select(x => new
                            {
                                muId = x.ObjectCr.RealityObject.Municipality.Id,
                                realObjId = x.ObjectCr.RealityObject.Id,
                                finSourceCode = x.FinanceSource.Code,
                                typeWorkId = x.Id,
                                workCode = x.Work.Code,
                                volume = x.Volume,
                                sum = x.Sum,
                                psd = x.HasPsd
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.muId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        x.GroupBy(y => y.realObjId)
                         .ToDictionary(
                             y => y.Key,
                             y => y.GroupBy(z => z.finSourceCode)
                                   .ToDictionary(
                                      z => z.Key,
                                      z => z.GroupBy(u => u.workCode).ToDictionary(
                                          u => u.Key,
                                          u =>
                                              {
                                                  var work = u.FirstOrDefault();
                                                  return work != null ? new { work.sum, work.volume, work.psd } : null;
                                              }))));

            var repManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                    .GetAll()
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= this.reportDate)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= this.reportDate);

            var manOrgContractIdsQuery = repManOrgContractRealityObject
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Select(x => x.ManOrgContract.Id);

            var contractRelationDict =
                this.Container.Resolve<IDomainService<ManOrgContractRelation>>()
                    .GetAll()
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
                            moId = (long?)x.ManOrgContract.ManagingOrganization.Id
                        })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                        x => x.Key, 
                        y =>
                        {
                            var moContractList = y.Select(x => new ManOrgContractProxy
                                                    {
                                                        ManageOrgId = x.moId,
                                                        TypeContractManOrgRealObj = x.TypeContractManOrgRealObj
                                                    }).ToList();
                            return this.GetManOrg(moContractList, contractRelationDict);
                        });

            var managingOrganization = this.Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                    .Where(x => x.Contragent != null)
                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id));

            var managementOrganizationDict = managingOrganization.Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                                    .Select(x => new
                                            {
                                                manageOrgId = x.Id,
                                                contragentId = x.Contragent.Id,
                                                uoName = x.Contragent.Name,
                                                typeManagement = x.TypeManagement.GetEnumMeta().Display,
                                                jurFiasPostCode = x.Contragent.FiasJuridicalAddress.PostCode,
                                                jurStreet = x.Contragent.FiasJuridicalAddress.StreetName,
                                                jurHouse = x.Contragent.FiasJuridicalAddress.House,
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
            var contragentContact =
                this.Container.Resolve<IDomainService<ContragentContact>>()
                    .GetAll()
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

            var protocolCrDict = this.Container.Resolve<IDomainService<ProtocolCr>>().GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                    .Where(x => realObjIds.Contains(x.ObjectCr.RealityObject.Id))
                    .Select(x => new
                            {
                                realtyObjId = x.ObjectCr.RealityObject.Id,
                                x.TypeDocumentCr,
                                x.DateFrom,
                                x.DocumentNum,
                                x.CountVote
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.realtyObjId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.TypeDocumentCr)
                              .ToDictionary(
                                 y => y.Key,
                                 y => new
                                     {
                                         DateFrom = y.Select(z => z.DateFrom.HasValue ? z.DateFrom.Value : new DateTime(1, 1, 1)).FirstOrDefault(),
                                         DocNum = y.Select(z => z.DocumentNum).FirstOrDefault(),
                                         CountVote = y.Select(z => z.CountVote.HasValue ? z.CountVote.Value : decimal.Zero).FirstOrDefault(),
                                     }));

            // Тех.паспорт
            var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт");
            if (passport == null)
            {
                throw new Exception("Не найден провайдер технического паспорта");
            }
            
            var tehPassportIds = this.Container.Resolve<IDomainService<TehPassport>>().GetAll()
                    .Where(x => realObjIds.Contains(x.RealityObject.Id))
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments && x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Select(x => x.Id);

            var tehPassportValues = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                    .Where(x => tehPassportIds.Contains(x.TehPassport.Id) && (x.FormCode == "Form_3_5" && x.CellCode == "1:3"))
                    .Select(x => new { realtyObjId = x.TehPassport.RealityObject.Id, x.FormCode, x.CellCode, x.Value })
                    .AsEnumerable()
                    .GroupBy(x => x.realtyObjId)
                    .ToDictionary(
                        x => x.Key, 
                        x =>
                        {
                            var tehPassportValue = x.FirstOrDefault();
                            if (tehPassportValue != null)
                            {
                                var valueName = passport.GetTextForCellValue(tehPassportValue.FormCode, tehPassportValue.CellCode, tehPassportValue.Value);
                                return new { tehPassportValue.FormCode, tehPassportValue.CellCode, tehPassportValue.Value, valueName };
                            }

                            return null;
                        });
            
            reportParams.SimpleReportParams["reportDate"] = this.reportDate.ToShortDateString();
            reportParams.SimpleReportParams["programCrName"] = programCr.Name;

            var counter = 1;
            string empty = string.Empty;

            var sumAllDict = new Dictionary<string, decimal?>();
            var worksTypes = new List<string> { "24", "28", "7", "25", "26" }; // TODO добавить XX после 7
            worksTypes.ForEach(x =>
                    {
                        sumAllDict.Add("volumeAll" + x, 0);
                        sumAllDict.Add("sumAll" + x, 0);
                    });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            var sectionMo = section.ДобавитьСекцию("секцияМо");
            var sectionRealtyObj = sectionMo.ДобавитьСекцию("секцияЖилДом");
            var sectionFinSource = sectionRealtyObj.ДобавитьСекцию("секцияИФ");

            foreach (var municipal in municipalityDict)
            {
                if (!objectCrByMunisipalityDict.ContainsKey(municipal.Key))
                {
                    continue;
                }

                section.ДобавитьСтроку();
                section["район"] = municipalityDict[municipal.Key];
                sectionMo.ДобавитьСтроку();

                foreach (var objectCr in objectCrByMunisipalityDict[municipal.Key])
                {
                    var dataObj = Enumerable.Range(1, 32).ToDictionary(x => string.Format("col{0}", x), x => new object());

                    dataObj["col1"] = counter;
                    dataObj["col2"] = objectCr.adress;

                    if (realityObjectManOrgDict.ContainsKey(objectCr.realObjId))
                    {
                        var manageOrgId = realityObjectManOrgDict[objectCr.realObjId].ManageOrgId;
                        
                        var manageOrg = manageOrgId.HasValue && managementOrganizationDict.ContainsKey(manageOrgId.Value)
                                            ? managementOrganizationDict[manageOrgId.Value]
                                            : null;

                        if (manageOrg != null)
                        {
                            dataObj["col3"] = manageOrg.typeManagement;
                            dataObj["col4"] = manageOrg.uoName;
                            dataObj["col5"] = manageOrg.jurAddress;
                            dataObj["col6"] = manageOrg.jurFiasPostCode;
                            dataObj["col7"] = manageOrg.jurStreet;
                            dataObj["col8"] = manageOrg.jurHouse;
                            dataObj["col9"] = empty;
                            dataObj["col10"] = manageOrg.inn;
                            dataObj["col11"] = manageOrg.kpp;
                            if (contragentContact.ContainsKey(manageOrg.contragentId))
                            {
                                dataObj["col12"] = contragentContact[manageOrg.contragentId];
                            }

                            dataObj["col13"] = manageOrg.phone;
                            dataObj["col14"] = manageOrg.email;
                        }
                        else
                        {
                            Enumerable.Range(3, 12).ForEach(x => dataObj["col" + x] = empty);
                        }
                    }

                    dataObj["col15"] = objectCr.floors;
                    dataObj["col16"] = objectCr.series;
                    dataObj["col17"] = objectCr.capitalGroup;
                    dataObj["col18"] = objectCr.areaMkd;
                    dataObj["col19"] = objectCr.areaLivingNotLivingMkd;
                    dataObj["col20"] = objectCr.areaLiving;
                    dataObj["col21"] = objectCr.areaLivingOwned;
                    dataObj["col22"] = objectCr.numberApartments;
                    dataObj["col23"] = objectCr.numberLiving;
                    dataObj["col24"] = objectCr.wallMaterial;
                    dataObj["col25"] = objectCr.roofingMaterial;
                    dataObj["col26"] = objectCr.dateCommissioning.HasValue
                                           ? objectCr.dateCommissioning.Value.Year.ToStr()
                                           : "-";
                    dataObj["col27"] = objectCr.physicalWear;
                    dataObj["col28"] = objectCr.dateLastOverhaul.HasValue
                                           ? objectCr.dateLastOverhaul.Value.Year.ToStr()
                                           : "-";
                    dataObj["col29"] = empty;
                    if (protocolCrDict.ContainsKey(objectCr.realObjId))
                    {
                        var protocolCrByType = protocolCrDict[objectCr.realObjId];
                        var protocolNeedCr = protocolCrByType.FirstOrDefault(x => x.Key.Key == TypeDocumentCrLocalizer.ProtocolNeedCrKey);
                        dataObj["col30"] = protocolNeedCr.Value != null ? "Да" : "Нет";
                    }
                    else
                    {
                        dataObj["col30"] = "Нет";
                    }

                    dataObj["col31"] = empty;

                    if (tehPassportValues.ContainsKey(objectCr.realObjId))
                    {
                        var tehPassportValue = tehPassportValues[objectCr.realObjId];
                        if (tehPassportValue != null)
                        {
                            dataObj["col32"] = tehPassportValue.valueName == "Приточная вентиляция" ? "Да" : "Нет";
                        }
                        else
                        {
                            dataObj["col32"] = "Нет";
                        }
                    }
                    else
                    {
                        dataObj["col32"] = "Нет";
                    }
                    
                    sectionRealtyObj.ДобавитьСтроку();

                    if (financeSourceResource.ContainsKey(objectCr.muId))
                    {
                        var financeSourceByMun = financeSourceResource[objectCr.muId];
                        if (financeSourceByMun.ContainsKey(objectCr.objectCrId))
                        {
                            var finSource = financeSourceByMun[objectCr.objectCrId];

                            foreach (var source in finSource)
                            {
                                sectionFinSource.ДобавитьСтроку();
                                dataObj.ForEach(x => sectionFinSource[x.Key] = x.Value);

                                // Источники финансирования
                                sectionFinSource["col33"] = source.finSourceName;

                                // Суммы и объемы по видам работ 
                                if (!typeWorks.ContainsKey(objectCr.muId))
                                {
                                    continue;
                                }

                                var worksByMu = typeWorks[objectCr.muId];
                                if (!worksByMu.ContainsKey(objectCr.realObjId))
                                {
                                    continue;
                                }

                                var worksByRealtyObj = worksByMu[objectCr.realObjId];
                                if (!worksByRealtyObj.ContainsKey(source.finSourceCode))
                                {
                                    continue;
                                }

                                var workByFinSource = worksByRealtyObj[source.finSourceCode];
                                if (workByFinSource != null)
                                {
                                    foreach (var workType in worksTypes)
                                    {
                                        if (!workByFinSource.ContainsKey(workType))
                                        {
                                            continue;
                                        }

                                        var work = workByFinSource[workType];
                                        if (work != null)
                                        {
                                            sectionFinSource["volume" + workType] = work.volume;
                                            sectionFinSource["sum" + workType] = work.sum;
                                            sectionFinSource["psd" + workType] = work.psd ? "Да" : "Нет";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        sectionFinSource.ДобавитьСтроку();
                        dataObj.ForEach(x => sectionFinSource[x.Key] = x.Value);
                        foreach (var workType in worksTypes)
                        {
                            sectionFinSource["volume" + workType] = 0;
                            sectionFinSource["sum" + workType] = 0;
                            sectionFinSource["psd" + workType] = empty;
                        }
                    }
                    
                    // Суммы и объемы по видам работ (Итого для дома)
                    dataObj.ForEach(x => sectionRealtyObj[x.Key] = x.Value);
                    if (typeWorks.ContainsKey(objectCr.muId))
                    {
                        var worksByMu = typeWorks[objectCr.muId];
                        if (worksByMu.ContainsKey(objectCr.realObjId))
                        {
                            var worksByRealtyObj = worksByMu[objectCr.realObjId];

                            foreach (var workType in worksTypes)
                            {
                                if (worksByRealtyObj.Count(x => x.Value.ContainsKey(workType)) <= 0)
                                {
                                    sectionRealtyObj["volumeRealty" + workType] = 0;
                                    sectionRealtyObj["sumRealty" + workType] = 0;
                                    continue;
                                }

                                sectionRealtyObj["volumeRealty" + workType] = worksByRealtyObj.Sum(x => x.Value.Where(y => y.Key == workType).Sum(y => y.Value.volume));
                                sectionRealtyObj["sumRealty" + workType] = worksByRealtyObj.Sum(x => x.Value.Where(y => y.Key == workType).Sum(y => y.Value.sum));
                            }
                        }
                    }

                    ++counter;
                }

                if (typeWorks.ContainsKey(municipal.Key))
                {
                    var worksByMunicipal = typeWorks[municipal.Key];
                    foreach (var typeWork in worksTypes)
                    {
                        var volume = worksByMunicipal.Sum(x => x.Value.Sum(y => y.Value.Where(z => z.Key == typeWork).Sum(z => z.Value.volume)));
                        sectionMo["volumeMo" + typeWork] = volume;
                        sumAllDict["volumeAll" + typeWork] += volume;

                        var sum = worksByMunicipal.Sum(x => x.Value.Sum(y => y.Value.Where(z => z.Key == typeWork).Sum(z => z.Value.sum)));
                        sectionMo["sumMo" + typeWork] = sum;
                        sumAllDict["sumAll" + typeWork] += sum;
                    }
                }
            }

            sumAllDict.ForEach(x => reportParams.SimpleReportParams[x.Key] = x.Value);
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
                var contractRelationList = contractRelationDict.Keys.Intersect(
                        manOrgContractList.Select(x => x.ManageOrgId.HasValue ? x.ManageOrgId.Value : 0)).ToList();
                
                var res = jskTsj;

                if (contractRelationList.Any())
                {
                    res.Name = string.Format("{0} ({1})", res.Name, contractRelationDict[contractRelationList.First()].Name);
                }

                return res;
            }

            // От безысходности
            return manOrgContractList.First();
        }

        /// <summary>
        /// The man org contract proxy.
        /// </summary>
        private sealed class ManOrgContractProxy
        {
            /// <summary>
            /// Gets or sets the mo id.
            /// </summary>
            public long? ManageOrgId { get; set; }

            /// <summary>
            /// Gets or sets the type contract man org real obj.
            /// </summary>
            public TypeContractManOrg TypeContractManOrgRealObj { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
