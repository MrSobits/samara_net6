namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Мигрирование типа документа в "Документы подрядных организаций" на справочник
    /// </summary>
    public class MigrateBuilderDocumentType : BaseExecutionAction
    {
        private readonly IDomainService<BuilderDocumentType> documentTypeDomainService;
        private readonly ISessionProvider sessionProvider;

        private readonly IDictionary<int, string> baseDocumentTypes;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="documentTypeDomainService">Домен-сервис <see cref="BuilderDocumentType" /></param>
        /// <param name="sessionProvider">Провайдер сессии NHibernate</param>
        public MigrateBuilderDocumentType(IDomainService<BuilderDocumentType> documentTypeDomainService, ISessionProvider sessionProvider)
        {
            this.documentTypeDomainService = documentTypeDomainService;
            this.sessionProvider = sessionProvider;

            this.baseDocumentTypes = new Dictionary<int, string>
            {
                {10, "Отсутствие задолженности по налогам"},
                {20, "Отсутствие кредиторской задолженности"},
                {30, "Отсутствие предписаний ГЖИ"},
                {40, "Непроведение ликвидации/неприостановление деятельности"},
                {50, "Сертификат соответствия стандартам качества"},
                {60, "Учредительные документы"},
                {70, "Свидетельство о постановке на учет в налоговый орган"},
                {80, "Выписка из ЕГР юр.лиц/ИП"},
                {90, "Документ, подтверждающий УСН"},
                {100, "Разрешение на проведение работ по КР"},
                {110, "Справка из банка об отсутствии картотеки на счете"},
                {120, "Членство в общественных и профессиональных организациях"}
            };
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
            =>
                "Мигрирование типа документа в \"Документы подрядных организаций\" на справочник по задаче [GKH-7038]. Создаёт базовое состояние справочника. Добавляет основные типы документов"
            ;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Мигрирование типа документа в \"Документы подрядных организаций\" на справочник.";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var keys = this.baseDocumentTypes.Keys;

            try
            {
                //если действие прошло с ошибкой, то сначала забираем старые и их уже не трогаем
                var existTypes = this.documentTypeDomainService.GetAll().Select(x => x.Code).ToList();

                foreach (var key in keys.Where(x => !existTypes.Contains(x)))
                {
                    this.documentTypeDomainService.SaveOrUpdate(
                        new BuilderDocumentType
                        {
                            Code = key,
                            Name = this.baseDocumentTypes[key]
                        });
                }
            }
            catch (Exception ex)
            {
                return BaseDataResult.Error(ex.Message);
            }

            using (var statelessSession = this.sessionProvider.OpenStatelessSession())
            {
                using (var tr = statelessSession.BeginTransaction())
                {
                    var query = @"update GKH_BUILDER_DOCUMENT d set DOCUMENT_TYPE_ID = 
                                (select dt.ID from GKH_DICT_BUILDER_DOCUMENT_TYPE dt where dt.Code = d.DOCUMENT_TYPE)
                            where d.DOCUMENT_TYPE_ID is null";

                    try
                    {
                        var count = statelessSession.CreateSQLQuery(query).ExecuteUpdate();
                        tr.Commit();
                        return new BaseDataResult(true, string.Format("Обновлено записей: {0}", count));
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        return BaseDataResult.Error(ex.Message);
                    }
                }
            }
        }
    }
}