namespace Bars.Gkh.Ris.Dictionaries.HouseManagement
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Связь вида коммунальной услуги, тарифицируемого ресурса и единиц измерения ставки тарифа"
    /// </summary>
    public class UnitMeasureDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public UnitMeasureDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Связь вида коммунальной услуги, тарифицируемого ресурса и единиц измерения ставки тарифа";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            var domain = this.Container.ResolveDomain<UnitMeasure>();

            try
            {
                return domain.GetAll().Select(x => new ExternalEntityProxy
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
