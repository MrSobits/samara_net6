﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция для справочников "Предмет проверки" и "Виды фактов нарушений".
    /// Добавляет таблицу для Предмета проверки.
    /// Добавляет таблицу для Вида факта нарушений.
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_DICT_SURVEY_SUBJ",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));

            this.Database.AddEntityTable(
                "GJI_DICT_TYPE_FACT_VIOL",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_SURVEY_SUBJ");
            this.Database.RemoveTable("GJI_DICT_TYPE_FACT_VIOL");
        }
    }
}
