namespace Bars.Gkh.Services.Impl.GlonassIntegration
{
    using System.Globalization;
    // using System.ServiceModel.Activation;
    using Castle.Windsor;
    using DataContracts;
    using ServiceContracts.GlonassIntegration;

    // TODO wcf
    /// <summary>
    /// Сервис интеграции с ГЛОНАСС 112
    /// </summary>
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class GlonassIntegService : IGlonassIntegService
    {
        protected IWindsorContainer Container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Io-контейнер</param>
        public GlonassIntegService(IWindsorContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Возвращает формат чисел текущей культуры
        /// </summary>
        public static NumberFormatInfo NumberFormatInfo
        {
            get
            {
                var cultureInfo = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                if (cultureInfo == null)
                {
                    return null;
                }
                cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

                return cultureInfo.NumberFormat;
            }
        }

        private Result IncorrectRequest()
        {
            return new Result
            {
                Code = "99",
                Name = "INCORRECT_REQUEST"
            };
        }
    }
}
