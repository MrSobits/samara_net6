namespace Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon
{
    using Bars.B4.Utils;
    /// <summary>
    /// Прокси-класс для отбора контрагентов в OrgRegistryRegistry
    /// </summary>
    public class ContragentProxy
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public ContragentState ContragentState { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public string FactAddress { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string JuridicalAddress { get; set; }

        /// <summary>
        /// Код организационно-правовой формы
        /// </summary>
        public string OrganizationFormCode { get; set; }
    }

    /// <summary>
    /// Статус контрагента
    /// </summary>
    public enum ContragentState
    {
        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Active = 10,

        /// <summary>
        /// Не предоставляет услуги управления
        /// </summary>
        [Display("Не предоставляет услуги управления")]
        NotManagementService = 20,

        /// <summary>
        /// Банкрот
        /// </summary>
        [Display("Банкрот")]
        Bankrupt = 30,

        /// <summary>
        /// Ликвидирован
        /// </summary>
        [Display("Ликвидирован")]
        Liquidated = 40
    }
}
