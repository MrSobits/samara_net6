using Bars.B4.DataAccess;
using System;

namespace Sobits.GisGkh.Entities
{
    public class OkeiRefField : BaseEntity
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
        /// Код единицы измерения по справочнику ОКЕИ
        /// </summary>
        public virtual string Code { get; set; }
    }
}