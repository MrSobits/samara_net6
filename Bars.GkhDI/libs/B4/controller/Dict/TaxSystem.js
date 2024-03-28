Ext.define('B4.controller.dict.TaxSystem', {
    extend: 'B4.base.Controller',
    requires: [
         'B4.aspects.InlineGrid',
         'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.TaxSystem'],
    stores: ['dict.TaxSystem'],
    views: ['dict.taxsystem.Grid'],

    mixins: {
        context: 'B4.mixins.Context'
    },
    mainView: 'dict.taxsystem.Grid',
    mainViewSelector: 'taxSystemGrid',
    refs: [{
        ref: 'mainView',
        selector: 'taxSystemGrid'
    }],

    aspects: [
         {
             xtype: 'inlinegridpermissionaspect',
             gridSelector: 'taxSystemGrid',
             permissionPrefix: 'GkhDi.Dict.TaxSystem'
         },
         {
             xtype: 'inlinegridaspect',
             name: 'taxSystemGridAspect',
             storeName: 'dict.TaxSystem',
             modelName: 'dict.TaxSystem',
             gridSelector: 'taxSystemGrid'
         }],

    index: function () {
        var view = this.getMainView() || Ext.widget('taxSystemGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TaxSystem').load();
    }
});