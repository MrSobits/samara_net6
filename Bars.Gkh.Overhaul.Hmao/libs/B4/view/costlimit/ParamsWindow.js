Ext.define('B4.view.costlimit.ParamsWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
     
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    //bodyPadding: 10,
    width: 300,
    height: 140,
    alias: 'widget.costlimitparamswindow',
    itemId: 'costlimitParamsWindow',
    title: 'Параметры рассчета',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {},
            items:
            [{
                xtype: 'container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                defaults: {
                    labelWidth: 180,
                    labelAlign: 'right',
                },
                items: [                    
                    {
                        xtype: 'numberfield',
                        name: 'CalcYear',
                        itemId:'nfCalcYear',
                        fieldLabel: 'Год фактических работ',
                        allowDecimals: false,
                        minValue: 2020,
                        allowBlank: false,
                    },
                    {
                        xtype: 'numberfield',
                        name: 'CalcIndex',
                        itemId: 'nfCalcIndex',
                        fieldLabel: 'Индекс (% инфляции)',
                        allowDecimals: true,
                        minValue: 0.01,
                        allowBlank: false,
                    }                
                ]
            }],
            dockedItems: [
                {
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4savebutton',
                        text: 'Применить',
                    }]
                },
                {
                    xtype: 'tbfill'
                },
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4closebutton',
                        handler: function () {
                            this.up('window').close();
                        }
                    }]
                }]
            }]
        });

        me.callParent(arguments);
    }
});