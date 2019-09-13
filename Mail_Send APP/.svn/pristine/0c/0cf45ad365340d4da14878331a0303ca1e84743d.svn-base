using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

namespace MetaBuilders.WebControls.Design
{

    /// <summary>Provides a type converter that can retrieve a list of data fields from the current component's selected data source.</summary>
    public class DataGroupFieldConverter : TypeConverter
    {
        /// <summary>Gets a value indicating whether the converter can convert an object of the specified source type to the native type of the converter.</summary>
        /// <returns>true if the converter can perform the conversion; otherwise, false.</returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> object that can be used to gain additional context information. </param>
        /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you wish to convert from. </param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return false;
        }

        /// <summary>Converts the specified object to the native type of the converter.</summary>
        /// <returns>An <see cref="T:System.Object"></see> that represents the specified object after conversion.</returns>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see> that can be used to support localization features. </param>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> object that can be used to gain additional context information. </param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert. </param>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            if (value.GetType() != typeof(string))
            {
                throw base.GetConvertFromException(value);
            }
            return (string) value;
        }

        /// <summary>Gets the data fields present within the selected data source if information about them is available.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> listing the standard accessible data sources.</returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> object indicating the component or control to get values for. </param>
		[ System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ComponentModel.Design.IComponentDesignerDebugService.Fail(System.String)" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            object[] objArray1 = null;
            if (context != null)
            {
                IComponent component1 = context.Instance as IComponent;
                if (component1 != null)
                {
                    ISite site1 = component1.Site;
                    if (site1 != null)
                    {
                        IDesignerHost host1 = (IDesignerHost) site1.GetService(typeof(IDesignerHost));
                        if (host1 != null)
                        {
                            IDesigner designer1 = host1.GetDesigner(component1);
                            DesignerDataSourceView view1 = this.GetView(designer1);
                            if (view1 != null)
                            {
                                IDataSourceViewSchema schema1 = null;
                                try
                                {
                                    schema1 = view1.Schema;
                                }
                                catch (Exception exception1)
                                {
                                    IComponentDesignerDebugService service1 = (IComponentDesignerDebugService) site1.GetService(typeof(IComponentDesignerDebugService));
                                    if (service1 != null)
                                    {
                                        service1.Fail( "DataSource DebugService FailedCall\r\n" + exception1.ToString() );
                                    }
                                }
                                if (schema1 != null)
                                {
                                    IDataSourceFieldSchema[] schemaArray1 = schema1.GetFields();
                                    if (schemaArray1 != null)
                                    {
                                        objArray1 = new object[schemaArray1.Length];
                                        for (int num1 = 0; num1 < schemaArray1.Length; num1++)
                                        {
                                            objArray1[num1] = schemaArray1[num1].Name;
                                        }
                                    }
                                }
                            }
                            if (((objArray1 == null) && (designer1 != null)) && (designer1 is IDataSourceProvider))
                            {
                                IDataSourceProvider provider1 = designer1 as IDataSourceProvider;
                                IEnumerable enumerable1 = null;
                                if (provider1 != null)
                                {
                                    enumerable1 = provider1.GetResolvedSelectedDataSource();
                                }
                                if (enumerable1 != null)
                                {
                                    PropertyDescriptorCollection collection1 = DesignTimeData.GetDataFields(enumerable1);
                                    if (collection1 != null)
                                    {
                                        ArrayList list1 = new ArrayList();
                                        foreach (PropertyDescriptor descriptor1 in collection1)
                                        {
                                            list1.Add(descriptor1.Name);
                                        }
                                        objArray1 = list1.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new TypeConverter.StandardValuesCollection(objArray1);
        }

        /// <summary>Gets a value indicating whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is a list of all possible values.</summary>
        /// <returns>true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exclusive list of all possible values that are valid; false if other values are possible.As implemented in this class, this method always returns false.</returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> object that can be used to gain additional context information. </param>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>Gets a value indicating whether the converter supports a standard set of values that can be picked from a list.</summary>
        /// <returns>true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> can be called to find a common set of values the object supports; otherwise, false.</returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information. </param>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance is IComponent))
            {
                return true;
            }
            return false;
        }

		[ System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		private DesignerDataSourceView GetView(IDesigner dataBoundControlDesigner)
        {
            DataBoundControlDesigner designer1 = dataBoundControlDesigner as DataBoundControlDesigner;
            if (designer1 != null)
            {
                return designer1.DesignerView;
            }
            return null;
        }

    }
}
