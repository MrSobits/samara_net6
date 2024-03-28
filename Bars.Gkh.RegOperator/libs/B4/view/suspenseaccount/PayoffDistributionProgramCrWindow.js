Ext.define('B4.view.suspenseaccount.PayoffDistributionProgramCrWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.suspenceaccpayoffdistribprogramcrwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCrAon',
        'B4.view.Control.GkhTriggerField'
    ],
    
    title: 'Распределение платежа',

    modal: true,

    width: 800,

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5',
                labelAlign: 'right'
            },
            layout: {
                type: 'vbox',
                align:'stretch'
            },
            bodyStyle: Gkh.bodyStyle,
            border: 0,
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    store: 'B4.store.dict.ProgramCrAon',
                    columns: [
                        {
                            text: 'Программа КР', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ObjectsCr',
                    fieldLabel: 'Объекты КР',
                    emptyText: 'Выберите объекты для зачисления'
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
                                   xtype: 'button',
                                   text: 'Продолжить',
                                   action: 'NextStep'
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