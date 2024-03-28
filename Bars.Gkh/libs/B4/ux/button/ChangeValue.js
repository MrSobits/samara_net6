Ext.define('B4.ux.button.ChangeValue', {
    extend: 'Ext.button.Button',

    alias: 'widget.changevalbtn',

    requires: [
        'Ext.window.Window',
        'Ext.form.Panel',

        'B4.Url',
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',

        'B4.view.entityloglight.EntityLogLightGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    className: null,

    propertyName: null,

    windowConfig: null,

    parameter: null,

    parameters: null,

    withHistory: false,

    valueFieldXtype: 'numberfield',

    valueFieldSelector: null,

    entityId: null,

    decimalPrecision: 2,

    decimalSeparator: ',',

    valueFieldConfig: null,

    valueGridColumnConfig: null,

    tooltip: 'Сменить значение',

    icon: B4.Url.content('content/img/icons/pencil.png'),

    width: 23,
    minWidth: 23,
    maxWidth: 23,

    constructor: function (config) {
        var me = this;
        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'valuesaved',
            'beforevaluesave',
            'beforeshowwindow'
        );
    },

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            listeners: {
                click: {
                    fn: me.showWindow,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    showWindow: function () {
        var me = this, win = me.editWindow, winCfg, renderTo = Ext.getBody(), valFieldCfg;

        if (me.fireEvent('beforeshowwindow', me) === true) {
            if (Ext.isString(me.windowContainerSelector)) {
                renderTo = Ext.ComponentQuery.query(me.windowContainerSelector, B4.getBody().getActiveTab());
                if (Ext.isArray(renderTo)) {
                    renderTo = renderTo[0];
                }
                if (!renderTo) {
                    throw "Не удалось найти контейнер для формы списка по селектору " + me.windowContainerSelector;
                }

                renderTo = Ext.isFunction(renderTo.getEl) ? renderTo.getEl() : (renderTo.dom ? renderTo : null);
            }
            if (!win) {
                valFieldCfg = me.valueFieldConfig || {
                    xtype: me.valueFieldXtype,
                    fieldLabel: 'Новое значение',
                    hideTrigger: false,
                    minValue: 0,
                    decimalPrecision: me.decimalPrecision,
                    decimalSeparator: me.decimalSeparator
                };

                valFieldCfg.name = 'value';

                if (me.withHistory) {
                    winCfg = {
                        modal: true,
                        width: 632,
                        height: 235,
                        closeAction: 'hide',
                        title: 'Смена значения',
                        renderTo: renderTo,
                        layout: 'fit',
                        items: {
                            xtype: 'tabpanel',
                            border: 0,
                            items: [
                                {
                                    title: 'Общие сведения',
                                    bodyStyle: Gkh.bodyStyle,
                                    bodyPadding: '27 15 27 25',
                                    xtype: 'form',
                                    border: 0,
                                    defaults: {
                                        margins: '15 15 15 15',
                                        labelWidth: 200,
                                        width: 575
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Дата вступления значения в силу',
                                            xtype: 'datefield',
                                            name: 'factDate',
                                            allowBlank: false
                                        },
                                        valFieldCfg,
                                        {
                                            xtype: 'b4filefield',
                                            name: 'BaseDoc',
                                            fieldLabel: 'Документ-основание'
                                        }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'toolbar',
                                            dock: 'top',
                                            items: [
                                                {
                                                    xtype: 'b4savebutton',
                                                    listeners: {
                                                        click: {
                                                            fn: me.changeValue,
                                                            scope: me
                                                        }
                                                    }
                                                },
                                                { xtype: 'tbfill' },
                                                {
                                                    xtype: 'b4closebutton',
                                                    listeners: {
                                                        click: {
                                                            fn: me.closeWindow,
                                                            scope: me
                                                        }
                                                    }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    title: 'История изменений',
                                    border: 0,
                                    xtype: 'entityloglightgrid',
                                    valueColumnConfig: me.valueGridColumnConfig,
                                    listeners: {
                                        render: {
                                            fn: me.loadLogGrid,
                                            scope: me
                                        }
                                    }
                                }
                            ]
                        }
                    };
                } else {
                    winCfg = {
                        modal: true,
                        width: 400,
                        height: 180,
                        closeAction: 'hide',
                        title: 'Смена значения',
                        renderTo: renderTo,
                        items: {
                            xtype: 'form',
                            border: 0,
                            defaults: {
                                margin: '5 5 5 5'
                            },
                            items: [
                                {
                                    fieldLabel: 'Дата вступления значения в силу',
                                    xtype: 'datefield',
                                    name: 'factDate',
                                    allowBlank: false
                                },
                                valFieldCfg,
                                {
                                    xtype: 'b4filefield',
                                    name: 'BaseDoc',
                                    fieldLabel: 'Документ-основание'
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'b4savebutton',
                                            listeners: {
                                                click: {
                                                    fn: me.changeValue,
                                                    scope: me
                                                }
                                            }
                                        },
                                        { xtype: 'tbfill' },
                                        {
                                            xtype: 'b4closebutton',
                                            listeners: {
                                                click: {
                                                    fn: me.closeWindow,
                                                    scope: me
                                                }
                                            }
                                        }
                                    ]
                                }
                            ]
                        }
                    };
                }

                if (me.windowConfig) {
                    Ext.apply(winCfg, me.windowConfig);
                }
                win = me.editWindow = Ext.widget('window', winCfg);
            }
            win.show();
            if (me.withHistory) {
                var logStore = win.down('entityloglightgrid').getStore();

                logStore.on('beforeload', function (store, operation) {
                    operation.params.entityId = me.entityId;
                    operation.params.className = me.className;
                    operation.params.parameter = me.parameter;
                    operation.params.parameters = me.parameters;
                });

                logStore.load();
            }
        }
    },

    loadLogGrid: function(grid) {
        var me = this;
    },

    changeValue: function (btn) {
        var me = this,
            frm = btn.up('form'),
            params = frm.getValues(),
            fields = frm.getForm().getFields();


        if (!frm.getForm().isValid()) {
            var invalidFields = '';

            //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
            Ext.each(fields.items, function (field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            //выводим сообщение
            Ext.Msg.alert('Ошибка заполнения полей!', 'Не заполнены обязательные поля: ' + invalidFields);
        }

        if (me.fireEvent('beforevaluesave', me, params) === false) {
            return;
        }

        me.mask('Сохранение', frm);

        Ext.applyIf(params, {
            className: me.className,
            propertyName: me.propertyName,
            entityId: me.entityId
        });
        frm.submit({
            url: B4.Url.action('ChangeParameter', 'Parameters', { 'b4_pseudo_xhr': true }),
            params: params,
            success: function () {
                me.editWindow.close();
                Ext.each(fields.items, function (field) {
                    field.setValue(null);
                });
                me.onValueSaved.apply(me, [params.value]);
                me.fireEvent('valuesaved', me, params.value);
                B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                me.unmask();
            },
            failure: function (resp, res) {
                var msg;

                if (res && res.result && res.result.message.length == 0) {
                    msg = 'Произошла ошибка при изменении значения параметра.';
                } else {
                    msg = res.result.message;
                }

                B4.QuickMsg.msg('Изменение параметра', msg, 'error');
                me.unmask();
            }
        });
    },

    closeWindow: function () {
        this.editWindow.close();
    },

    setEntityId: function (id) {
        this.entityId = id;
    },

    onValueSaved: function (newVal) {

    }
});