namespace Bars.GkhGji.Migrations.Version_2014012900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014012800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_TYPESURVEY", "NAME_GENITIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_TYPESURVEY", "NAME_DATIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_TYPESURVEY", "NAME_ACCUSATIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_TYPESURVEY", "NAME_ABLATIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_TYPESURVEY", "NAME_PREPOSITIONAL", DbType.String, 300);

            Database.AddColumn("GJI_DICT_REVENUESOURCE", "NAME_GENITIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_REVENUESOURCE", "NAME_DATIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_REVENUESOURCE", "NAME_ACCUSATIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_REVENUESOURCE", "NAME_ABLATIVE", DbType.String, 300);
            Database.AddColumn("GJI_DICT_REVENUESOURCE", "NAME_PREPOSITIONAL", DbType.String, 300);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_TYPESURVEY", "NAME_GENITIVE");
            Database.RemoveColumn("GJI_DICT_TYPESURVEY", "NAME_DATIVE");
            Database.RemoveColumn("GJI_DICT_TYPESURVEY", "NAME_ACCUSATIVE");
            Database.RemoveColumn("GJI_DICT_TYPESURVEY", "NAME_ABLATIVE");
            Database.RemoveColumn("GJI_DICT_TYPESURVEY", "NAME_PREPOSITIONAL");

            Database.RemoveColumn("GJI_DICT_REVENUESOURCE", "NAME_GENITIVE");
            Database.RemoveColumn("GJI_DICT_REVENUESOURCE", "NAME_DATIVE");
            Database.RemoveColumn("GJI_DICT_REVENUESOURCE", "NAME_ACCUSATIVE");
            Database.RemoveColumn("GJI_DICT_REVENUESOURCE", "NAME_ABLATIVE");
            Database.RemoveColumn("GJI_DICT_REVENUESOURCE", "NAME_PREPOSITIONAL");
        }
    }
}