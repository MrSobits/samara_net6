using System;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Entities.Dict;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    /// <summary>
    /// Документ ДПКР
    /// </summary>
    public class DpkrDocument : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Вид документа
        /// </summary>
        public virtual BasisOverhaulDocKind DocumentKind { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Орган, принявший документ
        /// </summary>
        public virtual string DocumentDepartment { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}
