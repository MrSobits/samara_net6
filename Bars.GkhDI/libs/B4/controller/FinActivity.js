Ext.define('B4.controller.FinActivity', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.finactivity.State'
    ],

    models: [
        'DisclosureInfo',
        'FinActivity',
        'finactivity.ManagRealityObj',
        'finactivity.RepairCategory',
        'finactivity.RepairSource',
        'finactivity.CommunalService',
        'finactivity.ManagCategory',
        'finactivity.DocByYear',
        'finactivity.Audit',
        'finactivity.Docs',
        'ManagingOrganization'
    ],

    stores: [
        'FinActivity',
        'finactivity.ManagRealityObj',
        'finactivity.RepairCategory',
        'finactivity.RepairSource',
        'finactivity.CommunalService',
        'finactivity.ManagCategory',
        'finactivity.DocByYear',
        'finactivity.Audit'
    ],

    views: [
        'finactivity.EditPanel',
        'finactivity.DocByYearEditWindow',
        'finactivity.AuditEditWindow'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'finactivity.EditPanel',
    mainViewSelector: '#finActivityEditPanel',

    aspects: [
        {
            xtype: 'finactstateperm',
            name: 'finActivityPermissionAspect'
        },
        {
            // Аспект документы
            xtype: 'gkheditpanel',
            name: 'finActivityDocsPanelAspect',
            editPanelSelector: '#tpDocsFinActivity',
            modelName: 'finactivity.Docs',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #saveDocsButton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
            },

            //перекрываем стандартную реализацию что бы сообщение из savesuccess выходило после сообщения успешного сохранения
            onPreSaveSuccess: function(asp, record) {
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                asp.fireEvent('savesuccess', asp, record);
            },

            listeners: {
                beforesave: function(asp, rec) {
                    if (Ext.isEmpty(rec.get("DisclosureInfo"))) {
                        rec.set("DisclosureInfo", asp.controller.params.disclosureInfoId);
                    }

                    return true;
                },
                savesuccess: function(asp, rec) {
                    var store;
                    var currentYear = new Date(asp.controller.params.disclosureInfo.PeriodDi.DateStart).getFullYear();
                    var panel = asp.getPanel();

                    asp.setPanelData(rec);

                    //Проверяем загружены ли документы в грид документов
                    if (asp.controller.params.disclosureInfo.TypeManagement != 10) {
                        var isEstimateCurrent, isEstimateLast, isRepEstimateLast, isConclusionCurrent, isConclusionLast, isConclusionLastTwo;
                        store = panel.down('#finActivityDocByYearGrid').getStore();
                        if (store) {
                            store.each(function(record) {
                                if (!record.phantom) {
                                    if (record.get('File')) {
                                        if (record.get('TypeDocByYearDi') == 10) {
                                            if (currentYear == record.get('Year')) {
                                                isEstimateCurrent = true;
                                            } else if (currentYear == record.get('Year') + 1) {
                                                isEstimateLast = true;
                                            }
                                        } else if (record.get('TypeDocByYearDi') == 20) {
                                            if (currentYear == record.get('Year')) {
                                                isConclusionCurrent = true;
                                            } else if (currentYear == record.get('Year') + 1) {
                                                isConclusionLast = true;
                                            } else if (currentYear == record.get('Year') + 2) {
                                                isConclusionLastTwo = true;
                                            }
                                        } else if (record.get('TypeDocByYearDi') == 30) {
                                            if (currentYear == record.get('Year') + 1) {
                                                isRepEstimateLast = true;
                                            }
                                        }
                                    }
                                }
                            });
                        }
                        if (!(isEstimateCurrent && isEstimateLast && isRepEstimateLast && isConclusionCurrent && isConclusionLast && isConclusionLastTwo)) {
                            Ext.Msg.alert('Сохранение', 'Загружены не все документы');
                        }
                    }
                },
                beforesetpaneldata: function(asp, rec, panel) {
                    var typeManagement = asp.controller.params.disclosureInfo.TypeManagement;
                    var grid = panel.down('#finActivityDocByYearGrid');
                    var gridNotation = panel.down('#cmpGridNotation');
                    //грид видят только ТСЖ и ЖСК
                    if (typeManagement == 10) {
                        grid.hide();
                        gridNotation.hide();
                    } else {
                        grid.show();
                        gridNotation.show();
                    }
                }
            }
        },
        {
            // Аспект общие сведения
            xtype: 'gkheditpanel',
            name: 'finActivityEditPanelAspect',
            editPanelSelector: '#tpGeneralFinActivity',
            modelName: 'FinActivity',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #saveGeneralDataButton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['#finActivityEditPanel #saveRepairButton'] = { 'click': { fn: this.saveRepairGrids, scope: this } };
                actions['#finActivityEditPanel #addRepairDataRealObjButton'] = { 'click': { fn: this.btnAddDataRealObjClick, scope: this } };
            },
            //перекрываем стандартную реализацию что бы сообщение из savesuccess выходило после сообщения успешного сохранения
            onPreSaveSuccess: function(asp, record) {
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                asp.fireEvent('savesuccess', asp, record);
            },

            btnAddDataRealObjClick: function() {
                this.addDataByRealityObjSource();
                this.addDataByRealityObjCategory();
            },

            addDataByRealityObjSource: function() {
                var asp = this;
                var store = asp.controller.getStore('finactivity.RepairSource');
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('AddDataByRealityObj', 'FinActivityRepairSource'),
                    params: {
                        disclosureInfoId: asp.controller.params.disclosureInfoId
                    }
                }).next(function() {
                    store.load();
                    asp.controller.unmask();
                }).error(function() {
                    asp.controller.unmask();
                });
            },

            addDataByRealityObjCategory: function() {
                var asp = this,
                    store = asp.controller.getStore('finactivity.RepairCategory');
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(
                    {
                        method: 'POST',
                        url: B4.Url.action('AddDataByRealityObj', 'FinActivityRepairCategory'),
                        params: {
                            disclosureInfoId: asp.controller.params.disclosureInfoId
                        }
                    }).next(function() {
                        store.load();
                        asp.controller.unmask();
                    }).error(function() {
                        asp.controller.unmask();
                    });
            },

            listeners: {
                beforesave: function(asp, rec) {
                    if (Ext.isEmpty(rec.get("DisclosureInfo"))) {
                        rec.set("DisclosureInfo", asp.controller.params.disclosureInfoId);
                    }

                    return true;
                },
                savesuccess: function(asp) {
                    var store;
                    var currentYear = new Date(asp.controller.params.disclosureInfo.PeriodDi.DateStart).getFullYear();
                    var panel = asp.getPanel();

                    //Проверяем загружены ли документы в грид аудиторских проверок                
                    var isCurrent, isLast, isLastTwo;
                    store = panel.down('#finActivityAuditGrid').getStore();
                    if (store) {
                        store.each(function(record) {
                            if (!record.phantom) {
                                if ((record.get('File') && record.get('TypeAuditStateDi') != 30) || record.get('TypeAuditStateDi') == 30) {
                                    if (record.get('Year') == currentYear) {
                                        isCurrent = true;
                                    } else if (record.get('Year') == currentYear - 1) {
                                        isLast = true;
                                    } else if (record.get('Year') == currentYear - 2) {
                                        isLastTwo = true;
                                    }
                                }
                            }
                        });
                    }
                    if (!isCurrent) {
                        Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок за отчетный год');
                    } else if (!isLast) {
                        Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок за год, предшествующий отчетному году');
                    } else if (!isLastTwo) {
                        Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок за 2 года, предшествующих отчетному году');
                    }
                }
            },

            saveRepairGrids: function() {
                this.controller.getAspect('finActivityRepairCategoryGridAspect').save();
                this.controller.getAspect('finActivityRepairSourceGridAspect').save();
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'finActivityManagRealityObjGridAspect',
            gridSelector: '#finActivityManagRealityObjGrid',
            storeName: 'finactivity.ManagRealityObj',
            modelName: 'finactivity.ManagRealityObj',
            saveButtonSelector: '#finActivityManagRealityObjGrid #saveManagRealityObjButton',

            otherActions: function(actions) {
            },

            save: function() {
                var store = this.controller.getStore(this.storeName);

                if (this.fireEvent('beforesave', this, store) !== false) {
                    var modifyRecordArray = store.getModifiedRecords();
                    var dataArray = [];
                    var asp = this;
                    Ext.Array.each(modifyRecordArray, function(value) {
                        dataArray.push(
                            {
                                DisclosureInfo: { Id: asp.controller.params.disclosureInfoId },
                                RealityObject: { Id: value.get('Id') },
                                Id: value.get('ObjectId'),
                                PresentedToRepay: value.get('PresentedToRepay'),
                                ReceivedProvidedService: value.get('ReceivedProvidedService'),
                                SumDebt: value.get('SumDebt'),
                                SumFactExpense: value.get('SumFactExpense'),
                                SumIncomeManage: value.get('SumIncomeManage')
                            });
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(
                        {
                            method: 'POST',
                            url: B4.Url.action('SaveManagRealityObj', 'FinActivityManagRealityObj'),
                            params: {
                                jsonString: Ext.JSON.encode(dataArray),
                                disclosureInfoId: asp.controller.params.disclosureInfoId
                            }
                        }).next(function() {
                            store.load();
                            asp.controller.unmask();
                            B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                        }).error(function() {
                            asp.controller.unmask();
                        });
                }
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'finActivityRepairCategoryGridAspect',
            gridSelector: '#finActivityRepairCategoryGrid',
            storeName: 'finactivity.RepairCategory',
            modelName: 'finactivity.RepairCategory',

            save: function() {
                var asp = this,
                    store = asp.controller.getStore(asp.storeName),
                    modifRecords = store.getModifiedRecords(),
                    records = [];

                Ext.Array.each(modifRecords, function(rec) {

                    rec.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                    rec.set('Id', 0);
                    records.push(rec.data);
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddWorkMode', 'FinActivityRepairCategory'),
                        params: {
                            records: Ext.JSON.encode(records),
                            disclosureInfoId: asp.controller.params.disclosureInfoId
                        }
                    }).next(function() {
                        asp.controller.unmask();
                        asp.updateGrid();
                        //B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'finActivityRepairSourceGridAspect',
            gridSelector: '#finActivityRepairSourceGrid',
            storeName: 'finactivity.RepairSource',
            modelName: 'finactivity.RepairSource',

            save: function() {
                var asp = this,
                    store = asp.controller.getStore(asp.storeName),
                    modifRecords = store.getModifiedRecords(),
                    records = [];

                Ext.Array.each(modifRecords, function(rec) {

                    rec.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                    rec.set('Id', 0);
                    records.push(rec.data);
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('AddWorkMode', 'FinActivityRepairSource'),
                    params: {
                        records: Ext.JSON.encode(records),
                        disclosureInfoId: asp.controller.params.disclosureInfoId
                    }
                }).next(function() {
                    asp.controller.unmask();
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                    return true;
                }).error(function() {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'finActivityCommunalServiceGridAspect',
            gridSelector: '#finActivityCommunalServiceGrid',
            storeName: 'finactivity.CommunalService',
            modelName: 'finactivity.CommunalService',
            saveButtonSelector: '#finActivityCommunalServiceGrid #saveCommunalServiceButton',

            otherActions: function(actions) {
                actions['#finActivityEditPanel #addDataRealObjButton'] = { 'click': { fn: this.btnAddDataRealObjClick, scope: this } };
            },

            save: function() {
                var asp = this,
                    store = asp.controller.getStore(asp.storeName),
                    modifRecords = store.getModifiedRecords(),
                    records = [];

                Ext.Array.each(modifRecords, function(rec) {
                    rec.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                    rec.set('Id', 0);
                    records.push(rec.data);
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('AddWorkMode', 'FinActivityCommunalService'),
                    params: {
                        records: Ext.JSON.encode(records),
                        disclosureInfoId: asp.controller.params.disclosureInfoId
                    }
                }).next(function() {
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                    asp.controller.unmask();
                    return true;
                }).error(function() {
                    asp.controller.unmask();
                });
            },

            btnAddDataRealObjClick: function() {
                var asp = this,
                    store = asp.controller.getStore(asp.storeName);
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('AddDataByRealityObj', 'FinActivityRealityObjCommunalService'),
                    params: {
                        disclosureInfoId: asp.controller.params.disclosureInfoId
                    }
                }).next(function() {
                    store.load();
                    asp.controller.unmask();
                }).error(function() {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'finActivityManagCategoryGridAspect',
            gridSelector: '#finActivityManagCategoryGrid',
            storeName: 'finactivity.ManagCategory',
            modelName: 'finactivity.ManagCategory',
            saveButtonSelector: '#finActivityManagCategoryGrid #saveManagCategoryButton',

            save: function() {
                var me = this,
                    store = me.controller.getStore(me.storeName),
                    modifRecords = store.getModifiedRecords(),
                    records = [];
                
                Ext.Array.each(modifRecords, function(rec) {
                    rec.set('DisclosureInfo', me.controller.params.disclosureInfoId);
                    rec.set('Id', 0);
                    records.push(rec.data);
                });

                me.controller.mask('Сохранение', me.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('AddWorkMode', 'FinActivityManagCategory'),
                    params: {
                        records: Ext.JSON.encode(records),
                        disclosureInfoId: me.controller.params.disclosureInfoId
                    }
                }).next(function() {
                    me.controller.unmask();
                    me.updateGrid();
                    B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                    return true;
                }).error(function() {
                    me.controller.unmask();
                });
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'finActivityDocByYearGridWindowAspect',
            gridSelector: '#finActivityDocByYearGrid',
            editFormSelector: '#finActivityDocByYearEditWindow',
            storeName: 'finactivity.DocByYear',
            modelName: 'finactivity.DocByYear',
            editWindowView: 'finactivity.DocByYearEditWindow',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' #cbTypeDocByYearDi'] = {
                    change: {
                        fn: this.onChangeTypeDocByYearDi,
                        scope: this
                    }
                };
            },
            listeners: {
                aftersetformdata: function(asp, rec) {

                    //получаем record по id => по record модель => по модели упр орг
                    var model = asp.controller.getModel('DisclosureInfo');
                    model.load(asp.controller.params.disclosureInfoId, {
                        success: function(record) {
                            if (Ext.isEmpty(record.get('ManagingOrganization'))) {
                                rec.set("ManagingOrganization", record.get('ManagingOrgId'));
                            } else {
                                rec.set("ManagingOrganization", record.get('ManagingOrgId'));
                            }
                        },
                        scope: this
                    });
                },
                beforesave: function(asp, rec) {
                    if (rec.get('ManagingOrganization')) {
                        return true;
                    }

                    Ext.Msg.alert('Ошибка сохранения', 'Попробуйте заново');
                    asp.getForm().hide();
                    return false;
                }
            },

            onChangeTypeDocByYearDi: function(sender, value) {
                var dfDocumentDate = sender.up().down('#dfDocumentDate');

                if (dfDocumentDate && dfDocumentDate.allowBlank !== undefined) {
                    dfDocumentDate.allowBlank = value != 20;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'finActivityAuditGridWindowAspect',
            gridSelector: '#finActivityAuditGrid',
            editFormSelector: '#finActivityAuditEditWindow',
            storeName: 'finactivity.Audit',
            modelName: 'finactivity.Audit',
            editWindowView: 'finactivity.AuditEditWindow',
            listeners: {
                beforesaverequest: function(asp) {
                    var form = asp.getForm();
                    if (Ext.isEmpty(form.down('#ffFile').value) && form.down('#cbTypeAuditStateDi').getValue() != 30) {
                        var year = new Date(asp.controller.params.disclosureInfo.PeriodDi.DateStart).getFullYear();
                        var formYear = form.down('#nfYear').getValue();
                        if (year == formYear) {
                            Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок за отчетный год');
                        } else if (year == formYear + 1) {
                            Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок за год, предшествующий отчетному году');
                        } else if (year == formYear + 2) {
                            Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок за 2 года, предшествующих отчетному году');
                        } else {
                            Ext.Msg.alert('Сохранение', 'Необходимо загрузить документы аудиторских проверок или указать что проверка не проведена');
                        }
                        return false;
                    }
                    return true;
                },
                beforesetformdata: function(asp, rec) {
                    if (Ext.isEmpty(rec.get('Year')) && !Ext.isEmpty(asp.controller.params) && !Ext.isEmpty(asp.controller.params.recDi) && !Ext.isEmpty(asp.controller.params.recDi.PeriodDi) && !Ext.isEmpty(asp.controller.params.recDi.PeriodDi.DateStart)) {
                        rec.set('Year', new Date(asp.controller.params.recDi.PeriodDi.DateStart).getFullYear());
                    }
                },
                aftersetformdata: function(asp, rec, form) {
                    //получаем record по id => по record модель => по модели упр орг
                    var model = asp.controller.getModel('DisclosureInfo');
                    model.load(asp.controller.params.disclosureInfoId, {
                        success: function(record) {
                            rec.set('ManagingOrganization', record.get('ManagingOrgId'));
                        },
                        scope: this
                    });
                },
                beforesave: function(asp, rec) {
                    if (rec.get('ManagingOrganization')) {
                        return true;
                    }

                    Ext.Msg.alert('Ошибка сохранения', 'Попробуйте заново');
                    asp.getForm().hide();
                    return false;
                }
            }
        }],

    init: function() {
        this.getStore('finactivity.ManagRealityObj').on('beforeload', this.onBeforeLoad, this);

        this.getStore('finactivity.RepairCategory').on('beforeload', this.onBeforeLoad, this);
        
        this.getStore('finactivity.RepairSource').on('beforeload', this.onBeforeLoad, this);

        this.getStore('finactivity.CommunalService').on('beforeload', this.onBeforeLoad, this);

        this.getStore('finactivity.ManagCategory').on('beforeload', this.onBeforeLoad, this);

        this.getStore('finactivity.DocByYear').on('beforeload', this.onBeforeLoad, this);

        this.getStore('finactivity.Audit').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        if (this.params) {
            this.params.Id = this.setDataFinActivity(this.params.disclosureInfoId);

            var me = this;
            me.params.getId = function() { return me.params.disclosureInfoId; };
            this.getAspect('finActivityPermissionAspect').setPermissionsByRecord(this.params);
        }

        this.getStore('finactivity.ManagRealityObj').load();

        this.getStore('finactivity.RepairCategory').load();

        this.getStore('finactivity.RepairSource').load();

        this.getStore('finactivity.CommunalService').load();

        this.getStore('finactivity.ManagCategory').load();

        this.getStore('finactivity.DocByYear').load();

        this.getStore('finactivity.Audit').load();
    },

    //Перед методом setdata получаем id finactivity по disclosureInfoId
    setDataFinActivity: function(disclosureInfoId) {

        var asp = this.getAspect('finActivityEditPanelAspect');
        var aspDocs = this.getAspect('finActivityDocsPanelAspect');

        var mainView = this.getMainComponent();

        if (mainView) {

            this.mask('Загрузка', mainView);
            B4.Ajax.request(B4.Url.action('GetIdByDisnfoId', 'FinActivity', {
                disclosureInfoId: disclosureInfoId
            })).next(function(response) {
                this.unmask();
                //десериализуем полученную строку
                var obj = Ext.JSON.decode(response.responseText);
                asp.controller.params.Id = obj.Id;
                asp.controller.params.disclosureInfo = obj.disclosureInfo;
                asp.setData(asp.controller.params.Id);
            }, this).error(function() {
                asp.setData(0);
                this.unmask();
            }, this);

            this.mask('Загрузка', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetIdByDisnfoId', 'FinActivityDocs', {
                disclosureInfoId: disclosureInfoId
            })).next(function(response) {
                this.unmask();
                //десериализуем полученную строку
                var obj = Ext.JSON.decode(response.responseText);
                aspDocs.controller.params.Id = obj.Id;
                aspDocs.controller.params.disclosureInfo = obj.disclosureInfo;
                aspDocs.setData(aspDocs.controller.params.Id);
            }, this).error(function() {
                aspDocs.setData(0);
                this.unmask();
            }, this);

        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    }
});