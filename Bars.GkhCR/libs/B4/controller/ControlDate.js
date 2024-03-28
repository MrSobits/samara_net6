Ext.define('B4.controller.ControlDate', {
    extend: 'B4.base.Controller',
    requires:
    [
       'B4.aspects.GridEditWindow',
       'B4.aspects.GkhGridMultiSelectWindow',
       'B4.aspects.permission.GkhPermissionAspect',
       'B4.aspects.permission.controldate.MunicipalityLimitDate'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['dict.ProgramCr', 'ControlDate', 'controldate.StageWork', 'dict.StageWorkCr', 'controldate.MunicipalityLimitDate'],
    stores: ['dict.ProgramCr', 'ControlDate', 'controldate.StageWork', 'dict.StageWorkCrSelect',
        'dict.StageWorkCrSelected', 'dict.WorkSelect', 'dict.WorkSelected', 'controldate.MunicipalityLimitDate'],
    views: ['controldate.Grid',
            'controldate.EditWindow',
             'controldate.WorkGrid',
             'controldate.WorkEditWindow',
             'controldate.StageWorkGrid',
             'SelectWindow.MultiSelectWindow',
             'controldate.MunicipalityLimitDateGrid',
             'controldate.MunicipalityLimitDateEditWindow'],

    mainView: 'controldate.Grid',
    mainViewSelector: 'controlDateGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'controlDateGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.ControlDate.Create', applyTo: 'b4addbutton', selector: '#controlDateWorkGrid' },
                { name: 'GkhCr.ControlDate.Edit', applyTo: 'b4savebutton', selector: '#controlDateWorkEditWindow' },
                {
                    name: 'GkhCr.ControlDate.Delete', applyTo: 'b4deletecolumn', selector: '#controlDateWorkGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'GkhCr.ControlDate.StageWork.Create', applyTo: 'b4addbutton', selector: '#controlDateStageWorkGrid' },
                { name: 'GkhCr.ControlDate.Field.Date', applyTo: '#dfDate', selector: '#controlDateWorkEditWindow' },
                {
                    name: 'GkhCr.ControlDate.StageWork.Delete', applyTo: 'b4deletecolumn', selector: '#controlDateStageWorkGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhCr.ControlDate.MunicipalityLimitDate.View',
                    applyTo: 'tabpanel',
                    selector: '#controlDateWorkEditWindow',
                    applyBy: function (component, allowed) {
                        var tab = component.down('controldatemunicipalitylimitdategrid');
                        if (allowed) {
                            component.showTab(tab);
                        } else {
                            component.hideTab(tab);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'municipalitylimitdateperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'controlDateGridWindowAspect',
            gridSelector: 'controlDateGrid',
            editFormSelector: '#controlDateEditWindow',
            storeName: 'dict.ProgramCr',
            modelName: 'dict.ProgramCr',
            editWindowView: 'controldate.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.programCrId = record.getId();
                    asp.controller.getStore('ControlDate').load();
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'controlDateWorkAspect',
            gridSelector: '#controlDateWorkGrid',
            storeName: 'ControlDate',
            modelName: 'ControlDate',
            editFormSelector: '#controlDateWorkEditWindow',
            editWindowView: 'controldate.WorkEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#controlDateWorkMultiSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            titleSelectWindow: 'Выбор видов работ',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddWorks', 'ControlDate', {
                            objectIds: recordIds,
                            programCrId: asp.controller.programCrId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды работ');
                        return false;
                    }
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.controlDateId = record.getId();
                    asp.controller.getStore('controldate.StageWork').load();
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'ctrlDateStageWorkGridAspect',
            gridSelector: '#controlDateStageWorkGrid',
            storeName: 'controldate.StageWork',
            modelName: 'controldate.StageWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#ctrlDateStageWorkMultiSelectWindow',
            storeSelect: 'dict.StageWorkCrSelect',
            storeSelected: 'dict.StageWorkCrSelected',
            titleSelectWindow: 'Выбор этапов работ',
            titleGridSelect: 'Этапы',
            titleGridSelected: 'Выбранные этапы работ',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
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
                            url: B4.Url.action('AddStageWorks', 'ControlDate'),
                            method: 'POST',
                            params: {
                                objectIds: recordIds,
                                controlDateId: asp.controller.controlDateId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать этапы работ');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'controlDateMunicipalityLimitDateGridWindowAspect',
            gridSelector: 'controldatemunicipalitylimitdategrid',
            editFormSelector: 'controldatemunicipalitylimitdateeditwindow',
            modelName: 'controldate.MunicipalityLimitDate',
            editWindowView: 'controldate.MunicipalityLimitDateEditWindow',
            listeners: {
                beforesave: function (asp, record) {
                    record.set('ControlDate', asp.controller.controlDateId);
                }
            }
        }
    ],

    init: function () {
        this.getStore('ControlDate').on('beforeload', this.onBeforeLoad, this, 'Rec');
        this.getStore('controldate.StageWork').on('beforeload', this.onBeforeLoadStageWork, this, 'Rec');
        this.control({
            'controldatemunicipalitylimitdategrid': { 'store.beforeload': { fn: this.onBeforeLoadStageWork, scope: this } },
            'tabpanel': { 'tabchange': { fn: this.changeTab, scope: this } },
        });
        this.callParent(arguments);
    },

    changeTab: function (tabPanel, newTab, oldTab) {
        newTab.getStore().load();
    },

    onBeforeLoadStageWork: function (store, operation) {
        operation.params.controlDateId = this.controlDateId;
    },

    onBeforeLoad: function (store, operation) {
        operation.params.programCrId = this.programCrId;
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('controlDateGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProgramCr').load();
    }
});