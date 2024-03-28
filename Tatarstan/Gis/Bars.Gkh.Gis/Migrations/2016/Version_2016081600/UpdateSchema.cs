namespace Bars.Gkh.Gis.Migrations._2016.Version_2016081600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016081600")]
    [MigrationDependsOn(typeof(Version_2016060800.UUpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("TRANSFER_CONTRACT_COMM_SERVICE",
                new RefColumn("CONTRACT_ID", "TRANSFER_COMM_SERV_CONTR", "GKH_MORG_CONTRACT_JSKTSJ", "ID"),
                new RefColumn("COMMUNAL_SERVICE_ID", "TRANSFER_COMM_SERV_SERVICE", "MANORG_BIL_COMMUNAL_SERVICE", "ID"),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime));

            this.Database.AddEntityTable("TRANSFER_CONTRACT_ADD_SERVICE",
                new RefColumn("CONTRACT_ID", "TRANSFER_ADD_SERV_CONTR", "GKH_MORG_CONTRACT_JSKTSJ", "ID"),
                new RefColumn("ADDITION_SERVICE_ID", "TRANSFER_ADD_SERV_SERVICE", "MANORG_BIL_ADDITION_SERVICE", "ID"),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveTable("TRANSFER_CONTRACT_COMM_SERVICE");
            this.Database.RemoveTable("TRANSFER_CONTRACT_ADD_SERVICE");
        }
    }
}
