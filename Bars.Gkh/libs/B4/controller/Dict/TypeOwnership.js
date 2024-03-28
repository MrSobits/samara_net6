Ext.define('B4.controller.dict.TypeOwnership', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeOwnershipGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeOwnership'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeOwnershipGridAspect',
            storeName: 'dict.TypeOwnership',
            modelName: 'dict.TypeOwnership',
            gridSelector: 'typeOwnershipGrid'
        }
    ],

    models: ['dict.TypeOwnership'],
    stores: ['dict.TypeOwnership'],
    views: ['dict.typeownership.Grid'],

    mainView: 'dict.typeownership.Grid',
    mainViewSelector: 'typeOwnershipGrid',

    refs: [{
        ref: 'mainView',
        selector: 'typeOwnershipGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typeOwnershipGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeOwnership').load();
    }
});