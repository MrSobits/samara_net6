select
	'Камчатский край'::text as "Регион",
	mu.name as "МР",
	stl.name as "МО",
	fa.place_name as "Населенный пункт",
	ro.address as "Улица, дом, литер, секция",
	r.croom_num as "Номер помещения",
	ca.account_number as "Расчетный счет",
	case
		when ca.type_account=10 then 'Счет регоператора'
		when ca.type_account=20 then 'Специальный счет'
	end as "Тип счета",
	
	roca.acc_num as "Счет начислений дома",
	pa.acc_num as "ЛС",
	tt.type_charge as "Тип начисления",
	tt.charge as "Сумма начисления",
	tt.recalc as "Перерасчет",
	tt.type_debt as "Тип задолженности",
	tt.debt as "Сумма задолженности",
	extract(YEAR FROM p.cstart) as "Период год",
	to_char(p.cstart, 'TMMonth') as "Период месяц"

from (

--Базовый тариф
	select
		ps.account_id,
		ps.period_id,
		ps.charge_base_tariff + ps.balance_change as charge,
		ps.charge_base_tariff + ps.balance_change - ps.tariff_payment as debt,
		0 as recalc,
		'Начисление по базовому тарифу' as type_charge,
		'Задолженность по базовому тарифу' as type_debt
	from regop_pers_acc_period_summ ps

	union all

--сверх базового тарифа
	select
		ps.account_id,
		ps.period_id,
		ps.CHARGE_TARIFF - ps.CHARGE_BASE_TARIFF as charge,
		ps.CHARGE_TARIFF - ps.CHARGE_BASE_TARIFF - ps.tariff_desicion_payment as debt,
		0 as recalc,
		'Начисление сверх базового тарифа' as type_charge,
		'Задолженность сверх базового тарифа' as type_debt
	from regop_pers_acc_period_summ ps

	union all

--начисления пени
	select
		ps.account_id,
		ps.period_id,
		ps.penalty as charge,
		ps.penalty - ps.penalty_payment as debt,
		0 as recalc,
		'Начисление пени' as type_charge,
		'Задолженность пени' as type_debt
	from regop_pers_acc_period_summ ps

	union all
	
--перерасчет
	select
		ps.account_id,
		ps.period_id,
		0 as charge,
		0 as debt,
		ps.recalc as recalc,
		'' as type_charge,
		'' as type_debt
	from regop_pers_acc_period_summ ps
) tt
	inner join regop_period p on p.id = tt.period_id
	inner join regop_pers_acc pa on pa.id = tt.account_id
	inner join gkh_room r on r.id = pa.room_id
	inner join gkh_reality_object ro on ro.id = r.ro_id
	inner join b4_fias_address fa on fa.id = ro.fias_address_id
	inner join gkh_dict_municipality mu on mu.id = ro.municipality_id
	left join gkh_dict_municipality stl on stl.id = ro.STL_MUNICIPALITY_ID
	inner join regop_ro_charge_account roca on roca.ro_id = ro.id
	left join regop_calc_acc_ro caro on caro.ro_id = ro.id and caro.date_start <= p.cstart and (caro.date_end is null or caro.date_end > p.cstart)
	left join regop_calc_acc ca on ca.id = caro.account_id and ca.type_owner = 20