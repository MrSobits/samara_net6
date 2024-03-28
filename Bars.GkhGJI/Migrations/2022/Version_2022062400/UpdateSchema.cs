namespace Bars.GkhGji.Migrations._2022.Version_2022062400
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022062400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022062200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_VIOLATION", new Column("NOT_ACTUAL", DbType.Boolean, ColumnProperty.None, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOLATION", "NOT_ACTUAL");
        }
    }
}