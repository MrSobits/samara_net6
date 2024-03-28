Ext.define('B4.controller.SMEVEGRIP', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
         'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev.SMEVEGRIP',
        'smev.SMEVEGRIPFile'
    ],
    stores: [
        'smev.SMEVEGRIPFile',
        'smev.SMEVEGRIP'
    ],
    views: [

        'smevegrip.Grid',
        'smevegrip.EditWindow',
        'smevegrip.FileInfoGrid'

    ],

    aspects: [
          {
              xtype: 'gkhbuttonprintaspect',
              name: 'sMEVEGRIPPrintAspect',
              buttonSelector: '#smevegripEditWindow #btnPrint',
              codeForm: 'SMEVEGRIP',
              getUserParams: function () {
                  var param = { Id: smevMVD };
                  this.params.userParams = Ext.JSON.encode(param);
              }
          },

        {
            xtype: 'grideditwindowaspect',
            name: 'smevegripGridAspect',
            gridSelector: 'smevegripgrid',
            editFormSelector: '#smevegripEditWindow',
            storeName: 'smev.SMEVEGRIP',
            modelName: 'smev.SMEVEGRIP',
            editWindowView: 'smevegrip.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevegripEditWindow #dfInnOgrn'] = { 'change': { fn: this.onChangeInnOgrn, scope: this } };
                actions['#smevegripEditWindow button[action=PrintExtract]'] = { click: this.getExtractPdf, scope: this };

            },
            onChangeInnOgrn: function (field, newValue) {
                var form = this.getForm(),
                dfINN = form.down('#dfInn');
                                 
                if (newValue == B4.enums.InnOgrn.INN) {  
                    dfINN.regex = /^(\d{10}|\d{12})$/;
                }
                else {
                    dfINN.regex = /^(\d{15})$/;
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
                    src: B4.Url.action('PrintExtract', 'SMEVEGRIPExecute', { id: smevMVD })
                });
            } ,
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevMVD = record.getId();
                    var me = this;
                    me.controller.getAspect('sMEVEGRIPPrintAspect').loadReportStore();
                    var grid = form.down('smevegripfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevEGRIP', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevegrip.Grid',
    mainViewSelector: 'smevegripgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevegripgrid'
        },
        {
            ref: 'smevegripFileInfoGrid',
            selector: 'smevegripfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevegripgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }

        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        debugger;
        if (rec.get('RequestState') != 0)
        {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVEGRIPExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVEGRIP').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVEGRIP').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevegripgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVEGRIP').load();
    },

    onLaunch: function () {
        if (this.params && this.params.reqId > 0) {
            var model = this.getModel('smev.SMEVEGRIP');
            this.getAspect('smevegripGridAspect').editRecord(new model({ Id: this.params.reqId }));
            this.params.reqId = 0;
        }
    },
});