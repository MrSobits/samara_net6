Ext.define('B4.controller.import.RoomImport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhImportAspect',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.RoomImportPanel'],

    mainView: 'import.RoomImportPanel',
    mainViewSelector: 'roomimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'roomimportpanel',
            importId: 'Bars.Gkh.RegOperator.Imports.Room.RoomImport',
            maxFileSize: 52428800,
            getUserParams: function () {
                var me = this;
                me.params = me.params || {};

                me.params['replaceExistRooms'] = me.controller.getMainView().down('checkbox[name=ReplaceExistRooms]').getValue();
                me.params['NoRefreshCash'] = me.controller.getMainView().down('checkbox[name=NoRefreshCash]').getValue();
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Import.RoomImport.ReplaceExistRooms', applyTo: 'checkbox[name=ReplaceExistRooms]', selector: 'roomimportpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});