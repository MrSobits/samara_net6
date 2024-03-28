namespace Bars.Gkh.DomainService.Impl
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;


    /// <summary>
    /// Заполняет поля в карточке дома
    /// </summary>
    public class RealityObjectFieldsService : IRealityObjectFieldsService
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получает значение поля для заполнения
        /// </summary>
        /// <param name="baseParams">Id - id дома, FieldName - поле для заполнения</param>
        public IDataResult GetFieldValue(BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAsId("Id");
            var fieldName = baseParams.Params.GetAs<string>("FieldName");

            var result = new BaseDataResult();

            switch (fieldName)
            {
                case "AreaOwned":
                    result.Data = this.GetAreaSum(objectId, x => x.OwnershipType == RoomOwnershipType.Private);
                    break;
                case "AreaMunicipalOwned":
                    result.Data = this.GetAreaSum(objectId, x => x.OwnershipType == RoomOwnershipType.Municipal);
                    break;
                case "AreaGovernmentOwned":
                    result.Data = this.GetAreaSum(objectId, x => x.OwnershipType == RoomOwnershipType.Goverenment);
                    break;
                case "AreaLivingNotLivingMkd":
                    result.Data = this.GetAreaSum(objectId, x => x.Type == RoomType.Living || x.Type == RoomType.NonLiving);
                    break;
                case "AreaLiving":
                    result.Data = this.GetAreaSum(objectId, x => x.Type == RoomType.Living);
                    break;
                case "AreaNotLivingPremises":
                    result.Data = this.GetAreaSum(objectId, x => x.Type == RoomType.NonLiving);
                    break;
                default:
                    result.Success = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Получает суммарную площадь помещений
        /// </summary>
        private decimal GetAreaSum(long objectId, Expression<Func<Room, bool>> predicate)
        {
            var result = 0m;

            this.Container.UsingForResolved<IDomainService<Room>>((container, domainService) =>
            {
                result = domainService.GetAll()
                    .Where(x => x.RealityObject.Id == objectId)
                    .Where(predicate)
                    .Select(x => x.Area)
                    .Sum();
            });

            return result;
        }
    }
}