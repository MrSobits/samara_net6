Ext.define('B4.controller.dict.CourtVerdictGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.CourtVerdictGji'],
    stores: ['dict.CourtVerdictGji'],

    views: ['dict.courtverdictgji.Grid'],

    mainView: 'dict.courtverdictgji.Grid',
    mainViewSelector: 'courtVerdictGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'courtVerdictGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'courtVerdictGjiGrid',
            permissionPrefix: 'GkhGji.Dict.CourtVerdict'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'courtVerdictGjiGridAspect',
            storeName: 'dict.CourtVerdictGji',
            modelName: 'dict.CourtVerdictGji',
            gridSelector: 'courtVerdictGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('courtVerdictGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.CourtVerdictGji').load();
    }
});