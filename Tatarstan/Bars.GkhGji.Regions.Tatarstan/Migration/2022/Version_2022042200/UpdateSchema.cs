namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022042200")]
    [MigrationDependsOn(typeof(Version_2022042100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DICT_CONTROL_TYPE_RISK_INDICATORS", 
                new RefColumn("CONTROL_TYPE_ID", ColumnProperty.NotNull, "GJI_DICT_RISK_INDICATORS_CONTROL_TYPE", "GJI_DICT_CONTROL_TYPES", "Id"), 
                new Column("NAME", DbType.String.WithSize(500)), 
                new Column("ERVK_ID", DbType.String.WithSize(36)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_CONTROL_TYPE_RISK_INDICATORS");
        }
    }
}