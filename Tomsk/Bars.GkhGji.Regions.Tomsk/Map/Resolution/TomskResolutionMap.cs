/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities;
///     using Bars.GkhGji.Regions.Tomsk.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class TomskResolutionMap : SubclassMap<TomskResolution>
///     {
///         public TomskResolutionMap()
///         {
///             Table("GJI_TOMSK_RESOLUTION");
///             KeyColumn("ID");
///             Map(x => x.PhysicalPersonGender, "PHYSICAL_GENDER").CustomType<TypeGender>();
///             Map(x => x.ResolutionText, "RESOLUTION_TEXT").Length(2000);
///             Map(x => x.HasPetition, "HAS_PETITION").CustomType<TypeResolutionPetitions>();
///             Map(x => x.PetitionText, "PETITION_TEXT").Length(2000);
///             Map(x => x.FioAttend, "FIO_ATTEND").Length(500);
///             Map(x => x.ExplanationText, "EXPLANATION_TEXT").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.TomskResolution"</summary>
    public class TomskResolutionMap : JoinedSubClassMap<TomskResolution>
    {
        
        public TomskResolutionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.TomskResolution", "GJI_TOMSK_RESOLUTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPersonGender, "PhysicalPersonGender").Column("PHYSICAL_GENDER");
            Property(x => x.ResolutionText, "ResolutionText").Column("RESOLUTION_TEXT").Length(2000);
            Property(x => x.HasPetition, "HasPetition").Column("HAS_PETITION");
            Property(x => x.PetitionText, "PetitionText").Column("PETITION_TEXT").Length(2000);
            Property(x => x.FioAttend, "FioAttend").Column("FIO_ATTEND").Length(500);
            Property(x => x.ExplanationText, "ExplanationText").Column("EXPLANATION_TEXT").Length(2000);
        }
    }
}
