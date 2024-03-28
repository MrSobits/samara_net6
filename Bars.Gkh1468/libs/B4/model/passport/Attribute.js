Ext.define('B4.model.passport.Attribute', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Type' },
        { name: 'Parent' },
        { name: 'ParentPart' },
        { name: 'ValueType' },
        { name: 'ValidateChilds', type: 'boolean', defaultValue: false },
        { name: 'GroupText' },
        { name: 'OrderNum' },
        { name: 'UnitMeasure' },
        { name: 'IntegrationCode' },
        { name: 'UseInPercentCalculation'},
        { name: 'MaxLength' },
        { name: 'MinLength' },
        { name: 'Pattern' },
        { name: 'Exp' },
        { name: 'Required' },
        { name: 'AllowNegative' },
        { name: 'DictCode' },
        { name: 'DataFillerCode' }
    ]
});