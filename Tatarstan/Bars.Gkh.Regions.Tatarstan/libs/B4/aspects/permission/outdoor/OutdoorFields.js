Ext.define('B4.aspects.permission.outdoor.OutdoorFields', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.outdoorfieldsperm',
    applyByPostfix: true,

    init: function() {
        var me = this;

        me.permissions = [
            //Element
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Condition_View',
                applyTo: 'gridcolumn[dataIndex=Condition]',
                selector: 'outdoorelementgrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Name_View',
                applyTo: 'gridcolumn[dataIndex=Name]',
                selector: 'outdoorelementgrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Measure_View',
                applyTo: 'gridcolumn[dataIndex=UnitMeasure]',
                selector: 'outdoorelementgrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Volume_View',
                applyTo: 'gridcolumn[dataIndex=Volume]',
                selector: 'outdoorelementgrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Condition_View',
                applyTo: 'b4combobox[name=Condition]',
                selector: 'outdoorelementeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Name_View',
                applyTo: 'b4selectfield[name=Element]',
                selector: 'outdoorelementeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Name_View',
                applyTo: 'textfield[name=Measure]',
                selector: 'outdoorelementeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.View.Volume_View',
                applyTo: 'gkhdecimalfield[name=Volume]',
                selector: 'outdoorelementeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.Edit.Condition_Edit',
                applyTo: 'b4combobox[name=Condition]',
                selector: 'outdoorelementeditwindow'
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Element.Field.Edit.Volume_Edit',
                applyTo: 'gkhdecimalfield[name=Volume]',
                selector: 'outdoorelementeditwindow'
            }, 
            //Image
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.DateImage_View',
                applyTo: 'b4editcolumn[dataIndex=DateImage]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.DateImage_View',
                applyTo: '[name=DateImage]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.Name_View',
                applyTo: 'gridcolumn[dataIndex=Name]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.Name_View',
                applyTo: '[name=Name]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.ImagesGroup_View',
                applyTo: 'b4enumcolumn[dataIndex=ImagesGroup]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.ImagesGroup_View',
                applyTo: '[name=ImagesGroup]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.Period_View',
                applyTo: 'gridcolumn[dataIndex=Period]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.Period_View',
                applyTo: '[name=Period]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.WorkCr_View',
                applyTo: 'gridcolumn[dataIndex=WorkCr]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.WorkCr_View',
                applyTo: '[name=WorkCr]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.File_View',
                applyTo: 'gridcolumn[dataIndex=File]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.File_View',
                applyTo: '[name=File]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.Description_View',
                applyTo: 'gridcolumn[dataIndex=Description]',
                selector: 'outdoorimagegrid',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.View.Description_View',
                applyTo: '[name=Description]',
                selector: 'outdoorimageeditwindow',
                applyBy: me.setVisible
            },
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.DateImage_Edit',
                applyTo: '[name=DateImage]',
                selector: 'outdoorimageeditwindow'
            }, 
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.Name_Edit',
                applyTo: '[name=Name]',
                selector: 'outdoorimageeditwindow'
            }, 
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.ImagesGroup_Edit',
                applyTo: '[name=ImagesGroup]',
                selector: 'outdoorimageeditwindow'
            }, 
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.Period_Edit',
                applyTo: '[name=Period]',
                selector: 'outdoorimageeditwindow'
            }, 
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.WorkCr_Edit',
                applyTo: '[name=WorkCr]',
                selector: 'outdoorimageeditwindow'
            }, 
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.File_Edit',
                applyTo: '[name=File]',
                selector: 'outdoorimageeditwindow'
            }, 
            {
                name: 'Gkh.RealityObjectOutdoor.Register.Image.Field.Edit.Description_Edit',
                applyTo: '[name=Description]',
                selector: 'outdoorimageeditwindow'
            }
        ];

        me.callParent(arguments);
    }
});