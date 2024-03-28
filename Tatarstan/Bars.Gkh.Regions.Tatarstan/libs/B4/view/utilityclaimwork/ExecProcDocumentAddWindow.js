Ext.define('B4.view.utilityclaimwork.ExecProcDocumentAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.execprocdocumentaddwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 200,
    bodyPadding: 5,

    title: 'Документ',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ExecutoryProcessDocumentType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'textfield',
                labelWidth: 100,
                labelAlign: 'right',
                padding: '5 8 0 0'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Документ',
                    enumName: 'B4.enums.ExecutoryProcessDocumentType',
                    name: 'ExecutoryProcessDocumentType'
                },               
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        flex: 1,
                        padding: '5 8 0 0'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Number',
                            fieldLabel: 'Номер',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    name: 'Notation',
                    fieldLabel: 'Примечание',
                    maxLength: 500,
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
                        { xtype: 'tbfill' },
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