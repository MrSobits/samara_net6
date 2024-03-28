Ext.define('B4.view.regop.loan.ManageWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.managewin',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Close',
        'B4.store.regop.loan.AvailableLoanProvidersStore',
        'B4.enums.TypeSourceLoan',
        'B4.enums.TypeLoanProcess'
    ],

    modal: true,
    closeAction:'destroy',
    bodyPadding: 10,
    width: 600,
    minHeight: 350,
    maxHeight: 550,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'TypeLoanProcess',
                    store: B4.enums.TypeLoanProcess.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Формирование займа',
                    labelWidth: 130,
                    editable: false,
                    value: 0
                },
                {
                    xtype: 'b4combobox',
                    name: 'TypeSourceLoan',
                    store: B4.enums.TypeSourceLoan.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Источник займа',
                    labelWidth: 130,
                    editable: false
                },
                {
                    xtype: 'container',
                    margins: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 130,
                        hideTrigger: true
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'InitialDeficit',
                            flex: 1,
                            fieldLabel: 'Начальный дефицит'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CurrentDeficit',
                            labelAlign: 'right',
                            flex: 1,
                            fieldLabel: 'Текущий дефицит'
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    name: 'LoanProviders',
                    disabled: true,
                    store: Ext.create('B4.store.regop.loan.AvailableLoanProvidersStore'),
                    flex: 1,
                    columns: [
                        {
                            text: 'Заимодатели',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            text: 'Год ближайшего КР',
                            dataIndex: 'PlanYear',
                            flex: 1,
                            filter: {
                                xtype: 'numberfield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            text: 'Имеющие средства',
                            dataIndex: 'AvailableMoney',
                            flex: 1,
                            filter: {
                                xtype: 'numberfield',
                                allowDecimals: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            text: 'Сумма заимствования',
                            dataIndex: 'LoanSum',
                            flex: 1,
                            editor: {
                                xtype: 'numberfield',
                                minValue: 0
                            }
                        }
                    ],
                    plugins: [
                        Ext.create('Ext.grid.plugin.CellEditing', {
                            clicksToEdit: 1,
                            listeners: {
                                beforeedit: function () {
                                    me.nRec = me._getAvailableMoney();
                                },
                                validateedit: function (editor) {
                                    var nv = +editor.editors.items[0].getValue(),
                                        error = me._valueIsValid(nv);

                                    if (!Ext.isEmpty(error)) {
                                        B4.QuickMsg.msg('Ошибка', error, 'error');
                                        return false;
                                    }
                                    return true;
                                },
                                edit: function(editor, e) {
                                    var nv = e.value,
                                        ov = e.originalValue;

                                    me._increaseLoan((Ext.isNumber(nv) ? nv : 0), (Ext.isNumber(ov) ? ov : 0));
                                }
                            }
                        }),
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ]
            }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            text: 'Применить изменения',
                            iconCls: 'icon-accept',
                            itemId: 'onAccept'
                        }, '->', {
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
        });
        me.callParent(arguments);
    },

    setInitialDeficit: function(val) {
        var me = this;

        me.down('[name="InitialDeficit"]').setValue(val);
        me.down('[name="CurrentDeficit"]').setValue(val);

        me.initialDeficit = val;
        // Для хранения дефицита с учетом займов 
        me.currentDeficit = val;
    },

    _getCurrentDeficit: function() {
        return this.down('[name="CurrentDeficit"]').getValue();
    },

    _setCurrentDeficit: function(val) {
        this.down('[name="CurrentDeficit"]').setValue(val);
    },

    _valueIsValid: function (val) {
        var me = this,
            availMoney = me.nRec,
            guessVal = me.initialDeficit > 0 ? val * -1 : val;

        if (availMoney < 0 || availMoney < val) {
            return 'Нельзя брать денег больше, чем есть!';
        }

        if ((guessVal + me.initialDeficit) < 0) {
            return 'Нельзя брать денег больше, чем нужно!';
        }

        return null;
    },

    _increaseLoan: function(val, oldVal) {
        var me = this,
            field = me.down('[name="CurrentDeficit"]');

        val = me.initialDeficit > 0 ? val * -1 : val;
        me.currentDeficit += val + oldVal;

        field.setValue(me.currentDeficit);
    },
    
    _getAvailableMoney: function() {
        var me = this,
            sel = me.down('gridpanel').getSelectionModel().getLastSelected();

        return sel.data ? sel.get('AvailableMoney') : 0;
    }
});