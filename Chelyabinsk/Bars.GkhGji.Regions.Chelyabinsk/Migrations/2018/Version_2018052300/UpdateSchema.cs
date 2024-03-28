﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018052300
{
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018052300")]
    [MigrationDependsOn(typeof(Version_2018040400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_CH_DICT_PAYER_STATUS",
              new Column("NAME", DbType.String, 5000, ColumnProperty.NotNull),
              new Column("CODE", DbType.String, 4, ColumnProperty.NotNull));

            Database.AddRefColumn("GJI_CH_GIS_GMP", new RefColumn("PAYER_STATUS_ID", ColumnProperty.None, "FK_GIS_GMP_PAYER_STATUS", "GJI_CH_DICT_PAYER_STATUS", "ID"));

            InsertPayerStatus();
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAYER_STATUS_ID");
            Database.RemoveTable("GJI_CH_DICT_PAYER_STATUS");
        }

        private void InsertPayerStatus()
        {
            var sql = @"INSERT INTO GJI_CH_DICT_PAYER_STATUS
                (object_version, object_create_date, object_edit_date, code, name) VALUES
                (0, now(), now(), '01', 'Юридическое лицо - налогоплательщик (плательщик сборов)'),
                (0, now(), now(), '02', 'Налоговый агент'),
                (0, now(), now(), '03', 'Организация федеральной почтовой связи, составившая распоряжение о переводе денежных средств по каждому платежу физического лица.'),
                (0, now(), now(), '04', 'Налоговый орган'),
                (0, now(), now(), '05', 'Территориальные органы Федеральной службы судебных приставов'),
                (0, now(), now(), '06', 'Участник внешнеэкономической деятельности- юридическое лицо'),
                (0, now(), now(), '07', 'Таможенный орган'),
                (0, now(), now(), '08', 'Плательщик - юридическое лицо (индивидуальный предприниматель), осуществляющее перевод денежных средств в уплату страховых взносов и иных платежей в бюджетную систему Российской Федерации.'),
                (0, now(), now(), '09', 'Налогоплательщик (плательщик сборов) - индивидуальный предприниматель'),
                (0, now(), now(), '10', 'Налогоплательщик (плательщик сборов) - нотариус, занимающийся частной практикой'),
                (0, now(), now(), '11', 'Налогоплательщик (плательщик сборов) - адвокат, учредивший адвокатский кабинет'),
                (0, now(), now(), '12', 'Налогоплательщик (плательщик сборов) - глава крестьянского (фермерского) хозяйства'),
                (0, now(), now(), '13', 'Налогоплательщик (плательщик сборов) - иное физическое лицо - клиент банка (владелец счета)'),
                (0, now(), now(), '14', 'Налогоплательщики, производящие выплаты физическим лицам'),
                (0, now(), now(), '15', 'Кредитная организация (филиал кредитной организации), платёжный агент, организация федеральной почтовой связи, составившие платёжное поручение на общую сумму с реестром на перевод денежных средств, принятых от плательщиков - физических лиц.'),
                (0, now(), now(), '16', 'Участник внешнеэкономической деятельности - физическое лицо'),
                (0, now(), now(), '17', 'Участник внешнеэкономической деятельности - индивидуальный предприниматель'),
                (0, now(), now(), '18', 'Плательщик таможенных платежей, не являющийся декларантом, на которого законодательством Российской Федерации возложена обязанность по уплате таможенных платежей.'),
                (0, now(), now(), '19', 'Организации и их филиалы (далее - организации), составившие распоряжение о переводе денежных средств, удержанных из заработной платы (дохода) должника - физического лица в счёт погашения задолженности по платежам в бюджетную систему Российской Федерации на основании исполнительного документа, направленного в организацию в установленном порядке.'),
                (0, now(), now(), '20', 'Кредитная организация (филиал кредитной организации), платёжный агент, составившие распоряжение о переводе денежных средств по каждому платежу физического лица.'),
                (0, now(), now(), '21', 'Ответственный участник консолидированной группы налогоплательщиков.'),
                (0, now(), now(), '22', 'Участник консолидированной группы налогоплательщиков.'),
                (0, now(), now(), '23', 'Органы контроля за уплатой страховых взносов.'),
                (0, now(), now(), '24', 'Плательщик - физическое лицо, осуществляющее перевод денежных средств в уплату страховых взносов и иных платежей в бюджетную систему Российской Федерации'),
                (0, now(), now(), '25', 'Банки - гаранты, составившие распоряжение о переводе денежных средств в бюджетную систему Российской Федерации при возврате налога на добавленную стоимость, излишне полученной налогоплательщиком (зачтенной ему) в заявительном порядке, а также при уплате акцизов, исчисленных по операциям реализации подакцизных товаров за пределы территории Российской Федерации, и акцизов по алкогольной и (или) подакцизной спиртосодержащей продукции.'),
                (0, now(), now(), '26', 'Учредители (участники) должника, собственники имущества должника - унитарного предприятия или третьи лица, составившие распоряжение о переводе денежных средств на погашение требований к должнику по уплате обязательных платежей, включённых в реестр требований кредиторов, в ходе процедур, применяемых в деле о банкротстве.');";

            this.Database.ExecuteNonQuery(sql);
        }
    }
}