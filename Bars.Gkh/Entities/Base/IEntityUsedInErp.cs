namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Сущность ЖКХ, которая регистрируется в ЕРП
    /// </summary>
    public interface IEntityUsedInErp
    {
        /// <summary>
        /// Идентификатор в ЕРП
        /// </summary>
        string ErpGuid { get; set; }
    }
}
