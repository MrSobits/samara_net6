namespace Bars.GkhCr.Migrations._2016.Version_2016092400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016092400
    /// </summary>
    [Migration("2016092400")]
    [MigrationDependsOn(typeof(Version_2016061100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "CR_OBJ_DESIGN_ASGMNT_TYPE_WORK",
                new RefColumn("ASSIGNMENT_ID", ColumnProperty.NotNull, "DESIGN_ASGMNT_ID", "CR_OBJ_DESIGN_ASSIGNMENT", "ID"),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "DESIGN_ASGMNT_TYPE_WORK_ID", "CR_OBJ_TYPE_WORK", "ID"));

            this.Database.ExecuteNonQuery(
                @"insert into cr_obj_design_asgmnt_type_work 
                (object_version, object_create_date, object_edit_date, assignment_id, type_work_id)
                SELECT 0, now(), now(), id, type_work_id from CR_OBJ_DESIGN_ASSIGNMENT");
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("CR_OBJ_DESIGN_ASGMNT_TYPE_WORK");
        }
    }
}