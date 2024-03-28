Ext.define('B4.view.dpkrdocument.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    height: 800,
    bodyPadding: 5,
    alias: 'widget.dpkrdocumenteditwindow',
    title: 'Документ',

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.store.dpkrdocument.DpkrDocument',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Import',
        'B4.store.dict.BaseDict',
        'B4.store.dpkrdocument.IncludedDpkrDocumentRealityObject',
        'B4.store.dpkrdocument.ExcludedDpkrDocumentRealityObject',
        'B4.view.dpkrdocument.DpkrDocumentRealityObject'
    ],

    initComponent: function () {
        var me = this,
            basicDocKindStore = Ext.create('B4.store.dict.BaseDict', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'BasisOverhaulDocKind'
                }
            });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 180,
                labelAlign: 'right',
                allowBlank: true
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: basicDocKindStore,
                    name: 'DocumentKind',
                    fieldLabel: 'Вид документа',
                    textProperty: 'Name',
                    editable: false,
                    allowBlank: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование документа',
                    allowBlank: false,
                    maxLength: 255
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Документ'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер документа',
                    allowBlank: false,
                    maxLength: 25
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'PublicationDate',
                    fieldLabel: 'Дата публикации документа',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentDepartment',
                    fieldLabel: 'Орган, принявший документ',
                    maxLength: 255
                },
                {
                    xtype: 'container',
                    padding: '0 0 10 0',
                    layout: {
                        type: 'hbox',
                    },
                    defaults: {
                        labelAlign: 'right',
                        flex: 1,
                        labelWidth: 180
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ObligationBefore2014',
                            fieldLabel: 'Дата возникновения обязанности для домов, введенных в эксплуатацию до 2014 г.',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'ObligationAfter2014',
                            fieldLabel: 'Дата возникновения обязанности для домов, введенных в эксплуатацию после 2014 г.',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'realityObjectsTabPanel',
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'dpkrdocumentrealityobject',
                            title: 'Включенные дома',
                            itemId: 'includedDpkrDocumentRealityObject',
                            store: 'dpkrdocument.IncludedDpkrDocumentRealityObject'
                        },
                        {
                            xtype: 'dpkrdocumentrealityobject',
                            title: 'Исключенные дома',
                            itemId: 'excludedDpkrDocumentRealityObject',
                            store: 'dpkrdocument.ExcludedDpkrDocumentRealityObject'
                        }
                    ]
                },
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
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4importbutton',
                                    text: 'Импорт домов',
                                    tooltip: 'Импорт домов',
                                    itemId: 'ImportFileButton',
                                    importId: 'DpkrDocumentImport',
                                    possibleFileExtensions: 'xls,xlsx'
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