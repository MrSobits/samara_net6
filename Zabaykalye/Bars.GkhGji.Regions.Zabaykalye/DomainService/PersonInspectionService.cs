namespace Bars.GkhGji.Regions.Zabaykalye.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;

    public sealed class PersonInspectionService : IPersonInspectionService
    {
        /// <summary>
        ///     Возвращает типы объектов проверки
        /// </summary>
        /// <param name="baseParams">
        ///     Параметры.
        /// </param>
        /// <returns>Типы объектов проверки</returns>
        public IDataResult GetPersonInspectionTypes(BaseParams baseParams)
        {
            return new ListDataResult(new List<object>
            {
                new
                {
                    Id = (int)PersonInspection.Official,
                    Display = PersonInspection.Official.GetEnumMeta().Display
                },
                new
                {
                    Id = (int)PersonInspection.Organization,
                    Display = PersonInspection.Organization.GetEnumMeta().Display
                },
                new
                {
                    Id = (int)PersonInspection.PhysPerson,
                    Display = PersonInspection.PhysPerson.GetEnumMeta().Display
                },
                new
                {
                    Id = (int)PersonInspection.RealityObject,
                    Display = PersonInspection.RealityObject.GetEnumMeta().Display
                }
            },
            
            4);
        }
    }
}