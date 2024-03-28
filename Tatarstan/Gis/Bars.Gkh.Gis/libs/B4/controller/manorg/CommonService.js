Ext.define('B4.controller.manorg.CommonService', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.manorg.CommonService'
    ],

    models: ['manorg.ManOrgBilCommunalService'],
    stores: ['manorg.ManOrgBilCommunalService'],
    views: [
        'manorg.CommunalServiceEditWindow',
        'manorg.CommunalServiceGrid'
    ],

    refs: [
        { ref: 'mainView', selector: 'manorgcommunalservicegrid' },
        { ref: 'editWindow', selector: 'manorgcommunalserviceeditwindow' }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgacommonserviceperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgCommonServiceGridWindowAspect',
            gridSelector: 'manorgcommunalservicegrid',
            editFormSelector: 'manorgcommunalserviceeditwindow',
            modelName: 'manorg.ManOrgBilCommunalService',
            editWindowView: 'manorg.CommunalServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.set('ManagingOrganization', this.controller.params.id);
                    }
                }
            }
        }
    ],

    params: {},
    
    init: function () {
        var me = this;

        me.control({
            'manorgcommunalserviceeditwindow b4selectfield[name=BilService]': {
                change: me.onBilServiceChange
            }
        });

        me.callParent(arguments);
    },

    onBilServiceChange: function (field, val) {
        var editWindow = this.getEditWindow();

        if (val != null) {
            editWindow.down('textfield[name=OrderNumber]').setValue(val.OrderNumber);
            editWindow.down('checkbox[name=IsOdnService]').setValue(val.IsOdnService);
        } else {
            editWindow.down('textfield[name=OrderNumber]').setValue('');
            editWindow.down('checkbox[name=IsOdnService]').setValue(false);
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgcommunalservicegrid'),
            editWindow = me.getEditWindow() || Ext.widget('manorgcommunalserviceeditwindow');

        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        view.getStore().filter('manorgId', id);
        editWindow.down('b4selectfield[name=BilService]').getStore().filter('manorgId', id);
    }
});