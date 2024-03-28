namespace Bars.GkhCr.Controllers
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    using Syncfusion.DocIO;
    using Syncfusion.DocIO.DLS;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class PhotoArchivePrintController : BaseController
    {
        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        public IDomainService<RealityObjectImage> RealityObjectImageDomain { get; set; }

        public IDomainService<ReportHistory> ReportHistoryDomain { get; set; }

        public IDomainService<PrintForm> PrintFormDomain { get; set; }

        public IFileManager FileManager { get; set; }
        
        public IUserInfoProvider UserInfoProvider { get; set; }
        
        public ActionResult PrintReport(BaseParams baseParams)
        {
            var municipalityIdsString = baseParams.Params["Municipalities"].ToString();
            var municipalityIds = municipalityIdsString.ToLongArray();

            var programCrId = baseParams.Params["ProgramCr"].ToInt();

            var realObjIdsString = baseParams.Params["RealityObjs"].ToString();
            var realObjIds = realObjIdsString.ToLongArray();

            var programCr = ProgramCrDomain.GetAll().FirstOrDefault(x => x.Id == programCrId);
            if (programCr == null)
            {
                return JsonNetResult.Failure("Выберите программу капитального ремонта!");
            }

            var id = baseParams.Params.GetAs<long>(baseParams.Params.ContainsKey("reportId") ? "reportId" : "id");

            var printFormObject = PrintFormDomain.Get(id);
            if (printFormObject == null)
            {
                return JsonNetResult.Failure("Ошибка выбора отчета!");
            }

            var manOrgByRo =
                ManOrgContractRealityObjectDomain.GetAll()
                                                 .Where(
                                                     x =>
                                                     x.ManOrgContract.ManagingOrganization != null
                                                     && (!x.ManOrgContract.EndDate.HasValue
                                                         || x.ManOrgContract.EndDate >= DateTime.Now))
                                                 .WhereIf(
                                                     municipalityIds.Length > 0,
                                                     x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                                 .WhereIf(
                                                     realObjIds.Length > 0,
                                                     x => realObjIds.Contains(x.RealityObject.Id))
                                                 .Select(
                                                     x =>
                                                     new
                                                         {
                                                             RealObjId = x.RealityObject.Id,
                                                             ManOrgName =
                                                         x.ManOrgContract.ManagingOrganization.Contragent.Name
                                                         })
                                                 .AsEnumerable()
                                                 .GroupBy(x => x.RealObjId)
                                                 .ToDictionary(
                                                     x => x.Key,
                                                     y => y.Select(x => x.ManOrgName).FirstOrDefault());

            var data =
                RealityObjectImageDomain.GetAll()
                                        .Where(
                                            x =>
                                            programCr.Period.Id == x.Period.Id && x.WorkCr != null
                                            && (x.ImagesGroup == ImagesGroup.AfterOverhaul
                                                || x.ImagesGroup == ImagesGroup.BeforeOverhaul))
                                        .WhereIf(
                                            municipalityIds.Length > 0,
                                            x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                        .WhereIf(realObjIds.Length > 0, x => realObjIds.Contains(x.RealityObject.Id));
            
            // если > 100 записей (т.е 200 фоток) невыгружаем  
            if (data.Count(x => x.File != null) > 100)
            {
                return JsonNetResult.Failure("Слишком большой объем информации. Выберите дома!");
            }

            // фотки до и после группируем по дому и по виду работ
            var dictByRo =
                data.Select(
                    x =>
                    new
                        {
                            x.RealityObject.Id,
                            MunicipalityName = x.RealityObject.Municipality.Name,
                            x.RealityObject.Address,
                            WorkId = x.WorkCr.Id,
                            WorkName = x.WorkCr.Name,
                            x.DateImage,
                            FileId = x.File != null ? x.File.Id : 0,
                            x.ImagesGroup,
                            ManOrg =
                        manOrgByRo.ContainsKey(x.RealityObject.Id) ? manOrgByRo[x.RealityObject.Id] : string.Empty
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        w =>
                        w.GroupBy(x => x.WorkId)
                         .ToDictionary(
                             x => x.Key,
                             y =>
                             new
                                 {
                                     BeforeId =
                                 y.FirstOrDefault(x => x.ImagesGroup == ImagesGroup.BeforeOverhaul)
                                  .Return(z => z.FileId),
                                     AfterId =
                                 y.FirstOrDefault(x => x.ImagesGroup == ImagesGroup.AfterOverhaul).Return(z => z.FileId),
                                     y.First().MunicipalityName,
                                     y.First().Address,
                                     y.First().WorkName,
                                     y.First().DateImage,
                                     y.First().ImagesGroup,
                                     y.First().ManOrg,
                                     BeforeDate =
                                 y.FirstOrDefault(x => x.ImagesGroup == ImagesGroup.BeforeOverhaul)
                                  .Return(z => z.DateImage.Value.ToShortDateString(), string.Empty),
                                     AfterDate =
                                 y.FirstOrDefault(x => x.ImagesGroup == ImagesGroup.AfterOverhaul)
                                  .Return(z => z.DateImage.Value.ToShortDateString(), string.Empty)
                                 }));

            var document = new WordDocument();

            foreach (var infByRo in dictByRo)
            {
                foreach (var infByWork in infByRo.Value)
                {
                    var info = infByWork.Value;
                    PrintPage(
                        document,
                        info.BeforeId,
                        info.AfterId,
                        info.MunicipalityName,
                        info.Address,
                        info.ManOrg,
                        programCr.Name,
                        info.WorkName,
                        info.BeforeDate,
                        info.AfterDate);
                }
            }

            using (var stream = new MemoryStream())
            {
                if (dictByRo.Count > 0)
                {
                    document.Save(stream, Syncfusion.DocIO.FormatType.Doc);
                }

                var file = FileManager.SaveFile(stream, "PhotoArchive.doc");

                this.ReportHistoryDomain.Save(
                    new ReportHistory
                    {
                        ReportType = ReportType.PrintForm,
                        ReportId = id,
                        Date = DateTime.UtcNow,
                        Category = printFormObject.Category,
                        Name = printFormObject.Name,
                        User = this.UserInfoProvider?.GetActiveUser(),
                        File = file
                    });

                return new JsonNetResult(new { taskedReport = false, fileId = file.Id });
            }
        }

        private void PrintPage(WordDocument document, long id1, long id2, string mu, string addres, string mo, string program, string workkind, string date1, string date2)
        {
            var section = document.AddSection();
            section.PageSetup.Orientation = Syncfusion.DocIO.DLS.PageOrientation.Landscape;
            section.PageSetup.DifferentFirstPage = true;
            var table = section.AddTable();
            section.PageSetup.VerticalAlignment = PageAlignment.Middle;
            var format = new RowFormat();
            format.Paddings.All = 5;
            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.Single;
            format.Borders.LineWidth = 2;
            format.HorizontalAlignment = RowAlignment.Center;

            table.ResetCells(8, 2, format, 350);

            table.Rows[2].Cells[0].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Start;
            table.Rows[2].Cells[1].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Continue;
            AddText(table.Rows[2].Cells[0], program);

            table.Rows[3].Cells[0].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Start;
            table.Rows[3].Cells[1].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Continue;
            AddText(table.Rows[3].Cells[0], mu);

            table.Rows[4].Cells[0].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Start;
            table.Rows[4].Cells[1].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Continue;
            AddText(table.Rows[4].Cells[0], addres);

            table.Rows[5].Cells[0].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Start;
            table.Rows[5].Cells[1].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Continue;
            AddText(table.Rows[5].Cells[0], workkind);

            table.Rows[6].Cells[0].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Start;
            table.Rows[6].Cells[1].CellFormat.HorizontalMerge = Syncfusion.DocIO.DLS.CellMerge.Continue;
            AddText(table.Rows[6].Cells[0], mo);

            AddText(table.Rows[1].Cells[0], "До капремонта");
            AddText(table.Rows[1].Cells[1], "После капремонта");

            AddText(table.Rows[7].Cells[0], date1);
            AddText(table.Rows[7].Cells[1], date2);

            if (id1 > 0)
            {
                AddPhoto(table.Rows[0].Cells[0], id1);
            }

            if (id2 > 0)
            {
                AddPhoto(table.Rows[0].Cells[1], id2);
            }
        }

        private void AddPhoto(WTableCell cell, long id1)
        {
            var par = cell.AddParagraph();
            var fileInfo = FileInfoDomain.GetAll().FirstOrDefault(x => x.Id == id1);

            if (fileInfo != null)
            {
                try
                {
                    WPicture withImg;
                    using (var imageStream = Container.Resolve<IFileManager>().GetFile(fileInfo))
                    {
                        withImg = (WPicture)par.AppendPicture(imageStream);
                    }

                    var scale = withImg.Width / withImg.Height >= 1 ? 295.0f / withImg.Width : 255.0f / withImg.Height;

                    withImg.Width = withImg.Width * scale;
                    withImg.Height = withImg.Height * scale;

                    withImg.VerticalAlignment = ShapeVerticalAlignment.Center;
                }
                catch (FileNotFoundException)
                {
                    par.AppendText("Изображение не найдено!");
                }
                catch (ArgumentException)
                {
                    par.AppendText("Неверный формат изображения!");
                }
            }
        }

        private void AddText(WTableCell cell, string mo)
        {
            cell.CellFormat.VerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Middle;
            var par = cell.AddParagraph();
            par.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
            var text = par.AppendText(mo);
            // todo Разобраться с Font
            //text.CharacterFormat.Font = new Font("Tahoma", 14);
        }
    }
}
