namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    public class MunicipalityFiasOktmo : BaseImportableEntity
    {
        public virtual Municipality Municipality { get; set; }

        public virtual string FiasGuid { get; set; }

        public virtual string Oktmo { get; set; }
    }
}
