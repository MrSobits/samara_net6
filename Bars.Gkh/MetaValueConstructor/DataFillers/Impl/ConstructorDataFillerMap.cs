namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.Gkh.MetaValueConstructor.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Базовый описатель источников данных
    /// </summary>
    public abstract class ConstructorDataFillerMap : IConstructorDataFillerMap
    {
        /// <summary>
        /// Тип конструктора
        /// </summary>
        public abstract DataMetaObjectType DataMetaObjectType { get; }

        private readonly IList<DataFillerInfo> infos = new List<DataFillerInfo>();

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод возвращает список зарегистрированных описаний
        /// </summary>
        /// <returns>Описания источников данных</returns>
        public IEnumerable<DataFillerInfo> GetInfo() => this.infos;

        /// <summary>
        /// Зарегистрировать все сущности
        /// </summary>
        public abstract void Map();

        /// <summary> Добавить имплементацию коллектора и зарегистрировать его в контейнере </summary>
        /// <param name="ns"> Полный идентификатор пути </param>
        /// <param name="description"> описание </param>
        /// <typeparam name="TCollector"> Тип источника данных коллектора </typeparam>
        public void CollectorImpl<TCollector>(string ns, string description) where TCollector : class, IConstructorDataFiller
        {
            this.Container.RegisterTransient<IConstructorDataFiller, TCollector>(ns);
            this.infos.Add(new DataFillerInfo
            {
                Name = description,
                EntityType = typeof(TCollector),
                Code = ns,
                Parent = this.GetParent(ns),
                ConstructorType = this.DataMetaObjectType
            });
        }

        /// <summary>
        /// Добавить пространство имён
        /// </summary>
        /// <param name="ns">
        /// Полный идентификатор пути
        /// </param>
        /// <param name="description">
        /// описание
        /// </param>
        public void Namespace(string ns, string description)
        {
            this.infos.Add(new DataFillerInfo
            {
                Name = description,
                IsNamespace = true,
                Code = ns,
                Parent = this.GetParent(ns)
            });
        }

        private DataFillerInfo GetParent(string ns)
        {
            if (!ns.Contains('.'))
            {
                return null;
            }

            var postfixIndex = ns.LastIndexOf('.');
            var code = ns.Substring(0, postfixIndex);
            return this.infos.FirstOrDefault(x => x.IsNamespace && x.Code == code);
        }
    }
}