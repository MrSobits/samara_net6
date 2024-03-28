using System.Data;

namespace Bars.GkhGji.Migrations.Version_2013032400
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032202.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "EDO_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "IS_EDO");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "DATE_ACTUAL");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("EDO_ID", DbType.Int64, ColumnProperty.Null));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("IS_EDO", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("DATE_ACTUAL", DbType.DateTime));
           
        }
    }
}