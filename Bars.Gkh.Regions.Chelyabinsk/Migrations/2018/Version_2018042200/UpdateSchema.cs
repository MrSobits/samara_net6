namespace Bars.Gkh.Regions.Chelyabinsk.Migrations._2018.Version_2018042200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018042200")]
    [MigrationDependsOn(typeof(Version_2018033000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" };

            this.Database.AddColumn(tableName, new Column("IS_MERGED", DbType.Int32,ColumnProperty.NotNull, 20));
            this.Database.AddColumn(tableName, new Column("ROOM_ID", DbType.Int64, ColumnProperty.None));
        }

        public override void Down()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" };
            this.Database.RemoveColumn(tableName, "ROOM_ID");
            this.Database.RemoveColumn(tableName, "ROOM_ID");
        }
    }
}