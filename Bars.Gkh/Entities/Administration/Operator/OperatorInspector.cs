namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Хранит инспекторов у оператора, по которым применяются фильтры
    /// </summary>
    public class OperatorInspector : BaseGkhEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }
    }
}
