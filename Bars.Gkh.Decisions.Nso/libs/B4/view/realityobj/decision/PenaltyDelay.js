Ext.define('B4.view.realityobj.decision.PenaltyDelay', {
    extend: 'Ext.form.Panel',

    alias: 'widget.penaltydelaydecision',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add'
    ],

    entity: 'PenaltyDelayDecision',

    minHeight: 150,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    validationMessage: null,

    initComponent: function () {
        var me = this,
            notDisabledStartDates = [
                '01.01', '01.02', '01.03', '01.04', '01.05', '01.06', '01.07', '01.08', '01.09', '01.10', '01.11', '01.12'
            ],
            notDisabledEndDates = [
                '31.01', '28.02', '29.02', '31.03', '30.04', '31.05', '30.06', '31.07', '31.08', '30.09', '31.10', '30.11', '31.12'
            ],
            startRegex = '^(?!' + notDisabledStartDates.join('|') + ').*$',
            endRegex = '^(?!' + notDisabledEndDates.join('|') + ').*$';

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
                            boxLabel: 'Срок уплаты взноса'
                        }
                    ]
                },
                {
                    col: 'right',
                    xtype: 'grid',
                    margin: '5 5 5 5',
                    store: {
                        fields: [
                            'From', 'To', 'DaysDelay', 'MonthDelay'
                        ]
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
                            xtype: 'datecolumn',
                            dataIndex: 'From',
                            text: 'Дата с',
                            flex: 1,
                            format: 'd.m.Y',
                            sortable: false,
                            editor: {
                                xtype: 'datefield',
                                disabledDates: [startRegex],
                                format: 'd.m.Y'
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'To',
                            text: 'Дата до',
                            flex: 1,
                            sortable: false,
                            format: 'd.m.Y',
                            editor: {
                                xtype: 'datefield',
                                disabledDates: [endRegex],
                                format: 'd.m.Y'
                            }
                        },
                        {
                            dataIndex: 'DaysDelay',
                            text: 'Допустимая просрочка, дней',
                            flex: 1,
                            sortable: false,
                            editor: {
                                xtype: 'numberfield',
                                allowDecimals: false,
                                hideTrigger: true,
                                minValue: 0
                            }
                        },
                        {
                            xtype: 'booleancolumn',
                            dataIndex: 'MonthDelay',
                            text: 'Допустимая просрочка, месяц',
                            flex: 1,
                            sortable: false,
                            editor: {
                                xtype: 'checkbox'
                            },
                            renderer: function(val) {
                                return val ? 'Да' : 'Нет';
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
                                beforeedit: function(editor, e) {
                                    if (e.field == 'DaysDelay' && e.record.get('MonthDelay')) {
                                        return false;
                                    }
                                },
                                edit: function (editor, e) {
                                    if ((e.record.data["To"] && e.record.data["From"]) && e.record.data["From"] >= e.record.data["To"]) {
                                        Ext.Msg.alert({
                                            title: 'Ошибка ввода данных',
                                            msg: 'Значение в поле "Дата с" должно быть меньше значения поля "Дата до".',
                                            buttons: Ext.Msg.OK,
                                            icon: Ext.window.MessageBox.ERROR
                                        });

                                        e.record.set(e.field, "");
                                    }

                                    if (e.field === 'MonthDelay') {
                                        if (e.record.get('MonthDelay')) {
                                            e.record.set('DaysDelay', null);
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
        this.down('grid').getStore().add({});
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

        return (!dtTo || dtFrom < dtTo) && prevTo < dtFrom && me.isValueValid(curr);
    },

    isValueValid: function (rec) {
        if (!rec.get('MonthDelay') && rec.get('DaysDelay') == 0) {
            return false;
        }

        return true;
    },

    /**
     * if periodEnd is undefined -> periodEnd = Infinity
     * if endDate is undefined -> endDate = Infinity
     */
    inPeriod: function (startDate, endDate, periodStart, periodEnd) {

        var me = this;

        if (!periodStart) {
            throw new Error("Undefined lower limit");
        }

        startDate = me.toDate(startDate);
        endDate = me.toDate(endDate);
        periodStart = me.toDate(periodStart);

        if (periodEnd) {
            periodEnd = me.toDate(periodEnd);
            if (endDate == "" || endDate == null) {
                endDate = periodEnd;
            }
            if (endDate) {
                return periodStart <= startDate && endDate <= periodEnd;
            }
            return false;
        }

        return periodStart <= startDate;
    },

    findPeriod: function (rec) {
        var me = this,
            startDate = rec.get('From'),
            endDate = rec.get('To'),
            defaults = me.ranges,
            peroids = Ext.Array.filter(defaults, function (p) {
                return me.inPeriod(startDate, endDate, p.From, p.To);
            });

        return Ext.Array.max(peroids, function (a, b) {
            if (a.Value > b.Value) {
                return 1;
            } else if (a.Value < b.Value) {
                return -1;
            }
            return 0;
        });
    }
});