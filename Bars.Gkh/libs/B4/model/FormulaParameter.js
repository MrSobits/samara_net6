Ext.define('B4.model.FormulaParameter', {
    extend: 'B4.base.Model',
    idProperty: 'Code',
    fields: [
        { name: 'Name' },
        { name: 'Code' },
        { name: 'DisplayName' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'Formula',
        listAction: 'List'
    }
});