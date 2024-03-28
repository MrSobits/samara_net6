namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017092700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017092700")]
    [MigrationDependsOn(typeof(Version_2017091600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LOC_CODE",
                new Column("FIAS_NAME_L3", DbType.String, ColumnProperty.Null));

            this.Database.AddColumn("REGOP_PAYMENT_DOC_INFO",
                new Column("LOCALITY_AOGUID", DbType.String, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_PAYMENT_DOC_INFO",
                new Column("LOCALITY_NAME", DbType.String, ColumnProperty.Null));

            this.Database.ExecuteNonQuery(@"
UPDATE regop_loc_code c
SET aoguid = f.aoguid,
    fias_name_l3 = f.formalname
FROM b4_fias f
WHERE c.fias_id_l3 = f.id;

UPDATE regop_payment_doc_info i
SET locality_aoguid = f.aoguid,
    locality_name = f.formalname
FROM b4_fias f
WHERE i.locality_id = f.id;
            ");

            this.Database.RemoveColumn("REGOP_LOC_CODE", "FIAS_ID_L3");
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_INFO", "LOCALITY_ID");

            ViewManager.Drop(this.Database, "Regop");
            //ViewManager.Create(this.Database, "Regop");
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.AddColumn("REGOP_LOC_CODE",
                new RefColumn("FIAS_ID_L3", ColumnProperty.Null, "LOC_CODE_FIAS3", "B4_FIAS", "ID"));

            this.Database.AddColumn("REGOP_PAYMENT_DOC_INFO",
                new RefColumn("LOCALITY_ID", ColumnProperty.Null, "REGOP_PAYMENT_DOC_INFO_FIAS", "B4_FIAS", "ID"));

            this.Database.ExecuteNonQuery(@"
UPDATE regop_loc_code c
SET fias_id_l3 = f.id
FROM b4_fias f
WHERE c.aoguid = f.aoguid;

UPDATE regop_payment_doc_info i
SET locality_id = f.id
FROM b4_fias f
WHERE i.locality_aoguid = f.aoguid;
            ");

            this.Database.RemoveColumn("REGOP_LOC_CODE", "FIAS_NAME_L3");
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_INFO", "LOCALITY_AOGUID");
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_INFO", "LOCALITY_NAME");
        }
    }
}