Ext.define('B4.view.regop.personal_account.privilegedcategory.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.paccountprivilegedcategoryeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 450,
    bodyPadding: 5,
    title: 'Группа льготных категорий граждан',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.PrivilegedCategory'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'PrivilegedCategory',
                    fieldLabel: 'Льготная категория',
                    store: 'B4.store.dict.PrivilegedCategory',
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            flex: 1,
                            filter: {
                                xtype: 'textfield',
                                maxLength: 300
                            }
                        },
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: {
                                xtype: 'textfield',
                                maxLength: 300
                            }
                        },
                        {
                            text: 'Процент льготы',
                            dataIndex: 'Percent',
                            flex: 1,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                minValue: 0,
                                maxValue: 100,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            text: 'Действует с',
                            dataIndex: 'DateFrom',
                            flex: 1,
                            format: 'd.m.Y',
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            text: 'Действует по',
                            dataIndex: 'DateTo',
                            flex: 1,
                            format: 'd.m.Y',
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateFrom',
                    fieldLabel: 'Действует с',
                    allowBlank: false,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'DateTo',
                    fieldLabel: 'Действует по',
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