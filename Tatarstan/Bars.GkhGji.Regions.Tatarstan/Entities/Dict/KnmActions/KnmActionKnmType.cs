namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Связь действия КНМ с видом КНМ
    /// </summary>
    public class KnmActionKnmType : KnmActionBundle
    {
        /// <summary>
        /// Вид КНМ
        /// </summary>
        public virtual KnmTypes KnmTypes { get; set; }
        
        /// <inheritdoc />
        public override BaseEntity GkhGjiEntity
        {
            get => this.KnmTypes;
            set => this.KnmTypes = new KnmTypes { Id = value.Id };
        }
    }
}
