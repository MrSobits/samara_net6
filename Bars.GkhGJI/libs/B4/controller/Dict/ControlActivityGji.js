Ext.define('B4.controller.dict.ControlActivityGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ControlActivityGji'],
    stores: ['dict.ControlActivityGji'],

    views: ['dict.controlactivity.Grid'],

    mainView: 'dict.controlactivity.Grid',
    mainViewSelector: 'controlactivitygji',

    refs: [
        {
            ref: 'mainView',
            selector: 'controlactivitygji'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'controlactivitygji',
            permissionPrefix: 'GkhGji.Dict.ControlActivity'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'controlActivityGridAspect',
            storeName: 'dict.ControlActivityGji',
            modelName: 'dict.ControlActivityGji',
            gridSelector: 'controlactivitygji'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('controlactivitygji');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ControlActivityGji').load();
    }
});