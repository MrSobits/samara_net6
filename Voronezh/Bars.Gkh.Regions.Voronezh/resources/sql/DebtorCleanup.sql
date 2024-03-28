-- Function: public.debtor_cleanup()

-- DROP FUNCTION public.debtor_cleanup();

CREATE OR REPLACE FUNCTION public.debtor_cleanup()
  RETURNS void AS
$BODY$
begin
  /*Обновление признака наличия выписки*/

 UPDATE regop_debtor
  SET extract_exists = 20, rosreg_acc_matched = 20;

  UPDATE regop_debtor
  SET extract_exists = 10, rosreg_acc_matched = 10
  WHERE id in
        (SELECT distinct rd.id
         from regop_debtor rd
                JOIN regop_pers_acc rpa on rd.account_id = rpa.id
                JOIN rosreg.extractegrn egrn on rpa.room_id = egrn.roomid);

  /*Снятие признака у помещений с расхождениями в площади*/
  update regop_debtor
  set  rosreg_acc_matched = 20
  where id in (select rd.id
               from regop_debtor rd
                      join regop_pers_acc rpa on rd.account_id = rpa.id
               where rpa.room_id in (select rrd.roomid
                                     from rosreg.extractegrn rrd
                                            join gkh_room r on rrd.roomid = r.id
                                     where rrd.area != r.carea));

  /*Снятие признака у помещений с расхождениями в ФИО*/
  drop table if exists tmp_a;
  create temp table tmp_a as (select distinct room_id, trim(lower(rpao.name)) nn
                              from regop_debtor rd
                                     join regop_pers_acc rpa on rd.account_id = rpa.id
                                     join regop_pers_acc_owner rpao on rpa.acc_owner_id = rpao.id
                              where extract_exists = 10 and rosreg_acc_matched = 10
                                and owner_type = 0);

  drop table if exists tmp_ex;
  create temp table tmp_ex as (select distinct egrn.extractid                                                                id,
                                               lower(
                                                   concat_ws(' ', surname, firstname, patronymic)) nn,
                                               surname                                                   f,
                                               firstname                                                     i,
                                               patronymic                                                o,
                                               roomid,
                                               ind.id                                                                 pid
                               from rosreg.extractegrn egrn
                                      join rosreg.extractegrnright egr on egrn.id=egr.egrnid
                                      join rosreg.extractegrnrightind ind on egr.id = ind.rightid
                               where egrn.roomid in (select room_id
                                                         from regop_debtor rd
                                                                join regop_pers_acc rpa on rd.account_id = rpa.id
                                                                join regop_pers_acc_owner rpao on rpa.acc_owner_id = rpao.id
                                                         where extract_exists = 10 and rosreg_acc_matched = 10
                                                           and owner_type = 0)
                               order by pid);

  drop table if exists tmp_r;
  create temp table tmp_r as (select tmp_ex.roomid,tmp_ex.nn
                              from tmp_a
                                     join tmp_ex on tmp_ex.roomid = tmp_a.room_id
                              where tmp_a.nn = tmp_ex.nn);

  update regop_debtor
  set rosreg_acc_matched=20
  where id in
        (
          select rd.id
          from regop_debtor rd
                 join regop_pers_acc rpa on rd.account_id = rpa.id
                 join regop_pers_acc_owner rpao on rpa.acc_owner_id = rpao.id
          where extract_exists = 10 and rosreg_acc_matched = 10
            and owner_type = 0
            and room_id in (select room_id from tmp_a where room_id not in (select distinct roomid from tmp_r))
        );

  /*Удаление должников по заданным адресам, полученным от ФКР Воронежа*/
  delete
  from regop_debtor
  where id in (select rd.id
               from regop_debtor rd
                      join regop_pers_acc rpa on rd.account_id = rpa.id
                      join gkh_room r on rpa.room_id = r.id
                      join gkh_reality_object gro on r.ro_id = gro.id
               where
                  --Batch#1
                 /*Ул. Старых Большевиков, д. 98, к. 83
                 Цимлянская д. 8, к. 142
                 Кулибина, д. 5Б, к. 6
                 Героев Стратосферы д. 2., к. 33
                 Ростовская д. 37, к. 10
                 Урицкого, д. 69, к. 304
                 Урицкого, д. 69, к. 313
                 Урицкого, д. 69, к. 120
                 Солнечная д. 7, к. 7*/
                   gro.address || ', к. ' || r.croom_num ilike '%Старых Большевиков, д. 98, к. 83%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Цимлянская, д. 8, к. 142%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Кулибина, д. 5Б, к. 6%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Героев Стратосферы, д. 2, к. 33%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Ростовская, д. 37, к. 10%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Урицкого, д. 69, к. 304%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Урицкого, д. 69, к. 313%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Урицкого, д. 69, к. 120%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Солнечная, д. 7, к. 7%'

                  --Batch#2
                 /*
                 Лозовская Олеся Васильевна- ул. Меркулова. Д. 5, к.21
                 Юрова Нина Николаевна – 25 Октября, д. 31, к. 507
                 Северинова Марина Игоревна – Березовая Роща, д.  48, кв.9, к.2
                 Калинина Анна Егоровна – 45 Стрелковой дивизии, д. 44, кв. 7, к. 1
                 Юров Евгений Александрович - 25 Октября, д. 31, к. 503
                 */
                  or gro.address || ', к. ' || r.croom_num ilike '%Меркулова, д. 5, к. 21%'
                  or gro.address || ', к. ' || r.croom_num ilike '%25 Октября, д. 31, к. 507%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Березовая роща, д. 48, к. 9%'
                  or gro.address || ', к. ' || r.croom_num ilike '%45 стрелковой дивизии, д. 44, к. 7%'
                  or gro.address || ', к. ' || r.croom_num ilike '%25 Октября, д. 31, к. 503%'

                  --Batch#3
                 /*
                 Пеше-Стрелецкая, д. 77 все  комнаты. Это общежитие
                 9 Января, д. 57, д. 7
                 Циолковского, д. 15, к. 55,
                 Циолковского, д. 15, к. 57,
                 Циолковского, д. 13, к. 7,
                 Циолковского, д. 13, к. 39,
                 Циолковского, д. 13, к. 52,
                 Циолковского, д. 13, к. 2,

                 ул. Южно- Моравская, д. 33, к. 38,
                 ул. Южно- Моравская, д. 33, к. 101,
                 ул. Южно- Моравская, д. 33, к. 1,
                 ул. Южно- Моравская, д. 33, к. 120,

                 Ул. Торпедо, д. 38, к. 17,

                 Ул. Теплоэнергетиков, д. 8, к. 215,
                 Ул. Теплоэнергетиков, д. 8, к. 522
                 */
                  or gro.address ilike '%Пеше-Стрелецкая, д. 77%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Циолковского, д. 15, к. 55%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Циолковского, д. 15, к. 57%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Циолковского, д. 13, к. 7%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Циолковского, д. 13, к. 39%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Циолковского, д. 13, к. 52%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Циолковского, д. 13, к. 2%'
                  or gro.address || ', к. ' || r.croom_num ilike '%ул. Южно-Моравская, д. 33, к. 38%'
                  or gro.address || ', к. ' || r.croom_num ilike '%ул. Южно-Моравская, д. 33, к. 101%'
                  or gro.address || ', к. ' || r.croom_num ilike '%ул. Южно-Моравская, д. 33, к. 1%'
                  or gro.address || ', к. ' || r.croom_num ilike '%ул. Южно-Моравская, д. 33, к. 120%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Ул. Торпедо, д. 38, к. 17%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Ул. Теплоэнергетиков, д. 8, к. 215%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Ул. Теплоэнергетиков, д. 8, к. 522%'

                  --Batch#4
                 /*
                 ул. 9 Января, д. 230. к. 14
                 Варейкиса, д. 72, к. 29
                 Антокольского, д. 8, к. 142
                 Антокольского, д. 8, к. 111
                 */
                  or gro.address || ', к. ' || r.croom_num ilike '%ул. 9 Января, д. 230. к. 14%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Варейкиса, д. 72, к. 29%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Антокольского, д. 8, к. 142%'
                  or gro.address || ', к. ' || r.croom_num ilike '%Антокольского, д. 8, к. 111%'

                  --Batch#5
                 /*
                 Ул. 9 Января, д. 48, к. 304
                 Беговая, д. 49, кв.2, к. 2
                 Ул. Любы Шевцовой, д. 17А, кв. 38, к.2
                 Ул. Розы Люксембург, д. 103, кв. 1, к. 1
--&& 5.5
                 ул. Космонавтов, д. 38, кв. 6
                 */
                  or gro.address || ', к. ' || r.croom_num ilike '%Ул. 9 Января, д. 48, к. 304%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Беговая, д. 49, кв. 2%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Ул. Любы Шевцовой, д. 17А, кв. 38%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%ул. Космонавтов, д. 38, кв. 6%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Ул. Розы Люксембург, д. 103, кв. 1%'
                  --Batch#6
                 /*
                 Г. Борисоглебск, ул. Матросовская, д.33, кв.70
                 г. Борисоглебск, ул. Матросовская, д.33, кв.89
                 г. Воронеж, ул. Хользунова, д. 3, кв.62
                 */
                  or gro.address || ', кв. ' || r.croom_num ilike '%Г. Борисоглебск, ул. Матросовская, д. 33, кв. 70%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%г. Борисоглебск, ул. Матросовская, д. 33, кв. 89%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%г. Воронеж, ул. Хользунова, д. 3, кв. 62%'
                  --&& 6.5
                 /*
                 Г. Воронеж, ул. 25 Января, д. 48, кв. 29, к.3
                 Г. Воронеж, ул. 25 Января, д. 40, кв. 44, к. 2
                 */
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. 25 Января, д. 48, кв. 29%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. 25 Января, д. 40, кв. 44%'
                  --Batch#7 Спец. счет
                 /*
                 г. Воронеж, ул. Зои Космодемьянской, д. 13
                 г. Воронеж, ул. Зои Космодемьянской, д. 19
                 */
                  or gro.address like '%г. Воронеж, ул. Зои Космодемьянской, д. 13%'
                  or gro.address like '%г. Воронеж, ул. Зои Космодемьянской, д. 19%'
                  --Batch#7.5 Спец. счет
                 /*
                 г. Воронеж, ул. Генерала Лизюкова, д. 44
                 */
                  or gro.address like '%г. Воронеж, ул. Генерала Лизюкова, д. 44%'
                  --Batch#8
                 /*
           ул. Театральная, д. 32, кв. 90, кв. 47.
           */
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. Театральная, д. 32, кв. 90%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. Театральная, д. 32, кв. 47%'
                  --Batch#8.5
                 /*
     ул. Депутатской, д. 11, кв. 50, 59, 64, 80
     ул. 9 Января, д.57, кв. 6
                 */
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. Депутатская, д. 11, кв. 50%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. Депутатская, д. 11, кв. 59%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. Депутатская, д. 11, кв. 64%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. Депутатская, д. 11, кв. 80%'
                  or gro.address || ', кв. ' || r.croom_num ilike '%Воронеж, ул. 9 Января, д.57, кв. 6%'
  );
  /*Удаление из реестра должников коммунальных квартир*/
  delete
  from regop_debtor
  where id in (select rd.id
               from regop_debtor rd
                      join regop_pers_acc rpa on rd.account_id = rpa.id
               where rpa.acc_num in (select * from import.temp_communal));
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION public.debtor_cleanup()
  OWNER TO bars;
