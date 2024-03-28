namespace Bars.Gkh.Overhaul.Regions.Kamchatka.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.FIAS;
    using B4.Utils;

    // Интерфейс для того чтобы можно было получить по всем записям фиас IDinamicAddress
    public interface ICustomFiasRepository
    {
        Dictionary<string, IDinamicAddress> GetAllDinamicAddress();
    }
    
    public class CustomFiasRepository : FiasRepository, ICustomFiasRepository
    {
        private string SqlDinamicAddressAllObject
        {
            get
            {
                /*
                 Данный запрос строит динамический адрес для каждой записи фиас
                */
                return
                    @"select t.aoguid, t.postalcode, t.offname t_name, t.shortname t_sname, t.coderecord t_code, t.aolevel t_aolevel,
                    p1.offname p1_name, p1.shortname p1_sname, p1.aoguid p1_aoguid, p1.aolevel p1_aolevel,
                    p2.offname p2_name, p2.shortname p2_sname, p2.aoguid p2_aoguid, p2.aolevel p2_aolevel,
                    p3.offname p3_name, p3.shortname p3_sname, p3.aoguid p3_aoguid, p3.aolevel p3_aolevel,
                    p4.offname p4_name, p4.shortname p4_sname, p4.aoguid p4_aoguid, p4.aolevel p4_aolevel,
                    p5.offname p5_name, p5.shortname p5_sname, p5.aoguid p5_aoguid, p5.aolevel p5_aolevel,
                    t.mirror_guid
                    from b4_fias t
                    left join b4_fias p1 on p1.aoguid = t.parentguid and p1.actstatus = 1
                    left join b4_fias p2 on p2.aoguid = p1.parentguid and p2.actstatus = 1
                    left join b4_fias p3 on p3.aoguid = p2.parentguid and p3.actstatus = 1
                    left join b4_fias p4 on p4.aoguid = p3.parentguid and p4.actstatus = 1
                    left join b4_fias p5 on p5.aoguid = p4.parentguid and p5.actstatus = 1
                    where t.actstatus = 1 ";
            }
        }

        private string SqlDinamicAddress
        {
            get
            {
                /*
                 Данный запрос получает актуальную запись по aoguid
                */
                return SqlDinamicAddressAllObject +
                    "and t.aoguid = :filter";
            }
        }


        private string getName(string name, string socr, string level)
        {
            if (level == "1" || level == "3")
            {
                return string.Format("{0} {1}", name, socr);
            }
            else
            {
                return string.Format("{0}. {1}", socr, name);
            }
        }

        /// <summary>
        /// Метод получения адреса
        /// </summary>
        /// <param name="filter">AOGuid запрашиваемой записи</param>
        /// <returns>IDinamicAddress</returns>
        public override IDinamicAddress GetDinamicAddress(string filter)
        {
            IDinamicAddress result = null;
            if (filter == null)
                return result;

            var query = OpenSession().CreateSQLQuery(SqlDinamicAddress);
            query.SetParameter("filter", string.Format("{0}", filter));
            var list = query.List();

            foreach (object[] paramRow in list)
            {
                result = this.SetDinamicAddress(paramRow);
            }

            return result;
        }


        /// <summary>
        /// Метод получения словаря всех Адресов по Guid
        /// </summary>
        public Dictionary<string, IDinamicAddress> GetAllDinamicAddress()
        {
            var result = new Dictionary<string, IDinamicAddress>();

            var query = OpenSession().CreateSQLQuery(this.SqlDinamicAddressAllObject);
            var list = query.List();

            foreach (object[] paramRow in list)
            {
                var address = this.SetDinamicAddress(paramRow);

                if (!result.ContainsKey(address.GuidId))
                {
                    result.Add(address.GuidId, address);
                }
            }

            return result;
        }

        private string SqlPlacesDinamicAddress
        {
            get
            {
                /*
                 Данный запрос получает все записи которые возможны для выбора населенного пункта
                */
                return
                @"select t.aoguid, t.postalcode, t.offname t_name, t.shortname t_sname, t.coderecord t_code, t.aolevel t_aolevel,
                p1.offname p1_name, p1.shortname p1_sname, p1.aoguid p1_aoguid, p1.aolevel p1_aolevel,
                p2.offname p2_name, p2.shortname p2_sname, p2.aoguid p2_aoguid, p2.aolevel p2_aolevel,
                p3.offname p3_name, p3.shortname p3_sname, p3.aoguid p3_aoguid, p3.aolevel p3_aolevel,
                p4.offname p4_name, p4.shortname p4_sname, p4.aoguid p4_aoguid, p4.aolevel p4_aolevel,
                p5.offname p5_name, p5.shortname p5_sname, p5.aoguid p5_aoguid, p5.aolevel p5_aolevel,
                t.mirror_guid
                from b4_fias t
                left join (
                  --Если запись целенаправлено сделали не активной и вместо нее ввели зеркальную запись
                  --у которой есть mirror_guid то необходимо вместо настоящего родителя показывать именно зеркальных родителей
                  --пока сделал тольк для первого уровня если будет потребность в других уровнях надо остальные p2,p3,p4,p5 обработать также
                  select distinct (case when f2.id is null then f1.id else f2.id end) id,
                         (case when f2.id is null then f1.offname else f2.offname end) offname,
	                 (case when f2.id is null then f1.shortname else f2.shortname end) shortname,
	                 (case when f2.id is null then f1.aoguid else f2.aoguid end) aoguid,
	                 (case when f2.id is null then f1.aolevel else f2.aolevel end) aolevel,
	                 (case when f2.id is null then f1.parentguid else f2.parentguid end) parentguid,
	                 (case when f2.id is null then f1.actstatus else f2.actstatus end) actstatus,
	                 (case when f2.id is null then f1.mirror_guid else f2.mirror_guid end) mirror_guid
                  from b4_fias f1
                  left join b4_fias f2 on f2.mirror_guid = f1.aoguid and f2.aolevel != 7
				and f2.mirror_guid != f2.parentguid 
  
                ) p1 on (p1.aoguid = t.parentguid or (p1.mirror_guid <> null and p1.mirror_guid != '' and 
                                         p1.mirror_guid = t.parentguid and p1.parentguid != t.parentguid)) and p1.id != t.id

                left join b4_fias p2 on p2.aoguid = p1.parentguid
                left join b4_fias p3 on p3.aoguid = p2.parentguid
                left join b4_fias p4 on p4.aoguid = p3.parentguid
                left join b4_fias p5 on p5.aoguid = p4.parentguid
                where t.actstatus = 1 and t.aolevel != 7 and lower(t.offname) like :filter                     
                     and ( p1.id is null or (p1.id is not null and p1.actstatus = 1))
                     and ( p2.id is null or (p2.id is not null and p2.actstatus = 1))
                     and ( p3.id is null or (p3.id is not null and p3.actstatus = 1))
                     and ( p4.id is null or (p4.id is not null and p4.actstatus = 1))
                     and ( p5.id is null or (p5.id is not null and p5.actstatus = 1)) ";
            }
        }

        private string SqlStreetsDinamicAddress
        {
            get
            {
                /*
                 Данный запрос получает только улицы по переданному гуиду родителя и строке фильтра
                */
                return
                    @"  select t.aoguid, t.postalcode, t.offname t_name, t.shortname t_sname, t.coderecord t_code, t.aolevel t_level, t.mirror_guid t_mirror
                        from b4_fias t
                        where t.actstatus = 1 and t.aolevel = 7 and (lower(t.offname||' '||t.shortname) like :filter or lower(t.shortname||' '||t.offname) like :filter)
                        and t.parentguid = :parentguid";
            }
        }

        /// <summary>
        /// Метод получения населенных пунктов
        /// </summary>
        /// <param name="filter">Строка фильтрации</param>
        /// <param name="parentGuid">AOGuid родительской записи</param>
        /// <returns>IDinamicAddress</returns>
        public override List<IDinamicAddress> GetPlacesDinamicAddress(string filter, string parentGuid = null)
        {
            var result = new List<IDinamicAddress>();
            if (filter == null)
                return result;

            var sql = SqlPlacesDinamicAddress + (string.IsNullOrEmpty(parentGuid)
                ? string.Empty
                : " and (t.parentguid = :parentguid or p1.parentguid = :parentguid or p2.parentguid = :parentguid or p3.parentguid = :parentguid or p4.parentguid = :parentguid or p5.parentguid = :parentguid) ");
            var query = OpenSession().CreateSQLQuery(sql);
            query.SetParameter("filter", string.Format("%{0}%", filter.ToLower()));
            if (!string.IsNullOrEmpty(parentGuid))
            {
                query.SetParameter("parentguid", parentGuid);
            }

            var list = query.List();

            foreach (object[] paramRow in list)
            {
                var socr = string.Empty;
                if (!string.IsNullOrEmpty(paramRow[3].ToStr()))
                    socr = paramRow[3].ToStr();

                var dinamicAddress = new DinamicAddress
                {
                    GuidId = paramRow[0].ToStr(),
                    PostCode = paramRow[1].ToStr(),
                    Name = getName(paramRow[2].ToStr(), socr, paramRow[5].ToStr()),
                    Code = paramRow[4].ToStr(),
                    Level = (FiasLevelEnum)paramRow[5].ToInt(),
                    MirrorGuid = paramRow[26].ToStr()
                };

                dinamicAddress.AddressName = dinamicAddress.Name;
                dinamicAddress.AddressGuid = paramRow[5].ToStr() + "_" + dinamicAddress.GuidId;

                if (!string.IsNullOrEmpty(paramRow[6].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[7].ToStr()))
                        socr = paramRow[7].ToStr();

                    dinamicAddress.ParentName = getName(paramRow[6].ToStr(), socr, paramRow[9].ToStr());
                    dinamicAddress.ParentGuidId = paramRow[8].ToStr();
                    dinamicAddress.ParentLevel = (FiasLevelEnum)paramRow[9].ToInt();

                    dinamicAddress.AddressName = getName(paramRow[6].ToStr(), socr, paramRow[9].ToStr()) + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[9].ToStr() + "_" + paramRow[8].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[10].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[11].ToStr()))
                        socr = paramRow[11].ToStr();

                    dinamicAddress.AddressName = getName(paramRow[10].ToStr(), socr, paramRow[13].ToStr()) + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[13].ToStr() + "_" + paramRow[12].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[14].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[15].ToStr()))
                        socr = paramRow[15].ToStr();

                    dinamicAddress.AddressName = getName(paramRow[14].ToStr(), socr, paramRow[17].ToStr()) + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[17].ToStr() + "_" + paramRow[16].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[18].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[19].ToStr()))
                        socr = paramRow[19].ToStr();

                    dinamicAddress.AddressName = getName(paramRow[18].ToStr(), socr, paramRow[21].ToStr()) + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[21].ToStr() + "_" + paramRow[20].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[22].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[23].ToStr()))
                        socr = paramRow[23].ToStr();

                    dinamicAddress.AddressName = getName(paramRow[22].ToStr(), socr, paramRow[25].ToStr()) + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[25].ToStr() + "_" + paramRow[24].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                result.Add(dinamicAddress);
            }

            return result;
        }

        public IDinamicAddress SetDinamicAddress(object[] paramRow)
        {
            var socr = string.Empty;
            if (!string.IsNullOrEmpty(paramRow[3].ToStr()))
                socr = paramRow[3].ToStr();

            var dinamicAddress = new DinamicAddress
            {
                GuidId = paramRow[0].ToStr(),
                PostCode = paramRow[1].ToStr(),
                Name = getName(paramRow[2].ToStr(), socr, paramRow[5].ToStr()),
                Code = paramRow[4].ToStr(),
                Level = (FiasLevelEnum)paramRow[5].ToInt()
            };

            dinamicAddress.MirrorGuid = null;

            var mirrorGuid = Guid.Empty;
            if (!string.IsNullOrEmpty(paramRow[26].ToStr()) && Guid.TryParse(paramRow[26].ToStr(), out mirrorGuid))
            {
                dinamicAddress.MirrorGuid = paramRow[26].ToStr();
            }

            dinamicAddress.AddressName = dinamicAddress.Name;
            dinamicAddress.AddressGuid = paramRow[5].ToStr() + "_" + dinamicAddress.GuidId;

            if (!string.IsNullOrEmpty(paramRow[6].ToStr()))
            {
                socr = string.Empty;
                if (!string.IsNullOrEmpty(paramRow[7].ToStr()))
                {
                    socr = paramRow[7].ToStr();
                }

                dinamicAddress.ParentName = getName(paramRow[6].ToStr(), socr, paramRow[9].ToStr());
                dinamicAddress.ParentGuidId = paramRow[8].ToStr();
                dinamicAddress.ParentLevel = (FiasLevelEnum)paramRow[9].ToInt();

                dinamicAddress.AddressName = getName(paramRow[6].ToStr(), socr, paramRow[9].ToStr()) + ", " + dinamicAddress.AddressName;
                dinamicAddress.AddressGuid = paramRow[9].ToStr() + "_" + paramRow[8].ToStr() + "#" + dinamicAddress.AddressGuid;
            }

            if (!string.IsNullOrEmpty(paramRow[10].ToStr()))
            {
                socr = string.Empty;
                if (!string.IsNullOrEmpty(paramRow[11].ToStr()))
                    socr = paramRow[11].ToStr();

                dinamicAddress.AddressName = getName(paramRow[10].ToStr(), socr, paramRow[13].ToStr()) + ", " + dinamicAddress.AddressName;
                dinamicAddress.AddressGuid = paramRow[13].ToStr() + "_" + paramRow[12].ToStr() + "#" + dinamicAddress.AddressGuid;
            }

            if (!string.IsNullOrEmpty(paramRow[14].ToStr()))
            {
                socr = string.Empty;
                if (!string.IsNullOrEmpty(paramRow[15].ToStr()))
                    socr = paramRow[15].ToStr();

                dinamicAddress.AddressName = getName(paramRow[14].ToStr(), socr, paramRow[17].ToStr()) + ", " + dinamicAddress.AddressName;
                dinamicAddress.AddressGuid = paramRow[17].ToStr() + "_" + paramRow[16].ToStr() + "#" + dinamicAddress.AddressGuid;
            }

            if (!string.IsNullOrEmpty(paramRow[18].ToStr()))
            {
                socr = string.Empty;
                if (!string.IsNullOrEmpty(paramRow[19].ToStr()))
                    socr = paramRow[19].ToStr();

                dinamicAddress.AddressName = getName(paramRow[18].ToStr(), socr, paramRow[21].ToStr()) + ", " + dinamicAddress.AddressName;
                dinamicAddress.AddressGuid = paramRow[21].ToStr() + "_" + paramRow[20].ToStr() + "#" + dinamicAddress.AddressGuid;
            }

            if (!string.IsNullOrEmpty(paramRow[22].ToStr()))
            {
                socr = string.Empty;
                if (!string.IsNullOrEmpty(paramRow[23].ToStr()))
                    socr = paramRow[23].ToStr();

                dinamicAddress.AddressName = getName(paramRow[22].ToStr(), socr, paramRow[25].ToStr()) + ", " + dinamicAddress.AddressName;
                dinamicAddress.AddressGuid = paramRow[25].ToStr() + "_" + paramRow[24].ToStr() + "#" + dinamicAddress.AddressGuid;
            }

            return dinamicAddress;
        }

        /// <summary>
        /// Метод получения улиц
        /// </summary>
        /// <param name="filter">Строка фильтрации</param>
        /// <param name="parentguid">AOGuid родительской записи</param>
        /// <returns>IDinamicAddress</returns>
        public override List<IDinamicAddress> GetStreetsDinamicAddress(string filter, string parentguid)
        {
            var result = new List<IDinamicAddress>();
            if (filter == null || parentguid == null)
                return result;

            var parentFias = Container.Resolve<IDomainService<Fias>>()
                        .GetAll()
                        .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.AOGuid == parentguid)
                        .FirstOrDefault();

            if (parentFias != null && !string.IsNullOrEmpty(parentFias.MirrorGuid))
                parentguid = parentFias.MirrorGuid;

            var query = OpenSession().CreateSQLQuery(SqlStreetsDinamicAddress);
            query.SetParameter("filter", string.Format("%{0}%", filter.ToLower().Replace(".", "")));
            query.SetParameter("parentguid", string.Format("{0}", parentguid));
            var list = query.List();

            foreach (object[] paramRow in list)
            {
                var socr = (!string.IsNullOrEmpty(paramRow[3].ToStr()) ? paramRow[3].ToStr() + ". " : string.Empty);
                var dinamicAddress = new DinamicAddress
                {
                    GuidId = paramRow[0].ToStr(),
                    PostCode = paramRow[1].ToStr(),
                    Name = socr + paramRow[2].ToStr(),
                    Code = paramRow[4].ToStr(),
                    Level = (FiasLevelEnum)paramRow[5].ToInt(),
                    MirrorGuid = paramRow[6].ToStr()
                };

                dinamicAddress.AddressName = dinamicAddress.Name;
                dinamicAddress.AddressGuid = paramRow[5].ToStr() + "_" + dinamicAddress.GuidId;

                result.Add(dinamicAddress);
            }

            return result;
        }
    }
}
