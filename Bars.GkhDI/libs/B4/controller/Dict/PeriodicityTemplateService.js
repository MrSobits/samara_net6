Ext.define('B4.controller.dict.PeriodicityTemplateService', {
    extend: 'B4.base.Controller',
    requires: [
       'B4.aspects.InlineGrid',
       'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.PeriodicityTemplateService'],
    stores: ['dict.PeriodicityTemplateService'],
    views: ['dict.periodicitytemplateservice.Grid'],

    mixins: {
        context: 'B4.mixins.Context'
    },
    mainView: 'dict.periodicitytemplateservice.Grid',
    mainViewSelector: 'periodicityTemplateServiceGrid',

    refs: [{
        ref: 'mainView',
        selector: 'periodicityTemplateServiceGrid'
    }],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'periodicityTemplateServiceGrid',
            permissionPrefix: 'GkhDi.Dict.PeriodicityTempServ'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'periodicityTemplateServiceGridAspect',
            storeName: 'dict.PeriodicityTemplateService',
            modelName: 'dict.PeriodicityTemplateService',
            gridSelector: 'periodicityTemplateServiceGrid'
        }],

    index: function () {
        var view = this.getMainView() || Ext.widget('periodicityTemplateServiceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PeriodicityTemplateService').load();
    }
});