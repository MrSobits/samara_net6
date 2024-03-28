Ext.define('B4.model.cmnestateobj.StructuralElementGroupAttribute', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    requires: [
        'B4.enums.AttributeType'
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'StructuralElementGroupAttribute'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'IsNeeded' },
        { name: 'Group', defaultValue: null },
        { name: 'AttributeType', defaultValue: null },
        { name: 'Hint' }
    ]
});