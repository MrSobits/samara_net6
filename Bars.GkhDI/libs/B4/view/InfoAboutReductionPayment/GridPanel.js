Ext.define('B4.view.infoaboutreductionpayment.GridPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.infreductpaymgridpanel',
    closable: false,
    itemId: 'infoAboutReductionPaymentGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.infoaboutreductionpayment.Grid',
        
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                { 
                    xtype: 'form',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyPadding: 2,
                    bodyStyle: Gkh.bodyStyle,
                    height: 35,
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Были случаи снижения платы',
                            store: B4.enums.YesNoNotSet.getStore(),
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'ReductionPayment',
                            itemId: 'cbReductionPayment',
                            labelWidth: 180
                        }
                    ]
                },
                {
                    xtype: 'infreductpaymgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
