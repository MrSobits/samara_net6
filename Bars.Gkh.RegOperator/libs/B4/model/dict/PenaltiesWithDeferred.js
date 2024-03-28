Ext.define('B4.model.dict.PenaltiesWithDeferred', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PenaltiesWithDeferred'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateStartCalc' },
        { name: 'DateEndCalc' }
    ]
});