namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022092900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022092900")]
    [MigrationDependsOn(typeof(Version_2022082200.UpdateSchema))]
    [MigrationDependsOn(typeof(_2020.Version_2020060400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string KbkTableName = "GJI_DICT_KBK";
        private const string KbkArticleLawTableName = "GJI_DICT_KBK_ARTICLE_LAW";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.KbkTableName, "START_DATE", DbType.Date, ColumnProperty.NotNull, "NOW()");
            this.Database.AddColumn(UpdateSchema.KbkTableName, "END_DATE", DbType.Date, ColumnProperty.NotNull, "NOW()");

            this.Database.AddEntityTable(UpdateSchema.KbkArticleLawTableName,
                new RefColumn("KBK_ID", ColumnProperty.NotNull, "KBK_ARTICLE_LAW_KBK", UpdateSchema.KbkTableName, "ID"),
                new RefColumn("ARTICLE_LAW_ID", ColumnProperty.NotNull, "KBK_ARTICLE_LAW_ARTICLELAW", "GJI_DICT_ARTICLELAW", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.KbkTableName, "START_DATE");
            this.Database.RemoveColumn(UpdateSchema.KbkTableName, "END_DATE");
            
            this.Database.RemoveTable(UpdateSchema.KbkArticleLawTableName);
        }
    }
}