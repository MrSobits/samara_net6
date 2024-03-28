Ext.define('B4.controller.fuelinfo.FuelInfoDetail',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['fuelinfo.FuelInfoPeriod'],

    views: ['fuelinfo.EditPanel'],

    mainView: 'fuelinfo.EditPanel',
    mainViewSelector: 'fuelinfoeditpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'fuelinfoeditpanel'
        },
        {
            ref: 'fuelAmountInfoGrid',
            selector: 'fuelamountinfogrid'
        },
        {
            ref: 'fuelExtractionDistanceInfoGrid',
            selector: 'fuelextractiondistanceinfogrid'
        },
        {
            ref: 'fuelContractObligationInfoGrid',
            selector: 'fuelcontractobligationinfogrid'
        },
        {
            ref: 'fuelEnergyDebtInfoGrid',
            selector: 'fuelenergydebtinfogrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'fuelAmountInfoGridAspect',
            storeName: 'fuelinfo.FuelAmountInfo',
            modelName: 'fuelinfo.FuelAmountInfo',
            gridSelector: 'fuelamountinfogrid',
            saveButtonSelector: 'fuelamountinfogrid b4savebutton'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'fuelExtractionDistanceInfoGridAspect',
            storeName: 'fuelinfo.FuelExtractionDistanceInfo',
            modelName: 'fuelinfo.FuelExtractionDistanceInfo',
            gridSelector: 'fuelextractiondistanceinfogrid',
            saveButtonSelector: 'fuelextractiondistanceinfogrid b4savebutton'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'fuelContractObligationInfoGridAspect',
            storeName: 'fuelinfo.FuelContractObligationInfo',
            modelName: 'fuelinfo.FuelContractObligationInfo',
            gridSelector: 'fuelcontractobligationinfogrid',
            saveButtonSelector: 'fuelcontractobligationinfogrid b4savebutton'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'fuelEnergyDebtInfoGridAspect',
            storeName: 'fuelinfo.FuelEnergyDebtInfo',
            modelName: 'fuelinfo.FuelEnergyDebtInfo',
            gridSelector: 'fuelenergydebtinfogrid',
            saveButtonSelector: 'fuelenergydebtinfogrid b4savebutton'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.HeatSeason.FuelInfoPeriod.Edit', applyTo: 'b4savebutton', selector: 'fuelamountinfogrid' },
                { name: 'GkhGji.HeatSeason.FuelInfoPeriod.Edit', applyTo: 'b4savebutton', selector: 'fuelextractiondistanceinfogrid' },
                { name: 'GkhGji.HeatSeason.FuelInfoPeriod.Edit', applyTo: 'b4savebutton', selector: 'fuelcontractobligationinfogrid' },
                { name: 'GkhGji.HeatSeason.FuelInfoPeriod.Edit', applyTo: 'b4savebutton', selector: 'fuelenergydebtinfogrid' }
            ]
        }
    ],

    init: function() {
        var me = this,
            actions = {};

        actions['fuelamountinfogrid'] = { 'store.beforeload': { fn: me.onStoreBeforeLoad, me: me } };
        actions['fuelextractiondistanceinfogrid'] = { 'store.beforeload': { fn: me.onStoreBeforeLoad, me: me } };
        actions['fuelcontractobligationinfogrid'] = { 'store.beforeload': { fn: me.onStoreBeforeLoad, me: me } };
        actions['fuelenergydebtinfogrid'] = { 'store.beforeload': { fn: me.onStoreBeforeLoad, me: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('fuelinfoeditpanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'periodId', id);
        me.application.deployView(view);

        me.loadFormData();
        me.getFuelAmountInfoGrid().getStore().load();
        me.getFuelExtractionDistanceInfoGrid().getStore().load();
        me.getFuelContractObligationInfoGrid().getStore().load();
        me.getFuelEnergyDebtInfoGrid().getStore().load();
    },

    onStoreBeforeLoad: function (store, operation) {
        var me = this,
            view = me.getMainView(); 

        operation.params.periodId = me.getContextValue(view, 'periodId');
    },

    loadFormData: function () {
        var me = this,
            view = me.getMainView(),
            form = view.down('form[name=periodInfo]'),
            model = me.getModel('fuelinfo.FuelInfoPeriod'),
            id = me.getContextValue(view, 'periodId');

        me.mask(form);

        model.load(id, {
            success: function (rec) {
                form.getForm().setValues(rec.getData());
            },
            callback: function() {
                me.unmask(form);
            },
            scope: me
        });
    }
});