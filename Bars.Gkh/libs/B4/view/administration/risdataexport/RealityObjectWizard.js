Ext.define('B4.view.administration.risdataexport.RealityObjectWizard', {
    extend: 'Ext.form.Panel',
    alias: 'widget.risdataexportrealityobjectwizard',

    requires: [
        'B4.store.dict.Municipality',
        'B4.store.view.ViewRealityObject',

        'B4.enums.FormatDataExportType',
        'B4.enums.TypeHouse',

        'B4.form.SelectField',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.ux.grid.column.Enum',
        'Ext.ux.grid.FilterBar',

        'B4.mixins.WizardPage'
    ],

    mixins: {
        page: 'B4.mixins.WizardPage'
    },

    allowSection: [
        'HouseEntityGroup'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.view.ViewRealityObject', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'RealityObject',
                    listAction: 'FormatDataExportList'
                }
            });

        store.on('beforeload', me.onBeforeLoadStore, me);

        Ext.apply(me, {
            bodyPadding: 5,
            disabled: true,
            items: [               
                {
                    xtype: 'b4grid',
                    title: 'Выбор домов',
                    name: 'RealityObjectList',
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    store: store,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
                            width: 140,
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
                            dataIndex: 'Address',
                            width: 250,
                            text: 'Адрес',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'TypeHouse',
                            text: 'Тип дома',
                            enumName: 'B4.enums.TypeHouse',
                            width: 150,
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
                            dataIndex: 'FiasHauseGuid',
                            flex: 1,
                            text: 'Код ФИАС',
                            filter: { xtype: 'textfield' }
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
                            store: store,
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onBeforeLoadStore: function (store, operation) {
        var me = this,
            baseFormWizard = me.up('risdataexportwizard'),
            duUstavWizard = baseFormWizard.down('risdataexportduustavwizard'),
            contragentId = baseFormWizard.down('b4selectfield[name=MainContragent]').getValue(),
            exportType = baseFormWizard.down('[name=ExportType]').getValue(),
            isDu,
            isUstav;
            
        isDu = exportType == B4.enums.FormatDataExportType.Du;
        isUstav = exportType == B4.enums.FormatDataExportType.Ustav;

        if (isDu) {
            duUstavWizard.setTitle('Договоры управления');
        }
        if (isUstav) {
            duUstavWizard.setTitle('Уставы');
        }

        operation.params = operation.params || {};

        Ext.apply(operation.params, {
            contragentId: contragentId,
            isDu: isDu,
            isUstav: isUstav
        });
    }
});