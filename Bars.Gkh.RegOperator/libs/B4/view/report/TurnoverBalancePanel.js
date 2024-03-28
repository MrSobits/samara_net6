Ext.define('B4.view.report.TurnoverBalancePanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.turnoverbalancepanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.municipality.MoArea',
        'B4.store.regop.ClosedChargePeriod'
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
                    xtype: 'b4selectfield',
                    itemId: 'tfMunicipality',
                    store: 'B4.store.dict.municipality.MoArea',
                    fieldLabel: 'Муниципальное образование',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.regop.ClosedChargePeriod',
                    itemId: 'sfStartPeriod',
                    textProperty: 'Name',
                    editable: false,
                    allowBlank: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }}],
                    fieldLabel: 'Период начала'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.regop.ClosedChargePeriod',
                    itemId: 'sfEndPeriod',
                    textProperty: 'Name',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                    fieldLabel: 'Период окончания'
                }
            ]
        });
        me.callParent(arguments);
    }
});