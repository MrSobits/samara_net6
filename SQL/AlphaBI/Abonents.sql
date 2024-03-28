select case when owner.owner_type = 0 then 'Физ. лицо' else 'Юр. лицо' end as "Тип абонента",
       owner.name as "ФИО абонента", 
       ro.id as "Id дома абонента",
       ro.address as "Адрес дома",
       room.croom_num "№ квартиры",
       acc.acc_num "Номер ЛС абонента",
       acc.area_share as "Доля собственности",
       case   when   ovrhl_paysize.PAYMENT_SIZE is not null and log.CPROP_VALUE is null then ovrhl_paysize.PAYMENT_SIZE 
              when  log.CPROP_VALUE  is not null and ovrhl_paysize.PAYMENT_SIZE is null then cast(log.CPROP_VALUE as double precision)
              when  ovrhl_paysize.PAYMENT_SIZE >  cast(log.CPROP_VALUE as double precision) then  ovrhl_paysize.PAYMENT_SIZE  else cast(log.CPROP_VALUE as double precision) end as "Размер взноса"
from REGOP_PERS_ACC_OWNER owner,
     REGOP_PERS_ACC acc,
     gkh_room room,
     gkh_reality_object ro left join
    (
	select OVRHL_DICT_PAYSIZE.ID,
	  OVRHL_DICT_PAYSIZE.TYPE_INDICATOR,
	  OVRHL_PAYSIZE_MU_RECORD.MUNICIPALITY_ID,
	  OVRHL_DICT_PAYSIZE.PAYMENT_SIZE,
	  OVRHL_DICT_PAYSIZE.DATE_START_PERIOD,
	  OVRHL_DICT_PAYSIZE.DATE_END_PERIOD ,
	  OVRHL_DICT_PAYSIZE.DATE_END_PERIOD
	from   OVRHL_PAYSIZE_MU_RECORD 
	     inner join OVRHL_DICT_PAYSIZE
	     on OVRHL_PAYSIZE_MU_RECORD.PAYSIZECR_ID=OVRHL_DICT_PAYSIZE.ID 
	where OVRHL_DICT_PAYSIZE.TYPE_INDICATOR = 20
     ) ovrhl_paysize  on  ovrhl_paysize.municipality_id = ro.municipality_id
     left join 
     (  
      select ID,
		ENTITY_ID,
		CCLASS_NAME,
		CCLASS_DESC,
		CPROP_NAME ,
		CPROP_DESCR ,
		CPROP_VALUE,
		CDATE_APPLIED ,
		DATE_ACTUAL ,
		CDATE_END,
		FILE_ID,
		PARAM_NAME,
		CUSER_NAME,
		USED_IN_RECALC 
      from GKH_ENTITY_LOG_LIGHT 
      where cprop_name = 'base_tariff') log on log.ENTITY_ID = ovrhl_paysize.ID    
where room.id = acc.room_id
and ro.id = room.ro_id
and owner.id = acc_owner_id