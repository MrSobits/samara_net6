namespace Bars.Gkh.Regions.Tatarstan.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.ConfigSections.PostalService;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Models;
    using Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.Gkh.Regions.Tatarstan.Properties;
    using Bars.Gkh.Services.ServiceContracts;
    using Bars.Gkh.Services.ServiceContracts.Mail;
    using Bars.Gkh.Utils;

    using Castle.Core.Internal;

    using Newtonsoft.Json;

    using NHibernate.Util;

    public class SubRequestPostSendAction : BaseExecutionAction
    {
        // Лог отправки сообщений
        private List<MailSendingLogger> mailSendingLog;

        private DebtRequestMailNotificationType notificationType;

        // Было ли выброшено исключение
        private bool isExceptionWasThrown;

        // Словарь шаблонов писем
        private Dictionary<TemplateType, string> maillingTemplateDict = new Dictionary<TemplateType, string>
        {
            {TemplateType.DebtSubRequestWaitingForResponseMailTemplate, Encoding.UTF8.GetString(Resources.DebtSubRequestWaitingForResponseMailTemplate)},
            {TemplateType.DebtSubRequestNotSentQuantityLessThan50MailTemplate, Encoding.GetEncoding(1251).GetString(Resources.DebtSubRequestNotSentQuantityLessThan50MailTemplate)},
            {TemplateType.DebtSubRequestNotSentQuantityMoreThan50MailTemplate, Encoding.GetEncoding(1251).GetString(Resources.DebtSubRequestNotSentQuantityMoreThan50MailTemplate)}
        };

        /// <inheritdoc />
        public override string Name => "РИС ЖКХ - Оповещение поставщиков ЖКУ по электронной почте о наличии запросов по задолженности";

        /// <inheritdoc />
        public override string Description => "Действие позволяет получить сведения из РИС ЖКХ о новых запросах направленных поставщикам ЖКУ " +
            "о наличии или отсутствии задолженности у граждан. Действие также позволяет оповещать поставщиков ЖКУ по электронной почте о поступивших запросах.";

        /// <inheritdoc />
        public override Func<IDataResult> Action => Execute;

        /// <summary>
        /// Выполнить действие
        /// </summary>
        /// <returns></returns>
        private LoggingDataResult Execute()
        {
            try
            {
                var isPostalServiceEnableInCfg = Container.Resolve<IConfigProvider>().GetConfig().AppSettings.GetAs<bool>("EmailServiceEnabled");
                if (!Container.GetGkhConfig<PostalServiceConfig>().EnablePostalService || !isPostalServiceEnableInCfg)
                {
                    throw new Exception("Почтовый сервис выключен");
                }
                
                isExceptionWasThrown = false;
                mailSendingLog = new List<MailSendingLogger>();

                var subRequests = GetNotSendedPostInformation().SubRequests;
                if (!subRequests.Any())
                {
                    return new LoggingDataResult(true, "Неотправленных задолженностей не обнаружено");
                }

                notificationType = this.ExecutionParams.Params.GetAs<DebtRequestMailNotificationType>("NotificationType");
                Func<DebtSubRequestInfo, bool> statusExpression = null;
                
                switch (notificationType)
                {
                    case DebtRequestMailNotificationType.RequestWithoutResponse:
                        statusExpression = (y => y.ResponseStatus.Equals("WaitingForResponse"));
                        break;
                    case DebtRequestMailNotificationType.RequestWitNotSentResponse:
                        statusExpression = (y => y.ResponseStatus.Equals("NotSent"));
                        break;
                }
                
                subRequests.ForEach(request =>
                {
                    request.SubRequestsInfo = request.SubRequestsInfo.Where(statusExpression).ToList();
                });

                subRequests.RemoveAll(request => request.SubRequestsInfo.IsEmpty());
                if (!subRequests.Any())
                {
                    return new LoggingDataResult(true, "Неотправленных задолженностей не обнаружено");
                }

                ProcessAndSendMailingInformation(subRequests);

                return this.isExceptionWasThrown 
                    ? new LoggingDataResult(false, mailSendingLog) 
                    : new LoggingDataResult(true, mailSendingLog);
            }
            catch (Exception ex)
            {
                return new LoggingDataResult(false, null, mailSendingLog, ex.StackTrace, ex.GetIerarchyErrorObject());
            }
        }

        /// <summary>
        /// Подготовка и отправка почтовой информации по полученному ответу из сервиса РИС ЖКХ "/subrequest/listnotsended"
        /// </summary>
        /// <param name="subRequests"></param>
        /// <returns></returns>
        private void ProcessAndSendMailingInformation(IEnumerable<DebtSubRequest> subRequests)
        {
            var contragentDomainService = Container.ResolveDomain<Contragent>();
            var postalService = Container.Resolve<IPostalService>();

            using (Container.Using(contragentDomainService, postalService))
            {
                foreach (var subRequest in subRequests)
                {
                    var expression = 
                            subRequest.Kpp != null
                                ? (Expression<Func<Contragent, bool>>) 
                                (
                                    contragent =>
                                        contragent.Inn == subRequest.Inn && 
                                        contragent.Kpp == subRequest.Kpp &&
                                        contragent.Ogrn == subRequest.Ogrn
                                )
                                : (Expression<Func<Contragent, bool>>) 
                                (
                                    contragent => 
                                        contragent.Inn == subRequest.Inn &&
                                        contragent.Ogrn == subRequest.Ogrn
                                );

                        try
                        {
                            var contragentList = contragentDomainService.GetAll()
                                .Where(contragent => contragent.ReceiveNotifications == YesNo.Yes)
                                .Where(expression)
                                .Select(x => new ContragentMailingProxy()
                                {
                                    Inn = x.Inn,
                                    Kpp = x.Kpp,
                                    Ogrn = x.Ogrn,
                                    Email = x.Email,
                                    Name = x.Name
                                })
                                .ToList();
                            
                            contragentList.ForEach(x => x.SubRequestInfoList = subRequest.SubRequestsInfo);

                            if (contragentList.Count == 0)
                            {
                                throw new Exception("не произошло сопоставления ни с одним контрагентом");
                            }

                            SendPost(contragentList, postalService);
                        }
                        catch (Exception ex)
                        {
                            isExceptionWasThrown = true;
                            mailSendingLog.Add
                            (
                                new MailSendingLogger(subRequest.Ogrn, 
                                subRequest.Inn, 
                                subRequest.Kpp, 
                                ex.GetIerarchyErrorObject($"Ошибка при сопоставлении контрагента: "))
                            );
                        }
                }
            }
        }

        /// <summary>
        /// Получение неотправленных сообщений из РИС ЖКХ
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private DebtSubRequestList GetNotSendedPostInformation()
        {
            var dataExportConfig = Container.GetGkhConfig<AdministrationConfig>().FormatDataExport.FormatDataExportGeneral;
            var endPointAddress = dataExportConfig.TransferServiceAddress;
            var token = dataExportConfig.TransferServiceToken;

            if (string.IsNullOrEmpty(endPointAddress) || string.IsNullOrEmpty(token))
            {
                throw new Exception($"В настройках приложения отсутствует информация об адресе или токене авторизации удаленного сервера для передачи данных");
            }

            WebRequest request = (HttpWebRequest) WebRequest.Create(endPointAddress + "/subrequest/listnotsended");
            request.Headers.Add("Authorization", token);
            request.Timeout = 120 * 1000;
            
            var response = (HttpWebResponse) request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                var subRequestList = JsonConvert.DeserializeObject<DebtSubRequestList>(responseText);

                if (!string.IsNullOrEmpty(subRequestList.ErrorMessage))
                {
                    throw new Exception(subRequestList.ErrorMessage);
                }

                return subRequestList;
            }
        }

        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="contragentMailingProxyList"></param>
        /// <param name="postalService"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private void SendPost(IEnumerable<ContragentMailingProxy> contragentMailingProxyList, IPostalService postalService)
        {
            var requestInformation = new StringBuilder();

            foreach (var mailingInformation in contragentMailingProxyList)
            {
                try
                {
                    if (string.IsNullOrEmpty(mailingInformation.Email))
                    {
                        throw new Exception("Не указан E-mail для получения оповещений");
                    }
                    
                    var message = string.Empty;

                    requestInformation.Clear();
                    switch (notificationType)
                    {
                        case DebtRequestMailNotificationType.RequestWithoutResponse:
                            mailingInformation.SubRequestInfoList
                                .ForEach(info =>
                                {
                                    requestInformation.Append($"<li>{info.Address} (Запрос №{info.RequestNumber}, крайний срок ответа на запрос: {info.FinalDate})</li>");
                                });

                            message = string.Format
                            (
                                maillingTemplateDict[TemplateType.DebtSubRequestWaitingForResponseMailTemplate],
                                mailingInformation.Name,
                                requestInformation
                            );
                            break;
                        
                        case DebtRequestMailNotificationType.RequestWitNotSentResponse:
                            if (mailingInformation.SubRequestInfoList.Count() > 50)
                            {
                                message = string.Format
                                (
                                    maillingTemplateDict[TemplateType.DebtSubRequestNotSentQuantityMoreThan50MailTemplate],
                                    mailingInformation.Name,
                                    mailingInformation.SubRequestInfoList.Count()
                                );
                            }
                            else
                            {
                                mailingInformation.SubRequestInfoList
                                    .ForEach(info =>
                                    {
                                        requestInformation.Append($"<li>Запрос № {info.RequestNumber}; Крайний срок ответа на запрос: {info.FinalDate};  Адрес по которому направлен запрос: {info.Address}</li>");
                                    });
                                
                                message = string.Format
                                (
                                    maillingTemplateDict[TemplateType.DebtSubRequestNotSentQuantityLessThan50MailTemplate],
                                    mailingInformation.Name,
                                    requestInformation
                                );
                            }
                            
                            break;
                    }

                    var mailInfo = new MailInfo(mailingInformation.Email,
                        "Ответ на запросы о наличии или отсутствии у граждан задолженности по оплате ЖКУ",
                        message);
                    postalService.Send(mailInfo);

                    // логируем успешно отправленные сообщения
                    mailSendingLog.Add
                    (
                        new MailSendingLogger(mailingInformation.Ogrn,
                            mailingInformation.Inn,
                            mailingInformation.Kpp)
                    );
                }
                catch (Exception ex)
                {
                    isExceptionWasThrown = true;
                    mailSendingLog.Add
                    (
                       new MailSendingLogger(mailingInformation.Ogrn,
                       mailingInformation.Inn,
                       mailingInformation.Kpp,
                       ex.GetIerarchyErrorObject($"Ошибка отправки E-mail ({mailingInformation.Email}): "))
                    );
                }
            }
        }

        /// <summary>
        /// Вспомогательная сущность для логирования действия
        /// </summary>
        private class MailSendingLogger
        {
            public MailSendingLogger(string ogrn, string inn, string kpp, ErrorAggregator error = null)
            {
                Ogrn = ogrn;
                Inn = inn;
                Kpp = kpp;
                Error = error;
            }
            
            /// <summary>
            /// ОГРН
            /// </summary>
            public string Ogrn { get; set; }
            
            /// <summary>
            /// ИНН
            /// </summary>
            public string Inn { get; set; }
            
            /// <summary>
            /// КПП
            /// </summary>
            public string Kpp { get; set; }

            /// <summary>
            /// Ошибка
            /// </summary>
            public ErrorAggregator Error { get; set; }
        }

        /// <summary>
        /// Прокси контрагента для работы с почтовым сервисом
        /// </summary>
        private class ContragentMailingProxy
        {
            /// <summary>
            /// ОГРН
            /// </summary>
            public string Ogrn { get; set; }
            
            /// <summary>
            /// ИНН
            /// </summary>
            public string Inn { get; set; }
            
            /// <summary>
            /// КПП
            /// </summary>
            public string Kpp { get; set; }

            /// <summary>
            /// Email адрес
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Наименование
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Список информации о задолженностях
            /// </summary>
            public List<DebtSubRequestInfo> SubRequestInfoList { get; set; }
        }

        private enum TemplateType
        {
            DebtSubRequestWaitingForResponseMailTemplate = 1,
            
            DebtSubRequestNotSentQuantityLessThan50MailTemplate = 2,
            
            DebtSubRequestNotSentQuantityMoreThan50MailTemplate = 3
        }
    }
}