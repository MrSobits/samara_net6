namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;
    using Bars.Gkh.Modules.Gkh1468.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Договоры ресурсоснабжения
    /// </summary>
    public class DrsoExportableEntity : BaseExportableEntity<PublicServiceOrgContract>
    {
        /// <inheritdoc />
        public override string Code => "DRSO";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <summary>
        /// Репозиторий <see cref="BaseContractPart" />
        /// </summary>
        public IRepository<BaseContractPart> BaseContractPartRepository { get; set; }

        /// <summary>
        /// Репозиторий <see cref="RsoAndServicePerformerContract" />
        /// </summary>
        public IRepository<RsoAndServicePerformerContract> RsoAndServicePerformerContractRepository { get; set; }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var contragentDict = this.FilterService
                .FilterByContragent(this.BaseContractPartRepository.GetAll(),
                    x => x.PublicServiceOrgContract.PublicServiceOrg.Contragent)
                .Select(x => new
                {
                    x.PublicServiceOrgContract.Id,
                    ContragentId = (long?) x.PublicServiceOrgContract.PublicServiceOrg.Contragent.ExportId,
                })
                .ToDictionary(x => x.Id, x => x.ContragentId);

            var commercialMeteringResourceTypeDict = this.FilterService
                .FilterByContragent(this.RsoAndServicePerformerContractRepository.GetAll(),
                    x => x.PublicServiceOrgContract.PublicServiceOrg.Contragent)
                .Select(x => new
                {
                    x.PublicServiceOrgContract.Id,
                    CommercialMeteringResourceType = (CommercialMeteringResourceType?) x.CommercialMeteringResourceType,
                })
                .ToDictionary(x => x.Id, x => x.CommercialMeteringResourceType);

            return this.GetFiltred(x => x.PublicServiceOrg.Contragent)
                .Select(x => new
                {
                    x.Id,
                    ContragentId = x.PublicServiceOrg.Contragent.ExportId,
                    x.ContractNumber,
                    x.ContractDate,
                    x.DateStart,
                    x.DateEnd,
                    x.TermBillingPaymentNoLaterThan,
                    x.TermPaymentNoLaterThan,
                    x.DeadlineInformationOfDebt,
                    x.DayStart,
                    x.DayEnd,
                    x.StartDeviceMetteringIndication,
                    x.ResOrgReason,
                    x.StopReason,
                    x.DateStop
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.ContragentId.ToStr(), // 2. Уникальный идентификатор РСО
                        this.No, // 3. Договор является публичным
                        x.ContractNumber.Cut(255), // 4. Номер документа
                        this.GetDate(x.ContractDate), // 5. Дата заключения
                        this.GetDate(x.DateStart), // 6. Дата вступления в силу
                        this.GetDate(x.DateEnd), // 7.  Дата окончания действия
                        x.TermBillingPaymentNoLaterThan.ToStr(), // 8. Срок выставления счетов к оплате, не позднее
                        x.TermPaymentNoLaterThan.ToStr(), // 9. Срок оплаты, не позднее
                        x.DeadlineInformationOfDebt.ToStr(), // 10. Срок предоставления информации о поступивших платежах и о задолженностях
                        x.DayStart.ToStr(), // 11. День начала сдачи текущих показаний по ПУ
                        x.DayEnd.ToStr(), // 12. День окончания сдачи текущих показаний по ПУ
                        this.GetDeviceMetteringIndicationMonth(x.StartDeviceMetteringIndication), // 13. Месяц сдачи текущих показаний по ПУ
                        this.GetResOrgReason(x.ResOrgReason), // 14. Основание заключения договора
                        "4", // 15. Вторая сторона договора - всегда УО
                        string.Empty, // 16. Физическое лицо
                        contragentDict.Get(x.Id).ToStr(), // 17. Контрагент
                        string.Empty, // 18. Наличие в договоре планового объема и режима подачи поставки ресурса
                        string.Empty, // 19. Показатели качества ресурсов и температурный график ведутся
                        this.GetCommercialMeteringResourceType(x.Id, commercialMeteringResourceTypeDict), // 20. Коммерческий учет ресурса осуществляет
                        x.StopReason == null ? "1" : "3", // 21. Статус ДРСО
                        this.GetDate(x.DateStop), // 22. Дата расторжения, прекращения действия
                        string.Empty, // 23. Причина расторжения договора
                        string.Empty, // 24. Дата окончания пролонгации
                        string.Empty, // 25. Причина аннулирования договора
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 13:
                case 14:
                case 18:
                case 20:
                    return row.Cells[cell.Key].IsEmpty();
                case 15:
                case 16:
                    return row.Cells[15].IsEmpty() && row.Cells[16].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код ",
                "Уникальный идентификатор РСО",
                "Договор является публичным",
                "Номер документа",
                "Дата заключения",
                "Дата вступления в силу",
                "Дата окончания действия",
                "Срок выставления счетов к оплате, не позднее",
                "Срок оплаты, не позднее",
                "Срок предоставления информации о поступивших платежах и о задолженностях",
                "День начала сдачи текущих показаний по ПУ ",
                "День окончания сдачи текущих показаний по ПУ",
                "Месяц сдачи текущих показаний по ПУ",
                "Основание заключения договора",
                "Вторая сторона договора",
                "Физическое лицо",
                "Контрагент",
                "Наличие в договоре планового объема и режима подачи поставки ресурса",
                "Показатели качества ресурсов и температурный график ведутся",
                "Коммерческий учет ресурса осуществляет",
                "Статус ДРСО",
                "Дата расторжения, прекращения действия",
                "Причина расторжения договора",
                "Дата окончания пролонгации",
                "Причина аннулирования договора"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "RSO",
                "IND",
                "CONTRAGENT"
            };
        }

        private string GetDeviceMetteringIndicationMonth(MonthType startDeviceMetteringIndication)
        {
            switch (startDeviceMetteringIndication)
            {
                case MonthType.CurrentMonth:
                {
                    return "1";
                }
                case MonthType.NextMonth:
                {
                    return "2";
                }
                default:
                {
                    return string.Empty;
                }
            }
        }

        private string GetResOrgReason(ResOrgReason resOrgReason)
        {
            switch (resOrgReason)
            {
                case ResOrgReason.OwnerMeetingDecision:
                    return "1";

                case ResOrgReason.OpenCompetition:
                    return "2";

                case ResOrgReason.ManagementContract:
                    return "3";

                case ResOrgReason.Charter:
                    return "4";

                default:
                    return string.Empty;
            }
        }

        private string GetCommercialMeteringResourceType(long contractId, IDictionary<long, CommercialMeteringResourceType?> commercialMeteringResourceTypeDict)
        {
            var commercialMeteringResourceType = commercialMeteringResourceTypeDict.Get(contractId);

            switch (commercialMeteringResourceType)
            {
                case CommercialMeteringResourceType.Rso:
                    return "1";

                case CommercialMeteringResourceType.CommunalServicesExecutor:
                    return "2";

                default:
                    return string.Empty;
            }
        }
    }
}