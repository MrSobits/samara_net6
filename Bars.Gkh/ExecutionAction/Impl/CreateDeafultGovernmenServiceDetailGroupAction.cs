namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.Entities.Licensing;
    using Bars.Gkh.Enums.Licensing;

    /// <summary>
    /// Действие по созданию показателей по умолчанию для сущности <see cref="GovernmenServiceDetailGroup" />
    /// </summary>
    public class CreateDeafultGovernmenServiceDetailGroupAction : BaseMandatoryExecutionAction
    {
        /// <inheritdoc />
        public override string Description => "Действие создаёт показатели формы 1-ГУ";

        /// <inheritdoc />
        public override string Name => "Создать показатели формы 1-ГУ";

        /// <summary>
        /// Домен-сервис <see cref="GovernmenServiceDetailGroup" />
        /// </summary>
        public IDomainService<GovernmenServiceDetailGroup> GovernmenServiceDetailGroupDomain { get; set; }

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <inheritdoc />
        public override bool IsNeedAction()
        {
            return !this.GovernmenServiceDetailGroupDomain.GetAll().Any();
        }

        private BaseDataResult Execute()
        {
            var data = new List<GovernmenServiceDetailGroup>(55)
            {
                new GovernmenServiceDetailGroup(1, "Общая штатная численность, человек", ServiceDetailSectionType.PublicServices),
                new GovernmenServiceDetailGroup(
                    2,
                    "количество сотрудников (работников), задействованных в предоставлении государственной услуги",
                    ServiceDetailSectionType.PublicServices,
                    "из них"),
                new GovernmenServiceDetailGroup(
                    3,
                    "осуществляющих непосредственное взаимодействие с заявителями (включая прием документов и выдачу результатов)",
                    ServiceDetailSectionType.PublicServices,
                    "в том числе"),
                new GovernmenServiceDetailGroup(
                    4,
                    "осуществляющих иные действия, связанные с предоставлением государственной услуги, в том числе принятие решения о выдаче заявителю результата, и не осуществляющих непосредственное взаимодействие с заявителями",
                    ServiceDetailSectionType.PublicServices,
                    "в том числе"),
                new GovernmenServiceDetailGroup(
                    5,
                    "Общее количество функционирующих мест (окон) предоставления государственной услуги, оборудованных в соответствии со стандартами предоставления государственной услуги, в органе, предоставляющем государственную услугу, либо в подведомственной организации, единица",
                    ServiceDetailSectionType.PublicServices),
                new GovernmenServiceDetailGroup(
                    6,
                    "Объем финансовых средств, переведенных в бюджет соответствующего уровня в уплату государственных пошлин за предоставление государственной услуги, рубль",
                    ServiceDetailSectionType.PublicServices),
                new GovernmenServiceDetailGroup(7, "зачисляемых в федеральный бюджет", ServiceDetailSectionType.PublicServices, "из них"),
                new GovernmenServiceDetailGroup(8, "зачисляемых в бюджет субъекта Российской Федерации", ServiceDetailSectionType.PublicServices, "из них"),
                new GovernmenServiceDetailGroup(9, "зачисляемых в местный бюджет", ServiceDetailSectionType.PublicServices, "из них"),
                new GovernmenServiceDetailGroup(
                    10,
                    "Объем финансовых средств, переведенных в бюджет соответствующего уровня в уплату иных обязательных платежей за предоставление государственной услуги, рубль",
                    ServiceDetailSectionType.PublicServices),
                new GovernmenServiceDetailGroup(11, "на счетах подведомственных организаций", ServiceDetailSectionType.PublicServices, "в том числе"),
                new GovernmenServiceDetailGroup(
                    12,
                    "Общее количество заявлений (запросов) о предоставлении государственной услуги, поступивших от физических лиц",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(
                    13,
                    "непосредственно в орган, предоставляющий государственную услугу, или подведомственную организацию",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(14, "через МФЦ", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    15,
                    "через Единый портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    16,
                    "через Региональный портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    17,
                    "через официальный сайт органа, предоставляющего государственную услугу",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(18, "иным способом", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    19,
                    "Общее количество заявлений (запросов) о предоставлении государственной услуги, поступивших от юридических лиц и (или) индивидуальных предпринимателей",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(
                    20,
                    "непосредственно в орган, предоставляющий государственную услугу, или подведомственную организацию",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(21, "через МФЦ", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    22,
                    "через Единый портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    23,
                    "через Региональный портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    24,
                    "через официальный сайт органа, предоставляющего государственную услугу",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(25, "иным способом", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    26,
                    "Общее количество положительных решений (выданных документов, совершенных действий), принятых по результатам предоставления государственной услуги, в отношении заявителей - физических лиц",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(
                    27,
                    "непосредственно в орган, предоставляющий государственную услугу, или подведомственную организацию",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(28, "через МФЦ", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    29,
                    "через Единый портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    30,
                    "через Региональный портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    31,
                    "через официальный сайт органа, предоставляющего государственную услугу",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(32, "иным способом", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    33,
                    "Общее количество положительных решений (выданных документов, совершенных действий), принятых по результатам предоставления государственной услуги, в отношении заявителей - юридических лиц и (или) индивидуальных предпринимателей",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(
                    34,
                    "непосредственно в орган, предоставляющий государственную услугу, или подведомственную организацию",
                    ServiceDetailSectionType.ServiceDelivery),
                new GovernmenServiceDetailGroup(35, "через МФЦ", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    36,
                    "через Единый портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    37,
                    "через Региональный портал государственных и муниципальных услуг (функций)",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    38,
                    "через официальный сайт органа, предоставляющего государственную услугу",
                    ServiceDetailSectionType.ServiceDelivery,
                    "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(39, "иным способом", ServiceDetailSectionType.ServiceDelivery, "из них запросы (заявления) представлены"),
                new GovernmenServiceDetailGroup(
                    40,
                    "Средний фактический срок предоставления государственной услуги при предоставлении государственной услуги непосредственно через орган, предоставляющий государственную услугу, или через подведомственную организацию, минута",
                    ServiceDetailSectionType.ServiceTime),
                new GovernmenServiceDetailGroup(41, "в том числе по предварительной записи", ServiceDetailSectionType.ServiceTime),
                new GovernmenServiceDetailGroup(
                    42,
                    "Среднее время ожидания заявителя в очереди на подачу заявления (запроса, документов) на предоставление государственной услуги при предоставлении государственной услуги непосредственно через орган, предоставляющий государственную услугу, или через подведомственную организацию, минута",
                    ServiceDetailSectionType.ServiceTime),
                new GovernmenServiceDetailGroup(43, "в том числе по предварительной записи", ServiceDetailSectionType.ServiceTime),
                new GovernmenServiceDetailGroup(
                    44,
                    "Среднее время ожидания заявителя в очереди на получение результата предоставления государственной услуги при предоставлении государственной услуги непосредственно через орган, предоставляющий государственную услугу, или через подведомственную организацию, минута",
                    ServiceDetailSectionType.ServiceTime),
                new GovernmenServiceDetailGroup(45, "в том числе по предварительной записи", ServiceDetailSectionType.ServiceTime),
                new GovernmenServiceDetailGroup(
                    46,
                    "Общее количество поступивших жалоб в рамках досудебного (внесудебного) обжалования",
                    ServiceDetailSectionType.AppealAndDecisions),
                new GovernmenServiceDetailGroup(
                    47,
                    "нарушений срока регистрации запросов заявителя о предоставлении государственной услуги, срока предоставления государственной услуги",
                    ServiceDetailSectionType.AppealAndDecisions,
                    "в том числе об обжаловании"),
                new GovernmenServiceDetailGroup(
                    48,
                    "требования у заявителя документов, не предусмотренных нормативными правовыми актами Российской Федерации, нормативными правовыми актами субъектов Российской Федерации, для предоставления государственной услуги",
                    ServiceDetailSectionType.AppealAndDecisions,
                    "в том числе об обжаловании"),
                new GovernmenServiceDetailGroup(
                    49,
                    "отказа в приеме документов, предоставление которых предусмотрено нормативными правовыми актами Российской Федерации, нормативными правовыми актами субъектов Российской Федерации, для предоставления государственной услуги, у заявителя",
                    ServiceDetailSectionType.AppealAndDecisions,
                    "в том числе об обжаловании"),
                new GovernmenServiceDetailGroup(
                    50,
                    "отказа в предоставлении государственной услуги, если основания отказа не предусмотрены федеральными законами и принятыми в соответствии с ними иными нормативными правовыми актами Российской Федерации, нормативными правовыми актами субъектов Российской Федерации",
                    ServiceDetailSectionType.AppealAndDecisions,
                    "в том числе об обжаловании"),
                new GovernmenServiceDetailGroup(
                    51,
                    "затребования с заявителя при предоставлении государственной услуги платы, не предусмотренной нормативными правовыми актами Российской Федерации, нормативными правовыми актами субъектов Российской Федерации",
                    ServiceDetailSectionType.AppealAndDecisions,
                    "в том числе об обжаловании"),
                new GovernmenServiceDetailGroup(
                    52,
                    "отказа органа, предоставляющего государственную услугу, должностного лица органа, предоставляющего государственную услугу, в исправлении допущенных опечаток и ошибок в выданных в результате предоставления государственной услуги документах либо нарушение установленного срока таких исправлений",
                    ServiceDetailSectionType.AppealAndDecisions,
                    "в том числе об обжаловании"),
                new GovernmenServiceDetailGroup(
                    53,
                    "Общее количество обращений в суд об обжаловании нарушений при предоставлении государственной услуги",
                    ServiceDetailSectionType.AppealAndDecisions),
                new GovernmenServiceDetailGroup(
                    54,
                    "в том числе удовлетворенных судами требований об обжаловании нарушений при предоставлении государственной услуги",
                    ServiceDetailSectionType.AppealAndDecisions),
                new GovernmenServiceDetailGroup(
                    55,
                    "Общее количество случаев привлечения к административной ответственности за нарушения при предоставлении государственной услуги",
                    ServiceDetailSectionType.AppealAndDecisions)
            };

            this.Container.InTransaction(
                () =>
                {
                    foreach (var governmenServiceDetailGroup in data)
                    {
                        this.GovernmenServiceDetailGroupDomain.Save(governmenServiceDetailGroup);
                    }
                });

            return new BaseDataResult();
        }
    }
}