namespace Bars.Gkh.Migrations._2020.Version_2020120400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020120400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020113000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_EMPLOYESS_FKR", //,большими буквами
                new Column("NAME", DbType.String, ColumnProperty.NotNull, 300),
                new Column("POSITION", DbType.String, ColumnProperty.None, 500),
                new Column("DEPARTAMENT", DbType.String, ColumnProperty.None, 500));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_EMPLOYESS_FKR");
        }
    }
}