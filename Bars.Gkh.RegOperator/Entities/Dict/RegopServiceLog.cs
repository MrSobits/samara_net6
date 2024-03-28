namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник расчетов пеней
    /// </summary>
    public class RegopServiceLog : BaseImportableEntity
    {
        /// <summary>
        /// Наименование РКЦ
        /// </summary>
        public virtual string CashPayCenterName { get; set; }

        /// <summary>
        /// Время 
        /// </summary>
        public virtual DateTime DateExecute { get; set; }

        /// <summary>
        /// Номер файла
        /// </summary>
        public virtual string FileNum { get; set; }

        /// <summary>
        /// Дата файла
        /// </summary>
        public virtual DateTime? FileDate { get; set; }

        /// <summary>
        /// Название метода
        /// </summary>
        public virtual RegopServiceMethodType MethodType { get; set; }

        /// <summary>
        /// Успешно - если все проверки успешны, Безуспешно - если какое либо из правил не успешен
        /// </summary>
        public virtual bool Status { get; set; }

        /// <summary>
        /// Файл лога
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}