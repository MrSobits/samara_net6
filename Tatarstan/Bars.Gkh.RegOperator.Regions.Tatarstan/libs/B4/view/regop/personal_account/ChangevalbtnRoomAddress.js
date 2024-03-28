Ext.define('B4.view.regop.personal_account.ChangevalbtnRoomAddress', {
    extend: 'Ext.button.Button',

    alias: 'widget.changevalbtnRoomAddress',

    requires: [
        'Ext.window.Window',
        'Ext.form.Panel',
        'B4.Url',
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.regop.personal_account.ApartmentNumber',
        'B4.enums.realty.RoomType'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    valueFieldSelector: null,

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
            'show',
            'valuesaved'
        );
    },

    initComponent: function () {
        var me = this;

        me.callParent(arguments);
    },

    showWindow: function () {
        var me = this,
            win = me.editWindow,
            winCfg,
            renderTo = Ext.getBody();

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

            winCfg = {
                modal: true,
                width: 400,
                closeAction: 'hide',
                title: 'Смена значения',
                renderTo: renderTo,
                layout: {
                    type:'vbox',
                    align:'stretch'
                },
                items: {
                    xtype: 'form',
                    border: 0,
                    defaults: {
                        margin: 5
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '5 0 5 0',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'HomeAddress',
                                    fieldLabel: 'Адрес дома',
                                    readOnly: true,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0 5 0',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ApartmentNum',
                                    fieldLabel: '№ квартиры / помещения',
                                    textProperty: 'RoomNum',
                                    store: 'B4.store.regop.personal_account.ApartmentNumber',
                                    flex: 1,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'RoomNum',
                                            flex: 1,
                                            text: '№ квартиры/помещения',
                                            filter: {
                                                xtype: 'numberfield',
                                                hideTrigger: true,
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            dataIndex: 'Entrance',
                                            flex: 1,
                                            text: 'Номер подъезда',
                                            filter: {
                                                xtype: 'numberfield',
                                                hideTrigger: true,
                                                operand: CondExpr.operands.eq
                                            },
                                            renderer: function(val) {
                                                return val ? val : '-';
                                            }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Area',
                                            flex: 1,
                                            text: 'Общая площадь',
                                            filter: {
                                                xtype: 'numberfield',
                                                hideTrigger: true,
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Type',
                                            flex: 1,
                                            text: 'Тип помещения',
                                            renderer: function (val) {
                                                return B4.enums.realty.RoomType.displayRenderer(val);
                                            },
                                            filter: {
                                                xtype: 'b4combobox',
                                                items: B4.enums.realty.RoomType.getItemsWithEmpty([null, '-']),
                                                editable: false,
                                                operand: CondExpr.operands.eq,
                                                valueField: 'Value',
                                                displayField: 'Display'
                                            }
                                        }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: this.store,
                                            dock: 'bottom'
                                        }
                                    ]
                                }
                            ]
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
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

            win = me.editWindow = Ext.widget('window', winCfg);
        }
        me.fireEvent('show', win.down('b4selectfield'));

        win.show();
    },

    initValue: function(accId, fiasAddress) {
        var window = this.editWindow;

        this.accId = accId;

        window.down('textfield[name=HomeAddress]').setValue(fiasAddress);
    },

    changeValue: function () {
        var me = this,
            window = me.editWindow,
            frm = window.down('form'),
            params = frm.getValues(),
            fields = frm.getForm().getFields(),
            apartmentNumNewValue = frm.down('[name=ApartmentNum]').rawValue;

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

        frm.body.mask('Сохранение');

        Ext.applyIf(params, {
            id: me.accId
        });
        frm.submit({
            url: B4.Url.action('SaveChange', 'RoomAddress', { 'b4_pseudo_xhr': true }),
            params: params,
            success: function () {
                me.closeWindow();
                me.fireEvent('valuesaved', me, ', кв. ' + apartmentNumNewValue);
                B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                frm.body.unmask();
            },
            failure: function (resp, res) {
                var msg;

                if (res || res.result || res.result.message.length == 0) {
                    msg = 'Произошла ошибка при изменении значения параметра.';
                } else {
                    msg = res.result.message;
                }

                B4.QuickMsg.msg('Изменение параметра', msg, 'error');
                frm.body.unmask();
            }
        });
    },

    closeWindow: function () {
        this.editWindow.close();
    }
});