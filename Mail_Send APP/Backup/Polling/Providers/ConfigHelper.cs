using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.UI.Design;
using System.Collections.Specialized;

namespace MetaBuilders.WebControls
{
	internal static class ConfigHelper
	{

		public static String GetConnectionString( String connectionStringName, String providerName )
		{
			if ( String.IsNullOrEmpty( connectionStringName ) )
			{
				throw new ProviderException( String.Format( System.Globalization.CultureInfo.InvariantCulture, Resources.Config_ConnectionStringNamePropertyRequired, providerName ) );
			}

			string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			if ( String.IsNullOrEmpty( connectionString ) )
			{
				throw new ProviderException( String.Format( System.Globalization.CultureInfo.InvariantCulture, Resources.Config_ConnectionStringNameNotFound, connectionStringName ) );
			}
			return connectionString;
		}

		public static Boolean GetBooleanValue( System.Collections.Specialized.NameValueCollection config, String valueName, Boolean defaultValue )
		{
			Boolean result;
			String value = config[valueName];
			if (value == null)
			{
				return defaultValue;
			}
			if (!Boolean.TryParse(value, out result))
			{
				throw new ProviderException( String.Format( System.Globalization.CultureInfo.InvariantCulture, Resources.Config_AttributeNotBoolean, valueName ) );
			}
			return result;
		}

		public static void WriteConfig( Configuration config )
		{

			if ( config.GetSection( "metabuilders/polling" ) != null ) {
			    return;
			}

			ConfigurationSectionGroup group = new ConfigurationSectionGroup();
			config.SectionGroups.Add( "metabuilders", group );

			PollingConfig pollingConfig = new PollingConfig();
			group.Sections.Add( "polling", pollingConfig );
			
			pollingConfig.DefaultProvider = "AccessPollingProvider";
			pollingConfig.SectionInformation.RequirePermission = false;
			pollingConfig.SectionInformation.RestartOnExternalChanges = true;
			pollingConfig.SectionInformation.AllowDefinition = ConfigurationAllowDefinition.MachineToApplication;

			ProviderSettings settings = new ProviderSettings();
			settings.Name = "AccessPollingProvider";
			settings.Type = "MetaBuilders.WebControls.AccessPollingProvider, MetaBuilders.WebControls";
			settings.Parameters.Add( "connectionStringName", "MetaBuildersPolling" ); 

			pollingConfig.Providers.Add( settings );

			if ( config.ConnectionStrings.ConnectionStrings["MetaBuildersPolling"] == null )
			{
				config.ConnectionStrings.ConnectionStrings.Add( new ConnectionStringSettings( "MetaBuildersPolling", "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=|DataDirectory|MetaBuildersPolling.mdb", "System.Data.OleDb" ) );
			}

			config.Save();

		}

	}
}
