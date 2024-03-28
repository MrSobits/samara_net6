Ext.define('B4.model.FormulaParam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Name' },
        { name: 'Attribute' },
        { name: 'ValueResolverCode' },
        { name: 'ValueResolverName' }
    ]
});