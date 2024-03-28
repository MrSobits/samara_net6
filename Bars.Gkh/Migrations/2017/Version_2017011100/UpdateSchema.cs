namespace Bars.Gkh.Migrations._2017.Version_2017011100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017011100
    /// </summary>
    [Migration("2017011100")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016122300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_COMMON_ESTATE_OBJECT", new Column("REFORM_CODE", DbType.String, 10));
            this.Database.AddColumn("GKH_DICT_WORK", new Column("REFORM_CODE", DbType.String, 10));
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("UNPUBLISH_DATE", DbType.DateTime));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_COMMON_ESTATE_OBJECT", "REFORM_CODE");
            this.Database.RemoveColumn("GKH_DICT_WORK", "REFORM_CODE");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "UNPUBLISH_DATE");
        }
    }
}