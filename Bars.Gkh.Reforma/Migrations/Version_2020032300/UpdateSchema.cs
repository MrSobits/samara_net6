namespace Bars.Gkh.Reforma.Migrations.Version_2020032300
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020032300")]
    [MigrationDependsOn(typeof(Version_2016061000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                --обновление значение 'не задано'
                update TP_COMPARE_CODE set code_reforma = null
                where form_code = 'Form_5_3' and code_mjf ='1' and value = '4';
                
                update TP_COMPARE_CODE set code_reforma = null
                where form_code = 'Form_3_1' and code_mjf ='0' and value = '0';
                
                --типы перекрытий
                update TP_COMPARE_CODE set code_reforma = '1'
                where form_code = 'Form_5_3' and code_mjf in ('6109','6051','6014','6013','6011','6012','6010','4','5');
                update TP_COMPARE_CODE set code_reforma = '2'
                where form_code = 'Form_5_3' and code_mjf in ('10','6129','7','8');
                update TP_COMPARE_CODE set code_reforma = '3'
                where form_code = 'Form_5_3' and code_mjf in ('6140','6123','2','3','6','6144','6143','6210','6208','9');
                
                insert into TP_COMPARE_CODE (object_version, object_create_date, object_edit_date, form_code, cell_code, value, code_mjf, code_ris, code_reforma) values
                --тип стен
                (0, now(), now(), 'Form_5_2', '1:3', '1', '7', null::text, '1'), --Стены кирпичные с облицовкой керамическими блоками и плитками
                (0, now(), now(), 'Form_5_2', '1:3', '2', '9', null::text, '4'), --Стены из крупноразмерных блоков и однослойных несущих панелей
                (0, now(), now(), 'Form_5_2', '1:3', '3', '16', null::text, '6'), --Стены из монолитного железобетона
                (0, now(), now(), 'Form_5_2', '1:3', '4', '8', null::text, '4'), --Стены из мелких блоков, искусственных и естественных камней
                (0, now(), now(), 'Form_5_2', '1:3', '5', '15', null::text, '5'), --Стены деревянные
                (0, now(), now(), 'Form_5_2', '1:3', '6', '1', null::text, '1'), --Стены кирпичные
                (0, now(), now(), 'Form_5_2', '1:3', '7', '17', null::text, '2'), --Стены из сборно-щитовых панелей
                (0, now(), now(), 'Form_5_2', '1:3', '8', '14', null::text, '7'), --Стены каркасные
                (0, now(), now(), 'Form_5_2', '1:3', '9', '13', null::text, '7'), --Стены из силикальцита
                (0, now(), now(), 'Form_5_2', '1:3', '10', '12', null::text, '5'), --Стены из бруса
                (0, now(), now(), 'Form_5_2', '1:3', '11', '3', null::text, '7'), --Стены из Поротерма
                (0, now(), now(), 'Form_5_2', '1:3', '12', '2', null::text, '3'), --Стены из шлакоблоков
                (0, now(), now(), 'Form_5_2', '1:3', '13', '11', null::text, '2'), --Стены из несущих панелей
                (0, now(), now(), 'Form_5_2', '1:3', '14', '10', null::text, '2'), --Стены из слоистых железобетонных панелей
                (0, now(), now(), 'Form_5_2', '1:3', '15', '6', null::text, '5'), --Стены рубленные из бревен и брусчатые
                (0, now(), now(), 'Form_5_2', '1:3', '16', '5', null::text, '5'), --Стены деревянные каркасные
                (0, now(), now(), 'Form_5_2', '1:3', '17', '4', null::text, '5'), --Стены деревянные, сборно-щитовые
                --Мусоропроводы
                (0, now(), now(), 'Form_3_7_2', '1:3', '1', '1', null::text, '2'), --Квартирные
                (0, now(), now(), 'Form_3_7_2', '1:3', '2', '2', null::text, '3'), --Обособленные помещения на лесничной клетке
                (0, now(), now(), 'Form_3_7_2', '1:3', '3', '3', null::text, '3'), --Лестничная клетка
                --Электроснабжение
                (0, now(), now(), 'Form_3_3', '1:3', '2', '2', null::text, '1'), --Отсутствует
                (0, now(), now(), 'Form_3_3', '1:3', '1', '1', null::text, '2'), --Центральное
                (0, now(), now(), 'Form_3_3', '1:3', '3', '3', null::text, '3'), --Комбинированное
                --Системы вентиляции
                (0, now(), now(), 'Form_3_5', '1:3', '1', '1', null::text, '2'), --Приточная вентиляция
                (0, now(), now(), 'Form_3_5', '1:3', '2', '2', null::text, '3'), --Вытяжная вентиляция
                (0, now(), now(), 'Form_3_5', '1:3', '3', '3', null::text, '4'), --Приточно-вытяжная вентиляция
                (0, now(), now(), 'Form_3_5', '1:3', '4', '4', null::text, '1'), --Отсутствует
                --Системы водостоков
                (0, now(), now(), 'Form_3_6', '1:3', '1', '1', null::text, '2'), --Наружные водостоки
                (0, now(), now(), 'Form_3_6', '1:3', '2', '2', null::text, '3'), --Внутренние водостоки
                (0, now(), now(), 'Form_3_6', '1:3', '3', '3', null::text, '1'), --Отсутствует
                --Системы пожаротушения
                (0, now(), now(), 'Form_3_8', '1:3', '2', '2', null::text, '2'), --Автоматическая
                (0, now(), now(), 'Form_3_8', '1:3', '3', '3', null::text, '3'), --Пожарные гидранты
                (0, now(), now(), 'Form_3_8', '1:3', '1', '1', null::text, '1'); --Отсутствует");
        }
    }
}
