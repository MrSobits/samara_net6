Ext.define('B4.view.passport.house.info.Grid', {
    alias: 'widget.infohousepassportgrid',

    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.form.GridStateColumn',
        'B4.form.ComboBox',

        'B4.store.PeriodYear',
        'B4.store.passport.HouseProviderPassport',
        'B4.QuickMsg'
    ],

    title: 'Паспорта дома',
    year: null,
    month: null,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.passport.HouseProviderPassport'),
            cbMonth = Ext.create('Ext.form.ComboBox', {
                index: 'Month',
                queryMode: 'local',
                valueField: 'num',
                displayField: 'name',
                store: Ext.create('Ext.data.Store', {
                    fields: ['num', 'name'],
                    data: [
                        { "num": 1, "name": "Январь" },
                        { "num": 2, "name": "Февраль" },
                        { "num": 3, "name": "Март" },
                        { "num": 4, "name": "Апрель" },
                        { "num": 5, "name": "Май" },
                        { "num": 6, "name": "Июнь" },
                        { "num": 7, "name": "Июль" },
                        { "num": 8, "name": "Август" },
                        { "num": 9, "name": "Сентябрь" },
                        { "num": 10, "name": "Октябрь" },
                        { "num": 11, "name": "Ноябрь" },
                        { "num": 12, "name": "Декабрь" }
                    ]
                })
            }),
            cbYear = Ext.create('Ext.form.ComboBox', {
                fieldLabel: 'Отчетный период',
                index: 'Year',
                queryMode: 'local',
                valueField: 'num',
                displayField: 'name',
                store: Ext.create('B4.store.PeriodYear')
            });

        cbMonth.setValue(me.month);
        cbYear.setValue(me.year);

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'houseproviderpassport';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'ObjectCreateDate',
                    width: 150,
                    text: 'Дата смены статуса',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Жилой дом',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    width: 250,
                    text: 'Ответственный',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Signature',
                    flex: 1,
                    text: 'Сведения о подписанте',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percent',
                    align: 'center',
                    text: '%',
                    tdCls: 'x-progress-cell',
                    renderer: function (value) {
                        return value + '%';
                    },
                    width: 100
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Переход',
                    action: 'openpassport',
                    width: 174,
                    items: [{
                        tooltip: 'Перейти к паспорту',
                        iconCls: 'icon-fill-button',
                        icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKgAAAAWCAYAAABUi9exAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NjRCRTAyNkUxMzFGMTFFM0E1OEE4MzMwREIzQkFCRDUiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NjRCRTAyNkYxMzFGMTFFM0E1OEE4MzMwREIzQkFCRDUiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo2NEJFMDI2QzEzMUYxMUUzQTU4QTgzMzBEQjNCQUJENSIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo2NEJFMDI2RDEzMUYxMUUzQTU4QTgzMzBEQjNCQUJENSIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PvsEoDoAAAX6SURBVHja7FtNTBtH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'
                    }]
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var c = parseFloat(record.get('Percent'));
                    if (c == isNaN) {
                        return;
                    }

                    if (c == 100) {
                        return 'x-percent-100';
                    }

                    if (c <= 10) {
                        return 'x-percent-10';
                    } else if (c > 10 && c <= 20) {
                        return 'x-percent-20';
                    } else if (c > 20 && c <= 40) {
                        return 'x-percent-30';
                    } else if (c > 40 && c <= 70) {
                        return 'x-percent-70';
                    } else if (c > 70 && c <= 99) {
                        return 'x-percent-90';
                    };
                    return;
                }
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
                                    xtype: 'button',
                                    text: 'Автозаполнение',
                                    iconCls: 'icon-cog-go',
                                    listeners: {
                                        click: function (btn) {
                                            var me = this,
                                                grid = btn.up('infohousepassportgrid'),
                                                sel = grid.getSelectionModel().getSelection()[0];
                                            
                                            if (sel) {
                                                grid.setLoading(true);
                                                B4.Ajax.request(B4.Url.action('Fill', 'Fill1468', {
                                                    paspId: sel.get('Id')
                                                })).next(function() {
                                                    B4.QuickMsg.msg('Сохранение', 'Автозаполнение паспорта прошло успешно', 'success');
                                                    grid.setLoading(false);
                                                    grid.getStore().reload();
                                                }).error(function() {
                                                    B4.QuickMsg.msg('Ошибка', 'Произошла ошибка в ходе автозаполнения паспорта', 'error');
                                                    grid.setLoading(false);
                                                });
                                            } else {
                                                B4.QuickMsg.msg('Инфо', 'Необходимо выбрать паспорт, для автозаполнения', 'info');
                                            }

                                        }
                                    }
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