namespace Bars.Gkh.Overhaul.Migration.Version_2014042300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014040400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.TableExists("GKH_CONTR_BANK_CR_ORG"))
            {
                if (Database.DatabaseKind == DbmsKind.Oracle)
                {
                    Database.RemoveIndex("IND_CONTR_BANK_CR_ORG_A", "GKH_CONTR_BANK_CR_ORG");
                }

                Database.RemoveTable("GKH_CONTR_BANK_CR_ORG");
            }

            //сия порнография связана с тем, что в методе AddJoinedSubclassTable колонка ID создается как PrimaryKey
            //и на нее дважды вешается индекс и падает в оракле
            Database.AddTable("GKH_CONTR_BANK_CR_ORG", new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique), new Column("CREDIT_ORG_ID", DbType.Int64), new Column("FILE_ID", DbType.Int64));

            Database.AddRefColumn("GKH_CONTR_BANK_CR_ORG", new RefColumn("CREDIT_ORG_ID", "CTR_BANK_CRO_CRO_A", "OVRHL_CREDIT_ORG", "ID"));
            Database.AddRefColumn("GKH_CONTR_BANK_CR_ORG", new RefColumn("FILE_ID", "GKH_CTR_BANK_CRO_FILE_A", "B4_FILE_INFO", "ID"));

            Database.AddForeignKey("FK_CTR_BANK_CRO_CRO_ID", "GKH_CONTR_BANK_CR_ORG", "ID", "GKH_CONTRAGENT_BANK", "ID");
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddIndex("IND_CTR_BANK_CRO_CRO_ID", true, "GKH_CONTR_BANK_CR_ORG", "ID");
            }

            Database.ExecuteNonQuery(@"insert into GKH_CONTR_BANK_CR_ORG (id)
                                     select id from GKH_CONTRAGENT_BANK");

            if (Database.TableExists("GKH_CONTRAGENT_BANK_NSO"))
            {
                Database.ExecuteNonQuery(@"insert into GKH_CONTR_BANK_CR_ORG (id, CREDIT_ORG_ID, FILE_ID)
                                     select id, CREDIT_ORG_ID,  FILE_ID from GKH_CONTRAGENT_BANK_NSO
                                     where id not in (select id from GKH_CONTR_BANK_CR_ORG )");

                Database.RemoveTable("GKH_CONTRAGENT_BANK_NSO");
            }

            if (Database.TableExists("GKH_CONTRAGENT_BANK_KAMCH"))
            {
                Database.ExecuteNonQuery(@"insert into GKH_CONTR_BANK_CR_ORG (id, CREDIT_ORG_ID, FILE_ID)
                                     select id, CREDIT_ORG_ID,  FILE_ID from GKH_CONTRAGENT_BANK_KAMCH
                                     where id not in (select id from GKH_CONTR_BANK_CR_ORG )");

                Database.RemoveTable("GKH_CONTRAGENT_BANK_KAMCH");
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTR_BANK_CR_ORG", "CREDIT_ORG_ID");
            Database.RemoveColumn("GKH_CONTR_BANK_CR_ORG", "FILE_ID");

            Database.RemoveConstraint("GKH_CONTR_BANK_CR_ORG", "FK_CTR_BANK_CRO_CRO_ID");
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.RemoveIndex("IND_CTR_BANK_CRO_CRO_ID", "GKH_CONTR_BANK_CR_ORG");
            }

            Database.RemoveTable("GKH_CONTR_BANK_CR_ORG");
        }
    }
}