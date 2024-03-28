namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017080400
{
    using System.Text;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017080400")]
    [MigrationDependsOn(typeof(Version_2017070400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            var sql = @"select 'alter table '||t.relname||' drop constraint '|| c.conname||';'
                  from pg_constraint c
                  join pg_class t on c.conrelid = t.oid
                  join pg_namespace n on t.relnamespace = n.oid
                  where t.relname like 'regop_pers_acc_charge%' and c.conname like '%packet_id%'; ";

            var sb = new StringBuilder();
            using (var reader = this.Database.ExecuteQuery(sql))
            {
                while (reader.Read())
                {
                    sb.AppendLine(reader.GetString(0));
                }
            }

            this.Database.ExecuteQuery(sb.ToString());
            this.Database.ExecuteQuery("alter table regop_unaccept_charge drop constraint if exists fk_unacc_ch_pac");
        }
    }
}