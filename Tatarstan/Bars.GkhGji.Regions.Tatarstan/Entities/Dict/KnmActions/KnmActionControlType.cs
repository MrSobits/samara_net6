using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Связь действия КНМ с видом контроля
    /// </summary>
    public class KnmActionControlType : KnmActionBundle
    {
        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <inheritdoc />
        public override BaseEntity GkhGjiEntity
        {
            get => this.ControlType;
            set => this.ControlType = new ControlType { Id = value.Id };
        }
    }
}
