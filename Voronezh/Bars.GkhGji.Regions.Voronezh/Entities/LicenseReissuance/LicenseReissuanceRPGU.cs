namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Предоставляемый документ заявки на лицензию
    /// </summary>
    public class LicenseReissuanceRPGU : BaseImportableEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual LicenseReissuance LicRequest { get; set; }

        /// <summary>
        /// Тип доп запроса
        /// </summary>
        public virtual RequestRPGUType RequestRPGUType { get; set; }

        /// <summary>
        /// Текст доп запроса
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// Дата доп запроса
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Файл предоставляемого документа
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Файл ответа
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }

        /// <summary>
        /// Текст ответа
        /// </summary>
        public virtual string AnswerText { get; set; }


        /// <summary>
        /// Текст ответа
        /// </summary>
        public virtual RequestRPGUState RequestRPGUState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }


    }
}