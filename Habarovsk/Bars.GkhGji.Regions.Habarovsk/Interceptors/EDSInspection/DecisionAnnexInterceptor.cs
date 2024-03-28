namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System.Linq;
    using System.Net.Mail;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.B4.DataAccess;

    public class DecisionAnnexInterceptor : EmptyDomainInterceptor<DecisionAnnex>
    {
        public IDomainService<EDSInspection> EDSInspectionDomain { get; set; }
        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        //public override IDataResult BeforeUpdateAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        //{
        //    if (entity.SignedFile != null && entity.TypeAnnex != TypeAnnex.NotSet)
        //    {
        //        if (entity.Decision.Inspection.Contragent != null)
        //        {
        //            if (entity.Decision.Inspection.Contragent.IsEDSE)
        //            {
        //                entity.MessageCheck = MessageCheck.Sent;
        //            }
        //        }
        //    }
        //    return Success();
        //}
        public override IDataResult BeforeUpdateAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
 
            try
            // снимаем задачу
            {
                if (entity.MessageCheck != MessageCheck.Sent && entity.SignedFile != null && entity.TypeAnnex != TypeAnnex.NotSet && entity.TypeAnnex != TypeAnnex.CorrespondentNotice)
                {
                    entity.MessageCheck = MessageCheck.Sent;
                    if (entity.Decision.Inspection.Contragent != null)
                    {
                        if (entity.Decision.Inspection.Contragent.IsEDSE)
                        {
                            var edsinsp = EDSInspectionDomain.GetAll()
                       .FirstOrDefault(x => x.InspectionGji == entity.Decision.Inspection);
                            if (edsinsp == null)
                            {
                                EDSInspection newEDS = new EDSInspection
                                {
                                    Contragent = entity.Decision.Inspection.Contragent,
                                    InspectionGji = entity.Decision.Inspection,
                                    InspectionDate = entity.Decision.Inspection.ObjectCreateDate,
                                    InspectionNumber = entity.Decision.Inspection.InspectionNumber,
                                    NotOpened = true,
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectEditDate = DateTime.Now,
                                    ObjectVersion = 1,
                                    TypeBase = entity.Decision.Inspection.TypeBase
                                };
                                EDSInspectionDomain.Save(newEDS);
                            }
                            else
                            {
                                edsinsp.NotOpened = true;
                                EDSInspectionDomain.Update(edsinsp);
                            }

                            string email = entity.Decision.Inspection.Contragent.Email;
                            if (!string.IsNullOrEmpty(email))
                            {
                                try
                                {
                                    EmailSender emailSender = EmailSender.Instance;
                                    emailSender.Send(email, "Уведомление о размещении документа ГЖИ", MakeMessageBody(entity.TypeAnnex), MakeAttachment(entity.SignedFile));
                                }
                                catch
                                { }
                            }
                        }
                    }

                }
                else if(entity.MessageCheck != MessageCheck.Sent && entity.SignedFile != null && entity.TypeAnnex == TypeAnnex.CorrespondentNotice)
                {
                    entity.MessageCheck = MessageCheck.Sent;
                    var appealId = InspectionAppealCitsDomain.GetAll().FirstOrDefault(x=> x.Inspection.Id == entity.Decision.Inspection.Id)?.AppealCits.Id;
                    if (appealId.HasValue)
                    {
                        var appeal = AppealCitsDomain.Get(appealId.Value);
                        if (appeal != null)
                        {
                            string email = appeal.Email;
                            string fio = appeal.Correspondent;
                            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(fio))
                            {
                                try
                                {
                                    EmailSender emailSender = EmailSender.Instance;
                                    emailSender.Send(email, "Уведомление о проведении КНМ", MakeMessageBodyCorresp(fio), MakeAttachment(entity.SignedFile));
                                }
                                catch
                                { }
                            }
                        }
                    }                 

             
                }
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.Name);
            }
            catch
            {
                return Failure("При создании документа произошла ошибка");
            }



            return this.Success();

        }

        public override IDataResult AfterCreateAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var signedInfoDomain = Container.ResolveDomain<PdfSignInfo>();
            try
            {
                var signed = signedInfoDomain.GetAll().Where(x => x.OriginalPdf == entity.File).Select(x => x.Id).ToList();
                foreach (long id in signed)
                {
                    signedInfoDomain.Delete(id);
                }
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Decision", "Решение" },
                { "TypeAnnex", "Тип приложения" },
                { "DocumentDate", "Дата документа" },
                { "Name", "Наименование" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "SignedFile", "Подписанный файл" },
                { "MessageCheck", "Статус файла" },
                { "DocumentSend", "Дата отправки" },
                { "DocumentDelivered", "Дата получения" }
            };
            return result;
        }

        Attachment MakeAttachment(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;
            var fm = Container.Resolve<IFileManager>();

            return new Attachment(fm.GetFile(fileInfo), fileInfo.FullName);
        }

        string MakeMessageBody(TypeAnnex type)
        {
            string state = "";
            switch (type)
            {
                case TypeAnnex.ActCheck:
                    state = "Акт проверки";
                    break;

                case TypeAnnex.Disposal:
                    state = "Приказ";
                    break;

                case TypeAnnex.Decision:
                    state = "Решение";
                    break;

                case TypeAnnex.DisposalNotice:
                    state = "Уведомление";
                    break;

                case TypeAnnex.CorrespondentNotice:
                    state = "Уведомление заявителя";
                    break;

                case TypeAnnex.Prescription:
                    state = "Предписание";
                    break;

                case TypeAnnex.PrescriptionNotice:
                    state = "Уведомление-вызов";
                    break;

                case TypeAnnex.Protocol:
                    state = "Протокол";
                    break;

                case TypeAnnex.Resolution:
                    state = "Постановление";
                    break;

                case TypeAnnex.MotivRequest:
                    state = "Мотивированный запрос";
                    break;
                case TypeAnnex.ProtocolNotification:
                    state = "Уведомления о составлении протокола";
                    break;



            }

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что в реестре электронного документооборота размещен новый документ: {state}, файл документа прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

        string MakeMessageBodyCorresp(string fio)
        {           

            string body = $"Уважаемый(ая) {fio}!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о проведении внеплановой выездной проверки. Файл уведомления прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления граждан и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

    }
}
