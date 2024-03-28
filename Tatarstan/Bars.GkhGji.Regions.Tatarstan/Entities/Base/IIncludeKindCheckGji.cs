using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Base
{
    /// <summary>
    /// Сущность содержит Вид проверки
    /// </summary>
    public interface IIncludeKindCheckGji
    {
        /// <summary>
        /// Вид проверки
        /// </summary>
        KindCheckGji KindCheckGji { get; set; }
    }
}
