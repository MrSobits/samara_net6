Ext.define('B4.view.wizard.preparedata.StartStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',

    requires: [
        'B4.form.SelectField'
    ],

    stepId: 'start',
    title: 'Начало работы с мастером',
    layout: 'border',

    items: [
        {
            region: 'west',
            width: 150,
            baseCls: 'icon_wizard'
        },
        {
            region: 'center',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                layout: 'fit'
            },
            items: [
                {
                    region: 'north',
                    title: 'Выберите метод',
                    height: 48,
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Method',
                            store: 'B4.store.integrations.gis.Method',
                            textProperty: 'Name',
                            flex: 1,
                            editable: false,
                            columns: [
                                {
                                    text: 'Наименование метода',
                                    flex: 1,
                                    dataIndex: 'Name',
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    text: 'Описание метода',
                                    flex: 2,
                                    dataIndex: 'Description',
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'center',
                    name: 'Description',
                    title: 'Описание',
                    flex: 2,
                    items: [
                        {
                            xtype: 'textareafield',
                            grow: true,
                            readOnly: true,
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'south',
                    name: 'RequiredMethods',
                    title: 'Методы, которые необходимо выполнить предварительно',
                    flex: 5,
                    items: [
                        {
                            xtype: 'textareafield',
                            grow: true,
                            readOnly: true,
                            flex: 1
                        }
                    ]
                }
            ]
        }
    ],

    allowBackward: function () {
        return false;
    },

    allowForward: function () {
        return this.wizard.exporter_Id;
    },

    doForward: function () {
        var me = this;
        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('CheckMethodFeasibility', 'GisIntegration'),
            params: {
                exporter_Id: me.wizard.exporter_Id
            },
            timeout: 9999999
        }).next(function (response) {
            me.wizard.unmask();

            var json = Ext.JSON.decode(response.responseText),
                currentWizardClassName = Ext.getClassName(me.wizard),
                extenderClassName = json.data.data.PrepareDataWizardClassName || currentWizardClassName,
                dataSupplierIsRequired = json.data.data.DataSupplierIsRequired || false;

            me.wizard.dataSupplierIsRequired = dataSupplierIsRequired;

            if (extenderClassName === currentWizardClassName) {
                me.wizard.setCurrentStep(dataSupplierIsRequired ? 'dataSupplier' : 'pageParameters');
            } else {
                //будет открыт мастер, расширяющий базовый
                me.wizard.openExtender = true;
                me.wizard.extenderClassName = extenderClassName;
                me.wizard.close();
            }          
        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось запустить формирование данных'
            }
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);
    },

    firstInit: function () {
        var me = this,
            methodSelectField = me.down('b4selectfield');

        methodSelectField.on('change', me.onSelectFieldChange, me);
    },

    onSelectFieldChange: function (selFld, val) {
        var me = this,
            descriptionArea = me.down('panel[name=Description]').down('textareafield'),
            requiredMethodsArea = me.down('panel[name=RequiredMethods]').down('textareafield'),
            requiredMethods,
            requiredMethodsText = '';

            descriptionArea.reset();
            requiredMethodsArea.reset();
            me.wizard.exporter_Id = undefined;

        if (val) {
            me.wizard.exporter_Id = val.Id;
            descriptionArea.setValue(val.Description);

            if (val.Dependencies) {
                requiredMethods = val.Dependencies.split(';');
                for (var i = 0; i < requiredMethods.length; i++) {
                    requiredMethodsText += Ext.String.format('{0}. {1}\n', i + 1, requiredMethods[i]);
                }
                requiredMethodsArea.setValue(requiredMethodsText);
            }
        }

        me.fireEvent('selectionchange', me);
    }
});