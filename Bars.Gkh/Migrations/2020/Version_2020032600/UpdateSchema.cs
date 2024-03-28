namespace Bars.Gkh.Migrations._2020.Version_2020032600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020032600")]
    [MigrationDependsOn(typeof(Version_2020032100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string OtherServiceDictTableName = "DI_DICT_TEMPL_OTHER_SERVICE";
        private const string OtherServiceTableName = "DI_OTHER_SERVICE";
        private const string OtherServiceDictColumnName = "TEMPLATE_OTHER_SERVICE_ID";
        private const string OtherServiceDictConstraintName = "TEMPLATE_OTHER_SERVICE_ID";
        private const string OtherServiceColumnName = "OTHER_SERVICE_ID";
        private const string TariffTableName = "DI_TARIFF_FCONSUMERS_OTHER_SERVICE";
        private const string ProviderTableName = "DI_SERVICE_PROVIDER_OTHER_SERVICE";

        public override void Up()
        {
            this.CreateOrRemoveOtherServiceDictTable(true);
            this.FillOrClearOtherServiceDictTable(true);
            this.UpdateTemplateOtherServiceId(true);
            this.AddOrRemoveTariffTableForOtherService(true);
            this.AddOrRemoveProviderTableForOtherService(true);
            this.FillTariffAndProviderTables();
        }

        public override void Down()
        {
            //откат миграции не реализован, потому что откатить изменения в таблице "Прочие услуги" нельзя (FillTariffAndProviderTables)
        }

        /// <summary>
        /// Добавление/удаление таблицы "Справочник прочих услуг", добавление/удаление колонки в таблицу "Прочие услуги"
        /// </summary>
        private void CreateOrRemoveOtherServiceDictTable(bool isUp)
        {
            if (!isUp)
            {
                if (this.Database.TableExists(UpdateSchema.OtherServiceTableName))
                {
                    this.Database.RemoveColumn(UpdateSchema.OtherServiceTableName, UpdateSchema.OtherServiceDictColumnName);
                }

                if (this.Database.TableExists(UpdateSchema.OtherServiceDictTableName))
                {
                    this.Database.RemoveTable(UpdateSchema.OtherServiceDictTableName);
                }

                return;
            }

            if (!this.Database.TableExists(UpdateSchema.OtherServiceDictTableName))
            {
                this.Database.AddEntityTable(UpdateSchema.OtherServiceDictTableName,
                    new Column("NAME", DbType.String, 300),
                    new Column("CODE", DbType.String, 300),
                    new Column("CHARACTERISTIC", DbType.String, 300),
                    new RefColumn("UNIT_MEASURE_ID", "DI_TEM_OTHER_SER_UM", "GKH_DICT_UNITMEASURE", "ID"));
            }

            if (this.Database.TableExists(UpdateSchema.OtherServiceTableName))
            {
                this.Database.AddRefColumn(UpdateSchema.OtherServiceTableName, new RefColumn(UpdateSchema.OtherServiceDictColumnName,
                    ColumnProperty.Null,
                    UpdateSchema.OtherServiceDictConstraintName,
                    UpdateSchema.OtherServiceDictTableName,
                    "id"));
            }
        }

        #region FillOrClearOtherServiceDictTable
        /// <summary>
        /// Заполнение/очистка справочника прочих услуг.
        /// </summary>
        private void FillOrClearOtherServiceDictTable(bool isUp)
        {
            if (!isUp)
            {
                if (!this.Database.TableExists(UpdateSchema.OtherServiceDictTableName))
                {
                    return;
                }

                var query = $"delete from {UpdateSchema.OtherServiceDictTableName}";
                this.Database.ExecuteNonQuery(query);
                return;
            }

            if (!this.Database.TableExists(UpdateSchema.OtherServiceDictTableName))
            {
                return;
            }

            this.FillOtherServiceDictTable();
        }

        /// <summary>
        /// Заполнение справочника "Прочие услуги".
        /// </summary>
        private void FillOtherServiceDictTable()
        {

            var query = $@"insert into {UpdateSchema.OtherServiceDictTableName} (object_version,object_create_date,object_edit_date,name,code,CHARACTERISTIC,UNIT_MEASURE_ID) values
		(1,now()::timestamp(0),now()::timestamp(0),'Освещение МОП','11','Освещение МОП',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Радио','12','Радио',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Антенна','13','Антенна',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Наем','15','Наем',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Телефон','27','Телефон',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Налог на жилые помещения','28','Налог на жилые помещения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'На печать счета','29','На печать счета',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'За лестничные площадки','30','За лестничные площадки',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание собак','31','Содержание собак',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Долг на 01.01.2003','32','Долг на 01.01.2003',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Долг предыдущей УК','33','Долг предыдущей УК',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Колокол-ТО и ремонт внутридомовых сетей центрального отопления','34','Колокол-ТО и ремонт внутридомовых сетей центрального отопления',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Колокол-ТО и ремонт внутредомовых электрических сетей','35','Колокол-ТО и ремонт внутредомовых электрических сетей',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Колокол-Содержание двора','36','Колокол-Содержание двора',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО Прочее','41','ТО Прочее',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Прочие','4','Прочие',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Взысканная задолженность','90','Взысканная задолженность',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Обслуживание лиц.счета','97','Обслуживание лиц.счета',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Текущий ремонт жилых зданий','98','Текущий ремонт жилых зданий',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Профицит','99','Профицит',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ЧЛЕНСКИЕ ВЗНОСЫ','188','ЧЛЕНСКИЕ ВЗНОСЫ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Долг на 01.04.2008','199','Долг на 01.04.2008',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Полив','200','Полив',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'Вода для домашних животных','201','Вода для домашних животных',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'Вода для транспорта','202','Вода для транспорта',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'Вода для бани','203','Вода для бани',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'Кабельное телевидение','204','Кабельное телевидение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Отопление хозяйственных построек','207','Отопление хозяйственных построек',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснабжение хозяйственных построек','208','Электроснабжение хозяйственных построек',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснабжение ночное','210','Электроснабжение ночное',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Прочие расходы на квартиру','211','Прочие расходы на квартиру',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Прочие расходы на квартиру','212','Прочие расходы на квартиру',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Банковские расходы','214','Банковские расходы',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Прочие расходы на кв.метр','215','Прочие расходы на кв.метр',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Кабельное телевидение','216','Кабельное телевидение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Кабельная антенна ГРТС','217','Кабельная антенна ГРТС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Кабельная антенна ООО Ильсар','218','Кабельная антенна ООО Ильсар',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Прочие расходы на человека','220','Прочие расходы на человека',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснабжение лифтов','221','Электроснабжение лифтов',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Вахтер','230','Вахтер',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Комендант','231','Комендант',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО общедомовых приборов','232','ТО общедомовых приборов',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО систем автоматической пожарной сигнализации','233','ТО систем автоматической пожарной сигнализации',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание общежития','234','Содержание общежития',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Платеж по установке газового оборудования','239','Платеж по установке газового оборудования',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Земельный налог','240','Земельный налог',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО индивид.систем газового отопления','241','ТО индивид.систем газового отопления',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснаб.лифта','242','Электроснаб.лифта',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Гараж','246','Гараж',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Тех.обслуживание внешнего газового оборудования','247','Тех.обслуживание внешнего газового оборудования',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Административно-хозяйственные расходы','248','Административно-хозяйственные расходы',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Установка приборов учета','250','Установка приборов учета',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Капитальный ремонт по 185ФЗ','251','Капитальный ремонт по 185ФЗ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание о/д.сетей газоснабжения','252','Содержание о/д.сетей газоснабжения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Питьевая вода','253','Питьевая вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО узлов учета тепловой энергии','254','ТО узлов учета тепловой энергии',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Лифт с квадратных метров','255','Лифт с квадратных метров',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Уборка подъезда с человека','256','Уборка подъезда с человека',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Ночное Освещение МОП','257','Ночное Освещение МОП',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание и текущий ремонт жилого помещения (кроме радио, домофона, антенны, газ.оборудования)','259','Содержание и текущий ремонт жилого помещения (кроме радио, домофона, антенны, газ.оборудования)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО узлов регулирования тепловой энергии','260','ТО узлов регулирования тепловой энергии',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО узлов учета водоснабжения','261','ТО узлов учета водоснабжения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Аренда контейтерных площадок','262','Аренда контейтерных площадок',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО газовых котлов инд.отопления','263','ТО газовых котлов инд.отопления',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Тревожная кнопка','265','Тревожная кнопка',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Капитальный ремонт (РегФонд)','267','Капитальный ремонт (РегФонд)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Наём (РегФонд)','268','Наём (РегФонд)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Капитальный ремонт по 185ФЗ (РегФонд)','269','Капитальный ремонт по 185ФЗ (РегФонд)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснабжение нерегулируемый тариф','270','Электроснабжение нерегулируемый тариф',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'Расходы по содержанию МКЖД','272','Расходы по содержанию МКЖД',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Управление многоквартирным домом','273','Управление многоквартирным домом',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Целевой сбор по решению МКД','274','Целевой сбор по решению МКД',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание контейнерных площадок','275','Содержание контейнерных площадок',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Страхование гражданской ответственности','276','Страхование гражданской ответственности',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Тех.обслуживание узлов учета тепловой энергии и ХВС','277','Тех.обслуживание узлов учета тепловой энергии и ХВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО узлов учета ГВС','278','ТО узлов учета ГВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание мусорных контейнеров по МКД','279','Содержание мусорных контейнеров по МКД',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Найм','280','Найм',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Теплоноситель (отопление)','281','Теплоноситель (отопление)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Налог УСН','282','Налог УСН',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Налог на зарплату','283','Налог на зарплату',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Сортировка ТБО','284','Сортировка ТБО',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Вывоз крупногабаритного мусора','285','Вывоз крупногабаритного мусора',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО узлов учета тепловой энергии','286','ТО узлов учета тепловой энергии',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Обсл.водонагр.','287','Обсл.водонагр.',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Страх.жилья','288','Страх.жилья',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО вентиляционных каналов','290','ТО вентиляционных каналов',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Тех.обслуживание внутридомового газового оборудования','291','Тех.обслуживание внутридомового газового оборудования',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Налог по экологии','292','Налог по экологии',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Ремонт кровельного покрытия','295','Ремонт кровельного покрытия',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Уличное освещение','296','Уличное освещение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Обслуживание котельной','299','Обслуживание котельной',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Сброс снега','300','Сброс снега',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Эксплуатация шлагбаума','301','Эксплуатация шлагбаума',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Оснащение и ТО видеонаблюдения','303','Оснащение и ТО видеонаблюдения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО ВКС без бойлера','304','ТО ВКС без бойлера',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО ВКС с бойлером','305','ТО ВКС с бойлером',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Проценты за рассрочку по холодной воде','306','Проценты за рассрочку по холодной воде',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Проценты за рассрочку по горячей воде','307','Проценты за рассрочку по горячей воде',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Проценты за рассрочку по канализации','308','Проценты за рассрочку по канализации',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Проценты за рассрочку по отоплению','309','Проценты за рассрочку по отоплению',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Проценты за рассрочку по электроснабжению','310','Проценты за рассрочку по электроснабжению',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Проценты за рассрочку по газу','311','Проценты за рассрочку по газу',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Платные услуги','313','Платные услуги',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Эксплуатационные расходы','316','Эксплуатационные расходы',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Агентское вознаграждение','317','Агентское вознаграждение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО в/д с.ц.о.с узл.р','318','ТО в/д с.ц.о.с узл.р',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО в/д с.ц.о.без узл','319','ТО в/д с.ц.о.без узл',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО и ремонт внутридомовых санитарно-технических сетей с ПУ','320','ТО и ремонт внутридомовых санитарно-технических сетей с ПУ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО и ремонт внутридомовых санитарно-технических сетей без ПУ','321','ТО и ремонт внутридомовых санитарно-технических сетей без ПУ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'МОП гаража','326','МОП гаража',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТР внутридом.сан.тех.сетей ХВС и ГВС (с приб.уч.)','327','ТР внутридом.сан.тех.сетей ХВС и ГВС (с приб.уч.)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТР внутридом.сан.тех.сетей ХВС и ГВС (без приб.уч.)','328','ТР внутридом.сан.тех.сетей ХВС и ГВС (без приб.уч.)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТР внутридом.сетей центр.отопл. (без приб.уч. и узл.рег.)','329','ТР внутридом.сетей центр.отопл. (без приб.уч. и узл.рег.)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Опрессовка','330','Опрессовка',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Консьерж','334','Консьерж',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Ремонт подъездов','335','Ремонт подъездов',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО авт.узл.рег.т/э','336','ТО авт.узл.рег.т/э',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО и рем в/д с.ц.о.','337','ТО и рем в/д с.ц.о.',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Установка ограждения','354','Установка ограждения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО и ТР систем КПТ','355','ТО и ТР систем КПТ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Взнос на капитальный ремонт','356','Взнос на капитальный ремонт',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'За обслуживание ЛС','358','За обслуживание ЛС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Авансовый платеж','367','Авансовый платеж',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Влажная уборка машиноместа','368','Влажная уборка машиноместа',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Взнос на капитальный ремонт','371','Взнос на капитальный ремонт',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО и ТР нежилого дома','375','ТО и ТР нежилого дома',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО и ТР парковки','376','ТО и ТР парковки',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснабжение паркинга','382','Электроснабжение паркинга',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Уборка парковки','385','Уборка парковки',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Размещение оборудования','393','Размещение оборудования',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Размещение слаботочного оборудования','396','Размещение слаботочного оборудования',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'СПУ контроля','397','СПУ контроля',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Аварийно-диспетчерская служба','398','Аварийно-диспетчерская служба',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Обслужив. приборов учета холод. водоснабжения','399','Обслужив. приборов учета холод. водоснабжения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Размещение РЭС','404','Размещение РЭС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Возн-е председ. МКД','405','Возн-е председ. МКД',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Обслуживание ворот','413','Обслуживание ворот',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Компонент на теплоноситель','414','Компонент на теплоноситель',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'ТР внутридом.сан.тех.сетей ХВС и ГВС (с приб.уч без ГВС)','419','ТР внутридом.сан.тех.сетей ХВС и ГВС (с приб.уч без ГВС)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТР внутридом.сан.тех.сетей ХВСи ГВС (с приб.учета без ХВС)','420','ТР внутридом.сан.тех.сетей ХВСи ГВС (с приб.учета без ХВС)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'% за рассрочку по Холодная вода для нужд ГВС','496','% за рассрочку по Холодная вода для нужд ГВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'% за рассрочку по Электроснабжение ночное','497','% за рассрочку по Электроснабжение ночное',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание(сан.очистка) помещений и зем.участка, вход-х в состав МОП','498','Содержание(сан.очистка) помещений и зем.участка, вход-х в состав МОП',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТР и ТО общего имущества в МКД за искл. лифта, домофона и антенны','499','ТР и ТО общего имущества в МКД за искл. лифта, домофона и антенны',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Пени','500','Пени',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Процент от начислений','501','Процент от начислений',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Пени для капитального ремонта','506','Пени для капитального ремонта',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Пени для найма','507','Пени для найма',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Холодная вода','510','ОДН-Холодная вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Канализация','511','ОДН-Канализация',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Отопление','512','ОДН-Отопление',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Горячая вода','513','ОДН-Горячая вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Холодная вода для нужд ГВС','514','ОДН-Холодная вода для нужд ГВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Электроснабжение','515','ОДН-Электроснабжение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Электроснабжение ночное','516','ОДН-Электроснабжение ночное',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН-Газ','517','ОДН-Газ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'ОДН - Комп. на тепл.','518','ОДН - Комп. на тепл.',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'Содержание и текущий ремонт дымоходов','522','Содержание и текущий ремонт дымоходов',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Техобслуживание вентканалов и дымоходов','523','Техобслуживание вентканалов и дымоходов',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'ТО блочно-модульной котельной','524','ТО блочно-модульной котельной',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Установка общедомовых приборов учета','525','Установка общедомовых приборов учета',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Прочие затраты','1000','Прочие затраты',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Система видеонаблюдения','1001','Система видеонаблюдения',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Услуга не определена','1097','Услуга не определена',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Ком услуги','1098','Ком услуги',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Найм и то','1099','Найм и то',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'Холодная вода на СОИ','1510','Холодная вода на СОИ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Водоотведение','1511','Водоотведение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Горячая вода на СОИ','1513','Горячая вода на СОИ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'Электроснабжение на СОИ','1515','Электроснабжение на СОИ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Холодная вода','3006','п\к Холодная вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Канализация (исп.п/к для ХВС и ГВС)','3007','п\к Канализация (исп.п/к для ХВС и ГВС)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Отопление','3008','п\к Отопление',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Горячая вода','3009','п\к Горячая вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Газоснабжение','3010','п\к Газоснабжение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Холодная вода для нужд ГВС','3014','п\к Холодная вода для нужд ГВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Электроснабжение','3025','п\к Электроснабжение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Полив','3200','п\к Полив',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Вода для домашних животных','3201','п\к Вода для домашних животных',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Вода для транспорта','3202','п\к Вода для транспорта',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Вода для бани','3203','п\к Вода для бани',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Отопление надворных построек','3207','п\к Отопление надворных построек',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Освещение надворных построек','3208','п\к Освещение надворных построек',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Электроснабжение (ночь)','3210','п\к Электроснабжение (ночь)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Теплоноситель (отопление)','3281','п\к Теплоноситель (отопление)',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Электроснабжение для отопления','3322','п\к Электроснабжение для отопления',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Газоснабжение на отопление','3325','п\к Газоснабжение на отопление',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'кв.м')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Водоснабжение надворных построек','3345','п\к Водоснабжение надворных построек',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к ОТОПЛЕНИЕ БАНИ','3349','п\к ОТОПЛЕНИЕ БАНИ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Транспортировка воды','3374','п\к Транспортировка воды',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Газовое отопление надворных построек','3401','п\к Газовое отопление надворных построек',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Освещение для сельскохозяйственных животных','3402','п\к Освещение для сельскохозяйственных животных',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Электроснабжение для приготовления пищи для животных','3403','п\к Электроснабжение для приготовления пищи для животных',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к Комп.на теплонос','3414','п\к Комп.на теплонос',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к ОДН-Холодная вода','3510','п\к ОДН-Холодная вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к ОДН-Горячая вода','3513','п\к ОДН-Горячая вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к ОДН-Холодная вода для нужд ГВС','3514','п\к ОДН-Холодная вода для нужд ГВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к ОДН-Электроснабжение','3515','п\к ОДН-Электроснабжение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'п\к ОДН-комп.на тепл','3518','п\к ОДН-комп.на тепл',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Холодная вода','4006','н\п Холодная вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Водоотведение','4007','н\п Водоотведение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Отопление','4008','н\п Отопление',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Горячая вода','4009','н\п Горячая вода',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'услуга')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Газ','4010','н\п Газ',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Холодная вода для нужд ГВС','4014','н\п Холодная вода для нужд ГВС',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Электроснабжение','4025','н\п Электроснабжение',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'Квт/час')),
(1,now()::timestamp(0),now()::timestamp(0),'н\п Компоненты на теплоноситель','4414','н\п Компоненты на теплоноситель',(SELECT id from GKH_DICT_UNITMEASURE WHERE name ilike 'куб.метр'));";

            this.Database.ExecuteNonQuery(query);
        }
        #endregion

        /// <summary>
        /// Обновляет поля template_other_service_id в таблице di_other_service
        /// </summary>
        private void UpdateTemplateOtherServiceId(bool isUp)
        {
            if (!this.Database.TableExists("di_other_service"))
            {
                return;
            }

            var query = isUp
                ? @"
                    --ссылки на услугу из справочника прочих услуг
                       update di_other_service set template_other_service_id = t.service_id, object_edit_date=now()::timestamp(0) from
	                    (select id service_id, name service_name, code service_code from DI_DICT_TEMPL_OTHER_SERVICE) t
	                    where (di_other_service.code is null or di_other_service.code = t.service_code) and LOWER(di_other_service.name) = LOWER(t.service_name);"
                : @"update di_other_service set template_other_service_id = NULL";

            this.Database.ExecuteNonQuery(query);
        }

        #region CreateorRemoveTariffAndProviderTables
        /// <summary>
        /// Добавляет/удаляет таблицу тарифов для прочих услуг.
        /// </summary>
        private void AddOrRemoveTariffTableForOtherService(bool isUp)
        {
            const string otherServiceConstraintName = "DI_TARIFF_FCONSUMERS_OTHER_SERVICE_CONSTRAINT";

            if (!this.Database.TableExists(UpdateSchema.TariffTableName) && isUp)
            {
                this.Database.AddEntityTable(UpdateSchema.TariffTableName,
                     new RefColumn(UpdateSchema.OtherServiceColumnName, otherServiceConstraintName, UpdateSchema.OtherServiceTableName, "ID"),
                     new Column("DATE_START", DbType.Date),
                     new Column("TARIFF_IS_SET_FOR", DbType.Int32, ColumnProperty.NotNull, 10),
                     new Column("COST", DbType.Decimal),
                     new Column("ORGANIZATION_SET_TARIFF", DbType.String, 300),
                     new Column("COST_NIGHT", DbType.Decimal),
                     new Column("TYPE_ORGAN_SET_TARIFF", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                     new Column("EXTERNAL_ID", DbType.String, 36),
                     new Column("DATE_END", DbType.Date),
                     new Column("IMPORT_ENTITY_ID", DbType.Int64));

                return;
            }

            if (!isUp && this.Database.TableExists(UpdateSchema.TariffTableName))
            {
                this.Database.RemoveTable(UpdateSchema.TariffTableName);
            }
        }

        /// <summary>
        /// Добавляет/удаляет таблицу поставщиков для прочих услуг.
        /// </summary>
        private void AddOrRemoveProviderTableForOtherService(bool isUp)
        {
            const string constraintName = "DI_PROVIDER_ID_OTHER_SERVICE";
            const string otherServiceConstraintName = "DI_SERVICE_PROVIDER_OTHER_SERVICE_CONSTRAINT";

            if (!this.Database.TableExists(UpdateSchema.ProviderTableName) && isUp)
            {
                const string contrAgentTableName = "GKH_CONTRAGENT";

                this.Database.AddEntityTable(UpdateSchema.ProviderTableName,
                    new RefColumn(UpdateSchema.OtherServiceColumnName, otherServiceConstraintName, UpdateSchema.OtherServiceTableName, "ID"),
                    new RefColumn("PROVIDER_ID", constraintName, contrAgentTableName, "ID"),
                    new Column("PROVIDER_NAME", DbType.String),
                    new Column("DATE_START_CONTRACT", DbType.Date),
                    new Column("DESCRIPTION", DbType.String, 500),
                    new Column("EXTERNAL_ID", DbType.String, 36),
                    new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull, true),
                    new Column("NUMBER_CONTRACT", DbType.String),
                    new Column("IMPORT_ENTITY_ID", DbType.Int64)
                );

                return;
            }

            if (!isUp && this.Database.TableExists(UpdateSchema.ProviderTableName))
            {
                this.Database.RemoveTable(UpdateSchema.ProviderTableName);
            }
        }
        #endregion

        /// <summary>
        /// Заполнение таблиц "Тарифы" и "Поставщики"
        /// </summary>
        private void FillTariffAndProviderTables()
        {
            if (!this.Database.TableExists(UpdateSchema.TariffTableName) || !this.Database.TableExists(UpdateSchema.ProviderTableName))
            {
                return;
            }

            var query = @"
                --тарифы
                drop table if exists tmp_other_service_tariff;
                create temp table tmp_other_service_tariff as                
                	SELECT id, 1 as object_version,NOW()::timestamp(0) as object_create_date,NOW()::timestamp(0) as object_edit_date, 
                	template_other_service_id as other_service_id, tariff as cost, 10 as tariff_is_set_for, 10 as type_organ_set_tariff, disinfo_ro_id
                	FROM 
                    di_other_service
                    WHERE template_other_service_id is not null;
                
                --поставщики
                drop table if exists tmp_other_service_provider;
                create temp table tmp_other_service_provider as
                	SELECT 1 as object_version,NOW()::timestamp(0) as object_create_date,NOW()::timestamp(0) as object_edit_date, 
                	template_other_service_id as other_service_id, provider as provider_name, TRUE as is_active, disinfo_ro_id
                	FROM 
                    di_other_service
                    WHERE template_other_service_id is not null;
                
                --удаление дублирующихся записей из прочих услуг
                DELETE FROM di_other_service
                WHERE id IN (SELECT id
                          FROM (SELECT id, ROW_NUMBER() 
                               OVER (partition BY template_other_service_id, disinfo_ro_id ORDER BY id) AS rnum
                               FROM di_other_service WHERE template_other_service_id is not null) t
                          WHERE t.rnum > 1);
                
                --обновление ссылки на прочие услуги во временных таблицах
                UPDATE tmp_other_service_tariff tost SET other_service_id = (SELECT id from di_other_service WHERE template_other_service_id = tost.other_service_id AND disinfo_ro_id = tost.disinfo_ro_id limit 1);
                UPDATE tmp_other_service_provider tosp SET other_service_id = (SELECT id from di_other_service WHERE template_other_service_id = tosp.other_service_id AND disinfo_ro_id = tosp.disinfo_ro_id limit 1);
                
                --заполнение таблицы тарифов
                insert into DI_TARIFF_FCONSUMERS_OTHER_SERVICE (object_version, object_create_date, object_edit_date, other_service_id, cost, tariff_is_set_for, type_organ_set_tariff) 
                (
                	SELECT object_version, object_create_date, object_edit_date, other_service_id, cost, tariff_is_set_for, type_organ_set_tariff from tmp_other_service_tariff
                );
                
                --заполнение таблицы поставщиков
                insert into DI_SERVICE_PROVIDER_OTHER_SERVICE (object_version, object_create_date, object_edit_date, other_service_id, provider_name, is_active) 
                (
                	SELECT object_version, object_create_date, object_edit_date, other_service_id, provider_name, is_active from tmp_other_service_provider
                );";
            this.Database.ExecuteNonQuery(query);
        }
    }
}