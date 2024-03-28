namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121301
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.Ecm7.Providers;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    
    [Migration("2021121301")]
    [MigrationDependsOn(typeof(Version_2021121300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiPreventiveActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT_PREVENTIVE_ACTION"};

        private readonly Column[] columns = {
            new Column("CONTROLLED_PERSON_TYPE", DbType.Int32),
            new Column("FULL_NAME", DbType.String.WithSize(255)),
            new Column("PHONE", DbType.String.WithSize(50)),
            new Column("FILE_NAME", DbType.String.WithSize(255)),
            new Column("FILE_NUMBER", DbType.String.WithSize(255)),
            new Column("FILE_DATE", DbType.Date),
            new RefColumn("FILE_ID", "GJI_DOCUMENT_PREVENTIVE_ACTION_B4_FILE_INFO", "B4_FILE_INFO", "ID"),
            new RefColumn("CONTROLLED_PERSON_ADDRESS_ID", "GJI_DOCUMENT_PREVENTIVE_ACTION_B4_FIAS_ADDRESS", "B4_FIAS_ADDRESS", "ID"),
            new RefColumn("HEAD_ID", "GJI_DOCUMENT_PREVENTIVE_ACTION_GKH_DICT_INSPECTOR", "GKH_DICT_INSPECTOR", "ID")
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RemoveColumn(gjiPreventiveActionTable, "ACTION_DATE");
            
            this.columns.ForEach(column =>
            {
                if (column is RefColumn refColumn)
                {
                    this.Database.AddRefColumn(gjiPreventiveActionTable, refColumn);
                }
                else
                {
                    this.Database.AddColumn(gjiPreventiveActionTable, column);
                }
            });
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.columns.ForEach(column =>
            {
                this.Database.RemoveColumn(this.gjiPreventiveActionTable, column.Name);
            });
        }
    }
}