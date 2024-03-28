::отключаем уведомления
@echo off
::имя пользователя
set username=postgres
::имя базы, которую надо обновить
set dbname=gkh_nn
::хост
set host=localhost
::копируем файл more.com, для машин у которых в переменной PATH нет пути до этого файла
@copy "C:/Windows/System32/more.com" "C:\Program Files\PostgreSQL\9.2\bin\more.com"
::запрос на получение запросов на обновление
::может запросить пароль если в файле pg_hba.conf для %username% указан метод подключения не trust
"C:\Program Files\PostgreSQL\9.2\bin\psql.exe" --host=%host% --dbname=%dbname% --username=%username% --file=query.sql --output=tmp_result.sql
::обновление последовательностей
::может запросить пароль если в файле pg_hba.conf для %username% указан метод подключения не trust
"C:\Program Files\PostgreSQL\9.2\bin\psql.exe" --host=%host% --dbname=%dbname% --username=%username% --file=tmp_result.sql
::удаляем временный файл
del tmp_result.sql
@pause