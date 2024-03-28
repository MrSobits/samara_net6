namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities.Base;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Base;

    /// <summary>
    /// Приложения акта проверки ГЖИ
    /// </summary>
    public class ActCheckAnnex : BaseGkhEntity, IEntityUsedInErknm, IAnnexEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        public virtual TypeAnnex TypeAnnex { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

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
        /// Статус файла 
        /// </summary>
        public virtual MessageCheck MessageCheck { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? DocumentSend { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public virtual DateTime? DocumentDelivered { get; set; }
        
        /// <summary>
        /// Передавать файл в ФГИС ЕРКНМ
        /// </summary>
        public virtual YesNoNotSet SendFileToErknm { get; set; }
        
        /// <summary>
        /// Идентифкатор Erknm
        /// </summary>
        public virtual string ErknmGuid { get; set; }
    }
}