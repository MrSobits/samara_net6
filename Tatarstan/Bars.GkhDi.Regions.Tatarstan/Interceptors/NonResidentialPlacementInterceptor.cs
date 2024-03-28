namespace Bars.GkhDi.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерсептор для <see cref="NonResidentialPlacement"/>
    /// </summary>
    public class NonResidentialPlacementInterceptor : Bars.GkhDi.Interceptors.NonResidentialPlacementInterceptor
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<NonResidentialPlacement> service, NonResidentialPlacement entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<NonResidentialPlacement> service, NonResidentialPlacement entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<NonResidentialPlacement> service, NonResidentialPlacement entity)
        {
            var dateStart = entity.DateStart ?? DateTime.MinValue;

            //получить все сведения об использовании нежилых помещений
            var existsPlacementInfos = service.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == entity.DisclosureInfoRealityObj.Id && x.Id != entity.Id)
                .Where(x => x.DateEnd == null || x.DateEnd >= dateStart)
                .ToList();

            //площадь из паспорта
            var totalArea = entity.DisclosureInfoRealityObj.RealityObject.AreaNotLivingPremises;

            var allDates = existsPlacementInfos
                .Where(x => x.DateStart != null)
                .Select(x => x.DateStart)
                .Distinct()
                .ToList();

            allDates.Add(dateStart);

            foreach (var date in allDates)
            {
                var existsArea = existsPlacementInfos.Where(x => date >= x.DateStart && date < x.DateEnd).SafeSum(x => x.Area ?? 0);

                if (entity.Area + existsArea > totalArea)
                {
                    return this.Failure("Сумма нежилых помещений не может быть больше значения площади в паспорте дома");
                }
            }

            return this.Success();
        }
    }
}