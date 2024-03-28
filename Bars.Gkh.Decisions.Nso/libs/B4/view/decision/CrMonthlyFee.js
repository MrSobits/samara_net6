Ext.define('B4.view.decision.CrMonthlyFee', {
    extend: 'Ext.form.Panel',

    border: false,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        Ext.apply(this, {
            items: [
                {
                    xtype: 'datefield',
                    labelWidth: 250,
                    name: 'StartDate',
                    fieldLabel: 'Дата ввода в действие решения',
                    hideTrigger: (this.saveable === false)
                }
            ]
        });

        this.callParent(arguments);
    },

    afterShow: function () {
        this.add({
            xtype: 'fieldcontainer',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            defaults: {
                flex: 1,
                labelWidth: 160,
                width: 220,
                hideTrigger: true,
                minValue: 0,
                maxValue: 100
            },
            items: [
                {
                    xtype: 'numberfield',
                    readOnly: true,
                    fieldLabel: 'Установленная норма, %',
                    name: 'Norm'
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Принятое решение, %',
                    labelAlign: 'right',
                    name: 'Decision'
                }
            ]
        });
    }
});