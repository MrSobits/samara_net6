namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    using System;

    /// <summary>
    /// ФЛ - Абонент
    /// </summary>
    public class ChesMatchIndAccountOwner : ChesMatchAccountOwner
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Firstname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Lastname { get; set; }

        /// <inheritdoc />
        public override string Name => $"{this.Surname} {this.Firstname} {this.Lastname}".Trim();

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }
    }
}