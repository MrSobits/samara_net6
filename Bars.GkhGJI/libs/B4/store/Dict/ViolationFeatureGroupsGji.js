Ext.define('B4.store.dict.ViolationFeatureGroupsGji', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ViolationFeatureGroupGji'],
    autoLoad: false,
    model: 'B4.model.dict.ViolationFeatureGroupGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ViolationFeatureGji',
        listAction: 'ListGroups'
    }
});