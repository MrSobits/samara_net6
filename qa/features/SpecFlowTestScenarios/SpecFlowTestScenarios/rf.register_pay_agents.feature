﻿
Функционал: доработки к реестру оплат платежных агентов

#реализация для всех регионов (на прмиере Камчатки)
Структура сценария: возможность загрузки файла любого расширения для всех типов импорта, кроме универсального
Дано Пользователь с ролью администратора, логин "admin", пароль "admin"
Дано тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka/"
Когда пользователь грузит документ из банка любого расширения
То система проверяет загружаемый файл на соответствие формату импорта <формат>
Если файл соответствует формату импорта
То файл успешно загружается системой и отображается в реестре оплат платежных агентов
Если файл НЕ соответствует ни одному из существующих в системе форматов импорта
То выводится сообщение об ошибке с текстом "Невозможно прочитать файл. Неверный формат файла"

Примеры: 
| формат                                         |
| Загрузка (Dbf - Питер) (dbf Import)            |
| Загрузка/Выгрузка (dbf Import)                 |
| Загрузка/Выгрузка (dbf 2) (dbf Import)         |
| Загрузка/Выгрузка (Json Import, Export)        |
| Загрузка/Выгрузка (Xml Import, Export)         |


#реализация для всех регионов (на прмиере Камчатки)
Сценарий: возможность загрузки файла любого расширения для универсального универсального типа импорта
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka/"
Когда пользователь грузит документ из банка любого расширения
И выбирает тип импорта "Универсальный импорт (Текстовый формат | Import)"
И выбирает тип универсального импорта
То система сначала проверяет загружаемый файл на соответствие формату импорта "Универсальный импорт (Текстовый формат | Import)"
И далее система проверяет загружаемый файл на соответствие формату из типов универсального импорта
Если файл соответствует выбранным форматам
То  файл успешно загружается системой и отображается в реестре оплат платежных агентов
Если файл не проходит хотя бы одну из проверок на форматы импорта 
То выводится сообщение об ошибке с текстом "Невозможно прочитать файл. Неверный формат файла"