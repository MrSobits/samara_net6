Ext.define('B4.controller.RealityObjectPassport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.enums.CrFundFormationType'
    ],

    models: ['RealityObjectPassport', 'DisclosureInfoRealityObj'],

    views: ['realityobjectpassport.ViewPanel'],

    mainView: 'realityobjectpassport.ViewPanel',
    mainViewSelector: '#realityobjectpassportViewPanel',
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'passportstatepermission',
            permissions: [
                {
                    name: 'GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation.OwnerInfoContainer',
                    applyTo: '#ownerInfoContainer',
                    selector: '#realityobjectpassportViewPanel',
                    applyBy: function (component, allowed) { component.setVisible(!component.hidden && allowed); }
                },
                {
                    name: 'GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation.ProtocolInfoContainer',
                    applyTo: '#protocolInfoContainer',
                    selector: '#realityobjectpassportViewPanel',
                    applyBy: function (component, allowed) { component.setVisible(!component.hidden && allowed); }
                }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'realityobjectpassportviewpanel b4updatebutton': {
                click: { fn: me.updateInfo, scope: me }
            }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        this.updateInfo();
    },

    updateInfo: function() {
        var me = this,
            model = me.getModel('RealityObjectPassport'),
            mainView = me.getMainView();

        me.mask('Загрузка', mainView);
        if (me.params) {
            B4.Ajax.request({
                url: B4.Url.action('GetRealtyObjectPassport', 'DisclosureInfoRealityObj'),
                params: {
                    diRoId: me.params.disclosureInfoRealityObjId,
                    diId: me.params.disclosureInfoId,
                    yearStart: me.params.year
                },
                timeout: 9999999
            }).next(function (response) {
                var obj, record;

                try {
                    obj = Ext.decode(response.responseText);
                } catch (e) {
                    obj = {};
                }

                record = new model(obj);
                mainView.loadRecord(record);

                var ownerInfoContainer = Ext.ComponentQuery.query('[name=OwnerInfoContainer]')[0],
                    protocolInfoContainer = Ext.ComponentQuery.query('[name=ProtocolInfoContainer]')[0];

                if (obj.RawTypeOfFormingCr !== B4.enums.CrFundFormationType.Unknown) {
                    protocolInfoContainer.show();

                    if (obj.RawTypeOfFormingCr !== B4.enums.CrFundFormationType.RegOpAccount) {
                        ownerInfoContainer.show();
                    } else {
                        ownerInfoContainer.hide();
                    }
                } else {
                    ownerInfoContainer.hide();
                    protocolInfoContainer.hide();
                }

                me.setPermissions();
                me.unmask();
            }).error(function (e) {
                Ext.msg.alert('Ошибка', e.message || 'При обработке запроса произошла ошибка');
                me.unmask();
            });
        }
    },
    
    setPermissions: function() {
        var me = this,
            model = me.getModel('DisclosureInfoRealityObj'),
            aspect = me.getAspect('passportstatepermission'),
            record = new model({ Id: me.params.disclosureInfoRealityObjId });

        aspect.setPermissionsByRecord(record);
    }
});