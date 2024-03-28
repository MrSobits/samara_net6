namespace Bars.Gkh.Migrations._2017.Version_2017022000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Gkh.2017022000
    /// </summary>
    [Migration("2017022000")]
    [MigrationDependsOn(typeof(Version_2017021800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("TYPE", DbType.Int16));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("NOTE", DbType.String, 1000));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("APPLICANT", DbType.String));
            this.Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("TAX_SUM", DbType.Decimal));
            this.Database.AddRefColumn("GKH_MANORG_LIC_REQUEST", new RefColumn("MANORG_LICENSE_ID", ColumnProperty.Null, "LIC_REQUEST_LICENSE", "GKH_MANORG_LICENSE", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "TYPE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "NOTE");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "APPLICANT");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "TAX_SUM");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "MANORG_LICENSE_ID");
        }
    }
}