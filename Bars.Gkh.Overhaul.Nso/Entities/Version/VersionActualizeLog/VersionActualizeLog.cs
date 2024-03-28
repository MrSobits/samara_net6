using System;

namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Nso.Enum;

    public class VersionActualizeLog : BaseEntity
    {
        /// <summary>
        /// Версия 
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// Наименвоание пользователя
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Дата выполнения действия
        /// </summary>
        public virtual DateTime DateAction { get; set; }

        /// <summary>
        /// Тип актуализации
        /// </summary>
        public virtual VersionActualizeType ActualizeType { get; set; }

        /// <summary>
        /// количество выполненных действий
        /// </summary>
        public virtual int CountActions { get; set; }
        
        public virtual FileInfo LogFile { get; set; }
    }
}
