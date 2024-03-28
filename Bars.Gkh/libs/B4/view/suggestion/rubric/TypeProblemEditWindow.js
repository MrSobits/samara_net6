Ext.define('B4.view.suggestion.rubric.TypeProblemEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'rubricTypeProblemEditWindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    height: 600,
    bodyPadding: 5,
    
    title: 'Форма редактирования подтематики',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ExecutorType',
        'B4.view.suggestion.TransitionGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 160,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                    {
                        xtype: 'hidden',
                        name: 'Id'
                    },                   
                    {
                        xtype: 'textfield',
                        name: 'Name',
                        fieldLabel: 'Наименование',
                        allowBlank: false,
                        maxLength: 300
                    },
                    {
                        xtype: 'textarea',
                        name: 'RequestTemplate',
                        fieldLabel: 'Шаблон обращения',
                        height:200,
                        allowBlank: false,
                        maxLength: 10000
                    },
                    {
                        xtype: 'textarea',
                        name: 'ResponceTemplate',
                        fieldLabel: 'Шаблон ответа',
                        height: 200,
                        allowBlank: true,
                        maxLength: 10000
                    }]
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