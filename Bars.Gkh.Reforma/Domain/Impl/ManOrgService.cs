namespace Bars.Gkh.Reforma.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.Application;

    using Gkh.Entities;
    using Entities;
    using Enums;
    using Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с заявками на раскрытие информации по УО
    /// </summary>
    public class ManOrgService : IManOrgService
    {
        #region Fields

        private readonly IWindsorContainer container;

        private Dictionary<string, long> manOrgIdByInnDict;

        private Dictionary<string, long> refManOrgIdByInnDict;

        private Dictionary<string, RequestStatus> requestStatusByInnDict;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public ManOrgService(IWindsorContainer container)
        {
            this.container = container;
            this.Init();
        }

        #endregion

        private static ManOrgService instance;

        private static readonly object LockObject = new object();

        public static ManOrgService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LockObject)
                    {
                        if (instance == null)
                        {
                            instance = ApplicationContext.Current.Container.Resolve<ManOrgService>();
                        }
                    }
                }

                return instance;
            }
        }

        #region Public Methods and Operators

        /// <summary>
        /// Получает список ИНН УО, по которым не создано заявок
        /// </summary>
        /// <returns>Список ИНН</returns>
        public string[] GetUnrequestedInns()
        {
            return this.manOrgIdByInnDict.Keys.Except(this.refManOrgIdByInnDict.Keys).ToArray();
        }

        /// <summary>
        /// Получает список ИНН УО, по которым заявки подтверждены
        /// </summary>
        /// <returns>Список ИНН</returns>
        public string[] GetSynchronizableInns()
        {
            return this.requestStatusByInnDict.Where(x => x.Value == RequestStatus.approved).Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Инициализация словарей
        /// </summary>
        public void Init()
        {
            var manOrgService = this.container.ResolveDomain<ManagingOrganization>();
            var refManOrgService = this.container.ResolveDomain<RefManagingOrganization>();
            try
            {
                this.manOrgIdByInnDict =
                    manOrgService.GetAll()
                        .Select(x => new { x.Id, x.Contragent.Inn, x.OrgStateRole, x.Contragent.ContragentState })
                        .Where(x => x.Inn != null) // x.Inn != string.Empty не поддерживается Oracle
                        .AsEnumerable()
                        .Where(x => Utils.VerifyInn(x.Inn, true)) // string.Empty отфильтруется на этом этапе
                        .GroupBy(x => x.Inn)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.OrgStateRole).ThenBy(y => y.ContragentState).First().Id);

                this.refManOrgIdByInnDict = refManOrgService.GetAll().Select(x => new { x.Id, x.Inn }).AsEnumerable().ToDictionary(x => x.Inn, x => x.Id);

                this.requestStatusByInnDict = refManOrgService.GetAll()
                    .Select(x => new { x.RequestStatus, x.Inn })
                    .AsEnumerable()
                    .ToDictionary(x => x.Inn, x => x.RequestStatus);
            }
            finally
            {
                this.container.Release(manOrgService);
                this.container.Release(refManOrgService);
            }
        }

        /// <summary>
        /// Обновляет статус заявки
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="requestDate">Дата запроса</param>
        /// <param name="processDate">Дата обработки</param>
        /// <param name="status">Статус</param>
        /// <returns>Успех. False - если УО с указанным ИНН не найдена</returns>
        public bool SetRequestState(string inn, DateTime requestDate, DateTime? processDate, RequestStatus status)
        {
            var service = this.container.ResolveDomain<RefManagingOrganization>();
            try
            {
                RefManagingOrganization entity;
                long refManOrgId;
                if (this.refManOrgIdByInnDict.TryGetValue(inn, out refManOrgId))
                {
                    if (this.requestStatusByInnDict[inn] == status)
                    {
                        return true;
                    }

                    entity = service.Get(refManOrgId);
                }
                else
                {
                    long manOrgId;
                    if (this.manOrgIdByInnDict.TryGetValue(inn, out manOrgId))
                    {
                        entity = new RefManagingOrganization { Inn = inn };
                    }
                    else
                    {
                        return false;
                    }
                }

                entity.RequestDate = requestDate;
                entity.ProcessDate = processDate;
                entity.RequestStatus = status;
                if (entity.Id > 0)
                {
                    service.Update(entity);
                    this.requestStatusByInnDict[inn] = status;
                }
                else
                {
                    service.Save(entity);
                    this.refManOrgIdByInnDict.Add(inn, entity.Id);
                    this.requestStatusByInnDict.Add(inn, status);
                }

                return true;
            }
            finally
            {
                this.container.Release(service);
            }
        }

        /// <summary>
        /// Получение УО по ИНН
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <returns>УО</returns>
        public ManagingOrganization GetManOrgByInn(string inn)
        {
            var id = this.manOrgIdByInnDict.Get(inn);
            var service = this.container.ResolveDomain<ManagingOrganization>();
            try
            {
                return service.Get(id);
            }
            finally
            {
                this.container.Release(service);
            }
        }

        /// <summary>
        /// Проверяет, является ли УО синхронизируемой
        /// </summary>
        /// <param name="organization">УО</param>
        /// <returns>Синхронизируема?</returns>
        public bool IsSynchronizable(ManagingOrganization organization)
        {
            var inn = organization.Contragent.Inn;
            if (inn == null)
            {
                return false;
            }

            var id = this.manOrgIdByInnDict.Get(inn);
            return organization.Id == id && this.requestStatusByInnDict.Get(inn) == RequestStatus.approved;
        }

        /// <summary>
        /// Получение Id синхронизируемой УО по ИНН
        /// </summary>
        /// <param name="inn">ИНН УО</param>
        /// <returns>Id синхронизируемой УО</returns>
        public long GetRefManOrgIdByInn(string inn)
        {
            return this.refManOrgIdByInnDict.Get(inn);
        }

        public RequestStatus GetRequestState(string inn)
        {
            return this.requestStatusByInnDict.Get(inn);
        }

        public static void Clear()
        {
            instance = null;
        }

        #endregion
    }
}