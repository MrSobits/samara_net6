namespace Bars.GkhGji.Migrations._2018.Version_2018040200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2018040200")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018032700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_STAT_SUB_SUBJECT", new Column("SSTU_NAME_SUB", DbType.String));
            Database.AddColumn("GJI_DICT_STAT_SUB_SUBJECT", new Column("SSTU_CODE_SUB", DbType.String));


        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_STAT_SUB_SUBJECT", "SSTU_CODE_SUB");
            Database.RemoveColumn("GJI_DICT_STAT_SUB_SUBJECT", "SSTU_NAME_SUB");

        }
    }
}