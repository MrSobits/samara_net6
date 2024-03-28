namespace Bars.GkhCr.Report
{
	using B4.Modules.Reports;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Entities.Dicts.Multipurpose;
	using Bars.Gkh.Enums;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Castle.Windsor;
	using System;
	using System.Collections.Generic;
	using System.Linq;

    /// <summary>
    /// Реестр протоколов и актов ГЖИ часть 3
    /// </summary>
    public class ProtocolsActsGjiReestrReport : BasePrintForm
    {
        // идентификатор программы КР
        private int programCrId;

        public ProtocolsActsGjiReestrReport()
            : base(new ReportTemplateBinary(Properties.Resources.ProtocolsActsGjiReestr))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ProtocolsActsGjiReestr"; }
        }

        public override string Name
        {
            get { return "Реестр протоколов и актов ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Реестр протоколов и актов ГЖИ"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ProtocolsActsGjiReestrReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>();
            var servObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var servTypeWorkCr = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var servProtocolCr = this.Container.Resolve<IDomainService<ProtocolCr>>();
            var servRealityObjectImage = this.Container.Resolve<IDomainService<RealityObjectImage>>();
            
            var program = servProgramCr.GetAll().Where(x => x.Id == this.programCrId).FirstOrDefault();
            if (program == null)
            {
                return;
            }

            var programPeriodId = program.Period.Id;
            var programEndDate = program.Period.DateEnd.HasValue ? program.Period.DateEnd.Value : DateTime.MaxValue;

            reportParams.SimpleReportParams["year"] = program.Period.DateEnd.HasValue ? program.Period.DateEnd.Value.Year.ToStr() : string.Empty;
            
            var queryObjectsCr = servObjectCr.GetAll()
                                     .Where(x => x.ProgramCr.Id == this.programCrId);

            var objectsCr = queryObjectsCr.Select(x => new
                                                      {
                                                          crObjId = x.Id, 
                                                          roId = x.RealityObject.Id, 
                                                          x.RealityObject.Address, 
                                                          muName = x.RealityObject.Municipality.Name, 
                                                          x.GjiNum, 
                                                          x.DateGjiReg
                                                      })
                                     .OrderBy(x => x.muName)
                                     .ThenBy(x => x.Address)
                                     .ToList();

            var queryObjCrIds = queryObjectsCr.Select(x => x.Id);
            var queryRealtyObjIds = queryObjectsCr.Select(x => x.RealityObject.Id);

			var glossaryItems = TypeDocumentCrLocalizer.GetAllGlossaryItems();
	        var typeDocumentCrList = new List<string>();
			AddTypeDocumentCrItemByKey(glossaryItems, typeDocumentCrList, TypeDocumentCrLocalizer.ProtocolNeedCrKey);
			AddTypeDocumentCrItemByKey(glossaryItems, typeDocumentCrList, TypeDocumentCrLocalizer.ProtocolChangeCrKey);
			AddTypeDocumentCrItemByKey(glossaryItems, typeDocumentCrList, TypeDocumentCrLocalizer.ProtocolCompleteCrKey);
			AddTypeDocumentCrItemByKey(glossaryItems, typeDocumentCrList, TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey);

            var repCrWorks = servTypeWorkCr.GetAll().Where(x => x.Work != null);

            var worksList = repCrWorks
                .Where(x => queryObjCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new { crObjectId = x.ObjectCr.Id, x.Sum, x.Work.TypeWork })
                .ToList();

            var protocolsCrList = servProtocolCr.GetAll()
                                                .Where(x => queryObjCrIds.Contains(x.ObjectCr.Id))
                                                .Where(x => typeDocumentCrList.Contains(x.TypeDocumentCr.Key))
                                                .Select(x => new { crObjectId = x.ObjectCr.Id, x.TypeDocumentCr, x.DocumentNum, x.DateFrom, x.Id })
                                                .ToList();

            var crWorks = worksList.GroupBy(x => x.crObjectId).ToDictionary(x => x.Key, x => x.Select(y => new { y.Sum, y.TypeWork }).ToList());

            var protocolsCrDict = protocolsCrList.GroupBy(x => x.crObjectId)
                                                 .ToDictionary(
                                                     x => x.Key,
                                                     x => x.GroupBy(y => y.TypeDocumentCr)
                                                           .ToDictionary(
                                                               y => y.Key.Key,
                                                               y => y.Select(z => new { z.DateFrom, z.DocumentNum }).ToList()));

            // Управляющие организации дома
            var repManagingOrgRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                        .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate <= programEndDate)
                        .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= programEndDate);

            var managingOrgRealityList = repManagingOrgRealityObject
                .Where(x => queryRealtyObjIds.Contains(x.RealityObject.Id))
                .Where(x => x.ManOrgContract.ManagingOrganization != null)                
                .Select(x => new
                {
                    roId = ((long?)x.RealityObject.Id) ?? -1,
                    moId = x.ManOrgContract.ManagingOrganization.Id,
                    contragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    contragentAddress = x.ManOrgContract.ManagingOrganization.Contragent.JuridicalAddress,
                    contragentPhone = x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                    x.ManOrgContract.ManagingOrganization.TypeManagement
                })
                .ToList();

            var realityObjectImageList =
                servRealityObjectImage.GetAll()
                                      .Where(x => queryRealtyObjIds.Contains(x.RealityObject.Id))
                                      .Where(x => x.Period.Id == programPeriodId)
                                      .Where(x => x.ImagesGroup == ImagesGroup.BeforeOverhaul)
                                      .Select(x => x.RealityObject.Id)
                                      .ToList();

            var managingOrgRealityObject = managingOrgRealityList
                        .GroupBy(x => x.roId)
                        .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new
                        {
                            y.moId,
                            y.contragentName,
                            y.contragentAddress,
                            y.contragentPhone,
                            y.TypeManagement
                        }).ToList());

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            foreach (var objectCr in objectsCr)
            {
                var countRowsOnObjectCr = 1;

                if (protocolsCrDict.ContainsKey(objectCr.crObjId))
                {
                    countRowsOnObjectCr = protocolsCrDict[objectCr.crObjId].Values.Select(x => x.Count).Max() > 1
                        ? protocolsCrDict[objectCr.crObjId].Values.Select(x => x.Count).Max()
                        : 1;
                }

                for (int i = 0; i < countRowsOnObjectCr; i++)
                {
                    section.ДобавитьСтроку();
                    section["ReestrNumber"] = objectCr.GjiNum;
                    section["RegDate"] = objectCr.DateGjiReg.HasValue ? objectCr.DateGjiReg.Value.ToShortDateString() : string.Empty;
                    section["Municipality"] = objectCr.muName;
                    section["Address"] = objectCr.Address;
                    section["FinanceLimit"] = crWorks.ContainsKey(objectCr.crObjId) ? crWorks[objectCr.crObjId].Sum(x => x.Sum) : 0M;
                    section["IncludingSumSmr"] = crWorks.ContainsKey(objectCr.crObjId) ? crWorks[objectCr.crObjId].Where(x => x.TypeWork == TypeWork.Work).Sum(x => x.Sum) : 0M;

                    if (managingOrgRealityObject.ContainsKey(objectCr.roId))
                    {
                        var currentMoList = managingOrgRealityObject[objectCr.roId];
                        if (currentMoList.Count == 1)
                        {
                            section["CustomerName"] = currentMoList.First().contragentName;
                            section["CustomerAddress"] = currentMoList.First().contragentAddress + " " + currentMoList.First().contragentPhone;
                        }
                        else if (currentMoList.Count > 1)
                        {
                            var uk = currentMoList.FirstOrDefault(x => x.TypeManagement == TypeManagementManOrg.UK);
                            if (uk != null)
                            {
                                section["CustomerName"] = uk.contragentName;
                                section["CustomerAddress"] = uk.contragentAddress + " " + uk.contragentPhone;
                            }
                            else
                            {
                                section["CustomerName"] = currentMoList.First().contragentName ?? string.Empty;
                                section["CustomerAddress"] = (currentMoList.First().contragentAddress
                                                                ?? string.Empty) + ", "
                                                                + (currentMoList.First().contragentPhone
                                                                ?? string.Empty);
                            }
                        }
                    }
                    else
                    {
                        section["CustomerName"] = string.Empty;
                        section["CustomerAddress"] = string.Empty;
                    }

                    Func<string, string> test = (typeDocCr) =>
                        {
                            var result = string.Empty;

                            if (protocolsCrDict.ContainsKey(objectCr.crObjId) && protocolsCrDict[objectCr.crObjId].ContainsKey(typeDocCr))
                            {
                                var currentProtocols = protocolsCrDict[objectCr.crObjId][typeDocCr];
                                if (currentProtocols.Count > i)
                                {
                                    result = string.Format("{0} от {1}", currentProtocols[i].DocumentNum, currentProtocols[i].DateFrom.HasValue ? currentProtocols[i].DateFrom.Value.ToShortDateString() : string.Empty);
                                }
                            }

                            return result;
                        };

                    section["GeneralConvocationProtocol"] = test(TypeDocumentCrLocalizer.ProtocolNeedCrKey);
                    section["ChangeTypesOfWorkProtocol"] = test(TypeDocumentCrLocalizer.ProtocolChangeCrKey);
                    section["EndRepairsProtocol"] = test(TypeDocumentCrLocalizer.ProtocolCompleteCrKey);
                    section["ActOfCommissioning"] = test(TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey);

                    section["Photofixation"] = realityObjectImageList.Contains(objectCr.roId) ? "Да" : "Нет";
                }
            }
        }

		private void AddTypeDocumentCrItemByKey(Dictionary<string, MultipurposeGlossaryItem> cache, List<string> items, string key)
	    {
			var item = TypeDocumentCrLocalizer.GetItemFromCacheByKey(cache, key);
			if (item != null)
			{
				items.Add(item.Key);
			}
	    }
    }
}