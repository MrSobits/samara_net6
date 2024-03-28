using Bars.B4.DataAccess;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Поле - ссылка на справочник
    /// </summary>
    public class NsiField : BaseEntity
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
        /// Реестровый номер справочника
        /// </summary>
        public virtual string NsiRegNumber { get; set; }

        /// <summary>
        /// Ссылка на справочник
        /// </summary>
        public virtual NsiList NsiList { get; set; }
    }
}