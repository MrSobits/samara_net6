namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Voronezh.Entities;

    class VDGOViolatorsInterceptor : EmptyDomainInterceptor<VDGOViolators>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }
        public IDomainService<VDGOViolators> domainService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<VDGOViolators> service, VDGOViolators entity)
        {
            try
            {
                if (entity.Contragent == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    OperatorContragent operatorContragent = OperatorContragentDomain
                        .GetAll()
                        .FirstOrDefault(x => x.Operator == thisOperator);

                    entity.Contragent = operatorContragent.Contragent;
                }
                if (entity.MinOrgContragent == null && entity.Address != null)
                {

                    var contract = ManOrgContractRealityObjectDomain.GetAll()
                        .Where(x => x.RealityObject == entity.Address)
                        .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                        .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
                    if (contract != null)
                    {
                        if (contract.ManOrgContract.ManagingOrganization != null)
                        {
                            entity.MinOrgContragent = contract.ManOrgContract.ManagingOrganization.Contragent;
                        }
                    }

                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }

        public override IDataResult AfterCreateAction(IDomainService<VDGOViolators> service, VDGOViolators entity)
        {
            try
            {
                if (entity.MinOrgContragent != null && entity.Address != null && entity.NotificationFile != null)
                {
                    string email = entity.Email;
                    if (!string.IsNullOrEmpty(email))
                    {
                        EmailSender emailSender = EmailSender.Instance;
                        emailSender.Send(email, "Уведомление", MakeMessageBody(entity), MakeAttachment(entity.NotificationFile));
                        entity.MarkOfMessage = true;                        
                    }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<VDGOViolators> service, VDGOViolators entity)
        {
            try
            {
                if (entity.MinOrgContragent == null && entity.Address != null)
                {

                    var contract = ManOrgContractRealityObjectDomain.GetAll()
                        .Where(x => x.RealityObject == entity.Address)
                        .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                        .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
                    if (contract != null)
                    {
                        if (contract.ManOrgContract.ManagingOrganization != null)
                        {
                            entity.MinOrgContragent = contract.ManOrgContract.ManagingOrganization.Contragent;
                        }
                    }

                }
                if (!entity.MarkOfMessage)
                {
                    if (entity.MinOrgContragent != null && entity.Address != null && entity.NotificationFile != null)
                    {
                        string email = entity.Email;
                        if (!string.IsNullOrEmpty(email))
                        {
                            EmailSender emailSender = EmailSender.Instance;
                            emailSender.Send(email, "Уведомление", MakeMessageBody(entity), MakeAttachment(entity.NotificationFile));
                            entity.MarkOfMessage = true;
                        }
                    }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }
        }

        Attachment MakeAttachment(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;
            var fm = Container.Resolve<IFileManager>();

            return new Attachment(fm.GetFile(fileInfo), fileInfo.FullName);
        }

        string MakeMessageBody(VDGOViolators entity)
        {
            string body = $"Уведомляем вас, что в ваш адрес поступило уведомление от {entity.Contragent.Name} о наличии нарушений со стороны жителей дома {entity.Address.Address}.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления и не предназначен для приема какого-либо рода электронных сообщений (обращений).";
            return body;
        }
    }
}
