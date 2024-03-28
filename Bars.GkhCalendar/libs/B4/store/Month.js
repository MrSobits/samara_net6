Ext.define('B4.store.Month', {
    extend: 'Ext.data.ArrayStore',
    storeId: 'monthStore',
    fields: [
        { name: 'number', type: 'int' },
        'name'
    ],
    data: [
        [1, 'Январь'],
        [2, 'Февраль'],
        [3, 'Март'],
        [4, 'Апрель'],
        [5, 'Май'],
        [6, 'Июнь'],
        [7, 'Июль'],
        [8, 'Август'],
        [9, 'Сентябрь'],
        [10, 'Октябрь'],
        [11, 'Ноябрь'],
        [12, 'Декабрь']
    ]
});