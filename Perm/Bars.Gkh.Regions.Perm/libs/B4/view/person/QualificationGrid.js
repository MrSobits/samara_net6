Ext.define('B4.view.person.QualificationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personqualificationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.YesNo',
        'B4.store.person.QualificationCertificate',
        'B4.enums.TypeCancelationQualCertificate'
    ],

    title: 'Квалификационные аттестаты',
    
    closable: false,
    enableColumnHide: true,

    cls: 'x-large-head',

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
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    text: 'Номер КА',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BlankNumber',
                    text: 'Номер бланка КА',
                    hidden: true,
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IssuedDate',
                    text: 'Дата выдачи',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания действия',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ProtocolDate',
                    text: 'Дата протокола',
                    format: 'd.m.Y',
                    hidden: true,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolNumber',
                    text: 'Номер протокола',
                    hidden: true,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RequestToExamName',
                    text: 'Заявка на доступ к экзамену',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DublicateIssuedDate',
                    text: 'Дата заявления о дубликате',
                    format: 'd.m.Y',
                    hidden: true,
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'RenewIssuedDate',
                    text: 'Дата заявления о переоформлении',
                    format: 'd.m.Y',
                    hidden: true,
                    flex: 1
                },
                {
                    xtype: 'yesnocolumn',
                    dataIndex: 'IsFromAnotherRegion',
                    text: 'КА другого региона',
                    hidden: true,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeCancelation',
                    text: 'Основание аннулирования',
                    renderer: function (val) {
                        if (val) {
                            return B4.enums.TypeCancelationQualCertificate.displayRenderer(val);
                        }
                        return '';
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CancelationDate',
                    text: 'Дата аннулирования',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'yesnocolumn',
                    dataIndex: 'HasRenewed',
                    text: 'Наличие решения об отмене аннулирования',
                    hidden: true,
                    flex: 1
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