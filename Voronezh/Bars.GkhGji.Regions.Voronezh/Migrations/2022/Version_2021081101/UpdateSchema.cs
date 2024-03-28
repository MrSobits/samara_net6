namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021081101
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021081101")]
    [MigrationDependsOn(typeof(Version_2021081100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_APPCIT_PR_FOND_OBJECT_CR",
                new RefColumn("PR_FOND_ID", "GJI_PR_FOND_OBJ_CR_APPCIT", "GJI_APPCIT_PRESCRIPTION_FOND", "ID"),
                new RefColumn("CR_OBJECT_ID", "GJI_PR_FOND_CR_OBJECT", "CR_OBJECT", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_PR_FOND_OBJECT_CR");
        }
    }
}


