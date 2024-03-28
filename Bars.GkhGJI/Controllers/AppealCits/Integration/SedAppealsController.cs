namespace Bars.GkhGji.Controllers.Integration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using GkhGji.Integration.AppealCits;

    public class SedAppealsController : BaseController
    {
        public ActionResult SendAppeals(BaseParams @params)
        {
            var files = @params.Files;

            if (files.Count == 0)
            {
                return "Нет файлов".ToSedError();
            }

            try
            {
                var result = new SedIntegrator(Container).Import(files.Values.ToArray());

                return result.Data.Id.ToSedSuccess();
            }
            catch (Exception ex)
            {
                return ex.Message.ToSedError();
            }
        }

        private void FillTestValues(BaseParams @params)
        {
            var root = @"D:\TMP\project_files";

            foreach (var file in Directory.EnumerateFiles(root))
            {
                using (var reader = new BinaryReader(System.IO.File.OpenRead(file)))
                {
                    var fileName = Path.GetFileName(file);
                    @params.Files.Add(
                        new KeyValuePair<string, FileData>(
                            fileName,
                            new FileData(
                                Path.GetFileNameWithoutExtension(fileName),
                                Path.GetExtension(fileName),
                                reader.ReadBytes((int) reader.BaseStream.Length))));
                }
            }
        }
    }
}