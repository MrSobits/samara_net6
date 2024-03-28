using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.DataProviders.Meta;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
using Bars.Gkh.RegOperator.Enums;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.BillsServiceAsync;
using GisGkhLibrary.Services;
//using GisGkhLibrary.Utils;
using Newtonsoft.Json;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using Sobits.GisGkh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    using Castle.Core;

    public class ImportPaymentDocumentDataService : IImportPaymentDocumentDataService
    {
        #region Constants

        const short numDocs = 200;

        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        public IDomainService<PaymentDocumentSnapshot> PaymentDocumentSnapshotDomain { get; set; }

        public IDomainService<AccountPaymentInfoSnapshot> AccountPaymentInfoSnapshotDomain { get; set; }

        public IDomainService<GisGkhPayDoc> GisGkhPayDocDomain { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IRepository<RealityObject> RealityObjectRepo { get; set; }

        public IRepository<PersAccGroupRelation> PersAccGroupRelationRepo { get; set; }

        public IRepository<BasePersonalAccount> BasePersonalAccountRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }
        public class PayInfo
        {
            /// <summary>
            /// Информация из снапшота
            /// </summary>
            public string Data;

            /// <summary>
            /// ИД ЛС
            /// </summary>
            public long AccountId;
        }

        #endregion

        #region Constructors

        public ImportPaymentDocumentDataService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string chargePeriodId, string RoId, string rewrite)
        {
            try
            {
              //  List<long> notNeedGis = PersAccGroupRelationRepo.GetAll()
              //.Where(x => x.Group.Name == "Выгружено в ГИС ЖКХ")
              //.Select(x => x.PersonalAccount.Room.RealityObject.Id).Distinct()
              //.ToList();

              //  var group = PersAccGroupRelationRepo.GetAll()
              //      .Where(x => x.Group.Name == "Выгружено в ГИС ЖКХ");

                var chargePeriod = ChargePeriodDomain.Get(long.Parse(chargePeriodId));
                if (chargePeriod.IsClosing)
                {
                    throw new Exception("Ошибка: Выбранный период в стадии закрытия");
                }
                if (!chargePeriod.IsClosed)
                {
                    throw new Exception("Ошибка: Выбранный период не закрыт");
                }
                var taskManager = Container.Resolve<ITaskManager>();

                long? houseId = null;
                if (!string.IsNullOrEmpty(RoId))
                {
                    houseId = long.Parse(RoId);
                }
                var rewriteFlag = bool.Parse(rewrite);
                try
                {
                    var baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id);
                    baseParams.Params.Add("chargePeriodId", chargePeriodId);
                    baseParams.Params.Add("roId", houseId);
                    baseParams.Params.Add("rewrite", rewriteFlag);
                    taskManager.CreateTasks(new ImportPaymentDocumentsTaskProvider(Container), baseParams);
                    req.Answer = "Задача формирования запросов выгрузки начислений в ГИС ЖКХ успешно поставлена";
                    GisGkhRequestsDomain.Update(req);
                    //return new BaseDataResult(true, "Задача успешно поставлена");
                }
                catch (Exception e)
                {
                    req.Answer = $"Ошибка при постановке задачи формирования запросов выгрузки начислений в ГИС ЖКХ: {e.Message}";
                    req.RequestState = GisGkhRequestState.Error;
                    GisGkhRequestsDomain.Update(req);
                    //return new BaseDataResult(false, e.Message);
                }
                finally
                {
                    Container.Release(taskManager);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        /*public void SaveRequestTaskExecutor(GisGkhRequests req, string chargePeriodId, long? roId, bool rewrite)
        {
            try
            {
                var chargePeriod = ChargePeriodDomain.Get(long.Parse(chargePeriodId));
                if (chargePeriod.IsClosing)
                {
                    throw new Exception("Ошибка: Выбранный период в стадии закрытия");
                }
                if (!chargePeriod.IsClosed)
                {
                    throw new Exception("Ошибка: Выбранный период не закрыт");
                }
                //var paymentDocumentSnapshots = PaymentDocumentSnapshotDomain.GetAll()
                //    .Where(x => x.Period == chargePeriod)
                //    .Where(x => x.OwnerType == PersonalAccountOwnerType.Individual) //пока только физики
                //    //.ToList()
                //    //.Select(x => x.Id)
                //    ;

                // список сопоставленных ЛС без выгруженных начислений
                List<long> gisAccs = BasePersonalAccountRepo.GetAll().Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                    .WhereIf(!rewrite, x =>
                        !GisGkhPayDocDomain.GetAll()
                        .Where(y => y.Period == chargePeriod)
                        .Where(y => y.Account == x).Any())
                    .WhereIf(roId != null, x => x.Room.RealityObject.Id == roId)
                    .Select(x => x.Id).ToList();
                //Dictionary<long, string> gisAccs = BasePersonalAccountRepo.GetAll().Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "").Select(x => new
                //{
                //    x.Id,
                //    x.GisGkhGuid
                //}).ToDictionary(x => x.Id, x => x.GisGkhGuid);

                List<PayInfo> accountPaymentInfoSnapshots = AccountPaymentInfoSnapshotDomain.GetAll()
                    .Where(x => x.Snapshot.Period == chargePeriod)
                    .Where(x => gisAccs.Contains(x.AccountId))
                    .Select(x => new PayInfo
                    {
                        Data = x.Snapshot.Data,
                        AccountId = x.AccountId
                    })
                    .ToList();


                //var paymentDocumentSnapshotsCount = paymentDocumentSnapshots.Count();

                var accountPaymentInfoSnapshotsCount = accountPaymentInfoSnapshots.Count();

                //var reqNum = paymentDocumentSnapshotsCount / numDocs;
                var reqNum = accountPaymentInfoSnapshotsCount / numDocs;
                //if (paymentDocumentSnapshotsCount % numDocs > 0)
                if (accountPaymentInfoSnapshotsCount % numDocs > 0)
                {
                    reqNum++;
                }
                for (int i = 0; i < reqNum; i++)
                {
                    //var paymentDocumentSnapshotsPart = paymentDocumentSnapshots.Skip(i * numDocs).Take(numDocs).ToList();
                    var accountPaymentInfoSnapshotsPart = accountPaymentInfoSnapshots.Skip(i * numDocs).Take(numDocs);

                    //SaveSingleRequest()

                    if (i > 0)
                    {
                        var newReq = new GisGkhRequests();
                        newReq.TypeRequest = req.TypeRequest;
                        newReq.Operator = req.Operator;
                        newReq.RequestState = GisGkhRequestState.NotFormed;
                        req = newReq;
                        GisGkhRequestsDomain.Save(req);
                    }
                    //List<Tuple<string, string, string>> paymentInfo = new List<Tuple<string, string, string>>();
                    List<importPaymentDocumentRequestPaymentInformation> paymentInfos = new List<importPaymentDocumentRequestPaymentInformation>();
                    List<importPaymentDocumentRequestPaymentDocument> paymentDocs = new List<importPaymentDocumentRequestPaymentDocument>();
                    //foreach (var payDoc in paymentDocumentSnapshotsPart)
                    foreach (var payDoc in accountPaymentInfoSnapshotsPart)
                    {
                        // PaymentInformation
                        var snap = JsonConvert.DeserializeObject<InvoiceInfo>(payDoc.Data);
                        //var snap = payDoc.Snapshot.ConvertTo<InvoiceInfo>();
                        //var persAcc = BasePersonalAccountRepo.Get(snap.AccountId);
                        var persAcc = BasePersonalAccountRepo.Get(payDoc.AccountId);
                        string InfoTransportGuid = null;
                        if (!paymentInfos.Any(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя))
                        {
                            InfoTransportGuid = Guid.NewGuid().ToString();
                            paymentInfos.Add(new importPaymentDocumentRequestPaymentInformation
                            {
                                BankBIK = snap.БикБанка,
                                operatingAccountNumber = snap.РсчетПолучателя,
                                TransportGUID = InfoTransportGuid
                            });
                        }
                        else
                        {
                            InfoTransportGuid = paymentInfos.Where(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя).First().TransportGUID;
                        }
                        string DocTransportGuid = Guid.NewGuid().ToString();
                        GisGkhPayDoc gisPayDoc = null;
                        if (rewrite)
                        {
                            gisPayDoc = GisGkhPayDocDomain.FirstOrDefault(x => x.Account == persAcc && x.Period == chargePeriod);
                        }
                        if (gisPayDoc == null)
                        {
                            gisPayDoc = new GisGkhPayDoc
                            {
                                Account = persAcc,
                                Period = chargePeriod,
                                PaymentDocumentTransportGUID = DocTransportGuid
                            };
                            GisGkhPayDocDomain.Save(gisPayDoc);
                        }
                        else
                        {
                            gisPayDoc.PaymentDocumentTransportGUID = DocTransportGuid;
                            GisGkhPayDocDomain.Update(gisPayDoc);
                        }
                        var Contribution = (snap.Тариф != null ? snap.Тариф.Value : 0).ToMagic(2);
                        var AccountingPeriodTotal = ((snap.НачисленоБазовый != null ? snap.НачисленоБазовый.Value : 0) +
                                                                (snap.НачисленоТарифРешения != null ? snap.НачисленоТарифРешения.Value : 0)).ToMagic(2);
                        var MoneyRecalculation = ((snap.ПерерасчетБазовый != null ? snap.ПерерасчетБазовый.Value : 0) -
                                                             (snap.ОтменыБазовый != null ? snap.ОтменыБазовый.Value : 0) -
                                                             (snap.ОтменыКорректировкаБазовый != null ? snap.ОтменыКорректировкаБазовый.Value : 0) +
                                                             (snap.КорректировкаБазовый != null ? snap.КорректировкаБазовый.Value : 0) +
                                                             (snap.СлияниеБазовый != null ? snap.СлияниеБазовый.Value : 0) -
                                                             (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                             (snap.ПерерасчетТарифРешения != null ? snap.ПерерасчетТарифРешения.Value : 0) -
                                                             (snap.ОтменыТарифРешения != null ? snap.ОтменыТарифРешения.Value : 0) -
                                                             (snap.ОтменыКорректировкаТарифРешения != null ? snap.ОтменыКорректировкаТарифРешения.Value : 0) +
                                                             (snap.КорректировкаТарифРешения != null ? snap.КорректировкаТарифРешения.Value : 0) +
                                                             (snap.СлияниеТарифРешения != null ? snap.СлияниеТарифРешения.Value : 0) -
                                                             (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0)).ToMagic(2);
                        var TotalPayable = AccountingPeriodTotal + MoneyRecalculation;
                        var TotalPayableOverall = ((snap.ДолгБазовыйНаКонец != null ? snap.ДолгБазовыйНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                       (snap.ДолгТарифРешенияНаКонец != null ? snap.ДолгТарифРешенияНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0) > 0 ?
                                                       (snap.ДолгБазовыйНаКонец != null ? snap.ДолгБазовыйНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                       (snap.ДолгТарифРешенияНаКонец != null ? snap.ДолгТарифРешенияНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0) : 0).ToMagic(2);
                        var DebtPreviousPeriodsOrAdvanceBillingPeriod = ((snap.ДолгБазовыйНаНачало != null ? snap.ДолгБазовыйНаНачало.Value : 0) +
                                                                                    (snap.ДолгТарифРешенияНаНачало != null ? snap.ДолгТарифРешенияНаНачало.Value : 0) -
                                                                                    (snap.ОплаченоБазовый != null ? snap.ОплаченоБазовый.Value : 0) -
                                                                                    (snap.ОплаченоТарифРешения != null ? snap.ОплаченоТарифРешения.Value : 0)).ToMagic(2);
                        var Penalties = (snap.ДолгПениНаКонец != null ? snap.ДолгПениНаКонец.Value : 0).ToMagic(2);
                        var PaidCash = ((snap.ОплаченоБазовый != null ? snap.ОплаченоБазовый.Value : 0) +
                                        (snap.ОплаченоТарифРешения != null ? snap.ОплаченоТарифРешения.Value : 0)).ToMagic(2);
                        paymentDocs.Add(new importPaymentDocumentRequestPaymentDocument
                        {
                            AccountGuid = persAcc.GisGkhGuid,
                            Item = true,
                            ItemElementName = ItemChoiceType5.Expose,
                            Items = new object[]
                            {
                                    new PaymentDocumentTypeCapitalRepairCharge
                                    {
                                        Contribution = Contribution,
                                        AccountingPeriodTotal = AccountingPeriodTotal,
                                        AccountingPeriodTotalSpecified = true,
                                        MoneyRecalculation = MoneyRecalculation,
                                        MoneyRecalculationSpecified = true,
                                        TotalPayable = TotalPayable,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriod = DebtPreviousPeriodsOrAdvanceBillingPeriod,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriodSpecified = true,
                                        Penalties = Penalties,
                                        PenaltiesSpecified = true,
                                        TotalPayableOverall = TotalPayableOverall,
                                        TotalPayableOverallSpecified = true,
                                        //PaymentInformationKey = InfoTransportGuid
                                    }
                            },
                            Items1 = new object[]
                            {
                                InfoTransportGuid
                            },
                            PaidCash = PaidCash,
                            PaidCashSpecified = true,
                            TransportGUID = DocTransportGuid,
                            PaymentDocumentID = !string.IsNullOrEmpty(gisPayDoc.PaymentDocumentID) ? gisPayDoc.PaymentDocumentID : null
                        });
                    }
                    if (paymentDocs.Count == 0)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        req.Answer = "Отсутствуют данные для выгрузки по заданным критериям";
                    }
                    else
                    {
                        var request = BillServiceAsync.importPaymentDocumentDataReq((short)chargePeriod.StartDate.Year,
                                            chargePeriod.StartDate.Month, paymentInfos, paymentDocs);
                        var prefixer = new XmlNsPrefixer();
                        XmlDocument document = SerializeRequest(request);
                        prefixer.Process(document);
                        SaveFile(req, GisGkhFileType.request, document, "request.xml");
                        req.RequestState = GisGkhRequestState.Formed;
                        req.Answer = "Запрос на выгрузку начислений сформирован";
                    }
                    GisGkhRequestsDomain.Update(req);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }*/

        public void SaveRequestTaskExecutor(GisGkhRequests req, string chargePeriodId, long? roId, bool rewrite, Pair<string,string>[] snaps, bool first)
        {
            try
            {
                var chargePeriod = ChargePeriodDomain.Get(long.Parse(chargePeriodId));
                if (!first)
                {
                    var newReq = new GisGkhRequests();
                    newReq.TypeRequest = req.TypeRequest;
                    newReq.Operator = req.Operator;
                    newReq.RequestState = GisGkhRequestState.NotFormed;
                    req = newReq;
                    GisGkhRequestsDomain.Save(req);
                }
                List<importPaymentDocumentRequestPaymentInformation> paymentInfos = new List<importPaymentDocumentRequestPaymentInformation>();
                List<importPaymentDocumentRequestPaymentDocument> paymentDocs = new List<importPaymentDocumentRequestPaymentDocument>();
                foreach (var payDoc in snaps)
                {
                    // сюда приходят строки с информацией о начислении из AccountPaymentInfoSnapshot (Second) и PaymentDocumentSnapshot (First)
                    // если в AccountPaymentInfoSnapshot пусто - то берём только из PaymentDocumentSnapshot, тип InvoiceInfo
                    if (payDoc.Second == null)
                    { 
                        // PaymentInformation из InvoiceInfo
                        var snap = JsonConvert.DeserializeObject<InvoiceInfo>((string)payDoc.First);
                        //var snap = payDoc.Snapshot.ConvertTo<InvoiceInfo>();
                        //var persAcc = BasePersonalAccountRepo.Get(snap.AccountId);
                        var persAcc = BasePersonalAccountRepo.Get(snap.AccountId);
                        string InfoTransportGuid = null;
                        if (!paymentInfos.Any(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя))
                        {
                            InfoTransportGuid = Guid.NewGuid().ToString();
                            paymentInfos.Add(new importPaymentDocumentRequestPaymentInformation
                            {
                                BankBIK = snap.БикБанка,
                                operatingAccountNumber = snap.РсчетПолучателя,
                                TransportGUID = InfoTransportGuid
                            });
                        }
                        else
                        {
                            InfoTransportGuid = paymentInfos.Where(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя).First().TransportGUID;
                        }
                        string DocTransportGuid = Guid.NewGuid().ToString();
                        GisGkhPayDoc gisPayDoc = null;
                        if (rewrite)
                        {
                            gisPayDoc = GisGkhPayDocDomain.FirstOrDefault(x => x.Account == persAcc && x.Period == chargePeriod);
                        }
                        if (gisPayDoc == null)
                        {
                            gisPayDoc = new GisGkhPayDoc
                            {
                                Account = persAcc,
                                Period = chargePeriod,
                                PaymentDocumentTransportGUID = DocTransportGuid
                            };
                            GisGkhPayDocDomain.Save(gisPayDoc);
                        }
                        else
                        {
                            gisPayDoc.PaymentDocumentTransportGUID = DocTransportGuid;
                            GisGkhPayDocDomain.Update(gisPayDoc);
                        }
                        var Contribution = (snap.Тариф != null ? snap.Тариф.Value : 0).ToMagic(2);
                        var AccountingPeriodTotal = ((snap.НачисленоБазовый != null ? snap.НачисленоБазовый.Value : 0) +
                                                                (snap.НачисленоТарифРешения != null ? snap.НачисленоТарифРешения.Value : 0)).ToMagic(2);
                        var MoneyRecalculation = ((snap.ПерерасчетБазовый != null ? snap.ПерерасчетБазовый.Value : 0) -
                                                             (snap.ОтменыБазовый != null ? snap.ОтменыБазовый.Value : 0) -
                                                             (snap.ОтменыКорректировкаБазовый != null ? snap.ОтменыКорректировкаБазовый.Value : 0) +
                                                             (snap.КорректировкаБазовый != null ? snap.КорректировкаБазовый.Value : 0) +
                                                             (snap.СлияниеБазовый != null ? snap.СлияниеБазовый.Value : 0) -
                                                             (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                             (snap.ПерерасчетТарифРешения != null ? snap.ПерерасчетТарифРешения.Value : 0) -
                                                             (snap.ОтменыТарифРешения != null ? snap.ОтменыТарифРешения.Value : 0) -
                                                             (snap.ОтменыКорректировкаТарифРешения != null ? snap.ОтменыКорректировкаТарифРешения.Value : 0) +
                                                             (snap.КорректировкаТарифРешения != null ? snap.КорректировкаТарифРешения.Value : 0) +
                                                             (snap.СлияниеТарифРешения != null ? snap.СлияниеТарифРешения.Value : 0) -
                                                             (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0)).ToMagic(2);
                        var TotalPayable = AccountingPeriodTotal + MoneyRecalculation;
                        var TotalPayableOverall = ((snap.ДолгБазовыйНаКонец != null ? snap.ДолгБазовыйНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                       (snap.ДолгТарифРешенияНаКонец != null ? snap.ДолгТарифРешенияНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0) +
                                                       (snap.ДолгПениНаКонец != null ? snap.ДолгПениНаКонец.Value : 0) > 0 ?
                                                       (snap.ДолгБазовыйНаКонец != null ? snap.ДолгБазовыйНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                       (snap.ДолгТарифРешенияНаКонец != null ? snap.ДолгТарифРешенияНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0) +
                                                       (snap.ДолгПениНаКонец != null ? snap.ДолгПениНаКонец.Value : 0) : 0).ToMagic(2);
                        var DebtPreviousPeriodsOrAdvanceBillingPeriod = ((snap.ДолгБазовыйНаНачало != null ? snap.ДолгБазовыйНаНачало.Value : 0) +
                                                                                    (snap.ДолгТарифРешенияНаНачало != null ? snap.ДолгТарифРешенияНаНачало.Value : 0) -
                                                                                    (snap.ОплаченоБазовый != null ? snap.ОплаченоБазовый.Value : 0) -
                                                                                    (snap.ОплаченоТарифРешения != null ? snap.ОплаченоТарифРешения.Value : 0)).ToMagic(2);
                        var Penalties = (snap.ДолгПениНаКонец != null ? snap.ДолгПениНаКонец.Value : 0).ToMagic(2);
                        var PaidCash = ((snap.ОплаченоБазовый != null ? snap.ОплаченоБазовый.Value : 0) +
                                        (snap.ОплаченоТарифРешения != null ? snap.ОплаченоТарифРешения.Value : 0)).ToMagic(2);
                        paymentDocs.Add(new importPaymentDocumentRequestPaymentDocument
                        {
                            AccountGuid = persAcc.GisGkhGuid,
                            Item = true,
                            ItemElementName = ItemChoiceType5.Expose,
                            Items = new object[]
                            {
                                    new PaymentDocumentTypeCapitalRepairCharge
                                    {
                                        Contribution = Contribution,
                                        AccountingPeriodTotal = AccountingPeriodTotal,
                                        AccountingPeriodTotalSpecified = true,
                                        MoneyRecalculation = MoneyRecalculation,
                                        MoneyRecalculationSpecified = true,
                                        TotalPayable = TotalPayable,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriod = DebtPreviousPeriodsOrAdvanceBillingPeriod,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriodSpecified = true,
                                        Penalties = Penalties,
                                        PenaltiesSpecified = true,
                                        TotalPayableOverall = TotalPayableOverall,
                                        TotalPayableOverallSpecified = true,
                                        //PaymentInformationKey = InfoTransportGuid
                                    }
                            },
                            Items1 = new object[]
                            {
                                InfoTransportGuid
                            },
                            PaidCash = PaidCash,
                            PaidCashSpecified = true,
                            TransportGUID = DocTransportGuid,
                            PaymentDocumentID = !string.IsNullOrEmpty(gisPayDoc.PaymentDocumentID) ? gisPayDoc.PaymentDocumentID : null
                        });
                    }
                    // если 2 строки - то строка из PaymentDocumentSnapshot (First) и строка из AccountPaymentInfoSnapshot (Second)
                    // типы = InvoiceInfo и AccountInfo соответственно
                    else
                    {
                        // PaymentInformation из AccountInfo
                        var snap = JsonConvert.DeserializeObject<InvoiceInfo>((string)payDoc.First);
                        var accSnap = JsonConvert.DeserializeObject<AccountInfo>((string)payDoc.Second);
                        var persAcc = BasePersonalAccountRepo.Get(accSnap.AccountId);
                        string InfoTransportGuid = null;
                        if (!paymentInfos.Any(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя))
                        {
                            InfoTransportGuid = Guid.NewGuid().ToString();
                            paymentInfos.Add(new importPaymentDocumentRequestPaymentInformation
                            {
                                BankBIK = snap.БикБанка,
                                operatingAccountNumber = snap.РсчетПолучателя,
                                TransportGUID = InfoTransportGuid
                            });
                        }
                        else
                        {
                            InfoTransportGuid = paymentInfos.Where(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя).First().TransportGUID;
                        }
                        string DocTransportGuid = Guid.NewGuid().ToString();
                        GisGkhPayDoc gisPayDoc = null;
                        if (rewrite)
                        {
                            gisPayDoc = GisGkhPayDocDomain.FirstOrDefault(x => x.Account == persAcc && x.Period == chargePeriod);
                        }
                        if (gisPayDoc == null)
                        {
                            gisPayDoc = new GisGkhPayDoc
                            {
                                Account = persAcc,
                                Period = chargePeriod,
                                PaymentDocumentTransportGUID = DocTransportGuid
                            };
                            GisGkhPayDocDomain.Save(gisPayDoc);
                        }
                        else
                        {
                            gisPayDoc.PaymentDocumentTransportGUID = DocTransportGuid;
                            GisGkhPayDocDomain.Update(gisPayDoc);
                        }
                        var Contribution = accSnap.Тариф.ToMagic(2);
                        var AccountingPeriodTotal = (accSnap.НачисленоБазовый +
                                                                accSnap.НачисленоТарифРешения).ToMagic(2);
                        var MoneyRecalculation = (accSnap.ПерерасчетБазовый -
                                                             accSnap.ОтменыБазовый -
                                                             accSnap.ОтменыКорректировкаБазовый +
                                                             accSnap.КорректировкаБазовый +
                                                             accSnap.СлияниеБазовый
                                                             //-
                                                             //accSnap.ЗачетСредствБазовый
                                                             +
                                                             accSnap.ПерерасчетТарифРешения -
                                                             accSnap.ОтменыТарифРешения -
                                                             accSnap.ОтменыКорректировкаТарифРешения +
                                                             accSnap.КорректировкаТарифРешения +
                                                             accSnap.СлияниеТарифРешения
                                                             //-
                                                             //accSnap.ЗачетСредствТарифРешения
                                                             ).ToMagic(2);
                        var TotalPayable = AccountingPeriodTotal + MoneyRecalculation;
                        var TotalPayableOverall = (accSnap.ДолгБазовыйНаКонец
                                                       //-
                                                       //accSnap.ЗачетСредствБазовый
                                                       +
                                                       accSnap.ДолгТарифРешенияНаКонец
                                                       //-
                                                       //accSnap.ЗачетСредствТарифРешения
                                                       +
                                                       accSnap.ДолгПениНаКонец
                                                       > 0 ?
                                                       accSnap.ДолгБазовыйНаКонец
                                                       //-
                                                       //accSnap.ЗачетСредствБазовый
                                                       +
                                                       accSnap.ДолгТарифРешенияНаКонец
                                                       //-
                                                       //accSnap.ЗачетСредствТарифРешения
                                                       +
                                                       accSnap.ДолгПениНаКонец
                                                       : 0).ToMagic(2);
                        var DebtPreviousPeriodsOrAdvanceBillingPeriod = (accSnap.ДолгБазовыйНаНачало +
                                                                                    accSnap.ДолгТарифРешенияНаНачало -
                                                                                    accSnap.ОплаченоБазовый -
                                                                                    accSnap.ОплаченоТарифРешения).ToMagic(2);
                        var Penalties = accSnap.ДолгПениНаКонец.ToMagic(2);
                        var PaidCash = (accSnap.ОплаченоБазовый +
                                        accSnap.ОплаченоТарифРешения).ToMagic(2);
                        paymentDocs.Add(new importPaymentDocumentRequestPaymentDocument
                        {
                            AccountGuid = persAcc.GisGkhGuid,
                            Item = true,
                            ItemElementName = ItemChoiceType5.Expose,
                            Items = new object[]
                            {
                                    new PaymentDocumentTypeCapitalRepairCharge
                                    {
                                        Contribution = Contribution,
                                        AccountingPeriodTotal = AccountingPeriodTotal,
                                        AccountingPeriodTotalSpecified = true,
                                        MoneyRecalculation = MoneyRecalculation,
                                        MoneyRecalculationSpecified = true,
                                        TotalPayable = TotalPayable,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriod = DebtPreviousPeriodsOrAdvanceBillingPeriod,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriodSpecified = true,
                                        Penalties = Penalties,
                                        PenaltiesSpecified = true,
                                        TotalPayableOverall = TotalPayableOverall,
                                        TotalPayableOverallSpecified = true,
                                        //PaymentInformationKey = InfoTransportGuid
                                    }
                            },
                            Items1 = new object[]
                            {
                                InfoTransportGuid
                            },
                            PaidCash = PaidCash,
                            PaidCashSpecified = true,
                            TransportGUID = DocTransportGuid,
                            PaymentDocumentID = !string.IsNullOrEmpty(gisPayDoc.PaymentDocumentID) ? gisPayDoc.PaymentDocumentID : null
                        });
                    }
                }
                if (paymentDocs.Count == 0)
                {
                    req.RequestState = GisGkhRequestState.Error;
                    req.Answer = "Отсутствуют данные для выгрузки по заданным критериям";
                }
                else
                {
                    var request = BillServiceAsync.importPaymentDocumentDataReq((short)chargePeriod.StartDate.Year,
                                        chargePeriod.StartDate.Month, paymentInfos, paymentDocs);
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    req.Answer = "Запрос на выгрузку начислений сформирован";
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        /*public void SaveSingleRequest(GisGkhRequests req, ChargePeriod chargePeriod, int i, bool rewrite, List<PayInfo> accountPaymentInfoSnapshotsPart)
        {
            try
            {
                for (int i = 0; i < reqNum; i++)
                {
                    //var paymentDocumentSnapshotsPart = paymentDocumentSnapshots.Skip(i * numDocs).Take(numDocs).ToList();
                    var accountPaymentInfoSnapshotsPart = accountPaymentInfoSnapshots.Skip(i * numDocs).Take(numDocs);

                    if (i > 0)
                    {
                        var newReq = new GisGkhRequests();
                        newReq.TypeRequest = req.TypeRequest;
                        newReq.Operator = req.Operator;
                        newReq.RequestState = GisGkhRequestState.NotFormed;
                        req = newReq;
                        GisGkhRequestsDomain.Save(req);
                    }
                    //List<Tuple<string, string, string>> paymentInfo = new List<Tuple<string, string, string>>();
                    List<importPaymentDocumentRequestPaymentInformation> paymentInfos = new List<importPaymentDocumentRequestPaymentInformation>();
                    List<importPaymentDocumentRequestPaymentDocument> paymentDocs = new List<importPaymentDocumentRequestPaymentDocument>();
                    //foreach (var payDoc in paymentDocumentSnapshotsPart)
                    foreach (var payDoc in accountPaymentInfoSnapshotsPart)
                    {
                        // PaymentInformation
                        var snap = JsonConvert.DeserializeObject<InvoiceInfo>(payDoc.Data);
                        //var snap = payDoc.Snapshot.ConvertTo<InvoiceInfo>();
                        //var persAcc = BasePersonalAccountRepo.Get(snap.AccountId);
                        var persAcc = BasePersonalAccountRepo.Get(payDoc.AccountId);
                        string InfoTransportGuid = null;
                        if (!paymentInfos.Any(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя))
                        {
                            InfoTransportGuid = Guid.NewGuid().ToString();
                            paymentInfos.Add(new importPaymentDocumentRequestPaymentInformation
                            {
                                BankBIK = snap.БикБанка,
                                operatingAccountNumber = snap.РсчетПолучателя,
                                TransportGUID = InfoTransportGuid
                            });
                        }
                        else
                        {
                            InfoTransportGuid = paymentInfos.Where(x => x.BankBIK == snap.БикБанка && x.operatingAccountNumber == snap.РсчетПолучателя).First().TransportGUID;
                        }
                        string DocTransportGuid = Guid.NewGuid().ToString();
                        GisGkhPayDoc gisPayDoc = null;
                        if (rewrite)
                        {
                            gisPayDoc = GisGkhPayDocDomain.FirstOrDefault(x => x.Account == persAcc && x.Period == chargePeriod);
                        }
                        if (gisPayDoc == null)
                        {
                            gisPayDoc = new GisGkhPayDoc
                            {
                                Account = persAcc,
                                Period = chargePeriod,
                                PaymentDocumentTransportGUID = DocTransportGuid
                            };
                            GisGkhPayDocDomain.Save(gisPayDoc);
                        }
                        else
                        {
                            gisPayDoc.PaymentDocumentTransportGUID = DocTransportGuid;
                            GisGkhPayDocDomain.Update(gisPayDoc);
                        }
                        var Contribution = (snap.Тариф != null ? snap.Тариф.Value : 0).ToMagic(2);
                        var AccountingPeriodTotal = ((snap.НачисленоБазовый != null ? snap.НачисленоБазовый.Value : 0) +
                                                                (snap.НачисленоТарифРешения != null ? snap.НачисленоТарифРешения.Value : 0)).ToMagic(2);
                        var MoneyRecalculation = ((snap.ПерерасчетБазовый != null ? snap.ПерерасчетБазовый.Value : 0) -
                                                             (snap.ОтменыБазовый != null ? snap.ОтменыБазовый.Value : 0) -
                                                             (snap.ОтменыКорректировкаБазовый != null ? snap.ОтменыКорректировкаБазовый.Value : 0) +
                                                             (snap.КорректировкаБазовый != null ? snap.КорректировкаБазовый.Value : 0) +
                                                             (snap.СлияниеБазовый != null ? snap.СлияниеБазовый.Value : 0) -
                                                             (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                             (snap.ПерерасчетТарифРешения != null ? snap.ПерерасчетТарифРешения.Value : 0) -
                                                             (snap.ОтменыТарифРешения != null ? snap.ОтменыТарифРешения.Value : 0) -
                                                             (snap.ОтменыКорректировкаТарифРешения != null ? snap.ОтменыКорректировкаТарифРешения.Value : 0) +
                                                             (snap.КорректировкаТарифРешения != null ? snap.КорректировкаТарифРешения.Value : 0) +
                                                             (snap.СлияниеТарифРешения != null ? snap.СлияниеТарифРешения.Value : 0) -
                                                             (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0)).ToMagic(2);
                        var TotalPayable = AccountingPeriodTotal + MoneyRecalculation;
                        var TotalPayableOverall = ((snap.ДолгБазовыйНаКонец != null ? snap.ДолгБазовыйНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                       (snap.ДолгТарифРешенияНаКонец != null ? snap.ДолгТарифРешенияНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0) > 0 ?
                                                       (snap.ДолгБазовыйНаКонец != null ? snap.ДолгБазовыйНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствБазовый != null ? snap.ЗачетСредствБазовый.Value : 0) +
                                                       (snap.ДолгТарифРешенияНаКонец != null ? snap.ДолгТарифРешенияНаКонец.Value : 0) -
                                                       (snap.ЗачетСредствТарифРешения != null ? snap.ЗачетСредствТарифРешения.Value : 0) : 0).ToMagic(2);
                        var DebtPreviousPeriodsOrAdvanceBillingPeriod = ((snap.ДолгБазовыйНаНачало != null ? snap.ДолгБазовыйНаНачало.Value : 0) +
                                                                                    (snap.ДолгТарифРешенияНаНачало != null ? snap.ДолгТарифРешенияНаНачало.Value : 0) -
                                                                                    (snap.ОплаченоБазовый != null ? snap.ОплаченоБазовый.Value : 0) -
                                                                                    (snap.ОплаченоТарифРешения != null ? snap.ОплаченоТарифРешения.Value : 0)).ToMagic(2);
                        var Penalties = (snap.ДолгПениНаКонец != null ? snap.ДолгПениНаКонец.Value : 0).ToMagic(2);
                        var PaidCash = ((snap.ОплаченоБазовый != null ? snap.ОплаченоБазовый.Value : 0) +
                                        (snap.ОплаченоТарифРешения != null ? snap.ОплаченоТарифРешения.Value : 0)).ToMagic(2);
                        paymentDocs.Add(new importPaymentDocumentRequestPaymentDocument
                        {
                            AccountGuid = persAcc.GisGkhGuid,
                            // Даже если ты мега-гуру и уверен на все 146 (сто сорок шесть) %, 
                            // что здесь ошибка и ты ее быстро исправишь - НЕ ТРОГАЙ ЭТУ СТРОЧКУ
                            Item = true,
                            ItemElementName = ItemChoiceType5.Expose,
                            Items = new object[]
                            {
                                    new PaymentDocumentTypeCapitalRepairCharge
                                    {
                                        Contribution = Contribution,
                                        AccountingPeriodTotal = AccountingPeriodTotal,
                                        AccountingPeriodTotalSpecified = true,
                                        MoneyRecalculation = MoneyRecalculation,
                                        MoneyRecalculationSpecified = true,
                                        TotalPayable = TotalPayable,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriod = DebtPreviousPeriodsOrAdvanceBillingPeriod,
                                        DebtPreviousPeriodsOrAdvanceBillingPeriodSpecified = true,
                                        Penalties = Penalties,
                                        PenaltiesSpecified = true,
                                        TotalPayableOverall = TotalPayableOverall,
                                        TotalPayableOverallSpecified = true,
                                        //PaymentInformationKey = InfoTransportGuid
                                    }
                            },
                            Items1 = new object[]
                            {
                                InfoTransportGuid
                            },
                            PaidCash = PaidCash,
                            PaidCashSpecified = true,
                            TransportGUID = DocTransportGuid,
                            PaymentDocumentID = !string.IsNullOrEmpty(gisPayDoc.PaymentDocumentID) ? gisPayDoc.PaymentDocumentID : null
                        });
                    }
                    if (paymentDocs.Count == 0)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        req.Answer = "Отсутствуют данные для выгрузки по заданным критериям";
                    }
                    else
                    {
                        var request = BillServiceAsync.importPaymentDocumentDataReq((short)chargePeriod.StartDate.Year,
                                            chargePeriod.StartDate.Month, paymentInfos, paymentDocs);
                        var prefixer = new XmlNsPrefixer();
                        XmlDocument document = SerializeRequest(request);
                        prefixer.Process(document);
                        SaveFile(req, GisGkhFileType.request, document, "request.xml");
                        req.RequestState = GisGkhRequestState.Formed;
                    }
                    GisGkhRequestsDomain.Update(req);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }*/

        public void CheckAnswer(GisGkhRequests req, string orgPPAGUID)
        {
            try
            {
                var response = BillServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer = "Ответ получен";
                    req.RequestState = GisGkhRequestState.ResponseReceived;
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = "Сбой создания задачи обработки ответа";
                            GisGkhRequestsDomain.Update(req);
                            //req.RequestState = GisGkhRequestState.У; ("Сбой создания задачи");
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа importPaymentDocumentData поставлена в очередь с id {taskInfo.TaskId}";
                            GisGkhRequestsDomain.Update(req);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer = "Запрос ещё в очереди";
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer = e.Message;
                GisGkhRequestsDomain.Update(req);
            }
        }

        public void ProcessAnswer (GisGkhRequests req)
        {
            if (req.RequestState == GisGkhRequestState.ResponseReceived)
            {
                try
                {
                    var fileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(fileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        //return Encoding.UTF8.GetString(data);
                        var response = DeserializeData<getStateResult>(Encoding.UTF8.GetString(data));
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)responseItem;
                                req.Answer = $"{error.ErrorCode}:{error.Description}";
                            }
                            else if (responseItem is CommonResultType)
                            {
                                var item = (CommonResultType)responseItem;
                                GisGkhPayDoc payDoc = GisGkhPayDocDomain.GetAll()
                                    .Where(x => x.PaymentDocumentTransportGUID == item.TransportGUID).FirstOrDefault();
                                if (payDoc != null)
                                {
                                    if (item.Items[0] is CommonResultTypeError)
                                    {

                                    }
                                    else
                                    {
                                        item.Items.ToList()
                                            .ForEach(x =>
                                            {
                                                if (x is string)
                                                {
                                                    payDoc.PaymentDocumentID = (string)x;
                                                }
                                            });
                                        payDoc.GisGkhGuid = item.GUID;
                                        GisGkhPayDocDomain.Update(payDoc);
                                    }
                                }
                            }
                        }
                        req.Answer = "Данные из ГИС ЖКХ обработаны";
                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                        GisGkhRequestsDomain.Update(req);
                    }
                    else
                    {
                        throw new Exception("Не найден файл с ответом из ГИС ЖКХ");
                    }
                }
                catch (Exception e)
                {
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: " + e.Message);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Сериаилазация запроса
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <returns>Xml-документ</returns>
        protected XmlDocument SerializeRequest(object data)
        {
            var type = data.GetType();
            XmlDocument result;

            var attr = (XmlTypeAttribute)type.GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(type, attr.Namespace);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, data);

                    result = new XmlDocument();
                    result.LoadXml(stringWriter.ToString());
                }
            }

            //var prefixer = new XmlNsPrefixer();
            //prefixer.Process(result);

            return result;
        }

        protected TDataType DeserializeData<TDataType>(string data)
        {
            TDataType result;

            var attr = (XmlTypeAttribute)typeof(TDataType).GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(typeof(TDataType), attr.Namespace);

            using (var reader = XmlReader.Create(new StringReader(data)))
            {
                result = (TDataType)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        private Bars.B4.Modules.FileStorage.FileInfo SaveFile(byte[] data, string fileName)
        {
            if (data == null)
                return null;

            try
            {
                //сохраняем пакет
                return _fileManager.SaveFile(fileName, data);
            }
            catch (Exception eeeeeeee)
            {
                return null;
            }
        }

        private void SaveFile(GisGkhRequests req, GisGkhFileType fileType, XmlDocument data, string fileName)
        {
            if (data == null)
                throw new Exception("Пустой документ для сохранения");

            MemoryStream stream = new MemoryStream();
            data.PreserveWhitespace = true;
            data.Save(stream);
            try
            {
                //сохраняем
                GisGkhRequestsFileDomain.Save(new GisGkhRequestsFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    GisGkhRequests = req,
                    GisGkhFileType = fileType,
                    FileInfo = _fileManager.SaveFile(stream, fileName)
                });
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при сохранении файла: " + e.Message);
            }
        }

        #endregion

    }
}
