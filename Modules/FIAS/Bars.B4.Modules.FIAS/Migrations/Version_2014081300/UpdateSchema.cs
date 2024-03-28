namespace Bars.B4.Modules.FIAS.Migrations.Version_2014081300
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014081300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2014012900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("B4_FIAS", new Column("OKTMO", DbType.String, 11));
        }

        public override void Down()
        {
           
        }
    }
}