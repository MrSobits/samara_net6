﻿using System.Collections.Generic;


namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Modules.FIAS;

    // TODO: Расскоментировать
    /*using Bars.B4.Modules.FIAS.Enums;
 using Bars.B4.Modules.FIAS.Helpers;*/

    /// <summary>
    /// Для дополнительных действий с адресами
    /// Например для получения IdimnamicAdress по всему фиас
    /// </summary>
    public interface IGkhCustomFiasRepository
    {
        /// <summary>
        /// Получение <see cref="IDinamicAddress"/> по всему ФИАС
        /// </summary>
        Dictionary<string, IDinamicAddress> GetAllDinamicAddress();
    }

    /// <summary>
    /// Добавляем к ФИАС дополнительные методы работы с адресами
    /// </summary>
    public class GkhCustomFiasRepository : FiasRepository, IGkhCustomFiasRepository
    {
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
                      select distinct f1.id,
                                 f1.offname,
                                 f1.shortname,
                                 f1.aoguid,
                                 f1.aolevel,
                                 f1.parentguid,
                                 f1.actstatus,
                                 f1.mirror_guid
                              from b4_fias f1
                              join b4_fias f2 on f2.mirror_guid = f1.aoguid and f2.aolevel != 7 and f2.mirror_guid != f2.parentguid
                    union
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
                    ) p1 on (case when (p1.mirror_guid is null or p1.mirror_guid = '')
                                  then p1.aoguid = t.parentguid
                                  else p1.mirror_guid = t.parentguid end )
                            and p1.parentguid != t.parentguid  and p1.id != t.id

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

        private string SqlDinamicAddressAllObject
        {
            get
            {
                /*
                 Данный запрос получает актуальную запись по aoguid
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
where t.actstatus = 1";
            }
        }

        /// <summary>
        /// Данный запрос получает только улицы по переданному гуиду родителя и строке фильтра
        /// </summary>
        private string SqlStreetsDinamicAddress =>
            @"  select t.aoguid, t.postalcode, t.offname t_name, t.shortname t_sname, t.coderecord t_code, t.aolevel t_level, t.mirror_guid t_mirror
                        from b4_fias t
                        where t.actstatus = 1 and (t.aolevel = 7 or t.aolevel = 65 or t.aolevel = 90) and (lower(t.offname||' '||t.shortname) like :filter or lower(t.shortname||' '||t.offname) like :filter)
                        and t.parentguid = :parentguid";

        /// <summary>
        /// Данный запрос получает номер дома, корпуса, строения с большей датой окончания действия записи по переданному гуиду родителя и строке фильтра
        /// </summary>
        private string SqlHousesDynamicAddress => @"
    select
        t.ao_guid,
        t.house_guid,
        t.postal_code,
        t.house_num,
        t.build_num,
        t.struc_num,
        t.end_date,
        t.structure_type,
        t.est_status
    from b4_fias_house t
    where (t.house_num like :filter
            or t.build_num like :filter
            or t.struc_num like :filter)
        and t.ao_guid = :parentguid
        and t.end_date = (
            select max(tt.end_date)
            from b4_fias_house tt
            where tt.house_guid = t.house_guid
            group by tt.house_guid
        );";

        private string SqlHousesDynamicAddressWithEstimate => @"
    select
        t.ao_guid,
        t.house_guid,
        t.postal_code,
        t.house_num,
        t.build_num,
        t.struc_num,
        t.end_date,
        t.structure_type,
        t.est_status
    from b4_fias_house t
    where (t.house_num like :filter
            or t.build_num like :filter
            or t.struc_num like :filter)
        and t.ao_guid = :parentguid
        and t.est_status = :est_status
        and t.end_date = (
            select max(tt.end_date)
            from b4_fias_house tt
            where tt.house_guid = t.house_guid
            group by tt.house_guid
        );";

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
                    socr = paramRow[3].ToStr() + ". ";

                var dinamicAddress = new DinamicAddress
                {
                    GuidId = paramRow[0].ToStr(),
                    PostCode = paramRow[1].ToStr(),
                    Name = socr + paramRow[2].ToStr(),
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
                        socr = paramRow[7].ToStr() + ". ";

                    dinamicAddress.ParentName = socr + paramRow[6].ToStr();
                    dinamicAddress.ParentGuidId = paramRow[8].ToStr();
                    dinamicAddress.ParentLevel = (FiasLevelEnum)paramRow[9].ToInt();

                    dinamicAddress.AddressName = socr + paramRow[6].ToStr() + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[9].ToStr() + "_" + paramRow[8].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[10].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[11].ToStr()))
                        socr = paramRow[11].ToStr() + ". ";

                    dinamicAddress.AddressName = socr + paramRow[10].ToStr() + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[13].ToStr() + "_" + paramRow[12].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[14].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[15].ToStr()))
                        socr = paramRow[15].ToStr() + ". ";

                    dinamicAddress.AddressName = socr + paramRow[14].ToStr() + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[17].ToStr() + "_" + paramRow[16].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[18].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[19].ToStr()))
                        socr = paramRow[19].ToStr() + ". ";

                    dinamicAddress.AddressName = socr + paramRow[18].ToStr() + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[21].ToStr() + "_" + paramRow[20].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                if (!string.IsNullOrEmpty(paramRow[22].ToStr()))
                {
                    socr = string.Empty;
                    if (!string.IsNullOrEmpty(paramRow[23].ToStr()))
                        socr = paramRow[23].ToStr() + ". ";

                    dinamicAddress.AddressName = socr + paramRow[22].ToStr() + ", " + dinamicAddress.AddressName;
                    dinamicAddress.AddressGuid = paramRow[25].ToStr() + "_" + paramRow[24].ToStr() + "#" + dinamicAddress.AddressGuid;
                }

                result.Add(dinamicAddress);
            }

            return result;
        }

        private string getName(string name, string socr, string level)
        {
            return string.Format("{0}. {1}", socr, name);
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
            var fiasDomainService = this.Container.Resolve<IDomainService<Fias>>();
            using (this.Container.Using(fiasDomainService))
            {
                var result = new List<IDinamicAddress>();
                if (filter == null || parentguid == null)
                    return result;

                var parentFias = fiasDomainService.GetAll()
                    .FirstOrDefault(x => x.ActStatus == FiasActualStatusEnum.Actual && x.AOGuid == parentguid);

                if (!string.IsNullOrEmpty(parentFias?.MirrorGuid))
                {
                    parentguid = parentFias.MirrorGuid;
                }

                var query = this.OpenSession().CreateSQLQuery(this.SqlStreetsDinamicAddress);
                query.SetParameter("filter", $"%{filter.ToLower().Replace(".", "")}%");
                query.SetParameter("parentguid", $"{parentguid}");
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

        // TODO: Расскоментировать
        /*
        /// <summary>
        /// Метод получения номера дома, корпуса, строения
        /// </summary>
        /// <param name="filter">Строка фильтрации</param>
        /// <param name="parentguid">AOGuid родительской записи</param>
        /// <returns>IDinamicAddress</returns>
        public override List<IDinamicAddress> GetHousesDynamicAddress(string filter, string parentguid, FiasEstimateStatusEnum estimatetype = FiasEstimateStatusEnum.NotDefined)
        {
            var result = new List<FiasHouseProxy>();
            if (filter == null || parentguid == null)
            {
                return result.ToList<IDinamicAddress>();
            }

            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.GetCurrentSession();

            var query = session.CreateSQLQuery(this.SqlHousesDynamicAddress);
           var query = estimatetype == FiasEstimateStatusEnum.NotDefined ? session.CreateSQLQuery(this.SqlHousesDynamicAddress) : session.CreateSQLQuery(this.SqlHousesDynamicAddressWithEstimate);
            query.SetParameter("filter", string.Format("%{0}%", filter));
            query.SetParameter("parentguid", Guid.Parse(parentguid));
            if(estimatetype != FiasEstimateStatusEnum.NotDefined)
                query.SetParameter("est_status", (short)estimatetype);

            foreach (object[] paramRow in query.List())
            {
                var fiasHouse = new FiasHouseProxy
                {
                    GuidId = paramRow[1].ToStr(),
                    PostCode = paramRow[2].ToStr(),
                    HouseNum = paramRow[3].ToStr(),
                    BuildNum = paramRow[4].ToStr(),
                    StrucNum = paramRow[5].ToStr(),
                    EndDate = paramRow[6].ToDateTimeNullable(),
                    StructureType = (FiasStructureTypeEnum) paramRow[7].ToInt(),
                    // TODO: Расскоментировать
                    //EstimateType = (FiasEstimateStatusEnum)paramRow[8].ToInt(),
                };

                var address = new StringBuilder();
                if (fiasHouse.HouseNum.IsNotEmpty())
                {
                    //address.AppendFormat(EnumHelper.GetPrefix(fiasHouse.EstimateType) +" {0} ", fiasHouse.HouseNum);
                }
                if (fiasHouse.BuildNum.IsNotEmpty())
                {
                    address.AppendFormat("корпус {0} ", fiasHouse.BuildNum);
                }
                if (fiasHouse.StrucNum.IsNotEmpty())
                {
                    var structName = fiasHouse.StructureType != FiasStructureTypeEnum.NotDefined
                        ? fiasHouse.StructureType.GetDescriptionName().ToLower()
                        : FiasStructureTypeEnum.Structure.GetDescriptionName().ToLower();

                    address.AppendFormat("{0} {1} ", structName, fiasHouse.StrucNum);
                }

                fiasHouse.AddressName = address.ToString().Trim();

                result.Add(fiasHouse);
            }

            if (result.Count > 1)
            {
                var maxEndDate = result.Where(x => x.EndDate.HasValue).Select(x => x.EndDate).Max();

                return result.OrderByDescending(x => x.EndDate)
                    .Select(x =>
                    {
                        x.AddressName = x.EndDate.HasValue && x.EndDate != maxEndDate
                            ? x.AddressName + $" - устарел ({x.EndDate:dd.MM.yyyy})"
                            : x.AddressName;

                        return x;
                    })
                    .ToList<IDinamicAddress>();
            }

            return result.ToList<IDinamicAddress>();
        }
    }*/

        /// <summary>
        /// Прокси класс для адреса дома
        /// </summary>
        public class FiasHouseProxy : IDinamicAddress
        {
            /// <summary>
            /// Код
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// AOGuid записи ФИАС
            /// </summary>
            public string GuidId { get; set; }

            public string Name { get; set; }

            /// <summary>
            /// Наименование полного адреса
            /// </summary>
            public string AddressName { get; set; }

            /// <summary>
            /// AOGuid всех составляющих данный адрес элементов
            /// </summary>
            public string AddressGuid { get; set; }

            /// <summary>
            /// Почтовый индекс
            /// </summary>
            public string PostCode { get; set; }

            /// <summary>
            /// Уровень
            /// </summary>
            public FiasLevelEnum Level { get; set; }

            /// <summary>
            /// AOGuid родительской записи
            /// </summary>
            public string ParentGuidId { get; set; }

            /// <summary>
            /// Наименвоание родительской записи
            /// </summary>
            public string ParentName { get; set; }

            /// <summary>
            /// Уровень родительской записи
            /// </summary>
            public FiasLevelEnum ParentLevel { get; set; }

            /// <summary>
            /// AOguid зеркальной записи
            /// </summary>
            public string MirrorGuid { get; set; }

            /// <summary>
            /// GUID дома
            /// </summary>
            public string HouseGuid { get; set; }

            /// <summary>
            /// Номер дома
            /// </summary>
            public string HouseNum { get; set; }

            /// <summary>
            /// Номер строения
            /// </summary>
            public string StrucNum { get; set; }

            /// <summary>
            /// Номер корпуса
            /// </summary>
            public string BuildNum { get; set; }

            /// <summary>
            /// Дата начала действия
            /// </summary>
            public DateTime? StartDate { get; set; }

            /// <summary>
            /// Дата окончания действия
            /// </summary>
            public DateTime? EndDate { get; set; }

            /// <summary>
            /// Вид строения
            /// </summary>
            public FiasStructureTypeEnum StructureType { get; set; }

            /*
            /// <summary>
            /// Тип владения
            /// </summary>
            public FiasEstimateStatusEnum EstimateType { get; set; }*/
        }
    }
}
