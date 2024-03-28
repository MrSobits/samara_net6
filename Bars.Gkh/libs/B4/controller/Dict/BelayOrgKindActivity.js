Ext.define('B4.controller.dict.BelayOrgKindActivity', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.BelayOrgKindActivity'],
    stores: ['dict.BelayOrgKindActivity'],
    views: ['dict.belayorgkindactivity.Grid'],

    mainView: 'dict.belayorgkindactivity.Grid',
    mainViewSelector: 'belayOrgKindActivityGrid',

    refs: [{
        ref: 'mainView',
        selector: 'belayOrgKindActivityGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: '#belayOrgKindActivityGrid',
            permissionPrefix: 'Gkh.Dictionaries.BelayOrgKindActivity'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'belayOrgKindActivityGridAspect',
            storeName: 'dict.BelayOrgKindActivity',
            modelName: 'dict.BelayOrgKindActivity',
            gridSelector: '#belayOrgKindActivityGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('belayOrgKindActivityGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.BelayOrgKindActivity').load();
    }
});