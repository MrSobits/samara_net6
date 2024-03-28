Ext.define('B4.model.dict.SettlementWithMo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ParentMo' }
    ]
});