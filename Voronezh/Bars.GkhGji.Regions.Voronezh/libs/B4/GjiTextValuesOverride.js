Ext.define('B4.GjiTextValuesOverride', {
    extend: 'B4.TextValues',
    singleton: true,
    overrideItems: {
        'распоряжение': 'Приказ',
        'номер распоряжения': 'Номер приказа',
        'дата распоряжения': 'Дата приказа',
        'номер': 'Номер ГЖИ'
    }
});
