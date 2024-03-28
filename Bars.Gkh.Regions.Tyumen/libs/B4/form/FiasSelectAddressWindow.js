Ext.define('B4.form.FiasSelectAddressWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    itemId: 'fiasSelectAddressWindow',
    title: 'Адрес с выбором дома',
    closeAction: 'destroy',
    padding: 2,

    requires: [
        'B4.form.ComboBox', 
        'B4.enums.UseFiasHouseIdentification',
        'B4.form.EnumCombo',
        'B4.enums.GkhStructureType',
        'B4.enums.FiasEstimateStatusEnum'
    ],

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
            addSeparator = false;

        var regionName = me.cbRegion.getValue() ? me.cbRegion.getDisplayValue() : '';
        if (!Ext.isEmpty(regionName)) {
            result = regionName;
            addSeparator = true;
        }

        var cityName = me.cbCity.getValue() ? me.cbCity.getDisplayValue() : '';
        if (!Ext.isEmpty(cityName)) {
            result = cityName;
            addSeparator = true;
        }

        var streetName = me.cbStreet.getValue() ? me.cbStreet.getDisplayValue() : '';
        if (!Ext.isEmpty(streetName)) {
            result += (addSeparator ? ', ' : '') + streetName;
            addSeparator = true;
        }

        if (me.tfHouse.getValue()) {
            result += (addSeparator ? ', ' : '') + this.GetEstimatePrefix(me.cbHouseEstimateType.getValue())+ ' ' + me.tfHouse.getValue().trim();
            addSeparator = true;
        }
        
        if (me.tfHousing.getValue()) {
            result += (addSeparator ? ', ' : '') + 'корп.' + ' ' + me.tfHousing.getValue();
            addSeparator = true;
        }
        
        if (me.tfBuilding.getValue()) {
            var record = me.cbHouse.getRecord(me.cbHouse.getValue()),
                structureType = (me.chbNoHouse.getValue() || me.cbStructureType.getValue()
                        ? me.cbStructureType.getValue()
                        : (record && record.StructureType))
                    || 0,
                structureResult = '';
            switch (structureType) {
                case 1: structureResult = 'стр.'; break;
                case 2: structureResult = 'соор.'; break;
                case 3: structureResult = 'лит.'; break;
            }
            me.cbStructureType.setValue(structureType);
            result += (addSeparator ? ', ' : '') + structureResult + ' ' + me.tfBuilding.getValue();
        }

        me.tfAddress.setValue(result);
    },

    GetEstimatePrefix: function (estimate) {
        switch (estimate) {
            case 1:
                return 'владение';
            case 3:
                return 'домовладение';
            case 4:
                return 'гараж';
            case 5:
                return 'здание';
            case 6:
                return 'шахта';
            default:
                return 'д.';
        }
    },

    onChangeStructureType: function () {
        var me = this,
            cmb = me.cbStructureType;
        if (me.chbNoHouse.getValue() == false || cmb.value == 0) {
            me.tfBuilding.setDisabled(true);
            return;
        }
        else if (me.chbNoHouse.getValue() == true || cmb.value != 0) {
            me.tfBuilding.setDisabled(false);         
        };
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
            minChars: 3,
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
            minChars: 3,
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
                            me.cbHouseEstimateType.setEditable(true);
                        } else {
                            combo.setEditable(true);
                            me.cbHouse.setEditable(false);
                            me.cbHouseEstimateType.setEditable(false);
                        }

                        this.fillAddressField();
                    },
                    scope: this
                }
            }
        });

        this.cbHouseEstimateType = Ext.create('B4.form.ComboBox', {
            editable: false,
            allowBlank: false,
            labelWidth: 120,
            labelAlign: 'right',
            storeAutoLoad: false,
            fieldLabel: 'Тип владения',
            emptyText: 'Выберите тип владения...',
            flex: 1,
            store: B4.enums.FiasEstimateStatusEnum.getStore(),
            typeAhead: false,
            //mode: 'remote',
            valueField: 'Value',
            displayField: 'Description',
            listeners: {
                change: {
                    fn: function () {
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
            fields: ['GuidId', 'AddressName', 'PostCode', 'HouseNum', 'BuildNum', 'StrucNum', 'StructureType','EstimateType'],
            url: '/Fias/GetHousesListWithEstimate',
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

                        var estimateType = me.cbHouseEstimateType.getRecord(me.cbHouseEstimateType.getValue());
                        if (estimateType)
                        {                            
                            options.params.estimatetype = estimateType.Value;
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
                                estimateType = record.get('EstimateType');
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
                            if (estimateType) {
                                me.cbHouseEstimateType.setValue(estimateType);
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
            flex: 0.4,
            fieldLabel: 'Дом',
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
                
        this.tfHousing = Ext.create('Ext.form.TextField', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            fieldLabel: 'Корпус',
            flex: 0.4,
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
            padding: '0 2 5 15',
            labelAlign: 'right',
            flex: 0.1,
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
        
        this.cbStructureType = Ext.create('B4.form.EnumCombo', {
            allowBlank: true,
            labelWidth: 120,
            labelAlign: 'right',
            width: 200,
            flex: 0.7,
            fieldLabel: 'Признак строения',
            disabled: useFiasHouseIdentification,
            enumName: 'B4.enums.GkhStructureType',
            listeners: {
                change: {
                    fn: function () {
                        this.onChangeStructureType();
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
                        me.cbHouseEstimateType.setDisabled(this.checked);
                        me.cbHouse.onTrigger1Click();
                        me.tfHouse.setDisabled(!this.checked);
                        me.tfHousing.setDisabled(!this.checked);
                        me.tfBuilding.setDisabled(!this.checked);
                        me.cbStructureType.setDisabled(!this.checked);
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
                included: true,
                item: this.cbHouseEstimateType
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
                        this.tfHousing,
                        this.cbStructureType,
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