namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataModels;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Интерфейс описания источников данных
    /// </summary>
    public interface IConstructorDataFillerMap
    {
        /// <summary>
        /// Метод возвращает список зарегистрированных описаний
        /// </summary>
        /// <returns>Описания источников данных</returns>
        IEnumerable<DataFillerInfo> GetInfo();

        /// <summary>
        /// Зарегистрировать все сущности
        /// </summary>
        void Map();
    }

    /// <summary>
    /// Описание источников данных
    /// </summary>
    public class DataFillerInfo : IHasParent<DataFillerInfo>, IHasNameCode, IHasId
    {
        /// <summary>Описание разрешения</summary>
        public string Name { get; set; }

        /// <summary>Тип сущности, к которой имеет отношение разрешение</summary>
        public Type EntityType { get; set; }

        /// <summary>Является ли разрешение пространством имен</summary>
        public bool IsNamespace { get; set; }

        /// <summary>Полный идентификатор разрешения</summary>
        public string Code { get; set; }

        /// <summary> Родительское пространство имён </summary>
        public DataFillerInfo Parent { get; set; }

        /// <summary>
        /// Тип конструктора (для регистрации реализаций)
        /// </summary>
        public DataMetaObjectType ConstructorType { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public object Id => this.Code;
    }
}