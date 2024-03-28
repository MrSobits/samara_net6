namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    using NHibernate.Util;

    [Migration("2021121600")]
    [MigrationDependsOn(typeof(Version_2021121400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName()
        {
            Name = "GJI_WARNING_DOC"
        };

        private readonly Column[] columns = new[]
        {
            new Column("OBJECTION_RECEIVED", DbType.Int32, ColumnProperty.NotNull, (int) YesNo.No),
            new RefColumn("COMPILATION_PLACE_ID", "GJI_WARNING_DOC_COMPILATION_PLACE", "B4_FIAS_ADDRESS", "ID")
        };

        public override void Up()
        {
            this.columns.ForEach(column =>
            {
                if (column is RefColumn refColumn)
                {
                    this.Database.AddRefColumn(this.table, refColumn);
                }
                else
                {
                    this.Database.AddColumn(this.table, column);
                }
            });
        }
        
        public override void Down()
        {
            this.columns.ForEach(column => this.Database.RemoveColumn(this.table, column.Name));
        }
    }
}
