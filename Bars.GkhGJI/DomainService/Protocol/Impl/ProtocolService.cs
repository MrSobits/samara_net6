namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Enums;

    using Gkh.Authentification;
    using Entities;
    using Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис протоколов
    /// </summary>
    public class ProtocolService : ProtocolService<Protocol>
    {
    }

    /// <summary>
    /// Базовый сервис протоколов
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    public class ProtocolService<T> : IProtocolService
        where T : Protocol
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string GetFormattedDocumentBase(ProtocolParentDocInfo protocolParentDocInfo) =>
            string.Format("{0} №{1} от {2}",
                Utils.GetDocumentName(protocolParentDocInfo.TypeDocumentGji),
                protocolParentDocInfo.DocumentNumber,
                protocolParentDocInfo.DocumentDate.ToDateTime().ToShortDateString());

        /// <summary>
        /// Получение информации о протоколе
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Информации о протоколе</returns>
        public virtual IDataResult GetInfo(long? documentId)
        {
            var serviceInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var inspectors = serviceInspector.GetAll()
                    .Where(x => x.DocumentGji.Id == documentId)
                    .Select(x => new { x.Inspector.Id, x.Inspector.Fio })
                    .ToList();

                foreach (var item in inspectors)
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

                    inspectorIds += item.Id;
                }

                // Пробегаемся по документам на основе которого создано предписание
                var parents = serviceChildren.GetAll()
                    .Where(x => x.Children.Id == documentId)
                    .Select(x => new ProtocolParentDocInfo
                        {
                            TypeDocumentGji = x.Parent.TypeDocumentGji,
                            DocumentDate = x.Parent.DocumentDate,
                            DocumentNumber = x.Parent.DocumentNumber
                        })
                    .ToList();

                baseName = string.Join(", ", parents.Select(this.GetFormattedDocumentBase));

                return new BaseDataResult(new ProtocolGetInfoProxy { InspectorNames = inspectorNames, InspectorIds = inspectorIds, BaseName = baseName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceInspector);
                this.Container.Release(serviceChildren);
            }
        }

        /// <summary>
        /// Получение списка протоколов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список протоколов</returns>
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
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

            var data = this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.TypeExecutant,
                    x.CountViolation,
                    x.InspectorNames,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.InspectionId,
                    x.TypeBase,
                    x.ControlType,
                    x.TypeDocumentGji,
                    x.ArticleLaw,
                    HasResolution = GetResolution(x.Id, x.DateToCourt)
                })
                .Filter(loadParam, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }


        private bool GetResolution(long protocolId, DateTime? dateToCourt) 
        {
            if (!dateToCourt.HasValue)
            {
                return true;
            }
            if (dateToCourt.Value.AddMonths(2) > DateTime.Now) 
            {
                return true;
            }
            var childrenId = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
             .Where(x => x.Parent.Id == protocolId && x.Children.TypeDocumentGji == Enums.TypeDocumentGji.Resolution)
            .Select(x => x.Children.Id).FirstOrDefault();


            return childrenId > 0;
            
        }



        /// <summary>
        /// В данном методе идет получение списка актов по Этапу проверки (в этапе может быть больше 1 акта)
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult ListForStage(BaseParams baseParams)
        {
            var protService = this.Container.Resolve<IDomainService<T>>();
            var protViolService = this.Container.Resolve<IDomainService<ProtocolViolation>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress =
                    protViolService.GetAll()
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
                    protService.GetAll()
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


                var totalCount = data.Count();

                return new ListDataResult(data, totalCount);
            }
            finally
            {
                this.Container.Release(protService);
                this.Container.Release(protViolService);
            }
        }

        /// <summary>
        /// Получение запроса представлений протоколов
        /// </summary>
        /// <returns>Запрос представлений протоколов</returns>
        public virtual IQueryable<ViewProtocol> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceViewProtocol = this.Container.Resolve<IDomainService<ViewProtocol>>();

            try
            {
                var inspectorList = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds();
                inspectorList.Clear();//убираем проверку на инспектора
                return serviceViewProtocol.GetAll()
                                .WhereIf(inspectorList.Count > 0, y => serviceDocumentInspector.GetAll()
                                    .Any(x => y.Id == x.DocumentGji.Id
                                        && inspectorList.Contains(x.Inspector.Id)))
                                .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceViewProtocol);
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
        protected virtual string GetDataExportRegistrationName() => "ProtocolDataExport";
    }

    /// <summary>
    /// Вспомогательный прокси протокола
    /// </summary>
    public class ProtocolGetInfoProxy
    {
        /// <summary>
        /// Имена инспекторов
        /// </summary>
        public string InspectorNames { get; set; }

        /// <summary>
        /// Идентификаторы инспекторов
        /// </summary>
        public string InspectorIds { get; set; }

        /// <summary>
        /// Базовое имя
        /// </summary>
        public string BaseName { get; set; }
    }

    /// <summary>
    /// Информация о родительском документе протокола
    /// </summary>
    public class ProtocolParentDocInfo
    {
        /// <summary>
        /// Тип документа
        /// </summary>
        public TypeDocumentGji TypeDocumentGji { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }
    }
}