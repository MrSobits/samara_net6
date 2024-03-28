namespace Bars.Gkh.Migrations._2017.Version_2017012502
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017012502
    /// </summary>
    [Migration("2017012502")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017011901.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {

            this.Database.AddEntityTable("GKH_DICT_MAN_CONTRACT_SERVICE", 
                new Column("CODE", DbType.String, 255, ColumnProperty.NotNull),
                new Column("SERVICE_TYPE", DbType.Int32, 255, ColumnProperty.NotNull),
                new RefColumn("UNIT_MEASURE_ID", "FK_MAN_CONTRACT_SERVICE_MEASURE", "GKH_DICT_UNITMEASURE", "ID"));

            this.Database.AddTable("GKH_DICT_ADD_CONTRACT_SERVICE", new RefColumn("ID", ColumnProperty.NotNull, "ADD_CONTRACT_SERVICE_ID", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID"));
            this.Database.AddForeignKey("FK_ADD_CONTRACT_SERVICE_ID", "GKH_DICT_ADD_CONTRACT_SERVICE", "ID", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID");
            this.Database.AddIndex("IND_ADD_CONTRACT_SERVICE_ID", true, "GKH_DICT_ADD_CONTRACT_SERVICE", "ID");
            this.Database.AddColumn("GKH_DICT_ADD_CONTRACT_SERVICE", "NAME", DbType.String, 255, ColumnProperty.NotNull);

            this.Database.AddTable("GKH_DICT_COM_CONTRACT_SERVICE", new RefColumn("ID", ColumnProperty.NotNull, "COM_CONTRACT_SERVICE_ID", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID"));
            this.Database.AddForeignKey("FK_COM_CONTRACT_SERVICE_ID", "GKH_DICT_COM_CONTRACT_SERVICE", "ID", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID");
            this.Database.AddIndex("IND_COM_CONTRACT_SERVICE_ID", true, "GKH_DICT_COM_CONTRACT_SERVICE", "ID");
            this.Database.AddColumn("GKH_DICT_COM_CONTRACT_SERVICE", "COM_RESOURCE", DbType.Int32, ColumnProperty.NotNull);
            this.Database.AddColumn("GKH_DICT_COM_CONTRACT_SERVICE", "SORT_ORDER", DbType.Int32, ColumnProperty.Null);
            this.Database.AddColumn("GKH_DICT_COM_CONTRACT_SERVICE", "IS_HOUSE_NEEDS", DbType.Boolean, ColumnProperty.NotNull, false);
            this.Database.AddColumn("GKH_DICT_COM_CONTRACT_SERVICE", "NAME", DbType.Int32, ColumnProperty.NotNull);

            this.Database.AddTable("GKH_DICT_AGR_CONTRACT_SERVICE", new RefColumn("ID", ColumnProperty.NotNull, "AGR_CONTRACT_SERVICE_ID", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID"));
            this.Database.AddForeignKey("FK_AGR_CONTRACT_SERVICE_ID", "GKH_DICT_AGR_CONTRACT_SERVICE", "ID", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID");
            this.Database.AddIndex("IND_AGR_CONTRACT_SERVICE_ID", true, "GKH_DICT_AGR_CONTRACT_SERVICE", "ID");
            this.Database.AddColumn("GKH_DICT_AGR_CONTRACT_SERVICE", "NAME", DbType.Int32, ColumnProperty.NotNull);
            this.Database.AddColumn("GKH_DICT_AGR_CONTRACT_SERVICE", "WORK_ASSIGN", DbType.Int32, ColumnProperty.NotNull);
            this.Database.AddColumn("GKH_DICT_AGR_CONTRACT_SERVICE", "TYPE_WORK", DbType.Int32, ColumnProperty.NotNull);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_AGR_CONTRACT_SERVICE");
            this.Database.RemoveTable("GKH_DICT_COM_CONTRACT_SERVICE");
            this.Database.RemoveTable("GKH_DICT_ADD_CONTRACT_SERVICE");
            this.Database.RemoveTable("GKH_DICT_MAN_CONTRACT_SERVICE");
        }
    }
}
