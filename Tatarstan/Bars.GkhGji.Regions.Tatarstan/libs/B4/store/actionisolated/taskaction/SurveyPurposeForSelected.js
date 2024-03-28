Ext.define('B4.store.actionisolated.taskaction.SurveyPurposeForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.SurveyPurpose'],
    autoLoad: false,
    storeId: 'taskSurveyPurposeForSelectedStore',
    model: 'B4.model.dict.SurveyPurpose'
});