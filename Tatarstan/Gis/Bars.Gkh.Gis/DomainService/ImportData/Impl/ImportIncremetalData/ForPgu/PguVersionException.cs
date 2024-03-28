namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.ForPgu
{
    using System;
    class PguVersionException : Exception
    {
        public PguVersionException(string message)
            : base(message)
        { }
    }
}
