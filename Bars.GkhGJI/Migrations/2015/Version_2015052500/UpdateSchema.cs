namespace Bars.GkhGji.Migrations._2015.Version_2015052500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    //перед применением выполнить RemoveViolationsWithDeletedRealtyObjAction
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2015041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			//в общем FK_GJI_INSPECT_VIO_OBJ и FK_GJI_INSPECTION_VIOL_GKH_RO по сути одно и тоже, хз зачем так сделали
	        if (Database.ConstraintExists("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECT_VIO_OBJ"))
	        {
				Database.RemoveConstraint("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECT_VIO_OBJ");
	        }

			Database.AddForeignKey("FK_GJI_INSPECTION_VIOL_GKH_RO", "GJI_INSPECTION_VIOLATION", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECTION_VIOL_GKH_RO");
			if (!Database.ConstraintExists("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECT_VIO_OBJ"))
			{
				Database.AddForeignKey("FK_GJI_INSPECT_VIO_OBJ", "GJI_INSPECTION_VIOLATION", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
			}
        }
    }
}
