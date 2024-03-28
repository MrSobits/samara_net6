namespace Bars.Gkh.Interceptors.RealityObjectOutdoor
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealityObj;

    using NHibernate.Util;

    public class RealityObjectOutdoorInterceptor : EmptyDomainInterceptor<RealityObjectOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectOutdoor> service, RealityObjectOutdoor entity)
        {
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            using (this.Container.Using(realityObjectDomain))
            {
                realityObjectDomain.GetAll()
                    .Where(x => x.Outdoor.Id == entity.Id)
                    .ForEach(x =>
                    {
                        x.Outdoor = null;
                        realityObjectDomain.Update(x);
                    });
            }

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectOutdoor> service, RealityObjectOutdoor entity)
        {
            return !RealityObjectOutdoorInterceptor.IsOutdoorCodeAvailable(service, entity)
                ? base.BeforeCreateAction(service, entity)
                : this.Failure("Данный код двора уже заведен в системе");
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<RealityObjectOutdoor> service, RealityObjectOutdoor entity)
        {
            return !RealityObjectOutdoorInterceptor.IsOutdoorCodeAvailable(service, entity)
                ? base.BeforeUpdateAction(service, entity)
                : this.Failure("Данный код двора уже заведен в системе");
        }

        /// <summary>
        /// Проверяет уникальность кода двора.
        /// </summary>
        private static bool IsOutdoorCodeAvailable(IDomainService<RealityObjectOutdoor> service, RealityObjectOutdoor entity) =>
            service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id);
    }
}
