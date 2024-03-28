Ext.define('B4.model.HousingInspection', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousingInspection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' },
        { name: 'FactAddress' },
        { name: 'Phone' },
        { name: 'Email' },
        { name: 'ContragentId' }
    ]
});