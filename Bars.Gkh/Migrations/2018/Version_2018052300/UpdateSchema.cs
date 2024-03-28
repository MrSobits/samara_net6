namespace Bars.Gkh.Migrations._2018.Version_2018052300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018052300")]
    [MigrationDependsOn(typeof(Version_2018051600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_DETERMINATION_RETURN", DbType.Boolean, ColumnProperty.None, false));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_DETERMINATION_RETURN", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_DETERMINATION_RENOUNCEMENT", DbType.Boolean, ColumnProperty.None, false));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_DETERMINATION_RENOUNCEMENT", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_JUDICAL_ORDER", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_DETERMINATION_CANCEL", DbType.Boolean, ColumnProperty.None, false));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_DETERMINATION_CANCEL", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("IS_DETERMINATION_TURN", DbType.Boolean, ColumnProperty.None, false));
            Database.AddColumn("CLW_LAWSUIT", new Column("DATE_DETERMINATION_TURN", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("FKR_AMMOUNT_COLLECTED", DbType.Decimal, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "IS_DETERMINATION_RETURN");
            Database.RemoveColumn("CLW_LAWSUIT", "DATE_DETERMINATION_RETURN");
            Database.RemoveColumn("CLW_LAWSUIT", "IS_DETERMINATION_RENOUNCEMENT");
            Database.RemoveColumn("CLW_LAWSUIT", "DATE_DETERMINATION_RENOUNCEMENT");
            Database.RemoveColumn("CLW_LAWSUIT", "DATE_JUDICAL_ORDER");
            Database.RemoveColumn("CLW_LAWSUIT", "IS_DETERMINATION_CANCEL");
            Database.RemoveColumn("CLW_LAWSUIT", "DATE_DETERMINATION_CANCEL");
            Database.RemoveColumn("CLW_LAWSUIT", "IS_DETERMINATION_TURN");
            Database.RemoveColumn("CLW_LAWSUIT", "DATE_DETERMINATION_TURN");
            Database.RemoveColumn("CLW_LAWSUIT", "FKR_AMMOUNT_COLLECTED");
        }
    }
}