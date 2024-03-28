Ext.define('B4.view.manorglicense.ManOrgRequestRPGUEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minHeight: 250,
    bodyPadding: 5,
    alias: 'widget.manorglicenserequestrpgueditwindow',
    title: 'Форма приложения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.form.ComboBox',
        'B4.enums.RequestRPGUType',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'combobox', editable: false,
                    name: 'RequestRPGUType',
                    itemId: 'cbTypeForm',
                    fieldLabel: 'Вид запроса',
                    displayField: 'Display',
                    store: B4.enums.RequestRPGUType.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'textarea',
                    name: 'Text',
                    fieldLabel: 'Текст запроса',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    name: 'AnswerText',
                    fieldLabel: 'Текст ответа',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл запроса',
                    editable: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'AnswerFile',
                    fieldLabel: 'Файл ответа',
                    editable: false
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