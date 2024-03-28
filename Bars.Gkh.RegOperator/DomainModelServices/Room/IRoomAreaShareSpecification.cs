namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;

    using Entities;
    using Gkh.Entities;

    public interface IRoomAreaShareSpecification
    {
        bool ValidateAreaShare(BasePersonalAccount account, Room room, decimal areaShare, DateTime? dateActual = null);
    }
}