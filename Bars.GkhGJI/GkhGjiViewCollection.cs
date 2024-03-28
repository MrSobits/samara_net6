namespace Bars.GkhGji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;

    public class GkhGjiViewCollection : BaseGkhViewCollection
    {
        public override int Number => 1;

        public override List<string> GetDropAll(DbmsKind dbmsKind)
        {
            var deleteView = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteView")).ToList();

            var queries = new List<string>();

            foreach (var method in deleteView)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!string.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            queries.Add(this.DeleteFuncGetDocumentParentCountRobjectByViolStage(dbmsKind));
            queries.Add(this.DeleteFuncGetInspectionCountRobject(dbmsKind));

            var deleteFunc = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteFunction")).ToList();
            foreach (var method in deleteFunc)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!string.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            return queries;
        }

        public override List<string> GetCreateAll(DbmsKind dbmsKind)
        {
            var queries = new List<string>
                              {
                                  this.CreateFuncGetDocumentParentCountRobjectByViolStage(dbmsKind),
                                  this.CreateFuncGetInspectionCountRobject(dbmsKind)
                              };

            queries.AddRange(base.GetCreateAll(dbmsKind));
            return queries;
        }

        #region Функции
        #region Create

        /// <summary>
        /// функция возвращает наименования актуальных управляющих организаций жилого дома
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRobjectManorg(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE FUNCTION gjiGetRobjectManorg(ro_id number) RETURN nvarchar2 IS
   manorg_name varchar2(4000) := '';
 result varchar2(4000) :='';
 zp varchar2(4000) :=', ';
 cur_date date := CURRENT_DATE;
  CURSOR cursorCon  IS
    select c.name
    from gkh_morg_contract_realobj mcro
        join gkh_morg_contract moc on moc.id = mcro.man_org_contract_id
        LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
        LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
    where mcro.reality_obj_id = ro_id
        and moc.start_date <= cur_date
        and (moc.end_date is null or moc.end_date >= cur_date);
 begin 
 OPEN cursorCon;
loop
FETCH cursorCon INTO manorg_name;
EXIT WHEN cursorCon%NOTFOUND;
IF (result is not null)
then
result:=result || zp;
end if;
result:=result || manorg_name;
end loop;
CLOSE cursorCon;
return result;
end;";
            }

            return @"CREATE OR REPLACE FUNCTION gjiGetRobjectManorg(bigint)
  RETURNS text AS
$BODY$ declare
 manorg_name text := '';
 result text :='';
 zp text:=', ';
 cur_date date := (select CURRENT_DATE);
 cursorCon CURSOR IS
    select c.name
    from gkh_morg_contract_realobj mcro
        join gkh_morg_contract moc on moc.id = mcro.man_org_contract_id
        LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
        LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
    where mcro.reality_obj_id = $1
        and moc.start_date <= cur_date
        and (moc.end_date is null or moc.end_date >= cur_date);
 begin OPEN cursorCon;
loop
FETCH cursorCon INTO manorg_name;
EXIT WHEN not FOUND;
if(result!='')
then
result:=result || zp;
end if;
result:=result || manorg_name;
end loop;
CLOSE cursorCon;
return result;
end; $BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Функция воозвращает наименования актуальных типов договоров управляющей организации жилого дома
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRobjectTypeContract(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetRobjectTypeContract(ro_id number) RETURN nvarchar2 IS
    result varchar2(4000) := '';
    type_contract varchar2(4000) :='';
    zp varchar2(4000):=', ';
    cur_date date := CURRENT_DATE;
    CURSOR cursorCon IS 
    select 
        CASE
            WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 20) THEN 'Договор между УК и ТСЖ'
            WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 40) THEN 'Договор между УК и ЖСК'
            WHEN moc.type_contract = 20 THEN 'Договор между УК и собственниками'
            WHEN moc.type_contract = 30 AND mo.type_management = 20 THEN 'ТСЖ'
            WHEN moc.type_contract = 30 AND mo.type_management = 40 THEN 'ЖСК'
            ELSE 'Непосредственное управление'
        END AS type_contract
    from gkh_morg_contract_realobj mcro
        JOIN gkh_morg_contract moc ON moc.id = mcro.man_org_contract_id
        LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
        LEFT JOIN gkh_morg_contract_jsktsj mocjsktsj ON mocjsktsj.id = moc.id
        LEFT JOIN gkh_managing_organization mo2 ON mo2.id = mocjsktsj.man_org_jsk_tsj_id
    where mcro.reality_obj_id = ro_id
        and moc.start_date <= cur_date 
        and (moc.end_date is null or moc.end_date >= cur_date);
 begin
OPEN cursorCon;
loop 
    FETCH cursorCon INTO type_contract;
    EXIT WHEN cursorCon%NOTFOUND;
    if(result is not null)
    then
        result:=result || zp;
    end if;
    result:=result || type_contract;
end loop;
CLOSE cursorCon;
return result;
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetRobjectTypeContract(bigint)
  RETURNS text AS
$BODY$ 
declare
    result text := '';
    type_contract text :='';
    zp text:=', ';
    cur_date date := (select CURRENT_DATE);
    cursorCon CURSOR IS 
    select 
        CASE
            WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 20) THEN 'Договор между УК и ТСЖ'::text
            WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 40) THEN 'Договор между УК и ЖСК'::text
            WHEN moc.type_contract = 20 THEN 'Договор между УК и собственниками'::text
            WHEN moc.type_contract = 30 AND mo.type_management = 20 THEN 'ТСЖ'::text
            WHEN moc.type_contract = 30 AND mo.type_management = 40 THEN 'ЖСК'::text
            ELSE 'Непосредственное управление'::text
        END AS type_contract
    from gkh_morg_contract_realobj mcro
        JOIN gkh_morg_contract moc ON moc.id = mcro.man_org_contract_id
        LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
        LEFT JOIN gkh_morg_contract_jsktsj mocjsktsj ON mocjsktsj.id = moc.id
        LEFT JOIN gkh_managing_organization mo2 ON mo2.id = mocjsktsj.man_org_jsk_tsj_id
    where mcro.reality_obj_id = $1 
        and moc.start_date <= cur_date 
        and (moc.end_date is null or moc.end_date >= cur_date);
 begin
OPEN cursorCon;
loop 
    FETCH cursorCon INTO type_contract;
    EXIT WHEN not FOUND;
    if(result <> '')
    then
        result:=result || zp;
    end if;
    result:=result || type_contract;
end loop;
CLOSE cursorCon;
return result;
end; $BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Функция возвращает строку наименований муниципальных районов жилых домов проверки
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspRobjectMuName(insp_id number) RETURN varchar2 IS
                   mu varchar2(4000) := '';
                   zp varchar2(4000):=', ';
                   result varchar2(4000):= '';
                   CURSOR cursorCon(insp_id number)  IS
                   select distinct mu.name 
                   from gji_inspection_robject gji_ro
                        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                        left join gkh_dict_municipality mu on mu.id = ro.municipality_id where gji_ro.inspection_id = insp_id;
                begin
                   OPEN cursorCon(insp_id); 
                   loop
                   FETCH cursorCon INTO mu; 
                   EXIT WHEN cursorCon%NOTFOUND; 
                   if(result is not null)
                   then
                       result := result || zp;
                   end if;
                   result := result || mu;
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end;
                ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspRobjectMuName(bigint)
  RETURNS text AS
$BODY$ 
declare 
   mu text := '';
   zp text:=', ';
   result text:= '';
   cursorCon CURSOR IS
   select distinct mu.name 
   from gji_inspection_robject gji_ro
        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
   where gji_ro.inspection_id = $1;
begin
   OPEN cursorCon; 
   loop
   FETCH cursorCon INTO mu; 
   EXIT WHEN not FOUND; 
   if(result <> '')
   then
       result := result || zp;
   end if;
   result := result || mu;
   end loop;
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";

        }

        /// <summary>
        /// Функция возвращает идентификатор первого муниципального образования жилых домов проверки
        /// gjiGetInspRobjectMuId
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspRobjectMuId(insp_id NUMBER) RETURN NUMBER IS
   result NUMBER := 0;
   CURSOR cursorCon IS
   select distinct mu.id 
   from gji_inspection_robject gji_ro
        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
   where gji_ro.inspection_id = insp_id AND ROWNUM = 1;
begin
   OPEN cursorCon; 
   FETCH cursorCon INTO result;
   CLOSE cursorCon; 
   return result; 
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspRobjectMuId(bigint)
  RETURNS integer AS
$BODY$ 
declare 
   result integer := 0;
   cursorCon CURSOR IS
   select distinct mu.id 
   from gji_inspection_robject gji_ro
        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
   where gji_ro.inspection_id = $1
   limit 1;
