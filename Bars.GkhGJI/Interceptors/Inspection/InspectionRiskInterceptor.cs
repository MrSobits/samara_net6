namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <inheritdoc />
    public class InspectionRiskInterceptor : EmptyDomainInterceptor<InspectionRisk>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<InspectionRisk> service, InspectionRisk entity)
        {
            var result = this.Validate(service, entity);
            if (!result.Success)
            {
                return result;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<InspectionRisk> service, InspectionRisk entity)
        {
            var result = this.Validate(service, entity);
            if (!result.Success)
            {
                return result;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Валидация полей сущности
        /// </summary>
        private IDataResult Validate(IDomainService<InspectionRisk> domain, InspectionRisk entity)
        {
            var hasEmptyProperty = entity.Inspection == null || entity.RiskCategory == null || !entity.StartDate.IsValid();
            if (hasEmptyProperty)
            {
                return this.Failure("Необходимо заполнить все поля");
            }

            if (entity.StartDate > entity.EndDate)
            {
                return this.Failure("Для каждой записи дата окончания должна быть больше или равна дате начала");
            }

            var actualRisk = domain.GetAll()
                .Where(x => x.Inspection == entity.Inspection)
                .Where(x => x.Id != entity.Id)
                .FirstOrDefault(x => !x.EndDate.HasValue);

            if (actualRisk != null && actualRisk.StartDate <= entity.StartDate)
            {
                return this.Failure("Добавляемые периоды не должны пересекаться с датой начала присвоения актуальной категории");
            }
            
            return this.Success();
        }
    }
}