Ext.define('B4.controller.dict.SurveySubject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SurveySubject'],
    stores: ['dict.SurveySubject'],

    views: ['dict.surveysubject.Grid'],

    mainView: 'dict.surveysubject.Grid',
    mainViewSelector: 'surveysubjectgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveysubjectgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'surveysubjectgrid',
            permissionPrefix: 'GkhGji.Dict.SurveySubject',
            permissions: [
                {
                    name: 'Create',
                    applyTo: 'b4addbutton',
                    applyBy: function(component, allowed) {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Delete',
                    applyTo: 'b4deletecolumn',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Edit',
                    applyTo: 'b4savebutton',
                    applyBy: function(component, allowed) {
                        component.setDisabled(!allowed);
                    }
                }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'surveysubjectgrid',
            name: 'surveySubjectGridAspect',
            storeName: 'dict.SurveySubject',
            modelName: 'dict.SurveySubject'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('surveysubjectgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveySubject').load();
    }
});