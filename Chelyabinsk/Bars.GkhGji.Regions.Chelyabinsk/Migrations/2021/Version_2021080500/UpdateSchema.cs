namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021080500
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021080500")]
    [MigrationDependsOn(typeof(Version_2021072700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_CH_APPCIT_EMERGENCY_HOUSE",
                new Column("APPCIT_ID", DbType.Int64),
                new Column("CONTRAGENT_ID", DbType.Int64),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("OMS_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String),
                new Column("DOCUMENT_NUM", DbType.String, 30),
                new Column("INSPECTOR_ID", DbType.Int64),
            new Column("FILE_INFO_ID", DbType.Int64));

            Database.AddForeignKey("FK_GJI_CH_APPCIT_EMHOUSE_FILE", "GJI_CH_APPCIT_EMERGENCY_HOUSE", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_CH_EMHOUSE_APPCIT", "GJI_CH_APPCIT_EMERGENCY_HOUSE", "APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID");
            Database.AddForeignKey("FK_GJI_CH_EMHOUSE_CONTR", "GJI_CH_APPCIT_EMERGENCY_HOUSE", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_CH_EMHOUSE_INSPECTOR", "GJI_CH_APPCIT_EMERGENCY_HOUSE", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_APPCIT_EMERGENCY_HOUSE");
        }
    }
}