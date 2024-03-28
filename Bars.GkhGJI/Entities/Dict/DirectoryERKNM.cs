namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочники ЕРКНМ
    /// </summary>
    public class DirectoryERKNM : BaseGkhEntity
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string CodeERKNM { get; set; }

        public virtual string EntityName { get; set; }
    }
}