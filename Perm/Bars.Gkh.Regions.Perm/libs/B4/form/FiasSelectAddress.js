/**
 Компонент выбора адреса
{   
    xtype: 'b4fiasselectaddress',
    name: 'Address',
    fieldLabel: 'Адрес'
},
*/
Ext.define('B4.form.FiasSelectAddress', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.b4fiasselectaddress',
    alternateClassName: ['B4.FiasSelectAddress'],
    requires: ['B4.form.FiasSelectAddressWindow'],
    // свойства для блокировки полей например чтобы нельзя было редактировать адрес
    flatIsReadOnly: false,

    // свойства для скрытия полей
    flatIsVisible: true,

    fieldsToHideNames: [],
    fieldsRegex: {}, //{tfHousing:{regex:'\d*', regexText:'Message'}}

    editable: false,
    enableKeyEvents: false,
    entityId: null,
    entityValue: null,
    cbCity: null,
    cbStreet: null,
    chbNoStreet: null,
    cbHouse: null,
    chbNoHouse: null,
    tfHouse: null,
    tfLetter: null,
    tfHousing: null,
    tfBuilding: null,
    tfFlat: null,
    tfCoordinate: null,
    tfPostCode: null,
    tfAddress: null,
    cbPlacementType: null,
    addressIsLoad: false,
    win: null,

    trigger1Cls: 'x-form-search-trigger',
    trigger2Cls: 'x-form-clear-trigger',

    onTrigger1Click: function () {
        var me = this;
        if (!me.win) {
            me.createControls();
        }
        me.loadData();
        me.win.show();
    },

    onTrigger2Click: function () {
        this.setValue(null);
    },

    validateForm: function (wind) {
        var me = this,
            form = wind.getForm();

        if (form) {
            if (!form.isValid()) {
                Ext.Msg.alert('Сообщение', 'Адрес заполнен не корректно');
                return false;
            }
        }

        if (me.tfHouse.getValue().trim() == "" && me.tfLetter.getValue().trim() == "") {
            Ext.Msg.alert('Адрес заполнен не корректно!', 'Заполните одно из полей: \'Номер дома\' или \'Литер\'');
            return false;
        }

        return true;
    },

    loadData: function () {
        var me = this,
            data = me.getValue() || {},
            region,
            addressPlaceGuid,
            addressStreetGuid,
            addressHouseGuid,
            recPlace,
            recStreet,
            recHouse;

        if (me.addressIsLoad) {
            me.resetForm();
        }

        if (data.AddressGuid) {
            region = {};
            Ext.Array.each(data.AddressGuid.split("#"), function (value) {
                if (data.StreetGuidId && value.indexOf(data.StreetGuidId) != -1) {
                    addressStreetGuid = value;
                }
                else if (data.HouseGuid && value.indexOf(data.HouseGuid) != -1) {
                    addressHouseGuid = value;
                }
                else {
                    if (addressPlaceGuid) {
                        addressPlaceGuid = addressPlaceGuid + "#";
                    }

                    addressPlaceGuid = (addressPlaceGuid || '') + value;
                }

                var vals = value.split('_');
                if (vals.length === 2 && vals[0] == 1) {
                    region.GuidId = vals[1];
                }

            }, me);

            region.Name = data.AddressName.split(',')[0];

            me.cbRegion.getStore().insert(0, region);
            me.cbRegion.setValue(region);
        } else {
            me.cbRegion.getStore().reload();
            me.cbCity.setEditable(true);
        }

        if (data.PlaceGuidId) {
            recPlace = {
                GuidId: data.PlaceGuidId,
                Code: data.PlaceCode,
                Name: data.PlaceName,
                PostCode: data.PostCode,
                AddressName: data.PlaceAddressName,
                AddressGuid: addressPlaceGuid,
                MirrorGuid: data.MirrorGuid
            };
            me.cbCity.getStore().insert(0, recPlace);
            me.cbCity.setValue(recPlace);
            me.cbCity.setEditable(false);
        }

        if (data.StreetGuidId) {

            recStreet = {
                GuidId: data.StreetGuidId,
                Code: data.StreetCode,
                Name: data.StreetName,
                PostCode: data.PostCode,
                AddressName: data.StreetName,
                AddressGuid: addressStreetGuid,
                MirrorGuid: data.MirrorGuid
            };
            me.cbStreet.getStore().insert(0, recStreet);
            me.cbStreet.setValue(recStreet);
            me.cbStreet.setEditable(false);
        } else if (data.PlaceGuidId) {
            //если есть населенный пункт, но нет улицы - задизаблить поле улицы и отметить чекбокс 
            me.cbStreet.setDisabled(true);
            me.chbNoStreet.setValue(true);
        }

        if (data.HouseGuid) {
            recHouse = {
                GuidId: data.HouseGuid,
                AddressName: data.HouseAddressName,
                PostalCode: data.PostCode,
                HouseNum: data.HouseNum,
                BuildNum: data.BuildNum,
                StrucNum: data.StrucNum,
                Letter: data.Letter,
                AddressGuid: addressHouseGuid
            };
            me.cbHouse.getStore().insert(0, recHouse);
            me.cbHouse.setValue(recHouse);
            me.cbHouse.setEditable(true);
        } else if (data.PlaceGuidId) {
            me.cbHouse.setDisabled(true);
            me.chbNoHouse.setValue(true);
        }

        me.tfHouse.setValue(data.House);
        me.tfLetter.setValue(data.Letter);
        me.tfHousing.setValue(data.Housing);
        me.tfBuilding.setValue(data.Building);
        me.tfFlat.setValue(data.Flat);
        me.tfCoordinate.setValue(data.Coordinate);
        me.tfPostCode.setValue(data.PostCode);
        me.tfAddress.setValue(data.AddressName);

        B4.Ajax.request({
            url: B4.Url.action('GetPlacementTypeList', 'Fias')
        }).next(function (response) {
            var palcementTypes = Ext.decode(response.responseText).data;
            if (data.AddressName) {
                var placementType = palcementTypes.filter(function (o) { return data.AddressName.indexOf(o.ShortName) != -1; })[0];
                if (placementType != null) {
                    me.cbPlacementType.setValue(placementType.Id);
                }
            }
            else {
                me.cbPlacementType.setValue(palcementTypes[1]);
            }
        });

        me.addressIsLoad = true;
    },
    acceptForm: function (wind) {
        var me = this;
        if (me.validateForm(wind)) {
            me.setValue(me.buildResult());
            me.addressIsLoad = true;
            me.isValid();
            me.win.destroy();
        }
    },

    cancelForm: function () {
        var me = this;
        me.cbRegion = null;
        me.cbCity = null;
        me.cbStreet = null;
        me.cbHouse = null;
        me.tfHouse = null;
        me.tfLetter = null;
        me.tfHousing = null;
        me.tfBuilding = null;
        me.tfFlat = null;
        me.tfCoordinate = null;
        me.tfPostCode = null;
        me.tfAddress = null;
        me.cbPlacementType = null;

        me.win.destroy();
    },

    resetForm: function () {
        var me = this;
        me.addressIsLoad = false;
        if (me.win) {
            me.cbRegion.clearValue();
            me.cbRegion.store.removeAll();
            me.cbCity.clearValue();
            me.cbCity.store.removeAll();
            me.cbStreet.clearValue();
            me.cbStreet.store.removeAll();
            me.cbHouse.clearValue();
            me.cbHouse.store.removeAll();
            me.tfHouse.setValue('');
            me.tfLetter.setValue('');
            me.tfHousing.setValue('');
            me.tfBuilding.setValue('');
            me.tfFlat.setValue('');
            me.tfCoordinate.setValue('');
            me.tfPostCode.setValue('');
            me.tfAddress.setValue('');
            me.cbPlacementType.clearValue();
            me.cbPlacementType.store.removeAll();
        }
    },

    buildResult: function () {

        var me = this,
            recordPlace = me.cbCity.getRecord(me.cbCity.getValue()) || { data: {} },
            recordStreet = me.cbStreet.getRecord(me.cbStreet.getValue()) || { data: {} },
            recordHouse = me.cbHouse.getRecord(me.cbHouse.getValue()) || { data: {} },
            addressGuid;

        addressGuid = recordPlace.AddressGuid;
        if (recordStreet.AddressGuid) {
            addressGuid = addressGuid + "#" + recordStreet.AddressGuid;
        }
        if (recordHouse.GuidId) {
            addressGuid = addressGuid + "#" + recordHouse.GuidId;
        }

        return {
            Id: me.getEntityId(),
            AddressName: me.tfAddress.getValue(),
            PlacementType: me.cbPlacementType.getValue(),
            AddressGuid: addressGuid,
            PlaceCode: recordPlace.Code,
            PlaceGuidId: recordPlace.GuidId,
            PlaceName: recordPlace.Name,
            PlaceAddressName: recordPlace.AddressName,
            StreetCode: recordStreet.Code,
            StreetGuidId: recordStreet.GuidId,
            StreetName: recordStreet.Name,
            MirrorGuid: recordStreet.MirrorGuid,
            HouseGuid: recordHouse.GuidId,
            HouseAddressName: recordHouse.AddressName,
            HouseNum: recordHouse.HouseNum,
            BuildNum: recordHouse.BuildNum,
            StrucNum: recordHouse.StrucNum,
            House: me.tfHouse.getValue(),
            Letter: me.tfLetter.getValue(),
            Housing: me.tfHousing.getValue(),
            Building: me.tfBuilding.getValue(),
            Flat: me.tfFlat.getValue(),
            Coordinate: me.tfCoordinate.getValue(),
            PostCode: me.tfPostCode.getValue()
        };

    },

    setValue: function (data) {
        var me = this, oldValue;
        if (!data) {
            me.entityId = 0;
            me.entityValue = null;
            me.setRawValue.call(me, '');
            me.resetForm();
        } else {
            if (typeof (data) === 'number') {
                me.loadAdrressById(data);
            } else {
                oldValue = me.getValue();
                me.entityId = data.id || data.Id || 0;
                me.entityValue = data;
                me.setRawValue.call(me, me.entityValue.AddressName || '');
                me.fireEvent('change', me, data, oldValue);
            }
        }
    },
    getValue: function () {
        var me = this;
        if (!me.entityValue) {
            return null;
        }
        return me.entityValue;
    },
    getEntityId: function () {
        return this.entityId || 0;
    },
    loadAdrressById: function (id) {
        var me = this;
        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/FiasAddress/Get'),
            params: { id: id },
            success: function (response) {
                //десериализуем полученный адрес
                var address = Ext.JSON.decode(response.responseText);
                me.setValue(address);
            }
        });
    },

    createControls: function () {
        var me = this;
        var regs = me.fieldsRegex;
        if (!me.win) {
            me.win = Ext.create('B4.form.FiasSelectAddressWindow',
            {
                constrain: true,
                autoDestroy: true,
                closeAction: 'destroy',
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.win.on('acceptform', me.acceptForm, me);
            me.win.on('resetform', me.resetForm, me);
            me.win.on('cancelform', me.cancelForm, me);
            me.win.on('destroy', function () {
                me.win = null;
            }, me);

            me.cbRegion = me.win.cbRegion;
            me.cbCity = me.win.cbCity;
            me.cbStreet = me.win.cbStreet;
            me.chbNoStreet = me.win.chbNoStreet;
            me.cbHouse = me.win.cbHouse;
            me.chbNoHouse = me.win.chbNoHouse;
            me.tfHouse = me.win.tfHouse;
            me.tfLetter = me.win.tfLetter;
            me.tfHousing = me.win.tfHousing;
            me.tfBuilding = me.win.tfBuilding;
            me.tfFlat = me.win.tfFlat;
            me.tfPostCode = me.win.tfPostCode;
            me.tfCoordinate = me.win.tfCoordinate;
            me.tfAddress = me.win.tfAddress;
            me.cbPlacementType = me.win.cbPlacementType;

            me.tfPostCode.regex = /^\d+$/;
            me.tfPostCode.regexText = 'В это поле можно вводить только цифры!';

            me.tfBuilding.regex = regs.tfBuilding ? regs.tfBuilding.regex : /^\d+(\s*([А-Яа-я]{0,1}))?$/;
            me.tfBuilding.regexText = regs.tfBuilding ? regs.tfBuilding.regexText : 'В это поле можно вводить только цифры и одну букву(кириллица)! (Пример: 45А)';

            me.tfHouse.regex = regs.tfHouse ? regs.tfHouse.regex : /^\d+[А-Яа-я]{0,1}((\/)\d*[А-Яа-я]{0,1}){0,2}?$/;
            me.tfHouse.regexText = regs.tfHouse ? regs.tfHouse.regexText : 'В это поле можно вводить значение следующих форматов: 33/22, 33/А, 33/22А, 33А, 17а/1, 23б/3а, 2/4/1, 1а/13в/1';

            if (me.flatIsReadOnly) {
                me.tfFlat.setDisabled(true);
            } else {
                me.tfFlat.setDisabled(false);
            }

            if (me.flatIsVisible) {
                me.tfFlat.setVisible(true);
            } else {
                me.tfFlat.setVisible(false);
            }

            Ext.Array.each(me.fieldsToHideNames, function (fieldName) {
                var field = me[fieldName];
                if (field && field.isComponent) {
                    field.setVisible(false);
                }
            });
        }
    }
});