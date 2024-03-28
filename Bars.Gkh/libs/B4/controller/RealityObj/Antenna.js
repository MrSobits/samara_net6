Ext.define('B4.controller.realityobj.Antenna',
    {
        extend: 'B4.controller.MenuItemController',

        requires: [
            'B4.enums.AntennaRange',
            'B4.enums.AntennaReason',
            'B4.enums.YesNoNotSet',
            'B4.enums.YesNoMinus',
            'B4.aspects.GridEditWindow'
        ],

        models: [
            'realityobj.RealityObjectAntenna'
        ],

        stores: [
            'realityobj.RealityObjectAntenna'
        ],

        views: [
            'realityobj.AntennaGrid',
            'realityobj.AntennaEditWindow'
        ],

        mixins: {
            context: 'B4.mixins.Context'
        },

        refs: [
            {
                ref: 'mainView',
                selector: 'realityobjantennagrid'
            }
        ],

        parentCtrlCls: 'B4.controller.realityobj.Navi',

        aspects: [
            //{
            //    xtype: 'realityobjantennaperm',
            //    name: 'realityObjAntennaPerm'
            //}
            {
                xtype: 'grideditwindowaspect',
                name: 'realityobjAntennaGridWindowAspect',
                gridSelector: 'realityobjantennagrid',
                editFormSelector: '#realityobjAntennaEditWindow',
                storeName: 'realityobj.RealityObjectAntenna',
                modelName: 'realityobj.RealityObjectAntenna',
                editWindowView: 'realityobj.AntennaEditWindow',
                listeners: {
                    getdata: function (asp, record) {
                        var me = this;
                        debugger;
                        if (!record.data.Id) {
                            record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                        }
                    }
                },
                otherActions: function (actions) {
                    var me = this;
                    actions['#realityobjAntennaEditWindow #availchild'] = { 'change': { fn: me.changeValue, scope: me } };
                },

                changeValue: function (field, newValue, oldValue) {
                    debugger;
                    var form = this.getForm();
                    work = form.down('#work');
                    range = form.down('#range');
                    freq = form.down('#freq');
                    freqto = form.down('#freqto');
                    apart = form.down('#apart');
                    reason = form.down('#reason');
                    file = form.down('#file');

                    switch (newValue)
                    {
                        case 10:
                            work.show();
                            range.show();
                            freq.show();
                            freqto.show();
                            apart.show();
                            reason.setValue(null);
                            file.setValue(null);
                            reason.allowBlank = true;
                            reason.validate();
                            reason.hide();
                            file.allowBlank = true;
                            file.validate();
                            file.hide();
                            break;
                        case 20:
                            reason.show();
                            reason.setDisabled(false);
                            file.show();
                            file.setDisabled(false);
                            work.setValue(null);
                            range.setValue(null);
                            freq.setValue(null);
                            freqto.setValue(null);
                            apart.setValue(null);
                            work.hide();
                            range.hide();
                            freq.hide();
                            freqto.hide();
                            apart.hide();
                            break;
                        case 30:
                            work.setValue(null);
                            range.setValue(null);
                            freq.setValue(null);
                            freqto.setValue(null);
                            apart.setValue(null);
                            reason.setValue(null);
                            file.setValue(null);
                            file.allowBlank = true;
                            file.validate();
                            reason.allowBlank = true;
                            reason.validate();
                            reason.hide();
                            file.hide();
                            work.hide();
                            range.hide();
                            freq.hide();
                            freqto.hide();
                            apart.hide();
                            break;
                    }
                },

                //changeValueWork: function (field, newValue, oldValue) {
                //    range = form.down('#range');
                //    freq = form.down('#freq');
                //    apart = form.down('#apart');

                //    switch (newValue) {
                //        case 10:
                //            range.show();
                //            range.setDisabled(false);
                //            freq.show();
                //            freq.setDisabled(false);
                //            apart.show();
                //            apart.setDisabled(false);
                //            break;
                //        case 20:
                //            range.hide();
                //            range.setDisabled(true);
                //            freq.hide();
                //            freq.setDisabled(true);
                //            apart.hide();
                //            apart.setDisabled(true);
                //            break;
                //    }
                //}
            }
        ],

        init: function () {
            var me = this;
            //this.control({

            //    '#realityobjAntennaEditWindow #cbavailfield': { change: { fn: this.onChangePayerType2, scope: this } }
                

            //});
            me.getStore('realityobj.RealityObjectAntenna').on('beforeload', me.onBeforeLoad, me);
            me.callParent(arguments);
        },

        onChangePayerType2: function (field, newValue) {
            debugger;
            //var form = this.getForm(),
            //    fsUrParams = form.down('#cont');
            //fsUrParams.hide();
            //fsUrParams.setDisabled(true);
        },

        index: function (id) {
            var me = this,
                view = me.getMainView() || Ext.widget('realityobjantennagrid');

            me.bindContext(view);
            me.setContextValue(view, 'realityObjectId', id);
            me.application.deployView(view, 'reality_object_info');

            me.getStore('realityobj.RealityObjectAntenna').load();
            //me.getAspect('realityObjAntennaPerm').setPermissionsByRecord({ getId: function () { return id; } });
        },

        onBeforeLoad: function (store, operation) {
            var me = this;
            operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
        }
    });