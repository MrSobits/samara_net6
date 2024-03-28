Ext.define('B4.view.regoperator.calcaccount.OperationEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.calcaccountoperationeditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 600,
    minHeight: 280,
    maxHeight: 320,
    bodyPadding: 5,
    title: 'Операция по счету',
    closable: false,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.AccountOperation',
        'B4.view.dict.accountoperation.Grid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 100,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Number',
                            fieldLabel: 'Номер П/П',
                            maxLength: 50
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            format: 'd.m.Y',
                            name: 'OperationDate',
                            fieldLabel: 'Дата операции'
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Операция',
                    name: 'Operation',
                    store: 'B4.store.dict.AccountOperation',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        flex: 1,
                        labelAlign: 'right',
                        labelWidth: 100,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            name: 'Sum',
                            fieldLabel: 'Сумма (руб.)',
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            minValue: 0
                        },
                        {
                            xtype: 'container'
                        }
                    ]
                },                
                {
                    xtype: 'textfield',
                    name: 'Receiver',
                    fieldLabel: 'Получатель',
                    maxLength: 128
                },
                {
                    xtype: 'textfield',
                    name: 'Payer',
                    fieldLabel: 'Плательщик',
                    maxLength: 128
                },
                {
                    xtype: 'textarea',
                    name: 'Purpose',
                    fieldLabel: 'Назначение',
                    maxLength: 500,
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
                            columns: 2,
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
                            columns: 2,
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