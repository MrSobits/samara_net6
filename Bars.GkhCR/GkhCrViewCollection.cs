namespace Bars.GkhCr
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;

    using Gkh;

    public class GkhCrViewCollection : BaseGkhViewCollection
    {
        public override int Number
        {
            get
            {
                return 1;
            }
        }

        #region Функции
        #region Create

        /// <summary>
        /// возвращает сумму смет по id объекта кап ремонта
        /// crGetEstimateSum
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetEstimateSum(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetEstimateSum (ec_id NUMBER)
                           RETURN NUMBER
                        IS
                           n   NUMBER;
                        BEGIN
                           EXECUTE IMMEDIATE
                            'select coalesce(sum(est.total_cost),0)                            
                            from CR_EST_CALC_ESTIMATE est
                            where est.ESTIMATE_CALC_ID = ' || ec_id                             
                            INTO n;

                           RETURN n;
                        END;";
            }

            return @"CREATE OR REPLACE FUNCTION crGetEstimateSum (integer)
                           RETURNS numeric AS
		    $BODY$
                        select coalesce(sum(est.total_cost),0)                            
                            from CR_EST_CALC_ESTIMATE est
                            where est.ESTIMATE_CALC_ID = $1 ;
                    $BODY$
              LANGUAGE sql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// возвращает сумму ведомостей ресурсов по id объекта кап ремонта
        /// crGetEstimateSum
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetResStatemSum(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetResStatemSum (ec_id NUMBER)
                           RETURN NUMBER
                        IS
                           n   NUMBER;
                        BEGIN
                           EXECUTE IMMEDIATE
                            'select coalesce(sum(rs.total_cost),0)                            
                            from CR_EST_CALC_RES_STATEM rs
                            where rs.ESTIMATE_CALC_ID = ' || ec_id                             
                            INTO n;

                           RETURN n;
                        END; ";
            }

            return @"CREATE OR REPLACE FUNCTION crGetResStatemSum (integer)
                           RETURNS numeric AS
		     $BODY$
                        select coalesce(sum(rs.total_cost),0)                            
                            from CR_EST_CALC_RES_STATEM rs
                            where rs.ESTIMATE_CALC_ID = $1 ;
                    $BODY$
              LANGUAGE sql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// возвращает количество смет по id объекта кап ремонта
        /// crGetEstimateCalcCnt
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetEstimateCalcCnt(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetEstimateCalcCnt (crObjId IN number)
                RETURN number
                IS
                  n number;
                begin    
                  execute immediate 'select coalesce(count(est_calc.id),0)
                    from cr_obj_estimate_calc est_calc
                        left join cr_object obj on est_calc.object_id = obj.id where obj.id =' || crObjId INTO n;
                  RETURN n;
                end;";
            }

            return @"CREATE OR REPLACE FUNCTION crGetEstimateCalcCnt(r integer)
            RETURNS bigint AS
            $BODY$
                select coalesce(count(est_calc.id),0)
                from cr_obj_estimate_calc est_calc
	                left join cr_object obj on est_calc.object_id = obj.id
                where obj.id = $1
            $BODY$
                LANGUAGE sql VOLATILE
                COST 100;";
        }

        /// <summary>
        /// возвращает количество видов работ по id объекта кап ремонта
        /// crGetTypeWorkCnt
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetTypeWorkObjsCnt(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetTypeWorkCnt(crObjId IN number)
RETURN number
IS
  n number;
begin    
  execute immediate 'select coalesce(count(type_work.id),0)
    from cr_obj_type_work type_work
        left join cr_object obj on type_work.object_id = obj.id
    where obj.id =' || crObjId INTO n;
  RETURN n;
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION crGetTypeWorkCnt(integer)
RETURNS bigint AS
$BODY$
    select coalesce(count(type_work.id),0)
    from cr_obj_type_work type_work
       left join cr_object obj on type_work.object_id = obj.id
    where obj.id = $1
$BODY$
LANGUAGE sql VOLATILE
COST 100;";
        }

        /// <summary>
        /// возвращает сумму по видам работ по id объекта кап ремонта
        /// crGetTypeWorkSum
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetTypeWorkObjsSum(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetTypeWorkSum(crObjId IN number)
                RETURN number
                IS
                  n number;
                begin    
                  execute immediate 'select coalesce(sum(type_work.sum),0)
                    from cr_obj_type_work type_work
                        left join cr_object obj on type_work.object_id = obj.id
                    where obj.id = ' || crObjId INTO n;
                  RETURN n;
                end;";
            }

            return @"
            CREATE OR REPLACE FUNCTION crGetTypeWorkSum(integer)
            RETURNS numeric AS
            $BODY$
                select coalesce(sum(type_work.sum),0)
                from cr_obj_type_work type_work
                   left join cr_object obj on type_work.object_id = obj.id
                where obj.id = $1
            $BODY$
            LANGUAGE sql VOLATILE
            COST 100;";
        }

        /// <summary>
        /// возвращает сумму по видам работ по id объекта кап ремонта и типу работ
        /// crGetTypeWorkSumByType
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetTypeWorkObjsSumByType(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetTypeWorkSumByType(crObjId IN number, typeWorkId IN number)
RETURN number
IS
  n number;
begin    
  execute immediate 'select coalesce(sum(type_work.sum),0)
    from cr_obj_type_work type_work
        left join cr_object obj on type_work.object_id = obj.id
        left join gkh_dict_work wrk on type_work.work_id = wrk.id
    where obj.id = ' || crObjId || 'and wrk.type_work ='|| typeWorkId INTO n;
  RETURN n;
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION crGetTypeWorkSumByType(integer, integer)
RETURNS numeric AS
$BODY$
    select coalesce(sum(type_work.sum),0)
    from cr_obj_type_work type_work
	    left join cr_object obj on type_work.object_id = obj.id
	    left join gkh_dict_work wrk on type_work.work_id = wrk.id
    where obj.id = $1 and wrk.type_work = $2
$BODY$
  LANGUAGE sql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// возвращает сумму по видам работ по id объекта кап ремонта, типу работ и коду работы
        /// crGetTypeWorkSumByTypeCode
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetTypeWorkObjsSumByTypeCode(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetTypeWorkSumByTypeCode(crObjId IN number, typeWorkId IN number, code varchar2)
RETURN number
IS
  n number;
begin    
  execute immediate 'select coalesce(sum(type_work.sum),0)
    from cr_obj_type_work type_work
        left join cr_object obj on type_work.object_id = obj.id
        left join gkh_dict_work wrk on type_work.work_id = wrk.id
    where obj.id =' || crObjId || 'and wrk.type_work ='|| typeWorkId || 'and wrk.code =' || code INTO n;
  RETURN n;
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION crGetTypeWorkSumByTypeCode(integer, integer, character varying)
RETURNS numeric AS
$BODY$
    select 
        coalesce(sum(type_work.sum),0)
    from cr_obj_type_work type_work
	    left join cr_object obj on type_work.object_id = obj.id
	    left join gkh_dict_work wrk on type_work.work_id = wrk.id
    where obj.id = $1 and wrk.type_work = $2 and wrk.code = $3
$BODY$
LANGUAGE sql VOLATILE
COST 100;";
        }

        /// <summary>
        /// возвращает сумму по видам работ по id объекта кап ремонта, типу работы и кодам работ
        /// crGetTypeWorkSumByTypeCodes
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetTypeWorkObjsSumByTypeAndCodes(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION crGetTypeWorkSumByTypeCodes(crObjId IN number, typeWorkId IN number, code1 varchar2, code2 varchar2)
                RETURN number
                IS
                  n number;
                begin    
                  execute immediate 'select coalesce(sum(type_work.sum),0)
                    from cr_obj_type_work type_work
                        left join cr_object obj on type_work.object_id = obj.id
                        left join gkh_dict_work wrk on type_work.work_id = wrk.id
                    where obj.id =' || crObjId || 'and wrk.type_work ='|| typeWorkId || 'and (wrk.code =' || code1 || ' or wrk.code ='|| code2 || ')' INTO n;
                  RETURN n;
                end;";
            }

            return @"
            CREATE OR REPLACE FUNCTION crGetTypeWorkSumByTypeCodes(integer, integer, character varying, character varying)
            RETURNS numeric AS
            $BODY$
                select coalesce(sum(type_work.sum),0)
                from cr_obj_type_work type_work
	                left join cr_object obj on type_work.object_id = obj.id
	                left join gkh_dict_work wrk on type_work.work_id = wrk.id
                where obj.id = $1 and wrk.type_work = $2 and (wrk.code = $3 or wrk.code = $4)
            $BODY$
            LANGUAGE sql VOLATILE
            COST 100;";
        }

        #endregion Create
        #region Delete

        /// <summary>
        /// Удаление crGetEstimateSum
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetEstimateSum(DbmsKind dbmsKind)
        {
			var funcName = "crGetEstimateSum";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"drop function if exists crGetEstimateSum(integer)";
        }

        /// <summary>
        /// Удаление crGetResStatemSum
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetResStatemSum(DbmsKind dbmsKind)
        {
			var funcName = "crGetResStatemSum";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"drop function if exists crGetResStatemSum(integer)";
        }

        /// <summary>
        /// Удаление crGetEstimateCalcCnt
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetEstimateCalcCnt(DbmsKind dbmsKind)
        {
			var funcName = "crGetEstimateCalcCnt";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"drop function if exists crGetEstimateCalcCnt(integer)";
        }

        /// <summary>
        /// Удаление crGetTypeWorkCnt
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetTypeWorkObjsCnt(DbmsKind dbmsKind)
        {
			var funcName = "crGetTypeWorkCnt";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"DROP FUNCTION if exists crGetTypeWorkCnt(integer)";
        }

        /// <summary>
        /// Удаление crGetTypeWorkSum
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetTypeWorkObjsSum(DbmsKind dbmsKind)
        {
			var funcName = "crGetTypeWorkSum";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"DROP FUNCTION if exists crGetTypeWorkSum(integer)";
        }

        /// <summary>
        /// Удаление crGetTypeWorkSumByType
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetTypeWorkObjsSumByType(DbmsKind dbmsKind)
        {
			var funcName = "crGetTypeWorkSumByType";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"DROP FUNCTION if exists crGetTypeWorkSumByType(integer, integer)";
        }

        /// <summary>
        /// Удаление crGetTypeWorkSumByTypeCode
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetTypeWorkObjsSumByTypeCode(DbmsKind dbmsKind)
        {
			var funcName = "crGetTypeWorkSumByTypeCode";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"DROP FUNCTION if exists crGetTypeWorkSumByTypeCode(integer, integer, character varying)";
        }

        /// <summary>
        /// Удаление crGetTypeWorkSumByTypeCodes
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetTypeWorkObjsSumByTypeAndCodes(DbmsKind dbmsKind)
        {
			var funcName = "crGetTypeWorkSumByTypeCodes";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return this.DropFunctionOracleQuery(funcName);
            }

            return @"DROP FUNCTION if exists crGetTypeWorkSumByTypeCodes(integer, integer, character varying, character varying)";
        }

        #endregion Delete
        #endregion Функции

        #region Вьюхи
        #region Create

        /// <summary>
        /// Вьюха сметный расчет в объекте кап ремонта
        /// VIEW_CR_OBJ_EST_CALC_DET
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewCrObjEstCalcDet(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW VIEW_CR_OBJ_EST_CALC_DET AS
SELECT 
	ec.id,
	ec.object_id as object_id,
	ec.state_id,
	w.name as work_name,
    ec.total_estimate,
	fs.name as fin_source_name,
	(
		select 
		    sum(est.total_cost)
		from cr_est_calc_estimate est
		where est.estimate_calc_id = ec.id
	) as sum_estimate,
	(
		select 
		    sum(rs.total_cost)
		from CR_EST_CALC_RES_STATEM rs
		where rs.estimate_calc_id = ec.id 
	) as sum_resource,
	ec.estimation_type
FROM cr_obj_estimate_calc ec
	left join CR_OBJ_TYPE_WORK tw on tw.ID = ec.TYPE_WORK_CR_ID
	left join GKH_DICT_WORK w on w.ID = tw.WORK_ID
	left join CR_DICT_FIN_SOURCE fs on fs.ID = tw.fin_source_id";
        }

        /// <summary>
        /// Вьюха объектов кап ремонта
        /// view_cr_object
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewCrObject(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_cr_object AS
SELECT 
    cr.id,
    cr.program_num,
    cr.program_id,
    cr.before_delete_program_id,
    prog.name as program_name,
    beforedelprog.name as before_del_program_name,
    mu.id AS municipality_id,
    mu.name AS municipality_name,
    stl.id AS settlement_id,
    stl.name AS settlement_name,
    ro.id AS reality_object_id,
    ro.address,
    cr.date_accept_gji,
    cr.allow_reneg,
    smr.id as smr_id,
    st.id AS state_id,
    smr_st.id as smr_state_id,
    per.name as period_name,
    COALESCE(ro.method_form_fund, 0) as method_form_fund
FROM cr_object cr
    inner join gkh_reality_object ro ON ro.id=cr.reality_object_id 
    inner join gkh_dict_municipality mu ON ro.municipality_id =  mu.id
    left join gkh_dict_municipality stl ON ro.stl_municipality_id =  stl.id
    left join cr_dict_program prog ON cr.program_id = prog.id
    left join gkh_dict_period per on per.id = prog.PERIOD_ID
    left join cr_dict_program beforedelprog ON cr.before_delete_program_id = beforedelprog.id
    left join b4_state st on st.id = cr.state_id
    left join CR_OBJ_MONITORING_CMP smr on smr.object_id = cr.id
    left join b4_state smr_st on smr_st.id = smr.state_id";
        }

        /// <summary>
        /// вьюха смет капремонта
        /// view_cr_object_est_calc
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewCrObjectEstCalc(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_cr_object_est_calc AS 
SELECT DISTINCT
    obj.id,
    obj.id AS object_id, 
    ro.address, 
    ro.id AS reality_object_id,
    mu.name AS municipality_name, 
    mu.id AS municipality_id,
    stl.id AS settlement_id,
    stl.name AS settlement_name,  
    prog.id AS program_id, 
    prog.name AS program_name, 
    ( 
        SELECT 
            count(tw.id) AS count_tw
        FROM cr_obj_type_work tw
        JOIN GKH_DICT_WORK dw on dw.id = tw.WORK_ID
        WHERE tw.object_id = obj.id and dw.TYPE_WORK = 10
    ) AS cnt_tw, 
    ( 
        SELECT 
            count(ec.id) AS count_ec
        FROM cr_obj_estimate_calc ec
        WHERE ec.object_id = obj.id
    ) AS cnt_est_calc
FROM cr_object obj
       JOIN cr_obj_estimate_calc ec ON ec.object_id = obj.id
       JOIN gkh_reality_object ro ON obj.reality_object_id = ro.id
       JOIN gkh_dict_municipality mu ON ro.municipality_id = mu.id
       JOIN cr_dict_program prog ON obj.program_id = prog.id
       left join gkh_dict_municipality stl ON ro.stl_municipality_id =  stl.id
WHERE prog.type_visibility <> 20 AND prog.type_visibility <> 30";
        }

        /// <summary>
        /// вьюха смет капремонта
        /// view_cr_special_object_est_calc
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewCrSpecialObjectEstCalc(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_cr_special_object_est_calc AS 
SELECT DISTINCT
    obj.id,
    obj.id AS object_id, 
    ro.address, 
    ro.id AS reality_object_id,
    mu.name AS municipality_name, 
    mu.id AS municipality_id,
    stl.id AS settlement_id,
    stl.name AS settlement_name,  
    prog.id AS program_id, 
    prog.name AS program_name, 
    ( 
        SELECT 
            count(tw.id) AS count_tw
        FROM cr_special_obj_type_work tw
        JOIN GKH_DICT_WORK dw on dw.id = tw.WORK_ID
        WHERE tw.object_id = obj.id and dw.TYPE_WORK = 10
    ) AS cnt_tw, 
    ( 
        SELECT 
            count(ec.id) AS count_ec
        FROM CR_SPECIAL_OBJ_ESTIMATE_CALC ec
        WHERE ec.object_id = obj.id
    ) AS cnt_est_calc
FROM cr_special_object obj
       JOIN CR_SPECIAL_OBJ_ESTIMATE_CALC ec ON ec.object_id = obj.id
       JOIN gkh_reality_object ro ON obj.reality_object_id = ro.id
       JOIN gkh_dict_municipality mu ON ro.municipality_id = mu.id
       JOIN cr_dict_program prog ON obj.program_id = prog.id
       left join gkh_dict_municipality stl ON ro.stl_municipality_id =  stl.id
WHERE prog.type_visibility <> 20 AND prog.type_visibility <> 30";
        }

        /// <summary>
        /// вьюха квалификационного отбора
        /// view_cr_object_qualification
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewCrObjectQualification(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE VIEW view_cr_object_qualification
AS
   SELECT o.id,
          p.name AS program,
          m.name AS municipality,
          ro.address AS address,
          (
            SELECT SUM (tw.SUM)
             FROM cr_obj_type_work tw
                  LEFT JOIN cr_dict_fin_source f ON f.id = tw.fin_source_id
            WHERE f.type_finance = 40 AND tw.object_id = o.id
          ) AS type_work_sum,
          rtng.name AS builder,
          CASE WHEN rtng.rating Is NOT NULL THEN  rtng.rating ||' из ' ||(select count(1) from cr_dict_qual_member qm where qm.period_id = per.id)
          else '' end
           as rating, 
          (SELECT COUNT (1)
             FROM cr_dict_qual_member qm
            WHERE qm.period_id = per.id)
             AS qual_member_count
     FROM cr_object o
          JOIN cr_dict_program p ON p.id = o.program_id AND p.type_program_state = 30
          JOIN gkh_dict_period per ON   per.id = p.period_id --AND per.date_start < CURRENT_DATE AND (per.date_end IS NULL OR per.date_end > CURRENT_DATE)
          LEFT JOIN gkh_reality_object ro ON ro.id = o.reality_object_id
          LEFT JOIN gkh_dict_municipality m ON m.id = ro.municipality_id
          LEFT JOIN
          (
              SELECT 
              q1.OBJECT_ID, 
              ff.name,
              ff.ss,
              ff.cnt AS rating,
              row_number() OVER (PARTITION BY q1.OBJECT_ID order by ff.ss desc) rn
              FROM cr_qualification q1
              JOIN
              (
                   SELECT 
                   q.Id,
                   c.name,
                   count(1) cnt,
                   sum(CASE WHEN cdqm.IS_PRIMARY = 1 THEN 1000 else 1 end) ss
                   FROM cr_voice_qual_member vm
                        JOIN cr_qualification q ON q.id = vm.qualification_id
                        JOIN gkh_builder b ON b.id = q.builder_id
                        JOIN gkh_contragent c ON c.id = b.contragent_id
                        JOIN CR_DICT_QUAL_MEMBER cdqm on cdqm.ID = vm.QUAL_MEMBER_ID
                        WHERE vm.type_accept_qual = 20
                  group by q.id, c.name
               ) ff ON ff.ID = q1.ID
               GROUP BY q1.OBJECT_ID, ff.name, ff.ss,ff.cnt
          
          ) rtng On rtng.object_id = o.id and rn= 1";
            }

            return @"
