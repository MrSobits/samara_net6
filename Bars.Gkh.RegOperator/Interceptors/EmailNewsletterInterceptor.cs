namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using Bars.Gkh.Authentification;
    using Entities;

    public class EmailNewsletterInterceptor : EmptyDomainInterceptor<EmailNewsletter>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager GkhUserManager { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<EmailNewsletter> service, EmailNewsletter entity)
        {
            var activeOperator = GkhUserManager.GetActiveOperator();
            if (activeOperator != null) 
            {
                entity.Sender = activeOperator.User.Name;
                return Success();
            }
            else
            {
                return Failure("У вашего пользователя не выбран оператор");
            }
        }
    }
}