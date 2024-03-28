namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using NHibernate.Transform;

    /// <summary>
    /// Скрипты для Ставрополя
    /// </summary>
    public partial class ActualizeVersionService
    {
        /// <summary>
        /// Корректировка КЭ "Кровля"(скрипт)
        /// </summary>
        /// <param name="baseParams"></param>
        public IDataResult RoofCorrection(BaseParams baseParams)
        {
            var sessions = this.Container.Resolve<ISessionProvider>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            try
            {
                var versionId = baseParams.Params.GetAs<long>("versionId");
                var muId = programVersionDomain.Get(versionId).Municipality.Id;

                using (var session = sessions.OpenStatelessSession())
                using (var tr = session.BeginTransaction())
                {
                    var query = session.CreateSQLQuery(@"
create temp table temp_temle_to_delete as 
 SELECT
   reality.id as reality_id,
   reality.address as addres,
   el.id AS RO_EL,
   el.STRUCT_EL_ID as struct_el, 
   srt_el.id AS STRUCT_EL_ID, srt_el.name as STRUCT_EL_NAME, state.id as state_id, state.name as state_name

  FROM gkh_reality_object reality
      JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
      JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
      JOIN b4_state state ON el.state_id = state.id   
								where  srt_el.name ~* 'Фасад'
      and reality.ID in(
								SELECT  reality.id
								FROM gkh_reality_object reality
								JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
								JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
								JOIN b4_state state ON el.state_id = state.id
								where  srt_el.name ~* 'Фасад'
								group by reality.id
								having count(srt_el.id)>1
								order by reality.address)
                 and el.id in (SELECT id FROM ovrhl_ro_struct_el _el  WHERE   _el.last_overhaul_year in
                     (SELECT  max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id ))
        AND state.name = 'На удаление'
        and reality.id in (
									SELECT  reality.id
									FROM gkh_reality_object reality
									JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
									JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
									JOIN b4_state state ON el.state_id = state.id   
									where     srt_el.name ~* 'Фасад'
									and reality.ID in(SELECT  reality.id
															   FROM gkh_reality_object reality
															   JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
															   JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
															   JOIN b4_state state ON el.state_id = state.id
															   where srt_el.name ~* 'Фасад'
															   group by reality.id
															   having count(srt_el.id)>1
															   order by reality.address)
                          and el.id in(SELECT id FROM ovrhl_ro_struct_el _el WHERE _el.last_overhaul_year in
                                      (SELECT max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id))
        AND state.name = 'На удаление'

 intersect

    SELECT reality.id
      FROM gkh_reality_object reality
      JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
      JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
      JOIN b4_state state ON el.state_id = state.id   
								where  srt_el.name ~* 'Фасад'
      and reality.ID in(
								SELECT  reality.id
								FROM gkh_reality_object reality
								JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
								JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
								JOIN b4_state state ON el.state_id = state.id
								where  srt_el.name ~* 'Фасад'
								group by reality.id
								having count(srt_el.id)>1
								order by reality.address)
                        and el.id in ( SELECT id FROM ovrhl_ro_struct_el _el WHERE _el.last_overhaul_year in
                                     (SELECT  max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id ))
      AND  state.name = 'Актуальный' );
										
create temp table temp_temle_to_actual as 
	   SELECT 
	      reality.id as reality_id,
   reality.address as addres,
   el.id AS RO_EL,
   el.STRUCT_EL_ID as struct_el, 
   srt_el.id AS STRUCT_EL_ID, srt_el.name as STRUCT_EL_NAME, state.id as state_id, state.name as state_name
      FROM gkh_reality_object reality
      JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
      JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
      JOIN b4_state state ON el.state_id = state.id   
								where  srt_el.name ~* 'Фасад'
      and reality.ID in(
								SELECT  reality.id
								FROM gkh_reality_object reality
								JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
								JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
								JOIN b4_state state ON el.state_id = state.id
								where srt_el.name ~* 'Фасад'
								group by reality.id
								having count(srt_el.id)>1
								order by reality.address)
                        and el.id in ( SELECT id FROM ovrhl_ro_struct_el _el WHERE _el.last_overhaul_year in
                                     (SELECT  max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id ))
      AND  state.name = 'Актуальный'; 

     update ovrhl_stage1_version  set struct_el_id = ss.RO_EL from 
		     (select del.reality_id as ro_id, del.RO_EL as del_RO_EL, act.RO_EL as RO_EL from temp_temle_to_delete del
				join temp_temle_to_actual act on act.reality_id = del.reality_id)ss 
		    where ss.ro_id = reality_object_id and ss.del_RO_EL = struct_el_id  and reality_object_id in  (
                select id from gkh_reality_object where stl_municipality_id = :muId or municipality_id = :muId
            );
			
		update ovrhl_version_rec set  is_changed_year = true,  changes = 'Изменено:'||cast(now() as date)  where  id in (  select rec.id from ovrhl_stage1_version satage1
		JOIN ovrhl_stage2_version stage2 ON satage1.stage2_version_id = stage2.id
		JOIN ovrhl_version_rec rec ON stage2.st3_version_id = rec.id and not rec.is_changed_publish_year
		JOIN temp_temle_to_actual act ON act.reality_id = satage1.reality_object_id  and act.reality_id in (
            select id from gkh_reality_object where stl_municipality_id = :muId or municipality_id = :muId
        )
		and satage1.struct_el_id = act.RO_EL 
		and satage1.stage2_version_id in (select stage2.id from ovrhl_stage1_version satage1
															JOIN ovrhl_stage2_version stage2 ON satage1.stage2_version_id = stage2.id
															JOIN ovrhl_version_rec rec ON stage2.st3_version_id = rec.id and not rec.is_changed_publish_year
															JOIN temp_temle_to_actual act ON act.reality_id = satage1.reality_object_id and satage1.struct_el_id = act.RO_EL 							 
															JOIN ovrhl_prg_version version on rec.version_id = version.id
															where version.is_main = true  ));

drop table temp_temle_to_delete;
drop table temp_temle_to_actual;")
                        .SetParameter("muId", muId);
                    query.ExecuteUpdate();

                    query = session.CreateSQLQuery(@"
-- КРЫША
create temp table temp_temle_to_delete as 
 SELECT
   reality.id as reality_id,
   reality.address as addres,
   el.id AS RO_EL,
   el.STRUCT_EL_ID as struct_el, 
   srt_el.id AS STRUCT_EL_ID, srt_el.name as STRUCT_EL_NAME, state.id as state_id, state.name as state_name

  FROM gkh_reality_object reality
      JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
      JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
      JOIN b4_state state ON el.state_id = state.id   
								where  srt_el.name ~* 'крыша'
      and reality.ID in(
								SELECT  reality.id
								FROM gkh_reality_object reality
								JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
								JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
								JOIN b4_state state ON el.state_id = state.id
								where  srt_el.name ~* 'крыша'
								group by reality.id
								having count(srt_el.id)>1
								order by reality.address)
                 and el.id in (SELECT id FROM ovrhl_ro_struct_el _el  WHERE   _el.last_overhaul_year in
                     (SELECT  max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id ))
        AND state.name = 'На удаление'
        and reality.id in (
									SELECT  reality.id
									FROM gkh_reality_object reality
									JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
									JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
									JOIN b4_state state ON el.state_id = state.id   
									where     srt_el.name ~* 'крыша'
									and reality.ID in(SELECT  reality.id
															   FROM gkh_reality_object reality
															   JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
															   JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
															   JOIN b4_state state ON el.state_id = state.id
															   where srt_el.name ~* 'крыша'
															   group by reality.id
															   having count(srt_el.id)>1
															   order by reality.address)
                          and el.id in(SELECT id FROM ovrhl_ro_struct_el _el WHERE _el.last_overhaul_year in
                                      (SELECT max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id))
        AND state.name = 'На удаление'

 intersect

    SELECT reality.id
      FROM gkh_reality_object reality
      JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
      JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
      JOIN b4_state state ON el.state_id = state.id   
								where  srt_el.name ~* 'крыша'
      and reality.ID in(
								SELECT  reality.id
								FROM gkh_reality_object reality
								JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
								JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
								JOIN b4_state state ON el.state_id = state.id
								where  srt_el.name ~* 'крыша'
								group by reality.id
								having count(srt_el.id)>1
								order by reality.address)
                        and el.id in ( SELECT id FROM ovrhl_ro_struct_el _el WHERE _el.last_overhaul_year in
                                     (SELECT  max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id ))
      AND  state.name = 'Актуальный' );										

create temp table temp_temle_to_actual as 
	   SELECT 
	      reality.id as reality_id,
   reality.address as addres,
   el.id AS RO_EL,
   el.STRUCT_EL_ID as struct_el, 
   srt_el.id AS STRUCT_EL_ID, srt_el.name as STRUCT_EL_NAME, state.id as state_id, state.name as state_name
      FROM gkh_reality_object reality
      JOIN ovrhl_ro_struct_el el ON reality.id=el.RO_ID
      JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
      JOIN b4_state state ON el.state_id = state.id   
								where  srt_el.name ~* 'крыша'
      and reality.ID in(
								SELECT  reality.id
								FROM gkh_reality_object reality
								JOIN ovrhl_ro_struct_el el ON reality.id=RO_ID
								JOIN OVRHL_STRUCT_EL srt_el ON el.struct_el_id = srt_el.id
								JOIN b4_state state ON el.state_id = state.id
								where srt_el.name ~* 'крыша'
								group by reality.id
								having count(srt_el.id)>1
								order by reality.address)
                        and el.id in ( SELECT id FROM ovrhl_ro_struct_el _el WHERE _el.last_overhaul_year in
                                     (SELECT  max(last_overhaul_year) from ovrhl_ro_struct_el GROUP BY ro_id ))
      AND  state.name = 'Актуальный'; 

     update ovrhl_stage1_version  set struct_el_id = ss.RO_EL from 
		     (select del.reality_id as ro_id, del.RO_EL as del_RO_EL, act.RO_EL as RO_EL from temp_temle_to_delete del
				join temp_temle_to_actual act on act.reality_id = del.reality_id)ss 
		    where ss.ro_id = reality_object_id and ss.del_RO_EL = struct_el_id  and reality_object_id in (
                select id from gkh_reality_object where stl_municipality_id = :muId or municipality_id = :muId
            );
			

		update ovrhl_version_rec set  is_changed_year = true,  changes = 'Изменено:'||cast(now() as date)  where  id in (  select rec.id from ovrhl_stage1_version satage1
		JOIN ovrhl_stage2_version stage2 ON satage1.stage2_version_id = stage2.id
		JOIN ovrhl_version_rec rec ON stage2.st3_version_id = rec.id  and not rec.is_changed_publish_year
		JOIN temp_temle_to_actual act ON act.reality_id = satage1.reality_object_id  and act.reality_id in (
            select id from gkh_reality_object where stl_municipality_id = :muId or municipality_id = :muId
        )
		and satage1.struct_el_id = act.RO_EL 
		and satage1.stage2_version_id in (select stage2.id from ovrhl_stage1_version satage1
															JOIN ovrhl_stage2_version stage2 ON satage1.stage2_version_id = stage2.id
															JOIN ovrhl_version_rec rec ON stage2.st3_version_id = rec.id and not rec.is_changed_publish_year
															JOIN temp_temle_to_actual act ON act.reality_id = satage1.reality_object_id and satage1.struct_el_id = act.RO_EL 							 
															JOIN ovrhl_prg_version version on rec.version_id = version.id
															where version.is_main = true  ));")
                        .SetParameter("muId", muId);
                    query.ExecuteUpdate();
                    try
                    {
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(sessions);
                this.Container.Release(programVersionDomain);
            }
        }

        /// <summary>
        /// Скопировать скорректированные года работ в плановые (скрипт)
        /// </summary>
        /// <param name="baseParams"></param>
        public IDataResult CopyCorrectedYears(BaseParams baseParams)
        {
            var sessions = this.Container.Resolve<ISessionProvider>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();

            try
            {
                var versionId = baseParams.Params.GetAs<long>("versionId");
                var muId = programVersionDomain.Get(versionId).Municipality.Id;

                using (var session = sessions.OpenStatelessSession())
                using (var tr = session.BeginTransaction())
                {
                    var query = session.CreateSQLQuery(@"
update ovrhl_version_rec st3
set
year = data.plan_year
    from
  (
    select st3.id, st3.year, corr.plan_year
    FROM ovrhl_version_rec st3
    JOIN ovrhl_prg_version v ON v.id = st3.version_id AND v.is_main IS TRUE
    JOIN gkh_dict_municipality m ON m.id = v.mu_id AND m.id = :muId
    JOIN ovrhl_stage2_version st2 ON st3.id = st2.st3_version_id
    JOIN ovrhl_dpkr_correct_st2 corr ON st2.id = corr.st2_version_id
    WHERE st3.is_manually_correct IS TRUE AND st3.is_changed_year IS FALSE and st3.is_changed_publish_year IS FALSE
  ) data
where st3.id = data.id;")
                        .SetParameter("muId", muId);
                    query.ExecuteUpdate();
                    try
                    {
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(sessions);
                this.Container.Release(programVersionDomain);
            }
        }

        /// <summary>
        /// Удалить повторные работы (скрипт)
        /// </summary>
        /// <param name="baseParams"></param>
        public IDataResult DeleteRepeatedWorks(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var fileMgr = this.Container.Resolve<IFileManager>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            var version = programVersionDomain.Get(versionId);
            var muId = version.Municipality.Id;
            var oldVersionId = version.ParentVersion?.Id;

            if (!version.IsMain)
            {
                throw new Exception("Версия должна быть основной");
            }

            if (oldVersionId == null)
            {
                throw new Exception("Не найдена версия, с которой было произведено копирование");
            }

            try
            {
                var deletedWorks = this.GetDeletedWorks(muId, (long) oldVersionId);

                var log = new StringBuilder();

                log.AppendLine("Причина;Адрес дома;ООИ;Конструктивный элемент;Плановый год;Сумма;Номер");
                foreach (var work in deletedWorks)
                {
                    log.AppendLine($"{work.Reason};{work.Address};{work.OOI};{work.StructElementName};{work.Year};{work.Sum};{work.Number};");
                }

                var fileInfo = fileMgr.SaveFile("result", "csv", Encoding.GetEncoding(1251).GetBytes(log.ToString()));

                return new BaseDataResult(fileInfo.Id);
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(fileMgr);
            }
        }

        private IList<QueryDto> GetDeletedWorks(long muId, long oldVersionId)
        {
            var sessions = this.Container.Resolve<ISessionProvider>();
           
            try
            {
                using (var session = sessions.OpenStatelessSession())
                {
                    var query = session.CreateSQLQuery(@"
begin;
drop table if exists temp_st1;

create temp table temp_st1 
(
	id bigint,
	Address character varying(1000),
	OOI character varying(500),
	StructElementName character varying(255),
	Year integer,
	Sum numeric(19,5) ,
	Number integer
); 
	
insert into temp_st1 
(
	id,
	Address,
	OOI,
	StructElementName,
	Year,
	Sum,
	Number
)
select st1.id, ro.address, ceo.name as ooi, el.name as structelementname, st3.year, st3.sum, st3.index_num as number
from ovrhl_stage1_version st1
  join ovrhl_ro_struct_el ro_el on st1.struct_el_id = ro_el.id
  join ovrhl_struct_el el on ro_el.struct_el_id = el.id
  join ovrhl_stage2_version st2 on st1.stage2_version_id = st2.id
  join ovrhl_version_rec st3 on st2.st3_version_id = st3.id
  join ovrhl_prg_version v on st3.version_id = v.id and v.is_main is true
  join gkh_dict_municipality m on m.id = v.mu_id and m.id = :muId
  join gkh_reality_object ro on st1.reality_object_id = ro.id
  join ovrhl_common_estate_object ceo on st2.common_estate_id = ceo.id
  join
  (select
     ro_id,
     struct_el_id,
     max_year
   from
     (
       select
         data2.ro_id,
         data2.struct_el_id,
         coalesce(data1.count, 0) old_count,
         coalesce(data2.count, 0) new_count,
         data2.max_year
       from
         (
           select
             ro_el.ro_id,
             ro_el.struct_el_id,
             count(*) count
           from ovrhl_publish_prg_rec pub_rec
             join ovrhl_publish_prg pub_prog on pub_rec.publish_prg_id = pub_prog.id
             join ovrhl_prg_version v on pub_prog.version_id = v.id and v.id = :oldVersionId
             join ovrhl_stage2_version st2 on pub_rec.stage2_id = st2.id
             join ovrhl_stage1_version st1 on st2.id = st1.stage2_version_id
             join ovrhl_ro_struct_el ro_el on st1.struct_el_id = ro_el.id
             join ovrhl_struct_el el on ro_el.struct_el_id = el.id
           group by ro_el.ro_id, ro_el.struct_el_id
         ) data1
         right join
         (
           select
             ro_el.ro_id,
             ro_el.struct_el_id,
             max(st1.year) max_year,
             count(*)      count
           from ovrhl_publish_prg_rec pub_rec
             join ovrhl_publish_prg pub_prog on pub_rec.publish_prg_id = pub_prog.id
             join ovrhl_prg_version v on pub_prog.version_id = v.id and v.is_main is true
             join gkh_dict_municipality m on m.id = v.mu_id and m.id = :muId
             join ovrhl_stage2_version st2 on pub_rec.stage2_id = st2.id
             join ovrhl_stage1_version st1 on st2.id = st1.stage2_version_id
             join ovrhl_ro_struct_el ro_el on st1.struct_el_id = ro_el.id
             join ovrhl_struct_el el on ro_el.struct_el_id = el.id
           group by ro_el.ro_id, ro_el.struct_el_id
         ) data2
           on data1.ro_id = data2.ro_id and data1.struct_el_id = data2.struct_el_id
     ) diff
   where old_count < new_count and not (old_count = 0 and new_count - old_count = 1)) for_delete
    on ro_el.ro_id = for_delete.ro_id and ro_el.struct_el_id = for_delete.struct_el_id and
       st1.year = for_delete.max_year;

delete from
ovrhl_type_work_cr_st1 tst1
where tst1.st1_id in
(
    select id from temp_st1
);

delete from
ovrhl_stage1_version st1
where st1.id in 
(
	select id from temp_st1
);

delete from 
OVRHL_DPKR_CORRECT_ST2 cr 
where st2_version_id in (select id from ovrhl_stage2_version st2 where id not in (select stage2_version_id from ovrhl_stage1_version st1));

delete from 
ovrhl_publish_prg_rec 
where stage2_id in (select id from ovrhl_stage2_version st2 where id not in (select stage2_version_id from ovrhl_stage1_version st1));

delete from 
ovrhl_short_prog_rec 
where stage2_id in (select id from ovrhl_stage2_version st2 where id not in (select stage2_version_id from ovrhl_stage1_version st1));

delete from 
ovrhl_stage2_version st2 
where id not in (select stage2_version_id from ovrhl_stage1_version st1);

select
	'Удаление повторной работы из опубликованной программы' as Reason,
	Address,
	OOI,
	StructElementName,
	Year,
	Sum,
	Number
from temp_st1;
end;
")
                        .SetParameter("muId", muId)
                        .SetParameter("oldVersionId", oldVersionId);

                    query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());
                    return query.List<QueryDto>();                   
                }
            }
            finally
            {
                this.Container.Release(sessions);
            }
        }

        private struct QueryDto
        {
            public string Reason { get; set; }
            public string Address { get; set; }
            public string OOI { get; set; }
            public string StructElementName { get; set; }
            public int Year { get; set; }
            public decimal Sum { get; set; }
            public long Number { get; set; }
        }
    }
}
