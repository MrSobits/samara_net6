namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015071500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015071500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015052901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddTable("GJI_DISPOSAL_PROVDOC_NSO",
		        new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
		        new Column("DOC_ORDER", DbType.Int16, ColumnProperty.NotNull, 0));

			this.Database.AddForeignKey("FK_DISP_PROVDOC_ID", "GJI_DISPOSAL_PROVDOC_NSO", "ID", "GJI_DISPOSAL_PROVDOC", "ID");

			this.Database.ExecuteNonQuery(@"
				insert into GJI_DISPOSAL_PROVDOC_NSO (id)
                select id from GJI_DISPOSAL_PROVDOC");

	        this.Database.ExecuteNonQuery(@"
				DO $$
				DECLARE
					disposal_curs CURSOR FOR 
						select d1.id, d2.disposal_id
						from GJI_DISPOSAL_PROVDOC_NSO d1
						join GJI_DISPOSAL_PROVDOC d2 on d1.id = d2.id
						order by d2.disposal_id, d1.id;
					id_curr int;
					disposal_id int;
					disposal_id_curr int;
					counter int := 0;
				BEGIN
					FOR disposal IN disposal_curs LOOP
					if (counter = 0)
						then select disposal.disposal_id INTO disposal_id;
					end if;
	
						select disposal.disposal_id INTO disposal_id_curr;
						select disposal.id INTO id_curr;
        
						if (disposal_id != disposal_id_curr)
						then 
						counter := 0; 
							select disposal.disposal_id INTO disposal_id;
						end if;
        
					update GJI_DISPOSAL_PROVDOC_NSO
					set doc_order = counter
					where id = id_curr;
	
					counter:=counter + 1;
					END LOOP;
				END$$;
			");
        }

        public override void Down()
        {
			this.Database.RemoveConstraint("GJI_DISPOSAL_PROVDOC_NSO", "FK_DISP_PROVDOC_ID");
			this.Database.RemoveTable("GJI_DISPOSAL_PROVDOC_NSO");
        }
    }
}