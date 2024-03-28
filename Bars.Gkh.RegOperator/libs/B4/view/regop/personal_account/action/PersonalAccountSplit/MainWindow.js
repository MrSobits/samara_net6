Ext.define('B4.view.regop.personal_account.action.PersonalAccountSplit.MainWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.personalaccountsplitwin',

    requires: [
        'B4.ux.button.Save',
		'B4.ux.button.Close',

		'B4.enums.RoomOwnershipType'
    ],

    modal: true,
    closable: false,
    width: 800,
    height: 550,
    minHeight: 300,
    title: 'Разделение лицевого счета',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    accountOperationCode: 'PersonalAccountSplitOperation',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    bodyPadding: 10,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        padding: '0 0 10 0',
                        defaults: {
                            labelAlign: 'right',
                            labelWidth: 150,
                            xtype: 'textfield',
                            flex: 1
                        }
                    },
                    items: [
                        {
                            items: [
                                {
                                    name: 'RoomAddress',
                                    fieldLabel: 'Адрес',
                                    readOnly: true
                                }
                            ]
						},
						{
							items: [
								{
									xtype: 'b4combobox',
									name: 'OwnershipType',
									fieldLabel: 'Тип собственности',
									readOnly: true,
									items: B4.enums.RoomOwnershipType.getItems()
								}
							]
						},
                        {
                            items: [
                                {
                                    name: 'OwnerName',
                                    fieldLabel: 'ФИО/Наименование владельца',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    name: 'PersonalAccountNum',
                                    fieldLabel: 'Номер ЛС',
                                    readOnly: true
                                },
                                {
                                    name: 'AreaShare',
                                    fieldLabel: 'Доля собственности',
                                    readOnly: true
                                },
                                {
                                    name: 'RoomArea',
                                    fieldLabel: 'Площадь помещения',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'SplitDate',
                                    fieldLabel: 'Дата разделения',
                                    allowBlank: false,
                                    minText: 'Дата разделения не должна быть ранее даты открытия существующего ЛС'
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'Document',
                                    fieldLabel: 'Документ-основание',
                                    allowBlank: false,
                                    editable: false,
                                    onClearFile: function () {
                                        var me = this,
                                            currentValue = me.getValue();
        
                                        me.setValue(null);

                                        me.fileIsDelete = true;
                                        me.fileIsLoad = false;

                                        me.fireEvent('fileclear', me, me.getName(), currentValue);
                                        B4.form.FileField.prototype.reset(me);
                                    },

                                    reset: function () {
                                        return;
                                    }
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    fieldLabel: 'Причина'
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Закрыть текущий лицевой счет',
                                    name: 'CloseCurrent'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'personalaccountsplitaccountsgrid',
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
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Продолжить'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function(btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],

            getForm: function() {
                return me.down('form');
            },
            getGrid: function() {
                return me.down('personalaccountsplitaccountsgrid');
            },
            getStore: function() {
                return me.getGrid().getStore();
            },
            listeners: {
                beforeclose: function(win) {
                    Ext.Msg.confirm('Внимание',
                        'При закрытие окна все внесенные изменение в операции "Распределение задолженности" не сохранятся. Действительно ли хотите закрыть окно?',
                        function(result) {
                            if (result === 'yes') {
                                win.clearListeners();  // чистим все слушателей, чтобы опять сюда не попасть
                                win.close();
                            }
                        });

                    return false;
                }
            }
        });

        me.callParent(arguments);
    }
});