namespace Bars.Gkh.Migrations._2023.Version_2023050110
{
    using System.Data;
    using System.Text;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050110")]

    [MigrationDependsOn(typeof(Version_2023050109.UpdateSchema))]

    /// Является Version_2018082600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_CONTRAGENT_ROLE",
                new Column("CODE", DbType.String, ColumnProperty.Unique),
                new Column("NAME", DbType.String, 400, ColumnProperty.NotNull),
                new Column("SHORT_NAME", DbType.String, 400, ColumnProperty.NotNull));

            this.Database.AddRefColumn("GKH_CONTRAGENT", new RefColumn("MAIN_ROLE", "GKH_CONTRAGENT_MAIN_ROLE", "GKH_DICT_CONTRAGENT_ROLE", "ID"));

            this.Database.AddEntityTable("GKH_CONTRAGENT_ADDITION_ROLE",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "CONTR_ADDITION_ROLE_CONTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "CONTR_ADDITION_ROLE_ROLE", "GKH_DICT_CONTRAGENT_ROLE", "ID"));

            this.Database.AddUniqueConstraint("GKH_CONTRAGENT_ADDITION_ROLE_UNIQUE", "GKH_CONTRAGENT_ADDITION_ROLE", "CONTRAGENT_ID", "ROLE_ID");

            var initDictSql = @"insert into GKH_DICT_CONTRAGENT_ROLE (object_version, object_create_date, object_edit_date,code, name, short_name) values
                    (0, now()::date, now()::date, '1', 'Управляющая организация', 'УО'),
                    (0, now()::date, now()::date, '2', 'Ресурсоснабжающая организация', 'РСО'),
                    (0, now()::date, now()::date, '3', 'Оператор ГИС ЖКХ', 'Оператор ГИС ЖКХ'),
                    (0, now()::date, now()::date, '4', 'Орган исполнительной власти субъекта РФ, уполномоченный на осуществление государственного жилищного надзора', 'ГЖИ'),
                    (0, now()::date, now()::date, '5', 'Орган местного самоуправления, осуществляющий муниципальный жилищный контроль', 'ОМЖК'),
                    (0, now()::date, now()::date, '6', 'Федеральный орган исполнительной власти в области государственного регулирования тарифов', 'ФСТ'),
                    (0, now()::date, now()::date, '7', 'Орган государственной власти субъекта РФ', 'ОГВ субъекта РФ'),
                    (0, now()::date, now()::date, '8', 'Орган местного самоуправления', 'ОМС'),
                    (0, now()::date, now()::date, '10', 'Орган исполнительной власти субъекта РФ в области государственного регулирования тарифов', 'ОИВ субъекта РФ по регулированию тарифов'),
                    (0, now()::date, now()::date, '11', 'Администратор общего собрания собственников помещений в многоквартирном доме', 'Администратор общего собрания собственников помещений в многоквартирном доме'),
                    (0, now()::date, now()::date, '12', 'Орган государственной власти субъекта РФ в области энергосбережения и повышения энергетической эффективности', 'ОГВ субъекта РФ по энергосбережению'),
                    (0, now()::date, now()::date, '13', 'Орган или организация, уполномоченная на осуществление государственного учета жилищного фонда', 'Орган или организация, уполномоченная на осуществление государственного учета жилищного фонда'),
                    (0, now()::date, now()::date, '14', 'Специализированная некоммерческая организация (региональный оператор капитального ремонта)', 'Региональный оператор капитального ремонта'),
                    (0, now()::date, now()::date, '15', 'Фонд содействия реформированию жилищно-коммунального хозяйства', 'Фонд содействия реформированию жилищно-коммунального хозяйства'),
                    (0, now()::date, now()::date, '16', 'Уполномоченный орган субъекта РФ', 'Уполномоченный орган субъекта РФ'),
                    (0, now()::date, now()::date, '17', 'Министерство строительства и жилищно-коммунального хозяйства Российской Федерации', 'Минстрой России'),
                    (0, now()::date, now()::date, '18', 'Региональный оператор по обращению с твердыми коммунальными отходами', 'Региональный оператор по обращению с твердыми коммунальными отходами'),
                    (0, now()::date, now()::date, '19', 'Товарищество собственников жилья', 'ТСЖ'),
                    (0, now()::date, now()::date, '20', 'Жилищный кооператив', 'ЖК'),
                    (0, now()::date, now()::date, '21', 'Иной специализированный потребительский кооператив', 'Иной специализированный потребительский кооператив'),
                    (0, now()::date, now()::date, '22', 'Жилищно-строительный кооператив', 'ЖСК'),
                    (0, now()::date, now()::date, '23', 'Оператор информационной системы', 'Оператор ИС'),
                    (0, now()::date, now()::date, '24', 'Оператор по приему платежей', 'Оператор по приему платежей'),
                    (0, now()::date, now()::date, '25', 'Платежный субагент', 'Платежный субагент'),
                    (0, now()::date, now()::date, '26', 'Кредитная организация/Банк', 'Банк'),
                    (0, now()::date, now()::date, '27', 'Собственник помещения (наниматель)', 'Собственник помещения (наниматель)'),
                    (0, now()::date, now()::date, '28', 'Уполномоченный орган исполнительной власти субъекта Российской Федерации, осуществляющий  государственный контроль (надзор) в области долевого строительства многоквартирных домов и (или) иных объектов недвижимости', 'Контролирующий орган'),
                    (0, now()::date, now()::date, '29', 'Орган власти, уполномоченный на просмотр отчетности', 'Орган власти, уполномоченный на просмотр отчетности'),
                    (0, now()::date, now()::date, '30', 'Председатель правления товарищества собственников жилья (жилищного кооператива)', 'Председатель правления товарищества собственников жилья (жилищного кооператива)'),
                    (0, now()::date, now()::date, '31', 'Председатель совета многоквартирного дома', 'Председатель совета многоквартирного дома'),
                    (0, now()::date, now()::date, '32', 'Организация, уполномоченная поставщиком информации на размещение информации', 'Организация, уполномоченная поставщиком информации на размещение информации'),
                    (0, now()::date, now()::date, '33', 'Уполномоченный орган исполнительной власти субъекта Российской Федерации, осуществляющий обобщение и систематизацию информации, необходимой для проведения мониторинга использования жилищного фонда и обеспечения его сохранности', 'Уполномоченный ОИВ субъекта РФ по мониторигу ЖФ'),
                    (0, now()::date, now()::date, '34', 'Единоличный собственник помещений в многоквартирном доме', 'Единоличный собственник помещений в МКД'),
                    (0, now()::date, now()::date, '35', 'Министерство связи и массовых коммуникаций Российской Федерации', 'Минкомсвязь России'),
                    (0, now()::date, now()::date, '36', 'Расчетный центр', 'Расчетный центр'),
                    (0, now()::date, now()::date, '37', 'Иной контрагент', 'Иной контрагент'),
                    (0, now()::date, now()::date, '38', 'Орган местного самоуправления, уполномоченный на ведение программы «Формирование современной городской среды»', 'ОМС по программе «Формирование современной городской среды»'),
                    (0, now()::date, now()::date, '39', 'Орган власти субъекта РФ, уполномоченный на ведение программы «Формирование современной городской среды»', 'ОГВ субъекта РФ по программе «Формирование современной городской среды»'),
                    (0, now()::date, now()::date, '40', 'Субъект общественного жилищного контроля', 'Субъект общественного жилищного контроля');
";

            var sb = new StringBuilder(initDictSql);

            sb.AppendLine(@"update GKH_CONTRAGENT c
                        set main_role = 
                        case mo.TYPE_MANAGEMENT when 10 then (select id from GKH_DICT_CONTRAGENT_ROLE where code = '1')
                                    when 20 then (select id from GKH_DICT_CONTRAGENT_ROLE where code = '19')
                                    when 40 then (select id from GKH_DICT_CONTRAGENT_ROLE where code = '22')
                                    when 100 then (select id from GKH_DICT_CONTRAGENT_ROLE where code = '21')
                                    else null end
                        from GKH_MANAGING_ORGANIZATION mo
                        where mo.contragent_id=c.id and c.main_role is null;");

            sb.AppendLine(this.SetMainRole(8, "GKH_LOCAL_GOVERNMENT"));
            sb.AppendLine(this.SetMainRole(7, "GKH_POLITIC_AUTHORITY"));
            sb.AppendLine(this.SetMainRole(24, "GKH_PAYMENT_AGENT"));
            sb.AppendLine(this.SetMainRole(4, "GKH_HOUSING_INSPECTION"));
            sb.AppendLine(this.SetMainRole(36, "REGOP_CASHPAYMENT_CENTER"));
            sb.AppendLine(this.SetMainRole(14, "OVRHL_REG_OPERATOR"));
            sb.AppendLine(this.SetMainRole(2, "GKH_PUBLIC_SERVORG"));

            sb.AppendLine(this.AddAdditionalRoleSql(8, "GKH_LOCAL_GOVERNMENT"));
            sb.AppendLine(this.AddAdditionalRoleSql(7, "GKH_POLITIC_AUTHORITY"));
            sb.AppendLine(this.AddAdditionalRoleSql(24, "GKH_PAYMENT_AGENT"));
            sb.AppendLine(this.AddAdditionalRoleSql(4, "GKH_HOUSING_INSPECTION"));
            sb.AppendLine(this.AddAdditionalRoleSql(36, "REGOP_CASHPAYMENT_CENTER"));
            sb.AppendLine(this.AddAdditionalRoleSql(14, "OVRHL_REG_OPERATOR"));
            sb.AppendLine(this.AddAdditionalRoleSql(2, "GKH_PUBLIC_SERVORG"));

            this.Database.ExecuteNonQuery(sb.ToString());
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONTRAGENT", "MAIN_ROLE");
            this.Database.RemoveTable("GKH_CONTRAGENT_ADDITION_ROLE");
            this.Database.RemoveTable("GKH_DICT_CONTRAGENT_ROLE");
        }

        private string SetMainRole(int roleCode, string roleTable, string contragentField = "contragent_id")
        {
            return $@"UPDATE gkh_contragent c
SET main_role = (SELECT id FROM gkh_dict_contragent_role WHERE code = '{roleCode}' LIMIT 1)
FROM {roleTable} rt
WHERE rt.{contragentField} = c.id AND c.main_role IS NULL;";
        }

        private string AddAdditionalRoleSql(int roleCode, string roleTable, string contragentField = "contragent_id")
        {
            return $@"WITH roles AS (
    WITH single_role AS (
        SELECT id FROM gkh_dict_contragent_role WHERE code = '{roleCode}' LIMIT 1
    )
    SELECT 0, now()::date, now()::date, c.id, (SELECT * FROM single_role)
    FROM gkh_contragent c
    WHERE c.main_role IS NOT NULL
        AND c.main_role <> (SELECT * FROM single_role)
        AND EXISTS (SELECT 1 FROM {roleTable} WHERE c.id = {contragentField})
)
INSERT INTO gkh_contragent_addition_role (object_version, object_create_date, object_edit_date, contragent_id, role_id)
SELECT * FROM roles;";
        }
    }
}