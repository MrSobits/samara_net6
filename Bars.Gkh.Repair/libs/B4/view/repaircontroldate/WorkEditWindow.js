Ext.define('B4.view.repaircontroldate.WorkEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 650,
    height: 150,
    minWidth: 500,
    minHeight: 150,
    bodyPadding: 5,
    itemId: 'repairControlDateWorkEditWindow',
    title: 'Виды работ',
    closeAction: 'hide',
    requires: [
        'B4.form.SelectField',
        'B4.view.dict.workkindcurrentrepair.Grid',
        'B4.store.dict.WorkKindCurrentRepair',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'ProgramName',
                    itemId: 'sfProgramCr',
                    fieldLabel: 'Программа текущего ремонта',
                    readOnly: true
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Work',
                    itemId: 'sflWork',
                    fieldLabel: 'Вид работы',
                   

                    store: 'B4.store.dict.WorkKindCurrentRepair',
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Контрольный срок',
                    itemId: 'dfDate',
                    width: 280,
                    format: 'd.m.Y'
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