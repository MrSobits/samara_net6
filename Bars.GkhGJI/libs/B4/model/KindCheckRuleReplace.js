Ext.define('B4.model.KindCheckRuleReplace', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'KindCheckRuleReplace'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code', defaultValue: 10 },
        { name: 'RuleCode' }
    ]
});