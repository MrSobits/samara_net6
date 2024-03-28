Ext.define('B4.view.nonresidentialplacement.GridPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.nonresidplacegridpanel',
    closable: false,
    itemId: 'nonResidentialPlacementGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.nonresidentialplacement.Grid',
        
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
                            fieldLabel: 'Данные об использовании нежилых помещений имеются',
                            store: B4.enums.YesNoNotSet.getStore(),
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'NonResidentialPlacement',
                            itemId: 'cbNonResidentialPlacement',
                            labelWidth: 340
                        }
                    ]
                },
                {
                    xtype: 'nonresidplacegrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
