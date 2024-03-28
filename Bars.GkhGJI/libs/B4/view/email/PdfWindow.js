Ext.define('B4.view.email.PdfWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.emailpdfWindow',

    requires: [
        'B4.mixins.MaskBody',
        'B4.view.email.PreviewAttachmentGrid',
        'B4.form.EnumCombo',
        'B4.enums.EmailDenailReason'
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

    title: 'Регистрация e-почты',

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
                                    //handler: function (b) {
                                    //    var win = b.up('pdfWindow');
                                    //    win.fireEvent('createsignature', win);
                                    //}
                                },
                                {
                                    xtype: 'radiogroup',
                                    name: 'MessageType',
                                    vertical: true,
                                    columns: 1,
                               //     flex: 0.3,
                                    border: '0 1 0 0',
                                    defaults: {
                                        padding: '1',
                                        name: 'MessageType'
                                    },
                                    items: [
                                        { boxLabel: 'Обращение', inputValue: 0, checked: true },
                                        { boxLabel: 'Заявление', inputValue: 1 },
                                    ],
                                }
                            ]
                        },                        
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
                            name: 'AppealNumber',
                            width: 120,
                            labelWidth: 40,
                            itemId: 'nfAppealNumber',
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
                        '->',
                        {
                            xtype: 'container',
                            border: false,
                            width: 250,
                            padding: '5 0 0 0',
                            layout: 'vbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Отклонить',
                                    width: 200,
                                    margin: '0 0 5 0',
                                    iconCls: 'icon-decline',
                                    scale: 'medium',
                                    bodyPadding: 20,
                                    border: true,
                                    itemId: 'btnDecline'
                                    //handler: function (b) {
                                    //    var win = b.up('pdfWindow');
                                    //    win.fireEvent('createsignature', win);
                                    //}
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'EmailDenailReason',
                                    width: 200,
                                    itemId: 'ecEmailDenailReason',
                                    labelWidth: 50,
                                    bodyPadding: 20,
                                    fieldLabel: 'Причина',
                                    enumName: 'B4.enums.EmailDenailReason',
                                    flex: 1
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'DeclineReason',
                                    itemId: 'taDeclineReason',
                                    width: 200,
                                    bodyPadding: 20,
                                    labelWidth: 50,
                                    fieldLabel: 'Описание',
                                    maxLength: 500,
                                }
                                
                            ]
                        },    
                    //    '->',
                        //{
                        //    xtype: 'buttongroup',
                        //    columns: 1,
                        //    items: [
                        //        {
                        //            xtype: 'b4closebutton',
                        //            handler: function (b) {
                        //                var win = b.up('emailpdfWindow');
                        //                win.close();
                        //            }
                        //        }
                        //    ]
                        //}
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [           
                        {
                            xtype: 'previewemailgjiattachmentgrid',
                            flex:3,
                            height: 150
                        },
                        '->',
                        {
                            xtype: 'container',
                            border: false,
                            flex: 0.6,
                            padding: '5 0 0 0',
                            layout: 'vbox',
                            defaults: {                               
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Следующий',
                                    width: 110,
                                    iconCls: 'icon-table-go',
                                    scale: 'large',
                                    border: true,
                                    itemId: 'btnNext'
                                    //handler: function (b) {
                                    //    var win = b.up('pdfWindow');
                                    //    win.fireEvent('createsignature', win);
                                    //}
                                },
                                {
                                    xtype: 'button',
                                    text: ' Пропустить ',
                                    width: 110,
                                    scale: 'large',
                                    iconCls: 'icon-decline',
                                    itemId: 'btnSkip'
                                    //handler: function (b) {
                                    //    var win = b.up('pdfWindow');
                                    //    win.fireEvent('createsignature', win);
                                    //}
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