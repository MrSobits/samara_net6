namespace Bars.GkhGji.Migrations._2018.Version_2018032700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2018032700")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018032000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_STATEMENT_SUBJ", new Column("SSTU_NAME", DbType.String));
            Database.AddColumn("GJI_DICT_STATEMENT_SUBJ", new Column("SSTU_CODE", DbType.String));


        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_STATEMENT_SUBJ", "SSTU_CODE");
            Database.RemoveColumn("GJI_DICT_STATEMENT_SUBJ", "SSTU_NAME");

        }
    }
}