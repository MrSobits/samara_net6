namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017060600
{
    using System.Text;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017060600")]
    [MigrationDependsOn(typeof(_2017.Version_2017052500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.ExecuteNonQuery("delete from regop_unaccept_charge");

            StringBuilder commands = new StringBuilder();

            using (var reader = this.Database.ExecuteReader(
                "select 'ALTER TABLE regop_pers_acc_charge_period_'||id||' drop CONSTRAINT IF EXISTS fk_regop_pers_acc_charge_'||id||'_packet_id;'"
                + "from regop_period"))
            {
                while (reader.Read())
                {
                    commands.AppendLine(reader[0].ToString());
                }
            }

            using (var reader = this.Database.ExecuteReader(
                "select 'ALTER TABLE public.regop_pers_acc_charge_period_'||id ||'" +
                " ADD CONSTRAINT fk_regop_pers_acc_charge_' || id || '_packet_id FOREIGN KEY (packet_id)" +
                "REFERENCES public.regop_unaccept_c_packet(id)" +
                "ON DELETE SET NULL;'" +
                "from regop_period"))
            {
                while (reader.Read())
                {
                    commands.AppendLine(reader[0].ToString());
                }
            }

            this.Database.ExecuteNonQuery(commands.ToString());
        }
    }
}