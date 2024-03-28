Ext.define('B4.controller.AdminResp', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.permission.adminresp.State',
        'B4.aspects.GkhInlineGrid'
    ],

    models: [
        'DisclosureInfo',
        'AdminResp',
        'adminresp.Actions'
    ],

    stores: [
        'AdminResp',
        'adminresp.ResolutionGjiForSelect',
        'adminresp.ResolutionGjiForSelected',
        'adminresp.Actions'
    ],

    views: [
        'adminresp.EditPanel',
        'adminresp.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    adminRespEditWindowSelector: '#adminRespEditWindow',

    mainView: 'adminresp.EditPanel',
    mainViewSelector: '#adminRespEditPanel',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'adminrespstateperm',
            editFormAspectName: 'adminRespEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'addResolutionAdminRespGridAspect',
            buttonSelector: '#adminRespEditPanel #loadGjiResolutionButton',
            storeName: 'AdminResp',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#adminRespMultiSelectWindow',
            storeSelect: 'adminresp.ResolutionGjiForSelect',
            storeSelected: 'adminresp.ResolutionGjiForSelected',
            titleSelectWindow: 'Выбор постановлений ГЖИ',
            titleGridSelect: 'Постановления ГЖИ для отбора',
            titleGridSelected: 'Выбранные постановления',
            columnsGridSelect: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Дата документа', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DocumentDate', flex: 1, 
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, sortable:false },
                { header: 'Дата документа', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DocumentDate', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.contragentId = this.controller.params.recDi.ManagingOrganization.Contragent;
                operation.params.periodDiDateStart = this.controller.params.recDi.PeriodDi.DateStart;
                operation.params.periodDiDateEnd = this.controller.params.recDi.PeriodDi.DateEnd;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('/Resolution/GetResolutionInfo'),
                            params: {
                                objectIds: Ext.encode(recordIds)
                            }
                        }).next(function(response) {
                            B4.Ajax.request({
                                url: B4.Url.action('/AdminResp/AddAdminRespByResolution'),
                                params: {
                                    diInfoId: asp.controller.params.disclosureInfoId,
                                    resolutions: response.responseText
                                }
                            }).next(function() {
                                asp.controller.getStore(asp.storeName).load();
                                asp.controller.unmask();
                                return true;
                            }).error(function() {
                                Ext.Msg.alert('Ошибка!', 'Ошибка сохранения');
                                asp.controller.unmask();
                            });
                        }).error(function() {
                            Ext.Msg.alert('Ошибка!', 'Не удалось получить постановления');
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать постановления');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'adminRespEditPanelAspect',
            modelName: 'DisclosureInfo',
            editPanelSelector: '#adminRespEditPanel',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' b4addbutton'] = { 'click': { fn: this.onAddBtnClick, scope: this} };
                actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this} };
                actions[this.editPanelSelector + ' #cbAdminResponsibility'] = { 'change': { fn: this.changeAdminResponsibility, scope: this } };
            },
            onAddBtnClick: function () {
                this.controller.getAspect('adminRespGridWindowAspect').editRecord();
            },
            onUpdateBtnClick: function () {
                this.setData(this.controller.params.disclosureInfoId);
                if (Ext.ComponentQuery.query('#cbAdminResponsibility')[0].value == 10) {
                    this.controller.getAspect('adminRespGridWindowAspect').updateGrid();
                }
            },

            changeAdminResponsibility: function (field, newValue, oldValue) {
                //При первом заходе не сохраняем
                if (oldValue) {
                    this.saveRequestHandler();
                }

                this.setDisableGrid(field);
            },

            setDisableGrid: function (field) {
                var grid = Ext.ComponentQuery.query('#adminRespGrid')[0],
                    addAdminRespButton = Ext.ComponentQuery.query('#addAdminRespButton')[0],
                    loadGjiResolutionButton = Ext.ComponentQuery.query('#loadGjiResolutionButton')[0];

                if (field.getValue() != 10) {
                    grid.setDisabled(true);
                    grid.getStore().removeAll();
                    addAdminRespButton.hide();
                    loadGjiResolutionButton.hide();
                }
                else {
                    grid.setDisabled(false);
                    grid.getStore().load();
                    addAdminRespButton.show();
                    loadGjiResolutionButton.show();
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'adminRespGridWindowAspect',
            gridSelector: '#adminRespGrid',
            editFormSelector: '#adminRespEditWindow',
            storeName: 'AdminResp',
            modelName: 'AdminResp',
            editWindowView: 'adminresp.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' combobox[name=TypePerson]'] = { 'change': { fn: this.onTypePersonChange, scope: this } };
            },
            onTypePersonChange: function (cb, nv) {
                var me = this,
                    wnd = me.getForm(),
                    fldFio = wnd.down('[name=Fio]'),
                    fldPosition = wnd.down('[name=Position]');

                if (nv != 20) {
                    fldFio.hide();
                    fldPosition.hide();
                }
                else {
                    fldFio.show();
                    fldPosition.show();
                }
            },
            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    var discInfo = asp.controller.getAspect('adminRespEditPanelAspect').getRecord(),
                        wnd = asp.getForm(),
                        periodDi = discInfo.get('PeriodDi'),
                        fieldInspectors = wnd.down('#dateImpositionPenalty');

                    asp.controller.setCurrentId(record.getId());

                    if (!Ext.isEmpty(periodDi.DateEnd)) {
                        fieldInspectors.maxValue = new Date(periodDi.DateEnd);
                    }
                    if (!Ext.isEmpty(periodDi.DateStart)) {
                        fieldInspectors.minValue = new Date(periodDi.DateStart);
                    }
                },
                validate: function (asp) {
                    var fileFld = asp.getForm().down('[name=File]');

                    if (fileFld.isFileLoad() && !fileFld.isFileExtensionOK()) {
                        Ext.Msg.alert('Внимание', 'Необходимо выбрать файл с допустимым расширением: ' + fileFld.possibleFileExtensions);
                        return false;
                    }

                    if (!Ext.isEmpty(fileFld.maxFileSize) && fileFld.isFileLoad() && fileFld.getSize() > fileFld.maxFileSize) {
                        Ext.Msg.alert('Внимание', 'Необходимо выбрать файл допустимого размера (не более 15 МБ)');
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'adminRespActionsGridAspect',
            gridSelector: '#adminRespActionsGrid',
            storeName: 'adminresp.Actions',
            modelName: 'adminresp.Actions',
            saveButtonSelector: '#adminRespActionsGrid #adminRespActionsSaveButton',
            listeners: {
                beforeaddrecord: function (asp, record) {
                    record.set('AdminResp', this.controller.adminRespId);
                }
            }
        }
    ],

    init: function () {
        this.getStore('AdminResp').on('beforeload', this.onBeforeLoad, this);

        this.getStore('adminresp.Actions').on('beforeload', this.onBeforeLoadActions, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('adminRespEditPanelAspect').setData(this.params.disclosureInfoId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    },
    
    onBeforeLoadActions: function (store, operation) {
        if (this.params) {
            operation.params.adminRespId = this.adminRespId;
        }
    },
    
    setCurrentId: function (id) {
        this.adminRespId = id;

        var editWindow = Ext.ComponentQuery.query(this.adminRespEditWindowSelector)[0];

        var store = this.getStore('adminresp.Actions');
        store.removeAll();

        if (id > 0) {
            editWindow.down('#adminRespActionsGrid').setDisabled(false);
            store.load();
        } else {
            editWindow.down('#adminRespActionsGrid').setDisabled(true);
        }
    }
});
