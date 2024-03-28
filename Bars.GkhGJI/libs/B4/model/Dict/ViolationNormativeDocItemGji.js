Ext.define('B4.model.dict.ViolationNormativeDocItemGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Violation', defaultValue: null },
        { name: 'NormativeDocItem', defaultValue: null },
        { name: 'NormativeDocItemName', defaultValue: null },
        { name: 'NormativeDoc', defaultValue: null },
        { name: 'NormativeDocName', defaultValue: null },
        { name: 'ViolationStructure' }
    ],
    proxy:
    {
        type: 'b4proxy',
        controllerName: 'ViolationNormativeDocItemGji'
    }
});