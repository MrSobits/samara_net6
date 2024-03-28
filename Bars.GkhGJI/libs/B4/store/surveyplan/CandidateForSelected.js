Ext.define('B4.store.surveyplan.CandidateForSelected', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.surveyplan.Candidate'],
    storeId: 'surveyPlanCandidateForSelectedStore',
    model: 'B4.model.surveyplan.Candidate'
});