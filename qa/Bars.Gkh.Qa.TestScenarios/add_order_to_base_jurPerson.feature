﻿@ScenarioInTransaction
Функционал: тесткейсы для добавления приказа к плановой проверке юридических лиц в разделе "Плановые проверки юридических лиц"
Жилищная инспекция - Основания проверок - Плановые проверки юридических лиц - Приказ

Предыстория: 
Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| те1ст | те1ст | те1ст      |

Дано добавлен контрагент с организационно правовой формой
| Name |
| те1ст |

И добавлена управляющая организация для этого контрагента
| Description |
| те1ст        |

И добавлен план проверки юридических лиц
| Name                      | DateStart  | DateEnd    |
| те1стовый план мероприятий | 01.01.2015 | 01.01.2016 |

И добавлен инспектор
| Code | Fio                   | Email          |
| 1519 | Тесто111вый инспектор | test@test.test |

И добавлена плановая проверка юридических лиц
| TypeJurPerson           | DateStart    |
| Управляющая организация | текущая дата |

И пользователь у этой плановой проверки юридических лиц формирует приказ
И пользователь у этого приказа заполняет поле Дата "текущая дата"
И пользователь у этого приказа заполняет поле Период проведения проверки с "текущая дата"
И пользователь у этого приказа заполняет поле по "текущая дата"
И пользователь у этого приказа заполняет поле ДЛ, вынесшее Приказ этим инспектором

#на разных регионах разные формы и таблицы
Сценарий: успешное добавление приказа к проверке юридических лиц
Когда пользователь сохраняет этот приказ
Тогда у этой плановой проверки юридических лиц присутствует приказ в списке разделов


