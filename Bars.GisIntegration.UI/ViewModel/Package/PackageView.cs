namespace Bars.GisIntegration.UI.ViewModel.Package
{
    /// <summary>
    /// Представление пакета
    /// </summary>
    public class PackageView
    {
        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тип пакета
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Наименование пакета
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Пакет подписан
        /// </summary>
        public bool Signed { get; set; }
    }
}
