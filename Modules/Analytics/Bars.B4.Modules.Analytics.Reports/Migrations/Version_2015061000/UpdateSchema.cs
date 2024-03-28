namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015061000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015061000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015060100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.RenameColumn("AL_STORED_REPORT", "TEMPLATE", "TEMPLATE_TMP");
                Database.AddColumn("AL_STORED_REPORT", new Column("TEMPLATE", DbType.Binary, int.MaxValue, ColumnProperty.Null));
                Database.ExecuteQuery("update al_stored_report set template=template_tmp");
                Database.RemoveColumn("AL_STORED_REPORT", "TEMPLATE_TMP");
            }
        }

        public override void Down()
        {
           
        }
    }
}