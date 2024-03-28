Ext.define('B4.view.builder.ActivityPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.enums.GroundsTermination'
    ],
    closable: true,
    layout: 'form',
    width: 500,
    frame: true,
    bodyPadding: 5,
    itemId: 'builderActivityPanel',
    title: 'Деятельность',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ActivityDateStart',
                            fieldLabel: 'Дата начала деятельности',
                            format: 'd.m.Y',
                            width: 280
                        },
                        {
                            xtype: 'datefield',
                            name: 'ActivityDateEnd',
                            fieldLabel: 'Дата окончания деятельности',
                            format: 'd.m.Y',
                            width: 290,
                            labelWidth: 190
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 180
                    },
                    items: [
                        {
                            xtype: 'combobox', 
                            name: 'ActivityGroundsTermination',
                            itemId: 'cbActivityGroundsTermination',
                            fieldLabel: 'Основание',
                            displayField: 'Display',
                            store: B4.enums.GroundsTermination.getStore(),
                            valueField: 'Value',
                            maxWidth: 570,
                            flex: 1,
                            editable: false
                        },
                        {
                            xtype: 'label',
                            itemId: 'lbActivityGroundsTerminationLabel',
                            margin: '0 0 0 10'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'ActivityDescription',
                    fieldLabel: 'Описание деятельности',
                    maxLength: 500,
                    labelWidth: 180,
                    anchor: '100%',
                    labelAlign: 'right'
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
                                    xtype: 'b4updatebutton'
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