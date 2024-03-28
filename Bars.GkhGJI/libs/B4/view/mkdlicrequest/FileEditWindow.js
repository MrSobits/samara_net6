Ext.define('B4.view.mkdlicrequest.FileEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 450,
    minWidth: 400,
    minHeight: 250,
    height: 250,
    bodyPadding: 5,
    itemId: 'mkdlicrequestFileEditWindow',
    title: 'Приложение',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.LicStatementDocType'

    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    anchor: '100%',
                    fieldLabel: 'Тип документа',
                    enumName: 'B4.enums.LicStatementDocType',
                    name: 'LicStatementDocType'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование документа',
                    maxLength: 100
                },
                {
                    xtype: 'datefield',
                    name: 'DocDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    maxLength: 1500
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    fieldLabel: 'Файл',
                    allowBlank: false,
                    editable: false
                },
                
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