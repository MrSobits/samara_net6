﻿namespace Bars.Gkh.Reforma.Enums
{
    using Bars.B4.Utils;

    public enum ProfilePartsEnum {
        /// <summary>
        /// Анкета УО - Общая характеристика --- Копии документов о применении мер административного воздействия, а также мер, принятых для устранения нарушений, повлекших применение административных санкций
        /// </summary>
        [Display("Анкета УО - Общая характеристика --- Копии документов о применении мер административного воздействия, а также мер, принятых для устранения нарушений, повлекших применение административных санкций")]
        overview_prosecute_documents_copies = 1,

        /// <summary>
        /// Анкета УО - Общая характеристика --- Дополнительная информация
        /// </summary>
        [Display("Анкета УО - Общая характеристика --- Дополнительная информация")]
        overview_additional_files = 2,

        /// <summary>
        /// Анкета УО – Финансовые показатели --- Годовая бухгалтерская отчетность
        /// </summary>
        [Display("Анкета УО – Финансовые показатели --- Годовая бухгалтерская отчетность")]
        financial_indicators_annual_financial_statements = 3,

        /// <summary>
        /// Анкета УО – Финансовые показатели --- Сметы доходов и расходов ТСЖ или ЖСК
        /// </summary>
        [Display("Анкета УО – Финансовые показатели --- Сметы доходов и расходов ТСЖ или ЖСК")]
        financial_indicators_revenues_expenditures_estimates = 4,

        /// <summary>
        /// Анкета УО – Финансовые показатели --- Отчет о выполнении сметы доходов и расходов
        /// </summary>
        [Display("Анкета УО – Финансовые показатели --- Отчет о выполнении сметы доходов и расходов")]
        financial_indicators_performance_report = 5,

        /// <summary>
        /// Анкета УО – Финансовые показатели --- Протоколы общих собраний членов товарищества или кооператива, заседаний правления и ревизионной комиссии
        /// </summary>
        [Display("Анкета УО – Финансовые показатели --- Протоколы общих собраний членов товарищества или кооператива, заседаний правления и ревизионной комиссии")]
        financial_indicators_general_meetings_protocol = 6,

        /// <summary>
        /// Анкета УО – Финансовые показатели --- Заключения ревизионной комиссии (ревизора) товарищества или кооператива по результатам проверки годовой бухгалтерской (финансовой) отчетности
        /// </summary>
        [Display("Анкета УО – Финансовые показатели --- Заключения ревизионной комиссии (ревизора) товарищества или кооператива по результатам проверки годовой бухгалтерской (финансовой) отчетности")]
        financial_indicators_audit_commision_report = 7,

        /// <summary>
        /// Анкета УО – Финансовые показатели --- Аудиторские заключения
        /// </summary>
        [Display("Анкета УО – Финансовые показатели --- Аудиторские заключения")]
        financial_indicators_audit_reports = 8,

        /// <summary>
        /// Анкета УО – Деятельность по управлению МКД --- Проект договора управления
        /// </summary>
        [Display("Анкета УО – Деятельность по управлению МКД --- Проект договора управления")]
        management_activities_management_contract = 9,

        /// <summary>
        /// Анкета УО – Деятельность по управлению МКД --- Стоимость услуг
        /// </summary>
        [Display("Анкета УО – Деятельность по управлению МКД --- Стоимость услуг")]
        management_activities_services_cost = 10,

        /// <summary>
        /// Анкета УО – Деятельность по управлению МКД --- Тарифы
        /// </summary>
        [Display("Анкета УО – Деятельность по управлению МКД --- Тарифы")]
        management_activities_tariffs = 11,

        /// <summary>
        /// Анкета МКД - Управление ---Выполняемые работы
        /// </summary>
        [Display("Анкета МКД - Управление ---Выполняемые работы")]
        contract_periodic_data_jobs = 12,

        /// <summary>
        /// Анкета МКД - Управление --- Выполнение обязательств
        /// </summary>
        [Display("Анкета МКД - Управление --- Выполнение обязательств")]
        contract_periodic_data_responsibility = 13,

        /// <summary>
        /// Анкета МКД - Управление --- Стоимость услуг
        /// </summary>
        [Display("Анкета МКД - Управление --- Стоимость услуг")]
        contract_periodic_data_cost_service = 14,

        /// <summary>
        /// Анкета МКД - Управление --- Средства ТСЖ или ЖСК
        /// </summary>
        [Display("Анкета МКД - Управление --- Средства ТСЖ или ЖСК")]
        contract_periodic_data_resources_tsz_zsk = 15,

        /// <summary>
        /// Анкета МКД - Управление --- Условия оказания услуг ТСЖ или ЖСК
        /// </summary>
        [Display("Анкета МКД - Управление --- Условия оказания услуг ТСЖ или ЖСК")]
        contract_periodic_data_terms_service_tsz_zsk = 16,

        /// <summary>
        /// Анкета МКД – Общая характеристика --- Файлы(акты), которые прикрепляются при смене состояния дома(на состояние «аварийный» и из состояния «аварийный»)
        /// </summary>
        [Display("Анкета МКД – Общая характеристика --- Файлы(акты), которые прикрепляются при смене состояния дома(на состояние «аварийный» и из состояния «аварийный»)")]
        mkd_overview_alarm_reason = 21
    }
}