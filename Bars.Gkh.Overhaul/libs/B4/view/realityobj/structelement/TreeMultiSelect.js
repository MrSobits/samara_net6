Ext.define('B4.view.realityobj.structelement.TreeMultiSelect', {
    extend: 'Ext.window.Window',

    alias: 'widget.structelselect',

    objectId: null,
    itemId: 'realityObjStructElSelectWindowTree',
    closeAction: 'destroy',
    height: 500,
    width: 900,
    layout: { type: 'vbox', align: 'stretch' },
    maximizable: true,
    mixins: ['B4.mixins.window.ModalMask'],
    trackResetOnLoad: true,
    title: 'Добавление конструктивных элементов',
    titleGridSelect: 'Элементы для выбора',
    titleGridSelected: 'Выбранные элементы',
    storeSelect: null,
    storeSelected: null,
    columnsGridSelected: [],

    bodyPadding: 5,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.Panel',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'Ext.grid.plugin.CellEditing',
        'Ext.selection.CellModel',
        'B4.store.realityobj.StructuralElementTree',
        'B4.plugin.TreeFilter'
    ],

    expandedNodeType: "common",

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.realityobj.StructuralElementTree', {
                realityObjectId: me.objectId
            });

        store.on('beforeexpand', me.onNodeBeforeExpand);
        store.on('beforeload', me.onTreeStoreBeforeLoad, me);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell;">     Отметьте конструктивные элементы, которые присутствуют на данном доме, и заполните данные по ним. Для добавления элементов из обязательных групп конструктивных элементов поставьте фильтр "Только обязательные группы КЭ" и повторите действия.</span>'
                },
                {
                    xtype: 'textfield',
                    name: 'Find',
                    fieldLabel: 'Поиск',
                    margin: '5 0 0 0',
                    labelAlign: 'right',
                    labelWidth: 50,
                    enableKeyEvents: true
                },
                {
                    xtype: 'treepanel',
                    itemId: 'tpSelect',
                    animate: false,
                    autoScroll: true,
                    useArrows: true,
                    containerScroll: true,
                    loadMask: true,
                    store: store,
                    rowLines: true,
                    columnLines: true,
                    columns: [
                        {
                            text: 'Наименование',
                            xtype: 'treecolumn',
                            flex: 2,
                            dataIndex: 'text',
                            sortable: false
                        },
                        {
                            text: 'Кол-во',
                            width: 50,
                            dataIndex: 'Count',
                            sortable: false,
                            editor: {
                                xtype: 'numberfield',
                                allowBlank: false,
                                minValue: 1,
                                allowDecimals: false
                            }
                        },
                        {
                            text: 'Год установки или последнего кап. ремонта',
                            //flex: 1,
                            width: 150,
                            dataIndex: 'LastYear',
                            sortable: false,
                            editor: {
                                xtype: 'numberfield',
                                maxValue: (new Date()).getFullYear(),
                                minValue: 0,
                                allowDecimals: false
                            }
                        },
                        {
                            text: 'Износ (%)',
                            //flex: 1,
                            width: 100,
                            dataIndex: 'Wearout',
                            sortable: false,
                            editor: {
                                xtype: 'numberfield',
                                allowBlank: false,
                                allowDecimals: true,
                                maxValue: 100,
                                minValue: 0
                            }
                        },
                        {
                            text: 'Объем',
                            width: 60,
                            dataIndex: 'Capacity',
                            sortable: false,
                            editor: {
                                xtype: 'numberfield',
                                allowDecimals: true,
                                minValue: 0
                            }
                        },
                        {
                            text: 'Ед. измерения',
                            width: 70,
                            dataIndex: 'UnitMeasure',
                            sortable: false
                        }
                    ],
                    rootVisible: false,
                    padding: '5 0 0 0',
                    viewConfig: {
                        loadMask: true
                    },
                    flex: 1,
                    plugins: [
                        Ext.create('Ext.grid.plugin.CellEditing', {
                            clicksToEdit: 1,
                            pluginId: 'cellEditing',
                            listeners: {
                                beforeedit: function (editor, e) {
                                    return e.record.get('type') == 'elem'
                                        && e.record.get('checked') == true
                                        && (e.record.get('multiple') || e.field != 'Count');
                                }
                            }
                        })
                        ,
                        {
                            ptype: 'treefilter',
                            allowParentFolders: true
                        }
                    ],
                    listeners: {
                        'checkchange': function (node, checked) {
                            if (checked) {
                                node.set('Count', 1);
                                if (node.get('multiple')) {
                                    this.getPlugin('cellEditing').startEdit(node, this.columns[1]);
                                } else {
                                    this.getPlugin('cellEditing').startEdit(node, this.columns[2]);
                                }
                            }
                        }
                    }

                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4savebutton',
                            text: 'Применить'
                        },
                        {
                            xtype: 'button',
                            itemId: 'expandButton',
                            cmd: 'Expand',
                            iconCls: 'icon-arrow-refresh',
                            text: 'Развернуть все'
                        },
                        {
                            xtype: 'checkbox',
                            checked: false,
                            name: 'OnlyRequired',
                            fieldLabel: 'Только обязательные группы КЭ',
                            labelWidth: 180
                        },
                        {
                            xtype: 'checkbox',
                            checked: false,
                            name: 'Detailed',
                            fieldLabel: 'Показать детализированные группы КЭ',
                            labelWidth: 220

                        },
                        { xtype: 'tbfill' },
                        { xtype: 'b4closebutton' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onNodeBeforeExpand: function (node) {
        var me = this;
        me.expandedNodeType = node.get("type");
    },

    onTreeStoreBeforeLoad: function (store, operation) {
        var me = this,
            onlyreqs = false,
            detailed = false,
            findValue = null;
        
        if (me.rendered) {
            onlyreqs = me.down('toolbar checkbox[name="OnlyRequired"]').getValue();
            detailed = me.down('toolbar checkbox[name="Detailed"]').getValue();
            findValue = me.down('textfield[name="Find"]').getValue();
        }
        operation.params.type = me.expandedNodeType;
        operation.params.onlyreqs = onlyreqs;
        operation.params.findValue = findValue;
        operation.params.detailed = detailed;
    }
});