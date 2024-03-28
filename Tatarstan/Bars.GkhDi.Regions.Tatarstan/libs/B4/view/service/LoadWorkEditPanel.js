Ext.define('B4.view.service.LoadWorkEditPanel', {
    extend: 'Ext.form.Panel',
    trackResetOnLoad: true,
    autoScroll: true,
    border: false,
    height: 600,
    bodyStyle: Gkh.bodyStyle,
    itemId: 'loadWorkEditPanel',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.menu.ManagingOrgRealityObjDataMenu',
        'B4.view.Control.GkhDecimalField'
    ],
    initComponent: function () {
        var me = this;

        var renderer = function (v, meta, record) {
            if (record.get('TypeColor') == 2) {
                meta.style = 'background: Silver;';
            } else if (record.get('TypeColor') == 1) {
                meta.style = 'background: Gainsboro;';
            }

            return v;
        };
        
        var rendererDate = function (v, meta, record) {
            if (record.get('TypeColor') == 2) {
                meta.style = 'background: Silver;';
            } else if (record.get('TypeColor') == 1) {
                meta.style = 'background: Gainsboro;';
            }
            
            return Ext.util.Format.date(v, 'd.m.Y');
        };

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'RealityObj',
                    itemId: 'sflRealityObj',
                    fieldLabel: 'Объект недвижимости',
                    textProperty: 'AddressName',
                   

                    store: 'B4.store.menu.ManagingOrgRealityObjDataMenu',
                    columns: [
                        {
                            text: 'Адрес',
                            dataIndex: 'AddressName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    anchor: '100%',
                    labelWidth: 150,
                    labelAlign: 'right',
                    margins: '0 5 5 0'
                },
                {
                    xtype: 'container',
                    bodyPadding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        '<span><div style = "display: inline-block; background-color: White; width: 20px; height: 20px;"></div> - Услуга и работы добавлена в дом</span><br/>' +
                        '<span><div style = "display: inline-block; background-color: Gainsboro; width: 20px; height: 20px;"></div> - Услуга добавлена, но работа отсутствует. При вводе данных работа добавляется</span><br/>' +
                        '<span><div style = "display: inline-block; background-color: Silver; width: 20px; height: 20px;"></div> - Услуга не добавлена, ввод данных невозможен</span><br/>' +
                        '</span>'
                },                
                {
                    xtype: 'tabpanel',
                    itemId: 'tpLoadWork',
                    border: false,
                    margins: -1,
                    anchor: '100% -100',
                    items: [
                        {
                            xtype: 'b4grid',
                            title: 'ППР',
                            sortableColumns: false,
                            store: 'service.LoadWorkPprRepair',
                            itemId: 'loadWorkPprGrid',
                            columnLines: true,
                            enableColumnHide: true,
                            columns: [
                                 {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ServiceName',
                                    flex: 4,
                                    text: 'Наименование услуги',
                                    renderer: renderer
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ProviderName',
                                    flex: 1,
                                    text: 'Поставщик',
                                    hidden: true,
                                    renderer: renderer
                                },                                
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    flex: 4,
                                    text: 'Группа ППР',
                                    renderer: renderer
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'PlannedCost',
                                    flex: 1,
                                    text: 'План. стоимость (руб.).',
                                    editor: 'gkhdecimalfield',
                                    renderer: renderer
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'FactCost',
                                    flex: 1,
                                    text: 'Факт стоимость',
                                    editor: 'gkhdecimalfield',
                                    renderer: renderer
                                },
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'DateStart',
                                    flex: 1,
                                    text: 'Дата начала',
                                    format: 'd.m.Y',
                                    editor: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y'
                                    },
                                    renderer: rendererDate
                                },
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'DateEnd',
                                    flex: 1,
                                    text: 'Дата окончания',
                                    format: 'd.m.Y',
                                    editor: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y'
                                    },
                                    renderer: rendererDate
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'InfoAboutExec',
                                    flex: 1,
                                    text: 'Cведения о выполнении',
                                    editor: 'textfield',
                                    renderer: renderer
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ReasonRejection',
                                    flex: 1,
                                    text: 'Причина отклонения',
                                    editor: 'textfield',
                                    renderer: renderer
                                }
                            ],
                            plugins: [
                                Ext.create('Ext.grid.plugin.CellEditing', {
                                    clicksToEdit: 1,
                                    pluginId: 'cellEditing'
                                })
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
                                                    xtype: 'button',
                                                    itemId: 'saveWorkPprButton',
                                                    text: 'Сохранить',
                                                    tooltip: 'Сохранить',
                                                    iconCls: 'icon-add'
                                                },
                                                {
                                                    xtype: 'button',
                                                    itemId: 'reloadWorkPprButton',
                                                    text: 'Обновить',
                                                    tooltip: 'Обновить',
                                                    iconCls: 'icon-arrow-refresh'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'grid',
                            title: 'TO',
                            store: 'service.LoadWorkToRepair',
                            itemId: 'loadWorkToGrid',
                            columnLines: true,
                            sortableColumns: false,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    text: 'Наименование услуги',
                                    renderer: renderer
                                },
                                {
                                     xtype: 'gridcolumn',
                                    dataIndex: 'ProviderName',
                                    flex: 1,
                                    text: 'Поставщик',
                                    hidden: true,
                                    renderer: renderer
                                },                                
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'SumWorkTo',
                                    width: 200,
                                    text: 'Сумма на ТО (руб.)',
                                    editor: 'gkhdecimalfield',
                                    renderer: renderer
                                }
                            ],
                            plugins: [
                                Ext.create('Ext.grid.plugin.CellEditing', {
                                    clicksToEdit: 1,
                                    pluginId: 'cellEditing'
                                })
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
                                                    xtype: 'button',
                                                    itemId: 'saveWorkToButton',
                                                    text: 'Сохранить',
                                                    tooltip: 'Сохранить',
                                                    iconCls: 'icon-add'
                                                },
                                                {
                                                    xtype: 'button',
                                                    itemId: 'reloadWorkToButton',
                                                    text: 'Обновить',
                                                    tooltip: 'Обновить',
                                                    iconCls: 'icon-arrow-refresh'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
