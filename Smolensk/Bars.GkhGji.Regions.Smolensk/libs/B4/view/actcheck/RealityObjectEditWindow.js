Ext.define('B4.view.actcheck.RealityObjectEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 600,
    bodyPadding: 5,
    itemId: 'actCheckRealityObjectEditWindow',
    title: 'Форма редактирования результатов проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.actcheck.RealityObjectByInspection',
        'B4.view.actcheck.ViolationGrid',
        'B4.view.actcheck.ViolationGroupGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'checkbox',
                    boxLabel: 'Нарушения технического состояния объекта',
                    name: 'IsRealityObject',
                    labelAlign: 'right'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    anchor: '100%',
                    labelAlign: 'right',
                    fieldLabel: 'Адрес',
                    editable: false,
                    textProperty: 'Address',
                    store: 'actcheck.RealityObjectByInspection',
                    columns: [
                        {
                            text: 'Муниципальное образование',
                            dataIndex: 'Municipality',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'actCheckViolationTabPanel',
                    border: false,
                    flex: 1,
                    items: [
                        {
                            xtype: 'actCheckViolationGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actCheckViolationGroupGrid',
                            bodyStyle: 'backrgound-color:transparent;'
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
                                    xtype: 'b4savebutton',
                                    itemId: 'actRealObjEditWindowSaveButton'
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