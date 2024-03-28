namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015041800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015041800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECKRO_LTEXT", new Column("ADDITIONALCHARS", DbType.Binary));

            if (Database.TableExists("GJI_ACTCHECK_LTEXT"))
            {
                Database.AddColumn("GJI_ACTCHECK_LTEXT", new Column("PERSON_VIOL_ACTION", DbType.Binary));
                Database.AddColumn("GJI_ACTCHECK_LTEXT", new Column("PERSON_VIOL", DbType.Binary));
            }
            else
            {
                Database.AddEntityTable("GJI_ACTCHECK_LTEXT",
                    new RefColumn("ACTCHECK_ID", ColumnProperty.NotNull, "GJI_ACTCHECKRO_LTEXT_ACT", "GJI_ACTCHECK", "ID"),
                    new Column("PERSON_VIOL_ACTION", DbType.Binary),
                    new Column("PERSON_VIOL", DbType.Binary));
            }

            Database.AddEntityTable("GJI_ACTCHECKVIOL_LTEXT",
                new RefColumn("ACTVIOL_ID", ColumnProperty.NotNull, "GJI_ACTCHECKVIOL_LTEXT_ACTV", "GJI_ACTCHECK_VIOLAT", "ID"),
                new Column("DESCRIPTION", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECKRO_LTEXT", "ADDITIONALCHARS");

            Database.RemoveTable("GJI_ACTCHECK_LTEXT");

            Database.RemoveTable("GJI_ACTCHECKVIOL_LTEXT");
        }
    }
}