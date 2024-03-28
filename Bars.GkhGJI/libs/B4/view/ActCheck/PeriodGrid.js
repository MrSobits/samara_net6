Ext.define('B4.view.actcheck.PeriodGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.actCheckPeriodGrid',
    title: 'Дата и время проведения проверки',
    store: 'actcheck.Period',
    itemId: 'actCheckPeriodGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateCheck',
                    text: 'Дата',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Время начала',
                    format: 'H:i',
                    width: 100,
                    // Гавно-код связан с тем что умный chrome прибавляет к датам сдвиг по гринвичу
                    renderer: function (val) {
                        if (Ext.isDate(val)) {
                            return Ext.Date.format(new Date(val), 'H:i');
                        } else {
                            if (val && val.indexOf('T') != -1) {
                                var partsDateTime = val.split('T');
                                if (partsDateTime.length == 2) {
                                    var partsHourMinute = partsDateTime[1].split(':');
                                    if (partsHourMinute.length >= 2) {
                                        return partsHourMinute[0] + ':' + partsHourMinute[1];
                                    }
                                }
                            }
                        }

                        return "";
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Время окончания',
                    format: 'H:i',
                    width: 100,
                     // Гавно-код связан с тем что умный chrome прибавляет к датам сдвиг по гринвичу
                    renderer: function (val) {
                        if (Ext.isDate(val)) {
                            return Ext.Date.format(new Date(val), 'H:i');
                        } else {
                            if (val && val.indexOf('T') != -1) {
                                var partsDateTime = val.split('T');
                                if (partsDateTime.length == 2) {
                                    var partsHourMinute = partsDateTime[1].split(':');
                                    if (partsHourMinute.length >= 2) {
                                        return partsHourMinute[0] + ':' + partsHourMinute[1];
                                    }
                                }
                            }
                        }

                        return "";
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
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
                                    xtype: 'b4addbutton'
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