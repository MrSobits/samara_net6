namespace Bars.Gkh.Regions.Tatarstan.Migrations._2018.Version_2018122000
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    [Migration("2018122000")]
    [MigrationDependsOn(typeof(_2017.Version_2017121800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_EGSO_INTEGRATION",
                new Column("END_DATE", DbType.DateTime),
                new Column("TASK_TYPE", DbType.Int64, ColumnProperty.NotNull),
                new Column("STATUS_TYPE", DbType.Int64, ColumnProperty.NotNull, (int) EgsoTaskStateType.Undefined),
                new RefColumn("B4_USER_ID", ColumnProperty.NotNull, "EGSO_INTEGRATION_B4_USR", "B4_USER", "ID"),
                new RefColumn("B4_LOG_INFO_ID", "EGSO_INTEGRATION_B4_LOG_INF", "B4_FILE_INFO", "ID"),
                new Column("REPORT_YEAR", DbType.Int16, ColumnProperty.NotNull));
            
            this.Database.AddTable("GKH_EGSO_DICT_MUNICIPALITY",
                    new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                    new Column("TERRITORY_NAME", DbType.String.WithSize(100), ColumnProperty.NotNull),
                    new Column("TERRITORY_CODE", DbType.String.WithSize(8), ColumnProperty.NotNull),
                    new Column("EGSO_KEY", DbType.String.WithSize(6), ColumnProperty.NotNull));
            
            this.Database.AddTable("GKH_EGSO_INTEGRATION_VALUES",
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new RefColumn("EGSO_INTEGRATION_ID", ColumnProperty.NotNull, "GKH_EGSO_INTEGRATION_VALUES_EGSO_INTEGRATION", "GKH_EGSO_INTEGRATION", "ID"),
                new RefColumn("MUNICIPALITY_DICT_ID", ColumnProperty.NotNull, "GKH_EGSO_INTEGRATION_VALUES_MUNICIPALITY_DICT", "GKH_EGSO_DICT_MUNICIPALITY", "ID"),
                new Column("VALUE", DbType.Int32, ColumnProperty.NotNull));

            this.FillDictMunicipality();
        }

        public void FillDictMunicipality()
        {
            var municipalities = new List<DictMunicipalityDTO>
            {
                new DictMunicipalityDTO { Territory = "Муниципальные образования Республики Татарстан (Татарстана)", Code = "92000000", Key = "139502" },
                new DictMunicipalityDTO { Territory = "Агрызский муниципальный район", Code = "92601000", Key = "139504" },
                new DictMunicipalityDTO { Territory = "Азнакаевский муниципальный район", Code = "92602000", Key = "139529" },
                new DictMunicipalityDTO { Territory = "Аксубаевский муниципальный район", Code = "92604000", Key = "139560" },
                new DictMunicipalityDTO { Territory = "Актанышский муниципальный район", Code = "92605000", Key = "139584" },
                new DictMunicipalityDTO { Territory = "Алексеевский муниципальный район", Code = "92606000", Key = "139612" },
                new DictMunicipalityDTO { Territory = "Алькеевский муниципальный район", Code = "92607000", Key = "139635" },
                new DictMunicipalityDTO { Territory = "Альметьевский муниципальный район", Code = "92608000", Key = "139658" },
                new DictMunicipalityDTO { Territory = "Апастовский муниципальный район", Code = "92610000", Key = "139698" },
                new DictMunicipalityDTO { Territory = "Арский муниципальный район", Code = "92612000", Key = "139723" },
                new DictMunicipalityDTO { Territory = "Атнинский муниципальный район", Code = "92613000", Key = "139743" },
                new DictMunicipalityDTO { Territory = "Бавлинский муниципальный район", Code = "92614000", Key = "139757" },
                new DictMunicipalityDTO { Territory = "Балтасинский муниципальный район", Code = "92615000", Key = "139774" },
                new DictMunicipalityDTO { Territory = "Бугульминский муниципальный район", Code = "92617000", Key = "139795" },
                new DictMunicipalityDTO { Territory = "Буинский муниципальный район", Code = "92618000", Key = "139817" },
                new DictMunicipalityDTO { Territory = "Верхнеуслонский муниципальный район", Code = "92620000", Key = "139851" },
                new DictMunicipalityDTO { Territory = "Высокогорский муниципальный район", Code = "92622000", Key = "139872" },
                new DictMunicipalityDTO { Territory = "Дрожжановский муниципальный район", Code = "92624000", Key = "139903" },
                new DictMunicipalityDTO { Territory = "Елабужский муниципальный район", Code = "92626000", Key = "139924" },
                new DictMunicipalityDTO { Territory = "Заинский муниципальный район", Code = "92627000", Key = "139943" },
                new DictMunicipalityDTO { Territory = "Зеленодольский муниципальный район", Code = "92628000", Key = "139969" },
                new DictMunicipalityDTO { Territory = "Кайбицкий муниципальный район", Code = "92629000", Key = "139998" },
                new DictMunicipalityDTO { Territory = "Камско-Устьинский муниципальный район", Code = "92630000", Key = "140017" },
                new DictMunicipalityDTO { Territory = "Спасский муниципальный район", Code = "92632000", Key = "140040" },
                new DictMunicipalityDTO { Territory = "Кукморский муниципальный район", Code = "92633000", Key = "140061" },
                new DictMunicipalityDTO { Territory = "Лаишевский муниципальный район", Code = "92634000", Key = "140094" },
                new DictMunicipalityDTO { Territory = "Лениногорский муниципальный район", Code = "92636000", Key = "140121" },
                new DictMunicipalityDTO { Territory = "Мамадышский муниципальный район", Code = "92638000", Key = "140149" },
                new DictMunicipalityDTO { Territory = "Менделеевский муниципальный район", Code = "92639000", Key = "140186" },
                new DictMunicipalityDTO { Territory = "Мензелинский муниципальный район", Code = "92640000", Key = "140204" },
                new DictMunicipalityDTO { Territory = "Муслюмовский муниципальный район", Code = "92642000", Key = "140227" },
                new DictMunicipalityDTO { Territory = "Нижнекамский муниципальный район", Code = "92644000", Key = "140248" },
                new DictMunicipalityDTO { Territory = "Новошешминский муниципальный район", Code = "92645000", Key = "140268" },
                new DictMunicipalityDTO { Territory = "Нурлатский муниципальный район", Code = "92646000", Key = "140285" },
                new DictMunicipalityDTO { Territory = "Пестречинский муниципальный район", Code = "92648000", Key = "140315" },
                new DictMunicipalityDTO { Territory = "Рыбно-Слободский муниципальный район", Code = "92650000", Key = "140338" },
                new DictMunicipalityDTO { Territory = "Сабинский муниципальный район", Code = "92652000", Key = "140369" },
                new DictMunicipalityDTO { Territory = "Сармановский муниципальный район", Code = "92653000", Key = "140392" },
                new DictMunicipalityDTO { Territory = "Ютазинский муниципальный район", Code = "92654000", Key = "140418" },
                new DictMunicipalityDTO { Territory = "Тетюшский муниципальный район", Code = "92655000", Key = "140432" },
                new DictMunicipalityDTO { Territory = "Тюлячинский муниципальный район", Code = "92656000", Key = "140462" },
                new DictMunicipalityDTO { Territory = "Тукаевский муниципальный район", Code = "92657000", Key = "140477" },
                new DictMunicipalityDTO { Territory = "Черемшанский муниципальный район", Code = "92658000", Key = "140502" },
                new DictMunicipalityDTO { Territory = "Чистопольский муниципальный район", Code = "92659000", Key = "140522" },
                new DictMunicipalityDTO { Territory = "город Казань", Code = "92701000", Key = "140550" },
                new DictMunicipalityDTO { Territory = "город Набережные Челны", Code = "92730000", Key = "140551" }
            };

            foreach (var municipality in municipalities)
            {
                this.Database.Insert(new SchemaQualifiedObjectName { Schema = "public", Name = "GKH_EGSO_DICT_MUNICIPALITY" },
                    new[] { "TERRITORY_NAME", "TERRITORY_CODE", "EGSO_KEY" },
                    new[] { municipality.Territory, municipality.Code, municipality.Key });
            }
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_EGSO_INTEGRATION");
            this.Database.RemoveTable("GKH_EGSO_DICT_MUNICIPALITY");
            this.Database.RemoveTable("GKH_EGSO_INTEGRATION_VALUES");
        }

        private class DictMunicipalityDTO
        {
            public string Territory { get; set; }
            
            public string Code { get; set; }
            
            public string Key { get; set; }
        }
    }
}