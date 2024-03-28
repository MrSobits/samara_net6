namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    /// <summary>
    /// ЮЛ - Абонент
    /// </summary>
    public class ChesMatchLegalAccountOwner : ChesMatchAccountOwner
    {
        /// <summary>
        /// Инн
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Кпп
        /// </summary>
        public virtual string Kpp { get; set; }
    }
}