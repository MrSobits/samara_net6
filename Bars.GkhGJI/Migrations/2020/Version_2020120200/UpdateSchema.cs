namespace Bars.GkhGji.Migrations._2020.Version_2020120200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020120200")]
    [MigrationDependsOn(typeof(Version_2020101900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_STAT_SUB_SUBJECT", new Column("ANSWER_TEXT", DbType.String, 20000));
            Database.AddColumn("GJI_DICT_STAT_SUB_SUBJECT", new Column("ANSWER_TEXT2", DbType.String, 1500));
            
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_STAT_SUB_SUBJECT", "DESCRIPTION");
            Database.RemoveColumn("GJI_DICT_STAT_SUB_SUBJECT", "DESCRIPTION");
        }
    }
}