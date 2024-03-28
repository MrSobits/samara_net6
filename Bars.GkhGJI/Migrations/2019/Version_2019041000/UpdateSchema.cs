namespace Bars.GkhGji.Migrations._2019.Version_2019041000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019041000")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019032000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("PAY_STATUS", DbType.Int32, ColumnProperty.NotNull, 30));
            UpdateResolutions();
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "PAY_STATUS");
        }

        private void UpdateResolutions()
        {
            var sql = @"UPDATE GJI_RESOLUTION SET PAY_STATUS = PAIDED;";

            this.Database.ExecuteNonQuery(sql);

        }
    }
}