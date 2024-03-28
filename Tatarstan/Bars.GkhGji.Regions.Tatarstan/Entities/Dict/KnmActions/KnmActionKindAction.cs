namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Связь Вид меропрития и действия КНМ
    /// </summary>
    public class KnmActionKindAction : KnmActionBundle
    {
        /// <summary>
        /// Вид меропрития
        /// </summary>
        public virtual KindAction KindAction { get; set; }

        /// <inheritdoc />
        /// <inheritdoc />
        public override BaseEntity GkhGjiEntity
        {
            get => new BaseEntity
            {
                Id = (long) this.KindAction
            };
            set => this.KindAction = (KindAction)value.Id;
        }
    }
}