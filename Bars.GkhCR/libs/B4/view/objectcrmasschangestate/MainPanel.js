Ext.define('B4.view.objectcrmasschangestate.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Массовая смена статусов объектов КР',
    alias: 'widget.massChangeStateMainPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    requires: [
        'B4.view.objectcrmasschangestate.Grid',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.ProgramCr'
    ],

    initComponent: function() {
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
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'ProgramCr',
                            itemId: 'sfProgramCr',
                            fieldLabel: 'Программа кап.ремонта',
                           

                            store: 'B4.store.dict.ProgramCr',
                            width: 400,
                            editable: false
                        },
                        {
                            xtype: 'b4combobox',
                            itemId: 'cbCurrentState',
                            url: '/State/GetListByType',
                            fields: ['Id', 'Name'],
                            fieldLabel: 'Текущий статус',
                            width: 400,
                            disabled: true,
                            editable: false
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    itemId: 'cbNextState',
                                    url: '/StateTransfer/GetStates',
                                    fields: ['Id', 'Name'],
                                    fieldLabel: 'Новый статус',
                                    padding: '0 5 0 0',
                                    width: 400,
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    disabled: true,
                                    editable: false
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnChangeState',
                                    text: 'Выполнить',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'objectCrMassChangeStateGrid',
                    border: false,
                    flex: 1
                }
            ]
        }, me);
        
        me.callParent(arguments);
    }
});