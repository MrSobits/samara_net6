Ext.define('B4.model.cmnestateobj.GroupAttributeWithResolvers', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    requires: [
        'B4.enums.AttributeType'
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'StructuralElementGroupAttribute',
        listAction: 'listwithresolvers'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'IsNeeded' },
        { name: 'Group', defaultValue: null },
        { name: 'AttributeType', defaultValue: null },
        { name: 'ValueResolverCode' },
        { name: 'ValueResolverName' }
    ]
});