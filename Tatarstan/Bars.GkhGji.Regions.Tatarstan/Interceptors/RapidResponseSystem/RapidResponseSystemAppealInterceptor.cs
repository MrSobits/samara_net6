namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.RapidResponseSystem
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// Interceptor для <see cref="RapidResponseSystemAppeal"/>
    /// </summary>
    public class RapidResponseSystemAppealInterceptor : EmptyDomainInterceptor<RapidResponseSystemAppeal>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<RapidResponseSystemAppeal> service, RapidResponseSystemAppeal entity)
        {
            var rapidResponseSystemAppealDetailsDomain = this.Container.ResolveDomain<RapidResponseSystemAppealDetails>();

            using (this.Container.Using(rapidResponseSystemAppealDetailsDomain))
            {
                if (rapidResponseSystemAppealDetailsDomain.GetAll().Any(x =>
                    x.RapidResponseSystemAppeal.Id == entity.Id))
                {
                    return this.Failure("При удалении произошла ошибка. " +
                        "По данной организации было сформировано обращение в системе оперативного реагирования.");
                }
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}