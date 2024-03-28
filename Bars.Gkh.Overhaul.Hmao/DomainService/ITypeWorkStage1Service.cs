namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using Entities;
    using Gkh.Entities.Dicts;
    using GkhCr.Entities;

    public interface ITypeWorkStage1Service
    {
        TypeWorkCrVersionStage1 GetTypeWorkStage1(TypeWorkCr typeWork);

        TypeWorkCrVersionStage1 GetTypeWorkStage1(Work work, ObjectCr objectCr);
    }
}