using System.ComponentModel;
using System;

namespace MetaBuilders.WebControls.Design
{

	[AttributeUsage( AttributeTargets.Property )]
	internal sealed class MbwcDefaultValueAttribute : DefaultValueAttribute
	{

		#region Ctor

		internal MbwcDefaultValueAttribute( string value )
			: base( value )
		{
		}

		internal MbwcDefaultValueAttribute( Type type, string value )
			: base( type, value )
		{
			this._type = type;
		}

		#endregion

		#region Properties

		public override object TypeId
		{
			get
			{
				return typeof( MbwcDefaultValueAttribute );
			}
		}

		public override object Value
		{
			get
			{
				if ( !this._isLocalized )
				{
					String localizationKey = (String)base.Value;
					if ( !string.IsNullOrEmpty( localizationKey ) )
					{
						Object localizedValue = Resources.ResourceManager.GetString( localizationKey );
						if ( this._type != null )
						{
							try
							{
								localizedValue = TypeDescriptor.GetConverter( this._type ).ConvertFromInvariantString( (String)localizedValue );
							}
							catch ( NotSupportedException )
							{
								localizedValue = null;
							}
						}
						
						base.SetValue( localizedValue );
						this._isLocalized = true;
					}
				}
				return base.Value;
			}
		}

		#endregion

		#region Fields

		private bool _isLocalized;
		private Type _type;

		#endregion

	}

}