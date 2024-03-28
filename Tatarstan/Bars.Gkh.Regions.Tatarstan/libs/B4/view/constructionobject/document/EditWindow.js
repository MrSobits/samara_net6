Ext.define('B4.view.constructionobject.document.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 100,
    bodyPadding: 5,
    alias: 'widget.constructobjdocumenteditwindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [        
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.enums.ConstructionObjectDocumentType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'combobox',
                    fieldLabel: 'Тип документа',
                    labelAlign: 'right',
                    store: B4.enums.ConstructionObjectDocumentType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'Type',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    labelAlign: 'right',
                    fieldLabel: 'Документ',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Number',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'от',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    labelAlign: 'right',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Contragent',
                    fieldLabel: 'Участник процесса',
                    labelAlign: 'right',
                    store: 'B4.store.Contragent',
                    itemId: 'sfContragent',
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    labelAlign: 'right',
                    fieldLabel: 'Описание',
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
                            columns: 2,
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
                            columns: 2,
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