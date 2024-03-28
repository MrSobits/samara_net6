namespace Bars.Gkh.Interceptors.EfficiencyRating
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Интерцептор для <see cref="ManagingOrganizationEfficiencyRating"/>
    /// </summary>
    public class ManagingOrganizationEfficiencyRatingInterceptor : EmptyDomainInterceptor<ManagingOrganizationEfficiencyRating>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<ManagingOrganizationEfficiencyRating> service, ManagingOrganizationEfficiencyRating entity)
        {
            if (service.GetAll().Any(x => x.Id != entity.Id && x.ManagingOrganization.Id == entity.ManagingOrganization.Id && x.Period.Id == entity.Period.Id))
            {
                return this.Failure("Текущая управляющая организация уже производит расчет рейтинга эффективности в текущем периоде");
            }

            return base.BeforeCreateAction(service, entity);
        }
    }
}