namespace Bars.Gkh.Migrations._2015.Version_2015120701
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015120700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_INTERLOCUTOR_INFORMATION",
                new Column("APARTMENT_NUMBER", DbType.String, 300),
                new Column("APARTMENT_AREA", DbType.Decimal, 300),
                new Column("FIO", DbType.String, 300),
                new Column("PROPERTY_TYPE", DbType.Int32, 300),
                new Column("AVAILABILITY_MINORD_AND_INCAPACITATED_PROPRIEORS", DbType.Boolean, 300),
                new Column("DATE_DEMOLITION_ISSUING", DbType.Date, 300),
                new Column("DATE_DEMOLITION_RECEIPT", DbType.Date, 300),
                new Column("DATE_NOTIFICATION", DbType.Date, 300),
                new Column("DATE_NOTIFICATION_RECEIPT", DbType.Date, 300),
                new Column("DATE_AGREEMENT", DbType.Date, 300),
                new Column("DATE_AGREEMENT_REFUSAL", DbType.Date, 300),
                new Column("DATE_OF_REFERRAL_CLAIM_COURT", DbType.Date, 300),
                new Column("DATE_OF_DECISION_BY_COURT", DbType.Date, 300),
                new Column("CONSIDERATION_RESULT_CLAIM", DbType.String, 300),
                new Column("DATE_APPEAL", DbType.Date, 300),
                new Column("DATE_APPEAL_DECISION", DbType.Date, 300),
                new Column("APPEAL_RESULT", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_INTERLOCUTOR_INFORMATION");
        }
    }
}
