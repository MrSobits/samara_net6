namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014041100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!this.Database.TableExists("GJI_APPCIT_EXECUTANT"))
            {
                this.Database.AddEntityTable("GJI_APPCIT_EXECUTANT",
                   new Column("ORDER_DATE", DbType.DateTime, ColumnProperty.NotNull),
                   new Column("PERFOM_DATE", DbType.DateTime, ColumnProperty.NotNull),
                   new Column("RESPONSIBLE", DbType.Boolean, ColumnProperty.NotNull, false),
                   new Column("DESCRIPTION", DbType.String, 255),
                   new RefColumn("APPCIT_ID", ColumnProperty.NotNull, "GJI_APPCITEXEC_APP", "GJI_APPEAL_CITIZENS", "ID"),
                   new RefColumn("EXECUTANT_ID", ColumnProperty.NotNull, "GJI_APPCITEXEC_EXEC", "GKH_DICT_INSPECTOR", "ID"),
                   new RefColumn("AUTHOR_ID", ColumnProperty.Null, "GJI_APPCITEXEC_AUTH", "GKH_DICT_INSPECTOR", "ID"),
                   new RefColumn("STATE_ID", ColumnProperty.Null, "GJI_APPCITEXEC_STATE", "B4_STATE", "ID"));
            }
           
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPCIT_EXECUTANT");
        }
    }
}