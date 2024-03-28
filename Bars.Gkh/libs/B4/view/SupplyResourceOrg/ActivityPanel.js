Ext.define('B4.view.supplyresourceorg.ActivityPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.supplyresorgactivitypanel',

    closable: true,
    width: 500,
    minWidth: 535,
    bodyPadding: 5,
    frame: true,
    
    title: 'Прекращение деятельности',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        
        'B4.enums.GroundsTermination'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults:
                {
                    anchor: '100%',
                    labelAlign: 'right',
                    labelWidth: 90
                },
            items: [
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата',
                    name: 'DateTermination',
                    format: 'd.m.Y',
                    anchor: null,
                    width: 200
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90
                    },
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            name: 'ActivityGroundsTermination',
                            itemId: 'cbActivityGroundsTermination',
                            fieldLabel: 'Основание',
                            displayField: 'Display',
                            store: B4.enums.GroundsTermination.getStore(),
                            valueField: 'Value',
                            flex: 0.6,
                            maxWidth: 500,
                            editable: false
                        },
                        {
                            xtype: 'label',
                            itemId: 'lbActivityGroundsTerminationLabel',
                            margin: '0 0 0 10',
                            flex: 0.4
                        }
                    ]
                },
                {
                    xtype: 'textareafield',
                    name: 'DescriptionTermination',
                    fieldLabel: 'Примечание'
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    tooltip: 'Обновить',
                                    iconCls: 'icon-arrow-refresh',
                                    itemId: 'btnActivityPanelRefresh'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});