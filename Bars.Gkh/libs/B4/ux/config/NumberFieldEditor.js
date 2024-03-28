Ext.define('B4.ux.config.NumberFieldEditor', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.numberfieldeditor',

    triggerTpl: '<td style="{triggerStyle}">' +
        '<div class="' + Ext.baseCSSPrefix + 'trigger-index-0 ' + Ext.baseCSSPrefix + 'form-trigger ' + Ext.baseCSSPrefix + 'form-spinner-up" role="button"></div>' +
        '<div class="' + Ext.baseCSSPrefix + 'trigger-index-1 ' + Ext.baseCSSPrefix + 'form-trigger ' + Ext.baseCSSPrefix + 'form-spinner-down" role="button"></div>' +       
        '</td>' +
        '<td style="{triggerStyle}"><div class="' + Ext.baseCSSPrefix + 'trigger-index-2 ' + Ext.baseCSSPrefix + 'form-trigger x-form-pencil-trigger" role="button"></div> </td>' +
        '</tr>',

    editable: false,
    

    initComponent: function() {
        var me = this;
        Ext.apply(me,
            {
                triggerWrapCls: 'x-form-pencil-trigger',
                onTriggerClick: function() {
                    var me = this,
                        value = me.getValue();

                    if (value == me.defaultValue) {
                        Ext.Msg.confirm('Предупреждение',
                            'В случае изменения данной настройки, возможность перехода на меньшее ' +
                            'количество знаков будет невозможно. Продолжить?',
                            function(result) {
                                me.setEditable(result === 'yes');
                                me.setSpinUpEnabled(result === 'yes');
                                me.setSpinDownEnabled(result === 'yes');
                            });
                    }

                    if (value >= 3 && value < 7) {
                        me.minValue = value;

                        Ext.Msg.confirm('Предупреждение',
                            'В случае изменения данной настройки, возможность перехода на меньшее ' +
                            'количество знаков будет невозможно. Продолжить?',
                            function(result) {
                                me.setEditable(result === 'yes');
                                me.setSpinUpEnabled(result === 'yes');
                                me.setSpinDownEnabled(result === 'yes');
                            });
                    }
                }

            });

        me.callParent(arguments);
        me.setSpinUpEnabled(false);
        me.setSpinDownEnabled(false);
    }
});