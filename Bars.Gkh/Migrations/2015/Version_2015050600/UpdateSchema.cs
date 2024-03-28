namespace Bars.Gkh.Migrations.Version_2015050600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015050500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.RemoveTable("clw_court_order_doc");
            Database.AddTable("clw_court_order_claim",
                new Column("id", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("claim_date", DbType.DateTime, ColumnProperty.NotNull),
                new Column("objection_arrived", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("document_id", ColumnProperty.Null, "clw_court_ord_doc", "b4_file_info", "id"));
            Database.AddTable("clw_petition", new Column("id", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique));

            Database.ExecuteNonQuery(
                "insert into clw_petition(id)"
                + " select id from clw_lawsuit");
        }

        public override void Down()
        {
            Database.RemoveTable("clw_petition");
            Database.RemoveTable("clw_court_order_claim");
            Database.AddEntityTable("clw_court_order_doc");
        }

        #endregion
    }
}