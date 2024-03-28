Ext.define('B4.store.actsurvey.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'actSurveyRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});