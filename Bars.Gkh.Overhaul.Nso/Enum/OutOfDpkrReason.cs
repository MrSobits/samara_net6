namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using System;

    [Flags]
    public enum OutOfDpkrReason
    {
        None = 0,
        NumberOfApartments = 0x1,
        PhysicalWearout = 0x2,
        MaxCost = 0x4,
        StructElementsWearout = 0x8
    }
}