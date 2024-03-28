Ext.define('B4.view.administration.notify.EditPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.notifyeditpanel',

    requires: [
        'B4.ux.form.field.EnumCheckboxGroup',
        'B4.enums.ButtonType'
    ],

    title: 'Текст уведомления',
    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this,
            buttonItems = [],
            allowButtons = [
                B4.enums.ButtonType.Accept,
                B4.enums.ButtonType.Decline,
                B4.enums.ButtonType.Familiarize
            ];

        Ext.each(B4.enums.ButtonType.getItemsMeta(),
            function (meta) {
                if (allowButtons.indexOf(meta.Value) !== -1) {
                    buttonItems.push(
                        {
                            inputValue: meta.Value,
                            boxLabel: meta.Display
                        }
                    );
                }
            });

        Ext.apply(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'Title',
                    fieldLabel: 'Заголовок',
                    labelAlign: 'right',
                    allowBlank: false,
                    maxLength: 250,
                    maxLengthText: 'Заголовок не должен быть длиннее 250 символов'
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    padding: '5 0 5 0',
                    border: 0,
                    defaults: {
                        xtype: 'datefield',
                        allowBlank: false,
                        format: 'd.m.Y',
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'StartDate',
                            fieldLabel: 'Актуально с',
                        },
                        {
                            name: 'EndDate',
                            fieldLabel: 'Актуально по',
                        }
                    ]
                },
                {
                    xtype: 'htmleditor',
                    name: 'Text',
                    enableSourceEdit: false,
                    styleHtmlContent: true,
                    allowBlank: false,
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            xtype: 'enumcheckboxgroup',
                            name: 'ButtonSet',
                            fieldLabel: 'Отображаемые кнопки',
                            enumName: 'B4.enums.ButtonType',
                            allowButtons: [
                                B4.enums.ButtonType.Accept,
                                B4.enums.ButtonType.Decline,
                                B4.enums.ButtonType.Familiarize
                            ],
                            labelAlign: 'left',
                            labelWidth: 150,
                            columns: 3,
                            margin: '10 5',
                            flex: 1,
                            allowBlank: false,
                            blankText: 'Необходимо выбрать хотя бы одно значение'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});