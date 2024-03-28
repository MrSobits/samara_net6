namespace Bars.Gkh.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Легковесная сущность для хранения изменения сущности
    /// </summary>
    public class ExternalExchangeTestingFiles : BaseEntity
    {
        /// <summary>
        /// Идентификатор сущности 
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Класс сущности
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// Описание сущности
        /// </summary>
        public virtual string ClassDescription { get; set; }               

        /// <summary>
        /// Дата поступления сведений об изменении значения
        /// </summary>
        public virtual DateTime? DateApplied { get; set; }      

        /// <summary>
        /// Документ - основание
        /// </summary>
        public virtual FileInfo Document { get; set; }       

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual string User { get; set; }
        
    }
}