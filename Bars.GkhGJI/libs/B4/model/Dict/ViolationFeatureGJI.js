Ext.define('B4.model.dict.ViolationFeatureGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ViolationFeatureGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'FeatureViolGji', defaultValue: null },
        { name: 'Name' },
        { name: 'CodePin' },
        { name: 'NormDocNum' }
    ]
});