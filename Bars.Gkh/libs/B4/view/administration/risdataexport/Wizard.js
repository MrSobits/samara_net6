Ext.define('B4.view.administration.risdataexport.Wizard', {
    extend: 'B4.form.Window',

    alias: 'widget.risdataexportwizard',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.Panel',
        'B4.store.Contragent',
        'B4.store.dict.Municipality',
        'B4.ux.form.SchedulerPanel',
        'B4.enums.FormatDataExportType',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.view.administration.risdataexport.GjiWizard',
        'B4.view.administration.risdataexport.RegopWizard',
        'B4.view.administration.risdataexport.OverhaulLongTermWizard',
        'B4.view.administration.risdataexport.OverhaulShortTermWizard',
        'B4.view.administration.risdataexport.DuUstavWizard',
        'B4.view.administration.risdataexport.RealityObjectWizard',
        'Ext.ux.grid.FilterBar',
        'B4.store.administration.risdataexport.FormatDataExportSection'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    title: 'Экспорт данных системы в РИС ЖКХ',

    minWidth: 720,
    minHeight: 430,
    width: 850,
    height: 430,
    closeAction: 'destroy',
    layout: {
        type: 'card',
        getLayoutItems: function () {
            var owner = this.owner,
                items = (owner && owner.items && owner.items.items) || [];

            return items.filter(function (i) {
                return !i.disabled;
            });
        }
    },

    initComponent: function () {
        var me = this,
            layout = me.getLayout(),
            sectionStore = Ext.create('B4.store.administration.risdataexport.FormatDataExportSection');

        sectionStore.on('beforeload', function(store, ops) {
            var exportType = me.down('[name=ExportType]');
            if (exportType) {
                Ext.apply(ops.params, {
                    exportType: exportType.getValue()
                });
            }
        });
        Ext.applyIf(me, {
            defaults: {
                xtype: 'form',
                border: false,
                bodyStyle: Gkh.bodyStyle,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                defaults: {
                    labelWidth: 210,
                    labelAlign: 'right',
                    defaults: {
                        labelWidth: 210,
                        labelAlign: 'right'
                    },
                },
                isValid: function() {
                    return true;
                }
            },
            items: [
                {
                    name: 'page0',
                    type: 'edit',
                    bodyPadding: 5,
                    items: [
                        {
                            xtype: 'propertygrid',
                            nameColumnWidth: '45%',
                            title: 'Параметры задачи',
                            sortableColumns: false,
                            source: {},
                            listeners: {
                                'beforeedit': {
                                    fn: function () {
                                        return false;
                                    }
                                }
                            }
                        }
                    ]
                },
                {
                    name: 'page1',
                    type: 'create',
                    title: 'Выберите выгружаемые секции',
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'ExportType',
                            padding: '5 10 0 0',
                            fieldLabel: 'Выберите блок сведений',
                            enumName: 'B4.enums.FormatDataExportType',
                            listeners: {
                                change: function (newValue) {
                                    var grid = me.down('b4grid[name=EntityGroup]');
                                    if (newValue !== undefined) {
                                        grid.getSelectionModel().b4deselectAll();
                                        sectionStore.load();
                                    }
                                },
                                scope: me
                            }
                        },
                        {
                            xtype: 'b4grid',
                            name: 'EntityGroup',
                            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {
                                idProperty: 'Code',
                            }),
                            flex: 1,
                            store: sectionStore,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    text: 'Название секции',
                                    dataIndex: 'Description',
                                    flex: 1,
                                    filter: { xtype: 'textfield' },
                                    renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                                        var sections = record.get('InheritedEntityCodeList') || [];
                                        if (!Ext.isEmpty(sections)) {
                                            metaData.tdAttr = Ext.String.format('data-qtip="Секции:<br/>{0}"', sections.join('<br/>\n'));
                                        }

                                        return value;
                                    },
                                }
                            ],
                            plugins: [
                                {
                                    ptype: 'filterbar',
                                    renderHidden: false,
                                    showShowHideButton: false,
                                    showClearAllButton: false
                                }
                            ],
                            viewConfig: {
                                loadMask: true,
                            },
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    name: 'buttons',
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
                                    store: sectionStore,
                                    dock: 'bottom'
                                }
                            ]
                        }
                    ],
                    isValid: function() {
                        return this.down('grid').selModel.getSelection().length === 0 ? 'Не выбран тип экспорта' : true;
                    }
                },
                {
                    name: 'page2',
                    type: 'create',
                    bodyPadding: 5,
                    items: [
                        {
                            xtype: 'fieldset',
                            name: 'MandatoryParams',
                            title: 'Обязательные параметры экспорта',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'MainContragent',
                                    fieldLabel: 'Головная организация',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    allowBlank: false,
                                    columns: [
                                        {
                                            text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListMoAreaWithoutPaging'
                                            }
                                        },
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Юридический адрес', dataIndex: 'JuridicalAddress', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'MunicipalityList',
                                    fieldLabel: 'Муниципальный район',
                                    store: 'B4.store.dict.Municipality',
                                    textProperty: 'Name',
                                    selectionMode: 'MULTI',
                                    isRendered: true, // скрыть кнопку Выбрать все
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 3, filter: { xtype: 'textfield' } },
                                        { text: 'ОКАТО', dataIndex: 'Okato', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'ОКТМО', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ContragentList',
                                    fieldLabel: 'Контрагенты для выгрузки информации',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    selectionMode: 'MULTI',
                                    isRendered: true, // скрыть кнопку Выбрать все
                                    columns: [
                                        {
                                            text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListMoAreaWithoutPaging'
                                            }
                                        },
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Юридический адрес', dataIndex: 'JuridicalAddress', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                }
                            ],
                        },
                        {
                            xtype: 'fieldset',
                            name: 'AdditionalParams',
                            title: 'Дополнительные параметры экспорта',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Инкрементальная выгрузка',
                                    name: 'UseIncremental',
                                    flex: 1
                                },
                                {
                                    xtype: 'container',
                                    name: 'EditDateInterval',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    padding: '0 0 10 110',
                                    defaults: {
                                        disabled: true,
                                        labelWidth: 100,
                                        width: 230,
                                        labelAlign: 'right',
                                        padding: '5 15 5 0'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'StartEditDate',
                                            fieldLabel: 'Дата с'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'EndEditDate',
                                            fieldLabel: 'Дата по'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Максимальный размер архива (МБ)',
                                    name: 'MaxFileSize',
                                    decimalPrecision: 0,
                                    minValue: 1,
                                    maxValue: 4096,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Отдельная выгрузка файлов',
                                    name: 'IsSeparateArch',
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Не выгружать файлы с ошибками',
                                    name: 'NoEmptyMandatoryFields',
                                    flex: 1
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Не выгружать ссылки на отсутствующие файлы',
                                    name: 'OnlyExistsFiles',
                                    flex: 1
                                }
                            ],
                        },
                    ],
                    isValid: function () {
                        return this.getForm().isValid() === false ? 'Не заполнены обязательные поля' : true;
                    }
                },
                {
                    xtype: 'risdataexportgjiwizard',
                    title: 'ГЖИ',
                    type: 'create',
                },
                {
                    xtype: 'risdataexportregopwizard',
                    title: 'Лицевые счета',
                    type: 'create',
                },
                {
                    xtype: 'risdataexportoverhaullongtermwizard',
                    title: 'ДПКР',
                    type: 'create',
                },
                {
                    xtype: 'risdataexportoverhaulshorttermwizard',
                    title: 'КПКР',
                    type: 'create',
                },
                {
                    xtype: 'risdataexportduustavwizard',
                    type: 'create',
                },
                {
                    xtype: 'risdataexportrealityobjectwizard',
                    type: 'create',
                },
                {
                    name: 'schedulerPage',
                    bodyPadding: 5,
                    disabled: true,
                    items: [
                        {
                            xtype: 'schedulerpanel'
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
                            action: 'prev',
                            type: 'navigation',
                            text: 'Назад',
                            iconCls: 'icon-arrow-left',
                            disabled: true
                        },
                        '->',
                        {
                            action: 'next',
                            type: 'navigation',
                            text: 'Далее',
                            iconCls: 'icon-arrow-right',
                            iconAlign: 'right'
                        },
                        {
                            xtype: 'b4savebutton',
                            action: 'save',
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});