using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh1468.Enums;
using Castle.Windsor;
using Newtonsoft.Json;
using ValueType = Bars.Gkh1468.Enums.ValueType;

namespace Bars.Gkh1468.PassportStructExport.Impl
{
    using System.IO;
    using B4;
    using Entities;

    public class PassportStructExporter : IPassportStructExporter
    {
        public IWindsorContainer Container { get; set; }

        public string Version
        {
            get { return "1"; }
        }

        public IDataResult Import(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var serilalizer = new JsonSerializer();
                var exportData = (ExportData)serilalizer.Deserialize(streamReader, typeof(ExportData));

                var pstructDomain = Container.Resolve<IDomainService<PassportStruct>>();
                var partDomain = Container.Resolve<IDomainService<Part>>();
                var attrDomain = Container.Resolve<IDomainService<MetaAttribute>>();
                var umDomain = Container.Resolve<IDomainService<UnitMeasure>>();
                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        Save(exportData.PassportStruct, pstructDomain, partDomain, attrDomain, umDomain);
                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                string.Format(
                                    "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                    e.Message,
                                    e.StackTrace),
                                exc);
                        }

                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }
            return new BaseDataResult();
        }

        public IDataResult Export(PassportStruct passportStruct)
        {
            var memoryStream = new MemoryStream();
            var partDomain = Container.Resolve<IDomainService<Part>>();
            var attrDomain = Container.Resolve<IDomainService<MetaAttribute>>();

            var structProxy = new PassportStructProxy(passportStruct);

            Expand(passportStruct, structProxy, partDomain, attrDomain);

            var export = new ExportData()
            {
                PassportStruct = structProxy,
                Version = Version
            };

            var serializer = new JsonSerializer();
            var jsonTextWriter = new JsonTextWriter(new StreamWriter(memoryStream));
            serializer.Serialize(jsonTextWriter, export);
            jsonTextWriter.Flush();
            memoryStream.Position = 0;

            return new BaseDataResult
            {
                Data = memoryStream,
                Success = true
            };
        }

        private class ExportData
        {
            public PassportStructProxy PassportStruct { get; set; }

            public string Version { get; set; }

        }

        private void Save(PassportStructProxy proxy, IDomainService<PassportStruct> structDomain,
            IDomainService<Part> partDomain, IDomainService<MetaAttribute> attrDomain, IDomainService<UnitMeasure> umDomain)
        {
            var passpStruct = new PassportStruct()
            {
                Name = proxy.Name,
                PassportType = proxy.PassportType,
                ValidFromMonth = proxy.ValidFromMonth,
                ValidFromYear = proxy.ValidFromYear
            };

            structDomain.Save(passpStruct);

            foreach (var partProxy in proxy.Parts)
            {
                Save(partProxy, partDomain, attrDomain, umDomain, passpStruct);
            }
        }

        private void Save(PartProxy proxy, IDomainService<Part> partDomain, IDomainService<MetaAttribute> attrDomain,
            IDomainService<UnitMeasure> umDomain, PassportStruct parentStruct = null, Part parentPart = null)
        {
            var part = new Part()
            {
                Code = proxy.Code,
                IntegrationCode = proxy.IntegrationCode,
                Name = proxy.Name,
                OrderNum = proxy.OrderNum,
                Pku = proxy.Pku,
                Pr = proxy.Pr,
                Uo = proxy.Uo,
                Struct = parentStruct,
                Parent = parentPart
            };

            partDomain.Save(part);

            foreach (var partProxy in proxy.Children)
            {
                Save(partProxy, partDomain, attrDomain, umDomain, parentPart: part);
            }

            foreach (var attrProxy in proxy.MetaAttributes)
            {
                Save(attrProxy, attrDomain, umDomain, part);
            }
        }

        private void Save(MetaAttributeProxy proxy, IDomainService<MetaAttribute> attrDomain,
            IDomainService<UnitMeasure> umDomain, Part parentPart = null, MetaAttribute parentAttr = null)
        {
            var attr = new MetaAttribute
            {
                Code = proxy.Code,
                OrderNum = proxy.OrderNum,
                Name = proxy.Name,
                Type = proxy.Type,
                ValueType = proxy.ValueType,
                DictCode = proxy.DictCode,
                ValidateChilds = proxy.ValidateChilds,
                GroupText = proxy.GroupText,
                IntegrationCode = proxy.IntegrationCode,
                DataFillerCode = proxy.DataFillerCode,
                MaxLength = proxy.MaxLength,
                MinLength = proxy.MinLength,
                Pattern = proxy.Pattern,
                Exp = proxy.Exp,
                Required = proxy.Required,
                AllowNegative = proxy.AllowNegative,
                UseInPercentCalculation = proxy.UseInPercentCalculation,
                Parent = parentAttr,
                ParentPart = parentPart,
                UnitMeasure = GetOrCreate(proxy.UnitMeasure, umDomain)
            };

            attrDomain.Save(attr);
            foreach (var child in proxy.Children)
            {
                Save(child, attrDomain, umDomain, parentPart, attr);
            }
        }

        private UnitMeasure GetOrCreate(UnitMeasureProxy proxy, IDomainService<UnitMeasure> umDomain)
        {
            var um =
                umDomain.GetAll()
                    .FirstOrDefault(
                        x =>
                            x.Description == proxy.Description && x.Name == proxy.Name && x.ShortName == proxy.ShortName);
            if (um == null)
            {
                um = new UnitMeasure()
                {
                    Name = proxy.Name,
                    Description = proxy.Description,
                    ShortName = proxy.ShortName
                };

                umDomain.Save(um);
            }

            return um;
        }

        private class PassportStructProxy
        {
            public PassportStructProxy(PassportStruct passportStruct)
            {
                if (passportStruct != null)
                {
                    Name = passportStruct.Name;
                    PassportType = passportStruct.PassportType;
                    ValidFromMonth = passportStruct.ValidFromMonth;
                    ValidFromYear = passportStruct.ValidFromYear;
                }

            }

            private IList<PartProxy> _parts = new List<PartProxy>();
            public IList<PartProxy> Parts
            {
                get { return _parts; }
                set { _parts = value; }
            }
            public string Name { get; set; }
            public int ValidFromMonth { get; set; }
            public int ValidFromYear { get; set; }
            public PassportType PassportType { get; set; }
        }

        private class PartProxy
        {
            public PartProxy(Part part)
            {
                if (part != null)
                {
                    Code = part.Code;
                    IntegrationCode = part.IntegrationCode;
                    Name = part.Name;
                    OrderNum = part.OrderNum;
                    Pku = part.Pku;
                    Pr = part.Pr;
                    Uo = part.Uo;
                }
            }

            private IList<PartProxy> _childs = new List<PartProxy>();
            private IList<MetaAttributeProxy> _attributes = new List<MetaAttributeProxy>();
            public string Code { get; set; }
            public int OrderNum { get; set; }
            public string Name { get; set; }
            public bool Uo { get; set; }
            public bool Pku { get; set; }
            public bool Pr { get; set; }
            public string IntegrationCode { get; set; }
            public IList<MetaAttributeProxy> MetaAttributes
            {
                get { return _attributes; }
                set { _attributes = value; }
            }
            public IList<PartProxy> Children
            {
                get { return _childs; }
                set { _childs = value; }
            }
        }

        private class MetaAttributeProxy
        {
            public MetaAttributeProxy(MetaAttribute attr)
            {
                if (attr != null)
                {
                    Code = attr.Code;
                    OrderNum = attr.OrderNum;
                    Name = attr.Name;
                    Type = attr.Type;
                    ValueType = attr.ValueType;
                    DictCode = attr.DictCode;
                    ValidateChilds = attr.ValidateChilds;
                    GroupText = attr.GroupText;
                    IntegrationCode = attr.IntegrationCode;
                    DataFillerCode = attr.DataFillerCode;
                    MaxLength = attr.MaxLength;
                    MinLength = attr.MinLength;
                    Pattern = attr.Pattern;
                    Exp = attr.Exp;
                    Required = attr.Required;
                    AllowNegative = attr.AllowNegative;
                    UseInPercentCalculation = attr.UseInPercentCalculation;
                    UnitMeasure = new UnitMeasureProxy(attr.UnitMeasure);
                }
            }

            private IList<MetaAttributeProxy> _childs = new List<MetaAttributeProxy>();

            public string Code { get; set; }
            public int OrderNum { get; set; }
            public string Name { get; set; }
            public MetaAttributeType Type { get; set; }
            public ValueType ValueType { get; set; }
            public string DictCode { get; set; }
            public bool ValidateChilds { get; set; }
            public IList<MetaAttributeProxy> Children
            {
                get { return _childs; }
                set { _childs = value; }
            }
            public string GroupText { get; set; }
            public UnitMeasureProxy UnitMeasure { get; set; }
            public string IntegrationCode { get; set; }
            public string DataFillerCode { get; set; }
            public int MaxLength { get; set; }
            public int MinLength { get; set; }
            public string Pattern { get; set; }
            public int Exp { get; set; }
            public bool Required { get; set; }
            public bool AllowNegative { get; set; }
            public bool UseInPercentCalculation { get; set; }
        }

        private class UnitMeasureProxy
        {
            public UnitMeasureProxy(UnitMeasure um)
            {
                if (um != null)
                {
                    Name = um.Name;
                    ShortName = um.ShortName;
                    Description = um.Description;
                }

            }

            public string Name { get; set; }
            public string ShortName { get; set; }
            public string Description { get; set; }

        }

        private void Expand(PassportStruct passportStruct, PassportStructProxy proxy, IDomainService<Part> partDomain, IDomainService<MetaAttribute> metaAttrDomain)
        {
            var parts = partDomain.GetAll().Where(x => x.Struct.Id == passportStruct.Id).ToList();
            foreach (var part in parts)
            {
                var partProxy = new PartProxy(part);
                Expand(part, partProxy, partDomain, metaAttrDomain);
                proxy.Parts.Add(partProxy);
            }
        }

        private void Expand(Part part, PartProxy proxy, IDomainService<Part> partDomain,
            IDomainService<MetaAttribute> metaAttrDomain)
        {
            var childParts = partDomain.GetAll().Where(x => x.Parent.Id == part.Id).ToList();
            var attrs = metaAttrDomain.GetAll().Where(x => x.ParentPart.Id == part.Id && x.Parent == null).ToList();
            foreach (var child in childParts)
            {
                var partProxy = new PartProxy(part);
                Expand(child, partProxy, partDomain, metaAttrDomain);
                proxy.Children.Add(partProxy);
            }

            foreach (var attr in attrs)
            {
                var attrProxy = new MetaAttributeProxy(attr);
                Expand(attr, attrProxy, metaAttrDomain, part.Id);
                proxy.MetaAttributes.Add(attrProxy);
            }

        }

        private void Expand(MetaAttribute metaAttribute, MetaAttributeProxy proxy,
            IDomainService<MetaAttribute> metaAttrDomain, long partId)
        {
            var childs = metaAttrDomain.GetAll().Where(x => x.Parent.Id == metaAttribute.Id && x.ParentPart.Id == partId).ToList();
            foreach (var attr in childs)
            {
                var attrProxy = new MetaAttributeProxy(attr);
                Expand(attr, attrProxy, metaAttrDomain, partId);
                proxy.Children.Add(attrProxy);
            }
        }
    }
}
