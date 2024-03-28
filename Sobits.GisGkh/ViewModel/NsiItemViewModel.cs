namespace Sobits.GisGkh.ViewModel
{
    using Entities;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.B4;
    using System.Collections.Generic;

    public class NsiItemViewModel : BaseViewModel<NsiItem>
    {
        public IDomainService<NsiItem> NsiItemDomain { get; set; }
        public IDomainService<StringField> StringFieldDomain { get; set; }
        public IDomainService<NsiRefField> NsiRefFieldDomain { get; set; }
        public IDomainService<DateField> DateFieldDomain { get; set; }
        public IDomainService<AttachmentField> AttachmentFieldDomain { get; set; }
        public override IDataResult List(IDomainService<NsiItem> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);
            var nsiListId = loadParams.Filter.GetAs("NsiListId", 0L);

            List<NsiItem> nsiItems = NsiItemDomain.GetAll()
                .Where(x => x.NsiList.Id == nsiListId).ToList();
            nsiItems.AddRange(FindRefItems(nsiItems));

            List<StringField> strings = StringFieldDomain.GetAll()
                .Where(x => nsiItems.Contains(x.NsiItem))
                .ToList();

            List<NsiRefField> nsiRefs = NsiRefFieldDomain.GetAll()
                .Where(x => nsiItems.Contains(x.NsiItem))
                .ToList();

            List<AttachmentField> attachments = AttachmentFieldDomain.GetAll()
                .Where(x => nsiItems.Contains(x.NsiItem))
                .ToList();

            var predata = domain.GetAll()
                .Where(x => x.NsiList != null && x.NsiList.Id == nsiListId)
           .Select(x => new
           {
               x.Id,
               x.EntityItemId,
               x.GisGkhGUID,
               x.GisGkhItemCode,
               x.NsiList,
               x.IsActual,
               Name = FindName(x.Id, strings, nsiRefs, attachments)
           }).AsEnumerable();

            var data = predata
                .Select(x => new
                {
                    x.Id,
                    x.EntityItemId,
                    x.GisGkhGUID,
                    x.GisGkhItemCode,
                    x.NsiList,
                    x.IsActual,
                    x.Name
                })
                .AsQueryable()
           .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }

        private List<NsiItem> FindRefItems(List<NsiItem> nsiItems)
        {
            List<NsiItem> refItems = NsiRefFieldDomain.GetAll()
                .Where(x => nsiItems.Contains(x.NsiItem))
                .Where(x => x.NsiRefItem != null)
                .Select(x => x.NsiRefItem)
                .ToList();
            if (refItems.Count() > 0)
            {
                List<NsiItem> newRefItems = FindRefItems(refItems);
                refItems.AddRange(newRefItems);
            }
            return refItems;
        }

        private string FindName(long id, List<StringField> allStrings, List<NsiRefField> allNsiRefs, List<AttachmentField> allAttachments)
        {
            //var dateFieldDomain = this.Container.Resolve<IDomainService<DateField>>();
            string result = "";
            var strings = allStrings
                .Where(x => x.NsiItem.Id == id)
                .Select(x => new
                {
                    x.Name,
                    x.Value
                })
                .ToList();
            foreach (var str in strings)
            {
                if (result == "")
                {
                    result += str.Name + ": " + str.Value;
                }
                else
                {
                    result += "\r\n" + str.Name + ": " + str.Value;
                }
            }
            //var dates = dateFieldDomain.GetAll()
            //    .Where(x => x.NsiItem.Id == id)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        x.Value
            //    })
            //    .ToList();
            //foreach (var date in dates)
            //{
            //    if (result == "")
            //    {
            //        result += date.Name + ": " + date.Value.ToString();
            //    }
            //    else
            //    {
            //        result += "\r\n" + date.Name + ": " + date.Value.ToString();
            //    }
            //}
            var nsiRefs = allNsiRefs
                .Where(x => x.NsiItem.Id == id)
                .Select(x => new
                {
                    x.Name,
                    x.NsiRefItem,
                    x.RefGUID
                })
                .ToList();
            foreach (var nsi in nsiRefs)
            {
                if (result == "")
                {
                    result += nsi.Name + ": " + (nsi.NsiRefItem != null ? FindName(nsi.NsiRefItem.Id, allStrings, allNsiRefs, allAttachments) : nsi.RefGUID);
                }
                else
                {
                    result += "\r\n" + nsi.Name + ": " + (nsi.NsiRefItem != null ? FindName(nsi.NsiRefItem.Id, allStrings, allNsiRefs, allAttachments) : nsi.RefGUID);
                }
            }
            var attachments = allAttachments
                .Where(x => x.NsiItem.Id == id)
                .Select(x => new
                {
                    x.Name,
                    x.Description
                })
                .ToList();
            foreach (var attachment in attachments)
            {
                if (result == "")
                {
                    result += attachment.Name + ": " + attachment.Description;
                }
                else
                {
                    result += "\r\n" + attachment.Name + ": " + attachment.Description;
                }
            }

            return result;
        }
    }
}