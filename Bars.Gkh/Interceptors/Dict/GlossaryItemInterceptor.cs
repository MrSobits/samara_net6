namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    public class GlossaryItemInterceptor : EmptyDomainInterceptor<MultipurposeGlossaryItem>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MultipurposeGlossaryItem> service, MultipurposeGlossaryItem entity)
        {
            if (service.GetAll().Any(x => x.Key == entity.Key))
            {
                return new SaveDataResult
                {
                    Success = false,
                    Message = "Нельзя добавлять элементы в справочник с одинаковым ключом!"
                };
            }

            return base.BeforeCreateAction(service, entity);
        }
    }
}