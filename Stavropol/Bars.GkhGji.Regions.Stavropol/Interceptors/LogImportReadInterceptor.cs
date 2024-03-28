namespace Bars.GkhGji.Regions.Stavropol.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерцептор чтения для <see cref="LogImport"/>
    /// </summary>
    // TODO: как только для Ставрополи выделится модуль GKH, перенести это туда
    public class LogImportReadInterceptor : IDomainServiceReadInterceptor<LogImport>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        /// <inheritdoc />
        public IQueryable<LogImport> BeforeGetAll(IQueryable<LogImport> query)
        {
            var currentOperator = this.UserManager.GetActiveOperator();

            // если текущего оператора нет (в случае, если он был создан под root'ом)
            // или оператор относится к роли администратор,
            // то не производим никакой фильтрации
            if (currentOperator.IsNull() || currentOperator.User.Roles.Any(x => x.Role.Name == "Администратор"))
            {
                return query;
            }

            // организации текущего оператора
            var contragentIds = this.UserManager.GetContragentIds();
            var contragentQuery = this.OperatorContragentDomain.GetAll().WhereContains(x => x.Contragent.Id, contragentIds);

            return query.Where(x => contragentQuery.Any(y => y.Operator.Id == x.Operator.Id));
        }
    }
}