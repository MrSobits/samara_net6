Ext.define('B4.view.suggestion.applicantnotification.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.applicantnotificationwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    height: 250,
    bodyPadding: 5,
    title: 'Редактирование',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '5',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        allowBlank: false,
                        labelAlign: 'right',
                        labelWidth: 120,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            minValue: 0,
                            allowBlank: false
                        },
                        {
                            fieldLabel: 'Статус обращения',
                            name: 'State',
                            xtype: 'b4combobox',
                            url: '/State/GetListByType',
                            editable: false,
                            storeAutoLoad: true,
                            listeners: {
                                storebeforeload: function (field, store, options) {
                                    options.params.typeId = 'gkh_citizen_suggestion';
                                }
                            }
                        },
                        {
                            xtype: 'textfield',
                            name: 'EmailSubject',
                            fieldLabel: 'Тема сообщения',
                            maxLength: 100,
                            allowBlank: false
                        },
                        {
                            xtype: 'textareafield',
                            name: 'EmailTemplate',
                            fieldLabel: 'Шаблон письма',
                            maxLength: 3000,
                            allowBlank: false
                        }
                    ]
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