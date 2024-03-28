namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022062800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    // Объединение всех открытых веток миграций в одну
    [MigrationDependsOn(typeof(Version_2022020300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022041601.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022042600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022051800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022052300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_202206082022.UpdateSchema))]
    [Migration("2022062800")]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName Table 
            => new SchemaQualifiedObjectName { Name = "GJI_DICT_ERKNM_TYPE_DOCUMENT" };

        private string ColumnName => "DOCUMENT_TYPE";

        public override void Up()
        {
            this.Database.StringColumnChangeSize(this.Table, this.ColumnName, 1000);
        }

        public override void Down()
        {
            this.Database.StringColumnChangeSize(this.Table, this.ColumnName);
        }
    }
}