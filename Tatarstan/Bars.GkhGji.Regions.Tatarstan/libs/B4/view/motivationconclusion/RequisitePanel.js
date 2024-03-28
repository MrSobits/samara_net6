Ext.define('B4.view.motivationconclusion.RequisitePanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.motivationconclusionrequisitepanel',

    requires: [
        'B4.enums.YesNo',
        'B4.store.dict.Inspector',
        'B4.store.warningdoc.Violations',
        'B4.form.FileField',
        'B4.ux.grid.Panel',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Реквизиты',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 8,

    initComponent: function() {
        var me = this,
            inspectorStore = Ext.create('B4.store.dict.Inspector'),
            violationStore = Ext.create('B4.store.warningdoc.Violations'),
            defaults = {
                labelWidth: 200,
                labelAlign: 'right'
            };

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: defaults,
                    title: 'Должностные лица',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: inspectorStore,
                            textProperty: 'Fio',
                            name: 'Autor',
                            flex: 1,
                            fieldLabel: 'ДЛ, вынесшее предостережение',
                            columns: [
                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            dockedItems: [
                               {
                                   xtype: 'b4pagingtoolbar',
                                   displayInfo: true,
                                   store: inspectorStore,
                                   dock: 'bottom'
                               }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            store: inspectorStore,
                            name: 'Executant',
                            flex: 1,
                            fieldLabel: 'Ответственный за исполнение',
                            textProperty: 'Fio',
                            columns: [
                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            dockedItems: [
                               {
                                   xtype: 'b4pagingtoolbar',
                                   displayInfo: true,
                                   store: inspectorStore,
                                   dock: 'bottom'
                               }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            readOnly: true,
                            name: 'Inspectors',
                            flex: 1,
                            fieldLabel: 'Инспекторы',
                        }
                    ]
                },
                {
                    xtype: 'b4grid',
                    name: 'Violations',
                    title: 'Перечень нарушений',
                    flex: 1,
                    columnLines: true,
                    store: violationStore,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RealityObject',
                            text: 'Адрес',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Violations',
                            text: 'Выявленные нарушения',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                    ],
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                            xtype: 'b4updatebutton',
                                            handler: function() {
                                                violationStore.load();
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: violationStore,
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});