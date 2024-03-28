Ext.define('B4.view.finactivity.DocByYearEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    minWidth: 300,
    maxWidth: 600,
    bodyPadding: 5,
    itemId: 'finActivityDocByYearEditWindow',
    title: 'Форма редактирования документа',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.form.FileField',
        'B4.enums.TypeDocByYearDi'
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
                    xtype: 'b4combobox',
                    editable: false,
                    fieldLabel: 'Тип документа',
                    store: B4.enums.TypeDocByYearDi.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeDocByYearDi',
                    itemId: 'cbTypeDocByYearDi'
                },
                {
                    xtype: 'gkhintfield',
                    fieldLabel: 'Год',
                    name: 'Year',
                    allowBlank: false,
                    minValue: 0,
                    negativeText: 'Год не может быть отрицательным'
                },
                {
                    xtype: 'datefield',
                    itemId: 'dfDocumentDate',
                    fieldLabel: 'Дата документа',
                    name: 'DocumentDate',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Файл',
                    name: 'File'
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