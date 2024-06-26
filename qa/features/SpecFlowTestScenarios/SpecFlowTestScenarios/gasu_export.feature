﻿
Функционал: проверки по Экспорту данных для ГАС "Управление"
#у каждого сценария данной фичи есть ID (например, "gasu_exp-1", где gasu_exp - название фичи, 1 - номер сценария )


#gasu_exp-1
#для Новосибирска



Структура сценария: Проверка возможности сохранения данных
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-nso"
Когда заполняет данные по аттрибутам <аттрибут>
И сохраняет данные
Тогда сохраняются данные по аттрибутам <аттрибут>

Примеры: 
| аттрибут           |
| Адрес ГАСУ сервиса |
| Логин              |
| Пароль             |


#gasu_exp-2
#для Новосибирска
Структура сценария: Проверка отработки сохранения данных
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-nso"
Когда заполняет данные по аттрибутам <аттрибут>
И не сохраняет данные
Тогда не сохраняются данные по аттрибутам <аттрибут>

Примеры: 
| аттрибут           |
| Адрес ГАСУ сервиса |
| Логин              |
| Пароль             |


#gasu_exp-1
#для Новосибирска
Сценарий: Проверка возможности сохранения данных, 
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-nso"
Когда заполняет данные по аттрибутам <аттрибут>
И сохраняет данные
Тогда сохраняются данные по аттрибутам <аттрибут>