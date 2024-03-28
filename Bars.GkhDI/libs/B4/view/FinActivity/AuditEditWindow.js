Ext.define('B4.view.finactivity.AuditEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    minWidth: 300,
    maxWidth: 600,
    bodyPadding: 5,
    itemId: 'finActivityAuditEditWindow',
    title: 'Форма редактирования аудиторской проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.form.FileField',
        
        'B4.enums.TypeAuditStateDi'
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
                    itemId: 'nfYear',
                    allowBlank: false,
                    minValue: 0,
                    negativeText: 'Год не может быть отрицательным'
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Состояние проверки',
                    store: B4.enums.TypeAuditStateDi.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeAuditStateDi',
                    itemId: 'cbTypeAuditStateDi'
                },
                {
                    xtype: 'b4filefield',
                    possibleFileExtensions: 'pdf',
                    fieldLabel: 'Файл',
                    name: 'File',
                    itemId: 'ffFile'
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