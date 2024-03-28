namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Bars.B4.Utils;

    public class XmlIRTranslator : IIRTranslator
    {
        public IEnumerable<IRModel> Parse(Stream data)
        {
            XDocument document = null;

            if (data != null && data.Length > 0)
            {
                try
                {
                    data.Position = 0;
                    document = XDocument.Load(data);
                }
                catch
                {
                    // ignored
                }
            }

            var result = new List<IRModel>();

            if (document == null)
            {
                return result;
            }

            var root = document.Root;

            if (root == null)
            {
                return result;
            }

            var pathMap = new Dictionary<string, int>();
            foreach (var xElement in root.Elements())
            {
                var model = new IRModel
                {
                    ModelName = xElement.Name.ToString(),
                    Path = xElement.Name + "(Запись {0})".FormatUsing(GetElementDepth(xElement, pathMap))
                };

                FillModel(xElement, model);

                result.Add(model);
            }

            return result;
        }

        public Stream FromModel(IRModel model, List<IRModelProperty> metaData)
        {
            if (model == null)
            {
                return new MemoryStream();
            }

            var xDoc = new XDocument(new XElement(model.ModelName, model.PropertyBag.Select(CreateElement), model.Children.Select(CreateChildren)));

            var ms = new MemoryStream();

            xDoc.Save(ms);

            return ms;
        }

        #region Private methods

        private object CreateChildren(IRModel model)
        {
            return new XElement(model.ModelName, model.PropertyBag.Select(CreateElement), model.Children.Select(CreateChildren));
        }

        private object CreateElement(IRModelProperty property)
        {
            return new XElement(property.Name, property.Value);
        }

        private void FillModel(XElement xElement, IRModel model)
        {
            foreach (var x in xElement.Attributes())
            {
                model.PropertyBag.Add(new IRModelProperty { Name = x.Name.ToString(), Value = x.Value, Type = typeof (string) });
            }

            if (!xElement.Elements().Any())
            {
                model.PropertyBag.Add(new IRModelProperty { Name = xElement.Name.ToString(), Value = xElement.Value, Type = typeof (string) });
            }

            var pathMap = new Dictionary<string, int>();
            foreach (var element in xElement.Elements())
            {
                if (!element.Elements().Any())
                {
                    model.PropertyBag.Add(new IRModelProperty { Name = element.Name.ToString(), Value = element.Value, Type = typeof (string) });
                }
                else
                {
                    var childModel = new IRModel
                    {
                        ModelName = element.Name.ToString(),
                        Path = model.Path + "/" + element.Name + "(Запись {0})".FormatUsing(GetElementDepth(element, pathMap))
                    };

                    model.Children.Add(childModel);

                    FillModel(element, childModel);
                }
            }
        }

        private int GetElementDepth(XElement xElement, Dictionary<string, int> pathMap)
        {
            if (pathMap.ContainsKey(xElement.Name.ToString()))
                return ++pathMap[xElement.Name.ToString()];

            pathMap[xElement.Name.ToString()] = 1;

            return 1;
        }

        #endregion
    }
}