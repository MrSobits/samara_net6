namespace Bars.Gkh.Migrations._2015.Version_2015121400
{
    using System.Data;

    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.12.14.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015120701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_BUILDER_DOCUMENT_TYPE",
                new Column("CODE", DbType.Int32),
                new Column("NAME", DbType.String, 250));

            this.Database.AddUniqueConstraint("GKH_DICT_BUILDER_DOCUMENT_TYPE_UK", "GKH_DICT_BUILDER_DOCUMENT_TYPE", "CODE");

            this.Database.AddRefColumn("GKH_BUILDER_DOCUMENT", new RefColumn("DOCUMENT_TYPE_ID", "FK_GKH_DICT_BUILDER_DOCUMENT_TYPE", "GKH_DICT_BUILDER_DOCUMENT_TYPE", "ID"));
            
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_BUILDER_DOCUMENT", "DOCUMENT_TYPE_ID");
            this.Database.RemoveTable("GKH_DICT_BUILDER_DOCUMENT_TYPE");
        }
    }
}
