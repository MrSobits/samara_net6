namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Utils;
    using Bars.Gkh.RegOperator.Utils.DbfTypes;

    using DbfDataReader;
    
    using DbfRecord = DbfDataReader.DbfRecord;
    
    using DbfHeader = DbfDataReader.DbfHeader;

    // TODO: Проверить работу после смены библиотеки
    public class DbfIRTranslator : IIRTranslator
    {
        private List<IRModel> _rows;

        public IEnumerable<IRModel> Parse(Stream data)
        {
            if (_rows == null)
            {
                ParseInternal(data);
            }

            return _rows;
        }

        private void ParseInternal(Stream data)
        {
            _rows = new List<IRModel>();

            try
            {
                data = CutIfLengthNotMatch(data);

                using (var table = new DbfTable(data, Encoding.GetEncoding(866)))
                {
                    var dbfRecord = new DbfRecord(table);
                    var columnNames = table.Columns.Select(x => x.ColumnName).ToList();
                    int index = 0;
                    while (table.Read(dbfRecord))
                    {
                        var model = new IRModel { ModelName = "dbfrow", Path = index.ToStr() };

                        var localRow = dbfRecord;
                        table.Columns.ToList().ForEach(x =>
                            model.PropertyBag.Add(new IRModelProperty
                            {
                                Name = x.ColumnName,
                                Value = localRow.GetValueOrDefault(x.ColumnName, columnNames),
                                Type = localRow.GetValueOrDefault(x.ColumnName, columnNames).GetType()
                            }));

                        _rows.Add(model);
                    }
                }
            }
            finally
            {
                data.Close();
            }
        }

        public Stream FromModel(IRModel model, List<IRModelProperty> metaData)
        {
            var path = Path.GetTempFileName();
            var modelList = ExpandModel(model);
            var firstModel = modelList.Count == 1 ? new IRModel { PropertyBag = metaData } : modelList.First();

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                var dbf = new DbfFile(Encoding.GetEncoding(866));

                try
                {
                    dbf.Open(fs);

                    var columns = CreateColumnDefinitions(firstModel);

                    foreach (var dbfColumn in columns)
                    {
                        dbf.Header.AddColumn(dbfColumn);
                    }

                    foreach (var m in modelList)
                    {
                        var rec = new RegOperator.Utils.DbfTypes.DbfRecord(dbf.Header);

                        int i = 0;

                        foreach (var property in m.PropertyBag)
                        {
                            switch (dbf.Header[i].ColumnType)
                            {
                                case RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Number:
                                {
                                    // вообщем если сделать от decimal .ToString то число представляется в таком виде
                                    // "243,45" а DBF число с таким разделителем непонимает ему над оименно точку 
                                    rec[i] = property.Value.ToStr().Replace(',', '.');
                                    break;
                                }

                                default:
                                {
                                    rec[i] = property.Value.ToStr();
                                    break;
                                }
                            }

                            i++;
                        }

                        dbf.Write(rec);
                    }
                }
                finally
                {
                    dbf.Close();
                }
            }

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        private List<IRModel> ExpandModel(IRModel model)
        {
            if (model.Children.Any())
            {
                return model.Children;
            }

            return new List<IRModel> { model };
        }

        private IEnumerable<RegOperator.Utils.DbfTypes.DbfColumn> CreateColumnDefinitions(IRModel model)
        {
            foreach (var kv in model.PropertyBag)
            {
                var convertType = Type.GetTypeCode(kv.Type);

                switch (convertType)
                {
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                        yield return new RegOperator.Utils.DbfTypes.DbfColumn(kv.Name, RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Number, kv.NLength, kv.DLength);
                        break;

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        yield return new RegOperator.Utils.DbfTypes.DbfColumn(kv.Name, RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Number, kv.NLength, kv.DLength);
                        break;

                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                        yield return new RegOperator.Utils.DbfTypes.DbfColumn(kv.Name, RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Integer);
                        break;
                    case TypeCode.String:
                    case TypeCode.Char:
                        yield return
                            new RegOperator.Utils.DbfTypes.DbfColumn(
                                kv.Name,
                                RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Character,
                                kv.NLength*2 /*кодировка UTF8, 2 байта на символ*/,
                                0);
                        break;
                    case TypeCode.Boolean:
                        yield return new RegOperator.Utils.DbfTypes.DbfColumn(kv.Name, RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Boolean);
                        break;
                    case TypeCode.DateTime:
                        yield return new RegOperator.Utils.DbfTypes.DbfColumn(kv.Name, RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Date);
                        break;
                    default:
                        yield return new RegOperator.Utils.DbfTypes.DbfColumn(kv.Name, RegOperator.Utils.DbfTypes.DbfColumn.DbfColumnType.Memo);
                        break;
                }
            }
        }

        private Stream CutIfLengthNotMatch(Stream stream)
        {
            try
            {
                var header = new DbfHeader(stream);

                // подсчет длины файла, которая должна быть
                var needLength = header.HeaderLength;

				//после переделки на последовательню обработку мы всегда возвращаем текущий поток, закрываем его, и в любом цикле, где идет перебор элементов
				//мы опять попадаем сюда, но поток уже закрыт, в результате ObjectDisposedException, пока комментирую так как задача очень срочная, 
				//чтобы всегда возвращался и закрывался
				//новый поток, тут надо либо убрать последовательную обработку, либо доработать ее
				
				//// если длина, которая должна быть, совпадает с длиной потока, то все ок
				//if (stream.Length >= needLength && stream.Length <= (needLength + 1))
				//{
				//	return stream;
				//}

                // иначе кроем матом сбер за фигню в конце файла
                // и обрезаем ненужное окончание файла
                var bytes = stream.ReadAllBytes();

                var copyTo = new byte[needLength];

                Array.Copy(bytes, copyTo, needLength);

                return new MemoryStream(copyTo);
            }
            catch (IOException e)
            {
                /* Перехватываем исключения NDbfReader.
                 * Если пришло исключение со следующим текстом, то изменяем сообщение на более понятное
                 * "Not a DBF file! ('Code page mark' is not valid!)"
                 */
                if (e.Message.Contains("Code page mark"))
                {
                    throw new IOException("Неверная кодировка DBF файла.");
                }

                throw;
            }
        }
    }
}