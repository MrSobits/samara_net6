Ext.define('B4.controller.manorg.AdditionService', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.manorg.AdditionService'
    ],

    models: ['manorg.ManOrgBilAdditionService'],
    stores: ['manorg.ManOrgBilAdditionService'],
    views: [
        'manorg.AdditionServiceEditWindow',
        'manorg.AdditionServiceGrid'
    ],

    refs: [
        { ref: 'mainView', selector: 'manorgadditionservicegrid' },
        { ref: 'editWindow', selector: 'manorgadditionserviceeditwindow' }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgadditionserviceperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgAdditionServiceGridWindowAspect',
            gridSelector: 'manorgadditionservicegrid',
            editFormSelector: 'manorgadditionserviceeditwindow',
            modelName: 'manorg.ManOrgBilAdditionService',
            editWindowView: 'manorg.AdditionServiceEditWindow',
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
            'manorgadditionserviceeditwindow b4selectfield[name=BilService]': {
                change: me.onBilServiceChange
            }
        });

        me.callParent(arguments);
    },

    onBilServiceChange: function (field, val) {
        var editWindow = this.getEditWindow();

        if (val != null) {
            editWindow.down('textfield[name=MeasureName]').setValue(val.MeasureName);
        } else {
            editWindow.down('textfield[name=MeasureName]').setValue('');
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgadditionservicegrid'),
            editWindow = me.getEditWindow() || Ext.widget('manorgadditionserviceeditwindow');

        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        
        me.application.deployView(view, 'manorgId_info');

        view.getStore().filter('manorgId', id);
        editWindow.down('b4selectfield[name=BilService]').getStore().filter('manorgId', id);
    }
});