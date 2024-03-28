using System.Data;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062001
{
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_PAYMENT_DOC_INFO",
                new Column("INFORMATION", DbType.String, 4000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.ChangeColumn("REGOP_PAYMENT_DOC_INFO",
                new Column("INFORMATION", DbType.String, 2000, ColumnProperty.Null));
        }
    }
}
