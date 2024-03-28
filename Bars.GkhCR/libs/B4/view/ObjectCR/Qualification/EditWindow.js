Ext.define('B4.view.objectcr.qualification.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.qualificationeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 600,
    minWidth: 700,
    maxWidth: 800,
    height: 600,
    minHeight: 300,
    bodyPadding: 5,
    title: 'Участник отбора',
    closeAction: 'hide',
    trackResetOnLoad: true,
    closable: false,
    
    requires: [
        'B4.form.SelectField',
        'B4.store.Builder',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.objectcr.qualification.VoiceMemberGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'form'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'Builder',
                            fieldLabel: 'Подрядчик',
                           

                            store: 'B4.store.Builder',
                            allowBlank: false,
                            textProperty: 'ContragentName',
                            columns: [
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                { text: 'Подрядчик', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'numberfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false
                        }
                    ]
                },
                {
                    xtype: 'qualvoicemembergrid',
                    anchor: '100% -55'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});