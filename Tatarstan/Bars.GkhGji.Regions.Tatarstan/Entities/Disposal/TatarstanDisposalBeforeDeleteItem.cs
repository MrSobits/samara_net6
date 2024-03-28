namespace Bars.GkhGji.Regions.Tatarstan.Entities.Disposal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Элемент, удаляющийся перед Распоряжением ГЖИ.
    /// </summary>
    public class TatarstanDisposalBeforeDeleteItem
    {
        /// <summary>
        /// Тип удаляемой сущности
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Список удаляемых идентификаторов
        /// </summary>
        public List<long> IdsList { get; }

        /// <summary>
        /// Место в цепочке
        /// </summary>
        public int Order { get; }

        /// <inheritdoc />
        public TatarstanDisposalBeforeDeleteItem(Type type, List<long> idsList, int order)
        {
            this.Type = type;
            this.IdsList = idsList;
            this.Order = order;
        }
    }
}
