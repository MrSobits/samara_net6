namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021102600
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021102600")]
    [MigrationDependsOn(typeof(Version_2021101300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                COMMENT ON COLUMN pgmu_addresses.erc_code IS 'Код расчетного центра';
                COMMENT ON COLUMN pgmu_addresses.post_code IS 'Почтовый индекс';
                COMMENT ON COLUMN pgmu_addresses.town IS 'Город';
                COMMENT ON COLUMN pgmu_addresses.district IS 'Район';
                COMMENT ON COLUMN pgmu_addresses.street IS 'Улица';
                COMMENT ON COLUMN pgmu_addresses.house IS 'Дом';
                COMMENT ON COLUMN pgmu_addresses.building IS 'Корпус';
                COMMENT ON COLUMN pgmu_addresses.apartment IS 'Квартира';
                COMMENT ON COLUMN pgmu_addresses.room IS 'Комната';

                COMMENT ON COLUMN fssp_address.address IS 'Адрес';
                COMMENT ON COLUMN fssp_address.pgmu_address_id IS 'Адрес ПГМУ';

                COMMENT ON COLUMN fssp_litigation.jur_institution IS 'Подразделение ОСП';
                COMMENT ON COLUMN fssp_litigation.state IS 'Статус';
                COMMENT ON COLUMN fssp_litigation.ind_entr_registration_number IS 'Регистрационный номер ИП';
                COMMENT ON COLUMN fssp_litigation.debtor IS 'Должник';
                COMMENT ON COLUMN fssp_litigation.debtor_fssp_address_id IS 'Адрес должника (ФССП)';");
        }
    }
}