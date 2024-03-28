Ext.define('B4.view.appealcits.RelatedAppealCitsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.AppealCits'
    ],

    alias: 'widget.relatedAppealCitsGrid',
    store: Ext.create('B4.store.AppealCits'),
    enableColumnHide: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
              {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 160,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appeal_citizens';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: '№ обращения',
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата регистрации',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Subjects',
                    flex: 1,
                    text: 'Тематика',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubSubjects',
                    flex: 1,
                    text: 'Подтематика',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Features',
                    flex: 1,
                    text: 'Характеристика',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    flex: 1,
                    text: 'Заявитель',
                    sortable: false
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ExecutantNames',
                //    flex: 1,
                //    text: 'Исполнитель',
                //    sortable: false
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executors',
                    text: 'Исполнитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executants',
                    flex: 1,
                    text: 'Проверяющий сотрудник',
                    sortable: false
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-decline',
                                    text: 'Удалить связанные',
                                    textAlign: 'left',
                                    action: 'Clear',
                                    itemId: 'btnclear'
                                }
                            ]
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
    },

    setRelatesToId: function(id) {
        this.store.relatesToId = id;
        this.store.load();
    }
});