namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021040200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021040200")]
    [MigrationDependsOn(typeof(Version_2021033101.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_LICENSE_REISSUANCE", new Column("REPLY_TO", DbType.String, 500));
            Database.AddColumn("GJI_CH_LICENSE_REISSUANCE", new Column("RPGU_NUMBER", DbType.String, 50));
            Database.AddColumn("GJI_CH_LICENSE_REISSUANCE", new Column("MESSAGE_ID", DbType.String, 50));
            Database.AddRefColumn("GJI_CH_LICENSE_REISSUANCE", new RefColumn("FILE_ID", ColumnProperty.Null, "GJI_CH_LICENSE_REISSUANCE_F", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable("GKH_MANORG_REISS_RPGU",
             new Column("ANSWER_TEXT", DbType.String, 5000),
             new Column("RPGU_TEXT", DbType.String, 5000),
             new Column("RPGU_REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
             new Column("RPGU_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 30),
             new Column("RPGU_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 30),
             new Column("MESSAGE_ID", DbType.String, 50),
             new RefColumn("LIC_REQUEST_ID", "GKH_MANORG_REISS_RPGU_REQUEST", "GJI_CH_LICENSE_REISSUANCE", "ID"),
             new RefColumn("ANSWER_FILE_ID", "GKH_MANORG_REISS_RPGU_FILEINFO", "B4_FILE_INFO", "ID"),
             new RefColumn("LIC_DOC_FILE_ID", "GKH_MANORG_REISS_RPGUFILE_FILEINFO", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_MANORG_REISS_RPGU");
            Database.RemoveColumn("GJI_CH_LICENSE_REISSUANCE", "REPLY_TO");
            Database.RemoveColumn("GJI_CH_LICENSE_REISSUANCE", "MESSAGE_ID");
            Database.RemoveColumn("GJI_CH_LICENSE_REISSUANCE", "RPGU_NUMBER");
            Database.RemoveColumn("GJI_CH_LICENSE_REISSUANCE", "FILE_ID");
        }


    }
}
