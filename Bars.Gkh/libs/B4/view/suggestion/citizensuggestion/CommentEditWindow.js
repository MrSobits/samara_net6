Ext.define('B4.view.suggestion.citizensuggestion.CommentEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.citsugcommentwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 750,
    height: 750,
    minHeight: 700,
    maxHeight: 768,
    bodyPadding: 5,
    title: 'Дополнительный вопрос',

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
                   title: 'Дополнительный вопрос',
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
                              maxLength: 2000,
                              allowBlank: false
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