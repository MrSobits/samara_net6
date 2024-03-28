Ext.define('B4.model.dict.ElementOutdoor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ElementOutdoor'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'ElementGroup' },
        { name: 'UnitMeasure' }
    ]
});