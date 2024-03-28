Ext.define('B4.view.dict.municipalitytree.Tree', {
    extend: 'Ext.panel.Panel',
    layout: {
        type: 'border',
        padding: 5
    },
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.dict.MunicipalityTree',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Дерево муниципальных образований',
    store: 'dict.Municipality',
    alias: 'widget.municipalityTree',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'center',
                    displayField: 'text',
                    rootVisible: false,
                    xtype: 'treepanel',
                    animate: true,
                    autoScroll: true,
                    useArrows: true,
                    containerScroll: true,
                    loadMask: true,
                    rowLines: true,
                    columnLines: true,
                    treetype: 'parttree',
                    store: Ext.create('B4.store.dict.MunicipalityTree'),
                    itemId: 'municipalityTreeGrid',
                    columns: [
                        {
                            xtype: 'b4editcolumn',
                            scope: me
                        },
                        {
                            text: 'Наименование',
                            xtype: 'treecolumn',
                            flex: 2,
                            dataIndex: 'Name',
                            sortable: false
                        },
                        {
                            text: 'Тип',
                            flex: 1,
                            dataIndex: 'Level',
                            sortable: false,
                            renderer: function (val) {
                                switch(val) {
                                    case 10:
                                        return "Сельское поселение";
                                    case 20:
                                        return "Городское поселение";
                                    case 30:
                                        return "Муниципальный район";
                                    case 40:
                                        return "Городской округ";
                                    case 50:
                                        return "Внутригородская территория города федерального значения";
                                    default:
                                        return "";
                                }
                            }
                        },
                        {
                            text: 'OKATO',
                            flex: 1,
                            dataIndex: 'Okato',
                            sortable: false
                        },
                        {
                            text: 'OKTMO',
                            flex: 1,
                            dataIndex: 'Oktmo',
                            sortable: false
                        },
                        {
                            flex: 1,
                            text: 'Код',
                            dataIndex: 'Code',
                            sortable: false
                        },
                        {
                            text: 'Группа',
                            flex: 1,
                            dataIndex: 'Group',
                            sortable: false
                        },
                        {
                            text: 'Федеральный номер',
                            flex: 1,
                            dataIndex: 'FederalNumber',
                            sortable: false
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    tbar: [
                        {
                            xtype: 'button',
                            text: 'Объединить МО',
                            action: 'UnionMo',
                            iconCls: 'icon-add',
                            scope: me
                        },
                        {
                            xtype: 'b4updatebutton',
                            tooltip: 'Обновить',
                            scope: me
                        },
                        {
                            flex: 1,
                            xtype: 'textfield',
                            name: 'tfSearch',
                            tooltip: 'Найти элемент',
                            emptyText: 'Поиск',
                            enableKeyEvents: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});