namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class TerminationReasonInterceptor : EmptyDomainInterceptor<TerminationReason>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TerminationReason> service, TerminationReason entity)
        {
            return this.CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TerminationReason> service, TerminationReason entity)
        {
            return this.CheckForm(entity);
        }

        private IDataResult CheckForm(TerminationReason entity)
        {
            if (entity.Name.IsEmpty() || entity.Code.IsEmpty())
            {
                return this.Failure("Не заполнены обязательные поля");
            }

            return this.Success();
        }
    }
}