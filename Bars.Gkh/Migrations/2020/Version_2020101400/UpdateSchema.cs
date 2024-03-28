namespace Bars.Gkh.Migrations._2020.Version_2020101400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020101400")]
    
    [MigrationDependsOn(typeof(Version_2020052100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
             "GKH_DICT_MKDPROTOCOL_STATE",
             new Column("NAME", DbType.String, 50),
             new Column("CODE", DbType.String, 300),
             new Column("DESCRIPTION", DbType.String, 300));

            Database.AddEntityTable(
           "GKH_DICT_MKDPROTOCOL_SOURCE",
           new Column("NAME", DbType.String, 50),
           new Column("CODE", DbType.String, 300),
           new Column("DESCRIPTION", DbType.String, 300));

            Database.AddEntityTable(
           "GKH_DICT_MKDPROTOCOL_INICIATOR",
           new Column("NAME", DbType.String, 50),
           new Column("CODE", DbType.String, 300),
           new Column("DESCRIPTION", DbType.String, 300));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_MKDPROTOCOL_INICIATOR");
            Database.RemoveTable("GKH_DICT_MKDPROTOCOL_SOURCE");
            Database.RemoveTable("GKH_DICT_MKDPROTOCOL_STATE");
        }
    }
}