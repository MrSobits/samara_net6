namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015041800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015041800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_ACTCHECKRO_LTEXT", new Column("ADDITIONALCHARS", DbType.Binary));

            if (this.Database.TableExists("GJI_ACTCHECK_LTEXT"))
            {
                this.Database.AddColumn("GJI_ACTCHECK_LTEXT", new Column("PERSON_VIOL_ACTION", DbType.Binary));
                this.Database.AddColumn("GJI_ACTCHECK_LTEXT", new Column("PERSON_VIOL", DbType.Binary));
            }
            else
            {
                this.Database.AddEntityTable("GJI_ACTCHECK_LTEXT",
                    new RefColumn("ACTCHECK_ID", ColumnProperty.NotNull, "GJI_ACTCHECKRO_LTEXT_ACT", "GJI_ACTCHECK", "ID"),
                    new Column("PERSON_VIOL_ACTION", DbType.Binary),
                    new Column("PERSON_VIOL", DbType.Binary));
            }

            this.Database.AddEntityTable("GJI_ACTCHECKVIOL_LTEXT",
                new RefColumn("ACTVIOL_ID", ColumnProperty.NotNull, "GJI_ACTCHECKVIOL_LTEXT_ACTV", "GJI_ACTCHECK_VIOLAT", "ID"),
                new Column("DESCRIPTION", DbType.Binary));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_ACTCHECKRO_LTEXT", "ADDITIONALCHARS");

            this.Database.RemoveTable("GJI_ACTCHECK_LTEXT");

            this.Database.RemoveTable("GJI_ACTCHECKVIOL_LTEXT");
        }
    }
}