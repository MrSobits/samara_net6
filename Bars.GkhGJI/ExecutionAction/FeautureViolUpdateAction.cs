namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    public class FeautureViolUpdateAction : BaseExecutionAction
    {
        public IRepository<FeatureViolGji> FeatureViolDomain { get; set; }

        public override string Description
            => "Обновление подгрупп нарушений, для того чтобы сформировать полное наименование относительно родителя, разделяя через '/'";

        public override string Name => "Обновление подгрупп нарушений";

        public override Func<IDataResult> Action => this.FeautureViolUpdate;

        public BaseDataResult FeautureViolUpdate()
        {
            var data = this.FeatureViolDomain.GetAll()
                .OrderBy(x => x.Id)
                .AsEnumerable();

            var dictData = data.ToDictionary(x => x.Id);
            var dictByParent = data.GroupBy(x => x.Parent != null ? x.Parent.Id : 0)
                .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Id).ToList());

            var keys = dictByParent.Keys.OrderBy(x => x).ToList();

            var listToSave = new List<FeatureViolGji>();

            foreach (var key in keys)
            {
                var list = dictByParent[key];

                foreach (var entity in list)
                {
                    if (entity.Parent != null)
                    {
                        var newFullName = entity.Name;
                        this.GetFullName(dictData, ref newFullName, entity.Parent.Id);

                        if (newFullName != entity.FullName)
                        {
                            entity.FullName = newFullName;
                            listToSave.Add(entity);
                        }
                    }
                    else
                    {
                        if (entity.FullName != entity.Name)
                        {
                            entity.FullName = entity.Name;
                            listToSave.Add(entity);
                        }
                    }
                }
            }

            if (listToSave.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var entity in listToSave)
                        {
                            this.FeatureViolDomain.Update(entity);
                        }

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        throw exc;
                    }
                }
            }

            return new BaseDataResult();
        }

        private void GetFullName(Dictionary<long, FeatureViolGji> dictData, ref string result, long parentId)
        {
            var parentData = dictData[parentId];

            if (parentData != null)
            {
                result = parentData.Name.Trim() + "/ " + result;

                if (parentData.Parent != null)
                {
                    this.GetFullName(dictData, ref result, parentData.Parent.Id);
                }
            }
        }
    }
}