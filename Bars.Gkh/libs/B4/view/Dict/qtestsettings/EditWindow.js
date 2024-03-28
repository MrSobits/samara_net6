Ext.define('B4.view.dict.qtestsettings.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 800,
    bodyPadding: 5,
    itemId: 'qtestsettingsEditWindow',
    title: 'Настройки квалификационного экзамена',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                anchor: '100%',
                labelAlign: 'left'
            },
            items: [
            {
                xtype: 'container',
                layout: 'hbox',
                padding: '0 5 5 0',
                defaults: {
                    labelWidth: 100,
                    labelAlign: 'left',
                },
                items: [{
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    allowBlank: false,
                    name: 'DateFrom',
                    fieldLabel: 'Действует с',
                    itemId: 'dfDateStart',
                    flex: 1,
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    allowBlank: true,
                    name: 'DateTo',
                    fieldLabel: 'по',
                    itemId: 'dfDateEnd',
                    flex: 1,
                },
                ]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                padding: '0 5 5 0',
                defaults: {
                    labelWidth: 100,
                    labelAlign: 'left',
                },
                items: [
                    {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        name: 'QuestionsCount',
                        fieldLabel: 'Количество вопросов',
                        flex: 1,
                        minValue: 0
                    },
                    {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        name: 'AcceptebleRate',
                        fieldLabel: 'Требуемый балл',
                        flex: 1,
                        minValue: 0
                    }
                ]
            },           
            {
                xtype: 'container',
                layout: 'hbox',
                padding: '0 5 5 0',
                defaults: {
                    labelWidth: 100,
                    labelAlign: 'left',
                },
                items: [
                    {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        name: 'TimeStampMinutes',
                        fieldLabel: 'Минут на экзамен',
                        flex: 1,
                        minValue: 0
                    },
                    {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        name: 'CorrectBall',
                        fieldLabel: 'Баллов за ответ',
                        flex: 1,
                        minValue: 0
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