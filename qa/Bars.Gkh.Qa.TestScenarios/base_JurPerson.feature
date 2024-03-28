﻿@ScenarioInTransaction
Функционал: тесткейсы для раздела "Плановые проверки юридических лиц"
Жилищная инспекция - Основания проверок - Плановые проверки юридических лиц

Предыстория: 
Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

Дано добавлен контрагент с организационно правовой формой
| Name |
| тест |

И добавлена управляющая организация для этого контрагента
| Description |
| тест        |

И добавлен план проверки юридических лиц
| Name                       | DateStart  | DateEnd    |
| те1стовый план мероприятий | 01.01.2015 | 01.01.2016 |

И добавлен инспектор
| Code | Fio                | Email          |
| 159  | Тестовый инспектор | test@test.test |

И пользователь добавляет новую плановую проверку юридических лиц
И пользователь у этой плановой проверки юридических лиц заполняет поле Тип юридического лица "Управляющая организация"
И пользователь у этой плановой проверки юридических лиц заполняет поле Юридическое лицо этим контрагентом
И пользователь у этой плановой проверки юридических лиц заполняет поле План этим планом
И пользователь у этой плановой проверки юридических лиц заполняет поле Дата начала проверки "текущая дата"
И пользователь у этой плановой проверки юридических лиц заполняет поле Инспекторы этим инспектором

Сценарий: успешное добавление плановой проверки юридических лиц
Когда пользователь сохраняет эту плановую проверку юридических лиц
Тогда запись по этой плановой проверки юридических лиц присутствует в справочнике плановых проверок юридических лиц

Сценарий: успешное удаление плановой проверки юридических лиц
Когда пользователь сохраняет эту плановую проверку юридических лиц
И пользователь удаляет эту плановую проверку юридических лиц
Тогда запись по этой плановой проверки юридических лиц отсутствует в справочнике плановых проверок юридических лиц

@ignore
Сценарий: успешное добавление дубля плановой проверки юридических лиц
Когда пользователь сохраняет эту плановую проверку юридических лиц
Дано пользователь добавляет новую плановую проверку юридических лиц
И пользователь у этой плановой проверки юридических лиц заполняет поле Тип юридического лица "Управляющая организация"
И пользователь у этой плановой проверки юридических лиц заполняет поле Юридическое лицо этим контрагентом
И пользователь у этой плановой проверки юридических лиц заполняет поле План этим планом
И пользователь у этой плановой проверки юридических лиц заполняет поле Дата начала проверки "текущая дата"
И пользователь у этой плановой проверки юридических лиц заполняет поле Инспекторы этим инспектором
Когда пользователь сохраняет эту плановую проверку юридических лиц
Тогда запись по этой плановой проверки юридических лиц присутствует в справочнике плановых проверок юридических лиц