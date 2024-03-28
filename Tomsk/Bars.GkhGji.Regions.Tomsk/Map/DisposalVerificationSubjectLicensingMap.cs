namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.DisposalVerificationSubjectLicensing"</summary>
    public class DisposalVerificationSubjectLicensingMap : BaseEntityMap<DisposalVerificationSubjectLicensing>
    {
        
        public DisposalVerificationSubjectLicensingMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.DisposalVerificationSubjectLicensing", "GJI_TOMSK_DISP_VERIFSUBJ_LIC")
        {
        }

	    protected override void Map()
	    {
		    Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
		    Reference(x => x.SurveySubject, "SurveySubject").Column("SURVEY_SUBJECT_LIC_ID").Fetch();
	    }
    }
}
