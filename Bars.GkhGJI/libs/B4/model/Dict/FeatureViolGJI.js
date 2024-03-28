Ext.define('B4.model.dict.FeatureViolGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FeatureViolGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'GkhReformCode' },
        { name: 'Parent' },
        { name: 'TrackAppealCits' }
    ]
});