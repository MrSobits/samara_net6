namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="IndProxy"/>
    /// </summary>
    public class IndSelectorService : BaseProxySelectorService<IndProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, IndProxy> GetCache()
        {
            return new Dictionary<long, IndProxy>();
        }

        /// <inheritdoc />
        protected override ICollection<IndProxy> GetAdditionalCache()
        {
            var individualAccountOwnerRepository = this.Container.ResolveRepository<IndividualAccountOwner>();

            using (this.Container.Using(individualAccountOwnerRepository))
            {
                return this.GetProxies(individualAccountOwnerRepository.GetAll()
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        private ICollection<IndProxy> GetProxies(IQueryable<IndividualAccountOwner> query)
        {
            return query
                .Select(x => new
                {
                    x.Id,
                    x.Surname,
                    x.FirstName,
                    x.SecondName,
                    x.BirthDate,
                    x.BirthPlace,
                    x.Gender,
                    x.IdentityType,
                    x.IdentitySerial,
                    x.IdentityNumber,
                    x.DateDocumentIssuance,

                })
                .AsEnumerable()
                .Select(x => new IndProxy
                {
                    Id = x.Id,
                    Surname = x.Surname,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    BirthDate = x.BirthDate,
                    Gender = x.Gender,
                    IdentityType = this.GetIdentityType(x.IdentityType),
                    SnilsNumber = x.IdentityType == IdentityType.InsuranceNumber ? x.IdentityNumber : null,
                    IdentitySerial = x.IdentityType != IdentityType.InsuranceNumber ? x.IdentitySerial : null,
                    IdentityNumber = x.IdentityType != IdentityType.InsuranceNumber ? x.IdentityNumber : null,
                    DateDocumentIssuance = x.DateDocumentIssuance.IsValid() ? x.DateDocumentIssuance : null,
                    BirthPlace = x.BirthPlace
                })
                .ToList();
        }

        private string GetIdentityType(IdentityType type)
        {
            switch (type)
            {
                case IdentityType.Passport:
                    return "1";
                case IdentityType.BirthCertificate:
                    return "11";
            }

            return string.Empty;
        }
    }
}