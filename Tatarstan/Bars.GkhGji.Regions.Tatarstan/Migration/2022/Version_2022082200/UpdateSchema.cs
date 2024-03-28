namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022082200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.GkhGji.Enums;

    [MigrationDependsOn(typeof(Version_2022071300.UpdateSchema))]
    [Migration("2022082200")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var tableNames = new[]
            {
                "gji_disposal",
                "gji_tat_disposal",
                "gji_inspection_jurperson",
                "gji_inspection_statement",
                "gji_inspection_disphead",
                "gji_inspection_prosclaim",
            };
            
            tableNames.ForEach(x => this.Database.ExecuteNonQuery($@"
                do  $$
					BEGIN
					ALTER TABLE {x}
					ADD CONSTRAINT {x}_pkey PRIMARY KEY (id);
					EXCEPTION WHEN OTHERS THEN NULL;
				end $$;
            "));
        }
    }
}