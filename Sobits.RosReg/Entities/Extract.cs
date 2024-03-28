namespace Sobits.RosReg.Entities
{
    using System;

    using Bars.B4.DataAccess;

    using Sobits.RosReg.Enums;

    /// <inheritdoc />
    /// <summary>
    /// Базовый класс для хранения выписок из Росреестра
    /// </summary>
    public class Extract : PersistentObject
    {
        /// <inheritdoc />
        public override long Id { get; set; }

        /// <summary>
        /// Дата импорта выписки
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// Тип выписки
        /// </summary>
        public virtual ExtractType Type { get; set; }

        //xml отдельно, ограничения nhibernate
        /// <summary>
        /// XML строкой
        /// </summary>
        public virtual string Xml { get; set; }

        /// <summary>
        /// Обработана ли выписка
        /// </summary>
        public virtual bool IsParsed { get; set; }

        /// <summary>
        /// Является ли выписка актуальной
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Id файла для печати
        /// </summary>
        public virtual long File { get; set; }

        /// <summary>
        /// Коментарии
        /// </summary>
        public virtual string Comment { get; set; }
    }
}