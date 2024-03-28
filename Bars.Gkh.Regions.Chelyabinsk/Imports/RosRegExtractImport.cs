namespace Bars.Gkh.Regions.Chelyabinsk.Imports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using Entities;
    using Ionic.Zip;
    using Castle.Windsor;
    using B4.DataAccess;
    using Import.Impl;
    using Import;
    using Gkh.Enums.Import;
    using Dapper;
    using System.Xml;
    using Gkh.Domain;
    using System.Collections.Generic;

    public class RosRegExtractImport : GkhImportBase
    {
        #region Base
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportRosRegExtract"; }
        }

        public override string Name
        {
            get { return "Импорт выписок из Росреестра"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.RosRegExtract"; }
        }
        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }
            LogImport.ImportKey = this.Name;
            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;
        }
        #endregion
        #region Services
        public IDomainService<RosRegExtractBig> rreBigService { get; set; }
        public ISessionProvider SessionProvider { get; private set; }
        #endregion
        //Таймеры глобальные, чтобы не пересоздавать их каждый раз заново
        Stopwatch global_stopwatch;
        Stopwatch local_stopwatch;
        public override ImportResult Import(BaseParams baseParams)
        {
            local_stopwatch = new Stopwatch();
            global_stopwatch= new Stopwatch();
            global_stopwatch.Start();
            var file = baseParams.Files["FileImport"];
            InitLog(file.FileName);

            using (var zipfileMemoryStream = new MemoryStream(file.Data))
            {
                using (var zipFile = ZipFile.Read(zipfileMemoryStream))
                {
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".xml")).ToArray();

                    if (zipEntries.Length < 1)
                    {
                        LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");
                        return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                    }

                    this.ImportInternal(zipEntries, zipFile.Name);
                }
            }
            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;
            global_stopwatch.Stop();
            return new ImportResult(statusImport, string.Format("time elapsed: {0} ms;", global_stopwatch.ElapsedMilliseconds));
        }  
        private void ImportInternal(ZipEntry[] zipEntries, string archiveName)
        {
            
            int indicator = 0;
            using (var zipfileMemoryStream = new MemoryStream())
            {
                var readOptions = new ReadOptions();
                readOptions.Encoding = Encoding.GetEncoding(866);
                foreach (ZipEntry zipentry in zipEntries)
                {
                    createExtractBig(zipentry);
                    indicator++;
                    Indicate(80 * indicator / zipEntries.Length, $"{indicator} выписок из {zipEntries.Length} загружено");
                }
                createExtractOwners();
            }
            Indicate(95, "Завершение импорта");
        }

        private void createExtractBig(ZipEntry zipentry)
        {
            var internal_stopwatch = new Stopwatch(); //Таймер цикла
            internal_stopwatch.Start();
            //Исправление кодировки для кириллических имен файлов
            string filename = Encoding.GetEncoding(866).GetString(Encoding.GetEncoding(437).GetBytes(zipentry.FileName));
            try
            {
                Stream s = new MemoryStream();
                zipentry.Extract(s);
                s.Seek(0, SeekOrigin.Begin);

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(s);
                XmlElement xRoot = xDoc.DocumentElement;

                XmlNameTable nametable = new NameTable();
                XmlNamespaceManager xmlnameman = new XmlNamespaceManager(nametable);
                xmlnameman.AddNamespace("base", "urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1");
                xmlnameman.AddNamespace("adrs", "urn://x-artefacts-rosreestr-ru/commons/complex-types/address-output/4.0.1");

                XmlNode cadastral_number = xRoot.SelectSingleNode("//*/@CadastralNumber");
                List<RosRegExtractBig> list = rreBigService.GetAll().Where(x => x.CadastralNumber == cadastral_number.InnerText).ToList();
                
                //Берем имеющуюся запись, если такой кадастровый номер существует, в противном случае создаем новую запись
                RosRegExtractBig rreb = (list.Count>=1) ? list.FirstOrDefault() : new RosRegExtractBig();

                rreb.CadastralNumber = (cadastral_number != null) ? cadastral_number.InnerText : null;
                
                XmlNode extract_number = xRoot.SelectSingleNode("//*/@ExtractNumber");
                rreb.ExtractNumber = (extract_number != null) ? extract_number.InnerText : null;
                
                XmlNode extract_date = xRoot.SelectSingleNode("//*/@ExtractDate");
                rreb.ExtractDate = (extract_date != null) ? extract_date.InnerText : null;
                
                XmlNode extract_area = xRoot.SelectSingleNode("//base:Flat/base:Area/text()",xmlnameman);
                rreb.RoomArea = (extract_area != null) ? extract_area.InnerText : null;

                { 
                    //Собираем адрес
                    StringBuilder address = new StringBuilder();
                    if (xRoot.SelectSingleNode("//adrs:City", xmlnameman) != null)
                    {
                        address.Append(xRoot.SelectSingleNode("//adrs:City/@Type", xmlnameman)?.InnerText ?? "");
                        address.Append(". ");
                        address.Append(xRoot.SelectSingleNode("//adrs:City/@Name", xmlnameman)?.InnerText ?? "");
                        address.Append(", ");
                    }
                    if (xRoot.SelectSingleNode("//adrs:Street", xmlnameman) != null)
                    {
                        address.Append(xRoot.SelectSingleNode("//adrs:Street/@Type", xmlnameman)?.InnerText ?? "");
                        address.Append(". ");
                        address.Append(xRoot.SelectSingleNode("//adrs:Street/@Name", xmlnameman)?.InnerText ?? "");
                        address.Append(", ");
                    }
                    if (xRoot.SelectSingleNode("//adrs:Level1", xmlnameman) != null)
                    {
                        address.Append(xRoot.SelectSingleNode("//adrs:Level1/@Type", xmlnameman)?.InnerText ?? "");
                        address.Append(". ");
                        address.Append(xRoot.SelectSingleNode("//adrs:Level1/@Value", xmlnameman)?.InnerText ?? "");
                        address.Append(", ");
                    }
                    if (xRoot.SelectSingleNode("//adrs:Apartment", xmlnameman) != null)
                    {
                        address.Append(xRoot.SelectSingleNode("//adrs:Apartment/@Type", xmlnameman)?.InnerText ?? "");
                        address.Append(". ");
                        address.Append(xRoot.SelectSingleNode("//adrs:Apartment/@Value", xmlnameman)?.InnerText ?? "");
                    }
                    rreb.Address = address.ToString();
                }

                rreBigService.SaveOrUpdate(rreb);

                //Прикрепление XML
                attachXML(extractXML(zipentry), rreb.Id);

                LogImport.Info(filename + " - " + internal_stopwatch.ElapsedMilliseconds + "мс", "Импортировано успешно", "");
                LogImport.CountAddedRows++;
            }
            catch (Exception e)
            {
                LogImport.Error(filename + " - " + internal_stopwatch.ElapsedMilliseconds + "мс", e.Message, "");
            }
        }

        private void createExtractOwners()
        {
            local_stopwatch.Restart();
            Indicate(80, "Создание списка собственников");
            Debug.WriteLine($@"IMPORT SQL GENERATING OWNERS BEGIN");
            
            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            
            var connection = StatelessSession.Connection;
            //Вызов функции пересоздания списка собственников
            var sql = $@"SELECT import.rosregextractbigownersgenerate()";
            connection.Execute(sql);
            connection.Close();
            connection.Dispose();
            Container.Release(StatelessSession);
            local_stopwatch.Stop();
            Debug.WriteLine($@"IMPORT SQL GENERATING OWNERS: {local_stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Прикрепление XML-файла к записи описания
        /// </summary>
        /// <param name="xmlstring">XML</param>
        /// <param name="desc_id">Id записи</param>
        private void attachXML(string xmlstring, long desc_id)
        {
            local_stopwatch.Restart();
            Debug.WriteLine($@"IMPORT SQL INSERT XML BEGIN");
            //Вставка прямым запросом, т.к. нхибернейт не желает работать с типом данных xml
            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            
            var connection = StatelessSession.Connection;

            var sql = $@"UPDATE import.rosregextractbig
                                SET XML='{xmlstring}'
                                WHERE ID={desc_id}";
            connection.Execute(sql);
            connection.Close();
            connection.Dispose();
            Container.Release(StatelessSession);
            local_stopwatch.Stop();
            Debug.WriteLine($@"IMPORT SQL INSERT: {local_stopwatch.ElapsedMilliseconds} ms");
            
        }
        /// <summary>
        /// Получение XML-файла из записи архива
        /// </summary>
        /// <param name="zipentry">Архивная запись</param>
        /// <returns></returns>
        private string extractXML(ZipEntry zipentry)
        {
            local_stopwatch.Restart();
            Debug.WriteLine($@"IMPORT XML EXTRACT BEGIN");
            var ms = new MemoryStream();
            zipentry.Extract(ms);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var xml = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            
            local_stopwatch.Stop();
            Debug.WriteLine($@"IMPORT XML EXTRACT: {local_stopwatch.ElapsedMilliseconds} ms");

            return xml;
        }        
    }
    public static class XMLPolyArrHelper
    {
        public static TResult GetItem<TIDentifier, TResult>(TResult[] items, TIDentifier[] itemIdentifiers, TIDentifier itemIdentifier)
        {
            if (itemIdentifiers == null)
            {
                Debug.Assert(items == null);
                return default(TResult);
            }
            Debug.Assert(items.Length == itemIdentifiers.Length);
            var i = Array.IndexOf(itemIdentifiers, itemIdentifier);
            if (i < 0)
                return default(TResult);
            return items[i];
        }
    }
}
