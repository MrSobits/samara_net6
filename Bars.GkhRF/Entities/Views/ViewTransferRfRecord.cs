namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Вьюха на запись договора рег. фонда
    /// </summary>
    public class ViewTransferRfRecord : PersistentObject
    {
        /// <summary>
        /// Перечисление рег. фонда
        /// </summary>
        public virtual TransferRf TransferRf { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата перечисления
        /// </summary>
        public virtual DateTime? TransferDate { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public virtual int CountRecords { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? SumRecords { get; set; }
    }
}