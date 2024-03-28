Ext.define('B4.store.SurveyPlan', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.SurveyPlan'],
    storeId: 'surveyPlanStore',
    model: 'B4.model.SurveyPlan'
});