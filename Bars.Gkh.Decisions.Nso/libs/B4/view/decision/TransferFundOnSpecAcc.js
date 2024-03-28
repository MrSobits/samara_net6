Ext.define('B4.view.decision.TransferFundOnSpecAcc', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.FileField'
    ],

    border: false,

    initComponent: function () {
        Ext.apply(this, {
            defaults: {
                hideTrigger: (this.saveable === false),
                labelWidth: 200,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата ввода в действие решения'
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Документ',
                    name: 'File'
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Сумма',
                    name: 'Sum',
                    hideTrigger: true
                }
            ]
        });

        this.callParent(arguments);
    }
});