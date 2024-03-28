Ext.define('B4.GjiTextValuesOverride',
    {
        extend: 'B4.TextValues',
        singleton: true,
        overrideItems: {
            'гжи, рассмотревшая обращение': 'Отдел',
            'жилищная инспекция': 'Отдел',
            'зональные жилищные инспекции': 'Отделы'
        }
    });