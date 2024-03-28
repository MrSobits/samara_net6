namespace Bars.GkhGji.Migrations.Version_2013071100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013071100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013070400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_VIOLATION", new Column("PPRF170", DbType.String, 2000));
            Database.AddColumn("GJI_DICT_VIOLATION", new Column("OTHER_DOCS", DbType.String, 2000));

            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("NAME", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("DESCRIPTION", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("GKRF", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("CODEPIN", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("PPRF25", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("PPRF307", DbType.String, 2000));
            Database.ChangeColumn("GJI_DICT_VIOLATION", new Column("PPRF491", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOLATION", "PPRF170");
            Database.RemoveColumn("GJI_DICT_VIOLATION", "OTHER_DOCS");
        }
    }
}