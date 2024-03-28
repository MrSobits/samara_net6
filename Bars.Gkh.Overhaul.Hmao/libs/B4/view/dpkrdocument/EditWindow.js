Ext.define('B4.view.dpkrdocument.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.dpkrdocumenteditwindow',
    title: 'Документ',

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.store.DpkrDocument',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.BaseDict',
        'B4.form.ComboBox',
        'B4.enums.DpkrDocumentState',
        'B4.view.dpkrdocument.VersionContainer',
        'B4.view.dpkrdocument.RealityObjectContainer'
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
                labelWidth: 120,
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
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Документ',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер документа'
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentDepartment',
                    fieldLabel: 'Орган, принявший документ'
                },
                {
                    xtype: 'dpkrdocumentversioncontainer',
                    margin: '5 0'
                },
                {
                    xtype: 'dpkrdocumentrealityobjectcontainer',
                    margin: '5 0'
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