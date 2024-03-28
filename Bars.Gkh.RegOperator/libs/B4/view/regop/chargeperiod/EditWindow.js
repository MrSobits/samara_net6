Ext.define('B4.view.regop.chargeperiod.EditWindow', {
        extend: 'B4.form.Window',

        alias: 'widget.chargeperiodwindow',

        requires: [
            'B4.ux.button.Close',
            'B4.ux.button.Save'
        ],

        modal: true,
        layout: 'form',
        width: 500,
        bodyPadding: 5,
        title: 'Редактирование',

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 100
                },
                items: [
                    {
                        xtype: 'textfield',
                        name: 'Name',
                        fieldLabel: 'Наименование',
                        anchor: '100%',
                        allowBlank: false
                    },
                    {
                        xtype: 'datefield',
                        fieldLabel: 'Дата открытия',
                        name: 'StartDate',
                        allowBlank: false
                    },
                    {
                        xtype: 'datefield',
                        fieldLabel: 'Дата закрытия',
                        readOnly: true,
                        name: 'EndDate'
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