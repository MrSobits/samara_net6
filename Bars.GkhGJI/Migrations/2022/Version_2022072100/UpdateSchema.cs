namespace Bars.GkhGji.Migrations._2022.Version_2022072100
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2022072100")]
    [MigrationDependsOn(typeof(Version_2022071500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLPROS", new Column("FORMAT_HOUR", DbType.Int32));
            Database.AddColumn("GJI_RESOLPROS", new Column("FORMAT_MINUTE", DbType.Int32));
            Database.AddColumn("GJI_RESOLPROS", new Column("DATE_RESOL_PROS", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLPROS", "DATE_RESOL_PROS");
            Database.RemoveColumn("GJI_RESOLPROS", "FORMAT_MINUTE");
            Database.RemoveColumn("GJI_RESOLPROS", "FORMAT_HOUR");
        }
    }
}