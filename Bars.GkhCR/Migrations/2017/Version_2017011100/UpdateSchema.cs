namespace Bars.GkhCr.Migrations._2017.Version_2017011100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017011100
    /// </summary>
    [Migration("2017011100")]
    [MigrationDependsOn(typeof(Bars.GkhCr.Migrations._2016.Version_2016092400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("CR_OBJ_BUILD_CONTRACT",
                new RefColumn("CONTRAGENT_ID", "FK_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "CONTRAGENT_ID");
        }
    }
}