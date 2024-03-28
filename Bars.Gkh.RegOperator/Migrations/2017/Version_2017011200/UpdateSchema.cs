namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017011200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using ForeignKeyConstraint = Bars.B4.Modules.Ecm7.Framework.ForeignKeyConstraint;

    /// <summary>
    /// Миграция RegOperator 2017011200
    /// </summary>
    [Migration("2017011200")]
    [MigrationDependsOn(typeof(_2016.Version_2016122000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC_CHARGE", new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull, true));

            // ON DELETE = SET NULL, т.к. пакеты удаляются при закрытии периода
            this.Database.AddColumn("REGOP_PERS_ACC_CHARGE", new Column("PACKET_ID", DbType.Int64, ColumnProperty.Null));
            this.Database.AddForeignKey(
                "REGOP_PERS_ACC_CHARGE_PACKET_ID", 
                "REGOP_PERS_ACC_CHARGE", 
                "PACKET_ID", 
                "REGOP_UNACCEPT_C_PACKET", 
                "ID", 
                ForeignKeyConstraint.SetNull);

            this.Database.AddForeignKeyToChildren("PACKET_ID", "REGOP_PERS_ACC_CHARGE", "PACKET_ID", "REGOP_UNACCEPT_C_PACKET", "ID", ForeignKeyConstraint.SetNull);
            this.Database.AddIndexToChildren("PACKET_ID", false, "REGOP_PERS_ACC_CHARGE", "PACKET_ID");

        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "IS_ACTIVE");
            this.Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "PACKET_ID");
        }
    }
}
