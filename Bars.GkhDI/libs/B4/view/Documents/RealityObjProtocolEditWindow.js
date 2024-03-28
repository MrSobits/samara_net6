Ext.define('B4.view.documents.RealityObjProtocolEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjprotocoleditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    minWidth: 300,
    maxWidth: 600,
    bodyPadding: 5,
    itemId: 'realityObjProtocolEditWindow',
    title: 'Протокол общих собраний товарищества или кооператива за текущий год',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'gkhintfield',
                    fieldLabel: 'Год',
                    name: 'Year',
                    hideTrigger: true
                },
                {
                    xtype: 'textfield',
                    name: 'DocNum',
                    fieldLabel: 'Номер',
                    maxLength: 250,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DocDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    maxFileSize: 15728640,
                    fieldLabel: 'Файл',
                    possibleFileExtensions: 'odt,ods,odp,doc,docx,xls,xlsx,ppt,tif,tiff,pptx,txt,dat,jpg,jpeg,png,pdf,gif,rtf'
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