namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;

    using Dapper;

    /// <summary>
    /// Действие обновляет FiasAddress, если у улицы случилось переподчинение
    /// </summary>
    public class UpdateFiasAddressStreetParentAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Description
            => "Действие обновляет таблицу B4_FIAS_ADDRESS, если у улицы в записи случилось переподчинение к другому PLACE_GUID";

        /// <inheritdoc />
        public override string Name => "Действие по обновление B4_FIAS_ADDRESS при переподчинении улиц";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        public ISessionProvider SessionProvider { get; set; }

        private string SqlPlacesFindFullAddress
            => @"select t.aoguid, t.postalcode, t.offname t_name, t.shortname t_sname, t.coderecord t_code, t.aolevel t_aolevel,
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
                where t.actstatus = 1 and t.aolevel != 7 and t.aoguid in ({0})
                     and ( p1.id is null or (p1.id is not null and p1.actstatus = 1))
                     and ( p2.id is null or (p2.id is not null and p2.actstatus = 1))
                     and ( p3.id is null or (p3.id is not null and p3.actstatus = 1))
                     and ( p4.id is null or (p4.id is not null and p4.actstatus = 1))
                     and ( p5.id is null or (p5.id is not null and p5.actstatus = 1)) ";

        private BaseDataResult Execute()
        {
            this.SessionProvider.InStatelessConnectionTransaction(this.SqlAction);
            return new BaseDataResult();
        }

        private void SqlAction(IDbConnection connection, IDbTransaction transaction)
        {
            // проставляем верный street_guid, недобавленный вручную
            connection.Execute(
                @"update b4_fias_address add
                    set street_guid = f.aoguid
                    from b4_fias f
                     join b4_fias f1 on
                                       f.parentguid = f1.parentguid and
                                       f.formalname = f1.formalname and
                                       f.shortname = f1.shortname and
                                       f1.aolevel = 7 and
                                       f1.actstatus = 1 and
                                       f1.typerecord = 20
                    where f.aolevel = 7 and f.actstatus = 1 and f.typerecord = 10 and add.street_guid = f1.aoguid", 
                transaction: transaction);

            const string SqlToFindFailAddresses = @"SELECT
                  faddr.id as Id,
                  actual.aoguid as AoGuid,
                  faddr.place_address_name as PlaceAddressName,
                  faddr.address_name as AddressName
                FROM b4_fias_address faddr
                  JOIN b4_fias ff ON ff.aoguid = faddr.street_guid AND ff.actstatus = 1 AND ff.aolevel = 7
                  JOIN b4_fias actual ON ff.parentguid = actual.aoguid AND actual.actstatus = 1
                WHERE NOT exists(SELECT NULL
                                 FROM b4_fias fias
                                 WHERE fias.aoguid = faddr.street_guid AND fias.parentguid = faddr.place_guid AND fias.actstatus = 1 AND
                                       fias.aolevel = 7)";

            // достаем адреса ФИАС, у которых не совпадает родитель улицы
            var list = connection.Query<FiasAddressProxy>(SqlToFindFailAddresses, transaction: transaction);

            if (list.IsEmpty())
            {
                return;
            }

            // формируем актуальные PlaceAddressName
            var uniqueParentGuids = list.Select(x => x.AoGuid).Distinct();
            var parentInfo = this.GetDynamicAddresses(connection, uniqueParentGuids);

            foreach (var fiasAddressProxy in list)
            {
                var aoGuid = fiasAddressProxy.AoGuid;
                var parent = parentInfo.Get(aoGuid);
                if (parent.IsNull())
                {
                    continue;
                }

                var addressName = fiasAddressProxy.AddressName?.Replace(fiasAddressProxy.PlaceAddressName ?? parent.Region, parent.AddressName);
                var placeAddressName = parent.AddressName;
                connection.Execute(
                    $@"UPDATE B4_FIAS_ADDRESS
                    SET place_address_name = '{placeAddressName}',
                        place_name = '{parent
                        .Name}',
                        address_name = '{addressName}',
                        place_guid = '{aoGuid}'
                    WHERE ID = {fiasAddressProxy
                            .Id}",
                    transaction: transaction);
            }
        }

        private IDictionary<string, DynamicAddressProxy> GetDynamicAddresses(IDbConnection connection, IEnumerable<string> uniqueParentGuids)
        {
            var dynamicAddresses = new List<DynamicAddressProxy>();
            using (
                var reader =
                    connection.ExecuteReader(
                        this.SqlPlacesFindFullAddress.FormatUsing(uniqueParentGuids.Select(x => $"'{x}'").AggregateWithSeparator(","))))
            {
                while (reader.Read())
                {
                    var socr = string.Empty;
                    var paramRow = reader;

                    if (!string.IsNullOrEmpty(paramRow[3].ToStr()))
                    {
                        socr = paramRow[3].ToStr() + ". ";
                    }

                    var dinamicAddress = new DynamicAddressProxy
                    {
                        GuidId = paramRow[0].ToStr(),
                        Name = socr + paramRow[2].ToStr()
                    };

                    dinamicAddress.AddressName = dinamicAddress.Name;

                    if (!string.IsNullOrEmpty(paramRow[6].ToStr()))
                    {
                        socr = string.Empty;
                        if (!string.IsNullOrEmpty(paramRow[7].ToStr()))
                        {
                            socr = paramRow[7].ToStr() + ". ";
                        }

                        dinamicAddress.AddressName = socr + paramRow[6].ToStr() + ", " + dinamicAddress.AddressName;
                    }

                    if (!string.IsNullOrEmpty(paramRow[10].ToStr()))
                    {
                        socr = string.Empty;
                        if (!string.IsNullOrEmpty(paramRow[11].ToStr()))
                        {
                            socr = paramRow[11].ToStr() + ". ";
                        }

                        dinamicAddress.Region = socr + paramRow[10].ToStr();
                        dinamicAddress.AddressName = socr + paramRow[10].ToStr() + ", " + dinamicAddress.AddressName;
                    }

                    if (!string.IsNullOrEmpty(paramRow[14].ToStr()))
                    {
                        socr = string.Empty;
                        if (!string.IsNullOrEmpty(paramRow[15].ToStr()))
                        {
                            socr = paramRow[15].ToStr() + ". ";
                        }

                        dinamicAddress.AddressName = socr + paramRow[14].ToStr() + ", " + dinamicAddress.AddressName;
                    }

                    if (!string.IsNullOrEmpty(paramRow[18].ToStr()))
                    {
                        socr = string.Empty;
                        if (!string.IsNullOrEmpty(paramRow[19].ToStr()))
                        {
                            socr = paramRow[19].ToStr() + ". ";
                        }

                        dinamicAddress.AddressName = socr + paramRow[18].ToStr() + ", " + dinamicAddress.AddressName;
                    }

                    if (!string.IsNullOrEmpty(paramRow[22].ToStr()))
                    {
                        socr = string.Empty;
                        if (!string.IsNullOrEmpty(paramRow[23].ToStr()))
                        {
                            socr = paramRow[23].ToStr() + ". ";
                        }

                        dinamicAddress.AddressName = socr + paramRow[22].ToStr() + ", " + dinamicAddress.AddressName;
                    }

                    dynamicAddresses.Add(dinamicAddress);
                }
            }

            return dynamicAddresses.ToDictionary(x => x.GuidId);
        }

        private class FiasAddressProxy
        {
            public long Id { get; set; }

            public string AoGuid { get; set; }

            public string PlaceAddressName { get; set; }

            public string AddressName { get; set; }
        }

        private class DynamicAddressProxy
        {
            public string GuidId { get; set; }

            public string AddressName { get; set; }

            public string Name { get; set; }
            public string Region { get; set; }
        }
    }
}