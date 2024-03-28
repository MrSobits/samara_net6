namespace Bars.GkhGji.Migrations._2024.Version_2024030114
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030114")]
    [MigrationDependsOn(typeof(Version_2024030113.UpdateSchema))]
    /// Является Version_2020041100 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            //this.MoveGisUinColumn("GJI_RESOLUTION", "GJI_DOCUMENT");
        }

        /// <inheritdoc />
        public override void Down()
        {
            //this.MoveGisUinColumn("GJI_DOCUMENT", "GJI_RESOLUTION");
        }

        private void MoveGisUinColumn(string tableFrom, string tableTo)
        {
            this.Database.AddColumn(tableTo, new Column("GIS_UIN", DbType.String, 50));
            this.Database.ExecuteNonQuery($"update {tableTo} set GIS_UIN = {tableFrom}.GIS_UIN from {tableFrom} where {tableTo}.ID = {tableFrom}.ID");
            this.Database.RemoveColumn(tableFrom, "GIS_UIN");
        }
    }
}