begin
   OPEN cursorCon; 
   FETCH cursorCon INTO result;
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Функция возвращает строку адресов жилых домов основания проверки
        /// gjiGetInspRobjectAddress
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobjectAddress(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspRobjectAddress(insp_id number) RETURN varchar2 IS

    address varchar2(4000) :=''; 
    result varchar2(4000) := ''; 
    zp varchar2(40):='; '; 
    cnt integer:= 0;    
    CURSOR cursorCon  IS  
    select ro.address 
    from gji_inspection_robject insp_ro
        inner join gkh_reality_object ro on ro.id = insp_ro.reality_object_id
    where insp_ro.inspection_id = insp_id;
begin 
   OPEN cursorCon; 
   loop 
    FETCH cursorCon INTO address; 
    EXIT WHEN cursorCon%NOTFOUND; 
    
    cnt := cnt + 1;
     IF (cnt>3) 
    then 
        EXIT; 
    end if;
    
    IF (result is not null) 
    then 
        result:= result || zp; 
    end if;
    
    result:= result || address;
   end loop; 
   CLOSE cursorCon; 
    return result;
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspRobjectAddress(bigint)
  RETURNS text AS
$BODY$ 
declare
    address text :=''; 
    result text :=''; 
    zp text:='; ';  
    cursorCon CURSOR IS  
    select ro.address 
    from gji_inspection_robject insp_ro
        inner join gkh_reality_object ro on ro.id = insp_ro.reality_object_id
    where insp_ro.inspection_id = $1;
begin 
   OPEN cursorCon; 
   loop 
    FETCH cursorCon INTO address; 
    EXIT WHEN not FOUND; 
    if(result!='') 
    then 
        result:=result || zp; 
    end if;
    result:=result || address; 
   end loop; 
   CLOSE cursorCon; 
    return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }


        /// <summary>
        /// Функция возвращает строку адресов жилых домов постановления прокуратуры
        /// gjiGetResolProsRobjectAddress
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetResolProsRobjectAddress(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetResolProsRobjectAddress(doc_id number) RETURN varchar2 IS

                        address varchar2(4000) :=''; 
                        result varchar2(4000) := ''; 
                        zp varchar2(40):='; '; 
                        cnt NUMBER:= 0;    
                        CURSOR cursorCon  IS  
                        select ro.address 
                        from GJI_RESOLPROS_ROBJECT resol_pros_ro
                            join gkh_reality_object ro on ro.id = resol_pros_ro.reality_object_id
                            join  gji_document_children doc_parent  on doc_parent.parent_id = resol_pros_ro.RESOLPROS_ID and doc_parent.children_id = doc_id
                        where resol_pros_ro.RESOLPROS_ID = doc_id;
                    begin 
                       OPEN cursorCon; 
                       loop 
                        FETCH cursorCon INTO address; 
                        EXIT WHEN cursorCon%NOTFOUND; 
    
                        cnt := cnt + 1;
                         IF (cnt>3) 
                        then 
                            EXIT; 
                        end if;
    
                        IF (result is not null) 
                        then 
                            result:= result || zp; 
                        end if;
    
                        result:= result || address;
                       end loop; 
                       CLOSE cursorCon; 
                        return result;
                    end; ";
            }

            return @"
                CREATE OR REPLACE FUNCTION gjiGetResolProsRobjectAddress(bigint)
                  RETURNS text AS
                $BODY$ 
                declare
                    address text :=''; 
                    result text :=''; 
                    zp text:='; ';  
                    cursorCon CURSOR IS  
                            select ro.address 
                                 from GJI_RESOLPROS_ROBJECT resol_pros_ro
                            join gkh_reality_object ro on ro.id = resol_pros_ro.reality_object_id
                            join  gji_document_children doc_parent  on doc_parent.parent_id = resol_pros_ro.RESOLPROS_ID and doc_parent.children_id = $1;
                begin 
                   OPEN cursorCon; 
                   loop 
                    FETCH cursorCon INTO address; 
                    EXIT WHEN not FOUND; 
                    if(result!='') 
                    then 
                        result:=result || zp; 
                    end if;
                    result:=result || address; 
                   end loop; 
                   CLOSE cursorCon; 
                    return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Функция возвращает строку идентификаторов жилых домов проверки вида /1/2/4/
        /// gjiGetResolProsRobject
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetResolProsRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetResolProsRobject(doc_id number)
                  RETURN varchar2 IS
                    objectId varchar2(4000) :=''; 
                    result varchar2(4000) := '/';    
                    CURSOR cursorCon IS  
                    select resol_pros_ro.reality_object_id 
                   from GJI_RESOLPROS_ROBJECT resol_pros_ro 
                     join  gji_document_children doc_parent  on doc_parent.parent_id = resol_pros_ro.RESOLPROS_ID and doc_parent.children_id = doc_id;  
                begin 
                   OPEN cursorCon; 
                   loop 
                   FETCH cursorCon INTO objectId; 
                   EXIT WHEN cursorCon%NOTFOUND; 
                      result:=result || objectId ||'/'; 
                   end loop; 
                   CLOSE cursorCon; 
                    return result; 
                end; 
                ";
            }

            return @"
            CREATE OR REPLACE FUNCTION gjiGetResolProsRobject(bigint)
              RETURNS text AS
            $BODY$ 
            declare
                objectId text :=''; 
                result text := '/'; 
                cursorCon CURSOR IS  
                 select resol_pros_ro.reality_object_id 
                   from GJI_RESOLPROS_ROBJECT resol_pros_ro 
                     join  gji_document_children doc_parent  on doc_parent.parent_id = resol_pros_ro.RESOLPROS_ID and doc_parent.children_id =  $1;  
            begin 
               OPEN cursorCon; 
               loop 
               FETCH cursorCon INTO objectId; 
               EXIT WHEN not FOUND; 
                  result:=result || objectId ||'/'; 
               end loop; 
               CLOSE cursorCon; 
                return result; 
            end; 
            $BODY$
              LANGUAGE plpgsql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// Функция возвращает строку идентификаторов жилых домов проверки вида /1/2/4/
        /// gjiGetInspRobject
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspRobject(insp_id number)
                  RETURN varchar2 IS
                    objectId varchar2(4000) :=''; 
                    result varchar2(4000) := '/';    
                    CURSOR cursorCon IS  
                    select insp_ro.reality_object_id 
                   from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp_id;  
                begin 
                   OPEN cursorCon; 
                   loop 
                   FETCH cursorCon INTO objectId; 
                   EXIT WHEN cursorCon%NOTFOUND; 
                      result:=result || objectId ||'/'; 
                   end loop; 
                   CLOSE cursorCon; 
                    return result; 
                end; 
                ";
            }

            return @"
            CREATE OR REPLACE FUNCTION gjiGetInspRobject(bigint)
              RETURNS text AS
            $BODY$ 
            declare
                objectId text :=''; 
                result text := '/'; 
                cursorCon CURSOR IS  
                select insp_ro.reality_object_id 
               from gji_inspection_robject insp_ro where insp_ro.inspection_id = $1;  
            begin 
               OPEN cursorCon; 
               loop 
               FETCH cursorCon INTO objectId; 
               EXIT WHEN not FOUND; 
                  result:=result || objectId ||'/'; 
               end loop; 
               CLOSE cursorCon; 
                return result; 
            end; 
            $BODY$
              LANGUAGE plpgsql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// Функция возвращает строку ФИО инспекторов оснвоания проверки
        /// gjiGetInspectionInsp
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionInspectors(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspectionInsp (insp_id NUMBER)
   RETURN VARCHAR2
IS
   inspectorFIO   VARCHAR2 (4000) := '';
   result         VARCHAR2 (4000) := '';
   zp             VARCHAR2 (4000) := ', ';

   CURSOR cursorCon
   IS
      SELECT ins.fio
        FROM gji_inspection_inspector insp_ins
             INNER JOIN gkh_dict_inspector ins
                ON ins.id = insp_ins.inspector_id
       WHERE insp_ins.inspection_id = insp_id;

BEGIN
   OPEN cursorCon;

   LOOP
      FETCH cursorCon INTO inspectorFIO;

      EXIT WHEN cursorCon%NOTFOUND;

      IF (result is not null)
      THEN
         result := result || zp;
      END IF;

      result := result || inspectorFIO;
   END LOOP;

   CLOSE cursorCon;

   RETURN result;
END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspectionInsp(bigint)
  RETURNS text AS
$BODY$ 
declare
   inspectorFIO text :='';
   result text :='';
   zp text:=', ';   
   cursorCon CURSOR IS
   select ins.fio 
   from gji_inspection_inspector insp_ins
   inner join gkh_dict_inspector ins on ins.id = insp_ins.inspector_id
   where insp_ins.inspection_id = $1;
begin 
   OPEN cursorCon; 
    loop 
    FETCH cursorCon INTO inspectorFIO; 
    EXIT WHEN not FOUND; 
    if(result!='') 
    then 
        result:=result || zp; 
    end if; 
    result:=result || inspectorFIO; 
    end loop; 
    CLOSE cursorCon; 
    return result; 
    end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Функция возвращает строку наименований отделов оснвоания проверки
        /// gjiGetInspectionZonalInspections
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionZonalInspections(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspectionZonalInspections (insp_id NUMBER)
   RETURN VARCHAR2
IS
   zonaName       VARCHAR2 (4000) := '';
   result         VARCHAR2 (4000) := '';
   zp             VARCHAR2 (4000) := ', ';

   CURSOR cursorCon
   IS
      SELECT ins.zone_name
        FROM gji_inspection_zonal_inspection insp_ins
             INNER JOIN gkh_dict_zonainsp ins
                ON ins.id = insp_ins.zonal_inspection_id
       WHERE insp_ins.inspection_id = insp_id;

BEGIN
   OPEN cursorCon;

   LOOP
      FETCH cursorCon INTO zoneName;

      EXIT WHEN cursorCon%NOTFOUND;

      IF (result is not null)
      THEN
         result := result || zp;
      END IF;

      result := result || zoneName;
   END LOOP;

   CLOSE cursorCon;

   RETURN result;
END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspectionZonalInspections(bigint)
  RETURNS text AS
$BODY$ 
declare
   zoneName text :='';
   result text :='';
   zp text:=', ';   
   cursorCon CURSOR IS
   select ins.zone_name 
   from gji_inspection_zonal_inspection insp_ins
   inner join gkh_dict_zonainsp ins on ins.id = insp_ins.zonal_inspection_id
   where insp_ins.inspection_id = $1;
begin 
   OPEN cursorCon; 
    loop 
    FETCH cursorCon INTO zoneName; 
    EXIT WHEN not FOUND; 
    if(result!='') 
    then 
        result:=result || zp; 
    end if; 
    result:=result || zoneName; 
    end loop; 
    CLOSE cursorCon; 
    return result; 
    end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }


        /// <summary>
        /// Функция возвращает строку наименований типов обследования главного распоряжения по id инспекции
        /// gjiGetInspDisposalTypeSurveys
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionDispTypeSurveys(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspDisposalTypeSurveys(insp_id number)
  RETURN varchar2 AS
   tsurvey_name varchar2(4000) :='';
   result varchar2(4000) :='';
   zp varchar2(4000):=', ';
   CURSOR cursorCon  IS
   select tsurvey.name
   from gji_disposal disp
       inner join gji_document doc on doc.id = disp.id
       left join gji_disposal_typesurvey disp_tsurvey on disp_tsurvey.disposal_id = doc.id
       left join gji_dict_typesurvey tsurvey on tsurvey.id = disp_tsurvey.typesurvey_id
   where disp.type_disposal = 10 and doc.inspection_id = insp_id;
begin
   OPEN cursorCon; 
   loop 
    FETCH cursorCon INTO tsurvey_name; 
    EXIT WHEN cursorCon%NOTFOUND; 
    IF (result is not null) 
    then 
        result:=result || zp;
    end if; 
    result:=result || tsurvey_name;    
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; 
";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspDisposalTypeSurveys(bigint)
  RETURNS text AS
$BODY$ 
declare 
   tsurvey_name text :='';
   result text :='';
   zp text:=', ';
   cursorCon CURSOR IS
   select tsurvey.name
   from gji_disposal disp
       inner join gji_document doc on doc.id = disp.id
       left join gji_disposal_typesurvey disp_tsurvey on disp_tsurvey.disposal_id = doc.id
       left join gji_dict_typesurvey tsurvey on tsurvey.id = disp_tsurvey.typesurvey_id
   where disp.type_disposal = 10 and doc.inspection_id = $1;
begin
   OPEN cursorCon; 
   loop 
    FETCH cursorCon INTO tsurvey_name; 
    EXIT WHEN not FOUND; 
    if(result != '') 
    then 
        result:=result || zp;
    end if; 
    result:=result || tsurvey_name; 
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Возвращает номер главного распоряжения проверки
        /// gjiGetInspDisposalNumber
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionDispNumber(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspDisposalNumber(insp_id number)
              RETURN varchar2 IS

               disp_number varchar2(4000) :='';
               result varchar2(4000) :='';
               zp varchar2(4000):=', ';
               CURSOR cursorCon IS
               select doc.document_number
               from gji_disposal disp
               inner join gji_document doc on doc.id = disp.id
               where disp.type_disposal = 10 and doc.inspection_id = insp_id;
            begin
               OPEN cursorCon; 
               loop 
                FETCH cursorCon INTO disp_number;
                EXIT WHEN cursorCon%NOTFOUND;
                IF (result is not null)
                then
                    result:=result || zp;
                end if;
                result:=result || disp_number;
               end loop;
               CLOSE cursorCon;
               return result;
            end; 
            ";
            }

            return @"
            CREATE OR REPLACE FUNCTION gjiGetInspDisposalNumber(bigint)
              RETURNS text AS
            $BODY$ 
            declare 
               disp_number text :='';
               result text :='';
               zp text:=', ';
               cursorCon CURSOR IS
               select doc.document_number
               from gji_disposal disp
               inner join gji_document doc on doc.id = disp.id
               where disp.type_disposal = 10 and doc.inspection_id = $1;
            begin
               OPEN cursorCon; 
               loop 
                FETCH cursorCon INTO disp_number;
                EXIT WHEN not FOUND;
                if(result != '')
                then
                    result:=result || zp;
                end if;
                result:=result || disp_number;
               end loop;
               CLOSE cursorCon;
               return result;
            end; 
            $BODY$
              LANGUAGE plpgsql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// Возвращает номера гжи обращений по который связаны с основанием проверки по обращениям
        /// gjiGetInspStatAppealsNumberGji
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspStatAppealsNumberGji(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspStatAppealsNumberGji(insp_stat_id NUMBER)
  RETURN varchar2 IS

    result varchar2(4000) := '';
    num varchar2(4000) :='';
    zp varchar2(4000) := ', ';
    CURSOR cursorCon  IS 
    select gac.gji_number
    from gji_appeal_citizens gac
        inner join GJI_BASESTAT_APPCIT gba on gba.gji_appcit_id=gac.id
    where gba.insp_stat_id = insp_stat_id and gac.gji_number is not null;
begin 
OPEN cursorCon; 
loop
FETCH cursorCon INTO num;
exit when cursorCon%notfound;
    if(result is not null)
    then 
        result:= result || zp;
    end if;
    result := result || num;
end loop; 
CLOSE cursorCon; 
return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspStatAppealsNumberGji(bigint)
  RETURNS text AS
$BODY$ 
declare 
    result text := '';
    num text :='';
    zp text := ', ';
    cursorCon CURSOR IS 
    select gac.gji_number
    from gji_appeal_citizens gac
        inner join GJI_BASESTAT_APPCIT gba on gba.gji_appcit_id=gac.id
    where gba.gji_insp_stat_id = $1 and gac.gji_number <> '';
begin 
OPEN cursorCon; 
loop
FETCH cursorCon INTO num;
exit when not found;
    if(result <> '')
    then 
        result:= result || zp;
    end if;
    result := result || num;
end loop; 
CLOSE cursorCon; 
return result; 
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Возвращает наименование документа отопительного сезона
        /// gjiGetHeatingSeasonDocName
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetHeatSeasonDocName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetHeatingSeasonDocName (objectid    NUMBER, type_doc    NUMBER)
   RETURN NUMBER
IS
   resultInt   NUMBER := 0;
   result      NUMBER := 0;

   CURSOR cursorCon
   IS
      SELECT hsd.id
        FROM GJI_HEATSEASON_DOCUMENT hsd
       WHERE hsd.HEATSEASON_ID = objectId AND hsd.type_document = type_doc;

BEGIN
   OPEN cursorCon;
   FETCH cursorCon INTO resultInt;
   IF (resultInt > 0)
   THEN
      result := 1;
   END IF;
   CLOSE cursorCon;
   RETURN result;
END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetHeatingSeasonDocName(objectid bigint, type_doc integer)
  RETURNS boolean AS
$BODY$ 
 declare
 resultInt integer:=0;
 result boolean:=false;
 cursorCon CURSOR IS 
     select hsd.id
     from GJI_HEATSEASON_DOCUMENT hsd
     where hsd.HEATSEASON_ID = objectId and hsd.type_document = type_doc;
 begin OPEN cursorCon;
FETCH cursorCon INTO resultInt;
if(resultInt > 0)
then
    result=true;
end if;
CLOSE cursorCon;
return result;
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований жилых домов документа гжи
        /// gjiGetDocMuByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentRobjectMuNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocMuByViolStage(doc_id number) RETURN varchar2 IS
                   result varchar2(4000) := ''; 
                   zp varchar2(4000) := ', ';
                   mu_name varchar2(4000) := '';
                  CURSOR cursorCon IS 
                    select distinct mu.name
                    from gji_inspection_viol_stage viol_stage 
                        join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
                        join gkh_reality_object ro on ro.id = insp_viol.reality_object_id
                        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                   where viol_stage.document_id = doc_id;
                 begin 
                    OPEN cursorCon;
                    loop
                    FETCH cursorCon INTO mu_name;
                    EXIT WHEN cursorCon%NOTFOUND;
                    if(result is not null)
                    then
                        result := result || zp;
                    end if;
                    result := result || mu_name;
                    end loop;
                    CLOSE cursorCon; 
                    return result; 
                end; 
                ";
            }

            return @"
                CREATE OR REPLACE FUNCTION gjiGetDocMuByViolStage(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   result text := ''; 
                   zp text := ', ';
                   mu_name text := '';
                   cursorCon CURSOR IS 
                    select distinct mu.name
                    from gji_inspection_viol_stage viol_stage 
                        join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
                        join gkh_reality_object ro on ro.id = insp_viol.reality_object_id
                        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                   where viol_stage.document_id = $1;
                 begin 
                    OPEN cursorCon;
                    loop
                    FETCH cursorCon INTO mu_name;
                    EXIT WHEN not FOUND;
                    if(result <> '')
                    then
                        result := result || zp;
                    end if;
                    result := result || mu_name;
                    end loop;
                    CLOSE cursorCon; 
                    return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Возвращает идентификатор муниципального образования первого дома документа гжи
        /// gjiGetDocMuIdByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentRobjectMuIdByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocMuIdByViolStage(doc_id NUMBER)
  RETURN NUMBER IS
   result NUMBER := 0;
   CURSOR cursorCon IS 
    select distinct mu.id
    from gji_inspection_viol_stage viol_stage 
        join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
        join gkh_reality_object ro on ro.id = insp_viol.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
   where viol_stage.document_id = doc_id AND ROWNUM = 1;
 begin
    OPEN cursorCon;
    FETCH cursorCon INTO result;
    CLOSE cursorCon;
    return result;
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocMuIdByViolStage(bigint)
  RETURNS integer AS
$BODY$ 
declare 
   result integer := 0;
   cursorCon CURSOR IS 
    select distinct mu.id
    from gji_inspection_viol_stage viol_stage 
        join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
        join gkh_reality_object ro on ro.id = insp_viol.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
   where viol_stage.document_id = $1
   limit 1;
 begin
    OPEN cursorCon;
    FETCH cursorCon INTO result;
    CLOSE cursorCon;
    return result;
end;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов документа гжи
        /// gjiGetDocRobjectByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocRobjectByViolStage(doc_id NUMBER)
  RETURN varchar2 IS
   objectId varchar2(4000) :=''; 
   result varchar2(4000) := '/'; 
   CURSOR cursorCon IS 
   select distinct insp_viol.reality_object_id 
   from gji_inspection_viol_stage viol_stage 
   join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
   where viol_stage.document_id = doc_id; 
begin 
   OPEN cursorCon;
   loop 
   FETCH cursorCon INTO objectId; 
   EXIT WHEN cursorCon%NOTFOUND;
      result:=result || objectId ||'/'; 
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocRobjectByViolStage(bigint)
  RETURNS text AS
$BODY$ 
declare 
   objectId text :=''; 
   result text := '/'; 
   cursorCon CURSOR IS 
   select distinct insp_viol.reality_object_id 
   from gji_inspection_viol_stage viol_stage 
   join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
   where viol_stage.document_id = $1; 
begin 
   OPEN cursorCon;
   loop 
   FETCH cursorCon INTO objectId; 
   EXIT WHEN not FOUND;
      result:=result || objectId ||'/'; 
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// возвращает строку наименвоаний муниципальных образований жилых домов родительского документа
        /// gjiGetDocParentMuByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRobjectMuNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocParentMuByViolStage(child_id NUMBER)
  RETURN varchar2 IS

   mu_name varchar2(4000) :=''; 
   zp varchar2(4000) := ', ';
   result varchar2(4000) := ''; 
   CURSOR cursorCon IS 
   SELECT DISTINCT munt.name
            FROM 
            (
        select 
        distinct doc_child.children_id, mu.name
        from gji_document_children doc_child
        JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
        JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
        JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
        JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
        LEFT JOIN gkh_dict_municipality mu on mu.id = ro.municipality_id
        where doc_par.TYPE_DOCUMENT != 80
            UNION ALL
            select 
           distinct doc_child.children_id, mu.name
            from gji_document_children doc_child
                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
                JOIN gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
                LEFT JOIN gkh_dict_municipality mu on mu.id = ro.municipality_id
            where doc_par.TYPE_DOCUMENT = 80
    ) munt
   where munt.children_id = child_id;
begin 
   OPEN cursorCon;
   loop 
   FETCH cursorCon INTO mu_name; 
   EXIT WHEN cursorCon%NOTFOUND;
   if(result is not null)
   then
      result:=result || zp; 
   end if;
   result := result || mu_name;
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjigetdocparentmubyviolstage(bigint)
  RETURNS text AS
$BODY$ 
declare 
   mu_name text :=''; 
   zp text := ', ';
   result text := ''; 
   cursorCon CURSOR IS 
   SELECT DISTINCT munt.name
            FROM 
            (
        select 
        distinct doc_child.children_id, mu.name
        from gji_document_children doc_child
        JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
        JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
        JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
        JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
        LEFT JOIN gkh_dict_municipality mu on mu.id = ro.municipality_id
        where doc_par.TYPE_DOCUMENT != 80
            UNION ALL
            select 
           distinct doc_child.children_id, mu.name
            from gji_document_children doc_child
                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
                JOIN gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
                LEFT JOIN gkh_dict_municipality mu on mu.id = ro.municipality_id
            where doc_par.TYPE_DOCUMENT = 80
    ) munt
   where munt.children_id = $1;
begin 
   OPEN cursorCon;
   loop 
   FETCH cursorCon INTO mu_name; 
   EXIT WHEN not FOUND;
   if(result <> '')
   then
      result:=result || zp; 
   end if;
   result := result || mu_name;
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// возвращает идентификатор мениципального образования первого жилого дома родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRobjectMuIdByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE FUNCTION gjiGetDocParentMuIdByViolStage(child_id NUMBER)
  RETURN  NUMBER IS

   result NUMBER := 0; 
   CURSOR cursorCon IS 
  SELECT DISTINCT munt.mu_id
            FROM 
            (
             select doc_child.children_id, mu.id mu_id
               from gji_document_children doc_child
                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
                INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
                join gkh_reality_object ro on ro.id = viol.reality_object_id
                left join gkh_dict_municipality mu on mu.id = ro.municipality_id
               where doc_par.TYPE_DOCUMENT != 80
            UNION ALL
            select doc_child.children_id, mu.id  mu_id
               from gji_document_children doc_child
                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
                join gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
                left join gkh_dict_municipality mu on mu.id = ro.municipality_id
               where doc_par.TYPE_DOCUMENT = 80
             ) munt
            WHERE munt.children_id = child_id
   AND ROWNUM= 1;
begin 
   OPEN cursorCon;
   FETCH cursorCon INTO result; 
   CLOSE cursorCon; 
   return result; 
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocParentMuIdByViolStage(bigint)
    RETURNS integer AS
$BODY$ 
declare 
   result integer := 0; 
   cursorCon CURSOR IS 
            SELECT DISTINCT munt.mu_id
            FROM 
            (
             select doc_child.children_id, mu.id mu_id
               from gji_document_children doc_child
                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
                INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
                join gkh_reality_object ro on ro.id = viol.reality_object_id
                left join gkh_dict_municipality mu on mu.id = ro.municipality_id
               where doc_par.TYPE_DOCUMENT != 80
            UNION ALL
            select doc_child.children_id, mu.id  mu_id
               from gji_document_children doc_child
                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
                join gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
                left join gkh_dict_municipality mu on mu.id = ro.municipality_id
               where doc_par.TYPE_DOCUMENT = 80
             ) munt
            WHERE munt.children_id = $1
   limit 1;
begin 
   OPEN cursorCon;
   FETCH cursorCon INTO result; 
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocParentRobjByViolStage(child_id NUMBER)
  RETURN varchar2 IS

   objectId varchar2(4000) :=''; 
   result varchar2(4000) := '/'; 
   CURSOR cursorCon IS 
   select distinct viol.reality_object_id
   from gji_document_children doc_child
   INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
   INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
   where doc_child.children_id = child_id;
begin 
   OPEN cursorCon;
   loop 
   FETCH cursorCon INTO objectId; 
   EXIT WHEN cursorCon%NOTFOUND;
      result:=result || objectId ||'/'; 
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocParentRobjByViolStage(bigint)
  RETURNS text AS
$BODY$ 
declare 
   objectId text :=''; 
   result text := '/'; 
   cursorCon CURSOR IS 
   select distinct viol.reality_object_id
   from gji_document_children doc_child
   INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
   INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
   where doc_child.children_id = $1;
begin 
   OPEN cursorCon;
   loop 
   FETCH cursorCon INTO objectId; 
   EXIT WHEN not FOUND;
      result:=result || objectId ||'/'; 
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }



        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRoAddrByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocParRoAdrByViolStage(child_id NUMBER)
  RETURN varchar2 IS

   address varchar2(4000) :=''; 
   result varchar2(4000) := ''; 
   zp NVARCHAR2(4000) :='; '; 
   CURSOR cursorCon(child_id NUMBER) IS 
   select distinct ro.address
   from gji_document_children doc_child
   INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
   INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
   INNER JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
   where doc_child.children_id = child_id;
BEGIN
    OPEN cursorCon(child_id); 
        LOOP 
        FETCH cursorCon INTO address; 
        EXIT WHEN cursorCon%NOTFOUND; 
        IF (result is not null) 
        THEN 
            result:=result || zp; 
        END if; 
        result:=result || address; 
        END LOOP; 
        CLOSE cursorCon; 
        return result; 
    END; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocParRoAdrByViolStage(bigint)
  RETURNS text AS
$BODY$ 
declare 
   address text :=''; 
   result text := ''; 
   zp text:='; '; 
   cursorCon CURSOR IS 
   select distinct ro.address
   from gji_document_children doc_child
   INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
   INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
   INNER JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
   where doc_child.children_id = $1;
begin 
   OPEN cursorCon;
   loop 
    FETCH cursorCon INTO address; 
    EXIT WHEN not FOUND; 
    if(result!='') 
    then 
        result:=result || zp; 
    end if; 
    result:=result || address; 
    end loop; 
    CLOSE cursorCon; 
    return result; 
    end;  
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        private string CreateFunctionGetDocumentArticleLaw(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE FUNCTION gjiGetDocArticleLaw(bigint)
    RETURNS varchar AS
$BODY$ 
declare 
    result varchar := null;
    doctype int := 0;
begin 
	select type_document into doctype from gji_document where id = $1;

	if doctype = 60 then
		select string_agg(al.name,'; ') into result
		from gji_document doc
		join gji_protocol prot on prot.id = doc.id
		join gji_protocol_artlaw protal on protal.protocol_id = prot.id
		join gji_dict_articlelaw al on al.id = protal.articlelaw_id
		where doc.id =$1;
	end if;

	if doctype = 140 then
		select string_agg(al.name,'; ') into result
		from gji_document doc
		join gji_protocol197 prot on prot.id = doc.id
		join gji_protocol197_artlaw protal on protal.protocol_id = prot.id
		join gji_dict_articlelaw al on al.id = protal.articlelaw_id
		where doc.id =$1;
	end if;

	if doctype = 70 then
		select coalesce(p.s, p197.s) into result
    from gji_document doc
    join gji_document_children dc on dc.parent_id = doc.id
    join gji_resolution res on res.id = dc.children_id
    left join (
    select gp.id, string_agg(al.name,'; ') as s
    from gji_protocol gp
    join gji_protocol_artlaw protal on protal.protocol_id = gp.id
    join gji_dict_articlelaw al on al.id = protal.articlelaw_id
    group by 1 ) p on p.id = dc.parent_id
    left join (
    select gp.id, string_agg(al.name,'; ') as s
    from gji_protocol197 gp 
    join gji_protocol197_artlaw protal on protal.protocol_id = gp.id
    join gji_dict_articlelaw al on al.id = protal.articlelaw_id
    group by 1 ) p197 on p197.id = dc.parent_id
    where res.id =$1;
	end if;
	
	return result;
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }


        /// <summary>
        /// Возвращает строку фио инспекторов документа гжи
        /// gji_get_document_inspectors
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentInspectors(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocumentInspectors (id NUMBER)
                           RETURN NVARCHAR2
                        IS
                           inspectorFIO NVARCHAR2(300):='';
                           result NVARCHAR2(4000):='';
                           zp NVARCHAR2(300):=', ';
                         CURSOR cursorCon(docId NUMBER) IS 
                                select insptr.FIO 
                                from gji_document doc 
                                    inner join GJI_DOCUMENT_INSPECTOR doc_insptr on doc_insptr.DOCUMENT_ID=doc.id 
                                    inner join gkh_dict_inspector insptr on insptr.id=doc_insptr.inspector_id
                                where doc.id = docId;
                        BEGIN
                        OPEN cursorCon(id); 
                            LOOP 
                            FETCH cursorCon INTO inspectorFIO; 
                            EXIT WHEN cursorCon%NOTFOUND; 
                            IF (result is not null) 
                            THEN 
                                result:=result || zp; 
                            END if; 
                            result:=result || inspectorFIO; 
                            END LOOP; 
                            CLOSE cursorCon; 
                            return result; 
                        END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocumentInspectors(bigint)
RETURNS text AS
$BODY$ 
declare 
    inspectorFIO text :=''; 
    result text :=''; 
    zp text:=', '; 
    cursorCon CURSOR IS 
        select insptr.FIO 
        from gji_document doc 
            inner join GJI_DOCUMENT_INSPECTOR doc_insptr on doc_insptr.DOCUMENT_ID=doc.id 
            inner join gkh_dict_inspector insptr on insptr.id=doc_insptr.inspector_id
        where doc.id = $1;
begin 
   OPEN cursorCon; 
    loop 
    FETCH cursorCon INTO inspectorFIO; 
    EXIT WHEN not FOUND; 
    if(result!='') 
    then 
        result:=result || zp; 
    end if; 
    result:=result || inspectorFIO; 
    end loop; 
    CLOSE cursorCon; 
    return result; 
    end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// Возвращает количество нарушений документа гжи
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentCountViolationByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocCountViolByViolStage (id NUMBER)
                           RETURN NUMBER
                        IS
                           result NUMBER;
                         CURSOR cursorCon(docId NUMBER) IS 
                                select count(distinct viol_stage.inspection_viol_id) 
                                from gji_inspection_viol_stage viol_stage 
                                where viol_stage.document_id = docId;
                        BEGIN
                           OPEN cursorCon(id);
                           loop
                           FETCH cursorCon INTO result;
                           EXIT WHEN cursorCon%NOTFOUND;
                           CLOSE cursorCon;
                           return result;
                           end loop;
                        END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocCountViolByViolStage(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer :=0;
begin 
    select count(distinct viol_stage.inspection_viol_id) into result
    from gji_inspection_viol_stage viol_stage 
    where viol_stage.document_id =$1;
    return result;
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// возвращает количество жилых домов документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentCountRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocCntRealObjByViolStage (id NUMBER)
                           RETURN NUMBER
                        IS
                           result NUMBER;
                         CURSOR cursorCon(docId NUMBER) IS 
                                select count(distinct insp_viol.reality_object_id) 
                                from gji_inspection_viol_stage viol_stage 
                                join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
                               where viol_stage.document_id = docId;
                        BEGIN
                           OPEN cursorCon(id);
                           loop
                           FETCH cursorCon INTO result;
                           EXIT WHEN cursorCon%NOTFOUND;
                           CLOSE cursorCon;
                           return result;
                           end loop;
                        END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocCntRealObjByViolStage(bigint)
  RETURNS integer AS
$BODY$ 
declare 
    result integer :=0;
begin 
    select count(distinct insp_viol.reality_object_id)  into result
    from gji_inspection_viol_stage viol_stage 
    join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
    where viol_stage.document_id = $1;
    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// возвращает количество дочерних документов документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentCountChildDocument(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocCountChildDoc (id NUMBER)
                           RETURN NUMBER
                        IS
                           result NUMBER;
                         CURSOR cursorCon(docId NUMBER) IS 
                                select count(doc_child.id) 
                               from gji_document_children doc_child
                               inner join gji_document doc on doc.id = doc_child.children_id
                               where doc_child.parent_id = docId and doc.type_document != 30;
                        BEGIN
                           OPEN cursorCon(id);
                           loop
                           FETCH cursorCon INTO result;
                           EXIT WHEN cursorCon%NOTFOUND;
                           CLOSE cursorCon;
                           return result;
                           end loop;
                        END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocCountChildDoc(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer := 0;
begin 
    select count(doc_child.id) into result
    from gji_document_children doc_child
    inner join gji_document doc on doc.id = doc_child.children_id
    where doc_child.parent_id = $1 and doc.type_document != 30; 
    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// возвращает строку наименований типов обследования распоряжения
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDisposalTypeSurveys(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE FUNCTION gjiGetDisposalTypeSurveys (id1 NUMBER)
RETURN NVARCHAR2
IS
    tsurvey_name NVARCHAR2(300) := '';
    result NVARCHAR2(4000) := '';
    zp NVARCHAR2(300) := ', ';
    CURSOR cursorCon(docId NUMBER) IS 
    select 
        tsurvey.name 
    from gji_disposal_typesurvey disp_tsurvey 
        left join gji_dict_typesurvey tsurvey on tsurvey.id = disp_tsurvey.typesurvey_id 
    where disp_tsurvey.disposal_id = docId;
BEGIN
    OPEN cursorCon(id1); 
    loop 
    FETCH cursorCon INTO tsurvey_name; 
    EXIT WHEN cursorCon%NOTFOUND; 
    if (result is not null) 
    then 
        result:=result||zp;
    end if; 
    result:=result||tsurvey_name;
    end loop; 
    CLOSE cursorCon; 
    return result; 
END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDisposalTypeSurveys(bigint)
  RETURNS text AS
$BODY$ 
declare 
   tsurvey_name text :='';  
   result text :=''; 
   zp text:=', '; 
   cursorCon CURSOR IS 
    select tsurvey.name 
    from gji_disposal doc 
    left join gji_disposal_typesurvey disp_tsurvey on disp_tsurvey.disposal_id = doc.id 
    left join gji_dict_typesurvey tsurvey on tsurvey.id = disp_tsurvey.typesurvey_id 
   where doc.id = $1;
begin 
   OPEN cursorCon; 
   loop 
    FETCH cursorCon INTO tsurvey_name; 
    EXIT WHEN not FOUND; 
    if(result != '') 
    then 
        result:=result || zp;
    end if; 
    result:=result || tsurvey_name; 
   end loop; 
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// если распоряжение главное то возвращает строку адресов жилых домов основания
        /// иначе строку адресов жилых домов родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDisposalRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDispRealityObj (id1 NUMBER, id2 NUMBER)
                           RETURN NVARCHAR2
                        IS
                           cnt NUMBER;
                           result NVARCHAR2(4000) := '';
                         CURSOR cursorCon(childId NUMBER) IS 
                            select count(doc_child.id) 
                            from gji_document_children
                            doc_child where doc_child.children_id = childId;
                        BEGIN
                           result := '/';
                           OPEN cursorCon(id1); 
                           loop
                           FETCH cursorCon INTO cnt; 
                           EXIT WHEN cursorCon%NOTFOUND; 
                           CLOSE cursorCon; 
                            
                           if(cnt>0)
                           then
                               return gjiGetDocParentRobjByViolStage(id1);
                            end if;
                            
                            return gjiGetInspRobject(id2); 
                            end loop;
                        END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDispRealityObj(bigint, bigint)
  RETURNS text AS
$BODY$ 
declare 
    cnt integer := 0;
    cursorCon CURSOR IS
    select 
        count(doc_child.id) 
    from gji_document_children 
doc_child where doc_child.children_id = $1;
begin
   OPEN cursorCon; 
LOOP
   FETCH cursorCon INTO cnt; 
   EXIT WHEN not FOUND; 
END LOOP;
   CLOSE cursorCon; 
    
   if(cnt>0)
   then
       return gjiGetDocParentRobjByViolStage($1);
    end if;
    
    return gjiGetInspRobject($2); 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// возвращает количество жилых домов родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFuncGetDocumentParentCountRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDocPrntCntRobjByViolStg(childId NUMBER)
  RETURN NUMBER IS

   result NUMBER := 0; 
   CURSOR cursorCon IS 
   select count(distinct viol.reality_object_id)
   from gji_document_children doc_child
       INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
       INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
   where doc_child.children_id = childId;
begin 
   OPEN cursorCon;
   loop
   FETCH cursorCon INTO result; 
   EXIT WHEN cursorCon%NOTFOUND; 
   end loop;
   CLOSE cursorCon; 
   return result; 
end; ";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDocPrntCntRobjByViolStg(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer := 0;
begin 
    select count(distinct viol.reality_object_id) into result
    from gji_document_children doc_child
        INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
        INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
    where doc_child.children_id = $1;
    return result;
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает количество жилых домов основания проверки
        /// gjiGetInspCountRobject
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFuncGetInspectionCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspCountRobject(insp_id NUMBER)
  RETURN NUMBER IS
   result NUMBER :=0; 
   CURSOR cursorCon  IS 
    select count(insp_ro.reality_object_id) 
    from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp_id; 
begin 
    OPEN cursorCon; 
      loop 
    FETCH cursorCon INTO result; 
    EXIT WHEN cursorCon%NOTFOUND; 
     end loop;
    CLOSE cursorCon; 
    return result; 
end; 
";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetInspCountRobject(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer :=0;
begin 
    select count(insp_ro.reality_object_id) INTO result
    from gji_inspection_robject insp_ro where insp_ro.inspection_id = $1; 
    return result; 
end; 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;";
        }

        /// <summary>
        /// если распоряжение главное то возвращает количество жилых домов основания
        /// иначе количество жилых домов родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDisposalCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDispCntRobj (id1 NUMBER, id2 NUMBER) RETURN NUMBER IS
                           cnt NUMBER;
                        BEGIN

execute immediate 'select count(doc_child.id) from 
 gji_document_children doc_child where doc_child.children_id = ' || id1 into cnt;

                           if(cnt>0)
                           then
                               return gjiGetDocPrntCntRobjByViolStg(id1);
                            end if;
                            
                            return gjiGetInspCountRobject(id2); 
                        END;";
            }

            //т.к. есть зависимость от других функций то перед создание этой нужно обязательно создать зависимые
            return
                //this.CreateFuncGetDocumentParentCountRobjectByViolStage(dbmsKind)
                //+
                //this.CreateFuncGetInspectionCountRobject(dbmsKind)
                //+
@"
CREATE OR REPLACE FUNCTION gjiGetDispCntRobj(bigint, bigint)
    RETURNS integer AS
$BODY$ 
declare 
    cnt integer := 0;
begin
    select count(doc_child.id) into cnt
    from gji_document_children doc_child 
    where doc_child.children_id = $1;
    
    if( cnt>0)
        then return gjiGetDocPrntCntRobjByViolStg($1);
    end if;
   
    return gjiGetInspCountRobject($2); 
end; 
$BODY$
    LANGUAGE plpgsql VOLATILE
    COST 100;";
        }

        /// <summary>
        /// Возвращает создан ли у распоряжения акт проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDisposalActCheckExist(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetDispActCheckExist (id NUMBER)
                               RETURN NUMBER
                            IS
                               resultInt   NUMBER := 0;
                               result      NUMBER := 0;

                            CURSOR cursorCon(childrenId NUMBER) IS 
                                                        select count(doc_child.id)
                                                            from gji_document_children doc_child
                                                            inner join gji_document doc on doc.id = doc_child.children_id
                                                            where doc.type_document=20 and doc_child.parent_id = childrenId;

                            BEGIN
                               OPEN cursorCon(id);
                               FETCH cursorCon INTO resultInt;
                               IF (resultInt > 0)
                               THEN
                                  result := 1;
                               END IF;
                               CLOSE cursorCon;
                               RETURN result;
                            END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetDispActCheckExist(bigint)
    RETURNS boolean AS
$BODY$ 
declare 
    result integer :=0;   
begin 
    select count(doc_child.id) into result
    from gji_document_children doc_child
    inner join gji_document doc on doc.id = doc_child.children_id
    where doc.type_document=20 and doc_child.parent_id=$1;
    return result>0; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает количество услуг предоставляемых юр.лицом в уведомлении о начале предпринимательской деятельности
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetBusinessActServCnt(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetBusinessActServCnt(id NUMBER)
                        RETURN NUMBER 
                        IS
                        n number;
                        BEGIN
                            execute immediate 'select count(serv_jurid.id)
                            from gji_dict_serv_jurid serv_jurid
                                left join gji_buisnes_notif buis_notif on serv_jurid.buisnes_notif_id = buis_notif.id
                            where buis_notif.id = ' || id into n;
                            return n;
                        END;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetBusinessActServCnt(bigint)
RETURNS bigint AS
$BODY$
    select count(serv_jurid.id)
    from gji_dict_serv_jurid serv_jurid
    left join gji_buisnes_notif buis_notif on serv_jurid.buisnes_notif_id = buis_notif.id
    where buis_notif.id = $1 
$BODY$
LANGUAGE sql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований акт обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActsurveyRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActSurveyRobjMun(id NUMBER)
                          RETURN NVARCHAR2 IS

                        mu_name NVARCHAR2(500) :=''; 
                        result NVARCHAR2(4000) :=''; 
                        zp NVARCHAR2(500):=', '; 
                            CURSOR cursorCon(act_id NUMBER) IS 
                            select distinct (mu.name)
                            from 

                        GJI_ACTSURVEY_ROBJECT actsur_ro 
                            join gkh_reality_object ro on ro.id=actsur_ro.reality_object_id 
                            left join gkh_dict_municipality mu on mu.id = ro.municipality_id 
                           where actsur_ro.ACTSURVEY_ID = act_id;
                        begin 
                        OPEN cursorCon(id);
                           loop  
                           FETCH cursorCon INTO mu_name; 
                            EXIT WHEN cursorCon%NOTFOUND; 
                            if(result is not null) 
                            then 
                                result:=result || zp; 
                            end if; 
                            result:=result || mu_name; 
                           end loop; 
                            CLOSE cursorCon; 
    
                        return result; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActSurveyRobjMun(bigint)
    RETURNS text AS
$BODY$ 
declare 
mu_name text :=''; 
result text :=''; 
zp text:=', '; 
    cursorCon CURSOR IS 
    select distinct (mu.name)
    from GJI_ACTSURVEY_ROBJECT actsur_ro 
    join gkh_reality_object ro on ro.id=actsur_ro.reality_object_id 
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id 
    where actsur_ro.ACTSURVEY_ID = $1;
begin 
    OPEN 
        cursorCon; 
    loop  
    FETCH cursorCon INTO mu_name; 
    EXIT WHEN not FOUND; 
    if(result <> '') 
    then 
        result:=result || zp; 
    end if; 
    result:=result || mu_name; 
    end loop; 
    CLOSE cursorCon; 
    
return result; 
end; 
$BODY$
    LANGUAGE plpgsql VOLATILE
    COST 100;";
        }

        /// <summary>
        /// Возвращает идентификатор муниципального образования первого жилого дома акта обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActsurveyRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActSurveyRobjMunId(id NUMBER)
                          RETURN NUMBER IS

                        result NUMBER :=0; 
                            CURSOR cursorCon(act_id NUMBER) IS 
                            select distinct (mu.id)
                            from GJI_ACTSURVEY_ROBJECT actsur_ro 
                            join gkh_reality_object ro on ro.id=actsur_ro.reality_object_id 
                            left join gkh_dict_municipality mu on mu.id = ro.municipality_id 
                           where actsur_ro.ACTSURVEY_ID = act_id and rownum = 1;
                        begin 
                        OPEN cursorCon(id);
                           FETCH cursorCon INTO result; 
                            CLOSE cursorCon; 
                            return result; 
                        return result; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActSurveyRobjMunId(bigint)
    RETURNS integer AS
$BODY$ 
declare 
result integer := 0;
begin 
    select distinct (mu.id) into result
    from GJI_ACTSURVEY_ROBJECT actsur_ro 
    join gkh_reality_object ro on ro.id=actsur_ro.reality_object_id 
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id 
    where actsur_ro.ACTSURVEY_ID = $1
    limit 1;
    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку адресов жилых домов акта обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActsurveyRobjectAddress(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActSurvRobjAddress(id NUMBER)
  RETURN NVARCHAR2 IS

   address NVARCHAR2(300) :=''; 
   result NVARCHAR2(4000) :=''; 
   zp NVARCHAR2(300):=', '; 
   CURSOR cursorCon(act_id NUMBER) IS 
    select ro.address 
    from GJI_ACTSURVEY act 
    join GJI_ACTSURVEY_ROBJECT act_ro on act_ro.actsurvey_id = act.id 
    join gkh_reality_object ro on ro.id=act_ro.reality_object_id
   where act.id = act_id;
begin 
    OPEN cursorCon(id); 
   loop 
    FETCH cursorCon INTO address; 
    EXIT WHEN cursorCon%NOTFOUND; 
    IF (result is not null) 
    then 
        result:=result || zp; 
    end if;
    result:=result || address; 
   end loop; 
   CLOSE cursorCon; 
    return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActSurvRobjAddress(bigint)
    RETURNS text AS
$BODY$ declare 
    address text :=''; 
    result text :=''; 
    zp text:=', '; 
    cursorCon CURSOR IS 
    select ro.address 
    from GJI_ACTSURVEY act 
    join GJI_ACTSURVEY_ROBJECT act_ro on act_ro.actsurvey_id = act.id 
    join gkh_reality_object ro on ro.id=act_ro.reality_object_id
    where act.id = $1;
begin 
    OPEN cursorCon; 
    loop 
    FETCH cursorCon INTO address; 
    EXIT WHEN not FOUND; 
    if(result!='') 
    then 
        result:=result || zp; 
    end if;
    result:=result || address; 
    end loop; 
    CLOSE cursorCon; 
    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов акта обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActsurveyRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActSurvRobj(id NUMBER)
                          RETURN NVARCHAR2 AS

                           objectId NVARCHAR2(300) :=''; 
                           result NVARCHAR2(4000) := '/';    
                           CURSOR cursorCon(act_id NUMBER) IS 
                           select act_ro.reality_object_id 
                           from GJI_ACTSURVEY_ROBJECT act_ro where act_ro.actsurvey_id = act_id; 
                        begin
                           OPEN cursorCon(id);
                           loop 
                           FETCH cursorCon INTO objectId;
                           EXIT WHEN cursorCon%NOTFOUND; 
                              result:=result || objectId ||'/';
                           end loop; 
                           CLOSE cursorCon; 
                           return result; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActSurvRobj(bigint)
    RETURNS text AS
$BODY$ 
declare 
    objectId text :=''; 
    result text := '/';  
    cursorCon CURSOR IS 
    select act_ro.reality_object_id 
    from GJI_ACTSURVEY_ROBJECT act_ro where act_ro.actsurvey_id = $1; 
begin
    OPEN cursorCon;
    loop 
    FETCH cursorCon INTO objectId;
    EXIT WHEN not FOUND; 
        result:=result || objectId ||'/';
    end loop; 
    CLOSE cursorCon; 
    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальнызх образований жилых домов Акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActCheckRobjMun(id NUMBER)
                          RETURN NVARCHAR2 IS

                            result NVARCHAR2(4000) := '';
                            zp NVARCHAR2(300) := ', ';
                            mu_name NVARCHAR2(500) := '';
                            CURSOR cursorCon(act_id NUMBER) IS 
                           select distinct mu.name
                           from GJI_ACTCHECK_ROBJECT actcheck_ro 
                            join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
                            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                           where actcheck_ro.actcheck_id = act_id;
                        begin 
                        OPEN cursorCon(id); 
                        loop
                        FETCH cursorCon INTO mu_name;
                        EXIT WHEN cursorCon%NOTFOUND;
                            if(result is not null)
                            then
                                result := result || zp;
                            end if;
                            result := result || mu_name;
                        end loop;
                        CLOSE cursorCon; 
                        return result; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActCheckRobjMun(bigint)
    RETURNS text AS
$BODY$ 
declare 
    result text := '';
    zp text := ', ';
    mu_name text := '';
    cursorCon CURSOR IS 
    select distinct mu.name
    from GJI_ACTCHECK_ROBJECT actcheck_ro 
    join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id
    where actcheck_ro.actcheck_id = $1;
begin 
OPEN cursorCon; 
loop
FETCH cursorCon INTO mu_name;
EXIT WHEN not FOUND;
    if(result <> '')
    then
        result := result || zp;
    end if;
    result := result || mu_name;
end loop;
CLOSE cursorCon; 
return result; 
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований жилых домов Акта без взаимодейтсвия
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActisolatedRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActIsolatedRobjMun(id NUMBER)
                          RETURN NVARCHAR2 IS

                            result NVARCHAR2(4000) := '';
                            zp NVARCHAR2(300) := ', ';
                            mu_name NVARCHAR2(500) := '';
                            CURSOR cursorCon(act_id NUMBER) IS 
                           select distinct mu.name
                           from GJI_ACTISOLATED_ROBJECT actisolated_ro 
                            join gkh_reality_object ro on ro.id = actisolated_ro.reality_object_id
                            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                           where actisolated_ro.actisolated_id = act_id;
                        begin 
                        OPEN cursorCon(id); 
                        loop
                        FETCH cursorCon INTO mu_name;
                        EXIT WHEN cursorCon%NOTFOUND;
                            if(result is not null)
                            then
                                result := result || zp;
                            end if;
                            result := result || mu_name;
                        end loop;
                        CLOSE cursorCon; 
                        return result; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActIsolatedRobjMun(bigint)
    RETURNS text AS
$BODY$ 
declare 
    result text := '';
    zp text := ', ';
    mu_name text := '';
    cursorCon CURSOR IS 
    select distinct mu.name
    from GJI_ACTISOLATED_ROBJECT actisolated_ro 
    join gkh_reality_object ro on ro.id = actisolated_ro.reality_object_id
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id
    where actisolated_ro.actisolated_id = $1;
begin 
OPEN cursorCon; 
loop
FETCH cursorCon INTO mu_name;
EXIT WHEN not FOUND;
    if(result <> '')
    then
        result := result || zp;
    end if;
    result := result || mu_name;
end loop;
CLOSE cursorCon; 
return result; 
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает идентификатор муниципального образования первого жилого дома акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActCheckRobjectMuId(id NUMBER)
                          RETURN NUMBER IS

                            result NVARCHAR2(4000) := '';
                            CURSOR cursorCon(act_id NUMBER) IS 
                           select distinct mu.id
                           from GJI_ACTCHECK_ROBJECT actcheck_ro 
                            join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
                            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                           where actcheck_ro.actcheck_id = act_id and rownum = 1;
                        begin 
                        OPEN cursorCon(id); 
                        loop
                        FETCH cursorCon INTO result;
                        CLOSE cursorCon; 
                        return result;
                        end loop; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActCheckRobjectMuId(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer := 0;
begin 
    select distinct mu.id into result
    from GJI_ACTCHECK_ROBJECT actcheck_ro 
    join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id
    where actcheck_ro.actcheck_id = $1
    limit 1;
    return result; 
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает идентификатор муниципального образования первого жилого дома акта без взаимодействия
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActisolatedRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActIsolatedRobjectMuId(id NUMBER)
                          RETURN NUMBER IS

                            result NVARCHAR2(4000) := '';
                            CURSOR cursorCon(act_id NUMBER) IS 
                           select distinct mu.id
                           from GJI_ACTISOLATED_ROBJECT actisolated_ro 
                            join gkh_reality_object ro on ro.id = actisolated_ro.reality_object_id
                            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                           where actisolated_ro.actisolated_id = act_id and rownum = 1;
                        begin 
                        OPEN cursorCon(id); 
                        loop
                        FETCH cursorCon INTO result;
                        CLOSE cursorCon; 
                        return result;
                        end loop; 
                        end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActIsolatedRobjectMuId(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer := 0;
begin 
    select distinct mu.id into result
    from GJI_ACTISOLATED_ROBJECT actisolated_ro 
    join gkh_reality_object ro on ro.id = actisolated_ro.reality_object_id
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id
    where actisolated_ro.actisolated_id = $1
    limit 1;
    return result; 
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActCheckRobj(act_id NUMBER)
  RETURN NVARCHAR2 IS
 
   objectId NVARCHAR2(300) :=''; 
   result NVARCHAR2(4000) := '/';    
   CURSOR cursorCon(act_id NUMBER) IS 
   select actcheck_ro.reality_object_id 
   from GJI_ACTCHECK_ROBJECT actcheck_ro where actcheck_ro.actcheck_id = act_id; 
begin 
   OPEN cursorCon(act_id);
   loop
   FETCH cursorCon INTO objectId;
   EXIT WHEN cursorCon%NOTFOUND; 
   result:=result || objectId ||'/'; 
   end loop;
   CLOSE cursorCon; 
   return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActCheckRobj(bigint)
  RETURNS text AS
$BODY$ 
declare 
   objectId text :=''; 
   result text := '/';  
   cursorCon CURSOR IS 
   select actcheck_ro.reality_object_id 
   from GJI_ACTCHECK_ROBJECT actcheck_ro where actcheck_ro.actcheck_id = $1; 
begin 
   OPEN cursorCon;
   loop
   FETCH cursorCon INTO objectId;
   EXIT WHEN not FOUND; 
   result:=result || objectId ||'/'; 
   end loop;
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов акта без взаимодействия
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActisolatedRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActIsolatedRobj(act_id NUMBER)
  RETURN NVARCHAR2 IS
 
   objectId NVARCHAR2(300) :=''; 
   result NVARCHAR2(4000) := '/';    
   CURSOR cursorCon(act_id NUMBER) IS 
   select actisolated_ro.reality_object_id 
   from GJI_ACTISOLATED_ROBJECT actisolated_ro where actisolated_ro.actisolated_id = act_id; 
begin 
   OPEN cursorCon(act_id);
   loop
   FETCH cursorCon INTO objectId;
   EXIT WHEN cursorCon%NOTFOUND; 
   result:=result || objectId ||'/'; 
   end loop;
   CLOSE cursorCon; 
   return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActIsolatedRobj(bigint)
  RETURNS text AS
$BODY$ 
declare 
   objectId text :=''; 
   result text := '/';  
   cursorCon CURSOR IS 
   select actisolated_ro.reality_object_id 
   from GJI_ACTISOLATED_ROBJECT actisolated_ro where actisolated_ro.actisolated_id = $1; 
begin 
   OPEN cursorCon;
   loop
   FETCH cursorCon INTO objectId;
   EXIT WHEN not FOUND; 
   result:=result || objectId ||'/'; 
   end loop;
   CLOSE cursorCon; 
   return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает наличие у акта проверки выявленных нарушений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckHasViolation(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActCheckHasViolation(act_id NUMBER)
  RETURN NUMBER IS
  
    hasViolation NUMBER := 0; 
    result NUMBER := 20; 
    CURSOR cursorCon(act_id NUMBER) IS  
    select act_robject.have_violation 
    from gji_actcheck act 
   inner join GJI_ACTCHECK_ROBJECT act_robject on act_robject.actcheck_id=act.id
   where act.id = act_id;
 begin 
    OPEN cursorCon(act_id); 
    loop 
    FETCH cursorCon INTO hasViolation; 
    EXIT WHEN cursorCon%NOTFOUND; 
    if(hasViolation = 10) 
    then 
        result := 10; 
    end if; 
    if(result != 10 and hasViolation = 30) 
    then 
        result := 30; 
    end if;    
    end loop; 
    CLOSE cursorCon; 
    return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActCheckHasViolation(bigint)
  RETURNS integer AS
$BODY$ 
declare 
    hasViolation integer := 0; 
    result integer := 20;  
begin 
    select act_robject.have_violation into hasViolation
    from gji_actcheck act 
    inner join GJI_ACTCHECK_ROBJECT act_robject on act_robject.actcheck_id=act.id
    where act.id = $1;
       
    if(hasViolation = 10) 
    then result = 10; 
    end if; 

    if(result != 10 and hasViolation = 30) 
    then 
        result = 30; 
    end if; 

    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает наличие у акта проверки выявленных нарушений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActisolatedHasViolation(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetActIsolatedHasViolation(act_id NUMBER)
  RETURN NUMBER IS
  
    hasViolation NUMBER := 0; 
    result NUMBER := 20; 
    CURSOR cursorCon(act_id NUMBER) IS  
    select act_robject.have_violation 
    from gji_actisolated act 
   inner join GJI_ACTISOLATED_ROBJECT act_robject on act_robject.actisolated_id=act.id
   where act.id = act_id;
 begin 
    OPEN cursorCon(act_id); 
    loop 
    FETCH cursorCon INTO hasViolation; 
    EXIT WHEN cursorCon%NOTFOUND; 
    if(hasViolation = 10) 
    then 
        result := 10; 
    end if; 
    if(result != 10 and hasViolation = 30) 
    then 
        result := 30; 
    end if;    
    end loop; 
    CLOSE cursorCon; 
    return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActIsolatedHasViolation(bigint)
  RETURNS integer AS
$BODY$ 
declare 
    hasViolation integer := 0; 
    result integer := 20;  
begin 
    select act_robject.have_violation into hasViolation
    from gji_actisolated act 
    inner join GJI_ACTISOLATED_ROBJECT act_robject on act_robject.actisolated_id=act.id
    where act.id = $1;
       
    if(hasViolation = 10) 
    then result = 10; 
    end if; 

    if(result != 10 and hasViolation = 30) 
    then 
        result = 30; 
    end if; 

    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает количество жилых домов акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE FUNCTION gjiGetActCheckCntRobj(act_id NUMBER)
  RETURN NUMBER IS
    result NUMBER :=0; 
begin 
 execute immediate 'select count(actcheck_ro.id) 
   from GJI_ACTCHECK_ROBJECT actcheck_ro 
   where actcheck_ro.actcheck_id = ' || act_id into result;
return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActCheckCntRobj(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer :=0;  
begin
    select count(actcheck_ro.id) into result
    from GJI_ACTCHECK_ROBJECT actcheck_ro 
    where actcheck_ro.actcheck_id = $1; 
    return result; 
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает количество жилых домов акта без взаимодействия
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActisolatedCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE FUNCTION gjiGetActIsolatedCntRobj(act_id NUMBER)
  RETURN NUMBER IS
    result NUMBER :=0; 
begin 
 execute immediate 'select count(actisolated_ro.id) 
   from GJI_ACTISOLATED_ROBJECT actisolated_ro 
   where actisolated_ro.actisolated_id = ' || act_id into result;
return result; 
end;";
            }

            return @"
CREATE OR REPLACE FUNCTION gjiGetActIsolatedCntRobj(bigint)
    RETURNS integer AS
$BODY$ 
declare 
    result integer :=0;  
begin
    select count(actisolated_ro.id) into result
    from GJI_ACTISOLATED_ROBJECT actisolated_ro 
    where actisolated_ro.actisolated_id = $1; 
    return result; 
end;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// Возвращает строку вида /1/2/4/ идентификаторов жилых домов Обращения граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetAppealRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetAppealRobject(app_id NUMBER)
                  RETURN NVARCHAR2 IS
 
                   objectId NVARCHAR2(300) :='';
                   result NVARCHAR2(4000) := '/';
                   CURSOR cursorCon(app_id NUMBER) IS 
                   select aro.reality_object_id 
                   from GJI_APPCIT_RO aro where aro.APPCIT_ID = app_id;
                begin 
                   OPEN cursorCon(app_id);
                   loop
                   FETCH cursorCon INTO objectId;
                   EXIT WHEN cursorCon%NOTFOUND;
                   result:=result || objectId ||'/';
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end;";
            }
            return @"
CREATE OR REPLACE FUNCTION gjiGetAppealRobject(bigint)
    RETURNS text AS
$BODY$ 
declare 
    objectId text :='';
    result text := '/';
    cursorCon CURSOR IS 
    select aro.reality_object_id 
    from GJI_APPCIT_RO aro where aro.APPCIT_ID = $1; 
begin 
    OPEN cursorCon;
    loop
    FETCH cursorCon INTO objectId;
    EXIT WHEN not FOUND; 
    result:=result || objectId ||'/'; 
    end loop;
    CLOSE cursorCon; 
    return result; 
end; 
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;";
        }

        /// <summary>
        /// функция возвращает адреса жилых домов Обращения граждан
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRobjectAppealCits(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
                    CREATE OR REPLACE FUNCTION gjiGetRobjectAdrAppeal(ac_id number) RETURN nvarchar2 IS
                       ro_adr varchar2(4000) := '';
                     result varchar2(4000) :='';
                     zp varchar2(10) :=', ';
                     cnt integer:= 0;
                      CURSOR cursorCon  IS
                        select distinct ro.address
                        from gji_appcit_ro aro
                            LEFT JOIN gkh_reality_object ro ON ro.id = aro.reality_object_id
                            where aro.appcit_id = ac_id ;
                     begin 
                     OPEN cursorCon;
                    loop
                    FETCH cursorCon INTO ro_adr;
                    EXIT WHEN cursorCon%NOTFOUND;
                     cnt := cnt + 1;
                         IF (cnt>3) 
                        then 
                            EXIT; 
                        end if;
                    
                    IF (result is not null)
                    then
                    result:=result || zp;
                    end if;
                    result:=result || ro_adr;
                    end loop;
                    CLOSE cursorCon;
                    return result;
                    end;";
            }

            return @"CREATE OR REPLACE FUNCTION gjiGetRobjectAdrAppeal(bigint)
                      RETURNS text AS
                    $BODY$ declare
                     ro_adr text := '';
                     result text :='';
                     zp text:=', ';
                     cursorCon CURSOR IS
                        select distinct ro.address
                        from gji_appcit_ro aro
                            LEFT JOIN gkh_reality_object ro ON ro.id = aro.reality_object_id
                            where aro.appcit_id = $1 ;
                     begin OPEN cursorCon;
                    loop
                    FETCH cursorCon INTO ro_adr;
                    EXIT WHEN not FOUND;
                    if(result!='')
                    then
                    result:=result || zp;
                    end if;
                    result:=result || ro_adr;
                    end loop;
                    CLOSE cursorCon;
                    return result;
                    end; $BODY$
                      LANGUAGE plpgsql VOLATILE
                      COST 100;";
        }

        /// <summary>
        /// функция возвращает адреса жилых домов Проверок
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobjectAdr(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gjiGetInspRobjectAdr(insp_id number) RETURN varchar2 IS
                   mu varchar2(4000) := '';
                   zp varchar2(4000):=', ';
                   cnt integer:= 0;    
                   result varchar2(4000):= '';
                   CURSOR cursorCon(insp_id number)  IS
                           select distinct ro.address
                           from gji_inspection_robject gji_ro
                                join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                           where gji_ro.inspection_id = insp_id;
                begin
                   OPEN cursorCon(insp_id); 
                   loop
                   FETCH cursorCon INTO mu; 
                   EXIT WHEN cursorCon%NOTFOUND; 
                   
                   cnt := cnt + 1;
                     IF (cnt>3) 
                    then 
                        EXIT; 
                    end if;
                   if(result is not null)
                   then
                       result := result || zp;
                   end if;
                   result := result || mu;
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end;
                ";
            }

            return @"
                CREATE OR REPLACE FUNCTION gjiGetInspRobjectAdr(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   mu text := '';
                   zp text:=', ';
                   result text:= '';
                   cursorCon CURSOR IS
                                   select distinct ro.address
                                   from gji_inspection_robject gji_ro
                                        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                                   where gji_ro.inspection_id = $1;
                begin
                   OPEN cursorCon; 
                   loop
                   FETCH cursorCon INTO mu; 
                   EXIT WHEN not FOUND; 
                   if(result <> '')
                   then
                       result := result || zp;
                   end if;
                   result := result || mu;
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }


        /// <summary>
        /// Функция возвращает строку наименований муниципальных образований жилых домов проверки
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobjectMoName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetInspRobjectMoName(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   mu text := '';
                   zp text:=', ';
                   result text:= '';
                   cursorCon CURSOR IS
                   select distinct mu.name 
                   from gji_inspection_robject gji_ro
                        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                        left join gkh_dict_municipality mu on mu.id = ro.stl_municipality_id
                   where gji_ro.inspection_id = $1;
                begin
                   OPEN cursorCon; 
                   loop
                   FETCH cursorCon INTO mu; 
                   EXIT WHEN not FOUND; 
                   if(result <> '')
                   then
                       result := result || zp;
                   end if;
                   result := result || mu;
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";

        }

        /// <summary>
        /// Функция возвращает строку наименований населенных пунктов жилых домов проверки
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetInspectionRobjectPlaceName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetInspRobjectPlaceName(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   mu text := '';
                   zp text:=', ';
                   result text:= '';
                   cursorCon CURSOR IS
                   select distinct fa.place_name 
                   from gji_inspection_robject gji_ro
                        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                        join b4_fias_address fa ON fa.id = ro.fias_address_id
                   where gji_ro.inspection_id = $1;
                begin
                   OPEN cursorCon; 
                   loop
                   FETCH cursorCon INTO mu; 
                   EXIT WHEN not FOUND; 
                   if(result <> '')
                   then
                       result := result || zp;
                   end if;
                   result := result || mu;
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";

        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований жилых домов документа гжи
        /// gjiGetDocMoByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentRobjectMoNameByViolStage(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetDocMoByViolStage(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   result text := ''; 
                   zp text := ', ';
                   mu_name text := '';
                   cursorCon CURSOR IS 
                    select distinct mu.name
                    from gji_inspection_viol_stage viol_stage 
                        join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
                        join gkh_reality_object ro on ro.id = insp_viol.reality_object_id
                        left join gkh_dict_municipality mu on mu.id = ro.stl_municipality_id
                   where viol_stage.document_id = $1;
                 begin 
                    OPEN cursorCon;
                    loop
                    FETCH cursorCon INTO mu_name;
                    EXIT WHEN not FOUND;
                    if(result <> '')
                    then
                        result := result || zp;
                    end if;
                    result := result || mu_name;
                    end loop;
                    CLOSE cursorCon; 
                    return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований жилых домов документа гжи
        /// gjiGetDocPlaceByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentRobjectPlaceByViolStage(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetDocPlaceByViolStage(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   result text := ''; 
                   zp text := ', ';
                   mu_name text := '';
                   cursorCon CURSOR IS 
                    select distinct fa.place_name 
                    from gji_inspection_viol_stage viol_stage 
                        join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
                        join gkh_reality_object ro on ro.id = insp_viol.reality_object_id
                        join b4_fias_address fa ON fa.id = ro.fias_address_id
                   where viol_stage.document_id = $1;
                 begin 
                    OPEN cursorCon;
                    loop
                    FETCH cursorCon INTO mu_name;
                    EXIT WHEN not FOUND;
                    if(result <> '')
                    then
                        result := result || zp;
                    end if;
                    result := result || mu_name;
                    end loop;
                    CLOSE cursorCon; 
                    return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальнызх образований жилых домов Акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckRobjectMoName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetActCheckRobjMo(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                    result text := '';
                    zp text := ', ';
                    mu_name text := '';
                    cursorCon CURSOR IS 
                   select distinct mu.name
                   from GJI_ACTCHECK_ROBJECT actcheck_ro 
                    join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
                    left join gkh_dict_municipality mu on mu.id = ro.stl_municipality_id
                   where actcheck_ro.actcheck_id = $1;
                begin 
                OPEN cursorCon; 
                loop
                FETCH cursorCon INTO mu_name;
                EXIT WHEN not FOUND;
                    if(result <> '')
                    then
                        result := result || zp;
                    end if;
                    result := result || mu_name;
                end loop;
                CLOSE cursorCon; 
                return result; 
                end;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальнызх образований жилых домов Акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckRobjectPlaceName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetActCheckRobjPlace(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                    result text := '';
                    zp text := ', ';
                    mu_name text := '';
                    cursorCon CURSOR IS 
                   select distinct fa.place_name 
                   from GJI_ACTCHECK_ROBJECT actcheck_ro 
                    join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
                    join b4_fias_address fa ON fa.id = ro.fias_address_id
                   where actcheck_ro.actcheck_id = $1;
                begin 
                OPEN cursorCon; 
                loop
                FETCH cursorCon INTO mu_name;
                EXIT WHEN not FOUND;
                    if(result <> '')
                    then
                        result := result || zp;
                    end if;
                    result := result || mu_name;
                end loop;
                CLOSE cursorCon; 
                return result; 
                end;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований акт обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActsurveyRobjectMoName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetActSurveyRobjMo(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                mu_name text :=''; 
                result text :=''; 
                zp text:=', '; 
                    cursorCon CURSOR IS 
                    select distinct (mu.name)
                    from 
                GJI_ACTSURVEY_ROBJECT actsur_ro 
                    join gkh_reality_object ro on ro.id=actsur_ro.reality_object_id 
                    left join gkh_dict_municipality mu on mu.id = ro.stl_municipality_id 
                   where actsur_ro.ACTSURVEY_ID = $1;
                begin 
                   OPEN 
                cursorCon; 
                   loop  
                   FETCH cursorCon INTO mu_name; 
                    EXIT WHEN not FOUND; 
                    if(result <> '') 
                    then 
                        result:=result || zp; 
                    end if; 
                    result:=result || mu_name; 
                   end loop; 
                    CLOSE cursorCon; 
    
                return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Возвращает строку наименований муниципальных образований акт обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActsurveyRobjectPlaceName(DbmsKind dbmsKind)
        {
            return @"
            CREATE OR REPLACE FUNCTION gjiGetActSurveyRobjPlace(bigint)
              RETURNS text AS
            $BODY$ 
            declare 
            mu_name text :=''; 
            result text :=''; 
            zp text:=', '; 
                cursorCon CURSOR IS 
                select distinct (fa.place_name)
                from 
            GJI_ACTSURVEY_ROBJECT actsur_ro 
                join gkh_reality_object ro on ro.id=actsur_ro.reality_object_id 
                join b4_fias_address fa ON fa.id = ro.fias_address_id
               where actsur_ro.ACTSURVEY_ID = $1;
            begin 
               OPEN 
            cursorCon; 
               loop  
               FETCH cursorCon INTO mu_name; 
                EXIT WHEN not FOUND; 
                if(result <> '') 
                then 
                    result:=result || zp; 
                end if; 
                result:=result || mu_name; 
               end loop; 
                CLOSE cursorCon; 
    
            return result; 
            end; 
            $BODY$
              LANGUAGE plpgsql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// возвращает строку наименвоаний муниципальных образований жилых домов родительского документа
        /// gjiGetDocParentMuByViolStage
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRobjectMoNameByViolStage(DbmsKind dbmsKind)
        {
            return @"
                    CREATE OR REPLACE FUNCTION gjigetdocparentmobyviolstage(bigint)
                      RETURNS text AS
                    $BODY$ 
                    declare 
                       mu_name text :=''; 
                       zp text := ', ';
                       result text := ''; 
                       cursorCon CURSOR IS 
                       SELECT DISTINCT munt.name
                                FROM 
                                (
                            select 
                            distinct doc_child.children_id, mu.name
                            from gji_document_children doc_child
                            JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                            JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
                            JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
                            JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
                            LEFT JOIN gkh_dict_municipality mu on mu.id = ro.stl_municipality_id
                            where doc_par.TYPE_DOCUMENT != 80
                                UNION ALL
                                select 
                               distinct doc_child.children_id, mu.name
                                from gji_document_children doc_child
                                        JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                                    JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
                                    JOIN gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
                                    LEFT JOIN gkh_dict_municipality mu on mu.id = ro.stl_municipality_id
                                where doc_par.TYPE_DOCUMENT = 80
                        ) munt
                       where munt.children_id = $1;
                    begin 
                       OPEN cursorCon;
                       loop 
                       FETCH cursorCon INTO mu_name; 
                       EXIT WHEN not FOUND;
                       if(result <> '')
                       then
                          result:=result || zp; 
                       end if;
                       result := result || mu_name;
                       end loop; 
                       CLOSE cursorCon; 
                       return result; 
                    end; 
                    $BODY$
                      LANGUAGE plpgsql VOLATILE
                      COST 100;";
        }

        /// <summary>
        /// возвращает строку наименвоаний муниципальных образований жилых домов родительского документа
        /// gjiGetDocParentPlaceByViolStg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRobjectPlaceNameByViolStage(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjigetdocparentplacebyviolstg(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   mu_name text :=''; 
                   zp text := ', ';
                   result text := ''; 
                   cursorCon CURSOR IS 
                   SELECT DISTINCT munt.place_name
                            FROM 
                            (
                        select 
                        distinct doc_child.children_id, fa.place_name
                        from gji_document_children doc_child
                        JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                        JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
                        JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
                        JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
                        join b4_fias_address fa ON fa.id = ro.fias_address_id
                        where doc_par.TYPE_DOCUMENT != 80
                            UNION ALL
                            select 
                           distinct doc_child.children_id, fa.place_name
                            from gji_document_children doc_child
                                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
                                JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
                                JOIN gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
                                join b4_fias_address fa ON fa.id = ro.fias_address_id
                            where doc_par.TYPE_DOCUMENT = 80
                    ) munt
                   where munt.children_id = $1;
                begin 
                   OPEN cursorCon;
                   loop 
                   FETCH cursorCon INTO mu_name; 
                   EXIT WHEN not FOUND;
                   if(result <> '')
                   then
                      result:=result || zp; 
                   end if;
                   result := result || mu_name;
                   end loop; 
                   CLOSE cursorCon; 
                   return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        /// <summary>
        /// Функция возвращает имена источников обращения
        /// gjiGetRevenueSourceNames(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRevenueSourceNames(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetRevenueSourceNames(bigint)
                          RETURNS text AS
                        $BODY$ declare
                         src_names text := '';
                         result text :='';
                         zp text:=', ';
                         cursorCon CURSOR IS
                            select distinct s.name
                            from gji_appeal_sources asrc
                            join gji_dict_revenuesource s ON s.id = asrc.revenue_source_id
                            where asrc.appcit_id = $1 ;
                         begin OPEN cursorCon;
                        loop
                        FETCH cursorCon INTO src_names;
                        EXIT WHEN not FOUND;
                        if(result!='')
                        then
                        result:=result || zp;
                        end if;
                        result:=result || src_names;
                        end loop;
                        CLOSE cursorCon;
                        return result;
                        end; $BODY$
                          LANGUAGE plpgsql VOLATILE
                          COST 100;";
        }

        /// <summary>
        /// Функция возвращает дату источников обращения
        /// gjiGetRevenueSourceDates(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRevenueSourceDates(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetRevenueSourceDates(param_id  bigint)
                RETURNS text AS
                $func$
                BEGIN
                RETURN (select array_to_string(
                          ARRAY(
                                select to_char(REVENUE_DATE, 'DD.MM.YYYY')
                                from gji_appeal_sources asrc 
                                where asrc.REVENUE_DATE is not null 
                                and appcit_id = param_id
                                ), ',')
                       );
                END
                $func$ LANGUAGE plpgsql;";
        }

        /// <summary>
        /// Функция возвращает номера источников обращения
        /// gjiGetRevenueSourceNumbers(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRevenueSourceNumbers(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetRevenueSourceNumbers(param_id  bigint)
                RETURNS text AS
                $func$
                BEGIN
                RETURN ( 
                        select array_to_string(
                        ARRAY(select REVENUE_SOURCE_NUMBER
                        from gji_appeal_sources asrc
                        where asrc.REVENUE_SOURCE_NUMBER is not null 
                          and appcit_id = param_id), ',')
                        );
                END
                $func$ LANGUAGE plpgsql;";
        }

        /// <summary>
        /// Функция возвращает номера источников обращения
        /// gjiGetSubSubjectsName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetSubSubjectsName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetSubSubjectsName(param_id  bigint)
                RETURNS text AS
                $func$
                BEGIN
                RETURN ( 
                        select array_to_string(
                        ARRAY(select s.Name
                              from GJI_APPCIT_STATSUBJ  gas
                              join GJI_DICT_STAT_SUB_SUBJECT s ON s.id = gas.SUBSUBJECT_ID
                              where gas.appcit_id = param_id), ',')
                        );
                END
                $func$ LANGUAGE plpgsql;";
        }

        /// <summary>
        /// Функция возвращает номера источников обращения
        /// gjiGetFeaturesName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetFeaturesName(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetFeaturesName(param_id  bigint)
                RETURNS text AS
                $func$
                BEGIN
                RETURN ( 
                        select array_to_string(
                        ARRAY(select f.Name
                              from GJI_APPCIT_STATSUBJ  gas
                              join GJI_DICT_FEATUREVIOL f ON f.id = gas.FEATURE_ID
                              where gas.appcit_id = param_id), ',')
                        );
                END
                $func$ LANGUAGE plpgsql;";
        }

        /// <summary>
        /// Функция возвращает номера источников обращения
        /// gjiGetControllerFio(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetExecutantsFio(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetControllerFio(param_id  bigint)
                RETURNS text AS
                $func$
                BEGIN
                RETURN ( 
                        select array_to_string(
                        ARRAY(select i.FIO
                              from GJI_APPCIT_EXECUTANT  exe
                              join GKH_DICT_INSPECTOR i ON i.id = exe.CONTROLLER_ID
                              where exe.appcit_id = param_id), ',')
                        );
                END
                $func$ LANGUAGE plpgsql;";
        }

        #endregion Create
        #region Delete

        /// <summary>
        /// Удаление
        /// gjiGetAppealRobject
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetAppealRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetAppealRobject");
            }

            return @"drop function if exists gjiGetAppealRobject(integer)";
        }


        /// <summary>
        /// Удаление
        /// gjiGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRobjectManorg(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetRobjectManorg");
            }

            return @"drop function if exists gjiGetRobjectManorg(integer)";
        }

        /// <summary>
        /// Удаление
        /// gkhGetRobjectTypeContract
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRobjectTypeContract(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetRobjectTypeContract");
            }

            return @"drop function if exists gjiGetRobjectTypeContract(integer)";
        }

        /// <summary>
        /// gjiGetActCheckCntRobj(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckCntRobj");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckCntRobj(bigint)";
        }

        /// <summary>
        /// gjiGetActIsolatedCntRobj(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActisolatedCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActIsolatedCntRobj");
            }

            return @"DROP FUNCTION if exists gjiGetActIsolatedCntRobj(bigint)";
        }

        /// <summary>
        /// gjiGetActCheckHasViolation(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckHasViolation(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckHasViolation");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckHasViolation(bigint)";
        }

        /// <summary>
        /// gjiGetActIsolatedHasViolation(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActisolatedHasViolation(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActIsolatedHasViolation");
            }

            return @"DROP FUNCTION if exists gjiGetActIsolatedHasViolation(bigint)";
        }

        /// <summary>
        /// gjiGetActCheckRobj(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckRobj");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckRobj(bigint)";
        }

        /// <summary>
        /// gjiGetActIsolatedRobj(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActisolatedRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActIsolatedRobj");
            }

            return @"DROP FUNCTION if exists gjiGetActIsolatedRobj(bigint)";
        }

        /// <summary>
        /// gjiGetActCheckRobjectMuId(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckRobjectMuId");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckRobjectMuId(bigint)";
        }

        /// <summary>
        /// gjiGetActIsolatedRobjectMuId(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActisolatedRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActIsolatedRobjectMuId");
            }

            return @"DROP FUNCTION if exists gjiGetActIsolatedRobjectMuId(bigint)";
        }

        /// <summary>
        /// gjiGetActCheckRobjMun(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckRobjMun");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckRobjMun(bigint)";
        }

        /// <summary>
        /// gjiGetActIsolatedRobjMun(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActisolatedRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActIsolatedRobjMun");
            }

            return @"DROP FUNCTION if exists gjiGetActIsolatedRobjMun(bigint)";
        }

        /// <summary>
        /// gjiGetActSurvRobj(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActsurveyRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActSurvRobj");
            }

            return @"DROP FUNCTION if exists gjiGetActSurvRobj(bigint)";
        }

        /// <summary>
        /// gjiGetActSurvRobjAddress(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActsurveyRobjectAddress(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActSurvRobjAddress");
            }

            return @"DROP FUNCTION if exists gjiGetActSurvRobjAddress(bigint)";
        }

        /// <summary>
        /// gjiGetActSurveyRobjMunId(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActsurveyRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActSurveyRobjMunId");
            }

            return @"DROP FUNCTION if exists gjiGetActSurveyRobjMunId(bigint)";
        }

        /// <summary>
        /// gjiGetActSurveyRobjMun(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActsurveyRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActSurveyRobjMun");
            }

            return @"DROP FUNCTION if exists gjiGetActSurveyRobjMun(bigint)";
        }

        /// <summary>
        /// gjiGetBusinessActServCnt(integer)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetBusinessActServCnt(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetBusinessActServCnt");
            }

            return @"DROP FUNCTION if exists gjiGetBusinessActServCnt(integer);";
        }

        /// <summary>
        /// gjiGetDispActCheckExist(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDisposalActcheckExist(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDispActCheckExist");
            }

            return @"DROP FUNCTION if exists gjiGetDispActCheckExist(bigint);";
        }

        /// <summary>
        /// gjiGetDispCntRobj(bigint, bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDisposalCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDispCntRobj");
            }

            return @"DROP FUNCTION if exists gjiGetDispCntRobj(bigint, bigint)";
            /*this.DeleteFuncGetInspectionCountRobject(dbmsKind) 
            + this.DeleteFuncGetDocumentParentCountRobjectByViolStage(dbmsKind)*/
        }

        /// <summary>
        /// gjiGetDispRealityObj(bigint, bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDisposalRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDispRealityObj");
            }

            return @"DROP FUNCTION if exists gjiGetDispRealityObj(bigint, bigint)";
        }

        /// <summary>
        /// gjiGetDisposalTypeSurveys(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDisposalTypeSurveys(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDisposalTypeSurveys");
            }

            return @"DROP FUNCTION if exists gjiGetDisposalTypeSurveys(bigint);";
        }

        /// <summary>
        /// gjiGetDocCountChildDoc(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentCountChildDocument(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocCountChildDoc");
            }

            return @"DROP FUNCTION if exists gjiGetDocCountChildDoc(bigint)";
        }

        /// <summary>
        /// gjiGetDocCntRealObjByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentCountRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocCntRealObjByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocCntRealObjByViolStage(bigint) cascade";
        }

        /// <summary>
        /// gjiGetDocCountViolByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentCountViolationByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocCountViolByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocCountViolByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocumentInspectors(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentInspectors(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocumentInspectors");
            }

            return @"DROP FUNCTION if exists gjiGetDocumentInspectors(bigint)";
        }

        /// <summary>
        /// gjiGetDocPrntCntRobjByViolStg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFuncGetDocumentParentCountRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocPrntCntRobjByViolStg");
            }

            return @"DROP FUNCTION if exists gjiGetDocPrntCntRobjByViolStg(bigint)";
        }

        /// <summary>
        /// gjiGetDocParentRobjByViolStage(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentParentRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocParentRobjByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocParentRobjByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocParentRobjByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentParentRobjectMuIdByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocParentMuIdByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocParentMuIdByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocParentMuByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentParentRobjectMuNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocParentMuByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocParentMuByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocRobjectByViolStage(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentRobjectByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocRobjectByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocRobjectByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocMuIdByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentRobjectMuIdByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocMuIdByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocMuIdByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocMuByViolStage(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentRobjectMuNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocMuByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocMuByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetHeatingSeasonDocName(bigint, integer);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetHeatSeasonDocumentName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetHeatingSeasonDocName");
            }

            return @"DROP FUNCTION if exists gjiGetHeatingSeasonDocName(bigint, integer)";
        }

        /// <summary>
        /// gjiGetInspStatAppealsNumberGji(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspStatAppealsNumberGji(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspStatAppealsNumberGji");
            }

            return @"DROP FUNCTION if exists gjiGetInspStatAppealsNumberGji(bigint)";
        }

        /// <summary>
        /// gjiGetInspCountRobject(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFuncGetInspectionCountRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspCountRobject");
            }

            return @"DROP FUNCTION if exists gjiGetInspCountRobject(bigint)";
        }

        /// <summary>
        /// gjiGetInspDisposalNumber(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionDisposalNumber(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspDisposalNumber");
            }

            return @"DROP FUNCTION if exists gjiGetInspDisposalNumber(bigint)";
        }

        /// <summary>
        /// gjiGetInspDisposalTypeSurveys(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionDisposalTypeSurveys(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspDisposalTypeSurveys");
            }

            return @"DROP FUNCTION if exists gjiGetInspDisposalTypeSurveys(bigint)";
        }

        /// <summary>
        /// gjiGetInspectionInsp(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionInspectors(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspectionInsp");
            }

            return @"DROP FUNCTION if exists gjiGetInspectionInsp(bigint)";
        }

        /// <summary>
        /// gjiGetInspectionZonalInspections(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionZonalInspections(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspectionZonalInspections");
            }

            return @"DROP FUNCTION if exists gjiGetInspectionZonalInspections(bigint)";
        }

        /// <summary>
        /// gjiGetInspRobject(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobject");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobject(bigint)";
        }

        /// <summary>
        /// gjiGetInspRobjectAddress(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionRobjectAddress(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobjectAddress");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobjectAddress(bigint)";
        }

        /// <summary>
        /// gjiGetInspRobjectMuId(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionRobjectMuId(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobjectMuId");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobjectMuId(bigint)";
        }

        /// <summary>
        /// gjiGetInspRobjectMuName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionRobjectMuName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobjectMuName");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobjectMuName(bigint)";
        }

        /// <summary>
        /// gjiGetRobjectAdrAppeal(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRobjectAdrAppeal(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetRobjectAdrAppeal");
            }

            return @"DROP FUNCTION if exists gjiGetRobjectAdrAppeal(bigint)";
        }


        /// <summary>
        /// gjiGetInspRobjectAdr(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspRobjectAdr(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobjectAdr");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobjectAdr(bigint)";
        }


        /// <summary>
        /// gjiGetResolProsRobjectAddress(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetResolProsRobjectAddress(DbmsKind dbmsKind)
        {
            if(dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetResolProsRobjectAddress");
            }

            return @"DROP FUNCTION if exists gjiGetResolProsRobjectAddress(bigint)";
        }

        /// <summary>
        /// gjiGetResolProsRobject(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetResolProsRobject(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetResolProsRobject");
            }

            return @"DROP FUNCTION if exists gjiGetResolProsRobject(bigint)";
        }
        
        /// <summary>
        /// gjiGetDocParRoAdrByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocParentRoAdrByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocParRoAdrByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocParRoAdrByViolStage(bigint)";
        }


        /// <summary>
        /// gjiGetInspRobjectMuName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionRobjectMoName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobjectMoName");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobjectMoName(bigint)";
        }

        /// <summary>
        /// gjiGetInspRobjectPlaceName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetInspectionRobjectPlaceName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetInspRobjectPlaceName");
            }

            return @"DROP FUNCTION if exists gjiGetInspRobjectPlaceName(bigint)";
        }

        /// <summary>
        /// gjiGetDocMuByViolStage(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentRobjectMoNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocMoByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocMoByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocPlaceByViolStage(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentRobjectPlaceNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocPlaceByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocPlaceByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetActCheckRobjMo(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckRobjectMoName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckRobjMo");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckRobjMo(bigint)";
        }

        /// <summary>
        /// gjiGetActCheckRobjPlace(bigint);
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckRobjectPlaceName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckRobjPlace");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckRobjPlace(bigint)";
        }

        /// <summary>
        /// gjiGetActSurveyRobjMo(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActsurveyRobjectMoName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActSurveyRobjMo");
            }

            return @"DROP FUNCTION if exists gjiGetActSurveyRobjMo(bigint)";
        }

        /// <summary>
        /// gjiGetActSurveyRobjPlace(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActsurveyRobjectPlaceName(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActSurveyRobjPlace");
            }

            return @"DROP FUNCTION if exists gjiGetActSurveyRobjPlace(bigint)";
        }

        /// <summary>
        /// gjiGetDocParentMoByViolStage(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentParentRobjectMoNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocParentMoByViolStage");
            }

            return @"DROP FUNCTION if exists gjiGetDocParentMoByViolStage(bigint)";
        }

        /// <summary>
        /// gjiGetDocParentPlaceByViolStg(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentParentRobjectPlaceNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetDocParentPlaceByViolStg");
            }

            return @"DROP FUNCTION if exists gjiGetDocParentPlaceByViolStg(bigint)";
        }

        /// <summary>
        /// gjiGetRevenueSourceNames(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRevenueSourceNames(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetRevenueSourceNames(bigint)";
        }

        /// <summary>
        /// gjiGetRevenueSourceDates(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRevenueSourceDates(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetRevenueSourceDates(bigint)";
        }

        /// <summary>
        /// gjiGetRevenueSourceNumbers(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRevenueSourceNumbers(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetRevenueSourceNumbers(bigint)";
        }


        /// <summary>
        /// gjiGetSubSubjectsName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteGjiGetSubSubjectsName(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetSubSubjectsName(bigint)";
        }

        /// <summary>
        /// gjiGetFeaturesName(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteGjiGetFeaturesName(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetFeaturesName(bigint)";
        }

        /// <summary>
        /// gjiGetControllerFio(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteGjiGetControllerFio(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetControllerFio(bigint)";
        }


        #endregion Delete
        #endregion Функции
        #region Вьюхи
        #region Create

        /// <summary>
        /// Вьюха актов устранения нарушений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActRemoval(DbmsKind dbmsKind)
        {
           return @"
CREATE OR REPLACE VIEW view_gji_act_removal AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    docparent.id AS parent_id, 
    docparent.type_document AS parent_type,
    parent.ctr_mu_name,
    parent.ctr_mu_id,
    parent.contragent_name, 
    (('№' || docparent.document_number) || ' ') || to_char(docparent.document_date, 'DD.MM.YYYY') AS parent_name, 
    gjiGetDocCountChildDoc(doc.id) AS count_children, 
    gjiGetDocCntRealObjByViolStage(doc.id) AS count_robject, 
    gjiGetDocumentInspectors(doc.id) AS inspector_names, 
    gjiGetDocRobjectByViolStage(doc.id) AS ro_ids, 
    ''::text AS ro_addresses,
    gjiGetDocMuByViolStage(doc.id) AS mu_names,
    ''::text AS mo_names,
    ''::text AS place_names, 
    gjiGetDocMuIdByViolStage(doc.id) AS mu_id, 
    insp.id AS inspection_id, 
    insp.type_base, 
    act.type_removal, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_actremoval act ON act.id = doc.id
    JOIN gji_document_children ch ON ch.children_id = doc.id
    JOIN gji_document docparent ON docparent.type_document <> 20 AND docparent.id = ch.parent_id
    JOIN (
        SELECT 
            presc.id,
            mu.name as ctr_mu_name,
            c.municipality_id as ctr_mu_id,
            c.name AS contragent_name
        FROM gji_prescription presc
            LEFT JOIN gkh_contragent c ON c.id = presc.contragent_id
            LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
        UNION 
        SELECT 
            prot.id,
            mu.name as ctr_mu_name,
            c.municipality_id as ctr_mu_id,
            c.name AS contragent_name
        FROM gji_protocol prot
            LEFT JOIN gkh_contragent c ON c.id = prot.contragent_id
            LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
        ) parent ON parent.id = docparent.id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id";
        }

        /// <summary>
        /// Вьюха актов проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActcheck(DbmsKind dbmsKind)
        {
            return $@"
				CREATE OR REPLACE VIEW view_gji_actcheck AS 
				SELECT doc.id,
				    doc.state_id,
				    doc.id AS document_id,
				    doc.document_date,
				    doc.document_number,
				    doc.document_num,
				    STRING_AGG(inspector.fio, ', '::text) AS inspector_names,
				    COUNT(DISTINCT ro.id) AS count_ro,
				    COALESCE(NULLIF(MIN(aro.have_violation), {(int)YesNoNotSet.NotSet}), {(int)YesNo.No}) AS has_violation,
				    COUNT(DISTINCT child_doc.id) AS count_exec_doc,
				    STRING_AGG(DISTINCT ro.id::text, '/'::text) AS ro_ids,
				    STRING_AGG(DISTINCT ro.address, ', '::text) AS ro_addresses,
				    STRING_AGG(DISTINCT mun.name, ', '::text) AS mu_names,
				    MIN(mun.id) AS mu_id,
				    insp.id AS inspection_id,
				    insp.type_base,
				    mu.name AS ctr_mu_name,
				    c.municipality_id AS ctr_mu_id,
				    c.name AS contragent_name,
				    doc.type_document AS type_doc
				   FROM gji_document doc
				   JOIN gji_inspection insp ON insp.id = doc.inspection_id
				   LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
				   LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
				   LEFT JOIN
				   (
				    gji_document_inspector insp_link
				      	 JOIN gkh_dict_inspector inspector ON inspector.id = insp_link.inspector_id 
				   ) ON insp_link.document_id = doc.id
				   LEFT JOIN 
				   (
						gji_actcheck_robject aro
						JOIN gkh_reality_object ro ON aro.reality_object_id = ro.id
						JOIN gkh_dict_municipality mun ON mun.id = ro.municipality_id
				   ) ON aro.actcheck_id = doc.id
				   LEFT JOIN
				   (
						gji_document_children child
						JOIN gji_document child_doc ON child_doc.id = child.children_id
				    	     AND child_doc.type_document != {(int)TypeDocumentGji.ActRemoval}
				   ) ON child.parent_id = doc.id
				   WHERE doc.type_document = {(int)TypeDocumentGji.ActCheck}
				   GROUP BY doc.id, insp.id, mu.id, c.id";;
        }

        /// <summary>
        /// Вьюха деятельности тсж
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActivityTsj(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE VIEW view_gji_activity_tsj as
SELECT 
    act_tsj.id,     --0
    mu.id as mu_id, --1
    mu.name AS mu_name,     --2
    c.id as contragent_id,  --3
    c.name AS contragent_name,  --4
    c.inn,  --5
    (SELECT 
        CASE WHEN count(s.id) > 0 THEN 1 ELSE 0 end
    FROM gji_act_tsj_statute s 
    WHERE s.activity_tsj_id = act_tsj.id) AS has_statute    --6
FROM gji_activity_tsj act_tsj
   JOIN gkh_managing_organization mo ON mo.id = act_tsj.managing_org_id
   JOIN gkh_contragent c ON c.id = mo.contragent_id
   LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id";
            }

            return @"
