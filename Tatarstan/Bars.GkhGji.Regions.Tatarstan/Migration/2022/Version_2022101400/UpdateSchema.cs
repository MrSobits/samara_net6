namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022101400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022101400")]
    [MigrationDependsOn(typeof(Version_2022101200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string KnmActionTable = "GJI_DICT_KNM_ACTION";
        private const string KnmActionKindActionTable = "GJI_DICT_KNM_ACTION_KIND_ACTION";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(KnmActionKindActionTable,
                new Column("KIND_ACTION", DbType.Int32),
                new RefColumn("KNM_ACTION_ID", ColumnProperty.NotNull, 
                    $"{KnmActionKindActionTable}_{KnmActionTable}", 
                    KnmActionTable, 
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(KnmActionKindActionTable);
        }
    }
}