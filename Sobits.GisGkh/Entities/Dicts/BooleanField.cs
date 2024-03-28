using Bars.B4.DataAccess;
using System;

namespace Sobits.GisGkh.Entities
{
    public class BooleanField : BaseEntity
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
        /// Значение
        /// </summary>
        public virtual bool? Value { get; set; }
    }
}