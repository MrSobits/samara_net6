namespace Bars.GkhGji.Controllers
{
    using Bars.GkhGji.Entities;

    public class DisposalInspFoundationCheckController : DisposalInspFoundationCheckController<DisposalInspFoundationCheck>
    {
    }


    public class DisposalInspFoundationCheckController<T> : B4.Alt.DataController<T>
        where T: DisposalInspFoundationCheck
    {
    }
}
