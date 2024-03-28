namespace Bars.Gkh.Migrations._2015.Version_2015120700
    {
    using System.Data;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015120700")]
    [MigrationDependsOn(typeof(Version_2015120200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
	        this.Database.AddEntityTable("GKH_EMERGENCY_DOCUMENTS",
		        new Column("REQ_PUB_DATE", DbType.DateTime),
		        new Column("REQ_DOC_NUM", DbType.String),
		        new Column("DEC_PUB_DATE", DbType.DateTime),
		        new Column("DEC_REQ_MPA", DbType.String),
		        new Column("DEC_PUB_DATE_MPA", DbType.DateTime),
		        new Column("DEC_REG_DATE_MPA", DbType.DateTime),
		        new Column("DEC_NTF_DATE_MPA", DbType.DateTime),
		        new Column("AGR_PUB_DATE", DbType.DateTime),
		        new RefColumn("EMERGENCY_OBJ_ID", "GKH_EMERGENCY_DOCEO", "GKH_EMERGENCY_OBJECT", "ID"),
		        new RefColumn("REQ_FILE_INFO_ID", "GKH_EMERGENCY_DOCRFI", "B4_FILE_INFO", "ID"),
		        new RefColumn("DEC_FILE_INFO_ID", "GKH_EMERGENCY_DOCDFI", "B4_FILE_INFO", "ID"),
		        new RefColumn("AGR_FILE_INFO_ID", "GKH_EMERGENCY_DOCAFI", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_EMERGENCY_DOCUMENTS");
        }
    }
}
