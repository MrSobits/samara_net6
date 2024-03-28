namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Интерцептор для <see cref="EfficiencyRatingPeriod"/>
    /// </summary>
    public class EfficiencyRatingPeriodInterceptor : EmptyDomainInterceptor<EfficiencyRatingPeriod>
    {
        /// <summary>
        /// Домен-сервис <see cref="MetaConstructorGroup"/>
        /// </summary>
        public IDomainService<MetaConstructorGroup> GroupDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<EfficiencyRatingPeriod> service, EfficiencyRatingPeriod entity)
        {
            var validationResult = this.ValidateEntity(service, entity);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            entity.Group = new MetaConstructorGroup { ConstructorType = DataMetaObjectType.EfficientcyRating };
            this.GroupDomain.Save(entity.Group);

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<EfficiencyRatingPeriod> service, EfficiencyRatingPeriod entity)
        {
            return this.ValidateEntity(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<EfficiencyRatingPeriod> service, EfficiencyRatingPeriod entity)
        {
            if (entity.DateStart > entity.DateEnd)
            {
                return this.Failure("Дата начала не может быть больше даты окончания периода");
            }


            var existsEntityInPeriod = service.GetAll().Where(x => x.Id != entity.Id)
                    .Any(x =>
                            (entity.DateStart >= x.DateStart && entity.DateStart <= x.DateEnd)
                            || (entity.DateEnd >= x.DateStart && entity.DateEnd <= x.DateEnd));

            if (existsEntityInPeriod)
            {
                return this.Failure("Период рейтинга эффективности пересекается с датой действия другого периода");
            }

            return this.Success();
        }
    }
}