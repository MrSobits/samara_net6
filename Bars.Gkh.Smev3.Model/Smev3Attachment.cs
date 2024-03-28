namespace Bars.Gkh.Smev3.Attachments
{
    using System;
    
    /// <summary>
    /// Базовая сущность вложения для сущности обмена со СМЭВ 3.0
    /// </summary>
    [Serializable]
    public abstract class Smev3Attachment
    {
        /// <summary>
        /// Идентификатор вложения, заполняется шлюзом в процессе обработки
        /// </summary>
        public string AttachmentId { get; set; }

        /// <summary>Идентификатор файла</summary>
        public long FileId { get; set; }

        /// <summary>Имя файла</summary>
        public string FileName { get; set; }

        /// <summary>Подпись пакета</summary>
        public byte[] PersonalSignature { get; set; }

        /// <summary>Пространство имен файла</summary>
        public string Namespace { get; set; }

        /// <summary>Описание вложений архива</summary>
        public Smev3ArchiveDescription[] Archive { get; set; }
    }
}