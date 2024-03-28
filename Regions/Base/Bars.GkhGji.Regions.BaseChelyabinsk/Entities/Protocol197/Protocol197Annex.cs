namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Приложения протокола ГЖИ 19.7
    /// </summary>
    public class Protocol197Annex : BaseGkhEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol197 Protocol197 { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }


        /// <summary>
        /// тип документа
        /// </summary>
        public virtual TypeAnnex TypeAnnex { get; set; }
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

    }
}