namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Определение акта проверки ГЖИ
    /// </summary>
    public class ActCheckDefinition : BaseGkhEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Номер документа (целая часть)
        /// </summary>
        public virtual int? DocumentNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// ДЛ, вынесшее определение
        /// </summary>
        public virtual Inspector IssuedDefinition { get; set; }

        /// <summary>
        /// Тип определения
        /// </summary>
        public virtual TypeDefinitionAct TypeDefinition { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID вложения
        /// </summary>
        public virtual string GisGkhAttachmentGuid { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? DocumentSend { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public virtual DateTime? DocumentDelivered { get; set; }
    }
}