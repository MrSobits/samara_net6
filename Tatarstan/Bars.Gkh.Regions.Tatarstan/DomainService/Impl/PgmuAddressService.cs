namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Реализация сервиса адресов ПГМУ
    /// </summary>
    public class PgmuAddressService : IPgmuAddressService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IEnumerable<object> GetPgmuAddressObjectValue(string propertyName, string value, Dictionary<string, string> parentValuesDict)
        {
            var pgmuAddressDomainService = Container.ResolveDomain<PgmuAddress>();
            var propertyMatchingDict = new Dictionary<string, string>
            {
                {propertyName, "Value"}
            };

            using (Container.Using(pgmuAddressDomainService))
            {
                var query = pgmuAddressDomainService
                    .GetAll()
                    .Where(ExpressionExtension.GetContainsIgnoreCaseMethodExpression<PgmuAddress>(propertyName, value));

                parentValuesDict?.ForEach(x => query = query.Where(ExpressionExtension.GetEqualityExpression<PgmuAddress>(x.Key, x.Value)));
      
                return query
                    .Select(ExpressionExtension.GetSelectNewObjectExpression<PgmuAddress, ResultObject>(propertyMatchingDict))
                    .Distinct()
                    .ToList();
            }
        }

        /// <inheritdoc />
        public long GetPgmuAddressId(Dictionary<string, string> addressObjectDict)
        {
            var pgmuAddressDomainService = Container.ResolveDomain<PgmuAddress>();

            using (Container.Using(pgmuAddressDomainService))
            {
                var query = pgmuAddressDomainService
                    .GetAll();
                
                addressObjectDict?.ForEach(x => query = query.Where(ExpressionExtension.GetEqualityExpression<PgmuAddress>(x.Key, x.Value)));

                return query
                    .Select(x => x.Id)
                    .FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public string CombinePgmuAddress(PgmuAddress address)
        {
            if (address == null)
            {
                return string.Empty;
            }
            
            var addressConstituents = typeof(PgmuAddress)
                .GetProperties()
                .Where(x => x.GetCustomAttribute<AddressObjectLevelAttribute>() != null)
                .Select(x => new
                {
                    x.GetCustomAttribute<AddressObjectLevelAttribute>().Level,
                    x.GetCustomAttribute<AddressObjectPrefixAttribute>()?.Prefix,
                    Value = (string) x.GetValue(address)
                })
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .OrderBy(x => x.Level)
                .Select(x => x.Prefix != null && !x.Value.Contains(x.Prefix) ? $"{x.Prefix} {x.Value}" : x.Value);

            return string.Join(", ", addressConstituents);
        }

        /// <summary>
        /// Вспомогательный класс для конвертации в JSON
        /// </summary>
        private class ResultObject
        {
            public string Value { get; set; }
        }
    }
}