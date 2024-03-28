namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Entities;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// ущность WorkPrice расширяется новым полем ТипДома
    /// </summary>
    public class HmaoWorkPrice : WorkPrice
    {
        public virtual RealEstateType RealEstateType { get; set; }
    }
}
