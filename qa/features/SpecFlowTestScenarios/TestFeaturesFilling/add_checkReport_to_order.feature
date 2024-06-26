﻿
Функционал: тесткейсы для добавления акта проверки к приказу в разделе "Плановые проверки юридических лиц"
Жилищная инспекция - Основания проверок - Плановые проверки юридических лиц - Приказ - Акт проверки

Предыстория: 
Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

И добавлен контрагент с этой организационно-правовой формой
| Name                | OrganizationForm |
| тестовый контрагент | тест             |

И добавлена управляющая организация для этого контрагента
| Contragent          | Description |
| тестовый контрагент | тест        |

И добавлен план мероприятий
| Name                      | DateStart  | DateEnd |
| тестовый план мероприятий | 01.01.2015 |         |

И добавлен инспектор
| Code | Fio                |
| 159  | Тестовый инспектор |

И добавлена плановая проверка юридических лиц
| TypeJurPerson       | Contragent          | Plan                      | DateStart    | JurPersonInspectors |
| JurPersonInspectors | тестовый контрагент | тестовый план мероприятий | текущая дата | Тестовый инспектор  |

И добавлено нарушение
| Name                |
| 1тестовое нарушение |

И добавлена группа нарушений
| Name                       |
| 1тестовая группа нарушений |

И добавлено это нарушение к этой группе нарушений
И пользователь у этой плановой проверки юридических лиц формирует приказ
И пользователь у этого приказа формирует акт проверки
И пользователь у этого акта проверки заполняет поле Дата "текущая дата"
И пользователь у этого акта проверки заполняет поле С копией приказа ознакомлен "тест"
И пользователь у этого акта проверки заполняет поле Площадь "51"

Сценарий: успешное добавление акта проверки к приказу
Когда пользователь сохраняет этот акт проверки
Тогда у этой плановой проверки юридических лиц присутствует акт проверки в списке разделов

Сценарий: неудачное добавление результата проверки с результатом Нарушения выявлены = да без добавления нарушений
Когда пользователь сохраняет этот акт проверки
И пользователь в редактирует результат проверки
И пользователь у этого результата проверки заполняет поле Нарушения выявлены "текущая дата"
Когда пользователь сохраняет этот результат проверки
Тогда у этого акта проверки запись по результату проверки отсутствует в списке результатов проверки
И выводится сообщение об ошибке с текстом "Если нарушения выявлены, то необходимо в таблице нарушений добавить записи нарушений"

Сценарий: успешное добавление результата проверки с результатом Нарушения выявлены = да с добавлением нарушений
Когда пользователь сохраняет этот акт проверки
И пользователь в редактирует результат проверки
И пользователь у этого результата проверки заполняет поле Нарушения выявлены "текущая дата"
И пользователь у этого результата проверки добавляет это нарушение из этой группы нарушений
Когда пользователь сохраняет этот результат проверки
Тогда у этого акта проверки запись по результату проверки присутствует в списке результатов проверки
И выводится сообщение с текстом "Результаты проверки сохранены успешно"


