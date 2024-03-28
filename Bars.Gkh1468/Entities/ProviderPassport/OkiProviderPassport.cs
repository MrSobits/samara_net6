namespace Bars.Gkh1468.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Паспорт поставщика ОКИ
    /// </summary>
    public class OkiProviderPassport : BaseProviderPassport
    {
        /// <summary>
        /// Паспорт ОКИ
        /// </summary>
        public virtual OkiPassport OkiPassport { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Последний изменивший статус пользователь
        /// </summary>
        public virtual string UserName { get; set; }
    }
}