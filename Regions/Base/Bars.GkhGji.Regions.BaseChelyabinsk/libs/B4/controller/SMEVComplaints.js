Ext.define('B4.controller.SMEVComplaints', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhBlobText',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.enums.CourtPracticeState'
    ],

    models: ['complaints.SMEVComplaints',
        'complaints.SMEVComplaintsStep',
        'complaints.SMEVComplaintsRequest',
        'complaints.SMEVComplaintsExecutant',
        'complaints.ComplaintsFile'
    ],
    stores: ['complaints.SMEVComplaints',
        'complaints.SMEVComplaintsStep',
        'complaints.SMEVComplaintsRequest',
        'complaints.SMEVComplaintsExecutant',
        'complaints.ComplaintsFile'
    ],
    views: [
        'complaints.EditWindow',
        'complaints.Grid',
        'complaints.StepGrid',
        'complaints.StepEditWindow',
        'complaints.ExecutantGrid',
        'complaints.ExecutantEditWindow',
        'complaints.FileInfoGrid',
        'complaintsrequest.Grid'
    ],
    mainView: 'complaints.Grid',
    mainViewSelector: 'complaintsgrid',
    courtpracticeId: null,
    globalAppeal: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'complaintsgrid'
        },
        {
            ref: 'complaintsEditWindow',
            selector: 'complaintseditwindow'
        }
    ],

    aspects: [
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'complaintsgridStateTransferAspect',
            gridSelector: 'complaintsgrid',
            stateType: 'gji_smev_complaints',
            menuSelector: 'complaintsgridStateMenu'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'complaintsButtonExportAspect',
            gridSelector: 'complaintsgrid',
            buttonSelector: 'complaintsgrid #btnExport',
            controllerName: 'ComplaintsOperations',
            actionName: 'Export'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'complaintsStateButtonAspect',
            stateButtonSelector: '#complaintsEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var model = this.controller.getModel('complaints.SMEVComplaints');
                    model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('complaintsGridAspect').setFormData(rec);
                        },
                        scope: this
                    })


                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'pauseResolutionPetitionAspect',
            fieldSelector: '[name=PauseResolutionPetition]',
            editPanelAspectName: 'complaintsGridAspect',
            controllerName: 'SMEVComplaintsLT',
            valueFieldName: 'PauseResolutionPetition',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'renewTermPetitionAspect',
            fieldSelector: '[name=RenewTermPetition]',
            editPanelAspectName: 'complaintsGridAspect',
            controllerName: 'SMEVComplaintsLT',
            valueFieldName: 'RenewTermPetition',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsGridAspect',
            gridSelector: 'complaintsgrid',
            editFormSelector: '#complaintsEditWindow',
            storeName: 'complaints.SMEVComplaints',
            modelName: 'complaints.SMEVComplaints',
            editWindowView: 'complaints.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#complaintsEditWindow #sfSMEVComplaintsDecision'] = { 'beforeload': { fn: this.onBeforeLoadReason, scope: this } };
            },
            onBeforeLoadReason: function (field, options, store) {
                debugger;
                var tfLifeEvent = field.up('#complaintsEditWindow').down('#tfLifeEvent');
                if (tfLifeEvent) {
                    options = options || {};
                    options.params = options.params || {};
                    options.params.lsText = tfLifeEvent.getValue();
                }

            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    debugger;
                    asp.controller.courtpracticeId = record.getId();

                    if (asp.controller.courtpracticeId != 0) {
                        asp.controller.getAspect('complaintsStateButtonAspect').setStateData(asp.controller.courtpracticeId, record.get('State'));
                        var grid = form.down('complaintsexecutantgrid'),
                            store = grid.getStore();
                        store.on('beforeload',
                            function (store, operation) {
                                operation.params.complaintId = record.getId();
                            },
                            me);
                        grid.setDisabled(false)
                        store.load();
                        var gridreq = form.down('complaintsrequestgrid'),
                            reqstore = gridreq.getStore();
                        reqstore.on('beforeload',
                            function (store, operation) {
                                operation.params.complaintId = record.getId();
                            },
                            me);
                        gridreq.setDisabled(false)
                        reqstore.load();

                        var stepgrid = form.down('complaintsstepgrid'),
                            stepstore = stepgrid.getStore();
                        stepstore.on('beforeload',
                            function (store, operation) {
                                operation.params.complaintId = record.getId();
                            },
                            me);
                        stepgrid.setDisabled(false)
                        stepstore.load();

                        var filegrid = form.down('complaintsfileinfogrid'),
                            filestore = filegrid.getStore();
                        filestore.on('beforeload',
                            function (store, operation) {
                                operation.params.complaintId = record.getId();
                            },
                            me);
                        filegrid.setDisabled(false)
                        filestore.load();

                    }
                    this.controller.getAspect('pauseResolutionPetitionAspect').doInjection();
                    this.controller.getAspect('renewTermPetitionAspect').doInjection();

                }
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsexecutantgridAspect',
            gridSelector: 'complaintsexecutantgrid',
            editFormSelector: '#complaintsExecutantEditWindow',
            storeName: 'complaints.SMEVComplaintsExecutant',
            modelName: 'complaints.SMEVComplaintsExecutant',
            editWindowView: 'complaints.ExecutantEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('SMEVComplaints', asp.controller.courtpracticeId);
                    }
                }
            },

        },
        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsstepgridAspect',
            gridSelector: 'complaintsstepgrid',
            editFormSelector: '#complaintsStepEditWindow',
            storeName: 'complaints.SMEVComplaintsStep',
            modelName: 'complaints.SMEVComplaintsStep',
            editWindowView: 'complaints.StepEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('SMEVComplaints', asp.controller.courtpracticeId);
                    }
                }
            },

        }

    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('complaintsgrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('complaints.SMEVComplaints').load();
    },

    init: function () {
        var me = this;
        me.params = {};
        me.control({

            'complaintsstepgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } },
            '#complaintsEditWindow #sendCalculateButton': { click: { fn: this.getinfo, scope: this } },
        });

        this.getStore('complaints.SMEVComplaints').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },

    getinfo: function (btn) {
        var me = this;

        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        debugger;
        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVComplaintsLT'),
            params: {
                taskId: me.courtpracticeId,
                isGetInfo: true
            },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);
            me.unmask();
            return true;
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();
            return false;
        });
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('YesNo') != 20) {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVComplaintsLT'),
            params: {
                taskId: rec.getId(),
                isGetInfo: false
            },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);

            me.unmask();
            me.getStore('complaints.SMEVComplaintsStep').load();
            return true;
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();

            me.getStore('complaints.SMEVComplaintsStep').load();
            return false;
        });
    },

    onLaunch: function () {
        debugger;
        var grid = this.getMainView();
        if (this.params && this.params.recId > 0) {
            var model = this.getModel('complaints.SMEVComplaints');
            this.getAspect('complaintsGridAspect').editRecord(new model({ Id: this.params.recId }));
            this.params.recId = 0;
        }
    },

    onBeforeLoadDoc: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    }


});