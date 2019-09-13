using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections.Specialized;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Collections;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>Provides an editing user interface (UI) for a data control field.</summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	public class DataGroupFieldTypeEditor : UITypeEditor
	{

		/// <exclude />
		[
		System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust"),
		]
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context == null)
			{
				return value;
			}
			Component target = context.Instance as Component;
			if ( target != null ) {
				if ( provider != null )
				{
					IWindowsFormsEditorService service1 = (IWindowsFormsEditorService)provider.GetService( typeof( IWindowsFormsEditorService ) );
					if ( service1 == null )
					{
						return value;
					}
					if ( this.dataFieldUI == null )
					{
						this.dataFieldUI = new DataFieldUI( this, context.Instance as Component );
					}
					this.dataFieldUI.Start( service1, value );
					service1.DropDownControl( this.dataFieldUI );
					value = this.dataFieldUI.Value;
					this.dataFieldUI.End();
				}
			}
			return value;

		}

		/// <exclude />
		public override bool IsDropDownResizable
		{
			[
			System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust"),
			]
			get
			{
				return true;
			}
		}



		/// <summary>Gets the editing style associated with this editor, using the specified <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> object.</summary>
		/// <returns>The <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value representing the editing style of this editor. The default value is Modal.</returns>
		/// <param name="context">The <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that indicates the context of the object being edited.</param>
		[
		System.Security.Permissions.PermissionSet( System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust" ),
		]
		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private DataFieldUI dataFieldUI;

		private class DataFieldUI : System.Windows.Forms.ListBox
		{
			public DataFieldUI( UITypeEditor editor, Component component )
			{
				this.editor = editor;


				foreach ( String dataField in GetStandardValues( component ) )
				{
					base.Items.Add( dataField );
				}

			}

			[ System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ComponentModel.Design.IComponentDesignerDebugService.Fail(System.String)" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
			public StringCollection GetStandardValues( Component component )
			{
				StringCollection fields = new StringCollection();

				if ( component != null )
				{
					ISite site1 = component.Site;
					if ( site1 != null )
					{
						IDesignerHost host1 = (IDesignerHost)site1.GetService( typeof( IDesignerHost ) );
						if ( host1 != null )
						{
							IDesigner designer1 = host1.GetDesigner( component );
							DesignerDataSourceView view1 = ( (DataBoundControlDesigner)designer1 ).DesignerView;
							if ( view1 != null )
							{
								IDataSourceViewSchema schema1 = null;
								try
								{
									schema1 = view1.Schema;
								}
								catch ( Exception exception1 )
								{
									IComponentDesignerDebugService service1 = (IComponentDesignerDebugService)site1.GetService( typeof( IComponentDesignerDebugService ) );
									if ( service1 != null )
									{
										service1.Fail( "DataSource DebugService FailedCall\r\n" + exception1.ToString() );
									}
								}
								if ( schema1 != null )
								{
									IDataSourceFieldSchema[] schemaArray1 = schema1.GetFields();
									if ( schemaArray1 != null )
									{
										for ( int num1 = 0; num1 < schemaArray1.Length; num1++ )
										{
											fields.Add( schemaArray1[ num1 ].Name );
										}
									}
								}
							}
							if ( ( ( fields.Count == 0 ) && ( designer1 != null ) ) && ( designer1 is IDataSourceProvider ) )
							{
								IDataSourceProvider provider1 = designer1 as IDataSourceProvider;
								IEnumerable enumerable1 = null;
								if ( provider1 != null )
								{
									enumerable1 = provider1.GetResolvedSelectedDataSource();
								}
								if ( enumerable1 != null )
								{
									PropertyDescriptorCollection collection1 = DesignTimeData.GetDataFields( enumerable1 );
									if ( collection1 != null )
									{
										foreach ( PropertyDescriptor descriptor1 in collection1 )
										{
											fields.Add( descriptor1.Name );
										}
									}
								}
							}
						}
					}
				}

				return fields;
			}

			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			protected override void OnClick( EventArgs e )
			{
				base.OnClick( e );
				this.value = base.SelectedItem;
				this.edSvc.CloseDropDown();
			}

			protected override bool ProcessDialogKey( Keys keyData )
			{
				if ( ( ( keyData & Keys.KeyCode ) == Keys.Return ) && ( ( keyData & ( Keys.Alt | Keys.Control ) ) == Keys.None ) )
				{
					this.OnClick( EventArgs.Empty );
					return true;
				}
				return base.ProcessDialogKey( keyData );
			}


			[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "value" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "edSvc" )]
			public void Start( IWindowsFormsEditorService edSvc, object value )
			{
				this.edSvc = edSvc;
				this.value = value;
				if ( value != null )
				{
					for ( int num1 = 0; num1 < base.Items.Count; num1++ )
					{
						if ( base.Items[ num1 ] == value )
						{
							this.SelectedIndex = num1;
							return;
						}
					}
				}
			}

			public object Value
			{
				get
				{
					return this.value;
				}
			}

			[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields" )]
			private UITypeEditor editor;
			private IWindowsFormsEditorService edSvc;
			private object value;

		}

	}
}
