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
    /// Приложения протокола ГЖИ
    /// </summary>
    public class ProtocolAnnex : BaseGkhEntity, Gkh.Entities.Base.IEntityUsedInErknm, Base.IAnnexEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol Protocol { get; set; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        public virtual Enums.TypeAnnex TypeAnnex { get; set; }

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
        public virtual Enums.MessageCheck MessageCheck { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? DocumentSend { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public virtual DateTime? DocumentDelivered { get; set; }
        
        /// Передавать файл в ФГИС ЕРКНМ
        /// </summary>
        public virtual Gkh.Enums.YesNoNotSet SendFileToErknm { get; set; }

        /// <summary>
        /// Идентифкатор Erknm
        /// </summary>
        public virtual string ErknmGuid { get; set; }
    }
}