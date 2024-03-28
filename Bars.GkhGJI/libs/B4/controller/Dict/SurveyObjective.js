Ext.define('B4.controller.dict.SurveyObjective', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SurveyObjective'],
    stores: ['dict.SurveyObjective'],

    views: ['dict.surveyobjective.Grid'],

    mainView: 'dict.surveyobjective.Grid',
    mainViewSelector: 'surveyobjectivegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveyobjectivegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'surveyobjectivegrid',
            permissionPrefix: 'GkhGji.Dict.SurveyObjective'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'surveyobjectivegrid',
            name: 'surveyObjectiveGridAspect',
            storeName: 'dict.SurveyObjective',
            modelName: 'dict.SurveyObjective'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('surveyobjectivegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveyObjective').load();
    }
});