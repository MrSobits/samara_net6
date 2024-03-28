Ext.define('B4.view.realestatetype.TreeMultiSelect', {
    extend: 'Ext.window.Window',

    alias: 'widget.realesttypestructelselect',

    closeAction: 'destroy',
    modal:true,
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
                autoLoad: false
            });

        store.on('beforeexpand', me.onNodeBeforeExpand);
        store.on('beforeload', me.onTreeStoreBeforeLoad, me);

        Ext.applyIf(me, {
            items: [

                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell;">     Выберите конструктивные элементы, которые должны присутствовать и (или) отсутствовать в доме.</span>'
                },
                {
                    xtype: 'treepanel',
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
                            text: 'Конструктивный элемент',
                            xtype: 'treecolumn',
                            flex: 2,
                            dataIndex: 'text',
                            sortable: false
                        },
                        {
                            xtype: 'checkcolumn',
                            dataIndex: 'Exists',
                            defaultValue: true,
                            width: 180,
                            text: 'КЭ присутствует/отсутствует в',
                            renderer: function (value, meta, rec) {
                                var result = "";
                                if (rec.isLeaf()) {
                                    var cssPrefix = Ext.baseCSSPrefix,
                                    cls = [cssPrefix + 'grid-checkheader'];

                                    if (value) {
                                        cls.push(cssPrefix + 'grid-checkheader-checked');
                                    }
                                    result = '<div class="' + cls.join(' ') + '">&#160;</div>';
                                }
                                
                                return result;0
                            }
                        }
                    ],
                    rootVisible: false,
                    padding: '5 0 0 0',
                    viewConfig: {
                        loadMask: true
                    },
                    flex: 1,
                    plugins: [
                        {
                            ptype: 'treefilter',
                            allowParentFolders: true
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
                            xtype: 'b4savebutton',
                            text: 'Применить'
                        },
                        //{
                        //    xtype: 'button',
                        //    itemId: 'expandButton',
                        //    cmd: 'Expand',
                        //    iconCls: 'icon-arrow-refresh',
                        //    text: 'Развернуть все'
                        //},
                        { xtype: 'tbfill' },
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function (btn) {
                                    btn.up('realesttypestructelselect').close();
                                }
                            }
                        }
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
        operation.params.realEstateTypeId = this.realEstateTypeId;
        operation.params.type = this.expandedNodeType;
    }
});