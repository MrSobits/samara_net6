namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;
	using Bars.GkhCr.Localizers;

    public class ActAuditDataExpense : BasePrintForm
    {
           // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;

        public ActAuditDataExpense()
            : base(new ReportTemplateBinary(Properties.Resources.ActAuditDataExpense))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ActAuditDataExpense"; }
        }

        public override string Name
        {
            get { return "Отчет по Актам сверок данных о расходах"; }
        }

        public override string Desciption
        {
            get { return "Отчет по Актам сверок данных о расходах"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ActAuditDataExpense"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programName = Container.Resolve<IDomainService<ProgramCr>>().GetAll()
                         .Where(x => x.Id == programCrId)
                         .Select(x => x.Name)
                         .First();

            var objectCrForMunicipalityDict = Container.Resolve<IDomainService<ObjectCr>>()
                     .GetAll()
                     .Where(x => x.ProgramCr.Id == programCrId)
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                     .Select(x => new { x.Id, Municipality = x.RealityObject.Municipality.Name, x.RealityObject.Address })
                     .OrderBy(x => x.Municipality)
                     .AsEnumerable()
                     .GroupBy(x => x.Municipality)
                     .ToDictionary(x => x.Key, x => x.Select(y => new { y.Id, y.Address }).OrderBy(y => y.Address));

            var protocolCrDict = Container.Resolve<IDomainService<ProtocolCr>>()
                     .GetAll()
                     .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActAuditDataExpenseKey)
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                     .Select(x => new { ObjectCrId = x.ObjectCr.Id, x.Id, x.SumActVerificationOfCosts })
                     .AsEnumerable()
                     .GroupBy(x => x.ObjectCrId)
                     .ToDictionary(x => x.Key, x => new { Count = x.Count(), SumActVerificationOfCosts = x.Sum(y => y.SumActVerificationOfCosts) });

            var resourceStatementDict = Container.Resolve<IDomainService<ResourceStatement>>()
                     .GetAll()
                     .Where(x => x.EstimateCalculation.ObjectCr.ProgramCr.Id == programCrId)
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.EstimateCalculation.ObjectCr.RealityObject.Municipality.Id))
                     .Select(x => new
                                      {
                                          ObjectCrId = x.EstimateCalculation.ObjectCr.Id, 
                                          x.TotalCost,
                                          x.EstimateCalculation.IsSumWithoutNds                 
                                      })
                     .AsEnumerable()
                     .GroupBy(x => x.ObjectCrId)
                     .ToDictionary(x => x.Key, x => x.Sum(y => y.IsSumWithoutNds ? y.TotalCost.ToDecimal() * 1.18m : y.TotalCost.ToDecimal()));

            reportParams.SimpleReportParams["dateReport"] = string.Format("Отчет по Актам сверок данных о расходах  на {0}", DateTime.Now.ToShortDateString());
            reportParams.SimpleReportParams["programCr"] = string.Format("По {0}", programName);
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMo");
            var num = 0;
            foreach (var objectCrForMunicipality in objectCrForMunicipalityDict)
            {
                sectionMo.ДобавитьСтроку();
                var section = sectionMo.ДобавитьСекцию("section");
                var sectionRed = sectionMo.ДобавитьСекцию("sectionRed");

                var protocolCrCountMo = 0M;
                var protocolCrSumMo = 0M;
                var resStatementTotalCostMo = 0M;

                foreach (var objectCr in objectCrForMunicipality.Value)
                {
                    var protocolCrCount = protocolCrDict.ContainsKey(objectCr.Id) ? protocolCrDict[objectCr.Id].Count : 0M;
                    var resStatementTotalCost = resourceStatementDict.ContainsKey(objectCr.Id) ? resourceStatementDict[objectCr.Id] : 0M;

                    var protocolCrSum = protocolCrDict.ContainsKey(objectCr.Id) ? (protocolCrDict[objectCr.Id].SumActVerificationOfCosts.HasValue
                                                           ? protocolCrDict[objectCr.Id].SumActVerificationOfCosts.Value
                                                           : 0M) : 0M;
                    var percentDiff = 0M;
                    if (resStatementTotalCost != 0M || protocolCrSum != 0M)
                    {
                        percentDiff = resStatementTotalCost > protocolCrSum
                                          ? 100 - ((protocolCrSum * 100) / resStatementTotalCost)
                                          : 100 - ((resStatementTotalCost * 100) / protocolCrSum);
                    }
                    
                    percentDiff = decimal.Round(percentDiff, MidpointRounding.AwayFromZero);
                    if (percentDiff > 1)
                    {
                        sectionRed.ДобавитьСтроку();
                        sectionRed["NumRed"] = ++num;
                        sectionRed["MunicipalityRed"] = objectCrForMunicipality.Key;
                        sectionRed["AddressRed"] = objectCr.Address;
                        sectionRed["ProtocolCrCountRed"] = protocolCrCount;
                        sectionRed["ProtocolCrSumRed"] = protocolCrSum;
                        sectionRed["ResStatementTotalCostRed"] = resStatementTotalCost;
                    }
                    else
                    {
                        section.ДобавитьСтроку();
                        section["Num"] = ++num;
                        section["Municipality"] = objectCrForMunicipality.Key;
                        section["Address"] = objectCr.Address;
                        section["ProtocolCrCount"] = protocolCrCount;
                        section["ProtocolCrSum"] = protocolCrSum;
                        section["ResStatementTotalCost"] = resStatementTotalCost;
                    }

                    protocolCrCountMo += protocolCrCount;
                    resStatementTotalCostMo += resStatementTotalCost;
                    protocolCrSumMo += protocolCrSum;
                }

                sectionMo["Municipality"] = objectCrForMunicipality.Key;
                sectionMo["AddressCount"] = objectCrForMunicipality.Value.Count();
                sectionMo["ProtocolCrCountMo"] = protocolCrCountMo;
                sectionMo["ResStatementTotalCostMo"] = resStatementTotalCostMo;
                sectionMo["ProtocolCrSumMo"] = protocolCrSumMo;
            }
        }
    }
}
