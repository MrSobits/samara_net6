namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.Reforma;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Enum;

    /// <summary>
    /// Сервис получение "Протоколы собственников помещений МКД" 
    /// </summary>
    public class PropertyOwnerProtocolsProvider : IPropertyOwnerProtocolsProvider
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получение "Протоколы собственников помещений МКД" 
        /// </summary>
        /// <param name="realtyObjIdsQuery">Id жилога дома</param>
        /// <returns>Словарь из прокси объектов</returns>
        public Dictionary<long, PropertyOwnerProtocolsData> GetData(IQueryable<long> realtyObjIdsQuery)
        {
            var domain = this.Container.ResolveDomain<PropertyOwnerProtocols>();
            try
            {
                var data = domain.GetAll()
                    .Where(x => realtyObjIdsQuery.Contains(x.RealityObject.Id))
                    .Where(x => x.TypeProtocol == PropertyOwnerProtocolType.SelectManOrg)
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(
                        y => y.Key,
                        y => y.Select(
                                x =>
                                    new PropertyOwnerProtocolsData
                                    {
                                        Date = x.DocumentDate,
                                        Number = x.DocumentNumber,
                                        File = x.DocumentFile
                                    }
                                ).First());

                return data;
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}