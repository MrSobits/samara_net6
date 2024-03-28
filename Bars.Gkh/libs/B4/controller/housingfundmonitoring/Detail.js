Ext.define('B4.controller.housingfundmonitoring.Detail',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['housingfundmonitoring.HousingFundMonitoringPeriod'],

    views: ['housingfundmonitoring.EditPanel'],

    mainView: 'housingfundmonitoring.EditPanel',
    mainViewSelector: 'housingfundmonitoringeditpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'housingfundmonitoringeditpanel'
        },
        {
            ref: 'infoGrid',
            selector: 'housingfundmonitoringinfogrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'housingfundmonitoringinfoGridAspect',
            storeName: 'housingfundmonitoring.HousingFundMonitoringInfo',
            modelName: 'housingfundmonitoring.HousingFundMonitoringInfo',
            gridSelector: 'housingfundmonitoringinfogrid',
            saveButtonSelector: 'housingfundmonitoringinfogrid b4savebutton'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.HousingFundMonitoringPeriod.Edit', applyTo: 'b4savebutton', selector: 'housingfundmonitoringinfogrid' }
            ]
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'HousingFundMonitoringInfoPrintAspect',
            buttonSelector: 'gkhbuttonprint[name=PrintButton]',
            codeForm: 'HousingFundMonitoringInfoReport',
            printController: 'GkhReport',
            printAction: 'ReportPrint',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = {
                        reportId: 'HousingFundMonitoringInfoReport',
                        periodId: me.controller.getContextValue(view, 'periodId')
                    };

                me.params.userParams = Ext.JSON.encode(param);
            }
        }
    ],

    init: function() {
        var me = this,
            actions = {};

        actions['housingfundmonitoringinfogrid'] = { 'store.beforeload': { fn: me.onStoreBeforeLoad, me: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('housingfundmonitoringeditpanel');
        
        me.getAspect('HousingFundMonitoringInfoPrintAspect').loadReportStore();

        me.bindContext(view);
        me.setContextValue(view, 'periodId', id);
        me.application.deployView(view);

        me.loadFormData();
        me.getInfoGrid().getStore().load();
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
            model = me.getModel('housingfundmonitoring.HousingFundMonitoringPeriod'),
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