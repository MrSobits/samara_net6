Ext.define('B4.view.realityobj.structelement.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.structelementpanel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    
    title: 'Конструктивные характеристики',
    requires: [
        'B4.view.realityobj.structelement.Grid',
        'B4.view.realityobj.missingceo.Grid',
        'B4.view.realityobj.structelement.HistoryGrid',
        'B4.ux.button.Save',
        'B4.enums.TypePresence'
    ],

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [             
                {
                    xtype: 'numberfield',
                    name: 'Points',
                    fieldLabel: 'Всего баллов',                    
                    labelWidth: 250,
                    labelAlign: 'right',
                    width: 400,
                    minValue: 0,
                    maxValue: 100,
                    margin: '0 5 10 0',
                    allowDecimals: false,
                    hideTrigger: true
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'structelementgrid',
                            flex: 1
                        },
                        {
                            xtype: 'missingcommonestobjgrid',
                            flex: 1
                        },
                        {
                            xtype: 'rostructelhistorygrid',
                            flex: 1
                        }
                    ]
                }
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        itemId: 'structElpPanelGroupButton',
                        items: [
                            {
                                 xtype: 'b4savebutton'
                            }
                        ]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});