Ext.define('B4.view.longtermprobject.specialaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.specialaccounteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minWidth: 750,
    minHeight: 500,
    height: 500,
    width: 750,
    title: 'Cпециальный счет',
    requires: [
        'B4.store.SpecialAccountDecision',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.CreditOrg',
        'B4.view.creditorg.Grid',
        'B4.store.account.ContragentForSpecial',
        'B4.view.contragent.Grid',
        'B4.view.longtermprobject.specialaccount.BankStatGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    enableTabScroll: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Решение',
                                    name: 'Decision',
                                    store: 'B4.store.SpecialAccountDecision',
                                    editable: false,
                                    allowBlank: false,
                                    isGetOnlyIdProperty: false,
                                    textProperty: 'DecisionType',
                                    columns: [
                                        { text: 'Владелец счета', dataIndex: 'OwnerName', flex: 1, filter: { xtype: 'textfield' } },
                                        { xtype: 'gridcolumn', text: 'Номер протокола', dataIndex: 'ProtocolNumber', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            text: 'Дата протокола',
                                            dataIndex: 'ProtocolDate',
                                            flex: 1,
                                            filter: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                operand: CondExpr.operands.eq
                                            }
                                        }
                                    ],
                                    //#warning убрать переопределение после добавления fireEvent('select', field, data); в платформенный компонент
                                    onSelectValue: function () {
                                        var field = this,
                                            rec = field.gridView.getSelectionModel().getSelected(),
                                            data;
                                        if (rec.length == 0) {
                                            Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                                            return;
                                        }

                                        if (field.selectionMode.toUpperCase() == 'SINGLE') {
                                            data = rec[0].getData();
                                        } else {
                                            data = [];
                                            for (var i in rec) {
                                                data.push(rec[i].getData());
                                            }
                                        }

                                        field.setValue(data);
                                        field.fireEvent('select', field, data);

                                        field.onSelectWindowClose();
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    allowBlank: false,
                                    maxLength: 50,
                                    name: 'Number',
                                    fieldLabel: 'Номер',
                                    readOnly: true
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        labelWidth: 150,
                                        flex: 1,
                                        labelAlign: 'right',
                                        readOnly: true
                                    },
                                    padding: '0 0 5 0',
                                    items: [
                                        {
                                            name: 'OpenDate',
                                            fieldLabel: 'Дата открытия',
                                            allowBlank: false
                                        },
                                        {
                                            name: 'CloseDate',
                                            fieldLabel: 'Дата закрытия'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Владелец счета',
                                    name: 'OwnerName',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Кредитная организация',
                                    name: 'CreditOrgName',
                                    readOnly: true
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Лимит по кредиту',
                                    name: 'CreditLimit',
                                    decimalSeparator: ',',
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'specialaccountbankstatgrid',
                                    columnLines: true,
                                    flex: 1
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