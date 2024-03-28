Ext.define('B4.controller.CSCalculation', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: ['cscalculation.CSCalculation',
        'cscalculation.CSCalculationRow'
    ],
    stores: ['cscalculation.CSCalculation',
        'cscalculation.CSCalculationRow'
    ],
    views: [
        'cscalculation.EditWindow',
        'cscalculation.Grid',
        'cscalculation.RowGrid'
    ],
    mainView: 'cscalculation.Grid',
    mainViewSelector: 'cscalculationGrid',
    calcId: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'cscalculationGrid'
        },
        {
            ref: 'cscalculationEditWindow',
            selector: 'cscalculationEditWindow'
        }
    ],
    mkdlicrequestId: null,

    aspects: [

        {
            xtype: 'gkhbuttonprintaspect',
            name: 'cscalculationPrintAspect',
            buttonSelector: '#cscalculationEditWindow #btnPrint',
            codeForm: 'CSCalculation',
            getUserParams: function () {
                debugger;
                var param = { Id: this.controller.calcId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'cscalculationGridAspect',
            gridSelector: 'cscalculationGrid',
            editFormSelector: '#cscalculationEditWindow',
            storeName: 'cscalculation.CSCalculation',
            modelName: 'cscalculation.CSCalculation',
            editWindowView: 'cscalculation.EditWindow',
            otherActions: function (actions) {               
                actions['#cscalculationEditWindow #btncalculate'] = { 'click': { fn: this.Calculate, scope: this } };   
                actions['#cscalculationEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
            }, 
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            onChangeRO: function (field, newValue) {
                //this.roId = newValue;
                var sfRoom = this.getForm().down('#sfRoom');
                sfRoom.setDisabled(false);
                sfRoom.getStore().filter('RO', newValue.Id);
            },
            Calculate: function (btn) {
                var me = this;
                debugger;
                var taskId = me.controller.calcId;
                var form = this.getForm();
                var nfResult = form.down('#nfResult');
                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Расчет', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('CalculateCS', 'CSCalculationOperations', {
                        taskId: taskId
                    }
                    )).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', 'Расчет завершен');
                        nfResult.setValue(data.data.result);
                        me.unmask();

                        return true;
                    })
                        .error(function (resp) {
                            Ext.Msg.alert('Ошибка', resp.message);
                            me.unmask();
                        });

                }
            },      
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    var cscalculationId = record.getId();
                    asp.controller.calcId = record.getId();
                    //пытаемся проставить прочитано для инспектора и/или руководителя управления
                    if (cscalculationId != 0) {
                        var rowgrid = form.down('cscalculationRowGrid'),
                            rowstore = rowgrid.getStore();
                        rowgrid.setDisabled(false);
                        rowstore.filter('cscalculationId', cscalculationId);    
                        this.controller.getAspect('cscalculationPrintAspect').loadReportStore();
                    }
                    else
                    {                       
                        var rowgrid = form.down('cscalculationRowGrid'),
                            rowstore = rowgrid.getStore();
                        rowstore.clearData();
                        rowgrid.setDisabled(true);
                    }

                }
            }
        },        
        {
            xtype: 'gkhinlinegridaspect',
            name: 'cscalculationRowAspect',
            storeName: 'cscalculation.CSCalculationRow',
            modelName: 'cscalculation.CSCalculationRow',
            gridSelector: 'cscalculationRowGrid'
        }
        
    ],

    mainView: 'cscalculation.Grid',
    mainViewSelector: 'cscalculationGrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('cscalculationGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('cscalculation.CSCalculation').load();
    },

    init: function () {
        var me = this;
        me.callParent(arguments);
    }
});