namespace Bars.GkhGji.Regions.Tatarstan.Integration
{
    using B4;

    public interface IGisGmpIntegration
    {
        IDataResult UploadCharges();

        IDataResult LoadPayments();
    }
}