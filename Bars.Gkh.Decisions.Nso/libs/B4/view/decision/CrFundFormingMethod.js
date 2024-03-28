Ext.define('B4.view.decision.CrFundFormingMethod', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.ComboBox'
    ],
    border: false,

    initComponent: function () {
        Ext.apply(this, {
            defaults: {
                hideTrigger: (this.saveable === false),
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
                    url: '/decision/getdecisions/?protocolId=' + this.protocolId + '&decisionTypeCode=' + this.decisionTypeCode,
                    emptyItem: { Name: '-' },
                    displayField: 'name',
                    valueField: 'code',
                    fields: ['name', 'code']
                }
            ]
        });

        this.callParent(arguments);
    }
});