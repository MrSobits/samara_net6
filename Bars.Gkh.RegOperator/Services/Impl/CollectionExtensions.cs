namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.RegOperator.Entities;

    public static class CollectionExtensions
    {
        public static IEnumerable<BasePersonalAccount> FilterByFio(
            this IQueryable<BasePersonalAccount> collection,
            string lastName,
            string firstName,
            string middleName)
        {
            return collection.Where(x => x.AccountOwner.Name
                        .ToLower()
                        .Replace(" ", "")
                    == ($"{lastName}{firstName}{middleName}")
                    .ToLower()
                    .Replace(" ", ""))
                .ToList();
        }
    }
}