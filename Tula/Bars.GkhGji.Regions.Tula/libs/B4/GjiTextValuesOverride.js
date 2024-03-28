﻿Ext.define('B4.GjiTextValuesOverride', {
    extend: 'B4.TextValues',
    singleton: true,
    overrideItems: {
        'гжи, рассмотревшая обращение': 'Отдел, рассмотревший обращение',
        'жилищная инспекция': 'Отдел',
        'инспекционные проверки': 'Инспекционные обследования',
        'зональные жилищные инспекции': 'Отделы',
        'проверки по обращениям граждан': 'Проверки по обращениям и заявлениям'
    }
});