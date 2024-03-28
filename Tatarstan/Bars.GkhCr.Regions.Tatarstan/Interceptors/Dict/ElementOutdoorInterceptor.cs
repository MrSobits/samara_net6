namespace Bars.GkhCr.Regions.Tatarstan.Interceptors.Dict
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict;
    using System.Linq;

    public class ElementOutdoorInterceptor : EmptyDomainInterceptor<ElementOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<ElementOutdoor> service, ElementOutdoor entity)
        {
            var moduleNames = string.Empty;

            var worksElementlDomain = this.Container.ResolveDomain<WorksElementOutdoor>();
            var roOutdoorElementlOutdoorDomain = this.Container.ResolveDomain<RealityObjectOutdoorElementOutdoor>();
            using (this.Container.Using(worksElementlDomain, roOutdoorElementlOutdoorDomain))
            {
                if (worksElementlDomain.GetAll().Any(x => x.ElementOutdoor == entity))
                {
                    moduleNames += "Справочник \"Работы двора\"<br>";
                }

                if (roOutdoorElementlOutdoorDomain.GetAll().Any(x => x.Element == entity))
                {
                    moduleNames += "Элементы двора до благоустройства < br>";
                }
            }

            return moduleNames != string.Empty
                ? Failure($"Существуют зависимые записи в:<br>{moduleNames}")
                : base.BeforeDeleteAction(service, entity);
        }
    }
}