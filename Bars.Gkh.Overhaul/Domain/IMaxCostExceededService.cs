namespace Bars.Gkh.Overhaul.Domain
{
    using System.Linq;

    public interface IMaxCostExceededService
    {
        IQueryable<MaxCostExeededRealty> GetAll();
    }
}
