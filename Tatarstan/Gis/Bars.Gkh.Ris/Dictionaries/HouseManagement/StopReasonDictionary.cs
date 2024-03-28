namespace Bars.Gkh.Ris.Dictionaries.HouseManagement
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Entities.Dicts;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Причина расторжения договора"
    /// </summary>
    public class StopReasonDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public StopReasonDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Причина расторжения договора";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            var domain = this.Container.ResolveDomain<StopReason>();

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
