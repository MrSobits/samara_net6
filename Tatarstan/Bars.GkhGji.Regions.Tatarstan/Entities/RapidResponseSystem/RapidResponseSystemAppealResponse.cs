namespace Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Ответ на обращение в СОПР
    /// </summary>
    public class RapidResponseSystemAppealResponse : BaseEntity
    {
        /// <summary>
        /// Детали обращения в СОПР
        /// </summary>
        public virtual RapidResponseSystemAppealDetails RapidResponseSystemAppealDetails { get; set; }

        /// <summary>
        /// Дата ответа
        /// </summary>
        public virtual DateTime? ResponseDate { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public virtual string Theme { get; set; }
        
        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Response { get; set; }
        
        /// <summary>
        /// Проведенные работы
        /// </summary>
        public virtual string CarriedOutWork { get; set; }
        
        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo ResponseFile { get; set; }
    }
}