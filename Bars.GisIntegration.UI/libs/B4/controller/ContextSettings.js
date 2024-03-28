Ext.define('B4.controller.ContextSettings', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'ContextSettings'
    ],
    stores: [
        'ContextSettings'
    ],
    views: [
        'contextsettings.Grid',
        'contextsettings.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'contextsettings.Grid',
    mainViewSelector: 'contextsettingsGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'contextsettingsGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'contextsettingsGridWindowAspect',
            gridSelector: 'contextsettingsGrid',
            editFormSelector: 'contextsettingseditwindow',
            storeName: 'ContextSettings',
            modelName: 'ContextSettings',
            editWindowView: 'contextsettings.EditWindow',

            listeners: {
                aftersetformdata: function(asp, record) {
                    asp.getForm().down('b4combobox[name=FileStorageName]').setReadOnly(!record.phantom);
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('contextsettingsGrid');
        me.bindContext(view);
        me.getStore('ContextSettings').load();
    }
});