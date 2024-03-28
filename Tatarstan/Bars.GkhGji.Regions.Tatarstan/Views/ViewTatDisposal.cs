namespace Bars.GkhGji.Regions.Tatarstan.Views
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Представление для <see cref="TatarstanDisposal"/>
    /// </summary>
    public class ViewTatDisposal : ViewDisposal
    {
        /// <summary>
        /// Учетный номер проверки в ЕРП
        /// </summary>
        public virtual string ErpRegistrationNumber { get; set; }
        
        /// <summary>
        /// Наименование вида контроля
        /// </summary>
        public virtual string ControlTypeName { get; set; }
    }
}