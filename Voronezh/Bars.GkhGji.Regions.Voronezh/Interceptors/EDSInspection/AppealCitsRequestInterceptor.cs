namespace Bars.GkhGji.Regions.Voronezh.Interceptors
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.DomainService;

    public class AppealCitsRequestInterceptor : EmptyDomainInterceptor<AppealCitsRequest>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsRequest> service, AppealCitsRequest entity)
        {

            Operator thisOperator = UserManager.GetActiveOperator();

            if (thisOperator?.Inspector == null)
            {
                return Failure("Изменение данных запроса доступно только сотрудникам ГЖИ");
            }
            else
            {
                entity.SenderInspector = thisOperator.Inspector;
            }

            return this.Success();

        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsRequest> service, AppealCitsRequest entity)
        {
            try
            // снимаем задачу
            {
                if (entity.SignedFile != null)
                {
                    var contragent = ContragentDomain.Get(entity.Contragent.Id);

                    if (contragent != null && contragent.IsEDSE)
                    {
                        string email = contragent.Email;
                        if (!string.IsNullOrEmpty(email))
                        {
                            try
                            {
                                EmailSender emailSender = EmailSender.Instance;
                                emailSender.Send(email, "Уведомление о размещении запроса ГЖИ", MakeMessageBody(), MakeAttachment(entity.SignedFile));
                            }
                            catch
                            { }
                        }
                    }
                }

                var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();

                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsRequest, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.DocumentNumber + " " + entity.DocumentNumber);

            }
            catch
            {


            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsRequest> service, AppealCitsRequest entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsRequest, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsRequest> service, AppealCitsRequest entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsRequest, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.DocumentNumber + " " + entity.DocumentNumber);
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
                { "AppealCits", "Обращение граждан" },
                { "CompetentOrg", "Компетентная организация" },
                { "DocumentName", "Документ" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentDate", "Дата документа" },
                { "PerfomanceDate", "Дата исполнения" },
                { "PerfomanceFactDate", "Дата фактического исполнения" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "SenderInspector", "Инспектор запроса ГЖИ" }
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

        string MakeMessageBody()
        {

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что в реестре электронного документооборота размещен новый запрос, файл запроса прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

    }
}
