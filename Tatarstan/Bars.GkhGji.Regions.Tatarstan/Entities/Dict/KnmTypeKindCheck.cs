namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Base;

    public class KnmTypeKindCheck : BaseEntity, IIncludeKindCheckGji
    {
        /// <summary>
        /// Вид КНМ
        /// </summary>
        public virtual KnmTypes KnmTypes { get; set; }

        /// <inheritdoc />
        public virtual KindCheckGji KindCheckGji { get; set; }
    }
}