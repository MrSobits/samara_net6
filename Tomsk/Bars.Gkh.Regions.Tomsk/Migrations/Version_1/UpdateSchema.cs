namespace Bars.Gkh.Regions.Tomsk.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Томск оператор
            Database.AddTable(
                "GKH_TOMSK_OPERATOR",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("SHOW_UNASSIGNED", DbType.Boolean,ColumnProperty.NotNull, false));
            Database.AddForeignKey("FK_GKH_TOMSK_OPERATOR_OP", "GKH_TOMSK_OPERATOR", "ID", "GKH_OPERATOR", "ID");

            Database.ExecuteNonQuery(@"insert into GKH_TOMSK_OPERATOR (id)
                                     select id from GKH_OPERATOR");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_TOMSK_OPERATOR", "FK_GKH_TOMSK_OPERATOR_OP");
            Database.RemoveTable("GKH_TOMSK_OPERATOR");
        }
    }
}