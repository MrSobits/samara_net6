namespace Bars.GkhGji.Migrations._2015.Version_2015092101
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015092100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("CITIZEN_ID", DbType.Int32));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("IS_IMPORTED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("STATE_ID", "GJI_APPCIT_ANSWER_ST", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "CITIZEN_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "IS_IMPORTED");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "STATE_ID");
        }
    }
}
