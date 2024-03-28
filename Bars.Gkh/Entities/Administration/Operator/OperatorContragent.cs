namespace Bars.Gkh.Entities
{
    public class OperatorContragent : BaseGkhEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// УК
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}