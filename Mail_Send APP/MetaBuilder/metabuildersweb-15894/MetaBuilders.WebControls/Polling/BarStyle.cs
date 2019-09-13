using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The <see cref="Style"/> used to describe the style of the horizontal bars shown
	/// in the <see cref="PollView"/> control's result display mode.
	/// </summary>
	/// <remarks>
	/// <see cref="BarStyle"/> adds <see cref="BackImageUrl"/> and <see cref="ForeImageUrl"/> properties
	/// to the standard <see cref="Style"/> class.
	/// </remarks>
	[
	System.ComponentModel.DesignerCategory( "Code" ),
	]
	public class BarStyle : Style
	{

		/// <summary>
		/// Creates a new instance of the BarStyle class.
		/// </summary>
		public BarStyle()
		{
		}

		/// <summary>
		/// Creates a new instance of the BarStyle class with the given <see cref="StateBag"/>.
		/// </summary>
		/// <param name="bag">The object used to save the state of the style.</param>
		public BarStyle( StateBag bag )
			: base( bag )
		{
		}

		/// <summary>
		/// Gets or sets the url of the image used as the background of the bar.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Description( "Gets or sets the url of the image used as the background of the bar." ),
		NotifyParentProperty( true ),
		Editor( typeof( ImageUrlEditor ), typeof( UITypeEditor ) ),
		UrlProperty(),
		]
		public virtual String BackImageUrl
		{
			get
			{
				if ( IsSet( Prop_BackImageUrl ) )
				{
					return (String)ViewState[Prop_BackImageUrl];
				}
				return String.Empty;
			}
			set
			{
				ViewState[Prop_BackImageUrl] = value;
			}
		}

		/// <summary>
		/// Gets or sets the url of the image used as the foreground, or filled-in area, of the bar.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Description( "Gets or sets the url of the image used as the foreground, or filled-in area, of the bar." ),
		NotifyParentProperty( true ),
		Editor( typeof( ImageUrlEditor ), typeof( UITypeEditor ) ),
		UrlProperty(),
		]
		public virtual String ForeImageUrl
		{
			get
			{
				if ( IsSet( Prop_ForeImageUrl ) )
				{
					return (String)ViewState[Prop_ForeImageUrl];
				}
				return String.Empty;
			}
			set
			{
				ViewState[Prop_ForeImageUrl] = value;
			}
		}



		/// <summary>
		/// Gets a value indicating if any of the properties of the style have been set.
		/// </summary>
		protected new internal Boolean IsEmpty
		{
			get
			{
				return base.IsEmpty && !IsSet( Prop_BackImageUrl ) && !IsSet( Prop_ForeImageUrl );
			}
		}

		/// <summary>
		/// Determines if the given property has been set.
		/// </summary>
		internal Boolean IsSet( String propertyName )
		{
			return ViewState[propertyName] != null;
		}

		/// <summary>
		/// Copies the properties of the given <see cref="Style"/> into this <see cref="Style"/>.
		/// </summary>
		/// <param name="s">The <see cref="Style"/> which contains the properties to be copied.</param>
		public override void CopyFrom( Style s )
		{
			if ( s != null )
			{
				base.CopyFrom( s );

				BarStyle barStyle = s as BarStyle;
				if ( barStyle != null )
				{
					if ( !barStyle.IsEmpty )
					{
						if ( barStyle.IsSet( Prop_BackImageUrl ) )
						{
							this.BackImageUrl = barStyle.BackImageUrl;
						}
						if ( barStyle.IsSet( Prop_ForeImageUrl ) )
						{
							this.ForeImageUrl = barStyle.ForeImageUrl;
						}
					}
				}
			}
		}

		/// <summary>
		/// Merges the given <see cref="Style"/>'s properties with this <see cref="Style"/>'s properties.
		/// </summary>
		/// <param name="s">The <see cref="Style"/> which contains the properties to be merged.</param>
		public override void MergeWith( Style s )
		{
			if ( s != null )
			{
				if ( this.IsEmpty )
				{
					this.CopyFrom( s );
					return;
				}

				base.MergeWith( s );

				BarStyle barStyle = s as BarStyle;
				if ( barStyle != null )
				{
					if ( !barStyle.IsEmpty )
					{
						if ( !this.IsSet( Prop_BackImageUrl ) && barStyle.IsSet( Prop_BackImageUrl ) )
						{
							this.BackImageUrl = barStyle.BackImageUrl;
						}
						if ( !this.IsSet( Prop_ForeImageUrl ) && barStyle.IsSet( Prop_ForeImageUrl ) )
						{
							this.ForeImageUrl = barStyle.ForeImageUrl;
						}
					}
				}
			}
		}

		/// <summary>
		/// Clears all the style properties to the original values.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			if ( IsEmpty )
			{
				return;
			}
			if ( IsSet( Prop_BackImageUrl ) )
			{
				ViewState.Remove( Prop_BackImageUrl );
			}
			if ( IsSet( Prop_ForeImageUrl ) )
			{
				ViewState.Remove( Prop_ForeImageUrl );
			}
		}


		private const String Prop_BackImageUrl = "BackImageUrl";
		private const String Prop_ForeImageUrl = "ForeImageUrl";


	}
}
