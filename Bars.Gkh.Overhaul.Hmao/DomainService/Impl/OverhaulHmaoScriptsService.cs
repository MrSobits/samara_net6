namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using NHibernate;
    using Overhaul.Entities;

    public class OverhaulHmaoScriptsService : IOverhaulHmaoScriptsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoSeService { get; set; }

        public IDomainService<StructuralElement> SeService { get; set; }

        public IDomainService<CommonEstateObject> CeoService { get; set; }

        public IDomainService<StructuralElementGroup> SeGroupService { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public IDataResult UpdateStructElements()
        {
            var codesForDelete = new List<string>
            {
                "109", "110", "116", "112",
                "49", "227", "228", "229",
                "230", "231", "11", "235",
                "236", "237", "239", "22",
                "241", "242", "243", "234",
                "13", "24", "220", "244",
                "252", "253", "36", "29",
                "30", "44", "23", "12",
                "256", "31", "259", "258",
                "114", "41", "119", "120",
                "111", "115", "117", "6",
                "2", "3", "4", "17",
                "16", "356"
            };

            var codesForReplace = new Dictionary<string, string>
            {
                {"125", "123"},
                {"122", "123"},
                {"124", "123"},
                {"192", "123"},
                {"53", "123"},
                {"54", "123"},
                {"55", "129"},
                {"222", "221"},

                {"238", "357"},
                {"21", "357"},
                {"32", "357"},
                {"34", "357"},
                {"35", "357"},
                {"37", "357"},
                {"232", "357"},
                {"233", "357"},
                {"251", "357"},
                {"250", "248"},
                {"302", "248"},
                {"601", "248"},
                {"246", "247"},
                {"249", "247"},
                {"301", "247"},
                {"600", "247"}
            };

            var codesForAgrg =
                new Dictionary<string, IEnumerable<string>>
                {
                    {"313", new[] {"251", "254", "255"}},
                    {"202", new[] {"202", "204"}}
                };

            DeleteDpkr();

            MoveGroupStructElements(new List<string> {"213", "224", "9", "214"}, "Приборы учета", "Приборы учета");

            MoveGroupStructElements(new List<string> {"226", "225"}, "Водоснабжение", "Внутренняя система горячего водоснабжения");

            MoveGroupStructElements(new List<string> {"261", "260", "262", "263", "38"}, "Крыша", "Чердачные помещения");

            MoveGroupStructElements(new List<string> { "5", "205", "207" }, "Отопление", "Отопительные приборы");

            MoveGroupStructElements(new List<string> { "8", "14", "221", "223", "225", "226" }, "Водоснабжение", "Иные конструктивные элементы сети водоснабжения");

            AggregateStructElems(codesForAgrg);

            codesForDelete.AddRange(ReplaceStructElement(codesForReplace));

            DeleteStructElems(codesForDelete);

            DeleteTwins();

            AddStructElements(new[] {TypeHouse.ManyApartments}, new[] {"361"});

            return new BaseDataResult();
        }

        private void AddStructElements(IEnumerable<TypeHouse> types, IEnumerable<string> codes)
        {
            var structElems = SeService.GetAll()
                .Where(x => codes.Contains(x.Code))
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First());

            if (!structElems.Any())
            {
                return;
            }

            var robjectIds = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => types.Contains(x.TypeHouse))
                .Select(x => x.Id)
                .ToList();

            var listToCreate = new List<RealityObjectStructuralElement>();

            foreach (var code in codes)
            {
                if (!structElems.ContainsKey(code))
                {
                    continue;
                }

                var se = structElems[code];

                var existSe = new HashSet<long>(RoSeService.GetAll()
                    .Where(x => x.StructuralElement.Code == code)
                    .Where(x => types.Contains(x.RealityObject.TypeHouse))
                    .Select(x => x.RealityObject.Id)
                    .AsEnumerable());

                foreach (var roId in robjectIds)
                {
                    if (existSe.Contains(roId))
                    {
                        continue;
                    }

                    listToCreate.Add(new RealityObjectStructuralElement
                    {
                        LastOverhaulYear = 0,
                        Name = se.Name,
                        StructuralElement = se,
                        RealityObject = new RealityObject {Id = roId},
                        Repaired = false,
                        Volume = 0
                    });
                }
            }

            var session = SessionProvider.GetCurrentSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    listToCreate.ForEach(x => session.Save(x));

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private void DeleteDpkr()
        {
            var session = SessionProvider.GetCurrentSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    session.CreateSQLQuery("delete from OVRHL_PUBLISH_PRG_REC").ExecuteUpdate();
                    session.CreateSQLQuery("delete from OVRHL_PUBLISH_PRG").ExecuteUpdate();

                    session.CreateSQLQuery("delete from OVRHL_SHORT_PROG_DIFITSIT").ExecuteUpdate();
                    session.CreateSQLQuery("delete from OVRHL_SHORT_PROG_REC").ExecuteUpdate();

                    session.CreateSQLQuery("delete from ovrhl_subsidy_rec_version").ExecuteUpdate();
                    session.CreateSQLQuery("delete from ovrhl_subsidy_rec").ExecuteUpdate();
                    session.CreateSQLQuery("delete from OVRHL_DPKR_CORRECT_ST2").ExecuteUpdate();

                    session.CreateSQLQuery("delete from ovrhl_change_year_owner_decision").ExecuteUpdate();

                    session.CreateSQLQuery("delete from ovrhl_stage1_version").ExecuteUpdate();
                    session.CreateSQLQuery("delete from ovrhl_stage2_version").ExecuteUpdate();
                    session.CreateSQLQuery("delete from ovrhl_version_rec").ExecuteUpdate();
                    session.CreateSQLQuery("delete from ovrhl_version_prm").ExecuteUpdate();

                    session.CreateSQLQuery("delete from ovrhl_prg_version").ExecuteUpdate();

                    session.CreateSQLQuery("delete from OVRHL_RO_STRUCT_EL_IN_PRG").ExecuteUpdate();
                    session.CreateSQLQuery("delete from OVRHL_RO_STRUCT_EL_IN_PRG_2").ExecuteUpdate();
                    session.CreateSQLQuery("delete from OVRHL_RO_STRUCT_EL_IN_PRG_3").ExecuteUpdate();

                    session.CreateSQLQuery("delete from REAL_EST_TYPE_STRUCT_EL").ExecuteUpdate();

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            SessionProvider.CloseCurrentSession();
        }

        private void DeleteTwins()
        {
            var roStructElements = RoSeService.GetAll()
                .Where(x => !x.StructuralElement.Group.CommonEstateObject.MultipleObject)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.StructuralElement.Id)
                        .ToDictionary(x => x.Key, z => z.ToList()));

            var keys = roStructElements.Keys.ToList();

            foreach (var key in keys)
            {
                var ro = roStructElements[key];

                var keysSe = ro.Keys.ToList();

                foreach (var keySe in keysSe)
                {
                    var roSe = ro[keySe];

                    if (roSe.Count == 1)
                    {
                        ro.Remove(keySe);
                        continue;
                    }

                    var max = roSe
                        .OrderByDescending(x => x.LastOverhaulYear)
                        .ThenByDescending(x => x.Volume)
                        .FirstOrDefault();
                    roSe.Remove(max);
                }
            }

            var session = SessionProvider.GetCurrentSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    foreach (var rose in roStructElements)
                    {
                        foreach (var se in rose.Value)
                        {
                            foreach (var item in se.Value)
                            {
                                session.Delete(item);
                            }
                        }
                    }
                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private void DeleteStructElems(ICollection<string> codesForDelete)
        {
            var roStructElems = RoSeService.GetAll()
                .Where(x => codesForDelete.Contains(x.StructuralElement.Code))
                .ToList();

            var structElems = SeService.GetAll()
                .Where(x => codesForDelete.Contains(x.Code))
                .ToList();

            var structElemsWorks = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                .Where(x => codesForDelete.Contains(x.StructuralElement.Code))
                .ToList();

            var session = SessionProvider.GetCurrentSession();
            using (var tr = session.BeginTransaction())
            {
                try
                {
                    foreach (var rose in roStructElems)
                    {
                        session.Delete(rose);
                    }

                    foreach (var rose in structElemsWorks)
                    {
                        session.Delete(rose);
                    }

                    foreach (var rose in structElems)
                    {
                        session.Delete(rose);
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            SessionProvider.CloseCurrentSession();
        }

        private void AggregateStructElems(Dictionary<string, IEnumerable<string>> codes)
        {
            var listForCreate = new List<IEntity>();
            var listForDelete = new List<IEntity>();

            var destCodes = codes.Select(x => x.Key).ToList();

            var destStructElems = SeService.GetAll()
                .Where(x => destCodes.Contains(x.Code))
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First());

            foreach (var code in codes)
            {
                if (!destStructElems.ContainsKey(code.Key))
                {
                    continue;
                }

                var value = code.Value;

                var roStructElems = RoSeService.GetAll()
                    .Where(x => value.Contains(x.StructuralElement.Code))
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.StructuralElement.Code,
                        RoSe = x
                    })
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new
                    {
                        x.Code,
                        x.RoSe
                    }));

                var destSe = destStructElems[code.Key];

                foreach (var roStructElem in roStructElems)
                {
                    var volume = roStructElem.Value.Sum(x => x.RoSe.Volume);
                    var lastOverhaulYear = roStructElem.Value.Average(x => x.RoSe.LastOverhaulYear).ToInt();

                    foreach (var structElem in roStructElem.Value)
                    {
                        if (structElem.Code != destSe.Code)
                        {
                            listForDelete.Add(structElem.RoSe);
                        }
                    }

                    var newRoSe = new RealityObjectStructuralElement
                    {
                        RealityObject = new RealityObject {Id = roStructElem.Key},
                        StructuralElement = destSe,
                        LastOverhaulYear = lastOverhaulYear,
                        Volume = volume,
                        Name = destSe.Name
                    };

                    listForCreate.Add(newRoSe);
                }
            }

            var session = SessionProvider.GetCurrentSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    listForDelete.ForEach(session.Delete);
                    listForCreate.ForEach(x => session.Save(x));
                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private void MoveGroupStructElements(ICollection<string> codes, string ceoName, string groupName)
        {
            var listForUpdate = new List<IEntity>();

            var seGroup = GetSeGroup(groupName, GetCeo(ceoName));

            var structElems = SeService.GetAll()
                .Where(x => codes.Contains(x.Code))
                .ToList();

            foreach (var se in structElems)
            {
                se.Group = seGroup;
                listForUpdate.Add(se);
            }

            Update(SessionProvider.GetCurrentSession(), listForUpdate);
            SessionProvider.CloseCurrentSession();
        }

        private IEnumerable<string> ReplaceStructElement(Dictionary<string, string> codes)
        {
            var result = new List<string>();

            result.AddRange(codes.Select(x => x.Key));

            var sourceCodes = codes.Select(x => x.Key).ToList();

            var roStructElements = RoSeService.GetAll()
                .Where(x => sourceCodes.Contains(x.StructuralElement.Code))
                .GroupBy(x => x.StructuralElement.Code)
                .ToDictionary(x => x.Key, y => y.ToList());

            var structElements = SeService.GetAll()
                .Where(x => x.Code != null)
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First());

            var listForUpdate = new List<IEntity>();

            foreach (var code in codes)
            {
                if (!roStructElements.ContainsKey(code.Key))
                {
                    continue;
                }

                var elements = roStructElements[code.Key];

                if (structElements.ContainsKey(code.Value))
                {
                    var destElement = structElements[code.Value];

                    foreach (var e in elements)
                    {
                        e.StructuralElement = destElement;
                        listForUpdate.Add(e);
                        result.Add(code.Key);
                    }
                }
            }

            Update(SessionProvider.GetCurrentSession(), listForUpdate);

            SessionProvider.CloseCurrentSession();

            return result;
        }

        private StructuralElementGroup GetSeGroup(string groupName, CommonEstateObject ceo)
        {
            var groupNameLower = groupName.ToLower().Replace(" ", "");

            var groupSe = SeGroupService.GetAll()
                .Where(x => x.CommonEstateObject.Id == ceo.Id)
                .FirstOrDefault(x => x.Name.ToLower().Replace(" ", "") == groupNameLower);

            if (groupSe == null)
            {
                groupSe = new StructuralElementGroup
                {
                    CommonEstateObject = ceo,
                    Name = groupName,
                    Required = false,
                    Formula = "",
                    FormulaName = "",
                    FormulaDescription = "",
                    FormulaParams = new List<FormulaParams>()
                };

                SeGroupService.Save(groupSe);
            }

            return groupSe;
        }

        private CommonEstateObject GetCeo(string ceoName)
        {
            var ceoNameLower = ceoName.ToLower().Replace(" ", "");
            var ceo = CeoService.GetAll().FirstOrDefault(x => x.Name.ToLower().Replace(" ", "") == ceoNameLower);

            if (ceo == null)
            {
                var ceoGroup = GetCeoGroup(ceoName);

                ceo = new CommonEstateObject
                {
                    Name = ceoName,
                    ShortName = ceoName,
                    Code = "0",
                    GroupType = ceoGroup,
                    IncludedInSubjectProgramm = true,
                    IsMatchHc = true,
                    IsEngineeringNetwork = false,
                    MultipleObject = false,
                    Weight = 0
                };

                CeoService.Save(ceo);
            }

            return ceo;
        }

        private GroupType GetCeoGroup(string ceoName)
        {
            var ceoNameLower = ceoName.ToLower().Replace(" ", "");
            var serviceCeoGroup = Container.Resolve<IDomainService<GroupType>>();

            var ceoGroup = serviceCeoGroup.GetAll()
                .FirstOrDefault(x => x.Name.ToLower().Replace(" ", "") == ceoNameLower);

            if (ceoGroup == null)
            {
                ceoGroup = new GroupType { Code = "0", Name = ceoName };
                serviceCeoGroup.Save(ceoGroup);
            }

            return ceoGroup;
        }

        private void Update(ISession session, IEnumerable<IEntity> entities)
        {
            using (var tr = session.BeginTransaction())
            {
                try
                {
                    entities.ForEach(x => session.Update(x));

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private void Delete(ISession session, IEnumerable<IEntity> entities)
        {
            using (var tr = session.BeginTransaction())
            {
                try
                {
                    entities.ForEach(session.Delete);

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}