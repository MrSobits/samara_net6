Ext.define('B4.form.FiasSelectAddressWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { 
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    itemId: 'fiasSelectAddressWindow',
    title: 'Адрес',
    closeAction: 'destroy',
    padding:2,
    
    requires: [ 'B4.form.ComboBox' ],

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'acceptform',
            'resetform',
            'cancelform'
        );
    },

    fillAddressField: function () {
        var result = '';
        var addSeparator = false;

        var cityName = this.cbCity.getValue() ? this.cbCity.getDisplayValue() : '';
        if (!Ext.isEmpty(cityName)) {
            result = cityName;
            addSeparator = true;
        }

        var streetName = this.cbStreet.getValue() ? this.cbStreet.getDisplayValue() : '';
        if (!Ext.isEmpty(streetName)) {
            result += (addSeparator ? ', ' : '') + streetName;
            addSeparator = true;
        }

        if (this.tfHouse.getValue()) {
            result += (addSeparator ? ', ' : '') + 'д. ' + this.tfHouse.getValue();
            addSeparator = true;
        }
        
        if (this.tfLetter.getValue()) {
            result += (addSeparator ? ', ' : '') + 'лит. ' + this.tfLetter.getValue();
            addSeparator = true;
        }

        if (this.tfHousing.getValue()) {
            result += (addSeparator ? ', ' : '') + 'корп.' + ' ' + this.tfHousing.getValue();
            addSeparator = true;
        }

        if (this.tfBuilding.getValue()) {
            result += (addSeparator ? ', ' : '') + 'секц.' + ' ' + this.tfBuilding.getValue();
            addSeparator = true;
        }
        if (this.tfFlat.getValue()) {
            result += (addSeparator ? ', ' : '') + 'кв.' + ' ' + this.tfFlat.getValue();
        }

        this.tfAddress.setValue(result);
    },
    
    initComponent: function () {
        var me = this;

        this.cbCity = Ext.create('B4.form.ComboBox', {
            editable: true,
            allowBlank: false,
            labelWidth: 120,
            labelAlign: 'right',
            storeAutoLoad: false,
            fieldLabel: 'Населенный пункт',
            emptyText: 'Введите наименование населенного пункта...',
            flex: 1,
            typeAhead: false,
            fields: ['GuidId', 'Code', 'Name', 'AddressName', 'PostCode', 'AddressGuid', 'MirrorGuid'],
            url: '/Fias/GetPlacesList',
            mode: 'remote',
            valueField: 'AddressGuid',
            displayField: 'AddressName',
            triggerAction: 'query',
            minChars: 2,
            autoSelect: true,
            queryDelay: 500,
            queryParam: 'filter',
            loadingText: 'Загрузка...',
            trigger1Cls: 'x-form-clear-trigger',
            selectOnFocus: false,
            onTrigger1Click:function () {
                this.clearValue();
                this.setEditable(true);
                if (me.cbStreet.getValue()) {
                    me.cbStreet.clearValue();
                    me.cbStreet.setEditable(false);
                    me.cbStreet.store.removeAll();
                }
                me.fillAddressField();
            },
            listeners: {
                
                select: {
                    fn: function (combo, records) {
                        var record = records[0];
                        if (record) {
                            if (record.data.PostCode) {
                                this.tfPostCode.setValue(record.data.PostCode);
                            }
                            if (combo.oldValue != record.data.GuidId && this.cbStreet.getValue()) {
                                this.cbStreet.clearValue();
                                this.cbStreet.store.removeAll();
                            }

                            combo.setEditable(false);
                            this.cbStreet.setEditable(true);
                        } else {
                            combo.setEditable(true);
                            this.cbStreet.setEditable(false);
                        }

                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.cbStreet = Ext.create('B4.form.ComboBox', {
            editable: false,
            allowBlank: true,
            storeAutoLoad: false,
            labelWidth: 120,
            labelAlign: 'right',
            fieldLabel: 'Улица',
            emptyText: 'Введите наименование улицы...',
            typeAhead: false,
            fields: ['GuidId', 'Code', 'Name', 'AddressName', 'PostCode', 'AddressGuid'],
            url: '/Fias/GetStreetsList',
            mode: 'remote',
            valueField: 'GuidId',
            displayField: 'AddressName',
            triggerAction: 'query',
            minChars: 2,
            flex: 1,
            autoSelect: true,
            queryDelay: 500,
            queryParam: 'filter',
            loadingText: 'Загрузка...',
            trigger1Cls: 'x-form-clear-trigger',
            selectOnFocus: false,
            onTrigger1Click: function () {
                this.clearValue();
                this.setEditable(true);
                
                me.fillAddressField();
            },
            listeners: {
                storebeforeload: {
                    fn: function (field, store, options) {

                        var rec = this.cbCity.getRecord(this.cbCity.getValue());
                        if (rec) {
                            options.params.parentguid = rec.GuidId;
                        }
                        return !Ext.isEmpty(options.params.parentguid);
                    },
                    scope: this
                },
                select: {
                    fn: function (combo, records) {
                        var record = records[0];
                        if (record) {
                            if (record.data.PostCode) {
                                this.tfPostCode.setValue(record.data.PostCode);
                            }
                            combo.setEditable(false);
                        } else {
                            combo.setEditable(true);
                        }

                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.tfHouse = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            flex: 1,
            fieldLabel: 'Дом',
            listeners: {
                change: {
                    fn: function () {
                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });
        
        this.tfLetter = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            fieldLabel: 'Литер',
            flex: 1,
            listeners: {
                change: {
                    fn: function () {
                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.tfHousing = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            fieldLabel: 'Корпус',
            flex: 1,
            listeners: {
                change: {
                    fn: function () {
                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.tfBuilding = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            flex: 1,
            fieldLabel: 'Секция',
            listeners: {
                change: {
                    fn: function () {
                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.tfFlat = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            flex: 1,
            fieldLabel: 'Квартира',
            listeners: {
                change: {
                    fn: function () {
                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.tfPostCode = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            flex: 1,
            fieldLabel: 'Почтовый индекс'
        });

        this.tfCoordinate = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            flex: 1,
            labelAlign: 'right',
            fieldLabel: 'Координаты'
        });

        this.tfAddress = Ext.create('Ext.form.TextArea', {
            labelWidth: 120,
            labelAlign: 'right',
            height: 50,
            fieldLabel: 'Адрес',
            readOnly: true
        });

        this.chbNoStreet = Ext.create('Ext.form.field.Checkbox', {
            fieldLabel: 'Улица отсутствует',
            labelWidth: 120,
            labelAlign: 'right',
            listeners: {
                change: {
                    fn: function () {
                        me.cbStreet.setDisabled(this.checked);
                        me.cbStreet.onTrigger1Click();
                    }
                }
            }
        });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                margin: '4px 10px 4px 0'
            },
            items: [
                {
                    xtype: 'component',
                    itemId: 'councillorsLabel',
                    padding: 5,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell; padding-left: 5px;">Для заполнения адреса необходимо последовательно выбрать населенный пункт, затем улицу и дом.</br>Для очистки выбранных значений используйте "крестик" напротив полей</br></span>'
                },
                this.cbCity,
                this.cbStreet,
                this.chbNoStreet,
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    items: [
                        this.tfHouse,
                        this.tfLetter,
                        this.tfHousing,
                        this.tfBuilding
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    items: [
                        this.tfFlat,
                        this.tfPostCode,
                        this.tfCoordinate
                    ]
                },
                this.tfAddress
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items:
                            [
                                {
                                    text: 'Применить',
                                    iconCls: 'icon-accept',
                                    handler: function () {
                                        this.fireEvent('acceptform', this);
                                    },
                                    scope:this
                                },
                                {
                                    text: 'Сбросить',
                                    iconCls: 'icon-cross',
                                    handler: function () {
                                        this.fireEvent('resetform', this);
                                    },
                                    scope: this
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items:
                            [
                                {
                                    text: 'Закрыть',
                                    iconCls: 'icon-decline',
                                    handler: function () {
                                        this.fireEvent('cancelform', this);
                                    },
                                    scope: this
                                    //handler: this.cancelForm,
                                    //scope: this
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