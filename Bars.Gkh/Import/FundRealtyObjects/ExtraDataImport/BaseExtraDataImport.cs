namespace Bars.Gkh.Import.FundRealtyObjects.Impl
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public abstract class BaseExtraDataImport
    {
        protected List<string> FieldNames;

        protected abstract void InitFields();

        protected abstract void AddData(ExtraDataProxy extraData, Dictionary<string, string> data);

        public void Import(MemoryStream memoryStream, Dictionary<string, ExtraDataProxy> extraDataDict)
        {
            this.InitFields();

            if (this.FieldNames == null)
            {
                return;
            }

            var dictHeaders = this.FieldNames.ToDictionary(x => x, y => -1);
            
            using (var sr = new StreamReader(memoryStream, Encoding.GetEncoding(1251)))
            {
                //считывание заголовков
                var headers = sr.ReadLine().Split(';').Select(x => x.Trim('"')).ToArray();

                for (int i = 0; i < headers.Length; i++)
                {
                    if (dictHeaders.ContainsKey(headers[i]))
                    {
                        dictHeaders[headers[i]] = i;
                    }
                }

                if (!dictHeaders.Values.Any(x => x > 0))
                {
                    return;
                }

                int rowNumber = 1;

                //читаем данные
                while (!sr.EndOfStream)
                {
                    rowNumber++;
                    var line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var row = line.Split(';').Select(x => x ?? "").ToArray();

                    if (row.Length == 0)
                    {
                        continue;
                    }

                    var federalNumber = row[0];

                    if (string.IsNullOrEmpty(federalNumber))
                    {
                        continue;
                    }

                    var data = dictHeaders.Where(x => x.Value != -1).ToDictionary(x => x.Key, x => row[x.Value]);

                    ExtraDataProxy extraData;
                    if (extraDataDict.ContainsKey(federalNumber))
                    {
                        extraData = extraDataDict[federalNumber];
                    }
                    else
                    {
                        extraData = new ExtraDataProxy();
                        extraDataDict[federalNumber] = extraData;
                    }

                    AddData(extraData, data);
                }
            }
        }
    }
}