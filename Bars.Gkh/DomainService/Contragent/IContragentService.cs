namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Интерфейс Сервиса для Контрагент
    /// </summary>
    public interface IContragentService
    {
        /// <summary>
        /// Получить список без потомков
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult ListExceptChildren(BaseParams baseParams);

        /// <summary>
        /// Получить список контрагентов для специального счета
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult ListForSpecialAccount(BaseParams baseParams);

        /// <summary>
        /// Получение МО, которые еще не привязаны к контрагенту
        /// </summary>
        IDataResult ListAvailableMunicipality(BaseParams baseParams);

        /// <summary>
        /// Получаем выборку контрагентов по типу Исполнителя
        /// </summary>
        /// <param name="type">Тип исполнителя это код в справочнике</param>
        /// <returns>Модифицированный запрос</returns>
        IQueryable<Contragent> ListForTypeExecutant(int type);

        /// <summary>
        /// Получаем выборку контрагентов по типу Юр лица
        /// </summary>
        /// <param name="type">Тип юр лица это в ГЖИ поэтому пока непривязваюсь к Enum потому что эт онад оставить ссылку либо переносить енум в модуль Gkh</param>
        /// <returns>Модифицированный запрос</returns>
        IQueryable<Contragent> ListForTypeJurOrg(int type);

        /// <summary>
        /// Получаем выборку контрагентов по типу Поставщиков услуг
        /// </summary>
        /// <param name="type">Тип поставщика услуг </param>
        /// <returns>Модифицированный запрос</returns>
        IQueryable<Contragent> ListForTypeServOrg(int type);

        /// <summary>
        /// Получить тип юридического лица
        /// </summary>
        /// <param name="contragent">Контрагент</param>
        /// <returns>Тип юридического лица</returns>
        TypeJurPerson GetTypeJurPerson(Contragent contragent);

        /// <summary>
        /// Получить контакты действующих руководителей для контрагентов
        /// </summary>
        /// <param name="contragentIds">Идентификаторы контрагентов</param>
        /// <returns>Словарь контактов действующих руководителей</returns>
        IDictionary<long, ContragentContact> GetActualManagerContacts(params long[] contragentIds);

        /// <summary>
        /// Получить контакт действующего руководителя для контрагента
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult GetActualManagerContact(BaseParams baseParams);

        IDataResult GetContactsFromDL(BaseParams baseParams, Int64 contragentId);

        IDataResult UpdateContactsFromDL(BaseParams baseParams, Int64 contragentId);

        /// <summary>
        /// Вернуть сгенерированный код поставщика
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Код поставщика</returns>
        IDataResult GenerateProviderCode(BaseParams baseParams);

        /// <summary>
        /// Вернуть всех активных контрагентов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        IDataResult GetAllActiveContragent(BaseParams baseParams);

        /// <summary>
        /// Получить дополнительные роли контагента
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        IDataResult ListAdditionRole(BaseParams baseParams);
    }
}