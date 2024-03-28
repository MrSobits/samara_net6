namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalInspFoundationCheckDomainService : DisposalInspFoundationCheckDomainService<DisposalInspFoundationCheck>
    {
    }

    public class DisposalInspFoundationCheckDomainService<T> : BaseDomainService<T>
    where T : DisposalInspFoundationCheck
    {
    }
}
