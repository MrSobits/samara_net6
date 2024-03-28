Ext.define('B4.view.taskcalendar.AdmonitionGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.taskcalendaradmongrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'taskcalendar.ListAdmonition',
    closable: false,
    enableColumnHide: true,
    title: 'Предостережения',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Номер обращения',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата предостережения',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес МКД',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executor',
                    flex: 1,
                    text: 'Инспектор',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'ДЛ издавшее предостережение',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceDate',
                    flex: 1,
                    text: 'Срок исполнения',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceFactDate',
                    flex: 1,
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                }               
            ],
            plugins: [{ ptype: 'filterbar'}],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var planDate = record.get('PerfomanceDate');
                    var factDate = record.get('PerfomanceFactDate');
                    var currentdate = new Date();
                    var planDateDt = new Date(planDate);
                    var datetime = currentdate.getFullYear() + "-" + currentdate.getDate();

                    if (planDateDt <= currentdate && factDate === null) {

                        return 'back-red';
                    }

                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4updatebutton'
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});