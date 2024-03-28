namespace Bars.Gkh.RegOperator.Interceptors.PersonalAccount.PayDoc
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Интерсептор для <see cref="PaymentDocumentTemplate"/>
    /// </summary>
    public class PaymentDocumentTemplateInterceptor : EmptyDomainInterceptor<PaymentDocumentTemplate>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<PaymentDocumentTemplate> service, PaymentDocumentTemplate entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<PaymentDocumentTemplate> service, PaymentDocumentTemplate entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<PaymentDocumentTemplate> service, PaymentDocumentTemplate entity)
        {
            if (string.IsNullOrEmpty(entity.TemplateCode) || entity.Period == null)
            {
                return this.Failure("Не выбраны тип шаблона или период");
            }

            if (!this.CheckUnique(service, entity))
            {
                return this.Failure("Внимание! В этом периоде уже добавлен шаблон выбранного типа");
            }

            return this.Success();
        }

        private bool CheckUnique(IDomainService<PaymentDocumentTemplate> service, PaymentDocumentTemplate entity)
        {
            return !service.GetAll().Any(x => x.TemplateCode == entity.TemplateCode && x.Period.Id == entity.Period.Id && x.Id != entity.Id);
        }
    }
}