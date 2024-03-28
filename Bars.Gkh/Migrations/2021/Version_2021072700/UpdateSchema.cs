namespace Bars.Gkh.Migrations._2021.Version_2021072700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021072700")]
    
    [MigrationDependsOn(typeof(Version_2021071900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
             "GKH_DICT_MONITORING_TYPE",
             new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
             new Column("CODE", DbType.String, 50),
             new Column("DESCRIPTION", DbType.String, 1000));

            Database.AddRefColumn("GKH_OBJ_TECHNICAL_MONITORING", new RefColumn("TYPE_ID", "GKH_OBJ_TECHNICAL_MONITORING_TYPE", "GKH_DICT_MONITORING_TYPE", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "TYPE_ID");
            Database.RemoveTable("GKH_DICT_MONITORING_TYPE");
        }
    }
}