namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042001
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022042001")]
    [MigrationDependsOn(typeof(Version_2022041900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Создаем таблицу сущности ControlTypeInspectorPositions
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DICT_CONTROL_TYPE_INSPECTOR_POSITIONS", 
                new RefColumn("CONTROL_TYPE_ID", ColumnProperty.NotNull, "GJI_DICT_CT_INS_POS_CONTROL_TYPE", "GJI_DICT_CONTROL_TYPES", "Id"),
                new RefColumn("INSPECTOR_POS_ID", "GJI_DICT_CT_INS_POS_INSPECTOR_POS", "GJI_DICT_INSPECTOR_POSITIONS", "Id"),
                new Column("IS_ISSUER", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_MEMBER", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("ERVK_ID", DbType.String.WithSize(36)));
        }

        /// <summary>
        /// Удаляем таблицу ControlTypeInspectorPositions
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_CONTROL_TYPE_INSPECTOR_POSITIONS");
        }
    }
}