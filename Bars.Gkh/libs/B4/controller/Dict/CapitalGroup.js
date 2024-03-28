Ext.define('B4.controller.dict.CapitalGroup', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.CapitalGroup'],
    stores: ['dict.CapitalGroup'],
    views: ['dict.capitalgroup.Grid'],

    mainView: 'dict.capitalgroup.Grid',
    mainViewSelector: 'capitalGroupGrid',

    refs: [{
        ref: 'mainView',
        selector: 'capitalGroupGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'capitalGroupGrid',
            permissionPrefix: 'Gkh.Dictionaries.CapitalGroup'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'capitalGroupGridAspect',
            storeName: 'dict.CapitalGroup',
            modelName: 'dict.CapitalGroup',
            gridSelector: 'capitalGroupGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('capitalGroupGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.CapitalGroup').load();
    }
});