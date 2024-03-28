namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020060400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020060400")]
    [MigrationDependsOn(typeof(Version_2020050800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string KbkTableName = "GJI_DICT_KBK";
        private const string KbkMunicipalityTableName = "GJI_DICT_KBK_MUNICIPALITY";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.KbkTableName,
                new Column("KBK", DbType.String)
            );

            this.Database.AddEntityTable(UpdateSchema.KbkMunicipalityTableName,
                new RefColumn("KBK_ID", ColumnProperty.NotNull, "KBK_MUNICIPALITY_KBK",UpdateSchema.KbkTableName,"ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "KBK_MUNICIPALITY_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.KbkMunicipalityTableName);
            this.Database.RemoveTable(UpdateSchema.KbkTableName);
        }
    }
}
