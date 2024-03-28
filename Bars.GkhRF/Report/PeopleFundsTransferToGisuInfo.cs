// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeopleFundsTransferToGisuInfo.cs" company="BarsGroup">
//   ©BarsGroup
// </copyright>
// <summary>
//   The people funds transfer to gisu info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Информация о перечислении денежных средств граждан в ГИСУ (без пересичления).
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class PeopleFundsTransferToGisuInfo : BasePrintForm
    {
        #region Параметры отчета

        /// <summary>
        /// The municipality ids.
        /// </summary>
        private List<long> municipalityIds;

        /// <summary>
        /// The date start.
        /// </summary>
        private DateTime dateStart;

        /// <summary>
        /// The date end.
        /// </summary>
        private DateTime dateEnd;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleFundsTransferToGisuInfo"/> class.
        /// </summary>
        public PeopleFundsTransferToGisuInfo() : base(new ReportTemplateBinary(Properties.Resources.PeopleFundsTransferToGisuInfo))
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
                return "Информация о перечислении денежных средств граждан в ГИСУ (без пересичления)";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Информация о перечислении денежных средств граждан в ГИСУ (без пересичления)";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Финансирование";
            }
        }

        /// <summary>
        /// Gets the params controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PeopleFundsTransferToGisuInfo";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.PeopleFundsTransferToGisuInfo"; 
            }
        }

        /// <summary>
        /// The set user params.
        /// </summary>
        /// <param name="baseParams">
        /// The base params.
        /// </param>
        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var strMunicipalityId = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = strMunicipalityId.Split(',').Select(x => x.ToLong()).ToList();
            this.dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            this.dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
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
            if (this.dateStart == DateTime.MinValue)
            {
                throw new Exception("Не указан параметр \"Начало периода\"");
            }

            if (this.dateEnd == DateTime.MinValue)
            {
                throw new Exception("Не указан параметр \"Конец периода\"");
            }

            var selectMunicipal = this.municipalityIds.Count > 0  && this.municipalityIds.First() != 0;
            var municipalNameDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                 .WhereIf(selectMunicipal, x => this.municipalityIds.Contains(x.Id))
                 .Select(x => new { x.Id, x.Name })
                 .OrderBy(x => x.Name)
                 .ToDictionary(x => x.Id, v => v.Name);

            var transferRfByRoDict = this.Container.Resolve<IDomainService<TransferRfRecObj>>().GetAll()
                 .WhereIf(selectMunicipal, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id)) 
                 .Where(x => x.TransferRfRecord.TransferDate >= this.dateStart && x.TransferRfRecord.TransferDate <= this.dateEnd)
                 .Where(x => x.TransferRfRecord.State.Name == "Без перечисления")
                 .Select(x => new
                 {
                     municipalId = x.RealityObject.Municipality.Id,
                     realtyObjId = x.RealityObject.Id,
                     realtyObjAddress = x.RealityObject.Address,
                     sum = x.Sum,
                     manOrgName = x.TransferRfRecord.TransferRf.ContractRf.ManagingOrganization.Contragent.Name,

                     dogovor = x.TransferRfRecord.TransferRf.ContractRf.DocumentDate.HasValue
                       ? string.Format("№{0} от {1}", x.TransferRfRecord.TransferRf.ContractRf.DocumentNum, x.TransferRfRecord.TransferRf.ContractRf.DocumentDate.Value.ToString("dd.MM.yy"))
                       : string.Format("№{0} от", x.TransferRfRecord.TransferRf.ContractRf.DocumentNum)
                 })
                 .AsEnumerable().Distinct()
                 .GroupBy(x => x.municipalId)
                 .ToDictionary(x => x.Key, x => x.OrderBy(y => y.realtyObjAddress));
            
            var counter = 1;
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияМо");
            var sectionUo = sectionMo.ДобавитьСекцию("секцияУо");

            foreach (var municipal in municipalNameDict)
            {
                sectionMo.ДобавитьСтроку();
                sectionMo["наименованиеМО"] = municipal.Value;

                if (!transferRfByRoDict.ContainsKey(municipal.Key))
                {
                    sectionMo["sumMo"] = 0;
                    continue;
                }

                var transferRfByRo = transferRfByRoDict[municipal.Key];
                foreach (var transfer in transferRfByRo)
                {
                    sectionUo.ДобавитьСтроку();

                    sectionUo["col1"] = counter;
                    sectionUo["col2"] = transfer.manOrgName;
                    sectionUo["col3"] = transfer.dogovor;
                    sectionUo["col4"] = transfer.realtyObjAddress;
                    sectionUo["col5"] = transfer.sum;

                    ++counter;
                }

                sectionMo["sumMo"] = transferRfByRo.Sum(x => x.sum);
            }

            reportParams.SimpleReportParams["sumAllMo"] = transferRfByRoDict.Sum(x => x.Value.Sum(y => y.sum));
        }
    }
}
