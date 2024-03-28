namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Localizers;

    public class MultipurposeGlossaryItemInterceptor : EmptyDomainInterceptor<MultipurposeGlossaryItem>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<MultipurposeGlossaryItem> service, MultipurposeGlossaryItem entity)
        {
            if (entity.Glossary == null || entity.Glossary.Code != TypeContractLocalizer.GlossaryCode)
            {
                return base.BeforeUpdateAction(service, entity);
            }

            var oldItem = service.GetAll().First(x => x.Id == entity.Id);
            if (TypeContractLocalizer.IsDefault(oldItem.Key))
            {
                throw new Exception("Нельзя модифицировать дефолтный тип контракта");
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MultipurposeGlossaryItem> service, MultipurposeGlossaryItem entity)
        {
            if (entity.Glossary == null || entity.Glossary.Code != TypeContractLocalizer.GlossaryCode)
            {
                return base.BeforeDeleteAction(service, entity);
            }

            var oldItem = service.GetAll().First(x => x.Id == entity.Id);
            if (TypeContractLocalizer.IsDefault(oldItem.Key))
            {
                throw new Exception("Нельзя удалять дефолтный тип контракта");
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}