Ext.define('B4.controller.publicservorg.Contract',
{
    extend: 'B4.base.Controller',
    params: null,
    ContractId: null,
    ContractPart: null,

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.TypeContractPart',
        'B4.enums.TypeOwnerContract',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'publicservorg.Contract',
        'publicservorg.RealtyObject',
        'publicservorg.RealObjPublicServiceOrgService',
        'publicservorg.contractpart.IndividualOwnerContract',
        'publicservorg.contractpart.JurPersonOwnerContract',
        'publicservorg.contractpart.OfferContract',
        'publicservorg.contractpart.RsoAndServicePerformerContract',
        'publicservorg.contractpart.FuelEnergyResourceContract',
        'publicservorg.contractpart.BudgetOrgContract',
        'publicservorg.RealtyObject'
    ],

    stores: [
        'publicservorg.Contract',
        'publicservorg.RealObjPublicServiceOrgService',
        'publicservorg.RealtyObjForSelect',
        'publicservorg.RealtyObjForSelected',
        'publicservorg.RealtyObject',
        'B4.store.publicservorg.RealityObjectInContract',
        'dict.TypeCustomer'
    ],

    views: [
        'publicservorg.ContractEditWindow',
        'publicservorg.ContractMainInfo',
        'publicservorg.ContractGrid',
        'publicservorg.ContractServiceGrid',
        'publicservorg.ContractServiceEditWindow',
        'publicservorg.contractqualitylevel.Panel',
        'publicservorg.contractqualitylevel.Grid',
        'publicservorg.contractqualitylevel.EditWindow',
        'publicservorg.ContractTempGraphGrid',
        'publicservorg.ContractTempGraph',
        'publicservorg.contractpart.MainPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'publicservorg.ContractGrid',
    mainViewSelector: 'publicservorgcontractgrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
    {
        xtype: 'grideditwindowaspect',
        name: 'publicServOrgContractGridWindowAspect',
        gridSelector: 'publicservorgcontractgrid',
        editFormSelector: 'publicservorgcontracteditwindow',
        storeName: 'publicservorg.Contract',
        modelName: 'publicservorg.Contract',
        editWindowView: 'publicservorg.ContractEditWindow',
        otherActions: function(actions) {
            actions['publicservorgcontracteditwindow #sfRealityObject'] = {
                'beforeload': {
                    fn: function(store, operation) {
                        operation.params = operation.params || {};
                        operation.params.publicServOrgId = this.controller.getContextValue(this.controller.getMainView(), 'publicservorgid');
                    },
                    scope: this
                }
            };

            actions['contractpartymainpanelpanel b4combobox[name=TypeContractPart]'] = {
                'change': {
                    fn: function(field) { this.updateForm(field.up('contractpartymainpanelpanel')); },
                    scope: this
                }
            };

            actions['contractpartymainpanelpanel b4combobox[name=TypeOwnerContract]'] = {
                'change': {
                    fn: function(field) { this.updateForm(field.up('contractpartymainpanelpanel')); },
                    scope: this
                }
            };
        },
        listeners: {
            getdata: function(asp, record) {
                if (!record.getId()) {
                    record.data.PublicServiceOrg = asp.controller.getContextValue(asp.controller.getMainView(), 'publicservorgid');
                }
            },
            aftersetformdata: function(asp, rec, form) {
                var servGrid = form.down('publicservorgcontractservicegrid'),
                    tepmInfoPanel = form.down('publicservorgcontracttepgraphpanel'),
                    realtyobjectincontractgrid = form.down('realtyobjectincontractgrid'),
                    tepmInfoGrid = form.down('publicservorgcontracttempgraphgrid'),
                    contractPanel = form.down('contractpartymainpanelpanel'),
                    rbStart = form.down('radiogroup[name=StartDeviceMetteringIndication]'),
                    rbEnd = form.down('radiogroup[name=EndDeviceMetteringIndication]');

                servGrid.contractId = rec.getId();
                asp.controller.ContractId = rec.getId();
                servGrid.getStore().filter('servOrgContractId', rec.getId());

                if (rec.phantom) {
                    tepmInfoPanel.disable();
                    realtyobjectincontractgrid.disable();
                } else {
                    tepmInfoGrid.contractId = rec.getId();
                    tepmInfoGrid.getStore().filter('servOrgContractId', rec.getId());
                    asp.controller.getStore('publicservorg.RealityObjectInContract').load();
                }

                if (rbStart && rbEnd) {
                    if (!rec.phantom) {
                        rbStart
                            .setValue({ 'StartDeviceMetteringIndication': rec.get('StartDeviceMetteringIndication') });
                        rbEnd.setValue({ 'EndDeviceMetteringIndication': rec.get('EndDeviceMetteringIndication') });
                    } else {
                        rbStart.setValue({ 'StartDeviceMetteringIndication': 1 });
                        rbEnd.setValue({ 'EndDeviceMetteringIndication': 1 });
                    }
                }

                this.updateForm(contractPanel);

                if (!rec.phantom) {
                    this.setReadOnly(contractPanel);
                }
            },
            // сохраняем сторону контракта для последующей его отправки на сервер
            beforesave: function(asp, rec) {
                var form = asp.getForm(),
                    panel = form.down('contractpartymainpanelpanel'),
                    typeContractPartyCb = panel.down('b4combobox[name=TypeContractPart]'),
                    typeOwnerContractCb = panel.down('b4combobox[name=TypeOwnerContract]'),
                    prefix = 'B4.model.publicservorg.contractpart.',
                    modelType,
                    typeContractPart = typeContractPartyCb.getValue(),
                    typeOwnerContract = typeOwnerContractCb.getValue(),
                    recordToSave;

                switch (typeContractPart) {
                    case B4.enums.TypeContractPart.OfferContract:
                        modelType = 'OfferContract';
                        break;
                    case B4.enums.TypeContractPart.RsoAndOwnersContact:
                        modelType = typeOwnerContract === B4.enums.TypeOwnerContract.JurPersonOwnerContact
                            ? 'JurPersonOwnerContract'
                            : 'IndividualOwnerContract';
                        break;
                    case B4.enums.TypeContractPart.RsoAndServicePerformerContract:
                        modelType = 'RsoAndServicePerformerContract';
                        break;
                    case B4.enums.TypeContractPart.BudgetOrgContract:
                        modelType = 'BudgetOrgContract';
                        break;
                    case B4.enums.TypeContractPart.FuelEnergyResourceContract:
                        modelType = 'FuelEnergyResourceContract';
                        break;
                }

                if (modelType) {
                    recordToSave = Ext.create(prefix + modelType, rec.data);
                    recordToSave.data.Id = rec.get('ContractPartId');
                    recordToSave.phantom = !rec.get('ContractPartId');
                    asp.controller.ContractPart = recordToSave;
                }
            },
            // когда сохранили и знаем Id контракта, можем уже отправлять на сервер для сохранения
            savesuccess: function(asp, rec) {
                var recordToSave = asp.controller.ContractPart;

                if (recordToSave) {
                    rec.setDirty();
                    recordToSave.data.PublicServiceOrgContract = rec.get('Id');
                    this.saveContactRecord(recordToSave);
                }
            }
        },
        saveContactRecord: function (record) {
            var me = this;

            record.save()
                .next(function(result) {
                    me.getGrid().getStore().load();
                })
                .error(function(resp) {
                    Ext.Msg.alert('Ошибка удаления!', Ext.isString(resp.responseData) ? resp.responseData : resp.responseData.message);
                });
        },

        updateForm: function(panel) {
            var me = this;
            if (!panel) {
                return;
            }
            // в зависимости от состояния сущности скрываем/отображаем нужные поля
            var typeContractPartyCb = panel.down('b4combobox[name=TypeContractPart]'),
                commercialMeteringResourceTypeCb = panel.down('b4combobox[name=CommercialMeteringResourceType]'),
                typeContactPersonCb = panel.down('b4combobox[name=TypeContactPerson]'),
                typeOwnerContractCb = panel.down('b4combobox[name=TypeOwnerContract]'),
                managingOrganizationSf = panel.down('b4selectfield[name=ManagingOrganization]'),
                contragentSf = panel.down('b4selectfield[name=Contragent]'),
                typeCustomerSf = panel.down('b4selectfield[name=TypeCustomer]'),
                organizationSf = panel.down('b4selectfield[name=Organization]'),
                fuelEnergyResourceOrgSf = panel.down('b4selectfield[name=FuelEnergyResourceOrg]'),
                lastNameTf = panel.down('textfield[name=LastName]'),
                firstNameTf = panel.down('textfield[name=FirstName]'),
                middleNameTf = panel.down('textfield[name=MiddleName]'),
                birthPlaceTf = panel.down('textfield[name=BirthPlace]'),
                additionalInfoContainer = panel.down('container[name=AdditionalInfo]'),
                passportInfoFs = panel.down('fieldset[name=PassportInfo]'),

                typeContractParty = typeContractPartyCb.getValue(),
                typeOwnerContract = typeOwnerContractCb.getValue();

            var isRsoAndServicePerformerContract = typeContractParty &&
                typeContractParty === B4.enums.TypeContractPart.RsoAndServicePerformerContract;
            var isJurOrIndividualContract = typeContractParty &&
                typeContractParty === B4.enums.TypeContractPart.RsoAndOwnersContact;

            var isJurpersonContract = isJurOrIndividualContract &&
                typeOwnerContract &&
                typeOwnerContract === B4.enums.TypeOwnerContract.JurPersonOwnerContact;

            var isIndividualPersonContract = isJurOrIndividualContract &&
                typeOwnerContract &&
                typeOwnerContract === B4.enums.TypeOwnerContract.IndividualOwnerContract;

            var isBudgetOrgContract = typeContractParty === B4.enums.TypeContractPart.BudgetOrgContract;
            var isFuelEnergyContract = typeContractParty === B4.enums.TypeContractPart.FuelEnergyResourceContract;

            me.setVisibility(commercialMeteringResourceTypeCb, isRsoAndServicePerformerContract);
            me.setVisibility(typeContactPersonCb, isJurOrIndividualContract);
            me.setVisibility(typeOwnerContractCb, isJurOrIndividualContract);
            me.setVisibility(managingOrganizationSf, isRsoAndServicePerformerContract);
            me.setVisibility(contragentSf, isJurpersonContract);
            me.setVisibility(lastNameTf, isIndividualPersonContract);
            me.setVisibility(firstNameTf, isIndividualPersonContract);
            me.setVisibility(middleNameTf, isIndividualPersonContract);
            me.setVisibility(birthPlaceTf, isIndividualPersonContract);
            me.setVisibility(additionalInfoContainer, isIndividualPersonContract);
            me.setVisibility(passportInfoFs, isIndividualPersonContract);
            me.setVisibility(typeCustomerSf, isBudgetOrgContract);
            me.setVisibility(organizationSf, isBudgetOrgContract);
            me.setVisibility(fuelEnergyResourceOrgSf, isFuelEnergyContract);
        },

        setReadOnly: function (element, enabled) {
            if (element.setReadOnly) {
                element.setReadOnly(!enabled && element.onCreate);
            }

            if (element.items && element.items.items) {

                Ext.each(element.items.items, function (el) { this.setReadOnly(el, enabled); }, this);
            }
        },

            setVisibility: function(elem, visible) {
                if (visible) {
                    elem.show();
                } else {
                    elem.hide();
                }

                this.setAllowBlank(elem, !visible);
            },
            // рекурсивно делаем необязательными все элементы (для того, чтобы захватить контейнеры и филдсеты)
            setAllowBlank: function(elem, allowBlank) {
                // если компонент допускает необязательное значение по вьюхе, то тут мы его не трогаем
                if (elem.required) {
                    elem.allowBlank = allowBlank;
                }
                

                if (elem.items && elem.items.items) {

                    Ext.each(elem.items.items, function(el) { this.setAllowBlank(el, allowBlank); }, this);
                }
            },
            saveRequestHandler: function() {
                var rec, from = this.getForm();
                if (this.fireEvent('beforesaverequest', this) !== false) {
                    from.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(from.getRecord());

                    var data = rec.getData();
                    var dateStart = data.DateStart;
                    var dateEnd = data.DateEnd;
                    if (dateEnd != null && dateEnd < dateStart) {
                        B4.QuickMsg.msg('Ошибка', '"Дата начала" должна быть меньше "Дата окончания"', 'error');
                        return;
                    }

                    this.fireEvent('getdata', this, rec);

                    if (from.getForm().isValid()) {
                        if (this.fireEvent('validate', this)) {
                            this.saveRecord(rec);
                        }
                    } else {
                        //получаем все поля формы
                        var fields = from.getForm().getFields();

                        var invalidFields = '';

                        //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                        Ext.each(fields.items, function(field) {
                            if (!field.isValid()) {
                                invalidFields += '<br>' + field.fieldLabel;
                            }
                        });

                        //выводим сообщение
                        Ext.Msg.alert('Ошибка сохранения!', 'Не заполнены обязательные поля: ' + invalidFields);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'publicServOrgContractServiceGridWindowAspect',
            gridSelector: 'publicservorgcontractservicegrid',
            editFormSelector: 'publicservorgcontractserviceeditwindow',
            modelName: 'publicservorg.RealObjPublicServiceOrgService',
            editWindowView: 'publicservorg.ContractServiceEditWindow',
            saveRequestHandler: function () {
                var rec, form = this.getForm();
                if (this.fireEvent('beforesaverequest', this) !== false) {
                    form.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(form.getRecord());

                    if (!rec.data.ResOrgContract) {
                        B4.QuickMsg.msg('Ошибка', 'Для добавления услуги нужно заполнить и сохранить договор', 'error');
                        return;
                    }

                    this.fireEvent('getdata', this, rec);

                    if (form.getForm().isValid()) {
                        if (this.fireEvent('validate', this)) {
                            this.saveRecord(rec);
                        }
                    } else {
                        var errorMessage = this.getFormErrorMessage(form);
                        Ext.Msg.alert('Ошибка сохранения!', errorMessage);
                    }
                }
            },
            getRecordBeforeSave: function(record) {
                var me = this,
                    grid = me.getGrid();

                if (grid.contractId) {
                    record.set('ResOrgContract', grid.contractId);
                }

                return record;
            },
            needToFill: function(value) {
                // да, да, костыль, но мы не можем иначе завязаться на значении справочника
                return value && (value.toLowerCase() === 'тепловая энергия' || value.toLowerCase() === 'горячая вода');
            },
            otherActions: function(actions) {
                var me = this;

                actions[me.editFormSelector + ' b4selectfield[name=CommunalResource]'] = { 'change': { fn: this.onСommunalResourceChange, scope: this } }
            },
            onСommunalResourceChange: function(field, newVal, oldVal) {
                var me = this,
                    form = me.getForm(),
                    heatingSystemTypeCb = form.down('b4combobox[name=HeatingSystemType]'),
                    schemeConnectionTypeCb = form.down('b4combobox[name=SchemeConnectionType]'),
                    required = newVal ? me.needToFill(newVal.Name) : null;

                if (typeof(required) == 'boolean' && heatingSystemTypeCb && schemeConnectionTypeCb) {
                    heatingSystemTypeCb.setDisabled(!required);
                    schemeConnectionTypeCb.setDisabled(!required);

                    if (!required) {
                        heatingSystemTypeCb.setValue(null);
                        schemeConnectionTypeCb.setValue(null);
                    }
                }
            },
            listeners: {
                aftersetformdata: function(asp, rec, form) {
                    var grid = form.down('contractservicequalitylevelgrid'),
                        panel = form.down('contractservicecontractqualitylevelpanel');

                    if (rec.phantom) {
                        panel.disable();
                    }

                    grid.serviceId = rec.getId();
                    grid.getStore().filter('serviceId', rec.getId());
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'publicServOrgServiceQualityLevelGridWindowAspect',
            gridSelector: 'contractservicequalitylevelgrid',
            editFormSelector: 'contractqualitylevelEditWindow',
            modelName: 'publicservorg.PublicOrgServiceQualityLevel',
            editWindowView: 'publicservorg.contractqualitylevel.EditWindow',
            getRecordBeforeSave: function(record) {
                var me = this,
                    grid = me.getGrid();

                if (grid.serviceId) {
                    record.data.ServiceOrg = grid.serviceId;
                }

                return record;
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tempGraphInlineGridAspect',
            storeName: 'publicservorg.ContractTempGraph',
            modelName: 'publicservorg.ContractTempGraph',
            gridSelector: 'publicservorgcontracttempgraphgrid',
            saveButtonSelector: 'publicservorgcontracttempgraphgrid button[name=saveGrid]',
            listeners: {
                beforeaddrecord: function(asp, rec) {
                    var grid = asp.getGrid();
                    if (grid.contractId) {
                        rec.data.Contract = grid.contractId;
                    }
                },

                beforesave: function(asp, store) {
                    var errorProps = [],
                        errorMsg = 'Не заполнены обязательные поля:',
                        modifiedRecs = store.getModifiedRecords(),
                        anyEmptyTemp = false,
                        anyEmptySupply = false,
                        anyEmptyReturn = false;

                    modifiedRecs.forEach(function(item) {
                        if (!Number.parseInt(item.data.OutdoorAirTemp)) {
                            anyEmptyTemp = true;
                        }
                        if (!Number.parseInt(item.data.CoolantTempSupplyPipeline)) {
                            anyEmptySupply = true;
                        }
                        if (!Number.parseInt(item.data.CoolantTempReturnPipeline)) {
                            anyEmptyReturn = true;
                        }
                    });

                    if (anyEmptyTemp) {
                        errorProps.push('<br>Температура наружного воздуха');
                    }
                    if (anyEmptySupply) {
                        errorProps.push('<br>Температура теплоносителя в подающем трубопроводе');
                    }
                    if (anyEmptyReturn) {
                        errorProps.push('<br>Температура теплоносителя в обратном трубопроводе');
                    }
                    if (errorProps.length > 0) {
                        Ext.Msg.alert('Предупреждение!', errorMsg + errorProps);
                        return false;
                    }
                    return true;
                }
            },
            otherActions: function(actions) {
                actions['publicservorgcontracttempgraphgrid b4deletecolumn'] = {
                    click: { fn: this.onDeleteRecord, scope: this }
                }
            },
            onDeleteRecord: function(a, b, t, y, r, rec) {
                var me = this;
                window.asp = this;
                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                    if (result == 'yes') {
                        if (!rec.phantom) {
                            me.mask("Удаление");
                            rec.destroy().next(me.onSuccess, me).error(me.onError, me);
                        } else {
                            me.getGrid().getStore().remove(rec);
                        }
                    }
                });
            },
            onSuccess: function() {
                var me = this;
                me.unmask();
                me.updateGrid();
            },
            onError: function(result) {
                var me = this;
                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                me.unmask();
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.RealityObjectResOrgService',
                    applyTo: 'publicservorggridpanel',
                    selector: 'publicservorgcontracteditwindow',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            /* 
            Аспект взаимодействия таблицы жилых домов с массовой формой выбора жилых домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'publicServOrgRosAspect',
            gridSelector: 'realtyobjectincontractgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realtyobjectincontractselect',
            storeSelect: 'publicservorg.ByPublicServOrg',
            storeSelected: 'publicservorg.ByPublicServOrg',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Жилые дома для отбора',
            titleGridSelected: 'Выбранные жилые дома',
            storeName: 'publicservorg.RealityObjectInContract',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 0.5, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManOrgs', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddRealtyObjectsToContract', 'PublicServiceOrgContractRealObj'),
                            method: 'POST',
                            params: {
                                roIds: Ext.encode(recordIds),
                                сontractId: asp.controller.ContractId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранение!', 'Жилые дома сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать жилые дома');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                if (this.controller.params) {
                    operation.params.publicServOrgId = this.controller.getContextValue(this.controller.getMainView(), 'publicservorgid');
                }
            }
        }
    ],

    init: function () {
        this.getStore('publicservorg.Contract').on('beforeload', this.onBeforeLoad, this);
        this.getStore('publicservorg.RealityObjectInContract').on('beforeload', this.onBeforeRoLoad, this);
        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('publicservorgcontractgrid'),
            args = me.processArgs();

        me.bindContext(view);
        me.setContextValue(view, 'publicservorgid', id);
        me.application.deployView(view, 'public_servorg');

        view.getStore().load();

        if (args && parseInt(args.contractId) > 0) {
            me.edit(id, parseInt(args.contractId));

            if (args.newToken) {
                me.application.redirectTo(args.newToken);
            }
        }
    },

    edit: function (id, contractId) {
        var me = this,
            aspect,
            baseModel = me.getModel('publicservorg.Contract');

        contractId ? baseModel.load(contractId, {
            success: function (record) {
                aspect = me.getAspect('publicServOrgContractGridWindowAspect');
                aspect.editRecord(record);
            },
            scope: aspect
        }) : function () {
            Ext.Msg.alert('Ошибка', 'Не найден поставщик ресурсов');
        };
    },

    onBeforeLoad: function(store, operation) {
        operation.params.publicServOrgId = this.getContextValue(this.getMainView(), 'publicservorgid');
        operation.params.fromContract = true;
    },

    onBeforeRoLoad: function (store, operation) {
        if (this.ContractId) {
            operation.params.contractId = this.ContractId;
        }
    },

    processArgs: function () {
        var token = Ext.History.getToken(),
            result = null,
            argsIndex = token.indexOf('?'),
            args,
            param;

        if (argsIndex > -1) {
            result = {};
            args = token.substring(argsIndex + 1).replace(new RegExp('/', 'g'), '').trim().split('&');

            if (args.length > 0) {
                args.forEach(function (item) {
                    if (item.indexOf('=') > 0) {
                        param = item.split('=');

                        if (param.length > 1) {
                            result[param[0]] = param[1];
                        }
                    }
                });
            }

            result.newToken = token.substring(0, argsIndex);
        }

        return result;
    }
});