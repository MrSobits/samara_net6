namespace Bars.Gkh.Ris.Dictionaries
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Вид проверки"
    /// </summary>
    public class KindCheckDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public KindCheckDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Вид проверки";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            var domain = this.Container.ResolveDomain<KindCheckGji>();

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
