Ext.define('B4.controller.baseprosclaim.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.BaseProsClaim',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'BaseProsClaim',
        'Disposal'
    ],

    stores: [
        'baseprosclaim.RealityObject',
        'ManagingOrganization',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],

    views: [
        'baseprosclaim.EditPanel',
        'baseprosclaim.RealityObjectGrid',
        'SelectWindow.MultiSelectWindow',
        'RoomNumsEditWindow'
    ],

    mainView: 'baseprosclaim.EditPanel',
    mainViewSelector: '#baseProsClaimEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseProsClaimEditPanelSelector: '#baseProsClaimEditPanel',

    aspects: [
        {
            /*
            Аспект формирвоания документов для данного основания проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseProsClaimCreateButtonAspect',
            buttonSelector: '#baseProsClaimEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseProsClaimEditPanel',
            typeBase: 50 // Тип требование прокуратуры
        },
        {
            xtype: 'baseprosclaimperm',
            name: 'baseProsClaimPermission',
            editFormAspectName: 'baseProsClaimEditPanelAspect'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Column.RoomNums', applyTo: '[dataIndex=RoomNums]', selector: '#baseProsClaimRealityObjectGrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                }
            ],
            editFormAspectName: 'baseProsClaimEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseProsClaimStateButtonAspect',
            stateButtonSelector: '#baseProsClaimEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseProsClaimEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели проверки по требованию прокуратуры
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseProsClaimEditPanelAspect',
            editPanelSelector: '#baseProsClaimEditPanel',
            modelName: 'BaseProsClaim',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' [name=IsResultSent]'] = { 'change': { fn: this.changeInspToFinalState, scope: this } };
            },
            onBeforeLoadContragent: function (store, operation) {
                var panel = Ext.ComponentQuery.query(this.controller.baseProsClaimEditPanelSelector)[0];
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.typeJurOrg = panel.down('#cbTypeJurPerson').getValue();
            },
            changeInspToFinalState: function (fld, newValue) {
                var me = this,
                    panel = me.getPanel(),
                    inspId = me.controller.params.inspectionId,
                    stateAspect = me.controller.getAspect('baseProsClaimStateButtonAspect'),
                    model = me.controller.getModel('BaseProsClaim'),
                    permAspect = me.controller.getAspect('baseProsClaimPermission'),
                    rec = panel.getForm().getRecord();

                if (rec.get('IsResultSent') ===false && newValue === true) {
                    me.controller.mask('Смена статуса', panel);
                    B4.Ajax.request({
                        url: B4.Url.action('ChangeInspState', 'BaseProsClaim'),
                        params: {
                            isFinal: true,
                            inspId: inspId
                        }
                    }).next(function (response) {
                        me.controller.unmask();
                        var obj = Ext.JSON.decode(response.responseText);

                        stateAspect.setStateData(obj.Id, obj.State);
                        model.load(inspId, {
                            success: function (rec) {
                                permAspect.setPermissionsByRecord(rec);
                            },
                            scope: this
                        });

                        return true;
                    }).error(function (e) {
                        me.controller.unmask();
                        Ext.Msg.alert('Ошибка!', e.message);
                    });
                }
            },
            onChangePerson: function (field, newValue) {
                var panel = this.getPanel(),
                    sfContragent = panel.down('#sfContragent'),
                    tfPhysicalPerson = panel.down('#tfPhysicalPerson'),
                    cbTypeJurPerson = panel.down('#cbTypeJurPerson');

                switch (newValue) {
                    case 10://физлицо
                        sfContragent.hide();
                        tfPhysicalPerson.show();
                        cbTypeJurPerson.hide();
                        break;
                    case 20://организация
                        sfContragent.show();
                        tfPhysicalPerson.hide();
                        cbTypeJurPerson.show();
                        break;
                    case 30://должностное лицо
                        sfContragent.show();
                        tfPhysicalPerson.show();
                        cbTypeJurPerson.show();
                        break;
                }
            },
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {

                    asp.controller.params = asp.controller.params || {};

                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }

                    asp.controller.params.contragentId = rec.data.Contragent ? rec.data.Contragent.Id : null;
                    asp.controller.params.dateInspection = rec.data.ProsClaimDateCheck;
                    asp.controller.params.typeJurOrg = rec.data.TypeJurPerson;

                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseProsClaim', {
                        inspectionId: asp.controller.params.inspectionId
                    })).next(function (response) {
                        asp.controller.unmask();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var fieldInspectors = panel.down('#prosClaimInspectorsTrigerField');
                        fieldInspectors.updateDisplayedText(obj.inspectorNames);
                        fieldInspectors.setValue(obj.inspectorIds);
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    //Обновляем статусы
                    this.controller.getAspect('baseProsClaimStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    //Обновляем кнопку Сформировать
                    this.controller.getAspect('baseProsClaimCreateButtonAspect').setData(rec.get('Id'));
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'prosClaimRealityObjectAspect',
            gridSelector: '#baseProsClaimRealityObjectGrid',
            storeName: 'baseprosclaim.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseProsClaimRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            onBeforeLoad: function (store, operation) {
                var panel = Ext.ComponentQuery.query(this.controller.baseProsClaimEditPanelSelector)[0];
                operation.params.contragentId = panel.down('#sfContragent').getValue();
                operation.params.date = panel.down('#dfProsClaimDateCheck').getValue();
                operation.params.typeJurPerson = panel.down('#cbTypeJurPerson').getValue();
                operation.params.isPhysicalPerson = panel.down('#cbPersonInspection').getValue() === 10 ? true : false;
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddRealityObjects', 'InspectionGjiRealityObject'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                inspectionId: asp.controller.params.inspectionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /DisposalGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseProsClaimInspectorMultiSelectWindowAspect',
            fieldSelector: '#baseProsClaimEditPanel #prosClaimInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseProsClaimInspectorsSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'InspectionGjiInspector'),
                        method: 'POST',
                        params: {
                            objectIds: Ext.encode(recordIds),
                            inspectionId: asp.controller.params.inspectionId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },

        {
            /*
            * обработка кнопки "Добавить квартиры" через отдельное окно
            */
            xtype: 'grideditwindowaspect',
            name: 'roomNumsEditWindowAspect',
            gridSelector: 'baseProsClaimRealityObjectGrid',
            editFormSelector: 'roomnumseditwindow',
            storeName: 'baseprosclaim.RealityObject',
            modelName: 'RealityObjectGji',
            editWindowView: 'RoomNumsEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.gridSelector + ' button[actionName=addRooms]'] = { 'click': { fn: me.addRoomNumsButtonHandler, scope: me } };
            },

            addRoomNumsButtonHandler: function () {
                var me = this,
                selectedArray = me.controller.getMainView().down('baseProsClaimRealityObjectGrid').selModel.getSelection();
                if (selectedArray.length > 0) {
                    me.editRecord(selectedArray[0]);
                }
            },
            listeners: {
                beforegridaction: function (asp, grid, action) {
                    //один грид юзают два аспекта, как результат две открытых формы вместо одной
                    if (action == 'add') {
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.getStore('baseprosclaim.RealityObject').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('baseProsClaimEditPanelAspect').setData(this.params.inspectionId);

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.params.title);

            //Обновляем список домов в проверке
            this.getStore('baseprosclaim.RealityObject').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.inspectionId = this.params.inspectionId;
    }
});