Ext.define('B4.controller.ServiceSettings', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'ServiceSettings'
    ],
    stores: [
        'ServiceSettings'
    ],
    views: [
        'servicesettings.Grid',
        'servicesettings.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'servicesettings.Grid',
    mainViewSelector: 'servicesettingsGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'servicesettingsGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'servicesettingsGridWindowAspect',
            gridSelector: 'servicesettingsGrid',
            editFormSelector: 'servicesettingseditwindow',
            storeName: 'ServiceSettings',
            modelName: 'ServiceSettings',
            editWindowView: 'servicesettings.EditWindow',

            listeners: {
                aftersetformdata: function(asp, record) {
                    asp.getForm().down('b4combobox[name=IntegrationService]').setReadOnly(!record.phantom);
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('servicesettingsGrid');
        me.bindContext(view);
        me.getStore('ServiceSettings').load();
    }
});