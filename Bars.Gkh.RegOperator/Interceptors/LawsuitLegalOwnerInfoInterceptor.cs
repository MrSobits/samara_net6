namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Enums;

    public class LawsuitLegalOwnerInfoInterceptor : LawsuitOwnerInfoInterceptor<LawsuitLegalOwnerInfo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<LawsuitLegalOwnerInfo> service, LawsuitLegalOwnerInfo entity)
        {
            this.UpdateFields(entity);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<LawsuitLegalOwnerInfo> service, LawsuitLegalOwnerInfo entity)
        {
            this.UpdateFields(entity);

            return base.BeforeUpdateAction(service, entity);
        }

        private void UpdateFields(LawsuitLegalOwnerInfo entity)
        {
            entity.OwnerType = PersonalAccountOwnerType.Legal;
            entity.Name = entity.ContragentName;
        }
    }
}