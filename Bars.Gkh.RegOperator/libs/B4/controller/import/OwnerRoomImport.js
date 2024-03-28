Ext.define('B4.controller.import.OwnerRoomImport', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.RoomImportPanel'],

    mainView: 'import.OwnerRoomImportPanel',
    mainViewSelector: 'ownerroomimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'ownerroomimportpanel',
            importId: 'Bars.Gkh.RegOperator.Imports.OwnerRoom.OwnerRoomImport',
            maxFileSize: 52428800
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});