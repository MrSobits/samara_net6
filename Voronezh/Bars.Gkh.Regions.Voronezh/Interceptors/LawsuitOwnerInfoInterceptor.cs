namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Entities.Owner;

    public class LawsuitOwnerInfoInterceptor : LawsuitOwnerInfoInterceptor<LawsuitOwnerInfo>
    {
    }

    public class LawsuitOwnerInfoInterceptor<T> : EmptyDomainInterceptor<T>
        where T : LawsuitOwnerInfo
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            return this.CheckBaseRules(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            return this.CheckBaseRules(service, entity);
        }

        private IDataResult CheckBaseRules(IDomainService<T> service, T entity)
        {
            if (entity.AreaShareNumerator == 0)
            {
                return this.Success();
            }
            if (entity.AreaShare < 0 || 1.01m < entity.AreaShare)
            {
                return this.Failure("Новая доля не может быть больше 1 и меньше 0");
            }

            var isNotUnique = service.GetAll()
                .WhereIf(entity.Id > 0, x => x.Id != entity.Id)
                .Where(x => x.Lawsuit.Id == entity.Lawsuit.Id)
                .Where(x => x.PersonalAccount.Id == entity.PersonalAccount.Id)
                .Any(x => x.Name == entity.Name);

            if (isNotUnique)
            {
                return this.Failure("Собственник с таким ЛС уже добавлен");
            }

            var areaShareSum = service.GetAll()
                    .WhereIf(entity.Id > 0, x => x.Id != entity.Id)
                    .Where(x => x.Lawsuit.Id == entity.Lawsuit.Id)
                    .Where(x => !x.SharedOwnership)
                    .Where(x => x.PersonalAccount.Id == entity.PersonalAccount.Id)
                    .Select(x => x.AreaShareNumerator / (decimal)x.AreaShareDenominator)
                    .SafeSum();

            var areaSharedOwnership = service.GetAll()
                    .WhereIf(entity.Id > 0, x => x.Id != entity.Id)
                    .Where(x => x.Lawsuit.Id == entity.Lawsuit.Id)
                    .Where(x => x.SharedOwnership)
                    .Where(x => x.PersonalAccount.Id == entity.PersonalAccount.Id)
                    .Select(x => x.AreaShareNumerator / (decimal)x.AreaShareDenominator)
                    .ToList()
                    .Distinct();

            var areaSharedOwnershipSum = areaSharedOwnership.SafeSum();

            if (!entity.SharedOwnership || (entity.SharedOwnership && !areaSharedOwnership.Contains(entity.AreaShare)))
            {
                if (areaShareSum + areaSharedOwnershipSum + entity.AreaShare > 1.01m)
                {
                    return this.Failure("Суммарная доля собственности по помещению превышает 1");
                }
            }

            return this.Success();
        }
    }
}