using Bars.B4.DataAccess;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Поле - ссылка на пункт справочника
    /// </summary>
    public class NsiRefField : BaseEntity
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
        /// GUID пункта справочника, на который ссылается запись
        /// </summary>
        public virtual string RefGUID { get; set; }

        /// <summary>
        /// Пункт справочника, на который ссылается запись
        /// </summary>
        public virtual NsiItem NsiRefItem { get; set; }
    }
}