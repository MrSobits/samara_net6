Ext.define('B4.view.controldate.WorkEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 650,
    height: 600,
    minWidth: 500,
    minHeight: 500,
    bodyPadding: 5,
    itemId: 'controlDateWorkEditWindow',
    title: 'Виды работ',
    closeAction: 'hide',
    requires: [
        'B4.form.SelectField',
        'B4.view.controldate.StageWorkGrid',
        'B4.view.controldate.MunicipalityLimitDateGrid',
        'B4.view.dict.work.Grid',
        'B4.store.dict.Work',
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
                    fieldLabel: 'Программа кап.ремонта',
                    readOnly: true
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Work',
                    itemId: 'sflWork',
                    fieldLabel: 'Вид работы',

                    store: 'B4.store.dict.Work',
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Контрольный срок',
                    itemId: 'dfDate',
                    width: 280,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'tabpanel',
                    name: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'controldatestageworkgrid'
                        },
                        {
                            xtype: 'controldatemunicipalitylimitdategrid'
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