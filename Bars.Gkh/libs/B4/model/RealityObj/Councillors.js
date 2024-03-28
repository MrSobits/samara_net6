Ext.define('B4.model.realityobj.Councillors', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeCouncillors'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectCouncillors'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Fio' },
        { name: 'Post', defaultValue: 10 },
        { name: 'Phone' },
        { name: 'Email' }
    ]
});