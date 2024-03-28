Ext.define('B4.controller.dict.ExpertGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ExpertGji'],
    stores: ['dict.ExpertGji'],

    views: ['dict.expertgji.Grid'],

    mainView: 'dict.expertgji.Grid',
    mainViewSelector: 'expertGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'expertGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'expertGjiGrid',
            permissionPrefix: 'GkhGji.Dict.Expert'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'expertGjiGridAspect',
            storeName: 'dict.ExpertGji',
            modelName: 'dict.ExpertGji',
            gridSelector: 'expertGjiGrid',
            requiredFields: true
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('expertGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ExpertGji').load();
    }
});