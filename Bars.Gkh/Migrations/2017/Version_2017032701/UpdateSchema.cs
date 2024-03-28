namespace Bars.Gkh.Migrations._2017.Version_2017032701
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017032701
    /// </summary>
    [Migration("2017032701")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017031300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                "INSERT INTO GKH_FIELD_REQUIREMENT(object_version, object_create_date, object_edit_date, requirementid) VALUES " +
                "(0, current_date, current_date, 'GkhGji.PersonRegisterGji.Field.Contragent_Rqrd'), " +
                "(0, current_date, current_date, 'GkhGji.PersonRegisterGji.Field.StartDate_Rqrd')");

            this.Database.ChangeColumnNotNullable("gkh_person_placework", "contragent_id", false);
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}