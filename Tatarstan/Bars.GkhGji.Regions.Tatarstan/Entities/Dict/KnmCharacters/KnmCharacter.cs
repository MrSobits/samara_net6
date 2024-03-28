using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters
{
    /// <summary>
    /// Характер КНМ
    /// </summary>
    public class KnmCharacter : BaseEntity
    {
        /// <summary>
        /// Код в ЕРКНМ
        /// </summary>
        public virtual int? ErknmCode { get; set; }
    }
}
