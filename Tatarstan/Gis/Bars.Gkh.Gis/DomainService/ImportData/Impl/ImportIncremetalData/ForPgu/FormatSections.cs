namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.ForPgu
{
    using System;

    using Bars.Gkh.Gis.Enum;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Секции ПГМУ
    /// </summary>
    public static class FormatSections
    {
        // Секции формата 1.2
        private static readonly List<FileSection> SectionsV12 = new List<FileSection>
        {
            new FileSection
            {
                PguFileSection = PguFileSection.InfoDescript,
                Description = "Файл информационного описания"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.CharacterGilFond,
                TableName = "public.parameters",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "num_ls", "post_code", "town", "rajon", "ulica",
                    "ndom", "complex", "nkor", "nkvar", "nkvar_n", "porch", "fio", "uk", "uk_code",
                    "comfortable", "is_priv", "floor", "kvar_on_floor", "square", "square_living",
                    "square_heating", "square_dom", "square_dom_mop", "square_dom_heat",
                    "count_gil", "count_departure", "count_arrive", "count_room", "prefix",
                    "account_state", "subscription_status", "subscription_email",
                    "subscription_date", "display_tenant_count_in_epd", "show_full_name"
                },
                Description = "Характеристики жилого фонда"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.ChargExpenseServ,
                TableName = "public.charge",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "service", "measure", "ordering",
                    "nzp_serv", "nzp_serv_base", "serv_group", "tarif", "rashod", "rashod_odn",
                    "rashod_ipu", "rashod_norm", "rashod_dom", "rashod_dom_kv", "rashod_dom_ipu",
                    "rashod_dom_norm", "rashod_dom_arend", "rashod_dom_lift", "rashod_dom_odn",
                    "rashod_dom_odpu", "rsum_tarif", "sum_tarif", "size_of_excess", "accured",
                    "sum_nedop", "rashod_nedop", "days_nedop", "reval", "real_charge", "sum_charge",
                    "sum_money", "sum_outsaldo", "sum_insaldo", "sum_tarif_odn", "sum_insaldo_odn",
                    "sum_outsaldo_odn", "reval_odn", "real_charge_odn", "sum_charge_odn", "sum_money_odn",
                    "sum_tarif_sn", "k_odn_ipu", "k_odn_norm", "supplier", "c_okaz", "c_nedop", "executor_code",
                    "total_to_pay", "display_gvs_rate_in_epd"
                },
                Description = "Начисления и расходы по услугам"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.Counters,
                TableName = "public.counters",
                Columns = new []
                {
                    "nzp_load", " dat_month", " erc_code", " pkod", " service", " measure", " ordering",
                    "type_ipu", " num_cnt", " nzp_counter", " dat_uchet", " val_cnt", " dat_uchet_pred",
                    "val_cnt_pred", " mmnogitel", " transform_coef", " rashod", " add_rashod", " rashod_arend",
                    "soi_odn_consumption", " rashod_lift", " place", " cnt_stage", " cnt_type_name", " dat_prov",
                    "dat_prov_next", " serv_volume_ipu", " serv_volume_dict_vol", " serv_volume_dict_measure",
                    "virtual_pu", "no_counters_reading", "resource", "service_code"
                },
                Description = "Показания счетчиков"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.PaymentDetails,
                TableName = "public.billinfo",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "str_type", "poluch", "poluch_bank",
                    "poluch_rschet", "spec_acc", "poluch_kschet", "poluch_inn", "poluch_kpp", "poluch_bik",
                    "fact_recipient_address", "poluch_phone", "poluch_email", "poluch_note", "small_kode_uk",
                    "full_text", "jur_recipient_address",  "display_system_operator_requisites"
                },
                Description = "Платежные реквизиты"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.Payment,
                TableName = "public.payments",
                Columns = new[]
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "date_vvod", "date_uchet", "date_epd", "sum_pay", "plat_place"
                },
                Description = "Оплаты"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.InfoSocProtection,
                TableName = "public.sz",
                Columns = new[]
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "fio", "nzp_exp", "expence", "type_expence",
                    "sum_charge", "sum_must", "viplat_place", "finish_subsidy", "sum_insaldo", "sum_delta", "sum_charge_p"
                },
                Description = "Информация органов социальной защиты"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.AdditionalInfo,
                TableName = "public.additionalinfo",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "chief_fio", "chief_post", "chief_phone",
                    "ads_phone", "uk_website", "senior_on_house_fio", "senior_address", "senior_on_house_phone",
                    "charges_department_address", "charges_department_phone", "charges_department_website",
                    "counter_values_input_start_day", "counter_values_input_end_day",
                    "payment_date", "pay_doc_info_calc", "pay_doc_info_payments", "debt_info"
                },
                Description = "Дополнительная информация"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.InfoExecutor,
                TableName = "public.executorinfo",
                Columns = new[]
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "code", "name", "address", "inn", "kpp", "pay_acc",
                    "bank", "corr_acc", "bik", "spec_acc", "phone", "email", "em_phone", "em_email", "note", "fax", "official_site"
                },
                Description = "Информация об исполнителе"
            }
        };

        // Секции формата 1.3
        private static readonly List<FileSection> SectionsV13 = new List<FileSection>
        {
            new FileSection
            {
                PguFileSection = PguFileSection.InfoDescript,
                Description = "Файл информационного описания"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.CharacterGilFond,
                TableName = "public.parameters",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "num_ls", "post_code", "town", "rajon", "ulica",
                    "ndom", "complex", "nkor", "nkvar", "nkvar_n", "porch", "fio", "uk", "uk_code",
                    "comfortable", "is_priv", "floor", "kvar_on_floor", "square", "square_living",
                    "square_heating", "square_dom", "square_dom_mop", "square_dom_heat",
                    "count_gil", "count_departure", "count_arrive", "count_room", "prefix",
                    "account_state", "subscription_status", "subscription_email", "subscription_date",
                    "display_tenant_count_in_epd", "show_full_name", "penalty_epd", "energobil_address", "ps_info",
                    "tenant_fio", "barcode", "debt_overpay"

                },
                Description = "Характеристики жилого фонда"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.ChargExpenseServ,
                TableName = "public.charge",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "service", "measure", "ordering",
                    "nzp_serv", "nzp_serv_base", "serv_group", "tarif", "rashod", "rashod_odn",
                    "rashod_ipu", "rashod_norm", "rashod_dom", "rashod_dom_kv", "rashod_dom_ipu",
                    "rashod_dom_norm", "rashod_dom_arend", "rashod_dom_lift", "rashod_dom_odn",
                    "rashod_dom_odpu", "rsum_tarif", "sum_tarif", "size_of_excess", "accured",
                    "sum_nedop", "rashod_nedop", "days_nedop", "reval", "real_charge", "sum_charge",
                    "sum_money", "sum_outsaldo", "sum_insaldo", "sum_tarif_odn", "sum_insaldo_odn",
                    "sum_outsaldo_odn", "reval_odn", "real_charge_odn", "sum_charge_odn", "sum_money_odn",
                    "sum_tarif_sn", "k_odn_ipu", "k_odn_norm", "supplier", "c_okaz", "c_nedop", "executor_code",
                    "total_to_pay", "display_gvs_rate_in_epd", "service_cost", "energobil_accrual", "cons_accrual"
                },
                Description = "Начисления и расходы по услугам"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.Counters,
                TableName = "public.counters",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "service", "measure", "ordering",
                    "type_ipu", "num_cnt", "nzp_counter", "dat_uchet", "val_cnt", "dat_uchet_pred",
                    "val_cnt_pred", "mmnogitel", "transform_coef", "rashod", "add_rashod", "rashod_arend",
                    "soi_odn_consumption", "rashod_lift", "place", "cnt_stage", "cnt_type_name", "dat_prov",
                    "dat_prov_next", "serv_volume_ipu", "serv_volume_dict_vol", "serv_volume_dict_measure",
                    "virtual_pu", "no_counters_reading", "resource", "service_code", "nonres_cost", "liv_cost"
                },
                Description = "Показания счетчиков"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.PaymentDetails,
                TableName = "public.billinfo",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "str_type", "poluch", "poluch_bank",
                    "poluch_rschet", "spec_acc", "poluch_kschet", "poluch_inn", "poluch_kpp", "poluch_bik",
                    "fact_recipient_address", "poluch_phone", "poluch_email", "poluch_note", "small_kode_uk",
                    "full_text", "jur_recipient_address",  "display_system_operator_requisites"
                },
                Description = "Платежные реквизиты"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.Payment,
                TableName = "public.payments",
                Description = "Оплаты"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.InfoSocProtection,
                TableName = "public.sz",
                Description = "Информация органов социальной защиты"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.AdditionalInfo,
                TableName = "public.additionalinfo",
                Columns = new []
                {
                    "nzp_load", "dat_month", "erc_code", "pkod", "chief_fio", "chief_post", "chief_phone",
                    "ads_phone", "uk_website", "senior_on_house_fio", "senior_address", "senior_on_house_phone",
                    "charges_department_address", "charges_department_phone", "charges_department_website",
                    "counter_values_input_start_day", "counter_values_input_end_day",
                    "payment_date", "pay_doc_info_calc", "pay_doc_info_payments", "debt_info", "sector_name",
                    "sector_address", "sector_phone", "epd_info"
                },
                Description = "Дополнительная информация"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.InfoExecutor,
                TableName = "public.executorinfo",
                Description = "Информация об исполнителе"
            },
            new FileSection
            {
                PguFileSection = PguFileSection.RevalServ,
                TableName = "public.revalservices",
                Description = "Перерасчеты по коммунальным услугам"
            }
        };

        /// <summary>
        /// Возвращает список секций в зависимости от версии формата
        /// </summary>
        /// <param name="formatVersion">Версия формата</param>
        /// <returns>Список секций</returns>
        public static List<FileSection> GetSections(string formatVersion)
        {
            if (Regex.IsMatch(formatVersion, "^1\\.3"))
            {
                return SectionsV13;
            }

            throw new PguVersionException("Архив не прошел валидацию! Неактуальная версия формата загрузки! " +
                $"(версия формата в файле: '{formatVersion}') " +
                "Необходимо обновить программное обеспечение и повторно выгрузить файл.");
        }
    }
}
