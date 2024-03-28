namespace Bars.GkhCr
{
    using Bars.B4.IoC;
    using Bars.GkhCr.Repositories.PerformedWorkActPayments;
    using Bars.GkhCr.Repositories.PerformedWorkActs;

    public partial class Module
    {
        void RegisterDDD()
        {
            RegisterRepositories();
        }

        private void RegisterRepositories()
        {
            Container.RegisterTransient<IPerformedWorkActRepository, PerformedWorkActRepository>();
            Container.RegisterTransient<IPerformedWorkActPaymentRepository, PerformedWorkActPaymentRepository>();
        }
    }
}
