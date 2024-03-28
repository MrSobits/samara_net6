Ext.define('B4.controller.dict.WallMaterial', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'wallMaterialGrid',
            permissionPrefix: 'Gkh.Dictionaries.WallMaterial'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'wallMaterialGridAspect',
            storeName: 'dict.WallMaterial',
            modelName: 'dict.WallMaterial',
            gridSelector: 'wallMaterialGrid'
        }
    ],

    models: ['dict.WallMaterial'],
    stores: ['dict.WallMaterial'],
    views: ['dict.wallmaterial.Grid'],

    mainView: 'dict.wallmaterial.Grid',
    mainViewSelector: 'wallMaterialGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'wallMaterialGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('wallMaterialGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.WallMaterial').load();
    }
});