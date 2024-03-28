Ext.define('B4.model.priorityparam.Addition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PriorityParamAddition'
    },
    fields: [
        { name: 'Id' },
        { name: 'Code' },
        { name: 'AdditionFactor' },
        { name: 'FactorValue' },
        { name: 'FinalValue' }
    ]
});