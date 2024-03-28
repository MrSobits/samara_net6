namespace Bars.GkhGji.Migrations._2024.Version_2024020500
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024020500")]
    [MigrationDependsOn(typeof(Version_2024020100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_REQUEST", new RefColumn("CONTRAGENT_ID", "GJI_APPCIT_REQUEST_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddColumn("GJI_APPCIT_REQUEST", new RefColumn("SIGNER_ID", "GJI_APPCIT_REQUEST_SIGNER", "GKH_DICT_INSPECTOR", "ID"));

            Database.ExecuteNonQuery(@"update GJI_APPCIT_REQUEST
                                       set CONTRAGENT_ID = selected.contId
                                       from (select req.ID reqId, cont.ID contId
                                             from GJI_APPCIT_REQUEST req
                                                 join GJI_DICT_COMPETENT_ORG comp on req.COMPETENT_ORG_ID = comp.ID
                                                 join GKH_CONTRAGENT cont on cont.INN = comp.CODE
                                             where comp.CODE != '') as selected
                                       where GJI_APPCIT_REQUEST.ID = selected.reqId");

            Database.RemoveColumn("GJI_APPCIT_REQUEST", "COMPETENT_ORG_ID");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_APPCIT_REQUEST", new RefColumn("COMPETENT_ORG_ID", "GJI_APPCIT_REQUEST_COMPETENT_ORG", "GJI_DICT_COMPETENT_ORG", "ID"));

            Database.ExecuteNonQuery(@"update GJI_APPCIT_REQUEST 
                                       set COMPETENT_ORG_ID = selected.compId
                                       from (select req.ID reqId, comp.ID compId
                                       	  from GJI_APPCIT_REQUEST req
                                       	      join GKH_CONTRAGENT cont on cont.ID = req.CONTRAGENT_ID
                                       	      join GJI_DICT_COMPETENT_ORG comp on comp.CODE = cont.INN
                                       	  where comp.CODE != '') as selected
                                       where GJI_APPCIT_REQUEST.ID = selected.reqId");

            Database.RemoveColumn("GJI_APPCIT_REQUEST", "SIGNER_ID");
            Database.RemoveColumn("GJI_APPCIT_REQUEST", "CONTRAGENT_ID");
        }
    }
}