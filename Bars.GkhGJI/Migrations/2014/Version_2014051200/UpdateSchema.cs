namespace Bars.GkhGji.Migrations.Version_2014051200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014050700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_PLANJURPERSON", new Column("DATE_DISPOSAL", DbType.DateTime));
            Database.AddColumn("GJI_DICT_PLANJURPERSON", new Column("NUMBER_DISPOSAL", DbType.String, 50));

            Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("COUNT_HOURS", DbType.Int32, 22));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_PLANJURPERSON", "DATE_DISPOSAL");
            Database.RemoveColumn("GJI_DICT_PLANJURPERSON", "NUMBER_DISPOSAL");

            Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "COUNT_HOURS");
        }
    }
}