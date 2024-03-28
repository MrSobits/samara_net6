Ext.define('B4.controller.dict.SurveyPurpose', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SurveyPurpose'],
    stores: ['dict.SurveyPurpose'],

    views: ['dict.surveypurpose.Grid'],

    mainView: 'dict.surveypurpose.Grid',
    mainViewSelector: 'surveypurposegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveypurposegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'surveypurposegrid',
            permissionPrefix: 'GkhGji.Dict.SurveyPurpose'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'surveypurposegrid',
            name: 'surveyPurposeGridAspect',
            storeName: 'dict.SurveyPurpose',
            modelName: 'dict.SurveyPurpose'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('surveypurposegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveyPurpose').load();
    }
});