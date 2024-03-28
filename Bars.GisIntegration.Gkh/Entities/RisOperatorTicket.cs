namespace Bars.GisIntegration.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    public class RisOperatorTicket : BaseEntity
    {
        /// <summary>
        /// Оператор проекта БарсЖКХ
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Токен пользователя в проекте РИС ЖКХ
        /// </summary>
        public virtual string Ticket { get; set; }
    }
}
