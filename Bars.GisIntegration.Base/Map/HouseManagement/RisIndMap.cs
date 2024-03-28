namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisInd"
    /// </summary>
    public class RisIndMap : BaseRisEntityMap<RisInd>
    {
        public RisIndMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisInd", "RIS_IND")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Surname, "Surname").Column("SURNAME").Length(50);
            this.Property(x => x.FirstName, "FirstName").Column("FIRSTNAME").Length(50);
            this.Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC").Length(50);
            this.Property(x => x.Sex, "Sex").Column("SEX");
            this.Property(x => x.DateOfBirth, "DateOfBirth").Column("DATEOFBIRTH");
            this.Property(x => x.IdTypeGuid, "IdTypeGuid").Column("IDTYPE_GUID").Length(50);
            this.Property(x => x.IdTypeCode, "IdTypeCode").Column("IDTYPE_CODE").Length(50);
            this.Property(x => x.IdSeries, "IdSeries").Column("IDSERIES").Length(50);
            this.Property(x => x.IdNumber, "IdNumber").Column("IDNUMBER").Length(50);
            this.Property(x => x.IdIssueDate, "IdIssueDate").Column("IDISSUEDATE");
            this.Property(x => x.Snils, "Snils").Column("SNILS").Length(50);
            this.Property(x => x.PlaceBirth, "PlaceBirth").Column("PLACEBIRTH").Length(50);
            this.Property(x => x.IsRegistered, "IsRegistered").Column("ISREGISTERED");
            this.Property(x => x.IsResides, "IsResides").Column("ISRESIDES");
        }
    }
}
