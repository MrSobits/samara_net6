Ext.define('B4.controller.dict.SurveySubject', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SurveySubject'],
    stores: ['dict.SurveySubject'],

    views: ['dict.surveysubject.Grid'],

    mainView: 'dict.surveysubject.Grid',
    mainViewSelector: 'surveySubjectGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveySubjectGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'surveySubjectGrid',
            permissionPrefix: 'GkhGji.Dict.SurveySubject'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'surveySubjectGridAspect',
            storeName: 'dict.SurveySubject',
            modelName: 'dict.SurveySubject',
            gridSelector: 'surveySubjectGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('surveySubjectGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveySubject').load();
    }
});