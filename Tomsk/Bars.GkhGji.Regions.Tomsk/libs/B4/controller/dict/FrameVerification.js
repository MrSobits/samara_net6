Ext.define('B4.controller.dict.FrameVerification', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.FrameVerification'],
    stores: ['dict.FrameVerification'],

    views: ['dict.frameverification.Grid'],

    mainView: 'dict.frameverification.Grid',
    mainViewSelector: 'frameverificationGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'frameverificationGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'frameverificationGrid',
            permissionPrefix: 'GkhGji.Dict.FrameVerification'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'frameverificationGridAspect',
            storeName: 'dict.FrameVerification',
            modelName: 'dict.FrameVerification',
            gridSelector: 'frameverificationGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('frameverificationGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.FrameVerification').load();
    }
});