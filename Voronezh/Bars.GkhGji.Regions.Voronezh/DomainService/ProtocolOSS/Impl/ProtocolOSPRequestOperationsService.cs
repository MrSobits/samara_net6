namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
  

    using Castle.Windsor;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using System.Net.Mail;
    using Bars.B4.Modules.FileStorage;

    public class ProtocolOSPRequestOperationsService : IProtocolOSPRequestOperationsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProtocolOSPRequest> ProtocolOSPRequestDomain { get; set; }
        public IDomainService<PropertyOwnerProtocolsDecision> PropertyOwnerProtocolsDecisiontDomain { get; set; }
        public IDomainService<PropertyOwnerProtocols> PropertyOwnerProtocolsDomain { get; set; }

        public IDataResult SendAnswer(BaseParams baseParams)
        {
            var requestId = baseParams.Params.ContainsKey("requestId") ? baseParams.Params["requestId"].ToLong() : 0;
           

            if (requestId == 0)
            {
                return new BaseDataResult(false, "Не удалось отправить ответ по неопределенному запросу");
            }
            var ossrequest = ProtocolOSPRequestDomain.Get(requestId);
            if (ossrequest == null)
            {
                return new BaseDataResult(false, "Не удалось получить запрос");
            }
            if (ossrequest.ProtocolFile == null && ossrequest.Approved == Enums.FuckingOSSState.Yes)
            {
                return new BaseDataResult(false, "Не указан файл протокола");
            }
            if (ossrequest.Approved == Enums.FuckingOSSState.Work)
            {
                return new BaseDataResult(false, "Отправка прервана, не указано решение по заявке");
            }
            SendEmail(ossrequest, MakeAttachment(ossrequest.ProtocolFile));
 

            return new BaseDataResult();
        }      

        public IDataResult GetDocInfo(BaseParams baseParams)
        {
                      
            var docId = baseParams.Params.GetAs("docId", 0L);
            if (docId == 0)
            {
                return new BaseDataResult(false, "Не удалось получить запрос");
            }
            var ossrequest = ProtocolOSPRequestDomain.Get(docId);
            if (ossrequest == null)
            {
                return new BaseDataResult(false, "Не удалось получить запрос");
            }
            if (ossrequest.RealityObject == null)
            {
                return new BaseDataResult(false, "Не установлен адрес МКД");
            }
            //все протоколы грузим в оперативку
            var prots = PropertyOwnerProtocolsDomain.GetAll().Where(x => x.RealityObject == ossrequest.RealityObject).ToList();
            if (!string.IsNullOrEmpty(ossrequest.ProtocolNum) && ossrequest.ProtocolDate.HasValue)
            {
                //ищем по номеру и дате
                var theOneProt = prots.Where(x => x.DocumentNumber == ossrequest.ProtocolNum && x.DocumentDate == ossrequest.ProtocolDate).FirstOrDefault();
                if (theOneProt != null)
                {
                    return new BaseDataResult(theOneProt.DocumentFile);
                }
            }
            if (ossrequest.OwnerProtocolType != null && ossrequest.DateFrom.HasValue && ossrequest.DateTo.HasValue)
            {
                var theOneProt = PropertyOwnerProtocolsDecisiontDomain.GetAll().Where(x => x.Protocol.RealityObject == ossrequest.RealityObject && x.Decision == ossrequest.OwnerProtocolType)
                    .Where(x=> x.Protocol.DocumentDate.HasValue && x.Protocol.DocumentDate >= ossrequest.DateFrom && x.Protocol.DocumentDate<= ossrequest.DateTo).FirstOrDefault();
                if (theOneProt != null)
                {
                    return new BaseDataResult(theOneProt.Protocol.DocumentFile);
                }
            }
            if (ossrequest.DateFrom.HasValue && ossrequest.DateTo.HasValue)
            {
                var theOneProt = prots.Where(x => x.DocumentDate >= ossrequest.DateFrom && x.DocumentDate <= ossrequest.DateTo).FirstOrDefault();
                if (theOneProt != null)
                {
                    return new BaseDataResult(theOneProt.DocumentFile);
                }
            }
            return new BaseDataResult(false, "Не удалось получить протокол по заявленным условиям");
        }

        private void SendEmail(ProtocolOSPRequest entity, Attachment attachment)
        {
            EmailSender emailSender = EmailSender.Instance;
            try
            {
                emailSender.Send(entity.Email, "Уведомление о решении по заявлению", MakeMessageBody(entity), attachment);
            }
            catch
            {

            }
        }
        string MakeMessageBody(ProtocolOSPRequest entity)
        {

            string body = $"Уважаемый(ая) {entity.FIO}!\r\n";
            if (entity.Approved == Enums.FuckingOSSState.Yes)
            {
                body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что Ваше заявление на получение протоколов общего собрания собственников по адресу {entity.Address} одобрена. С протоколами Вы можете ознакомиться в личном кабинете на сайте ГЖИ Воронежской области\r\n";
                body += $"Для ознакомления с приложениями к протоколам общего собрания собственников помещений в многоквартирном доме Вам необходимо обратиться по телефону 212-63-76.\r\n";
            }
            if (entity.Approved == Enums.FuckingOSSState.No)
            {
                body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что Ваше заявление на получение протоколов общего собрания собственников по адресу {entity.Address} отклонена по причине: {entity.ResolutionContent}.\r\n";
            }
            if (entity.Approved == Enums.FuckingOSSState.NotSet)
            {
                body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что протокол ОСС с заявленными параметрами по адресу {entity.Address} отсутствует в архиве АИС ГЖИ.\r\n";
            }
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

        Attachment MakeAttachment(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;

            var fileManager = this.Container.Resolve<IFileManager>();
            return new Attachment(fileManager.GetFile(fileInfo), fileInfo.FullName);
        }

    }
}