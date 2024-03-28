Ext.define('B4.controller.dict.RoofingMaterial', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'roofingMaterialGrid',
            permissionPrefix: 'Gkh.Dictionaries.RoofingMaterial'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'roofingMaterialGridAspect',
            storeName: 'dict.RoofingMaterial',
            modelName: 'dict.RoofingMaterial',
            gridSelector: 'roofingMaterialGrid'
        }
    ],

    models: ['dict.RoofingMaterial'],
    stores: ['dict.RoofingMaterial'],
    views:['dict.roofingmaterial.Grid'],

    mainView: 'dict.roofingmaterial.Grid',
    mainViewSelector: 'roofingMaterialGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'roofingMaterialGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('roofingMaterialGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RoofingMaterial').load();
    }
});