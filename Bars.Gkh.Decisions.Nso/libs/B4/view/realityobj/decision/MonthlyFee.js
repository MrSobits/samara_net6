Ext.define('B4.view.realityobj.decision.MonthlyFee', {
    extend: 'Ext.form.Panel',

    alias: 'widget.monthlyfeedecision',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add'
    ],

    entity: 'MonthlyFeeAmountDecision',

    minHeight: 150,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    validationMessage: null,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'container',
                    defaults: {
                        labelWidth: 150
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'Id',
                            flex: 0
                        },
                        {
                            margin: '0 0 0 10',
                            xtype: 'checkbox',
                            name: 'IsChecked',
                            boxLabel: 'Размер ежемесячного взноса на КР'
                        }
                    ]
                },
                {
                    col: 'right',
                    xtype: 'grid',
                    margin: '5 5 5 5',
                    store: {
                        fields: ['Value', 'From', 'To']
                    },
                    height: 110,
                    columns: [
                        {
                            xtype: 'b4deletecolumn',
                            handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                                gridView.getStore().remove(rec);
                            }
                        },
                        {
                            text: 'Дата с',
                            flex: 1,
                            dataIndex: 'From',
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            sortable: false,
                            editor: {
                                xtype: 'datefield'
                            }
                        },
                        {
                            text: 'Дата до',
                            flex: 1,
                            dataIndex: 'To',
                            xtype: 'datecolumn',
                            sortable: false,
                            format: 'd.m.Y',
                            editor: {
                                xtype: 'datefield'
                            }
                        },
                        {
                            text: 'Принятое решение',
                            flex: 1,
                            dataIndex: 'Value',
                            sortable: false,
                            editor: {
                                xtype: 'textfield',
                                regex: /^\d+(\.\d+)?$/
                            }
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    listeners: {
                                        click: {
                                            fn: me.addRow,
                                            scope: me
                                        }
                                    }
                                }
                            ]
                        }
                    ],
                    plugins: [
                        Ext.create('Ext.grid.plugin.CellEditing', {
                            clicksToEdit: 1,
                            pluginId: 'cellEditing',
                            listeners: {
                                edit: function (editor, e) {
                                    var period;

                                    if ((e.record.data["To"] && e.record.data["From"]) && e.record.data["From"] >= e.record.data["To"]) {
                                        Ext.Msg.alert({
                                            title: 'Ошибка ввода данных',
                                            msg: 'Значение в поле "Дата с" должно быть меньше значения поля "Дата до".',
                                            buttons: Ext.Msg.OK,
                                            icon: Ext.window.MessageBox.ERROR
                                        });

                                        e.record.set(e.field, "");
                                    }

                                    if (e.field === 'Value') {
                                        period = me.findPeriod(e.record);
                                        if (!me.isValueValid(e.record)) {
                                            Ext.Msg.alert({
                                                title: 'Ошибка ввода данных',
                                                msg: Ext.String.format('Указанный размер ежемесячного взноса меньше минимального ({0} р.), либо неправильно указан период его действия', period ? period.Value : 0),
                                                buttons: Ext.Msg.OK,
                                                icon: Ext.window.MessageBox.ERROR
                                            });
                                            e.record.set(e.field, "");
                                        }
                                    }
                                }
                            }
                        })
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    setValues: function (v) {
        var form = this,
            store;
        if (v) {
            form.enable();
            form.ranges = v.Defaults;
            store = form.down('grid').getStore();
            form.getForm().setValues(v);
            if (Ext.isArray(v.Decision)) {

                store.removeAll();
                store.add(v.Decision);
            }

            store.each(function (rec) {
                rec.phantom = false;
            });
        }
    },

    toDate: function (dt) {

        if (!dt) {
            return dt;
        }

        if (!Ext.isDate(dt)) {
            dt = new Date(dt);
        }

        return new Date(dt).setHours(0, 0, 0);
    },

    getValues: function () {
        var form = this,
            store = form.down('grid').getStore(),
            values = form.getForm().getValues(),
            decision = [];
        store.each(function (rec) {
            decision.push(rec.getData());
        });
        values.Decision = decision;
        return values;
    },

    addRow: function () {
        if (this.isValid()) {
            this.down('grid').getStore().add({ From: new Date(2014, 7, 1), To: new Date(2014, 11, 31), Value: 0 });
        } else {
            Ext.Msg.alert('Ошибка', this.validationMessage);
        }
    },

    isValid: function () {
        var me = this,
            store = me.down('grid').getStore(),
            result = true,
            records = store.getRange();

        me.validationMessage = null;

        Ext.Array.each(records, function (rec, ind, arr) {
            if (ind < arr.length - 1) {
                result = result && Ext.valueFrom(rec.get('To'), false);
            }

            result = result && me.validateRow(rec, arr[ind - 1]);

            return result;
        });

        return result;
    },

    validateRow: function (curr, prev) {

        var me = this,
            dtFrom = me.toDate(curr.get('From')),
            dtTo = me.toDate(curr.get('To'));

        if (!prev) {
            return (!dtTo || dtFrom < dtTo) && me.isValueValid(curr);
        }

        var prevTo = me.toDate(prev.get('To'));

        return (!dtTo || dtFrom < dtTo);
    },

    isValueValid: function (rec) {
        var me = this,
            period = me.findPeriod(rec);
        if (period) {
            if (rec.get('Value') < period.Value) {
                me.validationMessage = "Необходимо указать размер ежемесячного взноса.";
                return false;
            }
            return true;
        }
        me.validationMessage = "Отсутствует подходящий период для ежемесячного взноса.";
        return false;
    },

    /**
     * if periodEnd is undefined -> periodEnd = Infinity
     * if endDate is undefined -> endDate = Infinity
     */
    inPeriod: function (startDate, endDate, periodStart) {

        var me = this;

        if (!periodStart) {
            throw new Error("Undefined lower limit");
        }

        startDate = me.toDate(startDate);
        endDate = me.toDate(endDate);
        periodStart = me.toDate(periodStart);
        
        return periodStart <= startDate;
    },

    findPeriod: function (rec) {
        var me = this,
            startDate = rec.get('From'),
            endDate = rec.get('To'),
            defaults = me.ranges,
            periods = Ext.Array.filter(defaults, function (p) {
                return me.inPeriod(startDate, endDate, p.From);
            });

        return Ext.Array.max(periods, function (a, b) {
            if (a.Value > b.Value) {
                return 1;
            } else if (a.Value < b.Value) {
                return -1;
            }
            return 0;
        });
    }
});