Ext.define('B4.controller.regop.realty.RealtySubsidyAccount', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.model.RealityObjectSubsidyAccountOperation',
        'B4.model.regop.realty.RealtyObjectSubsidyAccountProxy',
        'B4.aspects.FormPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GridEditWindow'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'regop.realty.RealtySubsidyAccountPanel'
    ],
    
    models: [
        'RealityObjectSubsidyAccountOperation',
        'regop.realty.RealtyObjectSubsidyAccountProxy'
    ],
    
    stores: [
        'regop.realty.RealtyFactSubsidyAccountOperation'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realtysubsidyaccpanel'
        },
        {
            ref: 'planSubsidyGrid',
            selector: 'realtysubsidyaccpanel realtyplansubsidyoperationgrid'
        },
        {
            ref: 'factSubsidyGrid',
            selector: 'realtysubsidyaccpanel realtysubsidytransfergrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'formpanel',
            modelName: 'regop.realty.RealtyObjectSubsidyAccountProxy',
            formPanelSelector: 'realtysubsidyaccpanel form',
            objectId: function () {
                var me = this;
                return me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
            },
            afterLoadRecord: function (asp, rec) {
                var planSubsidyStore = asp.controller.getPlanSubsidyGrid().getStore(),
                    factSubsidyStore = asp.controller.getFactSubsidyGrid().getStore();
                
                planSubsidyStore.clearFilter(true);
                factSubsidyStore.clearFilter(true);
                planSubsidyStore.filter([{ property: 'accId', value: rec.get('Id') }]);
                factSubsidyStore.filter([{ property: 'accId', value: rec.get('Id') }]);
            },
            name: 'formpanel'
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'realtysubsidyaccpanel': {
                updateme: me.updatePanel
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = this.getMainView() || Ext.widget('realtysubsidyaccpanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    },

    updatePanel: function() {
        var me = this,
            asp = me.getAspect('formpanel');

        asp.controller = me;
        asp.loadRecord();
    }
});