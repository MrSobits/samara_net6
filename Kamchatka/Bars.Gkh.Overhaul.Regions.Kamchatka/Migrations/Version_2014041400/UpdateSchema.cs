namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Migrations.Version_2014041400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041400")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddJoinedSubclassTable("GKH_CONTRAGENT_BANK_KAMCH", "GKH_CONTRAGENT_BANK", "GKH_CONTR_BANK_HMAO",
                new RefColumn("CREDIT_ORG_ID", ColumnProperty.NotNull, "GKH_CONTR_BANK_HMAO_CRO", "OVRHL_CREDIT_ORG", "ID"),
                new RefColumn("FILE_ID", "GKH_CONTR_BANK_HMAO_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CONTRAGENT_BANK_KAMCH");
        }
    }
}