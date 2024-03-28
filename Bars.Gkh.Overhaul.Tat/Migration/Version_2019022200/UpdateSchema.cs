namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2019022200
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019022200")]
    [MigrationDependsOn(typeof(Version_2018102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteQuery(
                @"insert into GKH_DICT_MULTIITEM
                  (OBJECT_VERSION, OBJECT_CREATE_DATE, OBJECT_EDIT_DATE, GLOSSARY_ID, KEY, VALUE) values
                  (0, now()::timestamp(0), now()::timestamp(0), (select id from GKH_DICT_MULTIGLOSSARY where code = 'cr_contract_type'), 'EnergySurvey', 'Энергообследование'),
                  (0, now()::timestamp(0), now()::timestamp(0), (select id from GKH_DICT_MULTIGLOSSARY where code = 'cr_contract_type'), 'TechPassport', 'Техпаспорт')");
        }

        public override void Down()
        {
            this.Database.ExecuteQuery(
                @"delete from GKH_DICT_MULTIITEM where 
                  GLOSSARY_ID = (select id from GKH_DICT_MULTIGLOSSARY where code = 'cr_contract_type') AND
                  (KEY = 'EnergySurvey' OR KEY = 'TechPassport')");
        }
    }
}