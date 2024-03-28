/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.Version
/// {
///     using System.Collections.Generic;
/// 
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.DataAccess.UserTypes;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class VersionRecordMap : BaseEntityMap<VersionRecord>
///     {
///         public VersionRecordMap()
///             : base("OVRHL_VERSION_REC")
///         {
///             References(x => x.ProgramVersion, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.CommonEstateObjects, "CEO_STRING", true);
///             Map(x => x.Point, "POINT", true, 0m);
///             Map(x => x.IndexNumber, "INDEX_NUM", true, 0);
///             Map(x => x.IsChangedYear, "IS_CHANGED_YEAR", false, false);
///             Map(x => x.DocumentName, "DOC_NAME");
///             Map(x => x.DocumentNum, "DOC_NUM");
///             Map(x => x.DocumentDate, "DOC_DATE");
/// 
///             Map(x => x.Changes, "CHANGES");
///             Map(x => x.IsAddedOnActualize, "IS_ADD_ACTUALIZE");
///             Map(x => x.IsChangedYearOnActualize, "IS_CH_YEAR_ACTUALIZE");
///             Map(x => x.IsChangeSumOnActualize, "IS_CH_SUM_ACTUALIZE");
/// 
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
/// 
///             Property(x => x.StoredCriteria,
///                      m =>
///                      {
///                          m.Type<ImprovedJsonSerializedType<List<StoredPriorityParam>>>();
///                          m.Column("CRITERIA");
///                      });
/// 
///             Property(x => x.StoredPointParams,
///                     m =>
///                     {
///                         m.Type<ImprovedJsonSerializedType<List<StoredPointParam>>>();
///                         m.Column("POINT_PARAMS");
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.VersionRecord"</summary>
    public class VersionRecordMap : BaseEntityMap<VersionRecord>
    {
        
        public VersionRecordMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.VersionRecord", "OVRHL_VERSION_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.CommonEstateObjects, "CommonEstateObjects").Column("CEO_STRING").Length(250).NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUM").NotNull();
            Property(x => x.Point, "Point").Column("POINT").DefaultValue(0m).NotNull();
            Property(x => x.IsChangedYear, "IsChangedYear").Column("IS_CHANGED_YEAR").DefaultValue(false);
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME").Length(250);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOC_NUM").Length(250);
            Property(x => x.DocumentDate, "DocumentDate").Column("DOC_DATE");
            Property(x => x.StoredCriteria, "StoredCriteria").Column("CRITERIA");
            Property(x => x.StoredPointParams, "StoredPointParams").Column("POINT_PARAMS");
            Property(x => x.IsAddedOnActualize, "IsAddedOnActualize").Column("IS_ADD_ACTUALIZE");
            Property(x => x.IsChangedYearOnActualize, "IsChangedYearOnActualize").Column("IS_CH_YEAR_ACTUALIZE");
            Property(x => x.IsChangeSumOnActualize, "IsChangeSumOnActualize").Column("IS_CH_SUM_ACTUALIZE");
            Property(x => x.Changes, "Changes").Column("CHANGES").Length(250);
        }
    }

    public class VersionRecordNHibernateMapping : ClassMapping<VersionRecord>
    {
        public VersionRecordNHibernateMapping()
        {
            Property(x => x.StoredCriteria,
                     m =>
                     {
                         m.Type<ImprovedJsonSerializedType<List<StoredPriorityParam>>>();
                     });

            Property(x => x.StoredPointParams,
                    m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<StoredPointParam>>>();
                    });
        }
    }
}
