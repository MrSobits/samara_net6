namespace Bars.GkhGji.Migrations._2021.Version_2021122900
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021122900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021122800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECK", new Column("TYPE_CHECK_ACT", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.TypeCheck.NotPlannedExit));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK", "TYPE_CHECK_ACT");
        }
    }
}