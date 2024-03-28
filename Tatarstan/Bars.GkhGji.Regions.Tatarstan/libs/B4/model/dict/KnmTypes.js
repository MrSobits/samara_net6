Ext.define('B4.model.dict.KnmTypes', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KnmTypes'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'KindCheck' },
        { name: 'ErvkId' }
    ]
});