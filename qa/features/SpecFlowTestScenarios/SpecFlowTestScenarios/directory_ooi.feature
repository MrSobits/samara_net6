﻿
Функционал: справочник "ООИ"


Структура сценария: успешное сохранение значения в поле "Предельный срок эксплуатации"
Когда пользователь в справочнике ООИ по КЭ заполняет поле "Предельный срок эксплуатации" значением "<значение>" и сохраняет КЭ
Тогда по запись успешно сохраняется

Примеры: 
| значение |
| 30       |
|          |


Сценарий: запрет сохранения не целого значения в поле "Предельный срок эксплуатации"
Когда пользователь в справочнике ООИ по КЭ заполняет поле "Предельный срок эксплуатации" значением "30,1" и сохраняет КЭ
То запись не сохраняется и падает ошибка