Ext.define('B4.store.actionisolated.taskaction.SurveyPurposeForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.SurveyPurpose'],
    autoLoad: false,
    storeId: 'taskSurveyPurposeForSelectStore',
    model: 'B4.model.dict.SurveyPurpose'
});