Ext.define('B4.view.report.RepairContributionInfoPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.repaircontributioninfopanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.ManagingOrganization',
        'B4.enums.CrFundFormationDecisionType',
        'B4.enums.AccountOwnerDecisionType'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'ReportDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4combobox',
                    name: 'KindAccount',
                    items: B4.enums.CrFundFormationDecisionType.getItems(),
                    fieldLabel: 'Вид счета',
                    allowBlank: false,
                    editable: false,
                    value: 1
                },
                {
                    xtype: 'b4combobox',
                    name: 'AccountOwner',
                    items: B4.enums.AccountOwnerDecisionType.getItems(),
                    fieldLabel: 'Владелец специального счета',
                    allowBlank: false,
                    editable: false,
                    value: 4,
                    disabled: true
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RegOperator',
                    store: 'B4.store.RegOperator',
                    fieldLabel: 'Рег. оператор',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    textProperty: 'Contragent'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Robjects',
                    fieldLabel: 'Жилые дома',
                    emptyText: 'Выберите дом',
                    allowBlank: false,
                    editable: false
                }
            ]
        });

        me.callParent(arguments);
    }
});