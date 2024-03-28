namespace Bars.Gkh.Migrations.Version_2014030400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014030300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_POSITION", new Column("NAME_GENETIVE", DbType.String, 300));
            Database.AddColumn("GKH_DICT_POSITION", new Column("NAME_DATIVE", DbType.String, 300));
            Database.AddColumn("GKH_DICT_POSITION", new Column("NAME_ACCUSATIVE", DbType.String, 300));
            Database.AddColumn("GKH_DICT_POSITION", new Column("NAME_ABLATIVE", DbType.String, 300));
            Database.AddColumn("GKH_DICT_POSITION", new Column("NAME_PREPOSITIONAL", DbType.String, 300));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("NAME_GENETIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("SURNAME_GENETIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PATRONYMIC_GENETIVE", DbType.String, 100));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("NAME_DATIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("SURNAME_DATIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PATRONYMIC_DATIVE", DbType.String, 100));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("NAME_ACCUSATIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("SURNAME_ACCUSATIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PATRONYMIC_ACCUSATIVE", DbType.String, 100));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("NAME_ABLATIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("SURNAME_ABLATIVE", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PATRONYMIC_ABLATIVE", DbType.String, 100));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("NAME_PREPOSITIONAL", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("SURNAME_PREPOSITIONAL", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PATRONYMIC_PREPOSITIONAL", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_POSITION", "NAME_GENETIVE");
            Database.RemoveColumn("GKH_DICT_POSITION", "NAME_DATIVE");
            Database.RemoveColumn("GKH_DICT_POSITION", "NAME_ACCUSATIVE");
            Database.RemoveColumn("GKH_DICT_POSITION", "NAME_ABLATIVE");
            Database.RemoveColumn("GKH_DICT_POSITION", "NAME_PREPOSITIONAL");

            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "NAME_GENETIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "SURNAME_GENETIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PATRONYMIC_GENETIVE");

            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "NAME_DATIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "SURNAME_DATIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PATRONYMIC_DATIVE");

            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "NAME_ACCUSATIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "SURNAME_ACCUSATIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PATRONYMIC_ACCUSATIVE");

            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "NAME_ABLATIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "SURNAME_ABLATIVE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PATRONYMIC_ABLATIVE");

            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "NAME_PREPOSITIONAL");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "SURNAME_PREPOSITIONAL");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PATRONYMIC_PREPOSITIONAL");

        }
    }
}