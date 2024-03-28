Ext.define('B4.store.dict.SurveySubjectForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.SurveySubject'],
    autoLoad: false,
    storeId: 'surveySubjectSelectStore',
    model: 'B4.model.dict.SurveySubject'
});