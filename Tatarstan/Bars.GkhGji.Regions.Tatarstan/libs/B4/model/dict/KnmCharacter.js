Ext.define('B4.model.dict.KnmCharacter', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KnmCharacter'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'KindCheck' },
        { name: 'ErknmCode' }
    ]
});