CREATE OR REPLACE VIEW view_cr_object_qualification AS 
select
  o.id,
  p.name as program,
  m.name as municipality,
  ro.address as address,
  (
    select sum(tw.sum) 
    from cr_obj_type_work  tw
    left join cr_dict_fin_source f on f.id = tw.fin_source_id 
    where f.type_finance = 40 and tw.object_id = o.id
   ) as type_work_sum,
   (
       select 
       c.name
        from cr_voice_qual_member vm
       join cr_qualification q on q.id = vm.qualification_id
       join gkh_builder b on b.id = q.builder_id
       join gkh_contragent c on c.id = b.contragent_id
       JOIN CR_DICT_QUAL_MEMBER cdqm on cdqm.ID = vm.QUAL_MEMBER_ID
       where q.object_id = o.id and vm.type_accept_qual = 20
      group by q.id, c.name
      order by sum(CASE WHEN cdqm.IS_PRIMARY = true THEN 1000 else 1 end) desc limit 1 
     ) as builder,

   (select count(1) ||' из ' ||(select count(1) from cr_dict_qual_member qm where qm.period_id = per.id)  from cr_voice_qual_member vm
      left join cr_qualification q on q.id = vm.qualification_id
      left join gkh_builder b on b.id = q.builder_id
      left join gkh_contragent c on c.id = b.contragent_id
      JOIN CR_DICT_QUAL_MEMBER cdqm on cdqm.ID = vm.QUAL_MEMBER_ID
      where q.object_id = o.id and vm.type_accept_qual = 20
      group by q.id,c.name
      order by sum(CASE WHEN cdqm.IS_PRIMARY = true THEN 1000 else 1 end) desc limit 1) as rating,
     (select count(1) from cr_dict_qual_member qm where qm.period_id = per.id) as qual_member_count
