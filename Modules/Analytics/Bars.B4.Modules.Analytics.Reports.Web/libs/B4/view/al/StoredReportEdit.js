Ext.define('B4.view.al.StoredReportEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.storedreportedit',
    width: 600,
    height: 500,
    layout: 'card',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Add',
        'B4.ux.button.Save',

        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.SelectField',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',

        'B4.store.PrintFormCategory',

        'B4.enums.al.ReportType',
        'B4.enums.al.ReportEncoding',
        'B4.enums.al.OwnerType',

        'B4.model.al.ReportParam',
        'B4.model.al.DataSource',
        'B4.model.al.Role'
    ],

    modal: true,
    title: 'Редактирование отчета',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.al.DataSource',
                proxy: {
                    type: 'memory'
                },
                autoLoad: true
            }),
            paramStore = Ext.create('B4.base.Store', {
                model: 'B4.model.al.ReportParam',
                autoLoad: false
            }),
            rolesStore = Ext.create('B4.base.Store', {
                model: 'B4.model.al.Role',
                autoLoad: false
            });

        Ext.apply(me, {
            items: [
                {
                    title: 'Источники данных',
                    layout: 'fit',
                    minHeight: 200,
                    border: false,
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
                                            xtype: 'b4addbutton',
                                            name: 'datasource'
                                        }
                                    ]
                                }
                            ]
                        }
                    ],
                    items: [
                        {
                            xtype: 'grid',
                            border: false,
                            sortableColumns: false,
                            store: store,
                            columnLines: true,
                            name: 'dataStore',
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    text: 'Наименование',
                                    flex: 1
                                },
                                {
                                    xtype: 'b4deletecolumn'
                                }
                            ]
                        }
                    ]
                },
                {
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    bodyStyle: 'background: transparent;',
                    bodyPadding: 8,
                    defaults: {
                        margin: '7 0',
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    title: 'Свойства отчета',
                    items: [
                        {
                            xtype: 'hidden',
                            margin: 0,
                            name: 'DataSourcesIds'
                        },
                        {
                            xtype: 'hidden',
                            margin: 0,
                            name: 'TemplateFile'
                        },
                        {
                            xtype: 'hidden',
                            margin: 0,
                            name: 'DeletedParamsIds'
                        },
                        {
                            xtype: 'hidden',
                            margin: 0,
                            name: 'ReportParams'
                        },
                        {
                            xtype: 'hidden',
                            margin: 0,
                            name: 'Id'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код отчета'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DisplayName',
                            fieldLabel: 'Наименование'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Системное наименование'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Category',
                            store: 'B4.store.PrintFormCategory',
                            fieldLabel: 'Категория',
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    text: 'Наименование',
                                    flex: 1
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Description',
                                    text: 'Описание',
                                    flex: 1
                                }
                            ],
                            selectionMode: "SINGLE"
                        },
                        {
                            xtype: 'b4enumcombo',
                            name: 'ReportType',
                            enumName: 'B4.enums.al.ReportType',
                            fieldLabel: 'Тип отчета',
                            readOnly: true
                        },
                        {
                            xtype: 'b4enumcombo',
                            name: 'ReportEncoding',
                            enumName: 'B4.enums.al.ReportEncoding',
                            fieldLabel: 'Кодировка'
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Использовать параметры соединения с БД из шаблона',
                            name: 'UseTemplateConnectionString',
                            labelWidth: 315
                        },
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Формировать на сервере расчетов',
                            name: 'GenerateOnCalcServer',
                            labelWidth: 315
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Описание',
                            name: 'Description',
                        }
                    ]
                },
                {
                    title: 'Права доступа',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    minHeight: 200,
                    name: 'RolesPanel',
                    border: false,
                    bodyStyle: 'background: transparent;',
                    items: [
                        {
                            xtype: 'container',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Для всех ролей',
                                    name: 'ForAll',
                                    labelWidth: 150,
                                    margin: '5 0 10 0'
                                }
                            ]
                        },
                        {
                            xtype: 'grid',
                            flex: 1,
                            border: false,
                            disabled: true,
                            name: 'rolesGrid',
                            store: rolesStore,
                            title: 'Отметьте роли, для которых будет доступен отчет',
                            selModel: Ext.create('Ext.selection.CheckboxModel', {
                                mode: 'MULTI',
                                checkOnly: false,
                                ignoreRightMouseSelection: true,
                                checkSelector: '.' + Ext.baseCSSPrefix + 'grid-row-checker',
                                tdCls: Ext.baseCSSPrefix + 'grid-cell-special ' + Ext.baseCSSPrefix + 'grid-cell-row-checker',

                                toggleUiHeader: function (isChecked) {
                                    var view = this.views[0],
                                        headerCt = view.headerCt,
                                        checkHd = headerCt.child('gridcolumn[isCheckerHd]'),
                                        cls = this.checkerOnCls;

                                    if (checkHd) {
                                        if (isChecked) {
                                            checkHd.addCls(cls);
                                        } else {
                                            checkHd.removeCls(cls);
                                        }
                                    }
                                }
                            }),
                            columnLines: true,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    text: 'Наименование'
                                }
                            ]
                        }
                    ]
                },
                {
                    title: 'Входные параметры',
                    layout: 'fit',
                    minHeight: 200,
                    border: false,

                    items: [
                        {
                            xtype: 'grid',
                            border: false,
                            sortableColumns: false,
                            name: 'paramGrid',
                            store: paramStore,
                            columnLines: true,
                            columns: [
                                {
                                    xtype: 'b4editcolumn'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Label',
                                    text: 'Наименование',
                                    flex: 1
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    text: 'Системное название',
                                    flex: 1
                                },
                                {
                                    xtype: 'b4enumcolumn',
                                    dataIndex: 'OwnerType',
                                    enumName: 'B4.enums.al.OwnerType',
                                    text: 'Тип',
                                    flex: 1
                                },
                                {
                                    xtype: 'b4enumcolumn',
                                    dataIndex: 'ParamType',
                                    enumName: 'B4.enums.al.ParamType',
                                    text: 'Тип поля',
                                    flex: 1
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Required',
                                    text: 'Обязательность',
                                    flex: 1,
                                    renderer: function(val) {
                                        return val ? 'Да' : 'Нет';
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Multiselect',
                                    text: 'Множ. выбор',
                                    flex: 1,
                                    renderer: function(val) {
                                        return val ? 'Да' : 'Нет';
                                    }
                                },
                                {
                                    xtype: 'b4deletecolumn'
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            columns: 4,
                                            items: [
                                                {
                                                    xtype: 'b4addbutton'
                                                },
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
                                    store: paramStore,
                                    dock: 'bottom'
                                }
                            ]
                        }
                    ]
                },
                {
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    bodyStyle: 'background: transparent;',
                    bodyPadding: 8,
                    defaults: {
                        margin: '7 0',
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    title: 'Шаблон',
                    name: 'templateCard',
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'Template',
                            fieldLabel: 'Шаблон отчета',
                            possibleFileExtensions: 'mrt',
                            getFileUrl: function(id) {
                                return B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'StoredReport', 'GetTemplate', id));
                            }
                        },
                        {
                            xtype: 'container',
                            itemId: 'needSaveNotification',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Для получения исходного шаблона и онлайн редактирования требуется сохранить параметры отчета</span>'
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyPadding: 8,
                            border: false,
                            bodyStyle: 'background: transparent;',
                            itemId: 'magicPanel',
                            disabled: true,
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Добавить соединение к БД',
                                    name: 'addConn',
                                    labelWidth: 160,
                                    visible: false
                                },
                                {
                                    xtype: 'button',
                                    text: 'Скачать исходный шаблон',
                                    iconCls: 'icon-page-save',
                                    action: 'DownloadEmptyTemplate'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Редактировать в онлайн-редакторе',
                                    iconCls: 'icon-page-edit',
                                    action: 'OpenStimulDesigner'
                                }
                            ]
                        }
                    ]
                }
            ],
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
                                    xtype: 'b4savebutton',
                                    name: 'parameter'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ],
            bbar: [
                {
                    id: 'move-prev',
                    iconCls: 'icon-arrow-left',
                    text: 'Назад',
                    handler: function (btn) {
                        me.navigate(btn.up("panel"), "prev");
                    },
                    disabled: true
                },
                '->',
                {
                    id: 'move-next',
                    iconCls: 'icon-arrow-right',
                    iconAlign: 'right',
                    text: 'Далее',
                    handler: function (btn) {
                        me.navigate(btn.up("panel"), "next");
                    }
                }
            ]
        });

        me.callParent(arguments);
    },

    navigate: function (panel, direction) {
        var layout = panel.getLayout();
        layout[direction]();
        Ext.getCmp('move-prev').setDisabled(!layout.getPrev());
        Ext.getCmp('move-next').setDisabled(!layout.getNext());
    }
});