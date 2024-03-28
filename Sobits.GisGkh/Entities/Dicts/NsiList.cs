using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using GisGkhLibrary.NsiCommonAsync;
using System;

namespace Sobits.GisGkh.Entities
{
    public class NsiList : BaseEntity
    {
        /// <summary>
        /// Группа справочников
        /// </summary>
        public virtual ListGroup ListGroup { get; set; }
    
        /// <summary>
        /// Наименование справочника в ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhName { get; set; }

        /// <summary>
        /// Код справочника в ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhCode { get; set; }
        
        /// <summary>
        /// Наименование справочника в системе
        /// </summary>
        public virtual string EntityName { get; set; }

        /// <summary>
        /// Дата обновления справочника из ГИС ЖКХ
        /// </summary>
        public virtual DateTime? RefreshDate { get; set; }

        /// <summary>
        /// Актуальность справочника ГИС ЖКХ
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Дата сопоставления справочника
        /// </summary>
        public virtual DateTime? MatchDate { get; set; }

        /// <summary>
        /// Контрагент (владелец справочника)
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}