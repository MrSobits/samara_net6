using Bars.B4.DataAccess;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities.Base;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters
{
    /// <summary>
    /// Связь между характером КНМ и видом проверки
    /// </summary>
    public class KnmCharacterKindCheck : BaseEntity, IIncludeKindCheckGji
    {
        /// <summary>
        /// Характер КНМ
        /// </summary>
        public virtual KnmCharacter KnmCharacter { get; set; }

        /// <inheritdoc />
        public virtual KindCheckGji KindCheckGji { get; set; }
    }
}
