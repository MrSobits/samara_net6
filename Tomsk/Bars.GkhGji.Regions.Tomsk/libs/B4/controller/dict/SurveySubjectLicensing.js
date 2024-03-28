Ext.define('B4.controller.dict.SurveySubjectLicensing', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SurveySubjectLicensing'],
    stores: ['dict.SurveySubjectLicensing'],

    views: ['dict.surveysubject.LicensingGrid'],

    mainView: 'dict.surveysubject.LicensingGrid',
    mainViewSelector: 'licensinggrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licensinggrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'licensinggrid',
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
            gridSelector: 'licensinggrid',
            name: 'surveySubjectGridAspect',
            storeName: 'dict.SurveySubjectLicensing',
            modelName: 'dict.SurveySubjectLicensing'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('licensinggrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveySubjectLicensing').load();
    }
});