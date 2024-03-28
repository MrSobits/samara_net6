namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Абстрактный описатель источника для УО
    /// </summary>
    public abstract class BaseManagingOrganizationDataFiller : IConstructorDataFiller
    {
        /// <summary>
        /// Запрос УО, который будет участвовать в текущем расчете
        /// </summary>
        protected IQueryable<ManagingOrganization> ManorgQuery;

        /// <summary>
        /// Текущий период
        /// </summary>
        protected EfficiencyRatingPeriod period;

        /// <inheritdoc />
        public DataMetaObjectType Type => DataMetaObjectType.EfficientcyRating;

        /// <inheritdoc />
        public virtual void PrepareCache(BaseParams baseParams)
        {
            this.ManorgQuery = baseParams.Params.GetAs<IQueryable<ManagingOrganization>>("manorgQuery");
            this.period = baseParams.Params.GetAs<EfficiencyRatingPeriod>("period");
        }

        /// <inheritdoc />
        public void SetValue(IDataValue value)
        {
            this.SetValue((ManagingOrganizationDataValue)value);
        }

        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected abstract void SetValue(ManagingOrganizationDataValue value);
    }
}