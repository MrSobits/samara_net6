namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис массового изменения сальдо
    /// </summary>
    public interface IAccountSaldoChangeService
    {
        /// <summary>
        /// Обработать импортированные изменения
        /// </summary>
        /// <param name="saldoChangeCreator">
        /// Инициатор изменения сальдо
        /// </param>
        /// <param name="changeData">
        /// Список изменений сальдо
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult ProcessSaldoChange(ISaldoChangeCreator saldoChangeCreator, IList<ISaldoChangeData> changeData);

        /// <summary>
        /// Обработать импортированные изменения
        /// </summary>
        /// <param name="baseParams">
        /// Параметры запроса
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult ProcessSaldoChange(BaseParams baseParams);

        /// <summary>
        /// Добавить обработку прогресса
        /// </summary>
        /// <param name="progressIndicatorMethod">
        /// Метод индикации процесса обработки
        /// </param>
        void SetProgressIndicator(Action<int, string> progressIndicatorMethod);

        /// <summary>
        /// Изменение баланса за период
        /// </summary>
        /// <param name="account">
        /// Лицевой счет
        /// </param>
        /// <param name="newValue">
        /// Новое значение баланса
        /// </param>
        /// <param name="document">
        /// Документ-основание
        /// </param>
        /// <param name="reason">
        /// Причина
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult ProcessSaldoChange(BasePersonalAccount account, decimal newValue, FileData document, string reason);

        /// <summary>
        /// Вернуть информацию о лицевых счетах
        /// </summary>
        /// <typeparam name="T">Тип данных об лс</typeparam>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        IList<T> GetPersonalAccounts<T>(BaseParams baseParams) where T : PersonalAccountSaldoInfo, new();
    }

    /// <summary>
    /// Интерфейс выгрузки массового сальдо для последующего импорта изменений через интерфейс <see cref="IAccountSaldoChangeService"/>
    /// </summary>
    public interface IAccountMassSaldoExportService
    {
        /// <summary>
        /// Экспортировать сальдо ЛС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Поток</returns>
        ReportResult ExportSaldo(BaseParams baseParams);
    }

    /// <summary>
    /// Интерфейс изменений по сальдо, содержит новые значения по сальдо
    /// </summary>
    public interface ISaldoChangeData
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        BasePersonalAccount PersonalAccount { get; }

        /// <summary>
        /// Сальдо по базовому тарифу
        /// </summary>
        decimal SaldoByBaseTariff { get; }

        /// <summary>
        /// Сальдо по тарифу решения
        /// </summary>
        decimal SaldoByDecisionTariff { get; }

        /// <summary>
        /// Сальдо по пени
        /// </summary>
        decimal SaldoByPenalty { get; }

        /// <summary>
        /// Новое сальдо по базовому тарифу
        /// </summary>
        decimal NewSaldoByBaseTariff { get; }

        /// <summary>
        /// Новое сальдо по тарифу решения
        /// </summary>
        decimal NewSaldoByDecisionTariff { get; }

        /// <summary>
        /// Новое сальдо по пени
        /// </summary>
        decimal NewSaldoByPenalty { get; }
    }

    /// <summary>
    /// Инициатор изменения сальдо
    /// </summary>
    public interface ISaldoChangeCreator
    {
        /// <summary>
        /// Метод инициализирует сущность-инициатор изменение сальдо
        /// </summary>
        SaldoChangeSource CreateSaldoChangeOperation(string userName);
    }

    /// <summary>
    /// Информация об изменении сальдо ЛС
    /// </summary>
    public class SaldoChangeData : PersonalAccountSaldoInfo, ISaldoChangeData
    {
        private decimal? newSaldoByBaseTariff;
        private decimal? newSaldoByDecisionTariff;
        private decimal? newSaldoByPenalty;

        /// <summary>
        /// Лицевой счет
        /// </summary>
        [JsonIgnore]
        public BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Новое сальдо по базовому тарифу
        /// </summary>
        public decimal NewSaldoByBaseTariff
        {
            get
            {
                return this.newSaldoByBaseTariff ?? this.SaldoByBaseTariff;
            }
            set
            {
                this.newSaldoByBaseTariff = value;
            }
        }

        /// <summary>
        /// Новое сальдо по тарифу решения
        /// </summary>
        public decimal NewSaldoByDecisionTariff
        {
            get
            {
                return this.newSaldoByDecisionTariff ?? this.SaldoByDecisionTariff;
            }
            set
            {
                this.newSaldoByDecisionTariff = value;
            }
        }

        /// <summary>
        /// Новое сальдо по пени
        /// </summary>
        public decimal NewSaldoByPenalty
        {
            get
            {
                return this.newSaldoByPenalty ?? this.SaldoByPenalty;
            }
            set
            {
                this.newSaldoByPenalty = value;
            }
        }

        /// <summary>
        /// Сальдо
        /// </summary>
        public decimal Saldo => this.SaldoByBaseTariff + this.SaldoByDecisionTariff + this.NewSaldoByPenalty;

        /// <summary>
        /// Новое сальдо
        /// </summary>
        public decimal NewSaldo => this.NewSaldoByBaseTariff + this.NewSaldoByDecisionTariff + this.NewSaldoByPenalty;
    }
}