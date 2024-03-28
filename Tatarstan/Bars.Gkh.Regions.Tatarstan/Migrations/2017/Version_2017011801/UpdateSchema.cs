namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017011801
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017011801
    /// </summary>
    [Migration("2017011801")]
    [MigrationDependsOn(typeof(_2016.Version_2016121500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_PLAN_PAYMENTS_PERCENTAGE",
                                          new RefColumn("PUBLIC_SERVICE_ORG", "PUBLIC_SERVICE_ORG__PERCEN", "GKH_PUBLIC_SERVORG", "ID"),
                                          new RefColumn("SERVICE", "SERVICE_PERCEN", "GIS_SERVICE_DICTIONARY", "ID"),
                                          new RefColumn("RESOURCE", "RESOURCE_PERCEN", "GKH_DICT_COMM_RESOURCE", "ID"),
                                          new Column("PERCENTAGE", DbType.Decimal, ColumnProperty.NotNull, 0m),
                                          new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                                          new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_PLAN_PAYMENTS_PERCENTAGE");
        }
    }
}