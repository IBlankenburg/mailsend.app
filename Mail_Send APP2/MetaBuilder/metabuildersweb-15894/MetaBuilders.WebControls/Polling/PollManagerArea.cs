using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Represents an html fieldset and legend.
	/// </summary>
	internal class PollManagerArea : WebControl
	{

		public PollManagerArea()
		{

		}

		protected override System.Web.UI.HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Fieldset;
			}
		}

		public String Title
		{
			get
			{
				Object state = ViewState["Title"];
				if ( state != null )
				{
					return (String)state;
				}
				return "";
			}
			set
			{
				ViewState["Title"] = value;
			}
		}

		[
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		NotifyParentProperty( true ),
		]
		public Style TitleStyle
		{
			get
			{
				if ( _titleStyle == null )
				{
					_titleStyle = new Style();
					if ( IsTrackingViewState )
					{
						( (IStateManager)_titleStyle ).TrackViewState();
					}
				}
				return _titleStyle;
			}
		}
		private Style _titleStyle;

		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{
			base.AddAttributesToRender( writer );
			writer.AddStyleAttribute( "padding", "8px" );
			writer.AddStyleAttribute( "-moz-border-radius", "10px" );
		}


		public override void RenderBeginTag( HtmlTextWriter writer )
		{
			base.RenderBeginTag( writer );
			RenderLegendTag( writer );
		}

		private void RenderLegendTag( HtmlTextWriter writer )
		{
			if ( this.Title.Length > 0 )
			{
				Legend legend = new Legend();
				legend.Text = Title;
				legend.Font.Size = FontUnit.Parse( "1.5em", System.Globalization.CultureInfo.InvariantCulture );
				if ( this._titleStyle != null )
				{
					legend.ControlStyle.CopyFrom( this._titleStyle );
				}
				legend.RenderControl( writer );
			}
		}

		private class Legend : Label
		{
			protected override HtmlTextWriterTag TagKey
			{
				get
				{
					return HtmlTextWriterTag.Legend;
				}
			}
		}

	}
}
