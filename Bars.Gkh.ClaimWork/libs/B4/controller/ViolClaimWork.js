Ext.define('B4.controller.ViolClaimWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['claimwork.ViolClaimWork'],
    stores: ['claimwork.ViolClaimWork'],
    views: ['violclaimwork.Grid'],

    mainView: 'violclaimwork.Grid',
    mainViewSelector: 'violclaimworkGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'violclaimworkGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'violclaimworkGrid',
            permissionPrefix: 'Clw.Dictionaries.ViolClaimWork'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'violClaimWorkGridAspect',
            storeName: 'claimwork.ViolClaimWork',
            modelName: 'claimwork.ViolClaimWork',
            gridSelector: 'violclaimworkGrid'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('violclaimworkGrid');
        
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('claimwork.ViolClaimWork').load();
    }
});