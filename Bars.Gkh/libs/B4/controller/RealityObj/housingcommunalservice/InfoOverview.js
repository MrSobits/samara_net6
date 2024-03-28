Ext.define('B4.controller.realityobj.housingcommunalservice.InfoOverview', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.realityobj.housingcommunalservice.InfoOverview'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.housingcommunalservice.InfoOverview',
        'realityobj.housingcommunalservice.OverallBalance'
    ],
    
    stores: [
        'realityobj.housingcommunalservice.OverallBalance'
    ],
    
    views: [
        'realityobj.housingcommunalservice.InfoOverviewEditPanel',
        'realityobj.housingcommunalservice.OverallBalanceEditWindow',
        'realityobj.housingcommunalservice.OverallBalanceGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'hseinfoovervieweditpanel'
        },
        {
            ref:'monthField',
            selector: 'hseoverallbalancegrid datefield[name=month]'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'infooverviewperm',
            name: 'infoOverviewPerm'
        },
        {
            xtype: 'gkheditpanel',
            name: 'hseInfoOverviewAspect',
            editPanelSelector: 'hseinfoovervieweditpanel',
            modelName: 'realityobj.housingcommunalservice.InfoOverview',
            listeners: {
                getdata: function (me, record) {
                    if (!record.get('Id')) {
                        record.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'hseOverallBalanceGridWindowAspect',
            gridSelector: 'hseoverallbalancegrid',
            editFormSelector: 'hseoverallbalanceeditwindow',
            storeName: 'realityobj.housingcommunalservice.OverallBalance',
            modelName: 'realityobj.housingcommunalservice.OverallBalance',
            editWindowView: 'realityobj.housingcommunalservice.OverallBalanceEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Edit', applyTo: 'b4savebutton', selector: 'hseinfoovervieweditpanel' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Create', applyTo: 'b4addbutton', selector: 'hseoverallbalancegrid' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Edit', applyTo: 'b4savebutton', selector: 'hseoverallbalanceeditwindow' },
                {
                    name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Delete', applyTo: 'b4deletecolumn', selector: 'hseoverallbalancegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'hseoverallbalancegrid datefield[name=month]': {
                change: me.onMonthSelected
            }
        });
        me.getStore('realityobj.housingcommunalservice.OverallBalance').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onMonthSelected: function (dateField) {
        var grid = dateField.up('grid');
        grid.getStore().load();
    },

    index: function (id) {
        var me = this,
            aspect, obj,
            view = me.getMainView() || Ext.widget('hseinfoovervieweditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getAspect('infoOverviewPerm').setPermissionsByRecord({ getId: function () { return id; } });
        me.getStore('realityobj.housingcommunalservice.OverallBalance').load();
        
        aspect = me.getAspect('hseInfoOverviewAspect');
        
        if (aspect && me.params) {
            me.mask('Загрузка', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetHouseInfoOverviewByRealityObjectId', 'HouseInfoOverview', {
                realtyObjectId: id
            })).next(function (response) {
                obj = Ext.JSON.decode(response.responseText);
                aspect.setData(obj.houseInfoOverviewId);
                me.unmask();
                return true;
            }, me)
           .error(function () {
               Ext.Msg.alert('Сообщение', 'Произошла ошибка');
               me.unmask();
           }, me);
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            monthField;
        
        operation.params.realityObjectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
        monthField = me.getMonthField();
        if(monthField){
            operation.params.date = monthField.getValue();
        }
    }
});