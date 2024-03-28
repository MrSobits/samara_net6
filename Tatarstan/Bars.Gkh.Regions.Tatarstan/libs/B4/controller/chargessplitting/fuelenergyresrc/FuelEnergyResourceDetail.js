Ext.define('B4.controller.chargessplitting.fuelenergyresrc.FuelEnergyResourceDetail', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.StateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    views: [
        'chargessplitting.fuelenergyresrc.detail.Panel',
        'chargessplitting.fuelenergyresrc.detail.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'chargessplitting.fuelenergyresrc.ServiceOrgFuelEnergyResourcePeriodSumm'
    ],

    mainView: 'chargessplitting.fuelenergyresrc.detail.Panel',
    mainViewSelector: 'fuelenergyresourcedetailpanel',

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'FuelEnergyResourceDetailGridAspect',
            storeName: 'chargessplitting.fuelenergyresrc.FuelEnergyOrgContractInfo',
            modelName: 'chargessplitting.fuelenergyresrc.FuelEnergyOrgContractInfo',
            gridSelector: 'fuelenergyresourcedetailgrid',
            saveButtonSelector: 'fuelenergyresourcedetailpanel b4savebutton'
        },
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'fuelenergyresourcedetailgrid',
            permissionPrefix: 'Gkh.ChargesSplitting.FuelEnergyResource'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'FuelEnergyResourceDetailPrintAspect',
            buttonSelector: 'gkhbuttonprint[name=PrintButton]',
            codeForm: '',
            printController: 'GkhReport',
            printAction: 'ReportPrint',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = {
                        reportId: '',
                        periodSummId: me.controller.getContextValue(view, 'periodSummId')
                    };

                me.params.userParams = Ext.JSON.encode(param);
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['fuelenergyresourcedetailgrid'] = {
            'store.beforeload': { fn: me.onBeforeLoadStore, scope: me },
            'store.load': { fn: me.loadFormData, scope: me }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('fuelenergyresourcedetailpanel');

        me.getAspect('FuelEnergyResourceDetailPrintAspect').loadReportStore();

        me.bindContext(view);
        me.setContextValue(view, 'periodSummId', id);
        me.application.deployView(view);

        view.down('fuelenergyresourcedetailgrid').getStore().load();
    },

    onBeforeLoadStore: function (store, operation) {
        var me = this,
            view = me.getMainView();

        operation.params.periodSummId = me.getContextValue(view, 'periodSummId');
    },

    loadFormData: function () {
        var me = this,
            view = me.getMainView(),
            form = view.down('panel[name=fuelEnergyResourceContractForm]'),
            model = me.getModel('chargessplitting.fuelenergyresrc.ServiceOrgFuelEnergyResourcePeriodSumm'),
            id = me.getContextValue(view, 'periodSummId');

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