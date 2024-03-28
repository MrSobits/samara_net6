Ext.define('B4.view.dict.statsubsubjectgji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    height: 800,
    bodyPadding: 5,
    itemId: 'statSubsubjectGjiEditWindow',
    title: 'Подтематика',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.dict.statsubsubjectgji.SubjectGrid',
        'B4.view.dict.statsubsubjectgji.FeatureGrid',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'SSTUCodeSub',
                    fieldLabel: 'Код ССТУ',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'SSTUNameSub',
                    fieldLabel: 'Наименование ССТУ',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            itemId: 'cbISSOPR',
                            name: 'ISSOPR',
                            fieldLabel: 'СОПР',
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbTrackAppealCits',
                            margin: '0 0 0 40',
                            name: 'TrackAppealCits',
                            fieldLabel: 'Отслеживать обращение',
                        },
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'AppealAnswerText',
                    fieldLabel: 'Текст ответа',
                    anchor: '100%',
                    allowBlank: true,
                    height:300,
                    maxLength: 20000
                },
                {
                    xtype: 'textarea',
                    name: 'AppealAnswerText2',
                    fieldLabel: 'Подписант',
                    anchor: '100%',
                    allowBlank: true,
                    height: 50,
                    maxLength: 1500
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    layout: { type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            //грид тематик
                            xtype: 'statSubsubjectSubjectGrid',
                            flex: 1
                        },
                        {
                            //грид характеристик 
                            xtype: 'statSubsubjectFeatureGrid',
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
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