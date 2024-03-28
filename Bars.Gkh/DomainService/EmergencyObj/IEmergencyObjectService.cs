namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using B4;
    using Entities;

    public interface IEmergencyObjectService
    {
        IQueryable<EmergencyObject> GetFilteredByOperator();
    }
}