namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    /// <summary>
    /// Уведомление о составлении протокола
    /// </summary>
    public class ProtocolNoticeReport : GkhBaseStimulReport
    {
        /// <inheritdoc />
        public ProtocolNoticeReport()
            : base(new ReportTemplateBinary(Resources.ProtocolNotification))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        /// <inheritdoc />
        public override string Id => "ProtocolNotice";

        /// <inheritdoc />
        public override string CodeForm => this.Id;

        /// <inheritdoc />
        public override string Name => "Уведомление о составлении протокола";

        /// <inheritdoc />
        public override string Description => this.Name;

        /// <inheritdoc />
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор документа
        /// </summary>
        protected long DocumentId { get; set; }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
            
            var actCheckDomain = this.Container.ResolveDomain<ActCheck>();
            var documentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var zonalInspectionInspectorDomain = this.Container.ResolveDomain<ZonalInspectionInspector>();
            var inspectionRoDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var contragentContactDomain = this.Container.ResolveDomain<ContragentContact>();

            using (this.Container.Using(actCheckDomain,
                       documentInspectorDomain,
                       zonalInspectionInspectorDomain,
                       inspectionRoDomain,
                       contragentContactDomain))
            {
                var zonalInspectionInfo = documentInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == this.DocumentId)
                    .Join(zonalInspectionInspectorDomain.GetAll(),
                        x => x.Inspector.Id,
                        y => y.Inspector.Id,
                        (res, y) => new
                        {
                            ЗональноеНаименование1ГосЯзык = y.ZonalInspection.BlankName.ToUpper(),
                            ЗональноеНаименование2ГосЯзык = y.ZonalInspection.BlankNameSecond.ToUpper(),
                            Адрес1ГосЯзык = y.ZonalInspection.Address,
                            Адрес2ГосЯзык = y.ZonalInspection.AddressSecond,
                            Телефон = y.ZonalInspection.Phone,
                            Почта = y.ZonalInspection.Email,
                            ИмяИнспектора = res.Inspector.Fio,
                            ДолжностьИнспектора = res.Inspector.Position
                        })
                    .SingleOrDefault();

                var actCheckInfo = actCheckDomain.GetAll()
                    .Where(x => x.Id == this.DocumentId)
                    .Select(x => new
                    {
                        Организация = x.Inspection.Contragent.Name,
                        Адрес = x.Inspection.Contragent.FiasMailingAddress != null && x.Inspection.Contragent.FiasMailingAddress.AddressName != null
                            ? x.Inspection.Contragent.FiasMailingAddress.AddressName + (x.Inspection.Contragent.FiasMailingAddress.PostCode != null 
                                ? $", {x.Inspection.Contragent.FiasMailingAddress.PostCode}" 
                                : null)
                            : null,
                        x.Inspection
                    })
                    .Single();

                var headInfo = new
                {
                    Имя = contragentContactDomain.GetAll()
                        .Where(x => x.Contragent == actCheckInfo.Inspection.Contragent)
                        .Where(x => x.Position.Code == "1") // Руководитель
                        .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.HasValue && x.DateEndWork < DateTime.Now)
                        .AggregateWithSeparator(x => x.FullName, ", ")
                };

                var roInfo = new
                {
                    Адрес = inspectionRoDomain.GetAll()
                        .Where(x => x.Inspection.Id == actCheckInfo.Inspection.Id)
                        .AggregateWithSeparator(x => x.RealityObject.Address, ", ")
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ЗональнаяИнспекция",
                    MetaType = nameof(Object),
                    Data = zonalInspectionInfo
                });
                this.DataSources.Add(new MetaData
                {
                    SourceName = "АктПроверки",
                    MetaType = nameof(Object),
                    Data = actCheckInfo
                });
                this.DataSources.Add(new MetaData
                {
                    SourceName = "Руководитель",
                    MetaType = nameof(Object),
                    Data = headInfo
                });
                this.DataSources.Add(new MetaData
                {
                    SourceName = "ОбъектНарушения",
                    MetaType = nameof(Object),
                    Data = roInfo
                });
            }
        }

        /// <inheritdoc />
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <inheritdoc />
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = this.Name,
                    Code = this.Id,
                    Description = this.Description,
                    Template = Resources.ProtocolNotification
                }
            };
        }
    }
}