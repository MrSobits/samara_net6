Ext.define('B4.view.dict.ControlType.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.controltypeeditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.enums.ControlLevel',
        'B4.ux.grid.Panel',
        'B4.view.dict.ControlType.InspectorPositionsGrid',
        'B4.view.dict.ControlType.RiskIndicatorsGrid'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'fit',
    width: 1200,
    height: 800,
    bodyPadding: 5,
    title: 'Вид контроля',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: 5,
                    layout: {
                      type: 'vbox',
                      align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'left',
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование вида контроля',
                                    maxLength: 1000,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ErvkId',
                                    fieldLabel: 'Идентификатор вида контроля',
                                    maxLength: 36
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'Level',
                                    fieldLabel: 'Уровень вида контроля',
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    items: B4.enums.ControlLevel.getItemsWithEmpty([null, '-']),
                                    editable: false
                                }
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            flex: 1,
                            padding: '10 0 0 0',
                            layout: 'fit',
                            border: false,
                            items: [
                                {
                                    xtype: 'controltypeinspectorposgrid',
                                },
                                {
                                    xtype: 'controltyperiskindicatorsgrid',
                                }
                            ]
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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