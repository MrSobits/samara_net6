Ext.define('B4.view.decision.MinimalCrFundSize', {
    extend: 'Ext.form.Panel',

    border: false,
    defaults: {
        margins: '5 5 0 5',
        labelWidth: 200
    },
    initComponent: function() {
        Ext.apply(this, {
            items: [
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата ввода в действие решения',

                    hideTrigger: (this.saveable === false)
                },
                {
                    xtype: 'fieldcontainer',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Установленная норма, р',
                            labelWidth: 150,
                            name: 'Norm',
                            margins: '5 10 5 0'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Принятое решение, р',
                            labelWidth: 135,
                            name: 'Decision',
                            margins: '5 5 5 10'
                        }
                    ]
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Наличие фонда КР МКД, сформированного взносами сверх минимального установленного тарифа'
                }
            ]
        });

        this.callParent(arguments);
    }
});