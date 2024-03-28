namespace Bars.Gkh.BaseApiIntegration.Services.Impl
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <inheritdoc />
    public class ApiLoginService : IApiLoginService
    {
        /// <summary>
        /// Оператор пользователя
        /// </summary>
        protected Operator UserOperator;

        private readonly IRepository<Operator> operatorRepository;

        /// <inheritdoc cref="ApiLoginService"/>
        public ApiLoginService(IRepository<Operator> operatorRepository)
        {
            this.operatorRepository = operatorRepository;
        }

        /// <inheritdoc />
        public virtual ValidateResult AuthorizeUser(long userId)
        {
            this.UserOperator = this.operatorRepository.GetAll()
                .SingleOrDefault(x => x.User.Id == userId);

            if (this.UserOperator == null)
            {
                return ValidateResult.No("У пользователя не найден оператор");
            }

            if (!this.UserOperator.IsActive)
            {
                return ValidateResult.No($"Пользователь {this.UserOperator.User.Login} заблокирован");
            }

            return ValidateResult.Yes();
        }
    }
}