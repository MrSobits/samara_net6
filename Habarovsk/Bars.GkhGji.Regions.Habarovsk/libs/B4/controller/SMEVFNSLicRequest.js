Ext.define('B4.controller.SMEVFNSLicRequest', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
        //'B4.aspects.permission.GkhPermissionAspect',
    ],

    afterset: false,
    SMEVFNSLicRequest: null,
  
    models: [
        'smev.SMEVFNSLicRequest',
        'smev.SMEVFNSLicRequestFile'
    ],
    stores: [
        'smev.SMEVFNSLicRequestFile',
        'smev.SMEVFNSLicRequest'

    ],
    views: [
        'smevfnslicrequest.Grid',
        'smevfnslicrequest.EditWindow',
        'smevfnslicrequest.FileInfoGrid'
    ],
    aspects: [
        //{
        //    xtype: 'gkhpermissionaspect',
        //    permissions: [
        //        {
        //            name: 'GkhGji.SMEV.SMEVFNSLicRequest.ChangeState',
        //            applyTo: '#dfRequestState',
        //            selector: '#smevfnslicrequestEditWindow',
        //            applyBy: function (component, allowed) {
        //                component.setVisible(allowed);
        //            }
        //        }
        //    ]
        //},
        {
            xtype: 'grideditwindowaspect',
            name: 'smevfnslicrequestGridAspect',
            gridSelector: 'smevfnslicrequestgrid',
            editFormSelector: '#smevfnslicrequestEditWindow',
            storeName: 'smev.SMEVFNSLicRequest',
            modelName: 'smev.SMEVFNSLicRequest',
            editWindowView: 'smevfnslicrequest.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                this.controller.afterset = false;
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevfnslicrequestEditWindow #dfFNSLicRequestType'] = { 'change': { fn: this.onChangeRequestType, scope: this } };
                actions['#smevfnslicrequestEditWindow #dfFNSLicPersonType'] = { 'change': { fn: this.onChangePersonType, scope: this } };
                actions['#smevfnslicrequestEditWindow #send'] = { 'click': { fn: this.send, scope: this } };
                actions['#smevfnslicrequestEditWindow #checkAns'] = { 'click': { fn: this.checkAns, scope: this } };
                actions['#smevfnslicrequestEditWindow #dfLicense'] = { 'change': { fn: this.getLicenseInfo, scope: this } };
            },
            getLicenseInfo: function (record) {
                var me = this;
                var form = this.getForm();
                var license = form.down('#dfLicense');
                if (license.value != null) {
                    var licenseData = license.value.Id;
                }
               
                //var dfFamilyUL = form.down('#dfFamilyUL');
                var dfNameUL = form.down('#dfNameUL');
                //var dfName = form.down('#dfName');
                var dfOGRN = form.down('#dfOGRN');
                var dfINN = form.down('#dfINN');

                var dfDateLic = form.down('#dfDateLic');
                var dfDateStartLic = form.down('#dfDateStartLic');
                var dfDateEndLic = form.down('#dfDateEndLic');
                var dfLicOrgFullName = form.down('#dfLicOrgFullName');
                var dfLicOrgShortName = form.down('#dfLicOrgShortName');
                var dfLicOrgOGRN = form.down('#dfLicOrgOGRN');
                var dfLicOrgINN = form.down('#dfLicOrgINN');
                var dfLicOrgOKOGU = form.down('#dfLicOrgOKOGU');
                var dfNumLic = form.down('#dfNumLic');
                var dfAddress = form.down('#dfAddress');
                var dfPrAction = form.down('#dfPrAction');
                if (license != null) {
                    if (this.controller.afterset) {
                        //this.controller.afterset = false;
                        var result = B4.Ajax.request(B4.Url.action('GetLicenseInfo', 'SMEVFNSLicRequestExecute', {
                            licenseData: licenseData
                        }
                        )).next(function (response) {
                            
                            var data = Ext.decode(response.responseText);
                            dfOGRN.setValue(data.data.OGRN);
                            dfINN.setValue(data.data.INN);
                            dfNameUL.setValue(data.data.NameUL);
                            dfDateLic.setValue(data.data.DateRegister);
                            dfDateStartLic.setValue(data.data.DateIssued);
                            dfDateEndLic.setValue(data.data.DateTermination);
                            dfLicOrgFullName.setValue(data.data.NameLO);
                            dfLicOrgShortName.setValue(data.data.ShortNameLO);
                            dfLicOrgOGRN.setValue(data.data.OGRNLO);
                            dfLicOrgINN.setValue(data.data.INNLO);
                            dfLicOrgOKOGU.setValue(data.data.OKOGULO);
                            dfNumLic.setValue(data.data.LicNumber);
                            dfAddress.setValue(data.data.Address);
                            dfPrAction.setValue(data.data.PrAction);
                            return true;
                        }).error(function () {

                        });
                        //  Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                    }
                    else {
                        //this.afterset = true;
                    }
                }
            },
            onChangePersonType: function (field, newValue) {
                var form = this.getForm(),
                    dfULName = form.down('#dfULName'),
                    dfIPName = form.down('#dfIPName'),
                    fsUrParams = form.down('#fsUrParams'),
                    fsIpParams = form.down('#fsIpParams')
                    ;
                   
                if (newValue == B4.enums.FNSLicPersonType.Juridical) {
                    fsUrParams.show();
                    fsUrParams.setDisabled(false);
                    dfULName.show();
                    dfULName.setDisabled(false);
                    dfIPName.hide();
                    dfIPName.setDisabled(true);
                    //fsIpParams.hide();
                    //fsIpParams.setDisabled(true);
                }
                else if (newValue == B4.enums.FNSLicPersonType.IP) {
                    fsUrParams.show();
                    fsUrParams.setDisabled(false);
                    dfULName.hide();
                    dfULName.setDisabled(true);
                    dfIPName.show();
                    dfIPName.setDisabled(false);
                    //fsIpParams.show();
                    //fsIpParams.setDisabled(false);

                    //dfINN2.setValue(dfINN.getValue());
                }
            },

            onChangeRequestType: function (field, newValue) {
                var form = this.getForm(),
                    dfDeleteIdDoc = form.down('#dfDeleteIdDoc'),
                    dfFNSLicPersonType = form.down('#dfFNSLicPersonType'),
                    dfLicense = form.down('#dfLicense'),
                    fsLicParams = form.down('#fsLicParams'),
                    fsLicSvedParams = form.down('#fsLicSvedParams'),
                    fsDecLOParams = form.down('#fsDecLOParams'),
                    fsLOParams = form.down('#fsLOParams'),
                    fsUrParams = form.down('#fsUrParams'),
                    fsIpParams = form.down('#fsIpParams')
                    ;

                if (newValue == B4.enums.FNSLicRequestType.Add) {
                    dfFNSLicPersonType.show();
                    dfFNSLicPersonType.setDisabled(false);
                    //dfFNSLicPersonType.setValue(null);
                    dfLicense.show();
                    dfLicense.setDisabled(false);
                    fsLicParams.show();
                    fsLicParams.setDisabled(false);
                    fsLicSvedParams.show();
                    fsLicSvedParams.setDisabled(false);
                    fsDecLOParams.show();
                    fsDecLOParams.setDisabled(false);
                    fsLOParams.show();
                    fsLOParams.setDisabled(false);
                    dfDeleteIdDoc.hide();
                    dfDeleteIdDoc.setDisabled(true);
                    fsUrParams.show();
                    fsUrParams.setDisabled(false);
                }
                else if (newValue == B4.enums.FNSLicRequestType.Delete) {
                    dfFNSLicPersonType.hide();
                    dfFNSLicPersonType.setDisabled(true);
                    fsUrParams.hide();
                    fsUrParams.setDisabled(true);
                    //fsIpParams.hide();
                    //fsIpParams.setDisabled(true);
                    dfDeleteIdDoc.show();
                    dfDeleteIdDoc.setDisabled(false);
                    dfLicense.hide();
                    dfLicense.setDisabled(true);
                    fsLicParams.hide();
                    fsLicParams.setDisabled(true);
                    fsLicSvedParams.hide();
                    fsLicSvedParams.setDisabled(true);
                    fsDecLOParams.hide();
                    fsDecLOParams.setDisabled(true);
                    fsLOParams.hide();
                    fsLOParams.setDisabled(true);

                    //dfINN2.setValue(dfINN.getValue());
                }
            },
            send: function (record) {
                
                var me = this;
                var taskId = SMEVFNSLicRequest;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');
               
               if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
               else {
                   me.mask('Обмен данными с ФНС', me.getForm());
                   var result = B4.Ajax.request(B4.Url.action('SendRequest', 'SMEVFNSLicRequestExecute', {
                        taskId: taskId
                    }
                   )).next(function (response)
                   {           
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                       var grid = form.down('smevfnslicrequestfileinfogrid'),
                        store = grid.getStore();
                       store.filter('SMEVFNSLicRequest', taskId);

                        me.unmask();

                        return true;
                   })
                   .error(function (resp){
                       Ext.Msg.alert('Ошибка', resp.message);
                       me.unmask();                           
                   });

                }
            },
            checkAns: function (record) {
                var me = this;
                var taskId = SMEVFNSLicRequest;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVFNSLicRequestExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevfnslicrequestfileinfogrid'),
                            store = grid.getStore();
                        store.filter('SMEVFNSLicRequest', SMEVFNSLicRequest);

                        dfAnswer.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                        Ext.Msg.alert('Ошибка', response.message);
                        me.unmask();
                        me.getStore('smev.SMEVFNSLicRequest').load();
                    });

                }
            },
            
            listeners: {
                aftersetformdata: function (asp, record, form) {

                    this.controller.afterset = true;
                    SMEVFNSLicRequest = record.getId();
                    var grid = form.down('smevfnslicrequestfileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVFNSLicRequest', record.getId());
                },
                beforesetformdata: function (me, record) {
                    this.controller.afterset = false;
                }
              
             
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        }
    ],

    mainView: 'smevfnslicrequest.Grid',
    mainViewSelector: 'smevfnslicrequestgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevfnslicrequestgrid'
        },
        {
            ref: 'smevfnslicrequestFileInfoGrid',
            selector: 'smevfnslicrequestfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        //this.afterset = false;        
        this.callParent(arguments);
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevfnslicrequestGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevfnslicrequestgrid');
        this.bindContext(view);
        this.afterset = false;
        this.application.deployView(view);
        this.getStore('smev.SMEVFNSLicRequest').load();
    }
});