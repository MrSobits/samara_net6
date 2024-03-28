Ext.define('B4.view.activitytsj.StatuteEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    maxWidth: 800,
    width: 700,
    minwidth: 600,
    minHeight: 460,
    height: 500,
    bodyPadding: 5,
    itemId: 'activityTsjStatuteEditWindow',
    title: 'Устав',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.TypeConclusion',
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.activitytsj.ArticleGrid'
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
                            title: 'Общие сведения',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Общие сведения',
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocNumber',
                                            fieldLabel: 'Номер',
                                            width: 290,
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'StatuteApprovalDate',
                                            itemId: 'dfStatuteApprovalDate',
                                            format: 'd.m.Y',
                                            width: 290,
                                            fieldLabel: 'Дата утверждения устава'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'StatuteProvisionDate',
                                            itemId: 'dfStatuteProvisionDate',
                                            format: 'd.m.Y',
                                            width: 290,
                                            fieldLabel: 'Дата предоставления устава'
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'StatuteFile',
                                            itemId: 'ffStatuteFile',
                                            fieldLabel: 'Файл устава'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    anchor: '100% -120',
                                    title: 'Заключение',
                                    defaults: {
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox', editable: false,
                                            name: 'TypeConclusion',
                                            itemId: 'cbxTypeConclusion',
                                            fieldLabel: 'Тип заключения',
                                            displayField: 'Display',
                                            flex: 1,
                                            store: B4.enums.TypeConclusion.getStore(),
                                            valueField: 'Value'
                                        },
                                        {
                                            xtype: 'container',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 120,
                                                flex: 1
                                            },
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ConclusionNum',
                                                    itemId: 'tfConclusionNum',
                                                    fieldLabel: 'Номер',
                                                    width: 300,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ConclusionDate',
                                                    itemId: 'dfConclusionDate',
                                                    format: 'd.m.Y',
                                                    width: 200,
                                                    labelWidth: 120,
                                                    fieldLabel: 'Дата'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'ConclusionFile',
                                            itemId: 'ffConclusionFile',
                                            fieldLabel: 'Файл заключения'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'ConclusionDescription',
                                            itemId: 'taConclusionDescription',
                                            fieldLabel: 'Описание',
                                            maxLength: 500,
                                            anchor: '100% -80'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'activityTsjArticleGrid'
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
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'b4closebutton',
                                    text: ' Закрыть'
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