CREATE OR REPLACE VIEW view_gji_activity_tsj AS 
SELECT 
    act_tsj.id, --0
    mu.id as mu_id, --1
    mu.name AS mu_name,     --2
    c.id as contragent_id,  --3
    c.name AS contragent_name,  --4
    c.inn,  --5
    (SELECT 
        count(s.id) > 0
    FROM gji_act_tsj_statute s
    WHERE s.activity_tsj_id = act_tsj.id) AS has_statute    --6
FROM gji_activity_tsj act_tsj
   JOIN gkh_managing_organization mo ON mo.id = act_tsj.managing_org_id
   JOIN gkh_contragent c ON c.id = mo.contragent_id
   LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id";
        }

        /// <summary>
        /// вьюха актов обследования
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActsurvey(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_actsurvey AS 
SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    gjiGetDocumentInspectors(act.id) AS inspector_names, 
    gjiGetActSurvRobjAddress(act.id) AS ro_address, 
    gjiGetActSurvRobj(act.id) AS ro_ids, 
    gjiGetActSurveyRobjMun(act.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetActSurveyRobjMunId(act.id) AS mu_id, 
    insp.id AS inspection_id, 
    insp.type_base,
    act.fact_surveyed, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_actsurvey act ON act.id = doc.id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id";
        }


        /// <summary>
        /// Вьюха актов без взаимодействия
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActisolated(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_actisolated AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    gjiGetDocumentInspectors(doc.id) AS inspector_names, 
    gjiGetActIsolatedCntRobj(doc.id) AS count_ro, 
    gjiGetActIsolatedHasViolation(doc.id) AS has_violation, 
    gjiGetDocCountChildDoc(doc.id) AS count_exec_doc, 
    gjiGetActIsolatedRobj(doc.id) AS ro_ids, 
    gjiGetActIsolatedRobjMun(doc.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetActIsolatedRobjectMuId(doc.id) AS mu_id, 
    insp.id AS inspection_id,
    insp.type_base,
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name, 
    doc.type_document AS type_doc
FROM gji_document doc
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
    LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
WHERE doc.type_document = 21";
        }

        /// <summary>
        /// Вьюха обращений граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewAppealCits(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE VIEW view_gji_appeal_cits AS 
SELECT 
    gac.id, 
    gac.DOCUMENT_NUMBER, 
    gac.gji_number, 
    gac.GJI_NUM_SORT, 
    gac.date_from, 
    gac.check_time, 
    gac.questions_count, 
    countro.count_ro, 
    mun.municipality, 
    ''::varchar(300) as mo_settlement,
    ''::varchar(50) as place_name,
    c.name AS  contragent_name, 
    gac.state_id,
    gac.executant_id, 
    gac.tester_id, 
    gac.SURETY_RESOLVE_ID,
    gac.EXECUTE_DATE,
    gac.SURETY_DATE,
    gjiGetAppealRobject(gac.id) AS ro_ids,
    gac.ZONAINSP_ID,
    gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
    gac.correspondent,
    gac.correspondent_address,
    gac.EXTENS_TIME,
    mun.municipality_id,
    '' as executant_names,
    '' as subjects
FROM gji_appeal_citizens gac
    LEFT JOIN ( 
        SELECT 
            count(garo.reality_object_id) AS count_ro, 
            gac1.id AS gac_id
        FROM gji_appeal_citizens gac1
            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
            GROUP BY gac1.id
        ) countro ON countro.gac_id = gac.id
    LEFT JOIN ( 
        SELECT 
            gaac.id AS gac_id, 
            ( 
                SELECT 
                    gdm.name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id AND rownum =1
            ) AS municipality,
            ( 
                SELECT 
                    gdm.id AS municipality_id
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id AND rownum =1
            ) AS municipality_id
        FROM gji_appeal_citizens gaac
        ) mun ON mun.gac_id = gac.id
    LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
    LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id";
            }

            return @"
                CREATE OR REPLACE VIEW view_gji_appeal_cits AS 
                SELECT 
                    gac.id, 
                    gac.document_number, 
                    gac.gji_number,
                    gac.gji_num_sort, 
                    gac.date_from, 
                    gac.check_time, 
                    gac.questions_count, 
                    countro.count_ro, 
                    mun.municipality, 
                    ''::varchar(300) as mo_settlement,
                    ''::varchar(50) as place_name,
                    c.name AS contragent_name, 
                    gac.state_id,
                    gac.executant_id, 
                    gac.tester_id, 
                    gac.surety_resolve_id,
                    gac.execute_date,
                    gac.surety_date,
                    gjiGetAppealRobject(gac.id) AS ro_ids,
                    gac.zonainsp_id,
                    gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
                    gac.correspondent,
                    gac.correspondent_address,
                    mun.municipality_id,
                    gac.extens_time,
                    gjiGetRevenueSourceNames(gac.id) as source_names,
                    gjiGetRevenueSourceDates(gac.id::bigint) AS source_dates,
                    gjiGetRevenueSourceNumbers(gac.id::bigint) AS source_numbers,
                    gjiGetSubSubjectsName(gac.id::bigint) AS SubSubjects_name,
                    gjiGetFeaturesName(gac.id::bigint) AS Features_name,
                    gjiGetControllerFio(gac.id::bigint) AS controller_fio,
                    gac.file_id,
                    gac.gji_dict_kind_id,
                    gac.previous_appeal_citizens_id,
                    gac.surety_id,
                    gac.description
                FROM gji_appeal_citizens gac
                    LEFT JOIN ( 
                        SELECT 
                            count(garo.reality_object_id) AS count_ro, 
                            gac1.id AS gac_id
                        FROM gji_appeal_citizens gac1
                            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                            GROUP BY gac1.id
                        ) countro ON countro.gac_id = gac.id
                    LEFT JOIN ( 
                        SELECT 
                            gaac.id AS gac_id, 
                            ( 
                                SELECT 
                                    gdm.name AS municipality
                                FROM gji_appeal_citizens gac1
                                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                                    WHERE gac1.id = gaac.id
                                LIMIT 1
                            ) AS municipality,
                            ( 
                                SELECT 
                                    gdm.id AS municipality_id
                                FROM gji_appeal_citizens gac1
                                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                                    WHERE gac1.id = gaac.id
                                LIMIT 1
                            ) AS municipality_id
                        FROM gji_appeal_citizens gaac
                        ) mun ON mun.gac_id = gac.id
                    LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
                    LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id";
        }

        /// <summary>
        /// Вьюха уведомлений о начале предпринимательской деятельности
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewBuisnesNotif(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_buisnes_notif AS 
 SELECT 
    busin_notif.id, 
    busin_notif.type_kind_activity, 
    busin_notif.incoming_num, 
    busin_notif.date_registration, 
    busin_notif.date_notification, 
    busin_notif.reg_num, 
    busin_notif.is_original, 
    state.id AS state_id, 
    file_inf.id AS file_id, 
    gjiGetBusinessActServCnt(busin_notif.id) AS serv_count, 
    contr.ogrn, 
    contr.inn, 
    contr.mailing_address, 
    org_form.name AS org_form_name, 
    contr.name AS contr_name, 
    municipality.name AS municipality_name, 
    busin_notif.registered, 
    COALESCE(municipality.id, 0) AS municipality_id,
    contr.id as contr_id
FROM gji_buisnes_notif busin_notif
    LEFT JOIN gkh_contragent contr ON busin_notif.contragent_id = contr.id
    LEFT JOIN b4_state state ON busin_notif.state_id = state.id
    LEFT JOIN b4_file_info file_inf ON busin_notif.file_id = file_inf.id
    LEFT JOIN gkh_dict_org_form org_form ON contr.org_legal_form_id = org_form.id
    LEFT JOIN gkh_dict_municipality municipality ON contr.municipality_id = municipality.id";
        }

        /// <summary>
        /// Вьюха распоряжений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewDisposal(DbmsKind dbmsKind)
        {
            return $@"
                CREATE OR REPLACE VIEW public.view_gji_disposal
				 AS
				 SELECT doc.id,
				    doc.state_id,
				    doc.id AS document_id,
				    doc.document_num,
				    doc.document_number,
				    doc.document_date,
				    insp.id AS inspection_id,
				    insp.type_base,
				    mu.name AS ctr_mu_name,
				    c.municipality_id AS ctr_mu_id,
				    c.name AS contragent,
				    disp.date_start,
				    disp.date_end,
				    kind_check.name AS kind_check_name,
				    gjigetdisposaltypesurveys(doc.id::bigint) AS tsurveys_name,
				    gjigetdispcntrobj(doc.id::bigint, doc.inspection_id) AS ro_count,
				    gjigetdispactcheckexist(doc.id::bigint) AS act_check_exist,
				    gjigetdocumentinspectors(doc.id::bigint) AS inspector_names,
				    gjigetdisprealityobj(doc.id::bigint, doc.inspection_id) AS ro_ids,
				    ''::text AS ro_addresses,
				    gjigetinsprobjectmuname(doc.inspection_id) AS mu_names,
				    ''::text AS mo_names,
				    ''::text AS place_names,
				    gjigetinsprobjectmuid(doc.inspection_id) AS mu_id,
				    doc.type_document AS type_doc,
				    disp.type_disposal,
				    disp.type_agrprosecutor,
				    insp.control_type,
				    ml.id AS license,
				    child_doc.id IS NOT NULL AS has_act_survey
				   FROM gji_document doc
				     JOIN gji_disposal disp ON disp.id = doc.id
				     JOIN gji_inspection insp ON doc.inspection_id = insp.id
				     JOIN b4_state state ON doc.state_id = state.id
				     LEFT JOIN gji_inspection_lic_app ila ON doc.inspection_id = ila.id
				     LEFT JOIN gkh_manorg_lic_request mlr ON ila.man_org_lic_id = mlr.id
				     LEFT JOIN gkh_contragent c ON
				        CASE
				            WHEN insp.type_base = {(int)TypeBase.LicenseApplicants} THEN mlr.contragent_id
				            ELSE insp.contragent_id
				        END = c.id
				     LEFT JOIN gkh_manorg_license ml ON ml.contragent_id = c.id
				     LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
				     LEFT JOIN gji_dict_kind_check kind_check ON kind_check.id = disp.kind_check_id
				     LEFT JOIN (gji_document_children child
				     JOIN gji_document child_doc ON child_doc.id = child.children_id AND child_doc.type_document = {(int)TypeDocumentGji.ActSurvey}) ON child.parent_id = doc.id
					 WHERE disp.type_disposal <> {(int)TypeDisposalGji.NullInspection} AND doc.type_document <> {(int)TypeDocumentGji.TaskDisposal}";
        }

        /// <summary>
        /// Вьюха подготовки к отопительному сезону
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewHeatingSeason(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_heating_season AS 
SELECT 
    ro.id, 
    hs.id AS heat_season_id, 
    hs.date_heat, 
    per.id AS period_id, 
    per.id AS heat_seas_period_id, 
    per.name AS period_name, 
    ro.type_house,  
    ro.number_entrances, 
    ro.floors, 
    ro.maximum_floors,
    ro.area_mkd, 
    ro.address, 
    ro.condition_house,
    ro.RESIDENTS_EVICTED,
    mu.name AS municipality_name, 
    mu.id AS mu_id, 
    gjiGetRobjectManorg(ro.id) AS manorg_name, 
    gjiGetRobjectTypeContract(ro.id) AS type_contract_name, 
    gjiGetHeatingSeasonDocName(hs.id, 10) AS act_flushing, 
    gjiGetHeatingSeasonDocName(hs.id, 20) AS act_pressing, 
    gjiGetHeatingSeasonDocName(hs.id, 30) AS act_ventilation, 
    gjiGetHeatingSeasonDocName(hs.id, 40) AS act_chimney, 
    gjiGetHeatingSeasonDocName(hs.id, 50) AS passport,
    case when hs.id is not null then hs.heating_system else ro.heating_system end as heating_system,
    ro.date_commissioning
FROM gkh_reality_object ro
    CROSS JOIN gji_dict_heatseasonperiod per
    LEFT JOIN gji_heatseason hs ON hs.reality_object_id = ro.id AND hs.heatseason_period_id = per.id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
";
        }

        /// <summary>
        /// Вьюха документов подготовки к отопительному сезону
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewHeatseasonDoc(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_heatseason_doc AS 
 SELECT 
    doc.id, 
    doc.type_document, 
    doc.state_id, 
    hs.heating_system, 
    ro.address, 
    ro.type_house, 
    mu.name AS municipality_name, 
    gjiGetRobjectManorg(ro.id) AS manorg_name, 
    hs.heatseason_period_id,
    ro.condition_house
FROM gji_heatseason_document doc
    JOIN gji_heatseason hs ON hs.id = doc.heatseason_id
    JOIN gkh_reality_object ro ON ro.id = hs.reality_object_id
    JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id";
        }

        /// <summary>
        /// Вьюха оснований проверки без основания
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsBasedef(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_ins_basedef AS 
SELECT 
    insp.id, 
    insp.inspection_num, 
    insp.physical_person, 
    c.name AS contragent_name, 
    state.id as state_id,
    gjiGetInspRobjectMuName(insp.id) AS mu_names,
    ''::text AS mo_names,
    ''::text AS place_names, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_basedef t ON t.id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
        }

        /// <summary>
        /// вьюха оснований проверки по поручению руководства
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsDisphead(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_ins_disphead AS 
SELECT 
    t.id, 
    t.disphead_date, 
    c.name AS contragent_name, 
    t.head_id, 
    head.fio AS head_fio,
    t.document_number, 
    insp.inspection_number, 
    insp.person_inspection, 
    insp.type_jur_person,
    state.id as state_id,
    gjiGetInspDisposalTypeSurveys(insp.id) AS disp_types, 
    gjiGetInspectionInsp(insp.id) AS inspectors, 
    gjiGetInspectionZonalInspections(insp.id) AS zonal_inspections, 
    gjiGetInspRobjectMuName(insp.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    (select count(insp_ro.reality_object_id) 
            from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_disphead t ON t.id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN gkh_dict_inspector head ON head.id = t.head_id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
        }

        /// <summary>
        /// Вьюха оснований инспекционной проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsInschek(DbmsKind dbmsKind)
        {
           return @"
CREATE OR REPLACE VIEW view_gji_ins_inschek AS 
 SELECT 
    insp.id, 
    plan.id AS plan_id, 
    plan.name AS plan_name, 
    t.inscheck_date, 
    c.name AS contragent_name, 
    insp.inspection_number, 
    t.type_fact, 
    state.id as state_id,
    gjiGetInspDisposalNumber(insp.id) AS disp_number, 
    gjiGetInspectionInsp(insp.id) AS inspectors, 
    gjiGetInspectionZonalInspections(insp.id) AS zonal_inspections, 
    gjiGetInspRobject(insp.id) AS ro_ids, 
    gjiGetInspRobjectAddress(insp.id) AS ro_address, 
    gjiGetInspRobjectMuName(insp.id) AS mu_names,
    ''::text AS mo_names,
    ''::text AS place_names, 
    (select count(insp_ro.reality_object_id) 
            from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_inscheck t ON t.id = insp.id
    LEFT JOIN gji_dict_planinscheck plan ON t.plan_id = plan.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
        }

        /// <summary>
        /// Вьюха оснований плановой проверки юр лиц
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsJurpers(DbmsKind dbmsKind)
        {

            return @"
CREATE OR REPLACE VIEW view_gji_ins_jurpers AS 
 SELECT 
    insp.id, 
    insp.id AS inspection_id, 
    plan.id AS plan_id, 
    plan.name AS plan_name, 
    c.name AS contragent_name, 
    t.type_fact, 
    t.count_days, 
    t.date_start, 
    insp.inspection_number, 
    state.id as state_id,
    gjiGetInspDisposalNumber(insp.id) AS disp_number, 
    gjiGetInspectionInsp(insp.id) AS inspectors, 
    gjiGetInspectionZonalInspections(insp.id) AS zonal_inspections, 
    gjiGetInspRobject(insp.id) AS ro_ids, 
    gjiGetInspRobjectAddress(insp.id) AS ro_address, 
    gjiGetInspRobjectMuName(insp.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    (select count(insp_ro.reality_object_id) 
            from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_jurperson t ON t.id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN gji_dict_planjurperson plan ON t.plan_id = plan.id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
        }

        /// <summary>
        /// вьюха оснований проверки по требованию прокуратуры
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsProsclaim(DbmsKind dbmsKind)
        {
          return @"
                    CREATE OR REPLACE VIEW view_gji_ins_prosclaim AS 
                     SELECT 
                        insp.id, 
                        c.name AS contragent_name, 
                        t.prosclaim_date_check, 
                        t.document_number, 
                        insp.person_inspection,
                        insp.inspection_number, 
                        insp.type_jur_person,
                        state.id as state_id,
                        gjiGetInspectionInsp(insp.id) AS inspectors, 
                        gjiGetInspectionZonalInspections(insp.id) AS zonal_inspections, 
                        gjiGetInspRobjectMuName(insp.id) AS mu_names,
                        ''::text AS mo_names,
                        ''::text AS place_names,
                        (select count(insp_ro.reality_object_id) 
                             from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
                        ( select distinct mu.id 
                             from gji_inspection_robject gji_ro
                             join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                             left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                             where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
                    FROM gji_inspection insp
                        JOIN gji_inspection_prosclaim t ON t.id = insp.id
                        LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
                        LEFT JOIN b4_state state ON insp.state_id = state.id";
        }

        /// <summary>
        /// Вьюха оснований проверки по обращению граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsStatement(DbmsKind dbmsKind) =>
            $@"
            CREATE OR REPLACE VIEW public.view_gji_ins_statement AS
			SELECT insp.id,
			       ctr.name                                              contragent_name,
			       insp.person_inspection,
			       insp.inspection_number,
			       insp.type_jur_person,
			       CASE t.request_type
				       WHEN {(int)BaseStatementRequestType.MotivationConclusion} THEN gjidoc.document_number
				       ELSE appeal.document_number
				       END                                               document_number,
			       ARRAY_TO_STRING(ARRAY_AGG(DISTINCT mu.name), '; ')    mu_names,
			       NULL::text                                            mo_names,
			       NULL::text                                            place_names,
			       ARRAY_TO_STRING(ARRAY_AGG(DISTINCT ro.address), '; ') ro_adr,
			       t.request_type,
			       state.id                                              state_id,
			       docs.inspection_id IS NOT NULL                        is_disposal,
			       (ARRAY_AGG(mu.id))[1]                                 mu_id,
			       COUNT(DISTINCT ro.address)                            ro_count
			FROM gji_inspection insp
				JOIN gji_inspection_statement t ON t.id = insp.id
				LEFT JOIN gkh_contragent ctr ON ctr.id = insp.contragent_id
				LEFT JOIN b4_state state ON insp.state_id = state.id
				LEFT JOIN (SELECT doc.inspection_id
				           FROM gji_disposal disp
								JOIN gji_document doc ON doc.id = disp.id
				           WHERE disp.type_disposal = {(int)TypeDisposalGji.Base}
				           GROUP BY doc.inspection_id) docs ON docs.inspection_id = insp.id
				LEFT JOIN (gji_basestat_appcit base
                    JOIN gji_appeal_citizens appeal ON appeal.id = base.gji_appcit_id)
                    ON insp.id = base.inspection_id AND t.request_type = {(int)BaseStatementRequestType.AppealCits}
                LEFT JOIN (gji_basestat_document basedoc
                    JOIN gji_document gjidoc ON gjidoc.id = basedoc.document_id)
                    ON insp.id = basedoc.inspection_id AND t.request_type = {(int)BaseStatementRequestType.MotivationConclusion}
				LEFT JOIN (gji_inspection_robject gji_ro
	                JOIN gkh_reality_object ro ON ro.id = gji_ro.reality_object_id
	                JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id)
                    ON insp.id = gji_ro.inspection_id
			GROUP BY insp.id, ctr.id, t.id, gjidoc.id, appeal.id, state.id, docs.inspection_id";

        /// <summary>
        /// Вьюха оснований проверки по cоискателям лицензии
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewInsLicApplicants(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"
CREATE OR REPLACE VIEW view_gji_ins_license_app AS 
SELECT 
    insp.id, 
    ctr.name AS contragent_name, 
    insp.person_inspection, 
    insp.inspection_number, 
    insp.type_jur_person,
    gjigetinsprobjectmuname(insp.id) AS mu_names,
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetInspRobjectAdr(insp.id) as ro_adr,
    state.id as state_id,
    ( select distinct mu.id 
         from gji_inspection_robject gji_ro
         join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
         left join gkh_dict_municipality mu on mu.id = ro.municipality_id
         where gji_ro.inspection_id = insp.id AND ROWNUM = 1 ) AS mu_id,
    (CASE WHEN DOCS.COUNT_DOC > 0 THEN 1 ELSE 0 end) AS is_disposal,
     req.reg_number,
     ctr.id as ctr_id
FROM gji_inspection insp
    JOIN GJI_INSPECTION_LIC_APP t ON t.id = insp.id
    LEFT JOIN gkh_manorg_lic_request req ON req.id = t.man_org_lic_id
    LEFT JOIN gkh_contragent ctr ON ctr.id = req.contragent_id
    LEFT JOIN b4_state state ON insp.state_id = state.id
    LEFT JOIN ( 
        select 
        doc.inspection_id, 
        COUNT(doc.Id) COUNT_DOC
               from gji_disposal disp
               inner join gji_document doc on doc.id = disp.id
               where disp.type_disposal = 10 
               GROUP BY doc.inspection_id
    
    ) DOCS ON DOCS.inspection_id = insp.ID";
            }

            return @"
CREATE OR REPLACE VIEW view_gji_ins_license_app AS 
SELECT 
    insp.id, 
    ctr.name AS contragent_name, 
    insp.person_inspection, 
    insp.inspection_number, 
    insp.type_jur_person,
    gjigetinsprobjectmuname(insp.id) AS mu_names,
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetInspRobjectAdr(insp.id) as ro_adr,
    state.id as state_id,
    ( select distinct mu.id 
        from gji_inspection_robject gji_ro
        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
        where gji_ro.inspection_id = insp.id limit 1) AS mu_id,
    (CASE WHEN DOCS.COUNT_DOC > 0 THEN true ELSE FALSE end) AS is_disposal,
    req.reg_number,
    ctr.id as ctr_id
FROM gji_inspection insp
    JOIN GJI_INSPECTION_LIC_APP t ON t.id = insp.id
    LEFT JOIN gkh_manorg_lic_request req ON req.id = t.man_org_lic_id
    LEFT JOIN gkh_contragent ctr ON ctr.id = req.contragent_id
    LEFT JOIN b4_state state ON insp.state_id = state.id
    LEFT JOIN
    (
        select 
        doc.inspection_id, 
        COUNT(doc.Id)COUNT_DOC
               from gji_disposal disp
               inner join gji_document doc on doc.id = disp.id
               where disp.type_disposal = 10 
               GROUP BY doc.inspection_id
    
    ) DOCS ON DOCS.inspection_id = insp.ID";
        }

        /// <summary>
        /// Вьюха предписаний
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewPrescription(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FORCE VIEW VIEW_GJI_PRESCRIPTION
(
   ID,
   STATE_ID,
   DOCUMENT_ID,
   DOCUMENT_DATE,
   DOCUMENT_NUM,
   DOCUMENT_NUMBER,
   COUNT_RO,
   COUNT_VIOL,
   INSPECTOR_NAMES,
   RO_IDS,
   MU_NAMES,
   MO_NAMES,
   PLACE_NAMES,
   MU_ID,
   INSPECTION_ID,
   TYPE_BASE,
   CTR_MU_NAME,
   CTR_MU_ID,
   CONTRAGENT_NAME,
   TYPE_EXEC_NAME,
   TYPE_DOC,
   DATE_REMOVAL,
   DISP_ID
)
AS
SELECT doc.id,
       doc.state_id,
       doc.id AS document_id,
       doc.document_date,
       doc.document_num,
       doc.document_number,
       count_r.count_ro AS count_ro,
       count_v.count_viol AS count_viol,
       gjiGetDocumentInspectors (pr.id) AS inspector_names,
       gjiGetDocRobjectByViolStage (pr.id) AS ro_ids,
       ''::text AS ro_addresses,
       gjiGetDocMuByViolStage (pr.id) AS mu_names,
       ''::text AS mo_names,
       ''::text AS place_names,
       gjiGetDocMuIdByViolStage (pr.id) AS mu_id,
       insp.id AS inspection_id,
       insp.type_base,
       mu.name as ctr_mu_name,
       c.municipality_id as ctr_mu_id,
       c.name AS contragent_name,
       exec.name AS type_exec_name,
       doc.type_document AS type_doc,
       v.date_removal,
       cnt.COUNT_disp AS disp_id
  FROM gji_document doc
       JOIN gji_prescription pr ON pr.id = doc.id
       LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
       LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
       LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
       LEFT JOIN gji_dict_executant exec ON exec.id = pr.executant_id
       LEFT JOIN
       (  SELECT MAX (viol.DATE_PLAN_REMOVAL) AS date_removal, viol.document_id
            FROM gji_inspection_viol_stage viol
        GROUP BY viol.document_id) v
          ON v.document_id = pr.id
       LEFT JOIN
       (  SELECT prnt_chldr.PARENT_ID, COUNT (DISTINCT disp.id) COUNT_disp
            FROM GJI_DOCUMENT_CHILDREN prnt_chldr
                 JOIN
                 GJI_DOCUMENT disp
                    ON     disp.TYPE_DOCUMENT = 10
                       AND disp.id = prnt_chldr.CHILDREN_ID
        GROUP BY prnt_chldr.PARENT_ID) cnt
          ON cnt.PARENT_ID = doc.ID
       LEFT JOIN
       (  SELECT viol_stage.document_id,
                 COUNT (DISTINCT viol_stage.inspection_viol_id) count_viol
            FROM gji_inspection_viol_stage viol_stage
        GROUP BY viol_stage.document_id) count_v
          ON count_v.document_id = pr.id
       LEFT JOIN
       (  SELECT viol_stage.document_id,
                 COUNT (DISTINCT insp_viol.reality_object_id) count_ro
            FROM gji_inspection_viol_stage viol_stage
                 JOIN gji_inspection_violation insp_viol
                    ON insp_viol.id = viol_stage.inspection_viol_id
        GROUP BY viol_stage.document_id) count_r
          ON count_r.document_id = pr.id";
            }


            return @"
CREATE OR REPLACE VIEW view_gji_prescription AS 
  SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_num, 
    doc.document_number, 
    gjiGetDocCntRealObjByViolStage(pr.id) AS count_ro, 
    gjiGetDocCountViolByViolStage(pr.id) AS count_viol, 
    gjiGetDocumentInspectors(pr.id) AS inspector_names, 
    gjiGetDocRobjectByViolStage(pr.id) AS ro_ids, 
    ''::text AS ro_addresses,
    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetDocMuIdByViolStage(pr.id) AS mu_id, 
    insp.id AS inspection_id, 
    insp.type_base,
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name, 
    exec.name AS type_exec_name, 
    doc.type_document AS type_doc, 
    v.date_removal,
    (select count(distinct disp.id) 
     from GJI_DOCUMENT_CHILDREN prnt_chldr
     join GJI_DOCUMENT disp on disp.TYPE_DOCUMENT = 10 and disp.id = prnt_chldr.CHILDREN_ID
     where prnt_chldr.PARENT_ID = doc.ID
    )  disp_id
FROM gji_document doc
    JOIN gji_prescription pr ON pr.id = doc.id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
    LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
    LEFT JOIN gji_dict_executant exec ON exec.id = pr.executant_id
    LEFT JOIN ( SELECT 
            max(viol.DATE_PLAN_REMOVAL) AS date_removal, 
            viol.document_id
        FROM gji_inspection_viol_stage viol
        GROUP BY viol.document_id) v ON v.document_id = pr.id";
        }

        /// <summary>
        /// Вьюха представлений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewPresentation(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_presentation AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    gjiGetInspRobject(doc.inspection_id) AS ro_ids, 
    pr.physical_person, 
    pr.type_initiative_org, 
    c.name AS contragent_name, 
    insp.type_base, 
    insp.id AS inspection_id, 
    mu.name AS municipality_name, 
    mo.name AS mo_names, 
    fa.place_name AS place_names, 
    mu.id AS mu_id, 
    pr.official_id, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_presentation pr ON pr.id = doc.id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
    LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
    LEFT JOIN gkh_dict_municipality mo ON mo.id = c.mosettlement_id
    LEFT JOIN b4_fias_address fa ON fa.id = c.fias_jur_address_id";
        }

        /// <summary>
        /// Вьюха протоколов
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewProtocol(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FORCE VIEW VIEW_GJI_PROTOCOL
(
   ID,
   STATE_ID,
   DOCUMENT_ID,
   DOCUMENT_DATE,
   DOCUMENT_NUMBER,
   DOCUMENT_NUM,
   INSPECTOR_NAMES,
   COUNT_RO,
   COUNT_VIOL,
   RO_IDS,
   MU_NAMES,
   MO_NAMES,
   PLACE_NAMES,
   MU_ID,
   CTR_MU_NAME,
   CTR_MU_ID,
   CONTRAGENT_NAME,
   TYPE_EXEC_NAME,
   INSPECTION_ID,
   TYPE_BASE,
   TYPE_DOC
)
AS
    SELECT doc.id,
          doc.state_id,
          doc.id AS document_id,
          doc.document_date,
          doc.document_number,
          doc.document_num,
          gjiGetDocumentInspectors (pr.id) AS inspector_names,
          count_r.count_ro AS count_ro,
          count_v.count_viol AS count_viol,
          gjiGetDocRobjectByViolStage (pr.id) AS ro_ids,
          ''::text AS ro_addresses,
          gjiGetDocMuByViolStage (pr.id) AS mu_names,
          ''::text AS mo_names,
          ''::text AS place_names,
          gjiGetDocMuIdByViolStage (pr.id) AS mu_id,
          mo.name AS mo_names, 
          fa.place_name AS place_names, 
          mu.name as ctr_mu_name,
          c.municipality_id as ctr_mu_id,
          c.name AS contragent_name,
          exec.name AS type_exec_name,
          insp.id AS inspection_id,
          insp.type_base,
          doc.type_document AS type_doc
     FROM gji_document doc
          JOIN gji_protocol pr ON pr.id = doc.id
          LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
          LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
          LEFT JOIN gji_dict_executant exec ON exec.id = pr.executant_id
          LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
          LEFT JOIN 
          (
          
          select viol_stage.document_id,count(distinct viol_stage.inspection_viol_id) as count_viol
                                from gji_inspection_viol_stage viol_stage 
                                group by viol_stage.document_id
          ) count_v ON COUNT_V.DOCUMENT_ID = pr.id
          LEFT JOIN 
          (
          select viol_stage.document_id, count(distinct insp_viol.reality_object_id) as count_ro
                                from gji_inspection_viol_stage viol_stage 
                                join gji_inspection_violation insp_viol on insp_viol.id=viol_stage.inspection_viol_id 
                               group by viol_stage.document_id
          
          ) count_r ON count_r.document_id = pr.id";
            }


            return @"
CREATE OR REPLACE VIEW view_gji_protocol AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    gjiGetDocumentInspectors(pr.id) AS inspector_names, 
    gjiGetDocCntRealObjByViolStage(pr.id) AS count_ro, 
    gjiGetDocCountViolByViolStage(pr.id) AS count_viol, 
    gjiGetDocRobjectByViolStage(pr.id) AS ro_ids, 
    ''::text AS ro_addresses,
    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetDocMuIdByViolStage(pr.id) AS mu_id, 
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name, 
    exec.name AS type_exec_name, 
    insp.id AS inspection_id, 
    insp.type_base, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_protocol pr ON pr.id = doc.id
    LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
    LEFT JOIN gji_dict_executant exec ON exec.id = pr.executant_id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id";
        }

        /// <summary>
        /// Вьюха постановлений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewResolution(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_resolution AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    payfine.sum_pays, 
    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobject(doc.id) ELSE gjiGetDocParentRobjByViolStage(doc.id) end) AS ro_ids, 
    ''::text AS protocol_ro_addresses,
    gjiGetDocParentMuByViolStage(doc.id) AS mu_names, 
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetDocParentMuIdByViolStage(doc.id) AS mu_id, 
    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobjectAddress(doc.id) ELSE gjiGetDocParRoAdrByViolStage(doc.id) end) AS ro_address, 
    inspector.fio AS official_name, 
    res.official_id, 
    res.penalty_amount, 
    insp.id AS inspection_id, 
    insp.type_base, 
    exec.name AS type_exec_name, 
    s.name AS sanction_name,
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name, 
    doc.type_document AS type_doc,
    res.DELIVERY_DATE,
    res.PAIDED,
    res.BECAME_LEGAL,
    res.TYPE_INITIATIVE_ORG
FROM gji_document doc
    JOIN gji_resolution res ON res.id = doc.id
    LEFT JOIN ( 
        SELECT 
            gji_resolution_payfine.resolution_id, 
            sum(gji_resolution_payfine.amount) AS sum_pays
        FROM gji_resolution_payfine
        GROUP BY gji_resolution_payfine.resolution_id
        ) payfine ON payfine.resolution_id = doc.id
    LEFT JOIN gkh_dict_inspector inspector ON inspector.id = res.official_id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
    LEFT JOIN gkh_contragent c ON c.id = res.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
    LEFT JOIN gji_dict_executant exec ON exec.id = res.executant_id
    LEFT JOIN gji_dict_sanction s ON s.id = res.sanction_id";
        }

        /// <summary>
        /// View постановлений Роспотребнадзора
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewResolutionRospotrebnadzor(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_resolution_rospotrebnadzor AS
 SELECT
    doc.id,
    doc.state_id,
    doc.id AS document_id,
    doc.document_date,
    doc.document_number,
    doc.document_num,
    payfine.sum_pays,
    gjiGetDocParentRobjByViolStage(doc.id) AS ro_ids,
    gjiGetDocParentMuByViolStage(doc.id) AS mu_names,
    ''::text AS mo_names,
    ''::text AS place_names,
    gjiGetDocParentMuIdByViolStage(doc.id) AS mu_id,
    gjiGetDocParRoAdrByViolStage(doc.id) AS ro_address,
    inspector.fio AS official_name,
    res.official_id,
    res.penalty_amount,
    insp.id AS inspection_id,
    insp.type_base,
    exec.name AS type_exec_name,
    s.name AS sanction_name,
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name,
    doc.type_document AS type_doc,
    res.DELIVERY_DATE,
    res.PAIDED
FROM gji_document doc
    JOIN gji_resolution_rospotrebnadzor res ON res.id = doc.id
    LEFT JOIN (
        SELECT
            gji_resolution_rospotrebnadzor_payfine.resolution_id,
            sum(gji_resolution_rospotrebnadzor_payfine.amount) AS sum_pays
        FROM gji_resolution_rospotrebnadzor_payfine
        GROUP BY gji_resolution_rospotrebnadzor_payfine.resolution_id
        ) payfine ON payfine.resolution_id = doc.id
    LEFT JOIN gkh_dict_inspector inspector ON inspector.id = res.official_id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
    LEFT JOIN gkh_contragent c ON c.id = res.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
    LEFT JOIN gji_dict_executant exec ON exec.id = res.executant_id
    LEFT JOIN gji_dict_sanction s ON s.id = res.sanction_id";
        }

        /// <summary>
        /// Вьюха предписаний виджетов
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewPrescriptionWidget(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_prescription_widget AS 
 SELECT prescr.id, doc.document_number, doc.document_date, 
    contr.name AS contragent_name, oper_insp.operator_id, v.date_removal
   FROM gji_prescription prescr
   LEFT JOIN ( SELECT max(iv.date_fact_removal) AS date_removal, 
            viol.document_id
           FROM gji_inspection_viol_stage viol
      JOIN gji_inspection_violation iv ON iv.id = viol.inspection_viol_id
     GROUP BY viol.document_id) v ON v.document_id = prescr.id
   JOIN gji_document doc ON doc.id = prescr.id
   JOIN gji_inspection insp ON insp.id = doc.inspection_id
   LEFT JOIN gji_document_children pc ON pc.parent_id = prescr.id
   JOIN gji_actremoval va ON va.id = pc.children_id
   JOIN gji_document_inspector doc_insp ON doc_insp.document_id = prescr.id
   JOIN gkh_operator_inspect oper_insp ON doc_insp.inspector_id = oper_insp.inspector_id
   LEFT JOIN gkh_contragent contr ON contr.id = prescr.contragent_id
  WHERE va.id IS NULL OR va.id IS NOT NULL AND va.type_removal = 30
  ORDER BY v.date_removal";
        }

        /// <summary>
        /// Вьюха распоряжений виджетов
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewDisposalWidget(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_gji_disposal_widget AS 
 SELECT disp.id, disp.date_start, disp.date_end, doc.document_number, 
    insp.type_base, oper_insp.operator_id
   FROM gji_disposal disp
   JOIN gji_document doc ON doc.id = disp.id
   JOIN gji_inspection insp ON insp.id = doc.inspection_id
   LEFT JOIN gji_document_children pc ON pc.parent_id = disp.id
   JOIN gji_actcheck va ON va.id = pc.children_id
   JOIN gji_document_inspector doc_insp ON doc_insp.document_id = disp.id
   JOIN gkh_operator_inspect oper_insp ON doc_insp.inspector_id = oper_insp.inspector_id
  WHERE (va.id IS NULL OR va.id IS NOT NULL AND gjigetactcheckhasviolation(va.id) = 30) AND disp.type_disposal = 10
  ORDER BY disp.date_start";
        }

        private string CreateViewFormatDataExportInspection(DbmsKind dbmsKind)
        {
            return @"DROP VIEW IF EXISTS view_format_data_export_inspection;
CREATE OR REPLACE VIEW view_format_data_export_inspection AS
SELECT DISTINCT ON (insp.id)
   insp.id,
   insp.object_version,
   insp.object_create_date,
   insp.object_edit_date,
   doc.parent_id AS disposal_id,
   doc.children_id AS actcheck_id,
   insp.type_base = 30 AS is_planned,
   insp.type_base,
   disp.document_number,
   disp.document_date,
   insp.check_date,
   c.id AS contragent_id,
   c.name AS contragent_name,
   mun.name AS municipality_name
FROM gji_document_children doc
JOIN gji_document disp ON disp.id = doc.parent_id
JOIN gji_inspection insp ON insp.id = disp.inspection_id
LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
JOIN gji_document act ON act.id = doc.children_id
JOIN (gji_actcheck_robject actro
      LEFT JOIN gkh_reality_object ro ON actro.reality_object_id = ro.id
      LEFT JOIN gkh_dict_municipality mun ON ro.municipality_id = mun.id) ON actro.actcheck_id = doc.children_id
WHERE disp.type_document = 10
  AND act.type_document = 20
  AND insp.type_base IN (20, 30, 40, 50)
ORDER BY insp.id, doc.parent_id;";
        }

        #endregion Create
        #region Delete

        private string DeleteViewActRemoval(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_act_removal";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewActcheck(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_actcheck";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewActivityTsj(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_activity_tsj";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewActsurvey(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_actsurvey";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewActisolated(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_actisolated";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewAppealCits(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_appeal_cits";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewBuisnesNotif(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_buisnes_notif";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewDisposal(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_disposal";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewHeatingSeason(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_heating_season";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewHeatseasonDoc(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_heatseason_doc";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewInsBasedef(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_basedef";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewInsDisphead(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_disphead";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewInsInschek(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_inschek";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewInsJurpers(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_jurpers";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewInsProsclaim(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_prosclaim";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewInsStatement(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_statement";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewLicenseApp(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_ins_license_app";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewPrescription(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_prescription";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewPresentation(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_presentation";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewProtocol(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_protocol";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewResolution(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_resolution";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewResolutionRospotrebnadzor(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_resolution_rospotrebnadzor";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }

        private string DeleteViewPrescriptionWidget(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_prescription_widget";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewDisposalWidget(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_disposal_widget";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewFormatDataExportInspection(DbmsKind dbmsKind)
        {
            var viewName = "view_format_data_export_inspection";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        #endregion Delete
        #endregion Вьюхи
    }
}