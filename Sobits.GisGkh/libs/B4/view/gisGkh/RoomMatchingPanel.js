Ext.define('B4.view.gisGkh.RoomMatchingPanel', {
    extend: 'Ext.panel.Panel', 
    alias: 'widget.roommatchingpanel',

    requires: [
    ],

    closable: false,

    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    }, 

    items: [
        {
            xtype: 'gisgkhrogrid',
            flex: 1
        },
        {
            xtype: 'container',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            flex: 2,
            align: 'stretch',
            defaults: {
                flex: 1                
            },
            items: [
                {
                    xtype: 'gisgkhroomgrid'
                },
                {
                    xtype: 'gisgkhpremisesgrid'
                }
            ]
        }
    ],
    dockedItems: [
        {
            xtype: 'toolbar',
            dock: 'top',
            items: [
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        {
                            xtype: 'button',
                            text: 'Сопоставить',
                            tooltip: 'Сопоставить помещение',
                            iconCls: 'icon-accept',
                            width: 110,
                            itemId: 'btnMatchRoom',
                            disabled: true
                        },
                        {
                            xtype: 'button',
                            text: 'Отменить сопоставление',
                            tooltip: 'Отменить сопоставление помещения',
                            iconCls: 'icon-accept',
                            width: 160,
                            itemId: 'btnUnMatchRoom',
                            disabled: true
                        }
                    ]
                }
            ]
        }
    ]
});
