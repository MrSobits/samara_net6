namespace Bars.Gkh.Migrations._2023.Version_2023050112
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050112")]

    [MigrationDependsOn(typeof(Version_2023050111.UpdateSchema))]

    /// Является Version_2018091300 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (this.Database.TableExists("GKH_DICT_ENERGY_EFFICIENCY_CLASSES"))
            {
                this.Database.RemoveTable("GKH_DICT_ENERGY_EFFICIENCY_CLASSES");
            }

            this.Database.AddEntityTable("GKH_DICT_ENERGY_EFFICIENCY_CLASSES",
                new Column("CODE", DbType.String),
                new Column("DESIGNATION", DbType.String, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("DEVIATION_VALUE", DbType.String, ColumnProperty.NotNull));

            this.Database.ExecuteNonQuery(
                    @"insert into GKH_DICT_ENERGY_EFFICIENCY_CLASSES (object_version, object_create_date, object_edit_date, code, designation, name, deviation_value) values
                (0, now(), now(), '1', 'A++', 'Высочайший', '-60 включительно и менее'),
                (0, now(), now(), '2', 'A+', 'Высочайший', 'от -50 включительно до -60'),
                (0, now(), now(), '3', 'A', 'Очень высокий', 'от -40 включительно до -50'),
                (0, now(), now(), '4', 'B++', 'Класс неактуален', 'Класс неактуален'),
                (0, now(), now(), '5', 'B+', 'Класс неактуален', 'Класс неактуален'),
                (0, now(), now(), '6', 'B', 'Высокий', 'от -30 включительно до -40'),
                (0, now(), now(), '7', 'C', 'Повышенный', 'от -15 включительно до -30'),
                (0, now(), now(), '8', 'D', 'Нормальный', 'от 0 включительно до -15'),
                (0, now(), now(), '9', 'E', 'Пониженный', 'от +25 включительно до 0'),
                (0, now(), now(), '10', 'F', 'Низкий', 'от +50 включительно до +25'),
                (0, now(), now(), '11', 'G', 'Очень низкий', 'более +50')");

            this.Database.ExecuteNonQuery(@"update TP_TEH_PASSPORT_VALUE set value =                         
                case value when '1' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '3')::text
                        when '2' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '4')::text
                        when '3' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '5')::text
                        when '4' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '6')::text
                        when '5' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '7')::text
                        when '6' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '8')::text
                        when '7' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '9')::text
                        when '8' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '1')::text
                        when '9' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '2')::text
                        when '10' then (select id from GKH_DICT_ENERGY_EFFICIENCY_CLASSES where code = '10')::text
                else null end
                where form_code = 'Form_6_1_1' and (cell_code = '1:1' or cell_code = '3:1')");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_ENERGY_EFFICIENCY_CLASSES");
        }
    }
}