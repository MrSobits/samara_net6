namespace Bars.GkhGji.Regions.Saha.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;

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
            return
                new ListDataResult(
                    new List<object>
                        {
                            new
                                {
                                    Id = (int)TypeJurPerson.ManagingOrganization,
                                    Display = TypeJurPerson.ManagingOrganization.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.SupplyResourceOrg,
                                    Display = TypeJurPerson.SupplyResourceOrg.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.LocalGovernment,
                                    Display = TypeJurPerson.LocalGovernment.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.PoliticAuthority,
                                    Display = TypeJurPerson.PoliticAuthority.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.Builder,
                                    Display = TypeJurPerson.Builder.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.ServOrg,
                                    Display = TypeJurPerson.ServOrg.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.RenterOrg,
                                    Display = TypeJurPerson.RenterOrg.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.RegOp,
                                    Display = TypeJurPerson.RegOp.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.ServiceCompany,
                                    Display = TypeJurPerson.ServiceCompany.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.Tsj,
                                    Display = TypeJurPerson.Tsj.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.Owner,
                                    Display = TypeJurPerson.Owner.GetEnumMeta().Display
                                },
                            new
                                {
                                    Id = (int)TypeJurPerson.ResourceCompany,
                                    Display = TypeJurPerson.ResourceCompany.GetEnumMeta().Display
                                }
                        },
                    12);
        }
    }
}