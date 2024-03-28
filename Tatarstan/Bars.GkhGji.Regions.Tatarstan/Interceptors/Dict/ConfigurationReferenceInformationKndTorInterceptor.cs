using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{

    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ConfigurationReferenceInformationKndTorInterceptor : EmptyDomainInterceptor<ConfigurationReferenceInformationKndTor>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<ConfigurationReferenceInformationKndTor> service, ConfigurationReferenceInformationKndTor entity) =>
            this.CheckValue(service, entity);

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<ConfigurationReferenceInformationKndTor> service, ConfigurationReferenceInformationKndTor entity) =>
            this.CheckValue(service, entity);

        private IDataResult CheckValue(IDomainService<ConfigurationReferenceInformationKndTor> service, ConfigurationReferenceInformationKndTor entity) =>
            service.GetAll()
                .Any(x => x.Value.ToLower() == entity.Value.ToLower()
                    && x.Type == entity.Type
                    && x.Id != entity.Id)
                ? this.Failure("В данном типе справочника такое значение уже есть")
                : this.Success();
    }
}
