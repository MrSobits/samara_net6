namespace Bars.Gkh.Overhaul.Hmao.Entities.Version
{
    using B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Enum;
    using System;


    /// <summary>
    /// Лог актуализации версии программы
    /// </summary>
    public class VersionActualizeLog : BaseGkhEntity
    {
        /// <summary>
        /// Версия 
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// МО 
        /// </summary>
        public virtual Municipality Municipality { get; set; }

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
        /// Входные параметры
        /// </summary>
        public virtual string InputParams { get; set; }

        /// <summary>
        /// количество выполненных действий
        /// </summary>
        public virtual int CountActions { get; set; }

        /// <summary>
        /// Имя краткосрочной программы, в рамках которой запускалось действие
        /// </summary>
        public virtual string ProgramCrName{ get; set; }
        
        public virtual FileInfo LogFile { get; set; }
    }
}
