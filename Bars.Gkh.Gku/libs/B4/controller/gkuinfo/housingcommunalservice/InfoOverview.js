Ext.define('B4.controller.gkuinfo.housingcommunalservice.InfoOverview', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhEditPanel'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'realityobj.housingcommunalservice.InfoOverview',
        'realityobj.housingcommunalservice.OverallBalance'
    ],
    stores: [
        'realityobj.housingcommunalservice.OverallBalance'
    ],
    views: [
        'gkuinfo.InfoOverviewEditPanel',
        'realityobj.housingcommunalservice.OverallBalanceEditWindow',
        'realityobj.housingcommunalservice.OverallBalanceGrid'
    ],

    mainView: 'gkuinfo.InfoOverviewEditPanel',
    mainViewSelector: 'gkuhseinfoovervieweditpanel',

    refs:[
        {
            ref:'monthField',
            selector: 'hseoverallbalancegrid datefield[name=month]'
        }
    ],

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'hseInfoOverviewAspect',
            editPanelSelector: 'gkuhseinfoovervieweditpanel',
            modelName: 'realityobj.housingcommunalservice.InfoOverview',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('RealityObject', asp.controller.params.realityObjectId);
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
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.RealityObject = this.controller.params.realityObjectId;
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Edit', applyTo: 'b4savebutton', selector: 'gkuhseinfoovervieweditpanel' },
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
        me.getStore('realityobj.housingcommunalservice.OverallBalance').on('beforeload', this.onBeforeLoad, this);

        me.callParent(arguments);
    },

    onMonthSelected: function (dateField) {
        var grid = dateField.up('grid');
        grid.getStore().load();
    },

    onLaunch: function () {
        this.getStore('realityobj.housingcommunalservice.OverallBalance').load();
        
        var aspect = this.getAspect('hseInfoOverviewAspect');
        
        if (aspect && this.params) {
            this.mask('Загрузка', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetHouseInfoOverviewByRealityObjectId', 'HouseInfoOverview', {
                realtyObjectId: this.params.realityObjectId
            })).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                aspect.setData(obj.houseInfoOverviewId);
                this.unmask();
                return true;
            }, this)
           .error(function () {
               Ext.Msg.alert('Сообщение', 'Произошла ошибка');
               this.unmask();
           }, this);
        }
    },

    onBeforeLoad: function (store, operation) {
        var monthField;
        if (this.params) {
            operation.params.realityObjectId = this.params.realityObjectId;
            monthField = this.getMonthField();
            if(monthField){
                operation.params.date = monthField.getValue();
            }
        }
    }
});