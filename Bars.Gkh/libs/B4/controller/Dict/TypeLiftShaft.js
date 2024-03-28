Ext.define('B4.controller.dict.TypeLiftShaft', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeLiftShaftGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeLiftShaft'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeLiftShaftGridAspect',
            storeName: 'dict.TypeLiftShaft',
            modelName: 'dict.TypeLiftShaft',
            gridSelector: 'typeLiftShaftGrid'
        }
    ],

    models: ['dict.TypeLiftShaft'],
    stores: ['dict.TypeLiftShaft'],
    views: ['dict.typeliftshaft.Grid'],

    mainView: 'dict.typeliftshaft.Grid',
    mainViewSelector: 'typeLiftShaftGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'typeLiftShaftGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeLiftShaftGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeLiftShaft').load();
    }
});