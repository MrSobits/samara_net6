namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;

    using Gkh.Authentification;
    using Entities;
    using Utils;

    using Castle.Windsor;

    public class PrescriptionService : IPrescriptionService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult GetInfo(long? documentId)
        {
            var serviceDisposalInspector = this.Container.Resolve<IDocumentGjiInspectorService>();
            var serviceDocChildren = this.Container.ResolveDomain<DocumentGjiChildren>();
            
            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors = serviceDisposalInspector.GetInspectorsByDocumentId(documentId)
                    .Select(x => new
                    {
                        InspectorId = x.Inspector.Id,
                        FIO = x.Inspector.Fio
                    })
                    .ToList();

                foreach (var item in dataInspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.FIO;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.InspectorId.ToString();
                }

                // Пробегаемся по документам на основе которого создано предписание
                var parents = serviceDocChildren.GetAll()
                    .Where(x => x.Children.Id == documentId)
                    .Select(x => new
                    {
                        parentId = x.Parent.Id,
                        x.Parent.TypeDocumentGji,
                        x.Parent.DocumentDate,
                        x.Parent.DocumentNumber
                    })
                    .ToList();

                foreach (var doc in parents)
                {
                    var docName = Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (!string.IsNullOrEmpty(baseName))
                    {
                        baseName += ", ";
                    }

                    baseName += string.Format("{0} №{1} от {2}", docName, doc.DocumentNumber,
                        doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return
                    new BaseDataResult(new PrescriptionGetInfoProxy
                    {
                        inspectorNames = inspectorNames,
                        inspectorIds = inspectorIds,
                        baseName = baseName
                    });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(serviceDisposalInspector);
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
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var showYellow = baseParams.Params.GetAs<bool>("showYellow");
            var showRed = baseParams.Params.GetAs<bool>("showRed");
            bool showboth = false;
            if (showYellow && showRed)
            {
                showboth = true;
            }
            
            var data = this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.InspectorNames,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.DocumentDate,
                        x.ContragentName,
                        x.TypeExecutant,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                        x.CountRealityObject,
                        x.CountViolation,
                        x.InspectionId,
                        x.TypeBase,
                        x.TypeDocumentGji,
                        x.DateRemoval,
                        x.DisposalId,
                        x.ViolationList,
                        x.TypePrescriptionExecution,
                        x.PrescriptionState,
                        x.CancelledGJI,
                        x.AppealDate,
                        x.AppealDescription,
                        x.ControlType,
                        x.HasNotRemoovedViolations,
                        x.AppealNumber
                    })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        /// <summary>
        /// В данном методе идет получение списка актов по Этапу проверки (в этапе может быть больше 1 акта)
        /// </summary>
        public virtual IDataResult ListForStage(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var prescService = this.Container.Resolve<IDomainService<Prescription>>();
            var prescViolService = this.Container.Resolve<IDomainService<PrescriptionViol>>();

            try
            {
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress =
                    prescViolService.GetAll()
                                       .Where(x => x.Document.Stage.Id == stageId)
                                       .Where(x => x.InspectionViolation.RealityObject != null)
                                       .Select(
                                           x =>
                                           new
                                           {
                                               actId = x.Document.Id,
                                               address = x.InspectionViolation.RealityObject.Address
                                           })
                                       .AsEnumerable()
                                       .GroupBy(x => x.actId)
                                       .ToDictionary(x => x.Key, y => y.Select(x => x.address).FirstOrDefault());


                var data =
                    prescService.GetAll()
                                   .Where(x => x.Stage.Id == stageId)
                                   .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber })
                                   .AsEnumerable()
                                   .Select(
                                       x =>
                                       new
                                       {
                                           x.Id,
                                           DocumentId = x.Id,
                                           x.TypeDocumentGji,
                                           x.DocumentDate,
                                           x.DocumentNumber,
                                           Address = dictRoAddress.ContainsKey(x.Id) ? dictRoAddress[x.Id] : null
                                       })
                                   .AsQueryable()
                                   .Filter(loadParam, this.Container)
                                   .Order(loadParam)
                                   .ToList();


                int totalCount = data.Count();

                return new ListDataResult(data, totalCount);
            }
            finally
            {
                this.Container.Release(prescService);
                this.Container.Release(prescViolService);
            }
        }

        public virtual IQueryable<ViewPrescription> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceViewPrescription = this.Container.Resolve<IDomainService<ViewPrescription>>();
            var serviceDocumentInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            try
            {
                var inspectorIds = userManager.GetInspectorIds();
                var municipalityIds = userManager.GetMunicipalityIds();

                return serviceViewPrescription.GetAll()
                    .WhereIf(inspectorIds.Count > 0, y => serviceDocumentInspector.GetAll()
                        .Any(x => x.DocumentGji.Id == y.Id 
                            && inspectorIds.Contains(x.Inspector.Id)))
                    .WhereIf(municipalityIds.Count > 0, x => x.MunicipalityId.HasValue && municipalityIds.Contains((long)x.MunicipalityId));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceViewPrescription);
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
        protected virtual string GetDataExportRegistrationName() => "PrescriptionDataExport";
    }

    public class PrescriptionGetInfoProxy
    {
        public string inspectorNames { get; set; }
        public string inspectorIds { get; set; }
        public string baseName { get; set; }
        public long parentId { get; set; }
    }
}