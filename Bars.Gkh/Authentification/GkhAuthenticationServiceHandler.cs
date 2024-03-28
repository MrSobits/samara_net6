namespace Bars.Gkh.Authentification
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;
    using NHibernate.Exceptions;
    using System.Linq;

    /// <summary>
    /// Обработчик событий сервиса аутентификации
    /// </summary>
    public class GkhAuthenticationServiceHandler : IAuthenticationServiceHandler
    {
        /// <summary>
        /// Репозиторий операторов
        /// </summary>
        public IRepository<Operator> OperatorRepository { get; set; }

        /// <summary>
        /// При успешной аутентификации
        /// </summary>
        /// <param name="authenticationResult">Результат аутентификации</param>
        /// <returns>Текст ошибки</returns>
        public string OnAuthenticateSuccess(AuthenticationResult authenticationResult)
        {
            Operator operatorGkh = null;
            try
            {
                operatorGkh = this.OperatorRepository.GetAll().FirstOrDefault(x => x.User.Id == authenticationResult.UserId);
            }
            catch (GenericADOException)
            {
            }

            return operatorGkh != null && !operatorGkh.IsActive
                ? string.Format("Оператор {0} не активен", operatorGkh.Name)
                : string.Empty;
        }

        /// <summary>
        /// При ошибке аутентификации
        /// </summary>
        /// <param name="authenticationResult">Результат аутентификации</param>
        public void OnAuthenticateError(AuthenticationResult authenticationResult)
        {
        }

        /// <summary>
        /// При выходе из системы
        /// </summary>
        /// <param name="userIdentity">Пользователь</param>
        public void OnLogout(IUserIdentity userIdentity)
        {
        }
    }
}
