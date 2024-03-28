
Функционал: тестирование Импорта состояний ООИ (Москва)


Предыстория: 
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-msk"


Сценарий: последовательность выполнение Импорта оценок состояния ООИ
#описание сценариев Ипорта ДПКР (Москва) смотреть в import_dpkr_msk.feature
Когда пользователь проводит Ипорт ДПКР (Москва)
То после завершения импорта он обязани провести Импорта оценок состояния ООИ (Москва)
Если Ипорт ДПКР (Москва) ранее не был проведен
И пользователь проводит Импорта оценок состояния ООИ (Москва)
То оценки состояний ООИ не сядут и в логе будет соответствующая запись с наличием ошибок


Сценарий: проверка формата импортируемого файла
Дано файл для импорта данных
Когда пользователь импортирует файл
То система проверяет файл на соответствие формату по наличию значения обязательного аттрибута "UID"
Если в файле не указано значение по аттрибуту "UID"
То файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла, количество загруженных записей, количество незагруженных записей, прочая информация
Если в импортируемом файле нет столбца с аттрибутом "UID"
То файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла


Структура сценария: проверка формата импортируемого файла на наличие других аттрибутов 
Дано файл для импорта данных
Когда пользователь импортирует файл
То система проверяет файл на соответствие формату по наличию аттрибутов в файле <аттрибут>
Если в файле не указано значение хотя бы по одному из аттрибутов <аттрибут>
То файл загружается
И и в лог записывается количество загруженных записей, количество незагруженных записей, прочая информация
Если в файле отсутствует хотя бы один из аттрибутов <аттрибут>
То файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла


Примеры: 
| аттрибут                              |
| MKDOG1O01 MZI_ES_Ocenka               |
| MKDOG1O02 MZI_CO_cherdak_Ocenka       |
| MKDOG1O02 MZI_CO_tech_Ocenka          |
| MKDOG1O02 MZI_CO_etaj_Ocenka          |
| MKDOG1O03 MZI_GS_Ocenka               |
| MKDOG1O04 MZI_HVS_etaj_Ocenka         |
| MKDOG1O04 MZI_HVS_pojar_Ocenka        |
| MKDOG1O04 MZI_HVS_tech_Ocenka         |
| MKDOG1O05 MZI_GVS_cherdak_Ocenka      |
| MKDOG1O05 MZI_GVS_tech_Ocenka         |
| MKDOG1O05 MZI_GVS_etaj_Ocenka         |
| MKDOG1O06 MZI_Kanal_tech_Ocenka       |
| MKDOG1O06 MZI_Kanal_etaj_Ocenka       |
| MKDOG1O07 MZI_Mys_Ocenka              |
| MKDOG1O09 MZI_SB_PPADY_Ocenka         |
| MKDOG3O01 MZI_Fasad_Ocenka            |
| MKDOG3O01 MZI_Fasad_styki_Ocenka      |
| MKDOG3O02 MZI_Kon_el_Balk_Ocenka      |
| MKDOG3O04 MZI_Kon_el_Vodootvod_Ocenka |
| MKDOG4O06 MZI_Podval_Ocenka           |
| MKDOG5O01 MZI_Krov_Ocenka_krov        |


Структура сценария: распределение оценок состоянии ООИ
Дано файл для импорта данных
Когда пользователь импортирует файл
И данные из аттрибута <аттрибут> загружаются без ошибок
Тогда система находит дома по соотнесению аттрибута "uid" из файла с аттрибутом "uid", который уже есть в нашей системе после Импорта дпкр






Примеры: 
| аттрибут                              |
| MKDOG1O01 MZI_ES_Ocenka               |
| MKDOG1O02 MZI_CO_cherdak_Ocenka       |
| MKDOG1O02 MZI_CO_tech_Ocenka          |
| MKDOG1O02 MZI_CO_etaj_Ocenka          |
| MKDOG1O03 MZI_GS_Ocenka               |
| MKDOG1O04 MZI_HVS_etaj_Ocenka         |
| MKDOG1O04 MZI_HVS_pojar_Ocenka        |
| MKDOG1O04 MZI_HVS_tech_Ocenka         |
| MKDOG1O05 MZI_GVS_cherdak_Ocenka      |
| MKDOG1O05 MZI_GVS_tech_Ocenka         |
| MKDOG1O05 MZI_GVS_etaj_Ocenka         |
| MKDOG1O06 MZI_Kanal_tech_Ocenka       |
| MKDOG1O06 MZI_Kanal_etaj_Ocenka       |
| MKDOG1O07 MZI_Mys_Ocenka              |
| MKDOG1O09 MZI_SB_PPADY_Ocenka         |
| MKDOG3O01 MZI_Fasad_Ocenka            |
| MKDOG3O01 MZI_Fasad_styki_Ocenka      |
| MKDOG3O02 MZI_Kon_el_Balk_Ocenka      |
| MKDOG3O04 MZI_Kon_el_Vodootvod_Ocenka |
| MKDOG4O06 MZI_Podval_Ocenka           |
| MKDOG5O01 MZI_Krov_Ocenka_krov        |