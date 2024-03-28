namespace Bars.GkhGji.Migrations._2017.Version_2017022100
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017022100
    /// </summary>
    [Migration("2017022100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                $"delete from GJI_INSPECTION_LIC_APP la where not exists(select 1 from GJI_INSPECTION where id = la.id)");

            this.Database.AddForeignKey("FK_GJI_INSPECTION_LIC_APP", "GJI_INSPECTION_LIC_APP", "ID", "GJI_INSPECTION", "ID");
        }
    }
}