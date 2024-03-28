namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для запросов в ГИС ГМП
    /// </summary>
    public class GISGMPExecuteController : BaseController
    {
        #region Fields

        private IGISGMPService _GISGMPService;

        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        public IDomainService<GisGmp> GisGmpDomain { get; set; }

        public IDomainService<PayReg> PayRegDomain { get; set; }

        public IDomainService<GISGMPPayments> GISGMPPaymentsDomain { get; set; }

        public IDomainService<GisGmpFile> GisGmpFileDomain { get; set; }

        #endregion

        #region Constructors

        public GISGMPExecuteController(IWindsorContainer container, IFileManager fileManager, IDomainService<FileInfo> fileDomain, IGISGMPService GISGMPService, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _GISGMPService = GISGMPService;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправить начисление
        /// </summary>
        public ActionResult SendCalcRequest(BaseParams baseParams, Int64 taskId)
        {
            GisGmp smevRequestData = GisGmpDomain.Get(taskId);
            if(smevRequestData==null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");
            if (smevRequestData.TypeLicenseRequest == TypeLicenseRequest.NotSet)
            {
                //if (!smevRequestData.Protocol.DocumentDate.HasValue)
                //    return JsFailure("Не указана дата в постановлении");

                //if (string.IsNullOrEmpty(smevRequestData.Protocol.DocumentNumber))
                //    return JsFailure("Не указан номер в поставновлении");
            }

            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", taskId);

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendCalcRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if(taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на отправку начисления поставлена в очередь с id {taskInfo.TaskId}"); 
            }
            catch(Exception e)
            {
                return JsFailure("Ошибка при отправке начисления: "+e.Message);
            }
        }

        /// <summary>
        /// Отправить запрос о начислениях
        /// </summary>
        public ActionResult SendPayRequest(BaseParams baseParams, Int64 taskId)
        {
            GisGmp smevRequestData = GisGmpDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", taskId);

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendPayRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на запрос начислений поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при запросе начисления: " + e.Message);
            }
        }

        /// <summary>
        /// Отправить запрос о квитировании
        /// </summary>
        public ActionResult SendReconcileRequest(BaseParams baseParams, Int64 taskId)
        {
            var gisGmpId = GISGMPPaymentsDomain.Get(taskId).GisGmp.Id;
            GisGmp smevRequestData = GisGmpDomain.Get(gisGmpId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", taskId);

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendReconcileRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на запрос начислений поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при запросе начисления: " + e.Message);
            }
        }

        /// <summary>
        /// Отправить запрос о квитировании всех платежей
        /// </summary>
        public ActionResult SendReconcileAllRequest(BaseParams baseParams, Int64 taskId)
        {
            try
            {
                GisGmp smevRequestData = GisGmpDomain.Get(taskId);

                if (smevRequestData == null)
                    return JsFailure("Запрос не сохранен");

                if (smevRequestData.RequestState == RequestState.Queued)
                    return JsFailure("Запрос с этим начислением уже отправлен");

                // если нет сопоставленных несквитированных платежей, возвращаем сообщение
                if (!PayRegDomain.GetAll()
                    .Where(x => x.GisGmp == smevRequestData && !(x.Reconcile == Gkh.Enums.YesNoNotSet.Yes)).Any())
                {
                    return JsFailure("У начисления отсутствуют сопоставленные несквитированные платежи");
                }

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", taskId);

                try
                {
                    var taskInfo = _taskManager.CreateTasks(new SendReconcileRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                    if (taskInfo == null)
                        return JsFailure("Сбой создания задачи");
                    else
                        return JsSuccess($"Задача на квитирование поставлена в очередь с id {taskInfo.TaskId}");
                }
                catch (Exception e)
                {
                    return JsFailure("Ошибка при запросе начисления: " + e.Message);
                }

                //var gisGmpRepo = this.Container.Resolve<IRepository<GisGmp>>();

                return JsSuccess("Квитация выполнена");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка квитации: " + e.Message);
            }
        }

        /// <summary>
        /// Проверить ответ на запрос
        /// </summary>
        public ActionResult CheckAnswer(BaseParams baseParams, Int64 taskId)
        {
            // все проверки ответа запускают таску на проверку всех ответов
            GisGmp smevRequestData = GisGmpDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            try
            {
                var taskInfo = _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на проверку ответов в СМЭВ поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }

        public ActionResult GetListLicRequest(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IGISGMPService>();
            try
            {
                return resolutionService.GetListLicRequest(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult GetListReissuance(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IGISGMPService>();
            try
            {
                return resolutionService.GetListReissuance(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult GisGmpForPayRegEditWindow(BaseParams baseParams)
        {
            var gisGmpService = Container.Resolve<IGISGMPService>();
            try
            {
                return gisGmpService.ListGisGmp(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<BaseChelyabinsk.Entities.Protocol197.Protocol197> Protocol197Domain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }

        public IDomainService<BaseChelyabinsk.Entities.Protocol197.Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<GISGMPPayerStatus> GISGMPPayerStatusDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<ProtocolRSO> ProtocolRSODomain { get; set; }

        public IDomainService<ProtocolRSOArticleLaw> ProtocolRSOArticleLawDomain { get; set; }

        public IDomainService<ProtocolMhcArticleLaw> ProtocolMhcArticleLawDomain { get; set; }
        public IDomainService<ResolProsArticleLaw> ResolProsArticleLawDomain { get; set; }

        #endregion

        public ActionResult GetPayerInfo(BaseParams baseParams, Int64 protocolData)
        {
            decimal result = 0;
            string reasonVal = "";
            decimal ammount = 0;
            string oktmo = "";
            string innVal = "";
            string kppVal = "";
            string kbk = "";
            string seriesfl = "";
            string numberfl = "";
            PhysicalPersonDocType doctype = null;
            PayerType typeVal = PayerType.Juridical;
            GISGMPPayerStatus payerstate = null;
            DocumentGji doc = DocumentGjiDomain.Get(protocolData);

            try
            {
                if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.Protocol)
                {
                    Protocol entity = ProtocolDomain.Get(protocolData);
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.Description;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;
                            if (entity.PhysicalPersonDocType != null)
                            {
                                doctype = new PhysicalPersonDocType
                                {
                                    Code = entity.PhysicalPersonDocType.Code,
                                    Name = entity.PhysicalPersonDocType.Name,
                                    Id = entity.PhysicalPersonDocType.Id
                                };
                                seriesfl = entity.PhysicalPersonDocumentSerial;
                                numberfl = entity.PhysicalPersonDocumentNumber;
                            }

                        }
                        try
                        {
                            kbk = ProtocolArticleLawDomain.GetAll()
                                   .FirstOrDefault(x => x.Protocol == entity).ArticleLaw.KBK;
                        }
                        catch
                        { }

                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {                           
                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.Protocol197)
                {
                    BaseChelyabinsk.Entities.Protocol197.Protocol197 entity = Protocol197Domain.Get(protocolData);
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.Description;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;
                            if (entity.PhysicalPersonDocType != null)
                            {
                                doctype = new PhysicalPersonDocType
                                {
                                    Code = entity.PhysicalPersonDocType.Code,
                                    Name = entity.PhysicalPersonDocType.Name,
                                    Id = entity.PhysicalPersonDocType.Id
                                };
                                seriesfl = entity.PhysicalPersonDocumentSerial;
                                numberfl = entity.PhysicalPersonDocumentNumber;
                            }

                        }
                        try
                        {
                            kbk = Protocol197ArticleLawDomain.GetAll()
                                   .FirstOrDefault(x => x.Protocol197 == entity).ArticleLaw.KBK;
                        }
                        catch
                        { }

                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {
                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.ProtocolRSO)
                {
                    ProtocolRSO entity = ProtocolRSODomain.Get(protocolData);
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    try
                    {
                        kbk = ProtocolRSOArticleLawDomain.GetAll()
                               .FirstOrDefault(x => x.ProtocolRSO == entity).ArticleLaw.KBK;
                    }
                    catch
                    { }
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.Description;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;

                        }
                        if (resol.Municipality != null)
                        {                         
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {
                            
                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }            
             
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.ProtocolMhc)
                {
                    ProtocolMhc entity = this.Container.Resolve<IDomainService<ProtocolMhc>>().Get(protocolData);
                    try
                    {
                        kbk = ProtocolMhcArticleLawDomain.GetAll()
                               .FirstOrDefault(x => x.ProtocolMhc == entity).ArticleLaw.KBK;
                    }
                    catch
                    { }
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.Description;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;

                        }
                        if (resol.Municipality != null)
                        {                          
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {
                          
                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }
                else if (doc != null && doc.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                {
                    ResolPros entity = this.Container.Resolve<IDomainService<ResolPros>>().Get(protocolData);
                    try
                    {
                        kbk = ResolProsArticleLawDomain.GetAll()
                               .FirstOrDefault(x => x.ResolPros == entity).ArticleLaw.KBK;
                    }
                    catch
                    { }
                    var children = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent != null && x.Parent.Id == protocolData)
                        .Where(x => x.Children != null && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution).FirstOrDefault();
                    if (children != null)
                    {
                        var resol = ResolutionDomain.Get(children.Children.Id);
                        string typeInitiativeOrg = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ? "суда" : "ГЖИ";
                        string number = resol.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? resol.DocumentNumber : resol.Description;
                        string docdate = resol.DocumentDate.HasValue ? resol.DocumentDate.Value.ToShortDateString() : "";
                        reasonVal = $@"Административный штраф по постановлению {typeInitiativeOrg} № {number} от {docdate}";
                        ammount = resol.PenaltyAmount.HasValue ? resol.PenaltyAmount.Value : 0;
                        if (entity.Contragent != null && !entity.Executant.Name.Contains("Должностное"))
                        {
                            if (entity.Contragent.OrganizationForm != null && entity.Contragent.OrganizationForm.OkopfCode == "5 01 02")
                            {
                                typeVal = PayerType.IP;
                            }
                            else
                            {
                                typeVal = PayerType.Juridical;
                            }

                            innVal = entity.Contragent.Inn;
                            kppVal = entity.Contragent.Kpp;

                        }
                        else
                        {
                            typeVal = PayerType.Physical;

                        }
                        if (resol.Municipality != null)
                        {
                            oktmo = resol.Municipality.Oktmo;
                        }
                        if (resol.FineMunicipality != null)
                        {

                            oktmo = resol.FineMunicipality.Oktmo;
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }

            if (typeVal == PayerType.Physical)
            {
                payerstate = GISGMPPayerStatusDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "24");
            }
            if (typeVal == PayerType.Juridical)
            {
                payerstate = GISGMPPayerStatusDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "01");
            }
            if (typeVal == PayerType.IP)
            {
                payerstate = GISGMPPayerStatusDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "08");
            }
            var data = new { innVal = innVal, kppVal = kppVal, typeVal = typeVal, kbk = kbk, oktmo = oktmo, ammount = ammount, reasonVal = reasonVal, doctype, seriesfl, numberfl, payerstate };
            return JsSuccess(data);
        }


    }
}

