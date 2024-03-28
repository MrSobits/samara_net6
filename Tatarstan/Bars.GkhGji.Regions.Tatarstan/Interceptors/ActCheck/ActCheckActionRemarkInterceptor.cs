namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    using Castle.Core.Internal;

    public class ActCheckActionRemarkInterceptor : EmptyDomainInterceptor<ActCheckActionRemark>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<ActCheckActionRemark> service, ActCheckActionRemark entity)
        {
            return ValidateEntity(entity);
        }
        
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<ActCheckActionRemark> service, ActCheckActionRemark entity)
        {
            return ValidateEntity(entity);
        }
        
        private IDataResult ValidateEntity(ActCheckActionRemark entity)
        {
            if (entity.Remark.IsNullOrEmpty() && entity.MemberFio.IsNullOrEmpty())
            {
                return this.Failure("Не заполнены поля \"Замечание\" и \"ФИО участника\"");
            }
            
            return this.Success();
        }
    }
}