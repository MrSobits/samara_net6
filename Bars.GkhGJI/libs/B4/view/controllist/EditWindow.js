Ext.define('B4.view.controllist.EditWindow', {
    extend: 'B4.form.Window',
    itemId: 'controllistEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.KindKNDGJI',
        'B4.form.EnumCombo',
        'B4.view.controllist.QuestionsGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 800,
    bodyPadding: 10,
    title: 'Проверочный лист',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            defaults: {
                            labelWidth: 100,
                            labelAlign: 'right'
                            },
                            layout:
                            {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Код'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    anchor: '100%',
                                    fieldLabel: 'Вид КНД',
                                    enumName: 'B4.enums.KindKNDGJI',
                                    name: 'KindKNDGJI'
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            layout:
                            {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ActualFrom',
                                    allowBlank: true,
                                    fieldLabel: 'Дата с',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ActualTo',
                                    fieldLabel: 'Дата по',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование'
                        },
                        {
                            xtype: 'textfield',
                            name: 'ERKNMGuid',
                            fieldLabel: 'ЕРКНМ Guid'
                        },
                        {
                          xtype: 'tabpanel',
                          border: false,
                          flex: 1,
                          defaults: {
                              border: false
                          },
                          items: [
                             {
                                  xtype: 'controllistquestiongrid',
                                 flex: 1
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