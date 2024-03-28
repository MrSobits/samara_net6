Ext.define('B4.view.constructionobject.smr.ProgressEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.constructionobjsmrprogresseditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,
    title: 'Ход выполнения работ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
        [
            'B4.view.Control.GkhDecimalField',
            'B4.ux.button.Close',
            'B4.ux.button.Save'
        ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'gkhdecimalfield',
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    name: 'Volume',
                    fieldLabel: 'Плановый объем',
                    disabled: true
                },
                {
                    name: 'Sum',
                    fieldLabel: 'Плановая сумма',
                    disabled: true
                },
                {
                    name: 'VolumeOfCompletion',
                    fieldLabel: 'Объем выполнения',
                    minValue: 0
                },
                {
                    name: 'CostSum',
                    fieldLabel: 'Сумма расходов',
                    minValue: 0
                },
                {
                    name: 'PercentOfCompletion',
                    fieldLabel: 'Процент выполнения',
                    minValue: 0,
                    maxValue: 100
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