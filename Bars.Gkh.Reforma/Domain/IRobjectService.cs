namespace Bars.Gkh.Reforma.Domain
{
    using System;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.GkhDi.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    ///     Сервис жилых домов
    /// </summary>
    public interface IRobjectService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Ищет жилые дома по адресу Реформы
        /// </summary>
        /// <param name="address">Адрес Реформы</param>
        /// <returns>Жилые дома</returns>
        IQueryable<RealityObject> FindRobjects(FullAddress address);

        /// <summary>
        ///     Получает историю управления жилым домом
        /// </summary>
        /// <param name="robjectId">Идентификатор жилого дома</param>
        /// <returns>История управления</returns>
        ManagingHistory[] GetManagingHistory(long robjectId);

        /// <summary>
        ///     Получает информацию об обслуживаемых домах УО
        /// </summary>
        /// <param name="inn">ИНН УО</param>
        /// <returns>Информация об обслуживаемых домах</returns>
        RobjectManagement[] GetManagingRobjects(string inn);

        #endregion
    }

    /// <summary>
    ///     Информация об обслуживаемом доме
    /// </summary>
    public class RobjectManagement
    {
        #region Public Properties

        /// <summary>
        ///     Конец обслуживания
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        ///     Начало обслуживания
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        ///     Документ
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        ///     Идентификатор контракта
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     Плановая дата окончания обслуживания
        /// </summary>
        public DateTime? PlannedDateEnd { get; set; }

        /// <summary>
        ///     Идентификатор жилого дома
        /// </summary>
        public long RobjectId { get; set; }

        /// <summary>
        ///     Причина прекращения обслуживания
        /// </summary>
        public string TerminateReason { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Тип управления управляющей организации
        /// </summary>
        public TypeManagementManOrg TypeManagement { get; set; }

        /// <summary>
        /// Тип договора управляющей организации
        /// </summary>
        public TypeContractManOrg TypeContractManOrgRealObj { get; set; }

        public ContractStopReasonEnum ContractStopReason { get; set; }

        /// <summary>
        ///     Активен ли контракт на период (судя по датам начала конца)
        /// </summary>
        public bool GetIsManageable(PeriodDi period = null)
        {
            if (period == null)
            {
                return (!this.DateStart.HasValue || this.DateStart <= DateTime.Now) && (!this.DateEnd.HasValue || this.DateEnd > DateTime.Now);
            }

            var periodInExpression = DateTimeExpressionExtensions.CreatePeriodActiveInExpression<RobjectManagement>(
                 period.DateStart ?? DateTime.MinValue,
                 period.DateEnd ?? DateTime.MaxValue,
                 x => x.DateStart ?? DateTime.MinValue,
                 x => x.DateEnd ?? DateTime.MaxValue);

            return periodInExpression.Compile().Invoke(this);
        }

        #endregion
    }

    /// <summary>
    ///     История управления жилым домом
    /// </summary>
    public class ManagingHistory
    {
        #region Public Properties

        /// <summary>
        ///     Конец обслуживания
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        ///     Начало обслуживания
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        ///     Идентификатор контракта
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     Идентификатор УО
        /// </summary>
        public long ManOrgId { get; set; }

        /// <summary>
        ///     ИНН УО
        /// </summary>
        public string ManOrgInn { get; set; }

        /// <summary>
        ///     Плановая дата окончания обслуживания
        /// </summary>
        public DateTime? PlannedDateEnd { get; set; }

        /// <summary>
        ///     Причина прекращения обслуживания
        /// </summary>
        public string TerminateReason { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get; set; }

        private bool? isManageable;

        /// <summary>
        ///     Активен ли контракт (судя по датам начала конца)
        /// </summary>
        public bool IsManageable
        {
            get
            {
                return (this.isManageable ?? (this.isManageable = (!this.DateStart.HasValue || this.DateStart <= DateTime.Now) && (!this.DateEnd.HasValue || this.DateEnd > DateTime.Now))).Value;
            }
        }

        public TypeContractManOrg TypeManagement { get; set; }

        public string DocumentName { get; set; }

        public ManOrgContractOwnersFoundation ContractFoundation { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime? DocumentDate { get; set; }

        public ContractStopReasonEnum ContractStopReason { get; set; }

        public FileInfo DocumentFile { get; set; }

        #endregion
    }
}