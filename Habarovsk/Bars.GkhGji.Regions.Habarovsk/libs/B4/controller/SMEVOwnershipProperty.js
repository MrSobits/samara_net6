Ext.define('B4.controller.SMEVOwnershipProperty', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    SMEVOwnershipProperty: null,
  
    models: [
        'smevownershipproperty.SMEVOwnershipProperty',
        'smevownershipproperty.SMEVOwnershipPropertyFile'
    ],
    stores: [
        'smevownershipproperty.SMEVOwnershipProperty',
        'smevownershipproperty.SMEVOwnershipPropertyFile'
    ],
    views: [
        'smevownershipproperty.Grid',
        'smevownershipproperty.EditWindow',
        'smevownershipproperty.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevownershippropertyGridAspect',
            gridSelector: 'smevownershippropertygrid',
            editFormSelector: '#smevownershippropertyEditWindow',
            storeName: 'smevownershipproperty.SMEVOwnershipProperty',
            modelName: 'smevownershipproperty.SMEVOwnershipProperty',
            editWindowView: 'smevownershipproperty.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevownershippropertyEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                actions['#smevownershippropertyEditWindow #btnGetExtract'] = { 'click': { fn: this.getExtract, scope: this } };
                actions['#smevownershippropertyEditWindow #dfQueryType'] = { 'change': { fn: this.onChangeRequestType, scope: this } }
                actions['#smevownershippropertyEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
                actions['#smevownershippropertyEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };
            },

            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevownershippropertyfileinfogrid');
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },

            onChangeRO: function (field, newValue) {
                //this.roId = newValue;
                if (newValue != null) {
                    var sfRoom = this.getForm().down('#sfRoom');
                    sfRoom.setDisabled(false);
                    sfRoom.getStore().filter('RO', newValue.Id);
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    SMEVOwnershipProperty = record.getId();
                    var grid = form.down('smevownershippropertyfileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVOwnershipProperty', record.getId());
                }
            },
            onChangeRequestType: function (field, newValue) {
                var form = this.getForm(),
                    dfRealRoom = form.down('#dfRealRoom'),
                    dfCadastral = form.down('#dfCadastral'),
                    dfRegNumber = form.down('#dfRegNumber')
                    ;

                if (newValue == B4.enums.QueryTypeType.AddressQuery) {
                    dfRealRoom.show();
                    dfRealRoom.setDisabled(false);
                    dfRegNumber.hide();
                    dfRegNumber.setDisabled(true);
                    dfCadastral.hide();
                    dfCadastral.setDisabled(true);
                }
                else if (newValue == B4.enums.QueryTypeType.CadasterNumberQuery) {
                    dfRealRoom.hide();
                    dfRealRoom.setDisabled(true);
                    dfRegNumber.hide();
                    dfRegNumber.setDisabled(true);
                    dfCadastral.show();
                    dfCadastral.setDisabled(false);
                }
                else if (newValue == B4.enums.QueryTypeType.RegisterNumberQuery) {
                    dfRealRoom.hide();
                    dfRealRoom.setDisabled(true);
                    dfRegNumber.show();
                    dfRegNumber.setDisabled(false);
                    dfCadastral.hide();
                    dfCadastral.setDisabled(true);
                }
            },
            get: function (record) {
                var me = this;
                var taskId = SMEVOwnershipProperty;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVOwnershipPropertyExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevownershippropertyfileinfogrid'),
                        store = grid.getStore();
                        store.filter('SMEVOwnershipProperty', SMEVOwnershipProperty);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smevownershipproperty.SMEVOwnershipProperty').load();
                    });

                }
            },
            getExtract: function (btn) {
                var asp = this,
                    rec = SMEVOwnershipProperty;

                //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
                B4.Ajax.request({
                    //method: 'POST',
                    url: B4.Url.action('GetAnswerFile', 'SMEVOwnershipPropertyExecute'),
                    params: {
                        taskId: rec
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
                    if (id.Id > 0) {
                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: B4.Url.action('Download', 'FileUpload', { Id: id.Id })
                        });

                        //me.fireEvent('onprintsucess', me);
                    }
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });
            }
        },
   
    ],

    mainView: 'smevownershipproperty.Grid',
    mainViewSelector: 'smevownershippropertygrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevownershippropertygrid'
        },
        {
            ref: 'smevownershippropertyFileInfoGrid',
            selector: 'smevownershippropertyfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            
            'smevownershippropertygrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0) {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVOwnershipPropertyExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smevownershipproperty.SMEVOwnershipProperty').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smevownershipproperty.SMEVOwnershipProperty').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevownershippropertyGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevownershippropertygrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smevownershipproperty.SMEVOwnershipProperty').load();
    }
});