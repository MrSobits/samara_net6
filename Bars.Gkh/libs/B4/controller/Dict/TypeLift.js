Ext.define('B4.controller.dict.TypeLift', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeLiftGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeLift'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeLiftGridAspect',
            storeName: 'dict.TypeLift',
            modelName: 'dict.TypeLift',
            gridSelector: 'typeLiftGrid'
        }
    ],

    models: ['dict.TypeLift'],
    stores: ['dict.TypeLift'],
    views: ['dict.typelift.Grid'],

    mainView: 'dict.typelift.Grid',
    mainViewSelector: 'typeLiftGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'typeLiftGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeLiftGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeLift').load();
    }
});