Ext.define('B4.view.objectcr.ProgressExecutionWorkEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.progressexecutionworkeditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    height: 500,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    title: 'Ход выполнения работ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.store.dict.StageWorkCr',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.objectcr.ArchiveMultiplyContragentSmrGrid',
        'B4.view.objectcr.BuildControlTypeWorkSmrGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'hbox' },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'gkhdecimalfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                editable: false
                            },
                            items: [
                                {
                                    name: 'VolumeOfCompletion',
                                    fieldLabel: 'Объем выполнения'
                                },
                                {
                                    name: 'CostSum',
                                    fieldLabel: 'Сумма расходов'
                                },
                                {
                                    name: 'PercentOfCompletion',
                                    fieldLabel: 'Процент выполнения',
                                    maxValue: 100
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 105,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'StageWorkCr',
                                    itemId: 'sflStageWorkCr',
                                    fieldLabel: 'Этап работы',
                                    editable: false,
                                    store: 'B4.store.dict.StageWorkCr'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ManufacturerName',
                                    fieldLabel: 'Исполнитель',
                                    maxLength: 2000,
                                    readOnly: true,
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateEndWork',
                                    itemId: 'dfDateFrom',
                                    fieldLabel: 'Срок выполнения',
                                    format: 'd.m.Y'
                                }
                            ]
                        }                        
                    ]
                },
                {
                    xtype: 'tabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                        { xtype: 'archivemultiplycontragentsmrgrid' },
                        { xtype: 'buildcontroltypeworksmrgrid' }
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});