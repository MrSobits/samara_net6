namespace Bars.Gkh.Entities.Base
{
    /// <summary>
    /// Сущность ЖКХ, которая регистрируется в ЕРКНМ
    /// </summary>
    public interface IEntityUsedInErknm
    {
        /// <summary>
        /// Идентификатор в ЕРКНМ
        /// </summary>
        string ErknmGuid { get; set; }
    }
}