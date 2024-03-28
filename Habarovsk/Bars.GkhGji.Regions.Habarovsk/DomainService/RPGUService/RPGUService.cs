using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.States;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using Bars.GkhGji.Regions.Habarovsk.OrderRequest;
using Bars.GkhGji.Regions.Habarovsk.RPGULicRequest;
using SMEV3Library.Entities.GetResponseResponse;
using Bars.GkhGji.Regions.Habarovsk.OrderReissuanceRequest;
using Bars.GkhGji.Regions.Habarovsk.CreateOrdersRequest;
using Bars.GkhGji.Regions.Habarovsk.UpdateOrdersRequest;
using SMEV3Library.Services;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using SMEV3Library.Helpers;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Controllers;
using Castle.Windsor;
using Bars.GkhGji.Regions.Habarovsk.Tasks;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class RPGUService : IRPGUService
    {
        #region Constants
        static XNamespace soap = @"http://schemas.xmlsoap.org/soap/envelope/";
        static XNamespace tns = @"urn://ru.sgio.portalinteraction/1.0.0";
        static XNamespace ns1 = @"http://epgu.gosuslugi.ru/ordreg/order/3.1.0";
        static XNamespace ns2 = @"http://epgu.gosuslugi.ru/ordreg/common/3.1.0";
        static XNamespace fnstNamespace = @"urn://x-artefacts-fns/vipul-types/4.0.6";
        static XNamespace sgio = "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2";

        public IDomainService<Contragent> ContragentDomainService { get; set; }
        public IDomainService<ManOrgRequestRPGU> ManOrgRequestRPGUService { get; set; }
        public IDomainService<LicenseReissuanceRPGU> LicenseReissuanceRPGUService { get; set; }

        #endregion

        #region Properties

        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }

        public IDomainService<SMEVEGRULFile> SMEVEGRULFileDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        public IDomainService<Position> PositionDomain { get; set; }
        public IRepository<Contragent> ContragentRepository { get; set; }
        public IDomainService<PersonPlaceWork> PersonPlaceWorkDomain { get; set; }
        public IDomainService<PersonQualificationCertificate> PersonQualificationCertificateDomain { get; set; }
        public IDomainService<Person> PersonDomain { get; set; }
        public IDomainService<LicenseAction> LicenseActionDomain { get; set; }
        public IDomainService<LicenseActionFile> LicenseActionFileDomain { get; set; }
        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }
        public IDomainService<ManOrgLicenseRequest> ManOrgLicenseRequestDomain { get; set; }
        public IDomainService<LicenseReissuance> LicenseReissuanceDomain { get; set; }
        public IDomainService<LicenseReissuancePerson> LicenseReissuancePersonDomain { get; set; }
        public IDomainService<LicenseReissuanceProvDoc> LicenseReissuanceProvDocDomain { get; set; }
        public IDomainService<LicenseProvidedDoc> LicenseProvidedDocDomain { get; set; }
        public IDomainService<ManOrgRequestProvDoc> ManOrgRequestProvDocDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }
        public IDomainService<GisGmp> GISGMPDomain { get; set; }
        public IDomainService<ManOrgRequestAnnex> ManOrgRequestAnnexDomain { get; set; }
        public IDomainService<ManOrgRequestPerson> ManOrgRequestPersonDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        #endregion

        #region Constructors

        public RPGUService(IFileManager fileManager, ISMEV3Service SMEV3Service, ITaskManager taskManager, IWindsorContainer container)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
            _taskManager = taskManager;
            _container = container;
        }

        #endregion

        #region Public methods   

        private string GetTimeStampUuid()
        {
            return GUIDHelper.GenerateTimeBasedGuid().ToString();
        }

        public bool SendLicActionAcceptMessage(long requestId, bool success, IProgressIndicator indicator = null)
        {
            var request = LicenseActionDomain.Get(requestId);
            Stream docStream = _fileManager.GetFile(request.File);
            var xDoc = LoadFromStream(docStream);
            var Xelement = XElement.Parse(xDoc.ToString());
            if (request.LicenseActionType == BaseChelyabinsk.Enums.LicenseActionType.GetLicenseInfo)
            {
                LicenseInfoRequest.orderRequest order = new LicenseInfoRequest.orderRequest();

                try
                {
                    order = DeSerializerLicenseInfoRequest(Xelement);
                }
                catch (Exception e)
                {
                }
                if (request != null && order != null)
                {
                    var orderItem = (LicenseInfoRequest.order)order.Item;
                    //создаем запрос на портале рпгу
                    UpdateOrdersRequest.UpdateOrdersRequest create = new UpdateOrdersRequest.UpdateOrdersRequest();
                    UpdateOrdersRequestOrder createorder = new UpdateOrdersRequestOrder();
                    createorder.organization = new UpdateOrdersRequestOrderOrganization
                    {
                        id = orderItem.data.GJILicenseRequest.info.orgId.ToString(),
                        name = "Государственная жилищная инспекция Воронежской области"
                    };

                    createorder.number = orderItem.number;

                    List<UpdateOrdersRequest.StatusHistoryListState> stateList = new List<UpdateOrdersRequest.StatusHistoryListState>();
                    if (success)
                        stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                        {
                            comment = $"Данные о лицензии {request.LicenseNumber} от {request.LicenseDate.ToString("dd.MM.yyyy")} направлены заявителю",
                            date = DateTime.Now,
                            status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item3
                        });
                    else
                        stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                        {
                            comment = string.IsNullOrEmpty(request.DeclineReason)?  "Заявка отклонена в связи с некорректностью данных в заявке": request.DeclineReason,
                            date = DateTime.Now,
                            status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item4
                        });

                    createorder.states = stateList.ToArray();

                    List<UpdateOrdersRequestOrder> orders = new List<UpdateOrdersRequestOrder>();
                    orders.Add(createorder);
                    create.orders = orders.ToArray();

                    XElement requestElement = GetUpdateRequestElement(create);
                    var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();                  

                    return true;
                }
                return false;
            }
            else 
            {
                LicenseUndoRequest.orderRequest order = new LicenseUndoRequest.orderRequest();

                try
                {
                    order = DeSerializerLicenseUndoRequest(Xelement);
                }
                catch (Exception e)
                {
                }
                if (request != null && order != null)
                {
                    var orderItem = (LicenseUndoRequest.order)order.Item;
                    //создаем запрос на портале рпгу
                    UpdateOrdersRequest.UpdateOrdersRequest create = new UpdateOrdersRequest.UpdateOrdersRequest();
                    UpdateOrdersRequestOrder createorder = new UpdateOrdersRequestOrder();
                    createorder.organization = new UpdateOrdersRequestOrderOrganization
                    {
                        id = orderItem.data.GJILicenseRequest.info.orgId.ToString(),
                        name = "Государственная жилищная инспекция Воронежской области"
                    };

                    createorder.number = orderItem.number;

                    List<UpdateOrdersRequest.StatusHistoryListState> stateList = new List<UpdateOrdersRequest.StatusHistoryListState>();
                    if (success)
                        stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                        {
                            comment = $"Лицензия {request.LicenseNumber} от {request.LicenseDate.ToString("dd.MM.yyyy")} аннулирована",
                            date = DateTime.Now,
                            status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item3
                        });
                    else
                        stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                        {
                            comment = string.IsNullOrEmpty(request.DeclineReason) ? "Заявка отклонена в связи с некорректностью данных в заявке" : request.DeclineReason,
                            //comment = "Заявка отклонена в связи с некорректностью данных в заявке",
                            date = DateTime.Now,
                            status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item4
                        });

                    createorder.states = stateList.ToArray();

                    List<UpdateOrdersRequestOrder> orders = new List<UpdateOrdersRequestOrder>();
                    orders.Add(createorder);
                    create.orders = orders.ToArray();

                    XElement requestElement = GetUpdateRequestElement(create);
                    var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();

                    return true;
                }
                return false;
            }

        }

        public bool SendAcceptMessage(long requestId, bool success, IProgressIndicator indicator = null)
        {
            var request = ManOrgLicenseRequestDomain.Get(requestId);
            Stream docStream = _fileManager.GetFile(request.File);
            var xDoc = LoadFromStream(docStream);
            var Xelement = XElement.Parse(xDoc.ToString());
            OrderRequest.orderRequest order = new OrderRequest.orderRequest();
           
            try
            {
                order = DeSerializerOrder(Xelement);
            }
            catch (Exception e)
            {
            }
            if (request != null && order!=null)
            {
                var orderItem = (OrderRequest.order)order.Item;
                //создаем запрос на портале рпгу
                UpdateOrdersRequest.UpdateOrdersRequest create = new UpdateOrdersRequest.UpdateOrdersRequest();
                UpdateOrdersRequestOrder createorder = new UpdateOrdersRequestOrder();
                createorder.organization = new UpdateOrdersRequestOrderOrganization
                {
                    id = orderItem.data.GJILicenseRequest.info.orgId.ToString(),
                    name = "Государственная жилищная инспекция Воронежской области"
                };
               
                createorder.number = orderItem.number;
              
                List<UpdateOrdersRequest.StatusHistoryListState> stateList = new List<UpdateOrdersRequest.StatusHistoryListState>();
                if (success)
                    stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                    {
                        comment = "Принято решение о выдаче лицензии",
                        date = DateTime.Now,
                        status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item3
                    });
                else
                    stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                    {
                        comment = string.IsNullOrEmpty(request.ReasonRefusal) ? "Заявка отклонена в связи с некорректностью данных в заявке" : request.ReasonRefusal,
                    //    comment = "Заявка отклонена в связи с некорректностью данных в заявке",
                        date = DateTime.Now,
                        status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item4
                    });

                createorder.states = stateList.ToArray();

                List<UpdateOrdersRequestOrder> orders = new List<UpdateOrdersRequestOrder>();
                orders.Add(createorder);
                create.orders = orders.ToArray();

                XElement requestElement = GetUpdateRequestElement(create);
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
                ManOrgRequestRPGU req = new ManOrgRequestRPGU
                {
                    Date = DateTime.Now,
                    LicRequest = new ManOrgLicenseRequest { Id = requestId },
                    MessageId = requestResult.MessageId,
                    File = _fileManager.SaveFile("Request", "xml", requestResult.SendedData),
                    AnswerFile = _fileManager.SaveFile("Responce", "xml", requestResult.ReceivedData),
                    RequestRPGUState = RequestRPGUState.Queued,
                    RequestRPGUType = RequestRPGUType.NotSet,
                    Text = "Отчет об исполнении госуслуги"

                };
                ManOrgRequestRPGUService.Save(req);

                return true;
            }
            return false;
        }
        public bool SendAcceptReissuanceMessage(long requestId, bool success, IProgressIndicator indicator = null)
        {
            var request = LicenseReissuanceDomain.Get(requestId);
            Stream docStream = _fileManager.GetFile(request.File);
            var xDoc = LoadFromStream(docStream);
            var Xelement = XElement.Parse(xDoc.ToString());
            OrderReissuanceRequest.orderRequest order = new OrderReissuanceRequest.orderRequest();
          
            try
            {
                order = DeSerializerOrderReissuance(Xelement);
            }
            catch (Exception e)
            {
            }
            if (request != null && order != null)
            {
                var orderItem = (OrderReissuanceRequest.order)order.Item;
                //создаем запрос на портале рпгу
                UpdateOrdersRequest.UpdateOrdersRequest create = new UpdateOrdersRequest.UpdateOrdersRequest();
                UpdateOrdersRequestOrder createorder = new UpdateOrdersRequestOrder();
                createorder.organization = new UpdateOrdersRequestOrderOrganization
                {
                    id = orderItem.data.GJILicenseRequest.info.orgId.ToString(),
                    name = "Государственная жилищная инспекция Воронежской области"
                };

                createorder.number = orderItem.number;

                List<UpdateOrdersRequest.StatusHistoryListState> stateList = new List<UpdateOrdersRequest.StatusHistoryListState>();
                if (success)
                    stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                    {
                        comment = "Принято решение о переоформлении лицензии",
                        date = DateTime.Now,
                        status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item3
                    });
                else
                    stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                    {
                        comment = string.IsNullOrEmpty(request.DeclineReason) ? "Заявка отклонена в связи с некорректностью данных в заявке" : request.DeclineReason,
                        //comment = "Заявка отклонена в связи с некорректностью данных в заявке",
                        date = DateTime.Now,
                        status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item4
                    });

                createorder.states = stateList.ToArray();

                List<UpdateOrdersRequestOrder> orders = new List<UpdateOrdersRequestOrder>();
                orders.Add(createorder);
                create.orders = orders.ToArray();

                XElement requestElement = GetUpdateRequestElement(create);
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
                try
                {
                    LicenseReissuanceRPGU req = new LicenseReissuanceRPGU
                    {
                        Date = DateTime.Now,
                        LicRequest = new LicenseReissuance { Id = requestId },
                        File = _fileManager.SaveFile("Request", "xml", requestResult.SendedData),
                        AnswerFile = _fileManager.SaveFile("Responce", "xml", requestResult.ReceivedData),
                        MessageId = "",//requestResult.MessageId,
                        RequestRPGUState = RequestRPGUState.Queued,
                        RequestRPGUType = RequestRPGUType.NotSet,
                        Text = "Отчет об исполнении госуслуги"

                    };
                    LicenseReissuanceRPGUService.Save(req);
                }
                catch
                { }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessResponse(SMEV3Library.Entities.GetResponseResponse.GetRequestRequest requestData, IProgressIndicator indicator = null)
        {
            
            try
            {
                //     
               // _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                GetRequestResponse sgioRequest = new GetRequestResponse();
               // var request = requestData.FullMessageElement.Element(soap + "Body").Element(sgio + "GetRequestResponse");
                var request = requestData.FullMessageElement.Element(sgio + "GetRequestResponse");
                sgioRequest = DeSerializer(request);
                indicator?.Report(null, 50, "Десериализация");
                var message = XElement.Parse(sgioRequest.RequestMessage.Request.SenderProvidedRequestData.MessagePrimaryContent.OuterXml);

                //XmlDocument doc = new XmlDocument();
                //doc.Load("D:\\or\\getlic.xml");
                //XElement mpc = XElement.Parse(doc.InnerXml);

                //XmlDocument doccorrect = new XmlDocument();
                //doccorrect.Load("D:\\or\\getliccorrect.xml");
                //XElement mpccorrect = XElement.Parse(doccorrect.InnerXml);

                OrderRequest.orderRequest order = new OrderRequest.orderRequest();
                try
                {
                    order = DeSerializerOrder(message);
                   // order = DeSerializerOrder(mpc);
                }
                catch (Exception e)
                {
                    
                }
                //try
                //{
                //    //order = DeSerializerOrder(message);
                //    order = DeSerializerOrder(mpccorrect);
                //}
                //catch (Exception e)
                //{

                //}
                //проверяем не отмена ли
                try
                {
                    var cancellationItem = (OrderRequest.cancellation)order.Item;
                    if (cancellationItem != null)
                    {
                        if (cancellationItem.number.Contains("gji-forms_prekrashchenie_"))
                        {
                            _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();

                            var licAct = LicenseActionDomain.GetAll().FirstOrDefault(x => cancellationItem.number.Contains(x.RPGUNumber));
                            if (licAct != null)
                            {
                                Stream docStream = _fileManager.GetFile(licAct.File);
                                var xDoc = LoadFromStream(docStream);
                                var Xelement = XElement.Parse(xDoc.ToString());

                                LicenseUndoRequest.orderRequest undoOrder = new LicenseUndoRequest.orderRequest();

                                try
                                {
                                    undoOrder = DeSerializerLicenseUndoRequest(Xelement);
                                }
                                catch (Exception e)
                                {
                                }
                                if (request != null && order != null)
                                {
                                    var undoorderItem = (LicenseUndoRequest.order)undoOrder.Item;
                                    //создаем запрос на портале рпгу
                                    UpdateOrdersRequest.UpdateOrdersRequest create = new UpdateOrdersRequest.UpdateOrdersRequest();
                                    UpdateOrdersRequestOrder createorder = new UpdateOrdersRequestOrder();
                                    createorder.organization = new UpdateOrdersRequestOrderOrganization
                                    {
                                        id = undoorderItem.data.GJILicenseRequest.info.orgId.ToString(),
                                        name = "Государственная жилищная инспекция Воронежской области"
                                    };

                                    createorder.number = cancellationItem.number;

                                    List<UpdateOrdersRequest.StatusHistoryListState> stateList = new List<UpdateOrdersRequest.StatusHistoryListState>();
                                    stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                                    {
                                        comment = "Заявление отозвано пользователем портала",
                                        date = DateTime.Now,
                                        status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item15
                                    });

                                    createorder.states = stateList.ToArray();

                                    List<UpdateOrdersRequestOrder> orders = new List<UpdateOrdersRequestOrder>();
                                    orders.Add(createorder);
                                    create.orders = orders.ToArray();

                                    XElement requestElement = GetUpdateRequestElement(create);
                                    var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
                                }
                            }
                        }
                        else if (cancellationItem.number.Contains("gji-forms_svedeniya_"))
                        {
                            _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();

                            var licAct = LicenseActionDomain.GetAll().FirstOrDefault(x => cancellationItem.number.Contains(x.RPGUNumber));
                            if (licAct != null)
                            {
                                Stream docStream = _fileManager.GetFile(licAct.File);
                                var xDoc = LoadFromStream(docStream);
                                var Xelement = XElement.Parse(xDoc.ToString());

                                LicenseUndoRequest.orderRequest undoOrder = new LicenseUndoRequest.orderRequest();

                                try
                                {
                                    undoOrder = DeSerializerLicenseUndoRequest(Xelement);
                                }
                                catch (Exception e)
                                {
                                }
                                if (request != null && order != null)
                                {
                                    var undoorderItem = (LicenseUndoRequest.order)undoOrder.Item;
                                    //создаем запрос на портале рпгу
                                    UpdateOrdersRequest.UpdateOrdersRequest create = new UpdateOrdersRequest.UpdateOrdersRequest();
                                    UpdateOrdersRequestOrder createorder = new UpdateOrdersRequestOrder();
                                    createorder.organization = new UpdateOrdersRequestOrderOrganization
                                    {
                                        id = undoorderItem.data.GJILicenseRequest.info.orgId.ToString(),
                                        name = "Государственная жилищная инспекция Воронежской области"
                                    };

                                    createorder.number = cancellationItem.number;

                                    List<UpdateOrdersRequest.StatusHistoryListState> stateList = new List<UpdateOrdersRequest.StatusHistoryListState>();
                                    stateList.Add(new UpdateOrdersRequest.StatusHistoryListState
                                    {
                                        comment = "Заявление отозвано пользователем портала",
                                        date = DateTime.Now,
                                        status = UpdateOrdersRequest.StatusHistoryListStateStatus.Item15
                                    });

                                    createorder.states = stateList.ToArray();

                                    List<UpdateOrdersRequestOrder> orders = new List<UpdateOrdersRequestOrder>();
                                    orders.Add(createorder);
                                    create.orders = orders.ToArray();

                                    XElement requestElement = GetUpdateRequestElement(create);
                                    var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                { }

                var orderItem = (OrderRequest.order)order.Item;
                if (orderItem.data.GJILicenseRequest.info.procedureId == 3600000000164848729)
                {
                    _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();
                    //получение лицензии
                    CreateLicenseRequest(order, orderItem, requestData, message, sgioRequest);
                }
                else if (orderItem.data.GJILicenseRequest.info.procedureId == 3600000000164848730)
                {
                   _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();
                    //переоформление лицензии
                    var orderR = DeSerializerOrderReissuance(message);
                  //  var orderR = DeSerializerOrderReissuance(mpc);
                    var orderItemR = (OrderReissuanceRequest.order)orderR.Item;
                    CreateLicenseReissuanceRequest(orderR, orderItemR, requestData, message, sgioRequest);
                }
                else if (orderItem.data.GJILicenseRequest.info.procedureId == 3600000000164848734)
                {
                   _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();
                    //переоформление лицензии
                    var orderR = DeSerializerLicenseInfoRequest(message);
                   //  var orderR = DeSerializerLicenseInfoRequest(mpc);
                    var orderItemR = (LicenseInfoRequest.order)orderR.Item;
                    CreateGetLicenseInfoRequest(orderR, orderItemR, requestData, message, sgioRequest);
                }
                else if (orderItem.data.GJILicenseRequest.info.procedureId == 3600000000164848733)
                {
                   _SMEV3Service.GetAckAsyncSGIO(requestData.MessageId, true).GetAwaiter().GetResult();
                    //переоформление лицензии
                      var orderR = DeSerializerLicenseUndoRequest(message);
                //    var orderR = DeSerializerLicenseUndoRequest(mpc);
                    var orderItemR = (LicenseUndoRequest.order)orderR.Item;
                    CreateGetLicenseUndoRequest(orderR, orderItemR, requestData, message, sgioRequest);
                }

            }
            catch (Exception e)
            {
                
            }

            return true;
        }

        private void CreateGetLicenseUndoRequest(LicenseUndoRequest.orderRequest order, LicenseUndoRequest.order orderItem, SMEV3Library.Entities.GetResponseResponse.GetRequestRequest requestData, XElement message, GetRequestResponse sgioRequest)
        {
            long reqId = 0;
            string regnumberRPGU = order.id.ToString();
            if (orderItem.data.GJILicenseRequest.informationdeclarerul.innul == null || orderItem.data.GJILicenseRequest.license.licenseNumber == null)
            {
                SendDeclineResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU, "В запросе отсутствуют необходимые сведения (ИНН, ОГРН, номер лицензии)");
                //создаем запрос на портале рпгу
                CreateOrdersRequest.CreateOrdersRequest createDecline = new CreateOrdersRequest.CreateOrdersRequest();
                CreateOrdersRequestOrder createorderDecline = new CreateOrdersRequestOrder();
                createorderDecline.applicant = new CreateOrdersRequestOrderApplicant
                {
                    agreement = true,
                    email = orderItem.applicant.email,
                    esiaId = orderItem.applicant.esiaId,
                    firstName = orderItem.applicant.firstName,
                    inn = orderItem.applicant.inn,
                    lastName = orderItem.applicant.lastName,
                    middleName = orderItem.applicant.middleName,
                    phone = orderItem.applicant.phone,
                    snils = orderItem.applicant.snils,
                    type = GetApplicantType(orderItem.applicant.type),
                };
                createorderDecline.date = DateTime.Now;
                createorderDecline.number = orderItem.number;
                createorderDecline.service = new CreateOrdersRequestOrderService
                {
                    id = Convert.ToInt64(orderItem.service.id),
                    okato = orderItem.service.okato.ToString(),
                    procedure = Convert.ToInt64(orderItem.service.procedure),
                    procedureSpecified = true,
                    target = Convert.ToInt64(orderItem.service.target),
                    targetSpecified = true
                };
                List<CreateOrdersRequest.StatusHistoryListState> stateListDecline = new List<CreateOrdersRequest.StatusHistoryListState>();
                stateListDecline.Add(new CreateOrdersRequest.StatusHistoryListState
                {
                    comment = "Отклонено АИС ГЖИ в связи с отсутствием необходимых данных",
                    date = DateTime.Now,
                    status = CreateOrdersRequest.StatusHistoryListStateStatus.Item4
                });

                createorderDecline.states = stateListDecline.ToArray();

                List<CreateOrdersRequestOrder> ordersDecline = new List<CreateOrdersRequestOrder>();
                ordersDecline.Add(createorderDecline);
                createDecline.orders = ordersDecline.ToArray();

                XElement requestElementDecline = GetRequestElement(createDecline);
                var requestResultDecline = _SMEV3Service.SendRequestAsyncSGIO(requestElementDecline, null, true).GetAwaiter().GetResult();
                return;
            }
            var inn = orderItem.data.GJILicenseRequest.informationdeclarerul.innul;
            var ogrn = orderItem.data.GJILicenseRequest.informationdeclarerul.ogrnul;
            var licnumber = orderItem.data.GJILicenseRequest.license.licenseNumber;
            var licdate = orderItem.data.GJILicenseRequest.license.licenseDate;
            if (string.IsNullOrEmpty(inn) || string.IsNullOrEmpty(ogrn) || string.IsNullOrEmpty(licnumber))
            {
                SendDeclineResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU, "В запросе отсутствуют необходимые сведения (ИНН, ОГРН, Номер лицензии)");
                //создаем запрос на портале рпгу
                CreateOrdersRequest.CreateOrdersRequest createDecline = new CreateOrdersRequest.CreateOrdersRequest();
                CreateOrdersRequestOrder createorderDecline = new CreateOrdersRequestOrder();
                createorderDecline.applicant = new CreateOrdersRequestOrderApplicant
                {
                    agreement = true,
                    email = orderItem.applicant.email,
                    esiaId = orderItem.applicant.esiaId,
                    firstName = orderItem.applicant.firstName,
                    inn = orderItem.applicant.inn,
                    lastName = orderItem.applicant.lastName,
                    middleName = orderItem.applicant.middleName,
                    phone = orderItem.applicant.phone,
                    snils = orderItem.applicant.snils,
                    type = GetApplicantType(orderItem.applicant.type),
                };
                createorderDecline.date = DateTime.Now;
                createorderDecline.number = orderItem.number;
                createorderDecline.service = new CreateOrdersRequestOrderService
                {
                    id = Convert.ToInt64(orderItem.service.id),
                    okato = orderItem.service.okato.ToString(),
                    procedure = Convert.ToInt64(orderItem.service.procedure),
                    procedureSpecified = true,
                    target = Convert.ToInt64(orderItem.service.target),
                    targetSpecified = true
                };
                List<CreateOrdersRequest.StatusHistoryListState> stateListDecline = new List<CreateOrdersRequest.StatusHistoryListState>();
                stateListDecline.Add(new CreateOrdersRequest.StatusHistoryListState
                {
                    comment = "Отклонено АИС ГЖИ в связи с отсутствием необходимых данных",
                    date = DateTime.Now,
                    status = CreateOrdersRequest.StatusHistoryListStateStatus.Item4
                });

                createorderDecline.states = stateListDecline.ToArray();

                List<CreateOrdersRequestOrder> ordersDecline = new List<CreateOrdersRequestOrder>();
                ordersDecline.Add(createorderDecline);
                createDecline.orders = ordersDecline.ToArray();

                XElement requestElementDecline = GetRequestElement(createDecline);
                var requestResultDecline = _SMEV3Service.SendRequestAsyncSGIO(requestElementDecline, null, true).GetAwaiter().GetResult();
                return;
            }
            Contragent contragent = ContragentRepository.GetAll()
                .FirstOrDefault(x => x.Inn == inn && x.Ogrn == ogrn);
            if (contragent == null)
            {
                contragent = new Contragent
                {
                    Ogrn = ogrn,
                    Inn = inn,
                    Name = orderItem.data.GJILicenseRequest.informationdeclarerul.fullnameul,
                    ShortName = orderItem.data.GJILicenseRequest.informationdeclarerul.fullnameul,
                    Email = orderItem.data.GJILicenseRequest.informationdeclarerul.emailul,
                    IsEDSE = true,
                    IsSOPR = true,
                    ContragentState = ContragentState.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    Description = "Создан из заявки на аннулирование лицензии",
                    TaxRegistrationIssuedBy = "Не указано"
                };
                ContragentRepository.Save(contragent);
            }


            if (1 == 2)
            {
                //запрос на выдачу не создаем
            }
            else
            {
                LicenseAction licrequest = null;
                int registernum = GetNewNumber();
                var rpgustate = StateDomain.GetAll()
              .Where(x => x.TypeId == "gji_license_action" && x.Code == "РПГУ").FirstOrDefault();
                string pp = "Нет данных";
                licrequest = new LicenseAction
                {
                    Contragent = contragent,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    State = rpgustate,
                    ApplicantAgreement = orderItem.data.GJILicenseRequest.agreement.conditions_agreement ? "Подтвердил согласие на обработку персданных" : "Не подтвердил согласие на обработку персданных",
                    ApplicantEmail = orderItem.applicant.email,
                    ApplicantEsiaId = orderItem.applicant.esiaId,
                    ApplicantFirstName = orderItem.applicant.firstName,
                    ApplicantLastName = orderItem.applicant.lastName,
                    ApplicantMiddleName = orderItem.applicant.middleName,
                    ApplicantInn = orderItem.applicant.inn,
                    Address = orderItem.data.GJILicenseRequest.feedback.feedbackPostAddress,
                    TypeAnswer = "Отправить на почтовый адрес",
                    ApplicantPhone = orderItem.applicant.phone,
                    ApplicantSnils = orderItem.applicant.snils,
                    ApplicantType = orderItem.applicant.type == LicenseUndoRequest.type.FL ? "ФЛ" : orderItem.applicant.type == LicenseUndoRequest.type.UL ? "ЮЛ" : "ИП",
                    LicenseActionType = BaseChelyabinsk.Enums.LicenseActionType.GetLicenseCancelInfo,
                    MiddleNameFl = orderItem.data.GJILicenseRequest.informationdeclarerfl.midlenamefl,
                    NameFl = orderItem.data.GJILicenseRequest.informationdeclarerfl.namefl,
                    SurnameFl = orderItem.data.GJILicenseRequest.informationdeclarerfl.surnamefl,
                    Position = orderItem.data.GJILicenseRequest.informationdeclarerfl.position,
                    DocumentDate = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentDate,
                    DocumentNumber = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentNumber,
                    DocumentName = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentName,
                    DocumentSeries = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentSeries,
                    DocumentIssuer = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentIssuer,
                    LicenseDate = licdate,
                    LicenseNumber = licnumber,
                    MessageId = requestData.MessageId,
                    ReplyTo = requestData.ReplyTo,
                    RPGUNumber = regnumberRPGU
                };
                LicenseActionDomain.Save(licrequest);
                XDocument doc = new XDocument(message);
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.Indent = true;

                    using (XmlWriter xw = XmlWriter.Create(ms, xws))
                    {
                        doc.WriteTo(xw);
                    }

                    licrequest.File = _fileManager.SaveFile(ms, "order.xml");
                }
                LicenseActionDomain.Update(licrequest);
                try
                {
                    reqId = licrequest.Id;
                    var anotherFile = orderItem.data.GJILicenseRequest.files.anotherFiles;
                    Bars.B4.Modules.FileStorage.FileInfo another = null;
                    foreach (var attachment in sgioRequest.RequestMessage.AttachmentContentList)
                    {
                        if (attachment != null)
                        {
                            string convertedId = attachment.Id;
                            if (!string.IsNullOrEmpty(convertedId))
                            {
                                convertedId = convertedId.Replace('_', '/');
                            }
                            if (anotherFile != null)
                            {
                                if (anotherFile.file.Value.Contains(convertedId))
                                {
                                    another = _fileManager.SaveFile(anotherFile.file.filename, attachment.Content);
                                }
                            }

                        }
                    }
                    if (another != null)
                    {
                        try
                        {
                            LicenseActionFileDomain.Save(new LicenseActionFile
                            {
                                FileInfo = another,
                                FileName = string.IsNullOrEmpty(anotherFile.file.name)? anotherFile.file.filename: anotherFile.file.name,
                                LicenseAction = licrequest
                            });
                        }
                        catch
                        { }
                    }
                    if (requestData.Attachments != null && requestData.Attachments.Count > 0)
                    {
                        foreach (var attmnt in requestData.Attachments)
                        {
                            if (attmnt.FileData != null)
                            {
                                LicenseActionFileDomain.Save(new LicenseActionFile
                                {
                                    FileInfo = _fileManager.SaveFile(attmnt.FileName, attmnt.FileData),
                                    FileName = attmnt.FileName,
                                    SignedInfo = attmnt.SignerInfo,
                                    LicenseAction = licrequest,
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectEditDate = DateTime.Now
                                });
                            }
                        }
                    }
                }
                catch
                { }

            }
            //отправляем ответ в РПГУ
            SendResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU);

            //создаем запрос на портале рпгу
            CreateOrdersRequest.CreateOrdersRequest create = new CreateOrdersRequest.CreateOrdersRequest();
            CreateOrdersRequestOrder createorder = new CreateOrdersRequestOrder();
            createorder.applicant = new CreateOrdersRequestOrderApplicant
            {
                agreement = true,
                email = orderItem.applicant.email,
                esiaId = orderItem.applicant.esiaId,
                firstName = orderItem.applicant.firstName,
                inn = orderItem.applicant.inn,
                lastName = orderItem.applicant.lastName,
                middleName = orderItem.applicant.middleName,
                phone = orderItem.applicant.phone,
                snils = orderItem.applicant.snils,
                type = GetApplicantType(orderItem.applicant.type),
            };
            createorder.date = DateTime.Now;
            createorder.number = orderItem.number;
            createorder.service = new CreateOrdersRequestOrderService
            {
                id = Convert.ToInt64(orderItem.service.id),
                okato = orderItem.service.okato.ToString(),
                procedure = Convert.ToInt64(orderItem.service.procedure),
                procedureSpecified = true,
                target = Convert.ToInt64(orderItem.service.target),
                targetSpecified = true
            };
            List<CreateOrdersRequest.StatusHistoryListState> stateList = new List<CreateOrdersRequest.StatusHistoryListState>();
            stateList.Add(new CreateOrdersRequest.StatusHistoryListState
            {
                comment = "Принято в АИС ГЖИ",
                date = DateTime.Now,
                status = CreateOrdersRequest.StatusHistoryListStateStatus.Item2
            });

            createorder.states = stateList.ToArray();

            List<CreateOrdersRequestOrder> orders = new List<CreateOrdersRequestOrder>();
            orders.Add(createorder);
            create.orders = orders.ToArray();

            XElement requestElement = GetRequestElement(create);
            var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
            //LicenseReissuanceRPGU req = new LicenseReissuanceRPGU
            //{
            //    Date = DateTime.Now,
            //    LicRequest = new LicenseReissuance { Id = reqId },
            //    MessageId = requestResult.MessageId,
            //    RequestRPGUState = RequestRPGUState.Queued,
            //    RequestRPGUType = RequestRPGUType.NotSet,
            //    Text = "Отчет о приеме заявки в работу"

            //};
            //LicenseReissuanceRPGUService.Save(req);
        }
        private void CreateGetLicenseInfoRequest(LicenseInfoRequest.orderRequest order, LicenseInfoRequest.order orderItem, SMEV3Library.Entities.GetResponseResponse.GetRequestRequest requestData, XElement message, GetRequestResponse sgioRequest)
        {
            long reqId = 0;
            string regnumberRPGU = order.id.ToString();
            if (orderItem.data.GJILicenseRequest.license.licenseNumber == null)
            {
                SendDeclineResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU, "В запросе отсутствуют необходимые сведения (номер лицензии)");
                //создаем запрос на портале рпгу
                CreateOrdersRequest.CreateOrdersRequest createDecline = new CreateOrdersRequest.CreateOrdersRequest();
                CreateOrdersRequestOrder createorderDecline = new CreateOrdersRequestOrder();
                createorderDecline.applicant = new CreateOrdersRequestOrderApplicant
                {
                    agreement = true,
                    email = orderItem.applicant.email,
                    esiaId = orderItem.applicant.esiaId,
                    firstName = orderItem.applicant.firstName,
                    inn = orderItem.applicant.inn,
                    lastName = orderItem.applicant.lastName,
                    middleName = orderItem.applicant.middleName,
                    phone = orderItem.applicant.phone,
                    snils = orderItem.applicant.snils,
                    type = GetApplicantType(orderItem.applicant.type),
                };
                createorderDecline.date = DateTime.Now;
                createorderDecline.number = orderItem.number;
                createorderDecline.service = new CreateOrdersRequestOrderService
                {
                    id = Convert.ToInt64(orderItem.service.id),
                    okato = orderItem.service.okato.ToString(),
                    procedure = Convert.ToInt64(orderItem.service.procedure),
                    procedureSpecified = true,
                    target = Convert.ToInt64(orderItem.service.target),
                    targetSpecified = true
                };
                List<CreateOrdersRequest.StatusHistoryListState> stateListDecline = new List<CreateOrdersRequest.StatusHistoryListState>();
                stateListDecline.Add(new CreateOrdersRequest.StatusHistoryListState
                {
                    comment = "Отклонено АИС ГЖИ в связи с отсутствием необходимых данных",
                    date = DateTime.Now,
                    status = CreateOrdersRequest.StatusHistoryListStateStatus.Item4
                });

                createorderDecline.states = stateListDecline.ToArray();

                List<CreateOrdersRequestOrder> ordersDecline = new List<CreateOrdersRequestOrder>();
                ordersDecline.Add(createorderDecline);
                createDecline.orders = ordersDecline.ToArray();

                XElement requestElementDecline = GetRequestElement(createDecline);
                var requestResultDecline = _SMEV3Service.SendRequestAsyncSGIO(requestElementDecline, null, true).GetAwaiter().GetResult();
                return;
            }
            var inn = orderItem.data.GJILicenseRequest.informationdeclarer.innul;
            var ogrn = orderItem.data.GJILicenseRequest.informationdeclarer.ogrnul;
            var licnumber = orderItem.data.GJILicenseRequest.license.licenseNumber;
            var licdate = orderItem.data.GJILicenseRequest.license.licenseDate;
            //if (string.IsNullOrEmpty(inn) || string.IsNullOrEmpty(ogrn) || string.IsNullOrEmpty(licnumber))
            //{
            //    SendDeclineResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU, "В запросе отсутствуют необходимые сведения (ИНН, ОГРН, Номер лицензии)");
            //    //создаем запрос на портале рпгу
            //    CreateOrdersRequest.CreateOrdersRequest createDecline = new CreateOrdersRequest.CreateOrdersRequest();
            //    CreateOrdersRequestOrder createorderDecline = new CreateOrdersRequestOrder();
            //    createorderDecline.applicant = new CreateOrdersRequestOrderApplicant
            //    {
            //        agreement = true,
            //        email = orderItem.applicant.email,
            //        esiaId = orderItem.applicant.esiaId,
            //        firstName = orderItem.applicant.firstName,
            //        inn = orderItem.applicant.inn,
            //        lastName = orderItem.applicant.lastName,
            //        middleName = orderItem.applicant.middleName,
            //        phone = orderItem.applicant.phone,
            //        snils = orderItem.applicant.snils,
            //        type = GetApplicantType(orderItem.applicant.type),
            //    };
            //    createorderDecline.date = DateTime.Now;
            //    createorderDecline.number = orderItem.number;
            //    createorderDecline.service = new CreateOrdersRequestOrderService
            //    {
            //        id = Convert.ToInt64(orderItem.service.id),
            //        okato = orderItem.service.okato.ToString(),
            //        procedure = Convert.ToInt64(orderItem.service.procedure),
            //        procedureSpecified = true,
            //        target = Convert.ToInt64(orderItem.service.target),
            //        targetSpecified = true
            //    };
            //    List<CreateOrdersRequest.StatusHistoryListState> stateListDecline = new List<CreateOrdersRequest.StatusHistoryListState>();
            //    stateListDecline.Add(new CreateOrdersRequest.StatusHistoryListState
            //    {
            //        comment = "Отклонено АИС ГЖИ в связи с отсутствием необходимых данных",
            //        date = DateTime.Now,
            //        status = CreateOrdersRequest.StatusHistoryListStateStatus.Item4
            //    });

            //    createorderDecline.states = stateListDecline.ToArray();

            //    List<CreateOrdersRequestOrder> ordersDecline = new List<CreateOrdersRequestOrder>();
            //    ordersDecline.Add(createorderDecline);
            //    createDecline.orders = ordersDecline.ToArray();

            //    XElement requestElementDecline = GetRequestElement(createDecline);
            //    var requestResultDecline = _SMEV3Service.SendRequestAsyncSGIO(requestElementDecline, null, true).GetAwaiter().GetResult();
            //    return;
            //}
            Contragent contragent = ContragentRepository.GetAll()
                .FirstOrDefault(x => x.Inn == inn && x.Ogrn == ogrn);
            if (contragent == null && !string.IsNullOrEmpty(inn) && !string.IsNullOrEmpty(ogrn))
            {
                contragent = new Contragent
                {
                    Ogrn = ogrn,
                    Inn = inn,
                    Name = orderItem.data.GJILicenseRequest.informationdeclarer.fullnameul,
                    ShortName = orderItem.data.GJILicenseRequest.informationdeclarer.fullnameul,
                    Email = orderItem.data.GJILicenseRequest.informationdeclarer.emailul,
                    JuridicalAddress = orderItem.data.GJILicenseRequest.informationdeclarer.placecoordinat,
                    IsEDSE = true,
                    IsSOPR = true,
                    ContragentState = ContragentState.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    Description = "Создан из заявки на получение сведений о конкретной лицензии",
                    TaxRegistrationIssuedBy = "Не указано"
                };
                ContragentRepository.Save(contragent);
            }         
               
         
            if (1 == 2)
            {
                //запрос на выдачу не создаем
            }
            else
            {
                LicenseAction licrequest = null;
                int registernum = GetNewNumber();
                var rpgustate = StateDomain.GetAll()
              .Where(x => x.TypeId == "gji_license_action" && x.Code == "РПГУ").FirstOrDefault();
                string pp = orderItem.data.GJILicenseRequest.dutypay.dutypayNumber;
                string typeAnswer = "";
                typeAnswer += orderItem.data.GJILicenseRequest.agreement.sendEmail ? "По электронной почте" : "";
                if (orderItem.data.GJILicenseRequest.agreement.sendPost)
                {
                    typeAnswer += string.IsNullOrEmpty(typeAnswer) ? "Отправить на почтовый адрес" : ", отправить на почтовый адес";
                }
                licrequest = new LicenseAction
                {
                    Contragent = contragent,                  
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    State = rpgustate,
                    ApplicantAgreement = orderItem.data.GJILicenseRequest.agreement.conditions_agreement? "Подтвердил согласие на обработку персданных":"Не подтвердил согласие на обработку персданных",
                    ApplicantEmail = string.IsNullOrEmpty(orderItem.applicant.email)? orderItem.data.GJILicenseRequest.informationdeclarer.emailul: orderItem.applicant.email,
                    ApplicantEsiaId = orderItem.applicant.esiaId,
                    ApplicantFirstName = orderItem.applicant.firstName,
                    ApplicantLastName = orderItem.applicant.lastName,
                    ApplicantMiddleName = orderItem.applicant.middleName,   
                    ApplicantInn = orderItem.applicant.inn,
                    ApplicantPhone = orderItem.applicant.phone,
                    ApplicantSnils = orderItem.applicant.snils,
                    ApplicantType = orderItem.applicant.type == LicenseInfoRequest.type.FL? "ФЛ": orderItem.applicant.type == LicenseInfoRequest.type.UL ?"ЮЛ":"ИП",
                    LicenseActionType = BaseChelyabinsk.Enums.LicenseActionType.GetLicenseInfo,
                    MiddleNameFl = orderItem.applicant.middleName,
                    NameFl = orderItem.applicant.firstName,
                    SurnameFl = orderItem.applicant.lastName,
                    Position = "Не указана",
                    LicenseDate = licdate,
                    LicenseNumber = licnumber,
                    Address = orderItem.data.GJILicenseRequest.agreement.postAddress,
                    TypeAnswer = typeAnswer,
                    MessageId = requestData.MessageId,
                    ReplyTo = requestData.ReplyTo,
                    RPGUNumber = regnumberRPGU
                };
                LicenseActionDomain.Save(licrequest);
                XDocument doc = new XDocument(message);
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.Indent = true;

                    using (XmlWriter xw = XmlWriter.Create(ms, xws))
                    {
                        doc.WriteTo(xw);
                    }

                    licrequest.File = _fileManager.SaveFile(ms, "order.xml");
                }
                LicenseActionDomain.Update(licrequest);

                reqId = licrequest.Id;
                if (requestData.Attachments != null && requestData.Attachments.Count > 0)
                {
                    foreach (var attmnt in requestData.Attachments)
                    {
                        if (attmnt.FileData != null)
                        {
                            LicenseActionFileDomain.Save(new LicenseActionFile
                            {
                                FileInfo = _fileManager.SaveFile(attmnt.FileName, attmnt.FileData),
                                FileName = attmnt.FileName,
                                SignedInfo = attmnt.SignerInfo,
                                LicenseAction = licrequest,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now
                            });
                        }
                    }
                }
                //var anotherFile = orderItem.data.GJILicenseRequest.files.anotherFiles;
                //Bars.B4.Modules.FileStorage.FileInfo another = null;
                //foreach (var attachment in sgioRequest.RequestMessage.AttachmentContentList)
                //{
                //    if (attachment != null)
                //    {
                //        string convertedId = attachment.Id;
                //        if (!string.IsNullOrEmpty(convertedId))
                //        {
                //            convertedId = convertedId.Replace('_', '/');
                //        }
                //        if (anotherFile != null)
                //        {
                //            if (anotherFile.file.Value.Contains(convertedId))
                //            {
                //                another = _fileManager.SaveFile(anotherFile.file.filename, attachment.Content);
                //            }
                //        }

                //    }
                //}
                //if (another != null)
                //{
                //    LicenseReissuanceProvDocDomain.Save(new LicenseReissuanceProvDoc
                //    {
                //        Date = DateTime.Now,
                //        LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Name == anotherFile.file.name),
                //        File = another,
                //        LicenseReissuance = licrequest,
                //        Number = "б/н"
                //    });
                //}


            }
            //отправляем ответ в РПГУ
            SendResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU);

            //создаем запрос на портале рпгу
            CreateOrdersRequest.CreateOrdersRequest create = new CreateOrdersRequest.CreateOrdersRequest();
            CreateOrdersRequestOrder createorder = new CreateOrdersRequestOrder();
            createorder.applicant = new CreateOrdersRequestOrderApplicant
            {
                agreement = true,
                email = orderItem.applicant.email,
                esiaId = orderItem.applicant.esiaId,
                firstName = orderItem.applicant.firstName,
                inn = orderItem.applicant.inn,
                lastName = orderItem.applicant.lastName,
                middleName = orderItem.applicant.middleName,
                phone = orderItem.applicant.phone,
                snils = orderItem.applicant.snils,
                type = GetApplicantType(orderItem.applicant.type),
            };
            createorder.date = DateTime.Now;
            createorder.number = orderItem.number;
            createorder.service = new CreateOrdersRequestOrderService
            {
                id = Convert.ToInt64(orderItem.service.id),
                okato = orderItem.service.okato,
                procedure = Convert.ToInt64(orderItem.service.procedure),
                procedureSpecified = true,
                target = Convert.ToInt64(orderItem.service.target),
                targetSpecified = true
            };
            List<CreateOrdersRequest.StatusHistoryListState> stateList = new List<CreateOrdersRequest.StatusHistoryListState>();
            stateList.Add(new CreateOrdersRequest.StatusHistoryListState
            {
                comment = "Принято в АИС ГЖИ",
                date = DateTime.Now,
                status = CreateOrdersRequest.StatusHistoryListStateStatus.Item2
            });

            createorder.states = stateList.ToArray();

            List<CreateOrdersRequestOrder> orders = new List<CreateOrdersRequestOrder>();
            orders.Add(createorder);
            create.orders = orders.ToArray();

            XElement requestElement = GetRequestElement(create);
            var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
            //LicenseReissuanceRPGU req = new LicenseReissuanceRPGU
            //{
            //    Date = DateTime.Now,
            //    LicRequest = new LicenseReissuance { Id = reqId },
            //    MessageId = requestResult.MessageId,
            //    RequestRPGUState = RequestRPGUState.Queued,
            //    RequestRPGUType = RequestRPGUType.NotSet,
            //    Text = "Отчет о приеме заявки в работу"

            //};
            //LicenseReissuanceRPGUService.Save(req);
        }
        private void CreateLicenseReissuanceRequest(OrderReissuanceRequest.orderRequest order, OrderReissuanceRequest.order orderItem, SMEV3Library.Entities.GetResponseResponse.GetRequestRequest requestData, XElement message, GetRequestResponse sgioRequest)
        {
            long reqId = 0;
            string regnumberRPGU = order.id.ToString();
            //if (orderItem.data.GJILicenseRequest.informationdeclarer.innul == null || orderItem.data.GJILicenseRequest.informationdeclarer.ogrnul == null)
            //{
            //    SendDeclineResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU, "В запросе отсутствуют необходимые сведения (ИНН, ОГРН)");
            //    //создаем запрос на портале рпгу
            //    CreateOrdersRequest.CreateOrdersRequest createDecline = new CreateOrdersRequest.CreateOrdersRequest();
            //    CreateOrdersRequestOrder createorderDecline = new CreateOrdersRequestOrder();
            //    createorderDecline.applicant = new CreateOrdersRequestOrderApplicant
            //    {
            //        agreement = true,
            //        email = orderItem.applicant.email,
            //        esiaId = orderItem.applicant.esiaId,
            //        firstName = orderItem.applicant.firstName,
            //        inn = orderItem.applicant.inn,
            //        lastName = orderItem.applicant.lastName,
            //        middleName = orderItem.applicant.middleName,
            //        phone = orderItem.applicant.phone,
            //        snils = orderItem.applicant.snils,
            //        type = GetApplicantType(orderItem.applicant.type),
            //    };
            //    createorderDecline.date = DateTime.Now;
            //    createorderDecline.number = orderItem.number;
            //    createorderDecline.service = new CreateOrdersRequestOrderService
            //    {
            //        id = Convert.ToInt64(orderItem.service.id),
            //        okato = orderItem.service.okato.ToString(),
            //        procedure = Convert.ToInt64(orderItem.service.procedure),
            //        procedureSpecified = true,
            //        target = Convert.ToInt64(orderItem.service.target),
            //        targetSpecified = true
            //    };
            //    List<CreateOrdersRequest.StatusHistoryListState> stateListDecline = new List<CreateOrdersRequest.StatusHistoryListState>();
            //    stateListDecline.Add(new CreateOrdersRequest.StatusHistoryListState
            //    {
            //        comment = "Отклонено АИС ГЖИ в связи с отсутствием необходимых данных",
            //        date = DateTime.Now,
            //        status = CreateOrdersRequest.StatusHistoryListStateStatus.Item4
            //    });

            //    createorderDecline.states = stateListDecline.ToArray();

            //    List<CreateOrdersRequestOrder> ordersDecline = new List<CreateOrdersRequestOrder>();
            //    ordersDecline.Add(createorderDecline);
            //    createDecline.orders = ordersDecline.ToArray();

            //    XElement requestElementDecline = GetRequestElement(createDecline);
            //    var requestResultDecline = _SMEV3Service.SendRequestAsyncSGIO(requestElementDecline, null, true).GetAwaiter().GetResult();
            //    return;
            //}
            var inn = orderItem.data.GJILicenseRequest.informationdeclarer.innul;
            var ogrn = orderItem.data.GJILicenseRequest.informationdeclarer.ogrnul;
            //if (string.IsNullOrEmpty(inn) || string.IsNullOrEmpty(ogrn))
            //{
            //    SendDeclineResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU, "В запросе отсутствуют необходимые сведения (ИНН, ОГРН)");
            //    //создаем запрос на портале рпгу
            //    CreateOrdersRequest.CreateOrdersRequest createDecline = new CreateOrdersRequest.CreateOrdersRequest();
            //    CreateOrdersRequestOrder createorderDecline = new CreateOrdersRequestOrder();
            //    createorderDecline.applicant = new CreateOrdersRequestOrderApplicant
            //    {
            //        agreement = true,
            //        email = orderItem.applicant.email,
            //        esiaId = orderItem.applicant.esiaId,
            //        firstName = orderItem.applicant.firstName,
            //        inn = orderItem.applicant.inn,
            //        lastName = orderItem.applicant.lastName,
            //        middleName = orderItem.applicant.middleName,
            //        phone = orderItem.applicant.phone,
            //        snils = orderItem.applicant.snils,
            //        type = GetApplicantType(orderItem.applicant.type),
            //    };
            //    createorderDecline.date = DateTime.Now;
            //    createorderDecline.number = orderItem.number;
            //    createorderDecline.service = new CreateOrdersRequestOrderService
            //    {
            //        id = Convert.ToInt64(orderItem.service.id),
            //        okato = orderItem.service.okato.ToString(),
            //        procedure = Convert.ToInt64(orderItem.service.procedure),
            //        procedureSpecified = true,
            //        target = Convert.ToInt64(orderItem.service.target),
            //        targetSpecified = true
            //    };
            //    List<CreateOrdersRequest.StatusHistoryListState> stateListDecline = new List<CreateOrdersRequest.StatusHistoryListState>();
            //    stateListDecline.Add(new CreateOrdersRequest.StatusHistoryListState
            //    {
            //        comment = "Отклонено АИС ГЖИ в связи с отсутствием необходимых данных",
            //        date = DateTime.Now,
            //        status = CreateOrdersRequest.StatusHistoryListStateStatus.Item4
            //    });

            //    createorderDecline.states = stateListDecline.ToArray();

            //    List<CreateOrdersRequestOrder> ordersDecline = new List<CreateOrdersRequestOrder>();
            //    ordersDecline.Add(createorderDecline);
            //    createDecline.orders = ordersDecline.ToArray();

            //    XElement requestElementDecline = GetRequestElement(createDecline);
            //    var requestResultDecline = _SMEV3Service.SendRequestAsyncSGIO(requestElementDecline, null, true).GetAwaiter().GetResult();
            //    return;
            //}
            Contragent contragent = ContragentRepository.GetAll()
                .FirstOrDefault(x => x.Inn == inn && x.Ogrn == ogrn);
            if (string.IsNullOrEmpty(inn) || string.IsNullOrEmpty(ogrn))
            {
                long cid = 19345;
                contragent = ContragentRepository.Get(cid);
            }
            if (contragent == null)
            {
                contragent = new Contragent
                {
                    Ogrn = ogrn,
                    Inn = inn,
                    Name = orderItem.data.GJILicenseRequest.informationdeclarer.fullnameul,
                    ShortName = orderItem.data.GJILicenseRequest.informationdeclarer.shortnameul,
                    Email = orderItem.data.GJILicenseRequest.informationdeclarer.emailul,
                    JuridicalAddress = orderItem.data.GJILicenseRequest.informationdeclarer.placecoordinat,
                    IsEDSE = true,
                    IsSOPR = true,
                    ContragentState = ContragentState.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    Description = "Создан из заявки на получение лицензии",
                    TaxRegistrationIssuedBy = orderItem.data.GJILicenseRequest.informationdeclarer.ogrnuldoc,
                    FactAddress = orderItem.data.GJILicenseRequest.informationdeclarer.placecoordinat2.ToString()
                };
                ContragentRepository.Save(contragent);
            }

            Person person = null;
            var contact = ContragentContactDomain.GetAll()
                .Where(x => x.Contragent == contragent)
                .FirstOrDefault(x => x.FullName.ToLower() == $"{orderItem.applicant.lastName.ToLower()} {orderItem.applicant.firstName.ToLower()} {orderItem.applicant.middleName.ToLower()}");
            if (contact == null)
            {
                Position position = PositionDomain.GetAll().FirstOrDefault(x => x.Name.ToLower() == orderItem.data.GJILicenseRequest.informationdeclarer.position);
                if (position == null)
                {
                    string positionName = !string.IsNullOrEmpty(orderItem.data.GJILicenseRequest.informationdeclarer.position) ? orderItem.data.GJILicenseRequest.informationdeclarer.position : "Не указана";
                    position = new Position
                    {
                        Name = positionName,
                        Code = "10"
                    };
                    PositionDomain.Save(position);
                }
                contact = new ContragentContact
                {
                    Position = position,
                    Contragent = contragent,
                    Name = orderItem.applicant.firstName,
                    Surname = orderItem.applicant.lastName,
                    Patronymic = orderItem.applicant.middleName,
                    Phone = orderItem.applicant.phone
                };
                ContragentContactDomain.Save(contact);
                var personPW = PersonPlaceWorkDomain.GetAll()
                .Where(x => x.Contragent == contragent)
                .Where(x => x.Person.Surname == contact.Surname && x.Person.Name == contact.Name && x.Person.Patronymic == contact.Patronymic)
                .FirstOrDefault();
                if (personPW == null)
                {
                    person = new Person
                    {
                        FullName = $"{contact.Surname} {contact.Name} {contact.Patronymic}",
                        Phone = orderItem.applicant.phone,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        Patronymic = contact.Patronymic,
                        AddressReg = "Не указан",
                        AddressBirth = "Не указано",
                        Birthdate = null

                    };
                    PersonDomain.Save(person);
                    PersonPlaceWorkDomain.Save(new PersonPlaceWork
                    {
                        Contragent = new Contragent { Id = contragent.Id },
                        Person = person,
                        Position = position
                    });
                }
                else
                {
                    person = personPW.Person;
                    person.Phone = orderItem.applicant.phone;
                    person.AddressReg = "Не указан";
                    person.AddressBirth = "Не указано";
                    person.Birthdate = null;
                    PersonDomain.Update(person);
                }
            }
            else
            {

                var personPW = PersonPlaceWorkDomain.GetAll()
                .Where(x => x.Contragent == contragent)
                .Where(x => x.Person.Surname == contact.Surname && x.Person.Name == contact.Name && x.Person.Patronymic == contact.Patronymic)
                .FirstOrDefault();
                if (personPW == null)
                {
                    person = new Person
                    {
                        FullName = $"{contact.Surname} {contact.Name} {contact.Patronymic}",
                        Phone = orderItem.applicant.phone,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        Patronymic = contact.Patronymic,
                        AddressReg = "Не указан",
                        AddressBirth = "Не указано",
                        Birthdate = null

                    };
                    PersonDomain.Save(person);
                    PersonPlaceWorkDomain.Save(new PersonPlaceWork
                    {
                        Contragent = new Contragent { Id = contragent.Id },
                        Person = person,
                        Position = contact.Position
                    });
                }
                else
                {
                    person = personPW.Person;
                    person.Phone = orderItem.applicant.phone;
                    person.AddressReg = "Не указан";
                    person.AddressBirth = "Не указано";
                    person.Birthdate = null;
                    PersonDomain.Update(person);
                }
            }
            var existlicense = ManOrgLicenseDomain.GetAll()
          .Where(x => x.State.Name == "Выдана")
          .Where(x => x.Contragent == contragent).FirstOrDefault();
            if (1 == 2)
            {
                //запрос на выдачу не создаем
            }
            else
            {
                LicenseReissuance licrequest = null;
                int registernum = GetNewNumber();
                var rpgustate = StateDomain.GetAll()
              .Where(x => x.TypeId == "gkh_manorg_license_reissuance" && x.Code == "РПГУ").FirstOrDefault();
                string pp = "Нет данных";
                licrequest = new LicenseReissuance
                {
                    Contragent = contragent,
                    ReissuanceDate = DateTime.Now,
                    RegisterNum = registernum,
                    RegisterNumber = registernum.ToString(),
                    ConfirmationOfDuty = pp,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    State = rpgustate,
                    MessageId = requestData.MessageId,
                    ReplyTo = requestData.ReplyTo,
                    RPGUNumber = regnumberRPGU
                };
                try
                {
                    LicenseReissuanceDomain.Save(licrequest);
                }
                catch (Exception e)
                {
                    
                }
                XDocument doc = new XDocument(message);
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.Indent = true;

                    using (XmlWriter xw = XmlWriter.Create(ms, xws))
                    {
                        doc.WriteTo(xw);
                    }

                    licrequest.File = _fileManager.SaveFile(ms, "order.xml");
                }
                LicenseReissuanceDomain.Update(licrequest);

                reqId = licrequest.Id;
                LicenseReissuancePersonDomain.Save(new LicenseReissuancePerson
                {
                    LicenseReissuance = licrequest,
                    Person = person,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                });
                var anotherFile = orderItem.data.GJILicenseRequest.files.anotherFiles;
                Bars.B4.Modules.FileStorage.FileInfo another = null;
                try
                {
                    foreach (var attachment in sgioRequest.RequestMessage.AttachmentContentList)
                    {
                        if (attachment != null)
                        {
                            string convertedId = attachment.Id;
                            if (!string.IsNullOrEmpty(convertedId))
                            {
                                convertedId = convertedId.Replace('_', '/');
                            }
                            if (anotherFile != null)
                            {
                                if (anotherFile.file.Value.Contains(convertedId))
                                {
                                    another = _fileManager.SaveFile(anotherFile.file.filename, attachment.Content);
                                }
                            }

                        }
                    }
                }
                catch
                { }
                if (another != null)
                {
                    try
                    {
                        LicenseReissuanceProvDocDomain.Save(new LicenseReissuanceProvDoc
                        {
                            Date = DateTime.Now,
                            LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Name == anotherFile.file.name),
                            File = another,
                            LicenseReissuance = licrequest,
                            Number = "б/н"
                        });
                    }
                    catch
                    {
                        try
                        {
                            var docLPD = new LicenseProvidedDoc
                            {
                                Code = licrequest.Id.ToString(),
                                Name = string.IsNullOrEmpty(anotherFile.file.name)? anotherFile.file.filename: anotherFile.file.name
                            };
                            LicenseProvidedDocDomain.Save(docLPD);
                            LicenseReissuanceProvDocDomain.Save(new LicenseReissuanceProvDoc
                            {
                                Date = DateTime.Now,
                                LicProvidedDoc = docLPD,
                                File = another,
                                LicenseReissuance = licrequest,
                                Number = "б/н"
                            });
                        }
                        catch
                        { }
                    }
                }
                if (requestData.Attachments != null && requestData.Attachments.Count > 0)
                {
                    foreach (var attmnt in requestData.Attachments)
                    {
                        if (attmnt.FileData != null)
                        {
                            LicenseReissuanceProvDocDomain.Save(new LicenseReissuanceProvDoc
                            {
                                Date = DateTime.Now,
                                File = _fileManager.SaveFile(attmnt.FileName, attmnt.FileData),
                                LicenseReissuance = licrequest,
                                LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Code == "PortalAttachment"),
                                Number = "б/н",
                                SignedInfo = attmnt.SignerInfo,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now
                            });
                        }
                    }
                }


            }
            //отправляем ответ в РПГУ
            SendResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU);

            //создаем запрос на портале рпгу
            CreateOrdersRequest.CreateOrdersRequest create = new CreateOrdersRequest.CreateOrdersRequest();
            CreateOrdersRequestOrder createorder = new CreateOrdersRequestOrder();
            createorder.applicant = new CreateOrdersRequestOrderApplicant
            {
                agreement = true,
                email = orderItem.applicant.email,
                esiaId = orderItem.applicant.esiaId.ToString(),
                firstName = orderItem.applicant.firstName,
                inn = orderItem.applicant.inn.ToString(),
                lastName = orderItem.applicant.lastName,
                middleName = orderItem.applicant.middleName,
                phone = orderItem.applicant.phone,
                snils = orderItem.applicant.snils,
                type = GetApplicantType(orderItem.applicant.type),
            };
            createorder.date = DateTime.Now;
            createorder.number = orderItem.number;
            createorder.service = new CreateOrdersRequestOrderService
            {
                id = Convert.ToInt64(orderItem.service.id),
                okato = orderItem.service.okato.ToString(),
                procedure = Convert.ToInt64(orderItem.service.procedure),
                procedureSpecified = true,
                target = Convert.ToInt64(orderItem.service.target),
                targetSpecified = true
            };
            List<CreateOrdersRequest.StatusHistoryListState> stateList = new List<CreateOrdersRequest.StatusHistoryListState>();
            stateList.Add(new CreateOrdersRequest.StatusHistoryListState
            {
                comment = "Принято в АИС ГЖИ",
                date = DateTime.Now,
                status = CreateOrdersRequest.StatusHistoryListStateStatus.Item2
            });

            createorder.states = stateList.ToArray();

            List<CreateOrdersRequestOrder> orders = new List<CreateOrdersRequestOrder>();
            orders.Add(createorder);
            create.orders = orders.ToArray();

            XElement requestElement = GetRequestElement(create);
            var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
            LicenseReissuanceRPGU req = new LicenseReissuanceRPGU
            {
                Date = DateTime.Now,
                LicRequest = new LicenseReissuance { Id = reqId },
                MessageId = requestResult.MessageId,
                File = _fileManager.SaveFile("Request", "xml", requestResult.SendedData),
                AnswerFile = _fileManager.SaveFile("Responce", "xml", requestResult.ReceivedData),
                RequestRPGUState = RequestRPGUState.Queued,
                RequestRPGUType = RequestRPGUType.NotSet,
                Text = "Отчет о приеме заявки в работу"

            };
            try
            {
                LicenseReissuanceRPGUService.Save(req);
            }
            catch
            { }
        }

        private void CreateLicenseRequest(OrderRequest.orderRequest order, OrderRequest.order orderItem, SMEV3Library.Entities.GetResponseResponse.GetRequestRequest requestData, XElement message, GetRequestResponse sgioRequest)
        {
            long reqId = 0;
            string regnumberRPGU = order.id.ToString();
            var inn = orderItem.data.GJILicenseRequest.informationdeclarerul.innul.ToString();
            var ogrn = orderItem.data.GJILicenseRequest.informationdeclarerul.ogrnul.ToString();
            Contragent contragent = ContragentRepository.GetAll()
                .FirstOrDefault(x => x.Inn == inn && x.Ogrn == ogrn);
            string nameip = "";
            bool ip = false;
            if (orderItem.data.GJILicenseRequest.information.declarer == "ip")
            {
                ip = true;
                nameip = $"ИП {orderItem.data.GJILicenseRequest.informationdeclarerfl.surnamefl} {orderItem.data.GJILicenseRequest.informationdeclarerfl.namefl} {orderItem.data.GJILicenseRequest.informationdeclarerfl.midlenamefl}";
            }
            if (contragent == null)
            {
                contragent = new Contragent
                {
                    Ogrn = ogrn,
                    Inn = inn,
                    Name = ip? nameip: orderItem.data.GJILicenseRequest.informationdeclarerul.fullnameul,
                    ShortName = ip ? nameip : orderItem.data.GJILicenseRequest.informationdeclarerul.shortnameul,
                    Email = orderItem.data.GJILicenseRequest.informationdeclarerul.emailul,
                    JuridicalAddress = ip ? "нет" : orderItem.data.GJILicenseRequest.informationdeclarerul.placecoordinat,
                    IsEDSE = true,
                    IsSOPR = true,
                    ContragentState = ContragentState.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    Description = "Создан из заявки на получение лицензии",
                    TaxRegistrationIssuedBy = orderItem.data.GJILicenseRequest.informationdeclarerul.ogrnuldoc,
                    FactAddress = orderItem.data.GJILicenseRequest.informationdeclarerul.placecoordinat2.ToString()
                };
                ContragentRepository.Save(contragent);
            }

            Person person = null;
            var contact = ContragentContactDomain.GetAll()
                .Where(x => x.Contragent == contragent)
                .FirstOrDefault(x => x.FullName.ToLower() == $"{orderItem.data.GJILicenseRequest.informationdeclarerfl.surnamefl.ToLower()} {orderItem.data.GJILicenseRequest.informationdeclarerfl.namefl.ToLower()} {orderItem.data.GJILicenseRequest.informationdeclarerfl.midlenamefl.ToLower()}");
            if (contact == null)
            {
                Position position = ip? PositionDomain.GetAll().FirstOrDefault(x => x.Name == "Индивидуальный предприниматель"): PositionDomain.GetAll().FirstOrDefault(x => x.Name.ToLower() == orderItem.data.GJILicenseRequest.informationdeclarerfl.position);
                if (position == null)
                {
                    if (ip)
                    {
                        string positionName = "Индивидуальный предприниматель";
                        position = new Position
                        {
                            Name = positionName,
                            Code = "45"
                        };
                        PositionDomain.Save(position);
                    }
                    else
                    {
                        string positionName = !string.IsNullOrEmpty(orderItem.data.GJILicenseRequest.informationdeclarerfl.position) ? orderItem.data.GJILicenseRequest.informationdeclarerfl.position : "Не указана";
                        if (positionName == "Не указана")
                        {
                            position = PositionDomain.GetAll().FirstOrDefault(x => x.Name == "Не указана");
                        }
                        if (position == null)
                        {
                            position = new Position
                            {
                                Name = positionName,
                                Code = "10"
                            };
                            PositionDomain.Save(position);
                        }
                    }
                }
                contact = new ContragentContact
                {
                    Position = position,
                    Contragent = contragent,
                    Name = orderItem.data.GJILicenseRequest.informationdeclarerfl.namefl,
                    Surname = orderItem.data.GJILicenseRequest.informationdeclarerfl.surnamefl,
                    Patronymic = orderItem.data.GJILicenseRequest.informationdeclarerfl.midlenamefl,
                    Phone = orderItem.data.GJILicenseRequest.informationdeclarerfl.phone
                };
                ContragentContactDomain.Save(contact);
                var personPW = PersonPlaceWorkDomain.GetAll()
                .Where(x => x.Contragent == contragent)
                .Where(x => x.Person.Surname == contact.Surname && x.Person.Name == contact.Name && x.Person.Patronymic == contact.Patronymic)
                .FirstOrDefault();
                if (personPW == null)
                {
                    person = new Person
                    {
                        FullName = $"{contact.Surname} {contact.Name} {contact.Patronymic}",
                        Phone = orderItem.data.GJILicenseRequest.informationdeclarerfl.phone,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        IdNumber = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentNumber,
                        IdSerial = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentSeries,
                        Email = orderItem.applicant.email,
                        Patronymic = contact.Patronymic,
                        AddressReg = orderItem.data.GJILicenseRequest.informationdeclarerfl.registrationAddress,
                        AddressBirth = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthPlace,
                        Birthdate = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthDate

                    };
                    PersonDomain.Save(person);
                    PersonPlaceWorkDomain.Save(new PersonPlaceWork
                    {
                        Contragent = new Contragent { Id = contragent.Id },
                        Person = person,
                        Position = position
                    });
                    try
                    {
                        if (orderItem.data.GJILicenseRequest.requirements.hasAttestationDoc)
                        {
                            var existsCa = PersonQualificationCertificateDomain.GetAll()
                                .Where(z => z.Person == person && z.Number == orderItem.data.GJILicenseRequest.requirements.attestationDocNumber)
                                .FirstOrDefault();
                            if (existsCa == null)
                                PersonQualificationCertificateDomain.Save(new PersonQualificationCertificate
                                {
                                    Person = person,
                                    Number = orderItem.data.GJILicenseRequest.requirements.attestationDocNumber,
                                    IssuedDate = orderItem.data.GJILicenseRequest.requirements.attestationDocDate,
                                    HasCancelled = false,
                                    IssuedBy = orderItem.data.GJILicenseRequest.requirements.attestationDocIssuer
                                });
                        }
                    }
                    catch
                    { }

                }
                else
                {
                    person = personPW.Person;
                    person.Phone = orderItem.data.GJILicenseRequest.informationdeclarerfl.phone;
                    person.AddressReg = orderItem.data.GJILicenseRequest.informationdeclarerfl.registrationAddress;
                    person.AddressBirth = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthPlace;
                    person.IdNumber = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentNumber;
                    person.IdSerial = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentSeries;
                    person.Email = orderItem.applicant.email;
                    person.Birthdate = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthDate;
                    person.AddressReg = orderItem.data.GJILicenseRequest.informationdeclarerfl.registrationAddress;
                    person.AddressBirth = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthPlace;
                    PersonDomain.Update(person);
                    try
                    {
                        if (orderItem.data.GJILicenseRequest.requirements.hasAttestationDoc)
                        {
                            var existsCa = PersonQualificationCertificateDomain.GetAll()
                                .Where(z => z.Person == person && z.Number == orderItem.data.GJILicenseRequest.requirements.attestationDocNumber)
                                .FirstOrDefault();
                            if (existsCa == null)
                                PersonQualificationCertificateDomain.Save(new PersonQualificationCertificate
                                {
                                    Person = person,
                                    Number = orderItem.data.GJILicenseRequest.requirements.attestationDocNumber,
                                    IssuedDate = orderItem.data.GJILicenseRequest.requirements.attestationDocDate,
                                    HasCancelled = false,
                                    IssuedBy = orderItem.data.GJILicenseRequest.requirements.attestationDocIssuer
                                });
                        }
                    }
                    catch
                    { }
                }
            }
            else
            {

                var personPW = PersonPlaceWorkDomain.GetAll()
                .Where(x => x.Contragent == contragent)
                .Where(x => x.Person.Surname == contact.Surname && x.Person.Name == contact.Name && x.Person.Patronymic == contact.Patronymic)
                .FirstOrDefault();
                if (personPW == null)
                {
                    person = new Person
                    {
                        FullName = $"{contact.Surname} {contact.Name} {contact.Patronymic}",
                        Phone = orderItem.data.GJILicenseRequest.informationdeclarerfl.phone,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        Patronymic = contact.Patronymic,
                        IdNumber = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentNumber,
                        IdSerial = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentSeries,
                        Email = orderItem.applicant.email,
                        AddressReg = orderItem.data.GJILicenseRequest.informationdeclarerfl.registrationAddress,
                        AddressBirth = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthPlace,
                        Birthdate = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthDate

                    };
                    PersonDomain.Save(person);
                    PersonPlaceWorkDomain.Save(new PersonPlaceWork
                    {
                        Contragent = new Contragent { Id = contragent.Id },
                        Person = person,
                        Position = contact.Position
                    });
                    try
                    {
                        if (orderItem.data.GJILicenseRequest.requirements.hasAttestationDoc)
                        {
                            var existsCa = PersonQualificationCertificateDomain.GetAll()
                                .Where(z => z.Person == person && z.Number == orderItem.data.GJILicenseRequest.requirements.attestationDocNumber)
                                .FirstOrDefault();
                            if (existsCa == null)
                                PersonQualificationCertificateDomain.Save(new PersonQualificationCertificate
                                {
                                    Person = person,
                                    Number = orderItem.data.GJILicenseRequest.requirements.attestationDocNumber,
                                    IssuedDate = orderItem.data.GJILicenseRequest.requirements.attestationDocDate,
                                    HasCancelled = false,
                                    IssuedBy = orderItem.data.GJILicenseRequest.requirements.attestationDocIssuer
                                });
                        }
                    }
                    catch
                    { }
                }
                else
                {
                    person = personPW.Person;
                    person.Phone = orderItem.data.GJILicenseRequest.informationdeclarerfl.phone;
                    person.AddressReg = orderItem.data.GJILicenseRequest.informationdeclarerfl.registrationAddress;
                    person.AddressBirth = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthPlace;
                    person.IdNumber = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentNumber;
                    person.IdSerial = orderItem.data.GJILicenseRequest.informationdeclarerfl.identityDocumentSeries;
                    person.Email = orderItem.applicant.email;
                    person.Birthdate = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthDate;
                    person.AddressReg = orderItem.data.GJILicenseRequest.informationdeclarerfl.registrationAddress;
                    person.AddressBirth = orderItem.data.GJILicenseRequest.informationdeclarerfl.birthPlace;
                    PersonDomain.Update(person);
                    try
                    {
                        if (orderItem.data.GJILicenseRequest.requirements.hasAttestationDoc)
                        {
                            var existsCa = PersonQualificationCertificateDomain.GetAll()
                                .Where(z => z.Person == person && z.Number == orderItem.data.GJILicenseRequest.requirements.attestationDocNumber)
                                .FirstOrDefault();
                            if(existsCa == null)
                            PersonQualificationCertificateDomain.Save(new PersonQualificationCertificate
                            {
                                Person = person,
                                Number = orderItem.data.GJILicenseRequest.requirements.attestationDocNumber,
                                IssuedDate = orderItem.data.GJILicenseRequest.requirements.attestationDocDate,
                                HasCancelled = false,
                                IssuedBy = orderItem.data.GJILicenseRequest.requirements.attestationDocIssuer
                            });
                        }
                    }
                    catch
                    { }
                }
            }
            var existlicense = ManOrgLicenseDomain.GetAll()
          .Where(x => x.State.Name == "Выдана")
          .Where(x => x.Contragent == contragent).FirstOrDefault();
            if (1==2)
            {
                //запрос на выдачу не создаем
            }
            else
            {
                ManOrgLicenseRequest licrequest = null;
                int registernum = GetNewNumber();
                var rpgustate = StateDomain.GetAll()
              .Where(x => x.TypeId == "gkh_manorg_license_request" && x.Code == "РПГУ").FirstOrDefault();
                string pp = orderItem.data.GJILicenseRequest.dutypay.dutypayNumber;
                licrequest = new ManOrgLicenseRequest
                {
                    Contragent = contragent,
                    DateRequest = DateTime.Now,
                    RegisterNum = registernum,
                    RegisterNumber = registernum.ToString(),
                    ConfirmationOfDuty = pp,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    State = rpgustate,
                    MessageId = requestData.MessageId,
                    ReplyTo = requestData.ReplyTo,
                    RPGUNumber = regnumberRPGU
                };
                ManOrgLicenseRequestDomain.Save(licrequest);
                XDocument doc = new XDocument(message);
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.Indent = true;

                    using (XmlWriter xw = XmlWriter.Create(ms, xws))
                    {
                        doc.WriteTo(xw);
                    }

                    licrequest.File = _fileManager.SaveFile(ms, "order.xml");
                }
                ManOrgLicenseRequestDomain.Update(licrequest);

                reqId = licrequest.Id;
                ManOrgRequestPersonDomain.Save(new ManOrgRequestPerson
                {
                    LicRequest = licrequest,
                    Person = person,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                });
                var file1 = orderItem.data.GJILicenseRequest.files.document1;
                var file2 = orderItem.data.GJILicenseRequest.files.document2;
                var anotherFile = orderItem.data.GJILicenseRequest.files.anotherFiles;
                Bars.B4.Modules.FileStorage.FileInfo fi1 = null;
                Bars.B4.Modules.FileStorage.FileInfo fi2 = null;
                Bars.B4.Modules.FileStorage.FileInfo another = null;
                foreach (var attachment in sgioRequest.RequestMessage.AttachmentContentList)
                {
                    if (attachment != null)
                    {
                        string convertedId = attachment.Id;
                        if (!string.IsNullOrEmpty(convertedId))
                        {
                            convertedId = convertedId.Replace('_', '/');
                        }
                        if (file1 != null)
                        {

                            if (file1.Value.Contains(convertedId))
                            {
                                fi1 = _fileManager.SaveFile(file1.filename, attachment.Content);
                            }
                        }
                        if (file2 != null)
                        {
                            if (file2.Value.Contains(convertedId))
                            {
                                fi2 = _fileManager.SaveFile(file2.filename, attachment.Content);
                            }
                        }
                        if (anotherFile != null)
                        {
                            if (anotherFile.file.Value.Contains(convertedId))
                            {
                                another = _fileManager.SaveFile(anotherFile.file.filename, attachment.Content);
                            }
                        }

                    }
                }
                if (fi1 != null)
                {
                    ManOrgRequestProvDocDomain.Save(new ManOrgRequestProvDoc
                    {
                        Date = DateTime.Now,
                        LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Name == file1.name),
                        File = fi1,
                        LicRequest = licrequest,
                        Number = "б/н"
                    });
                }

                if (fi2 != null)
                {
                    ManOrgRequestProvDocDomain.Save(new ManOrgRequestProvDoc
                    {
                        Date = DateTime.Now,
                        LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Name == file2.name),
                        File = fi2,
                        LicRequest = licrequest,
                        Number = "б/н"
                    });
                }
                if (another != null)
                {
                    ManOrgRequestProvDocDomain.Save(new ManOrgRequestProvDoc
                    {
                        Date = DateTime.Now,
                        LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Name == anotherFile.file.name),
                        File = another,
                        LicRequest = licrequest,
                        Number = "б/н"
                    });
                }
                if (requestData.Attachments != null && requestData.Attachments.Count > 0)
                {
                    foreach (var attmnt in requestData.Attachments)
                    {
                        if (attmnt.FileData != null)
                        {
                            ManOrgRequestProvDocDomain.Save(new ManOrgRequestProvDoc
                            {
                                Date = DateTime.Now,
                                File = _fileManager.SaveFile(attmnt.FileName, attmnt.FileData),
                                LicRequest = licrequest,
                                SignedInfo = attmnt.SignerInfo,
                                LicProvidedDoc = LicenseProvidedDocDomain.GetAll().FirstOrDefault(x => x.Code == "PortalAttachment"),
                                Number = "б/н",
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now
                            });
                        }
                    }
                }
                //Создаем начисление в ГИС ГМП

            }
            //отправляем ответ в РПГУ
            SendResponceSGIO(requestData.ReplyTo, requestData.MessageId, true, regnumberRPGU);

            //создаем запрос на портале рпгу
            CreateOrdersRequest.CreateOrdersRequest create = new CreateOrdersRequest.CreateOrdersRequest();
            CreateOrdersRequestOrder createorder = new CreateOrdersRequestOrder();
            createorder.applicant = new CreateOrdersRequestOrderApplicant
            {
                agreement = true,
                email = orderItem.applicant.email,
                esiaId = orderItem.applicant.esiaId,
                firstName = orderItem.applicant.firstName,
                inn = orderItem.applicant.inn,
                lastName = orderItem.applicant.lastName,
                middleName = orderItem.applicant.middleName,
                phone = orderItem.applicant.phone,
                snils = orderItem.applicant.snils,
                type = GetApplicantType(orderItem.applicant.type),
            };
            createorder.date = DateTime.Now;
            createorder.number = orderItem.number;
            createorder.service = new CreateOrdersRequestOrderService
            {
                id = Convert.ToInt64(orderItem.service.id),
                okato = orderItem.service.okato.ToString(),
                procedure = Convert.ToInt64(orderItem.service.procedure),
                procedureSpecified = true,
                target = Convert.ToInt64(orderItem.service.target),
                targetSpecified = true
            };
            List<CreateOrdersRequest.StatusHistoryListState> stateList = new List<CreateOrdersRequest.StatusHistoryListState>();
            stateList.Add(new CreateOrdersRequest.StatusHistoryListState
            {
                comment = "Принято в АИС ГЖИ",
                date = DateTime.Now,
                status = CreateOrdersRequest.StatusHistoryListStateStatus.Item2
            });

            createorder.states = stateList.ToArray();

            List<CreateOrdersRequestOrder> orders = new List<CreateOrdersRequestOrder>();
            orders.Add(createorder);
            create.orders = orders.ToArray();

            XElement requestElement = GetRequestElement(create);
            var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
            ManOrgRequestRPGU req = new ManOrgRequestRPGU
            {
                Date = DateTime.Now,
                LicRequest = new ManOrgLicenseRequest { Id = reqId },
                File = _fileManager.SaveFile("Request", "xml", requestResult.SendedData),
                AnswerFile = _fileManager.SaveFile("Responce", "xml", requestResult.ReceivedData),
                MessageId = requestResult.MessageId,
                RequestRPGUState = RequestRPGUState.Queued,
                RequestRPGUType = RequestRPGUType.NotSet,
                Text = "Отчет о приеме заявки в работу"

            };
            ManOrgRequestRPGUService.Save(req);

            var licrequestProxy = ManOrgLicenseRequestDomain.Load(reqId);

            //Создаем начисление в ГИС ГМП
            GisGmp newDuty = new GisGmp
            {
                CalcDate = DateTime.Now,
                TypeLicenseRequest = TypeLicenseRequest.First,
                BillFor = "Пошлина за выдачу лицензии",
                PayerType = ip ? PayerType.IP : PayerType.Juridical,
                BillDate = DateTime.Now,
                GisGmpChargeType = GisGmpChargeType.First,
                INN = contragent.Inn,
                KBK = "80110807400010000110",
                OKTMO = "20701000",
                ManOrgLicenseRequest = licrequestProxy,
                KPP = contragent.Kpp,
                IsRF = true,
                TotalAmount = 30000,
                PayerName = contragent.Name,
                PayerNameSent = contact.Name,
                LegalAct = "Заявка на лицензию " + licrequestProxy.RegisterNumber + " от " + licrequestProxy.DateRequest.Value.ToShortDateString(),
                GisGmpPaymentsKind = GisGmpPaymentsKind.PAYMENT,
                Inspector = InspectorDomain.GetAll().FirstOrDefault(x=>x.Fio == "Гончарова Диана Ивановна"),
                IdentifierType = IdentifierType.INN
            };
            GISGMPDomain.Save(newDuty);
            BaseParams bParams = new BaseParams();
            bParams.Params.Add("taskId", newDuty.Id);
            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendCalcRequestTaskProvider(_container), bParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo != null)
                {
                    //создаем запрос на портале рпгу
                    var sentOD = SendOrderDuty(orderItem, reqId, newDuty);

                }
            }
            catch (Exception e)
            {

            }


        }

        private bool SendOrderDuty(OrderRequest.order orderItem, long reqId, GisGmp newDuty)
        {
            CreateOrdersRequest.CreateOrdersRequest create = new CreateOrdersRequest.CreateOrdersRequest();
            CreateOrdersRequestOrder createorder = new CreateOrdersRequestOrder();
            createorder.applicant = new CreateOrdersRequestOrderApplicant
            {
                agreement = true,
                email = orderItem.applicant.email,
                esiaId = orderItem.applicant.esiaId,
                firstName = orderItem.applicant.firstName,
                inn = orderItem.applicant.inn,
                lastName = orderItem.applicant.lastName,
                middleName = orderItem.applicant.middleName,
                phone = orderItem.applicant.phone,
                snils = orderItem.applicant.snils,
                type = GetApplicantType(orderItem.applicant.type),
            };
            createorder.date = DateTime.Now;
            createorder.number = orderItem.number;
            createorder.service = new CreateOrdersRequestOrderService
            {
                id = Convert.ToInt64(orderItem.service.id),
                okato = orderItem.service.okato.ToString(),
                procedure = Convert.ToInt64(orderItem.service.procedure),
                procedureSpecified = true,
                target = Convert.ToInt64(orderItem.service.target),
                targetSpecified = true
            };
            List<CreateOrdersRequest.StatusHistoryListState> stateList = new List<CreateOrdersRequest.StatusHistoryListState>();
            stateList.Add(new CreateOrdersRequest.StatusHistoryListState
            {
                comment = $"Оплата государственной пошлины за выдачу лицензии по заявлению №{orderItem.number} от {DateTime.Now.ToShortDateString()} УИН начисления {newDuty.UIN}",
                date = DateTime.Now,
                status = CreateOrdersRequest.StatusHistoryListStateStatus.Item7
            });

            createorder.states = stateList.ToArray();

            List<CreateOrdersRequestOrder> orders = new List<CreateOrdersRequestOrder>();
            orders.Add(createorder);
            create.orders = orders.ToArray();

            XElement requestElement = GetRequestElement(create);
            var requestResult = _SMEV3Service.SendRequestAsyncSGIO(requestElement, null, true).GetAwaiter().GetResult();
            ManOrgRequestRPGU req = new ManOrgRequestRPGU
            {
                Date = DateTime.Now,
                LicRequest = new ManOrgLicenseRequest { Id = reqId },
                File = _fileManager.SaveFile("Request", "xml", requestResult.SendedData),
                AnswerFile = _fileManager.SaveFile("Responce", "xml", requestResult.ReceivedData),
                MessageId = requestResult.MessageId,
                RequestRPGUState = RequestRPGUState.Queued,
                RequestRPGUType = RequestRPGUType.NotSet,
                Text = "Уведомление об оплате пошлины"

            };
            ManOrgRequestRPGUService.Save(req);
            return true;
        }

        private XElement GetRequestElement(CreateOrdersRequest.CreateOrdersRequest order)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(CreateOrdersRequest.CreateOrdersRequest));
                    xmlSerializer.Serialize(streamWriter, order);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private XElement GetUpdateRequestElement(UpdateOrdersRequest.UpdateOrdersRequest order)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(UpdateOrdersRequest.UpdateOrdersRequest));
                    xmlSerializer.Serialize(streamWriter, order);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private XElement GetResponceElement(OrderRequest.orderResponse order)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(OrderRequest.orderResponse));
                    xmlSerializer.Serialize(streamWriter, order);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }
        private CreateOrdersRequestOrderApplicantType GetApplicantType(LicenseUndoRequest.type type)
        {
            switch (type)
            {
                case LicenseUndoRequest.type.UL:
                    return CreateOrdersRequestOrderApplicantType.UL;
                case LicenseUndoRequest.type.FL:
                    return CreateOrdersRequestOrderApplicantType.FL;
                case LicenseUndoRequest.type.IP:
                    return CreateOrdersRequestOrderApplicantType.IP;
                default: return CreateOrdersRequestOrderApplicantType.UL;
            }
        }
        private CreateOrdersRequestOrderApplicantType GetApplicantType(LicenseInfoRequest.type type)
        {
            switch (type)
            {
                case LicenseInfoRequest.type.UL:
                    return CreateOrdersRequestOrderApplicantType.UL;
                case LicenseInfoRequest.type.FL:
                    return CreateOrdersRequestOrderApplicantType.FL;
                case LicenseInfoRequest.type.IP:
                    return CreateOrdersRequestOrderApplicantType.IP;
                default: return CreateOrdersRequestOrderApplicantType.UL;
            }
        }
        private CreateOrdersRequestOrderApplicantType GetApplicantType(OrderRequest.type type)
        {
            switch (type)
            {
                case OrderRequest.type.UL:
                    return CreateOrdersRequestOrderApplicantType.UL;
                case OrderRequest.type.FL:
                    return CreateOrdersRequestOrderApplicantType.FL;
                case OrderRequest.type.IP:
                    return CreateOrdersRequestOrderApplicantType.IP;
                default: return CreateOrdersRequestOrderApplicantType.UL;
            }
        }
        private CreateOrdersRequestOrderApplicantType GetApplicantType(OrderReissuanceRequest.type type)
        {
            switch (type)
            {
                case OrderReissuanceRequest.type.UL:
                    return CreateOrdersRequestOrderApplicantType.UL;
                case OrderReissuanceRequest.type.FL:
                    return CreateOrdersRequestOrderApplicantType.FL;
                case OrderReissuanceRequest.type.IP:
                    return CreateOrdersRequestOrderApplicantType.IP;
                default: return CreateOrdersRequestOrderApplicantType.UL;
            }
        }

        private static XDocument LoadFromStream(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return XDocument.Load(reader);
            }
        }

        private void SendResponceSGIO(string to, string messageId, bool isSuccess, string number)
        {
            OrderRequest.orderResponse responce = new OrderRequest.orderResponse();
            responce.id = "ID_"+messageId;
            responce.result = new OrderRequest.result
            {
                message = "Успех",
                number = number,
                status = 0
            };
            if (isSuccess)
            {
                var result = GetResponceElement(responce);
                _SMEV3Service.SendResponceAsync(result, messageId, to, false).GetAwaiter().GetResult();
            }
        }

        private void SendDeclineResponceSGIO(string to, string messageId, bool isSuccess, string number, string message)
        {
            OrderRequest.orderResponse responce = new OrderRequest.orderResponse();
            responce.id = "ID_" + messageId;
            responce.result = new OrderRequest.result
            {
                message = message,
                number = number,
                status = 0
            };
            if (isSuccess)
            {
                var result = GetResponceElement(responce);
                _SMEV3Service.SendResponceAsync(result, messageId, to, false).GetAwaiter().GetResult();
            }
        }


        #endregion

        private int GetNewNumber()
        {
            var maxRequest = ManOrgLicenseRequestDomain.GetAll()
                  .OrderByDescending(x => x.RegisterNum).FirstOrDefault();
            int newRegisterNum = 0;
            if (maxRequest.RegisterNum.HasValue)
            {
                newRegisterNum = maxRequest.RegisterNum.Value + 1;
                return newRegisterNum;
            }
            else
            {
                var maxRequestById = ManOrgLicenseRequestDomain.GetAll()
                 .OrderByDescending(x => x.Id).FirstOrDefault();

                string s3 = maxRequestById.RegisterNumber;
                string result = "";
                char[] charsS3 = s3.ToCharArray();
                for (int i = 0; i < s3.Length; i++)
                {
                    if (char.IsDigit(charsS3[i]))
                    {
                        result = result + charsS3[i];

                    }
                }
                if (!string.IsNullOrEmpty(result))
                {
                    return Convert.ToInt32(result);
                }
                else
                    return 1;

            }
        }

        private GetRequestResponse DeSerializer(XElement element)
        {
            StringReader reader = new StringReader(element.ToString(SaveOptions.DisableFormatting));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GetRequestResponse));
            return((GetRequestResponse)xmlSerializer.Deserialize(reader));
        }
        private OrderRequest.orderRequest DeSerializerOrder(XElement element)
        {
            StringReader reader = new StringReader(element.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrderRequest.orderRequest));
            return ((OrderRequest.orderRequest)xmlSerializer.Deserialize(reader));
        }
        private OrderReissuanceRequest.orderRequest DeSerializerOrderReissuance(XElement element)
        {
            StringReader reader = new StringReader(element.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrderReissuanceRequest.orderRequest));
            return ((OrderReissuanceRequest.orderRequest)xmlSerializer.Deserialize(reader));
        }
        private LicenseInfoRequest.orderRequest DeSerializerLicenseInfoRequest(XElement element)
        {
            StringReader reader = new StringReader(element.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LicenseInfoRequest.orderRequest));
            return ((LicenseInfoRequest.orderRequest)xmlSerializer.Deserialize(reader));
        }
        private LicenseUndoRequest.orderRequest DeSerializerLicenseUndoRequest(XElement element)
        {
            StringReader reader = new StringReader(element.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LicenseUndoRequest.orderRequest));
            return ((LicenseUndoRequest.orderRequest)xmlSerializer.Deserialize(reader));
        }

    }
}
