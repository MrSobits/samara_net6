// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Global.Constants
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Collections;
    using System.Threading;

    public static class Constants
    {
        public static Thread[] ExcelThreads = new Thread[30];
        public static Queue ExcelQueue = new Queue();
        public static Thread QThreadExcel = (Thread)null;
        public static bool ExcelIsInstalled = false;
        public static string TmpFilesDirWeb = "tmp/";
        public static string Login = "";
        public static string Password = "";
        public static string VersionWeb = "2014.093 от 19.09.2014";
        public static string VersionSrv = "2014.093 от 19.09.2014";
        public static int VersionDB = 28;
        public static string DefaultAspx = "";
        public static string cons_Portal = "";
        public static string ContractNameIP = "Договор";
        public static string ContractNameIPWbr = "Дого<wbr>вор";
        public static string ContractNameMCh = "Договоры";
        public static string ContractNameEChMChIP = "Договор(ы)";
        public static string ContractNameRP = "Договора";
        public static string ContractNameEChMChRP = "Договора(ов)";
        public static string ContractNameMChRP = "Договоров";
        public static string ContractNameDP = "Договору";
        public static string ContractNameMChDP = "Договорам";
        public static string ContractNameVP = "Договор";
        public static string ContractNameEChMChVP = "Договор(ы)";
        public static string ContractNameTP = "Договором";
        public static string ContractNamePP = "Договоре";
        public static int users_min = 60;
        public static string[] svc_errors = new string[9]
        {
      "нормальное завершение",
      "неверные входные данные",
      "ошибка выборки данных",
      "неверный формат лицевого счета",
      "неверный префикс УК в лицевом счете",
      "неверный номер лицевого счета",
      "неверный контрольный бит в лицевом счете",
      "данный лицевой счет не обслуживается",
      "лицевой счет не определен в базе данных"
        };
        public const string Linespace = "http://www.stcline.ru";
        public const string Kassa_3_0 = "WorkOnlyWithCentralBank";
        public const string access_error = "Сервис временно недоступен. Попробуйте выполнить операцию позже.";
        public const int access_code = -1000;
        public const string name_logfile = "Komplat50Log";
        public const int AllZap = -101;
        public const int DefaultZap = -102;
        public const string ChooseData = "<Выберите данные>";
        public const string ChooseServ = "<Выберите услугу>";
        public const string ChooseSupp = "<Выберите договор>";
        public const int regprm_ls = 1;
        public const int regprm_dom = 2;
        public const int regprm_odn = 3;
        public const int ist = 9;
        public const int roleKartoteka = 10;
        public const int roleAnalitika = 11;
        public const int roleAdministrator = 12;
        public const int rolePriboriUcheta = 13;
        public const int rolePasportistka = 14;
        public const int roleFinance = 15;
        public const int roleHidePersonalInfo = 18;
        public const int roleSupg = 19;
        public const int roleKassa = 20;
        public const int roleSubsidy = 22;
        public const int roleCalcSubsidy = 23;
        public const int roleNormAdd = 918;
        public const int roleDebt = 920;
        public const int roleRaschetNachisleniy = 921;
        public const int roleReport = 30;
        public const int roleUpgOperator = 934;
        public const int roleUpgDispetcher = 935;
        public const int roleUpgPodratchik = 936;
        public const int roleUpgUK = 937;
        public const int roleUpgAdministrator = 919;
        public const int roleFinSpravChange = 942;
        public const int roleChangeSaldoOplatami = 956;
        public const int typKrtPrib = 1;
        public const int typKrtUbit = 2;
        public const string typKrtPribName = "ПРИБЫТИЕ";
        public const string typKrtUbitName = "УБЫТИЕ";
        public const string tprpConst = "П";
        public const string tprpTemp = "В";
        public const string tprpConstName = "ЖИТЕЛЬСТВА";
        public const string tprpTempName = "ПРЕБЫВАНИЯ";
        public const string zTypeExternalSource = "99";
        public const bool flEmptyAddress = true;
        public const string NzpEmptyAddress = "0";
        public const int selNzp_kvar = 1;
        public const int selNzp_dom = 2;
        public const int acc_in = 1;
        public const int acc_exit = 2;
        public const int acc_failure = 3;
        public const string _UNDEF_ = "_UNDEF_";
        public const int _ZERO_ = -999987654;
        public const int blocking_lifetime = 7;
        public const int recovery_link_lifetime = 24;
        public const int workinfon = -999;
        public const int arm_kartoteka = 10;
        public const int arm_analitika = 11;
        public const int arm_admin = 12;
        public const int page_login = 0;
        public const int page_default = 1;
        public const int page_ps = 10;
        public const int page_myreport = 5;
        public const int page_settings = 6;
        public const int page_perekidkioplatami = 7;
        public const int page_prm_normatives = 8;
        public const int page_findls = 31;
        public const int page_findprm = 32;
        public const int page_findch = 33;
        public const int page_findgil3 = 34;
        public const int page_findgil = 34;
        public const int page_findcnt = 35;
        public const int page_findnedop = 36;
        public const int page_findodn = 37;
        public const int page_findserv = 38;
        public const int page_findsupg = 39;
        public const int page_spis = 40;
        public const int page_spisls = 41;
        public const int page_spisdom = 42;
        public const int page_spisul = 43;
        public const int page_spisar = 44;
        public const int page_spisgeu = 45;
        public const int page_spisbd = 46;
        public const int page_domul = 47;
        public const int page_backtoprm = 48;
        public const int page_spissupp = 49;
        public const int page_perechen_lsdom = 50;
        public const int page_spisprm = 51;
        public const int page_spispu = 53;
        public const int page_spisval = 54;
        public const int page_spisnd = 55;
        public const int page_spisgil = 56;
        public const int page_spisdomls = 57;
        public const int page_spisuldom = 58;
        public const int page_spisprmdom = 59;
        public const int page_spisvaldom = 61;
        public const int page_puls = 62;
        public const int page_pudom = 63;
        public const int page_spisvalgroup = 66;
        public const int page_spisvalgroupdom = 67;
        public const int page_counterscardls = 68;
        public const int page_counterscarddom = 69;
        public const int page_data = 70;
        public const int page_datadom = 71;
        public const int page_аnalytics = 72;
        public const int page_dictionaries = 73;
        public const int page_datapack = 76;
        public const int page_operations = 77;
        public const int page_data_about_order = 78;
        public const int page_aa_adres = 81;
        public const int page_aa_supp = 82;
        public const int page_counterscard = 91;
        public const int page_gil = 91;
        public const int page_pugroup = 92;
        public const int page_counterscardgroup = 93;
        public const int page_countertypes = 94;
        public const int page_spisserv = 95;
        public const int page_supp_formuls = 96;
        public const int page_groupls = 97;
        public const int page_cardls = 98;
        public const int page_carddom = 99;
        public const int page_groupspisprmls = 100;
        public const int page_groupspisprmdom = 102;
        public const int page_groupprmls = 101;
        public const int page_groupprmdom = 103;
        public const int page_groupspisserv = 104;
        public const int page_group_supp_formuls = 105;
        public const int page_statcharge = 106;
        public const int page_distrib = 107;
        public const int page_groupcardls = 108;
        public const int page_groupcarddom = 109;
        public const int page_groupnedop = 110;
        public const int page_changesostls = 111;
        public const int page_statchargedom = 112;
        public const int page_charge = 120;
        public const int page_charges = 122;
        public const int page_listpays = 123;
        public const int page_odn = 124;
        public const int page_saldols = 121;
        public const int page_saldodom = 126;
        public const int page_saldouk = 127;
        public const int page_saldosupp = 130;
        public const int page_bill = 129;
        public const int page_billrt = 131;
        public const int page_pay = 132;
        public const int page_reportls = 133;
        public const int page_reportlistls = 134;
        public const int page_reportgil = 135;
        public const int page_reportlistgil = 136;
        public const int page_reportdom = 137;
        public const int page_reportlist = 138;
        public const int page_reportlistplan = 139;
        public const int page_users = 151;
        public const int page_roles = 152;
        public const int page_usercard = 153;
        public const int page_rolecard = 154;
        public const int page_access = 155;
        public const int page_processes = 161;
        public const int page_kvargil = 162;
        public const int page_spisgilper = 163;
        public const int page_glp = 164;
        public const int page_perekidki = 165;
        public const int page_rashod_kvar = 166;
        public const int page_rashod_dom = 167;
        public const int page_tarifs = 168;
        public const int page_one_tarif = 169;
        public const int page_sysparams = 170;
        public const int page_prm_pu_kvar = 171;
        public const int page_prm_pu_dom = 172;
        public const int page_prm_kvar = 173;
        public const int page_prm_supp = 174;
        public const int page_suppparams = 175;
        public const int page_frmparams = 176;
        public const int page_spissobstw = 177;
        public const int page_kartsobstw = 178;
        public const int page_group_nedop_dom = 179;
        public const int page_group_spis_ls_prm_dom = 180;
        public const int page_group_ls_prm_dom = 181;
        public const int page_group_spis_serv_dom = 182;
        public const int page_group_supp_formuls_dom = 183;
        public const int page_report_odn = 184;
        public const int page_spispu_communal = 185;
        public const int page_spisval_communal = 186;
        public const int page_pu_communal = 187;
        public const int page_prm_dom = 188;
        public const int page_supg_kvar_orders = 189;
        public const int page_spisservdom = 190;
        public const int page_spisnddom = 191;
        public const int page_prmodn = 192;
        public const int page_joborder = 193;
        public const int page_spisgilhistory = 194;
        public const int page_findgroupls = 195;
        public const int page_group = 196;
        public const int page_spis_order = 197;
        public const int page_pack = 198;
        public const int page_pack_ls = 199;
        public const int page_upload_pack = 200;
        public const int page_finances_findpack = 201;
        public const int page_finances_pack = 202;
        public const int page_finances_operday = 203;
        public const int page_finances_pack_ls = 204;
        public const int page_supg_order = 205;
        public const int page_report_common = 206;
        public const int page_supg_arm_operator = 207;
        public const int page_supg_raw_orders = 208;
        public const int page_incoming_job_orders = 209;
        public const int page_prm_norms = 210;
        public const int page_sprav_cel_prib = 211;
        public const int page_sprav_docs = 212;
        public const int page_sprav_rodst = 213;
        public const int page_sprav_grazhd = 214;
        public const int page_sprav_adresses = 215;
        public const int page_sprav_rajon_doma = 216;
        public const int page_sprav_organ_reg_ucheta = 217;
        public const int page_sprav_mesto_vidachi_doc = 218;
        public const int page_sprav_doc_sobst = 219;
        public const int page_supg_nedop = 220;
        public const int page_services = 221;
        public const int page_service_params = 222;
        public const int page_prm_serv = 223;
        public const int page_available_services = 224;
        public const int page_available_service = 225;
        public const int page_find_server = 226;
        public const int page_area_params = 227;
        public const int page_prm_area = 228;
        public const int page_geu_params = 229;
        public const int page_prm_geu = 230;
        public const int page_area_requisites = 231;
        public const int page_payer_requisites = 232;
        public const int page_payer_contracts = 233;
        public const int page_payer_transfer = 234;
        public const int page_menu_oper_day = 235;
        public const int page_supg_kvar_job_order = 238;
        public const int page_percent = 239;
        public const int page_ls_contracts = 240;
        public const int page_counter_readings = 241;
        public const int page_add_period_ub_to_selected = 242;
        public const int page_upload_counter_values = 243;
        public const int page_credit = 244;
        public const int page_find_planned_works = 245;
        public const int page_planned_works = 246;
        public const int page_planned_work_add = 247;
        public const int page_sprav_servorgs = 248;
        public const int page_planned_work_show = 250;
        public const int page_planned_work_ls = 251;
        public const int page_claimcatalog = 252;
        public const int page_case = 253;
        public const int page_analisis = 255;
        public const int page_contractorcatalog = 256;
        public const int page_bankcatalog = 257;
        public const int page_basket = 258;
        public const int page_gendomls = 260;
        public const int page_streetcatalog = 261;
        public const int page_correctsaldo = 262;
        public const int page_groupprm = 263;
        public const int page_genlspu = 264;
        public const int page_condistrpayments = 265;
        public const int page_addtask = 266;
        public const int page_survey_job_orders = 267;
        public const int page_calc_month = 268;
        public const int page_percpt = 284;
        public const int page_cashplan = 285;
        public const int page_requests = 272;
        public const int page_request = 273;
        public const int page_agreements = 279;
        public const int page_agreement = 280;
        public const int page_messagelist = 282;
        public const int page_newmessage = 286;
        public const int page_phonesprav = 289;
        public const int page_refresh_kp_sprav = 297;
        public const int page_sprav_themes = 298;
        public const int page_prepareprintinvoices = 342;
        public const int page_jobs = 343;
        public const int page_ls_events = 345;
        public const int page_dom_events = 347;
        public const int page_download_logs = 348;
        public const int act_groupby_month = 520;
        public const int act_groupby_service = 521;
        public const int act_groupby_supplier = 522;
        public const int act_groupby_formula = 523;
        public const int act_groupby_area = 524;
        public const int act_groupby_geu = 525;
        public const int act_groupby_bd = 526;
        public const int act_groupby_dom = 527;
        public const int act_show_saldo = 528;
        public const int act_groupby_device = 529;
        public const int act_groupby_payer = 536;
        public const int act_groupby_bank = 537;
        public const int act_groupby_date = 538;
        public const int act_groupby_town = 539;
        public const int act_groupby_princip = 540;
        public const int act_groupby_supp = 541;
        public const int act_groupby_agent = 542;
        public const int sortby_adr = 601;
        public const int sortby_ls = 602;
        public const int sortby_ul = 603;
        public const int sortby_uk = 604;
        public const int sortby_serv = 605;
        public const int sortby_supp = 606;
        public const int sortby_fiodr = 607;
        public const int sortby_login = 608;
        public const int sortby_username = 609;
        public const int sortby_nzp_user = 1608;
        public const int sortby_email = 1609;
        public const int menu_help = 3;
        public const int menu_previos = 4;
        public const int menu_myfiles = 5;
        public const int menu_exit = 999;
        public const int menu_seans = 950;
        public const int menu_not = 949;
        public const int act_find = 1;
        public const int act_erase = 2;
        public const int act_open = 3;
        public const int act_add = 4;
        public const int act_refresh = 5;
        public const int act_showmap = 7;
        public const int act_print = 8;
        public const int act_block = 9;
        public const int act_process_start = 10;
        public const int act_process_pause = 11;
        public const int act_delete_all = 12;
        public const int act_delete_session = 13;
        public const int act_open_puindication = 14;
        public const int act_reset_user_pwd = 15;
        public const int act_save_val = 16;
        public const int act_del_val = 17;
        public const int act_add_nedop = 18;
        public const int act_del_nedop = 19;
        public const int act_showallprm = 20;
        public const int act_add_serv = 21;
        public const int act_del_serv = 22;
        public const int act_add_gil = 23;
        public const int act_copy_ls = 24;
        public const int act_add_dom = 25;
        public const int act_delete = 26;
        public const int act_del_pu = 64;
        public const int act_aa_recalc = 65;
        public const int act_aa_refresh = 66;
        public const int act_save = 61;
        public const int act_prm = 67;
        public const int act_update_role_filters = 68;
        public const int act_get_report = 69;
        public const int act_calc = 70;
        public const int act_export_to_excel = 73;
        public const int act_enter_archive = 74;
        public const int act_exit_archive = 75;
        public const int act_open_period_serv = 76;
        public const int act_open_prm_serv = 77;
        public const int act_copy = 78;
        public const int act_paste = 79;
        public const int act_showls = 80;
        public const int act_add_joborder = 81;
        public const int act_add_ingroup = 82;
        public const int act_del_outgroup = 83;
        public const int act_go_back_to_orders = 84;
        public const int act_add_pack = 85;
        public const int act_close_pack = 86;
        public const int act_add_pack_ls = 87;
        public const int act_upload_pack = 88;
        public const int act_find_kassa = 89;
        public const int act_del_pack_ls = 90;
        public const int act_cancel_pack_distribution = 91;
        public const int act_delete_pack = 92;
        public const int act_distribute_pack = 93;
        public const int act_add_zvk = 94;
        public const int act_clear_users_cache = 95;
        public const int act_add_user = 96;
        public const int act_sentJobOrder_toExecute = 97;
        public const int act_checkJobOrder_toExecute = 98;
        public const int act_set_zakaz_act_actual = 100;
        public const int act_close_zakaz_act_actual = 101;
        public const int act_open_params = 103;
        public const int act_open_area_requisites = 104;
        public const int act_open_payer_transfer = 107;
        public const int act_calc_saldo = 108;
        public const int act_add_period_ub_to_selected = 109;
        public const int act_upload_counter_values = 110;
        public const int act_restart_hosting = 113;
        public const int act_incase = 114;
        public const int act_outcase = 115;
        public const int act_cancelplat = 116;
        public const int act_action = 117;
        public const int act_repair_one = 118;
        public const int act_repair_select = 119;
        public const int act_distrib_one = 120;
        public const int act_distrib_select = 121;
        public const int act_gen_dom_ls = 122;
        public const int act_gen_pu = 123;
        public const int act_cont_distrib = 124;
        public const int act_find_error_distrib = 125;
        public const int act_find_error_payment = 126;
        public const int act_open_pack_ls = 127;
        public const int act_print_show = 128;
        public const int act_refresh_addresses = 129;
        public const int act_closeJobOrder_toExecute = 130;
        public const int act_cancelJobOrder = 136;
        public const int act_req_edit = 138;
        public const int act_req_approve = 139;
        public const int act_req_del = 140;
        public const int act_req_add = 142;
        public const int act_page_refresh = 143;
        public const int act_agreement_add = 145;
        public const int act_agreement_del = 146;
        public const int act_new_sms = 150;
        public const int act_send_sms = 151;
        public const int act_del_sms = 152;
        public const int act_load_cashplan = 153;
        public const int act_charge = 164;
        public const int act_exec_refresh = 173;
        public const int act_calc_odpu_rashod = 177;
        public const int act_move_to = 540;
        public const int act_report_spravka_po_smerti = 201;
        public const int act_report_spravka_o_sostave_semji = 202;
        public const int act_report_spravka_s_mesta_reg_v_sud = 203;
        public const int act_report_spravka_na_privatizac = 204;
        public const int act_report_liсevoi_schet = 206;
        public const int act_report_spravka_sostav_semji = 208;
        public const int act_report_listok_ubit = 218;
        public const int act_report_listok_pribit = 219;
        public const int act_report_zay_reg_preb = 214;
        public const int act_report_zay_reg_git_f6 = 215;
        public const int act_report_zay_reg_git = 216;
        public const int act_report_rfl1 = 223;
        public const int act_report_listok_stat_prib = 224;
        public const int act_report_spis_reg_snyat = 220;
        public const int act_report_spis_vuchet = 221;
        public const int act_report_spis_smena_dok = 222;
        public const int act_report_spis_gil = 225;
        public const int act_report_smena_passp = 226;
        public const int act_report_listok_stat_ubit = 227;
        public const int act_report_report_prm = 228;
        public const int act_report_kart_analis = 229;
        public const int act_report_sverka_rashet = 230;
        public const int act_report_dom_nach = 231;
        public const int act_report_sprav_suppnach = 232;
        public const int act_report_sprav_hasdolg = 233;
        public const int act_report_sprav_otkl_uslug = 234;
        public const int act_report_sprav_v_sud = 235;
        public const int act_report_lic_schet_excel = 236;
        public const int act_report_kart_registr = 237;
        public const int act_report_sprav_reg = 238;
        public const int act_report_sprav_suppnachhar = 239;
        public const int act_report_izvechenie_za_mesyac = 240;
        public const int act_report_kvar_kart = 241;
        public const int act_report_spravsmg = 242;
        public const int act_report_spravpozapros_smr = 243;
        public const int act_report_spravpozapros = 339;
        public const int act_report_serv_supp_nach_tula = 340;
        public const int act_report_serv_supp_money_tula = 341;
        public const int act_report_vipis_counters = 342;
        public const int act_report_list_dom_faktura = 346;
        public const int act_report_vipis_ls_tula = 351;
        public const int act_report_svid_registr = 879;
        public const int act_report_AllOver = 343;
        public const int act_report_AllAgreement = 344;
        public const int act_report_AllPrikazt = 345;
        public const int act_report_oplata_uslug_za_post_uslugi = 244;
        public const int act_report_Inf_PoRashet_SNasel = 245;
        public const int act_report_energo_act = 246;
        public const int act_report_report_Nachisleniya = 247;
        public const int act_report_sprav_pu_ls = 248;
        public const int act_report_norma_potr = 249;
        public const int act_report_sos_gil_fond = 250;
        public const int act_report_dom_nach_pere = 251;
        public const int act_report_sprav_nach_pu = 252;
        public const int act_report_spis_dolg = 253;
        public const int act_report_ved_dolg = 254;
        public const int act_report_nach_opl_serv = 255;
        public const int act_report_dom_odpu = 256;
        public const int act_report_ved_opl = 257;
        public const int act_report_ved_pere = 258;
        public const int act_report_make_kvit = 259;
        public const int act_report_protokol_odn = 260;
        public const int act_report_calc_tarif = 261;
        public const int act_report_oplata_uslug_za_post_uslugi_svod = 262;
        public const int act_report_order_list = 263;
        public const int act_report_count_orders_serv = 264;
        public const int act_report_count_orders_supp = 265;
        public const int act_report_10_14_3 = 266;
        public const int act_report_10_14_1 = 267;
        public const int act_report_spravsmg2_smr = 268;
        public const int act_report_spravsmg2 = 337;
        public const int act_report_zakaz = 269;
        public const int act_report_sprav_group = 270;
        public const int act_report_sprav_po_otkl_usl_dom_vinovnik = 271;
        public const int act_report_sprav_po_otkl_usl_geu_vinovnik = 272;
        public const int act_report_planned_works_list_supp = 273;
        public const int act_report_planned_works_list = 274;
        public const int act_report_planned_works_list_act = 275;
        public const int act_report_count_joborder_dest = 276;
        public const int act_report_info_from_service = 279;
        public const int act_report_appinfo_from_service = 278;
        public const int act_report_joborder_period_outstand = 277;
        public const int act_report_count_order_readres = 280;
        public const int act_report_message_list = 281;
        public const int act_report_message_quest_list = 282;
        public const int act_report_spis_gil_mod = 283;
        public const int act_report_sprav_reg_old = 284;
        public const int act_report_sverka_day = 285;
        public const int act_report_sverka_period = 286;
        public const int act_report_control_distrib_payments = 287;
        public const int act_report_register_counters = 288;
        public const int act_report_nach_supp = 289;
        public const int act_report_sald_statment_services = 290;
        public const int act_report_charges = 291;
        public const int act_report_pu_data = 292;
        public const int act_report_rashod_pu = 293;
        public const int act_report_prot_calc_odn = 294;
        public const int act_report_pasp_ras = 295;
        public const int act_report_sprav_supp = 299;
        public const int act_report_ls_pu_vipiska = 300;
        public const int act_report_spravka_na_privatizac2_smr = 301;
        public const int act_report_spravka_na_privatizac2 = 338;
        public const int act_report_prot_calc_odn2 = 302;
        public const int act_report_upload_kassa_3_0 = 303;
        public const int daily_payments = 304;
        public const int act_report_sverka_month = 305;
        public const int act_report_saldo_v_bank = 306;
        public const int act_report_gub_curr_charge = 307;
        public const int act_report_gub_itog_oplat = 308;
        public const int act_report_sprav_suppnachhar2 = 309;
        public const int act_report_nach_opl_serv2 = 310;
        public const int stat_gilfond = 311;
        public const int act_report_raspiska_docum = 312;
        public const int act_report_svid_reg_preb = 313;
        public const int act_report_spravpozapros_gub = 314;
        public const int act_report_vipiska_ls = 315;
        public const int act_report_zay_privat = 316;
        public const int act_report_nachisl_ls = 317;
        public const int act_report_nachisl_dom = 318;
        public const int act_report_nachisl_uch = 319;
        public const int act_report_upload_ipu = 320;
        public const int act_report_upload_charge = 321;
        public const int act_report_upload_reestr = 322;
        public const int act_report_soc_protection = 323;
        public const int act_report_saldo_ved_energo = 324;
        public const int act_report_dolg_ved_energo = 325;
        public const int act_report_protocol_sver_data = 326;
        public const int act_report_protocol_sver_data_ls_dom = 327;
        public const int act_report_pasp_ras_gub = 328;
        public const int act_report_sprav_na_prog = 330;
        public const int act_report_sprav_o_smert_kazan = 331;
        public const int act_report_sprav_po_mest_treb = 878;
        public const int act_report_vrem_zareg = 332;
        public const int act_report_sobsv = 333;
        public const int act_report_voenkomat = 334;
        public const int act_report_vip_kvar = 335;
        public const int act_report_vip_dom_gas = 336;
        public const int act_aa_showuk = 721;
        public const int act_aa_showbd = 722;
        public const int act_aa_showul = 723;
        public const int act_as_showsupp = 724;
        public const int act_as_showserv = 725;
        public const int act_as_showuk = 726;
        public const int act_findls = 501;
        public const int act_findprm = 502;
        public const int act_findch = 503;
        public const int act_findgil3 = 504;
        public const int act_findpu = 505;
        public const int act_findnedop = 506;
        public const int act_findodn = 507;
        public const int act_findserv = 508;
        public const int act_findsupg = 509;
        public const int act_findgroup = 510;
        public const int act_findpack = 511;
        public const int act_findplannedworks = 512;
        public const int act_finddebt = 513;
        public const int act_findeals = 514;
        public const int act_mode_edit = 611;
        public const int act_mode_view = 610;
        public const int act_online = 530;
        public const int act_blocked = 531;
        public const int act_process_in_queue = 532;
        public const int act_process_active = 533;
        public const int act_process_finished = 534;
        public const int act_process_with_errors = 535;
        public const int rowsview_20 = 701;
        public const int rowsview_50 = 702;
        public const int rowsview_100 = 703;
        public const int rowsview_10 = 705;
        public const int act_ubil = 851;
        public const int act_actual = 852;
        public const int act_neuch = 853;
        public const int act_arx = 854;
        public const int act_open_gilkart = 861;
        public const int act_open_gilper = 862;
        public const int act_open_dossier = 863;
        public const int act_gil_owner = 551;
        public const int act_responsible = 552;
        public const int role_sql = 101;
        public const int role_sql_wp = 101;
        public const int role_sql_area = 102;
        public const int role_sql_geu = 103;
        public const int role_sql_subrole = 105;
        public const int role_sql_prm = 106;
        public const int role_sql_ext = 107;
        public const int role_sql_supp = 120;
        public const int role_sql_serv = 121;
        public const int role_sql_bank = 122;
        public const int role_sql_payer = 123;
        public const int role_sql_server = 124;
        public const int role_sql_town = 125;
        public const int role_ext_mm = 201;
        public const int role_ext_pm = 202;
        public const int role_valid_srv = 210;
        public const int role_invalid_id = 211;
        public const int svc_normal = 0;
        public const int svc_wrongdata = -1;
        public const int svc_sqlerror = -2;
        public const int svc_pk_Format = -3;
        public const int svc_pk_Prefix = -4;
        public const int svc_pk_NumLs = -5;
        public const int svc_pk_Bit = -6;
        public const int svc_pk_NotUk = -7;
        public const int svc_pk_NotLs = -8;
        public const string encryptpathkey = "zRZ1/BHciushX1T9Ks5WwdRNjgjEEGzFypuKZTmkAOE=";
        public const string encryptrequestkey = "4PSckw3IcgQ/at00qGJ2RPcvDvmr=UfjcSm64cXLycw";
        public const int getInfo_supp = 1;
        public const int getInfo_area = 2;
        public static string cons_Webdata;
        public static string cons_Kernel;
        public static string cons_User;
        public static bool Viewerror;
        public static bool Debug;
        public static bool Trace;

        public static string ExcelDir
        {
            get
            {
                return Constants.Directories.ReportDir;
            }
        }

        public static string FilesDir
        {
            get
            {
                return Constants.Directories.FilesDir;
            }
        }

        public static bool UseExtendedConnectionfactory { get; set; }

        public static int RowsGrid(string rowsview)
        {
            int num = 20;
            int result = 0;
            int.TryParse(rowsview, out result);
            switch (result)
            {
                case 701:
                    num = 20;
                    break;
                case 702:
                    num = 50;
                    break;
                case 703:
                    num = 100;
                    break;
                case 705:
                    num = 10;
                    break;
            }
            return num;
        }

        public class Directories
        {
            private static string _FilesDir = "~/files/";
            private static string _AbsoluteRootPath = "";

            public static string BillDir
            {
                get
                {
                    return Constants.Directories.FilesDir + "bill/";
                }
            }

            public static string BillAbsoluteDir
            {
                get
                {
                    return Constants.Directories.BillDir.Replace("~", Constants.Directories._AbsoluteRootPath);
                }
            }

            public static string BillWebDir
            {
                get
                {
                    return Constants.Directories.BillDir + "web/";
                }
            }

            public static string BillWebAbsoluteDir
            {
                get
                {
                    return Constants.Directories.BillWebDir.Replace("~", Constants.Directories._AbsoluteRootPath);
                }
            }

            public static string FilesDir
            {
                get
                {
                    return Constants.Directories._FilesDir;
                }
                set
                {
                    Constants.Directories._FilesDir = value;
                }
            }

            public static string ReportDir
            {
                get
                {
                    return Constants.Directories.FilesDir + "reports/";
                }
            }

            public static string ReportsAbsoluteDir
            {
                get
                {
                    return Constants.Directories.ReportDir.Replace("~", Constants.Directories._AbsoluteRootPath);
                }
            }

            public static string ImportDir
            {
                get
                {
                    return Constants.Directories.FilesDir + "import/";
                }
            }

            public static string ImportAbsoluteDir
            {
                get
                {
                    return Constants.Directories.ImportDir.Replace("~", Constants.Directories._AbsoluteRootPath);
                }
            }

            public static string ActsOfSupplyDir
            {
                get
                {
                    return Constants.Directories.FilesDir + "subsidy/acts/";
                }
            }

            public static string ActsOfSupplyAbsoluteDir
            {
                get
                {
                    return Constants.Directories.ActsOfSupplyDir.Replace("~", Constants.Directories._AbsoluteRootPath);
                }
            }

            public static string THGFDir
            {
                get
                {
                    return Constants.Directories.FilesDir + "subsidy/thgf/";
                }
            }

            public static string THGFAbsoluteDir
            {
                get
                {
                    return Constants.Directories.THGFDir.Replace("~", Constants.Directories._AbsoluteRootPath);
                }
            }

            public static void SetRootAbsolutePath(string path)
            {
                Constants.Directories._AbsoluteRootPath = path;
            }
        }
    }
}
