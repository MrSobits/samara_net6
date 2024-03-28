Ext.define('B4.model.efficiencyrating.ManagingOrganizationDataValueTree', {
    extend: 'B4.model.efficiencyrating.ManagingOrganizationDataValue',
    idProperty: 'Id',
    proxy: {
        type: 'memory'   
    },
    fields: [
        { name: 'Children' },
        { name: 'Name' },
        { name: 'AttributeObjects' },
        { name: 'iconCls', defaultValue: '' },
        { name: 'expanded', defaultValue: true },
        { name: 'leaf' }
    ]
});