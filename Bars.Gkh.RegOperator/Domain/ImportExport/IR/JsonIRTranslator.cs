namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Bars.B4.Utils;

    using Newtonsoft.Json.Linq;

    public class JsonIRTranslator : IIRTranslator
    {
        public IEnumerable<IRModel> Parse(Stream data)
        {
            var token = JToken.Parse(new StreamReader(data).ReadToEnd());

            switch (token.Type)
            {
                case JTokenType.Array:
                    return ReadArray(token);
                case JTokenType.Object:
                    return ReadObject(token);
                default:
                    throw new NotImplementedException();
            }
        }

        private List<IRModel> ReadObject(JToken token)
        {
            var jObj = token as JObject;

            if (jObj == null)
            {
                return new List<IRModel>();
            }

            var model = new IRModel { ModelName = "AnonymousType" };

            FillModel(jObj, model);

            return new List<IRModel> { model };
        }

        private void FillModel(JToken jObj, IRModel model)
        {
            foreach (var prop in jObj.Children())
            {
                switch (prop.Type)
                {
                    case JTokenType.Boolean:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<bool>());
                        continue;
                    case JTokenType.Bytes:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<byte[]>());
                        continue;
                    case JTokenType.Date:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<DateTime>());
                        continue;
                    case JTokenType.Float:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<float>());
                        continue;
                    case JTokenType.Guid:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<Guid>());
                        continue;
                    case JTokenType.Integer:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<long>());
                        continue;
                    case JTokenType.Null:
                        AddValue(model, jObj.As<JProperty>().Name, null);
                        continue;
                    case JTokenType.String:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<string>());
                        continue;
                    case JTokenType.TimeSpan:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<TimeSpan>());
                        continue;
                    case JTokenType.Undefined:
                        AddValue(model, jObj.As<JProperty>().Name, null);
                        continue;
                    case JTokenType.Uri:
                        AddValue(model, jObj.As<JProperty>().Name, prop.Value<Uri>());
                        continue;
                    case JTokenType.Property:
                        FillModel(prop, model);
                        continue;
                    case JTokenType.Array:
                    case JTokenType.Object:
                        var subModel = new IRModel();
                        model.Children.Add(subModel);
                        FillModel(prop, subModel);
                        continue;
                }
            }
        }

        private static void AddValue(IRModel model, string name, object value)
        {
            model.PropertyBag.Add(new IRModelProperty
            {
                Name = name,
                Type = value.With(x => x.GetType()),
                Value = value
            });
        }

        private List<IRModel> ReadArray(JToken token)
        {
            var result = new List<IRModel>();

            foreach (var child in token.Children())
            {
                var model = new IRModel();
                FillModel(child, model);

                result.Add(model);
            }

            return result;
        }

        public Stream FromModel(IRModel model, List<IRModelProperty> metaData)
        {
            var children = model.Children;

            var jArray = new JArray();


            foreach (var irModel in children)
            {
                var jObj = new JObject();

                foreach (var prop in irModel.PropertyBag)
                {
                    switch (prop.Type.Name)
                    {
                        case "Decimal":
                            jObj[prop.Name] = prop.Value.ToDecimal();
                            break;
                        default:
                            jObj[prop.Name] = prop.Value.ToStr();
                            break;
                    }
                }

                jArray.Add(jObj);
            }

            var result = new MemoryStream(Encoding.UTF8.GetBytes(jArray.ToString()));

            return result;
        }
    }
}