namespace Sobits.RosReg.Entities
{
    using Bars.B4.DataAccess;

    public class ExtractEgrnRight : PersistentObject
    {
        /// <inheritdoc />
        public override long Id { get; set; }

        /// <summary>
        /// Тип собственности
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Номер права
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public virtual string Share { get; set; }

        /// <summary>
        /// Выписка
        /// </summary>
        public virtual ExtractEgrn EgrnId { get; set; }
    }
}