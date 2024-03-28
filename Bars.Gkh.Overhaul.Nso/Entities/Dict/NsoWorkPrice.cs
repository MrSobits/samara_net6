namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.Gkh.Overhaul.Entities;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// Сущность WorkPrice расширяется новым полем ТипДома
    /// </summary>
    public class NsoWorkPrice : WorkPrice
    {
        public virtual RealEstateType RealEstateType { get; set; }
    }
}
