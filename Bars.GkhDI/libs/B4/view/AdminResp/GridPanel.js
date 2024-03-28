Ext.define('B4.view.adminresp.GridPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.adminrespgridpanel',
    closable: false,
    itemId: 'adminRespGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.adminresp.Grid',
        
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
                            fieldLabel: 'Есть случаи привлечения к административной ответственности в данном отчетном периоде',
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            store: B4.enums.YesNoNotSet.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'AdminResponsibility',
                            itemId: 'cbAdminResponsibility',
                            labelWidth: 550
                        }
                    ]
                },
                {
                    xtype: 'adminrespgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
