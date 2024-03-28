/**
* модель связи между подтематикой и характеристикой нарушения
*/
Ext.define('B4.model.dict.StatSubsubjectFeatureGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StatSubsubjectFeatureGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Subsubject', defaultValue: null },
        { name: 'FeatureViol', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' }
    ]
});