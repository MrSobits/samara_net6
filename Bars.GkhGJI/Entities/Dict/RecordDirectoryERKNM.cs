namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Запись справочники ЕРКНМ
    /// </summary>
    public class RecordDirectoryERKNM : BaseGkhEntity
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string IdentSMEV { get; set; }

        public virtual long? EntityId { get; set; }

        public virtual string CodeERKNM { get; set; }

        public virtual string IdentERKNM { get; set; }

        public virtual DirectoryERKNM DirectoryERKNM { get; set; }
    }
}