namespace Bars.GkhCr.Regions.Tatarstan.Interceptors.Dict
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    using System.Linq;

    public class WorkRoOutdoorInterceptor : EmptyDomainInterceptor<WorkRealityObjectOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<WorkRealityObjectOutdoor> service, WorkRealityObjectOutdoor entity)
        {
            var moduleNames = string.Empty;

            var worksElementlDomain = this.Container.ResolveDomain<WorksElementOutdoor>();
            var typeWorksRoOutdoorlDomain = this.Container.ResolveDomain<TypeWorkRealityObjectOutdoor>();
            using (this.Container.Using(worksElementlDomain, typeWorksRoOutdoorlDomain))
            {
                if (worksElementlDomain.GetAll().Any(x => x.Work == entity))
                {
                    moduleNames += "Справочник \"Элементы двора\"<br>";
                }

                if (typeWorksRoOutdoorlDomain.GetAll().Any(x => x.WorkRealityObjectOutdoor == entity))
                {
                    moduleNames += "Объект программы благоустройства дворов<br>";
                }
            }

            return moduleNames != string.Empty
                ? Failure($"Существуют зависимые записи в:<br>{moduleNames}")
                : base.BeforeDeleteAction(service, entity);
        }
    }
}
