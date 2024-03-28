Ext.define('B4.model.person.PlaceWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonPlaceWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Person', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Position', defaultValue: null },
        { name: 'FileInfo', defaultValue: null },
        { name: 'StartDate' },
        { name: 'EndDate' }
    ]
});