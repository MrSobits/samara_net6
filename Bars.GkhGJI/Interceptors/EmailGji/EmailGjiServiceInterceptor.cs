namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities.Email;
    using Entities;
    using System;
    using System.Linq;

    class EmailGjiServiceInterceptor : EmptyDomainInterceptor<EmailGji>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<EmailGjiAttachment> EmailGjiAttachmentDomain { get; set; }
        public IDomainService<EmailGjiLongText> EmailGjiLongTextDomain { get; set; }     


        public override IDataResult BeforeDeleteAction(IDomainService<EmailGji> service, EmailGji entity)
        {
            try
            {
                EmailGjiAttachmentDomain.GetAll()
               .Where(x => x.Message.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => EmailGjiAttachmentDomain.Delete(x));

                EmailGjiLongTextDomain.GetAll()
              .Where(x => x.EmailGji.Id == entity.Id)
              .Select(x => x.Id)
              .ToList()
              .ForEach(x => EmailGjiLongTextDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<EmailGji>: {e.Message}");
            }

        }
    }
}
