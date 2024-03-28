Ext.define('B4.model.gisrealestate.realestatetype.RealEstateTypeGrouped', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'ajax',
        url: 'GisRealEstateType/GroupedTypeList'
    },
    fields: [
        { name: 'Id' },
        { name: 'EntityId' },
        { name: 'Entity' },
        { name: 'Name' },
        { name: 'Group' },
        { name: 'IndCount' },
        { name: 'iconCls', type: 'string', defaultValue: 'treenode-no-icon' }
    ]
});