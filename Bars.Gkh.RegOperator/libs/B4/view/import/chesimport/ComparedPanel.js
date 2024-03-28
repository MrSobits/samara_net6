Ext.define('B4.view.import.chesimport.ComparedPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.import.chesimport.ComparedAddressGrid',
        'B4.view.import.chesimport.ComparedIndOwnerGrid',
        'B4.view.import.chesimport.ComparedLegalOwnerGrid'
    ],

    title: 'Сопоставленные данные',
    alias: 'widget.chesimportcomparedpanel',

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
                            xtype: 'chesimportcomparedaddressgrid'
                        },
                        {
                            xtype: 'chesimportcomparedindownergrid'
                        },
                        {
                            xtype: 'chesimportcomparedlegalownergrid'
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