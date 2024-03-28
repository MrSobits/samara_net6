namespace Bars.GkhDi.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерсептор для <see cref="InfoAboutUseCommonFacilities"/>
    /// </summary>
    public class InfoAboutUseCommonFacilitiesInterceptor : EmptyDomainInterceptor<InfoAboutUseCommonFacilities>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<InfoAboutUseCommonFacilities> service, InfoAboutUseCommonFacilities entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<InfoAboutUseCommonFacilities> service, InfoAboutUseCommonFacilities entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<InfoAboutUseCommonFacilities> service, InfoAboutUseCommonFacilities entity)
        {
            var dateStart = entity.DateStart ?? DateTime.MinValue;

            //получить все сведения об использовании мест общего пользования
            var existsPlacementInfos = service.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == entity.DisclosureInfoRealityObj.Id && x.Id != entity.Id)
                .Where(x => x.DateEnd == null || x.DateEnd >= dateStart)
                .ToList();

            //площадь из паспорта
            var totalArea = entity.DisclosureInfoRealityObj.RealityObject.AreaNotLivingFunctional;

            var allDates = existsPlacementInfos
                .Where(x => x.DateStart != null)
                .Select(x => x.DateStart)
                .Distinct()
                .ToList();

            allDates.Add(dateStart);

            var entityArea = entity.AreaOfCommonFacilities ?? 0;

            foreach (var date in allDates)
            {
                var existsArea = existsPlacementInfos.Where(x => date >= x.DateStart && date < x.DateEnd).SafeSum(x => x.AreaOfCommonFacilities ?? 0);

                if (entityArea + existsArea > totalArea)
                {
                    return this.Failure("Сумма нежилых помещений не может быть больше значения площади в паспорте дома");
                }
            }

            return this.Success();
        }
    }
}