namespace Bars.Gkh.Entities.Administration.PrintCertHistory
{
    using System;

    using Bars.B4.DataAccess;

    public class PrintCertHistory : BaseEntity
    {
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual string AccNum { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Наименование/ФИО
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата печати отчета
        /// </summary>
        public virtual DateTime PrintDate { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public virtual string Role { get; set; }
    }
}