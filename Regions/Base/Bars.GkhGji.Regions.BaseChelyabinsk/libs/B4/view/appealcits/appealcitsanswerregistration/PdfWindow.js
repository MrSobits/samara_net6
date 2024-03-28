Ext.define('B4.view.appealcits.appealcitsanswerregistration.PdfWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.ansRegPdfWindow',

    requires: [
        'B4.mixins.MaskBody',
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    closeAction: 'destroy',
    modal: true,
    signAspect: null,

    layout: {
        type: 'fit',
        align: 'stretch'
    },

    width: 900,
    height: 750,

    title: 'Регистрация ответа по обращению',

    maximizable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'container',
                            border: false,
                            flex: 1,
                            padding: '5 0 0 0',
                            layout: 'vbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Зарегистрировать',
                                    width: 150,
                                    iconCls: 'icon-accept',
                                    scale: 'medium',
                                    border: true,
                                    itemId: 'btnReg'
                                },
                                {
                                    xtype: 'container',
                                    border: false,
                                    flex: 1,
                                    padding: '5 0 0 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Prefix',
                                            itemId: 'tfPrefix',
                                            width: 100,
                                            labelWidth: 50,
                                            fieldLabel: 'Префикс'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'AnswerNumber',
                                            width: 120,
                                            labelWidth: 40,
                                            itemId: 'nfAnswerNumber',
                                            fieldLabel: 'Номер',
                                            hideTrigger: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Suffix',
                                            itemId: 'tfSuffix',
                                            width: 100,
                                            labelWidth: 50,
                                            fieldLabel: 'Суффикс'
                                        },
                                    ]
                                }, 
                            ]
                        },
                        '->',
                        {
                            xtype: 'container',
                            border: false,
                            width: 200,
                            padding: '5 0 0 0',
                            layout: 'vbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Зарегестрировать и отправить',
                                    width: 200,
                                    margin: '0 0 5 0',
                                    iconCls: 'icon-accept',
                                    scale: 'medium',
                                    bodyPadding: 20,
                                    border: true,
                                    itemId: 'btnRegAndSend'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Email',
                                    itemId: 'tfEmail',
                                    width: 200,
                                    labelWidth: 50,
                                    fieldLabel: 'Email',
                                    disabled: true
                                }
                                
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [           
                        {
                            xtype: 'container',
                            border: false,
                            flex: 1,
                            padding: '5 0 0 0',
                            layout: 'hbox',
                            defaults: {                               
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    text: ' Пропустить ',
                                    width: 110,
                                    scale: 'large',
                                    iconCls: 'icon-decline',
                                    itemId: 'btnSkip'
                                },
                                { xtype: 'component', flex: 3 },
                                {
                                    xtype: 'button',
                                    text: 'Следующий',
                                    width: 110,
                                    iconCls: 'icon-table-go',
                                    scale: 'large',
                                    border: true,
                                    itemId: 'btnNext'
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