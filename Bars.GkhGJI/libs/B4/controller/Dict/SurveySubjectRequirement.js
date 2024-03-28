Ext.define('B4.controller.dict.SurveySubjectRequirement', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['SurveySubjectRequirement'],
    stores: ['dict.SurveySubjectRequirement'],

    views: ['dict.surveysubjectrequirement.Grid'],

    mainView: 'dict.surveysubjectrequirement.Grid',
    mainViewSelector: 'surveysubjectrequirementgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveysubjectrequirementgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'surveysubjectrequirementgrid',
            permissionPrefix: 'GkhGji.Dict.SurveySubjectRequirement'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'surveysubjectrequirementgrid',
            name: 'surveySubjectRequirementGridAspect',
            storeName: 'dict.SurveySubjectRequirement',
            modelName: 'SurveySubjectRequirement'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('surveysubjectrequirementgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveySubjectRequirement').load();
    }
});