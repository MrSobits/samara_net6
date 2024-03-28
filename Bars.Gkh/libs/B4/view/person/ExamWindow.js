    Ext.define('B4.view.person.ExamWindow',
    {
        extend: 'B4.form.Window',
        alias: 'widget.personexamwindow',
        mixins: ['B4.mixins.window.ModalMask'],
        layout: 'hbox',
        width: 772,
        minWidth: 772,
        maxWidth: 772,
        title: 'Экзамен',
        closeAction: 'destroy',
        step: 0,
        modal: true,
        closable: false,

        requires: [
            'B4.ux.button.Close',
            'B4.ux.button.Save',
            'B4.form.FileField',
            'B4.enums.TypeCancelationQualCertificate',
            'B4.form.SelectField',
            'B4.store.person.RequestToExam',
            'B4.view.person.QualificationDocumentGrid',
            'B4.view.person.TechnicalMistakeGrid'
        ],

        initComponent: function() {
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
                                    itemId: 'focusPanel',
                                    bodyStyle: 'background-color: #dfe8f6;',
                                    height: 120,
                                    layout: {
                                        type: 'hbox',
                                        align: 'fit',
                                        pack: 'center'
                                    },
                                    width: 640,
                                    items: [
                                        {
                                            xtype: 'label',
                                            focusable: true,
                                            padding: '5 5 5 5',
                                            margin: '5 5 5 5',
                                            itemId: 'questionFieldId',
                                            name: 'questionField',
                                            height:'100%',
                                            bodyStyle: 'font-size: 12px; font-weight: bold;height:100%',
                                            style: {
                                                'font-weight': 'bold',
                                                'font-size': '12px',
                                                'text-align': 'center',
                                                'height': '100%',
                                                        
                                                    },
                                        hidden: true,
                                        text: ''
                                        
                                    },
                                    {
                                        xtype: 'label',
                                        padding: '5 5 5 5',
                                        margin: '10 10 10 10',
                                        labelWidth: 640,
                                        labelAlign: 'center',
                                        itemId: 'helpFieldId',
                                        name: 'helpField',
                                        style: {
                                                        'font-weight': 'bold',
                                                        'font-size': '14px',
                                                        'text-align': 'center',
                                                        'margin': '0 auto'
                                                    },
                                        hidden: false,
                                        text: 'Что нужно знать перед экзаменом'
                                    }
                                    ]                               
                            },
                            {
                                xtype: 'panel',
                                frame: false,
                                bodyStyle: 'background-color: #dfe8f6;',
                                height: 200,
                                width: 640,
                                items:[
                                    {
                                        xtype: 'radiogroup',
                                        itemId: 'radGroupId',
                                        name: 'radGroup',
                                        width: 640,
                                        padding: '5 5 5 5',
                                        margin: '10 10 10 10',
                                        labelWidth: 180,
                                        columns: 1,
                                        hidden: true,
                                        items: [
                                            {
                                                name: 'ThisMonthInputMeteringDeviceValuesEndDate',
                                                boxLabel: '',
                                                inputValue: true,
                                                itemId: 'ThisMonth'
                                            },
                                            {
                                                name: 'ThisMonthInputMeteringDeviceValuesEndDate',
                                                boxLabel: '',
                                                itemId: 'NextMonth',
                                                inputValue: false
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
                                        xtype: 'textarea',
                                        itemId: 'infoFieldId',
                                        name: 'infoField',
                                        width: 600,
                                        padding: '5 5 5 5',
                                        margin: '10 10 10 10',
                                        labelWidth: 180,
                                        columns: 1,
                                        hidden: false,
                                        value: 'Тут размещается информация для изучения перед началом экзамена',//'Если текст вопроса не умещается на экране, щелкните по нему левой кнопкой мыши, откроется окно с полным текстом',
                                        text: 'Тут размещается информация для изучения перед началом экзамена'
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
                                bodyStyle: 'background-color: #dfe8f6;',
                                height: 120,
                                width: 120,
                                items: [
                                    {
                                        xtype: 'label',
                                        name: 'questionLabel',
                                        text: '',
                                        width: 100
                                    },
                                    {
                                        xtype: 'label',
                                        name: 'questionNumberLabel',
                                        text: ''
                                    },
                                ]
                            },
                            {
                                xtype: 'panel',
                                frame: false,
                                layout: 'fit',
                                bodyStyle: 'background-color: #dfe8f6;',
                                height: 150,
                                style: {
                                    'text-align': 'center',
                                    'margin': '0 auto'
                                },
                                html: "<div id='clockEl'>&nbsp;</div>",
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