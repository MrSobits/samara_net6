namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022031500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022031500")]
    [MigrationDependsOn(typeof(Version_2022031400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_WARNING_INSPECTION";
        private const string ColumnName = "CONTROL_TYPE_ID";

        public override void Up()
        {
            this.Database.AddRefColumn(TableName, new RefColumn(ColumnName, ColumnProperty.Null, 
                "FK_CONTROL_TYPE_ID", "GJI_DICT_CONTROL_TYPES", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.TableName, ColumnName);
        }
    }
}