Ext.define('B4.view.realityobj.decision_protocol.DecisionWindow', {
    extend: 'Ext.window.Window',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    modal: true,
    minWidth: 500,
    title: 'Решение',
    layout: 'fit',
    defaults: {
        bodyPadding: 10,
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        bodyStyle: Gkh.bodyStyle
    },

    initComponent: function() {
        var me = this,
            child = Ext.create(me.childCls, {
                protocolId: me.protocolId,
                decisionTypeCode: me.decisionTypeCode,
                saveable: this.saveable
            });
        Ext.apply(me, {
            
            items: child,
            listeners: {
                show: function() {
                    if (Ext.isFunction(child.afterShow)) {
                        child.afterShow(); // Гребаный костыль для тех форм, которые нормально не отображаются
                    }
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    hidden: (me.saveable === false),
                    items: [
                        {
                            xtype: 'b4savebutton',
                            listeners: {
                                click: {
                                    fn: me.onSaveBtnClick,
                                    scope: me
                                }
                            }
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    },

    onSaveBtnClick: function() {
        var me = this,
            form = me.down('form');

        me.mask('Сохранение...');

        if (form.getForm().isValid()) {
            form.getForm().submit({
                url: B4.Url.action('ApplyDecision', 'Decision'),
                method: 'POST',
                params: {
                    protocolId: me.protocolId,
                    Protocol: me.protocolId,
                    decisionTypeCode: me.decisionTypeCode,
                    DecisionCode: me.decisionTypeCode,
                    typeCode: me.decisionTypeCode
                },
                success: function() {
                    me.unmask();
                    me.close();
                },
                failure: function() {
                    me.unmask();
                }
            });
        } else {
            Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля!');
            me.unmask();
        }
    },

    loadValues: function(raw) {
        this.down('form').getForm().setValues(raw);
    }
});