Ext.define('B4.controller.GASU', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    gasuId: null,
    afterset: false,
  
    models: [
        'smev.GASU',
        'smev.GASUData',
        'smev.GASUFile'
    ],
    stores: [
        'smev.GASUFile',
        'smev.GASUData',
        'smev.GASU'

    ],
    views: [
        'gasu.Grid',
        'gasu.EditWindow',
        'gasu.FileInfoGrid',
        'gasu.GASUDataGrid',
        'gasu.DataEditWindow'
    ],
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'gasuGridAspect',
            gridSelector: 'gasugrid',
            editFormSelector: '#gasuEditWindow',
            storeName: 'smev.GASU',
            modelName: 'smev.GASU',
            editWindowView: 'gasu.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#gasuEditWindow #sendCalculateButton'] = { 'click': { fn: this.sendCalculate, scope: this } };
            },          
            sendCalculate: function (record) {
                var me = this;
                var taskId = gasuId;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');
               
               if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
               else {
                   me.mask('Обмен данными со СМЭВ', me.getForm());
                   var result = B4.Ajax.request(B4.Url.action('Execute', 'GASUExecute', {
                        taskId: taskId
                    }
                   )).next(function (response)
                   {           
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                       var grid = form.down('gasufileinfogrid'),
                        store = grid.getStore();
                       store.filter('GASU', taskId);

                        me.unmask();

                        return true;
                   })
                   .error(function (resp){
                       Ext.Msg.alert('Ошибка', resp.message);
                       me.unmask();                           
                   });

                }
            },      
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    gasuId = record.getId();
                    var grid = form.down('gasufileinfogrid'),
                    store = grid.getStore();
                    store.filter('GASU', record.getId()); 
                    var violgrid = form.down('gasudatagrid'),
                        violstore = violgrid.getStore();
                    violstore.filter('GASU', record.getId());      
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'gasudatagridAspect',
            gridSelector: 'gasudatagrid',
            editFormSelector: '#gasuDataEditWindow',
            storeName: 'smev.GASUData',
            modelName: 'smev.GASUData',
            editWindowView: 'gasu.DataEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('GASU', gasuId);
                    }
                }
            }
        },
    ],

    mainView: 'giserp.Grid',
    mainViewSelector: 'gasugrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'gasugrid'
        },
        {
            ref: 'gasuFileInfoGrid',
            selector: 'gasufileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this;
        me.params = {};
        this.getStore('smev.GASU').load();
        this.callParent(arguments);
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('gasuGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('gasugrid');
        afterset = false;   
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.GASU').load();
    }

});