namespace Bars.Gkh.Migrations._2023.Version_2023050118
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050118")]

    [MigrationDependsOn(typeof(Version_2023050117.UpdateSchema))]

    /// Является Version_2019021200 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_TYPE_FLOOR",
                new Column("CODE", DbType.String, 30, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));

            this.Database.ExecuteNonQuery(
              @"insert into GKH_DICT_TYPE_FLOOR 
                (OBJECT_VERSION, OBJECT_CREATE_DATE, OBJECT_EDIT_DATE, CODE, NAME) values
                (0, now(), now(), '1', 'Железобетонные'),
                (0, now(), now(), '2', 'Смешанные'),
                (0, now(), now(), '3', 'Деревянные'),
                (0, now(), now(), '0', 'Не задано'),
                (0, now(), now(), '4', 'Каркасные из тонкостенных оцинкованных термопрофилей для сборных зданий с утеплителем и обшивкой с двух сторон цементно-стружечной плитой'),
                (0, now(), now(), '5', 'Деревянные отепленные'),
                (0, now(), now(), '6', 'Панельно-стоечная конструкция из сборно-разборных панелей'),
                (0, now(), now(), '7', 'Перекрытия из железобетонных плит'),
                (0, now(), now(), '8', 'Перекрытие монолитное железобетонное'),
                (0, now(), now(), '9', 'Монолитные железобетонные плиты'),
                (0, now(), now(), '10', 'Плоские железобетонные плиты'),
                (0, now(), now(), '11', 'Железобетонные сборные плиты'),
                (0, now(), now(), '12', 'Перекрытия железобетонные'),
                (0, now(), now(), '13', 'Перекрытие монолитное'),
                (0, now(), now(), '14', 'Перекрытия из сборных и монолитных сплошных плит'),
                (0, now(), now(), '15', 'Перекрытия из сборного железобетонного настила'),
                (0, now(), now(), '16', 'Перекрытия из двухскорлупных железобетонных прокатных панелей'),
                (0, now(), now(), '17', 'Перекрытия из кирпичных сводов по стальным балкам'),
                (0, now(), now(), '18', 'Перекрытия деревянные оштукатуренные'),
                (0, now(), now(), '19', 'Перекрытия деревянные неоштукатуренные'),
                (0, now(), now(), '20', 'Арболитовые плиты'),
                (0, now(), now(), '21', 'Из плит OSB с утеплителем'),
                (0, now(), now(), '22', 'Многопустотные'),
                (0, now(), now(), '23', 'Сборные')");

            if (this.Database.TableExists("TP_TEH_PASSPORT_VALUE"))
            {
                /*Переводим значения в тех. паспорте из редактора Dict в MultiDict*/
                this.Database.ExecuteNonQuery(
                    @"update TP_TEH_PASSPORT_VALUE set VALUE = format('[%s]', VALUE) 
                      where FORM_CODE = 'Form_5_3' and CELL_CODE = '1:3' and VALUE is not null");
            }
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_TYPE_FLOOR");
        }
    }
}