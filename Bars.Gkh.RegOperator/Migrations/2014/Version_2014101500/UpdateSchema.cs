namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101500
{
    using System.Data;
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_TRANSFER", new Column("IS_COPY_FOR_SOURCE", DbType.Boolean, ColumnProperty.Null));

            var updateWitOrig = "update REGOP_TRANSFER set IS_COPY_FOR_SOURCE={0} where ORIGINATOR_ID is not null";
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(updateWitOrig.FormatUsing("TRUE"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(updateWitOrig.FormatUsing("t"));
            }

            var updateWhereNull = "update REGOP_TRANSFER set IS_COPY_FOR_SOURCE={0} where IS_COPY_FOR_SOURCE is null";
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(updateWhereNull.FormatUsing("FALSE"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(updateWhereNull.FormatUsing("f"));
            }

            Database.AlterColumnSetNullable("REGOP_TRANSFER", "IS_COPY_FOR_SOURCE", false);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_TRANSFER", "IS_COPY_FOR_SOURCE");
        }
    }
}
