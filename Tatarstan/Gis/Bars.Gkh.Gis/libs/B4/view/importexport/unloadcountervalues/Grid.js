Ext.define('B4.view.importexport.unloadcountervalues.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.enums.TypeStatus',
        'B4.form.MonthPicker',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Выгрузка показаний ПУ',
    alias: 'widget.unloadcountervaluesgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.importexport.UnloadCounterValues');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'FormationDate',
                    text: 'Дата формирования',
                    flex: 1,
                    format: 'd.m.Y H:i:s',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Month',
                    text: 'Расчетный месяц',
                    flex: 1,
                    format: 'M.Y',
                    filter: {
                        xtype: 'b4monthpicker',
                        onOKClick: function (comp, data) {
                            var me = this,
                                tempData = me.selectMoth ? me.selectMoth : new Date((data[0] + 1) + '/1/' + data[1]);

                            if (tempData) {
                                me.setValue(tempData);
                                me.fireEvent('select', me, tempData);
                            }
                            me.collapse();
                            store.load();
                        }
                    },
                    renderer: function (val) {
                        if (val === '0001-01-01T00:00:00') {
                            return null;
                        }
                        return Ext.Date.format(new Date(val), 'M.Y');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'User',
                    text: 'Пользователь',
                    flex: 5,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrganizationName',
                    text: 'Наименование организации пользователя',
                    flex: 5,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeStatus',
                    text: 'Статус выгрузки',
                    flex: 1,
                    enumName: 'B4.enums.TypeStatus',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    text: 'Файл',
                    flex: 1,
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Log',
                    text: 'Лог',
                    flex: 1,
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Выгрузить показания ПУ',
                                    iconCls: 'icon-add',
                                    name: 'Unload'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});