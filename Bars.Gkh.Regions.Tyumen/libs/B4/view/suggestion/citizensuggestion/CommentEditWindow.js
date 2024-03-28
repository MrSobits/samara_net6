Ext.define('B4.view.suggestion.citizensuggestion.CommentEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.citsugcommentwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 900,
    height: 850,
    minHeight: 500,
    maxHeight: 900,
    bodyPadding: 5,
    autoScroll: true,
    title: 'Вопрос',

    requires: [
        'B4.form.SelectField',
        'B4.form.Combobox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.suggestion.rubric.Grid',
        'B4.view.realityobj.Grid',
        'B4.store.suggestion.Rubric',
        'B4.store.RealityObject',
        'B4.store.dict.ProblemPlace',
        'B4.view.dict.problemplace.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Исполнитель',
                    defaults: {
                        //allowBlank: false,
                        labelAlign: 'right',
                        labelWidth: 170,
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Тип исполнителя',
                            store: B4.enums.ExecutorType.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'ExecutorType',
                            value: -1
                        },
                        {
                            xtype: 'textfield',
                            name: 'Executor',
                            fieldLabel: 'Исполнитель',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    name: 'firstcomment',
                    title: 'Описание проблемы',
                    defaults: {
                        allowBlank: false,
                        labelAlign: 'right',
                        labelWidth: 100,
                        anchor: '100%'
                    },
                    items: [
                           {
                               xtype: 'b4selectfield',
                               name: 'ProblemPlace',
                               fieldLabel: 'Место проблемы',
                               store: 'B4.store.dict.ProblemPlace',
                               columns: [
                                   { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                               ],
                               editable: false,
                               readOnly: true
                               //allowBlank: false
                           },
                           {
                               xtype: 'textarea',
                               name: 'Description',
                               fieldLabel: 'Описание проблемы',
                               maxLength: 2000,
                               readOnly: true,
                               height: 100
                           },
                           {
                               xtype: 'citsugfilegrid',
                               height: 175,
                               store: 'suggestion.CommentQuestionFiles',
                               type: 'CommentQuestion'
                           }
                    ]
                },
               {
                   xtype: 'fieldset',
                   title: 'Дополнительный вопрос',
                   name: 'comment',
                   defaults: {
                       allowBlank: false,
                       labelAlign: 'right',
                       labelWidth: 100,
                       anchor: '100%'
                   },
                   items: [
                          {
                              xtype: 'datefield',
                              format: 'd.m.Y',
                              name: 'CreationDate',
                              fieldLabel: 'Дата'
                          },
                          {
                              xtype: 'textarea',
                              name: 'Question',
                              fieldLabel: 'Вопрос',
                              maxLength: 2000
                          },
                          {
                              xtype: 'citsugfilegrid',
                              height: 175,
                              store: 'suggestion.CommentQuestionFiles',
                              type: 'CommentQuestion'
                          }
                   ]
               },
               {
                   xtype: 'fieldset',
                   title: 'Ответ',
                   name: 'Answer',
                   defaults: {
                       labelAlign: 'right',
                       labelWidth: 100,
                       anchor: '100%'
                   },
                   items: [
                             {
                                xtype: 'datefield',
                                format: 'd.m.Y',
                                name: 'AnswerDate',
                                fieldLabel: 'Дата'
                             },
                             {
                                 xtype: 'textarea',
                                 name: 'Answer',
                                 fieldLabel: 'Ответ',
                                 maxLength: 2000
                             },
                             {
                                 xtype: 'b4filefield',
                                 name: 'CommentAnswerFile',
                                 fieldLabel: 'Файл'
                             },
                             {
                                 xtype: 'citsugfilegrid',
                                 height: 175,
                                 store: 'suggestion.CommentAnswerFiles',
                                 type: 'CommentAnswer'
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