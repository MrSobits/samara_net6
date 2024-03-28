Ext.define('B4.view.decision.AccountOwner', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.ComboBox'
    ],
    border: false,
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                hideTrigger: (me.saveable === false),
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата ввода в действие решения'
                },
                {
                    xtype: 'b4combobox',
                    allowBlank: false,
                    fieldLabel: 'Принятое решение',
                    name: 'DecisionCode',
                    url: '/decision/getdecisions/?protocolId=' + me.protocolId + '&decisionTypeCode=' + me.decisionTypeCode,
                    emptyItem: { Name: '-' },
                    displayField: 'name',
                    valueField: 'code',
                    fields: ['name', 'code']
                },
                {
                    xtype: 'textfield',
                    name: 'OwnerName',
                    fieldLabel: 'Наименование владельца счета'
                }
            ]
        });

        me.callParent(arguments);
    }
});