Ext.define('B4.view.import.chesimport.ComparingPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.import.chesimport.AddressComparingPanel',
        'B4.view.import.chesimport.LegalOwnerComparingPanel',
        'B4.view.import.chesimport.IndOwnerComparingPanel'
    ],

    title: 'Несопоставленные данные',
    alias: 'widget.chesimportcomparingpanel',

    bodyStyle: Gkh.bodyStyle,

    closable: true,
    layout: 'fit',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'chesimportaddresscomparingpanel'
                        },
                        {
                            xtype: 'chesimportindownercomparingpanel'
                        },
                        {
                            xtype: 'chesimportlegalownercomparingpanel'
                        },
                        {
                            title: 'Лицевые счета',
                            disabled: true 
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});