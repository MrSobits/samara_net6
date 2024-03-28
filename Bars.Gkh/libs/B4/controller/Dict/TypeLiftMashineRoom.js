Ext.define('B4.controller.dict.TypeLiftMashineRoom', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeLiftMashineRoomGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeLiftMashineRoom'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeLiftMashineRoomGridAspect',
            storeName: 'dict.TypeLiftMashineRoom',
            modelName: 'dict.TypeLiftMashineRoom',
            gridSelector: 'typeLiftMashineRoomGrid'
        }
    ],

    models: ['dict.TypeLiftMashineRoom'],
    stores: ['dict.TypeLiftMashineRoom'],
    views: ['dict.typeliftmashineroom.Grid'],

    mainView: 'dict.typeliftmashineroom.Grid',
    mainViewSelector: 'typeLiftMashineRoomGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'typeLiftMashineRoomGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeLiftMashineRoomGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeLiftMashineRoom').load();
    }
});