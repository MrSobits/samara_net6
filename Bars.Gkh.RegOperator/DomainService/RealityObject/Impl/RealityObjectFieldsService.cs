namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

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
                case "AreaLivingOwned":
                    result.Data = this.GetAreaLivingOwnedSum(objectId);
                    break;
                case "PercentDebt":
                    result.Data = this.GetPercentDebt(objectId);
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
                    .SafeSum();
            });

            return result;
        }

        /// <summary>
        /// Получает суммарную площадь помещений
        /// В т.ч. жилых, находящихся в собственности граждан (кв.м.)
        /// </summary>
        private decimal GetAreaLivingOwnedSum(long objectId)
        {
            var result = 0m;

            this.Container.UsingForResolved<IDomainService<Room>>((container, domainService) =>
            {
                var query = domainService.GetAll().Where(x => x.RealityObject.Id == objectId);

                this.Container.UsingForResolved<IDomainService<BasePersonalAccount>>((c, account) =>
                {
                    var indivualAccountsQuery = account.GetAll()
                        .Where(x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual);

                    result = query
                        .Where(x => x.Type == RoomType.Living)
                        .Where(x => indivualAccountsQuery.Any(z => z.Room == x))
                        .Select(x => x.Area)
                        .SafeSum();
                });
            });

            return result;
        }

        /// <summary>
        /// Получает значение собираемости платежей
        /// </summary>
        private decimal GetPercentDebt(long objectId)
        {
            var result = 0m;

            this.Container.UsingForResolved<IDomainService<RealityObjectChargeAccount>>((container, domainService) =>
            {
                    var entity = domainService.GetAll().FirstOrDefault(x => x.RealityObject.Id == objectId);

                if (entity != null)
                {
                        var chargedTotal = entity.Operations.SafeSum(y => y.ChargedTotal);
                        result = chargedTotal == 0 ? 0 : entity.PaidTotal / chargedTotal * 100;
                }
            });

            return result;
        }
    }
}