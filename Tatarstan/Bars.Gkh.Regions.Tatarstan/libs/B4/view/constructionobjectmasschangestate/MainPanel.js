Ext.define('B4.view.constructionobjectmasschangestate.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Массовая смена статусов объектов строительства',
    alias: 'widget.constructionobjectmasschangestatemainpanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.constructionobjectmasschangestate.Grid',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.ResettlementProgram'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    itemId: 'changeStatePanel',
                    closable: false,
                    layout: 'vbox',
                    frame: true,
                    border: false,
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'ResettlementProgram',
                            fieldLabel: 'Программа переселения',
                            store: 'B4.store.dict.ResettlementProgram',
                            width: 500,
                            editable: false
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'CurrentState',
                            url: '/State/GetListByType',
                            fields: ['Id', 'Name'],
                            fieldLabel: 'Текущий статус',
                            width: 500,
                            disabled: true,
                            editable: false
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'NextState',
                                    url: '/StateTransfer/GetStates',
                                    fields: ['Id', 'Name'],
                                    fieldLabel: 'Новый статус',
                                    padding: '0 5 0 0',
                                    width: 500,
                                    labelWidth: 200,
                                    labelAlign: 'right',
                                    disabled: true,
                                    editable: false
                                },
                                {
                                    xtype: 'button',
                                    action: 'Execute',
                                    text: 'Выполнить',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'constructionobjectmasschangestategrid',
                    border: false,
                    flex: 1
                }
            ]
        }, me);

        me.callParent(arguments);
    }
});