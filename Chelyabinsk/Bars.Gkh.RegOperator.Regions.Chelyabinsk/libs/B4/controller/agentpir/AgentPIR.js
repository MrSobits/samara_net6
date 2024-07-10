Ext.define('B4.controller.agentpir.AgentPIR', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url'
    ],

    contagentINN: null,
    agentPIRId: null,
    periodId: null,
    agentPIRDebtorId: null,
    extractId: null,

    models: [
        'AgentPIR',
        'AgentPIRDebtor',
        'DebtorPayment',
        'AgentPIRDocument',
        'DebtorReferenceCalculation',
        'AgentPIRDebtorCredit'
    ],
    stores: [
        'AgentPIR',
        'AgentPIRDebtor',
        'ListPersonalAccountDebtorForSelected',
        'ListPersonalAccountDebtor',
        'DebtorPayment',
        'AgentPIRDocument',
        'DebtorReferenceCalculation',
        'AgentPIRDebtorCredit'
    ],
    views: [

        'agentpir.EditWindow',
        'agentpir.Grid',
        'agentpir.DebtorGrid',
        'agentpir.DebtorEditWindow',
        'agentpir.PaymentGrid',
        'agentpir.DocumentEditWindow',
        'agentpir.DocumentGrid',
        'agentpir.ReferenceCalculationGrid',
        'agentpir.CreditGrid'

    ],

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'agentPIRDebtorPrintAspect',
            buttonSelector: '#agentPIRDebtorEditWindow #btnPrint',
            codeForm: 'AgentPirDebtor',
            getUserParams: function () {
                
                var param = { Id: this.controller.agentPIRDebtorId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'agentPIRPrintAspect',
            buttonSelector: '#agentPIREditWindow #btnPrint',
            codeForm: 'AgentPir',
            onMenuItemClick: function (itemMenu) {
                var me = this;
                var btn = itemMenu.up('#btnPrint'),
                    win = btn.up('#agentPIREditWindow');
                me.controller.mask('Формирую отчет...', win);
                if (itemMenu.actionName == "AgentPirOperationsByPeriodReport") {
                    if (!this.controller.periodId) {
                        me.controller.unmask();
                        Ext.Msg.alert("Внимание!", "Для получения операций за период необходимо выбрать период!")
                    }
                    else
                    this.printReport(itemMenu);
                }
                else
                    this.printReport(itemMenu);
                me.controller.unmask();
            },
            getUserParams: function () {
                var param = { Id: this.controller.agentPIRId, PeriodId: this.controller.periodId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'agentPIRGridAspect',
            gridSelector: 'agentpirGrid',
            editFormSelector: '#agentPIREditWindow',
            storeName: 'AgentPIR',
            modelName: 'AgentPIR',
            editWindowView: 'agentpir.EditWindow',
            onSaveSuccess: function () {                
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             
            },
            otherActions: function (actions) {

                actions['#agentPIREditWindow b4selectfield[name=ChargePeriod]'] = { 'change': { fn: this.setPeriodId, scope: this } };
              
            }, 
            setPeriodId: function (field, newValue, oldValue) {
                
                this.controller.periodId = newValue.Id;
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    asp.controller.agentPIRId = record.getId();
                    var grid = form.down('agentpirdebtorGrid'),
                    store = grid.getStore();
                    store.filter('agentPIRId', asp.controller.agentPIRId);
                    me.controller.getAspect('agentPIRPrintAspect').loadReportStore();
                }
               
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'agentPIRDebtorGridMultiSelectAspect',
            gridSelector: 'agentpirdebtorGrid',
            storeName: 'AgentPIRDebtor',
            modelName: 'AgentPIRDebtor',
            editFormSelector: '#agentPIRDebtorEditWindow',
            editWindowView: 'agentpir.DebtorEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#agentpirdebtorMultiSelectWindow',
            storeSelect: 'ListPersonalAccountDebtor',
            storeSelected: 'ListPersonalAccountDebtorForSelected',
            titleSelectWindow: 'Выбор лицевых счетов',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Абонент', xtype: 'gridcolumn', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Номер лс', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Абонент', xtype: 'gridcolumn', dataIndex: 'AccountOwner', flex: 1, sortable: false }
            ],
            otherActions: function (actions) {
             
                actions['#agentPIRDebtorEditWindow #cbUseCustomDate'] = { 'change': { fn: this.onChangeUseCustomDate, scope: this } };
                actions['#agentPIRDebtorEditWindow button[action=DebtStartCalculate]'] = { 'click': { fn: this.calculateDebtStartDate, scope: this } };
              
            },
            onChangeUseCustomDate: function (field, newValue) {
                var form = field.up('#agentPIRDebtorEditWindow'),
                    dfCustomDate = form.down('#dfCustomDate');

                if (newValue == true) {
                    dfCustomDate.show();
                }
                else {
                    dfCustomDate.hide();
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this;

                    var recordIds = [];

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddPersonalAccountDebtor', 'AgentPIRExecute', {
                            partIds: recordIds,
                            agentPIRId: asp.controller.agentPIRId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать лс');
                        return false;
                    }
                    return true;
                },

                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    asp.controller.agentPIRDebtorId = record.getId();
                    
                    asp.controller.extractId = record.get('BasePersonalAccountId');

                    var paygrid = form.down('paymentGrid'),
                        store = paygrid.getStore();
                    store.filter('agentPIRDebtorId', record.getId());

                    var docgrid = form.down('agentpirdebtordocumentGrid'),
                        store = docgrid.getStore();
                    store.filter('agentPIRDebtorId', record.getId());

                    var credgrid = form.down('agentpirdebtorcreditgrid'),
                        store = credgrid.getStore();
                    store.filter('agentPIRDebtorId', record.getId());
                    
                    var refgrid = form.down('agentpirreferencecalculationgrid');
                    refgrid.getStore().on('beforeload', function (store, operation) {
                        operation.params.debtorId = record.getId();
                    });
                    refgrid.getStore().load();
                    me.controller.getAspect('agentPIRDebtorPrintAspect').loadReportStore();
                }
            },
            calculateDebtStartDate: function (btn) {
                var me = this;
                var form = me.getForm();
                
                //  var rec = view.getRecord;
                //var curRec = me.getRecord();
                var docId = me.controller.agentPIRDebtorId;
                var panel = btn.up('#agentPIRDebtorEditWindow');
                var debtCalcType = panel.down('#cbDebtCalc').getValue();
                var penChargeType = panel.down('#cbPenaltyCharge').getValue();

                me.controller.mask('Расчет даты...', panel);
               
                B4.Ajax.request({
                    url: B4.Url.action('DebtStartDateCalculate', 'AgentPIRExecute'),
                    method: 'POST',
                    timeout: 100 * 60 * 60 * 3,
                    params: {
                        docId: docId,
                        debtCalcType: debtCalcType,
                        penChargeType: penChargeType
                    }
                })
                    .next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);
                        me.controller.unmask();
                        Ext.Msg.alert('Результаты расчета', obj.message);
                        
                        var datefieldDSD = panel.down('#sfDebtStartDate');
                        var datefieldDED = panel.down('#sfDebtEndDate');
                        datefieldDSD.setValue(obj.dateStartDebt);
                        datefieldDED.setValue(obj.DebtEndDate);

                        var debtBaseField = panel.down('#nfDebtBaseTariff');
                        var penaltyDebtField = panel.down('#nfPenaltyDebt');
                        debtBaseField.setValue(obj.DebtBaseTariffSum);                      
                        penaltyDebtField.setValue((obj.PenaltyDebt === undefined) ? 0 : (obj.PenaltyDebt === null) ? 0 : obj.PenaltyDebt);
                        //   grid.getStore().load();
                    })
                    .error(function (error) {
                        me.controller.unmask();
                        Ext.Msg.alert('Ошибка расчета', error.message || 'Ошибка при расчете даты');
                    });
            }
        },

        {
            xtype: 'grideditwindowaspect',
            name: 'agentPIRDebtorGridAspect',
            gridSelector: 'agentpirdebtordocumentGrid',
            editFormSelector: '#agentPIRDocumentEditWindow',
            storeName: 'AgentPIRDocument',
            modelName: 'AgentPIRDocument',
            editWindowView: 'agentpir.DocumentEditWindow',

            onSaveSuccess: function () {
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');

            },
            listeners: {
                getdata: function (asp, record) {
                    
                    if (!record.get('Id')) {
                        record.set('AgentPIRDebtor', asp.controller.agentPIRDebtorId);
                    }
                }
            }
            
        },
    ],

    mainView: 'agentpir.Grid',
    mainViewSelector: 'agentpirGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'agentpirGrid'
        }        
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('agentpirGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('AgentPIR').load();
    },

    init: function () {
        var me = this,
            actions = {
                'debtoreditwindow button[action=getExtract]': {
                    'click': { fn: me.getExtract, scope: me }
                }
            };
        me.control(actions);
        me.callParent(arguments);
    },

    getExtract: function (btn) {
        var contr = this;
        //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
        B4.Ajax.request({
            //method: 'POST',
            url: B4.Url.action('DownloadOrdering', 'Ordering'),
            params: {
                Id: contr.extractId
            }
        }).next(function (resp) {
            var tryDecoded;

            //asp.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;
            if (tryDecoded.success == false) {
                throw new Error(tryDecoded.message);
            }
            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });

                //me.fireEvent('onprintsucess', me);
            }
        }).error(function (err) {
            //asp.unmask();
            Ext.Msg.alert('Ошибка', err.message);
        });
    }

});