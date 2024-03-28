namespace Bars.Gkh.Migrations._2020.Version_2020111000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020111000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020110200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GKH_CS_CALCULATION", new RefColumn("RO_ID", ColumnProperty.None, "GKH_CS_CALCULATION_RO_ID", "GKH_REALITY_OBJECT", "ID"));
            Database.AddColumn("GKH_CS_CALCULATION", new RefColumn("ROOM_ID", ColumnProperty.None, "GKH_CS_CALCULATION_ROOM_ID", "GKH_ROOM", "ID"));
            ClearTable("GKH_CS_CALCULATION_ROW");
            ClearTable("GKH_CS_CALCULATION");
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CS_CALCULATION", "ROOM_ID");
            Database.RemoveColumn("GKH_CS_CALCULATION", "RO_ID");
        }
        private void ClearTable(string tablename)
        {
            var sql = $"Delete from {tablename}";

            this.Database.ExecuteNonQuery(sql);

        }

    }
}