namespace Bars.GisIntegration.Smev.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Smev.Exporters;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Castle.Windsor;
    using System;
    using System.Linq;

    /// <inheritdoc />
    public class ErpIntegrationService : IErpIntegrationService
    {
        /// <summary>
        /// IoC container
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult SendDisposal(BaseParams baseParams)
        {
            const string disposalInitExporterName = nameof(DisposalExporter);
            const string disposalCorrectionExporterName = nameof(DisposalCorrectionExporter);

            var gisIntegrationService = this.Container.Resolve<IGisIntegrationService>();
            var disposalInitExporter = this.Container.Resolve<IDataExporter>(disposalInitExporterName);
            var tatarstanDisposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();
            var risTaskDomain = this.Container.ResolveDomain<RisTask>();
            using (this.Container.Using(gisIntegrationService, tatarstanDisposalDomain, risTaskDomain, disposalInitExporter))
            {
                var id = baseParams.Params.GetAsId();

                if (!(tatarstanDisposalDomain.Get(id) is TatarstanDisposal disposal))
                {
                    return new BaseDataResult(false);
                }

                var erpId = baseParams.Params.GetAs<string>("erpId");

                // Контрольная дата начала отправки распоряжения
                var compareTime = DateTime.Now.AddSeconds(-disposalInitExporter.Interval * (disposalInitExporter.MaxRepeatCount + 1));

                if (string.IsNullOrEmpty(erpId)
                    && risTaskDomain.GetAll()
                        .Any(x => x.DocumentGji.Id == id
                            && x.TaskState != TaskState.Error
                            && x.TaskState != TaskState.CompleteSuccess
                            && x.TaskState != TaskState.CompleteWithErrors
                            && x.StartTime > compareTime))
                {
                    return new BaseDataResult(false, "Первичное размещение уже выполняется");
                }

                var par = new DynamicDictionary { { "0", new DynamicDictionary { { "id", id }, { "documentGji", disposal } } } };

                var gisParams = new BaseParams();

                if (string.IsNullOrEmpty(erpId))
                {
                    gisParams.Params.SetValue("exporter_Id", disposalInitExporterName);
                }
                else
                {
                    gisParams.Params.SetValue("exporter_Id", disposalCorrectionExporterName);
                }

                gisParams.Params.SetValue("params", par);

                var prepareDataResult = gisIntegrationService.SchedulePrepareData(gisParams);
                if (prepareDataResult.Success)
                {
                    disposal.IsSentToErp = true;
                    tatarstanDisposalDomain.Update(disposal);
                }

                return prepareDataResult;
            }
        }

        /// <inheritdoc />
        public IDataResult RequestProsecutorsOffices(BaseParams baseParams)
        {
            var gisIntegrationService = this.Container.Resolve<IGisIntegrationService>();
            using (this.Container.Using(gisIntegrationService))
            {
                var id = baseParams.Params.GetAsId();
                var par = new DynamicDictionary { { "0", new DynamicDictionary { { "id", id } } } };

                var gisParams = new BaseParams();
                gisParams.Params.SetValue("exporter_Id", nameof(ProsecutorOfficesExport));
                gisParams.Params.SetValue("params", par);

                return gisIntegrationService.SchedulePrepareData(gisParams);
            }
        }
    }
}