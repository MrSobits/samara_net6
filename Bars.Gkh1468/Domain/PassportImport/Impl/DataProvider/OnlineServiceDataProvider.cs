namespace Bars.Gkh1468.Domain.PassportImport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using DomainService;
    using Entities;
    using Entities.Passport;
    using Enums;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Import;
    using Interfaces;
    using Newtonsoft.Json;
    using ProxyEntity;
    using ValueType = Enums.ValueType;
    using Ionic.Zip;
    using System.IO;
    using System.Security;
    using Bars.B4.Modules.FIAS;

    public class OnlineServiceDataProvider : IDynamicDataProvider
    {
        private IDomainService<Municipality> MunDomain { get; set; }
        private IDomainService<RealityObject> RoDomain { get; set; }
        private IDomainService<Contragent> ContragentDomain { get; set; }
        private IDomainService<HouseProviderPassport> PassportDomain { get; set; }
        private IDomainService<PassportStruct> StructDomain { get; set; }
        private IDomainService<MetaAttribute> MetaAttributeDomain { get; set; }
        private IDomainService<HouseProviderPassportRow> PassportRowDomain { get; set; } 
        private IHousePassportService PassportService { get; set; }
        private IDomainService<Fias> FiasDomain { get; set; }

        private string _templateUrl = "https://pp1468.ru/export/bars/{0}/{1}/{2}/{3}";

        private BaseParams _baseParams;

        private ILogImport _logger;

        private DynamicDictionary passportDict;

        private const int batchCount = 100;


        public OnlineServiceDataProvider(BaseParams baseParams, ILogImport logger, IWindsorContainer Container)
        {
            _baseParams = baseParams;
            _logger = logger;
            MunDomain = Container.Resolve<IDomainService<Municipality>>();
            RoDomain = Container.Resolve<IDomainService<RealityObject>>();
            ContragentDomain = Container.Resolve<IDomainService<Contragent>>();
            PassportDomain = Container.Resolve<IDomainService<HouseProviderPassport>>();
            StructDomain = Container.Resolve<IDomainService<PassportStruct>>();
            MetaAttributeDomain = Container.Resolve<IDomainService<MetaAttribute>>();
            PassportRowDomain = Container.Resolve<IDomainService<HouseProviderPassportRow>>();
            PassportService = Container.Resolve<IHousePassportService>();
            FiasDomain = Container.Resolve<IDomainService<Fias>>();
            passportDict = DynamicDictionary.Create();

        }

        public object GetData()
        {
            var dynamicData = GetDynamicData();

            var buildings = new List<object>();

            var index = 0;

            while (dynamicData != null)
            {
                buildings.AddRange(dynamicData.Get("building", new List<object>()));

                index += batchCount;

                dynamicData = GetDynamicData(index);
            }

            return buildings.Any() ? CollectData(buildings) : null;
        }

        private DynamicDictionary GetDynamicDictionary(List<object> list)
        {
            var dict = DynamicDictionary.Create();
            foreach (var str in list)
            {
                var s = (KeyValuePair<string, object>)str;
                dict.Add(s);
            }

            return dict;
        }

        private List<PassportWithAttributes> CollectData(List<object> buildings)
        {
            var month = _baseParams.Params.GetAs<int>("month");
            var year = _baseParams.Params.GetAs<int>("year");

            var result = new List<PassportWithAttributes>();

            foreach (var building in buildings)
            {
                var bld = building as DynamicDictionary;
                var once = false;
                if (bld == null){
                    //один дом. пришел список параметров, а не список словарей.
                    //родить из них словарь.
                    once = true;
                    bld = GetDynamicDictionary(buildings);
                }

                var box = bld.GetAs<string>("@box");
                var fraction = bld.GetAs<string>("@fraction");
                var letter = bld.GetAs<string>("@letter");
                var housing = bld.GetAs<string>("@housing");
                var number = bld.GetAs<string>("@number");
                var streetGuid = bld.GetAs<string>("@street_guid");

                var ro = RoDomain.GetAll()
                    .WhereIf(!housing.IsEmpty(), x => x.FiasAddress.Housing == housing)
                    .WhereIf(!letter.IsEmpty(), x => x.FiasAddress.Letter == letter)
                    .FirstOrDefault(x => x.FiasAddress.StreetGuidId == streetGuid && x.FiasAddress.House == number);

                if (ro == null){
                    _logger.Error("", string.Format("Не найден дом по адресу: ул.{0}, д.{1}!", streetGuid, number));
                    if (once){
                        break;
                    }
                    continue;
                }

                var suppliers = bld.Get("supplier", new List<object>());
                foreach (var supplier in suppliers)
                {
                    var splOnce = false;
                    var spl = supplier as DynamicDictionary;
                    if (spl == null){
                        splOnce = true;
                        spl = GetDynamicDictionary(suppliers);
                    }

                    var title = spl.GetAs<string>("@title");
                    var inn = spl.GetAs<string>("@inn");
                    var kpp = spl.GetAs<string>("@kpp");
                    var passportFiles = spl.Get("passport", new List<object>());

                    var contragent = this.ContragentDomain.GetAll()
                        .FirstOrDefault(x => x.Inn == inn && x.Kpp == kpp);

                    if (contragent == null){
                        _logger.Error("",string.Format("Ошибка: Не найден контрагент \"{0}\". ИНН: {1}, КПП: {2}", title, inn,kpp));
                        if (splOnce){
                            break;
                        }
                        continue;
                    }

                    foreach (var passportFile in passportFiles)
                    {
                        var paspOnce = false;
                        var pasp = passportFile as DynamicDictionary;
                        if (pasp == null){
                            paspOnce = true;
                            pasp = GetDynamicDictionary(passportFiles);
                        }

                        var fileName = pasp.GetAs<string>("@file");
                        var pasport = PassportDomain.GetAll().FirstOrDefault(x => 
                            x.Contragent.Id == contragent.Id 
                            && x.RealityObject.Id == ro.Id 
                            && x.ReportMonth == month 
                            && x.ReportYear == year);
                        
                        var structId = pasport != null ? pasport.PassportStruct.Id : 0;
                        if (pasport == null)
                        {
                            var isMkd = ro.TypeHouse.To1468RealObjType() == TypeRealObj.Mkd;
                            var passportStruct = StructDomain.GetAll()
                                .FirstOrDefault(
                                    x =>
                                        (isMkd && x.PassportType == PassportType.Mkd) ||
                                        (!isMkd && x.PassportType == PassportType.House));

                            if (passportStruct == null)
                            {
                                _logger.Error("", string.Format(
                                    "Ошибка: На указанный период в системе не создана структура паспорта c типом {0}!",
                                    isMkd ? "МКД" : "ОКИ"));
                                if (paspOnce)
                                {
                                    break;
                                }
                                continue;
                            }

                            structId = passportStruct.Id;

                            pasport = new HouseProviderPassport
                            {
                                HouseType = isMkd ? HouseType.Mkd : HouseType.House,
                                RealityObject = ro,
                                ReportMonth = month,
                                ReportYear = year,
                                Contragent = contragent,
                                PassportStruct = passportStruct,
                                HousePassport = PassportService.GetPassport(ro, year, month).Data as HousePassport
                            };
                        }
                        else
                        {
                            _logger.Error("", string.Format("Паспорт дома '{0}' контрагента '{1}' уже существует на период {2}/{3}", ro.Address,contragent.Name, month, year));
                            continue;
                        }

                        var attrs = this.MetaAttributeDomain.GetAll()
                            .Where(x => x.ParentPart.Struct.Id == structId || x.ParentPart.Parent.Struct.Id == structId)
                            .ToList();

                        var values = PassportRowDomain.GetAll()
                            .Where(x => x.ProviderPassport.Id == pasport.Id)
                            .ToArray();

                        var data = passportDict.Get(fileName, new XmlDocument());

                        result.Add(CollectPassportAttributes(data, attrs, values, pasport));

                        if (paspOnce){
                            break;
                        }
                    }
                    
                    if (splOnce){
                        break;
                    }
                }

                if (once){
                    break;
                }
            }
            return result;
        }

        private DynamicDictionary GetPassportNode(ref DynamicDictionary dd)
        {
            var paths = new[]{
                "C_ЭП_ОКИ",
                "C_ЭП_ЖД",
                "C_ЭП_МКД"
            };

            foreach (var xpath in paths){
                var data = dd.GetAs<DynamicDictionary>(xpath);
                if (data != null){
                    return data;
                }
            }

            return null;
        }

        private XmlNode GetValuableDataTag(XmlDocument doc)
        {
            var xpaths = new[]
            {
                "//C_ЭП_ОКИ",
                "//C_ЭП_ЖД",
                "//C_ЭП_МКД"
            };

            foreach (var xpath in xpaths)
            {
                var data = GetTagByXpath(doc, xpath);
                if (data != null)
                {
                    return data;
                }
            }

            return null;
        }

        private XmlNode GetTagByXpath(XmlDocument doc, string xpath)
        {
            return doc.SelectSingleNode(xpath);
        }

        private List<HouseProviderPassportRow> CollectAttributes(XmlNode data,
            ref List<MetaAttribute> attrs,
            ref HouseProviderPassportRow[] values,
            ref HouseProviderPassport pasport,
            ref StringBuilder sb)
        {
            var list = new List<HouseProviderPassportRow>();

            var localSb = new StringBuilder();

            if (data.HasChildNodes)
            {
                foreach (XmlNode chNode in data.ChildNodes)
                {
                    if (chNode.Attributes != null)
                    {
                        var val = (XmlAttribute) chNode.Attributes.GetNamedItem("значение");
                        if (val != null)
                        {
                            var metaAttr = attrs.FirstOrDefault(x => chNode.Name.Trim().TrimEnd('.') == x.IntegrationCode.Trim().TrimEnd('.'));
                            if (metaAttr != null)
                            {
                                var currentVal = values.FirstOrDefault(x => x.MetaAttribute.Id == metaAttr.Id);
                                if (currentVal == null)
                                {
                                    currentVal = new HouseProviderPassportRow
                                    {
                                        MetaAttribute = metaAttr,
                                        Value = val.Value,
                                        Passport = pasport
                                    };
                                }
                                else
                                {
                                    currentVal.Value = val.Value;
                                }

                                string msg;
                                if (!ValidateValue(currentVal, out msg))
                                {
                                    localSb.AppendLine(string.Format("Код интеграции: {0}. Ошибка: {1}",
                                        metaAttr.IntegrationCode, msg));
                                }

                                list.Add(currentVal);
                            }
                            else
                            {
                                sb.AppendLine(string.Format("Не найден атрибут с кодом интеграции: {0}", chNode.Name));
                            }
                        }

                        //добавить детские элементы
                        list.AddRange(CollectAttributes(chNode, ref attrs, ref values, ref pasport, ref sb));
                    }
                }
            }

            if (localSb.Length > 0)
            {
                sb.Append(localSb);
                _logger.Error("", sb.ToString());
            }

            return list;
        }

        private PassportWithAttributes CollectPassportAttributes(XmlDocument pasp,
            List<MetaAttribute> attrs,
            HouseProviderPassportRow[] values,
            HouseProviderPassport pasport)
        {
            var data = GetValuableDataTag(pasp);
            var sb = new StringBuilder();

            var result = new PassportWithAttributes();

            result.Rows.AddRange(CollectAttributes(data, ref attrs, ref values, ref pasport, ref sb));

            if (sb.Length > 0)
            {
                _logger.Error("", sb.ToString());
            }

            result.Passport = pasport;

            return result;
        }

        private DynamicDictionary GetDynamicData(int index = 0)
        {
            passportDict.Clear();
            var moId = _baseParams.Params.GetAs<long>("mo_id");
            var oktmo = MunDomain.Get(moId).Oktmo;
            var month = _baseParams.Params.GetAs<int>("month");
            var year = _baseParams.Params.GetAs<int>("year");

            var requestFormat = _templateUrl;
#if DEBUG
            WebRequest.DefaultWebProxy = new WebProxy("http://proxy:8080", true, null,
                new NetworkCredential("bg-guest", "ujcntdjq"));
#endif
            var url = string.Format(requestFormat, oktmo, month, year, index);
            byte[] response = { };

            using (var cl = new WebClient())
            {
                var oldCallback = ServicePointManager.ServerCertificateValidationCallback;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                try
                {
                    cl.Encoding = Encoding.UTF8;
                    response = cl.DownloadData(url);
                }
                catch (Exception exp)
                {
                    _logger.Error("Ошибка!", exp.Message);
					return null; //в случае ошибки прерываем бесконечный цикл
                }
                finally
                {
                    ServicePointManager.ServerCertificateValidationCallback = oldCallback;
                }
            }

            if (response.Length == 0)
            {
                return new DynamicDictionary();
            }

           var xDoc = new XmlDocument();

           using (var zipfileMemoryStream = new MemoryStream(response))
           {
               using (var zipFile = ZipFile.Read(zipfileMemoryStream))
               {
                   var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".xml") && !x.FileName.StartsWith("metadata")).ToList();
                   if (!zipEntries.Any() )
                   {
                       if (index == 0)
                       {
                           var msg = string.Format("По ОКТМО '{0}' на дату '{1}/{2}' портал не передал паспортов!", oktmo, month, year);
                           _logger.Error("", msg);
                       }
                       
                       return null;
                   }

                   foreach (var zipEntry in zipEntries)
                   {
                       using (var fileImport = new MemoryStream())
                       {
                           zipEntry.Extract(fileImport);

                           if (fileImport.CanSeek)
                           {
                               fileImport.Seek(0, SeekOrigin.Begin);
                           }

                           var fileArr = fileImport.ToArray();
                           using (var fileImport2 = new MemoryStream(fileArr))
                           {

                               var xmlDoc = new XmlDocument();

                               // игнорирование если загружен пустой документ
                               try
                               {
                                   xmlDoc.Load(fileImport2);
                                   if (!passportDict.ContainsKey(zipEntry.FileName))
                                   {
                                       passportDict.Add(zipEntry.FileName, xmlDoc);
                                   }
                               }
                               catch
                               {
                                   // ignored
                               }
                           }
                       }
                   }

                   var passpList = zipFile.FirstOrDefault(x => x.FileName.EndsWith(".xml") && x.FileName.StartsWith("metadata"));
                   if (passpList != null)
                   {
                       using (var fileImport = new MemoryStream())
                       {
                           passpList.Extract(fileImport);
                           var fileArr = fileImport.ToArray();
                           var fileImport2 = new MemoryStream(fileArr);
                           xDoc.Load(fileImport2);
                       }
                   }
               }
           }
           
            var dd = DynamicDictionary.FromJson(JsonConvert.SerializeXmlNode(xDoc));
            var buildingList = dd.GetAs<DynamicDictionary>("export_data");
            return buildingList;
        }

        private bool ValidateValue(HouseProviderPassportRow row, out string msg)
        {
            msg = string.Empty;
            if (row.MetaAttribute.Required && row.Value.IsEmpty())
            {
                msg = "Не заполнено значение обязательного аттрибута";
                return false;
            }

            var validType = true;
            switch (row.MetaAttribute.ValueType)
            {
                case ValueType.Decimal:
                    decimal d_tmp;
                    validType = decimal.TryParse(row.Value, out d_tmp);
                    break;
                case ValueType.Int:
                    int i_tmp;
                    validType = int.TryParse(row.Value, out i_tmp);
                    break;
            }

            if (!validType)
            {
                msg = "Неверный тип импортируемых данных";
                return false;
            }

            return true;
        }
    }
}