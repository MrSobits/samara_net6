Ext.define('B4.view.version.RecordsEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.version.Stage1RecordsGrid',
        'B4.view.version.OwnerDecisionForm'
    ],
    alias: 'widget.versionrecordseditwin',
    title: 'Работа',
    modal: true,
    bodyPadding: '5 5 0 0',
    width: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: false,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            defaults: {
                padding: '0 5 0 5',
                labelAlign: 'right',
                labelWidth: 170
            },
            items: [
                {
                    xtype: 'numberfield',
                    padding: '5 5 0 5',
                    hideTrigger: true,
                    allowDecimals: false,
                    minValue: 2013,
                    maxValue: 2100,
                    fieldLabel: 'Плановый год',
                    allowBlank: false,
                    name: 'Year'
                },
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Зафиксировать год',
                    name: 'FixedYear'
                },
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Пересчитать стоимость работ',
                    name: 'ReCalcSum'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    name: 'IndexNumber',
                    fieldLabel: 'Текущий номер',
                    readOnly: true
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    minValue: 1,
                    name: 'NewIndexNumber',
                    fieldLabel: 'Новый номер'
                },
                {
                    xtype: 'versionownerdecisionform'
                },
                {
                    xtype: 'container',
                    margin: '0 0 10 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'tbfill',
                            flex: 1
                        },
                        {
                            xtype: 'button',
                            text: 'Разделить работу',
                            textAlign: 'left',
                            itemId: 'btnSplitWork'
                        }
                    ]
                },
                {
                    xtype: 'stage1recordsgrid',
                    flex: 1
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
