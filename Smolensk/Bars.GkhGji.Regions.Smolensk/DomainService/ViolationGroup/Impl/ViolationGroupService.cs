namespace Bars.GkhGji.Regions.Smolensk.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using Castle.Windsor;

    public class ViolationGroupService : IViolationGroupService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SavePoints(BaseParams baseParams)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();
            var violGroupPointDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();

            try
            {
                var groupId = baseParams.Params.GetAs("groupId", 0L);
                var documentId = baseParams.Params.GetAs("documentId", 0L);
                var roId = baseParams.Params.GetAs("roId", 0L);
                var description = baseParams.Params.GetAs("description", string.Empty);
                var strIds = baseParams.Params.GetAs("pointIds", string.Empty);
                var pointIds = !string.IsNullOrEmpty(strIds) ? strIds.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

                if (pointIds.Count() == 0)
                {
                    return new BaseDataResult();
                }

                DocumentViolGroup groupToSave = null;
                var group = violGroupDomain.GetAll().FirstOrDefault(x => x.Id == groupId);

                if (group == null)
                {
                    group = new DocumentViolGroup()
                                {
                                    Document = new DocumentGji { Id = documentId },
                                    RealityObject = roId > 0 ? new RealityObject { Id = roId } : null,
                                    Description = description
                                };
                    groupToSave = group;
                }
                else if (description != group.Description)
                {
                    group.Description = description;
                    groupToSave = group;
                }

                var listToSave = new List<DocumentViolGroupPoint>();

                // В этом словаре будет существующие записи пунктов нарушений
                var dictPoints = violGroupPointDomain.GetAll()
                                        .Where(x => x.ViolGroup.Id == groupId)
                                        .Select(x => new { x.Id, StageId = x.ViolStage.Id })
                                        .AsEnumerable()
                                        .GroupBy(x => x.StageId)
                                        .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());
                
                // По переданным id инспекторов если их нет в списке существующих, то добавляем
                foreach (var id in pointIds)
                {
                    if (id > 0)
                    {
                        if (dictPoints.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictPoints.Remove(id);
                            continue;
                        }

                        listToSave.Add(new DocumentViolGroupPoint
                        {
                            ViolGroup = group,
                            ViolStage = new InspectionGjiViolStage { Id = id }
                        });
                    }
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        if (groupToSave != null)
                        {
                            if (groupToSave.Id > 0)
                            {
                                violGroupDomain.Update(groupToSave);
                            }
                            else
                            {
                                violGroupDomain.Save(groupToSave);
                            }
                        }

                        foreach (var item in listToSave)
                        {
                            violGroupPointDomain.Save(item);
                        }

                        foreach (var keyValue in dictPoints)
                        {
                            violGroupPointDomain.Delete(keyValue.Value);
                        }

                        transaction.Commit();
                        return new BaseDataResult(group);
                    }
                    catch (ValidationException e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult { Success = false, Message = e.Message };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally 
            {
                Container.Release(violGroupDomain);
                Container.Release(violGroupPointDomain);
            }
        }
    }
}