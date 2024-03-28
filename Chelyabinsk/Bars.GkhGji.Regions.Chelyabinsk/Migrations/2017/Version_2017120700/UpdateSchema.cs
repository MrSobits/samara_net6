namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017120700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017120700")]
    [MigrationDependsOn(typeof(Version_2017112800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_APPCIT_TRANSFER_RESULT",
              new Column("START_DATE", DbType.DateTime),
              new Column("END_DATE", DbType.DateTime),
              new Column("TYPE", DbType.Int32, ColumnProperty.NotNull),
              new Column("STATUS", DbType.Int32, ColumnProperty.NotNull),
              new Column("EXPORT_PARAMS", DbType.Binary, ColumnProperty.NotNull),
              new RefColumn("APPCIT_ID", ColumnProperty.NotNull, "GJI_APPCIT_TRANSFER_RESULT_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
              new RefColumn("USER_ID", "GJI_APPCIT_TRANSFER_RESULT_USER", "B4_USER", "ID"),
              new RefColumn("LOG_FILE_ID", "GJI_APPCIT_TRANSFER_RESULT_LOG_FILE", "B4_FILE_INFO", "ID")
          );

            this.InsertRevenueSourceGji();
            this.InsertKindStatementGji();

            this.Database.RemoveColumn("GJI_APPCIT_TRANSFER_RESULT", "CREATE_DATE");
            this.Database.RemoveColumn("GJI_APPCIT_TRANSFER_RESULT", "APPCIT_ANSWER_ID");
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "ANSWER_UID");

        }
        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPCIT_TRANSFER_RESULT");
        }

        /// <summary>
        /// Заполнение источников поступления
        /// </summary>
        private void InsertRevenueSourceGji()
        {
            var sql = @"INSERT INTO GJI_DICT_REVENUESOURCE
                (object_version, object_create_date, object_edit_date, code, name) VALUES
                (0, now(), now(), '1', 'Лично от граждан'),
                (0, now(), now(), '2', 'Администрация Президента РФ'),
                (0, now(), now(), '3', 'Аппарат Правительства РФ'),
                (0, now(), now(), '4', 'Интернет-приемная'),
                (0, now(), now(), '5', 'Почтовое отправление'),
                (0, now(), now(), '6', 'Электронная почта'),
                (0, now(), now(), '7', 'Аппарат полномочного представителя Президента РФ в УрФО'),
                (0, now(), now(), '8', 'Встречи с Губернатором области и его заместителями'),
                (0, now(), now(), '9', 'Главный Федеральный инспектор по Челябинской области'),
                (0, now(), now(), '10', 'Государственная Дума Федерального Собрания РФ'),
                (0, now(), now(), '11', 'ГУ МЧС России по Челябинской области'),
                (0, now(), now(), '12', 'Депутат городской Думы'),
                (0, now(), now(), '13', 'Депутат Законодательного Собрания Челябинской области'),
                (0, now(), now(), '14', 'Другие организации'),
                (0, now(), now(), '15', 'Законодательное Собрание Челябинской области'),
                (0, now(), now(), '16', 'Коммерческая организация (юр.лицо)'),
                (0, now(), now(), '17', 'МАУ «МФЦ города Челябинска»'),
                (0, now(), now(), '18', 'Мобильная приемная Администрации Президента РФ'),
                (0, now(), now(), '19', 'Некоммерческая организация'),
                (0, now(), now(), '20', 'Общественная организация'),
                (0, now(), now(), '21', 'Общественная приемная'),
                (0, now(), now(), '22', 'Общественная приемная Губернатора'),
                (0, now(), now(), '23', 'СМИ'),
                (0, now(), now(), '24', 'Совет депутатов'),
                (0, now(), now(), '25', 'Совет депутатов Калининского р-на ЧГО'),
                (0, now(), now(), '26', 'Совет депутатов Курчатовского р-на ЧГО'),
                (0, now(), now(), '27', 'Совет депутатов Ленинского р-на ЧГО'),
                (0, now(), now(), '28', 'Совет депутатов Металлургического р-на ЧГО'),
                (0, now(), now(), '29', 'Совет депутатов Советского р-на ЧГО'),
                (0, now(), now(), '30', 'Совет депутатов Тракторозаводского р-на ЧГО'),
                (0, now(), now(), '31', 'Совет депутатов Центрального р-на ЧГО'),
                (0, now(), now(), '32', 'Уполномоченный по правам'),
                (0, now(), now(), '33', 'Федеральные Министерства и ведомства'),
                (0, now(), now(), '34', '«Прямая линия с Президентом РФ»'),
                (0, now(), now(), '35', 'ВПП «Единая Россия»'),
                (0, now(), now(), '36', 'Областной избирательный штаб'),
                (0, now(), now(), '37', 'Совет Федерации Федерального Собрания РФ'),
                (0, now(), now(), '38', 'Прокуратура г. Златоуста'),
                (0, now(), now(), '39', 'Прокуратура г. Магнитогорска'),
                (0, now(), now(), '40', 'Прокуратура г. Миасса'),
                (0, now(), now(), '41', 'Прокуратура г. Озерска'),
                (0, now(), now(), '42', 'Прокуратура г. Снежинска'),
                (0, now(), now(), '43', 'Прокуратура г. Троицка'),
                (0, now(), now(), '44', 'Прокуратура г. Челябинска'),
                (0, now(), now(), '45', 'Прокуратура города'),
                (0, now(), now(), '46', 'Прокуратура Калининского р-на ЧГО'),
                (0, now(), now(), '47', 'Прокуратура Курчатовского р-на ЧГО'),
                (0, now(), now(), '48', 'Прокуратура Ленинского р-на ЧГО'),
                (0, now(), now(), '49', 'Прокуратура Металлургического р-на ЧГО'),
                (0, now(), now(), '50', 'Прокуратура района'),
                (0, now(), now(), '51', 'Прокуратура Советского р-на ЧГО'),
                (0, now(), now(), '52', 'Прокуратура Тракторозаводского р-на ЧГО'),
                (0, now(), now(), '53', 'Прокуратура Центрального р-на ЧГО'),
                (0, now(), now(), '54', 'Прокуратура Челябинской области'),
                (0, now(), now(), '55', 'Депутат Государственной Думы'),
                (0, now(), now(), '56', 'Калининский районный суд г. Челябинска'),
                (0, now(), now(), '57', 'КТОС'),
                (0, now(), now(), '58', 'Курчатовский  районный суд г. Челябинска'),
                (0, now(), now(), '59', 'Ленинский  районный суд г. Челябинска'),
                (0, now(), now(), '60', 'Металлургический районный суд р-на г. Челябинска'),
                (0, now(), now(), '61', 'Совет ветеранов войны и труда'),
                (0, now(), now(), '62', 'Советский районный суд г. Челябинска'),
                (0, now(), now(), '63', 'Тракторозаводский районный суд г. Челябинска'),
                (0, now(), now(), '64', 'Уполномоченные лица'),
                (0, now(), now(), '65', 'Управление ФСБ по Челябинской области'),
                (0, now(), now(), '66', 'Центральный районный суд г. Челябинска'),
                (0, now(), now(), '67', 'Челябинский областной суд'),
                (0, now(), now(), '68', 'МЭДО');";

            this.Database.ExecuteNonQuery(sql);
        }

        private void InsertKindStatementGji()
        {
            this.Database.ChangeDefaultValue("GJI_DICT_KINDSTATEMENT", "POSTFIX", "''");
            var sql = @"INSERT INTO GJI_DICT_KINDSTATEMENT
                (object_version, object_create_date, object_edit_date, code, name) VALUES
                (0, now(), now(), '1', 'В устной форме'),
                (0, now(), now(), '2', 'В электронной форме'),
                (0, now(), now(), '3', 'В письменной форме'),
                (0, now(), now(), '4', 'Телефон'),
                (0, now(), now(), '5', 'СМС'),
                (0, now(), now(), '6', 'Телеграмма'),
                (0, now(), now(), '7', 'Видео-прием'),
                (0, now(), now(), '8', 'Личный кабинет');";

            this.Database.ExecuteNonQuery(sql);
        }
    }
}