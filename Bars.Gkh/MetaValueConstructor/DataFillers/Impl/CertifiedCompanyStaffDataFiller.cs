namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Сертифицированный персонал компании
    /// </summary>
    public class CertifiedCompanyStaffDataFiller : BaseManagingOrganizationDataFiller
    {
        private IDictionary<long, int> certifiedCompanyStaff;

        /// <summary>
        /// Домен-сервис <see cref="PersonPlaceWork"/>
        /// </summary>
        public IDomainService<PersonPlaceWork> PersonPlaceWorkDomainService { get; set; }

        /// <inheritdoc />
        public override void PrepareCache(BaseParams baseParams)
        {
            base.PrepareCache(baseParams);

            this.certifiedCompanyStaff =
                this.PersonPlaceWorkDomainService.GetAll()
                    .Where(x => this.ManorgQuery.Any(y => y.Contragent.Id == x.Contragent.Id))
                    .Where(this.period.CreateContainsExpression<PersonPlaceWork>(x => x.StartDate ?? DateTime.MinValue, x => x.EndDate ?? DateTime.MaxValue))
                    .GroupBy(x => x.Contragent.Id)
                    .ToDictionary(x => x.Key, y => y.Count());
        }

        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = this.certifiedCompanyStaff.Get(value.EfManagingOrganization.ManagingOrganization.Contragent.Id);
        }
    }
}