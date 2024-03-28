Ext.define('B4.view.Control.GkhWorkModeGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gkhworkmodegrid',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        
        'B4.enums.TypeDayOfWeek',
        'B4.enums.TypeDayOfWeek'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            height: 230,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDayOfWeek',
                    flex: 1,
                    text: 'День недели',
                    renderer: function (val) {
                        return B4.enums.TypeDayOfWeek.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StartDate',
                    width: 100,
                    text: 'Время начала',
                    // Гавно-код связан с тем что умный chrome прибавляет к датам сдвиг по гринвичу
                    renderer: function (val) {
                        if (Ext.isDate(val)) {
                            return Ext.Date.format(new Date(val), 'H:i');
                        } else {
                            if (val && val.indexOf('T') != -1) {
                                var partsDateTime = val.split('T');
                                if (partsDateTime.length == 2) {
                                    var partsHourMinute = val.split('T')[1].split(':');
                                    if (partsHourMinute.length >= 2) {
                                        return partsHourMinute[0] + ':' + partsHourMinute[1];
                                    }
                                }
                            }
                        }
                        
                        return "";
                    },
                    editor: 
                        {
                            xtype: 'timefield',
                            format: 'H:i',
                            submitFormat: 'Y-m-d H:i:s',
                            minValue: '0:00',
                            maxValue: '23:00',
                            editable: false
                        }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EndDate',
                    width: 100,
                    text: 'Время окончания',
                    // Гавно-код связан с тем что умный chrome прибавляет к датам сдвиг по гринвичу
                    renderer: function (val) {
                        if (Ext.isDate(val)) {
                            return Ext.Date.format(new Date(val), 'H:i');
                        } else {
                            if (val && val.indexOf('T') != -1) {
                                var partsDateTime = val.split('T');
                                if (partsDateTime.length == 2) {
                                    var partsHourMinute = val.split('T')[1].split(':');
                                    if (partsHourMinute.length >= 2) {
                                        return partsHourMinute[0] + ':' + partsHourMinute[1];
                                    }
                                }
                            }
                        }

                        return "";
                    },
                    editor:
                        {
                            xtype: 'timefield',
                            format: 'H:i',
                            submitFormat: 'Y-m-d H:i:s',
                            minValue: '0:00',
                            maxValue: '23:00',
                            editable: false
                        }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pause',
                    width: 120,
                    text: 'Перерыв 00:00-00:00',
                    editor: {
                        xtype: 'textfield',
                        regex: /^([0-2]{1}[0-9]{1}:[0-6]{1}[0-9]{1})-([0-2]{1}[0-9]{1}:[0-6]{1}[0-9]{1})$/,
                        regexText: 'Формат ввода времени: 00:00-00:00'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AroundClock',
                    width: 100,
                    text: 'Круглосуточно',
                    editor: {
                        xtype: 'b4combobox',
                        editable: false,
                        items: [[false, 'Нет'], [true, 'Да']]
                    },
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
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
                                    xtype: 'b4updatebutton'
                                }, 
                                {
                                    xtype: 'b4savebutton'
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