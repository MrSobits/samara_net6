namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2017042000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017042000")]
    [MigrationDependsOn(typeof(Version_2017030900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("OVRHL_PRIOR_PARAM_QUANT", new Column("POINT", DbType.Decimal, (object)0m));
        }

        public override void Down()
        {
            this.Database.ChangeColumn("OVRHL_PRIOR_PARAM_QUANT", new Column("POINT", DbType.Int32, (object)0));
        }
    }
}