namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023041100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2023041100")]
    [MigrationDependsOn(typeof(Version_2023033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_CH_SMEV_EGRN_LOG",
           new Column("OP_TYPE", DbType.String),
           new Column("USER_LOGIN", DbType.String),
           new Column("USER_NAME", DbType.String),
           new RefColumn("SMEV_EGRN_ID", ColumnProperty.None, "SMEVEGRNLOG_SMEVEGRN_ID", "GJI_CH_SMEV_EGRN", "ID"),
           new RefColumn("FILE_INFO_ID", ColumnProperty.None, "SMEVEGRNLOG_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_CH_SMEV_EGRN_LOG");
        }
    }
}