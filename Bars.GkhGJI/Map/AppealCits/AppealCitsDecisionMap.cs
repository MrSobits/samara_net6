namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Ответ по обращению"</summary>
    public class AppealCitsDecisionMap : BaseEntityMap<AppealCitsDecision>
    {
        
        public AppealCitsDecisionMap() : 
                base("Ответ по обращению", "GJI_APPCIT_DECISION")
        {

        }
        
        protected override void Map()
        {
            this.Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
            this.Property(x => x.AppealDate, "Дата документа").Column("APPEAL_DATE");
            this.Property(x => x.AppealNumber, "AppealNumber").Column("APPEAL_NUMBER");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.Established, "Established").Column("ESTABLISHED");
            this.Property(x => x.Decided, "Decided").Column("DECIDED");
            this.Reference(x => x.IssuedBy, "IssuedBy").Column("ISSUED_INSPECTOR_ID");
            this.Reference(x => x.SignedBy, "IssuedBy").Column("SIGNER_ID").NotNull();
            this.Reference(x => x.ConsideringBy, "СonsideringBy").Column("CONSIDER_INSPECTOR_ID").NotNull();
            this.Reference(x => x.Resolution, "Resolution").Column("DOC_ID");
            this.Property(x => x.Apellant, "Apellant").Column("APELLANT");
            this.Property(x => x.ApellantPosition, "ApellantPosition").Column("APELLANT_POSITION");
                 this.Property(x => x.ApellantPlaceWork, "ApellantPosition").Column("APELLANT_PLACE_WORK");
            this.Property(x => x.TypeDecisionAnswer, "TypeDecisionAnswer").Column("TYPE_ANSWER");
            this.Property(x => x.TypeAppelantPresence, "TypeDecisionAnswer").Column("TYPE_PRESENCE");
            this.Property(x => x.RepresentativeFio, "RepresentativeFio").Column("REPRESENTATIVE");
        }
    }

//    public class AppealCitsDecisionNHibernateMapping : ClassMapping<AppealCitsDecision>
//    {
//        public AppealCitsDecisionNHibernateMapping()
//        {
//            this.Property(
//                x => x.Established,
//                mapper =>
//                {
//                    mapper.Type<BinaryBlobType>();
//                });
//            this.Property(
//                x => x.Decided,
//                mapper =>
//                {
//                    mapper.Type<BinaryBlobType>();
//                });
          
//        }
//    }
}
