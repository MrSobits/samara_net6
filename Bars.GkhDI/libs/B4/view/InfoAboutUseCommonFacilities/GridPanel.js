Ext.define('B4.view.infoaboutusecommonfacilities.GridPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.infusecommonfacgridpanel',
    closable: false,
    itemId: 'infoAboutUseCommonFacilitiesGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.infoaboutusecommonfacilities.Grid',
        
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
                            itemId: 'cbPlaceGeneralUse',
                            fieldLabel: 'Договоры на использовании мест общего пользования имеются',
                            store: B4.enums.YesNoNotSet.getStore(),
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'PlaceGeneralUse',
                            labelWidth: 385
                        }
                    ]
                },
                {
                    xtype: 'infusecommonfacgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
