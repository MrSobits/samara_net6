namespace Bars.GisIntegration.RegOp.Migrations.Version_2016101700
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016101700")]
    [MigrationDependsOn(typeof(Version_2016101200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ExecuteNonQuery(@"
            //    CREATE OR REPLACE FUNCTION ris.get_gkh_house_id(ris_house_id integer)
            //      RETURNS integer AS
            //    $BODY$
            //    --=================================================================
            //    -- Назначение: Получить id дома из МЖФ
            //    -- Автор: Царегородцева Е.Д.                                               
            //    -- Дата создания: 06.10.2016                                       
            //    -- Дата изменения:                                      
            //    --=================================================================
            //    BEGIN
            //        return COALESCE(ris_house_id, 0);
            //    END;
            //    $BODY$
            //      LANGUAGE plpgsql VOLATILE
            //      COST 100;
            //    ALTER FUNCTION ris.get_gkh_house_id(integer)
            //      OWNER TO bars;
            //");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}