Ext.define('B4.view.activitytsj.ProtocolEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    maxWidth: 800,
    width: 700,
    minWidth: 600,
    height: 380,
    minHeight: 380,
    bodyPadding: 5,
    itemId: 'activityTsjProtocolEditWindow',
    title: 'Протокол',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.KindProtocolTsj',
        'B4.view.activitytsj.ProtocolRealObjGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Общие сведения',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Общие сведения',
                                    defaults: {
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'KindProtocolTsj',
                                            fieldLabel: 'Тип протокола',
                                           

                                            store: 'B4.store.dict.KindProtocolTsj',
                                            editable: false,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'container',
                                            defaults: {
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNum',
                                                    padding: '0 0 5 0',
                                                    fieldLabel: 'Номер',
                                                    width: 300,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DocumentDate',
                                                    format: 'd.m.Y',
                                                    width: 200,
                                                    labelWidth: 100,
                                                    fieldLabel: 'Дата'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'File',
                                            fieldLabel: 'Файл',
                                            anchor: '100%'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Принятие решения',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 210,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'VotesDate',
                                                    format: 'd.m.Y',
                                                    width: 290,
                                                    fieldLabel: 'Дата голосования'
                                                },
                                                {
                                                    xtype: 'gkhintfield',
                                                    name: 'CountVotes',
                                                    fieldLabel: 'Количество голосов'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 210,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'gkhintfield',
                                                    name: 'GeneralCountVotes',
                                                    fieldLabel: 'Общее количество голосов (кв.м.)'
                                                },
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'PercentageParticipant',
                                                    fieldLabel: 'Доля принявших участие (%)'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'FileBulletin',
                                            fieldLabel: 'Бюллютень',
                                            labelAlign: 'right',
                                            anchor: '100%',
                                            labelWidth: 210
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'activityTsjProtocolRealObjGrid'
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
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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