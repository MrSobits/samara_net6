namespace Bars.GkhGji.Migrations._2020.Version_2020041600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020041600")]
    [MigrationDependsOn(typeof(Version_2020031700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {       
            Database.AddColumn("GJI_DICT_STATEMENT_SUBJ", new Column("IS_SOPR", DbType.Boolean, false));
            Database.AddColumn("GJI_DICT_STAT_SUB_SUBJECT", new Column("IS_SOPR", DbType.Boolean, false));
        }

        public override void Down()
        {           
            Database.RemoveColumn("GJI_DICT_STAT_SUB_SUBJECT", "IS_SOPR");
            Database.RemoveColumn("GJI_DICT_STATEMENT_SUBJ", "IS_SOPR");
        }
    }
}