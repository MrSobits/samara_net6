namespace Bars.Gkh.Entities.Dicts
{
    using System;

    public class FiasOktmo : BaseGkhEntity
    {
        public virtual Municipality Municipality { get; set; }

        public virtual String FiasGuid { get; set; }
    }
}
