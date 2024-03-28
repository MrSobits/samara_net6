Ext.define('B4.model.dict.StageWorkCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StageWorkCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name'}, 
        { name: 'Code' }
    ]
});