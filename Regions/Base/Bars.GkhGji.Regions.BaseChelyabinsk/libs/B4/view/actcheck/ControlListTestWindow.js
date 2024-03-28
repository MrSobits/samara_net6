Ext.define('B4.view.actcheck.ControlListTestWindow',
    {
        extend: 'B4.form.Window',
        alias: 'widget.controllistwindow',
        mixins: ['B4.mixins.window.ModalMask'],
        layout: 'hbox',
        width: 772,
        minWidth: 772,
        maxWidth: 772,
        title: 'Вопрос проверочного листа',
        closeAction: 'destroy',
        step: 0,
        qid: 0,
        modal: true,
        closable: false,

        requires: [
            'B4.ux.button.Close',
            'B4.ux.button.Save',
            'B4.form.FileField',
            'B4.enums.TypeCancelationQualCertificate',
            'B4.form.SelectField'            
        ],

        initComponent: function () {
            var me = this;

            Ext.applyIf(me,
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'fit'
                    },
                    width: 640,
                    frame: false,
                    //padding: '0 0 0 0',
                    //margin: '0 0 0 0',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'fit',
                                pack: 'center'
                            },
                            width: 640,
                            frame: false,
                            padding: '0 0 0 0',
                            margin: '0 0 0 0',
                            items: [
                                {
                                    xtype: 'panel',
                                    frame: false,
                                    itemId: 'focusPanelTwo',
                                    bodyStyle: 'background-color: #dfe8f6;',
                                    height: 100,
                                    layout: {
                                        type: 'hbox',
                                        align: 'fit',
                                        pack: 'center'
                                    },
                                    width: 640,
                                    items: [
                                        {
                                            xtype: 'label',
                                            padding: '5 5 5 5',
                                            margin: '10 10 10 10',
                                            labelWidth: 640,
                                            labelAlign: 'center',
                                            itemId: 'NameCCLId',
                                            name: 'NameCCL',
                                            style: {
                                                'font-weight': 'bold',
                                                'font-size': '14px',
                                                'text-align': 'center',
                                                'margin': '0 auto'
                                            },
                                            hidden: false,
                                            text: 'Название ПЛ'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'panel',
                                    frame: false,
                                    itemId: 'focusPanel',
                                    bodyStyle: 'background-color: #dfe8f6;',
                                    height: 200,
                                    layout: {
                                        type: 'hbox',
                                        align: 'fit',
                                        pack: 'center'
                                    },
                                    width: 640,
                                    items: [
                                        {
                                            xtype: 'label',
                                            padding: '5 5 5 5',
                                            margin: '10 10 10 10',
                                            labelWidth: 640,
                                            labelAlign: 'center',
                                            itemId: 'questionFieldId',
                                            name: 'questionField',
                                            height: '50%',
                                            style: {
                                                'font-weight': 'bold',
                                                'font-size': '12px',
                                                'text-align': 'center',
                                                'margin': '0 auto'
                                            },
                                            hidden: false,
                                            text: 'Вопрос'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'panel',
                                    frame: false,
                                    bodyStyle: 'background-color: #dfe8f6;',
                                    height: 140,
                                    width: 640,
                                    items: [
                                        {
                                            xtype: 'radiogroup',
                                            itemId: 'radGroupId',
                                            name: 'radGroup',
                                            width: 600,
                                            padding: '5 5 5 5',
                                            margin: '10 10 10 10',
                                            labelWidth: 180,
                                            columns: 3,
                                            vertical: false,
                                            items: [
                                                {
                                                    name: 'radioGroupItems',
                                                    boxLabel: 'Да',
                                                    itemId: 'ChoiseYes',
                                                    inputValue: 10
                                                    
                                                },
                                                {
                                                    name: 'radioGroupItems',
                                                    boxLabel: 'Нет',
                                                    itemId: 'ChoiseNo',
                                                    inputValue: 20
                                                },
                                                {
                                                    name: 'radioGroupItems',
                                                    boxLabel: 'Не применимо',
                                                    itemId: 'ChoiseNotApplicable',
                                                    inputValue: 30
                                                }
                                            ]//,
                                            //listeners: {
                                            //    change: {
                                            //        fn: function (radiogroup) {
                                            //            console.log('change');
                                            //            var window = radiogroup.up('personexamwindow');
                                            //            var nextbtn = window.down('button[action = nextQuestion]');
                                            //            nextbtn.setDisabled(false);}
                                            //    }
                                            //}
                                        },
                                        {
                                            fieldLabel: 'Примечание',
                                            xtype: 'textarea',
                                            itemId: 'infoFieldId',
                                            name: 'infoField',
                                            width: 600,
                                            padding: '5 5 5 5',
                                            margin: '10 10 10 10',
                                            labelWidth: 80,
                                            columns: 1,
                                            hidden: false,
                                            value: '',//'Если текст вопроса не умещается на экране, щелкните по нему левой кнопкой мыши, откроется окно с полным текстом',
                                            text: 'Примечание'
                                        }

                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'match'
                            },
                            width: 120,

                            padding: '0 0 0 0',
                            margin: '0 0 0 0',
                            items: [
                                {
                                    xtype: 'panel',
                                    frame: false,
                                    layout: 'fit',
                                    bodyStyle: 'background-color: #dfe8f6;',
                                    height: 390,
                                    style: {
                                        'text-align': 'center',
                                        'margin': '0 auto'
                                    },
                                   // html: "<div id='clockEl'>&nbsp;</div>",
                                    width: 120
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnExamNextQuestion',
                                    action: 'nextQuestion',
                                    height: 50,
                                    width: 120,
                                    hidden: false,
                                    text: 'Далее'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnExamClose',
                                    action: 'closeExamForm',
                                    height: 50,
                                    width: 120,
                                    hidden: true,
                                    text: 'Закрыть'
                                }
                            ]
                        }
                    ]

                });

            me.callParent(arguments);
        }
    });