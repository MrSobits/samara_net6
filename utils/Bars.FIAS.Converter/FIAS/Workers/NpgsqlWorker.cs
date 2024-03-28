namespace Bars.FIAS.Converter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Npgsql;

    public class NpgsqlWorker : IDbWorker
    {
        public BackgroundWorker BackgroundWorker { get; set; }

        private string ConnectionString { get; }

        private string DefaultFiasTableName => "b4_fias";
        private string DefaultFiasHouseTableName => "b4_fias_house";

        public NpgsqlWorker(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <inheritdoc />
        public bool TestConnection()
        {
            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"select count(1) from {this.DefaultFiasTableName};";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return reader.GetValue(0) != null;
                    }
                }
            }

            return false;
        }

        /// <inheritdoc />
        public void InsertRecords(IList<FiasRecord> fiasRecords, IList<FiasHouseRecord> fiasHouseRecords)
        {
            this.CopyFiasRecord(fiasRecords);
            this.CopyFiasHouseRecord(fiasHouseRecords);
        }

        /// <inheritdoc />
        public void UpdateFiasRecords(IList<FiasRecord> fiasRecords, IList<FiasHouseRecord> fiasHouseRecords)
        {
            this.CopyFiasRecord(fiasRecords, true);
            this.CopyFiasHouseRecord(fiasHouseRecords, true);
        }

        /// <inheritdoc />
        public void DeleteRecords()
        {
            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM {this.DefaultFiasTableName} WHERE TYPERECORD = 10;\n" +
                        $"DELETE FROM {this.DefaultFiasHouseTableName} WHERE TYPE_RECORD = 10;";
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <inheritdoc />
        public Dictionary<string, FiasRecord> GetCurrentFiasRecords()
        {
            var result = new Dictionary<string, FiasRecord>();

            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //получаем id только записей ФИАС
                    command.CommandText = $"SELECT ID, AOID, UPDATEDATE FROM {this.DefaultFiasTableName} WHERE TYPERECORD = 10;";
                    var reader = command.ExecuteReader();
                    while (reader.Read() && !this.BackgroundWorker.CancellationPending)
                    {
                        var id = (int)reader.GetValue(0);
                        var aoid = reader.GetString(1);
                        var updateDate = reader.GetDateTime(2);
                        if (!result.ContainsKey(aoid))
                        {
                            result.Add(aoid, new FiasRecord()
                            {
                                Id = id,
                                UpdateDate = updateDate,
                                AOId = aoid
                            });
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public Dictionary<string, FiasHouseRecord> GetCurrentFiasHouseRecords()
        {
            var result = new Dictionary<string, FiasHouseRecord>();

            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //получаем id только записей ФИАС
                    command.CommandText = $"SELECT ID, HOUSE_ID, UPDATE_DATE FROM {this.DefaultFiasHouseTableName} WHERE TYPE_RECORD = 10;";
                    var reader = command.ExecuteReader();
                    while (reader.Read() && !this.BackgroundWorker.CancellationPending)
                    {
                        var id = (int)reader.GetValue(0);
                        var houseid = reader.GetGuid(1).ToString();
                        var updateDate = reader.GetDateTime(2);
                        if (!result.ContainsKey(houseid))
                        {
                            result.Add(houseid, new FiasHouseRecord()
                            {
                                Id = id,
                                UpdateDate = updateDate,
                                HouseId = houseid
                            });
                        }
                    }
                }
            }

            return result;
        }

        private void CopyFiasRecord(IList<FiasRecord> records, bool update = false)
        {
            if (this.BackgroundWorker.CancellationPending)
            {
                return;
            }

            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var tmpTable = $"tmp_{this.DefaultFiasTableName}";

                    if (update)
                    {
                        command.CommandText = $"CREATE TEMP TABLE {tmpTable} " +
                            $"AS SELECT * FROM {this.DefaultFiasTableName} LIMIT 0;\n" +

                            $"COMMIT;\n" +

                            $"{this.GetCopyFiasTableCmd(tmpTable)}\n" +

                            $"UPDATE {this.DefaultFiasTableName} AS t1 " +
                            "SET AOGUID = t2.AOGUID, " +
                            "FORMALNAME = t2.FORMALNAME, " +
                            "REGIONCODE = t2.REGIONCODE, " +
                            "AUTOCODE = t2.AUTOCODE, " +
                            "AREACODE = t2.AREACODE, " +
                            "CITYCODE = t2.CITYCODE, " +
                            "CTARCODE = t2.CTARCODE, " +
                            "PLACECODE = t2.PLACECODE, " +
                            "STREETCODE = t2.STREETCODE, " +
                            "EXTRCODE = t2.EXTRCODE, " +
                            "SEXTCODE = t2.SEXTCODE, " +
                            "OFFNAME = t2.OFFNAME, " +
                            "POSTALCODE = t2.POSTALCODE, " +
                            "IFNSFL = t2.IFNSFL, " +
                            "TERRIFNSFL = t2.TERRIFNSFL, " +
                            "IFNSUL = t2.IFNSUL, " +
                            "TERRIFNSUL = t2.TERRIFNSUL, " +
                            "OKATO = t2.OKATO, " +
                            "OKTMO = t2.OKTMO, " +
                            "UPDATEDATE = t2.UPDATEDATE, " +
                            "SHORTNAME = t2.SHORTNAME, " +
                            "AOLEVEL = t2.AOLEVEL, " +
                            "PARENTGUID = t2.PARENTGUID, " +
                            "AOID = t2.AOID, " +
                            "PREVID = t2.PREVID, " +
                            "NEXTID = t2.NEXTID, " +
                            "KLADRCODE = t2.KLADRCODE, " +
                            "KLADRPLAINCODE = t2.KLADRPLAINCODE, " +
                            "ACTSTATUS = t2.ACTSTATUS, " +
                            "CENTSTATUS = t2.CENTSTATUS, " +
                            "OPERSTATUS = t2.OPERSTATUS, " +
                            "KLADRPCURRSTATUS = t2.KLADRPCURRSTATUS, " +
                            "STARTDATE = t2.STARTDATE, " +
                            "ENDDATE = t2.ENDDATE, " +
                            "NORMDOC = t2.NORMDOC, " +
                            "CODERECORD = t2.CODERECORD, " +
                            "TYPERECORD = t2.TYPERECORD " +
                            $"FROM {tmpTable} AS t2 WHERE t1.id = t2.id;\n" +

                            $"DROP TABLE {tmpTable};";
                    }
                    else
                    {
                        command.CommandText = this.GetCopyFiasTableCmd(this.DefaultFiasTableName);
                    }

                    NpgsqlCopySerializer serializer = new NpgsqlCopySerializer(connection);
                    NpgsqlCopyIn copyIn = new NpgsqlCopyIn(command, connection, serializer.ToStream);
                    try
                    {
                        copyIn.Start();

                        foreach (var rec in records)
                        {
                            if (this.BackgroundWorker.CancellationPending)
                            {
                                break;
                            }

                            serializer.AddString(rec.AOGuid);
                            serializer.AddString(rec.FormalName);
                            serializer.AddString(rec.CodeRegion);
                            serializer.AddString(rec.CodeAuto);
                            serializer.AddString(rec.CodeArea);
                            serializer.AddString(rec.CodeCity);
                            serializer.AddString(rec.CodeCtar);
                            serializer.AddString(rec.CodePlace);
                            serializer.AddString(rec.CodeStreet);
                            serializer.AddString(rec.CodeExtr);
                            serializer.AddString(rec.CodeSext);
                            serializer.AddString(rec.OffName);
                            serializer.AddString(rec.PostalCode);
                            serializer.AddString(rec.IFNSFL);
                            serializer.AddString(rec.TerrIFNSFL);
                            serializer.AddString(rec.IFNSUL);
                            serializer.AddString(rec.TerrIFNSUL);
                            serializer.AddString(rec.OKATO);
                            serializer.AddString(rec.OKTMO);
                            serializer.AddString(this.FormatDate(rec.UpdateDate));
                            serializer.AddString(rec.ShortName);
                            serializer.AddString(rec.AOLevel.ToString());
                            serializer.AddString(rec.ParentGuid);
                            serializer.AddString(rec.AOId);
                            serializer.AddString(rec.PrevId);
                            serializer.AddString(rec.NextId);
                            serializer.AddString(rec.KladrCode);
                            serializer.AddString(rec.KladrPlainCode);
                            serializer.AddString(rec.ActStatus.ToString());
                            serializer.AddString(rec.CentStatus.ToString());
                            serializer.AddString(rec.OperStatus.ToString());
                            serializer.AddString(rec.KladrCurrStatus.ToString());
                            serializer.AddString(this.FormatDate(rec.StartDate));
                            serializer.AddString(this.FormatDate(rec.EndDate));
                            serializer.AddString(rec.NormDoc);
                            serializer.AddString(rec.CodeRecord);
                            serializer.AddString(rec.TypeRecord.ToString());

                            serializer.EndRow();
                            serializer.Flush();
                        }

                        serializer.Close();
                        copyIn.End();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            copyIn.Cancel("Undo copy on exception.");
                            throw;
                        }
                        catch (NpgsqlException e2)
                        {
                            // we should get an error in response to our cancel request:
                            if (!e2.BaseMessage.Contains("Undo copy on exception."))
                            {
                                throw new Exception("Failed to cancel copy:" + e2 + " upon failure: " + e);
                            }
                        }
                    }
                }
            }
        }

        private void CopyFiasHouseRecord(IList<FiasHouseRecord> records, bool update = false)
        {
            if (this.BackgroundWorker.CancellationPending)
            {
                return;
            }

            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var tmpTable = $"tmp_{this.DefaultFiasHouseTableName}";

                    if (update)
                    {
                        command.CommandText = $"CREATE TEMP TABLE {tmpTable} " +
                            $"AS SELECT * FROM {this.DefaultFiasHouseTableName} LIMIT 0;\n" +

                            $"COMMIT;\n" +

                            $"{this.GetCopyFiasHouseTableCmd(tmpTable)}\n" +

                            $"UPDATE {this.DefaultFiasHouseTableName} AS t1 " +
                            "SET HOUSE_ID = t2.HOUSE_ID, " +
                            "HOUSE_GUID = t2.HOUSE_GUID, " +
                            "AO_GUID = t2.AO_GUID, " +
                            "POSTAL_CODE = t2.POSTAL_CODE, " +
                            "OKATO = t2.OKATO, " +
                            "OKTMO = t2.OKTMO, " +
                            "HOUSE_NUM = t2.HOUSE_NUM, " +
                            "BUILD_NUM = t2.BUILD_NUM, " +
                            "STRUC_NUM = t2.STRUC_NUM, " +
                            "STAT_STATUS = t2.STAT_STATUS, " +
                            "UPDATE_DATE = t2.UPDATE_DATE, " +
                            "TYPE_RECORD = t2.TYPE_RECORD, " +
                            "START_DATE = t2.START_DATE, " +
                            "END_DATE = t2.END_DATE " +
                            $"FROM {tmpTable} AS t2 WHERE t1.id = t2.id;\n" +

                            $"DROP TABLE {tmpTable};";
                    }
                    else
                    {
                        command.CommandText = this.GetCopyFiasHouseTableCmd(this.DefaultFiasHouseTableName);
                    }

                    NpgsqlCopySerializer serializer = new NpgsqlCopySerializer(connection);
                    NpgsqlCopyIn copyIn = new NpgsqlCopyIn(command, connection, serializer.ToStream);
                    try
                    {
                        copyIn.Start();

                        foreach (var rec in records)
                        {
                            if (this.BackgroundWorker.CancellationPending)
                            {
                                break;
                            }

                            serializer.AddString(rec.HouseId);
                            serializer.AddString(rec.HouseGuid);
                            serializer.AddString(rec.AoGuid);
                            serializer.AddString(rec.PostalCode);
                            serializer.AddString(rec.Okato);
                            serializer.AddString(rec.Oktmo);
                            serializer.AddString(rec.HouseNum);
                            serializer.AddString(rec.BuildNum);
                            serializer.AddString(rec.StrucNum);
                            serializer.AddString(rec.ActualStatus.ToString());
                            serializer.AddString(this.FormatDate(rec.UpdateDate));
                            serializer.AddString(rec.TypeRecord.ToString());
                            serializer.AddString(this.FormatDate(rec.StartDate));
                            serializer.AddString(this.FormatDate(rec.EndDate));

                            serializer.EndRow();
                            serializer.Flush();
                        }

                        serializer.Close();
                        copyIn.End();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            copyIn.Cancel("Undo copy on exception.");
                            throw;
                        }
                        catch (NpgsqlException e2)
                        {
                            if (!e2.BaseMessage.Contains("Undo copy on exception."))
                            {
                                throw new Exception("Failed to cancel copy:" + e2 + " upon failure: " + e);
                            }
                        }
                    }
                }
            }
        }

        private string FormatDate(DateTime? date)
        {
            return (date.HasValue ? $"{date.Value:dd.MM.yyyy}" : string.Empty);
        }

        private string GetCopyFiasTableCmd(string tableName)
        {
            return $"COPY {tableName} " +
                        "(AOGUID, FORMALNAME, REGIONCODE, AUTOCODE, AREACODE, CITYCODE, " +
                        "CTARCODE, PLACECODE, STREETCODE, EXTRCODE, SEXTCODE, OFFNAME, POSTALCODE, " +
                        "IFNSFL, TERRIFNSFL, IFNSUL, TERRIFNSUL, OKATO, OKTMO, UPDATEDATE, SHORTNAME, " +
                        "AOLEVEL, PARENTGUID, AOID, PREVID, NEXTID, KLADRCODE, KLADRPLAINCODE, ACTSTATUS, CENTSTATUS, " +
                        "OPERSTATUS, KLADRPCURRSTATUS, STARTDATE, ENDDATE, NORMDOC, CODERECORD, TYPERECORD) FROM STDIN;";
        }

        private string GetCopyFiasHouseTableCmd(string tableName)
        {
            return $"COPY {tableName} " +
                        "(HOUSE_ID, HOUSE_GUID, AO_GUID, POSTAL_CODE, OKATO, OKTMO, " +
                        "HOUSE_NUM, BUILD_NUM, STRUC_NUM, STAT_STATUS, UPDATE_DATE, " +
                        "TYPE_RECORD, START_DATE, END_DATE) FROM STDIN;";
        }
    }
}