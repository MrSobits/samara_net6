Ext.define('B4.store.surveyplan.Candidate', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.surveyplan.Candidate'],
    storeId: 'surveyPlanCandidateStore',
    model: 'B4.model.surveyplan.Candidate'
});