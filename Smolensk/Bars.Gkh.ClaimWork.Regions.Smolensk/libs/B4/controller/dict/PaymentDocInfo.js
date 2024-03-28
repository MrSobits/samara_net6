Ext.define('B4.controller.dict.PaymentDocInfo', {
    extend: 'B4.base.Controller',

    requires: [
         'B4.aspects.GridEditCtxWindow',
         'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'paymentdocinfogrid'
        }
    ],

    models: [
        'dict.PaymentDocInfo',
        'regop.Fias'
    ],

    stores: [
        'dict.PaymentDocInfo'
    ],

    views: [
        'dict.paymentdocinfo.Grid',
        'dict.paymentdocinfo.EditWindow'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.Settings.PaymentDocInfo.Create', applyTo: 'b4addbutton', selector: 'paymentdocinfogrid' },
                { name: 'GkhRegOp.Settings.PaymentDocInfo.Edit', applyTo: 'b4savebutton', selector: 'paymentdocinfoeditwin' },
                { name: 'GkhRegOp.Settings.PaymentDocInfo.Delete', applyTo: 'b4deletecolumn', selector: 'paymentdocinfogrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'paymentdocinfogrideditwinaspect',
            gridSelector: 'paymentdocinfogrid',
            editFormSelector: 'paymentdocinfoeditwin',
            storeName: 'dict.PaymentDocInfo',
            editWindowView: 'dict.paymentdocinfo.EditWindow',
            modelName: 'dict.PaymentDocInfo',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            otherActions: function (actions) {
                var me = this;

                actions['paymentdocinfoeditwin [name=Municipality]'] = {
                    'change': { fn: me.onChangeMunicipality, scope: me }
                };

                actions['paymentdocinfoeditwin [name=MoSettlement]'] = {
                    'change': { fn: me.onChangeMoSettlement, scope: me }
                };

                actions['paymentdocinfoeditwin [name=LocalityName]'] = {
                    'change': { fn: me.onChangeLocality, scope: me }
                };

                actions['paymentdocinfoeditwin checkbox[name="IsForRegion"]'] = {
                    change: { fn: me.onChangeIsForRegion, scope: me }
                };

                actions['paymentdocinfoeditwin b4enumcombo[name="FundFormationType"]'] = {
                    change: { fn: me.onChangeFundFormation, scope: me }
                };

                actions['paymentdocinfoeditwin b4deletebutton'] = {
                    click: { fn: me.onClickDeleteButton, scope: me }
                };
            },
            listeners: {
                beforesave: function (asp, record) {
                    var fias = record.get('LocalityName') || false,
                        aoGuid = fias.AOGuid || '',
                        name = fias.FormalName || '',
                        isForRegion = record.get('IsForRegion') || false;

                    if (isForRegion) {
                        return true;
                    }

                    if (!record.get('Municipality') && !record.get('MoSettlement')
                        && fias && !record.get('RealityObject')) {
                        Ext.Msg.alert('Ошибка сохранения!', 'Заполните хотя бы одно из адресных полей!');

                        return false;
                    }

                    if (fias && (aoGuid.Length !== 0 || name.Length !== 0)) {
                        record.set('LocalityName', name);
                        record.set('LocalityAoGuid', aoGuid);
                    }

                    return true;
                },
                aftersetformdata: function(asp) {
                    var form = asp.getForm(),
                        settlementField = form.down('[name=MoSettlement]'),
                        localityStore = form.down('[name=LocalityName]').getStore(),
                        realObjStore = form.down('[name=RealityObject]').getStore(),
                        dateStartField = form.down('datefield[name="DateStart"]'),
                        dateEndField = form.down('datefield[name="DateEnd"]')
                        ;
                    debugger;

                    if (dateStartField.getValue() === null ) {
                        dateStartField.setValue(new Date());
                    }
                    if (dateEndField.getValue() === null) {
                        var date = new Date();
                        date.setDate(date.getDate() + 30);
                        dateEndField.setValue(date);
                    }
                    localityStore.on('beforeload', this.onBeforeLocalityLoad, this);

                    

                    if (typeof settlementField.store == 'string') {
                        settlementField.store = Ext.create(settlementField.store);
                    }

                    settlementField.store.on('beforeload', this.onBeforeSettlementLoad, this);
                    realObjStore.on('beforeload', this.onBeforeRealObjLoad, this);


                },
                deleteSuccess: function (aspect) {
                    var form = aspect.getForm();
                    if (form) {
                        form.close();
                    }
                }
            },

            onChangeMunicipality: function () {
                var settlementField = this.getForm().down('[name=MoSettlement]');
                if (settlementField) {
                    settlementField.setValue(null);
                }
            },

            onChangeMoSettlement: function () {
                var localityField = this.getForm().down('[name=LocalityName]');
                if (localityField) {
                    localityField.setValue(null);
                }
            },

            onChangeLocality: function () {
                var realObjField = this.getForm().down('[name=RealityObject]');
                if (realObjField) {
                    realObjField.setValue(null);
                }
            },

            onChangeFundFormation: function() {
                var realObjField = this.getForm().down('[name=RealityObject]');
                if (realObjField) {
                    realObjField.setValue(null);
                }
            },

            onBeforeSettlementLoad: function (store, operation) {
                var municipalityField = this.getForm().down('[name=Municipality]');

                if (municipalityField && municipalityField.getValue()) {
                    operation.params.parentMoId = municipalityField.getValue();
                  }
            },

            onBeforeLocalityLoad: function (store, operation) {
                var settlementField = this.getForm().down('[name=MoSettlement]');

                if (settlementField && settlementField.getValue()) {
                    operation.params.municipalityId = settlementField.getValue();

                }
            },

            onBeforeRealObjLoad: function (store, operation) {
                var me = this,
                    form = me.getForm(),
                    localityField = form.down('[name=LocalityName]'),
                    settlementId = form.down('[name=MoSettlement]').getValue(),
                    muId = form.down('[name=Municipality]').getValue(),
                    fundForm = form.down('[name=FundFormationType]').getValue(),
                    dateStart = form.down('[name=DateStart]').getValue(),
                    dateEnd = form.down('[name=DateEnd]').getValue();

                operation.params.fundForm = fundForm;
                operation.params.dateStart = dateStart;
                operation.params.dateEnd = dateEnd;

                if (localityField && localityField.value && localityField.value.AOGuid) {
                    operation.params.localityGuid = localityField.value.AOGuid;
                    operation.params.settlementId = settlementId;
                    operation.params.muId = muId;
                }
            },

            onChangeIsForRegion: function (cb, newValue) {
                var win = cb.up('paymentdocinfoeditwin'),
                    moSettlementField = win.down('treeselectfield[name="MoSettlement"]'),
                    realityObjectField = win.down('b4selectfield[name="RealityObject"]'),
                    municipalityField = win.down('b4selectfield[name="Municipality"]'),
                    localityField = win.down('b4selectfield[name="LocalityName"]');

                if (newValue) {
                    moSettlementField.setDisabled(true);
                    moSettlementField.setValue('');
                    realityObjectField.setDisabled(true);
                    realityObjectField.setValue('');
                    municipalityField.setDisabled(true);
                    municipalityField.setValue('');
                    localityField.setDisabled(true);
                    localityField.setValue('');
                    return;
                }

                moSettlementField.setDisabled(false);
                realityObjectField.setDisabled(false);
                municipalityField.setDisabled(false);
                localityField.setDisabled(false);
            },

            onClickDeleteButton: function () {
                var me = this;
                var form = me.getForm();
                var record = form.getRecord();

                me.deleteRecord(record);
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('paymentdocinfogrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});