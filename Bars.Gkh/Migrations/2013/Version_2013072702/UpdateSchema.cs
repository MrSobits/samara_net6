namespace Bars.Gkh.Migrations.Version_2013072702
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072702")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_OBJ_CONSTRUCT_ELEM",
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_CONSTR_ELM_OBJ", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("CONSTRUCT_ELEM_ID", ColumnProperty.NotNull, "GKH_OBJ_CONSTR_ELM_ELM", "GKH_DICT_CONST_ELEMENT", "ID"),
                new RefColumn("FILE_ID", "GKH_OBJ_CONSTR_ELM_FIL", "B4_FILE_INFO", "ID"),
                new Column("YEAR_INSTALLATION", DbType.Int32),
                new Column("REPAIRED", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("RATE_WEAR", DbType.Decimal),
                new Column("VOLUME", DbType.Decimal),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DOCUMENT_NUMBER", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_OBJ_CONSTRUCT_ELEM");
        }
    }
}