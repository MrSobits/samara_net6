namespace Bars.Gkh.Overhaul.Migration.Version_2018051700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018051700")]
    [MigrationDependsOn(typeof(Version_2017030900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_CREDIT_ORG", new Column("PRINTNAME", DbType.String, 300));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_CREDIT_ORG", "PRINTNAME");
        }
    }
}