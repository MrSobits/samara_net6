namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;

    public class AdministrativeCaseDescription : BaseEntity
    {
        public virtual AdministrativeCase AdministrativeCase { get; set; }

        public virtual byte[] DescriptionSet { get; set; }
    }
}