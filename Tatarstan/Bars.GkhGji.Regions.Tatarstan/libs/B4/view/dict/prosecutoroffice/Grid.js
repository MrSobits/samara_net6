Ext.define('B4.view.dict.prosecutoroffice.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.dict.ProsecutorOffice',
        'B4.enums.ProsecutorOfficeType',
        'B4.form.EnumCombo',
        'B4.ux.grid.filter.YesNo',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Save',
        'B4.ux.button.Update'
    ],
    
    title: 'Справочник прокуратур',
    alias: 'widget.prosecutorofficegrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.dict.ProsecutorOffice');
        
        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ProsecutorOfficeType',
                    text: 'Тип прокуратуры',
                    dataIndex: 'Type',
                    flex: 4,
                    filter: true,
                    editor: {
                        xtype: 'b4enumcombo',
                        enumName: 'B4.enums.ProsecutorOfficeType',
                        allowBlank: false
                    }
                },
                {
                    text: 'Код прокуратуры',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'Code',
                    format: '0',
                    sortable: false,
                    filter: {
                        xtype: 'textfield',
                        operator: 'eq'
                    },
                    editor: {
                        xtype: 'textfield',
                        allowBlank: false
                    }
                },
                {
                    text: 'Код ЕРКНМ',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'ErknmCode',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 10
                    }
                },
                {
                    text: 'Наименование прокуратуры',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'Name',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1024,
                        allowBlank: false
                    }
                },
                {
                    text: 'Код федерального уровня',
                    xtype: 'numbercolumn',
                    flex: 2,
                    dataIndex: 'FederalDistrictCode',
                    format: '0',
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        allowDigits: false,
                        minValue: 0,
                        maxValue: 50,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'numberfield',
                        minValue: 0,
                        maxValue: 50,
                        allowBlank: false
                    }
                },
                {
                    text: 'Наименование федерального уровня',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'FederalDistrictName',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300,
                        allowBlank: false
                    }
                },
                {
                    text: 'Код федерального центра',
                    xtype: 'numbercolumn',
                    flex: 2,
                    dataIndex: 'FederalCenterCode',
                    format: '0',
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        allowDigits: false,
                        minValue: 0,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'numberfield',
                        allowBlank: false
                    }
                },
                {
                    text: 'Наименование федерального центра',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'FederalCenterName',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300,
                        allowBlank: false
                    }
                },
                {
                    text: 'Код района по версии ЕРП',
                    xtype: 'numbercolumn',
                    flex: 2,
                    dataIndex: 'DistrictCode',
                    format: '0',
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        allowDigits: false,
                        minValue: 0,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 200,
                        allowBlank: false
                    }
                },
                {
                    text: 'Код региона (ОКАТО)',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'OkatoTer',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 2,
                        allowBlank: false
                    }
                },
                {
                    text: 'Код района (ОКАТО)',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'OkatoKod1',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 3,
                        allowBlank: false
                    }
                },
                {
                    text: 'Код рабочего поселка/сельсовета (ОКАТО)',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'OkatoKod2',
                    format: '0',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 3,
                        allowBlank: false
                    }
                },
                {
                    text: '	Код населенного пункта (ОКАТО)',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'OkatoKod3',
                    sortable: false,
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 3,
                        allowBlank: false
                    }
                },
                {
                    text: 'Использовать по умолчанию',
                    xtype: 'gridcolumn',
                    flex: 2,
                    dataIndex: 'UseDefault',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: {
                        xtype: 'b4dgridfilteryesno'
                    },
                    editor: {
                        xtype: 'b4dgridfilteryesno',
                        allowBlank: false
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                ,
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Запрос получения справочника',
                                    textAlign: 'left',
                                    itemId: 'btnSendRequest'
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