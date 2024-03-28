Ext.define('B4.model.dict.ErknmTypeDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ErknmTypeDocument'
    },
    fields: [
        { name: 'DocumentType' },
        { name: 'Code' }, 
        { name: 'IsBasisKnm', defaultValue: false},
        { name: 'KindCheck' }
    ]
});