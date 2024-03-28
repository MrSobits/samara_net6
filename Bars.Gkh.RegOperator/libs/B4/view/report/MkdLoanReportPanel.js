Ext.define('B4.view.report.MkdLoanReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.mkdLoanReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'LoanStatus',
                    itemId: 'tfLoanStatus',
                    fieldLabel: 'Статус',
                    emptyText: 'Все',
                    editable: false,
                    displayField: 'Name',
                    valueField: 'Id',
                    storeAutoLoad: false,
                    emptyItem: { Name: '-' },
                    url: '/State/GetListByType',
                    listeners: {
                        storebeforeload: function (field, store, options) {
                            options.params.typeId = 'gkh_regop_reality_object_loan';
                        }
                    }
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'CrPrograms',
                    itemId: 'tfCrPrograms', 
                    fieldLabel: 'Краткосрочная программа',
                    emptyText: 'Все программы'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальный район',
                    emptyText: 'Все МР'
                }
            ],
            scope: me
        });

        me.callParent(arguments);
    }
});