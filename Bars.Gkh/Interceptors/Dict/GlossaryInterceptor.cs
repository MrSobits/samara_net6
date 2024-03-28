namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    /// <summary>
    /// Перехватчик универсального справочника
    /// </summary>
    public class GlossaryInterceptor : EmptyDomainInterceptor<MultipurposeGlossary>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<MultipurposeGlossary> service, MultipurposeGlossary entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code))
            {
                return new SaveDataResult
                {
                    Success = false,
                    Message = "Нельзя добавлять справочник с тем же кодом!"
                };
            }

            return base.BeforeCreateAction(service, entity);
        }
    }
}