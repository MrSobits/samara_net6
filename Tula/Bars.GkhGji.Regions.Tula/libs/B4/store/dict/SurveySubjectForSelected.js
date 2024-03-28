Ext.define('B4.store.dict.SurveySubjectForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.SurveySubject'],
    autoLoad: false,
    storeId: 'surveySubjectSelectedStore',
    model: 'B4.model.dict.SurveySubject'
});