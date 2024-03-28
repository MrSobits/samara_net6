Ext.define('B4.view.appealcits.MotivatedPresentationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.motivatedpresentationappealcitsgrid',

    requires: [
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.enums.MotivatedPresentationType',
        'B4.enums.MotivatedPresentationResultType'
    ],

    itemId: 'motivatedPresentationAppealCitsGrid',
    store: 'appealcits.MotivatedPresentation',
    title: 'Мотивированное представление',
    closable: false,

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
                    text: 'Номер',
                    flex: 1,
                    filter: {xtype: 'textfield'}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    text: 'Дата',
                    flex: 1,
                    filter: {xtype: 'datefield', format: 'd.m.Y'}
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'PresentationType',
                    text: 'Объект мероприятия',
                    enumName: 'B4.enums.MotivatedPresentationType',
                    flex: 2,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ResultType',
                    text: 'Вид мероприятия',
                    enumName: 'B4.enums.MotivatedPresentationResultType',
                    flex: 2,
                    filter: true
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
                            columns: 1,
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});