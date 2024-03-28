namespace Bars.Gkh.Migrations._2021.Version_2021061000
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021061000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021052600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_JUR_INSTITUTION", new Column("OUTSIDE_ADDRESS", DbType.String, ColumnProperty.None, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_JUR_INSTITUTION", "OUTSIDE_ADDRESS");
        }
    }
}