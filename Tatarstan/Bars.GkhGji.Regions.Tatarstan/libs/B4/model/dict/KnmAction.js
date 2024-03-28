Ext.define('B4.model.dict.KnmAction', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KnmAction'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ActCheckActionType' },
        { name: 'KnmType' },
        { name: 'ControlType' },
        { name: 'ErvkId' },
        { name: 'KindAction'}
    ]
});