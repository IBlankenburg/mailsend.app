using System;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Represents the Polling configuration defined in web.config.
	/// </summary>
	internal class PollingConfig : ConfigurationSection
	{

		/// <summary>
		/// Creates a new instance of the PollingConfig class.
		/// </summary>
		public PollingConfig()
		{
		}

		#region Properties

		/// <summary>
		/// Gets or sets the default polling provider
		/// </summary>
		[
		StringValidator( MinLength = 1 ),
		ConfigurationProperty( "defaultProvider", DefaultValue = "AccessPollingProvider" ),
		]
		public string DefaultProvider
		{
			get
			{
				return (string)base["defaultProvider"];
			}
			set
			{
				base["defaultProvider"] = value;
			}
		}

		/// <summary>
		/// Gets a collection of the polling providers registered in the providers element.
		/// </summary>
		[ConfigurationProperty( "providers" )]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base["providers"];
			}
		}

		#endregion

	}
}
