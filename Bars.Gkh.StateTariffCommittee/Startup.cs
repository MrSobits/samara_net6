namespace Bars.Gkh.StateTariffCommittee
{
    using Bars.Gkh.BaseApiIntegration.Startup;

    /// <summary>
    /// Конфигурация WebApi приложения
    /// </summary>
    public class Startup : AssemblyApiStartup
    {
        /// <inheritdoc />
        protected override string ApiPrefix => "stateTariffCommittee";
    }
}