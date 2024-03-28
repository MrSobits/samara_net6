/*
* перекрывается в модуле Bars.GkhDi.Regions.Nso
*/
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
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.form.FileField',
        'B4.enums.TypeDocByYearDi'
    ],

    initComponent: function () {
        var me = this,
            typeDocs = B4.enums.TypeDocByYearDi.getStore();

        typeDocs.on('load', function (s) {
            var record = s.findRecord('Value', 40, 0, false, true, true);
            
            if (record) {
                s.remove([record]);
            }
        });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип документа',
                    store: typeDocs,
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