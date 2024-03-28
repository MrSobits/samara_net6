namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
   
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PublishedProgram"</summary>
    public class PublishedProgramMap : BaseEntityMap<Entities.PublishedProgram>
    {
        
        public PublishedProgramMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.PublishedProgram", "OVRHL_PUBLISH_PRG")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
