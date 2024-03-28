Ext.define('B4.controller.dict.InstanceGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.InstanceGji'],
    stores: ['dict.InstanceGji'],

    views: ['dict.instancegji.Grid'],

    mainView: 'dict.instancegji.Grid',
    mainViewSelector: 'instanceGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'instanceGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'instanceGjiGrid',
            permissionPrefix: 'GkhGji.Dict.Instance'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'instanceGjiGridAspect',
            storeName: 'dict.InstanceGji',
            modelName: 'dict.InstanceGji',
            gridSelector: 'instanceGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('instanceGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.InstanceGji').load();
    }
});