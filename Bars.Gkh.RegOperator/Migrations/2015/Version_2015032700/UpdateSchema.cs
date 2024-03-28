namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015032700
{
    using System.Data;
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015032501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            try
            {
                bool firstSequenceExist = false;
                bool secondSequenceExist = false;

                using (var dr = GetReader("reg_op_suspen_account_id_seq"))
                {
                    if (dr.Read())
                    {
                        firstSequenceExist = dr[0].ToBool();
                    }
                }

                using (var dr = GetReader("regop_suspense_account_id_seq"))
                {
                    if (dr.Read())
                    {
                        secondSequenceExist = dr[0].ToBool();
                    }
                }

                if (firstSequenceExist && !secondSequenceExist)
                {
                    if (Database.DatabaseKind == DbmsKind.PostgreSql)
                    {
                        Database.ExecuteNonQuery("alter sequence reg_op_suspen_account_id_seq rename to regop_suspense_account_id_seq");
                    }
                }
            }
            catch
            {
                //на всякий случай
            }
        }

        public override void Down()
        {
        }

        private IDataReader GetReader(string sequenceName)
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                return Database.ExecuteQuery("SELECT 1 as name FROM pg_class where relname = '{0}'".FormatUsing(sequenceName));
            }

            return null;
        }

        #endregion
    }
}
