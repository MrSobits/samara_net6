namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    public sealed class JurPersonService : IJurPersonService
    {
        /// <summary>
        ///     Возвращает типы контрагентов
        /// </summary>
        /// <param name="baseParams">
        ///     Параметры.
        /// </param>
        /// <returns>Типы контрагентов</returns>
        public IDataResult GetJurPersonTypes(BaseParams baseParams)
        {
            return new List<TypeJurPerson>
                {
                    TypeJurPerson.ManagingOrganization,
                    TypeJurPerson.SupplyResourceOrg,
                    TypeJurPerson.LocalGovernment,
                    TypeJurPerson.PoliticAuthority,
                    TypeJurPerson.Builder,
                    TypeJurPerson.ServOrg,
                    TypeJurPerson.RenterOrg,
                    TypeJurPerson.RegOp,
                    TypeJurPerson.Tsj,
                    TypeJurPerson.Owner,
                    TypeJurPerson.ResourceCompany,
                    TypeJurPerson.PublicServiceOrg
                }.Select(x => new
                {
                    Id = (int) x,
                    Display = x.GetDisplayName()
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}