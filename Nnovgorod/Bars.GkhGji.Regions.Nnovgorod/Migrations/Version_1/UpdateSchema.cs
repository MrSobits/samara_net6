﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_CHECK_TIME_CH",
                new Column("NEW_VALUE", DbType.DateTime, ColumnProperty.Null),
                new Column("OLD_VALUE", DbType.DateTime, ColumnProperty.Null),

                new RefColumn("USER_ID", ColumnProperty.Null, "GJI_CHTIME_USER", "B4_USER", "ID"),
                new RefColumn("APPEAL_ID", "GJI_CHTIME_APPEAL", "GJI_APPEAL_CITIZENS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CHECK_TIME_CH");
        }
    }
}