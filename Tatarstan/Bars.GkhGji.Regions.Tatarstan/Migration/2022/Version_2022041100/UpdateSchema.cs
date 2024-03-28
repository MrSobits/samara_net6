namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022041100")]
    [MigrationDependsOn(typeof(Version_2022040704.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        private const string DictTableName = "GJI_DICT_KNM_TYPES";
        private const string DictKindCheckTableName = "GJI_DICT_KNM_TYPE_KIND_CHECK";
        
        /// <inheritdoc />
        public override void Up()
        {
            //справочник "Виды контроля"
            this.Database.AddEntityTable(DictTableName,
                new Column("ERVK_ID", DbType.String, 36, ColumnProperty.NotNull));
            this.Database.AddEntityTable(DictKindCheckTableName,
                new RefColumn("KIND_CHECK_ID", "DICT_KIND_CHECK_ID", "GJI_DICT_KIND_CHECK", "ID"),
                new RefColumn("KNM_TYPE_ID", "DICT_KNM_TYPE_ID", "GJI_DICT_KNM_TYPES", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(DictKindCheckTableName);
            this.Database.RemoveTable(DictTableName);
        }
    }
}