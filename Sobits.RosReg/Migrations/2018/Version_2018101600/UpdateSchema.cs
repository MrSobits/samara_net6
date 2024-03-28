namespace Sobits.RosReg.Migrations._2018.Version_2018101600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2018101600")]
    [MigrationDependsOn(typeof(Version_2018090400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
            alter table rosreg.extractegrn
            add constraint fk_extractegrn_extract_id foreign key (extractid) references rosreg.extract (id) on delete cascade;

            alter table rosreg.extractegrn
            add constraint fk_extractegrn_room_id foreign key (roomid) references gkh_room (id) on delete set null ;

            alter table rosreg.extractegrnright
            add constraint fk_extractegrnright_extractegrn_id foreign key (egrnid) references rosreg.extractegrn (id) on delete cascade;

            alter table rosreg.extractegrnrightind
            add constraint fk_extractegrnrightind_extractegrnright_id foreign key (rightid) references rosreg.extractegrnright (id) on delete cascade;");
        }


        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"
            alter table rosreg.extractegrn drop constraint fk_extractegrn_extract_id;
            alter table rosreg.extractegrn drop constraint fk_extractegrn_room_id;
            alter table rosreg.extractegrnright drop constraint fk_extractegrnright_extractegrn_id;
            alter table rosreg.extractegrnrightind drop constraint fk_extractegrnrightind_extractegrnright_id;");
        }
    }
}