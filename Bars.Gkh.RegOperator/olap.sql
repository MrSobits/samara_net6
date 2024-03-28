select * from (SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  p_acc.acc_num                                       AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Счет ЛС'                                           AS "Источник поступления",
  coalesce(imp_sus.payment_agent_name, imp_pay.payment_agent_name) as "Платежный агент",
  coalesce(imp_sus.document_number, imp_pay.document_number) as "Номер сводного реестра"

FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_wallet w ON w.id = p_acc.bt_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND (mop.reason is null or mop.reason != 'Слияние счетов')
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id
  -- связь с платежными агентами со стороны НВС
  left join regop_suspense_account susp on susp.c_guid = t.target_guid
  left join regop_imported_payment imp_item_sus on imp_item_sus.payment_id = susp.id and imp_item_sus.payment_state = 20 /*нвс*/
  left join regop_bank_doc_import imp_sus on imp_sus.id = imp_item_sus.bank_doc_id
  -- связь с платежными агентами со стороны НВС
  left join regop_unaccepted_pay pay on pay.transfer_guid = t.target_guid
  left join regop_imported_payment imp_item_pay on imp_item_pay.payment_id = pay.id and imp_item_pay.payment_state = 10 /*ноп*/
  left join regop_bank_doc_import imp_sus on imp_sus.id = imp_item_sus.bank_doc_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  p_acc.acc_num                                       AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Счет ЛС'                                           AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_wallet w ON w.id = p_acc.dt_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND (mop.reason is null or mop.reason != 'Слияние счетов')
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  p_acc.acc_num                                       AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Счет ЛС'                                           AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_wallet w ON w.id = p_acc.p_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND (mop.reason is null or mop.reason != 'Слияние счетов')
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  '-'                                                 AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Субсидия фонда'                                    AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.fsu_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  '-'                                                 AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Региональная субсидия'                             AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.rsu_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  '-'                                                 AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Стимулирующая субсидия'                            AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.ssu_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  '-'                                                 AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Целевая субсидия'                                  AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.tsu_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  '-'                                                 AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'иные поступления'                                  AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.os_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                             AS "Рег оператор",
  calc_acc.account_number                             AS "Расчетный счет",
  ro.address                                          AS "Дом",
  pay_acc.acc_num                                     AS "Счет дома",
  '-'                                                 AS "ЛС",
  t.operation_date                                    AS "Дата операции",
  t.payment_date                                      AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')                AS "Период",
  extract(YEAR FROM t.operation_date)                 AS "Год",
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS "Тип операции",
  t.amount                                            AS "Сумма",
  'Процент банка'                                     AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.bp_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

/*Отмены*/

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  p_acc.acc_num                               AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Счет ЛС'                                   AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_wallet w ON w.id = p_acc.bt_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  p_acc.acc_num                               AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Счет ЛС'                                   AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_wallet w ON w.id = p_acc.dt_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  p_acc.acc_num                               AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Счет ЛС'                                   AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_wallet w ON w.id = p_acc.p_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  '-'                                         AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Субсидия фонда'                            AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.fsu_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  '-'                                         AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Региональная субсидия'                     AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.rsu_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  '-'                                         AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Стимулирующая субсидия'                    AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.ssu_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  '-'                                         AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Целевая субсидия'                          AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.tsu_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  '-'                                         AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'иные поступления'                          AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.os_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id

UNION ALL

SELECT
  ca.name                                     AS "Рег оператор",
  calc_acc.account_number                     AS "Расчетный счет",
  ro.address                                  AS "Дом",
  pay_acc.acc_num                             AS "Счет дома",
  '-'                                         AS "ЛС",
  t.operation_date                            AS "Дата операции",
  t.payment_date                              AS "Дата поступления",
  to_char(t.operation_date, 'TMMonth')        AS "Период",
  extract(YEAR FROM t.operation_date)         AS "Год",
  '(отмена)' || coalesce(t.reason, tt.reason) AS "Тип операции",
  -1 * t.amount                               AS "Сумма",
  'Процент банка'                             AS "Источник поступления"
FROM regop_calc_acc_regop calc_acc_regop
  LEFT JOIN regop_calc_acc calc_acc ON calc_acc.id = calc_acc_regop.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN GKH_CONTR_BANK_CR_ORG cr_org ON cr_org.id = calc_acc_regop.contr_credit_org_id
  LEFT JOIN OVRHL_CREDIT_ORG cr ON cr.id = cr_org.credit_org_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc_regop.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN regop_wallet w ON w.id = pay_acc.bp_wallet_id
  INNER JOIN regop_transfer t ON t.source_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND mop.canceled_op_id IS NOT NULL
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id) as query
where "Тип операции" != 'Перенос долга при слиянии'