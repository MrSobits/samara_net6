Ext.define('B4.controller.dict.SanctionGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SanctionGji'],
    stores: ['dict.SanctionGji'],

    views: ['dict.sanctiongji.Grid'],

    mainView: 'dict.sanctiongji.Grid',
    mainViewSelector: 'sanctionGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'sanctionGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'sanctionGjiGrid',
            permissionPrefix: 'GkhGji.Dict.Sanction'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'sanctionGjiGridAspect',
            storeName: 'dict.SanctionGji',
            modelName: 'dict.SanctionGji',
            gridSelector: 'sanctionGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('sanctionGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SanctionGji').load();
    }
});