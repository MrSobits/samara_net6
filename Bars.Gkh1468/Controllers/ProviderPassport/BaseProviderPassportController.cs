namespace Bars.Gkh1468.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh1468.DomainService;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class BaseProviderPassportController<T> : B4.Alt.DataController<T> where T : BaseProviderPassport
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;

        public BaseProviderPassportController(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public ActionResult GetPdf(BaseParams baseParams)
        {
            var pdfId = baseParams.Params.GetAs<long>("pdfId");

            byte[] data;
            if (pdfId == 0)
            {
                data = new byte[0];
            }
            else
            {
                using (var file = _fileManager.GetFile(_fileDomain.Get(pdfId)))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        file.CopyTo(tmpStream);
                        data = tmpStream.ToArray();
                    }
                }
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=passport.pdf");
            Response.Headers.Add("Content-Length", data.Length.ToString());
            return File(data, "application/pdf");
        }

        public virtual bool CheckPermissions(long id)
        {
            return false;
        }

        public ActionResult GetData(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");
            if (!this.CheckPermissions(id))
            {
                return new JsonNetResult(new
                                             {
                                                 message = "Нет прав для подписи.",
                                                 success = false
                                             });
            }

            if (id == 0)
            {
                return new JsonNetResult("Нет данных для отображения");
            }

            var sig = Resolve<ISignature<T>>();

            if (sig == null)
            {
                return new JsonNetResult("Нет данных для отображения");
            }

            using (var pdfStream = sig.GetPdfStream(id))
            {
                using (var xmlStream = sig.GetXmlStream(id))
                {
                    var xml = _fileManager.SaveFile(xmlStream, "xml.xml");

                    var str = Convert.ToBase64String(xmlStream.ToArray());

                    var pdf = _fileManager.SaveFile(pdfStream, "pdf.pdf");

                    return new JsonNetResult(new { success = true, data = new { dataToSign = str, xmlId = xml.Id, pdfId = pdf.Id } });
                }
            }
        }

        public ActionResult Sign(BaseParams baseParams)
        {
            var fm = Resolve<IFileManager>();
            var ds = Resolve<IDomainService<T>>();

            var id = baseParams.Params.GetAs<long>("Id");
            var xmlId = baseParams.Params.GetAs<long>("xmlId");
            var pdfId = baseParams.Params.GetAs<long>("pdfId");
            var sign = baseParams.Params.GetAs<string>("sign");
            var certificate = baseParams.Params.GetAs<string>("certificate");

            var entity = ds.Get(id);

            var message = string.Empty;

            using (var tr = Resolve<IDataTransaction>())
            {
                try
                {
                    entity.Xml = _fileDomain.Get(xmlId);
                    entity.Pdf = _fileDomain.Get(pdfId);
                    entity.Signature = fm.SaveFile("signature", "sig", Encoding.UTF8.GetBytes(sign));
                    entity.SignDate = DateTime.Now;
                    entity.Certificate = fm.SaveFile("certificate", "cer",
                        Encoding.UTF8.GetBytes("-----BEGIN CERTIFICATE-----" + certificate + "-----END CERTIFICATE-----"));
                    
                    ds.Update(entity);

                    var infoList = Container.ResolveAll<IStatefulEntitiesManifest>().SelectMany(x => x.GetAllInfo()).ToList();
                    var info = infoList.FirstOrDefault(x => x.Type == typeof(T));

                    if (info != null)
                    {
                        var stateCode = "Подписано";
                        var state =
                            Resolve<IRepository<State>>()
                                .GetAll()
                                .FirstOrDefault(
                                                x =>
                                                    x.TypeId == info.TypeId
                                                    && (x.Name == stateCode || x.Code == stateCode));

                        if (state != null)
                        {
                            Resolve<IStateProvider>()
                                .ChangeState(entity.Id, info.TypeId, state, "После подписания", false, true);
                        }
                        else
                        {
                            message = string.Format("Для паспорта не существует статус \"{0}\"", stateCode);
                        }
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return message.IsEmpty() ? JsonNetResult.Success : JsonNetResult.Failure(message);
        }


        public ActionResult DeleteDocs(BaseParams baseParams)
        {
            var xmlId = baseParams.Params.GetAs<long>("xmlId");
            var pdfId = baseParams.Params.GetAs<long>("pdfId");

            if (xmlId > 0)
            {
                _fileDomain.Delete(xmlId);
            }

            if (pdfId > 0)
            {
                _fileDomain.Delete(pdfId);
            }

            return JsSuccess();
        }
    }
}