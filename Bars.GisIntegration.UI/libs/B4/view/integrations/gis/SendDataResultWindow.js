Ext.define('B4.view.integrations.gis.SendDataResultWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.senddataresultwindow',
    requires: [
        'B4.enums.MessageType',
        'B4.ux.grid.column.Enum'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    height: 600,
    title: 'Результат отправки данных',

    triggerId: 0,
    packageId: 0,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.ObjectProcessingResult'),
            showPackageColumn = !(me.packageId > 0);

        store.on('beforeload', me.beforeStoreLoad, me);

        var items = [{
                xtype: 'b4grid',
                store: store,
                name: 'ObjectProcessingResultGrid',
                flex: 1,
                columns: me.getColumns(showPackageColumn),
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
            }];


        Ext.applyIf(me, {
            items: items
        });

        me.on('afterrender',
            function () {
                store.load();
            },
            me);

        me.callParent(arguments);
    },

    getColumns: function (addPackageColumn) {
        var result = [];

        if (addPackageColumn === true) {

            result.push({
                text: 'Наименование пакета',
                width: 120,
                sortable: true,
                dataIndex: 'PackageName',
                filter: {
                    xtype: 'textfield'
                }
            });
        }

        result = result.concat([
            {
                text: 'Адаптер: Id объекта',
                width: 110,
                sortable: true,
                dataIndex: 'RisId',
                filter: {
                    xtype: 'numberfield',
                    operand: CondExpr.operands.eq, hideTrigger: true
                }
            },
            {
                text: 'Сторонняя система: Id объекта',
                width: 110,
                sortable: true,
                dataIndex: 'ExternalId',
                filter: {
                    xtype: 'numberfield',
                    operand: CondExpr.operands.eq, hideTrigger: true
                }
            },
            {
                text: 'Система ГИС: ID объекта',
                width: 130,
                sortable: true,
                dataIndex: 'GisId',
                filter: {
                    xtype: 'textfield'
                }
            },
            {
                text: 'Описание объекта',
                flex: 1,
                sortable: true,
                dataIndex: 'Description',
                filter: {
                    xtype: 'textfield'
                }
            },
            {
                xtype: 'b4enumcolumn',
                enumName: 'B4.enums.ObjectProcessingState',
                dataIndex: 'State',
                text: 'Статус',
                width: 90,
                filter: true
            },
            {
                text: 'Сообщение',
                flex: 1,
                sortable: true,
                dataIndex: 'Message',
                filter: {
                    xtype: 'textfield'
                }
            }
        ]);

        return result;
    },

    beforeStoreLoad: function (store, operation, eOpts) {
        var me = this,
            params = operation.params;

        if (me.triggerId > 0) {
            Ext.apply(params, {
                triggerId: me.triggerId
            });
        }

        if (me.packageId > 0) {
            Ext.apply(params, {
                packageId: me.packageId
            });
        }
    }
});