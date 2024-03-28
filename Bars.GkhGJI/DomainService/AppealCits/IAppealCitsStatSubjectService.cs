namespace Bars.GkhGji.DomainService
{
    using System.Collections;

    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиа взаимодействия с тематиками обращений
    /// </summary>
    public interface IAppealCitsStatSubjectService
    {
        /// <summary>
        /// Добавить тематику
        /// </summary>
        IDataResult AddStatementSubject(BaseParams baseParams);
    }
}