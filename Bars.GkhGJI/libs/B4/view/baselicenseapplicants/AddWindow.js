Ext.define('B4.view.baselicenseapplicants.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.baselicenseapplicantsaddwin',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.store.Contragent',
        'B4.form.ComboBox'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minHeight: 100,
    maxHeight: 230,
    bodyPadding: 5,
    title: 'Проверка соискателей лицензии',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                anchor: '100%',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ManOrgLicenseRequest',
                    textProperty: 'RegisterNum',
                    fieldLabel: 'Обращение',
                    store: 'B4.store.manorglicense.Request',
                    columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'RegisterNum',
                                    text: 'Номер заявки',
                                    filter: {
                                        xtype: 'numberfield',
                                        hideTrigger: true,
                                        minValue: 0,
                                        operand: CondExpr.operands.eq
                                    },
                                    width: 100
                                },
                                {
                                    xtype: 'datecolumn',
                                    dataIndex: 'DateRequest',
                                    text: 'Дата заявки',
                                    format: 'd.m.Y',
                                    filter: {
                                        xtype: 'datefield',
                                        ormat: 'd.m.Y'
                                    },
                                    width: 100
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ContragentMunicipality',
                                    width: 160,
                                    text: 'Муниципальный район',
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Contragent',
                                    flex: 1,
                                    text: 'Управляющая организация',
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                }
                    ],
                    editable: false
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