from cr_object o
  join cr_dict_program p on p.id = o.program_id and p.type_program_state = 30
  join gkh_dict_period per on per.id =  p.period_id and per.date_start < current_date and (per.date_end is null or per.date_end > current_date)
  left join gkh_reality_object ro on ro.id = o.reality_object_id
  left join gkh_dict_municipality m on m.id = ro.municipality_id";
        }

        private string CreateViewSpecialCrObject(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_cr_special_object AS
SELECT 
    cr.id,
    cr.program_num,
    cr.program_id,
    cr.before_delete_program_id,
    prog.name as program_name,
    beforedelprog.name as before_del_program_name,
    mu.id AS municipality_id,
    mu.name AS municipality_name,
    stl.id AS settlement_id,
    stl.name AS settlement_name,
    ro.id AS reality_object_id,
    ro.address,
    cr.date_accept_gji,
    cr.allow_reneg,
    smr.id as smr_id,
    st.id AS state_id,
    smr_st.id as smr_state_id,
    per.name as period_name,
    COALESCE(ro.method_form_fund, 0) as method_form_fund
FROM cr_special_object cr
    inner join gkh_reality_object ro ON ro.id=cr.reality_object_id 
    inner join gkh_dict_municipality mu ON ro.municipality_id =  mu.id
    left join gkh_dict_municipality stl ON ro.stl_municipality_id =  stl.id
    left join cr_dict_program prog ON cr.program_id = prog.id
    left join gkh_dict_period per on per.id = prog.PERIOD_ID
    left join cr_dict_program beforedelprog ON cr.before_delete_program_id = beforedelprog.id
    left join b4_state st on st.id = cr.state_id
    left join cr_special_obj_monitoring_cmp smr on smr.object_id = cr.id
    left join b4_state smr_st on smr_st.id = smr.state_id";
        }

        private string CreateViewCrSpecialObjEstCalcDet(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_cr_special_obj_est_calc_det AS
SELECT 
	ec.id,
	ec.object_id as object_id,
	ec.state_id,
	w.name as work_name,
    ec.total_estimate,
	fs.name as fin_source_name,
	(
		select 
		    sum(est.total_cost)
		from cr_special_est_calc_estimate est
		where est.estimate_calc_id = ec.id
	) as sum_estimate,
	(
		select 
		    sum(rs.total_cost)
		from cr_special_est_calc_res_statem rs
		where rs.estimate_calc_id = ec.id 
	) as sum_resource,
	ec.estimation_type
FROM cr_special_obj_estimate_calc ec
	left join cr_special_obj_type_work tw on tw.ID = ec.TYPE_WORK_CR_ID
	left join GKH_DICT_WORK w on w.ID = tw.WORK_ID
	left join CR_DICT_FIN_SOURCE fs on fs.ID = tw.fin_source_id";
        }

        /// <summary>
        /// вьюха пир по договорам подряда
        /// </summary>
        /// <returns></returns>
        private string CreateViewClwBuild(DbmsKind dbmsKind)
        {
            return @"
create or replace view view_clw_build as
select
    ls.id,
	cw.id as clw_id,
	doc.type_document as document_type,
	c.name as builder,
	bc.document_name as contract_name,
	bc.document_num as contract_num,
	bc.document_date_from as contract_date,
	ro.id as ro_id,
	ro.address,
	mu.id as mu_id,
	mu.name as municipality,
	stl.id as stl_id,
	stl.name as settlement,
	ji.name as jursector_number,
	doc.document_date as ls_doc_date,
	doc.document_number as ls_doc_number,
	ls.who_considered,
	ls.date_of_adoption,
	ls.date_of_rewiew,
	ls.result_consideration,
	ls.debt_sum_approv,
	ls.penalty_debt_approv,
	ls.law_type_document,
	ls.date_considerat,
	ls.num_considerat,
	ls.cb_debt_sum,
	ls.cb_penalty_debt as cb_penalty_debt_sum,
	pdoc.document_date as pret_doc_date,
	pdoc.document_number as pret_doc_number,
	pret.sum as pret_sum,
	pret.penalty as pret_penalty_sum
	
from clw_lawsuit ls
	join clw_document doc on doc.id = ls.id
	
    left join clw_jur_institution ji on ji.id = ls.jinst_id

	join clw_claim_work cw on cw.id = doc.claimwork_id
	join clw_build_claim_work bcw on bcw.id = cw.id

	join cr_obj_build_contract bc on bc.id = bcw.contract_id
	join gkh_builder bld on bld.id = bc.builder_id
	join gkh_contragent c on c.id = bld.contragent_id
	
	join cr_object cro on cro.id = bc.object_id
	join gkh_reality_object ro on ro.id = cro.reality_object_id

	join gkh_dict_municipality mu on mu.id = ro.municipality_id
	left join gkh_dict_municipality stl on stl.id = ro.stl_municipality_id

	left join clw_document pdoc on pdoc.claimwork_id = cw.id and pdoc.type_document=20
	left join clw_pretension pret on pret.id = pdoc.id";
        }

        #endregion Create
        #region Delete

        private string DeleteViewCrObjEstCalcDet(DbmsKind dbmsKind)
        {
            var viewName = @"VIEW_CR_OBJ_EST_CALC_DET";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewCrObject(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_object";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewCrObjectEstCalc(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_object_est_calc";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewCrObjectQualification(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_object_qualification";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewSpecialCrObject(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_special_object";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewCrSpecialObjEstCalcDet(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_special_obj_est_calc_det";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewCrSpecialObjectEstCalc(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_special_object_est_calc";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewClwBuild(DbmsKind dbmsKind)
        {
            var viewName = "view_clw_build";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        #endregion Delete
        #endregion Вьюхи
        
    }
}