using Bars.B4.DataAccess;
using System;

namespace Sobits.GisGkh.Entities
{
    public class FiasAddressRefField : BaseEntity
    {
        /// <summary>
        /// Пункт справочника
        /// </summary>
        public virtual NsiItem NsiItem { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Строка с GUID справочника ФИАС
        /// </summary>
        public virtual string FiasGUID { get; set; }
    }
}