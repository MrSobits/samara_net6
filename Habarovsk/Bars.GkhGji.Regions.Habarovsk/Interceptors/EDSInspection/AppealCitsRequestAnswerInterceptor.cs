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

    public class AppealCitsRequestAnswerInterceptor : EmptyDomainInterceptor<AppealCitsRequestAnswer>
    {
        public IDomainService<AppealCitsRequest> EDSInspectionDomain { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsRequestAnswer> service, AppealCitsRequestAnswer entity)
        {
            try
            // снимаем задачу
            {
                if (entity.SignedFile != null)
                {
                    var x = entity.AppealCitsRequest.SenderInspector;
                    try
                    {
                        if (!string.IsNullOrEmpty(x.Email))
                        {
                            EmailSender emailSender = EmailSender.Instance;
                            emailSender.Send(x.Email, "Уведомление о размещении документа", MakeMessageBody(x, entity.AppealCitsRequest.Contragent.Name, "Ответ на запрос по обращению"), MakeAttachment(entity.SignedFile));
                        }
                    }
                    catch
                    { }

                    var req = EDSInspectionDomain.Get(entity.AppealCitsRequest.Id);
                    req.PerfomanceFactDate = DateTime.Now;
                    EDSInspectionDomain.Update(req);

                }
            }
            catch
            {
                return Failure("При создании документа произошла ошибка");
            }



            return this.Success();

        }

        Attachment MakeAttachment(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;
            var fm = Container.Resolve<IFileManager>();

            return new Attachment(fm.GetFile(fileInfo), fileInfo.FullName);
        }

        string MakeMessageBody(Inspector inspector, string cg, string docname)
        {

            string body = $"Уважаемый(ая) {inspector.Fio}!\r\n";
            body += $"АИС ГЖИ уведомляет Вас о том, что в реестре электронного документооборота организацией {cg} размещен новый документ: {docname}, файл документа прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

    }
}
