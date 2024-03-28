namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Сервис для работы с ограничениями на выбор скорректированного года
    /// </summary>
    public class YearCorrectionConfigService : IYearCorrectionConfigService
    {
        private readonly string[] adminRoles = { "Администратор", "МСАЖКХ" };

        /// <summary>
        /// Домен-сервис <see cref="YearCorrection"/>
        /// </summary>
        public IDomainService<YearCorrection> YearCorrectionDomainService { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Метод проверяет доступность указанного года корректировки из единых настроек
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Результат проверки</returns>
        public IDataResult IsValidYear(int year)
        {
            var activeUser = this.UserManager.GetActiveUser();

            if (activeUser.Roles.Any(x => this.adminRoles.Contains(x.Role.Name)))
            {
                return new BaseDataResult();
            }

            if (this.YearCorrectionDomainService.GetAll().Count() == 0)
            {
                return new BaseDataResult();
            }

            if (this.YearCorrectionDomainService.GetAll().Any(x => x.Year == year))
            {
                return new BaseDataResult();
            }

            return BaseDataResult.Error("Введенное значение в поле скорректированный год является некорректным");
        }
    }
}