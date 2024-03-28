Ext.define('B4.view.person.QualificationRegistryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personqualificationregistrygrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.form.GridStateColumn',
        'B4.store.person.QualificationCertificate',
        'B4.enums.TypeCancelationQualCertificate'
    ],

    title: 'Квалификационные аттестаты',
    
    closable: true,

    // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
    provideStateId: Ext.emptyFn,
    stateful: false,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.person.QualificationCertificate');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me,
                    handler: function (grid, rowIndex, colIndex, el, e, rec) {
                        Ext.History.add(Ext.String.format("personedit/{0}/{1}/1", rec.get('personId'), rec.get('Id')));
                    }
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 175,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_person_qc';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    width: 100,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FullName',
                    width: 100,
                    text: 'ФИО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IssuedDate',
                    text: 'Дата выдачи',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RequestToExamName',
                    width: 150,
                    text: 'Заявка на доступ к экзамену',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания действия',
                    format: 'd.m.Y',
                    width: 150,
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeCancelation',
                    flex: 1,
                    text: 'Основание аннулирования',
                    enumName: 'B4.enums.TypeCancelationQualCertificate',
                    filter: true
                },               
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CancelationDate',
                    text: 'Дата аннулирования',
                    format: 'd.m.Y',
                    width: 150,
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
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
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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