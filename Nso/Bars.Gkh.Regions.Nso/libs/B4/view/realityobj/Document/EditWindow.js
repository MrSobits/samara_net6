Ext.define('B4.view.realityobj.Document.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjDocumentEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 400,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,

    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.RealityObjectDocumentType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип документа',
                    store: B4.enums.RealityObjectDocumentType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'DocumentType'
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 100
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});