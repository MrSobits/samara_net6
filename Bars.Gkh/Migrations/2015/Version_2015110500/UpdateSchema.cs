namespace Bars.Gkh.Migration.Version_2015110500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015110500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015102602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накат миграции
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GKH_PERSON_DISQUAL", new Column("NAME_OF_COURT", DbType.String));
        }
    
        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON_DISQUAL", "NAME_OF_COURT");
        }
    }
}