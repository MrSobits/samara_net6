Ext.define('B4.controller.report.RepairContributionInfo', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RepairContributionInfoPanel',
    mainViewSelector: 'repaircontributioninfopanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.ux.button.Update'
    ],

    stores: [
        'report.RepairContributionInfoRobject',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'repaircontributioninfoMultiselectwindowaspect',
            fieldSelector: 'repaircontributioninfopanel [name=Robjects]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#repaircontributioninfoselectwindow',
            storeSelect: 'report.RepairContributionInfoRobject',
            storeSelected: 'realityobj.RealityObjectForSelected',
            textProperty: 'Address',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function(store, operation) {
                var panel = this.controller.getMainView(),
                    dfReportDate = panel.down('[name=ReportDate]'),
                    cbKindAcc = panel.down('[name=KindAccount]'),
                    cbAccOwner = panel.down('[name=AccountOwner]');

                operation.params = operation.params || {};

                operation.params.reportDate = dfReportDate.getValue();
                operation.params.kindAccount = cbKindAcc.getValue();
                operation.params.accOwner = cbAccOwner.getValue();
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'repaircontributioninfopanel b4combobox[name=KindAccount]': {
                    'change': { fn: me.changeKindAccount, scope: me }
                }
            };
        
        me.control(actions);
        me.callParent(arguments);
    },

    changeKindAccount: function (cmp, newValue) {
        var cbAccountOwner = cmp.up().down('[name=AccountOwner]');

        if (newValue === 1) {
            cbAccountOwner.setDisabled(true);
            cbAccountOwner.setValue(4);
        } else {
            cbAccountOwner.setDisabled(false);
        }
    },

    getParams: function () {
        var panel = this.getMainView(),
            dfReportDate = panel.down('[name=ReportDate]'),
            cbKindAcc = panel.down('[name=KindAccount]'),
            cbAccOwner = panel.down('[name=AccountOwner]'),
            sfRegop = panel.down('[name=RegOperator]'),
            trfRobject = panel.down('[name=Robjects]');

        return {
            reportDate: (dfReportDate ? dfReportDate.getValue() : null),
            kindAccount: (cbKindAcc ? cbKindAcc.getValue() : null),
            accOwner: (cbAccOwner ? cbAccOwner.getValue() : null),
            regopId: (sfRegop ? sfRegop.getValue() : null),
            roIds: (trfRobject ? trfRobject.getValue() : null)
        };
    }
});