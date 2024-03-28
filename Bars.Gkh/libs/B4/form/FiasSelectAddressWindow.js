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
    padding: 2,

    requires: ['B4.form.ComboBox', 'B4.enums.UseFiasHouseIdentification'],

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
        var me = this,
            result = '',
            addSeparator = false,
            regionName = '',
            cityName = '',
            streetName = '',
            houseNum = '';

        regionName = me.cbRegion.getValue() ? me.cbRegion.getDisplayValue() : '';
        if (!Ext.isEmpty(regionName)) {
            result = regionName;
            addSeparator = true;
        }

        cityName = me.cbCity.getValue() ? me.cbCity.getDisplayValue() : '';
        if (!Ext.isEmpty(cityName)) {
            result = cityName;
            addSeparator = true;
        }

        streetName = me.cbStreet.getValue() ? me.cbStreet.getDisplayValue() : '';
        if (!Ext.isEmpty(streetName)) {
            result += (addSeparator ? ', ' : '') + streetName;
            addSeparator = true;
        }

        if (me.chbNoHouse.getValue() || !me.cbHouse.isVisible()) {
            if (me.tfHouse.getValue()) {
                result += (addSeparator ? ', ' : '') + 'д. ' + me.tfHouse.getValue().trim();
                addSeparator = true;
            }

            if (me.tfLetter.getValue()) {
                result += (addSeparator ? ', ' : '') + 'лит. ' + me.tfLetter.getValue();
                addSeparator = true;
            }

            if (me.tfHousing.getValue()) {
                result += (addSeparator ? ', ' : '') + 'корп.' + ' ' + me.tfHousing.getValue();
                addSeparator = true;
            }

            if (me.tfBuilding.getValue()) {
                result += (addSeparator ? ', ' : '') + 'секц.' + ' ' + me.tfBuilding.getValue();
                addSeparator = true;
            }

            if (me.tfFlat.getValue()) {
                result += (addSeparator ? ', ' : '') + 'пом.' + ' ' + me.tfFlat.getValue();
                addSeparator = true;
            }
        } else {
            houseNum = me.cbHouse.getValue() ? me.cbHouse.getDisplayValue() : '';
            if (!Ext.isEmpty(houseNum)) {
                result += (addSeparator ? ', ' : '') + houseNum;
                addSeparator = true;
            }
        }

        me.tfAddress.setValue(result);
    },

    initComponent: function () {
        var me = this;

        var useFiasHouseIdentification = Gkh.config.General.UseFiasHouseIdentification === B4.enums.UseFiasHouseIdentification.Use;

        this.cbRegion = Ext.create('B4.form.ComboBox', {
            editable: false,
            allowBlank: false,
            labelWidth: 120,
            labelAlign: 'right',
            storeAutoLoad: false,
            fieldLabel: 'Регион',
            emptyText: 'Выберите регион...',
            flex: 1,
            typeAhead: false,
            fields: ['GuidId', 'Name', 'PostCode'],
            url: '/Fias/GetRegionList',
            mode: 'remote',
            valueField: 'GuidId',
            displayField: 'Name',
            triggerAction: 'all',
            listeners: {
                select: {
                    fn: function (combo, records) {
                        var record = records[0];
                        if (combo.oldValue == record.data.GuidId) {
                            return;
                        }
                        if (this.cbCity.getValue()) {
                            this.cbCity.clearValue();
                            this.cbCity.store.removeAll();

                            if (this.cbStreet.getValue()) {
                                this.cbStreet.clearValue();
                                this.cbStreet.store.removeAll();
                                this.cbStreet.setEditable(false);
                            }

                            if (me.cbHouse.getValue()) {
                                me.cbHouse.clearValue();
                                me.cbHouse.store.removeAll();
                                me.cbHouse.setEditable(false);
                            }
                        }

                        this.cbCity.setEditable(true);

                        this.tfPostCode.setValue(record.data.PostCode);

                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.cbCity = Ext.create('B4.form.ComboBox', {
            editable: true,
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
            allowBlank:false,
            onTrigger1Click: function() {
                this.clearValue();
                this.setEditable(true);
                if (me.cbStreet.getValue()) {
                    me.cbStreet.clearValue();
                    me.cbStreet.setEditable(false);
                    me.cbStreet.store.removeAll();
                }
                if (me.cbHouse.getValue()) {
                    me.cbHouse.clearValue();
                    me.cbHouse.setEditable(false);
                    me.cbHouse.store.removeAll();
                    me.cbHouse.clearHouseFields();
                }
                me.fillAddressField();
            },
            validator: function () {
                return this.value ? true : "Выберите значение из списка";
            },
            listeners: {
                storebeforeload: {
                    fn: function (field, store, options) {
                        var rec = this.cbRegion.getRecord(this.cbRegion.getValue());
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
            editable: true,
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
            allowBlank:false,
            onTrigger1Click: function () {
                this.clearValue();
                this.setEditable(true);
                if (me.cbHouse.getValue()) {
                    me.cbHouse.clearValue();
                    me.cbHouse.setEditable(false);
                    me.cbHouse.store.removeAll();
                    me.cbHouse.clearHouseFields();
                }

                me.fillAddressField();
            },
            validator: function () {
                return this.value ? true : "Выберите значение из списка";
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
                            me.cbHouse.setEditable(true);
                        } else {
                            combo.setEditable(true);
                            me.cbHouse.setEditable(false);
                        }

                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.cbHouse = Ext.create('B4.form.ComboBox', {
            editable: true,
            storeAutoLoad: false,
            labelWidth: 120,
            labelAlign: 'right',
            fieldLabel: 'Номер дома',
            emptyText: 'Введите номер дома...',
            typeAhead: false,
            fields: ['GuidId', 'AddressName', 'PostCode', 'HouseNum', 'BuildNum', 'StrucNum', 'Letter'],
            url: '/Fias/GetHousesList',
            mode: 'remote',
            valueField: 'GuidId',
            displayField: 'AddressName',
            triggerAction: 'query',
            minChars: 1,
            flex: 1,
            autoSelect: true,
            queryDelay: 500,
            queryParam: 'filter',
            loadingText: 'Загрузка...',
            trigger1Cls: 'x-form-clear-trigger',
            selectOnFocus: false,
            allowBlank: false,
            onTrigger1Click: function () {
                this.clearValue();
                this.clearHouseFields();
                this.setEditable(true);

                me.fillAddressField();
            },
            validator: function () {
                return this.value ? true : "Выберите значение из списка";
            },
            clearHouseFields: function() {
                me.tfPostCode.setValue('');
                me.tfHouse.setValue('');
                me.tfHousing.setValue('');
                me.tfBuilding.setValue('');
                me.tfLetter.setValue('');
            },
            listeners: {
                storebeforeload: {
                    fn: function (field, store, options) {
                        var rec = me.cbStreet.getRecord(me.cbStreet.getValue());

                        if (!rec && me.chbNoStreet.checked) {
                            rec = me.cbCity.getRecord(me.cbCity.getValue());
                        }
                        if (rec) {
                            options.params.parentguid = rec.GuidId;
                        }
                        return !Ext.isEmpty(options.params.parentguid);
                    },
                    scope: this
                },
                select: {
                    fn: function (combo, records) {
                        combo.clearHouseFields();

                        var record = records[0];
                        if (record) {
                            var postalCode = record.get('PostCode'),
                                houseNum = record.get('HouseNum'),
                                buildNum = record.get('BuildNum'),
                                strucNum = record.get('StrucNum'),
                                letter = record.get('Letter');

                            if (postalCode) {
                                me.tfPostCode.setValue(postalCode);
                            }
                            if (houseNum) {
                                me.tfHouse.setValue(houseNum);
                            }
                            if (buildNum) {
                                me.tfHousing.setValue(buildNum);
                            }
                            if (strucNum) {
                                me.tfBuilding.setValue(strucNum);
                            }
                            if (letter) {
                                me.tfLetter.setValue(letter);
                            }

                            combo.setEditable(false);
                        } else {
                            combo.setEditable(true);
                        }

                        me.fillAddressField();
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
            maxLength: 20,
            disabled: useFiasHouseIdentification,
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
            disabled: useFiasHouseIdentification,
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
            maxLength: 20,
            flex: 1,
            disabled: useFiasHouseIdentification,
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
            disabled: useFiasHouseIdentification,
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
            labelWidth: 80,
            labelAlign: 'right',
            flex: 1,
            fieldLabel: 'Номер',
            padding: '0 0 0 40',

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
            width: 200,
            fieldLabel: 'Почтовый индекс'
        });

        this.tfCoordinate = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            name: 'Coords',
            labelWidth: 80,
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
                    fn: function() {
                        me.cbStreet.setDisabled(this.checked);
                        me.cbStreet.onTrigger1Click();
                    }
                }
            }
        });

        this.chbNoHouse = Ext.create('Ext.form.field.Checkbox', {
            fieldLabel: 'Дом отсутствует в ФИАС',
            labelWidth: 120,
            labelAlign: 'right',
            listeners: {
                change: {
                    fn: function () {
                        me.cbHouse.setDisabled(this.checked);
                        me.cbHouse.onTrigger1Click();
                        me.tfHouse.setDisabled(!this.checked);
                        me.tfHousing.setDisabled(!this.checked);
                        me.tfBuilding.setDisabled(!this.checked);
                        me.tfLetter.setDisabled(!this.checked);
                    }
                }
            }
        });

        var items = [
            {
                included: true,
                item:
                {
                    xtype: 'component',
                    itemId: 'councillorsLabel',
                    padding: 5,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell; padding-left: 5px;">Для заполнения адреса необходимо последовательно выбрать регион, населенный пункт, затем улицу и дом.</br>Для очистки выбранных значений используйте "крестик" напротив полей</br></span>'
                }
            },
            {
                included: true,
                item: this.cbRegion
            },
            {
                included: true,
                item: this.cbCity
            },
            {
                included: true,
                item: this.cbStreet
            },
            {
                included: true,
                item: this.chbNoStreet
            },
            {
                included: useFiasHouseIdentification,
                item: this.cbHouse
            },
            {
                included: useFiasHouseIdentification,
                item: this.chbNoHouse
            },
            {
                included: true,
                item: {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    items: [
                        this.tfHouse,
                        this.tfLetter,
                        this.tfHousing,
                        this.tfBuilding
                    ]
                }
            },
            {
                included: true,
                item: {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    items: [
                        this.tfFlat,
                        this.tfPostCode,
                        this.tfCoordinate
                    ]
                }
            },
            {
                included: true,
                item: this.tfAddress
            }
        ];

        var includedItems = [];
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (item.included) {
                includedItems.push(item.item);
            }
        }

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                margin: '4px 10px 4px 0'
            },
            items: includedItems,
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
                                    scope: this
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