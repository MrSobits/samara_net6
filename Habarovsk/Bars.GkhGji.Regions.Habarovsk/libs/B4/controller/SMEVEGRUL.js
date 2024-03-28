Ext.define('B4.controller.SMEVEGRUL', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    smevEGRUL: null,
  
    models: [
        'smev.SMEVEGRUL',
        'smev.SMEVEGRULFile'
    ],
    stores: [
        'smev.SMEVEGRULFile',
        'smev.SMEVEGRUL'
    ],
    views: [

        'smevegrul.Grid',
        'smevegrul.EditWindow',
        'smevegrul.FileInfoGrid'

    ],

    aspects: [
             {
                 xtype: 'gkhbuttonprintaspect',
                 name: 'sMEVEGRULPrintAspect',
                 buttonSelector: '#smevegrulEditWindow #btnPrint',
                 codeForm: 'SMEVEGRUL',
                 getUserParams: function () {
                     var param = { Id: smevEGRUL };
                     this.params.userParams = Ext.JSON.encode(param);
                 }
             },
        {
            xtype: 'grideditwindowaspect',
            name: 'smevegrulGridAspect',
            gridSelector: 'smevegrulgrid',
            editFormSelector: '#smevegrulEditWindow',
            storeName: 'smev.SMEVEGRUL',
            modelName: 'smev.SMEVEGRUL',
            editWindowView: 'smevegrul.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevegrulEditWindow #dfInnOgrn'] = { 'change': { fn: this.onChangeInnOgrn, scope: this } };
                actions['#smevegrulEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                
                actions['#smevegrulEditWindow button[action=PrintExtract]'] = { click: this.getExtractPdf, scope: this };
                
                //actions['#smevegrulEditWindow #btnPrintExtract'] = { 'click': { fn: this.printExtract, scope: this } };
               
            },            
            //printExtract: function (button) {
            //    //Обновление ссылки при наведении на кнопку выписки
            //    button.btnEl.dom.href = B4.Url.action('/SMEVEGRULExecute/PrintEGRULExtract/?egrul_id=' + smevEGRUL);
            //},
            onChangeInnOgrn: function (field, newValue) {
                var form = this.getForm(),
                dfINN = form.down('#dfInn');

                if (newValue == B4.enums.InnOgrn.INN) {
                    dfINN.regex = /^(\d{10}|\d{12})$/;
                }
                else {
                    dfINN.regex = /^(\d{13})$/;
                }

            },
            getExtractPdf: function (){
                debugger;     
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('PrintExtract', 'SMEVEGRULExecute', { id: smevEGRUL })
                });        
            } ,
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevEGRUL = record.getId();
                    var me = this;
                    me.controller.getAspect('sMEVEGRULPrintAspect').loadReportStore();
                    var grid = form.down('smevegrulfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevEGRUL', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            },
            get: function (record) {
                var me = this;
                var taskId = smevEGRUL;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponce', 'SMEVEGRULExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevegrulfileinfogrid'),
                        store = grid.getStore();
                        store.filter('smevEGRUL', smevEGRUL);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smev.SMEVEGRUL').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevegrul.Grid',
    mainViewSelector: 'smevegrulgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevegrulgrid'
        },
        {
            ref: 'smevegrulFileInfoGrid',
            selector: 'smevegrulfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevegrulgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
            
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0)
        {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVEGRULExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);

            me.unmask();
            me.getStore('smev.SMEVEGRUL').load();
            return true;
         }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask(); 

             me.getStore('smev.SMEVEGRUL').load();
             return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevegrulgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVEGRUL').load();
    },
    onLaunch: function () {
        if (this.params && this.params.reqId > 0) {
            var model = this.getModel('smev.SMEVEGRUL');
            this.getAspect('smevegrulGridAspect').editRecord(new model({ Id: this.params.reqId }));
            this.params.reqId = 0;
        }
    },
});