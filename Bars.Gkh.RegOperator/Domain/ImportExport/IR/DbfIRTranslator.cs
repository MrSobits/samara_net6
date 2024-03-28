namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;

    using NDbfReaderEx;

    using SocialExplorer.IO.FastDBF;

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

                using (var table = DbfTable.Open(data, Encoding.GetEncoding(866)))
                {
                    foreach (var row in table)
                    {
                        var model = new IRModel { ModelName = "dbfrow", Path = row.recNo.ToStr() };

                        var localRow = row;
                        row.columns.ForEach(x =>
                            model.PropertyBag.Add(new IRModelProperty
                            {
                                Name = x.name,
                                Value = localRow.GetValue(x),
                                Type = x.type
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
                        var rec = new DbfRecord(dbf.Header);

                        int i = 0;

                        foreach (var property in m.PropertyBag)
                        {
                            switch (dbf.Header[i].ColumnType)
                            {
                                case DbfColumn.DbfColumnType.Number:
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

        private IEnumerable<DbfColumn> CreateColumnDefinitions(IRModel model)
        {
            foreach (var kv in model.PropertyBag)
            {
                var convertType = Type.GetTypeCode(kv.Type);

                switch (convertType)
                {
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                        yield return new DbfColumn(kv.Name, DbfColumn.DbfColumnType.Number, kv.NLength, kv.DLength);
                        break;

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        yield return new DbfColumn(kv.Name, DbfColumn.DbfColumnType.Number, kv.NLength, kv.DLength);
                        break;

                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                        yield return new DbfColumn(kv.Name, DbfColumn.DbfColumnType.Integer);
                        break;
                    case TypeCode.String:
                    case TypeCode.Char:
                        yield return
                            new DbfColumn(
                                kv.Name,
                                DbfColumn.DbfColumnType.Character,
                                kv.NLength*2 /*кодировка UTF8, 2 байта на символ*/,
                                0);
                        break;
                    case TypeCode.Boolean:
                        yield return new DbfColumn(kv.Name, DbfColumn.DbfColumnType.Boolean);
                        break;
                    case TypeCode.DateTime:
                        yield return new DbfColumn(kv.Name, DbfColumn.DbfColumnType.Date);
                        break;
                    default:
                        yield return new DbfColumn(kv.Name, DbfColumn.DbfColumnType.Memo);
                        break;
                }
            }
        }

        private Stream CutIfLengthNotMatch(Stream stream)
        {
            try
            {
                var header = DbfTable.ReadDbfHeader(stream);

                // подсчет длины файла, которая должна быть
                var needLength = header.firstRecordPosition + (header.rowLength*header.recCount);

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