namespace Bars.Gkh.Overhaul.Domain
{
    using System;
    using Gkh.Entities;

    public interface IPaysizeRepository
    {
        decimal GetRoPaysize(RealityObject robject);

        decimal GetRoPaysize(RealityObject robject, DateTime date);

        decimal? GetRoPaysizeByType(RealityObject robject, DateTime date);

        decimal? GetRoPaysizeByMu(RealityObject robject, DateTime date);

        decimal? GetRoPaysizeByDecision(RealityObject robject, DateTime date);
    }
}