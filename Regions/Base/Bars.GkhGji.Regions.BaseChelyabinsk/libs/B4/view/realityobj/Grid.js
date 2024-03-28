Ext.define('B4.view.realityobj.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.form.field.ComboBox',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',

        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.enums.HeatingSystem',
        'B4.enums.TypeRoof',
        'B4.enums.YesNo'
    ],

    title: 'Реестр жилых домов',
    store: 'view.ViewRealityObject',
    alias: 'widget.realityobjGrid',
    closable: true,
    enableColumnHide: true,

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
                    dataIndex: 'ExternalId',
                    width: 100,
                    text: 'Внешний Id',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 175,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_real_obj';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                //{
                //    xtype: 'actioncolumn',
                //    text: '',
                //    menuText: 'Запросить доступ на редактирование',
                //    action: 'sendrequestemail',
                //    items: [{
                //        id: 'SendRequestEmailIcon',
                //        tooltip: 'Запросить доступ на редактирование',
                //        iconCls: 'icon-requestemail-button',
                //        icon: B4.Url.content('libs/content/img/emailstatus.png'),
                //    }]
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'District',
                    width: 160,
                    text: 'Административный округ',
                    filter: { xtype: 'textfield' }
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'Municipality',
                     width: 160,
                     text: 'Муниципальный район',
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
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Settlement',
                    width: 160,
                    itemId: 'SettlementColumn',
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlaceName',
                    width: 160,
                    text: 'Населенный пункт',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    width: 250,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'actioncolumn',
                    width: 40,
                    hideable: false,
                    icon: B4.Url.content('content/img/icons/map_go.png'),
                    align: 'center',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'map', rec);
                    },
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeHouse',
                    width: 150,
                    text: 'Тип дома',
                    renderer: function (val) {
                        return B4.enums.TypeHouse.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeHouse.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GkhCode',
                    width: 100,
                    text: 'Код дома',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConditionHouse',
                    width: 100,
                    text: 'Состояние дома',
                    renderer: function (val) {
                        return B4.enums.ConditionHouse.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ConditionHouse.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsCulturalHeritage',
                    width: 100,
                    text: 'Статус объекта культурного наследия',
                    hidden: true,
                    renderer: function (val) {
                        return val ? "Имеется" : "Не имеется";
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: [[false, 'Не имеется'], [true, 'Имеется']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateDemolition',
                    flex: 1,
                    text: 'Дата сноса',
                    format: 'd.m.Y',
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Floors',
                    flex: 1,
                    text: 'Этажность',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberEntrances',
                    flex: 1,
                    text: 'Количество подъездов',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberLiving',
                    flex: 1,
                    text: 'Количество проживающих',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberApartments',
                    flex: 1,
                    text: 'Количество квартир',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLivingNotLivingMkd',
                    width: 100,
                    text: 'Общая площадь жилых и нежилых помещений',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaMkd',
                    width: 100,
                    text: 'Общая площадь',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLiving',
                    flex: 1,
                    text: 'Жилая площадь',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PhysicalWear',
                    flex: 1,
                    text: 'Физический износ',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberLifts',
                    flex: 1,
                    text: 'Количество лифтов',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HeatingSystem',
                    flex: 1,
                    text: 'Тип отопления',
                    hidden: true,
                    renderer: function (val) {
                        return B4.enums.HeatingSystem.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.HeatingSystem.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoofingMaterialName',
                    flex: 1,
                    text: 'Материал кровли',
                    renderer: function (v, m, r) {
                        return r.data.RoofingMaterialName;
                    },
                    hidden: true,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/RoofingMaterial/List',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        triggerAction: 'all',
                        valueField: 'Name',
                        listeners: {
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WallMaterialName',
                    flex: 1,
                    text: 'Материал стен',
                    renderer: function (v, m, r) {
                        return r.data.WallMaterialName;
                    },
                    hidden: true,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/WallMaterial/List',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        triggerAction: 'all',
                        valueField: 'Name',
                        listeners: {
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeRoof',
                    flex: 1,
                    text: 'Тип кровли',
                    hidden: true,
                    renderer: function (val) {
                        return B4.enums.TypeRoof.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeRoof.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateLastOverhaul',
                    text: 'Дата последнего кап. ремонта',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateCommissioning',
                    text: 'Дата сдачи в эксплуатацию',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'DateCommissioningLastSection',
                     text: 'Дата сдачи в эксплуатацию последней секции дома',
                     format: 'd.m.Y',
                     flex: 1,
                     hidden: true,
                     filter: {
                         xtype: 'datefield',
                         operand: CondExpr.operands.eq
                     }
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManOrgNames',
                    flex: 1,
                    text: 'Управляющая организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsInvolvedCr',
                    flex: 1,
                    text: 'Дом участвует в программе КР',
                    hidden: true,
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HouseGuid',
                    flex: 1,
                    text: 'ФИАС Guid',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContracts',
                    flex: 1,
                    text: 'Вид управления',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CapitalGroup',
                    width: 160,
                    text: 'Группа капитальности',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/CapitalGroup/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CodeErc',
                    width: 100,
                    text: 'Код ЕРЦ',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsInsuredObject',
                    flex: 1,
                    text: 'Объект застрахован',
                    hidden: true,
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasVidecam',
                    flex: 1,
                    hidden: true,
                    text: 'Имеются камеры видеонаблюдения',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsBuildSocialMortgage',
                    flex: 1,
                    hidden: true,
                    text: 'Построен по соц.ипотеке',
                    renderer: function (val) {
                        return B4.enums.YesNo.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.YesNo.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalBuildingVolume',
                    flex: 1,
                    text: 'Общий строительный объем',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BuildYear',
                    text: 'Год постройки',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PrivatizationDateFirstApartment',
                    text: 'Дата первой приватизации',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateTechInspection',
                    text: 'Дата проведения тех. обследования',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
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
                    layout: 'vbox',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                xtype: 'textfield',
                                margin: '5 5 5 5'
                            },
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Расчет',
                                    action: 'CreateCalcFecabililty',
                                    iconCls: 'icon-cog-go',
                                    itemId: 'btnCreateCalcFecabililty'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowDemolished',
                                    boxLabel: 'Показать снесенные дома',
                                    labelWidth: 150
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowIndividual',
                                    boxLabel: 'Показать индивидуальные дома',
                                    labelWidth: 150
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowBlockedBuilding',
                                    boxLabel: 'Показать дома блокированной застройки',
                                    labelWidth: 150
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                xtype: 'textfield',
                                margin: '5 5 5 5'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowWoDeliveryAgent',
                                    boxLabel: 'Показать дома без агентов доставки',
                                    labelWidth: 150
                                },
                                {
                                    xtype: 'b4combobox',
                                    operand: CondExpr.operands.eq,
                                    width: 700,
                                    storeAutoLoad: false,
                                    fieldLabel: 'Агент доставки',
                                    editable: false,
                                    itemId: 'deliveryAgentSelect',
                                    valueField: 'Id',
                                    emptyItem: { Name: '-' },
                                    url: '/DeliveryAgent/ListWithoutPaging'
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
    }
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: [[false, 'Не имеется'], [true, 'Имеется']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateDemolition',
                    flex: 1,
                    text: 'Дата сноса',
                    format: 'd.m.Y',
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Floors',
                    flex: 1,
                    text: 'Этажность',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberEntrances',
                    flex: 1,
                    text: 'Количество подъездов',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberLiving',
                    flex: 1,
                    text: 'Количество проживающих',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberApartments',
                    flex: 1,
                    text: 'Количество квартир',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLivingNotLivingMkd',
                    width: 100,
                    text: 'Общая площадь жилых и нежилых помещений',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaMkd',
                    width: 100,
                    text: 'Общая площадь',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLiving',
                    flex: 1,
                    text: 'Жилая площадь',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PhysicalWear',
                    flex: 1,
                    text: 'Физический износ',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberLifts',
                    flex: 1,
                    text: 'Количество лифтов',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HeatingSystem',
                    flex: 1,
                    text: 'Тип отопления',
                    hidden: true,
                    renderer: function (val) {
                        return B4.enums.HeatingSystem.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.HeatingSystem.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoofingMaterialName',
                    flex: 1,
                    text: 'Материал кровли',
                    renderer: function (v, m, r) {
                        return r.data.RoofingMaterialName;
                    },
                    hidden: true,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/RoofingMaterial/List',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        triggerAction: 'all',
                        valueField: 'Name',
                        listeners: {
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WallMaterialName',
                    flex: 1,
                    text: 'Материал стен',
                    renderer: function (v, m, r) {
                        return r.data.WallMaterialName;
                    },
                    hidden: true,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/WallMaterial/List',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        triggerAction: 'all',
                        valueField: 'Name',
                        listeners: {
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeRoof',
                    flex: 1,
                    text: 'Тип кровли',
                    hidden: true,
                    renderer: function (val) {
                        return B4.enums.TypeRoof.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeRoof.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateLastOverhaul',
                    text: 'Дата последнего кап. ремонта',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateCommissioning',
                    text: 'Дата сдачи в эксплуатацию',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'DateCommissioningLastSection',
                     text: 'Дата сдачи в эксплуатацию последней секции дома',
                     format: 'd.m.Y',
                     flex: 1,
                     hidden: true,
                     filter: {
                         xtype: 'datefield',
                         operand: CondExpr.operands.eq
                     }
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManOrgNames',
                    flex: 1,
                    text: 'Управляющая организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsNotInvolvedCr',
                    flex: 1,
                    text: 'Дом участвует в программе КР',
                    hidden: true,
                    renderer: function (val) {
                        return !val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContracts',
                    flex: 1,
                    text: 'Вид управления',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CapitalGroup',
                    width: 160,
                    text: 'Группа капитальности',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/CapitalGroup/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CodeErc',
                    width: 100,
                    text: 'Код ЕРЦ',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsInsuredObject',
                    flex: 1,
                    text: 'Объект застрахован',
                    hidden: true,
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsBuildSocialMortgage',
                    flex: 1,
                    hidden: true,
                    text: 'Построен по соц.ипотеке',
                    renderer: function (val) {
                        return B4.enums.YesNo.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.YesNo.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalBuildingVolume',
                    flex: 1,
                    text: 'Общий строительный объем',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BuildYear',
                    text: 'Год постройки',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PrivatizationDateFirstApartment',
                    text: 'Дата первой приватизации',
                    format: 'd.m.Y',
                    flex: 1,
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
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
                    layout: 'vbox',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                xtype: 'textfield',
                                margin: '5 5 5 5'
                            },
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузка в Excel',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowDemolished',
                                    boxLabel: 'Показать снесенные дома',
                                    labelWidth: 150
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowIndividual',
                                    boxLabel: 'Показать индивидуальные дома',
                                    labelWidth: 150
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowBlockedBuilding',
                                    boxLabel: 'Показать дома блокированной застройки',
                                    labelWidth: 150
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                xtype: 'textfield',
                                margin: '5 5 5 5'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowWoDeliveryAgent',
                                    boxLabel: 'Показать дома без агентов доставки',
                                    labelWidth: 150
                                },
                                {
                                    xtype: 'b4combobox',
                                    operand: CondExpr.operands.eq,
                                    width: 700,
                                    storeAutoLoad: false,
                                    fieldLabel: 'Агент доставки',
                                    editable: false,
                                    itemId: 'deliveryAgentSelect',
                                    valueField: 'Id',
                                    emptyItem: { Name: '-' },
                                    url: '/DeliveryAgent/ListWithoutPaging'
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
    }
});