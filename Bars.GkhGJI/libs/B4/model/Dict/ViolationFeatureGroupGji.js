Ext.define('B4.model.dict.ViolationFeatureGroupGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ViolationFeatureGji',
        listAction: 'ListGroups'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'FeatureViolGji', defaultValue: null },
        { name: 'Name' },
        { name: 'FullName' },
        { name: 'Code' },
        { name: 'Parent', defaultValue: null },
        { name: 'IsChecked' }
    ]
});