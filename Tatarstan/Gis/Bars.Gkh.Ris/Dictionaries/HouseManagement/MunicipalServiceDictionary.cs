namespace Bars.Gkh.Ris.Dictionaries.HouseManagement
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Вид коммунальной услуги"
    /// </summary>
    public class MunicipalServiceDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public MunicipalServiceDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Вид коммунальной услуги";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            var domain = this.Container.ResolveDomain<ServiceDictionary>();

            try
            {
                return domain.GetAll()
                    .Where(x => x.TypeService == TypeServiceGis.Communal)
                    .Select(x => new ExternalEntityProxy
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}
