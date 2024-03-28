Ext.define('B4.controller.dict.TypeFactViolation', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.TypeFactViolation'],
    stores: ['dict.TypeFactViolation'],

    views: ['dict.typefactviolation.Grid'],

    mainView: 'dict.typefactviolation.Grid',
    mainViewSelector: 'typefactviolationgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typefactviolationgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typefactviolationgrid',
            permissionPrefix: 'GkhGji.Dict.TypeFactViolation'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'typefactviolationgrid',
            name: 'typeFactViolationGridAspect',
            storeName: 'dict.TypeFactViolation',
            modelName: 'dict.TypeFactViolation'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typefactviolationgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeFactViolation').load();
    }
});