using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Regions.Voronezh.Migrations._2019.Version_2019011700
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Regions.Voronezh.Helpers;

    [Migration("2019011700")]
    [MigrationDependsOn(typeof(_2019.Version_2019011400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            this.Database.AddEntityTable("CLW_LAWSUIT_OWNER_REPRESENTATIVE",
                new RefColumn("Rloi", "FK_LawOwnRep_RLOI_ID", "REGOP_LAWSUIT_OWNER_INFO", "ID"),
                new Column("RepresentativeType", DbType.Int32,0),
                new Column("Surname", DbType.String),
                new Column("FirstName", DbType.String),
                new Column("Patronymic", DbType.String),
                new Column("BirthDate", DbType.DateTime),
                new Column("BirthPlace", DbType.String),
                new Column("LivePlace", DbType.String),
                new Column("Note", DbType.String,1000));

        }

        public override void Down()
        {
            this.Database.RemoveTable("CLW_LAWSUIT_OWNER_REPRESENTATIVE");
        }
    }
}