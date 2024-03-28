Ext.define('B4.view.regop.loan.TakeLoanWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.loanmanagewindow',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Close',
        'B4.enums.TypeSourceLoan',
        'B4.enums.TypeLoanProcess',
        'B4.store.regop.loan.AvailableSources',
        'B4.view.Control.GkhDecimalField'
    ],

    title: 'Формирование займа',
    //modal: true,
    closeAction: 'destroy',
    bodyPadding: 10,
    width: 600,
    minHeight: 350,
    maxHeight: 550,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    robjectsSaldoSum: null,

    initComponent: function () {
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
                    xtype: 'container',
                    margins: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'gkhdecimalfield',
                        labelAlign: 'right',
                        labelWidth: 130,
                        hideTrigger: true,
                        readOnly: true,
                        flex: 1
                    },
                    items: [
                        {
                            name: 'InitialDeficit',
                            fieldLabel: 'Начальный дефицит'
                        },
                        {
                            name: 'CurrentDeficit',
                            fieldLabel: 'Текущий дефицит'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Имеющиеся для займа средства формируются без учёта сальдо домов-занимателей и их сумма ограничена неснижаемым размером фонда.</span>'
                },
                {
                    xtype: 'gridpanel',
                    name: 'LoanSources',
                    disabled: true,
                    store: Ext.create('B4.store.regop.loan.AvailableSources', {
                        remoteSort: false,
                        remoteFilter: false
                    }),
                    flex: 1,
                    columns: [
                        {
                            text: 'Источник займа',
                            dataIndex: 'Name',
                            flex: 1
                        },
                        {
                            text: 'Имеющиеся средства',
                            dataIndex: 'AvailableMoney',
                            width: 130,
                            renderer: function(val) {
                                return val ? Ext.util.Format.currency(val) : '';
                            }
                        },
                        {
                            text: 'Сумма заимствования',
                            dataIndex: 'TakenMoney',
                            width: 130,
                            editor: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                decimalSeparator: ','
                            },
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : '';
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
                                edit: function (editor, e) {
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
                            action: 'accept'
                        }, '->', {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function (btn) {
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

    setInitialDeficit: function (val, robjectsSaldoSum) {
        var me = this;

        me.down('[name="InitialDeficit"]').setValue(val);
        me.down('[name="CurrentDeficit"]').setValue(val);

        me.initialDeficit = val;
        // Для хранения дефицита с учетом займов 
        me.currentDeficit = val;

        me.robjectsSaldoSum = robjectsSaldoSum;
    },

    _getCurrentDeficit: function () {
        return this.down('[name="CurrentDeficit"]').getValue();
    },

    _setCurrentDeficit: function (val) {
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

    _increaseLoan: function (val, oldVal) {
        var me = this,
            field = me.down('[name="CurrentDeficit"]');

        val = me.initialDeficit > 0 ? val * -1 : val;
        me.currentDeficit += val + oldVal;

        field.setValue(me.currentDeficit);
    },

    _getAvailableMoney: function () {
        var me = this,
            sel = me.down('gridpanel').getSelectionModel().getLastSelected();

        return sel.data ? sel.get('AvailableMoney') : 0;
    }
});