/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.ShortTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности Опубликованная программа
///     /// </summary>
///     public class PublishedProgramMap : BaseEntityMap<PublishedProgram>
///     {
///         public PublishedProgramMap()
///             : base("OVRHL_PUBLISH_PRG")
///         {
///             Map(x => x.EcpSigned, "ECP_SIGNED", true, false);
///             Map(x => x.SignDate, "SIGN_DATE");
///             Map(x => x.PublishDate, "PUBLISH_DATE");
/// 
///             References(x => x.ProgramVersion, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.FileSign, "FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.FileXml, "FILE_XML_ID", ReferenceMapConfig.Fetch);
///             References(x => x.FilePdf, "FILE_PDF_ID", ReferenceMapConfig.Fetch);
///             References(x => x.FileCertificate, "FILE_CER_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.PublishedProgram"</summary>
    public class PublishedProgramMap : BaseEntityMap<PublishedProgram>
    {
        
        public PublishedProgramMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.PublishedProgram", "OVRHL_PUBLISH_PRG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            Property(x => x.EcpSigned, "EcpSigned").Column("ECP_SIGNED").DefaultValue(false).NotNull();
            Reference(x => x.FileSign, "FileSign").Column("FILE_ID").Fetch();
            Reference(x => x.FileXml, "FileXml").Column("FILE_XML_ID").Fetch();
            Reference(x => x.FilePdf, "FilePdf").Column("FILE_PDF_ID").Fetch();
            Reference(x => x.FileCertificate, "FileCertificate").Column("FILE_CER_ID").Fetch();
            Property(x => x.SignDate, "SignDate").Column("SIGN_DATE");
            Property(x => x.PublishDate, "PublishDate").Column("PUBLISH_DATE");
        }
    }
}
