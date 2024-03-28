using Bars.B4.DataAccess;
using System;

namespace Sobits.GisGkh.Entities
{
    public class EnumField : BaseEntity
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
        /// Строка со значениями {GUID, value}
        /// </summary>
        public virtual string Position { get; set; }
    }
}