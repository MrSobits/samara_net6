namespace Bars.GkhGji.Regions.Sahalin.Report.Form1Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;

    public abstract class BaseReportSection
    {
        public string GetCell(int row, int col)
        {
            var report = this;
            var method = report.GetType().GetMethod(string.Format("GetCell{0}_{1}", row, col), BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method == null ? 0 : method.Invoke(report, null);
            return result.ToString();
        }

        protected long[] InspectionsIds;
        protected DateTime DateStart;
        protected DateTime DateEnd;

        protected BaseReportSection(long[] inspections, DateTime dateStart, DateTime dateEnd)
        {
            InspectionsIds = inspections;
            DateStart = dateStart;
            DateEnd = dateEnd;

            FillDocuments();
        }

        private IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        private void FillDocuments()
        {
            var disposals = Container.ResolveDomain<DocumentGji>().GetAll()
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Where(x => InspectionsIds.Contains(x.Inspection.Id))
                .WhereIf(DateStart != DateTime.MinValue, x => x.DocumentDate >= DateStart)
                .WhereIf(DateEnd != DateTime.MinValue, x => x.DocumentDate <= DateEnd)
                .ToList();

            DictDocuments[typeof (Disposal)] = disposals;

            FillChildDocuments(disposals.Select(x => x.Id).ToArray());
        }

        private void FillChildDocuments(long[] parentDocs)
        {
            if (!parentDocs.Any())
            {
                return;
            }

            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();

            var children = childrenDomain.GetAll()
                .Where(x => parentDocs.Contains(x.Parent.Id))
                .Where(x => x.Children.TypeDocumentGji != TypeDocumentGji.Disposal)
                .Select(x => x.Children)
                .ToList();

            children.ForEach(x =>
            {
                var t = x.GetType();

                if (!DictDocuments.ContainsKey(t))
                {
                    DictDocuments[t] = new List<DocumentGji>();
                }

                DictDocuments[t].Add(x);
            });

            var ids = children.Select(x => x.Id).ToArray();

            FillChildDocuments(ids);
        }

        protected Dictionary<Type, ICollection<DocumentGji>> DictDocuments = new Dictionary<Type, ICollection<DocumentGji>>();

        protected IEnumerable<T> GetDocuments<T>() where T : DocumentGji
        {
            var t = typeof (T);

            if (t == typeof (DocumentGji))
            {
                return DictDocuments
                    .SelectMany(x => x.Value)
                    .Select(x => x as T)
                    .Where(x => x != null)
                    .ToList();
            }

            var keys = DictDocuments.Keys;

            foreach (var key in keys)
            {
                if (key.IsAssignableFrom(t))
                {
                    return DictDocuments[key].Select(x => x as T).Where(x => x != null).ToList();
                }
            }

            if (DictDocuments.ContainsKey(t))
            {
                return DictDocuments[t]
                    .Select(x => x as T)
                    .Where(x => x != null)
                    .ToList();
            }

            return new List<T>();
        }
    }    
}