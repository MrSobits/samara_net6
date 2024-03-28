Ext.define('B4.view.integrations.gis.PrepareDataResultWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.preparedataresultwindow',
    requires: [
        'B4.view.integrations.gis.ValidationResultGrid',
        'B4.view.integrations.gis.UploadResultGrid'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    height: 700,
    title: 'Результат подготовки данных',

    validationResult: undefined,
    packages: undefined,
    uploadAttachmentsResult: undefined,
    triggerId: 0,

    initComponent: function () {
        var me = this,
            items = [];

        if (me.validationResult && me.validationResult.length !== 0) {
            items.push(me.getValidationResultPanelCfg(me.validationResult));
        }

        if (me.uploadAttachmentsResult && me.uploadAttachmentsResult.length !== 0) {
            items.push(me.getUploadAttachmentsResultPanelCfg(me.uploadAttachmentsResult));
        }

        if (me.packages && me.packages.length !== 0) {
            items.push(me.getPackagesPanelCfg(me.packages));
        }
        

        Ext.applyIf(me, {
            items: items
        });

        me.callParent(arguments);
    },

    getValidationResultPanelCfg: function (validationResult) {
        var me = this;

        return {
            xtype: 'panel',
            flex: 1,
            title: 'Результат валидации',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'validationresultgrid',
                    flex: 1,
                    validationResult: validationResult,
                    triggerId: me.triggerId,
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
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            name: 'Export'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        };
    },

    getUploadAttachmentsResultPanelCfg: function(uploadAttachmentsResult) {
        var me = this;

        return {
            xtype: 'panel',
            flex: 1,
            title: 'Результаты загрузки вложений',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'uploadresultgrid',
                    flex: 1,
                    uploadAttachmentsResult: uploadAttachmentsResult,
                    triggerId: me.triggerId,
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
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            name: 'Export'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        };
    },

    getPackagesPanelCfg: function (packages) {

        var me = this,
            store = Ext.create('B4.store.RisPackage', {
                //параметры для экспорта
                lastOptions: {
                    params: {
                        triggerId: me.triggerId
                    }
                }
            });

        if (packages && packages.length !== 0) {
            store.loadData(packages);
        }

        return {
            xtype: 'panel',
            title: 'Пакеты данных',
            flex: 1,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'b4grid',
                    flex: 1,
                    store: store,
                    name: 'PackageGrid',
                    columnLines: true,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            align: 'center',
                            text: 'Наименование',
                            dataIndex: 'Name'
                        },
                        {
                            xtype: 'actioncolumn',
                            flex: 1,
                            align: 'center',
                            text: 'Неподписанная XML',
                            dataIndex: 'NotSignedData', // нужно для выгрузки в excel
                            name: 'NotSignedData',
                            renderer: function (val) {
                               // if (val > 0) {
                                    return '<a href="javascript:void(0)" style="color: black;">Просмотр</a>';
                               // }
                              //  return null;
                            }
                        }
                    ],
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
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            name: 'Export'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        };
    }
});