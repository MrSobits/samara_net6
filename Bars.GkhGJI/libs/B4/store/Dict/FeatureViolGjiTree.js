Ext.define('B4.store.dict.FeatureViolGjiTree', {
    extend: 'Ext.data.TreeStore',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Parent' },
        { name: 'parentId' }
    ],
    proxy: {
        type: 'memory'
    },
    root: {
        text: 'Root',
        expanded: true,
        leaf: false,
        children: []
    }
});