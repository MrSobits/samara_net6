Ext.define('B4.controller.FiasController', {
    extend: 'B4.base.Controller',
    
    requires: ['B4.aspects.FiasTreeEditWindow', 'B4.aspects.GridEditWindow'],

    models: ['Fias'],
    stores: ['FiasStreet'],
    views: ['Fias.Panel', 'Fias.EditWindow', 'Fias.StreetGrid'],

    mainView: 'Fias.Panel',
    mainViewSelector: '#fiasPanel',

    aspects: [
        {
            /*
            Подключаем аспект взаимодействия дерева, навигационной панели и формы редактирования ФИАС
            */
            xtype: 'fiastreeeditwindowaspect',
            name: 'fiasTreeEditWindowAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'fiasStreetGridWindowAspect',
            gridSelector: '#fiasStreetGrid',
            editFormSelector: '#fiasEditWindow',
            storeName: 'FiasStreet',
            modelName: 'Fias',
            editWindowView: 'Fias.EditWindow',

            setFormData: function (rec) {
                var form = this.getForm();
                //проверяем выбран ли родительский элемент и если выбран то проставляем значения из родительского
                var selectRecord = this.controller.getAspect('fiasTreeEditWindowAspect').selectedTreeRecord;
                if (!selectRecord) {
                    Ext.Msg.alert('Внимание!', 'Необходимо выбрать родительский адресный объект');
                    return;
                }

                var model = this.controller.getModel(this.modelName);
                var me = this;

                model.load(selectRecord.get('fiasId'), {
                    success: function (parentRec) {

                        if (parentRec) {
                            me.setParentFields(rec, parentRec);

                            form.loadRecord(rec);

                            //Блокируем выбор Уровня , потомучт опока будет только уровень улиц
                            form.down('#levelCombobox').setDisabled(true);

                            if (rec.get('TypeRecord') == 10) {
                                form.down('#btnSave').setDisabled(false);
                            } else {
                                form.down('#btnSave').setDisabled(false);
                            }

                            form.show();
                        }
                    },
                    scope: this
                });
            },
            setParentFields: function (rec, parentRec) {
                if (rec.getId() == 0) {
                    // Так как фильтруется по MirrorGuid, то временно делаем так, пока в B4 не исправят
                    var mirrorGuid = parentRec.get('MirrorGuid');
                    if (mirrorGuid) {
                        rec.set('ParentGuid', mirrorGuid);
                    } else {
                        rec.set('ParentGuid', parentRec.get('AOGuid'));
                        rec.set('MirrorGuid', parentRec.get('MirrorGuid'));
                    }
                    
                    rec.set('CodeRegion', parentRec.get('CodeRegion'));
                    rec.set('CodeAuto', parentRec.get('CodeAuto'));
                    rec.set('CodeArea', parentRec.get('CodeArea'));
                    rec.set('CodeCity', parentRec.get('CodeCity'));
                    rec.set('CodeCtar', parentRec.get('CodeCtar'));
                    rec.set('CodePlace', parentRec.get('CodePlace'));
                    rec.set('CodeStreet', parentRec.get('CodeStreet'));
                    rec.set('CodeExtr', parentRec.get('CodeExtr'));
                    rec.set('CodeSext', parentRec.get('CodeSext'));
                    rec.set('PostalCode', parentRec.get('PostalCode'));
                    rec.set('AOLevel', 7);
                }
            }
        }
    ],

    init: function () {
        this.getStore('FiasStreet').on('beforeload', this.onStreetBeforeLoad, this);
        this.callParent(arguments);
    },

    onStreetBeforeLoad: function (store, operation) {
        var selectRecord = this.getAspect('fiasTreeEditWindowAspect').selectedTreeRecord;
        if (selectRecord) {
            if (selectRecord.get('mirrorGuid')) {
                console.log(selectRecord.get('mirrorGuid'));
                operation.params.parentGuid = selectRecord.get('mirrorGuid');
            } else {
                operation.params.parentGuid = selectRecord.get('fiasGuidId');
            }

            operation.params.level = 7;
        }
    }
});