Ext.define('B4.view.manorg.ActivityPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manorgactivitypanel',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        
        'B4.enums.GroundsTermination'
    ],
    closable: true,
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    title: 'Прекращение деятельности',
    trackResetOnLoad: true,
    autoScroll: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },


    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    maxWidth: 650,
                    padding: '10 0 0 0',
                    layout: {
                        align: 'stretch',
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 75,
                        labelAlign: 'right'
                    },
                    items: [
                            {
                                xtype: 'datefield',
                                fieldLabel: 'Дата',
                                name: 'ActivityDateEnd',
                                itemId: 'dfActivityDateEnd',
                                format: 'd.m.Y',
                                width: 175,
                                margin: '0 20 0 0'
                            },
                            {
                                xtype: 'combobox', editable: false,
                                name: 'ActivityGroundsTermination',
                                itemId: 'cbActivityGroundsTermination',
                                fieldLabel: 'Основание',
                                displayField: 'Display',
                                store: B4.enums.GroundsTermination.getStore(),
                                valueField: 'Value',
                                flex: 1
                            },
                            {
                                xtype: 'label',
                                itemId: 'lbActivityGroundsTerminationLabel',
                                margin: '0 0 0 10'
                            }
                    ]
                },
                {
                    xtype: 'textareafield',
                    labelWidth: 75,
                    minHeight: 200,
                    maxWidth: 650,
                    name: 'ActivityDescription',
                    fieldLabel: 'Примечание',
                    margin: '10 10 0 0'
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