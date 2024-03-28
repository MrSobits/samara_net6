Ext.define('B4.controller.dict.CompetentOrgGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.CompetentOrgGji'],
    stores: ['dict.CompetentOrgGji'],

    views: ['dict.competentorggji.Grid'],

    mainView: 'dict.competentorggji.Grid',
    mainViewSelector: 'competentOrgGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'competentOrgGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'competentOrgGjiGrid',
            permissionPrefix: 'GkhGji.Dict.CompetentOrg'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'competentOrgGjiGridAspect',
            storeName: 'dict.CompetentOrgGji',
            modelName: 'dict.CompetentOrgGji',
            gridSelector: 'competentOrgGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('competentOrgGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.CompetentOrgGji').load();
    }
});