Ext.define('B4.view.warningdoc.ViolationsEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minHeight: 250,
    bodyPadding: 5,
    itemId: 'warningdocViolationsEditWindow',
    title: 'Добавление нарушения',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.RealityObjectGji',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.ViolationGji',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    initComponent: function() {
        var me = this,
            realityObjectStore = Ext.create('B4.store.RealityObjectGji'),
            normativeStore = Ext.create('B4.store.dict.NormativeDoc'),
            violationStore = Ext.create('B4.store.dict.ViolationGji');

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: realityObjectStore,
                    name: 'RealityObject',
                    idProperty: 'RealityObjectId',
                    fieldLabel: 'Проверяемый объект',
                    textProperty: 'Address',
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 3, filter: { xtype: 'textfield' } },
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: realityObjectStore,
                            dock: 'bottom'
                        }
                    ],
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    store: normativeStore,
                    name: 'NormativeDoc',
                    fieldLabel: 'Нормативный документ',
                    textProperty: 'Name',
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 3, filter: { xtype: 'textfield' } },
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: normativeStore,
                            dock: 'bottom'
                        }
                    ],
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    store: violationStore,
                    name: 'Violations',
                    fieldLabel: 'Нарушения',
                    textProperty: 'Name',
                    selectionMode: 'MULTI',
                    columns: [
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'CodePin', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 3, filter: { xtype: 'textfield' } }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: violationStore,
                            dock: 'bottom'
                        }
                    ],
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'TakenMeasures',
                    fieldLabel: 'Принятые меры',
                    maxLength: 500
                },
                {
                    xtype: 'datefield',
                    name: 'DateSolution',
                    fieldLabel: 'Срок устранения нарушения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание нарушения',
                    maxLength: 2000,
                    flex: 1
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
                                    xtype: 'b4savebutton'
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
            ]
        });

        me.callParent(arguments);
    }
});