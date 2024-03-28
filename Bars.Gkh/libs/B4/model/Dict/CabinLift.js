Ext.define('B4.model.dict.CabinLift', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'cabinlift'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});