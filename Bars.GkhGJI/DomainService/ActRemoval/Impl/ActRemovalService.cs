namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;

    using Gkh.Authentification;
    using Entities;
    using Enums;

    using Castle.Windsor;

    public class ActRemovalService : IActRemovalService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult GetInfo(long? documentId)
        {
            var serviceInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceDocChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;

                // Пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors = serviceInspector.GetAll()
                    .Where(x => x.DocumentGji.Id == documentId)
                    .Select(x => new
                    {
                        x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .ToList();

                foreach (var item in dataInspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.Fio;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.Id.ToString();
                }

                // Пробегаемся по документам на основе которого создан акт предписания (он же акт устранения нарушений)
                // по полученным ids актов проверки предписаний получаем предписания
                var prescription = serviceDocChildren.GetAll()
                    .Where(x => x.Children.Id == documentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Select(x => new
                    {
                        x.Parent.DocumentDate,
                        x.Parent.DocumentNumber
                    })
                    .FirstOrDefault();

                if (prescription != null)
                {
                    baseName = string.Format(
                        "№{0} от {1}",
                        prescription.DocumentNumber,
                        prescription.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(new { inspectorNames, inspectorIds, baseName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceInspector);
                this.Container.Release(serviceDocChildren);
            }
        }

        public virtual IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);

            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MaxValue);

            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var data = this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ParentContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ParentContragentMuId,
                    ParentContragentName = x.ParentContragent,
                    x.CountExecDoc,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.InspectorNames,
                    x.RealityObjectCount,
                    x.ParentDocumentName,
                    x.TypeRemoval,
                    x.InspectionId,
                    x.TypeBase,
                    x.ControlType,
                    x.TypeDocumentGji
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public virtual IQueryable<ViewActRemoval> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceViewActRemoval = this.Container.Resolve<IDomainService<ViewActRemoval>>();
            
            try
            {
                var inspectorList = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds();

                return serviceViewActRemoval.GetAll()
                    .WhereIf(inspectorList.Count > 0,
                        y => serviceDocumentInspector.GetAll()
                            .Any(x => x.DocumentGji.Id == y.Id && inspectorList.Contains(x.Inspector.Id)))
                    .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains((long)x.MunicipalityId));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceViewActRemoval);
            }
        }

        /// <inheritdoc />
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>(this.GetDataExportRegistrationName());

            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Получить наименование, под которым зарегистрирован сервис
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDataExportRegistrationName() => "ActRemovalDataExport";
    }
}