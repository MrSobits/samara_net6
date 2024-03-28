Ext.define('B4.controller.SMEVEGRN', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    smevEGRN: null,

    models: [
        'smev.SMEVEGRN',
        'smev.SMEVEGRNFile'
    ],
    stores: [
        'smev.SMEVEGRNFile',
        'smev.SMEVEGRN'
    ],
    views: [

        'smevegrn.Grid',
        'gisGkh.SignWindow',
        'smevegrn.EditWindow',
        'smevegrn.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'smevegrnGridAspect',
            gridSelector: 'smevegrngrid',
            editFormSelector: '#smevegrnEditWindow',
            storeName: 'smev.SMEVEGRN',
            modelName: 'smev.SMEVEGRN',
            editWindowView: 'smevegrn.EditWindow',
            //roId: null,
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevegrnEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
                actions['#smevegrnEditWindow #sfEGRNObjectType'] = { 'change': { fn: this.onChangeObjType, scope: this } };
                actions['#smevegrnEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };
            },
            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevegrnfileinfogrid');
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },
            onChangeRO: function (field, newValue) {
                //this.roId = newValue;
                if (newValue) {
                    var sfRoom = this.getForm().down('#sfRoom');
                    sfRoom.setDisabled(false);
                    sfRoom.getStore().filter('RO', newValue.Id);
                }
            },
            onChangeObjType: function (field, newValue) {
                //this.roId = newValue;
                var sfRoom = this.getForm().down('#sfRoom');
                var sfRealityObject = this.getForm().down('#sfRealityObject');
                switch (newValue.Code) {
                    case '002001002000': //здание
                        sfRealityObject.show();
                        sfRoom.hide();
                        break;
                    case '002001003000': //помещение
                        sfRealityObject.show();
                        sfRoom.show();
                        break;
                    default:
                        sfRealityObject.hide();
                        sfRoom.hide();
                        break;
                }
                sfRoom.setDisabled(false);
                sfRoom.getStore().filter('RO', newValue.Id);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevEGRN = record.getId();
                    var grid = form.down('smevegrnfileinfogrid'),
                        store = grid.getStore();
                    store.filter('smevEGRN', record.getId());
                    if (record.getId()) {
                      
                        B4.Ajax.request(B4.Url.action('LoggingOpen', 'SMEVEGRNExecute', {
                            docId: record.getId()
                        })).next(function (response) {                        
                            return true;
                        }).error(function () {                          
                        });
                    }
                    var smevEGRN = record.getId(),
                        grid = form.down('smevegrnfileinfogrid'),
                        store = grid.getStore();
                    store.filter('smevEGRN', smevEGRN);
                }
            }
        }
    ],

    mainView: 'smevegrn.Grid',
    mainViewSelector: 'smevegrngrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevegrngrid'
        },
        {
            ref: 'smevegrnFileInfoGrid',
            selector: 'smevegrnfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({

            'smevegrngrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } },
            'gisGkhSignWindow': { createsignature: { fn: this.sendRequest, scope: this } },
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0) {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, вы инициировали повторный запуск, предыдущие данные будут удалены');
            //return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        signwindow = Ext.create('B4.view.gisGkh.SignWindow');
        signwindow.rec = rec;
        signwindow.signer = new XadesSigner();
        signwindow.signer.getCertificates(2, 'My', me.fillCertificatesCombo, function (e) {
            Ext.Msg.alert('Ошибка получения списка сертификатов!', 'Не удалось получить сертификаты' + '<br>' + (e.message || e));
        }, me);
        signwindow.show();

        //B4.Ajax.request({
        //    url: B4.Url.action('Execute', 'SMEVEGRNExecute'),
        //    params: {
        //        taskId: rec.getId()
        //    },
        //    timeout: 9999999
        //}).next(function (response) {
        //    me.unmask();
        //    me.getStore('smev.SMEVEGRN').load();
        //    return true;
        //}).error(function () {
        //    me.unmask();
        //    me.getStore('smev.SMEVEGRN').load();
        //});
    },

    sendRequest: function () {
        var me = this;
        debugger;
        var reqId = signwindow.rec.getId();
        signwindow.close();
        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVEGRNExecute'),
            params: {
                taskId: reqId
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVEGRN').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVEGRN').load();
        });
    },

    getCertificates: function (win) {
        var finder = this.findCert();
        finder.then(function (certNames) {
            return certNames;
        }).then(function (val) {
            var field = win.down('#dfCert');
            var newStore = Ext.create('Ext.data.Store', {
                fields: ['cert', 'subjectName', 'thumbprint'],
                data: val
            });
            field.bindStore(newStore);
            field.getStore().load();
            return val;
        });
    },

    fillCertificatesCombo: function (certificates) {
        var me = this,
            certCombo = signwindow.down('#dfCert'),
            certComboStore = certCombo.getStore();

        certCombo.clearValue();
        certComboStore.removeAll();
        Ext.each(certificates, function (cert) {
            var certificateRec = Ext.create('B4.model.gisGkh.Certificate', {
                SubjectName: cert.subjectName,
                Certificate: cert,
                Thumbprint: cert.thumbprint
            });

            certComboStore.add(certificateRec);
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevegrngrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVEGRN').load();
    }
});