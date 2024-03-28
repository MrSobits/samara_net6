namespace Bars.GisIntegration.Smev.Migrations._2022.Version_2022060400
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022060400")]
    [MigrationDependsOn(typeof(Version_2022060300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                           CREATE SCHEMA IF NOT EXISTS system;

                            DO
                            $$
                            BEGIN
                                    CREATE SEQUENCE SYSTEM.FILE_METADATA_ID_seq
                                    INCREMENT 1
                                    START 1
                                    MINVALUE 1
                                    MAXVALUE 9223372036854775807
                                    CACHE 1;
                            EXCEPTION WHEN duplicate_table THEN
                                    -- ничего не делать
                            END
                            $$ LANGUAGE plpgsql;

                           CREATE TABLE IF NOT EXISTS system.file_metadata
                          (
                            id bigint NOT NULL DEFAULT nextval('SYSTEM.FILE_METADATA_ID_seq'::regclass), -- Идентификатор
                            name character varying(1000) NOT NULL, -- Наименование файла без расширения
                            extension character varying(100) NOT NULL, -- Расширение файла
                            cached_name character varying(100) NOT NULL, -- Кэшированное имя файла
                            last_access timestamp without time zone NOT NULL, -- now()
                            checksum character varying(100) NOT NULL, -- Контрольная сумма
                            file_size bigint NOT NULL, -- Размер файла в байтах
                            checksum_hash_algorithm character varying(100) NOT NULL DEFAULT 'MD5'::character varying, -- Алгоритм хэширования для получения контрольной суммы
                            is_temporary boolean NOT NULL DEFAULT false, -- Является временным
                            object_version bigint NOT NULL DEFAULT 0, -- Версия объекта
                            object_create_date timestamp without time zone NOT NULL DEFAULT now(), -- Дата создания
                            object_edit_date timestamp without time zone NOT NULL DEFAULT now(), -- Дата изменения
                            uid uuid,
                            CONSTRAINT file_metadata_pkey PRIMARY KEY (id)
                          );

                          COMMENT ON TABLE system.file_metadata
                            IS '""Метаданные файлов""';
                          COMMENT ON COLUMN system.file_metadata.id IS 'Идентификатор';
                          COMMENT ON COLUMN system.file_metadata.name IS 'Наименование файла без расширения';
                          COMMENT ON COLUMN system.file_metadata.extension IS 'Расширение файла';
                          COMMENT ON COLUMN system.file_metadata.cached_name IS 'Кэшированное имя файла';
                          COMMENT ON COLUMN system.file_metadata.last_access IS 'now()';
                          COMMENT ON COLUMN system.file_metadata.checksum IS 'Контрольная сумма';
                          COMMENT ON COLUMN system.file_metadata.file_size IS 'Размер файла в байтах';
                          COMMENT ON COLUMN system.file_metadata.checksum_hash_algorithm IS 'Алгоритм хэширования для получения контрольной суммы';
                          COMMENT ON COLUMN system.file_metadata.is_temporary IS 'Является временным';
                          COMMENT ON COLUMN system.file_metadata.object_version IS 'Версия объекта';
                          COMMENT ON COLUMN system.file_metadata.object_create_date IS 'Дата создания';
                          COMMENT ON COLUMN system.file_metadata.object_edit_date IS 'Дата изменения';");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(new SchemaQualifiedObjectName {Schema = "system", Name = "file_metadata"});
            this.Database.RemoveSequence("SYSTEM.FILE_METADATA_ID_seq");
        }
    }
}