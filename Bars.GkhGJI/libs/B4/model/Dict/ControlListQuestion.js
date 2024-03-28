Ext.define('B4.model.dict.ControlListQuestion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlListQuestion'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'ControlList' },
        { name: 'Description' },
        { name: 'ERKNMGuid' },
        { name: 'NPDName' }
    ]
});