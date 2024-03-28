namespace Bars.Gkh.Reforma.Migrations.Version_2014120200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014112900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddPersistentObjectTable(
                "RFRM_FILE",
                new Column("EXTERNAL_ID", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("FILE_INFO_ID", ColumnProperty.Null, "RFRM_FILE_FILEINFO", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("RFRM_FILE");
        }
    }
}