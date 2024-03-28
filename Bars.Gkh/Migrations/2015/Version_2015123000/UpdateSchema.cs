namespace Bars.Gkh.Migrations._2015.Version_2015123000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015123000")]
    [MigrationDependsOn(typeof(Version_2015122500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("INPUT_MDV_BEGIN_DATE", DbType.Byte));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("INPUT_MDV_END_DATE", DbType.Byte));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DRAWING_PD_DATE", DbType.Byte));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("PROTOCOL_NUMBER", DbType.String, 300));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("PROTOCOL_DATE", DbType.DateTime));
            this.Database.AddRefColumn("GKH_MORG_CONTRACT_OWNERS", new RefColumn("PROTOCOL_FILE_INFO_ID", "GKH_MORG_CONTRACT_OWNERS_PROTOCOL_FILE", "B4_FILE_INFO", "ID"));

            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("INPUT_MDV_BEGIN_DATE", DbType.Byte));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("INPUT_MDV_END_DATE", DbType.Byte));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("DRAWING_PD_DATE", DbType.Byte));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("PROTOCOL_NUMBER", DbType.String, 300));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("PROTOCOL_DATE", DbType.DateTime));
            this.Database.AddRefColumn("GKH_MORG_CONTRACT_JSKTSJ", new RefColumn("PROTOCOL_FILE_INFO_ID", "GKH_MORG_CONTRACT_JSKTSJ_PROTOCOL_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "INPUT_MDV_BEGIN_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "INPUT_MDV_END_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DRAWING_PD_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "PROTOCOL_NUMBER");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "PROTOCOL_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "PROTOCOL_FILE_INFO_ID");

            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "INPUT_MDV_BEGIN_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "INPUT_MDV_END_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "DRAWING_PD_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "PROTOCOL_NUMBER");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "PROTOCOL_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "PROTOCOL_FILE_INFO_ID");
        }
    }
}
