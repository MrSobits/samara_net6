Ext.define('B4.view.appealcits.AnotherRelatedAppealCitsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.AppealCits'
    ],

    alias: 'widget.anotherrelatedAppealCitsGrid',
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
                    dataIndex: 'Correspondent',
                    flex: 1,
                    text: 'Заявитель',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExecutantNames',
                    flex: 1,
                    text: 'Исполнитель',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executants',
                    flex: 1,
                    text: 'Сотрудник',
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
                            columns: 2,
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    },

    setAnotherRelatesToId: function(id) {
        this.store.anotherRelatesToId = id;
        this.store.load();
    }
});