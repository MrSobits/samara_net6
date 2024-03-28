Ext.define('B4.model.dict.ViolationActionsRemovGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ViolationActionsRemovGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'ActionsRemovViol', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' }
    ]
});