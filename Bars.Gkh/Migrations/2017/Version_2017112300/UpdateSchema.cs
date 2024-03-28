namespace Bars.Gkh.Migrations._2017.Version_2017112300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2017112300")]
    [MigrationDependsOn(typeof(Version_2017102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn("GKH_REALITY_OBJECT", new RefColumn("TECH_PASSPORT_FILE_ID", "GKH_REALITY_OBJECT_TPF", "B4_FILE_INFO", "ID"));

            this.Database.AddColumn("GKH_ROOM", new Column("IS_COMMUNAL", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_ROOM", new Column("COMMUNAL_AREA", DbType.Decimal, ColumnProperty.Null));
            this.Database.AddColumn("GKH_ROOM", new Column("PREV_ASS_REG_NUM", DbType.Decimal, ColumnProperty.Null));
            this.Database.AddColumn("GKH_ROOM", new Column("REC_UNFIT", DbType.Byte, ColumnProperty.NotNull, (byte)YesNo.No));
            this.Database.AddColumn("GKH_ROOM", new Column("REC_UNFIT_REASON", DbType.String, ColumnProperty.Null));
            this.Database.AddColumn("GKH_ROOM", new Column("REC_UNFIT_DOC_NUMBER", DbType.Int32, ColumnProperty.Null));
            this.Database.AddColumn("GKH_ROOM", new Column("REC_UNFIT_DOC_DATE", DbType.DateTime, ColumnProperty.Null));
            this.Database.AddRefColumn("GKH_ROOM", new RefColumn("REC_UNFIT_DOC_FILE_ID", "GKH_ROOM_RUDF", "B4_FILE_INFO", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "TECH_PASSPORT_FILE_ID");

            this.Database.RemoveColumn("GKH_ROOM", "IS_COMMUNAL");
            this.Database.RemoveColumn("GKH_ROOM", "COMMUNAL_AREA");
            this.Database.RemoveColumn("GKH_ROOM", "PREV_ASS_REG_NUM");
            this.Database.RemoveColumn("GKH_ROOM", "REC_UNFIT");
            this.Database.RemoveColumn("GKH_ROOM", "REC_UNFIT_REASON");
            this.Database.RemoveColumn("GKH_ROOM", "REC_UNFIT_DOC_NUMBER");
            this.Database.RemoveColumn("GKH_ROOM", "REC_UNFIT_DOC_DATE");
            this.Database.RemoveColumn("GKH_ROOM", "REC_UNFIT_DOC_FILE_ID");
        }
    }
}