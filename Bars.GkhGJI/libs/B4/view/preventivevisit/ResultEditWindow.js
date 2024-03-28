Ext.define('B4.view.preventivevisit.ResultEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minHeight: 350,
    bodyPadding: 5,
    itemId: 'preventivevisitResultEditWindow',
    title: 'Результат визита',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.form.EnumCombo',
        'B4.enums.ProfVisitResult',
        'B4.form.SelectField',
        'B4.store.preventivevisit.ListRoForResultPV',
        'B4.view.preventivevisit.ResultViolationGrid',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    anchor: '100%',
                    fieldLabel: 'Результат визита',
                    enumName: 'B4.enums.ProfVisitResult',
                    name: 'ProfVisitResult'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    textProperty: 'Address',
                    fieldLabel: 'Объект',
                    itemId: 'sfRealityObject',
                    store: 'B4.store.preventivevisit.ListRoForResultPV',
                    editable: false,
                    allowBlank: true,
                    columns: [
                        { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },

                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'InformText',
                    fieldLabel: 'Информирование',
                    maxLength: 10000,
                    flex: 1
                },
                {
                    xtype: 'preventivevisitresultviolationgrid'
                },
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