Ext.define('B4.controller.dict.FurtherUse', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.FurtherUse'],
    stores: ['dict.FurtherUse'],
    views: ['dict.furtheruse.Grid'],

    mainView: 'dict.furtheruse.Grid',
    mainViewSelector: 'furtherUseGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'furtherUseGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'furtherUseGrid',
            permissionPrefix: 'Gkh.Dictionaries.FurtherUse'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'furtherUseGridAspect',
            storeName: 'dict.FurtherUse',
            modelName: 'dict.FurtherUse',
            gridSelector: 'furtherUseGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('furtherUseGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.FurtherUse').load();
    }
});